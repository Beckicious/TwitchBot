using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;
using System.Windows.Forms;

namespace RawChatModule
{
    public class RawChat : TwitchBotModule
    {
        private RawChatForm rw;
        private bool active = false;
        private readonly string name = "Raw Chat";

        public bool Activate()
        {
            return true;
        }

        public void Deactivate()
        {
            active = false;
            rw = null;
        }

        public Command HandleIncomingMessage(string msg)
        {
            if(active)
            {
                rw.AddLine(msg);
            }
            return null;
        }

        public Command HandleOutgoingMessage(string msg)
        {
            if (active)
            {
                rw.AddLine(msg);
            }
            return null;
        }

        public string GetName()
        {
            return name;
        }

        public void Open()
        {
            if (rw == null)
            {
                rw = new RawChatForm(this);
                rw.Show();
                active = true;
            }
        }
    }
}
