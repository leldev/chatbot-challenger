using System.Collections.Generic;

namespace Jobsity.Chatbot.Api.Features.Rooms.Models
{
    public class ChatRoomModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ChatModel> Chats { get; set; }
    }
}