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

namespace KFile
{
    public partial class Form1 : Form
    {
        #region Variables
        private Byte[] Buffer = new Byte[512];
        private int Bytes;
        private bool Logged = false;
        private Socket FTPSocket = null;
        private string Password = "";
        private int Port;
        private string Result="";
        private string Server = "";
        private int StatusCode;
        private string StatusMessage = "";
        private string UserName = "";
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
            Server = txtServer.Text;
            UserName = txtusername.Text;
            Password = txtPassword.Text;
            Port = int.Parse(txtPort.Text);
            FTPLogin();

            // TODO : put it in the log window
            // Alert the user for the moment
            if (Logged)
            {
                MessageBox.Show("Logged !");
            }
            else
            {
                MessageBox.Show("Please check your user name and your password.");
            }
        }
        #endregion

        #region Handle FTP Connection

        private void FTPLogin()
        {
            // Close the connection if one is already open
            if (Logged)
            {
                CloseConnection();
            }

            // Clean IP variables
            IPAddress remoteAddress = null;
            IPEndPoint addrEndPoint = null;
            
            // Try to open the connection, can we proceed ?
            WriteTextInLogWindow(logWindow, "Status : Opening Connection to : " + Server + "\n", Color.Red);
            try
            {
                FTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                WriteTextInLogWindow(logWindow, "Status : Resolving IP Address\n", Color.Red);
                remoteAddress = Dns.GetHostEntry(Server).AddressList[0];
                WriteTextInLogWindow(logWindow, "Status : IP Address Found ->" + remoteAddress.ToString() + "\n", Color.Red);
                addrEndPoint = new IPEndPoint(remoteAddress, Port);
                WriteTextInLogWindow(logWindow, "Status : EndPoint Found ->" + addrEndPoint.ToString() + "\n", Color.Red);
                FTPSocket.Connect(addrEndPoint);

                StartFTPConnection();
            }
            catch (Exception ex)
            {
                if (FTPSocket != null && FTPSocket.Connected)
                {
                    FTPSocket.Close();
                }
                WriteTextInLogWindow(logWindow, "Status : Couldn't connect to remote server. " + ex.Message + "\n", Color.Red);
            }
        }

        private void StartFTPConnection()
        {
            ReadResponse();
            FTPLoginHandleFTPSocketResponse();
        }

        private void FTPLoginHandleFTPSocketResponse()
        {
            // 220 : the server is ready for a new user
            if (StatusCode != 220)
            {
                CloseConnection();
                WriteTextInLogWindow(logWindow, "Status : " + Result.Substring(4) + "\n", Color.Red); //Error
            }
            else
            {
                FTPLoginSendUserName();
            }
        }

        private void FTPLoginSendUserName()
        {
            SendCommand("USER " + UserName);

            // 230 : the user is logged in, proceed
            // 331 : the user has been recognized, waiting for the password
            // 530 : connection fail, inform the user
            if (!(StatusCode == 230 || StatusCode == 331) || StatusCode == 530)
            {
                FTPLogout();
                WriteTextInLogWindow(logWindow, "Status : " + Result.Substring(4) + "\n", Color.Red);
            }
            else
            {
                // Need to send the password to the server
                if (StatusCode != 230)
                {
                    FTPLoginSendPassword();
                }
                else
                {
                    Logged = true;
                    WriteTextInLogWindow(logWindow, "Status : Connected to " + Server + "\n", Color.Red);
                }
            }
        }

        private void FTPLoginSendPassword()
        {
            SendCommand("PASS " + Password);

            // 202 : the command is not implemented, superfluous at this site / password not required
            // 230 : the user is logged in, proceed
            if (!(StatusCode == 202 || StatusCode == 230))
            {
                FTPLogout();
                WriteTextInLogWindow(logWindow, "Status : " + Result.Substring(4) + "\n", Color.Red);
            }
            else
            {
                Logged = true;
                WriteTextInLogWindow(logWindow, "Status : Connected to " + Server + "\n", Color.Red);
            }
        }

        private void ReadResponse()
        {
            StatusMessage = "";
            Result = SplitResponse();
            StatusCode = int.Parse(Result.Substring(0, 3));
        }

        private string SplitResponse()
        {
            string splitedResponse = ""; 

            try
            {
                TranslateBytesIntoStatusMessage();

                string[] msg = StatusMessage.Split('\n');
                if (StatusMessage.Length > 2)
                {
                    // Remove Last \n
                    StatusMessage = msg[msg.Length - 2];
                }
                else
                {
                    StatusMessage = msg[0];
                }

                for (int i = 0; i < msg.Length - 1; i++)
                {
                    WriteTextInLogWindow(logWindow, "Response : " + msg[i] + "\n", Color.Green);
                }

                splitedResponse = StatusMessage;
            }
            catch (Exception ex)
            {
                WriteTextInLogWindow(logWindow, "Status : ERROR. " + ex.Message + "\n", Color.Red);
                FTPSocket.Close();
            }

            return splitedResponse;
        }

        private void TranslateBytesIntoStatusMessage()
        {
            // Count number of Bytes : socket lenght
            Bytes = FTPSocket.Receive(Buffer, Buffer.Length, 0);
            // Convert bytes into a string so we can understand the message
            StatusMessage += Encoding.ASCII.GetString(Buffer, 0, Bytes);
        }


        private void SendCommand(string msg)
        {
            WriteTextInLogWindow(logWindow, "Command : " + msg + "\n", Color.Blue);
            Byte[] CommandBytes = Encoding.ASCII.GetBytes((msg + "\r\n").ToCharArray());
            FTPSocket.Send(CommandBytes, CommandBytes.Length, 0);
            //read Response
            ReadResponse();
        }
        #endregion

        #region Close Connection Properly
        private void CloseConnection()
        {
            WriteTextInLogWindow(logWindow, "Status : Closing Connection to " + Server + "\n", Color.Red);
            if (FTPSocket != null)
            {
                SendCommand("QUIT");
            }
            FTPLogout();
        }

        private void FTPLogout()
        {
            if (FTPSocket != null)
            {
                FTPSocket.Close();
                FTPSocket = null;
            }
            Logged = false;
        }
        #endregion

        #region Log Window
        private void WriteTextInLogWindow(RichTextBox box, string text, Color color)
        {
            // TODO : show message in a log window (rchLog)
            // Like upper part in FileZila
            MessageBox.Show(text);
        }
        #endregion
    }
}
