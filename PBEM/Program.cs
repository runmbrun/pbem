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
    using System.Threading.Tasks;
    using System.Windows.Forms;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppObjects.Log = new Logging();
            AppObjects.Server = null;
            AppObjects.Constants = new AppConstants();
            AppObjects.User = new AppUser();

            Application.Run(new MainForm());
        }
    }
}
