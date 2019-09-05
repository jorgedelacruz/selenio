using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;
using static Selenio.Core.SUT.SUTDriver;

namespace Selenio.Tests.PageObjects.Google
{
    public class HomeScreen : PageObject
    {
        [FindsBy(How = How.CssSelector, Using = "input[role='combobox']")]
        public IWebElement Omnibox { get; set; }

        [FindsBy(How = How.Id, Using = "dummy-element")]
        public IWebElement DummyElement { get; set; }

        [WaitForThisElement]
        [FindsBy(How = How.Id, Using = "hplogo")]
        public IWebElement Logo { get; set; }

        public virtual HomeScreen Open(string url)
        {
            Reporter.TestStep = "Open google homepage";
            Driver.Url = url;
            this.WaitForScreen();
            return this;
        }

        public virtual HomeScreen Search(string query)
        {
            Reporter.TestStep = "Search youtube in google";
            this.WaitForElement(Logo);
            Omnibox.SendKeys(query);
            Omnibox.Submit();
            return this;
        }

    }
}
