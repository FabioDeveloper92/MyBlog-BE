using System;

namespace Web.Api.Models.Post
{
    public class AddPostRelated
    {
        public Guid PostRelatedId { get; set; }
        public Guid PostId { get; set; }
    }
}
