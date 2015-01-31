using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FTP_Client.ServerEntities
{
    public class ServerTreeView : TreeView
    {
        public ServerTreeView(IContainer container) : base()
        {
            container.Add(this);
        }

        public void InitRoot()
        {
            TreeNode serverNode = new TreeNode();
            serverNode = new TreeNode("");
            serverNode.Tag = "";
            serverNode.ImageIndex = 0;
            serverNode.SelectedImageIndex = 0;
            this.Nodes.Add(serverNode);
        }

        public TreeNode CreateNode(FileServer fileServer)
        {
            string name = fileServer.GetName();
            TreeNode serverNode = new TreeNode(name);
            serverNode.Name = name;
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

            return serverNode;
        }

        public void AddNode(FileServer fileServer, TreeNode parentNode)
        {
            TreeNode serverNode = CreateNode(fileServer);
            parentNode.Nodes.Add(serverNode);
        }

        public void AddNode(TreeNode serverNode, TreeNode parentNode)
        {
            parentNode.Nodes.Add(serverNode);
        }

        public bool IsNodeADirectory(TreeNode node)
        {
            bool isADirectory = false;

            if (node.ImageIndex == 0 || node.ImageIndex == 1)
            {
                isADirectory = true;
            }

            return isADirectory;
        }

        public bool IsNodeNameOK(NodeLabelEditEventArgs e)
        {
            bool nameCheck = false;
            string newName = e.Label;

            if (newName != null)
            {
                if (newName.Length > 0)
                {
                    if (newName.IndexOfAny(new char[] { '@', ',', '!' }) == -1)
                    {
                        e.Node.EndEdit(false);
                        nameCheck = true;
                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\n" +
                            "The invalid characters are: '@', ',', '!'",
                            "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                        place the node in edit mode again. */
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
                        "Node Label Edit");
                    e.Node.BeginEdit();
                }
            }

            return nameCheck;
        }
    }
}
