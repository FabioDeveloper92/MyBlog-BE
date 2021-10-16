using Infrastructure.Core;
using System;

namespace Infrastructure.Read.User
{
    public class UserReadDto : Dto
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string InternalToken { get; }
        public DateTime ExpiredToken { get; }

        public UserReadDto(Guid id, string name, string surname, string email, string internalToken, DateTime expiredToken) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            InternalToken = internalToken;
            ExpiredToken = expiredToken;
        }
    }
}
