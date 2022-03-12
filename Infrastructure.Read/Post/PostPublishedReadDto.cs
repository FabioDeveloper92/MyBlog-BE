using Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace Infrastructure.Read.Post
{
    public class PostPublishedReadDto : Dto
    {
        public string Title { get; }
        public string Text { get; }
        public string ImageMain { get; }
        public int[] Tags { get; }
        public string CreateBy { get; }
        public DateTime? PublishDate { get; }
        public List<PostCommentReadDto> Comments { get; }
        public List<PostsRelatedCompletedDto> PostsRelatedCompleted { get; }

        public PostPublishedReadDto(Guid id, string title, string text, string imageMain, int[] tags, string createBy, DateTime? publishDate, List<PostCommentReadDto> comments, List<PostsRelatedCompletedDto> postsRelatedCompleted) : base(id)
        {
            Title = title;
            Text = text;
            ImageMain = imageMain;
            Tags = tags;
            CreateBy = createBy;
            PublishDate = publishDate;
            Comments = comments;
            PostsRelatedCompleted = postsRelatedCompleted;
        }
    }

    public class PostsRelatedCompletedDto : Dto
    {
        public string Title { get; }
        public string ImageThumb { get; }

        public PostsRelatedCompletedDto(Guid id, string title, string imageThumb) : base(id)
        {
            Title = title;
            ImageThumb = imageThumb;
        }

    }
}
