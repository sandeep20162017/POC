public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use session
    app.UseSession();

    // Use custom middleware to set session variables
    app.UseMiddleware<RoleSessionMiddleware>();

    // Other middleware
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}