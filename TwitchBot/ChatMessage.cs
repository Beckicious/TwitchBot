namespace TwitchBot
{
    /// <summary>
    /// Object that represents a message in the chat with its user.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// writer of the message
        /// </summary>
        public string Writer { get; set; }

        /// <summary>
        /// message that has been written in chat
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="message">message</param>
        public ChatMessage(string writer, string message)
        {
            this.Writer = writer;
            this.Message = message;
        }
    } 
}
