using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.NUnit.PageObjects
{
    public class SeleniumHomePage : PageObject
    {
        [WaitForThisElement]
        [FindsBy(How = How.Id, Using = "menu_about")]
        public IWebElement AboutLink { get; set; }

        [FindsBy(How = How.Id, Using = "menu_support")]
        public IWebElement SupportLink { get; set; }

        [FindsBy(How = How.Id, Using = "menu_documentation")]
        public IWebElement DocumentationLink { get; set; }

        [FindsBy(How = How.Id, Using = "menu_download")]
        public IWebElement DownloadLink { get; set; }
    }

}
