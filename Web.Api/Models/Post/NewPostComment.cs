using System;

namespace Web.Api.Models.Post
{
    public class NewPostComment
    {
        public Guid PostId { get; set; }
        public string Text { get; set; }
    }
}
