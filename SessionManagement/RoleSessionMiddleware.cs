using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

public class RoleSessionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public RoleSessionMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the session variable is already set
        if (context.Session.GetString("RoleId") == null)
        {
            // Get the environment name
            var environmentName = _env.EnvironmentName;

            // Fetch the role from the database based on the environment name
            var roleId = GetRoleIdFromDatabase(environmentName);

            // Set the role ID in the session
            context.Session.SetString("RoleId", roleId.ToString());
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }

    private int GetRoleIdFromDatabase(string environmentName)
    {
        // Simulate fetching role ID from the database
        // Replace this with your actual database query logic
        switch (environmentName)
        {
            case "Development":
                return 1; // Role ID for Development environment
            case "Staging":
                return 2; // Role ID for Staging environment
            case "Production":
                return 3; // Role ID for Production environment
            default:
                return 0; // Default role ID
        }
    }
}