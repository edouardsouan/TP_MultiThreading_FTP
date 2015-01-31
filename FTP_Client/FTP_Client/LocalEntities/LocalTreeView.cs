using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Client.LocalEntities
{
    public class LocalTreeView : TreeView
    {
        public LocalTreeView()
        {
        }

        public void AddRootNode(TreeNode rootNode)
        {
            this.Nodes.Add(rootNode);
        }

        public void AddNode(TreeNode childNode, TreeNode parentNode)
        {
            parentNode.Nodes.Add(childNode);
        }

        public TreeNode GenerateTreeNode(FileSystemInfo fileInfo, int imageIndex)
        {
            string name = fileInfo.Name;
            TreeNode node = new TreeNode(name);
            node.Name = name;
            node.Tag = fileInfo;
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;

            return node;
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

        public bool IsNodeALogicalDrive(TreeNode node)
        {
            bool isADirectory = false;
            if (node.ImageIndex == 0)
            {
                isADirectory = true;
            }
            return isADirectory;
        }

        public void RenameNode(NodeLabelEditEventArgs e)
        {
            if (IsNodeNameOK(e))
            {
                if (IsNodeALogicalDrive(e.Node))
                {
                    e.CancelEdit = true;
                    MessageBox.Show("You can not rename a logical drive");
                }
                else
                {
                    if (IsNodeADirectory(e.Node))
                    {
                        RenameDirectory((DirectoryInfo)e.Node.Tag, e.Label);
                    }
                    else
                    {
                        RenameFile((FileInfo)e.Node.Tag, e.Label);
                    }
                    e.Node.Name = e.Label;
                }
            }
        }

        private void RenameDirectory(DirectoryInfo dirToRename, string newName)
        {
            try
            {
                dirToRename.MoveTo(Path.Combine(dirToRename.Parent.FullName, newName));
            }
            catch(IOException exception)
            {
                Console.WriteLine(exception.ToString());
                MessageBox.Show("Access denied");
            }
        }
        private void RenameFile(FileInfo fileToRename, string newName)
        {
            try
            {
                File.Move(fileToRename.FullName, Path.Combine(fileToRename.DirectoryName.ToString(), newName));
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.ToString());
                MessageBox.Show("Access denied");
            }
        }

        private bool IsNodeNameOK(NodeLabelEditEventArgs e)
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

        #region Delete
        public void DeleteNode(TreeNode nodeToDelete)
        {
            if (IsNodeADirectory(nodeToDelete))
            {
                DirectoryInfo dirToDelete = (DirectoryInfo)nodeToDelete.Tag;
                dirToDelete.Delete(true);
            }
            else
            {
                FileInfo fileToDelete = (FileInfo)nodeToDelete.Tag;
                fileToDelete.Delete();
            }
        }
        #endregion
    }
}
