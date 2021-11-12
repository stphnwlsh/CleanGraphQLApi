namespace CleanGraphQL.Presentation.Tests.Integration;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

internal class GraphQLApplication : WebApplicationFactory<Program>
{
    private readonly string environment;

    public GraphQLApplication(string environment = "local")
    {
        this.environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.UseEnvironment(this.environment);

        return base.CreateHost(builder);
    }
}
