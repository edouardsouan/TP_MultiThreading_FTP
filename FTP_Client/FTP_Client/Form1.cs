using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using FTP_Client.ServerEntities;

namespace FTP_Client
{
    public partial class Form1 : Form
    {
        #region Variables
        string localPath = "";
        string serverPath = "";
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        #endregion

        public Form1()
        {
            InitializeComponent();

            cancellationTokenSource = new CancellationTokenSource();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Local_ShowLogicalDrives();
        }

        #region Transfert Gauge
        private void TransfertGauge(double totalWeigth, double actualWeigth)
        {
            fileTransfertBar.Minimum = 0;
            fileTransfertBar.Maximum = (int)totalWeigth;

            fileTransfertBar.Value = (int)actualWeigth;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            fileTransfertBar.Value = 0;
            Console.WriteLine("The transfert has been stop.");
        }
        #endregion

        #region Time Left
        private double CalculateTimeLeft(DateTime beginDate, double nbProcessed, double nbTotal)
        {
            return (DateTime.Now.Subtract(beginDate).TotalSeconds / nbProcessed) * (nbTotal - nbProcessed);
        }
        #endregion

        #region Drag and Drop check
        private bool IsServerListTheSender(ListView listView)
        {
            bool isSender = false;

            if (listView == serverListView)
            {
                isSender = true;
            }

            return isSender;
        }
        #endregion

        #region local travel
        private void localTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Local_OpenNode(nodeClicked);
        }

        private void localListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (localListView.SelectedItems.Count > 0)
            {
                TreeNode nodeClicked;

                ListViewItem selectedItem = localListView.SelectedItems[0];
                if (selectedItem.Text.Equals(".."))
                {
                    nodeClicked = ((TreeNode)selectedItem.Tag).Parent;
                }
                else
                {
                    nodeClicked = (TreeNode)selectedItem.Tag;
                }

                Local_OpenNode(nodeClicked);
                nodeClicked.Expand();
            }
        }
        #endregion

        #region Server travel
        private void serverTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Server_OpenNode(nodeClicked);
        }

        private void serverListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (serverListView.SelectedItems.Count > 0)
            {
                TreeNode nodeClicked;

                ListViewItem selectedItem = serverListView.SelectedItems[0];
                if (selectedItem.Text.Equals(".."))
                {
                    nodeClicked = ((TreeNode)selectedItem.Tag).Parent;

                }
                else
                {
                    nodeClicked = (TreeNode)selectedItem.Tag;
                }

                Server_OpenNode(nodeClicked);
            }
        }

        
        #endregion

        #region Download files/directory from the server
        private void localListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            localListView.DoDragDrop(localListView.SelectedItems, DragDropEffects.Move);
        }

        private void localListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void localListView_DragDrop(object sender, DragEventArgs e)
        {
            Point pointWhereDropped = new Point(e.X, e.Y);
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            fileQueue.Items.Clear();

            ListView.SelectedListViewItemCollection draggedFiles =
            (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
            foreach (ListViewItem fileToDownload in draggedFiles)
            {
                if (IsServerListTheSender(fileToDownload.ListView) && !fileToDownload.Text.Equals(".."))
                {
                    string fileToDownloadPath = serverPath + "/" + fileToDownload.Name;
                    string targetPath = localPath;

                    try
                    {
                        targetPath += '\\';
                        targetPath += serverListView.GetDirecoryNamePointed(pointWhereDropped);
                    }
                    catch (NullReferenceException exception)
                    {
                        Console.WriteLine("Exception " + exception.ToString() + " was thrown because no Patrice was found !!");
                        Console.WriteLine("Exception " + exception.ToString() + " no file directory pointed.");
                    }
                    finally
                    {
                        FileServer fileFromServer = (FileServer)((TreeNode)fileToDownload.Tag).Tag;

                        targetPath += "\\" + fileToDownload.Name;
                        DownloadTransfert(fileToDownloadPath, targetPath, fileFromServer);
                    }
                }
            }

        }

        private async void DownloadTransfert(string filePathToDownload, string localPathTarget, FileServer fileInfo)
        {
            string serverTarget = filePathToDownload;
            string fileName = fileInfo.GetName();

            if (fileInfo.IsADirectory())
            {
                try
                {
                    Directory.CreateDirectory(localPathTarget);
                    Local_RefreshView();
                }
                catch (System.IO.IOException exception)
                {
                    Console.WriteLine("File name already exist : " + exception.ToString());
                }

                FtpWebRequest ftpRequest = ftpManager.CreatRequestListDirectoriesAndFiles(serverTarget);
                FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();
                logWindow.WriteLog(ftpResponse);
                string[] serverData = ftpManager.ParseRawData(ftpResponse);

                foreach (String rawData in serverData)
                {
                    FileServer fileServer = new FileServer(rawData);
                    if (fileServer.IsNameOKToDisplay() && !cancellationToken.IsCancellationRequested)
                    {
                        DownloadTransfert(serverTarget + "/" + fileServer.GetName(),
                            localPathTarget + "\\" + fileServer.GetName(),
                            fileServer);
                    }
                }
            }
            else
            {
                DownloadFile(localPathTarget, fileInfo, serverTarget);
            }
        }

        private void DownloadFile(string localPathTarget, FileServer fileInfo, string serverTarget)
        {
            fileQueue.AddItem(fileInfo);
            ListViewItem itemQueue = fileQueue.GetLastItem();

            Task.Factory.StartNew(() =>
            {
                FtpWebRequest downloadRequest = ftpManager.CreatRequestDownloadFile(serverTarget);
                Task.Factory.StartNew(() => logWindow.Invoke(new Action(() =>
                   logWindow.WriteLog(downloadRequest.Method + " : " + serverTarget + "\n", Color.Blue)))
                );
                FileStream downloadedFileStream = new FileStream(localPathTarget, FileMode.Create);

                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                Task.Factory.StartNew(() => logWindow.Invoke(new Action(() =>
                    logWindow.WriteLog(downloadResponse)))
                );
                
                Stream responseStream = downloadResponse.GetResponseStream();
                Int32 bufferSize = 2048;
                Int32 readCount;
                Byte[] buffer = new Byte[bufferSize];
                double totalWeight = (double)fileInfo.GetSize();
                double actualWeigth = 0;
                readCount = responseStream.Read(buffer, 0, bufferSize);

                DateTime beginDate = DateTime.Now;
                while (readCount > 0 && !cancellationToken.IsCancellationRequested)
                {
                    actualWeigth += readCount;
                    downloadedFileStream.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, bufferSize);
                    
                    Task.Factory.StartNew(() =>
                    {
                        fileQueue.Invoke(new Action(() =>
                            itemQueue.SubItems[4].Text = CalculateTimeLeft(beginDate, actualWeigth, totalWeight).ToString()
                        ));
                        fileTransfertBar.Invoke(new Action(() =>
                            TransfertGauge(totalWeight, actualWeigth)
                        ));
                    });
                }
                responseStream.Close();
                downloadedFileStream.Close();
                downloadResponse.Close();

            }, cancellationToken);

            Local_RefreshView();
        }

        public void Local_RefreshView()
        {
            ListViewItem parentItem = localListView.Items[0];
            TreeNode parentNode = (TreeNode)parentItem.Tag;
            parentNode.Nodes.Clear();
            Local_ShowLinkedElements(parentNode);
        }
        #endregion

        #region Upload files / directories to the server#
        private void serverListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            serverListView.DoDragDrop(serverListView.SelectedItems, DragDropEffects.Move);
        }

        private void serverListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void serverListView_DragDrop(object sender, DragEventArgs e)
        {
            Point draggedFilePoint = new Point(e.X, e.Y);
            string serverPathTarget = serverPath.Replace("\\", "//");
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            fileQueue.Items.Clear();

            try
            {
                string draggedFileName = localListView.GetDirecoryNamePointed(draggedFilePoint);
                serverPathTarget += draggedFileName;
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine("Exception " + exception.ToString() + "; nothing pointed");
            }
            finally
            {
                ListView.SelectedListViewItemCollection draggedFiles =
                (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
                foreach (ListViewItem draggedFile in draggedFiles)
                {
                    UploadTransfert(draggedFile, serverPathTarget +"//" + draggedFile.Name);
                }
            }
        }

        private void UploadTransfert(ListViewItem draggedFile, string serverPathTarget)
        {
            TreeNode fileNode = (TreeNode)draggedFile.Tag;

            try
            {
                FileInfo fileToUpload = (FileInfo)fileNode.Tag;
                UploadFile(fileToUpload.FullName, serverPathTarget);
            }
            catch (InvalidCastException exception)
            {
                Console.WriteLine("Exception " + exception.ToString() + " directory to .");

                DirectoryInfo directoryToUpload = (DirectoryInfo)fileNode.Tag;
                UploadDirectory(directoryToUpload, serverPathTarget);
            }
        }

        private void UploadDirectory(DirectoryInfo directoryToUpload, string serverPathTarget)
        {
            Server_CreateDirectory(serverPathTarget);
            Server_RefreshView();
            string subServerPathTarget = serverPathTarget + "/";

            List<DirectoryInfo> subDirectories = Local_GetLocalDirectories(directoryToUpload);
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                UploadDirectory(subDirectory, subServerPathTarget + subDirectory.Name);
            }

            List<FileInfo> subFiles = Local_GetLocalFiles(directoryToUpload);
            foreach (FileInfo subFile in subFiles)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    UploadFile(subFile.FullName, subServerPathTarget + subFile.Name);
                }
            }
        }

        private void UploadFile(string filePathToUpload, string serverPathTarget)
        {
            FileInfo fi = new FileInfo(filePathToUpload);
            fileQueue.AddItem(fi);
            ListViewItem itemQueue = fileQueue.GetLastItem();

            Task.Factory.StartNew(() =>
            {
                FtpWebRequest uploadRequest = ftpManager.CreatRequestUploadFile(serverPathTarget);
                Task.Factory.StartNew(() => logWindow.Invoke(new Action(() => 
                    logWindow.WriteLog(uploadRequest.Method + " : " + serverPathTarget+"\n", Color.Blue)))
                );
                
                Int32 buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                FileStream fs = fi.OpenRead();
                Stream strm = uploadRequest.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                double actualWeigth = 0;

                DateTime beginDate = DateTime.Now;
                while (contentLen != 0 && !cancellationToken.IsCancellationRequested)
                {
                    actualWeigth += contentLen;

                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);

                    Task.Factory.StartNew(() =>
                    {
                        fileQueue.Invoke(new Action(() =>
                            itemQueue.SubItems[4].Text = CalculateTimeLeft(beginDate, actualWeigth, fi.Length).ToString()
                        ));
                        fileTransfertBar.Invoke(new Action(() =>
                            TransfertGauge(fi.Length, actualWeigth)
                        ));
                    });
                }

                strm.Close();
                fs.Close();

            }, cancellationToken);

            Server_RefreshView();
        }

        public void Server_RefreshView()
        {
            ListViewItem parentItem = serverListView.Items[0];
            TreeNode parentNode = (TreeNode)parentItem.Tag;
            parentNode.Nodes.Clear();
            Server_ShowLinkedFTPElements(serverPath,parentNode);
        }
        #endregion

    }
}
