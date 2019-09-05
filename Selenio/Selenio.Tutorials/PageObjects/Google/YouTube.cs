using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.SUT;
using Selenio.Extensions;

namespace Selenio.Tests.PageObjects.Google
{
    public class YouTube : PageObject
    {
        [FindsBy(How = How.Id, Using = "logo-container")]
        public IWebElement Logo { get; set; }
        
        [WaitForThisElement]
        [FindsBy(How = How.Id, Using = "button")]
        public IWebElement Upload { get; set; }

        public virtual void ClickUpload()
        {
            TutorialDriver.Reporter.TestStep = "Click upload video";
            this.WaitForScreen();
            string text = Upload.Text;
            Upload.Click();
        }
    }
}
