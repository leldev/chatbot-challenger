using Newtonsoft.Json;

namespace Jobsity.Chatbot.Domain.Base
{
    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected internal set; }
    }
}