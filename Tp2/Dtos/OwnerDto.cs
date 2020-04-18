using Entities.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Dtos
{
    public class OwnerDto
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public ICollection<AccountDto> Accounts { get; set; }

    }
}
