using System;

namespace Infrastructure.Read.Post
{
    public class MyPostRelatedSimpleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public MyPostRelatedSimpleDto(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
