// -----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Abstract base class for all modules which provides common functionality
    /// </summary>
    public abstract class GameModuleBase : IGameModule
    {
        /// <summary>
        /// Gets a value indicating whether the module must be loaded.
        /// </summary>
        public abstract bool IsRequired { get; }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the identify code for the module.  Normally a single character value unique to the module
        /// </summary>
        public abstract string IdentityCode { get; }

        /// <summary>
        /// Gets the processing order priority.  The order in which to process the requests across modules.  Processing is done in ascending order.
        /// </summary>
        public abstract int Priority { get; }

        /// <summary>
        /// Gets the version of the module.  
        /// </summary>
        public abstract int Version { get; }

        /// <summary>
        /// Gets the  user control for the game module.
        /// </summary>
        /// <returns></returns>
        public abstract UserControl GetVisualComponent();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IMenuItem GetMenuItems();
    }
}
