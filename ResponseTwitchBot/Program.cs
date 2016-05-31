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
                reader.Close();

                ResponseTwitchBot bsbot = new ResponseTwitchBot(d.UserName, d.Token);
                bsbot.Connect();
                bsbot.StartConsoleReader();
            } catch
            {
                var d = new Data("INSERT_TWITCH_BOT_ACCOUNT_NAME_HERE", "INSERT_TWITCH_BOT_TOKEN_HERE");
                string s = JsonConvert.SerializeObject(d);

                StreamWriter writer = new StreamWriter(@".\data.txt");
                writer.WriteLine(s);
                writer.Flush();
                writer.Close();

                Console.WriteLine("no data.txt file found");
                Console.WriteLine("Empty data.txt file created, please replace the values.");
                Console.ReadLine();
            }


        }
    }
}
