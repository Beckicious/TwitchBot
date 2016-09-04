using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;
using System.Net;

namespace UptimeModule
{
    class UptimeCommand : Command
    {
        private WebClient wc;
        private string channelname;
        private string writer;

        public UptimeCommand(string channelname, string writer)
        {
            wc = new WebClient();
            this.channelname = channelname;
            this.writer = writer;
        }


        public string Execute()
        {
            return "@" + writer + " " + wc.DownloadString("https://api.rtainc.co/twitch/uptime?channel=" + channelname.ToLowerInvariant());
        }
    }
}
