using System;
using System.Collections.Generic;
using System.Text;

namespace Elan.Posts.Models
{
    public class CreatePostViewModel
    {
        public string Content { get; set; }
        public string ToUserId { get; set; } = null;
    }
}
