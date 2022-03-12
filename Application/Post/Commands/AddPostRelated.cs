using Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Post.Commands
{
    public class AddPostRelated : ICommand
    {
        public Guid PostId { get; set; }

        public AddPostRelated(Guid postId)
        {
            PostId = postId;
        }
    }
}
