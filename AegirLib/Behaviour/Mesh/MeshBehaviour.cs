using AegirLib.Asset;
using AegirLib.Mesh;
using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace AegirLib.Behaviour.Mesh
{
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class MeshBehaviour : BehaviourComponent
    {
        private MeshDataAssetReference mesh;

        public MeshDataAssetReference Mesh
        {
            get { return mesh; }
            set
            {
                if (value != mesh)
                {
                    ChangeMesh(mesh, value);
                    mesh = value;
                }
            }
        }

        public MeshBehaviour(Entity parentEntity)
            : base(parentEntity)
        {
        }

        public override XElement Serialize()
        {
            return new XElement(GetType().Name) { Value = "NA" };
        }

        public override void Deserialize(XElement data)
        {
            var meshReference = data.Element("MeshDataReference");
            string assetUriString = meshReference.Value;
            Uri assetUri = new Uri(assetUriString);
            var meshRef = AssetCache.DefaultInstance.Load<MeshDataAssetReference>(assetUri);
            //Assign directly to backing field, no need to tringe change mesh
            //as deserialize is just called on new entities and they will have their mesh loaded
            //on parent entity insertion into scenegraph
            mesh = meshRef;
        }

        private void ChangeMesh(MeshDataAssetReference oldMesh, MeshDataAssetReference newMesh)
        {
            MeshChangeAction action;
            //If mesh is null but value is an instance,
            //we assume we are setting a new mesh
            if (oldMesh == null && newMesh is MeshDataAssetReference)
            {
                action = MeshChangeAction.New;
            }
            //else if old mesh is an instance, but we are nulling it
            //we assume it means remove
            else if (oldMesh is MeshDataAssetReference && newMesh == null)
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
            MeshChanged?.Invoke(this, new MeshChangedArgs(oldMesh, newMesh, action));
        }

        public delegate void MeshChangedHandler(MeshBehaviour source, MeshChangedArgs args);

        public event MeshChangedHandler MeshChanged;
    }
}