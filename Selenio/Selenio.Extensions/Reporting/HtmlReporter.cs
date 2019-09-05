using Selenio.Core.Reporting;

namespace Selenio.Extensions.Reporting
{
    public class HtmlReporter : IReporter
    {
        private ReportConfiguration configuration;

        public ReportConfiguration Configuration => configuration;

        public string TestStep { set => InternalHtmlReporter.TestStep = value; }

        public string TestDescription { set => InternalHtmlReporter.TestDescription = value; }

        public HtmlReporter(IReportSettingsProvider provider, string assemblyLocation, string methodName, string className)
        {
            configuration = new ReportConfiguration(provider);
            InternalHtmlReporter.Initialize(methodName, className, configuration.ReportDropDirectory, assemblyLocation);

            DebugLog("Test run log", true);
        }

        public void DebugLog(string message, bool sectionEnd = false)
        {
            InternalHtmlReporter.Log(message, sectionEnd);
        }

        public void StatusUpdate(string message, bool status)
        {
            InternalHtmlReporter.StatusUpdate(message, status);
        }

        public void ReportElementAction(string element, string method, string property, string value, bool status, string outcome)
        {
            InternalHtmlReporter.ReportAction(controlName: element, action: method, property: property, value: value, result: status, errorMessage: outcome);
        }

        public void ReportAssertion<T>(string element, string action, T expected, T actual, bool status, string outcome)
        {
            InternalHtmlReporter.ReportAssertion(action, expected, actual, status, outcome);
        }

        public void FinishTest(bool status, string message)
        {
            if (!status)
                StatusUpdate(message, false);
            InternalHtmlReporter.EndTest(status);
        }
    }
}
