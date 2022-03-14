namespace Jobsity.Chatbot.Api.ServiceBus.Configuration
{
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}