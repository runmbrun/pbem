// -----------------------------------------------------------------------
// <copyright file="DropBoxExplorer.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM
{
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public class DropBoxExplorer : IServerExplorer
    {
        private UserControl userControl = null;

        public UserControl GetUserControl()
        {
            if (this.userControl == null)
            {
                this.userControl = new DropBoxExplorerUI();
            }

            return this.userControl;
        }
    }
}
