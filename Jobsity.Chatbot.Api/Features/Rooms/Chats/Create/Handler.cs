using AutoMapper;
using Jobsity.Chatbot.Api.Common.Models;
using Jobsity.Chatbot.Api.Features.Rooms.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using Jobsity.Chatbot.Api.ServiceBus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.Rooms.Chats.Create
{
    public class Handler : IRequestHandler<CommandRequest, IActionResult>
    {
        private const string STOCKKEY = "/stock=";
        private readonly IRepository db;
        private readonly IMapper mapper;
        private readonly IStockBotService stockeBotService;

        public Handler(IRepository db, IMapper mapper, IStockBotService stockeBotService)
        {
            this.db = db;
            this.mapper = mapper;
            this.stockeBotService = stockeBotService;
        }

        public async Task<IActionResult> Handle(CommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var commitChanges = true;
                var stockModel = new StockModel();

                // Check for Bot key.
                if (this.ContainChatBotKey(request.Body.Message))
                {
                    // Bot action wont require to save changes in DB.
                    commitChanges = false;

                    stockModel = await this.GetStockValue(request.Body.Message).ConfigureAwait(false);
                }

                // Get room entity.
                var room = await this.db.GetItemAsync<Domain.Room>(request.RoomId).ConfigureAwait(false);

                // Create chat entity.
                Domain.Chat newChat;

                if (commitChanges)
                {
                    newChat = this.mapper.Map<Domain.Chat>(request.Body);
                    newChat.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    // Set Chat Bot name and id.
                    newChat = new Domain.Chat()
                    {
                        UserName = "Chat Bot",
                        UserId = "chatbot-stock",
                        Message = this.GetMessageValueFromStock(stockModel),
                        CreatedDate = DateTime.UtcNow
                    };
                }

                // Add new chat entity in room collection.
                room.Chats.Add(newChat);

                if (commitChanges)
                {
                    // Commit changes.
                    room = await this.db.UpdateItemAsync(room).ConfigureAwait(false);
                }

                var roomModel = this.mapper.Map<ChatRoomModel>(room);

                // Take last n chats
                roomModel.Chats = roomModel.Chats.OrderBy((x) => x.CreatedDate).Skip(Math.Max(0, roomModel.Chats.Count() - Domain.Chat.MaxChatsToShow)).ToList();

                return new CreatedAtRouteResult("Room.GetById", new { request.RoomId }, roomModel);
            }
            catch (DocumentClientException ex)
            {
                // Room not found
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw ex;
                }
            }
        }

        private string GetMessageValueFromStock(StockModel stockModel)
        {
            return stockModel.Close.Equals("N/D") ? 
                $"{stockModel.Symbol} is unavailable" :
                $"{stockModel.Symbol} quote is $${stockModel.Close} per share";
        }

        private async Task<StockModel> GetStockValue(string message)
        {
            var code = message.ToLower().Replace(STOCKKEY, string.Empty);

            // Method will queue message in Azure Service Bus, then will trigger Receiver Service to read queue message
            // With message value will request stooq.com API.
            return await this.stockeBotService.GetStockValueAsync(code).ConfigureAwait(false);
        }

        private bool ContainChatBotKey(string message)
        {
            return message.ToLower().StartsWith(STOCKKEY);
        }
    }
}
