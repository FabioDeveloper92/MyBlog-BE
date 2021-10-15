using Domain.Core;
using Domain.Exceptions;
using System;

namespace Domain
{
    public enum LoginProvider
    {
        Jwt = 0,
        Google = 1
    }

    public class User : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string ExternalToken { get; private set; }
        public int LoginWith { get; private set; }
        public string InternalToken { get; private set; }
        public DateTime? ExpiredDate { get; private set; }

        private User(Guid id, string name, string surname, string email, string externalToken, int loginWith, string internalToken, DateTime? expiredDate) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            ExternalToken = externalToken;
            LoginWith = loginWith;
            InternalToken = internalToken;
            ExpiredDate = expiredDate;
        }

        public static User Create(string name, string surname, string email, string externalToken, int loginWith, string internalToken = null, DateTime? expiredDate = null, Guid? userId = null)
        {
            if (userId == null)
                userId = Guid.NewGuid();

            var item = new User(userId.Value, name, surname, email, externalToken, loginWith, internalToken, expiredDate);

            item.Validate();

            return item;
        }

        public void SetName(string name)
        {
            Name = name;
            Validate();
        }

        public void SetSurname(string surname)
        {
            Surname = surname;
            Validate();
        }

        public void SetEmail(string email)
        {
            Email = email;
            Validate();
        }

        public void SetExternalToken(string internalToken)
        {
            ExternalToken = internalToken;
            Validate();
        }

        public void SetInternalToken(string internalToken)
        {
            InternalToken = internalToken;
            Validate();
        }

        public void SetExpiredDate(DateTime expiredDate)
        {
            ExpiredDate = expiredDate;
            Validate();
        }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new EmptyFieldException(nameof(Name));

            if (string.IsNullOrEmpty(Surname))
                throw new EmptyFieldException(nameof(Surname));

            if (string.IsNullOrEmpty(Email))
                throw new EmptyFieldException(nameof(Email));

            if (string.IsNullOrEmpty(ExternalToken))
                throw new EmptyFieldException(nameof(ExternalToken));

            if (!ValidateLoginProvider(LoginWith))
                throw new LoginProviderNotExistException(LoginWith.ToString());

            if (ExpiredDate.HasValue && ExpiredDate.Value < DateTime.Now)
                throw new InvalidDateException(nameof(ExpiredDate), ExpiredDate.Value.ToString("dd MM yyyy"));
            else if (ExpiredDate.HasValue && string.IsNullOrEmpty(InternalToken))
                throw new EmptyFieldException(nameof(InternalToken));
        }

        private bool ValidateLoginProvider(int loginWith)
        {
            switch (loginWith)
            {
                case (int)LoginProvider.Jwt:
                case (int)LoginProvider.Google:
                    return true;

                default:
                    return false;
            }
        }
    }
}
