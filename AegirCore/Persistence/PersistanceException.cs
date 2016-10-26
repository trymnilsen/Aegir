using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Persistence
{
    public class PersistanceException : Exception
    {
        public PersistanceException(string message) :
            base(message)
        { }
    }
}