namespace CleanGraphQLApi.Presentation.GraphQL.Authors.Mapping;

using AutoMapper;
using Application = Application.Authors.Entities;
using Presentation = Presentation.GraphQL.Authors.Types;

public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        _ = this.CreateMap<Presentation.Author, Application.Author>()
            .ReverseMap();
    }
}
