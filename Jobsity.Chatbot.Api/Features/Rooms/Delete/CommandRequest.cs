using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.Rooms.Delete
{
    public class CommandRequest : IRequest<IActionResult>
    {
        public string RoomId { get; set; }
    }
}