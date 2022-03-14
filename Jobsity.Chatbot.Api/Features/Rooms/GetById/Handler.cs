using AutoMapper;
using Jobsity.Chatbot.Api.Features.Rooms.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.Rooms.GetById
{
    public class Handler : IRequestHandler<QueryRequest, IActionResult>
    {
        private readonly IRepository db;
        private readonly IMapper mapper;

        public Handler(IRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(QueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await this.db.GetItemAsync<Domain.Room>(request.RoomId).ConfigureAwait(false);
                var roomModel = this.mapper.Map<ChatRoomModel>(room);

                // Take last n chats
                roomModel.Chats = roomModel.Chats.OrderBy((x) => x.CreatedDate).Skip(Math.Max(0, roomModel.Chats.Count() - Domain.Chat.MaxChatsToShow)).ToList();

                return new OkObjectResult(roomModel);
            }
            catch (DocumentClientException ex)
            {
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
    }
}
