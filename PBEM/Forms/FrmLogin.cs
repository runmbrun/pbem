// -----------------------------------------------------------------------
// <copyright file="FrmLogin.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Windows.Forms;

    public partial class FrmLogin : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public FrmLogin()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            get
            {
                return this.textBoxUserID.Text;
            }

            set
            {
                this.textBoxUserID.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get
            {
                return this.textBoxPassword.Text;
            }

            set
            {
                this.textBoxPassword.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Default
        {
            get
            {
                return this.checkBoxSaveAsDefault.Checked;
            }

            set
            {
                this.checkBoxSaveAsDefault.Checked = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLogon_Click(object sender, EventArgs e)
        {
            // Validate User Id and Password are filled out
            if (string.IsNullOrWhiteSpace(this.UserID))
            {
                AppObjects.Log.Error("You must fill out User ID.");
            }
            else if (string.IsNullOrWhiteSpace(this.Password))
            {
                AppObjects.Log.Error("You must fill out Password.");
            }
            else
            {
                // Success!
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
