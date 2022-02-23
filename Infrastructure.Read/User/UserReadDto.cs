using Infrastructure.Core;
using System;

namespace Infrastructure.Read.User
{
    public class UserReadDto : Dto
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }

        public UserReadDto(Guid id, string name, string surname, string email) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }
    }
}
