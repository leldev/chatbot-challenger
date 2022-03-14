using Jobsity.Chatbot.Api.Features.Rooms.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.Rooms.Chats.Create
{
    public class CommandRequest : IRequest<IActionResult>
    {
        [FromRoute]
        public string RoomId { get; set; }

        [FromBody]
        public ChatWriteModel Body { get; set; }
    }
}