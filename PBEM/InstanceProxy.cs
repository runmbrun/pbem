// -----------------------------------------------------------------------
// <copyright file="InstanceProxy.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Proxy class used to load an assembly into a different application domain.
    /// </summary>
    public class InstanceProxy : MarshalByRefObject
    {
        /// <summary>
        /// Loads an assembly and determines if it contains an object that implements the module interface.
        /// </summary>
        /// <param name="path">Full path and name of the assembly to load</param>
        /// <returns>Information about the object that implements the module interface if one could be found in the assembly.</returns>
        public GameModuleInfo LoadAssembly(string path)
        {
            GameModuleInfo result = null;
            Assembly asm = Assembly.LoadFrom(path);
            Type[] types = asm.GetExportedTypes();
            foreach (Type t in types)
            {
                if ((t.GetInterface("IGameModule") != null) && !t.IsAbstract)
                {
                    object tempObj = Activator.CreateInstance(t);
                    result = new GameModuleInfo();
                    result.Name = t.GetProperty("Name").GetValue(tempObj, null).ToString();
                    result.IdentityCode = t.GetProperty("IdentityCode").GetValue(tempObj, null).ToString();
                    result.Priority = (int)t.GetProperty("Priority").GetValue(tempObj, null);
                    result.IsRequired = (t.GetProperty("IsRequired").GetValue(tempObj, null) as bool?) == true ? true : false;
                    result.ObjectClassType = t.FullName;
                }
            }

            return result;
        }
    }
}
