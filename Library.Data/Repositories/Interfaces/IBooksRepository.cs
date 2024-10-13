using Library.Data.Models;

namespace Library.Data.Repositories.Interfaces
{
    public interface IBooksRepository : IBaseRepository<Book>
    {
        Book? Get(int? id);
        List<Book> GetAll();
        Book GetByAuthorAndName(string bookName, Author author);
        Book? GetByName(string name);
        Book? GetByISBN(string ISBN);
        bool IsExist(string bookName, Author author);
        void Update(Book book);
    }
}