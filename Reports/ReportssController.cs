using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

[Route("api/reports")]
[ApiController]
public class ReportsController : ReportsControllerBase
{
    public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
        : base(reportServiceConfiguration)
    {
    }

    protected override CatalogItem ResolveReport(string report)
    {
        // Load the report definition from the file system or any other source
        var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
        var reportSource = new Telerik.Reporting.UriReportSource { Uri = report + ".trdp" };
        return new CatalogItem { Name = report, Type = "report", Path = report };
    }
}