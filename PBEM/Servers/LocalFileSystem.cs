// -----------------------------------------------------------------------
// <copyright file="LocalFileSystem.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class LocalFileSystem : ServerModuleBase
    {
        #region " Private Variables "
        #endregion

        public LocalFileSystem()
        {
            // Setup the name
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.Name = "Local File System";
            }

            this.Root = AppDomain.CurrentDomain.BaseDirectory;
        }

        #region " Public Override Functions "
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Connect()
        {
            bool result = false;

            // No action necessary for Local File System
            if (this.data == null)
            {
                string fileName = Path.Combine(this.Root, this.AccountsFileName);

                // First see if one exists
                if (File.Exists(fileName))
                {
                    // Unserialize file
                    result = this.DeserializeFile(fileName);
                }
                else
                {
                    result = this.CreateNewDataSet();
                }
            }
            else
            {
                // Already connected!
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Logon()
        {
            bool result = false;

            if (this.data.Tables["Accounts"] != null && !string.IsNullOrWhiteSpace(AppObjects.User.Name) && !string.IsNullOrWhiteSpace(AppObjects.User.Password))
            {
                if (this.data.Tables["Accounts"].Select($"userid = '{AppObjects.User.Name}' and password = '{AppObjects.User.Password}'").Length > 0)
                {
                    result = true;
                }
                else
                {
                    if (MessageBox.Show($"It looks like the user [{AppObjects.User.Name}] is a first time user. Would you like this user to be added?", "Add User?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // Add the new user
                        DataRow newRow = this.data.Tables["Accounts"].NewRow();
                        newRow["UserID"] = AppObjects.User.Name;
                        newRow["Password"] = AppObjects.User.Password;
                        this.data.Tables["Accounts"].Rows.Add(newRow);

                        // Now Save to file
                        result = this.SerializeToFile(Path.Combine(this.Root, this.AccountsFileName));
                    }
                }
            }

            return result;
        }

        private bool SerializeToFile(string fileName)
        {
            bool result = false;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(this.data.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, this.data);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }

                // Success!
                result = true;
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            return result;
        }

        private bool DeserializeFile(string fileName)
        {
            bool result = false;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        this.data = (DataSet)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();

                    // Success!
                    result = true;
                }
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            return result;
        }

        private bool CreateNewDataSet()
        {
            bool result = false;

            try
            {
                // Create the new DataSet
                this.data = new DataSet();

                // Create the Accounts table
                DataTable accounts = new DataTable("Accounts");
                DataColumn col = new DataColumn("UserID");
                accounts.Columns.Add(col);
                col = new DataColumn("Password");
                accounts.Columns.Add(col);
                this.data.Tables.Add(accounts);

                // Success!
                result = true;
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            return result;
        }

        public override bool CreateFolder(string path)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(string path)
        {
            throw new NotImplementedException();
        }

        public override bool DownloadFile(string remote, string local)
        {
            throw new NotImplementedException();
        }

        public override DataTable ListFolders(string path)
        {
            DataTable result = null;

            try
            {
                AppObjects.Log.Log("--- Files ---");
                result = new DataTable();
                result.Columns.Add("Name");
                result.Columns.Add("IsFile", typeof(bool));

                if (!string.IsNullOrWhiteSpace(path))
                {
                    // Root - Add the "..."
                    DataRow row = result.NewRow();
                    row[0] = "...";
                    row[1] = false;
                    result.Rows.Add(row);
                }

                // show folders then files
                foreach (string dir in Directory.GetDirectories(path))
                {
                    Console.WriteLine("D  {0}/", dir);
                    DataRow row = result.NewRow();
                    row[0] = dir;
                    row[1] = false;
                    result.Rows.Add(row);
                }

                foreach (string file in Directory.GetFiles(path))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine("F{0,8} {1}", fileInfo.Length, fileInfo.Name);
                    DataRow row = result.NewRow();
                    row[0] = file;
                    row[1] = true;
                    result.Rows.Add(row);
                }

                AppObjects.Log.Log("--- End Files ---");
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
                result = null;
            }

            return result;
        }

        public override bool UploadFile(string remote, string local)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IServerExplorer GetServerExplorer()
        {
            if (base.explorer == null)
            {
                base.explorer = new LocalFileSystemExplorer();
            }

            return base.explorer;
        }
        #endregion
    }
}
