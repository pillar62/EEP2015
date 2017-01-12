using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace OfficeTools.RunTime
{
    /// <summary>
    /// The form to select mode of output
    /// </summary>
    public partial class frmOutputMode : Form
    {
        /// <summary>
        /// Create a new instance of frmOutputMode
        /// </summary>
        /// <param name="item">The office file type, etc excel, word</param>
        public frmOutputMode(string item)
        {
            InitializeComponent();
            this.cbMode.Items.AddRange(new object[] { "None", item, "Email" });
            cbMode.SelectedIndex = 0;
        }

        private void frmOutputMode_Load(object sender, EventArgs e)
        {
            #region setup lanuage
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.btnOK.Text = OfficeTools.Properties.Resources.btnOK.Split(',')[lang];
            this.btnCancel.Text = OfficeTools.Properties.Resources.btnCancel.Split(',')[lang]; 
            #endregion
        }

    }
}