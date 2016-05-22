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

        public ChatMessage(string writer, string message)
        {
            this.Writer = writer;
            this.Message = message;
        }
    } 
}
