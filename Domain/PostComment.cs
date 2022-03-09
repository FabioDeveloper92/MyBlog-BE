using Domain.Core;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class PostComment : Entity<Guid>
    {
        public string Username { get; private set; }
        public string Text { get; private set; }
        public DateTime CreateDate { get; private set; }

        private PostComment(Guid id, string username, string text, DateTime createDate) : base(id)
        {
            Username = username;
            Text = text;
            CreateDate = createDate;
        }

        public static PostComment Create(string username, string text, DateTime createDate, Guid? id = null)
        {
            if (id == null)
                id = Guid.NewGuid();

            var c = new PostComment(id.Value, username, text, createDate);

            c.Validate();

            return c;
        }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Username))
                throw new EmptyFieldException(nameof(Username));

            if (string.IsNullOrEmpty(Text))
                throw new EmptyFieldException(nameof(Text));
        }
    }
}
