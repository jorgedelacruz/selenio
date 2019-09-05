using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.PageObjects;
using Selenio.Core.Proxy;
using Selenio.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;

namespace Selenio.Core.CustomPageFactory
{
    internal class SimplePageObjectDecorator : IPageObjectMemberDecorator
    {
        private string containerId;

        public SimplePageObjectDecorator(IReporter reporter)
        {
            this.reporter = reporter;
            containerId = null;
        }

        public SimplePageObjectDecorator(IReporter reporter, string containerId)
        {
            this.reporter = reporter;
            this.containerId = containerId;
        }

        /// <summary>
        /// Locates an element or list of elements for a Page Object member, and returns a
        /// proxy object for the element or list of elements.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> containing information about
        /// a class's member.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> used to locate elements.</param>
        /// <returns>A transparent proxy to the WebDriver element object.</returns>
        public object Decorate(MemberInfo member, IElementLocator locator)
        {
            FieldInfo field = member as FieldInfo;
            PropertyInfo property = member as PropertyInfo;

            Type targetType = null;
            if (field != null)
            {
                targetType = field.FieldType;
            }

            bool hasPropertySet = false;
            if (property != null)
            {
                hasPropertySet = property.CanWrite;
                targetType = property.PropertyType;
            }

            if (field == null & (property == null || !hasPropertySet))
            {
                return null;
            }

            IList<By> bys = CreateLocatorList(member);

            if (bys.Count > 0)
            {
                return CreateProxyObject(targetType, member.Name, locator, bys, containerId, reporter);
            }

            return null;
        }

        /// <summary>
        /// Creates a list of <see cref="By"/> locators based on the attributes of this member.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> containing information about
        /// the member of the Page Object class.</param>
        /// <returns>A list of <see cref="By"/> locators based on the attributes of this member.</returns>
        protected static ReadOnlyCollection<By> CreateLocatorList(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member", "memeber cannot be null");
            }

            var useSequenceAttributes = Attribute.GetCustomAttributes(member, typeof(FindsBySequenceAttribute), true);
            bool useSequence = useSequenceAttributes.Length > 0;

            var useFindAllAttributes = Attribute.GetCustomAttributes(member, typeof(FindsByAllAttribute), true);
            bool useAll = useFindAllAttributes.Length > 0;

            if (useSequence && useAll)
            {
                throw new ArgumentException("Cannot specify FindsBySequence and FindsByAll on the same member");
            }

            List<By> bys = new List<By>();
            var attributes = Attribute.GetCustomAttributes(member, typeof(FindsByAttribute), true);
            if (attributes.Length > 0)
            {
                Array.Sort(attributes);
                foreach (var attribute in attributes)
                {
                    var castedAttribute = (FindsByAttribute)attribute;
                    if (castedAttribute.Using == null)
                    {
                        castedAttribute.Using = member.Name;
                    }

                    bys.Add(CreateByInstance(castedAttribute));
                }

                if (useSequence)
                {
                    ByChained chained = new ByChained(bys.ToArray());
                    bys.Clear();
                    bys.Add(chained);
                }

                if (useAll)
                {
                    ByAll all = new ByAll(bys.ToArray());
                    bys.Clear();
                    bys.Add(all);
                }
            }

            return bys.AsReadOnly();
        }

        private static object CreateProxyObject(Type memberType, string name, IElementLocator locator, IEnumerable<By> bys, string containerId, IReporter reporter)
        {
            object proxyObject = null;

            if (memberType == typeof(IWebElement))
            {
                proxyObject = WebElementProxy.CreateProxy(InterfaceProxyType, locator, bys, name, containerId, reporter);
            }
            else if (memberType == typeof(IList<IWebElement>))
            {
                proxyObject = WebElementListProxy.CreateProxy(memberType, locator, bys, name, containerId, reporter);
            }
            else
            {
                throw new ArgumentException("Type of member '" + memberType.Name + "' is not IWebElement or IList<IWebElement>.");
            }

            return proxyObject;
        }

        private static By CreateByInstance(FindsByAttribute attribute)
        {
            switch (attribute.How)
            {
                case How.Id:
                    return By.Id(attribute.Using);
                case How.Name:
                    return By.Name(attribute.Using);
                case How.TagName:
                    return By.TagName(attribute.Using);
                case How.ClassName:
                    return By.ClassName(attribute.Using);
                case How.CssSelector:
                    return By.CssSelector(attribute.Using);
                case How.LinkText:
                    return By.LinkText(attribute.Using);
                case How.PartialLinkText:
                    return By.PartialLinkText(attribute.Using);
                case How.XPath:
                    return By.XPath(attribute.Using);
                default:
                    throw new Exception("Invalid locator");
            }
        }

        private static List<Type> interfacesToBeProxied;
        private static Type interfaceProxyType;
        private IReporter reporter;

        private static List<Type> InterfacesToBeProxied
        {
            get
            {
                if (interfacesToBeProxied == null)
                {
                    interfacesToBeProxied = new List<Type>();
                    interfacesToBeProxied.Add(typeof(IWebElement));
                    interfacesToBeProxied.Add(typeof(ILocatable));
                    interfacesToBeProxied.Add(typeof(IWrapsElement));
                }

                return interfacesToBeProxied;
            }
        }

        private static Type InterfaceProxyType
        {
            get
            {
                if (interfaceProxyType == null)
                {
                    interfaceProxyType = CreateTypeForASingleElement();
                }

                return interfaceProxyType;
            }
        }

        private static Type CreateTypeForASingleElement()
        {
            AssemblyName tempAssemblyName = new AssemblyName(Guid.NewGuid().ToString());

            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = dynamicAssembly.DefineDynamicModule(tempAssemblyName.Name);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeof(IWebElement).FullName, TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);

            foreach (Type type in InterfacesToBeProxied)
            {
                typeBuilder.AddInterfaceImplementation(type);
            }

            return typeBuilder.CreateType();
        }

    }
}
