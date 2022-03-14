using Jobsity.Chatbot.Domain.Base;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jobsity.Chatbot.Domain
{
    public class Room : DocumentBase
    {
        public Room()
        {
            this.Chats = new List<Chat>();
        }

        public static int MaxNameLength => 40;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "chats")]
        public IList<Chat> Chats { get; set; }
    }
}