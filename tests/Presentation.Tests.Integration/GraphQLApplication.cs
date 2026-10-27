namespace CleanGraphQLApi.Presentation.Tests.Integration;

using System.Net.Http;
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

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        // GraphQL.NET blocks GET requests without this header to protect against CSRF.
        client.DefaultRequestHeaders.Add("GraphQL-Require-Preflight", "true");
    }
}
