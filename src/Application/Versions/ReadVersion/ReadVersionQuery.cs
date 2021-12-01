namespace CleanGraphQLApi.Application.Versions.ReadVersion;

using CleanGraphQLApi.Application.Entities;
using MediatR;

public class ReadVersionQuery : IRequest<ApplicationVersion>
{
}
