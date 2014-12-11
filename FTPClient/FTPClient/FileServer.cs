using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient
{
    class FileServer
    {
        #region properties
        private string type;
        private string rights;
        private string random;
        private string size;
        private string user;
        private string date;
        private string name;
        #endregion

        #region constructor
        public FileServer(string rawFileServer) {
            Byte[] textByte = Encoding.ASCII.GetBytes(rawFileServer);
            List<string> testArray = new List<string>();
            bool isSpaceBefore = false;
            int iArray = 0;
            string stringTempo = "";
            foreach (Byte byteTest in textByte)
            {
                if (byteTest == 32)
                {
                    if (!isSpaceBefore)
                    {
                        iArray++;
                        testArray.Add(stringTempo);
                        stringTempo = "";
                    }
                    isSpaceBefore = true;
                }
                else
                {
                    isSpaceBefore = false;
                    Byte[] byteTempo = new Byte[] { byteTest };
                    stringTempo += Encoding.ASCII.GetString(byteTempo);
                }

                // Console.WriteLine(byteTest);
            }
            testArray.Add(stringTempo);

            this.type = testArray.ElementAt(0).Substring(0,1);
            this.rights = testArray.ElementAt(0).Substring(1);
            this.random = testArray.ElementAt(1);
            this.size = testArray.ElementAt(2);
            this.user = testArray.ElementAt(3);
            this.date = testArray.ElementAt(4) + ":" + testArray.ElementAt(5) + ":" + testArray.ElementAt(6) + ":" + testArray.ElementAt(7);
            this.name = testArray.ElementAt(8);
        }
        #endregion

        #region getters
        public string GetType() { return this.type; }
        public string GetSize() { return this.size; }
        public string GetDate() { return this.date; }
        public string GetName() { return this.name; }
        #endregion
    }
}
