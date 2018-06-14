// -----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Forms;

namespace PBEM
{
    public interface IGameModule
    {
        #region " Descriptions "
        /// <summary>
        /// Gets a value indicating whether the module must be loaded.
        /// </summary>
        bool IsRequired
        {
            get;
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the identify code for the module.  Normally a single character value unique to the module
        /// </summary>
        string IdentityCode
        {
            get;
        }

        /// <summary>
        /// Gets the processing order priority.  The order in which to process the requests across modules.  Processing is done in ascending order.
        /// </summary>
        int Priority
        {
            get;
        }

        /// <summary>
        /// Gets the version of the module.  
        /// </summary>
        int Version
        {
            get;
        }

        /* todo: is this needed?
        /// <summary>
        /// Gets the set of functions included in this module.
        /// </summary>
        ModuleFunctionality IncludedFunctions
        {
            get;
        }*/
        #endregion

        #region " Operations "
        /// <summary>
        /// Retrieves a user control that displays the game.
        /// </summary>
        /// <returns>A user control for the game.  Null if the module does not contain any visual elements.</returns>
        UserControl GetVisualComponent();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IMenuItem GetMenuItems();

        /* Todo: need to add Game module's own toolbar and menu items, as well as visual components like the game board gui.
    /// <summary>
    /// A list of menu items to add to the global menu.
    /// </summary>
    /// <returns>List of menu items.</returns>
    IMenuItem[] GetGlobalMenus();

    /// <summary>
    /// A list of individual tool strip items to add to the global tool strip.
    /// </summary>
    /// <returns>A list of tool strip items to add to the global tool strip.</returns>
    IToolStripItem[] GetGlobalToolStripItems();

    /// <summary>
    /// A list of module specific global toolbars.
    /// </summary>
    /// <returns>List of toolbars in the module which should be displayed in the application.</returns>
    ToolStrip[] GetGlobalToolbars();

    /// <summary>
    /// Retrieves an array of visual components for the specific component type.  
    /// </summary>
    /// <param name="componenttype">The type of visual component to retrieve</param>
    /// <returns>An array of visual components of the specific type.  Null if the module does not contain any visual elements of that type.</returns>
    IVisualComponent[] GetVisualComponent(ModuleEnum.VisualComponent componenttype);
    */
        #endregion
    }
}
