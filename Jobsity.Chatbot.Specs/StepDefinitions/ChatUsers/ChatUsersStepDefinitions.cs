using Jobsity.Chatbot.Api.Features.ChatUsers;
using Jobsity.Chatbot.Api.Features.ChatUsers.Models;
using Jobsity.Chatbot.Api.Features.Models;
using Jobsity.Chatbot.Specs.Common;
using Jobsity.Chatbot.Specs.Drivers;
using System.Collections;
using System.Net;

namespace Jobsity.Chatbot.Specs.StepDefinitions.ChatUsers
{
    [Binding]
    public class ChatUsersStepDefinitions
    {
        private const string chatUserApiUrl = "api/chatusers";
        private readonly HttpClient httpClient;
        private ScenarioContext scenarioContext;
        private readonly Fixture fixture;
        private Api.Features.ChatUsers.Create.CommandRequest postRequest;

        public ChatUsersStepDefinitions(ChatbotAppFactory<TestStartup> chatbotAppFactory, ScenarioContext scenarioContext)
        {
            this.httpClient = chatbotAppFactory.CreateClient();
            this.scenarioContext = scenarioContext;
            this.fixture = new Fixture();
            this.postRequest = this.fixture.Create<Api.Features.ChatUsers.Create.CommandRequest>();
        }

        [Given(@"a new user data")]
        public void GivenANewUserData()
        {
            this.postRequest = this.fixture.Create<Api.Features.ChatUsers.Create.CommandRequest>();
        }

        [Given(@"a new user with invalid name")]
        public void GivenANewUserWithInvalid()
        {
            this.postRequest.Name = string.Empty;
        }

        [Given(@"a new user with long name")]
        public void GivenANewUserWithLongName()
        {
            this.postRequest.Name = string.Concat(Enumerable.Repeat("x", Domain.ChatUser.MaxNameLength + 1));
        }

        [Given(@"a new user with invalid password")]
        public void GivenANewUserWithInvalidPassword()
        {
            this.postRequest.Password = string.Empty;
        }

        [Given(@"a new user with long password")]
        public void GivenANewUserWithLongPassword()
        {
            this.postRequest.Password = string.Concat(Enumerable.Repeat("x", Domain.ChatUser.MaxPasswordLength + 1));
        }

        [Given(@"a valid user")]
        [When(@"create new user")]
        public async Task WhenCreateNewUserAsync()
        {
            var url = new Uri(chatUserApiUrl, UriKind.Relative);
            var result = await this.httpClient.PostAsJsonAsync(url, this.postRequest).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                this.AddLocationToTearDownList(result.Headers.Location);
            }

            this.scenarioContext.Set(result);

            await this.SaveModelToScenarioContextAsync().ConfigureAwait(false);
        }

        [When(@"get user detail")]
        public async Task WhenGetUserDetailAsync()
        {
            // Get created id
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var model = await response.Content.ReadAsJsonAsync<ChatUserModel>().ConfigureAwait(false);

            var url = new Uri($"{chatUserApiUrl}/{model.Id}", UriKind.Relative);
            var result = await this.httpClient.GetAsync(url).ConfigureAwait(false);
            this.scenarioContext.Set(result);
        }

        [When(@"delete user")]
        public async Task WhenDeleteUserAsync()
        {
            // Get created id
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var model = await response.Content.ReadAsJsonAsync<ChatUserModel>().ConfigureAwait(false);

            var url = new Uri($"{chatUserApiUrl}/{model.Id}", UriKind.Relative);
            var result = await this.httpClient.DeleteAsync(url).ConfigureAwait(false);
            this.scenarioContext.Set(result);
        }

        [Then(@"user should be created")]
        public async Task ThenUserShouldBeCreatedAsync()
        {
            await ValidateModelAsync(HttpStatusCode.Created).ConfigureAwait(false);

        }

        [Then(@"user should be retrieved")]
        public async Task ThenUserShouldBeRetrievedAsync()
        {
            await ValidateModelAsync(HttpStatusCode.OK).ConfigureAwait(false);
        }

        [Then(@"user should be deleted")]
        public void ThenUserShouldBeDeleted()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Then(@"user should not be created with invalid name error")]
        public async Task ThenUserShouldNotBeCreatedWithInvalidNameError()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, ChatUserErrors.InvalidChatUserName.Message).ConfigureAwait(false);
        }

        [Then(@"user should not be created with invalid password error")]
        public async Task ThenUserShouldNotBeCreatedWithInvalidPasswordError()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, ChatUserErrors.InvalidChatUserPassword.Message).ConfigureAwait(false);

        }

        [Then(@"user should not be created with name lenght exceeds characters")]
        public async Task ThenUserShouldNotBeCreatedWithNameLenghtExceedsCharactersAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, ChatUserErrors.ChatUserNameMaxLength.Message).ConfigureAwait(false);
        }

        [Then(@"user should not be created with password lenght exceeds characters")]
        public async Task ThenUserShouldNotBeCreatedWithPasswordLenghtExceedsCharactersAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, ChatUserErrors.ChatUserPasswordMaxLength.Message).ConfigureAwait(false);
        }


        private async Task ValidateModelAsync(HttpStatusCode statusCode)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(statusCode);

            var model = await response.Content.ReadAsJsonAsync<ChatUserModel>().ConfigureAwait(false);

            model.Id.ShouldNotBeNull();
            model.Name.ShouldBe(this.postRequest.Name);

            this.AddLocationToTearDownList(response.Headers.Location);
        }

        private async Task SaveModelToScenarioContextAsync()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var model = await response.Content.ReadAsJsonAsync<ChatUserModel>().ConfigureAwait(false);

            // Save for shared step
            this.scenarioContext.Set(model);
        }

        private void AddLocationToTearDownList(Uri location)
        {
            var tearDownUriList = this.scenarioContext.Get<List<Uri>>();
            tearDownUriList.Add(location);

            this.scenarioContext.Set(tearDownUriList);
        }
    }
}
