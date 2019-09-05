using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.Interceptors;
using Selenio.Core.Reporting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Selenio.Core.CustomPageFactory
{
    internal class InterceptableElementsLocator : ISimpleElementLocator
    {
        private ISearchContext context;
        private IReporter reporter;
        private IElementLocator locator;
        private string elementName;

        public ISearchContext SearchContext
        {
            get
            {
                return context;
            }
        }

        public InterceptableElementsLocator(ISearchContext context, IReporter WebElementReporter)
        {
            this.context = context;
            reporter = WebElementReporter;
            locator = new DefaultElementLocator(context);
        }

        public IWebElement LocateElement(IEnumerable<By> bys, string elementName)
        {
            this.elementName = elementName;
            return LocateElement(bys);
        }

        public IWebElement LocateElement(IEnumerable<By> bys)
        {
            return SimpleProxyGenerator.Generator.CreateInterfaceProxyWithoutTarget<IWebElement>(new WebElementInterceptor(locator, bys, reporter, elementName));
        }

        public ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys, string elementName)
        {
            this.elementName = elementName;
            return LocateElements(bys);
        }

        public ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys)
        {
            List<IWebElement> proxiedElements = new List<IWebElement>
            {
                SimpleProxyGenerator.Generator.CreateInterfaceProxyWithoutTarget<IWebElement>(new WebElementInterceptor(locator, bys, reporter, elementName))
            };
            return new ReadOnlyCollection<IWebElement>(proxiedElements);
        }
    }
}
