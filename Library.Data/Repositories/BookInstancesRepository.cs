using Library.Data.Models;
using Library.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class BookInstancesRepository : BaseRepository<BookInstance>, IBookInstancesRepository
    {
        public BookInstancesRepository(LibraryDbContext dbContext) : base(dbContext) { }


        public List<BookInstance> GetByBook(Book book)
        {
            return _dbContext.BookInstances.Where(bi => bi.Book.Id == book.Id).ToList();
        }

        public void CreateBookInstances(Book book, int count, IBookInstancesRepository bookInstancesRepository)
        {
            for (int i = 0; i < count; i++)
            {
                var bookInstance = new BookInstance
                {
                    Book = book
                };
                Create(bookInstance);
            }
        }

        public void UpdateBookInstances(Book book, int newCount, int currentCount)
        {
            if (newCount > currentCount)
            {
                for (int i = 0; i < newCount - currentCount; i++)
                {
                    var bookInstance = new BookInstance
                    {
                        Book = book
                    };
                    Create(bookInstance);
                }
            }
            else if (newCount < currentCount)
            {
                var instancesToDelete = GetByBook(book)
                    .Where(i => i.DeliveryDate == null && i.ExpectedReturnDate == null)
                    .Take(currentCount - newCount)
                    .ToList();

                foreach (var instance in instancesToDelete)
                {
                    Delete(instance.Id);
                }
            }
        }


        public void Update(BookInstance instance)
        {
            var dbModel = Get(instance.Id);

            dbModel.Id = instance.Id;
            dbModel.User = instance.User;
            dbModel.Book = instance.Book;
            dbModel.ExpectedReturnDate = instance.ExpectedReturnDate;
            dbModel.DeliveryDate = instance.DeliveryDate;

            _dbContext.SaveChanges();
        }

        public int GetCountOfFreeBooks(Book book)
        {
            return _dbSet
                .Where(i => i.User == null && i.Book == book)
                .Count();
        }


        public BookInstance? GetById(int instanceId)
        {
            return _dbSet
                .Include(i => i.User)
                .Include(i => i.Book)
                .FirstOrDefault(i => i.Id == instanceId);

        }

    }
}
