namespace CleanGraphQLApi.Application.Versions.ReadVersion;

using CleanGraphQLApi.Domain.Version;
using MediatR;

public class ReadVersionQuery : IRequest<ApplicationVersion>
{
}
