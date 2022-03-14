using Jobsity.Chatbot.Domain.Base;
using Newtonsoft.Json;
using System;

namespace Jobsity.Chatbot.Domain
{
    public class Chat : DocumentBase
    {
        public Chat()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public static int MaxMessageLength => 200;
        public static int MaxChatsToShow => 50;

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "room")]
        public virtual Room Room { get; set; }
    }
}