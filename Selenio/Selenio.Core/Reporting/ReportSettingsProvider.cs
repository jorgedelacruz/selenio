using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Selenio.Core.Reporting
{
    public class ReportSettingsProvider : IReportSettingsProvider
    {
        public string ReportFilesDropDirectory { get => ""; }

        public ReportSettingsProvider()
        {

        }

        public ReadOnlyCollection<string> GetPageObjectMethodNames()
        {
            return new ReadOnlyCollection<string>(
                new List<string>()
                {

                });
        }

        public ReadOnlyCollection<string> GetWebElementMethodNames()
        {
            return new ReadOnlyCollection<string>(
                new List<string>()
                {
                    "SendKeys",
                    "Click",
                    "get_Enabled",
                    "Submit",
                    "get_Text"
                });
        }
    }
}
