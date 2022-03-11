using System;

namespace Infrastructure.Write.Post
{
    public class PostCommentWriteDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
