using Tp2.repositories.interfaces;

namespace Tp2.repositories
{
    public interface IRepositoryWrapper
    {
        IOwnerRepository Owner { get; }
        IAccountRepository Account { get; }
        void Save();
    }
}