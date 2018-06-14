// -----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// The main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region " Private Variables "
        /// <summary>
        /// Stores if the current user is an admin or not.
        /// </summary>
        private bool admin = false;

        /// <summary>
        /// 
        /// </summary>
        private bool useDefaults = false;
        #endregion

        #region " Constructor "
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            // Default the status text
            this.toolStripStatusLabel1.Text = "Welcome! Please log on first to find a list of games to play!";

            // Disable all Functionality until user logs on
            this.menuStrip1.Enabled = false;
            this.toolStripComboBoxGameList.Enabled = false;

            // Figure out which server and root path should be used
            this.LoadConfig();
#if DEBUG
            this.admin = true;
#endif      
        }
        #endregion

        #region " Private Tool Strip Events "
        /// <summary>
        /// Fires when the Log On button is pressed.
        /// </summary>
        /// <param name="sender">The button.</param>
        /// <param name="e">Event arguments. Not used.</param>
        private void ToolStripButtonLogOn_Click(object sender, EventArgs e)
        {
            bool result = false;

            this.toolStripStatusLabel1.Text = "Connecting to Server...";
            this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = true;

            // Log on
            using (FrmLogin login = new FrmLogin())
            {
                if (this.useDefaults)
                {
                    login.UserID = AppObjects.User.Name;
                    login.Password = AppObjects.User.Password;
                    login.Default = this.useDefaults;
                }

                if (login.ShowDialog() == DialogResult.OK)
                {
                    result = true;
                    AppObjects.User.Name = login.UserID;
                    AppObjects.User.Password = login.Password;
                    this.useDefaults = login.Default;

                }
            }

            if (result)
            {
                // Get account info to test connection
                result = AppObjects.Server.Connect();

                // Stop the progress bar
                this.toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                this.toolStripProgressBar1.Value = 100;

                if (result)
                {
                    // Able to connect to the server
                    this.toolStripStatusLabel1.Text = "Successfully connected!";

                    // Reset the result value
                    result = false;

                    // Now validate the logon
                    if (AppObjects.Server.Logon())
                    {
                        result = true;
                        this.menuStrip1.Enabled = true;
                        this.toolStripComboBoxGameList.Enabled = true;

                        // Check if the admin options should be visible or not
                        if (admin)
                        {
                            this.adminToolStripMenuItem.Visible = true;
                        }

                        this.toolStripStatusLabel1.Text = $"Welcome {AppObjects.User.Name}!";
                        this.Text = $"{this.Text} - {AppObjects.User.Name}";
                    }
                }
                else
                {
                    MessageBox.Show("Cannot connect to this server as this time.  Please try again later.");
                    this.toolStripStatusLabel1.Text = "Error connecting to server, please check log file.";
                }
            }

            this.toolStripProgressBar1.Visible = false;
        }

        /// <summary>
        /// Fires when the a new option on the game list combo box is selected.
        /// </summary>
        /// <param name="sender">The combo box.</param>
        /// <param name="e">Event arguments. Not used.</param>
        private void ToolStripComboBoxGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(((ToolStripComboBox)sender).SelectedItem.ToString()))
            {
                foreach (IGameModule game in AppObjects.Constants.Modules())
                {
                    if (game.Name == ((ToolStripComboBox)sender).SelectedItem.ToString())
                    {
                        // Load The game's visual component
                        this.panelMainDisplay.Controls.Clear();
                        this.panelMainDisplay.Controls.Add(game.GetVisualComponent());
                        this.panelMainDisplay.ClientSize = game.GetVisualComponent().Size;

                        // Now Add the Game Specific ToolStrip Buttons
                        this.AddMenuItems(game.GetMenuItems());
                    }
                }
            }
            else
            {
                this.panelMainDisplay.Controls.Clear();

                for (int i = 0; i < this.toolStrip1.Items.Count; i++)
                {
                    if (this.toolStrip1.Items[i].Tag != null && this.toolStrip1.Items[i].Tag.ToString() == "Game")
                    {
                        this.toolStrip1.Items.Remove(this.toolStrip1.Items[i--]);
                    }
                }

                for (int i = 0; i < this.menuStrip1.Items.Count; i++)
                {
                    if (this.menuStrip1.Items[i].Tag != null && this.menuStrip1.Items[i].Tag.ToString() == "Game")
                    {
                        this.menuStrip1.Items.Remove(this.menuStrip1.Items[i--]);
                    }
                }
            }
        }
        #endregion

        #region " Private Menu Strip Events "
        /// <summary>
        /// Fires when the Options -> Settings menu is selected.
        /// </summary>
        /// <param name="sender">The menu option.</param>
        /// <param name="e">Event arguments. Not used.</param>
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open up the Settings Dialog
            using (FrmSettings frm = new FrmSettings())
            {
                // Set the current values
                List<string> serverTypes = new List<string>() { "DropBox", "Local File System" };
                frm.ServerTypes = serverTypes;
                frm.SelectedServerType = AppObjects.Server.Name;
                frm.SelectedRoot = AppObjects.Server.Root;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Now save the settings back to the main forms
                    AppObjects.Server.Name = frm.SelectedServerType;
                    AppObjects.Server.Root = frm.SelectedRoot;
                    Properties.Settings.Default["Server"] = AppObjects.Server.Name;
                    Properties.Settings.Default["Root"] = AppObjects.Server.Root;
                    Properties.Settings.Default.Save();
                }
            }
        }
        #endregion

        #region " Private Menu Strip ADMIN Events "
        /// <summary>
        /// Fires when the Server Explorer menu is selected.
        /// </summary>
        /// <param name="sender">The menu option.</param>
        /// <param name="e">Event arguments. Not used.</param>
        private void LoadServerExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panelMainDisplay.Controls.Clear();
            this.panelMainDisplay.Controls.Add(AppObjects.Server.GetServerExplorer().GetUserControl());
        }

        /// <summary>
        /// Create and setup a new account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewAccountFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display a form. Must be able to get the 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserAccountSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region " Private Events "
        /// <summary>
        /// Fires when the form is loaded.
        /// </summary>
        /// <param name="sender">The form.</param>
        /// <param name="e">Event arguments. Not used.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Use the begin invoke to do the rest of the initialization so that the screen shows up.
            this.BeginInvoke((Action)(() =>
            {
                try
                {
                    // Load the modules, verify the DB connection and login 
                    if (this.LoadModules())
                    {
                        this.toolStripComboBoxGameList.Items.Add(string.Empty);
                        foreach (IGameModule game in AppObjects.Constants.Modules())
                        {
                            this.toolStripComboBoxGameList.Items.Add(game.Name);
                        }
                        
                        /*
                        // Notify the modules we are ready to load.
                        CancelEventArgs args = new CancelEventArgs(false);
                        this.Loading?.Invoke(this, args);

                        Application.DoEvents();

                        if (args.Cancel)
                        {
                            // Close the application.
                            this.Close();
                        }*/
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                    this.Close();
                }
            }));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveConfig();
        }
        #endregion

        #region " Private Functions "
        /// <summary>
        /// Loads in the module class libraries.
        /// </summary>
        /// <returns>A flag indicating if the modules were successfully loaded.  If false the application should be shut down.</returns>
        private bool LoadModules()
        {
            List<GameModuleInfo> availableModules = new List<GameModuleInfo>();
            HashSet<string> dependentModules = new HashSet<string>();
            int modulesToLoad = 0;

            // build a temporary domain to check the modules to load.
            string pathToDll = Assembly.GetExecutingAssembly().CodeBase;
            AppDomainSetup domainSetup = new AppDomainSetup { PrivateBinPath = pathToDll };
            AppDomain domain = AppDomain.CreateDomain("TempDomain", null, domainSetup);
            InstanceProxy proxy = domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(typeof(InstanceProxy)).FullName, typeof(InstanceProxy).ToString()) as InstanceProxy;
            if (proxy != null)
            {
                DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                foreach (FileInfo fi in di.GetFiles("*.dll"))
                {
                    GameModuleInfo m = proxy.LoadAssembly(fi.Name);
                    if (m != null)
                    {
                        if ((AppObjects.Constants as AppConstants).ModulesToBeLoaded.Contains(m.Name.Replace(" ", string.Empty)) || m.IsRequired)
                        {
                            m.Load = true;
                            modulesToLoad++;
                        }

#if DEBUG
                        // In debug always load all modules.
                        m.Load = true;
#endif
                        m.CodeBase = fi.Name;
                        availableModules.Add(m);
                    }
                }
            }

            // Verify that all dependent modules are loaded.
            if (modulesToLoad != 0)
            {
#if DEBUG
                /* todo: 
                if ((AppObjects.Overrides != null) && AppObjects.Overrides.SelectModules)
                {
                    using (System.Windows.Forms.FrmLoadModules frm = new System.Windows.Forms.FrmLoadModules(availableModules, dependentModules))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            availableModules = frm.GetModulesToLoad();
                        }
                    }
                }*/
#endif
            }

            // Finally load the needed modules into the current domain.
            foreach (GameModuleInfo mi in availableModules.OrderBy(m => m.Priority))
            {
                if (!string.IsNullOrEmpty(mi.CodeBase) && mi.Load)
                {
                    // Add the module to the dispose wrapper so they get disposed with the form.
                    // todo: this.disposeWrapper1.ItemsToDispose.Add(m);
                    Assembly a = Assembly.LoadFrom(mi.CodeBase);
                    IGameModule m = Activator.CreateInstance(a.GetType(mi.ObjectClassType)) as IGameModule;
                    (AppObjects.Constants as AppConstants).LoadedModules.Add(m);
                }
            }

            AppDomain.Unload(domain);

            return (AppObjects.Constants as AppConstants).LoadedModules.Count > 0;
        }

        /// <summary>
        /// Will Add menu items from a game module to the main form's tool strip or menu strip.
        /// </summary>
        /// <param name="menuItem">The menu items to merge.</param>
        private void AddMenuItems(IMenuItem menuItem)
        {
            if (menuItem != null)
            {
                ToolStrip toolStrip = menuItem.GetToolStrip();
                foreach (ToolStripItem ts in toolStrip.Items)
                {
                    ts.Tag = "Game";
                }

                ToolStripManager.Merge(toolStrip, this.toolStrip1);
                toolStrip = null;

                MenuStrip menuStrip = menuItem.GetMenuStrip();

                if (menuStrip.Items.Count > 0)
                {
                    menuStrip.Items[0].Tag = "Game";
                    this.menuStrip1.Items.Insert(0, menuStrip.Items[0]);
                }

                menuStrip = null;

                menuItem = null;
            }
        }

        /// <summary>
        /// Attempts to load the server from 
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                // Determine the Server type.
                string serverType = Properties.Settings.Default.Server;
                switch (serverType)
                {
                    case "DropBox":
                        AppObjects.Server = new DropBox();
                        AppObjects.Server.Root = Properties.Settings.Default.Root;
                        break;
                    case "Local File System":
                        AppObjects.Server = new LocalFileSystem();
                        AppObjects.Server.Root = AppDomain.CurrentDomain.BaseDirectory;
                        break;
                    default:
                        AppObjects.Server = new LocalFileSystem();
                        AppObjects.Log.Error("Type of server cannot be determined. Defaulting to Local File System.");
                        break;
                }

                this.useDefaults = Properties.Settings.Default.UseDefaults;
                AppObjects.User.Name = Properties.Settings.Default.UserID;
                AppObjects.User.Password = Properties.Settings.Default.Password;
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

        }

        /// <summary>
        /// Save the config values back to the file.
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                if (AppObjects.Server != null)
                {
                    Properties.Settings.Default["Server"] = AppObjects.Server.Name;
                    Properties.Settings.Default["Root"] = AppObjects.Server.Root;
                }

                if (this.useDefaults)
                {
                    Properties.Settings.Default["UserID"] = AppObjects.User.Name;
                    Properties.Settings.Default["Password"] = AppObjects.User.Password;
                    Properties.Settings.Default["UseDefaults"] = this.useDefaults;
                }

                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }
        }
        #endregion
    }
}
