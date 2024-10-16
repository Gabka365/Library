﻿using Library.Data.Models;
using Library.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public User? GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(x => x.Name == username);
        }

        public override User? Get(int? id)
        {
            return _dbSet
                .Include(x => x.Books)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<BookInstance> GetBookInstances(int userId)
        {
            return _dbSet
                .Include(x => x.BookInstances)
                .ThenInclude(x => x.Book)
                .FirstOrDefault(x => x.Id == userId)?
                .BookInstances;
        }

        public bool Exist(string login)
        {
            return _dbSet.Any(x => x.Name == login);
        }

        public void Update(User user)
        {
            var dbModel = Get(user.Id);

            dbModel.Id = user.Id;
            dbModel.Name = user.Name;
            dbModel.HashedPassword = user.HashedPassword;
            dbModel.RefreshToken = user.RefreshToken;
            dbModel.TokenCreated = user.TokenCreated;
            dbModel.TokenExpires = user.TokenExpires;
            dbModel.Books = user.Books;

            _dbContext.SaveChanges();
        }

    }
}
