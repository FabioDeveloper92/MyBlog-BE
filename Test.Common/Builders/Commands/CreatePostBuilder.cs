using Application.Post.Commands;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreatePostBuilder
    {
        private Guid _id;
        private string _title;
        private string _text;
        private string _imageThumb;
        private string _imageMain;
        private int[] _tags;
        private string _createBy;
        private DateTime _createDate;
        private DateTime _updateDate;
        private DateTime? _publishDate;

        public CreatePostBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _title = "Test every moment";
            _text = "This is default value";
            _imageMain = "imgUrlMain";
            _imageThumb = "imageUrlThumb";
            _tags = new[] { 1 };
            _createBy = "Fabio";

            var date = DateTime.Now;
            _createDate = date;
            _updateDate = date;
            _publishDate = null;

            return this;
        }

        public CreatePostBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public CreatePostBuilder WithTitle(string name)
        {
            _title = name;
            return this;
        }

        public CreatePostBuilder WithText(string description)
        {
            _text = description;
            return this;
        }

        public CreatePostBuilder WithImageMain(string imageMain)
        {
            _imageMain = imageMain;
            return this;
        }

        public CreatePostBuilder WithImageThumb(string imageThumb)
        {
            _imageThumb = imageThumb;
            return this;
        }

        public CreatePostBuilder WithTags(int[] tags)
        {
            _tags = tags;
            return this;
        }

        public CreatePostBuilder WithCreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public CreatePostBuilder WithUpdateDate(DateTime updateDate)
        {
            _updateDate = updateDate;
            return this;
        }
        public CreatePostBuilder WithPublishDate(DateTime? publishDate)
        {
            _publishDate = publishDate;
            return this;
        }

        public CreatePostBuilder WithCreateBy(string createdBy)
        {
            _createBy = createdBy;
            return this;
        }


        public CreatePost Build()
        {
            return new CreatePost(_id, _title, _imageThumb, _imageMain, _text, _tags, _createBy, _createDate, _updateDate, _publishDate);
        }
    }
}
