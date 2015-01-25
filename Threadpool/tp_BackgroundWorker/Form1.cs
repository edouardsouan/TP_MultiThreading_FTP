using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tp_BackgroundWorker
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        int maxValue = 1000;

        public Form1()
        {
            InitializeComponent();

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            progressBar1.Maximum = maxValue;
        }

        private void btnStartAsyncOperation_Click(object sender, EventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                int i = 0;
                bool cancelled = cancellationToken.WaitHandle.WaitOne(1000);

                while (i < maxValue && !cancelled)
                {
                    cancelled = cancellationToken.WaitHandle.WaitOne(1000);

                    if (cancelled)
                    {
                        //throw new OperationCanceledException(cancellationToken);
                        lblStatus.Invoke(new Action(() => lblStatus.Text = "Stopped"));

                        return;
                    }
                    else
                    {
                        lblStatus.Invoke(new Action(() => lblStatus.Text = "Value " + i + " : Cancelled? " + cancelled));
                    }
                    
                    i++;
                }

            }, cancellationToken);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }        
    }
}
