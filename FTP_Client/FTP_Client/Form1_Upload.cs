using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using TimeLibrary;

namespace FTP_Client
{
    public partial class Form1 : Form
    {
        private void Upload_Transfert(ListViewItem draggedFile, string serverPathTarget)
        {
            TreeNode fileNode = (TreeNode)draggedFile.Tag;

            try
            {
                FileInfo fileToUpload = (FileInfo)fileNode.Tag;
                Upload_File(fileToUpload.FullName, serverPathTarget);
            }
            catch (InvalidCastException exception)
            {
                Console.WriteLine("Exception " + exception.ToString() + " directory to .");

                DirectoryInfo directoryToUpload = (DirectoryInfo)fileNode.Tag;
                Upload_Directory(directoryToUpload, serverPathTarget);
            }

            Server_RefreshView();
        }

        private void Upload_Directory(DirectoryInfo directoryToUpload, string serverPathTarget)
        {
            Server_CreateDirectory(serverPathTarget);
            Server_RefreshView();
            string subServerPathTarget = serverPathTarget + "/";

            List<DirectoryInfo> subDirectories = Local_GetLocalDirectories(directoryToUpload);
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                Upload_Directory(subDirectory, subServerPathTarget + subDirectory.Name);
            }

            List<FileInfo> subFiles = Local_GetLocalFiles(directoryToUpload);
            foreach (FileInfo subFile in subFiles)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Upload_File(subFile.FullName, subServerPathTarget + subFile.Name);
                }
            }
        }

        private void Upload_File(string filePathToUpload, string serverPathTarget)
        {
            FileInfo fi = new FileInfo(filePathToUpload);
            fileQueue.AddItem(fi);
            ListViewItem itemQueue = fileQueue.GetLastItem();

            Task.Factory.StartNew(() =>
            {
                FtpWebRequest uploadRequest = ftpManager.CreatRequestUploadFile(serverPathTarget);

                Int32 buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                FileStream fs = fi.OpenRead();
                Stream strm = uploadRequest.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                double actualWeigth = 0;

                DateTime beginDate = DateTime.Now;
                while (contentLen != 0 && !cancellationToken.IsCancellationRequested)
                {
                    actualWeigth += contentLen;

                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);

                    Task.Factory.StartNew(() =>
                    {
                        fileQueue.Invoke(new Action(() =>
                            itemQueue.SubItems[4].Text = TimeManager.CalculateTimeLeft(beginDate, actualWeigth, fi.Length).ToString()
                        ));
                        fileTransfertBar.Invoke(new Action(() =>
                            UpdateTransfertGauge(fi.Length, actualWeigth)
                        ));
                    });

                }

                strm.Close();
                fs.Close();
            }, cancellationToken);

            logWindow.WriteLog("Response:	226-File successfully transferred", Color.Green);
            Server_RefreshView();
        }
    }
}
