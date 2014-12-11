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
        private string dataType;
        private string rights;
        private string linkNumnber;
        private string owner;
        private string group;
        private long size; // Ko
        private string lastModifiedDate;
        private string name;
        #endregion

        #region constructor
        public FileServer(string rawFileServer) 
        {
            string rawFileToClean = rawFileServer.Remove(rawFileServer.LastIndexOf("\r"), 1);
            Byte[] byteFields = Encoding.ASCII.GetBytes(rawFileToClean);
            
            bool isSpaceBefore = false;
            int iCleanField = 0;
            List<string> cleanedFields = new List<string>();
            string cleanField = "";

            // 32 = ASCII code for untimely space
            foreach (Byte byteTest in byteFields)
            {
                if (byteTest == 32)
                {
                    if (!isSpaceBefore)
                    {
                        iCleanField++;
                        cleanedFields.Add(cleanField);
                        cleanField = "";
                    }
                    isSpaceBefore = true;
                }
                else
                {
                    isSpaceBefore = false;
                    Byte[] byteTempo = new Byte[] { byteTest };
                    cleanField += Encoding.ASCII.GetString(byteTempo);
                }
            }
            cleanedFields.Add(cleanField);

            this.rights  = cleanedFields.ElementAt(0).Substring(1);
            this.linkNumnber = cleanedFields.ElementAt(1);
            this.owner = cleanedFields.ElementAt(2);
            this.group = cleanedFields.ElementAt(3);
            this.size = Convert.ToInt64(cleanedFields.ElementAt(4)) % 1024;
            this.lastModifiedDate = cleanedFields.ElementAt(5) + ":" + cleanedFields.ElementAt(6) + ":" + cleanedFields.ElementAt(7);
            this.name = cleanedFields.ElementAt(8);
            SetDataType(cleanedFields.ElementAt(0));
        }

        private void SetDataType(string dataField)
        {
            this.dataType = "File";
            
            string codeType = dataField.Substring(0, 1);

            if (codeType.Equals("d"))
            {
                this.dataType = "Directory";
            }
            else
            {
                if (!this.name.Substring(0, 1).Equals("."))
                {
                    int formatIndex = this.name.LastIndexOf(".")+1;

                    if(formatIndex > -1)
                        this.dataType = this.name.Substring(formatIndex);
                }
            }
        }
        #endregion

        #region getters
        public string GetDataType() { return this.dataType; }
        public string GetRights() { return this.rights; }
        public string GetOwner() { return this.owner; }
        public string GetGroup() { return this.group; }
        public long GetSize() { return this.size; }
        public string GetLastModifiedDate() { return this.lastModifiedDate; }
        public string GetName() { return this.name; }
        #endregion
    }
}
