using Selenio.Core.Reporting;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Selenio.PlainTextReporter
{
    public class Reporter : IReporter
    {
        private string debugFile;
        bool descriptionAlreadySet = false;
        private ReportConfiguration configuration;

        public ReportConfiguration Configuration => configuration;

        public string TestStep
        {
            set
            {
                if (InternalReporter.PreviousTestStep != value)
                {
                    InternalReporter.PreviousTestStep = value;
                    DebugLog(value, true);
                }
            }
        }

        public string TestDescription
        {
            set
            {
                if (!descriptionAlreadySet)
                {
                    descriptionAlreadySet = true;
                    DebugLog("TEST DESCRIPTION: " + value, true, true);
                }
            }
        }

        public Reporter(IReportSettingsProvider provider, string className, string methodName)
        {
            configuration = new ReportConfiguration(provider);

            if (className != InternalReporter.PreviousClassName)
            {
                InternalReporter.PreviousClassName = className;
                InternalReporter.PreviousTimeStamp = Regex.Replace(DateTime.Now.ToString(), @"[^0-9a-zA-Z]+", "");
            }

            string directory = $@"{provider.ReportFilesDropDirectory}\{className}.{InternalReporter.PreviousTimeStamp}";
            Directory.CreateDirectory(directory);
            debugFile = $@"{directory}\{methodName}.txt";
            File.AppendAllText(debugFile, "Test run log" + Environment.NewLine + Environment.NewLine);
        }

        public void ReportAssertion<T>(string element, string action, T expected, T actual, bool status, string outcome)
        {
            throw new NotImplementedException();
        }

        public void StatusUpdate(string message, bool status)
        {
            DebugLog($"[{message}] [{status}]", false, false);
        }

        public void DebugLog(string message, bool sectionStart = false, bool sectionEnd = false)
        {
            string lineStart = sectionStart ? "-----------------------" + Environment.NewLine : "";
            string lineEnd = Environment.NewLine + (sectionEnd ? "-----------------------" + Environment.NewLine : "");
            File.AppendAllText(debugFile, $"{lineStart}{DateTime.Now.ToString()}: {message}{lineEnd}");
        }

        public void ReportElementAction(string element, string method, string property, string value, bool status, string errorMessage)
        {
            DebugLog($"{(status ? "Pass" : "Fail")} - {element} {method} '{property}' Value: '{value}' Error: '{errorMessage}'", false, false);
        }

        public void FinishTest(bool status, Exception exception)
        {
            if (status)
            {
                DebugLog("Test finished.", true, true);
            }
            else
            {
                DebugLog("Test Failed: " + exception.Message, true, true);
                throw exception;
            }
        }

        public void FinishTest(bool status, string errorMessage)
        {
            if (status)
            {
                DebugLog("Test finished.", true, true);
            }
            else
            {
                DebugLog("Test Failed: " + errorMessage, true, true);
                throw new Exception(errorMessage);
            }
        }
    }
}
