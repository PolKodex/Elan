namespace Elan.Posts.Models
{
    public class CreatePostViewModel
    {
        public string Content { get; set; }
        public string ToUserId { get; set; } = null;
        public int? BasePostId { get; set; } = null;
    }
}
