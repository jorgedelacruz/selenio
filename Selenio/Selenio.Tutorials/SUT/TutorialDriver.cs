using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Selenio.Core.SUT;
using Selenio.HtmlReporter;
using Selenio.Tests.PageObjects.Google;
using Selenio.Tests.Reporting;

namespace Selenio.Tests
{
    public class TutorialDriver : SUTDriver
    {
        public static void Initialize<T>(TestContext context) where T : IWebDriver
        {
            var reporter = new Reporter(new ReportSettingsProvider(), context.DeploymentDirectory, context.FullyQualifiedTestClassName, context.TestName);
            InitDriver<T>(reporter);
            Driver.Manage().Window.Maximize();
        }

        public static HomeScreen GoogleHomeScreen => GetPage<HomeScreen>();
        public static SearchResults GoogleSearchResults => GetPage<SearchResults>();
        public static YouTube YouTubeHomeScreen => GetPage<YouTube>();
        public static GoogleSignIn GoogleSignIn => GetPage<GoogleSignIn>();

    }
}
