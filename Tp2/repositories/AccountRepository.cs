using Entities;
using Entities.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tp2.Controllers.QueryParameters;
using Tp2.Extensions;
using Tp2.repositories.interfaces;

namespace Tp2.repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(Tp2DbContext tp2DbContext) : base(tp2DbContext)
        {
        }


        public Account GetAccountById(Guid guid)
        {
            return FindByCondition(a => a.AccountId.Equals(guid)).FirstOrDefault();
        }
 

        public PagedList<Account> getAccountsByOwner(Guid ownerId,AccountQueryStringParams accountParams)
        {
            var accounts = FindByCondition(a => a.OwnerId.Equals(ownerId)).OrderByQueryOrDefault(accountParams.OrderBy, "DateCreated",false);
            var count = accounts.Count();
            var accountsList = accounts.Paginate(accountParams.PageSize, accountParams.PageNumber).ToList();
            return new PagedList<Account>(accountsList, count, accountParams.PageNumber, accountParams.PageSize);
        }

        
    }
}
