using Application.Interfaces;
using System;

namespace Application.User.Commands
{
    public class UpdateUserTokenJwt : ICommand
    {
        public string Email { get; }
        public string Password { get; }
        public string InternalToken { get; }
        public DateTime ExpiredDate { get; }

        public UpdateUserTokenJwt(string email, string password, string internalToken, DateTime expiredDate)
        {
            Email = email;
            Password = password;
            InternalToken = internalToken;
            ExpiredDate = expiredDate;
        }
    }
}
