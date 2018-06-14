// -----------------------------------------------------------------------
// <copyright file="GameModuleInfo.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class GameModuleInfo : MarshalByRefObject
    {
        /// <summary>
        /// Gets or sets a value indicating the identify code for the module.
        /// </summary>
        public string IdentityCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the processing order priority.  The order in which to process the requests across modules.  Processing is done in ascending order.
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the name of the module.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the module is required to be loaded.
        /// </summary>
        public bool IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the code base of the module.
        /// </summary>
        public string CodeBase
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the name of the type for the module object class.
        /// </summary>
        public string ObjectClassType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the module should be loaded.
        /// </summary>
        public bool Load
        {
            get;
            set;
        }
    }
}
