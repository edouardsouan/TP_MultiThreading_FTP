using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.ServerEntities
{
    public partial class ServerTreeView : TreeView
    {
        public ServerTreeView()
        {
            InitializeComponent();
        }

        public ServerTreeView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
