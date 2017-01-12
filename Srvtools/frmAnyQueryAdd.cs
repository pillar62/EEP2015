using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class frmAnyQueryAdd : Form
    {
        public InfoBindingSource BindingSource;
        public String caption;
        public String column;
        public String columnType;
        public String refval;
        public String defaultValue;
        public String operatorMark;
        public int width;
        public String condition;

        public bool isOk = false;
        public frmAnyQueryAdd()
        {
            InitializeComponent();
        }

        public frmAnyQueryAdd(InfoBindingSource bs)
        {
            BindingSource = bs;
            InitializeComponent();
        }

        private void frmAnyQueryAdd_Load(object sender, EventArgs e)
        {
            InfoDataSet ids = null;
            if (this.BindingSource != null)
            {
                ids = this.BindingSource.DataSource as InfoDataSet;
                foreach (DataColumn dc in ids.RealDataSet.Tables[0].Columns)
                {
                    this.cbColumnName.Items.Add(dc.ColumnName);
                }
            }

            DataSet dsRef = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", "Select * from SYS_REFVAL", true, CliUtils.fCurrentProject);
            if (dsRef != null && dsRef.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRef.Tables[0].Rows)
                {
                    this.cbRefVal.Items.Add(dr["REFVAL_NO"].ToString());
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            caption = this.tbCaption.Text;
            column = this.cbColumnName.Text;
            columnType = this.cbColumnType.Text;
            refval = this.cbRefVal.Text;
            defaultValue = this.tbDefaultValue.Text;
            operatorMark = this.cbOperator.Text;
            width = Convert.ToInt16(this.tbWidth.Text);
            condition = this.cbCondition.Text;
            isOk = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isOk = false;
            this.Close();
        }

        private void cbColumnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).Text == "AnyQueryComboBoxColumn" || (sender as ComboBox).Text == "AnyQueryRefValColumn")
            {
                this.cbRefVal.Enabled = true;
            }
            else
            {
                this.cbRefVal.Enabled = false;
            }
        }

        private void cbColumnName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).Text != String.Empty)
            {
                tbCaption.Text = GetHeaderText((sender as ComboBox).Text);
            }
        }

        private String GetHeaderText(String ColName)
        {
            DataSet ds = DBUtils.GetDataDictionary(BindingSource, false);

            String strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }
    }
}