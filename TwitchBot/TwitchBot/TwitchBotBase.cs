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
using System.Timers;
using TwitchLib.Api;

namespace TwitchBot
{
    public class TwitchBotBase
    {
        private TwitchClient client;
        private string Botname { get; set; }
        private string Token { get; set; }
        private Channel JoinedChannel { get; set; } = null;

        private TwitchBotDataContext db;

        private Timer pointTimer;

        public event EventHandler<string> OnStatusChange;
        public event EventHandler<ChatMessage> OnChatMessageReceived;

        public TwitchBotBase()
        {
            UpdateStatus("Initializing");

            db = new TwitchBotDataContext();

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

            client.LeaveChannel(this.JoinedChannel.User.Name);
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
            this.JoinedChannel = GetOrAddChannel(e.Channel.ToLowerInvariant());

            UpdateStatus($"Joined Channel {this.JoinedChannel.User.Name}");

            SetPointTimer();
            // client.SendMessage(this.JoinedChannel.User.Name, DateTime.Now.ToString()); //TODO: remove
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

            StopTimer();
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            OnChatMessageReceived?.Invoke(this, e.ChatMessage);

            var user = GetOrAddUser(e.ChatMessage.Username);
            AddMessage(user.UserID, this.JoinedChannel.ChannelID, e.ChatMessage.Message);
        }

        private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            Console.WriteLine($"Received whisper from {e.WhisperMessage.Username}: {e.WhisperMessage.Message}");
        }

        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            Console.WriteLine($"User joined channel: {e.Username}");
            GetOrAddUser(e.Username);
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

        private void SetPointTimer()
        {
            pointTimer = new Timer(1000);
            pointTimer.Elapsed += AddPointsToViewers;
            pointTimer.AutoReset = true;
            pointTimer.Enabled = true;
        }

        public void AddPointsToViewers(object sender, ElapsedEventArgs e)
        {
            Chatters chatters = Chatters.GetChatters(this.JoinedChannel.User.Name);

            foreach (string username in chatters.AllViewers)
            {
                var point = GetOrAddPoint(username, this.JoinedChannel.User.Name);
                point.Amount += 1;
            }
            db.SubmitChanges();
        }

        private void StopTimer()
        {
            pointTimer.Enabled = false;
        }


        private User GetOrAddUser(string name)
        {
            var user = GetUserByName(name);
            if (user == null)
            {
                user = AddUser(name);
            }
            return user;
        }

        private User AddUser(string name)
        {
            var u = new User()
            {
                Name = name
            };
            db.Users.InsertOnSubmit(u);
            db.SubmitChanges();
            return u;
        }

        private User GetUserByName(string name)
        {
            return db.Users.SingleOrDefault(u => u.Name == name.ToLowerInvariant());
        }

        private Channel GetOrAddChannel(string name)
        {
            var user = GetOrAddUser(name);
            var ch = GetChannelToUser(user.UserID);
            if (ch == null)
            {
                ch = AddChannel(user.UserID);
            }
            return ch;
        }

        private Channel AddChannel(int userID)
        {
            var ch = new Channel()
            {
                UserID = userID
            };
            db.Channels.InsertOnSubmit(ch);
            db.SubmitChanges();
            return ch;
        }

        private Channel GetChannelToUser(int userID)
        {
            return db.Channels.SingleOrDefault(ch => ch.UserID == userID);
        }

        private Message AddMessage(int userID, int channelID, string text)
        {
            var msg = new Message()
            {
                UserID = userID,
                ChannelID = channelID,
                Date = DateTime.Now,
                Text = text
            };
            db.Messages.InsertOnSubmit(msg);
            db.SubmitChanges();
            return msg;
        }

        private Point GetOrAddPoint(string username, string channelname)
        {
            var point = GetPoint(username, channelname);
            if (point == null)
            {
                var user = GetOrAddUser(username);
                var channel = GetOrAddChannel(channelname);
                point = AddPoint(user.UserID, channel.ChannelID);
            }
            return point;
        }

        private Point GetPoint(string username, string channelname)
        {
            return db.Points.SingleOrDefault(p => p.User.Name == username.ToLowerInvariant() && p.Channel.User.Name == channelname.ToLowerInvariant());
        }

        private Point AddPoint(int userID, int channelID)
        {
            var point = new Point()
            {
                UserID = userID,
                ChannelID = channelID,
                Amount = 0
            };
            db.Points.InsertOnSubmit(point);
            db.SubmitChanges();
            return point;
        }
    }
}
