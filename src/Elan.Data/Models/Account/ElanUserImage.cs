﻿using System;

namespace Elan.Data.Models.Account
{
    public class ElanUserImage
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string RawValue { get; set; }
        public bool IsMain { get; set; }
        public virtual ElanUser User { get; set; }
    }
}
