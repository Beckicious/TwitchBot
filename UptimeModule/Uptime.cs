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

        public void Deactivate()
        {
            //nothing to do
        }

        public string GetName()
        {
            return name;
        }

        public void HandleChannelJoin(string channelName)
        {
            //nothing to do
        }

        public void HandleChannelLeave(string channelName)
        {
            //nothing to do
        }

        public Command HandleIncomingMessage(string msg)
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

        public Command HandleOutgoingMessage(string msg)
        {
            return null;
        }

        public void Open()
        {
            //nothing to do
        }
    }
}
