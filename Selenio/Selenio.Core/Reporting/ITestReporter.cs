using System;

namespace Selenio.Core.Reporting
{
    public interface IReporter
    {
        ReportConfiguration Configuration { get; }

        string TestStep { set; }

        string TestDescription { set; }

        void StatusUpdate(string message, bool status);

        void DebugLog(string message, bool newSection = false, bool sectionEnd = false);

        void ReportElementAction(string element, string method, string property, string value, bool status, string errorMessage);

        void ReportAssertion<T>(string element, string action, T expectedValue, T actualValue, bool status, string outcome);

        void FinishTest(bool status, string errorMessage);

        void FinishTest(bool status, Exception exception);
    }
}
