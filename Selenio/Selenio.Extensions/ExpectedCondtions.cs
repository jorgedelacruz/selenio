using OpenQA.Selenium;
using System;

namespace Selenio.Extensions
{
    public class ExpectedCondtions
    {
        public static Func<IWebDriver, bool> ElementIsVisible(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    bool visible = element.Displayed;
                    return visible;
                }
                catch (Exception)
                {
                    return false;
                }
            };
        }

        public static Func<IWebDriver, bool> ElementIsNotVisible(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    return !element.Displayed;
                }
                catch (Exception)
                {
                    return false;
                }
            };
        }
    }
}
