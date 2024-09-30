using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Models;

namespace Library.Data.Repositories
{
    public class AuthorsRepository
    {
        private LibraryDbContext _db;

        public AuthorsRepository(LibraryDbContext db) 
        { 
            _db = db;
        }

        public List<Author> GetAll()
        {
            return _db.Authors.ToList();
        }

        public bool Any() 
        { 
            return _db.Authors.Any(); 
        }

        public Author Create(Author author)
        {
            _db.Authors.Add(author);

            _db.SaveChanges();
            
            return author;
        }

        public Author? Get(int? id)
        {
            return _db.Authors.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var author = Get(id);

            _db.Authors.Remove(author);

            _db.SaveChanges();
        }

        public void Update(Author author)
        {
            var db = Get(author.Id);

            db.FirstName = author.FirstName;
            db.LastName = author.LastName;
            db.Birthday = author.Birthday;
            db.Motherland = author.Motherland;

            _db.SaveChanges();
        }
    }
}
