using Application.Interfaces;
using System;

namespace Application.Post.Commands
{
    public class CreatePost : ICommand
    {
        public Guid PostId { get; }
        public string Title { get; }
        public string Text { get; }

        public CreatePost(Guid postId, string title, string text)
        {
            PostId = postId;
            Title = title;
            Text = text;
        }

    }
}
