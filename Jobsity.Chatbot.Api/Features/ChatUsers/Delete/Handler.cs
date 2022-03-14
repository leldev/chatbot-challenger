using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.Delete
{
    public class Handler : IRequestHandler<CommandRequest, IActionResult>
    {
        private readonly IRepository db;

        public Handler(IRepository db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Handle(CommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await this.db.DeleteItemAsync<Domain.ChatUser>(request.UserId).ConfigureAwait(false);

                return new NoContentResult();
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return new NoContentResult();
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
