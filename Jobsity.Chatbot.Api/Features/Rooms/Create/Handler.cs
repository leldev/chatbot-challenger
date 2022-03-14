using AutoMapper;
using Jobsity.Chatbot.Api.Features.Rooms.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.Rooms.Create
{
    public class Handler : IRequestHandler<CommandRequest, IActionResult>
    {
        private readonly IRepository db;
        private readonly IMapper mapper;

        public Handler(IRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(CommandRequest request, CancellationToken cancellationToken)
        {
            var newRoom = this.mapper.Map<Domain.Room>(request);
            var item = await this.db.CreateItemAsync(newRoom).ConfigureAwait(false);
            var roomModel = this.mapper.Map<RoomModel>(item);

            return new CreatedAtRouteResult("Room.GetById", new { RoomId = item.Id }, roomModel);
        }
    }
}
