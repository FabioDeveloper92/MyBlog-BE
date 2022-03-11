using System;

namespace Infrastructure.Read.Post
{
    public class PostCommentReadDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
