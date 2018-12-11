using System;
using Elan.Common.Utils;
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
            RawValue = ImageUtil.Resize(image.RawValue, 1024);
            UploadDate = image.UploadDate;
            LastUpdateDate = image.LastUpdateDate;
            IsMain = image.IsMain;
        }
    }
}
