using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.Tests.PageObjects.Google
{
    public class GoogleSignIn : PageObject
    {
        [WaitForThisElement]
        [FindsBy(How = How.CssSelector, Using = "input[type='email']")]
        public IWebElement Username { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[type='password']")]
        public IWebElement Password { get; set; }
    }
}
