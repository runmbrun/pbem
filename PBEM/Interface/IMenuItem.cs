// -----------------------------------------------------------------------
// <copyright file="IMenuItem.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public interface IMenuItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ToolStrip GetToolStrip();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        MenuStrip GetMenuStrip();
    }
}
