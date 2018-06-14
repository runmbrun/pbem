// -----------------------------------------------------------------------
// <copyright file="ServerModuleBase.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Data;

namespace PBEM
{
    public abstract class ServerModuleBase : IServerModule
    {
        #region Private Variables "
        /// <summary>
        /// 
        /// </summary>
        internal IServerExplorer explorer = null;

        /// <summary>
        /// 
        /// </summary>
        public string Root { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string AccountsFileName = "accounts.dat";

        /// <summary>
        /// 
        /// </summary>
        internal DataSet data = null;
        #endregion

        #region " Abstract Functions "
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IServerExplorer GetServerExplorer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract bool CreateFolder(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract bool Delete(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public abstract bool DownloadFile(string remote, string local);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public abstract string GetName();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract DataTable ListFolders(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public abstract bool UploadFile(string remote, string local);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool Logon();
        #endregion

        #region " Base Class Functions "
        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public void SetRoot(string root)
        {
            this.root = root;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetRoot()
        {
            return this.root;
        }*/
        #endregion
    }
}
