using Application.Interfaces;

namespace Application.User.Commands
{
    public class LogoutUser : ICommand
    {
        public string InternalToken { get; }

        public LogoutUser(string token)
        {
            InternalToken = token;
        }
    }
}
