using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.Create
{
    public class CommandRequest : IRequest<IActionResult>
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}