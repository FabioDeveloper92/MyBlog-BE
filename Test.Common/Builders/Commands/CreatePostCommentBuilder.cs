using Application.Post.Commands;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreatePostCommentBuilder
    {
        private Guid _postCommentId;
        private Guid _postId;
        private string _username;
        private string _text;
        private DateTime _createDate;

        public CreatePostCommentBuilder WithDefaults(Guid postId)
        {
            _postCommentId = Guid.NewGuid();
            _postId = postId;
            _username = "This is default value";
            _text = "imgUrlMain";
            _createDate = DateTime.Now;

            return this;
        }

        public CreatePostCommentBuilder WithPostCommentId(Guid id)
        {
            _postCommentId = id;
            return this;
        }

        public CreatePostCommentBuilder WithPostId(Guid id)
        {
            _postId = id;
            return this;
        }

        public CreatePostCommentBuilder WithUsername(string username)
        {
            _username = username;
            return this;
        }

        public CreatePostCommentBuilder WithText(string text)
        {
            _text = text;
            return this;
        }

        public CreatePostCommentBuilder WithCreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public AddPostComment Build()
        {
            return new AddPostComment(_postCommentId, _postId, _text, _username, _createDate);
        }

    }
}
