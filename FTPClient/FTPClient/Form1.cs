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
        private Socket ftpSocket = null;
        private bool logged = false;

        public Form1()
        {
            InitializeComponent();
        }

        ~Form1()
        {
            FTPLogout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateTreeViewWithLogicalDrives();
        }


        #region PopulateTreeView
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
                    treeViewLocalFiles.Nodes.Add(rootNode);
                }
            }
        }

        private void treeViewLocalFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;

            if (nodeClicked.Nodes.Count == 0)
            {
                PropulateTreeNodeWithDirectories(nodeClicked);
                PropulateTreeNodeWithFiles(nodeClicked);
            }
        }

        private void PropulateTreeNodeWithDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (DirectoryInfo subDir in nodeClickedInfo.GetDirectories())
            {
                TreeNode newDirNode = new TreeNode(subDir.Name, 0, 0);
                newDirNode.Tag = nodeClickedInfo;
                newDirNode.ImageIndex = 0;
                nodeClicked.Nodes.Add(newDirNode);
            }

        }

        private void PropulateTreeNodeWithFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (FileInfo file in nodeClickedInfo.GetFiles())
            {
                TreeNode newFileNode = new TreeNode(file.Name, 0, 0);
                newFileNode.Tag = nodeClickedInfo;
                newFileNode.ImageIndex = 1;
                nodeClicked.Nodes.Add(newFileNode);
            }
        }
        #endregion

        #region FTP
        private void toolStripButtonConnection_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            int port = int.Parse(txtPort.Text);
            FTPLogin(server, username, password, port);
        }

        private void FTPLogin(string server, string username, string password, int port)
        {
            if (logged)
            {
                CloseConnection();
            }
            IPAddress remoteAddress = null;
            IPEndPoint addressEndPoint = null;

            WriteTextInLogWindow("Status : Opening Connection to " + server, Color.Green);
            try
            {
                ftpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                WriteTextInLogWindow("Status : Resolving IP Adress ", Color.Green);

                remoteAddress = Dns.GetHostEntry(server).AddressList[0];
                WriteTextInLogWindow("Status : IP Adresse found ->  " + remoteAddress, Color.Green);

                addressEndPoint = new IPEndPoint(remoteAddress, port);
                WriteTextInLogWindow("Status : IP EndPoint found ->  " + addressEndPoint, Color.Green);

                if (IsServerReadyForANewUser())
                {
                    FTPLogin(username, password);
                }
            }
            catch (Exception ex)
            {
                if (ftpSocket != null && ftpSocket.Connected)
                {
                    ftpSocket.Close();
                }
                WriteTextInLogWindow("ERROR : Couldn't connect to remote server. " + ex.Message, Color.Red);
            }
        }

        private bool IsServerReadyForANewUser()
        {
            bool canGoOn = false;
            int replyCode = FetchNumericReplyCode();

            if (replyCode != 220)
            {
                CloseConnection();
                WriteTextInLogWindow("STATUS : " + replyCode.ToString(), Color.Red);
            }
            else
            {
                canGoOn = true;
            }

            return canGoOn;
        }

        private void FTPLogin(string userName, string password)
        {
            int replyCode = SendCommand("USER " + userName);

            if( !( replyCode == 230 || replyCode == 331 || replyCode == 530) )
            {
                FTPLogout();
                WriteTextInLogWindow("STATUS : " + replyCode.ToString(), Color.Red);
            }
            else
            {
                if (replyCode != 230)
                {
                    FTPLoginSendPassword(password);
                }
                else
                {
                    logged = true;
                    WriteTextInLogWindow("STATUS : connection succeed ", Color.Green);
                }
            }
        }

        private void FTPLoginSendPassword(string password)
        {
            int replyCode = SendCommand("PASS "+password);

            // 202 : the command is not implemented, superfluous at this site / password not required
            // 230 : the user is logged in, proceed
            if (!(replyCode == 202 || replyCode == 230))
            {
                FTPLogout();
                WriteTextInLogWindow("STATUS : " + replyCode.ToString(), Color.Red);
            }
            else
            {
                logged = true;
                WriteTextInLogWindow("STATUS : connection succeed ", Color.Green);
            }
        }

        private void FTPLogout()
        {
            if (ftpSocket != null)
            {
                ftpSocket.Close();
                ftpSocket = null;
            }
            logged = false;

            WriteTextInLogWindow("Status : Connection has been closed", Color.Green);
        }

        private void CloseConnection()
        {
            WriteTextInLogWindow("Status : Closing Connection to ", Color.Black);
            if (ftpSocket != null)
            {
                int replyCode = SendCommand("QUIT");
                WriteTextInLogWindow("Response : " + replyCode.ToString(), Color.Black);
            }
            FTPLogout();
        }

        private int SendCommand(string command)
        {
            WriteTextInLogWindow("Command : "+command, Color.Blue);

            Byte[] commandBytes = Encoding.ASCII.GetBytes((command+"\r\n").ToCharArray());
            ftpSocket.Send(commandBytes, commandBytes.Length, 0);

            return FetchNumericReplyCode();
        }

        private int FetchNumericReplyCode()
        {
            string splitedResponse = "";

            try
            {
                string statusMessage = TranslateBytesIntoStatusMessage();
                String[] messageMultiligne = statusMessage.Split('\n');

                if (messageMultiligne.Length > 2)
                {
                    splitedResponse = messageMultiligne[messageMultiligne.Length - 2];
                }
                else
                {
                    splitedResponse = messageMultiligne[0];
                }

                for (int iMessage = 0; iMessage < messageMultiligne.Length - 1; iMessage++ )
                {
                    WriteTextInLogWindow("Response : " + messageMultiligne[iMessage], Color.Black);
                }
            }
            catch(Exception ex)
            {
                WriteTextInLogWindow("Status : ERROR "+ex.Message, Color.Red);
                ftpSocket.Close();
            }

            return int.Parse(splitedResponse.Substring(0, 3));
        }

        private string TranslateBytesIntoStatusMessage()
        {
            Byte[] socketBuffer = new Byte[512];
            int bytesReceived = ftpSocket.Receive(socketBuffer, socketBuffer.Length, 0);
            return Encoding.ASCII.GetString(socketBuffer, 0, bytesReceived);
        }
        #endregion

        #region Log Window
        private void WriteTextInLogWindow(string message, Color colour)
        {
            DateTime now = DateTime.Now;
            logWindow.SelectionColor = colour;
            logWindow.AppendText(now + " - " + message + "\n");
        }
        #endregion
    }
}
