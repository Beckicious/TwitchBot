using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RawChatModule
{
    public partial class RawChatForm : Form
    {
        private RawChat rc;

        public RawChatForm(RawChat rc)
        {
            this.rc = rc;

            InitializeComponent();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddLine(string msg)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AddLine), new object[] { msg });
                return;
            }

            try
            {
                rawChatBox.AppendText(msg + "\r\n");
            }
            catch (ObjectDisposedException) { }
        }

        private void RawChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rc.Deactivate();
        }
    }
}
