using System;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using Elan.Data.Models.Friends;
using Elan.Data.Models.Notifications;
using Elan.Data.Models.Posts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elan.Data
{
    public class ElanDbContext : IdentityDbContext<ElanUser, ElanRole, Guid>
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ElanUserSetting> ElanUserSettings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FriendsRelation> Friends { get; set; }
        public DbSet<FriendsInvitation> FriendsInvitations { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ElanDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ElanUserSetting>()
                .HasKey(c => new { c.UserId, c.Setting });

            var friendsInvitationBuilder = builder.Entity<FriendsInvitation>();

            friendsInvitationBuilder
                .HasKey(c => new { c.UserFromId, c.UserToId });

            friendsInvitationBuilder
                .HasOne(x => x.UserFrom)
                .WithMany(x => x.SentFriendInvitations)
                .OnDelete(DeleteBehavior.Restrict);

            friendsInvitationBuilder
                .HasOne(x => x.UserTo)
                .WithMany(x => x.ReceivedFriendInvitations)
                .OnDelete(DeleteBehavior.Restrict);

            var friendsRelationBuilder = builder.Entity<FriendsRelation>();

            friendsRelationBuilder
                .HasKey(c => new { c.FirstUserId, c.SecondUserId });

            friendsRelationBuilder
                .HasOne(x => x.FirstUser)
                .WithMany(x => x.FirstUserFriends)
                .OnDelete(DeleteBehavior.Restrict);

            friendsRelationBuilder
                .HasOne(x => x.SecondUser)
                .WithMany(x => x.SecondUserFriends)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
