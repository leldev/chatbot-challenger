using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.Delete
{
    public class CommandRequest : IRequest<IActionResult>
    {
        public string UserId { get; set; }
    }
}