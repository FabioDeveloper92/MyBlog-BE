using Application.User.Commands;
using Domain;
using System;

namespace Test.Common.Builders.Commands
{
    public class CreateGoogleUserBuilder
    {
        private Guid _id;
        public string _name;
        public string _surname;
        public string _email;
        public string _externalToken;
        public string _internalToken;
        public DateTime? _expiredDate;

        public CreateGoogleUserBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _name = "Fabio";
            _surname = "Boh";
            _email = "fabio@test.it";
            _externalToken = "123456789";
            _internalToken = Guid.NewGuid().ToString();
            _expiredDate = DateTime.Now.AddDays(5);

            return this;
        }

        public CreateGoogleUserBuilder WithId(Guid loginWith)
        {
            _id = loginWith;
            return this;
        }

        public CreateGoogleUserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CreateGoogleUserBuilder WithSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public CreateGoogleUserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }   
        
        public CreateGoogleUserBuilder WithExternalToken(string externalToken)
        {
            _externalToken = externalToken;
            return this;
        }

        public CreateGoogleUserBuilder WithInternalToken(string internalToken)
        {
            _internalToken = internalToken;
            return this;
        }

        public CreateGoogleUserBuilder WithExpiredDate(DateTime? expiredDate)
        {
            _expiredDate = expiredDate;
            return this;
        }

        public CreateOrUpdateUser Build()
        {
            return new CreateOrUpdateUser(_name, _surname, _email, "", _externalToken, (int)LoginProvider.Google, _internalToken, _expiredDate);
        }
    }
}
