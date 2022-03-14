using Newtonsoft.Json.Linq;
using System;

namespace Jobsity.Chatbot.Api.Features.Models
{
    public class ApiErrorModel
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public dynamic Errors { get; set; }

        public string GetErrorMessage()
        {
            if (this.Errors != null)
            {
                var _errors = (JContainer)this.Errors;
                return this.GetErrorMessage(_errors);
            }

            return string.Empty;
        }

        private string GetErrorMessage(JContainer error)
        {
            if (error.Count > 0)
            {
                if (error.First is JValue)
                {
                    return ((JValue)error.First).Value<string>();
                }
                else
                {
                    return this.GetErrorMessage((JContainer)error.First);
                }
            }
            else
            {
                return error.Value<string>();
            }
        }
    }
}
