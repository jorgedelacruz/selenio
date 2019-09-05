using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.Tests.PageObjects.Google
{
    public class SearchResults : PageObject
    {
        [FindsBy(How = How.Id, Using = "logo")]
        public IWebElement Logo { get; set; }

        [WaitForThisElement]
        [FindsBy(How = How.XPath, Using = "//a/h3/div[text()='YouTube']")]
        public IWebElement YouTubeLink { get; set; }

        public virtual SearchResults ClickYoutubeLink()
        {
            TutorialDriver.Reporter.TestStep = "Open youtube page";
            this.WaitForScreen();
            string href = YouTubeLink.GetAttribute("href");
            YouTubeLink.Click();
            return this;
        }
    }
}
