using Jobsity.Chatbot.Api.Features.ChatUsers.Models;
using Jobsity.Chatbot.Api.Features.Models;
using Jobsity.Chatbot.Api.Features.Rooms;
using Jobsity.Chatbot.Api.Features.Rooms.Models;
using Jobsity.Chatbot.Specs.Common;
using Jobsity.Chatbot.Specs.Drivers;
using System.Net;

namespace Jobsity.Chatbot.Specs.StepDefinitions.Rooms.Chats
{
    [Binding]
    public class ChatsStepDefinitions
    {
        private const string roomApiUrlFormat = "api/rooms/{0}/chats";
        private int chatCant = 1;
        private readonly HttpClient httpClient;
        private ScenarioContext scenarioContext;
        private readonly Fixture fixture;
        private RoomModel roomModel;
        private ChatUserModel chatUserModel;
        private List<ChatWriteModel> postRequestList;

        public ChatsStepDefinitions(ChatbotAppFactory<TestStartup> chatbotAppFactory, ScenarioContext scenarioContext)
        {
            this.httpClient = chatbotAppFactory.CreateClient();
            this.scenarioContext = scenarioContext;
            this.fixture = new Fixture();
            this.postRequestList = new List<ChatWriteModel>();
            this.roomModel = new RoomModel();
            this.chatUserModel = new ChatUserModel();
        }

        [Given(@"a new chat data")]
        public void GivenANewChatData()
        {
            this.LoadCreatedEntitiesFromOtherSteps();

            this.GenerateChatPostRequestData();
        }

        [Given(@"multiple chats data")]
        public void GivenMultipleChatsData()
        {
            this.LoadCreatedEntitiesFromOtherSteps();
            this.chatCant = 3;
            this.GenerateChatPostRequestData();
        }

        [Given(@"a new chat with invalid name")]
        public void GivenANewChatWithInvalidName()
        {
            this.LoadCreatedEntitiesFromOtherSteps();

            var postData = this.fixture.Build<ChatWriteModel>()
                    .With((x) => x.Message, string.Empty)
                    .With((x) => x.UserId, this.chatUserModel.Id)
                    .With((x) => x.UserName, this.chatUserModel.Name)
                    .Create();

            this.postRequestList.Add(postData);
        }

        [Given(@"a new chat with long name")]
        public void GivenANewChatWithLongName()
        {
            this.LoadCreatedEntitiesFromOtherSteps();

            var postData = this.fixture.Build<ChatWriteModel>()
                    .With((x) => x.Message, string.Concat(Enumerable.Repeat("x", Domain.Chat.MaxMessageLength + 1)))
                    .With((x) => x.UserId, this.chatUserModel.Id)
                    .With((x) => x.UserName, this.chatUserModel.Name)
                    .Create();

            this.postRequestList.Add(postData);
        }

        [Given(@"more chats data than limit to show")]
        public async Task GivenMoreChatsDataThanLimitToShowAsync()
        {
            this.LoadCreatedEntitiesFromOtherSteps();
            this.chatCant = Domain.Chat.MaxChatsToShow + 3;
            this.GenerateChatPostRequestData();
            
            await this.CreateChatRoomAsync().ConfigureAwait(false);
        }

        [Given(@"a new chat data with bot key")]
        public void GivenANewChatDataWithBotKey()
        {
            this.LoadCreatedEntitiesFromOtherSteps();

            var postData = this.fixture.Build<ChatWriteModel>()
                    .With((x) => x.Message, "/stock=aapl.us")
                    .With((x) => x.UserId, this.chatUserModel.Id)
                    .With((x) => x.UserName, this.chatUserModel.Name)
                    .Create();

            this.postRequestList.Add(postData);
        }

        [When(@"create new chat")]
        [When(@"create multiple chats")]
        public async Task WhenCreateNewChatAsync()
        {
            await this.CreateChatRoomAsync().ConfigureAwait(false);
        }

        [Then(@"chat should be created")]
        public async Task ThenChatShouldBeCreatedAsync()
        {
            await this.ValidateModelAsync(HttpStatusCode.Created);
        }

        [Then(@"multiple chats should be created")]
        public async Task ThenMultipleChatsShouldBeCreatedAsync()
        {
            await this.ValidateModelAsync(HttpStatusCode.Created);
        }

        [Then(@"room chat should be equal to limit to show")]
        public async Task ThenRoomChatShouldBeEqualToLimitToShowAsync()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var model = await response.Content.ReadAsJsonAsync<ChatRoomModel>().ConfigureAwait(false);

            model.Id.ShouldNotBeNull();
            model.Name.ShouldBe(this.roomModel.Name);
            model.Chats.ShouldNotBeNull();
            model.Chats.Count.ShouldBe(Domain.Chat.MaxChatsToShow);
        }

        [Then(@"chat should not be created with invalid name error")]
        public async Task WhenCreateMultipleChatsAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, RoomErrors.InvalidChatName.Message).ConfigureAwait(false);
        }

        [Then(@"chat should not be created with message lenght exceeds characters")]
        public async Task ThenChatShouldNotBeCreatedWithMessageLenghtExceedsCharactersAsync()
        {
            await ValidationHelper.ValidateFluentValidationErrorMessageAsync(this.scenarioContext, RoomErrors.ChatNameMaxLength.Message).ConfigureAwait(false);
        }

        [Then(@"chat bot should be retrieved")]
        public async Task ThenChatBotShouldBeRetrievedAsync()
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var model = await response.Content.ReadAsJsonAsync<ChatRoomModel>().ConfigureAwait(false);

            model.Id.ShouldNotBeNull();
            model.Name.ShouldBe(this.roomModel.Name);
            model.Chats.ShouldNotBeNull();
            model.Chats.Count.ShouldBe(this.chatCant);

            foreach (var chat in model.Chats)
            {
                this.postRequestList.Any((x) => x.Message != chat.Message).ShouldBeTrue();
                chat.UserId.ShouldBe("chatbot-stock");
                chat.UserName.ShouldBe("Chat Bot");
                chat.CreatedDate.ToString().ShouldNotBeNull();
            }
        }

        private void LoadCreatedEntitiesFromOtherSteps()
        {
            this.roomModel = this.scenarioContext.Get<RoomModel>();
            this.chatUserModel = this.scenarioContext.Get<ChatUserModel>();
        }

        private void GenerateChatPostRequestData()
        {
            for (int i = 0; i < this.chatCant; i++)
            {
                var postData = this.fixture.Build<ChatWriteModel>()
                    .With((x) => x.UserId, this.chatUserModel.Id)
                    .With((x) => x.UserName, this.chatUserModel.Name)
                    .Create();

                this.postRequestList.Add(postData);
            }
        }

        private async Task CreateChatRoomAsync()
        {
            foreach (var postRequest in this.postRequestList)
            {
                var url = new Uri(String.Format(roomApiUrlFormat, this.roomModel.Id), UriKind.Relative);
                var result = await this.httpClient.PostAsJsonAsync(url, postRequest).ConfigureAwait(false);

                if (result.IsSuccessStatusCode)
                {
                    this.AddLocationToTearDownList(result.Headers.Location);
                }

                this.scenarioContext.Set(result);
            }
        }

        private async Task ValidateModelAsync(HttpStatusCode statusCode)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.ShouldBe(statusCode);

            var model = await response.Content.ReadAsJsonAsync<ChatRoomModel>().ConfigureAwait(false);

            model.Id.ShouldNotBeNull();
            model.Name.ShouldBe(this.roomModel.Name);
            model.Chats.ShouldNotBeNull();
            model.Chats.Count.ShouldBe(this.chatCant);

            foreach (var chat in model.Chats)
            {
                this.postRequestList.Any((x) => x.Message == chat.Message).ShouldBeTrue();
                chat.UserId.ShouldBe(this.chatUserModel.Id);
                chat.UserName.ShouldBe(this.chatUserModel.Name);
                chat.CreatedDate.ToString().ShouldNotBeNull();
            }
        }

        private void AddLocationToTearDownList(Uri location)
        {
            var tearDownUriList = this.scenarioContext.Get<List<Uri>>();
            tearDownUriList.Add(location);

            this.scenarioContext.Set(tearDownUriList);
        }
    }
}
