// -----------------------------------------------------------------------
// <copyright file="DropBoxExplorerUI.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Data;
    using System.Windows.Forms;

    public partial class DropBoxExplorerUI : UserControl
    {
        /// <summary>
        /// Stores the current folder that is being displayed.
        /// </summary>
        private string currentFolder = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DropBoxExplorerUI()
        {
            this.InitializeComponent();
            this.ListFolders();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.textBoxCreateFolder.Text))
            {
                string newFolder = string.IsNullOrWhiteSpace(this.currentFolder) ? this.textBoxCreateFolder.Text : $"{this.currentFolder}/{this.textBoxCreateFolder.Text}";
                if (AppObjects.Server.CreateFolder(newFolder))
                {
                    this.ListFolders(this.currentFolder);
                }
            }
            else
            {
                this.ListFolders(this.currentFolder);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUploadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    AppObjects.Server.UploadFile($"{this.currentFolder}/{dlg.SafeFileName}", dlg.FileName);
                    this.ListFolders(this.currentFolder);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string path = string.Empty;

            // Find out what is selected
            if (this.listBox1.SelectedIndex != -1)
            {
                path = $"{this.currentFolder}/{((System.Data.DataRowView)(this.listBox1.SelectedItems[0])).Row[0].ToString()}";
            }

            if (!string.IsNullOrWhiteSpace(path))
            {
                AppObjects.Server.Delete(path);
                this.ListFolders(this.currentFolder);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string newPath = string.Empty;
            bool isFile = false;

            if (((DataRowView)((ListBox)sender).SelectedItem) != null)
            {
                newPath = ((DataRowView)((ListBox)sender).SelectedItem).Row[0].ToString();
                isFile = (bool)((DataRowView)((ListBox)sender).SelectedItem).Row[1];
            }

            if (newPath == "...")
            {
                // Go UP a folder
                if (this.currentFolder.LastIndexOf('/') != -1 && this.currentFolder.LastIndexOf('/') != 0)
                {
                    this.currentFolder = this.currentFolder.Substring(0, this.currentFolder.LastIndexOf('/'));
                }
                else
                {
                    // Back to root
                    this.currentFolder = string.Empty;
                }

                this.ListFolders(this.currentFolder);
            }
            else if (!string.IsNullOrWhiteSpace(newPath))
            {
                if (!isFile)
                {
                    // Go DOWN a folder
                    this.currentFolder = $"{(string.IsNullOrWhiteSpace(this.currentFolder) ? string.Empty : this.currentFolder)}/{newPath}";
                    this.ListFolders(this.currentFolder);
                }
                else
                {
                    // Download the file
                    using (SaveFileDialog dlg = new SaveFileDialog())
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            AppObjects.Server.DownloadFile(this.currentFolder + "/" + newPath, dlg.FileName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void ListFolders(string path = "")
        {
            DataTable dt = AppObjects.Server.ListFolders(path);
            this.listBox1.DataSource = dt;
            this.listBox1.DisplayMember = "Name";
            this.listBox1.ValueMember = "isFile";
        }
    }
}
