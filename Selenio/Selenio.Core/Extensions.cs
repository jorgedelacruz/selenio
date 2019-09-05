using System;
using System.Linq;
using System.Reflection;

namespace Selenio.Core.Extensions
{
    internal static class Extensions
    {
        public static bool IsProperty(this MethodInfo method)
        {
            var splitName = method.Name.Split('_');

            return splitName.Count() == 2 && (splitName[0].ToLower() == "get" || splitName[0].ToLower() == "set");
        }

        public static string GetPropertyName(this MethodInfo method)
        {
            return method.IsProperty() ? method.Name.Split('_')[1] : null;
        }

        public static string GetMethodName(this MethodInfo method)
        {
            return method.IsProperty() ? FirstCharToUpper(method.Name.Split('_')[0]) : method.Name;
        }

        public static string SerializeArgumentValues(this object[] arguments)
        {
            try
            {
                var strings = arguments.Where(a => a.GetType().IsValueType()).Select(a => a.ToString());
                string values = string.Join(", ", strings);

                return values;
            }
            catch
            {
                return "";
            }
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}
