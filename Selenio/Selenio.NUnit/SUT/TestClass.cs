using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Chrome;
using Selenio.Core.Reporting;

namespace Selenio.NUnit
{
    [TestFixture]
    public class TestClass
    {
        public IReporter Reporter;

        [OneTimeSetUp]
        public void ClassSetup()
        {

        }

        [SetUp]
        public void Setup()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("incognito");
            Driver.Initialize<ChromeDriver>(chromeOptions);
            
            Reporter = Driver.Reporter;
        }

        [TearDown]
        public void Teardown()
        {
            Driver.Reporter.FinishTest(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed, TestContext.CurrentContext.Result.Message);
            Driver.Driver?.Quit();
        }
    }
}
