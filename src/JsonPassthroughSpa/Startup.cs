namespace JsonPassthroughSpa
{
    using System;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Threading;
    using JsonPassthroughSpa.Extensions;
    using JsonPassthroughSpa.Interfaces;
    using JsonPassthroughSpa.Services;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.SqlServer.Dac;
    using Serilog;

    public class Startup
    {
        private const string DacpacEmbeddedResourceName = "JsonPassthroughSpa.dacpac";

        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddSerilog());
            services.AddSingleton<IFileProvider>(new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly()));
            services.Configure<ConnectionStringsOptions>(this.Configuration.GetSection(ConnectionStringsOptions.SectionName));
            services.AddSingleton<IDbService, DbService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<SerilogLoggingActionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IFileProvider fileProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            try
            {
                using var stream = fileProvider.GetFileInfo(DacpacEmbeddedResourceName).CreateReadStream();
                var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = connectionStringBuilder.InitialCatalog;
                var dacServices = new DacServices(connectionString);
                using var dacPackage = DacPackage.Load(stream);
                dacServices.Deploy(dacPackage, targetDatabaseName: databaseName, upgradeExisting: true, options: null, cancellationToken: CancellationToken.None);
            }
            catch (DacServicesException exception)
            {
                logger.LogError(exception, exception.Message);
            }

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSerilogRequestLogging(options => { options.EnrichDiagnosticContext = EnrichDiagnosticContext; });
            app.UseRequestResponseLogging();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static void EnrichDiagnosticContext(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();

            if (endpoint is object)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }
    }
}
