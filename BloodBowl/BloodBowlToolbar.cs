// -----------------------------------------------------------------------
// <copyright file="BloodBowlToolbar.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM.BloodBowl
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// The Blood Bowl Toolbar class.
    /// </summary>
    public partial class BloodBowlToolbar : UserControl, IMenuItem
    {
        #region " Constructor "
        /// <summary>
        /// Initializes a new instance of the <see cref="BloodBowlToolbar"/> class.
        /// </summary>
        public BloodBowlToolbar()
        {
            this.InitializeComponent();
        }
        #endregion

        #region " Public Event Functions "
        /// <summary>
        /// Event that is called when the Main tool strip button is pressed.
        /// </summary>
        public event EventHandler<CancelEventArgs> MainStart;

        /// <summary>
        /// Event that is called when the Team Manager tool strip button is pressed.
        /// </summary>
        public event EventHandler<CancelEventArgs> TeamManagerStart;

        /// <summary>
        /// Event that is called when the Game Manager tool strip button is pressed.
        /// </summary>
        public event EventHandler<CancelEventArgs> GameManagerStart;
        #endregion

        #region " Public Functions "
        /// <summary>
        /// Returns all the menu strip items.
        /// </summary>
        /// <returns>The menu strip for this game.</returns>
        public MenuStrip GetMenuStrip()
        {
            return this.menuStrip1;
        }

        /// <summary>
        /// Returns all the tool strip items.
        /// </summary>
        /// <returns>The menu strip for this game.</returns>
        public ToolStrip GetToolStrip()
        {
            return this.toolStrip1;
        }
        #endregion

        #region " Private Event Functions "
        /// <summary>
        /// Fires when the main tool strip button is pressed.  Passes on event to handlers.
        /// </summary>
        /// <param name="sender">The tool strip button.</param>
        /// <param name="e">Event arguments.</param>
        private void ToolStripButtonMain_Click(object sender, EventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs(false);
            this.MainStart?.Invoke(this, args);
        }

        /// <summary>
        /// Fires when the team manager tool strip button is pressed.  Passes on event to handlers.
        /// </summary>
        /// <param name="sender">The tool strip button.</param>
        /// <param name="e">Event arguments.</param>
        private void ToolStripButtonTeamManager_Click_1(object sender, EventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs(false);
            this.TeamManagerStart?.Invoke(this, args);
        }

        /// <summary>
        /// Fires when the game manager tool strip button is pressed.  Passes on event to handlers.
        /// </summary>
        /// <param name="sender">The tool strip button.</param>
        /// <param name="e">Event arguments.</param>
        private void ToolStripButtonGameManger_Click(object sender, EventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs(false);
            this.GameManagerStart?.Invoke(this, args);
        }
        #endregion
    }
}
