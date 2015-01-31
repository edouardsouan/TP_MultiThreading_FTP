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
using FTP_Client.ServerEntities;

namespace FTP_Client
{
    public partial class Form1 : Form
    {
        private async void Download_Transfert(string filePathToDownload, string localPathTarget, FileServer fileInfo)
        {
            string serverTarget = filePathToDownload;
            string fileName = fileInfo.GetName();

            if (fileInfo.IsADirectory())
            {
                try
                {
                    Directory.CreateDirectory(localPathTarget);
                    Local_RefreshView();
                }
                catch (System.IO.IOException exception)
                {
                    Console.WriteLine("File name already exist : " + exception.ToString());
                }

                FtpWebRequest ftpRequest = ftpManager.CreatRequestListDirectoriesAndFiles(serverTarget);
                FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();
                logWindow.WriteLog(ftpRequest);
                logWindow.WriteLog(ftpResponse);
                string[] serverData = ftpManager.ParseRawData(ftpResponse);

                foreach (String rawData in serverData)
                {
                    FileServer fileServer = new FileServer(rawData);
                    if (fileServer.IsNameOKToDisplay() && !cancellationToken.IsCancellationRequested)
                    {
                        Download_Transfert(serverTarget + "/" + fileServer.GetName(),
                            localPathTarget + "\\" + fileServer.GetName(),
                            fileServer);
                    }
                }
            }
            else
            {
                Download_File(localPathTarget, fileInfo, serverTarget);
            }
        }

        private void Download_File(string localPathTarget, FileServer fileInfo, string serverTarget)
        {
            fileQueue.AddItem(fileInfo);
            ListViewItem itemQueue = fileQueue.GetLastItem();

            Task.Factory.StartNew(() =>
            {
                FtpWebRequest downloadRequest = ftpManager.CreatRequestDownloadFile(serverTarget);
                Task.Factory.StartNew(() => logWindow.Invoke(new Action(() =>
                   logWindow.WriteLog(downloadRequest)
                )));
                FileStream downloadedFileStream = new FileStream(localPathTarget, FileMode.Create);

                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                Stream responseStream = downloadResponse.GetResponseStream();
                Int32 bufferSize = 2048;
                Int32 readCount;
                Byte[] buffer = new Byte[bufferSize];
                double totalWeight = (double)fileInfo.GetSize();
                double actualWeigth = 0;
                readCount = responseStream.Read(buffer, 0, bufferSize);

                DateTime beginDate = DateTime.Now;
                while (readCount > 0 && !cancellationToken.IsCancellationRequested)
                {
                    actualWeigth += readCount;
                    downloadedFileStream.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, bufferSize);

                    Task.Factory.StartNew(() =>
                    {
                        fileQueue.Invoke(new Action(() =>
                            itemQueue.SubItems[4].Text = TimeManager.CalculateTimeLeft(beginDate, actualWeigth, totalWeight).ToString()
                        ));
                        fileTransfertBar.Invoke(new Action(() =>
                            UpdateTransfertGauge(totalWeight, actualWeigth)
                        ));
                    });
                }
                responseStream.Close();
                downloadedFileStream.Close();
                downloadResponse.Close();

            }, cancellationToken);

            logWindow.WriteLog("Response:	226-File successfully transferred", Color.Green);
            Local_RefreshView();
        }
    }
}
