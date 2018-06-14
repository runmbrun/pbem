// -----------------------------------------------------------------------
// <copyright file="DropBox.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Dropbox.Api;
    using Dropbox.Api.Files;

    // <summary>
    /// App folder name: PBem-BB
    /// App key: rxo8y7md82zgr33
    /// App secret: g1yf1oy8407b3nf
    /// Access token: 5WgKsxAbHzAAAAAAAAAAvFGSHNPaZ3z4yHOer3jf3F5IeK-rU8f3vCOMXtSvyokr
    /// </summary>
    public class DropBox : ServerModuleBase
    {
        #region " Private Variables "
        /// <summary>
        /// Stores the drop box client.
        /// </summary>
        private DropboxClient dbc = null;
        #endregion

        #region " Class Constructor "
        /// <summary>
        /// Initializes a new instance of the <see cref="DropBox"/> class.
        /// </summary>
        public DropBox()
        {
            // Setup the DropBox Client
            if (this.dbc == null)
            {
                try
                {
                    this.dbc = new DropboxClient("5WgKsxAbHzAAAAAAAAAAvFGSHNPaZ3z4yHOer3jf3F5IeK-rU8f3vCOMXtSvyokr");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            // Setup the name
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.Name = "Drop Box";
            }
        }
        #endregion

        #region " Public Functions "
        /// <summary>
        /// Attempts to get user information to test the connection.
        /// </summary>
        /// <returns>True if the connection was successful.</returns>
        public override bool Connect()
        {
            bool result = false;

            if (this.dbc != null)
            {
                try
                {
                    var task = Task.Run((Func<Task<bool>>)this.GetCurrentAccountAsync);
                    task.Wait();
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    // Don't display this error as it just means we can't connect to DropBox and
                    //  this error message will just be more confusing.  
                    // Calling function will display a more user friendly error.
                    AppObjects.Log.LogException(ex, false);
                }
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
                    if (MessageBox.Show($"It looks like the user [{AppObjects.User.Name}] is a first time user. Would you like this user to be added?") == DialogResult.Yes)
                    {
                        DataRow newRow = this.data.Tables["Accounts"].NewRow();
                        newRow["UserID"] = AppObjects.User.Name;
                        newRow["Password"] = AppObjects.User.Password;
                        this.data.Tables["Accounts"].Rows.Add(newRow);
                        //AppObjects.Server.UploadFile();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DataTable ListFolders(string path)
        {
            DataTable result = null;

            if (this.dbc != null)
            {
                try
                {
                    var task = Task.Run(() => this.ListFoldersAsync(path));
                    task.Wait();
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Delete(string path)
        {
            bool result = false;

            if (this.dbc != null)
            {
                try
                {
                    var task = Task.Run(() => this.DeleteAsync(path));
                    task.Wait();
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CreateFolder(string path)
        {
            bool result = false;

            if (this.dbc != null)
            {
                try
                {
                    var task = Task.Run(() => this.CreateNewFolderAsync(path));
                    task.Wait();
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DownloadFile(string remote, string local)
        {
            bool result = false;

            if (this.dbc != null)
            {
                try
                {
                    var task = Task.Run(() => this.DownloadFileAsync(remote, local));
                    task.Wait();
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool UploadFile(string remote, string local)
        {
            bool result = false;

            if (this.dbc != null)
            {
                try
                {
                    var task = Task.Run(() => this.UploadFileAsync(remote, local));
                    task.Wait();
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    AppObjects.Log.LogException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IServerExplorer GetServerExplorer()
        {
            if (base.explorer == null)
            {
                base.explorer = new DropBoxExplorer();
            }

            return base.explorer;
        }
        #endregion

        #region " Private Async Functions "
        /// <summary>
        /// Gets the current user account information.
        /// </summary>
        /// <returns>True if successful.</returns>
        private async Task<bool> GetCurrentAccountAsync()
        {
            bool result = false;

            try
            {
                var full = await this.dbc.Users.GetCurrentAccountAsync();
                string text = $"{full.Name.DisplayName} - {full.Email}";
                Console.WriteLine(text);

                if (this.data == null)
                {
                    string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.dat");

                    // First see if one exists
                    if (File.Exists(fileName))
                    {
                        // Unserialize file
                        //this.DeserializeFile(fileName);
                    }
                    else
                    {
                        //this.CreateNewDataSet();
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                // Don't display this error as it just means we can't connect to DropBox and
                //  this error message will just be more confusing.  
                // Calling function will display a more user friendly error.
                AppObjects.Log.LogException(ex, false);
            }

            return result;
        }

        /// <summary>
        /// Lists the items within a folder.
        /// </summary>
        /// <remarks>This demonstrates calling an RPC style API in the Files namespace.</remarks>
        /// <param name="path">The path to list.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        private async Task<DataTable> ListFoldersAsync(string path = "")
        {
            DataTable result = null;

            try
            {
                var list = new ListFolderResult();
                list = await this.dbc.Files.ListFolderAsync(path);

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
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    Console.WriteLine("D  {0}/", item.Name);
                    DataRow row = result.NewRow();
                    row[0] = item.Name;
                    row[1] = false;
                    result.Rows.Add(row);
                }

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    var file = item.AsFile;
                    Console.WriteLine("F{0,8} {1}", file.Size, item.Name);
                    DataRow row = result.NewRow();
                    row[0] = item.Name;
                    row[1] = true;
                    result.Rows.Add(row);
                }

                if (list.HasMore)
                {
                    AppObjects.Log.Log("   ...");
                }
                else
                {
                    AppObjects.Log.Log("--- End Files ---");
                }
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Creates the specified folder.
        /// </summary>
        /// <remarks>This demonstrates calling an RPC style API in the Files namespace.</remarks>
        /// <param name="path">The path of the folder to create.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        private async Task<bool> CreateNewFolderAsync(string path)
        {
            bool result = false;
            
            try
            {
                AppObjects.Log.Log("--- Creating Folder ---");
                var folderArg = new CreateFolderArg(path);
                var folder = await this.dbc.Files.CreateFolderV2Async(folderArg);
                AppObjects.Log.Log("Folder: " + path + " created!");
                result = true;
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            return result;
        }

        /// <summary>
        /// Deletes the specified file or folder.
        /// </summary>
        /// <remarks>This demonstrates calling an RPC style API in the Files namespace.</remarks>
        /// <param name="path">The path of the folder to create.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        private async Task<bool> DeleteAsync(string path)
        {
            bool result = false;

            try
            {
                AppObjects.Log.Log("--- Deleting ---");
                var deleteArg = new DeleteArg(path);
                var folder = await this.dbc.Files.DeleteV2Async(deleteArg);
                AppObjects.Log.Log($"{path} deleted!");
                result = true;
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            AppObjects.Log.Log("Delete Done!");

            return result;
        }

        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <remarks>This demonstrates calling a download style API in the Files namespace.</remarks>
        /// <param name="input">The folder path in which the file should be found.</param>
        /// <param name="output">The file and path in which to create the file to download.</param>
        /// <returns>The Async task.</returns>
        private async Task<bool> DownloadFileAsync(string remote, string local)
        {
            bool result = false;
            AppObjects.Log.Log("Download file...");

            try
            {
                using (var response = await this.dbc.Files.DownloadAsync(remote))
                {
                    AppObjects.Log.Log($"Downloaded {response.Response.Name} Rev {response.Response.Rev}");
                    AppObjects.Log.Log("------------------------------");

                    // Todo: to view the files as text...
                    // AppObjects.Log.Log(await response.GetContentAsStringAsync());

                    // To output the file to the local machine
                    using (var fileStream = File.Create(local))
                    {
                        response.GetContentAsStreamAsync().Result.CopyTo(fileStream);
                    }

                    AppObjects.Log.Log("------------------------------");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            return result;
        }

        /// <summary>
        /// Uploads given content to a file in Drop box.
        /// </summary>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="fileContent">The file content.</param>
        /// <returns>The Async task.</returns>
        private async Task<bool> UploadFileAsync(string remote, string local)
        {
            bool result = false;

            try
            {
                AppObjects.Log.Log("Upload file...");
                using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(File.ReadAllText(local))))
                {
                    var response = await this.dbc.Files.UploadAsync(remote, WriteMode.Overwrite.Instance, body: stream);
                    AppObjects.Log.Log($"Uploaded Id {response.Id} Rev {response.Rev}");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                AppObjects.Log.LogException(ex);
            }

            AppObjects.Log.Log("Upload file Done!");

            return result;
        }

        /// <summary>
        /// Uploads a big file in chunk. The is very helpful for uploading large file in slow network condition
        /// and also enable capability to track upload progress.
        /// </summary>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The Async task.</returns>
        private async Task ChunkUpload(string remote, string local)
        {
            // Chunk size is 128KB.
            const int ChunkSize = 128 * 1024;

            AppObjects.Log.Log("Chunk upload file...");

            // Create a random file of 1MB in size.
            var fileContent = new byte[1024 * 1024];
            new Random().NextBytes(fileContent);

            using (var stream = new MemoryStream(fileContent))
            {
                int numChunks = (int)Math.Ceiling((double)stream.Length / ChunkSize);

                byte[] buffer = new byte[ChunkSize];
                string sessionId = null;

                for (var idx = 0; idx < numChunks; idx++)
                {
                    AppObjects.Log.Log($"Start uploading chunk {idx}");
                    var byteRead = stream.Read(buffer, 0, ChunkSize);

                    using (MemoryStream memStream = new MemoryStream(buffer, 0, byteRead))
                    {
                        if (idx == 0)
                        {
                            var result = await this.dbc.Files.UploadSessionStartAsync(body: memStream);
                            sessionId = result.SessionId;
                        }
                        else
                        {
                            UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(ChunkSize * idx));

                            if (idx == numChunks - 1)
                            {
                                await this.dbc.Files.UploadSessionFinishAsync(cursor, new CommitInfo(remote), memStream);
                            }
                            else
                            {
                                await this.dbc.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                            }
                        }
                    }
                }

                AppObjects.Log.Log("ChunkUpload file Done!");
            }
        }
        #endregion
    }
}
