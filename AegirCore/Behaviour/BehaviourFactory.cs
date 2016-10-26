using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour
{
    public class BehaviourFactory
    {
        public static BehaviourComponent CreateWithName(string name, Node parent)
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
    }
}