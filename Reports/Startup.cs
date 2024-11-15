using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

var builder = WebApplication.CreateBuilder(args);

// Register Telerik Reporting services
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IReportSourceResolver>(sp =>
    new ReportFileResolver(System.IO.Path.Combine(builder.Environment.ContentRootPath, "Reports"))
        .AddFallbackResolver());
builder.Services.AddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
{
    Storage = new FileStorage(),
    ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>(),
    HostAppId = "MyApp",
    ReportSharingTimeout = 60,
    MaxConcurrentReportExecutions = 2
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
