using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    public List<Role> Roles { get; set; }
    public List<UserRoleViewModel> UserRoles { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Fetch roles from the database
            Roles = await FetchRolesFromDatabaseAsync();

            // Fetch user roles from the database
            UserRoles = await FetchUserRolesFromDatabaseAsync();
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error)
            // For simplicity, we'll just rethrow the exception
            throw new Exception("Error fetching data from the database.", ex);
        }
    }

    private async Task<List<Role>> FetchRolesFromDatabaseAsync()
    {
        // Simulate fetching roles from a database
        return new List<Role>
        {
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" },
            new Role { Id = 3, Name = "Guest" }
        };
    }

    private async Task<List<UserRoleViewModel>> FetchUserRolesFromDatabaseAsync()
    {
        // Simulate fetching user roles from a database
        return new List<UserRoleViewModel>
        {
            new UserRoleViewModel { User = "John Doe", Role = "Admin" },
            new UserRoleViewModel { User = "Jane Smith", Role = "User" }
        };
    }

    public async Task<IActionResult> OnGetReadAsync()
    {
        try
        {
            var userRoles = await FetchUserRolesFromDatabaseAsync();
            return new JsonResult(userRoles);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error)
            // For simplicity, we'll just return a server error
            return StatusCode(500, "Error fetching user roles from the database.");
        }
    }
}

public class UserRoleViewModel
{
    public string User { get; set; }
    public string Role { get; set; }
}