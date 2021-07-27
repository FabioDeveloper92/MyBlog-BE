namespace Infrastructure.Write.Post
{
    public interface IPostWriteMapper
    {
        PostWriteDto ToTaskDto(Domain.Post item);
    }

    public class PostWriteMapper : IPostWriteMapper
    {
        public PostWriteDto ToTaskDto(Domain.Post item)
        {
            var dto = new PostWriteDto(item.Id, item.Title, item.Text);

            return dto;
        }
    }
}
