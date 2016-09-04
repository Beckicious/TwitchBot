using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;
using System.Net;

namespace UptimeModule
{
    public class Uptime : TwitchBotModule
    {
        private readonly string name = "Uptime";


        public bool Activate()
        {
            return true;
        }

        public string GetName()
        {
            return name;
        }

        public Command HandleMessage(string msg)
        {
            if (msg.IsChatMessage())
            {
                ChatMessage cm = msg.ParseMessageToChatMessage();

                if (cm.FirstWord.ToLowerInvariant() == "!uptime")
                {
                    return new UptimeCommand(cm.Channel, cm.Writer);
                }
            }

            return null;
        }

        public void Open()
        {
            //do nothing
        }
    }
}
