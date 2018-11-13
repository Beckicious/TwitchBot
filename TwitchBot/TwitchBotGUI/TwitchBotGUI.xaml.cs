using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchBot;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for TwitchBotGUI.xaml
    /// </summary>
    public partial class TwitchBotGUI : Window
    {

        TwitchBotBase tb;

        public TwitchBotGUI()
        {
            InitializeComponent();

            tb = new TwitchBotBase();

            tb.AddOnJoinedChannel(OnJoinedChannel);
            tb.AddOnFailedJoinChannel(OnFailedJoinChannel);
            tb.AddOnLeftChannel(OnLeftChannel);
            tb.OnStatusChange += UpdateStatus;
            tb.OnChatMessageReceived += OnChatMessageReceived;
        }

        private void OnChatMessageReceived(object sender, ChatMessage e)
        {
            this.Dispatcher.Invoke(() =>
            {
                spChatLines.Children.Add(new ChatLine(e));
                svChatLines.ScrollToBottom();
            });
        }

        private void UpdateStatus(object sender, string e)
        {
            this.Dispatcher.Invoke(() =>
            {
                txtStatus.Content = e;
            });
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                butConnect.IsEnabled = false;
                txtChannel.IsEnabled = false;
                butDisconnect.IsEnabled = true;
            });
        }

        private void OnFailedJoinChannel(object sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                butConnect.IsEnabled = true;
                txtChannel.IsEnabled = true;
                butDisconnect.IsEnabled = false;
            });
        }

        private void OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                butConnect.IsEnabled = true;
                txtChannel.IsEnabled = true;
                butDisconnect.IsEnabled = false;
            });
        }

        private void butConnect_Click(object sender, RoutedEventArgs e)
        {
            butConnect.IsEnabled = false;
            txtChannel.IsEnabled = false;
            butDisconnect.IsEnabled = false;
            tb.JoinChannel(txtChannel.Text.Trim());         
        }

        private void butDisconnect_Click(object sender, RoutedEventArgs e)
        {
            butConnect.IsEnabled = false;
            txtChannel.IsEnabled = false;
            butDisconnect.IsEnabled = false;
            tb.LeaveChannel();
        }
    }
}
