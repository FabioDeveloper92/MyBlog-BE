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
            var dto = new PostWriteDto(item.Id, item.Title, item.ImageThumb, item.ImageMain, item.Text, item.Tags, item.CreateBy, item.CreateDate, item.UpdateDate, item.PublishDate, item.Comments);

            return dto;
        }
    }
}
