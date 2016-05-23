using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;
using TwitchBot.Commands;

namespace TwitchBot
{
    /// <summary>
    /// basic implementation of a twitch chat communicator
    /// 
    /// Multithreaded with 4 layers
    /// 1.layer
    ///     1 thread that constantly reads from the inputstream and writes it into the inputQueue
    /// 
    /// 2.layer
    ///     number of threads (default 10) that take messages from the inputQueue and handle the 
    ///     messages with HandleMessage and put the result into the commandQueue if needed
    /// 
    /// 3.layer
    ///     number of threads (default 10) that take commands from the commandQueue and execute them
    ///     if needed commands will write messages into the outputQueue
    /// 
    /// 4.layer
    ///     1 thread that constantly takes messages from the outputQueue and writes them into the outputStream
    /// </summary>
    public class BasicTwitchBot
    {
        /// <summary>
        /// userName of the twitchbot account
        /// </summary>
        private readonly string userName;

        /// <summary>
        /// token of the twitchbot account (https://twitchapps.com/tmi/)
        /// </summary>
        private readonly string token;
        private string channelName = "";

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private BlockingCollection<string> inputQueue;
        private BlockingCollection<string> outputQueue;
        private BlockingCollection<Command> commandQueue;

        private readonly int messageHandlerCount;
        private readonly int commandHandlerCount;

        /// <summary>
        /// constructor
        /// creates the TwitchBot without connecting
        /// </summary>
        /// <param name="userName">userName of the twitchbot account</param>
        /// <param name="token">token of the twitchbot account</param>
        /// <param name="messageHandlerCount">number of layer 2 threads (default 10)</param>
        /// <param name="commandHandlerCount">number of layer 3 threads (default 10)</param>
        public BasicTwitchBot(string userName, string token, int messageHandlerCount = 10, int commandHandlerCount = 10)
        {
            this.userName = userName;
            this.token = token;

            this.messageHandlerCount = messageHandlerCount;
            this.commandHandlerCount = commandHandlerCount;
        }

        /// <summary>
        /// sets up the TcpClient and connects with twitch
        /// starts the threads for all 4 layers
        /// </summary>
        /// <returns>true if everything went well, false if an exception was thrown</returns>
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

                outputStream.WriteLine("PASS " + token);
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

        /// <summary>
        /// creates and starts the threads of all 4 layers
        /// </summary>
        public void startReadWriting()
        {
            //layer 1 thread
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

            //layer 2 threads
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

            //layer 3 threads
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

            //layer 4 thread
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
        }

        /// <summary>
        /// converts the message into a ChatMessage and sends it to HandleMessage
        /// </summary>
        /// <param name="message">raw message from twitch chat</param>
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

        /// <summary>
        /// handles the ChatMessage and returns a Command
        /// meant to be overridden
        /// </summary>
        /// <param name="cm">ChatMessage</param>
        /// <returns>a Command to be executed or null if nothing is to be executed</returns>
        public virtual Command HandleMessage(ChatMessage cm)
        {
            return new ResponseCommand(this, $"Hi {cm.Writer}, you wrote: {cm.Message}");
        }

        /// <summary>
        /// send message to twitch to join a channel
        /// does NOT check if channel name is valid
        /// </summary>
        /// <param name="channelName">channel name</param>
        /// <returns>true if channel is not empty string</returns>
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

        /// <summary>
        /// sends message to twitch to leave the current channel
        /// </summary>
        public void LeaveChannel()
        {
            outputQueue.Add("PART #" + channelName);
            this.channelName = "";
        }

        /// <summary>
        /// converts and adds a message to be added to the outputQueue
        /// </summary>
        /// <param name="message">raw message to be sent</param>
        public void SendChatMessage(string message)
        {
            outputQueue.Add(message.convertForTwitch(userName, channelName));
        }
    }
}
