using Application.Interfaces;

namespace Application.User.Commands
{
    public class CreateUserFromJwt : ICommand
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string Password { get; }
        public int LoginWith { get; }

        public CreateUserFromJwt(string name, string surname, string email, string password, int loginWith)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            LoginWith = loginWith;
        }
    }
}
