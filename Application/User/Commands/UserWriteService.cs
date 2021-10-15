using Domain.Exceptions;
using Infrastructure.Write.User;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class UserWriteService : IRequestHandler<CreateOrUpdateUser>
    {
        private readonly IUserWriteRepository _userWriteRepository;

        public UserWriteService(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }
        public async Task<Unit> Handle(CreateOrUpdateUser command, CancellationToken cancellationToken)
        {
            var entity = Domain.User.Create(command.Name, command.Surname, command.Email, command.ExternalToken, command.LoginWith, command.InternalToken, command.ExpiredDate);

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
    }
}
