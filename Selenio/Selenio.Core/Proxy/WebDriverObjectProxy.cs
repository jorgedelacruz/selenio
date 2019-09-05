using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Selenio.Core.Proxy
{
    /// <summary>
    /// Represents a base proxy class for objects used with the PageFactory.
    /// </summary>
    internal abstract class WebDriverObjectProxy : RealProxy
    {
        private readonly IElementLocator locator;
        private readonly IEnumerable<By> bys;
        private readonly bool cache;
        private readonly string name;
        private readonly string containerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverObjectProxy"/> class.
        /// </summary>
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        protected WebDriverObjectProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, string name, string containerId)
            : base(classToProxy)
        {
            this.locator = locator;
            this.bys = bys;
            this.name = name;
            this.containerId = containerId;
            cache = false;
        }

        /// <summary>
        /// Gets the <see cref="IElementLocator"/> implementation that determines how elements are located.
        /// </summary>
        protected IElementLocator Locator
        {
            get { return locator; }
        }

        /// <summary>
        /// Gets the list of methods by which to search for the elements.
        /// </summary>
        protected IEnumerable<By> Bys
        {
            get { return bys; }
        }

        /// <summary>
        /// Gets a value indicating whether element search results should be cached.
        /// </summary>
        protected bool Cache
        {
            get { return this.cache; }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string ContainerId
        {
            get
            {
                return containerId;
            }
        }

        /// <summary>
        /// Invokes a method on the object this proxy represents.
        /// </summary>
        /// <param name="msg">Message containing the parameters of the method being invoked.</param>
        /// <param name="representedValue">The object this proxy represents.</param>
        /// <returns>The <see cref="ReturnMessage"/> instance as a result of method invocation on the
        /// object which this proxy represents.</returns>
        protected static ReturnMessage InvokeMethod(IMethodCallMessage msg, object representedValue)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg", "The message containing invocation information cannot be null");
            }

            MethodInfo proxiedMethod = msg.MethodBase as MethodInfo;
            return new ReturnMessage(proxiedMethod.Invoke(representedValue, msg.Args), null, 0, msg.LogicalCallContext, msg);
        }
    }
}
