using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RoleModel : PageModel
{
    public List<Role> Roles { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Fetch roles from the database
            Roles = await FetchRolesFromDatabaseAsync();
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error)
            // For simplicity, we'll just rethrow the exception
            throw new Exception("Error fetching roles from the database.", ex);
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
}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
}