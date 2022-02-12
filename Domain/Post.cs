using System;
using System.Linq;
using Domain.Core;
using Domain.Exceptions;
using Infrastructure.Core.Enum;

namespace Domain
{
    public class Post : Entity<Guid>
    {
        public string Title { get; private set; }
        public string ImageThumb { get; private set; }
        public string ImageMain { get; private set; }
        public string Text { get; private set; }
        public int[] Tags { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public DateTime? PublishDate { get; private set; }

        private Post(Guid id, string title, string imageThumb, string imageMain, string text, int[] tags, string createBy, DateTime createDate, DateTime updateDate, DateTime? publishDate) : base(id)
        {
            Title = title;
            ImageThumb = imageThumb;
            ImageMain = imageMain;
            Text = text;
            Tags = tags;
            CreateBy = createBy;
            CreateDate = createDate;
            UpdateDate = updateDate;
            PublishDate = publishDate;
        }

        public static Post Create(string title, string imageThumb, string imageMain, string text, int[] tags, string createBy, DateTime createDate, DateTime updateDate, DateTime? publishDate, Guid? postId = null)
        {
            if (postId == null)
                postId = Guid.NewGuid();

            var item = new Post(postId.Value, title, imageThumb, imageMain, text, tags, createBy, createDate, updateDate, publishDate);

            item.Validate();

            return item;
        }

        public void SetTitle(string title)
        {
            Title = title;
            Validate();
        }

        public void SetImageThumb(string imageThumb)
        {
            ImageThumb = imageThumb;
            Validate();
        }

        public void SetImageMain(string imageMain)
        {
            ImageMain = imageMain;
            Validate();
        }

        public void SetTags(int[] tags)
        {
            Tags = tags;
            Validate();
        }

        public void SetCreateBy(string createBy)
        {
            CreateBy = createBy;
            Validate();
        }

        public void SetCreateDate(DateTime createDate)
        {
            CreateDate = createDate;
            Validate();
        }

        public void SetUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
            Validate();
        }

        public void SetPublishDate(DateTime? publishDate)
        {
            PublishDate = publishDate;
            Validate();
        }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Title))
                throw new EmptyFieldException(nameof(Title));

            if (string.IsNullOrEmpty(Text))
                throw new EmptyFieldException(nameof(Text));

            if (string.IsNullOrEmpty(ImageThumb))
                throw new EmptyFieldException(nameof(ImageThumb));

            if (string.IsNullOrEmpty(ImageMain))
                throw new EmptyFieldException(nameof(ImageMain));

            if (string.IsNullOrEmpty(CreateBy))
                throw new EmptyFieldException(nameof(CreateBy));

            if (Tags == null || Tags.Length == 0 || !TagsAvailable())
                throw new EmptyFieldException(nameof(Tags));

            if (CreateDate == null)
                throw new EmptyFieldException(nameof(CreateDate));

            if (UpdateDate == null)
                throw new EmptyFieldException(nameof(UpdateDate));

            if (CreateDate > UpdateDate)
                throw new InvalidDateException(nameof(UpdateDate), "");

            if (PublishDate.HasValue && (PublishDate.Value < UpdateDate || PublishDate.Value < CreateDate))
                throw new InvalidDateException(nameof(UpdateDate), "");
        }

        private bool TagsAvailable()
        {
            var tagsAvailable = ((CategoryPost[])Enum.GetValues(typeof(CategoryPost))).Select(s => (int)s).ToList();
            var itemDiff = Tags.Intersect(tagsAvailable).Count();

            return itemDiff == Tags.Length;
        }
    }
}
