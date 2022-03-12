using Application.Interfaces;
using System;

namespace Application.Post.Commands
{
    public class AddPostRelated : ICommand
    {
        public Guid PostRelatedId { get; set; }
        public Guid PostId { get; set; }

        public AddPostRelated(Guid postRelatedId, Guid postId)
        {
            PostRelatedId = postRelatedId;
            PostId = postId;
        }
    }
}
