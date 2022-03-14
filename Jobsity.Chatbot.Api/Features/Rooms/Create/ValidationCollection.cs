using FluentValidation;

namespace Jobsity.Chatbot.Api.Features.Rooms.Create
{
    public class ValidationCollection : AbstractValidator<CommandRequest>
    {
        public ValidationCollection()
        {
            this.CascadeMode = CascadeMode.Stop;

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(RoomErrors.InvalidRoomName.Message)
                .MaximumLength(Domain.ChatUser.MaxNameLength)
                .WithMessage(RoomErrors.RoomNameMaxLength.Message);
        }
    }
}