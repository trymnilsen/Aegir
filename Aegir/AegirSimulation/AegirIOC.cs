using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib
{
    public class AegirIOC
    {
        /// <summary>
        /// The backing store for our instances
        /// </summary>
        private static Dictionary<Type, object> resolvedInstances;
        /// <summary>
        /// Static Constructor
        /// </summary>
        static AegirIOC()
        {
            resolvedInstances = new Dictionary<Type, object>();
        }
        /// <summary>
        /// Adds an instance to the IOC container
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="InvalidOperationException">Type of 
        ///     <paramref name="instance"/> is already registered
        /// </exception>
        public static void Register(object instance)
        {
            Type typeToAdd = instance.GetType();
            if(!resolvedInstances.ContainsKey(typeToAdd))
            {
                resolvedInstances.Add(typeToAdd, instance);
            }
            else
            {
                //Lets be explicit that you should not try to add two types
                throw new InvalidOperationException(@"Cannot Add To IOC, 
                                Type already Added " + typeToAdd.ToString());
            }
        }
        /// <summary>
        /// Returns a instance stored in our IOC
        /// </summary>
        /// <typeparam name="T">The Class to Retrieve</typeparam>
        /// <returns>Instance of the Wanted Class</returns>
        public static T Get<T>() where T : class 
        {
            //Get the type
            Type typeToGet = typeof(T);
            if(resolvedInstances.ContainsKey(typeToGet))
            {
                //Double check that our fetched type is correct
                object registeredInstance = resolvedInstances[typeToGet];
                if(registeredInstance is T)
                {
                    return registeredInstance as T;
                }
            }
            else
            {
                return null;
            }

            return null;
        }
    }
}
