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

        public CreateGoogleUserBuilder WithDefaults()
        {
            _id = Guid.NewGuid();
            _name = "Fabio";
            _surname = "Boh";
            _email = "fabio@test.it";

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
        public CreateOrUpdateUser Build()
        {
            return new CreateOrUpdateUser(_name, _surname, _email, "", (int)LoginProvider.Google);
        }
    }
}
