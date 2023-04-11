using OpenQA.Selenium.DevTools.V109.Network;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace Standups_BDD_Tests.StepDefinitions
{
    [Binding]
    public class AdminStepDefinitions
    {
        private readonly AdminQuestionsPageObject _adminQuestionsPage;
        private readonly AdminQuestionsCreatePageObject _adminQuestionsCreatePage;
        private readonly ScenarioContext _scenarioContext;

        public AdminStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _scenarioContext = context;
            _adminQuestionsPage = new AdminQuestionsPageObject(browserDriver.Current);
            _adminQuestionsCreatePage = new AdminQuestionsCreatePageObject(browserDriver.Current);
        }

        [When(@"I click on Create New question")]
        public void WhenIClickOnCreateNewQuestion()
        {
            // We are on the Admin/Questions/Index page
            _adminQuestionsPage.CreateNewQuestion();
        }


        [When(@"I create a new ""([^""]*)"" question called ""([^""]*)""")]
        public void WhenICreateANewQuestionCalled(string active, string question)
        {
            // We are on the Admin/Questions/Create page
            bool activeState = active == "active";
            _scenarioContext["CurrentQuestionId"] = question;
            _scenarioContext["CurrentQuestionText"] = _adminQuestionsCreatePage.EnterRandomQuestion();
            _adminQuestionsCreatePage.SetActive(true);
            _adminQuestionsCreatePage.SubmitNewQuestion();
        }
    }
}
