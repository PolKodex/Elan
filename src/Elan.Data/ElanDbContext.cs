using System;
using Elan.Data.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elan.Data
{
    public class ElanDbContext : IdentityDbContext<ElanUser, ElanRole, Guid>
    {
        public ElanDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
