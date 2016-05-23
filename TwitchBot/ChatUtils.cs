namespace TwitchBot
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class ChatUtils
    {
        /// <summary>
        /// parses a message from twitch to a ChatMessage
        /// </summary>
        /// <param name="rawMessage">raw message as recieved from twitch</param>
        /// <returns>ChatMessage</returns>
        static public ChatMessage ParseMessageToChatMessage(this string rawMessage)
        {

            rawMessage = rawMessage.Remove(0, 1);

            var writerStart = rawMessage.IndexOf('!') + 1;
            var writerLength = rawMessage.IndexOf('@') - writerStart;

            var writer = rawMessage.Substring(writerStart, writerLength);
            var message = rawMessage.Substring(rawMessage.IndexOf(':') + 1).Trim();

            return new ChatMessage(writer, message);
        }

        /// <summary>
        /// checks if a recieved message is a chat message
        /// </summary>
        /// <param name="msg">the message to check</param>
        /// <returns>true if it's a chat message, false otherwise</returns>
        public static bool IsChatMessage(this string msg)
        {
            return msg.Contains("PRIVMSG");
        }

        /// <summary>
        /// converts a message into twitch format
        /// </summary>
        /// <param name="message">the message to send</param>
        /// <param name="userName">the userName of the sender</param>
        /// <param name="channelName">the channelName to send the message</param>
        /// <returns>converted string ready to send</returns>
        public static string convertForTwitch(this string message, string userName, string channelName)
        {
            return ":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + channelName + " :" + message;
        }
    }
}
