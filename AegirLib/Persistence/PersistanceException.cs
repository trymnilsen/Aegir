using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Persistence
{
    public class PersistanceException : Exception
    {
        public PersistanceException(string message) :
            base(message)
        { }
    }
}