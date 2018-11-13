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
using System.Drawing;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for ChatLine.xaml
    /// </summary>
    public partial class ChatLine : UserControl
    {
        ChatMessage cm;
        private const float colorCoef = 15f;

        public ChatLine(ChatMessage msg)
        {
            InitializeComponent();
            cm = msg;
            txtUser.Foreground = new SolidColorBrush(GetLighterColor(cm.Color));
            txtMessage.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255,255,255));
            txtUser.Text = cm.Username;
            txtMessage.Text = cm.Message;
        }

        private System.Windows.Media.Color GetLighterColor(System.Drawing.Color color)
        {
            var oldColor = System.Windows.Media.Color.FromArgb(byte.MaxValue, color.R, color.G, color.B);
            var raisedColor = System.Windows.Media.Color.Add(oldColor, System.Windows.Media.Color.FromArgb(byte.MaxValue, 15, 15, 15));
            return System.Windows.Media.Color.Multiply(raisedColor, colorCoef);
        }

        private void ctrChatLine_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = this.ActualWidth - txtUser.ActualWidth;
            if (size < 10)
            {
                size = 10;
            }
            txtMessage.Width = size;
        }
    }
}
