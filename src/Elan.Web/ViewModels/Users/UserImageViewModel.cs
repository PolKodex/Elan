using System;
using Elan.Data.Models.Account;

namespace Elan.Web.ViewModels.Users
{
    public class UserImageViewModel
    {
        public string Id { get; set; }
        public string RawValue { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsMain { get; set; }

        public UserImageViewModel(ElanUserImage image)
        {
            Id = image.Id.ToString();
            RawValue = image.RawValue;
            UploadDate = image.UploadDate;
            LastUpdateDate = image.LastUpdateDate;
            IsMain = image.IsMain;
        }
    }
}
