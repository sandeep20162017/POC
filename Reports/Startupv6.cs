var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
{
    Storage = new Telerik.Reporting.Cache.File.FileStorage(),
    HostAppId = "BCESApp",
    ReportSharingTimeout = 60,
    MaxConcurrentReportExecutions = 2,

    // Configure the ReportSourceResolver to locate .trdp files
    ReportSourceResolver = new Telerik.Reporting.Services.Engine.ReportFileResolver(
        Path.Combine(builder.Environment.ContentRootPath, "Reports"))
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
