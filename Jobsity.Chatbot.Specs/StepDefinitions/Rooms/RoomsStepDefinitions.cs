using Jobsity.Chatbot.Api.Features.Models;
using Jobsity.Chatbot.Api.Features.Rooms;
using Jobsity.Chatbot.Api.Features.Rooms.Models;
using Jobsity.Chatbot.Specs.Common;
using Jobsity.Chatbot.Specs.Drivers;
using System.Net;

namespace Jobsity.Chatbot.Specs.StepDefinitions.Rooms
{
    [Binding]
    public class RoomsStepDefinitions
    {
        private const string roomApiUrl = "api/rooms";
        private readonly HttpClient httpClient;
        private ScenarioContext scenarioContext;
        private readonly Fixture fixture;
        private Api.Features.Rooms.Create.CommandRequest postRequest;
        private IList<Api.Features.Rooms.Create.CommandRequest> postRequestList;

        public RoomsStepDefinitions(ChatbotAppFactory<TestStartup> chatbotAppFactory, ScenarioContext scenarioContext)
        {
            this.httpClient = chatbotAppFactory.CreateClient();
            this.scenarioContext = scenarioContext;
            this.fixture = new Fixture();
            this.postRequest = this.fixture.Create<Api.Features.Rooms.Create.CommandRequest>();
            this.postRequestList = new List<Api.Features.Rooms.Create.CommandRequest>();
        }

        [Given(@"a new room data")]
        public void GivenANewRoomData()
        {
            this.postRequest = this.fixture.Create<Api.Features.Rooms.Create.CommandRequest>();
        }

        [Given(@"a new room with invalid name")]
        public void GivenANewRoomWithInvalid()
        {
            this.postRequest.Name = string.Empty;
        }

        [Given(@"a new room with long name")]
        public void GivenANewRoomWithLongName()
        {
            this.postRequest.Name = string.Concat(Enumerable.Repeat("x", Domain.Room.MaxNameLength + 1));
        }

        [Given(@"a valid list of room")]
        public async Task GivenAValidListOfRoomAsync()
        {
            // Create few rooms
            for (int i = 0; i < 2; i++)
            {
                var room1 = this.fixture.Create<Api.Features.Rooms.Create.CommandRequest>();
                this.postRequestList.Add(room1);
                this.postRequest = room1;
                await this.CreateRoomAsync().ConfigureAwait(false);
            }
        }

        [Given(@"a valid room")]
        [When(@"create new room")]
        public async Task WhenCreateNewRoomAsync()
        {
            await this.CreateRoomAsync().ConfigureAwait(false);
        }

        [When(@"get room detail")]
        public async Task WhenGetRoomDetailAsync()
        {
            // Get id created
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var model = await response.Content.ReadAsJsonAsync<RoomModel>().ConfigureAwait(false);

            var url = new Uri($"{roomApiUrl}/{model.Id}", UriKind.Relative);
            var result = await this.httpClient.GetAsync(url).ConfigureAwait(false);
            this.scenarioContext.Set(result);
        }

        [When(@"get room list")]
        public async Task WhenGetRooms()
        {
            var url = new Uri(roomApiUrl, UriKind.Relative);
            var result = await this.httpClient.GetAsync(url).ConfigureAwait(false);
            this.scenarioContext.Set(result);
        }

        [When(@"delete room")]
        public async Task WhenDeleteRoomAsync()
        {
            // Get id created
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var model = await response.Content.ReadAsJsonAsync<RoomModel>().ConfigureAwait(false);

            var url = new Uri($"{roomApiUrl}/{model.Id}", UriKind.Relative);
            var result = await this.httpClient.DeleteAsync(url).ConfigureAwait(false);
            this.scenarioContext.Set(result);
        }

        [Then(@"room should be created")]
        public async Task ThenRoomShouldBeCreatedAsync()
        {
            await ValidateModelAsync(HttpStatusCode.Created).ConfigureAwait(false);

        }

        [Then(@"room should be retrieved")]
        public async Task ThenRoomShouldBeRetrievedAsync()
        {
            await ValidateModelAsync(HttpStatusCode.OK).ConfigureAwait(false);
        }

        [Then(@"rooms should be retrieved")]
        public async Task ThenRoomsShouldBeRetrieved()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var list = await response.Content.ReadAsJsonAsync<List<RoomModel>>().ConfigureAwait(false);

            list.ShouldNotBeNull();
            list.ShouldNotBeEmpty();

            foreach (var item in this.postRequestList)
            {
                list.Any(x => x.Name == item.Name).ShouldBeTrue();
            }

            this.AddLocationToTearDownList(response.Headers.Location);
        }

        [Then(@"room should be deleted")]
        public void ThenRoomShouldBeDeleted()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Then(@"room should not be created with invalid name error")]
        public async Task ThenRoomShouldNotBeCreatedWithInvalidNameError()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, RoomErrors.InvalidRoomName.Message).ConfigureAwait(false);
        }

        [Then(@"room should not be created with name lenght exceeds characters")]
        public async Task TaskThenRoomShouldNotBeCreatedWithNameLenghtExceedsCharactersAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, RoomErrors.RoomNameMaxLength.Message).ConfigureAwait(false);
        }

        private async Task CreateRoomAsync()
        {
            var url = new Uri(roomApiUrl, UriKind.Relative);
            var response = await this.httpClient.PostAsJsonAsync(url, this.postRequest).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                this.AddLocationToTearDownList(response.Headers.Location);
            }

            this.scenarioContext.Set(response);

            await this.SaveModelToScenarioContextAsync().ConfigureAwait(false);
        }

        private async Task SaveModelToScenarioContextAsync()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var model = await response.Content.ReadAsJsonAsync<RoomModel>().ConfigureAwait(false);

            // Save for shared step
            this.scenarioContext.Set(model);
        }

        private async Task ValidateModelAsync(HttpStatusCode statusCode)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(statusCode);

            var model = await response.Content.ReadAsJsonAsync<RoomModel>().ConfigureAwait(false);

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
