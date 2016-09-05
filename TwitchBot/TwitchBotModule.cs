using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public interface TwitchBotModule
    {
        void Open();

        bool Activate();

        string GetName();

        Command HandleIncomingMessage(string msg);
        Command HandleOutgoingMessage(string msg);
    }
}
