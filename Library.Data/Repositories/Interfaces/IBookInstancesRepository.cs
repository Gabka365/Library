using Library.Data.Models;

namespace Library.Data.Repositories.Interfaces
{
    public interface IBookInstancesRepository : IBaseRepository<BookInstance>
    {
        void CreateBookInstances(Book book, int count, IBookInstancesRepository bookInstancesRepository);
        List<BookInstance> GetByBook(Book book);
        BookInstance? GetById(int instanceId);
        int GetCountOfFreeBooks(Book book);
        void Update(BookInstance instance);
        void UpdateBookInstances(Book book, int newCount, int currentCount);
    }
}