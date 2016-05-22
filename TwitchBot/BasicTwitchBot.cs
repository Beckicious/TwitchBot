using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;
using TwitchBot.Commands;

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

        private readonly int messageHandlerCount;
        private readonly int commandHandlerCount;

        public BasicTwitchBot(string userName, string password, int messageHandlerCount = 10, int commandHandlerCount = 10)
        {
            this.userName = userName;
            this.password = password;

            this.messageHandlerCount = messageHandlerCount;
            this.commandHandlerCount = commandHandlerCount;

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

                startReadWriting();
                return true;
            }

            catch
            {
                return false;
            }

        }

        public void startReadWriting()
        {

            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        inputQueue.Add(inputStream.ReadLine());
                    }
                    catch { }
                }
            }).Start();

            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        outputStream.WriteLine(outputQueue.Take());
                        outputStream.Flush();
                    }
                    catch { }
                }
            }).Start();

            for (int i = 0; i < messageHandlerCount; i++)
            {
                new Thread(() =>
                {
                    while(true)
                    {
                        HandleMessageIntern(inputQueue.Take());
                    }
                }).Start();
            }

            for (int i = 0; i < commandHandlerCount; i++)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        commandQueue.Take().Execute();
                    }
                }).Start();
            }
        }

        private void HandleMessageIntern(string message)
        {
            if (message.IsChatMessage())
            {
                ChatMessage cm = message.ParseMessageToChatMessage();

                Command c = HandleMessage(cm);

                if (c != null)
                {
                    commandQueue.Add(c);
                }
            }
        }

        public virtual Command HandleMessage(ChatMessage cm)
        {
            return new ResponseCommand(this, $"Hi {cm.Writer}, you wrote: {cm.Message}");
        }

        public bool JoinChannel(string channelName)
        {
            this.channelName = channelName;

            if (this.channelName != "")
            {
                outputQueue.Add("JOIN #" + channelName);
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
    }
}
