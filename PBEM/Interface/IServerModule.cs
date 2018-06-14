// -----------------------------------------------------------------------
// <copyright file="IServerModule.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System.Data;

    public interface IServerModule
    {
        #region " Properties "
        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Root { get; set; }
        #endregion

        #region " Operations "
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Logon();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataTable ListFolders(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CreateFolder(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Delete(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DownloadFile(string remote, string local);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool UploadFile(string remote, string local);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IServerExplorer GetServerExplorer(); 
        #endregion
    }
}
