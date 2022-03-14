using Jobsity.Chatbot.Api.Common.Models;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.ServiceBus
{
    public interface IStockBotService
    {
        void ConfirgueReceiverService();
        Task<StockModel> GetStockValueAsync(string stockCode);
    }
}