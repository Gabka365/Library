using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public abstract class BaseRepository<DbModel> where DbModel : BaseModel
    {
        protected readonly LibraryDbContext _dbContext;
        protected readonly DbSet<DbModel> _dbSet;

        public BaseRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<DbModel>(); 
        }

        public virtual List<DbModel> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual bool Any()
        {
            return _dbSet.Any();
        }

        public virtual DbModel Create(DbModel model)
        {
            _dbSet.Add(model);

            _dbContext.SaveChanges();

            return model;
        }

        public virtual DbModel? Get(int? id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual void Delete(int id)
        {
            var author = Get(id);

            _dbSet.Remove(author);

            _dbContext.SaveChanges();
        }

        
    }
}
