using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class WebsiteKeyPhrasesConfiguration : IEntityTypeConfiguration<WebsiteKeyPhrase>
{
    public void Configure(EntityTypeBuilder<WebsiteKeyPhrase> builder)
    {
        builder.HasKey(wkp => new { wkp.WebsiteUrl, wkp.KeyPhraseName });

        builder.HasOne(wkp => wkp.KeyPhrase)
            .WithMany(kp => kp.WebsitesKeyPhrases)
            .HasForeignKey(wkp => wkp.KeyPhraseName);

        builder.HasOne(wkp => wkp.Website)
            .WithMany(w => w.WebsiteKeyPhrases)
            .HasForeignKey(wkp => wkp.WebsiteUrl);
    }
}