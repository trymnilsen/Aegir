using Aegir.Util;
using AegirCore.Behaviour;
using AegirCore.Behaviour.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public static class BehaviourViewModelFactory
    {
        /// <summary>
        /// Our cache of behaviour objects to view model objects
        /// T1 (first type) is The type of the behaviour object
        /// T2 (second type) is the type of the viewmodel
        /// </summary>
        private static Dictionary<Type, Type> behaviourVmMapping;
        static BehaviourViewModelFactory()
        {
            behaviourVmMapping = new Dictionary<Type, Type>();

            EnumerateVms();
        }

        private static void EnumerateVms()
        {
            //Only look for viewmodels in the main assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            //This is quite an expensive op, but it's only run on startup
            var viewModelTypes =
                // Partition on the type list initially.
                from t in assembly.GetTypes()
                //Get attributes
                let attributes = t.GetCustomAttributes(typeof(ProxyForBehaviourAttribute), false)
                //Query where
                where attributes != null && attributes.Length > 0
                //Select the results into a anonomous type
                select new { Type = t, Attributes = attributes.Cast<ProxyForBehaviourAttribute>() };

            foreach(var vmType in viewModelTypes)
            {
                //Do some extra checks
                //Check that the type of the object holding the attribute is a subclass of vmproxy
                if(!ReflectionUtils.IsSubclassOfRawGeneric(typeof(TypedBehaviourViewModelProxy<>), vmType.Type))
                {
                    //No it was not, jump to next
                    continue;
                }
                //Check that the target is a correct subtype
                //First get the data out of the attribute, we assume only one
                Type vmTargetType = vmType.Attributes.FirstOrDefault()?.TargetBehaviourType;
                if(vmTargetType == null || !vmTargetType.IsSubclassOf(typeof(BehaviourComponent)))
                {
                    //No it was not, jump to next
                    continue;
                }

                //Everything is ok, add it to the mapping
                behaviourVmMapping.Add(vmTargetType,vmType.Type);
            }
        }

        public static BehaviourViewModelProxy GetViewModelProxy(BehaviourComponent behaviour)
        {
            try
            {
                Type vmType = null;
                //Check if we have a viewmodel for this behaviour
                if(behaviourVmMapping.TryGetValue(behaviour.GetType(), out vmType))
                {
                    //Create a new instance of this ViewModel 
                    //the only constructor parameter is the source behaviour this 
                    //viewmodel is wrapping
                    object instance = Activator.CreateInstance(vmType, behaviour);
                    BehaviourViewModelProxy vm = instance as BehaviourViewModelProxy;
                    return vm;
                }
            }
            catch(Exception e)
            {
                DebugUtil.LogWithLocation($"Error Occured Getting ViewModel for {behaviour.ToString()}");
            }
            return null;
        }

    }
}
