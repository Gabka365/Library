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
    }
}
