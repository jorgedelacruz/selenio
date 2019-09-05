using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.NUnit.PageObjects
{
    public class SearchResults : PageObject
    {
        [WaitForThisElement]
        [FindsBy(How = How.Id, Using = "b_content")]
        public IWebElement ContentSection { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href = 'http://docs.seleniumhq.org/']")]
        public IWebElement SeleniumDocsResultLink { get; set; }

        public SearchResults WaitToLoad()
        {
            this.WaitForScreen();

            return this;
        }
    }
}
