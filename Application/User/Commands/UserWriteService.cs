using Domain.Exceptions;
using Infrastructure.Write.User;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class UserWriteService : IRequestHandler<CreateOrUpdateUser>,
                                    IRequestHandler<LogoutUser>,
                                    IRequestHandler<CreateUserFromJwt>,
                                    IRequestHandler<UpdateUserTokenJwt>
    {
        private readonly IUserWriteRepository _userWriteRepository;

        public UserWriteService(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }
        public async Task<Unit> Handle(CreateOrUpdateUser command, CancellationToken cancellationToken)
        {
            var entity = Domain.User.Create(command.Name, command.Surname, command.Email, command.Password, command.ExternalToken, command.LoginWith, command.InternalToken, command.ExpiredDate);

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

        public async Task<Unit> Handle(LogoutUser command, CancellationToken cancellationToken)
        {
            var user = await _userWriteRepository.SingleOrDefaultByInternalToken(command.InternalToken);

            if (user == null)
                throw new UserNotExistException();

            user.SetExpiredToken(null);
            user.SetInternalToken(string.Empty);
            user.SetExternalToken(string.Empty);

            await _userWriteRepository.Update(user);

            return Unit.Value;
        }

        public async Task<Unit> Handle(CreateUserFromJwt command, CancellationToken cancellationToken)
        {
            var entity = Domain.User.Create(command.Name, command.Surname, command.Email, command.Password, command.ExternalToken, command.LoginWith, command.InternalToken, command.ExpiredDate);

            var user = await _userWriteRepository.SingleOrDefault(entity.Email);

            if (user != null)
                throw new UserAlreadyExistException();

            await _userWriteRepository.Add(entity);

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateUserTokenJwt command, CancellationToken cancellationToken)
        {
            var user = await _userWriteRepository.SingleOrDefault(command.Email);

            if (user == null || !user.Password.Equals(command.Password))
                throw new UserNotExistException();

            user.SetExpiredToken(command.ExpiredDate);
            user.SetInternalToken(command.InternalToken);

            await _userWriteRepository.Update(user);

            return Unit.Value;
        }
    }
}
