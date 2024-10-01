using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class BooksRepository : BaseRepository<Book>
    {
        public BooksRepository(LibraryDbContext dbContext) : base(dbContext) {  }

        public override List<Book> GetAll()
        {
            return _dbSet
                .Include(x => x.BookAuthor)
                .ToList();
        }

        public override Book? Get(int? id)
        {
            return _dbSet
                .Include(x => x.BookAuthor)
                .FirstOrDefault(x => x.Id == id);
        }

        public Book? GetByISBN(string ISBN)
        {
            return _dbSet
                .Include(x => x.BookAuthor)
                .FirstOrDefault(x => x.ISBN == ISBN);
        }

        public void Update(Book book)
        {
            var dbModel = Get(book.Id);

            dbModel.Id = book.Id;
            dbModel.ISBN = book.ISBN;
            dbModel.Name = book.Name;
            dbModel.Description = book.Description;
            dbModel.Genre = book.Genre; 
            dbModel.BookAuthor.FirstName = book.BookAuthor.FirstName;
            dbModel.BookAuthor.LastName = book.BookAuthor.LastName;

            _dbContext.SaveChanges();
        }
    }
}
