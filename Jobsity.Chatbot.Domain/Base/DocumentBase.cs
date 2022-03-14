using Newtonsoft.Json;

namespace Jobsity.Chatbot.Domain.Base
{
    public abstract class DocumentBase : EntityBase
    {
        [JsonProperty(PropertyName = "_attachments")]
        public string Attachments { get; protected internal set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; protected internal set; }

        [JsonProperty(PropertyName = "_rid")]
        public string ResourceId { get; protected internal set; }

        [JsonProperty(PropertyName = "_self")]
        public string Self { get; protected internal set; }

        [JsonProperty(PropertyName = "_ts")]
        public int TimeStamp { get; protected internal set; }
    }
}