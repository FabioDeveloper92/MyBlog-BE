using Infrastructure.Core;
using System;

namespace Infrastructure.Write.Post
{
    public class PostWriteDto : Dto
    {
        public string Title { get; }
        public string Text { get; }
        public PostWriteDto(Guid id, string title, string text) : base(id)
        {
            Title = title;
            Text = text;
        }
    }
}
