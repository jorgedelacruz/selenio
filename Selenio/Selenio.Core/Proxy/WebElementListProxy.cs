using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Selenio.Core.Proxy
{
    /// <summary>
    /// Represents a proxy class for a list of elements to be used with the PageFactory.
    /// </summary>
    internal class WebElementListProxy : WebDriverObjectProxy
    {
        private List<IWebElement> collection = null;
        private IReporter reporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementListProxy"/> class.
        /// </summary>
        /// <param name="typeToBeProxied">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        private WebElementListProxy(Type typeToBeProxied, IElementLocator locator, IEnumerable<By> bys, string name, string containerId, IReporter reporter)
            : base(typeToBeProxied, locator, bys, name, containerId)
        {
            this.reporter = reporter;
        }

        /// <summary>
        /// Gets the list of IWebElement objects this proxy represents, returning a cached one if requested.
        /// </summary>
        private List<IWebElement> ElementList
        {
            get
            {
                if (!Cache || collection == null)
                {
                    collection = new List<IWebElement>();
                    collection.AddRange(string.IsNullOrEmpty(ContainerId) ?
                        Locator.LocateElements(Bys) :
                        Locator.SearchContext.FindElement(By.Id(ContainerId)).FindElements(Bys.First()));

                    //this.collection.AddRange(this.Locator.LocateElement(this.Bys));
                }

                return collection;
            }
        }

        /// <summary>
        /// Creates an object used to proxy calls to properties and methods of the
        /// list of <see cref="IWebElement"/> objects.
        /// </summary>
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cacheLookups"><see langword="true"/> to cache the lookup to the
        /// element; otherwise, <see langword="false"/>.</param>
        /// <returns>An object used to proxy calls to properties and methods of the
        /// list of <see cref="IWebElement"/> objects.</returns>
        public static object CreateProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, string name, string containerId, IReporter reporter)
        {
            return new WebElementListProxy(classToProxy, locator, bys, name, containerId, reporter).GetTransparentProxy();
        }

        /// <summary>
        /// Invokes the method that is specified in the provided <see cref="IMessage"/> on the
        /// object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">An <see cref="IMessage"/> that contains an <see cref="IDictionary"/>  of
        /// information about the method call. </param>
        /// <returns>The message returned by the invoked method, containing the return value and any
        /// out or ref parameters.</returns>
        public override IMessage Invoke(IMessage msg)
        {
            try
            {
                var elements = ElementList;
                return InvokeMethod(msg as IMethodCallMessage, elements);



                //var returnValue = invocation.Method.Invoke(locator.LocateElement(bys), invocation.Arguments);
                //invocation.ReturnValue = returnValue;

                //string outcome = returnValue?.GetType().IsValueType() != null ? returnValue.ToString() : "Passed";
                //reporter.ReportElementAction(Name, methodName, value, true, outcome);
            }
            catch (Exception ex)
            {
                //base.reporter.ReportElementAction(elementName, methodName, value, false, "Failed: " + ex.Message);
                throw;
            }
        }
    }
}
