using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DashboardModule
{
    public partial class DashboardForm : Form
    {
        private Dashboard df;

        public DashboardForm(Dashboard df)
        {
            InitializeComponent();
            this.df = df;
            webBrowser.Navigate("www.twitch.tv/dashboard");
        }

        private void DashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            df.Deactivate();
        }
    }
}
