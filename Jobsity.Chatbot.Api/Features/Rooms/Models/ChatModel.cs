using System;

namespace Jobsity.Chatbot.Api.Features.Rooms.Models
{
    public class ChatModel
    {
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
