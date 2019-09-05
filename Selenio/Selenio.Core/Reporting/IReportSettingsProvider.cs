using System.Collections.ObjectModel;

namespace Selenio.Core.Reporting
{
    public interface IReportSettingsProvider
    {
        string ReportFilesDropDirectory { get; }

        ReadOnlyCollection<string> GetWebElementMethodNames();
        ReadOnlyCollection<string> GetPageObjectMethodNames();
    }
}
