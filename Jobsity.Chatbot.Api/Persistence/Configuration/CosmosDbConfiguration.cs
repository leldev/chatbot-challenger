namespace Jobsity.Chatbot.Api.Persistence.Configuration
{
    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string AuthKey { get; set; }
        public string Collection { get; set; }
        public string Database { get; set; }
        public string Endpoint { get; set; }
    }
}