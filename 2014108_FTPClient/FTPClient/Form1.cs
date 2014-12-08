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
        private Byte[] buffer = new Byte[512];
        private int bytes;
        private bool isLogged = false;
        private Socket ftpSocket = null;
        private string password = "";
        private int port;
        private string result = "";
        private string server = "";
        private int statusCode;
        private string statusMessage = "";
        private string userName = "";
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            // The password characters are only asterisks
            txtPassword.TextBox.PasswordChar = '*';
        }
        #endregion

        #region Destructor
        ~Form1()
        {
            FTPLogout();
        }
        #endregion

        #region Buttons actions
        private void btnConnect_Click(object sender, EventArgs e)
        {
            server = txtServer.Text;
            userName = txtUsername.Text;
            password = txtPassword.Text;
            port = int.Parse(txtPort.Text);
            FTPLogin();

            // TODO : put it in the log window
            // Alert the user for the moment
            if (isLogged)
            {
                WriteTextInLogWindow(logWindow, "Logged !", Color.Red);
            }
            else
            {
                WriteTextInLogWindow(logWindow, "Please check your user name and your password.", Color.Red);
            }
        }
        #endregion

        #region Handle FTP Connection

        private void FTPLogin()
        {
            // Close the connection if one is already open
            if (isLogged)
            {
                CloseConnection();
            }

            // Clean IP variables
            IPAddress remoteAddress = null;
            IPEndPoint addressEndPoint = null;

            // Try to open the connection, can we proceed ?
            WriteTextInLogWindow(logWindow, "Status : Opening Connection to : " + server + "\n", Color.Red);
            try
            {
                ftpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                WriteTextInLogWindow(logWindow, "Status : Resolving IP Address\n", Color.Red);
                remoteAddress = Dns.GetHostEntry(server).AddressList[0]; //for IPv6 but our serveur support only IPv4
                remoteAddress = IPAddress.Parse(Dns.GetHostEntry(server).AddressList[0].ToString());
                //                IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                //                remoteAddress = ipv4Addresses[0];
                WriteTextInLogWindow(logWindow, "Status : IP Address Found ->" + remoteAddress.ToString() + "\n", Color.Red);
                addressEndPoint = new IPEndPoint(remoteAddress, port);
                WriteTextInLogWindow(logWindow, "Status : EndPoint Found ->" + addressEndPoint.ToString() + "\n", Color.Red);
                ftpSocket.Connect(addressEndPoint);

                StartFTPConnection();
            }
            catch (Exception ex)
            {
                if (ftpSocket != null && ftpSocket.Connected)
                {
                    ftpSocket.Close();
                }
                WriteTextInLogWindow(logWindow, "Status : Couldn't connect to remote server. " + ex.Message + "\n", Color.Red);
            }
        }

        private void StartFTPConnection()
        {
            int statusCode = FetchStatusCode();
            FTPLoginHandleFTPSocketResponse();
        }

        private void FTPLoginHandleFTPSocketResponse()
        {
            // 220 : the server is ready for a new user
            if (statusCode != 220)
            {
                CloseConnection();
                WriteTextInLogWindow(logWindow, "Status : " + result.Substring(4), Color.Red); //Error
            }
            else
            {
                FTPLoginSendUserName();
            }
        }

        private void FTPLoginSendUserName()
        {
            SendCommand("USER " + userName);

            // 230 : the user is logged in, proceed
            // 331 : the user has been recognized, waiting for the password
            // 530 : connection fail, inform the user
            if (!(statusCode == 230 || statusCode == 331) || statusCode == 530)
            {
                FTPLogout();
                WriteTextInLogWindow(logWindow, "Status : " + result.Substring(4) + "\n", Color.Red);
            }
            else
            {
                // Need to send the password to the server
                if (statusCode != 230)
                {
                    FTPLoginSendPassword();
                }
                else
                {
                    isLogged = true;
                    WriteTextInLogWindow(logWindow, "Status : Connected to " + server, Color.Red);
                }
            }
        }

        private void FTPLoginSendPassword()
        {
            SendCommand("PASS " + password);

            // 202 : the command is not implemented, superfluous at this site / password not required
            // 230 : the user is logged in, proceed
            if (!(statusCode == 202 || statusCode == 230))
            {
                FTPLogout();
                WriteTextInLogWindow(logWindow, "Status : " + result.Substring(4) + "\n", Color.Red);
            }
            else
            {
                isLogged = true;
                WriteTextInLogWindow(logWindow, "Status : Connected to " + server + "\n", Color.Red);
            }

            SendFile("/Users/edouard/Documents/test.txt", "/www/tempo");
        }

        private int FetchStatusCode()
        {
            statusMessage = "";
            result = SplitResponse();
            int statusCode = int.Parse(result.Substring(0, 3));

            return statusCode;
        }

        private string SplitResponse()
        {
            string splitedResponse = "";

            try
            {
                TranslateBytesIntoStatusMessage();

                string[] msg = statusMessage.Split('\n');
                if (statusMessage.Length > 2)
                {
                    // Remove Last \n
                    statusMessage = msg[msg.Length - 2];
                }
                else
                {
                    statusMessage = msg[0];
                }

                for (int i = 0; i < msg.Length - 1; i++)
                {
                    WriteTextInLogWindow(logWindow, "Response : " + msg[i], Color.Green);
                }

                splitedResponse = statusMessage;
            }
            catch (Exception ex)
            {
                WriteTextInLogWindow(logWindow, "Status : ERROR. " + ex.Message, Color.Red);
                ftpSocket.Close();
            }
            WriteTextInLogWindow(logWindow, "SplitResponse() : statusMessage.Split =" + splitedResponse, Color.Red);

            return splitedResponse;
        }

        private void TranslateBytesIntoStatusMessage()
        {
            bytes = ftpSocket.Receive(buffer, buffer.Length, 0);
            statusMessage += Encoding.ASCII.GetString(buffer, 0, bytes);
            WriteTextInLogWindow(logWindow, "StatusMessage = " + statusMessage, Color.Black);
        }


        private void SendCommand(string msg)
        {
            WriteTextInLogWindow(logWindow, "SendCommand : msg = " + msg, Color.Blue);
            Byte[] CommandBytes = Encoding.ASCII.GetBytes((msg + "\r\n").ToCharArray());
            ftpSocket.Send(CommandBytes, CommandBytes.Length, 0);
            int statusCode = FetchStatusCode();
        }
        #endregion

        #region Close Connection Properly
        private void CloseConnection()
        {
            WriteTextInLogWindow(logWindow, "Status : Closing Connection to " + server, Color.Black);
            if (ftpSocket != null)
            {
                SendCommand("QUIT");
            }
            FTPLogout();
        }

        private void FTPLogout()
        {
            if (ftpSocket != null)
            {
                ftpSocket.Close();
                ftpSocket = null;
            }
            isLogged = false;

            WriteTextInLogWindow(logWindow, "Close socket and logout", Color.Black);
        }
        #endregion

        #region Log Window
        private void WriteTextInLogWindow(RichTextBox box, string text, Color color)
        {
            DateTime now = DateTime.Now;
            string finalLog = now + " ---- " + text + "\n";
            logWindow.SelectionColor = color;
            logWindow.AppendText(finalLog);
            logWindow.SelectionColor = logWindow.ForeColor;
        }
        #endregion

        #region Send File
        public void SendFile(string filePath, string repositoryPath)
        {
            /*
             * if (ftpSocket != null) {
                ftpSocket = null;
            }
            */
            SendCommand("CD " + repositoryPath);
            SendCommand("STOR");
            //FileStream fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read);
            // Send file fileName to remote device
            WriteTextInLogWindow(logWindow, "Sending " + filePath + " to the host.", Color.Black);
            ftpSocket.SendFile(filePath);

            // Release the socket.
            /* ftpSocket.Shutdown(SocketShutdown.Both);
            ftpSocket.Close();
            */
        }

        #endregion
    }
}
