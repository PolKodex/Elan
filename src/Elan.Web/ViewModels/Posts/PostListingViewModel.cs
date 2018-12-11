using System.Collections.Generic;

namespace Elan.Web.ViewModels.Posts
{
    public class PostListingViewModel
    {
        public List<PostViewModel> Posts { get; set; }
        public int TotalCount { get; set; }
    }
}
