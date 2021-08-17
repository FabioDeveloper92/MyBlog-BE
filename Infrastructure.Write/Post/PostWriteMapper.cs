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
            var dto = new PostWriteDto(item.Id, item.Title, item.Text, item.Category, item.ImageUrl, item.CreateDate, item.CreateBy);

            return dto;
        }
    }
}
