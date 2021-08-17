using Application.Post.Commands;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreatePostBuilder
    {
        private Guid _id;
        private string _title;
        private string _text;
        private int _category;
        private string _imageUrl;
        private DateTime _createDate;
        private string _createBy;

        public CreatePostBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _title = "Test every moment";
            _text = "This is default value";
            _category = 0;
            _imageUrl = "imageUrl";
            _createDate = new DateTime();
            _createBy = "Fabio";
            
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
        
        public CreatePostBuilder WithCategory(int category)
        {
            _category = category;
            return this;
        }

        public CreatePostBuilder WithImageUrl(string imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }

        public CreatePostBuilder WithCreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public CreatePostBuilder WithCreateBy(string createdBy)
        {
            _createBy = createdBy;
            return this;
        }


        public CreatePost Build()
        {
            return new CreatePost(_id, _title, _text, _category, _imageUrl, _createDate, _createBy);
        }
    }
}
