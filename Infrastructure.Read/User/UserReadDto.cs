namespace Infrastructure.Read.User
{
    public class UserReadDto
    {
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string InternalToken { get; }

        public UserReadDto(string name, string surname, string email, string internalToken)
        {
            Name = name;
            Surname = surname;
            Email = email;
            InternalToken = internalToken;
        }
    }
}
