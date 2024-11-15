using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;

namespace BCES.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ReportsControllerBase
    {
        public ReportController(IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        {
        }
    }
}
