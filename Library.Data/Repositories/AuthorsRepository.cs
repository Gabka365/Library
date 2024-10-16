﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Models;
using Library.Data.Repositories.Interfaces;

namespace Library.Data.Repositories
{
    public class AuthorsRepository : BaseRepository<Author>, IAuthorsRepository
    {
        public AuthorsRepository(LibraryDbContext db) : base(db) { }

        public void Update(Author author)
        {
            var dbModel = Get(author.Id);

            dbModel.FirstName = author.FirstName;
            dbModel.LastName = author.LastName;
            dbModel.Birthday = author.Birthday;
            dbModel.Motherland = author.Motherland;

            _dbContext.SaveChanges();
        }

        public Author? GetByLastName(string lastName)
        {
            return _dbSet.FirstOrDefault(x => x.LastName == lastName);
        }

        public Author? GetByFirstName(string firstName)
        {
            return _dbSet.FirstOrDefault(x => x.FirstName == firstName);
        }


        public Author? GetByName(string firstName, string lastName)
        {
            return _dbSet.FirstOrDefault(x => x.LastName == lastName && x.FirstName == firstName);
        }
    }
}
