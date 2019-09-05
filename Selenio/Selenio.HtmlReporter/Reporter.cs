using Selenio.Core.Reporting;
using System;

namespace Selenio.HtmlReporter
{
    public class Reporter : IReporter
    {
        private ReportConfiguration configuration;

        public ReportConfiguration Configuration => configuration;

        public string TestStep { set => InternalHtmlReporter.TestStep = value; }

        public string TestDescription { set => InternalHtmlReporter.TestDescription = value; }

        public Reporter(IReportSettingsProvider provider, string assemblyLocation, string className, string methodName)
        {
            configuration = new ReportConfiguration(provider);
            InternalHtmlReporter.Initialize(methodName, className, configuration.ReportDropDirectory, assemblyLocation);

            DebugLog($"Test run log for {methodName}", true);
        }

        public void DebugLog(string message, bool newSection = false, bool sectionEnd = false)
        {
            InternalHtmlReporter.Log(message, newSection, sectionEnd);
        }

        public void StatusUpdate(string message, bool status)
        {
            InternalHtmlReporter.StatusUpdate(message, status);
        }

        public void ReportElementAction(string element, string method, string property, string value, bool status, string errorMessage)
        {
            InternalHtmlReporter.ReportAction(controlName: element, action: method, property: property, value: value, result: status, errorMessage: errorMessage);
        }

        public void ReportAssertion<T>(string element, string action, T expected, T actual, bool status, string outcome)
        {
            InternalHtmlReporter.ReportAssertion(action, expected, actual, status, outcome);
        }

        public void FinishTest(bool status, string errorMessage)
        {
            if (!status)
                StatusUpdate(errorMessage, false);
            
            var outcome = InternalHtmlReporter.EndTest(status);
            if (!outcome.Passed)
                throw new Exception(outcome.Message);
        }

        public void FinishTest(bool status, Exception exception)
        {
            if (!status)
                StatusUpdate(exception.Message, false);

            var outcome = InternalHtmlReporter.EndTest(status);
            if (!outcome.Passed)
                throw new Exception(outcome.Message);

        }
    }
}
