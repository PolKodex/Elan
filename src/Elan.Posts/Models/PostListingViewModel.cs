using System.Collections.Generic;
using Elan.Data.Models.Posts;

namespace Elan.Posts.Models
{
    public class PostListingViewModel
    {
        public List<Post> Posts { get; set; }
        public int TotalCount { get; set; }
    }
}
