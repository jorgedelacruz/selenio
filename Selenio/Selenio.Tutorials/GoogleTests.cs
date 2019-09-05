using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Selenio.Extensions;
using static Selenio.Core.SUT.SUTDriver;

namespace Selenio.Tests
{
    [TestClass]
    public class GoogleTests : MasterTestClass
    {
        [TestMethod]
        public void Passes()
        {
            RunTest(() =>
            {
                Reporter.TestDescription = "Youtube upload";

                TutorialDriver.GoogleHomeScreen.Open("http://www.google.com.do");
                TutorialDriver.GoogleHomeScreen.Search("youtube");
                TutorialDriver.GoogleSearchResults.ClickYoutubeLink();
            });
        }

        [TestMethod]
        public void FailsWithAssertion()
        {
            RunTest(() =>
            {
                Reporter.TestDescription = "Search YouTube";

                TutorialDriver.GoogleHomeScreen.Open("http://www.google.com.do");
                TutorialDriver.GoogleHomeScreen.Search("youtube");
                Reporter.ReportAssertion("Test", "Failing test using assertion...", "foo", "var", false, "fail");
                //Reporter.StatusUpdate("Failing test using assertion or reports...", false);
            });
        }

        [TestMethod]
        public void FailsWithReports()
        {
            RunTest(() =>
            {
                Reporter.TestDescription = "Search YouTube";

                TutorialDriver.GoogleHomeScreen.Open("http://www.google.com.do");
                TutorialDriver.GoogleHomeScreen.Search("youtube");
                Reporter.StatusUpdate("Failing test using reports...", false);
            });
        }

        [TestMethod]
        public void FailWithException()
        {
            RunTest(() =>
            {
                Reporter.TestDescription = "Search YouTube";

                TutorialDriver.GoogleHomeScreen.Open("http://www.google.com.do");
                TutorialDriver.GoogleHomeScreen.Search("youtube");
                throw new System.Exception("Failing the test using an exception...");
            });
        }

        [TestMethod]
        public void SignIn()
        {
            RunTest(() =>
            {
                Reporter.TestDescription = "Google Sign in";

                TutorialDriver.GoogleHomeScreen.Open("http://www.google.com.do");
                TutorialDriver.GoogleHomeScreen.SignIn.Click();
                TutorialDriver.GoogleSignIn.WaitForScreen();
                TutorialDriver.GoogleSignIn.Username.SendKeys("jcruzz92" + Keys.Enter);
                TutorialDriver.GoogleSignIn.WaitForElement(TutorialDriver.GoogleSignIn.Password);
                TutorialDriver.GoogleSignIn.Password.SendKeys("testpassword");
            });
        }

    }
}
