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

namespace CleanChatModule
{
    public partial class CleanChatForm : Form
    {
        private CleanChat cc;

        public CleanChatForm(CleanChat cc)
        {
            this.cc = cc;

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
                cleanChatBox.AppendText(msg + "\r\n");
            }
            catch (ObjectDisposedException) { }
        }

        private void CleanChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cc.Deactivate();
        }
    }
}
