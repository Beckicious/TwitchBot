using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Commands
{
    public class CommandFactory
    {
        private BasicTwitchBot tb;

        public CommandFactory(BasicTwitchBot tb)
        {
            this.tb = tb;
        }

        public Command CreateResponseCommand(string response)
        {
            return new ResponseCommand(tb, response);
        }
    }
}
