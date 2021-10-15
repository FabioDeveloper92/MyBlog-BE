using Domain.Exceptions;
using Infrastructure.Read.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Queries
{
    public class UserReadService : IRequestHandler<GetUser, UserReadDto>
    {
        private readonly IUserReadRepository _userReadRepository;
        public UserReadService(IUserReadRepository UserReadRepository)
        {
            _userReadRepository = UserReadRepository;
        }
        public async Task<UserReadDto> Handle(GetUser request, CancellationToken cancellationToken)
        {
            var userReadDto = await _userReadRepository.SingleOrDefault(request.InternalToken);

            if (userReadDto == null)
                throw new NotFoundItemException();

            return userReadDto;
        }
    }
}
