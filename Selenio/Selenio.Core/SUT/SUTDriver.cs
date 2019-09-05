using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Selenio.Core.CustomPageFactory;
using Selenio.Core.Reporting;
using System;
using System.Configuration;

namespace Selenio.Core.SUT
{
    public class SUTDriver
    {
        public static IWebDriver Driver;
        public static IReporter Reporter { get; private set; }

        public static void InitDriver<T>(IReporter reporter) where T : IWebDriver
        {
            Driver = Activator.CreateInstance<T>();
            Reporter = reporter;
        }

        public static void InitDriver<T>(IReporter reporter, DriverOptions options) where T : IWebDriver
        {
            Driver = (T)Activator.CreateInstance(typeof(T), options);
            Reporter = reporter;
        }

        private static void EnsureDriver()
        {
            if (Driver == null)
                throw new Exception("Make sure you initialize the WebDriver using the 'InitDriver<T>()' method");
        }

        public static T GetPage<T>() where T : PageObject
        {
            EnsureDriver();

            var pageObjectInstance = Activator.CreateInstance<T>();
            pageObjectInstance.Driver = Driver;
            double waitTimeout = double.Parse(ConfigurationManager.AppSettings["WaitTime"]?.ToString() ?? "10");
            pageObjectInstance.Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTimeout));

            //PageFactory.InitElements(Driver, instance);
            PageFactory.InitElements(Driver, pageObjectInstance, new SimplePageObjectDecorator(Reporter));

            return pageObjectInstance;
        }
    }
}
