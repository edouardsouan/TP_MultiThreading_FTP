using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.FormEntites
{
    public partial class LocalTreeView : TreeView
    {
        public LocalTreeView()
        {
            InitializeComponent();
        }

        public LocalTreeView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
