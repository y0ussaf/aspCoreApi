using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Dtos
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public DateTime DateCreated { get; set; }
        public string AccountType { get; set; }
    }
}
