using OpenQA.Selenium;
using Standups_BDD_Tests.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups_BDD_Tests.PageObjects
{
    public class AdminQuestionsPageObject : PageObject
    {
        public AdminQuestionsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "AdminQuestions";
        }

        public IWebElement CreateNewLink => _webDriver.FindElement(By.CssSelector("a[href=\"/Admin/Questions/Create\"]"));

        public void CreateNewQuestion()
        {
            CreateNewLink.Click();
        }
    }
}
