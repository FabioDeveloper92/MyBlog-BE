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
        public int LoginWith { get; private set; }

        private User(Guid id, string name, string surname, string email, string password, int loginWith) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            LoginWith = loginWith;
        }

        public static User Create(string name, string surname, string email, string password, int loginWith, Guid? userId = null)
        {
            if (userId == null)
                userId = Guid.NewGuid();

            var item = new User(userId.Value, name, surname, email, password, loginWith);

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

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new EmptyFieldException(nameof(Name));

            if (string.IsNullOrEmpty(Surname))
                throw new EmptyFieldException(nameof(Surname));

            if (string.IsNullOrEmpty(Email))
                throw new EmptyFieldException(nameof(Email));

            ValidateCustomFieldFromProvider(LoginWith, Password);
        }

        private void ValidateCustomFieldFromProvider(int loginWith, string password)
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
                        return;
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
