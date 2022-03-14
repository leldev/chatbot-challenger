using AutoMapper;
using Jobsity.Chatbot.Api.Features.Rooms.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.Rooms.GetAll
{
    public class Handler : IRequestHandler<QueryRequest, IActionResult>
    {
        private readonly IRepository db;
        private readonly IMapper mapper;

        public Handler(IRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(QueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await this.db.GetItemsAsync<Domain.Room>().ConfigureAwait(false);
                var roomsModel = this.mapper.Map<List<RoomModel>>(rooms);

                return new OkObjectResult(roomsModel);
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
