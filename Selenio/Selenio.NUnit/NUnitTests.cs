using NUnit.Framework;
using Selenio.Extensions;

namespace Selenio.NUnit
{
    [TestFixture]
    public class NUnitTests : TestClass
    {
        [Test]
        public void SeleniumHqTest()
        {
            Reporter.TestDescription = "Upload YouTube video";

            Reporter.TestStep = "Go to Bing";
            Driver.HomePage.Open();

            Reporter.TestStep = "Search for selenium";
            Driver.HomePage.Search("selenium");

            Reporter.TestStep = "Validate selenium docs is a result";
            Driver.SearchResults.WaitToLoad();
            Assert.IsTrue(Driver.SearchResults.SeleniumDocsResultLink.Displayed);

            Reporter.TestStep = "Open Selenium site";
            Driver.SearchResults.SeleniumDocsResultLink.Click();

            Reporter.TestStep = "Click Downloads";
            Driver.SeleniumHomePage.WaitForScreen();
            Driver.SeleniumHomePage.DownloadLink.Click();

            Reporter.TestStep = "Wait for Downloads page";
            Driver.SeleniumDownlaods.WaitForScreen();
        }
        
    }
}
