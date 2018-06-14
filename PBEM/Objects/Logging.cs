// -----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// This is a basic Logging class.
    /// Usage:
    ///   Logging log = new Logging();
    ///   log.WriteToLog = true;
    ///   log.DebugMode = true;
    ///   log.LogFile = @"c:\temp\logfile.log";
    ///   log.log("Test Message");
    ///   log.debug("Test Message");
    ///   log.error("Test Message");
    /// Output:
    ///   Test Message
    ///   Test Message
    ///   Test Message
    /// </summary>
    public class Logging : ILoggingModule
    {
        #region " Variables "
        /// <summary>
        /// List of all the errors that have occured.
        /// </summary>
        public ArrayList Errors = new ArrayList();
        #endregion

        #region " Properties "
        /// <summary>
        /// Write to Console property.
        ///   True = attempt to write to the console.
        ///   False = do not write to the console.
        /// </summary>
        public Boolean WriteToConsole { get; set; } = true;

        /// <summary>
        /// Write to Log property.
        ///   True = attempt to write to a log file.
        ///   False = do not write to a log file.
        /// </summary>
        public Boolean WriteToLog { get; set; } = false;

        /// <summary>
        /// Debug mode Property
        ///   True = Will allow all debug log messages to go to the log file.
        ///   False = Will not write any debug log messages to the log file.
        /// </summary>
        public Boolean DebugMode { get; set; } = false;

        /// <summary>
        /// Log file Property
        ///   This property will contain the full path of the log file 
        ///   being written to.
        /// </summary>
        public String LogFilePath { get; set; }
        #endregion

        #region " Basic Logging Functions "
        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(String message)
        {
            if (this.WriteToConsole)
            {
                Console.WriteLine(message);
            }

            if (this.WriteToLog)
            {
                try
                {
                    // Output Message to log file
                    File.AppendAllText(LogFilePath, message + "\n");
                }
                catch (Exception ex)
                {
                    // Output error to console instead
                    Console.Error.WriteLine("Error: AppendAllText - " + ex.Message);

                    // set bool to false so we don't keep on trying to log messages that can't be logged
                    WriteToLog = false;
                }
            }
        }

        /// <summary>
        /// Log an exception message
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void LogException(Exception ex, bool displayMessage = true)
        {
            if (displayMessage)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Error(ex.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogMessage"></param>
        public void Debug(String LogMessage)
        {
            if (this.DebugMode)
            {
                this.Log("[debug] " + LogMessage);
            }
        }

        /// <summary>
        /// This adds an error message to a ArrayList that contains all the errors
        /// </summary>
        /// <param name="Message"></param>
        public void Error(String Message)
        {
            this.Errors.Add(Message);
            this.Log("[error] " + Message);
        }
        #endregion

        #region " Special Error Functions "
        /// <summary>
        /// This prints out an error message and then quits the application with a return code of 1
        /// </summary>
        /// <param name="ErrorMessage"></param>
        public void ErrorOut(String ErrorMessage)
        {
            Console.Error.WriteLine(ErrorMessage);
            Environment.Exit(1);
        }

        /// <summary>
        /// This prints out multiple error messages and then quits the application with a return code of 1
        /// </summary>
        /// <param name="ErrorMessages"></param>
        public void ErrorOut(ArrayList ErrorMessages)
        {
            foreach (String message in ErrorMessages)
            {
                Console.Error.WriteLine(message);
            }

            Environment.Exit(1);
        }
        #endregion
    }
}
