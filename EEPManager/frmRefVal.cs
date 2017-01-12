using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Collections;

namespace EEPManager
{
    public partial class frmRefVal : Form
    {
        public frmRefVal()
        {
            InitializeComponent();
            SetViewState(true);
        }

        private void SetViewState(bool value)
        {
            tbCaption.Enabled = !value;
            tbDescription.Enabled = !value;
            tbDisplayMember.Enabled = !value;
            tbSelectAlias.Enabled = !value;
            tbSelectCommand.Enabled = !value;
            tbTableName.Enabled = !value;
            tbValueMember.Enabled = !value;
            btnConnect.Enabled = !value;

            lbRefVal.Enabled = value;
            idgRefVal.ReadOnly = value;

            pnAUD.Visible = value;
            pnOC.Visible = !value;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            frmAddRefVal frmadd = new frmAddRefVal();
            if (frmadd.ShowDialog() == DialogResult.OK)
            {
                if (frmadd.tbNewRefVal.Text.Trim() == "")
                {
                    MessageBox.Show("RefValNo can not be null");
                    return;
                }
                else
                {
                    int count = this.idsRefVal.RealDataSet.Tables[0].Select("REFVAL_NO='" + frmadd.tbNewRefVal.Text + "'").Length;
                    if (count > 0)
                    {
                        MessageBox.Show(frmadd.tbNewRefVal.Text + " has exsit!");
                        return;
                    }
                    DataRow dr = this.idsRefVal.RealDataSet.Tables[0].NewRow();
                    dr["REFVAL_NO"] = frmadd.tbNewRefVal.Text;
                    dr["DESCRIPTION"] = "";
                    dr["TABLE_NAME"] = "";
                    dr["CAPTION"] = "";
                    dr["DISPLAY_MEMBER"] = "";
                    dr["SELECT_ALIAS"] = "";
                    dr["SELECT_COMMAND"] = "";
                    dr["VALUE_MEMBER"] = "";
                    this.idsRefVal.RealDataSet.Tables[0].Rows.Add(dr);
                    this.idsRefVal.ApplyUpdates();
                    this.lbRefVal.SelectedValue = frmadd.tbNewRefVal.Text;
                    SetViewState(false);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lbRefVal.SelectedValue == null)
            {
                return;
            }
            SetViewState(false);
            InitialDVMember(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete " + lbRefVal.SelectedValue.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DataRow dr = this.idsRefVal.RealDataSet.Tables[0].Select("REFVAL_NO='" + lbRefVal.SelectedValue.ToString() + "'")[0];
                DataRow[] drd = this.idsRefVal.RealDataSet.Tables[1].Select("REFVAL_NO='" + lbRefVal.SelectedValue.ToString() + "'");
                dr.Delete();
                foreach (DataRow odr in drd)
                {
                    odr.Delete();
                }
                this.idsRefVal.ApplyUpdates();

            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataRow dr = this.idsRefVal.RealDataSet.Tables[0].Select("REFVAL_NO='" + lbRefVal.SelectedValue.ToString() + "'")[0];
            string[] value = new string[7]{ tbCaption.Text,
                                            tbDescription.Text,
                                            tbDisplayMember.Text,
                                            tbSelectAlias.Text,
                                            tbSelectCommand.Text,
                                            tbTableName.Text,
                                            tbValueMember.Text
                                            };
            dr["CAPTION"] = value[0];
            dr["DESCRIPTION"] = value[1];
            dr["DISPLAY_MEMBER"] = value[2];
            dr["SELECT_ALIAS"] = value[3];
            dr["SELECT_COMMAND"] = value[4];
            dr["TABLE_NAME"] = value[5];
            dr["VALUE_MEMBER"] = value[6];
            this.idsRefVal.ApplyUpdates();
            SetViewState(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.idsRefVal.RealDataSet.RejectChanges();
            DataRow dr = this.idsRefVal.RealDataSet.Tables[0].Select("REFVAL_NO='" + lbRefVal.SelectedValue.ToString() + "'")[0];
            tbCaption.Text = dr["CAPTION"].ToString();
            tbDescription.Text = dr["DESCRIPTION"].ToString();
            tbDisplayMember.Text = dr["DISPLAY_MEMBER"].ToString();
            tbSelectAlias.Text = dr["SELECT_ALIAS"].ToString();
            tbSelectCommand.Text = dr["SELECT_COMMAND"].ToString();
            tbTableName.Text = dr["TABLE_NAME"].ToString();
            tbValueMember.Text = dr["VALUE_MEMBER"].ToString();
            SetViewState(true);
        }

        private void lbRefVal_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lbRefVal.SelectedValue != null)
            {
                DataRow dr = this.idsRefVal.RealDataSet.Tables[0].Select("REFVAL_NO='" + lbRefVal.SelectedValue.ToString() + "'")[0];
                tbCaption.Text = dr["CAPTION"].ToString();
                tbDescription.Text = dr["DESCRIPTION"].ToString();
                tbDisplayMember.Text = dr["DISPLAY_MEMBER"].ToString();
                tbSelectAlias.Text = dr["SELECT_ALIAS"].ToString();
                tbSelectCommand.Text = dr["SELECT_COMMAND"].ToString();
                tbTableName.Text = dr["TABLE_NAME"].ToString();
                tbValueMember.Text = dr["VALUE_MEMBER"].ToString();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            InitialDVMember(true);
        }

        private void frmRefVal_Load(object sender, EventArgs e)
        {
            object[] myRet1 = CliUtils.CallMethod("GLModule", "GetDB", null);
            if (myRet1[1] != null && myRet1[1] is ArrayList)
            {
                ArrayList dbList = (ArrayList)myRet1[1];
                foreach (string db in dbList)
                {
                    tbSelectAlias.Items.Add(db);
                }
            }
        }

        private void InitialDVMember(bool showMessage)
        {
            tbDisplayMember.Items.Clear();
            tbValueMember.Items.Clear();
            if (tbSelectCommand.Text.Trim() != "" && tbSelectAlias.Text != "")
            {
                if (!tbSelectCommand.Text.Trim().StartsWith("Select", StringComparison.OrdinalIgnoreCase))//IgnoreCase
                {
                    MessageBox.Show("Bad Command", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    DataSet ds = idsRefVal.Execute(CliUtils.InsertWhere(tbSelectCommand.Text, "1=0"), tbSelectAlias.Text, false);
                    if (ds == null)
                    {
                        return;
                    }
                    else
                    {

                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            string column = ds.Tables[0].Columns[i].ColumnName;
                            tbDisplayMember.Items.Add(column);
                            tbValueMember.Items.Add(column);
                        }
                        if (showMessage)
                        {
                            MessageBox.Show("Connect Successfully", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                }
            }
            else if (showMessage)
            {
                string message = "";

                if (tbSelectAlias.Text == "")
                {
                    message += "SelectAlias is Empty";
                }
                if (tbSelectCommand.Text.Trim() == "")
                {
                    if (message != "")
                    {
                        message += "\r\n";
                    }
                    message += "SelectCommand is Empty";
                }

                MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }




        }
    }
}