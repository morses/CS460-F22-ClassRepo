using NUnit.Framework;
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

        [Given(@"I am on the ""([^""]*)"" page"), When(@"I am on the ""([^""]*)"" page")]
        public void WhenIAmOnThePage(string pageName)
        {
            _homePage.GoTo();
        }

        [Then(@"The page title contains ""([^""]*)""")]
        public void ThenThePageTitleContains(string p0)
        {
            //Thread.Sleep(4000);
            _homePage.GetTitle().Should().ContainEquivalentOf(p0, AtLeast.Once());
        }

        [Then(@"The page presents a Register button")]
        public void ThenThePagePresentsARegisterButton()
        {
            _homePage.RegisterButton.Should().NotBeNull();
            _homePage.RegisterButton.Displayed.Should().BeTrue();

            // And must it be a button?  Makes it more fragile and increases "friction" in developing the UI, but...
            // Here using normal NUnit constraints
            Assert.That(_homePage.RegisterButton.GetAttribute("class"), Does.Contain("btn"));
        }


        [Then(@"I can save cookies")]
        public void ThenICanSaveCookies()
        {
            _homePage.SaveAllCookies().Should().BeTrue();
        }

        [When(@"I load previously saved cookies")]
        public void WhenILoadPreviouslySavedCookies()
        {
            _homePage.LoadAllCookies().Should().BeTrue();
        }


    }
}
