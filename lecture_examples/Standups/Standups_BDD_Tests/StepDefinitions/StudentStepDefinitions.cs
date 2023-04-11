using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace Standups_BDD_Tests.StepDefinitions
{
    [Binding]
    public class StudentStepDefinitions
    {
        private readonly SelectGroupPageObject _selectGroupPage;

        public StudentStepDefinitions(BrowserDriver browserDriver)
        {
            _selectGroupPage = new SelectGroupPageObject(browserDriver.Current);
        }

        [Given(@"I select a group")]
        public void GivenISelectAGroup()
        {
            _selectGroupPage.SelectRandomGroup();
            _selectGroupPage.SubmitGroupSelection();
        }
    }
}
