using CleanGraphQL.Presentation.Extensions;
using Serilog;

var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

try
{
    Log.Information("Starting host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
