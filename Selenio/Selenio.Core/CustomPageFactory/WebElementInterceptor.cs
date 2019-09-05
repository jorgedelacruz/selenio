using Castle.DynamicProxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.Reporting;
using System;
using System.Collections.Generic;

namespace Selenio.Core.Interceptors
{
    [Serializable]
    internal class WebElementInterceptor : IInterceptor
    {
        private IElementLocator locator;
        private IEnumerable<By> bys;

        private IReporter reporter;
        private string elementName;

        public WebElementInterceptor(IElementLocator locator, IEnumerable<By> bys, IReporter reporter, string name)
        {
            this.locator = locator;
            this.bys = bys;
            this.reporter = reporter;
            elementName = name;
        }

        public void Intercept(IInvocation invocation)
        {
            string value = invocation.Arguments.SerializeArgumentValues();
            string methodName = invocation.Method.GetMethodName();

            try
            {
                var returnValue = invocation.Method.Invoke(locator.LocateElement(bys), invocation.Arguments);
                invocation.ReturnValue = returnValue;

                string outcome = returnValue?.GetType().IsValueType() != null ? returnValue.ToString() : "Passed";
                reporter.ReportElementAction(elementName, methodName, value, true, outcome);
            }
            catch (Exception ex)
            {
                reporter.ReportElementAction(elementName, methodName, value, false, "Failed: " + ex.Message);
                throw;
            }
        }
    }
}
