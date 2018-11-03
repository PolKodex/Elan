﻿using Elan.Common.Enums;
using System;

namespace Elan.Data.Models.Account
{
    public class ElanUserSetting
    {
        public virtual ElanUser User { get; set; }
        public Guid UserId { get; set; }
        public UserSetting Setting { get; set; }
        public PrivacySetting PrivacySetting { get; set; }
    }
}
