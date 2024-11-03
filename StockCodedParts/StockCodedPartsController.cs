using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNamespace.Data;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class StockCodedPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockCodedPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetStockCodedParts([DataSourceRequest] DataSourceRequest request)
        {
            var stockCodedParts = await _context.StockCodedParts.ToListAsync();
            return Json(stockCodedParts.ToDataSourceResult(request));
        }
    }
}