using Application.Post.Commands;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreatePostBuilder
    {
        private Guid _id;
        private string _title;
        private string _text;

        public CreatePostBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _title = "Test every moment";
            _text = "This is default value";

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

        public CreatePost Build()
        {
            return new CreatePost(_id, _title, _text);
        }
    }
}
