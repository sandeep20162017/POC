using Microsoft.AspNetCore.Mvc;

namespace BCES.Controllers.Reports
{
    public class ReportsController : BaseController
    {
        public ReportsController(DapperContext dapper, IHttpContextAccessor httpContextAccessor)
            : base(dapper, httpContextAccessor)
        {
        }

        [Route("Reports/{reportName}")]
        public IActionResult ReportViewer(string reportName)
        {
            // Pass the report name to the view
            ViewData["ReportName"] = reportName;
            return View("DynamicReportViewer");
        }
    }
}
