using AutoMapper;
using Entities.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tp2.Dtos;

namespace Tp2.AutoMapperConfig
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerForCreation, Owner>();
            CreateMap<Account, AccountDto>();
            CreateMap<AccountForCreation, Account>();
            CreateMap<AccountForUpdate, Account>();
            CreateMap<OwnerForUpdate, Owner>();
        }
    }
}
