// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM.BloodBowl
{
    using System.Windows.Forms;

    /// <summary>
    /// The user control that displays the startup information for the game.
    /// </summary>
    public partial class MainUI : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainUI"/> class.
        /// </summary>
        public MainUI()
        {
            this.InitializeComponent();
            this.labelWelcome.Text = $"Welcome {AppObjects.User.Name} to the game of Blood Bowl!";
        }
    }
}
