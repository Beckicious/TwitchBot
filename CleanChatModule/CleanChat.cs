using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;

namespace CleanChatModule
{
    public class CleanChat : TwitchBotModule
    {
        private CleanChatForm cc;
        private bool active = false;
        private readonly string name = "Clean Chat";

        public bool Activate()
        {
            return true;
        }

        public void Deactivate()
        {
            active = false;
            cc = null;
        }

        public string GetName()
        {
            return name;
        }

        public Command HandleMessage(string msg)
        {
            if (active)
            {
                if (msg.IsChatMessage())
                {
                    ChatMessage cm = msg.ParseMessageToChatMessage();

                    cc.AddLine(cm.Writer + ": " + cm.Message);
                }
            }
            return null;
        }

        public void Open()
        {
            if (cc == null)
            {
                cc = new CleanChatForm(this);
                cc.Show();
                active = true;
            }
        }
    }
}
