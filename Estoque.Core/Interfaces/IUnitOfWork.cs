namespace Estoque.Core.Interfaces
{
    public interface IUnitOfWork
    {
        bool Commit();
        Task<bool> CommitAsync();
        void BeginTransaction();
        bool CommitTransaction();
        void RollBack();
        bool TransactionOpened();
    }
}
