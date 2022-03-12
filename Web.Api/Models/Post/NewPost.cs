using System;

namespace Web.Api.Models.Post
{
    public class NewPost
    {
        public string Title { get; set; }
        public string ImageThumb { get; set; }
        public string ImageMain { get; set; }
        public string Text { get; set; }
        public int[] Tags { get; set; }
        public string CreateBy { get; set; }
        public bool ToPublished { get; set; }
        public Guid[] PostsRelated { get; set; }
    }
}
