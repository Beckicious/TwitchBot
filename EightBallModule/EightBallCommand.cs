using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot;

namespace EightBallModule
{
    class EightBallCommand : Command
    {

        private Random rng;
        private string writer;

        public EightBallCommand(string writer)
        {
            this.writer = writer;
        }

        public string Execute()
        {
            rng = new Random();

            var res = rng.NextDouble();

            if (res < 0.4)
            {
                return "@" + writer + " Yes";
            }
            else if (res < 0.8)
            {
                return "@" + writer + " No";
            }
            else
            {
                return "@" + writer + " Maybe";
            }
        }
    }
}
