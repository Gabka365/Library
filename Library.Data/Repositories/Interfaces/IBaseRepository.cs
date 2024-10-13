using Library.Data.Models;

namespace Library.Data.Repositories.Interfaces
{
    public interface IBaseRepository<DbModel> where DbModel : BaseModel
    {
        bool Any();
        DbModel Create(DbModel model);
        void Delete(int id);
        DbModel? Get(int? id);
        List<DbModel> GetAll();
    }
}