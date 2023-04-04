using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Standups_BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace Standups_BDD_Tests.PageObjects
{
    public class HomePageObject : PageObject
    {
        private const string Url = "https://localhost:7283/";

        //The default wait time in seconds for wait.Until
        public const int DefaultWaitInSeconds = 5;
        public HomePageObject(IWebDriver webDriver) : base(webDriver)
        {
            _webDriver.Navigate().GoToUrl(Url);
        }

    }
}
