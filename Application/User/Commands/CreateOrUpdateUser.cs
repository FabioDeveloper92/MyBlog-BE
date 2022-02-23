using Application.Interfaces;
using System;

namespace Application.User.Commands
{
    public class CreateOrUpdateUser : ICommand
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string Password { get; }
        public int LoginWith { get; }

        public CreateOrUpdateUser(string name, string surname, string email, string password, int loginWith)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            LoginWith = loginWith;
        }
    }
}
