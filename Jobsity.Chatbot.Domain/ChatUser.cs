using Jobsity.Chatbot.Domain.Base;
using Newtonsoft.Json;

namespace Jobsity.Chatbot.Domain
{
    public class ChatUser : DocumentBase
    {
        public static int MaxNameLength => 40;
        public static int MaxPasswordLength => 50;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}