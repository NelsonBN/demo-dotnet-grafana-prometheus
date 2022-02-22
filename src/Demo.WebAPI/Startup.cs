using System;
using Demo.WebAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.DotNetRuntime;

namespace Demo.WebAPI;

public class Startup
{
    public Startup()
    {
        var collector = DotNetRuntimeStatsBuilder
            .Customize()
            .WithContentionStats(CaptureLevel.Informational)
            .WithGcStats(CaptureLevel.Verbose)
            .WithThreadPoolStats(CaptureLevel.Informational)
            .WithExceptionStats(CaptureLevel.Errors)
            .WithJitStats();

        collector.RecycleCollectorsEvery(TimeSpan.FromMinutes(20));

        collector.StartCollecting();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();

        services.AddSingleton<IProductRepository, ProductRepository>();

        services.AddHealthChecks()
                .ForwardToPrometheus();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseHttpMetrics(); // Needs to be added after `app.UseRouting();`
        app.UseMetricServer();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Mapping all controller
            endpoints.MapMetrics();
        });
    }
}
