using FluentValidation;

namespace Jobsity.Chatbot.Api.Features.Rooms.Chats.Create
{
    public class ValidationCollection : AbstractValidator<CommandRequest>
    {
        public ValidationCollection()
        {
            this.CascadeMode = CascadeMode.Stop;

            this.RuleFor(x => x.Body.Message)
                .NotEmpty()
                .WithMessage(RoomErrors.InvalidChatName.Message)
                .MaximumLength(Domain.Chat.MaxMessageLength)
                .WithMessage(RoomErrors.ChatNameMaxLength.Message);

            this.RuleFor(x => x.Body.UserId)
                .NotEmpty()
                .WithMessage(RoomErrors.InvalidChatName.Message);

            this.RuleFor(x => x.Body.UserName)
                .NotEmpty()
                .WithMessage(RoomErrors.InvalidChatName.Message)
                .MaximumLength(Domain.ChatUser.MaxNameLength)
                .WithMessage(RoomErrors.ChatNameMaxLength.Message);
        }
    }
}