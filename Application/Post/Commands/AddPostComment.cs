using Application.Interfaces;
using System;

namespace Application.Post.Commands
{
    public class AddPostComment : ICommand
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }

        public AddPostComment(Guid id, Guid postId, string text, string username, DateTime createDate)
        {
            Id = id;
            PostId = postId;
            Text = text;
            Username = username;
            CreateDate = createDate;
        }
    }
}
