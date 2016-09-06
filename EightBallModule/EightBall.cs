using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;

namespace EightBallModule
{
    public class EightBall : TwitchBotModule
    {

        private readonly string name = "8Ball";

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

                if (cm.FirstWord.ToLowerInvariant() == "!ask")
                {
                    return new EightBallCommand(cm.Writer);
                }
            }

            return null;
        }

        public Command HandleOutgoingMessage(string msg)
        {
            return HandleIncomingMessage(msg);
        }

        public void Open()
        {
            //nothing to do
        }
    }
}
