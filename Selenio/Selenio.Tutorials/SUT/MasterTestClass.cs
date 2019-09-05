using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;

namespace Selenio.Tests
{
    public class MasterTestClass
    {
        private Exception ExceptionThrown;
        private bool TestResult = true;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initilize()
        {
            TutorialDriver.Initialize<ChromeDriver>(TestContext);
            TutorialDriver.Reporter.DebugLog("Test started.", true, true);
        }

        [TestCleanup]
        public void FinishOff()
        {
            TutorialDriver.Driver?.Quit();
            TutorialDriver.Reporter.FinishTest(TestResult, ExceptionThrown);
        }

        public void RunTest(Action test)
        {
            try
            {
                test.Invoke();
            }
            catch (Exception ex)
            {
                TestResult = false;
                ExceptionThrown = ex;
                throw;
            }
        }

    }
}
