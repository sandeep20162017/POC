using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace YourNamespace.Controllers
{
    public class CommonController : Controller
    {
        private readonly string _connectionString;

        public CommonController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult GetAutoCompleteData(string columnName, string searchText)
        {
            // Validate inputs to prevent SQL injection
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(searchText))
            {
                return BadRequest("Invalid parameters.");
            }

            // Combined mapping: Maps grid column names to SQL column names and table names
            var combinedMapping = new Dictionary<string, (string SqlColumnName, string TableName)>
            {
                { "TaskDescription", ("TaskDesc", "Tasks") }, // Grid column: TaskDescription, SQL column: TaskDesc, Table: Tasks
                { "OrigSupplierName", ("SupplierName", "Suppliers") },
                { "KeyWord", ("Keyword", "Keywords") },
                { "Description", ("PartDescription", "Descriptions") }
            };

            if (!combinedMapping.ContainsKey(columnName))
            {
                return BadRequest("Invalid column name.");
            }

            // Get the SQL column name and table name from the combined mapping
            var mapping = combinedMapping[columnName];
            var sqlColumnName = mapping.SqlColumnName;
            var tableName = mapping.TableName;

            // Example: Use Dapper to fetch data from the database
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Dynamically construct the SQL query
                var sql = $@"
                    SELECT DISTINCT {sqlColumnName} 
                    FROM {tableName} 
                    WHERE {sqlColumnName} LIKE @SearchText + '%'";

                var results = connection.Query<string>(sql, new { SearchText = searchText });
                return Json(results);
            }
        }
    }
}
