using System;
using System.Collections.Generic;
using System.Linq;

namespace Elan.Common.Extensions
{
    public static class EnumHelper
    {
        public static Dictionary<int, string> GetEnumDictionary<T>()
        {
            return Enum.GetValues(typeof(T))
                .Cast<int>()
                .ToDictionary(e => e, e => Enum.GetName(typeof(T), e));
        }
    }
}
