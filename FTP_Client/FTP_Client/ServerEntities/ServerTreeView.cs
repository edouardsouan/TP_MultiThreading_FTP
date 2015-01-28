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

        public TreeNode CreateNode(FileServer fileServer, TreeNode parentNode)
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
            TreeNode serverNode = CreateNode(fileServer, parentNode);
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
    }
}
