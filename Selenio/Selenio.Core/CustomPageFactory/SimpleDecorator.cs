using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Selenio.Core.CustomPageFactory
{
    internal class SimpleDecorator : IPageObjectMemberDecorator
    {
        public object Decorate(MemberInfo member, IElementLocator locator)
        {
            var finders = member.GetCustomAttributes<FindsByAttribute>();
            var bys = new List<By>();

            foreach (var finder in finders)
            {
                bys.Add(From(finder));
            }

            if (bys.Count > 0)
            {
                // I need to define my own Decorator interface to avoid having to cast in here.
                return ((ISimpleElementLocator)locator.LocateElement(bys, member.Name);
            }
            else
                return null;
        }

        private static By From(FindsByAttribute attribute)
        {
            var how = attribute.How;
            var usingValue = attribute.Using;
            switch (how)
            {
                case How.Id:
                    return By.Id(usingValue);
                case How.Name:
                    return By.Name(usingValue);
                case How.TagName:
                    return By.TagName(usingValue);
                case How.ClassName:
                    return By.ClassName(usingValue);
                case How.CssSelector:
                    return By.CssSelector(usingValue);
                case How.LinkText:
                    return By.LinkText(usingValue);
                case How.PartialLinkText:
                    return By.PartialLinkText(usingValue);
                case How.XPath:
                    return By.XPath(usingValue);
                case How.Custom:
                    if (attribute.CustomFinderType == null)
                    {
                        throw new ArgumentException("Cannot use How.Custom without supplying a custom finder type");
                    }

                    if (!attribute.CustomFinderType.IsSubclassOf(typeof(By)))
                    {
                        throw new ArgumentException("Custom finder type must be a descendent of the By class");
                    }

                    ConstructorInfo ctor = attribute.CustomFinderType.GetConstructor(new Type[] { typeof(string) });
                    if (ctor == null)
                    {
                        throw new ArgumentException("Custom finder type must expose a public constructor with a string argument");
                    }

                    By finder = ctor.Invoke(new object[] { usingValue }) as By;
                    return finder;
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Did not know how to construct How from how {0}, using {1}", how, usingValue));
        }

    }
}
