using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data
{
    public interface IActorContainer
    {
        IActorContainer Parent { get; set; }
        void RemoveActor(Actor actor);
        void AddChildActor(Actor actor);
    }
}
