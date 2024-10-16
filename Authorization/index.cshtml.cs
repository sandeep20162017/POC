using BCES.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCES.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DbContext _dbContext;

        public IndexModel(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Vehicle> Vehicles { get; set; }
        public string UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get the username from the machine the user is logged into
                string username = Environment.UserName;

                // Get the user's role
                UserRole = await GetUserRoleAsync(username);

                // Retrieve vehicles based on the user's role
                Vehicles = await _dbContext.Set<Vehicle>()
                    .FromSqlInterpolated($"SELECT * FROM BCES.Vehicles")
                    .ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception
                return RedirectToPage("/Error");
            }
        }

        private async Task<string> GetUserRoleAsync(string username)
        {
            try
            {
                var userRole = await _dbContext.Set<UserRole>()
                    .FromSqlInterpolated($"SELECT * FROM BCES.UserRoles WHERE UserId = (SELECT UserId FROM BCES.Users WHERE Username = {username})")
                    .Join(_dbContext.Set<Role>(), ur => ur.RoleId, r => r.RoleId, (ur, r) => r.RoleName)
                    .FirstOrDefaultAsync();

                return userRole;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving user role", ex);
            }
        }
    }
}