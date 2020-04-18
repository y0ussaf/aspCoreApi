using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Controllers.QueryParameters
{
    public class OwnerQueryStringParams : QueryStringParams
    {

        public OwnerQueryStringParams()
        {
            OrderBy = "name";
        }


        public uint MinYearOfBirth { get; set; }
        public uint MaxYearOfBirth { get; set; } = (uint)DateTime.Now.Year;

        public bool ValidYearRange => MaxYearOfBirth > MinYearOfBirth;

        public string Name { get;  set; }
    }
}
