using System.Collections.Generic;
using System.Linq;

namespace Elan.Web.ViewModels.Posts
{
    public class PostListingViewModel
    {
        public List<PostViewModel> Posts { get; set; }
        public int TotalCount { get; set; }
        public PostListingViewModel(Elan.Posts.Models.PostListingViewModel model)
        {
            Posts = model.Posts.Select(x => new PostViewModel(x)).ToList();
            TotalCount = model.TotalCount;
        }
    }
}
