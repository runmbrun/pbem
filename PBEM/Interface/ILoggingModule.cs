using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBEM
{
    public interface ILoggingModule
    {
        #region " Operations "
        /// <summary>
        /// Logs the details about an exception to the error log.  
        /// </summary>
        /// <param name="ex">The exception being logged.</param>
        void Log(string message);

        /// <summary>
        /// Logs the details about an exception to the error log.  
        /// </summary>
        /// <param name="ex">The exception being logged.</param>
        void Error(string message);

        /// <summary>
        /// Logs the details about an exception to the error log.  
        /// </summary>
        /// <param name="ex">The exception being logged.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the details about an exception to the error log.  
        /// </summary>
        /// <param name="ex">The exception being logged.</param>
        void LogException(Exception ex, bool displayError = true);
        #endregion
    }
}
