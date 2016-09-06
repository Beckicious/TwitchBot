using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using TwitchBot;


namespace ResponseTwitchBot
{
    public class ResponseTwitchBot : BasicTwitchBot
    {

        private ConcurrentDictionary<string, Command> commandDict;

        public ResponseTwitchBot(string botTwitchName, string botTwitchToken): base(botTwitchName, botTwitchToken)
        {
            commandDict = new ConcurrentDictionary<string, Command>();

            LoadDictFromString();

            Console.WriteLine("Welcome to this simple text TwitchBot.");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Type 'join' to join a channel");
            Console.WriteLine("Type 'leave' to leave the channel");
            Console.WriteLine("Type 'add' to add a command");
            Console.WriteLine("Type 'remove' to remove a command");
            Console.WriteLine("Type 'commandlist' to see registered commands");
            Console.WriteLine("---------------------------------------\n");
        }

        public void StartConsoleReader()
        {
            new Thread(() =>
            {
                while(true)
                {
                    string msg = Console.ReadLine();

                    if (msg.ToLowerInvariant() == "commandlist")
                    {
                        PrintCommandList();
                    }

                    else if (msg.ToLowerInvariant() == "add")
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("Add a command");
                        Console.WriteLine("Please enter the trigger:");
                        string trigger = Console.ReadLine();

                        Console.WriteLine("Please enter the response:");
                        string response = Console.ReadLine();
                        AddResponseCommand(trigger, response);
                        Console.WriteLine("---------------------------------------\n");
                    }

                    else if (msg.ToLowerInvariant() == "remove")
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("Remove a command");
                        Console.WriteLine("Please enter the trigger:");
                        string trigger = Console.ReadLine();
                        RemoveResponseCommand(trigger);
                        Console.WriteLine("---------------------------------------\n");
                    }

                    else if (msg.ToLowerInvariant() == "join")
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("Join a channel");
                        Console.WriteLine("Please enter the channel name:");
                        string channelName = Console.ReadLine();
                        JoinChannel(channelName);
                        Console.WriteLine("---------------------------------------\n");
                    }

                    else if (msg.ToLowerInvariant() == "leave")
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("left channel");
                        LeaveChannel();
                        Console.WriteLine("---------------------------------------\n");
                    }


                    else
                    {
                        Console.WriteLine("command is unknown");
                        Console.WriteLine("---------------------------------------\n");
                    }
                }

            }).Start();
        }

        public void AddResponseCommand(string trigger, string response)
        {
            //commandDict.AddOrUpdate(trigger, new ResponseCommand(this, response), (string s, Command c) => { return new ResponseCommand(this, response); });
            //Properties.Settings.Default.commandDict = ConvertDictToString();
            //Properties.Settings.Default.Save();
        }

        public void AddAnswerCommand(string trigger)
        {
            //commandDict.AddOrUpdate(trigger, new AnswerCommand(this), (string s, Command c) => { return new AnswerCommand(this); });
        }

        public void RemoveResponseCommand(string trigger)
        {
            Command c;
            commandDict.TryRemove(trigger, out c);
            Properties.Settings.Default.commandDict = ConvertDictToString();
            Properties.Settings.Default.Save();
        }

        public void PrintCommandList()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("The following commands have been added:");
            foreach (string s in commandDict.Keys)
            {
                Console.WriteLine($"trigger: {s} | response: {commandDict[s].ToString()}");
            }
            Console.WriteLine("---------------------------------------\n");
        }

        private string ConvertDictToString()
        {
            string dictString = "";

            foreach (string s in commandDict.Keys)
            {
                dictString += s + "¢";
                dictString += commandDict[s].ToString() + "¢";
            }

            return dictString;
        }

        private void LoadDictFromString()
        {
            string[] dictStrings = Properties.Settings.Default.commandDict.Split('¢');

            for (int i = 0; i < dictStrings.Length / 2; i++)
            {
                //commandDict.TryAdd(dictStrings[2 * i], new ResponseCommand(this, dictStrings[(2 * i) + 1]));
            }
        }
    }
}
