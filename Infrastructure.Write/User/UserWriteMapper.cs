namespace Infrastructure.Write.User
{
    public interface IUserWriteMapper
    {
        UserWriteDto ToUserDto(Domain.User item);
    }
    public class UserWriteMapper : IUserWriteMapper
    {
        public UserWriteDto ToUserDto(Domain.User item)
        {
            var dto = new UserWriteDto(item.Id, item.Name, item.Surname, item.Email, item.Password, item.LoginWith);

            return dto;
        }
    }
}
