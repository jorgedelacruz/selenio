using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenio.Extensions
{
    public static class WebElementExtensions
    {
        public static void SelectByText(this IWebElement element, string optionToSelect)
        {
            if (string.IsNullOrEmpty(optionToSelect))
                return;

            new SelectElement(element).SelectByText(optionToSelect);
        }

        public static string GetSelectedText(this IWebElement element)
        {
            return new SelectElement(element).SelectedOption.Text;
        }

        public static void SetText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        public static void Toggle(this IWebElement element, bool state)
        {
            if ((element.Selected && !state) || (!element.Selected && state))
                element.SendKeys(Keys.Space);
        }

    }
}
