using Domain.Exceptions;
using Infrastructure.Write.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class UserWriteService : IRequestHandler<CreateOrUpdateUser>,
                                    IRequestHandler<CreateUserFromJwt>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        public UserWriteService(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }
        public async Task<Unit> Handle(CreateOrUpdateUser command, CancellationToken cancellationToken)
        {
            var entity = Domain.User.Create(command.Name, command.Surname, command.Email, command.Password, command.LoginWith);

            var oldUser = await _userWriteRepository.SingleOrDefault(entity.Email);

            if (oldUser != null)
            {
                if (oldUser.LoginWith != entity.LoginWith)
                    throw new LoginWithWrongProviderException();

                await _userWriteRepository.Update(entity);
            }
            else
            {
                await _userWriteRepository.Add(entity);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(CreateUserFromJwt command, CancellationToken cancellationToken)
        {
            var entity = Domain.User.Create(command.Name, command.Surname, command.Email, command.Password, command.LoginWith);

            var user = await _userWriteRepository.SingleOrDefault(entity.Email);

            if (user != null)
                throw new UserAlreadyExistException();

            await _userWriteRepository.Add(entity);

            return Unit.Value;
        }
    }
}
