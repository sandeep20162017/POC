public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();

    // Configure Telerik Reporting
    services.AddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
    {
        Storage = new Telerik.Reporting.Cache.File.FileStorage(),
        HostAppId = "BCESApp",
        ReportSharingTimeout = 60,
        MaxConcurrentReportExecutions = 2,

        // Set up the resolver for .trdp files
        ReportSourceResolver = new Telerik.Reporting.Services.Engine.ReportFileResolver(
            System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Reports"))
    });
}
