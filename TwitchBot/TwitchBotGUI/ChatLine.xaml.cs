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
using TwitchLib.Client.Models;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for ChatLine.xaml
    /// </summary>
    public partial class ChatLine : UserControl
    {
        ChatMessage cm;

        public ChatLine(ChatMessage msg)
        {
            InitializeComponent();
            cm = msg;
            txtUser.Content = cm.Username;
            txtMessage.Content = cm.Message;
        }
    }
}
