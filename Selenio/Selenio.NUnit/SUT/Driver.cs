using NUnit.Framework;
using OpenQA.Selenium;
using Selenio.Core.SUT;
using Selenio.HtmlReporter;
using Selenio.NUnit.PageObjects;
using Selenio.NUnit.Reporting;

namespace Selenio.NUnit
{
    public class Driver : SUTDriver
    {
        public static void Initialize<T>(DriverOptions options = null) where T : IWebDriver
        {
            string assemblyLocation = TestContext.CurrentContext.TestDirectory;
            string methodName = TestContext.CurrentContext.Test.MethodName;
            string className = TestContext.CurrentContext.Test.ClassName;
            var reporter = new Reporter(new ReportSettingsProvider(), assemblyLocation, className, methodName);

            if (options == null) InitDriver<T>(reporter); else InitDriver<T>(reporter, options);
            Driver.Manage().Window.Maximize();
        }

        public static LandingPage HomePage => GetPage<LandingPage>();
        public static SearchResults SearchResults => GetPage<SearchResults>();
        public static SeleniumHomePage SeleniumHomePage => GetPage<SeleniumHomePage>();
        public static SeleniumDownlaods SeleniumDownlaods => GetPage<SeleniumDownlaods>();
    }
}
