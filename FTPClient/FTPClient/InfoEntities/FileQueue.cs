using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FTPClient.ServerEntities;

namespace FTPClient.InfoEntities
{
    public partial class FileQueue : ListView
    {
        public FileQueue()
        {
            InitializeComponent();
        }

        public FileQueue(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void AddItem(FileServer fileToDownload)
        {
            ListViewItem item = new ListViewItem(fileToDownload.GetName());
            item.Name = fileToDownload.GetName();
            string direction = "<--";
            string distFile = fileToDownload.GetName();
            string taille = fileToDownload.GetSize().ToString();

            ListViewItem.ListViewSubItem[]  subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, direction), 
                            new ListViewItem.ListViewSubItem(item, distFile),
                            new ListViewItem.ListViewSubItem(item, taille)
                        };
            item.SubItems.AddRange(subItems);

            this.Items.Add(item);
        }
        
    }
}
