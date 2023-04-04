using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace Standups_BDD_Tests.StepDefinitions
{
    [Binding]
    public class HomeStepDefinitions
    {
        private readonly HomePageObject _homePage;

        public HomeStepDefinitions( BrowserDriver browserDriver ) 
        {
            _homePage = new HomePageObject(browserDriver.Current);
        }


        [Given(@"I am a visitor")]
        public void GivenIAmAVisitor()
        {
            // Nothing to do at present
        }

        [When(@"I am on the ""([^""]*)"" page")]
        public void WhenIAmOnThePage(string pageName)
        {
            // using a named page (in Common.cs)
            _homePage.GoTo(pageName);
        }

        [Then(@"The page title contains ""([^""]*)""")]
        public void ThenThePageTitleContains(string p0)
        {
            //Thread.Sleep(4000);
            _homePage.GetTitle().Should().ContainEquivalentOf(p0, AtLeast.Once());
        }

        [Then(@"I can save cookies")]
        public void ThenICanSaveCookies()
        {
            _homePage.SaveAllCookies().Should().BeTrue();
        }

    }
}
