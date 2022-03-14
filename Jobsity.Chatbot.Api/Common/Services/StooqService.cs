using Jobsity.Chatbot.Api.Common.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Common.Services
{
    public class StooqService
    {
        private const string STOOQURL = @"https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

        public async Task<StockModel> GetStockValueAsync(string stock)
        {
            var result = new StockModel();

            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(string.Format(STOOQURL, stock)).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                this.ParseStockContent(content, result);
            }

            return result;
        }

        private void ParseStockContent(string content, StockModel stockModel)
        {
            // Takes lines
            var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 2)
            {
                // Has header and one line of value.
                var header = lines[0].Split(',');
                var values = lines[1].Split(',');

                // Create stock model with values

                for (int i = 0; i < header.Length; i++)
                {
                    switch (header[i])
                    {
                        case nameof(stockModel.Symbol):
                            stockModel.Symbol = values[i];
                            break;
                        case nameof(stockModel.Open):
                            stockModel.Open = values[i];
                            break;
                        case nameof(stockModel.Low):
                            stockModel.Low = values[i];
                            break;
                        case nameof(stockModel.Close):
                            stockModel.Close = values[i];
                            break;
                        case nameof(stockModel.Date):
                            stockModel.Date = values[i];
                            break;
                        case nameof(stockModel.High):
                            stockModel.High = values[i];
                            break;
                        case nameof(stockModel.Time):
                            stockModel.Time = values[i];
                            break;
                        case nameof(stockModel.Volume):
                            stockModel.Volume = values[i];
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
