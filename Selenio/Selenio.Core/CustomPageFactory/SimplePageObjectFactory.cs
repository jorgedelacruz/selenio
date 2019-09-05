using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Selenio.Core.Reporting;
using Selenio.Core.SUT;
using System;
using System.Configuration;

namespace Selenio.Core.CustomPageFactory
{
    public class SimplePageObjectFactory
    {
        public static string DefaultWaitTime = "5";

        /// <summary>
        /// Creates a new Page Object that can be intercepted through dynamic proxies.
        /// </summary>
        public static T1 CreatePageObject<T1>(IWebDriver driver, IReporter reporter, string containerId) where T1 : PageObject
        {
            var instance = Activator.CreateInstance<T1>();

            instance.Driver = driver;

            if (containerId != null)
                PageFactory.InitElements(driver, instance, new SimplePageObjectDecorator(containerId));
            else
                PageFactory.InitElements(driver, instance, new SimplePageObjectDecorator());

            // Default wait time is 5 seconds.
            instance.Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Int32.Parse(ConfigurationManager.AppSettings["WaitLimit"] ?? DefaultWaitTime)));

            // TODO: Need to use "Exception" as a generic whitelisted exception type.
            instance.Wait.IgnoreExceptionTypes(new[] { typeof(NullReferenceException), typeof(NoSuchElementException) });

            return instance;
        }
    }
}
