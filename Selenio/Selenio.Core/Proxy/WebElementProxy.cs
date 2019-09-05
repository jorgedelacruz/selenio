using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.Extensions;
using Selenio.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace Selenio.Core.Proxy
{
    /// <summary>
    /// Intercepts the request to a single <see cref="IWebElement"/>
    /// </summary>
    internal sealed class WebElementProxy : WebDriverObjectProxy, IWrapsElement
    {
        private IWebElement cachedElement;
        private IReporter reporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementProxy"/> class.
        /// </summary>
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that determines
        /// how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        private WebElementProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, string name, string containerId, IReporter reporter)
            : base(classToProxy, locator, bys, name, containerId)
        {
            this.reporter = reporter;
        }

        /// <summary>
        /// Gets the <see cref="IWebElement"/> wrapped by this object.
        /// </summary>
        public IWebElement WrappedElement
        {
            get { return this.Element; }
        }

        /// <summary>
        /// Gets the IWebElement object this proxy represents, returning a cached one if requested.
        /// </summary>
        private IWebElement Element
        {
            get
            {
                if (!Cache || cachedElement == null)
                {
                    cachedElement = string.IsNullOrEmpty(ContainerId) ?
                    Locator.LocateElement(Bys) :
                    Locator.SearchContext.FindElement(By.Id(ContainerId)).FindElement(Bys.First());

                    //this.Locator.LocateElement(this.Bys);
                }

                return cachedElement;
            }
        }

        /// <summary>
        /// Creates an object used to proxy calls to properties and methods of an <see cref="IWebElement"/> object.
        /// </summary>
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cacheLookups"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        /// <returns>An object used to proxy calls to properties and methods of the list of <see cref="IWebElement"/> objects.</returns>
        public static object CreateProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, string name, string containerId, IReporter reporter)
        {
            return new WebElementProxy(classToProxy, locator, bys, name, containerId, reporter).GetTransparentProxy();
        }

        /// <summary>
        /// Invokes the method that is specified in the provided <see cref="IMessage"/> on the
        /// object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">An <see cref="IMessage"/> that contains a dictionary of
        /// information about the method call. </param>
        /// <returns>The message returned by the invoked method, containing the return value and any
        /// out or ref parameters.</returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;

            var methodInfo = (methodCallMessage.MethodBase as MethodInfo);

            string methodName = methodInfo?.GetMethodName();
            string property = methodInfo?.GetPropertyName() ?? methodCallMessage.Args.SerializeArgumentValues();
            bool reportAction = reporter.Configuration.CanReportElememtAction(methodInfo.Name);

            return ExecuteAction(() =>
            {
                var returnValue = (typeof(IWrapsElement).IsAssignableFrom((methodCallMessage.MethodBase as MethodInfo).DeclaringType)) ?
                    new ReturnMessage(Element, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage) :
                    InvokeMethod(methodCallMessage, Element);

                string outcome = returnValue?.ReturnValue?.GetType().IsValueType() != null ? returnValue.ReturnValue.ToString() : "";

                if (methodName == WebElementMethod.SendKeys && Element.GetAttribute("type").ToLower() == "password")
                {
                    property = "●●●●●●●●●●●●●●";
                }

                if (reporter.Configuration.CanReportElememtAction(methodInfo.Name))
                    reporter.ReportElementAction(Name, methodName, property, outcome, true, "");

                return returnValue;
            });

            IMessage ExecuteAction(Func<IMessage> action)
            {
                switch (methodInfo.Name)
                {
                    case WebElementMethod.Displayed:
                        return GetDisplayed(action);
                    default:
                        return ExecuteGenericAction(action);
                }
            }

            IMessage SendKeys(Func<IMessage> code)
            {
                try
                {
                    return code.Invoke();
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(TargetInvocationException) && ex.InnerException != null)
                        ex = ex.InnerException;

                    if (reportAction)
                        reporter.ReportElementAction(Name, methodName, property, "", false, ex.Message);

                    throw;
                }
            }

            IMessage GetDisplayed(Func<IMessage> code)
            {
                try
                {
                    return code.Invoke();
                }
                catch
                {
                    if (reportAction)
                        reporter.ReportElementAction(Name, methodName, property, "False", true, "");
                    return new ReturnMessage(false, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
                }
            }

            IMessage ExecuteGenericAction(Func<IMessage> code)
            {
                try
                {
                    return code.Invoke();
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(TargetInvocationException) && ex.InnerException != null)
                        ex = ex.InnerException;

                    if (reportAction)
                        reporter.ReportElementAction(Name, methodName, property, "", false, ex.Message);

                    throw;
                }
            }
        }
    }
}
