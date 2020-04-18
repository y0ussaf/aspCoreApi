using Entities.models;
using System;
using System.Collections.Generic;
using Tp2.Controllers.QueryParameters;

namespace Tp2.repositories.interfaces
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        public PagedList<Account> getAccountsByOwner(Guid ownerId,AccountQueryStringParams accountParams);
        public Account GetAccountById(Guid guid);
    }
}