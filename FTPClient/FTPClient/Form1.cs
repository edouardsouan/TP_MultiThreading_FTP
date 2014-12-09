using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        #region Variables
        private Socket FTPSocket = null;
        private bool isLogged = false;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Destructor
        ~Form1()
        {
            LogOut();
        }
        #endregion

        #region FTP Connection
        private void LogOut()
        {
            if (FTPSocket != null)
            {
                FTPSocket.Close();
                FTPSocket = null;
            }
            isLogged = false;
        }

        private void CloseConnection(string server)
        {
            WriteLog( "STATUS : Closing Connection to " + server, Color.Red);
            if (FTPSocket != null)
            {
                SendCommand("QUIT");
            }
            LogOut();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string userName = txtUsername.Text;
            string password = txtPassword.Text;
            int port = int.Parse(txtPort.Text);

            FTPLogin(server, userName, password, port);
        }

        private void FTPLogin(string server, string userName, string password, int port)
        {
            if (isLogged)
            {
                CloseConnection(server);
            }

            WriteLog("STATUS : Opening Connection to : " + server, Color.Black);
            try
            {
                if (FTPCheckServerReady(server, port))
                {
                    if (FTPCheckUserAndPassword(server, userName, password, port))
                    {
                        isLogged = true;
                        WriteLog("STATUS : Logged to " + server, Color.Green);
                    }
                }
            }
            catch (Exception ex)
            {
                if (FTPSocket != null && FTPSocket.Connected)
                {
                    FTPSocket.Close();
                }
                WriteLog("ERROR : Couldn't connect to the server. " + ex.Message, Color.Red);
            }
        }

        private bool FTPCheckServerReady(string server, int port)
        {
            bool isServerReady = true;
            IPAddress remoteAddress = null;
            IPEndPoint addressEndPoint = null;

            FTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            WriteLog("STATUS : Resolving IP Address", Color.Black);

            remoteAddress = Dns.GetHostEntry(server).AddressList[0];
            WriteLog("STATUS : IP Address Found -> " + remoteAddress.ToString(), Color.Black);

            addressEndPoint = new IPEndPoint(remoteAddress, port);
            WriteLog("STATUS : EndPoint Found -> " + addressEndPoint.ToString(), Color.Black);

            FTPSocket.Connect(addressEndPoint);

            if ( ReadResponse() != 220)
            {
                isServerReady = false;
                CloseConnection(server);
                WriteLog("ERROR : the server is not ready for a new user.", Color.Red);
            }

            return isServerReady;
        }

        private bool FTPCheckUserAndPassword(string server, string userName, string password, int port)
        {
            bool isUserAndPwdCorrect = false;

            int statusCode = SendCommand("USER", userName);
            if (!(statusCode == 230 || statusCode == 331))
            {
                LogOut();
                WriteLog("ERROR : Login failed. Please check your user name.", Color.Red);
            }
            else
            {
                if (statusCode == 331)
                {
                    WriteLog("STATUS : the user name is ok but the server needs a password.", Color.Green);
                    statusCode = SendCommand("PASS", password);
                }

                if (statusCode == 202 || statusCode == 230)
                {
                    // 230 : user logged in
                    // 202 : command not implemented, superfluous at this site
                    isUserAndPwdCorrect = true;
                    WriteLog("STATUS : Connected to " + server, Color.Green);
                }
                else
                {
                    LogOut();
                    WriteLog("ERROR : Login failed. Please check your user name and your password.", Color.Red);
                }
            }

            return isUserAndPwdCorrect;
        }

		private int SendCommand(string command ,string msg="")
        {
			command = command.ToUpper();
            if (!command.Contains("PASS"))
            {
				WriteLog("COMMAND : "+command+ " "+ msg, Color.Blue);
            }

            Byte[] CommandBytes = Encoding.ASCII.GetBytes((command +" "+ msg + "\r\n").ToCharArray());
            FTPSocket.Send(CommandBytes, CommandBytes.Length, 0);

            return ReadResponse();
        }

        private int ReadResponse()
        {
            string result = SplitResponse();

            // StatusCode = int.Parse(result.Substring(0, 3));
            return int.Parse(result.Substring(0, 3));
        }

        private string SplitResponse()
        {
            string splitResponse = "";

            try
            {
                string statusMessage = "";
                Byte[] Buffer = new Byte[512];
                int Bytes;
                
                do
                {
                    Bytes = FTPSocket.Receive(Buffer, Buffer.Length, 0);
                    statusMessage += Encoding.ASCII.GetString(Buffer, 0, Bytes);
                } while (Bytes > Buffer.Length);

                string[] msg = statusMessage.Split('\n');

                if (statusMessage.Length > 2)
                {
                    statusMessage = msg[msg.Length - 2]; //Remove Last \n
                }
                else
                {
                    statusMessage = msg[0];
                }

                for (int i = 0; i < msg.Length - 1; i++)
                {
                    WriteLog("RESPONSE : " + msg[i], Color.Black);
                }
                
                splitResponse = statusMessage;
            }
            catch (Exception ex)
            {
                WriteLog("STATUS : ERROR. " + ex.Message , Color.Red);
                FTPSocket.Close();
            }

            return splitResponse;
        }
        #endregion

        #region TreeView with local directories and local files
        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateTreeViewWithLogicalDrives();
        }

        private void PopulateTreeViewWithLogicalDrives()
        {
            TreeNode rootNode;

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
                    treeViewLocalFiles.Nodes.Add(rootNode);
                }
            }
        }

        private void treeViewLocalFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode nodeClicked = e.Node;
                if (IsNodeADirectory(nodeClicked))
                {
                    if (nodeClicked.Nodes.Count == 0)
                    {
                        PropulateTreeNodeWithDirectories(nodeClicked);
                        PopulateTreeNodeWithFiles(nodeClicked);
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog("ERROR : "+ex.Message, Color.Red);
            }
        }

        private void PropulateTreeNodeWithDirectories(TreeNode nodeClicked)
        {
            
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (DirectoryInfo subDir in nodeClickedInfo.GetDirectories())
            {
                TreeNode newDirNode = new TreeNode(subDir.Name, 0, 0);
                newDirNode.Tag = subDir;
                newDirNode.ImageIndex = 1;
                newDirNode.SelectedImageIndex = 1;
                nodeClicked.Nodes.Add(newDirNode);
            }
        }

        private void PopulateTreeNodeWithFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (FileInfo file in nodeClickedInfo.GetFiles())
            {
                TreeNode newFileNode = new TreeNode(file.Name, 0, 0);
                newFileNode.Tag = file;
                newFileNode.ImageIndex = 2;
                newFileNode.SelectedImageIndex = 2;
                nodeClicked.Nodes.Add(newFileNode);
            }

        }
        #endregion

        #region Upload
        private void treeViewLocalFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;

            if (IsNodeADirectory(nodeClicked))
            {
                DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            }
            else
            {
                FileInfo nodeClickedInfo = (FileInfo)nodeClicked.Tag;
            }

            WriteLog("UploadFile : "+nodeClicked.FullPath, Color.Blue);
            // UploadFile();
        }
        
		private void UploadFile(string localPath, string remotePath)
        {
			if (!isLogged) {
				WriteLog("STATUS :Please loggin .", Color.Red);
			} else {
				
			}
            
        }

        private bool IsNodeADirectory(TreeNode node)
        {
            bool isADirectory = false;
            if (node.ImageIndex == 0 || node.ImageIndex == 1)
            {
                isADirectory = true;
            }
            return isADirectory;
        }
        #endregion


        #region Log Window
        private void WriteLog(string text, Color color)
        {
            logWindow.SelectionColor = color;
            logWindow.AppendText(DateTime.Now+" - "+text+"\n");
        }
        #endregion
    }
}
