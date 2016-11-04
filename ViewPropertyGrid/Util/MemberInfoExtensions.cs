using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.Util
{
    internal static class MemberInfoExtensions
    {
        internal static T GetFirstOrDefaultAttribute<T>(this MemberInfo memberInfo) where T:class
        {
            object[] attributes = memberInfo.GetCustomAttributes(false);
            return attributes.FirstOrDefault(x => x is T) as T;
        } 
    }
}
