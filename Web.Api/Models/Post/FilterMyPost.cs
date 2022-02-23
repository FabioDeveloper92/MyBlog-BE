namespace Web.Api.Models.Post
{
    public class FilterMyPost
    {
        public string Title { get; set; }
        public int Status { get; set; }

        public int OrderByDate { get; set; }

        public int Limit { get; set; }
    }
}
