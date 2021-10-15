using Infrastructure.Core;
using System;

namespace Infrastructure.Write.User
{
    public class UserWriteDto : Dto
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string ExternalToken { get; }
        public int LoginWith { get; }
        public string InternalToken { get; }
        public DateTime? ExpiredToken { get; }

        public UserWriteDto(Guid id, string name, string surname, string email, string externalToken, int loginWith, string internalToken, DateTime? expiredToken) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            ExternalToken = externalToken;
            LoginWith = loginWith;
            InternalToken = internalToken;
            ExpiredToken = expiredToken;
        }
    }
}
