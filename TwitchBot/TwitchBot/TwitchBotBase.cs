using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using System.Drawing;

namespace TwitchBot
{
    public class TwitchBotBase
    {
        private TwitchClient client;
        private string Botname { get; set; }
        private string Token { get; set; }
        private string JoinedChannel { get; set; } = null;

        public event EventHandler<string> OnStatusChange;
        public event EventHandler<ChatMessage> OnChatMessageReceived;

        public TwitchBotBase()
        {
            UpdateStatus("Initializing");

            //read credentials file
            XmlDocument doc = new XmlDocument();
            doc.Load("credentials.xml");
            this.Botname = doc.DocumentElement.SelectSingleNode("/login/username").InnerText;
            this.Token = doc.DocumentElement.SelectSingleNode("/login/token").InnerText;
            this.JoinedChannel = null;

            ConnectionCredentials credentials = new ConnectionCredentials(this.Botname, this.Token);

            client = new TwitchClient();
            client.Initialize(credentials);

            //Connection
            client.OnConnected += OnConnected;
            client.OnConnectionError += OnConnectionError;
            client.OnDisconnected += OnDisconnected;

            //Channel
            client.OnJoinedChannel += OnJoinedChannel;
            client.OnFailureToReceiveJoinConfirmation += OnFailureToReceiveJoinConfirmation;
            client.OnLeftChannel += OnLeftChannel;

            //Chat
            client.OnMessageReceived += OnMessageReceived;
            client.OnWhisperReceived += OnWhisperReceived;


            //Users
            client.OnUserJoined += OnUserJoined;
            client.OnUserLeft += OnUserLeft;
            //client.OnUserTimedout += OnUserTimedout;
            //client.OnUserBanned += OnUserBanned;

            ////Mods
            //client.OnModeratorJoined += OnModeratorJoined;
            //client.OnModeratorLeft += OnModeratorLeft;

            ////Events
            //client.OnNewSubscriber += OnNewSubscriber;
            
            client.Connect();
            UpdateStatus("Connecting");
        }

        private void UpdateStatus(string status)
        {
            if (OnStatusChange != null)
            {
                OnStatusChange(this, status);
            }
        }

        public void JoinChannel(string channelName)
        {
            UpdateStatus($"Joining channel {channelName}");

            if (this.JoinedChannel != null) {
                LeaveChannel();
            }
            client.JoinChannel(channelName);
        }

        public void LeaveChannel()
        {
            UpdateStatus("Leaving channel");

            client.LeaveChannel(this.JoinedChannel);
        }

        #region Events

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            UpdateStatus("Connected");
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            UpdateStatus("Error while connecting");
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            UpdateStatus("Disconnected");
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            this.JoinedChannel = e.Channel;
            UpdateStatus($"Joined Channel {this.JoinedChannel}");
            client.SendMessage(this.JoinedChannel, DateTime.Now.ToString()); //TODO: remove
        }

        private void OnFailureToReceiveJoinConfirmation(object sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            this.JoinedChannel = null;
            UpdateStatus("Could not join channel");
        }

        private void OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            this.JoinedChannel = null;
            UpdateStatus($"Left channel {e.Channel}");
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Console.WriteLine($"Received message: {e.ChatMessage.Message}");
            OnChatMessageReceived?.Invoke(this, e.ChatMessage);
        }

        private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            Console.WriteLine($"Received whisper: {e.WhisperMessage.Message}");
        }

        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            Console.WriteLine($"User joined channel: {e.Username}");
        }

        private void OnUserLeft(object sender, OnUserLeftArgs e)
        {
            Console.WriteLine($"User left channel: {e.Username}");
        }

        public void AddOnJoinedChannel(EventHandler<OnJoinedChannelArgs> f)
        {
            client.OnJoinedChannel += f;
        }

        public void AddOnFailedJoinChannel(EventHandler<OnFailureToReceiveJoinConfirmationArgs> f)
        {
            client.OnFailureToReceiveJoinConfirmation += f;
        }

        public void AddOnLeftChannel(EventHandler<OnLeftChannelArgs> f)
        {
            client.OnLeftChannel += f;
        }

        #endregion Events
    }
}
