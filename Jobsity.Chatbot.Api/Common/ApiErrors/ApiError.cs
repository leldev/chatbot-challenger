namespace Jobsity.Chatbot.Api.Common.ApiErrors
{
    public class ApiError
    {
        private readonly string description;
        private readonly int majorErrorCode;
        private readonly int minorErrorCode;
        public string Message => $"{majorErrorCode + minorErrorCode} - {description}";

        public ApiError(int majorErrorCode, int minorErrorCode, string description)
        {
            this.majorErrorCode = majorErrorCode;
            this.minorErrorCode = minorErrorCode;
            this.description = description;
        }

        public static ApiError Create(int majorErrorCode, int minorErrorCode, string description)
        {
            return new ApiError(majorErrorCode, minorErrorCode, description);
        }
    }
}