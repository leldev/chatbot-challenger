using Jobsity.Chatbot.Api.Features.ChatUsers.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.ChatUsers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatUsersController : Controller
    {
        private readonly IMediator mediator;

        public ChatUsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ChatUserModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAsync([FromBody] Create.CommandRequest command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }

        [HttpGet("{userId}", Name = "ChatUser.GetById")]
        [ProducesResponseType(typeof(ChatUserModel), ((int)HttpStatusCode.OK))]
        [ProducesResponseType(((int)HttpStatusCode.NotFound))]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetById.QueryRequest query)
        {
            return await this.mediator.Send(query).ConfigureAwait(false);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Delete.CommandRequest command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }
    }
}