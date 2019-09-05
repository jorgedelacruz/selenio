using OpenQA.Selenium;
using System;
using System.Threading;

namespace Selenio.Extensions
{
    public static class WebDriverExtensions
    {
        public static void Wait(this IWebDriver driver, int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        public static void Wait(this IWebDriver driver, TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        //TODO: add extension to wait for Action (task) to complete.
    }
}
