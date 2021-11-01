using Application.User.Commands;
using Domain;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreateJwtUserBuilder
    {
        private Guid _id;
        public string _name;
        public string _surname;
        public string _email;
        public string _password;
        public string _internalToken;
        public DateTime? _expiredDate;

        public CreateJwtUserBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _name = "Fabio";
            _surname = "Boh";
            _email = "fabio@test.it";
            _password = "Fake100*";
            _internalToken = Guid.NewGuid().ToString();
            _expiredDate = DateTime.Now.AddDays(5);

            return this;
        }

        public CreateJwtUserBuilder WithId(Guid loginWith)
        {
            _id = loginWith;
            return this;
        }

        public CreateJwtUserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CreateJwtUserBuilder WithSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public CreateJwtUserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }   
        
        public CreateJwtUserBuilder WithPassword(string password)
        {
            _password = password;
            return this;
        }
        public CreateJwtUserBuilder WithInternalToken(string internalToken)
        {
            _internalToken = internalToken;
            return this;
        }

        public CreateJwtUserBuilder WithExpiredDate(DateTime? expiredDate)
        {
            _expiredDate = expiredDate;
            return this;
        }

        public CreateUserFromJwt Build()
        {
            return new CreateUserFromJwt(_name, _surname, _email, _password, "", (int)LoginProvider.Jwt, _internalToken, _expiredDate);
        }
    }
}
