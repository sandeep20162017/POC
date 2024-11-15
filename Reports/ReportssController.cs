using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;

namespace BCES.Controllers.Reports
{
    public class ReportsController : BaseController
    {
        private readonly IReportServiceConfiguration _reportServiceConfiguration;

        public ReportsController(
            DapperContext dapper, 
            IHttpContextAccessor httpContextAccessor, 
            IReportServiceConfiguration reportServiceConfiguration)
            : base(dapper, httpContextAccessor)
        {
            _reportServiceConfiguration = reportServiceConfiguration;
        }

        [Route("Reports/{reportName}")]
        public IActionResult ReportViewer(string reportName)
        {
            // Pass the report name to the view
            ViewData["ReportName"] = reportName;
            return View();
        }
    }
}
