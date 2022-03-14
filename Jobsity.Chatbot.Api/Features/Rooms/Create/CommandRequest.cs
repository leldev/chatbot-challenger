using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.Rooms.Create
{
    public class CommandRequest : IRequest<IActionResult>
    {
        public string Name { get; set; }
    }
}