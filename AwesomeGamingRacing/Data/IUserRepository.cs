using AwesomeGamingRacing.Models;

namespace AwesomeGamingRacing.Data
{
    public interface IUserRepository
    {
        User GetUser(string Name);
        Task<bool> AddUser(User user);
    }
}
