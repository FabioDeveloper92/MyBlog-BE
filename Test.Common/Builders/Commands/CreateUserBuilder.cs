using Application.User.Commands;
using Domain;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreateUserBuilder
    {
        private Guid _id;
        public string _name;
        public string _surname;
        public string _email;
        public string _externalToken;
        public LoginProvider _loginWith;
        public string _internalToken;
        public DateTime? _expiredDate;

        public CreateUserBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _name = "Fabio";
            _surname = "Boh";
            _email = "fabio@test.it";
            _externalToken = "123456789";
            _loginWith = LoginProvider.Google;
            _internalToken = Guid.NewGuid().ToString();
            _expiredDate = DateTime.Now.AddDays(5);

            return this;
        }

        public CreateUserBuilder WithId(Guid loginWith)
        {
            _id = loginWith;
            return this;
        }

        public CreateUserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CreateUserBuilder WithSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public CreateUserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public CreateUserBuilder WithExternalToken(string externalToken)
        {
            _externalToken = externalToken;
            return this;
        }
        public CreateUserBuilder WithLoginWith(LoginProvider loginWith)
        {
            _loginWith = loginWith;
            return this;
        }

        public CreateUserBuilder WithInternalToken(string internalToken)
        {
            _internalToken = internalToken;
            return this;
        }

        public CreateUserBuilder WithExpiredDate(DateTime? expiredDate)
        {
            _expiredDate = expiredDate;
            return this;
        }

        public CreateOrUpdateUser Build()
        {
            return new CreateOrUpdateUser(_name, _surname, _email, _externalToken, (int)_loginWith, _internalToken, _expiredDate);
        }
    }
}
