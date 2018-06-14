// -----------------------------------------------------------------------
// <copyright file="IUser.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    public interface IUser
    {
        #region " Properties "
        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Password { get; set; }
        #endregion
    }
}
