using AutoMapper;
using Jobsity.Chatbot.Api.Features.ChatUsers.Models;

namespace Jobsity.Chatbot.Api.Features.ChatUsers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Domain.ChatUser, ChatUserModel>().ReverseMap();
            this.CreateMap<Domain.ChatUser, Create.CommandRequest>().ReverseMap();
        }
    }
}