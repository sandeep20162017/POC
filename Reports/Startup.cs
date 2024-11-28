// Configure Telerik Reporting services
        services.Configure<ReportServiceConfiguration>(config =>
        {
            config.HostAppId = "BCESReports";
            config.Storage = new FileStorage("ReportsCache");

            // Configure UriReportSourceResolver for Telerik Reporting
            config.ReportSourceResolver = new UriReportSourceResolver(
                System.IO.Path.Combine(
                    System.AppContext.BaseDirectory, // Content root for the application
                    "Reports"
                )
            );
        });