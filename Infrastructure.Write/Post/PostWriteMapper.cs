using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Write.Post
{
    public interface IPostWriteMapper
    {
        PostWriteDto ToPostDto(Domain.Post item);
    }

    public class PostWriteMapper : IPostWriteMapper
    {
        public PostWriteDto ToPostDto(Domain.Post item)
        {
            List<PostCommentWriteDto> comments = null;
            if (item.Comments != null)
                comments = item.Comments.Select(c => new PostCommentWriteDto() { Username = c.Username, Text = c.Text, CreateDate = c.CreateDate, Id = c.Id }).ToList();

            var dto = new PostWriteDto(item.Id, item.Title, item.ImageThumb, item.ImageMain, item.Text, item.Tags, item.CreateBy, item.CreateDate, item.UpdateDate, item.PublishDate, comments);
            return dto;
        }
    }
}
