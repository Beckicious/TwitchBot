using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public interface TwitchBotModule
    {
        string GetName();

        void Open();

        bool Activate();
        void Deactivate();

        void HandleChannelJoin(string channelName);
        void HandleChannelLeave(string channelName);

        Command HandleIncomingMessage(string msg);
        Command HandleOutgoingMessage(string msg);
    }
}
