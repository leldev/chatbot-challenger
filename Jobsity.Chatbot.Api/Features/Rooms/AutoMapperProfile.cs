using AutoMapper;
using Jobsity.Chatbot.Api.Features.Rooms.Models;

namespace Jobsity.Chatbot.Api.Features.Rooms
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Domain.Room, RoomModel>().ReverseMap();
            this.CreateMap<Domain.Room, ChatRoomModel>().ReverseMap();
            this.CreateMap<Domain.Chat, ChatModel>().ReverseMap();
            this.CreateMap<Domain.Room, Create.CommandRequest>().ReverseMap();
            this.CreateMap<Domain.Chat, ChatWriteModel>().ReverseMap();
        }
    }
}