using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MvcRefactorTest.SeleniumTest
{
    internal static class SeleniumGetMethods
    {
        /// <summary>
        ///     Get Text.
        /// </summary>
        /// <param name="element">By element.</param>
        /// <param name="elementType">By element type.</param>
        /// <returns>Returns the text of the element.</returns>
        public static string GetText(string element, PropertyType elementType)
        {
            switch (elementType)
            {
                case PropertyType.Id:
                    return PropertiesCollection.Driver.FindElement(By.Id(element)).GetAttribute("value");
                case PropertyType.Name:
                    return PropertiesCollection.Driver.FindElement(By.Name(element)).GetAttribute("value");
                default:
                    return string.Empty;
            }
        }

        public static string GetTextFromDropDownList(string element, PropertyType elementType)
        {
            switch (elementType)
            {
                case PropertyType.Id:
                    var First =
                        new SelectElement(PropertiesCollection.Driver.FindElement(By.Id(element))).AllSelectedOptions
                            .First();
                    if (First != null) return First.Text;
                    break;
                case PropertyType.Name:
                    var webElement =
                        new SelectElement(PropertiesCollection.Driver.FindElement(By.Name(element))).AllSelectedOptions
                            .First();
                    if (webElement != null) return webElement.Text;
                    break;
                default:
                    return string.Empty;
            }

            return string.Empty;
        }
    }
}