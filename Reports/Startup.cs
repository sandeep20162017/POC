public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // Add Telerik Reporting service
    services.TryAddSingleton<IReportServiceConfiguration>(sp =>
        new ReportServiceConfiguration
        {
            ReportingEngineConfiguration = ConfigurationHelper.ResolveConfiguration(sp.GetService<IWebHostEnvironment>()),
            HostAppId = "YourAppId",
            Storage = new FileStorage(),
            ReportSourceResolver = new UriReportSourceResolver(System.IO.Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports"))
        });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}