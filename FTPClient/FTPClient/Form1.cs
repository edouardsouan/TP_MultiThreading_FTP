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

using FTPLibrary;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        #region Variables
        bool isSendingListCommand = false;
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
            // LogOut(); Mesure de sécurité, à refaire ???
        }
        #endregion

        #region IsADirectory => Put in each Tree (local and server) ?
        private bool IsADirectory(FileSystemInfo fileSystemInfo)
        {
            bool isADirectory = false;

            FileAttributes attributes = File.GetAttributes(fileSystemInfo.FullName);
            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                isADirectory = true;

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
            /* 
             * TODO : Drag & Drop depuis le treeViewServer
             * Si targetFileInfo null alors prendre le chemin de localView
             * Traitement différents de la listView
             * Récupérer un TreeNode et non un ListViewItem
             */

            ListViewItem fileToDownload = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            if (fileToDownload.ListView == serverListView)
            {
                string fileToDownloadPath = serverPath + "/" + fileToDownload.Name;
                string targetPath = localPath;

                try
                {
                    Point pointWhereFileDropped = localListView.PointToClient(new Point(e.X, e.Y));
                    ListViewItem targetFile = localListView.GetItemAt(pointWhereFileDropped.X, pointWhereFileDropped.Y);
                    FileSystemInfo targetFileInfo = (FileSystemInfo)targetFile.Tag;
                    if (IsADirectory(targetFileInfo))
                    {
                        targetPath += '\\' + targetFile.Name;
                    }
                }
                catch (NullReferenceException exception)
                {
                    Console.WriteLine("Exception " + exception.ToString() + " was thrown because no Patrice was found !!");
                    Console.WriteLine("Exception " + exception.ToString() + " no file directory pointed.");
                }
                finally
                {
                    targetPath += "\\" + fileToDownload.Name;
                    ListViewItem.ListViewSubItem[] subItems;
                    ListViewItem item = new ListViewItem(targetPath);
                    item.Name = targetPath;
                    string direction = "<--";
                    string distFile = fileToDownloadPath;
                    string taille = ((FileServer)fileToDownload.Tag).GetSize().ToString();

                    subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, direction), 
                            new ListViewItem.ListViewSubItem(item, distFile),
                            new ListViewItem.ListViewSubItem(item, taille)
                        };
                    item.SubItems.AddRange(subItems);
                    FileQueue.Items.Add(item);

                    DownloadFile(fileToDownloadPath, targetPath, (FileServer)fileToDownload.Tag);
                }

            }
        }

        // TODO : Traitement récursif pour les dossiers
        private async void DownloadFile(string filePathToDownload, string localPathTarget, FileServer fileInfo)
        {
            string serverTarget = "ftp://" + this.txtServer.Text + "/" + filePathToDownload;
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
                     * */
                }

                // Get file and directory list
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
                ftpRequest.KeepAlive = false;
                ftpRequest.Credentials = new NetworkCredential(this.txtUserName.Text, this.txtPassword.Text);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();
                Stream responseStream = ftpResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);

                string rawResult = streamReader.ReadToEnd();
                string data = rawResult.Remove(rawResult.LastIndexOf("\n"), 1);
                string[] serverData = data.Split('\n');

                foreach (String rawData in serverData)
                {
                    FileServer fileServer = new FileServer(rawData);
                    if (!(fileServer.GetName().Equals(".") || fileServer.GetName().Equals("..")))
                    {
                        DownloadFile(filePathToDownload + "/" + fileServer.GetName(), localPathTarget + "\\" + fileServer.GetName(), fileServer);
                    }
                }
            }
            else
            {
                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                downloadRequest.Credentials = new NetworkCredential(this.txtUserName.Text, this.txtPassword.Text);

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

            // TODO : update treeViewLocal and listViewLocal
        }
        #endregion


        // --------------------------------------------------------------
        //  TODO : upload to the server
        // --------------------------------------------------------------
        #region File Transfert
        private void TransfertGauge(double totalWeigth, double actualWeigth)
        {
            fileTransfertBar.Minimum = 0;
            fileTransfertBar.Maximum = (int)totalWeigth;

            fileTransfertBar.Value = (int)actualWeigth;
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
            String filePath = this.localPath;
            ListViewItem draggedFile = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            try
            {
                Point targetPoint = serverListView.PointToClient(new Point(e.X, e.Y));
                ListViewItem targetFile = serverListView.GetItemAt(targetPoint.X, targetPoint.Y);
                if (IsADirectory((FileInfo)targetFile.Tag))
                {
                    filePath += '\\' + targetFile.Name;
                }
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine("Exception " + exception.ToString() + " no file directory pointed.");
            }
            finally
            {
                FileInfo fileToUpload = (FileInfo)draggedFile.Tag;
                filePath += "\\" + fileToUpload.Name;
                UploadFile(filePath, serverPath.Replace("\\", "//") + "//" + fileToUpload.Name, fileToUpload);
            }
        }
        // TODO : Traitement récursif pour les dossiers
        private void UploadFile(string filePathToUpload, string distPathTarget, FileInfo fileInfo)
        {
            string serverTarget = "ftp://" + this.txtServer.Text + distPathTarget;
            FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
            uploadRequest.Credentials = new NetworkCredential(this.txtUserName.Text, this.txtPassword.Text);



            // Copy the contents of the file to the request stream.
            /*
            StreamReader sourceStream = new StreamReader("testfile.txt");
            byte [] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            */
            // ----


            StreamReader uploadFileStream = new StreamReader(filePathToUpload);
            byte[] fileContents = System.IO.File.ReadAllBytes(filePathToUpload);
            uploadFileStream.Close();
            uploadRequest.ContentLength = fileContents.Length;


            Stream responseStream = uploadRequest.GetRequestStream();
            responseStream.Write(fileContents, 0, fileContents.Length);
            responseStream.Close();

            FtpWebResponse uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
            // ???
            //  FtpWebResponse uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();


            responseStream.Close();
            uploadFileStream.Close();
            uploadResponse.Close();

            // TODO : update treeViewLocal and listViewLocal
            Console.WriteLine("Download Complete, status {0}", uploadResponse.StatusDescription);
        }
        #endregion
    }
}
