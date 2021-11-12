namespace CleanGraphQL.Application.Authors.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQL.Application.Common.Interfaces;
using CleanGraphQL.Domain.Authors.Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Author>>
{
    private readonly IAuthorsRepository repository;

    public ReadAllHandler(IAuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Author>> Handle(ReadAllQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadAllAuthors(cancellationToken);
    }
}
