namespace CleanGraphQLApi.Presentation.Tests.Integration;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class Serializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    public static string Serialize(this object input)
    {
        return JsonSerializer.Serialize(input, Options);
    }

    public static T? Deserialize<T>(this string input)
    {
        return JsonSerializer.Deserialize<T>(input, Options);
    }

    public static StringContent ToStringContent(this object input)
    {
        return new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
    }
}
