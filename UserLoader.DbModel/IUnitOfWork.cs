using System.Threading.Tasks;

using UserLoader.DbModel.Entities;

namespace UserLoader.DbModel
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Task BeginTransactionAsync();
        void SaveChanges();
        Task SaveChangesAsync();
        void AbortTransaction();
        Task AbortTransactionAsync();
        IRepository<T> GetRepository<T>() where T : AbstractEntity;
    }
}