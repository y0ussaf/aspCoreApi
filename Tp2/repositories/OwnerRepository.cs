using Entities;
using Entities.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tp2.Controllers;
using Tp2.Controllers.QueryParameters;
using Tp2.Dtos;
using Tp2.Extensions;
using Tp2.repositories.interfaces;

namespace Tp2.repositories
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(Tp2DbContext tp2DbContext) : base(tp2DbContext)
        {

        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(o => o.OwnerId.Equals(ownerId)).Include(o => o.Accounts).FirstOrDefault();
        }

        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.OwnerId.Equals(ownerId))
                    .Include(ac => ac.Accounts)
                    .FirstOrDefault();
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }

        public PagedList<Owner> GetOwners(OwnerQueryStringParams ownerParameters)
        {
            var y = RepositoryContext.Owners.FromSqlRaw($"EXEC FirstProcedure '144F038F-1A9E-4522-0D01-08D7D7359868'").ToList();
           foreach(var a in y)
            {
                Console.WriteLine("jjjjjjjjjjjjjjjjjj {}");
            }
            
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
                                        o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth)
                                        .Include(o => o.Accounts).OrderByQueryOrDefault(ownerParameters.OrderBy, "Name");
            var count = owners.Count();
            SearchByName(ref owners, ownerParameters.Name);
            var ownersList = owners.Paginate(ownerParameters.PageSize, ownerParameters.PageNumber)
                                     .ToList();
            return new PagedList<Owner>(ownersList,
                                           count,                      
                                           ownerParameters.PageNumber,
                                            ownerParameters.PageSize
                                           );
        }

        private void SearchByName(ref IEnumerable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;

            owners = owners.Where(o => o.Name.ToLower().Contains(ownerName.Trim().ToLower()));
        }

      
        
    }

 }

