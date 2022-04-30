namespace Web.Api.Models.Post
{
    public class FilterAllPosts
    {
        public int Limit { get; set; }
        public int? FilterByTime { get; set; }

        public int? OrderByVisibility { get; set; }
    }
}
