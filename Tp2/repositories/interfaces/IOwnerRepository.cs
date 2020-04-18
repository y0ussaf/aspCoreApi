
using Entities.models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Tp2.Controllers.QueryParameters;

namespace Tp2.repositories.interfaces
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        public PagedList<Owner> GetOwners(OwnerQueryStringParams ownerParameters);
        Owner GetOwnerById(Guid ownerId);
        Owner GetOwnerWithDetails(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}