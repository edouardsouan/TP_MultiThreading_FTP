using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.FormEntites
{
    public partial class LocalListView : ListView
    {
        public LocalListView()
        {
            InitializeComponent();
        }

        public LocalListView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void AddItem(FileSystemInfo fileInfo)
        {
            ListViewItem newItem = GenerateItem(fileInfo);
            this.Items.Add(newItem);
        }

        private ListViewItem GenerateItem(FileSystemInfo fileInfo)
        {
            ListViewItem newItem = new ListViewItem(fileInfo.Name, 0);
            newItem.Name = fileInfo.Name;
            newItem.Tag = fileInfo;
            newItem.ImageIndex = AssignImage(fileInfo.Extension);

            ListViewItem.ListViewSubItem[] newSubItems = new ListViewItem.ListViewSubItem[]
            {
                new ListViewItem.ListViewSubItem(newItem, RetrieveSize(fileInfo)),
                new ListViewItem.ListViewSubItem(newItem, fileInfo.Extension), 
                new ListViewItem.ListViewSubItem(newItem, fileInfo.LastAccessTime.ToShortDateString())
            };
            newItem.SubItems.AddRange(newSubItems);

            return newItem;
        }

        private string RetrieveSize(FileSystemInfo fileInfo)
        {
            string size = "";

            try
            {
                FileInfo fileDetails = (FileInfo)fileInfo;
                size = fileDetails.Length.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception + " : The file is a directory so its size is not displayed");
            }

            return size;
        }

        private int AssignImage(string fileExtension)
        {
            int imageIndex = 1;

            if (!fileExtension.Equals(""))
                imageIndex = 2;

            return imageIndex;
        }

        public void ClearItems()
        {
            this.Items.Clear();
        }
    }
}
