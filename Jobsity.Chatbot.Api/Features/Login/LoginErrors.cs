using Jobsity.Chatbot.Api.Common.ApiErrors;

namespace Jobsity.Chatbot.Api.Features.Login
{
    public class LoginErrors
    {
        public static ApiError CreateLogin => new ApiError(MajorErrorCodes.ChatUser, 1, "Create login call failed");
        public static ApiError InvalidChatUserName => new ApiError(MajorErrorCodes.ChatUser, 2, "Invalid or empty chat user name");
        public static ApiError ChatUserNameMaxLength => new ApiError(MajorErrorCodes.ChatUser, 3, $"Chat user name length exceeds more than {Domain.ChatUser.MaxNameLength} characters");
        public static ApiError InvalidChatUserPassword => new ApiError(MajorErrorCodes.ChatUser, 4, "Invalid or empty chat user password");
        public static ApiError ChatUserPasswordMaxLength => new ApiError(MajorErrorCodes.ChatUser, 5, $"Chat user password length exceeds more than {Domain.ChatUser.MaxPasswordLength} characters");
    }
}
