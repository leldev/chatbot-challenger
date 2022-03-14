using Jobsity.Chatbot.Api.Features.Models;
using Jobsity.Chatbot.Specs.Drivers;
using System.Net;

namespace Jobsity.Chatbot.Specs.Common
{
    public  class ValidationHelper
    {
        public static async Task ValidateFluentValidationErrorMessageAsync(ScenarioContext scenarioContext, string message)
        {
            var response = scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadAsJsonAsync<ApiErrorModel>().ConfigureAwait(false);

            error.GetErrorMessage().ShouldBe(message);
        }
    }
}
