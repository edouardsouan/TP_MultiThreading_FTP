using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

// Créer une interface IsADirectory

using FTPClient.ServerEntities;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        #region Variables
        string localPath = "";
        string serverPath = "";
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            txtPassword.TextBox.PasswordChar = '*';
            cancellationTokenSource = new CancellationTokenSource();
        }
        #endregion

        #region Destructor
        ~Form1()
        {
            ftpManager.LogOut();
        }
        #endregion

        #region IsADirectory or IsAFile => Put in each Tree (local and server) ?
        private bool IsADirectory(FileSystemInfo fileSystemInfo)
        {
            bool isADirectory = false;
            
            try
            {
                FileAttributes attributes = File.GetAttributes(fileSystemInfo.FullName);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    isADirectory = true;
            }
            catch(IOException exception)
            {
                logWindow.WriteLog("Acces denied : " + fileSystemInfo.FullName, Color.Red);
                Console.WriteLine(exception.ToString());
            }

            return isADirectory;
        }

        private bool IsADirectory(TreeNode node)
        {
            bool isADirectory = false;
            if (node.ImageIndex == 0 || node.ImageIndex == 1)
            {
                isADirectory = true;
            }
            return isADirectory;
        }
        #endregion

        #region Recognise Sender
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

        #region File Transfert
        private void TransfertGauge(double totalWeigth, double actualWeigth)
        {
            fileTransfertBar.Minimum = 0;
            fileTransfertBar.Maximum = (int)totalWeigth;
        
            fileTransfertBar.Value = (int)actualWeigth;
        }
        #endregion

        #region Download files/directory from the server
        private void listViewLocal_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            localListView.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void listViewLocal_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void listViewLocal_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem fileToDownload = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            Point pointWhereDropped = new Point(e.X, e.Y);
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;


            if (IsServerListTheSender(fileToDownload.ListView))
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
                    
                    fileQueue.AddItem(fileFromServer);

                    targetPath += "\\" + fileToDownload.Name;
                    DownloadTransfert(fileToDownloadPath, targetPath, fileFromServer);
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
                        DownloadTransfert(serverTarget  + "/" + fileServer.GetName(), 
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
           Task task = Task.Factory.StartNew( () =>{

                FtpWebRequest downloadRequest = ftpManager.CreatRequestDownloadFile(serverTarget);
                FileStream downloadedFileStream = new FileStream(localPathTarget, FileMode.Create);

                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                Stream responseStream = downloadResponse.GetResponseStream();
                Int32 bufferSize = 2048;
                Int32 readCount;
                Byte[] buffer = new Byte[bufferSize];
                double totalWeight = (double)fileInfo.GetSize();
                double actualWeigth = 0;
                readCount = responseStream.Read(buffer, 0, bufferSize);
                while (readCount > 0 && !cancellationToken.IsCancellationRequested)
                {
                    actualWeigth += readCount;
                    downloadedFileStream.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, bufferSize);
                    fileTransfertBar.Invoke(new Action(() => TransfertGauge(totalWeight, actualWeigth)));
                }
                responseStream.Close();
                downloadedFileStream.Close();
                downloadResponse.Close();
                Local_RefreshView();
            }, cancellationToken);
        }

        public void Local_RefreshView() {
            /*
            ListViewItem parentItem = localListView.Items[0];
            TreeNode parentNode = (TreeNode)parentItem.Tag;
            parentNode.Nodes.Clear();
            Local_ShowLinkedElements(parentNode);
             * */
        }
        #endregion

        #region Upload files / directories to the server
        private void listViewServer_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            serverListView.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void listViewServer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void listViewServer_DragDrop(object sender, DragEventArgs e)
        {
            Point draggedFilePoint = new Point(e.X, e.Y);
            string serverPathTarget = serverPath.Replace("\\", "//");
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            try
            {
                string draggedFileName = localListView.GetDirecoryNamePointed(draggedFilePoint);
                serverPathTarget += draggedFileName;
            }
            catch(NullReferenceException exception)
            {
                Console.WriteLine("Exception "+ exception.ToString() +"; nothing pointed");
            }
            finally
            {
                ListViewItem draggedFile = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                serverPathTarget += "//" + draggedFile.Name;
                UploadTransfert(draggedFile, serverPathTarget);
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
            foreach(DirectoryInfo subDirectory in subDirectories)
            {
                UploadDirectory(subDirectory, subServerPathTarget + subDirectory.Name);
            }

            List<FileInfo> subFiles = Local_GetLocalFiles(directoryToUpload);
            foreach (FileInfo subFile in subFiles)
            {
                if(!cancellationToken.IsCancellationRequested)
                {
                    UploadFile(subFile.FullName, subServerPathTarget + subFile.Name);
                }
            }
        }

        private void UploadFile(string filePathToUpload, string serverPathTarget)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                FileInfo fi = new FileInfo(filePathToUpload);
                FtpWebRequest uploadRequest = ftpManager.CreatRequestUploadFile(serverPathTarget);


                Int32 buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fi.OpenRead();

                //try{
                    // Stream to which the file to be upload is written
                    Stream strm = uploadRequest.GetRequestStream();

                    // Read from the file stream 2kb at a time
                    contentLen = fs.Read(buff, 0, buffLength);

                    double actualWeigth = 0;
                    // Till Stream content ends
                    while (contentLen != 0 && !cancellationToken.IsCancellationRequested)
                    {
                        actualWeigth += contentLen;

                        // Write Content from the file stream to the 
                        // FTP Upload Stream
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);

                        fileTransfertBar.Invoke(new Action(() => TransfertGauge(fi.Length, actualWeigth)));
                    }

                    // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
                Server_RefreshView();
            }, cancellationToken);
        }
        public void Server_RefreshView()
        {
            /*
            ListViewItem parentItem = serverListView.Items[0];
            TreeNode parentNode = (TreeNode)parentItem.Tag;
            parentNode.Nodes.Clear();
            Server_ShowLinkedFTPElements(serverPath,parentNode);
             * */
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            fileTransfertBar.Value = 0;
            Console.WriteLine("The transfert has been stop.");
        }
    }
}
