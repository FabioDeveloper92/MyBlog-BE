using Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace Infrastructure.Read.Post
{
    public class PostReadDto : Dto
    {
        public string Title { get; }
        public string Text { get; }
        public string ImageMain { get; }
        public int[] Tags { get; }
        public string CreateBy { get; }
        public DateTime? PublishDate { get; }
        public List<PostCommentReadDto> Comments { get; }
        public List<Guid> PostsRelated { get; }

        public PostReadDto(Guid id, string title, string text, string imageMain, int[] tags, string createBy, DateTime? publishDate, List<PostCommentReadDto> comments, List<Guid> postsRelated) : base(id)
        {
            Title = title;
            Text = text;
            ImageMain = imageMain;
            Tags = tags;
            CreateBy = createBy;
            PublishDate = publishDate;
            Comments = comments;
            PostsRelated = postsRelated;
        }

    }
}
