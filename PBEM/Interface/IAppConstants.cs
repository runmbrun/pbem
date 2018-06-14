

namespace PBEM
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// 
    /// </summary>
    public interface IAppConstants
    {
        #region " Descriptions "
        /// <summary>
        /// Gets a value indicating the version of the software
        /// </summary>
        string SoftwareVersion
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating the prior version of the software
        /// </summary>
        string PriorVersion
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating the release of the software
        /// </summary>
        string SoftwareRelease
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating the last release of the prior software version
        /// </summary>
        string PriorRelease
        {
            get;
        }
        #endregion

        #region " Operations "
        /// <summary>
        /// Gets a settings value
        /// </summary>
        /// <param name="identifier">The identifier of the setting</param>
        /// <returns>The value for the setting or an empty string.</returns>
        string GetValue(string identifier);

        /// <summary>
        /// Gets the icon for the application
        /// </summary>
        /// <param name="width">The width of the application (16, 32, 48)</param>
        /// <param name="height">The height of the application (16, 32, 48)</param>
        /// <returns>The application Icon object</returns>
        Icon GetApplicationIcon(int width, int height);

        /// <summary>
        /// Saves Constant file to the stream
        /// </summary>
        /// <param name="identifier">The identifier of the setting</param>
        /// <param name="dataout">The stream to write the constant file data to.</param>
        /// <returns>A flag indicating whether the data was successfully written to the stream.</returns>
        bool GetConstantFile(string identifier, Stream dataout);

        /// <summary>
        /// Determines if a specific financial program module is loaded.
        /// </summary>
        /// <param name="identitycode">Unique identifier for the module to look for.</param>
        /// <returns>A flag indicating if the module is currently loaded (true) or not (false)</returns>
        bool IsModuleLoaded(string identitycode);

        /// <summary>
        /// Enumerator to list out the loaded modules.
        /// </summary>
        /// <returns>An enumerator allowing listing out all loaded modules.</returns>
        List<IGameModule> Modules();

        /// <summary>
        /// Allows a module to put shared constants in the global constants.
        /// </summary>
        /// <param name="module">Identifier for the module</param>
        /// <param name="name">Name of the constant</param>
        /// <param name="value">Value of the constant</param>
        /// <returns>A flag indicating if the module constant was successfully set.</returns>
        bool SetModuleConstant(string module, string name, object value);

        /// <summary>
        /// Gets a module shared constant
        /// </summary>
        /// <typeparam name="T">The data type of the constant</typeparam>
        /// <param name="module">Identifier for the module</param>
        /// <param name="name">Name of the constant</param>
        /// <returns>Gets the value of the module constant.</returns>
        T GetModuleConstant<T>(string module, string name);
        #endregion
    }
}
