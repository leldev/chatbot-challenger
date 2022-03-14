using Jobsity.Chatbot.Specs.Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobsity.Chatbot.Specs.StepDefinitions.CommonSteps
{
    [Binding]
    public class CommonSterpsDefinitions
    {
        private readonly HttpClient httpClient;
        private ScenarioContext scenarioContext;

        public CommonSterpsDefinitions(ChatbotAppFactory<TestStartup> chatbotAppFactory, ScenarioContext scenarioContext)
        {
            this.httpClient = chatbotAppFactory.CreateClient();
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void SetupTierDown()
        {
            this.scenarioContext.Set(new List<Uri>());
        }

        [AfterScenario]
        public async Task TierDownAsync()
        {
            var tearDownUriList = this.scenarioContext.Get<List<Uri>>();
            foreach (var url in tearDownUriList)
            {
                await this.httpClient.DeleteAsync(url).ConfigureAwait(false);
            }
        }
    }
}
