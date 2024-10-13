using Library.Data.Models;

namespace Library.Data.Repositories.Interfaces
{
    public interface IAuthorsRepository : IBaseRepository<Author>
    {
        Author? GetByFirstName(string firstName);
        Author? GetByLastName(string lastName);
        Author? GetByName(string firstName, string lastName);
        void Update(Author author);
    }
}