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

        #region Tools -> to put in another class
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

        #region TreeView with local directories and local files
        private void Form1_Load(object sender, EventArgs e)
        {
            Local_ShowLogicalDrives();    
        }

        private void Local_ShowLogicalDrives()
        {
            string[] drives = Environment.GetLogicalDrives();

            foreach (string drive in drives)
            {
                DirectoryInfo logicalDrive = new DirectoryInfo(drive);
                localTreeView.AddNode(logicalDrive, 0);
                listViewLocal.AddItem(logicalDrive);
            }
        }

        private List<DirectoryInfo> Local_GetLocalDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();

            if (nodeClicked.Nodes.Count == 0)
            {
                subDirectories = nodeClickedInfo.GetDirectories().ToList();
            }
            else
            {
                TreeNodeCollection subNodes = nodeClicked.Nodes;
                foreach (TreeNode subNode in subNodes)
                {
                    if (IsADirectory(subNode))
                    {
                        subDirectories.Add((DirectoryInfo)subNode.Tag);
                    }
                }
            }

            return subDirectories;
        }

        private List<FileInfo> Local_GetLocalFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<FileInfo> subFiles = new List<FileInfo>();

            if (nodeClicked.Nodes.Count == 0)
            {
                subFiles = nodeClickedInfo.GetFiles().ToList();
            }
            else
            {
                TreeNodeCollection subNodes = nodeClicked.Nodes;
                foreach (TreeNode subNode in subNodes)
                {
                    if (!IsADirectory(subNode))
                    {
                        subFiles.Add((FileInfo)subNode.Tag);
                    }
                }
            }

            return subFiles;
        }

        private void Local_ShowLinkedDirectories(TreeNode nodeClicked, List<DirectoryInfo> subDirectories)
        {
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                localTreeView.AddNode(subDirectory, 1, nodeClicked);
                listViewLocal.AddItem(subDirectory);
            }
        }

        private void Local_ShowLinkedFiles(TreeNode nodeSelected,List<FileInfo> subFiles)
        {
            foreach (FileInfo subFile in subFiles)
            {
                localTreeView.AddNode(subFile, 2, nodeSelected);
                listViewLocal.AddItem(subFile);
            }
        }

        private void Local_ShowLinkedElements(TreeNode nodeSelected)
        {
            listViewLocal.ClearItems();

            List<DirectoryInfo> sudDirectories = Local_GetLocalDirectories(nodeSelected);
            List<FileInfo> subFiles = Local_GetLocalFiles(nodeSelected);

            Local_ShowLinkedDirectories(nodeSelected, sudDirectories);
            nodeSelected.Expand();
            Local_ShowLinkedFiles(nodeSelected, subFiles);
        }

        private void treeViewLocal_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Local_ShowLinkedElements(nodeClicked);

            localPath = nodeClicked.FullPath;
        }
        #endregion

        #region Treeview with server directories and local files
        private void btnConnection_Click(object sender, EventArgs e)
        {
            TreeNode serverNode = new TreeNode();
            serverNode = new TreeNode("/");
            serverNode.Tag = "/";
            serverNode.ImageIndex = 0;
            serverNode.SelectedImageIndex = 0;
            treeViewServer.Nodes.Add(serverNode);

            GetTreeViewFromServer("", treeViewServer.Nodes[0]);
        }

        private async void GetTreeViewFromServer(string serverPath, TreeNode parentNode)
        {
            string serverTarget = "ftp://" + this.txtServer.Text + serverPath + "/";

            if (!isSendingListCommand)
            {
                try
                {
                    isSendingListCommand = true;
                    FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Credentials = new NetworkCredential(this.txtUserName.Text, this.txtPassword.Text);
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                    FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();
                    logWindow.WriteLog(ftpResponse.BannerMessage, Color.Green);
                    logWindow.WriteLog(ftpResponse.WelcomeMessage, Color.Green);
                    logWindow.WriteLog(ftpResponse.StatusDescription, Color.Blue);
                    logWindow.WriteLog(ftpResponse.StatusCode.ToString(), Color.Blue);
                    logWindow.WriteLog(WebRequestMethods.Ftp.ListDirectoryDetails, Color.Black);

                    Stream responseStream = ftpResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);

                    string rawResult = streamReader.ReadToEnd();
                    string data = rawResult.Remove(rawResult.LastIndexOf("\n"), 1);
                    BuildServerTreeView(data.Split('\n'), parentNode);

                    streamReader.Close();
                    ftpResponse.Close();
                }
                catch (WebException ex)
                {
                    logWindow.WriteLog(ex.Message, Color.Red);
                    isSendingListCommand = false;
                }
                finally
                {
                    isSendingListCommand = false;
                }
            }
        }

        // TODO : Build in async
        private void BuildServerTreeView(string[] directories, TreeNode parentNode)
        {
            FileServer fileServer;
            List<FileServer> fileSystInfos = new List<FileServer>();

            foreach (string rawDirectory in directories)
            {
                fileServer = new FileServer(rawDirectory);
                if (!(fileServer.GetName().Equals(".") || fileServer.GetName().Equals("..")))
                {
                    AddNodeServerTreeView(fileServer, parentNode);
                    fileSystInfos.Add(fileServer);
                }
            }

            PopulateServerListView(fileSystInfos.ToArray());
            parentNode.Expand();
        }

        private void AddNodeServerTreeView(FileServer fileServer, TreeNode parentNode)
        {
            TreeNode serverNode = new TreeNode(fileServer.GetName());
            serverNode.Tag = fileServer;

            if (fileServer.GetDataType().Equals("Directory"))
            {
                serverNode.ImageIndex = 1;
                serverNode.SelectedImageIndex = 1;
            }
            else
            {
                serverNode.ImageIndex = 2;
                serverNode.SelectedImageIndex = 2;
            }

            parentNode.Nodes.Add(serverNode);
        }

        private void treeViewServer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            if (IsADirectory(nodeClicked))
            {
                serverPath = nodeClicked.FullPath;
                listViewServer.Items.Clear();
                if (nodeClicked.Nodes.Count == 0)
                {
                    GetTreeViewFromServer(nodeClicked.FullPath, nodeClicked);
                }
                else
                {
                    List<FileServer> fileSystInfos = new List<FileServer>();
                    TreeNodeCollection nodes = nodeClicked.Nodes;
                    foreach (TreeNode node in nodes)
                    {
                        fileSystInfos.Add((FileServer)node.Tag);
                    }
                    PopulateServerListView(fileSystInfos.ToArray());
                }
            }
        }

        private void PopulateServerListView(FileServer[] files)
        {
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            string extension = "";
            string size = "";
            string date = "";
            string rights = "";
            string owner = "";
            string group = "";

            foreach (FileServer subFile in files)
            {
                item = new ListViewItem(subFile.GetName(), 0);
                item.Name = subFile.GetName();
                item.Tag = subFile;
                size = subFile.GetSize().ToString();
                extension = subFile.GetDataType();
                date = subFile.GetLastModifiedDate();
                rights = subFile.GetRights();
                owner = subFile.GetOwner();
                group = subFile.GetGroup();

                subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, size),
                        new ListViewItem.ListViewSubItem(item, extension), 
                        new ListViewItem.ListViewSubItem(item, date),
                        new ListViewItem.ListViewSubItem(item, rights),
                        new ListViewItem.ListViewSubItem(item, owner),
                        new ListViewItem.ListViewSubItem(item, group)
                    };

                if (extension.Equals("Directory"))
                {
                    item.ImageIndex = 1;
                }
                else
                {
                    item.ImageIndex = 2;
                }

                item.SubItems.AddRange(subItems);
                listViewServer.Items.Add(item);
            }

            listViewServer.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #endregion

        #region Download files/directory from the server
        private void listViewLocal_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            listViewLocal.DoDragDrop(e.Item, DragDropEffects.Move);
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
            if (fileToDownload.ListView == listViewServer)
            {
                string fileToDownloadPath = serverPath + "/" + fileToDownload.Name;
                string targetPath = localPath;

                try
                {
                    Point pointWhereFileDropped = listViewLocal.PointToClient(new Point(e.X, e.Y));
                    ListViewItem targetFile = listViewLocal.GetItemAt(pointWhereFileDropped.X, pointWhereFileDropped.Y);
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
            listViewServer.DoDragDrop(e.Item, DragDropEffects.Move);
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
                Point targetPoint = listViewServer.PointToClient(new Point(e.X, e.Y));
                ListViewItem targetFile = listViewServer.GetItemAt(targetPoint.X, targetPoint.Y);
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
