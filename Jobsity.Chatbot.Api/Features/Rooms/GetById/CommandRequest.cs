using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.Rooms.GetById
{
    public class QueryRequest : IRequest<IActionResult>
    {
        public string RoomId { get; set; }
    }
}