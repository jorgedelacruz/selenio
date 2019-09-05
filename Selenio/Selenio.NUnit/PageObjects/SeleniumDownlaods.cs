using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.NUnit.PageObjects
{
    public class SeleniumDownlaods : PageObject
    {
        [WaitForThisElement]
        [FindsBy(How = How.XPath, Using = "//h2[text() = 'Downloads']")]
        public IWebElement Header { get; set; }
    }

}
