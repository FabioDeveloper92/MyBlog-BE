using Application.Interfaces;
using Infrastructure.Read.User;

namespace Application.User.Queries
{
    public class GetUser : IQuery<UserReadDto>
    {
        public string Email { get; }

        public GetUser(string email)
        {
            Email = email;
        }
    }
}
