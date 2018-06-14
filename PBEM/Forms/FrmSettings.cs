// -----------------------------------------------------------------------
// <copyright file="FrmSettings.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// The form that displays all the settings of the application.
    /// </summary>
    public partial class FrmSettings : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrmSettings"/> class.
        /// </summary>
        public FrmSettings()
        {
            this.InitializeComponent();
        }

        public List<string> ServerTypes
        {
            set
            {
                this.comboBoxServerType.DataSource = value;
            }
        }

        public string SelectedServerType
        {
            get
            {
                return this.comboBoxServerType.SelectedItem.ToString();
            }

            set
            {
                this.comboBoxServerType.SelectedItem = value;
            }
        }

        public string SelectedRoot
        {
            get
            {
                return this.textBoxRoot.Text;
            }

            set
            {
                this.textBoxRoot.Text = value;
            }
        }
    }
}
