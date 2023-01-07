using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class WebsiteConfiguration : IEntityTypeConfiguration<Website>
{
    public void Configure(EntityTypeBuilder<Website> builder)
    {
        builder.HasKey(w => w.Url);

        builder.Property(w => w.Url)
            .HasMaxLength(400);

        builder.Property(w => w.Title)
            .HasMaxLength(200);

        builder.Property(w => w.Image)
            .IsRequired(false);
    }
}