using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FTPClient.ServerEntities
{
    public class FTPManager
    {
        // TODO : destructeur et logout pour clean les accès
        private string user;
        private string password;
        private string server;
        private string port;

        public FTPManager(string user, string password, string server, string port)
        {
            this.user = user;
            this.password = password;
            this.server = server;
            this.port = port;
        }

        ~FTPManager()
        {
            LogOut();
        }

        public void LogOut()
        {
            this.user = "";
            this.password = "";
            this.server = "";
            this.port = "";
        }

        public FtpWebRequest CreateFtpWebRequest(string complementPath)
        {
            string serverTarget = "ftp://" + this.server + ":" + this.port + "/" + complementPath;

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
            ftpRequest.KeepAlive =true;
            ftpRequest.UsePassive = true;
            ftpRequest.Credentials = new NetworkCredential(this.user, this.password);

            return ftpRequest;
        }

        public FtpWebRequest CreatRequestMakeDirectory(string complementPath)
        {
            FtpWebRequest ftpRequest = CreateFtpWebRequest(complementPath);
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

            return ftpRequest;
        }

        public FtpWebRequest CreatRequestUploadFile(string complementPath)
        {
            FtpWebRequest ftpRequest = CreateFtpWebRequest(complementPath);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            return ftpRequest;
        }

        public FtpWebRequest CreatRequestDownloadFile(string complementPath)
        {
            FtpWebRequest ftpRequest = CreateFtpWebRequest(complementPath);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            return ftpRequest;
        }

        public FtpWebRequest CreatRequestListDirectoriesAndFiles(string complementPath)
        {
            FtpWebRequest ftpRequest = CreateFtpWebRequest(complementPath);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            return ftpRequest;
        }

        public string[] ParseRawData(FtpWebResponse ftpResponse)
        {
            Stream responseStream = ftpResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);

            string rawResult = streamReader.ReadToEnd();
            Console.WriteLine(rawResult);
            
            string data = rawResult.Remove(rawResult.LastIndexOf("\n"), 1);
           
            // data = Regex.Replace(rawResult, "\r", "");
            //string data = Regex.Replace(rawResult, "\r", "");
            //data = rawResult.Remove(data.LastIndexOf("\n"), 1);

            streamReader.Close();
            ftpResponse.Close();

            return data.Split('\n');
        }
    }
}
