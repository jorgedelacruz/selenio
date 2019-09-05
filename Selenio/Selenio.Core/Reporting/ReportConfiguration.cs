using System;
using System.Collections.ObjectModel;

namespace Selenio.Core.Reporting
{
    public class ReportConfiguration
    {
        private IReportSettingsProvider provider;

        public string ReportDropDirectory
        {
            get
            {
                return provider.ReportFilesDropDirectory;
            }
        }

        public ReadOnlyCollection<string> ReportablePageObjectActions { get; private set; }
        public ReadOnlyCollection<string> ReportableElementActions { get; private set; }

        public ReportConfiguration(IReportSettingsProvider configurationProvider)
        {
            provider = configurationProvider;

            ReportablePageObjectActions = provider.GetPageObjectMethodNames();
            ReportableElementActions = provider.GetWebElementMethodNames();
        }

        public void Reset()
        {
            ReportablePageObjectActions = provider.GetPageObjectMethodNames();
            ReportableElementActions = provider.GetWebElementMethodNames();
        }

        public void IncludeElementMethod(string methodName)
        {
            throw new NotImplementedException("IncludeElementMethod has not been implemented yet.");
        }

        public void IncludePageObjectMethod(string methodName)
        {
            throw new NotImplementedException("IncludePageObjectMethod has not been implemented yet.");
        }

        public bool CanReportElememtAction(string actionName)
        {
            return ReportableElementActions.Count > 0 ? ReportableElementActions.Contains(actionName) : true;
        }
    }
}
