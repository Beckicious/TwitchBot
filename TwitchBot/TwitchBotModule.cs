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

        Command HandleMessage(string msg);
    }
}
