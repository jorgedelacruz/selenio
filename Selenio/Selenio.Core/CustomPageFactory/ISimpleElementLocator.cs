using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace Selenio.Core.CustomPageFactory
{
    internal interface ISimpleElementLocator : IElementLocator
    {
        IWebElement LocateElement(IEnumerable<By> bys, string elementName);

    }
}
