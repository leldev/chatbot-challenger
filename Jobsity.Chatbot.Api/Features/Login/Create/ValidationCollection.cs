using FluentValidation;

namespace Jobsity.Chatbot.Api.Features.Login.Create
{
    public class ValidationCollection : AbstractValidator<CommandRequest>
    {
        public ValidationCollection()
        {
            this.CascadeMode = CascadeMode.Stop;

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(LoginErrors.InvalidChatUserName.Message)
                .MaximumLength(Domain.ChatUser.MaxNameLength)
                .WithMessage(LoginErrors.ChatUserNameMaxLength.Message);

            this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(LoginErrors.InvalidChatUserPassword.Message)
                .MaximumLength(Domain.ChatUser.MaxPasswordLength)
                .WithMessage(LoginErrors.ChatUserPasswordMaxLength.Message);
        }
    }
}
