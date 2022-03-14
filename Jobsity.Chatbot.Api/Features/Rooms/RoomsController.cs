using Jobsity.Chatbot.Api.Features.Rooms.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.Rooms
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly IMediator mediator;

        public RoomsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RoomModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAsync([FromBody] Create.CommandRequest command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }

        [HttpGet("{roomId}", Name = "Room.GetById")]
        [ProducesResponseType(typeof(ChatRoomModel), ((int)HttpStatusCode.OK))]
        [ProducesResponseType(((int)HttpStatusCode.NotFound))]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetById.QueryRequest query)
        {
            return await this.mediator.Send(query).ConfigureAwait(false);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RoomModel>), ((int)HttpStatusCode.OK))]
        public async Task<IActionResult> GetAllAsync([FromRoute] GetAll.QueryRequest query)
        {
            return await this.mediator.Send(query).ConfigureAwait(false);
        }

        [HttpDelete("{roomId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Delete.CommandRequest command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }

        [HttpPost("{roomId}/Chats")]
        [ProducesResponseType(typeof(List<ChatRoomModel>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateChatAsync([FromRoute] Chats.Create.CommandRequest command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }
    }
}