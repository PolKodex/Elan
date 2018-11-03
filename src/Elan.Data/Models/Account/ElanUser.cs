using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Elan.Data.Models.Account
{
    public class ElanUser : IdentityUser<Guid>
    {
        public ICollection<ElanUserSetting> Settings { get; set; }

        public ElanUser()
        {
            Settings = new List<ElanUserSetting>();
        }
    }
}
