using System;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using Elan.Data.Models.Posts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elan.Data
{
    public class ElanDbContext : IdentityDbContext<ElanUser, ElanRole, Guid>
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ElanUserSetting> ElanUserSettings { get; set; }
        public DbSet<Post> Posts{ get; set; }

        public ElanDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ElanUserSetting>()
                .HasKey(c => new { c.UserId, c.Setting });

            base.OnModelCreating(builder);
        }
    }
}
