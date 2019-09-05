using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenio.Core.SUT
{
    public abstract class PageObject
    {
        public IWebDriver Driver;
        public WebDriverWait Wait;
    }
}
