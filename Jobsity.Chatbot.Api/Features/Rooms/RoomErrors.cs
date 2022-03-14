using Jobsity.Chatbot.Api.Common.ApiErrors;

namespace Jobsity.Chatbot.Api.Features.Rooms
{
    public class RoomErrors
    {
        public static ApiError CreateChat => new ApiError(MajorErrorCodes.Room, 1, "Create room call failed");
        public static ApiError InvalidRoomName => new ApiError(MajorErrorCodes.Room, 2, "Invalid or empty room name");
        public static ApiError RoomNameMaxLength => new ApiError(MajorErrorCodes.Room, 3, $"Room name length exceeds more than {Domain.Room.MaxNameLength} characters");
        public static ApiError InvalidChatName => new ApiError(MajorErrorCodes.Room, 4, "Invalid or empty chat name");
        public static ApiError ChatNameMaxLength => new ApiError(MajorErrorCodes.Room, 5, $"Chat name length exceeds more than {Domain.Chat.MaxMessageLength} characters");
    }
}
