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
using System.Windows.Forms;

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

        #region Log Window
        private void WriteLog(string text, Color color)
        {
            logWindow.SelectionColor = color;
            logWindow.AppendText(DateTime.Now + " - " + text + "\n");
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
            
            PopulateLocalTreeViewWithLogicalDrives();
        }

        private void PopulateLocalTreeViewWithLogicalDrives()
        {
            TreeNode rootNode;
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(drive);
                if (directoryInfo.Exists)
                {
                    
                    rootNode = new TreeNode(directoryInfo.Name);
                    rootNode.Tag = directoryInfo;
                    rootNode.ImageIndex = 0;
                    rootNode.SelectedImageIndex = 0;
                    treeViewLocal.Nodes.Add(rootNode);

                    directories.Add(directoryInfo);
                }
            }

            PopulateLocalListView(directories.ToArray());
        }

        private void treeViewLocal_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode nodeClicked = e.Node;

                if (IsADirectory(nodeClicked))
                {
                    localPath = nodeClicked.FullPath;
                    listViewLocal.Items.Clear();

                    if (nodeClicked.Nodes.Count == 0)
                    {
                        PropulateLocalTreeNodeWithDirectories(nodeClicked);
                        PopulateLocalTreeNodeWithFiles(nodeClicked);
                    }
                    else
                    {
                        List<FileSystemInfo> fileSystInfos = new List<FileSystemInfo>();
                        TreeNodeCollection nodes = nodeClicked.Nodes;
                        foreach (TreeNode node in nodes)
                        {
                            fileSystInfos.Add((FileSystemInfo)node.Tag);
                        }
                        PopulateLocalListView(fileSystInfos.ToArray());
                    }

                    nodeClicked.Expand();
                }
            }
            catch(Exception ex)
            {
                WriteLog("ERROR : "+ex.Message, Color.Red);
            }
        }

        private void PropulateLocalTreeNodeWithDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<DirectoryInfo> files = new List<DirectoryInfo>();

            foreach (DirectoryInfo subDir in nodeClickedInfo.GetDirectories())
            {
                AddLocalTreeNode(subDir, 1, nodeClicked);
                files.Add(subDir);
            }

            PopulateLocalListView(files.ToArray());
        }

        private void PopulateLocalTreeNodeWithFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<FileInfo> files = new List<FileInfo>();

            foreach (FileInfo file in nodeClickedInfo.GetFiles())
            {
                AddLocalTreeNode(file, 2, nodeClicked);
                files.Add(file);
            }

            PopulateLocalListView(files.ToArray());
        }

        private void AddLocalTreeNode(FileSystemInfo data, int imageIndex, TreeNode parentNode)
        {
            TreeNode newFileNode = new TreeNode(data.Name, 0, 0);
            newFileNode.Tag = data;
            newFileNode.ImageIndex = imageIndex;
            newFileNode.SelectedImageIndex = imageIndex;
            parentNode.Nodes.Add(newFileNode);
        }

        private void PopulateLocalListView(FileSystemInfo[] files)
        {
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            string extension = "";
            string size = "";
            string LastAccessTime = "";

            foreach (FileSystemInfo subFile in files)
            {
                item = new ListViewItem(subFile.Name, 0);
                item.Name = subFile.Name;
                item.Tag = subFile;
                extension = subFile.Extension;
                LastAccessTime = subFile.LastAccessTime.ToShortDateString();

                try
                {
                    FileInfo fileInfo = (FileInfo)subFile;
                    size = fileInfo.Length.ToString();
                }
                catch
                {
                    size = "";
                }

                subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, size),
                        new ListViewItem.ListViewSubItem(item, extension), 
                        new ListViewItem.ListViewSubItem(item, LastAccessTime)
                    };

                if (extension.Equals(""))
                    item.ImageIndex = 1;
                else
                    item.ImageIndex = 2;

                item.SubItems.AddRange(subItems);
                listViewLocal.Items.Add(item);
            }

            listViewLocal.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
                    WriteLog(ftpResponse.BannerMessage, Color.Green);
                    WriteLog(ftpResponse.WelcomeMessage, Color.Green);
                    WriteLog(ftpResponse.StatusDescription, Color.Blue);
                    WriteLog(ftpResponse.StatusCode.ToString(), Color.Blue);
                    WriteLog(WebRequestMethods.Ftp.ListDirectoryDetails, Color.Black);

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
                    WriteLog(ex.Message, Color.Red);
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
                
                Point pointWhereFileDropped = listViewLocal.PointToClient(new Point(e.X, e.Y));
                ListViewItem targetFile = listViewLocal.GetItemAt(pointWhereFileDropped.X, pointWhereFileDropped.Y);
                FileSystemInfo targetFileInfo = (FileSystemInfo)targetFile.Tag;
                string targetPath = localPath;
                if(IsADirectory(targetFileInfo))
                    targetPath += '\\' + targetFile.Name;
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

        // TODO : Traitement récursif pour les dossiers
        private void DownloadFile(string filePathToUpload, string localPathTarget, FileServer fileInfo)
        {
            string serverTarget = "ftp://" + this.txtServer.Text + "/" + filePathToUpload;
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
                fileTransfertBar.Invoke(new Action(()=>TransfertGauge(totalWeight ,actualWeigth)));
            }
            responseStream.Close();
            downloadedFileStream.Close();
            downloadResponse.Close();

            // TODO : update treeViewLocal and listViewLocal
            Console.WriteLine("Download Complete, status {0}", downloadResponse.StatusDescription);
        }
        #endregion


        // --------------------------------------------------------------
        //  TODO : upload to the server
        // --------------------------------------------------------------
        #region File Transfert
        private void TransfertGauge(double totalWeigth, double actualWeigth){
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
            Point targetPoint = listViewServer.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetFile = listViewServer.GetItemAt(targetPoint.X, targetPoint.Y);

            ListViewItem draggedFile = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            Console.WriteLine(draggedFile + "->" + targetFile);
        }
        #endregion
    }
}
