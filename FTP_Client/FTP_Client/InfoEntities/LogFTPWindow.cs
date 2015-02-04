using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Client.InfoEntities
{
    public class LogFTPWindow : RichTextBox
    {
        string logLocation = "";

        public LogFTPWindow(IContainer container)
        {
            container.Add(this);

            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logLocation = Path.Combine(executableLocation, "network.log");
        }

        public void WriteLog(string text, Color color)
        {
            this.SelectionColor = color;
            this.AppendText(DateTime.Now + " : " + text + "\n");
            ScrollToEnd();
        }


        public void WriteLog()
        {
            Task.Factory.StartNew(() =>
            {
                FileStream logFileStream = new FileStream(logLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader logFileReader = new StreamReader(logFileStream);

                if (logFileReader.BaseStream.Length > 2048)
                {
                    logFileReader.BaseStream.Seek(-2048, SeekOrigin.End);
                }

                string line;
                while ((line = logFileReader.ReadLine()) != null)
                {
                    this.Invoke(new Action(() =>
                        AppendText(line + "\n")
                    ));
                }
                logFileReader.Close();
                logFileStream.Close();
            });

            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            this.SelectionStart = this.Text.Length;
            this.ScrollToCaret();
        }
    }
}
