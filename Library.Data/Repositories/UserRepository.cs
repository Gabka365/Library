using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public User? GetByUsername(string username) 
        { 
            return _dbSet.FirstOrDefault(x => x.Name == username); 
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

            _dbContext.SaveChanges();   
        }

    }
}
