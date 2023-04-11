using Bogus;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups_BDD_Tests.PageObjects
{
    public class AdminQuestionsCreatePageObject : PageObject
    {
        public AdminQuestionsCreatePageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "AdminQuestionsCreate";
        }

        public IWebElement QuestionTextInput => _webDriver.FindElement(By.Id("Question"));
        public IWebElement ActiveNumberInput => _webDriver.FindElement(By.Id("Active"));
        public IWebElement SubmitButton => _webDriver.FindElement(By.Id("submit"));

        public string EnterRandomQuestion()
        {
            // Using the very cook project, Bogus, to generate realistic text (among other things)
            Faker faker = new Faker("en");
            //string questionText = faker.Lorem.Sentence(8);  // or 
            string questionText = faker.Hacker.Phrase();
            QuestionTextInput.SendKeys(questionText);
            return questionText;
        }

        public void SetActive(bool active)
        {
            if(active)
            {
                ActiveNumberInput.SendKeys("1");
            }
            else
            {
                ActiveNumberInput.SendKeys("0");
            }
        }

        public void SubmitNewQuestion()
        {
            SubmitButton.Click();
        }
    }
}
