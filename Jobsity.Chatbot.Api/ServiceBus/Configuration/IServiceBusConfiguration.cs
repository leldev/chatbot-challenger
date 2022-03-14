namespace Jobsity.Chatbot.Api.ServiceBus.Configuration
{
    public interface IServiceBusConfiguration
    {
        string ConnectionString { get; set; }
        string QueueName { get; set; }
    }
}