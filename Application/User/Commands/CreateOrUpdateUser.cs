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
        public string ExternalToken { get; }
        public int LoginWith { get; }
        public string InternalToken { get; }
        public DateTime? ExpiredDate { get; }

        public CreateOrUpdateUser(string name, string surname, string email, string password, string externalToken, int loginWith, string internalToken, DateTime? expiredDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            ExternalToken = externalToken;
            LoginWith = loginWith;
            InternalToken = internalToken;
            ExpiredDate = expiredDate;
        }
    }
}
