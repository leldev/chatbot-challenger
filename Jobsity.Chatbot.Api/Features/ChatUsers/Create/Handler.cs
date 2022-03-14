using AutoMapper;
using Jobsity.Chatbot.Api.Common.Services;
using Jobsity.Chatbot.Api.Features.ChatUsers.Models;
using Jobsity.Chatbot.Api.Persistence.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.Create
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
            var newChatUser = this.mapper.Map<Domain.ChatUser>(request);
            newChatUser.Password = SecurityService.HashText(request.Password);

            var item = await this.db.CreateItemAsync(newChatUser).ConfigureAwait(false);
            var newUserModel = this.mapper.Map<ChatUserModel>(item);

            return new CreatedAtRouteResult("ChatUser.GetById", new { UserId = newUserModel.Id }, newUserModel);
        }
    }
}
