using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.Commands;

namespace ResponseTwitchBot
{
    class Program
    {
        static void Main(string[] args)
        {

            TestBasicBot();

        }


        private static void TestBasicBot()
        {
            ResponseTwitchBot bsbot = new ResponseTwitchBot("samplebot", "oauth:kj299d8n505eqn861pct0lrceoseph");

            bsbot.Connect();

            bsbot.StartConsoleReader();

            bsbot.AddAnswerCommand("!ask");

        }
    }
}
