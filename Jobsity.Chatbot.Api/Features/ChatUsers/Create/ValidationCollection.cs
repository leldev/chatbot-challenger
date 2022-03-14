using FluentValidation;

namespace Jobsity.Chatbot.Api.Features.ChatUsers.Create
{
    public class ValidationCollection : AbstractValidator<CommandRequest>
    {
        public ValidationCollection()
        {
            this.CascadeMode = CascadeMode.Stop;

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ChatUserErrors.InvalidChatUserName.Message)
                .MaximumLength(Domain.ChatUser.MaxNameLength)
                .WithMessage(ChatUserErrors.ChatUserNameMaxLength.Message);

            this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ChatUserErrors.InvalidChatUserPassword.Message)
                .MaximumLength(Domain.ChatUser.MaxPasswordLength)
                .WithMessage(ChatUserErrors.ChatUserPasswordMaxLength.Message);
        }
    }
}
