using AutoMapper;
using Jobsity.Chatbot.Api.Features.ChatUsers.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.GetById
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
                var chatUser = await this.db.GetItemAsync<Domain.ChatUser>(request.UserId).ConfigureAwait(false);
                var userModel = this.mapper.Map<ChatUserModel>(chatUser);

                return new OkObjectResult(userModel);
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
