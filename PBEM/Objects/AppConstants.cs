
namespace PBEM
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.AccessControl;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;

    internal class AppConstants : IAppConstants
    {
        /// <summary>
        /// Settings information
        /// </summary>
        private XmlDocument settings = null;

        /// <summary>
        /// The full path to the shared application setting file.
        /// </summary>
        private string applicationSettingsFile = string.Empty;

        /// <summary>
        /// Dictionary of module settings
        /// </summary>
        private Dictionary<string, object> moduleSettings = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppConstants"/> class.
        /// </summary>
        public AppConstants()
        {
            this.LoadConstants();
        }

        #region " Public Properties "
        /// <summary>
        /// Gets or sets a list of all the modules currently loaded by the application.
        /// </summary>
        public List<IGameModule> LoadedModules
        {
            get;
            set;
        }
        = new List<IGameModule>();

        /// <summary>
        /// Gets or sets a list of modules that need to be loaded based on the configuration file.
        /// </summary>
        public List<string> ModulesToBeLoaded
        {
            get;
            set;
        }
        = new List<string>();
        #endregion

        #region " IApplicationConstants "
        /// <summary>
        /// Gets a value indicating the password to use to access the import file archive file.
        /// </summary>
        public string ArchivePassword
        {
            // todo: store password for Dropbox here?
            get
            {
                return this.GetValue("ArchivePassword");
            }
        }

        /// <summary>
        /// Gets a value indicating the last release of the prior software version
        /// </summary>
        public string PriorRelease
        {
            get
            {
                return this.GetValue("PriorRelease");
            }
        }

        /// <summary>
        /// Gets a value indicating the prior version of the software
        /// </summary>
        public string PriorVersion
        {
            get
            {
                return this.GetValue("PriorVersion");
            }
        }

        /// <summary>
        /// Gets a value indicating the release of the software
        /// </summary>
        public string SoftwareRelease
        {
            get
            {
                return this.GetValue("SoftwareRelease");
            }
        }

        /// <summary>
        /// Gets a value indicating the version of the software
        /// </summary>
        public string SoftwareVersion
        {
            get
            {
                return this.GetValue("SoftwareVersion");
            }
        }

        /// <summary>
        /// Gets a value indicating the full path to the folder where the application configuration files are stored.
        /// </summary>
        public string ApplicationConfigFolder
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating the path to the executable as stored in the settings file.
        /// </summary>
        public string ExecutablePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines if a specific financial program module is loaded.
        /// </summary>
        /// <param name="identitycode">Unique identifier for the module to look for.</param>
        /// <returns>A flag indicating if the module is currently loaded (true) or not (false)</returns>
        public bool IsModuleLoaded(string identitycode)
        {
            return this.LoadedModules.Where((t) => string.Compare(t.IdentityCode, identitycode, true) == 0).FirstOrDefault() != null;
        }

        /// <summary>
        /// Enumerator to list out the loaded modules.
        /// </summary>
        /// <returns>An enumerator allowing listing out all loaded modules.</returns>
        public List<IGameModule> Modules()
        {
            return this.LoadedModules;
        }

        /// <summary>
        /// Gets a settings value
        /// </summary>
        /// <param name="identifier">The identifier of the setting</param>
        /// <returns>The value for the setting or an empty string.</returns>
        public string GetValue(string identifier)
        {
            string result = string.Empty;

            if (this.settings == null)
            {
                this.LoadConstants();
            }

            if (this.settings != null)
            {
                XmlNode v = this.settings.DocumentElement.SelectSingleNode(string.Format(@"Constant[@Name='{0}']", identifier));
                if (v != null)
                {
                    result = v.Attributes["Value"].Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets an integer constant value.
        /// </summary>
        /// <param name="identifier">The identifier of the setting</param>
        /// <param name="defaultvalue">Default value if the setting is not in the file.</param>
        /// <returns>The value for the setting or an empty string.</returns>
        public int GetValue(string identifier, int defaultvalue = 0)
        {
            int result = defaultvalue;

            if (this.settings == null)
            {
                this.LoadConstants();
            }

            if (this.settings != null)
            {
                XmlNode v = this.settings.DocumentElement.SelectSingleNode(string.Format(@"Constant[@Name='{0}']", identifier));
                if (v != null)
                {
                    result = Convert.ToInt32(v.Attributes["Value"].Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the icon for the application
        /// </summary>
        /// <param name="width">The width of the application (16, 32, 48)</param>
        /// <param name="height">The height of the application (16, 32, 48)</param>
        /// <returns>The application Icon object</returns>
        public Icon GetApplicationIcon(int width, int height)
        {
            Icon result = null;
            string icon = this.GetValue("ApplicationIcon");
            if (!string.IsNullOrEmpty(icon))
            {
                // First ensure that the image is an encoded file.
                Match m = Regex.Match(icon, @"\[ContentType\:\.(?<FileType>\w+)\](?<FileContents>.+)");
                if (m.Success)
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(m.Groups["FileContents"].Value)))
                    {
                        if (string.Compare(m.Groups["FileType"].Value, "ico", true) == 0)
                        {
                            result = new Icon(ms, width, height);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Saves Constant file to the stream
        /// </summary>
        /// <param name="identifier">The identifier of the setting</param>
        /// <param name="dataout">The stream to write the constant file data to.</param>
        /// <returns>A flag indicating whether the data was successfully written to the stream.</returns>
        public bool GetConstantFile(string identifier, Stream dataout)
        {
            bool result = false;
            if ((dataout != null) && dataout.CanWrite)
            {
                string fileData = this.GetValue(identifier);
                if (!string.IsNullOrEmpty(fileData))
                {
                    // First ensure that the image is an encoded file.
                    Match m = Regex.Match(fileData, @"\[ContentType\:\.(?<FileType>\w+)\](?<FileContents>.+)");
                    if (m.Success)
                    {
                        byte[] contents = Convert.FromBase64String(m.Groups["FileContents"].Value);
                        dataout.Write(contents, 0, contents.Length);
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Allows a module to put shared constants in the global constants.
        /// </summary>
        /// <param name="module">Identifier for the module</param>
        /// <param name="name">Name of the constant</param>
        /// <param name="value">Value of the constant</param>
        /// <returns>A flag indicating if the module constant was successfully set.</returns>
        public bool SetModuleConstant(string module, string name, object value)
        {
            bool result = false;
            if (!this.moduleSettings.ContainsKey($"{module}.{name}"))
            {
                this.moduleSettings[$"{module}.{name}"] = value;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Gets a module shared constant
        /// </summary>
        /// <typeparam name="T">The data type of the constant</typeparam>
        /// <param name="module">Identifier for the module</param>
        /// <param name="name">Name of the constant</param>
        /// <returns>Gets the value of the module constant.</returns>
        public T GetModuleConstant<T>(string module, string name)
        {
            T result = default(T);
            if (this.moduleSettings.ContainsKey($"{module}.{name}"))
            {
                if (this.moduleSettings[$"{module}.{name}"] is T)
                {
                    result = (T)this.moduleSettings[$"{module}.{name}"];
                }
            }

            return result;
        }
        #endregion

        #region " Public Functions "
        
        #endregion

        #region " Private Functions "
        /// <summary>
        /// Loads the constants from the application database
        /// </summary>
        private void LoadConstants()
        {
            // Load the shared users program settings.
            string[] pathSegments = new string[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "EDESuite",
                $"TEST",
                this.GetAssemblyDescription(),
            };

            this.ApplicationConfigFolder = Path.Combine(pathSegments);
            this.applicationSettingsFile = Path.Combine(this.ApplicationConfigFolder, "EDESuite.xml");
            XmlDocument doc = new XmlDocument();
            if (File.Exists(this.applicationSettingsFile))
            {
                doc.Load(this.applicationSettingsFile);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("x", doc.DocumentElement.NamespaceURI);
                XmlNode n = doc.DocumentElement.SelectSingleNode("x:Database", nsmgr);

                if (n != null)
                {
                    //this.SchoolDatabaseFile = n.Attributes["Path"].Value;
                }

                n = doc.DocumentElement.SelectSingleNode("x:Executable", nsmgr);

                if (n != null)
                {
                    this.ExecutablePath = n.Attributes["Path"].Value;
                }

                foreach (XmlNode n2 in doc.DocumentElement.SelectNodes("x:InstalledModules/x:Module[@Load='true']", nsmgr))
                {
                    this.ModulesToBeLoaded.Add(n2.Attributes["Name"].Value);
                }
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(this.applicationSettingsFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(this.applicationSettingsFile));
                }

                // Generate a new file
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Indent = true;
                settings.NewLineHandling = NewLineHandling.Entitize;
                settings.NewLineOnAttributes = false;
                using (XmlWriter writer = XmlWriter.Create(this.applicationSettingsFile, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("EDESuite", "http://tempuri.org/EDESuiteAppSettings.xsd");
                    writer.WriteStartElement("Executable");
                    writer.WriteAttributeString("Path", Path.GetDirectoryName(typeof(AppConstants).Assembly.Location));
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                try
                {
                    // Set the file security settings so all users can modify the file.
                    FileSecurity fileSecurity = File.GetAccessControl(this.applicationSettingsFile);
                    foreach (FileSystemAccessRule r in fileSecurity.GetAccessRules(false, true, typeof(System.Security.Principal.NTAccount)))
                    {
                        if (string.Compare(r.IdentityReference.Value, "BUILTIN\\Users", true) == 0)
                        {
                            fileSecurity.RemoveAccessRule(r);
                        }
                    }

                    FileSystemAccessRule accessRule = new FileSystemAccessRule("BUILTIN\\Users", FileSystemRights.FullControl, AccessControlType.Allow);
                    fileSecurity.AddAccessRule(accessRule);
                    File.SetAccessControl(this.applicationSettingsFile, fileSecurity);
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                    this.settings = null;
                }
            }
        }

        /// <summary>
        /// Retrieves the assembly description attribute.
        /// </summary>
        /// <returns>The description value from the assembly file.</returns>
        private string GetAssemblyDescription()
        {
            string result = string.Empty;
            Assembly assembly = typeof(AppConstants).Assembly;
            var descriptionAttribute = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
            if (descriptionAttribute != null)
            {
                result = descriptionAttribute.Description.TrimStart(new char[] { ' ', '-' });
            }

            return result;
        }

        /// <summary>
        /// Saves the constant information back to the application database.
        /// </summary>
        private void SaveConstants()
        {
            try
            {
                // todo:
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }
        }
    }
    #endregion
}

