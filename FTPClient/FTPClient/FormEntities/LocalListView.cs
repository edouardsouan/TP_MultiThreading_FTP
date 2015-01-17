using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.FormEntities
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
    }
}
