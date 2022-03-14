using AutoMapper;
using Jobsity.Chatbot.Api.Features.ChatUsers.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Jobsity.Chatbot.Api.Features.Login.Model;
using Jobsity.Chatbot.Api.Common.Services;

namespace Jobsity.Chatbot.Api.Features.Login.Create
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
            var hashPassword = SecurityService.HashText(request.Password);
            var result = await this.db.GetDynamicItemsAsync<Domain.ChatUser>(x => x.Name == request.Name && x.Password == hashPassword).ConfigureAwait(false);

            if (result.Any())
            {
                var login = result.FirstOrDefault();
                // TODO: improve dynamic convert
                var jsonData = JsonConvert.SerializeObject(login);
                var loginModel = JsonConvert.DeserializeObject<LoginModel>(jsonData);

                return new OkObjectResult(loginModel);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}