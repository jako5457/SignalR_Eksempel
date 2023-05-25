namespace BlazorApp.Models
{
    public class ChatMessage
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public bool IsYou { get; set; } = false;

        public bool IsPrivate { get; set; } = false;
    }
}
