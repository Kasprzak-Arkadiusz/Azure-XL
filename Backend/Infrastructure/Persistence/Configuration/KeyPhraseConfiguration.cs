using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class KeyPhraseConfiguration : IEntityTypeConfiguration<KeyPhrase>
{
    public void Configure(EntityTypeBuilder<KeyPhrase> builder)
    {
        builder.HasKey(kp => kp.Name);

        builder.Property(kp => kp.Name)
            .HasMaxLength(40);
    }
}