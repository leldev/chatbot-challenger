namespace Jobsity.Chatbot.Api.Persistence.Configuration
{
    public interface ICosmosDbConfiguration
    {
        string AuthKey { get; set; }
        string Collection { get; set; }
        string Database { get; set; }
        string Endpoint { get; set; }
    }
}