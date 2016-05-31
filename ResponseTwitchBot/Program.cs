using System;
using System.IO;
using Newtonsoft.Json;

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
            try
            {
                StreamReader reader = new StreamReader(@".\data.txt");
                string s = reader.ReadLine();
                Data d = JsonConvert.DeserializeObject<Data>(s);

                ResponseTwitchBot bsbot = new ResponseTwitchBot(d.UserName, d.Token);
                bsbot.Connect();
                bsbot.StartConsoleReader();
            } catch
            {
                Console.WriteLine("no data.txt file found");
                Console.ReadLine();
            }


        }
    }
}
