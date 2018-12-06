using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<PostReactionViewModel> Reactions { get; set; }
        public int ReactionsCount { get; set; }
        public string AuthorMainImageRawValue { get; set; }
        public string UserId { get; set; }
        public int CommentsCount { get; set; }

        public PostViewModel(Post model)
        {
            Id = model.Id;
            Content = model.Content;
            CreatedOn = model.CreatedOn;
            CreatedBy = model.CreatedBy?.UserName;
            TargetUser = model.TargetUser?.UserName;
            Reactions = model.Reactions?.GroupBy(x => x.Type)
                .Select(x => new PostReactionViewModel {Count = x.Count(), Type = x.Key}).ToList();
            ReactionsCount = model.Reactions?.GroupBy(x => x.Type)
                                 .Sum(x => x.Count()) ?? 0;
            CommentsCount = model.CommentsCount;
            AuthorMainImageRawValue = model.CreatedBy?.Images.FirstOrDefault(x => x.IsMain)?.RawValue;
            UserId = model.CreatedBy?.Id.ToString();
        }
    }
}
