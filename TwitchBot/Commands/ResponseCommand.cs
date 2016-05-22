using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Commands
{
    public class ResponseCommand : Command
    {
        private BasicTwitchBot tb;
        private string response;

        public ResponseCommand(BasicTwitchBot tb, string response)
        {
            this.tb = tb;
            this.response = response;
        }

        public void Execute()
        {
            tb.SendChatMessage(response);
        }

        public override string ToString()
        {
            return response;
        }
    }
}
