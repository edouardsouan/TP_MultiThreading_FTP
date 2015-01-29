using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FTP_Client
{
    public partial class Form1 : Form
    {
        #region Variables
        string localPath = "";
        string serverPath = "";
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Local_ShowLogicalDrives();
        }

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

    }
}
