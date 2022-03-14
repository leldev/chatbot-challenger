using Jobsity.Chatbot.Api.Features.Login;
using Jobsity.Chatbot.Api.Features.Login.Model;
using Jobsity.Chatbot.Api.Features.Models;
using Jobsity.Chatbot.Specs.Common;
using Jobsity.Chatbot.Specs.Drivers;
using System.Net;

namespace Jobsity.Chatbot.Specs.StepDefinitions.Login
{
    [Binding]
    public class LoginStepDefinitions
    {
        private const string loginApiUrl = "api/login";
        private const string chatUserApiUrl = "api/chatusers";
        private readonly HttpClient httpClient;
        private ScenarioContext scenarioContext;
        private readonly Fixture fixture;
        private Api.Features.Login.Create.CommandRequest postRequest;

        public LoginStepDefinitions(ChatbotAppFactory<TestStartup> chatbotAppFactory, ScenarioContext scenarioContext)
        {
            this.httpClient = chatbotAppFactory.CreateClient();
            this.scenarioContext = scenarioContext;
            this.fixture = new Fixture();
            this.postRequest = this.fixture.Create<Api.Features.Login.Create.CommandRequest>();
        }

        [Given(@"a valid user name and password")]
        public async Task WhenCreateNewUserAsync()
        {
            var url = new Uri(chatUserApiUrl, UriKind.Relative);
            var response = await this.httpClient.PostAsJsonAsync(url, this.postRequest).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                this.AddLocationToTearDownList(response.Headers.Location);
            }

            this.scenarioContext.Set(response);
        }

        [Given(@"a invalid user name and password")]
        public void GivenAInvalidUserNameAndPassword()
        {
            this.postRequest = this.fixture.Create<Api.Features.Login.Create.CommandRequest>();
        }

        [Given(@"a login user with invalid name")]
        public void GivenANewUserWithInvalid()
        {
            this.postRequest.Name = string.Empty;
        }

        [Given(@"a login user with long name")]
        public void GivenANewUserWithLongName()
        {
            this.postRequest.Name = string.Concat(Enumerable.Repeat("x", Domain.ChatUser.MaxNameLength + 1));
        }

        [Given(@"a login user with invalid password")]
        public void GivenANewUserWithInvalidPassword()
        {
            this.postRequest.Password = string.Empty;
        }

        [Given(@"a login user with long password")]
        public void GivenANewUserWithLongPassword()
        {
            this.postRequest.Password = string.Concat(Enumerable.Repeat("x", Domain.ChatUser.MaxPasswordLength + 1));
        }

        [When(@"user login")]
        public async Task WhenUserLoginAsync()
        {
            var url = new Uri(loginApiUrl, UriKind.Relative);
            var data = new Api.Features.Login.Create.CommandRequest()
            {
                Name = this.postRequest.Name,
                Password = this.postRequest.Password
            };

            var result = await this.httpClient.PostAsJsonAsync(url, data).ConfigureAwait(false);
            this.scenarioContext.Set(result);
        }

        [Then(@"user should be logged")]
        public async Task ThenUserShouldBeLoggedAsync()
        {
            await ValidateModelAsync(HttpStatusCode.OK).ConfigureAwait(false);
        }

        [Then(@"user should not be logged")]
        public async Task ThenUserShouldNotBeLoggedAsync()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        [Then(@"user login should not be created with invalid name error")]
        public async Task ThenUserShouldNotBeCreatedWithInvalidNameErrorAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, LoginErrors.InvalidChatUserName.Message).ConfigureAwait(false);
        }

        [Then(@"user login should not be created with invalid password error")]
        public async Task ThenUserShouldNotBeCreatedWithInvalidPasswordErrorAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, LoginErrors.InvalidChatUserPassword.Message).ConfigureAwait(false);
        }

        [Then(@"user login should not be created with name lenght exceeds characters")]
        public async Task ThenUserLoginShouldNotBeCreatedWithNameLenghtExceedsCharactersAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, LoginErrors.ChatUserNameMaxLength.Message).ConfigureAwait(false);
        }

        [Then(@"user login should not be created with password lenght exceeds characters")]
        public async Task ThenUserLoginShouldNotBeCreatedWithPasswordLenghtExceedsCharactersAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, LoginErrors.ChatUserPasswordMaxLength.Message).ConfigureAwait(false);
        }

        private async Task ValidateModelAsync(HttpStatusCode statusCode)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(statusCode);

            var model = await response.Content.ReadAsJsonAsync<LoginModel>().ConfigureAwait(false);

            model.Id.ShouldNotBeNull();
            model.Name.ShouldBe(this.postRequest.Name);
        }

        private void AddLocationToTearDownList(Uri location)
        {
            var tearDownUriList = this.scenarioContext.Get<List<Uri>>();
            tearDownUriList.Add(location);

            this.scenarioContext.Set(tearDownUriList);
        }
    }
}
