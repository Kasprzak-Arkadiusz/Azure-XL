using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<KeyPhrase> KeyPhrases { get; set; }
    DbSet<WebsiteKeyPhrase> WebsiteKeyPhrases { get; set; }
    DbSet<Website> Websites { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}