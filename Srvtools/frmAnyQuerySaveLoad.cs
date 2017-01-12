using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace Srvtools
{
    public partial class frmAnyQuerySaveLoad : Form
    {
        private InfoBindingSource BindingSource;
        private String Status;
        private String QueryID = String.Empty;
        public String fileName = String.Empty;
        public bool isOK = false;
        public frmAnyQuerySaveLoad()
        {
            InitializeComponent();
        }

        public frmAnyQuerySaveLoad(InfoBindingSource ibs, String status, String queryID)
        {
            BindingSource = ibs;
            Status = status;
            QueryID = queryID;
            InitializeComponent();
        }

        private void frmQuerySaveLoad_Load(object sender, EventArgs e)
        {
            if (Status == "Save")
            {
                this.cbSaveLoad.Visible = true;
                this.lbLoad.Visible = false;
            }
            else if (Status == "Load")
            {
                this.lbLoad.Visible = true;
                this.cbSaveLoad.Visible = false;
                this.lbLoad.Height = 200;

                this.Height = 350;
                this.buttonOK.Location = new Point(this.buttonOK.Location.X, 280);
                this.buttonCancel.Location = new Point(this.buttonCancel.Location.X, 280);
                this.buttonDelete.Location = new Point(this.buttonDelete.Location.X, 280);
            }

            this.cbSaveLoad.Items.Clear();
            this.lbLoad.Items.Clear();

            labelMessage.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveLoadMessage");

            object[] param = new object[1];
            param[0] = QueryID;
            object[] myRet = CliUtils.CallMethod("GLModule", "AnyQueryLoadFile", param);

            DataSet dsLoadFile = new DataSet();
            if (myRet != null && myRet[0].ToString() == "0")
                dsLoadFile = myRet[1] as DataSet;
            foreach (DataRow dr in dsLoadFile.Tables[0].Rows)
            {
                cbSaveLoad.Items.Add(dr[0].ToString());
                lbLoad.Items.Add(dr[0].ToString());
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (Status == "Save")
            {
                bool isExist = this.cbSaveLoad.Items.Contains(this.cbSaveLoad.Text);
                if (isExist)
                {
                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveWarning");
                    if (MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }
                isOK = true;
                fileName = this.cbSaveLoad.Text;
            }
            else if (Status == "Load")
            {
                bool isExist = false;
                if (this.lbLoad.SelectedItem != null)
                    isExist = this.lbLoad.Items.Contains(this.lbLoad.SelectedItem.ToString());
                if (!isExist)
                {
                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "LoadWarning");
                    MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                isOK = true;

                if (this.lbLoad.SelectedItem != null)
                    fileName = this.lbLoad.SelectedItem.ToString();
            }

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (Status == "Save")
            {
                fileName = this.cbSaveLoad.Text;
            }
            else if (Status == "Load")
            {
                if (this.lbLoad.SelectedItem != null)
                    fileName = this.lbLoad.SelectedItem.ToString();
            }

            if (fileName != String.Empty)
            {
                String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "DeleteSure");
                message = String.Format(message, fileName);
                if (MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    object[] param = new object[2];
                    param[0] = QueryID;
                    param[1] = fileName;
                    CliUtils.CallMethod("GLModule", "AnyQueryDeleteFile", param);

                    this.cbSaveLoad.Text = String.Empty;
                    frmQuerySaveLoad_Load(null, new EventArgs());
                }
            }
        }
    }
}