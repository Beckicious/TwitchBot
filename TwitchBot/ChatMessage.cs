using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public class ChatMessage
    {
        public string Writer { get; set; }
        public string Message { get; set; }
        public string FirstWord { get; set; }
        public string Channel { get; set; }

        public ChatMessage(string writer, string message, string channel)
        {
            this.Writer = writer;
            this.Message = message;
            this.Channel = channel;

            this.FirstWord = message.Split(' ')[0];
        }
    } 
}
