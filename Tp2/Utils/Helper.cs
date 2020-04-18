using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Utils
{
    public static class Helper
    {
        public static object GetDefaultValue(Type t)
        {
            if (!t.IsValueType || Nullable.GetUnderlyingType(t) != null)
                return null;

            return Activator.CreateInstance(t);
        }
    }
}
