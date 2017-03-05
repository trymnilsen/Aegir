using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Behaviour
{
    public class BehaviourFactory
    {
        public static BehaviourComponent CreateWithName(string name, Entity parent)
        {
            try
            {
                Type behaviourType = Assembly.GetExecutingAssembly().ExportedTypes.FirstOrDefault(x => x.Name == name);
                if (behaviourType != null && behaviourType.IsSubclassOf(typeof(BehaviourComponent)))
                {
                    return Activator.CreateInstance(behaviourType, parent) as BehaviourComponent;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"An error occured while creating the requested behaviour: {name}", e);
            }

            //If we get here no behaviour was created, just return null
            return null;
        }

        public static BehaviourComponent CreateFromType(Type type, Entity entity)
        {
            try
            {
                return Activator.CreateInstance(type, entity) as BehaviourComponent;
            }
            catch(Exception e)
            {
                throw new ArgumentException($"An error occured creating the behaviour {type.Name}", e);
            }
        }
    }
}