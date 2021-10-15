using Application.Interfaces;
using Infrastructure.Read.User;

namespace Application.User.Queries
{
    public class GetUser : IQuery<UserReadDto>
    {
        public string InternalToken { get; }

        public GetUser(string internalToken)
        {
            InternalToken = internalToken;
        }
    }
}
