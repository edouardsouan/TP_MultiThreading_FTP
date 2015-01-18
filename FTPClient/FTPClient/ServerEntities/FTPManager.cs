using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        private FtpWebRequest CreateFtpWebRequest(string complementPath)
        {
            string serverTarget = "ftp://" + this.server + ":" + this.port + complementPath + "/";
            
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
            ftpRequest.KeepAlive = false;
            ftpRequest.Credentials = new NetworkCredential(this.user, this.password);

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
            string data = rawResult.Remove(rawResult.LastIndexOf("\n"), 1);

            streamReader.Close();
            ftpResponse.Close();

            return data.Split('\n');
        }



    }
}
