using Library.Data.Models;

namespace Library.Data.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        bool Exist(string login);
        User? Get(int? id);
        List<BookInstance> GetBookInstances(int userId);
        User? GetByUsername(string username);
        void Update(User user);
    }
}