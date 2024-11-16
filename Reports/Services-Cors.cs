var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Telerik Reporting service
builder.Services.AddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
{
    Storage = new Telerik.Reporting.Cache.File.FileStorage(),
    HostAppId = "BCESApp",
    ReportSharingTimeout = 60,
    MaxConcurrentReportExecutions = 2
});

var app = builder.Build();

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

// Apply CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
