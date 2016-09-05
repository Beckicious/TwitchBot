using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;

namespace DashboardModule
{
    public class Dashboard : TwitchBotModule
    {
        private DashboardForm df;
        private readonly string name = "Dashboard";

        public bool Activate()
        {
            return true;
        }

        public void Deactivate()
        {
            df = null;
        }

        public string GetName()
        {
            return name;
        }

        public Command HandleIncomingMessage(string msg)
        {
            return null;
        }

        public Command HandleOutgoingMessage(string msg)
        {
            return null;
        }

        public void Open()
        {
            if (df == null)
            {
                df = new DashboardForm(this);
                df.Show();
            }
        }
    }
}
