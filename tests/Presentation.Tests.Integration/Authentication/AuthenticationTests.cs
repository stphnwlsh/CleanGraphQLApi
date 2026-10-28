namespace CleanGraphQLApi.Presentation.Tests.Integration.Authentication;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using GraphQL.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Shouldly;
using Xunit;

public class AuthenticationTests : IDisposable
{
    private const string Authority = "https://login.example.com";
    private const string Audience = "https://api.example.com";

    private static readonly SymmetricSecurityKey SigningKey = new(Encoding.UTF8.GetBytes("a-test-signing-key-that-is-long-enough-for-hs256"));

    private readonly GraphQLApplication application = new(
        settings: new Dictionary<string, string?>
        {
            ["Authentication:Authority"] = Authority,
            ["Authentication:Audience"] = Audience
        },
        configureServices: services => services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Configuration = new OpenIdConnectConfiguration { Issuer = Authority };
            options.TokenValidationParameters.IssuerSigningKey = SigningKey;
            options.TokenValidationParameters.ValidIssuer = Authority;
        }));

    [Theory]
    [InlineData("{movies{items{id}}}")]
    [InlineData("{authors{items{id}}}")]
    [InlineData("{reviews{items{id}}}")]
    public async Task Query_WithoutToken_ShouldReturn_NotAuthenticated(string query)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        var result = await Send(client, query);

        // Assert
        new object?[] { result.Data?.Movies, result.Data?.Authors, result.Data?.Reviews }.ShouldAllBe(field => field == null);
        result.Errors.ShouldNotBeEmpty();
        result.Errors[0].Extensions.Code.ShouldBe("AUTH_NOT_AUTHENTICATED");
    }

    [Fact]
    public async Task Mutation_WithoutToken_ShouldReturn_NotAuthenticated()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        var result = await Send(client, $"mutation{{deleteReview(id:\"{Guid.NewGuid()}\")}}");

        // Assert
        result.Errors.ShouldNotBeEmpty();
        result.Errors[0].Extensions.Code.ShouldBe("AUTH_NOT_AUTHENTICATED");
    }

    [Fact]
    public async Task Query_WithWrongAudience_ShouldReturn_NotAuthenticated()
    {
        // Arrange
        using var client = this.application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken("https://someone-else.example.com"));

        // Act
        var result = await Send(client, "{movies{items{id}}}");

        // Assert
        result.Errors.ShouldNotBeEmpty();
        result.Errors[0].Extensions.Code.ShouldBe("AUTH_NOT_AUTHENTICATED");
    }

    [Fact]
    public async Task Query_WithToken_ShouldReturn_Data()
    {
        // Arrange
        using var client = this.application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(Audience));

        // Act
        var result = await Send(client, "{movies{items{id}}}");

        // Assert
        result.Errors.ShouldBeNull();
        result.Data.Movies.Items.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Version_WithoutToken_ShouldReturn_Version()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        var result = await Send(client, "{version{fileVersion}}");

        // Assert
        result.Errors.ShouldBeNull();
        result.Data.Version.FileVersion.ShouldNotBeNullOrWhiteSpace();
    }

    public void Dispose()
    {
        this.application.Dispose();
        GC.SuppressFinalize(this);
    }

    private static async Task<GraphData> Send(HttpClient client, string query)
    {
        using var response = await client.PostAsync("/graphql", new { query }.ToStringContent());

        return (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();
    }

    private static string CreateToken(string audience)
    {
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = Authority,
            Audience = audience,
            Subject = new ClaimsIdentity([new Claim("sub", "test-user")]),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)
        });
    }
}
