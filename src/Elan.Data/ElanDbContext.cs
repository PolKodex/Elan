using System;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elan.Data
{
    public class ElanDbContext : IdentityDbContext<ElanUser, ElanRole, Guid>
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public ElanDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
