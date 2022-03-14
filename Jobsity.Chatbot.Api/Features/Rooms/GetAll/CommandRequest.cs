using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chatbot.Api.Features.Rooms.GetAll
{
    public class QueryRequest : IRequest<IActionResult>
    {
    }
}