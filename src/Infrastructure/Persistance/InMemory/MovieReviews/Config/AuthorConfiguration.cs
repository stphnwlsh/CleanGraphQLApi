namespace CleanGraphQL.Infrastructure.Persistance.InMemory.MovieReviews.Config;

using CleanGraphQL.Domain.Authors.Entities;
using CleanGraphQL.Infrastructure.Persistance.InMemory.Common.Config;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuthorConfiguration : EntityConfiguration<Author>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        _ = builder.HasMany(m => m.Reviews).WithOne(r => r.ReviewAuthor).HasForeignKey(r => r.ReviewAuthorId);
    }
}
