using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups_BDD_Tests.PageObjects
{
    public class SelectGroupPageObject : PageObject
    {
        private readonly Random random = new Random();
        public SelectGroupPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "SelectGroup";
        }

        public IWebElement GroupSelectList => _webDriver.FindElement(By.Id("GroupId"));

        public IWebElement SubmitButton => _webDriver.FindElement(By.Id("submit-group"));

        public void SelectRandomGroup()
        {
            // SelectElement is in Selenium.Support package
            SelectElement select = new SelectElement(GroupSelectList);
            IList<IWebElement> options = select.Options;
            int index = random.Next(options.Count);
            select.SelectByIndex(index);
        }

        public void SubmitGroupSelection()
        {
            SubmitButton.Click();
        }

    }
}
