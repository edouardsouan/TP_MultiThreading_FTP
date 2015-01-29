using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Client.InfoEntities
{
    public class LogFTPWindow : RichTextBox
    {
        public LogFTPWindow(IContainer container)
        {
            container.Add(this);
        }

        public void WriteLog(string text, Color color)
        {
            this.SelectionColor = color;
            this.AppendText(DateTime.Now + " - " + text);
            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            this.SelectionStart = this.Text.Length;
            this.ScrollToCaret();
        }

        public void WriteLog(FtpWebResponse ftpResponse)
        {
            this.WriteLog(ftpResponse.BannerMessage, Color.Green);
            this.WriteLog(ftpResponse.WelcomeMessage, Color.Green);
            this.WriteLog(ftpResponse.StatusDescription, Color.Blue);
            this.WriteLog(ftpResponse.StatusCode.ToString() + "\n", Color.Blue);
        }
    }
}
