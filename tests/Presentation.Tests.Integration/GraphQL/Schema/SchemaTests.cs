namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Schema;

using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

public class SchemaTests
{
    private static readonly GraphQLApplication Application = new();

    [Fact]
    public async Task Schema_ShouldMatch_Snapshot()
    {
        // Arrange
        using var client = Application.CreateClient();
        var expected = await File.ReadAllTextAsync(Path.Combine("GraphQL", "Schema", "schema.graphql"));

        // Act
        using var response = await client.GetAsync("/graphql?sdl");
        var actual = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        Normalize(actual).ShouldBe(
            Normalize(expected),
            "The schema has changed. If the change is intended, copy the output of /graphql?sdl into tests/Presentation.Tests.Integration/GraphQL/Schema/schema.graphql.");
    }

    [Fact]
    public async Task Nitro_ShouldBe_Served()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/graphql/");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("text/html");
    }

    private static string Normalize(string schema)
    {
        return schema.Replace("\r\n", "\n").Trim();
    }
}
