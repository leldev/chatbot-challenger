using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.GetById
{
    public class QueryRequest : IRequest<IActionResult>
    {
        public string UserId { get; set; }
    }
}