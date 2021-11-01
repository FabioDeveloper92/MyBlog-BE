namespace Web.Api.Models.User
{
    public class NewUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ExternalToken { get; set; }
        public string Password { get; set; }
        public int LoginWith { get; set; }
    }
}
