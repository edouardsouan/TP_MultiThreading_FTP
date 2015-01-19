using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            txtPassword.TextBox.PasswordChar = '*';
        }
        #endregion

        #region Destructor
        ~Form1()
        {
            ftpManager.LogOut();
        }
        #endregion

        #region IsADirectory => Put in each Tree (local and server) ?
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
                    FileServer fileFromServer = (FileServer)fileToDownload.Tag;
                    
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
                }
                catch (System.IO.IOException exception)
                {
                    Console.WriteLine("File name already exist : " + exception.ToString());
                    /*
                     * 3 options possibles :
                     * - ignorer : arrêter
                     * - remplacer : on suppose que mettre à jour un fichier devrait mettre à jour 
                     * la date du dernier accès du dossier
                     * - copier : rajouter 1
                     * => Pour le moment on rajoute "1"
                     * */
                    fileName += "1"; 
                }

                FtpWebRequest ftpRequest = ftpManager.CreatRequestListDirectoriesAndFiles(serverTarget);
                FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();
                logWindow.WriteLog(ftpResponse);
                string[] serverData = ftpManager.ParseRawData(ftpResponse);

                foreach (String rawData in serverData)
                {
                    FileServer fileServer = new FileServer(rawData, filePathToDownload);
                    if (fileServer.IsNameOKToDisplay())
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
            FtpWebRequest downloadRequest = ftpManager.CreateFtpWebRequest(serverTarget);
            downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            FileStream downloadedFileStream = new FileStream(localPathTarget, FileMode.Create);
            FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
            Stream responseStream = downloadResponse.GetResponseStream();
            Int32 bufferSize = 2048;
            Int32 readCount;
            Byte[] buffer = new Byte[bufferSize];
            FileServer fileSize = (FileServer)fileInfo;
            double totalWeight = (double)fileSize.GetSize();
            double actualWeigth = 0;
            readCount = responseStream.Read(buffer, 0, bufferSize);
            while (readCount > 0)
            {
                actualWeigth += readCount;
                downloadedFileStream.Write(buffer, 0, readCount);
                readCount = responseStream.Read(buffer, 0, bufferSize);
                fileTransfertBar.Invoke(new Action(() => TransfertGauge(totalWeight, actualWeigth)));
            }
            responseStream.Close();
            downloadedFileStream.Close();
            downloadResponse.Close();
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
            // TO REFACTO
        }
        #endregion
              
//
//        #region Upload files / directories to the server
//        private void listViewServer_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
//        {
//            serverListView.DoDragDrop(e.Item, DragDropEffects.Move);
//        }
//        private void listViewServer_DragEnter(object sender, DragEventArgs e)
//        {
//            e.Effect = DragDropEffects.Move;
//        }
//        private void listViewServer_DragDrop(object sender, DragEventArgs e)
//        {
//            String filePath = this.localPath;
//            ListViewItem draggedFile = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
//            try
//            {
//                Point targetPoint = serverListView.PointToClient(new Point(e.X, e.Y));
//                ListViewItem targetFile = serverListView.GetItemAt(targetPoint.X, targetPoint.Y);
//                if (IsADirectory((FileInfo)targetFile.Tag))
//                {
//                    filePath += '\\' + targetFile.Name;
//                }
//            }
//            catch (NullReferenceException exception)
//            {
//                Console.WriteLine("Exception " + exception.ToString() + " no file directory pointed.");
//            }
//            finally
//            {
//                FileInfo fileToUpload = (FileInfo)draggedFile.Tag;
//                filePath += "\\" + fileToUpload.Name;
//                UploadFile(filePath, serverPath.Replace("\\", "//") + "//" + fileToUpload.Name, fileToUpload);
//            }
//        }
//        // TODO : Traitement récursif pour les dossiers
//        private void UploadFile(string filePathToUpload, string distPathTarget, FileInfo fileInfo)
//        {
//            string serverTarget = "ftp://" + this.txtServer.Text + distPathTarget;
//            FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
//            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
//            uploadRequest.Credentials = new NetworkCredential(this.txtUserName.Text, this.txtPassword.Text);
//
//
//            StreamReader uploadFileStream = new StreamReader(filePathToUpload);
//            byte[] fileContents = System.IO.File.ReadAllBytes(filePathToUpload);
//            uploadFileStream.Close();
//            uploadRequest.ContentLength = fileContents.Length;
//
//
//            Stream responseStream = uploadRequest.GetRequestStream();
//            responseStream.Write(fileContents, 0, fileContents.Length);
//            responseStream.Close();
//
//            FtpWebResponse uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
//
//
//            responseStream.Close();
//            uploadFileStream.Close();
//            uploadResponse.Close();
//
//            Console.WriteLine("Download Complete, status {0}", uploadResponse.StatusDescription);
//        }
//        #endregion

    }
}
