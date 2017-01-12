/*  filename:       frmClientQuery.cs 
 *  version:        3.1
 *  lastedittime:   14:47 9/5/2006
 *  remark:
 *  1.correct the mistake of valuemember and displaymember in refvalcolumn and combbox                              at 13:41 28/4/2006 
 *  2.correct the alignment of the labels                                                                           at 9:21 30/4/2006
 *  3 change the display of form                                                                                    at 11:26 30/4/2006
 *  4.correct the mistake of the judgement of  typeof data                                                          at 9:32 8/5/2006
 *  5 change the display of split                                                                                   at 10:10 8/5/2006
 *  6 use new inforefvalbox and solve the problem of refvalcolumn by edit the file("InfoTextBox" & "InfoRefvalBox") at 13:33 8/5/2006
 *   detail: (1)in GetCurrentDataRow(): add new judgement whether the property "SelectedValue" has be bonded
 *           (2)add new property "SelectedValue" into Control inforefvalbox
 *  7.correct the mistake of alignment of the textbox                                                               at 17:12 8/5/2006 
 *  8.correct the mistake of the label text display                                                                 at 14:47 9/5/2006
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace Srvtools
{
    public partial class frmClientQuery : Form
    {
        #region Var

        ClientQuery cqFormQuery;
        public Button btnCancel = new Button();
        public Button btnOk = new Button();
        bool isPreview = false;

        #endregion

        #region Constructor

        public frmClientQuery(ClientQuery cq, bool ispreview)
        {
            this.Text = cq.Caption;
            cqFormQuery = cq;
            isPreview = ispreview;
            InitializeComponent();
        }

        private void frmClientQuery_Load(object sender, EventArgs e)
        {
            cqFormQuery.Show(splitContainer1.Panel1);
            splitContainer1.Panel1.Enabled = !isPreview;
            int bottom = 0;
            int right = 0;
            foreach (Control control in splitContainer1.Panel1.Controls)
            {
                bottom = Math.Max(bottom, control.Bottom);
                right = Math.Max(right, control.Right);
            }

            this.Size = new Size(right + cqFormQuery.Margin.Right, bottom + cqFormQuery.Margin.Bottom + 82);
            string caption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "NavText");
            for (int i = 0; i < 7; i++)
            {
                caption = caption.Substring(caption.IndexOf(";") + 1);
            }
            btnOk.Name = "btnOk";
            btnOk.Text = caption.Substring(0, caption.IndexOf(";"));
            caption = caption.Substring(caption.IndexOf(";") + 1);
            btnOk.Location = new System.Drawing.Point((this.Width / 2 - 95), 12);
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Size = new System.Drawing.Size(75, 26);
            btnOk.TabIndex = 0;
            btnCancel.Name = "btnCancel";
            btnCancel.Text = caption.Substring(0, caption.IndexOf(";"));
            btnCancel.Location = new System.Drawing.Point((this.Width / 2 + 20), 12);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Size = new System.Drawing.Size(75, 26);
            btnCancel.TabIndex = 1;

            this.splitContainer1.Panel2.Controls.Add(btnOk);
            this.splitContainer1.Panel2.Controls.Add(btnCancel);
        }


        #endregion
    }
}
