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


        public bool IsExist(string bookName, Author author)
        {
            if (_dbSet?.Include(x => x.BookAuthor).Where(x => x.Name.Equals(bookName) && x.BookAuthor.Equals(author)) is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public Book GetByAuthorAndName(string bookName, Author author)
        {
            return _dbSet?.Include(x => x.BookAuthor)
                 .FirstOrDefault(x => x.Name == bookName && x.BookAuthor == author);
        }

        public void UpdateCount(Book? book, uint count)
        {
            book.Count += count;
            _dbContext.SaveChanges();
        }

        public void Update(Book book)
        {
            var dbModel = Get(book.Id);

            dbModel.Id = book.Id;
            dbModel.ISBN = book.ISBN;
            dbModel.Name = book.Name;
            dbModel.Description = book.Description;
            dbModel.Genre = book.Genre; 
            dbModel.Count = book.Count;
            dbModel.BookAuthor.FirstName = book.BookAuthor.FirstName;
            dbModel.BookAuthor.LastName = book.BookAuthor.LastName;

            _dbContext.SaveChanges();
        }
    }
}
