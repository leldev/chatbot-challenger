using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jobsity.Chatbot.Api.Common.Models;
using Jobsity.Chatbot.Api.Common.Services;
using Jobsity.Chatbot.Api.ServiceBus.Configuration;
using Microsoft.Azure.ServiceBus;

namespace Jobsity.Chatbot.Api.ServiceBus
{
    public class StockBotService : IStockBotService, IDisposable
    {
        private IQueueClient queueClient;
        private IServiceBusConfiguration config;
        private TaskCompletionSource<StockModel> taskCompletionSource;

        public StockBotService(IServiceBusConfiguration config)
        {
            this.config = config;
            this.queueClient = new QueueClient(this.config.ConnectionString, this.config.QueueName);

            ConfirgueReceiverService();
        }

        public async Task<StockModel> GetStockValueAsync(string stockCode)
        {
            this.taskCompletionSource = new TaskCompletionSource<StockModel>();

            var message = new Message(Encoding.UTF8.GetBytes(stockCode));

            // Send the message to the queue.
            await this.queueClient.SendAsync(message).ConfigureAwait(false);

            // Return a Task and wailt for Receiver to read queue.
            return await this.taskCompletionSource.Task;

        }

        public void ConfirgueReceiverService()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandlerAsync)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            this.queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var queue = Encoding.UTF8.GetString(message.Body);

            // Get value from Stooq
            var stooqService = new StooqService();
            var result = await stooqService.GetStockValueAsync(queue).ConfigureAwait(false);

            await this.queueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);

            // Return message value to source
            this.taskCompletionSource.SetResult(result);
        }

        private Task ExceptionReceivedHandlerAsync(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (!this.queueClient.IsClosedOrClosing)
            {
                this.queueClient.CloseAsync();
            }
        }
    }
}
