using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FTP_Client.ServerEntities;

namespace FTP_Client.InfoEntities
{
    public class FileQueue : ListView
    {
        public void AddItem(FileServer fileToDownload)
        {
            ListViewItem item = new ListViewItem(fileToDownload.GetName());
            item.Name = fileToDownload.GetName();
            string direction = "<--";
            string distFile = fileToDownload.GetName();
            string taille = fileToDownload.GetSize().ToString();

            ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, direction), 
                            new ListViewItem.ListViewSubItem(item, distFile),
                            new ListViewItem.ListViewSubItem(item, taille),
                            new ListViewItem.ListViewSubItem(item, "---")
                        };
            item.SubItems.AddRange(subItems);

            this.Items.Add(item);
        }

        public void AddItem(FileInfo fileToUpload)
        {
            ListViewItem item = new ListViewItem(fileToUpload.Name);
            item.Name = fileToUpload.Name;
            string direction = "-->";
            string distFile = fileToUpload.Name;
            string taille = fileToUpload.Length.ToString();

            ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, direction), 
                            new ListViewItem.ListViewSubItem(item, distFile),
                            new ListViewItem.ListViewSubItem(item, taille),
                            new ListViewItem.ListViewSubItem(item, "---")
                        };
            item.SubItems.AddRange(subItems);

            this.Items.Add(item);
        }

        public ListViewItem GetLastItem()
        {
            return this.Items[this.Items.Count - 1];
        }
    }
}
