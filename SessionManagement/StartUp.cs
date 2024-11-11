public void ConfigureServices(IServiceCollection services)
{
    // Add session services
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    // Add IHttpContextAccessor
    services.AddHttpContextAccessor();

    // Add other services
    services.AddControllersWithViews();
}