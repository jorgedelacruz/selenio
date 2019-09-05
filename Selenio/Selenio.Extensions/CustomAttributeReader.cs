using OpenQA.Selenium;
using Selenio.Core.SUT;
using System;
using System.Linq;

namespace Selenio.Extensions
{
    public static class CustomAttributeReader
    {
        public static IWebElement GetTaggedElement<T>(PageObject page) where T : Attribute
        {
            var allElementsInPageObject = page.GetType().GetProperties(); //group all PO elements as Properties

            var match = allElementsInPageObject.FirstOrDefault(prop => prop.GetCustomAttributes(false).Any(attr => attr.GetType() == typeof(T)));

            return (IWebElement)match.GetValue(page); //return match
        }
    }
}
