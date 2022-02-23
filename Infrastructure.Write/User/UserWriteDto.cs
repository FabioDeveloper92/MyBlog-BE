using Infrastructure.Core;
using System;

namespace Infrastructure.Write.User
{
    public class UserWriteDto : Dto
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string Password { get; }
        public int LoginWith { get; }

        public UserWriteDto(Guid id, string name, string surname, string email, string password, int loginWith) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            LoginWith = loginWith;
        }
    }
}
