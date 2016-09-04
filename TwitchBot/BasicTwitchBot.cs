using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;

namespace TwitchBot
{
    public class BasicTwitchBot
    {

        private readonly string userName;
        private readonly string password;
        private string channelName = "";

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private BlockingCollection<string> inputQueue;
        private BlockingCollection<string> outputQueue;
        private BlockingCollection<Command> commandQueue;

        private Thread threadLayer1;
        private Thread[] threadLayer2;
        private Thread[] threadLayer3;
        private Thread threadLayer4;

        private volatile bool stopThreads;

        private readonly int messageHandlerCount;
        private readonly int commandHandlerCount;

        private List<TwitchBotModule> modules;

        public BasicTwitchBot(string userName, string password, int messageHandlerCount = 1, int commandHandlerCount = 1)
        {
            this.userName = userName;
            this.password = password;

            threadLayer2 = new Thread[messageHandlerCount];
            threadLayer3 = new Thread[commandHandlerCount];

            this.messageHandlerCount = messageHandlerCount;
            this.commandHandlerCount = commandHandlerCount;

            modules = new List<TwitchBotModule>();

        }

        public bool Connect()
        {
            try
            {
                tcpClient = new TcpClient("irc.twitch.tv", 6667);

                inputStream = new StreamReader(tcpClient.GetStream());
                outputStream = new StreamWriter(tcpClient.GetStream());

                inputQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
                outputQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
                commandQueue = new BlockingCollection<Command>(new ConcurrentQueue<Command>());

                outputStream.WriteLine("PASS " + password);
                outputStream.WriteLine("NICK " + userName);
                outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
                outputStream.Flush();

                SetupAndStartThreads();
                return true;
            }

            catch
            {
                return false;
            }

        }

        private void SetupAndStartThreads()
        {
            stopThreads = false;

            threadLayer1 = new Thread(() =>
            {
                while (!stopThreads)
                {
                    try
                    {
                        inputQueue.Add(inputStream.ReadLine());
                    }
                    catch { }
                }
            });

            threadLayer4 = new Thread(() =>
            {
                while (!stopThreads)
                {
                    try
                    {
                        outputStream.WriteLine(outputQueue.Take());
                        outputStream.Flush();
                    }
                    catch { }
                }
            });

            for (int i = 0; i < messageHandlerCount; i++)
            {
                threadLayer2[i] = new Thread(() =>
                {
                    while(!stopThreads)
                    {
                        HandleMessageIntern(inputQueue.Take());
                    }
                });
            }

            for (int i = 0; i < commandHandlerCount; i++)
            {
                threadLayer3[i] = new Thread(() =>
                {
                    while (!stopThreads)
                    {
                        var s = commandQueue.Take().Execute();

                        if (s != null)
                        {
                            outputQueue.Add(s.convertForTwitch(userName,channelName));
                        }
                    }
                });
            }

            threadLayer4.IsBackground = true;
            threadLayer4.Start();

            foreach (Thread t in threadLayer3)
            {
                t.IsBackground = true;
                t.Start();
            }

            foreach (Thread t in threadLayer2)
            {
                t.IsBackground = true;
                t.Start();
            }

            threadLayer1.IsBackground = true;
            threadLayer1.Start();
        }

        private void HandleMessageIntern(string message)
        {
            foreach (TwitchBotModule mod in modules)
            {
                Command c = mod.HandleMessage(message);

                if (c != null)
                {
                    commandQueue.Add(c);
                }
            }
        }

        public bool JoinChannel(string channelName)
        {
            if (this.channelName != "")
            {
                LeaveChannel();
            }

            this.channelName = channelName.ToLowerInvariant();

            if (this.channelName != "")
            {
                outputQueue.Add("JOIN #" + this.channelName);
                return true;
            }
            else
            {
                return false;
            }
        }


        public void LeaveChannel()
        {
            outputQueue.Add("PART #" + channelName);
            this.channelName = "";
        }

        public void SendChatMessage(string message)
        {
            outputQueue.Add(message.convertForTwitch(userName, channelName));
        }

        public void AddModule(TwitchBotModule mod)
        {
            modules.Add(mod);
        }

        public void RemoveModule(TwitchBotModule mod)
        {
            modules.Remove(mod);
        }

        public List<TwitchBotModule> GetModules()
        {
            return modules;
        }
    }
}
