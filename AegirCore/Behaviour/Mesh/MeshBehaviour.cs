using AegirCore.Mesh;
using AegirCore.Scene;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Xml.Linq;

namespace AegirCore.Behaviour.Mesh
{
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class MeshBehaviour : BehaviourComponent
    {
        private MeshData mesh;

        public MeshData Mesh
        {
            get { return mesh; }
            set
            {
                if(value!=mesh)
                {
                    MeshData oldMesh = mesh;
                    mesh = value;

                    MeshChangeAction action;
                    //If mesh is null but value is an instance, 
                    //we assume we are setting a new mesh
                    if(oldMesh == null && value is MeshData)
                    {
                        action = MeshChangeAction.New;
                    }
                    //else if old mesh is an instance, but we are nulling it
                    //we assume it means remove
                    else if(oldMesh is MeshData && value == null)
                    {
                        action = MeshChangeAction.Remove;
                    }
                    //If none of the above applies we assume its a 
                    //change from one to another
                    else
                    {
                        action = MeshChangeAction.Change;
                    }
                    //Trigger change event
                    TriggerMeshChanged(new MeshChangedArgs(oldMesh, mesh, action));
                }
            }
        }

        public MeshBehaviour(Node parentNode)
            :base(parentNode)
        {
           
        }

        private void TriggerMeshChanged(MeshChangedArgs args)
        {
            MeshChangedHandler meshHandler = MeshChanged;
            if(meshHandler!=null)
            {
                meshHandler(this, args);
            }
        }

        public override XElement Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(XElement data)
        {
            throw new NotImplementedException();
        }

        public delegate void MeshChangedHandler(MeshBehaviour source, MeshChangedArgs args);
        public event MeshChangedHandler MeshChanged;
    }
}