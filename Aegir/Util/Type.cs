using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Util
{
    /// <summary>
    /// Wraps the type info for a type, but constricting it to only be
    /// of subtypes of T
    /// </summary>
    /// <typeparam name="T">Type inheritance constraint</typeparam>
    public class Type<T>
    {
        public Type TypeInfo { get; private set; }

        public static bool TryGetTypeInstance(Type subtype, out Type<T> outType)
        {
            if (subtype.GetType().IsSubclassOf(typeof(T)))
            {
                outType = new Type<T>();
                outType.TypeInfo = subtype.GetType();
                return true;
            }
            outType = null;
            return false;
        }
    }
}