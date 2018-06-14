// -----------------------------------------------------------------------
// <copyright file="BloodBowl.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM.BloodBowl
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// The Blood Bowl game module.
    /// </summary>
    public class BloodBowl : GameModuleBase
    {
        #region " Private Class Variables "
        /// <summary>
        /// 
        /// </summary>
        private UserControl gameUI = null;

        /// <summary>
        /// 
        /// </summary>
        private IMenuItem menuItems = null;
        #endregion

        #region " Descriptions "
        public override string Name
        {
            get
            {
                return "Blood Bowl";
            }
        }

        public override string IdentityCode
        {
            get
            {
                return "BB";
            }
        }

        public override int Version
        {
            get
            {
                return 1;
            }
        }

        public override bool IsRequired
        {
            get
            {
                return true;
            }
        }

        public override int Priority
        {
            get
            {
                return 1;
            }
        }
        #endregion

        #region " Operations "
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override UserControl GetVisualComponent()
        {
            if (this.gameUI == null)
            {
                this.gameUI = new BloodBowlUI();
            }

            return this.gameUI;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IMenuItem GetMenuItems()
        {
            if (this.menuItems == null || 
                ((BloodBowlToolbar)this.menuItems).GetMenuStrip().Items.Count == 0 ||
                ((BloodBowlToolbar)this.menuItems).GetToolStrip().Items.Count == 0)
            {
                this.menuItems = new BloodBowlToolbar();
                ((BloodBowlToolbar)this.menuItems).MainStart += BloodBowl_MainStart;
                ((BloodBowlToolbar)this.menuItems).TeamManagerStart += BloodBowl_TeamManagerStart;
                ((BloodBowlToolbar)this.menuItems).GameManagerStart += BloodBowl_GameManagerStart;
            }

            return this.menuItems;
        }
        #endregion

        #region " Events "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BloodBowl_MainStart(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((BloodBowlUI)this.gameUI).SetUI(new MainUI());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BloodBowl_TeamManagerStart(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((BloodBowlUI)this.gameUI).SetUI(new TeamManagerUI());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BloodBowl_GameManagerStart(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Uri aUri = new Uri("/BloodBowl;Content/field.jpg", UriKind.Relative);
            Image image = Properties.Resources.field;
            ((BloodBowlUI)this.gameUI).SetUI(image);
        }
        #endregion
    }
}
