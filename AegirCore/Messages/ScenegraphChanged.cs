using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace AegirCore.Messages
{
    public class ScenegraphChanged : GenericTinyMessage<SceneGraph>
    {

        public ScenegraphChanged(SceneGraph scene, object sender)
            :base(sender,scene)
        {

        }
    }
}
