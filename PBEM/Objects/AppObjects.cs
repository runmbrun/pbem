// -----------------------------------------------------------------------
// <copyright file="AppObjects.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class AppObjects
    {
        #region " Private Variables "
        /// <summary>
        /// 
        /// </summary>
        private static ILoggingModule log;

        /// <summary>
        /// 
        /// </summary>
        private static IServerModule server;

        /// <summary>
        /// The application constants object.
        /// </summary>
        private static IAppConstants appConstants;

        /// <summary>
        /// 
        /// </summary>
        private static IUser appUser;
        #endregion

        #region " Public Properties "
        /// <summary>
        /// 
        /// </summary>
        public static ILoggingModule Log
        {
            get
            {
                return AppObjects.log;
            }

            set
            {
                if (AppObjects.log == null)
                {
                    AppObjects.log = value;
                }
                else
                {
                    throw new InvalidOperationException("Exception logger has already been initialized.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServerModule Server
        {
            get
            {
                return AppObjects.server;
            }

            set
            {
                if (AppObjects.server == null)
                {
                    AppObjects.server = value;
                }
                else
                {
                    throw new InvalidOperationException("Server has already been initialized.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the object containing the applications constant information
        /// </summary>
        public static IAppConstants Constants
        {
            get
            {
                return AppObjects.appConstants;
            }

            set
            {
                if (AppObjects.appConstants == null)
                {
                    AppObjects.appConstants = value;
                }
                else
                {
                    throw new InvalidOperationException("Application constants has already been initialized.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the object containing the applications user information
        /// </summary>
        public static IUser User
        {
            get
            {
                return AppObjects.appUser;
            }

            set
            {
                if (AppObjects.appUser == null)
                {
                    AppObjects.appUser = value;
                }
                else
                {
                    throw new InvalidOperationException("Application user has already been initialized.");
                }
            }
        }
        #endregion
    }
}
