using Domain.Core;
using Domain.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace Domain
{
    public enum LoginProvider
    {
        Jwt = 0,
        Google = 1,


        Empty = 99
    }

    public class User : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string ExternalToken { get; private set; }
        public int LoginWith { get; private set; }
        public string InternalToken { get; private set; }
        public DateTime? ExpiredToken { get; private set; }

        private User(Guid id, string name, string surname, string email, string password, string externalToken, int loginWith, string internalToken, DateTime? expiredToken) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            ExternalToken = externalToken;
            LoginWith = loginWith;
            InternalToken = internalToken;
            ExpiredToken = expiredToken;
        }

        public static User Create(string name, string surname, string email, string password, string externalToken, int loginWith, string internalToken = null, DateTime? expiredToken = null, Guid? userId = null)
        {
            if (userId == null)
                userId = Guid.NewGuid();

            var item = new User(userId.Value, name, surname, email, password, externalToken, loginWith, internalToken, expiredToken);

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

        public void SetPassword(string password)
        {
            Password = password;
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

        public void SetExpiredToken(DateTime? expiredToken)
        {
            ExpiredToken = expiredToken;
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

            ValidateCustomFieldFromProvider(LoginWith, Password, ExternalToken);

            if (ExpiredToken.HasValue && string.IsNullOrEmpty(InternalToken))
                throw new EmptyFieldException(nameof(InternalToken));
        }

        private void ValidateCustomFieldFromProvider(int loginWith, string password, string externalToken)
        {
            switch (loginWith)
            {
                case (int)LoginProvider.Jwt:
                    {
                        if (!ValidatePassword(password))
                            throw new PasswordNotValidException();

                        break;
                    }

                case (int)LoginProvider.Google:
                    {
                        if (string.IsNullOrEmpty(externalToken))
                            throw new EmptyFieldException(nameof(ExternalToken));

                        break;
                    }

                default:
                    throw new LoginProviderNotExistException(LoginWith.ToString());
            }
        }

        private bool ValidatePassword(string password)
        {
            var regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$");
            return regex.IsMatch(password);
        }
    }
}
