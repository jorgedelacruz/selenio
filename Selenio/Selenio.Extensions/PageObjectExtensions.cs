using OpenQA.Selenium;
using Selenio.Core.SUT;
using System;

namespace Selenio.Extensions
{
    public static class PageObjectExtensions
    {
        public static void WaitForScreen(this PageObject page, IWebElement element = null)
        {
            try
            {
                page.WaitForElement(element ?? CustomAttributeReader.GetTaggedElement<WaitForThisElementAttribute>(page));
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException($"There's no default element to wait for in page object {page.GetType().Name}. Either set a default element to wait for, or pass an element to the WaitForScreen method.");
            }
        }

        public static void WaitForElement(this PageObject page, IWebElement element, bool waitVisible = true)
        {
            if (waitVisible) page.Wait.Until(ExpectedCondtions.ElementIsVisible(element));
            else page.Wait.Until(ExpectedCondtions.ElementIsNotVisible(element));
        }
    }
}
