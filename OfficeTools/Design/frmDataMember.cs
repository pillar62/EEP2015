using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Srvtools;

namespace OfficeTools.Design
{
    /// <summary>
    /// The class of frmDataMember, used to edit datamember
    /// </summary>
    public partial class frmDataMember : Form
    {
        /// <summary>
        /// Create a new instance of frmDataMember
        /// </summary>
        /// <param name="objmember">The list of datamember</param>
        /// <param name="value">The selected datamember</param>
        public frmDataMember(ArrayList objmember, string value)
        {
            InitializeComponent();
            for (int i = 0; i < objmember.Count; i++)
            {
                lbDataMember.Items.Add(objmember[i].ToString());
            }
           
            if (lbDataMember.Items.Contains(value))
            {
                lbDataMember.SelectedItem = value;
            }

        }

        private void frmDataMember_Load(object sender, EventArgs e)
        {
            #region setup language
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.Text = "DataMember" + OfficeTools.Properties.Resources.editor.Split(',')[lang];
            this.btnOK.Text = OfficeTools.Properties.Resources.btnOK.Split(',')[lang];
            this.btnCancel.Text = OfficeTools.Properties.Resources.btnCancel.Split(',')[lang]; 
            #endregion
        }
    }
}