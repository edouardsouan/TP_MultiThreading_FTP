using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient
{
    public partial class LogFTPWindow : RichTextBox
    {
        public LogFTPWindow()
        {
            InitializeComponent();
        }

        public LogFTPWindow(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
