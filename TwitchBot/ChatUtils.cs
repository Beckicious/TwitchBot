using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public static class ChatUtils
    {
        static public ChatMessage ParseMessageToChatMessage(this string rawMessage)
        {

            rawMessage = rawMessage.Remove(0, 1);

            var writerStart = rawMessage.IndexOf('!') + 1;
            var writerLength = rawMessage.IndexOf('@') - writerStart;

            var writer = rawMessage.Substring(writerStart, writerLength);
            var message = rawMessage.Substring(rawMessage.IndexOf(':') + 1).Trim();

            var channel = rawMessage.Substring(rawMessage.IndexOf('#') + 1).Split(' ')[0];

            return new ChatMessage(writer, message, channel);
        }

        public static bool IsChatMessage(this string msg)
        {
            return msg.Contains("PRIVMSG");
        }

        public static bool IsBotMessage(this string msg)
        {
            return msg.First() == '!';
        }

        public static string convertForTwitch(this string message, string userName, string channelName)
        {
            return ":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + channelName + " :" + message;
        }
    }
}
