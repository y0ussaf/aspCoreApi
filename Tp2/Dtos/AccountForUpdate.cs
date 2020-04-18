using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Dtos
{
    public class AccountForUpdate
    {
        [Required(ErrorMessage = "Account type is required")]
        public string AccountType { get; set; }
    }
}
