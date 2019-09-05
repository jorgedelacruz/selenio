using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.NUnit.PageObjects
{
    public class LandingPage : PageObject
    {
        [WaitForThisElement]
        [FindsBy(How = How.Id, Using = "sb_form_q")]
        public IWebElement SearchBox { get; set; }

        [FindsBy(How = How.Id, Using = "sb_form_go")]
        public IWebElement SubmitButton { get; set; }

        public LandingPage Open()
        {
            Driver.Url = "http://www.bing.com";
            this.WaitForScreen();

            return this;
        }

        public LandingPage Search(string query)
        {
            SearchBox.SendKeys(query);
            SubmitButton.Click();

            return this;
        }
    }

}
