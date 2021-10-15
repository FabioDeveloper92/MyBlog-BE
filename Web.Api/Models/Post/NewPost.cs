using System;

namespace Web.Api.Models.Post
{
    public class NewPost
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
