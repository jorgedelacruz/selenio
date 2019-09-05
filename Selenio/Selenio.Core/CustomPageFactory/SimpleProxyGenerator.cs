using Castle.DynamicProxy;

namespace Selenio.Core.CustomPageFactory
{
    internal class SimpleProxyGenerator
    {
        private static ProxyGenerator generator;
        public static ProxyGenerator Generator
        {
            get
            {
                if (generator == null)
                {
                    generator = new ProxyGenerator();
                }
                return generator;
            }
        }
    }
}
