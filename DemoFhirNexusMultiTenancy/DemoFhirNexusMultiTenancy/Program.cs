// -------------------------------------------------------------------------------------------------
// Copyright (c) Integrated Health Information Systems Pte Ltd. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Ihis.FhirEngine.Core.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Reads the 'FhirEngineSerilog' configuration section to add and configure Serilog for logging audit and application logs
builder.AddFhirEngineSerilogAuditLogging();

// Reads the 'FhirEngine' configuration section to add services
builder.AddFhirEngineServer();

var app = builder.Build();

app.UseHsts()
    .UseStaticFiles() // for serving rapidoc index.html
    .UseRouting()
    .UseFhirEngineMiddlewares()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapFhirEngineHealthChecks("/health");
        endpoints.MapFhirEngine(endpoints.ServiceProvider.GetService<IOptions<FhirEngineOptions>>().Value?.PathFormat);
    });

await app.RunAsync();
