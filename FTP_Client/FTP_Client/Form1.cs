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

        #region Token method
        private void GenerateNewCancellationToken()
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }
        #endregion

        #region Connection button event
        private void btnConnection_Click(object sender, EventArgs e)
        {
            serverTreeView.Nodes.Clear();
            serverTreeView.InitRoot();
            serverListView.Items.Clear();

            ftpManager = new FTPManager(this.txtUserName.Text, this.txtPassword.Text, this.txtServer.Text, this.txtPort.Text);
            Server_ShowLinkedFTPElements("", serverTreeView.Nodes[0]);
        }
        #endregion

        #region Local mouse events
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

        #region Server mouse events
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

        #region Transfert events
        private void UpdateTransfertGauge(double totalWeigth, double actualWeigth)
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

        #region Drag and Drop helpers
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

        #region Drag and Drop : download files/directory from the server
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
            GenerateNewCancellationToken();
            fileQueue.Items.Clear();

            Point pointWhereDropped = new Point(e.X, e.Y);
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
                        Download_Transfert(fileToDownloadPath, targetPath, fileFromServer);
                    }
                }
            }

        }
        #endregion

        #region Drag and Drop : upload files / directories to the server
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
            fileQueue.Items.Clear();
            GenerateNewCancellationToken();

            Point draggedFilePoint = new Point(e.X, e.Y);
            string serverPathTarget = serverPath.Replace("\\", "//");
            
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
                    Upload_Transfert(draggedFile, serverPathTarget +"//" + draggedFile.Name);
                }
            }
        }
        #endregion
    }
}
