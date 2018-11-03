using System;
using Elan.Data.Models.Posts;

namespace Elan.Web.ViewModels.Posts
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string TargetUser { get; set; }

        public PostViewModel(Post model)
        {
            Id = model.Id;
            Content = model.Content;
            CreatedOn = model.CreatedOn;
            CreatedBy = model.CreatedBy?.UserName;
            TargetUser = model.TargetUser?.UserName;
        }
    }
}
