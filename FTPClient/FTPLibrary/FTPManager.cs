using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPLibrary
{
    public class FTPManager
    {
        // TODO : destructeur et logout pour clean les accès
        private string user;
        private string password;
        private string server;
        private string port;

        public FTPManager(string user, string password, string server)
        {
            this.user = user;
            this.password = password;
            this.server = server;
            this.port = "21";
        }

        public FTPManager(string user, string password, string server, string port)
        {
            this.user = user;
            this.password = password;
            this.server = server;
            this.port = port;
        }



    }
}
