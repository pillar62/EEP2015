using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Srvtools
{
    public partial class frmClientQueryEditor : Form
    {
        public ClientQuery cqeditor = new ClientQuery();
        private ClientQuery cqold;
        private IContainer ict;
        IDesignerHost DesignerHost = null;


        public frmClientQueryEditor(ClientQuery cq, IDesignerHost host)
        {
            InitializeComponent();
            DesignerHost = host;
            ict = host.Container;
            cqold = cq;
            InitializeColumn(cqold);
        }

        

        private void AddInfoRefVal()
        {
            this.InfoRefVal.Items.Add((object)"(None)");

            foreach (IComponent cp in ict.Components)
            {
                if (cp is InfoRefVal)
                {
                    this.InfoRefVal.Items.Add((object)cp.Site.Name);
                }
            }
       
        }



        private void InitializeColumn(ClientQuery cq)
        {
            cmbbindingsource.Items.Clear();

            try
            {
                cmbbindingsource.Text = cq.BindingSource.Site.Name;
            }
            catch
            { 
            
            }

            AddInfoRefVal();

            int columncount = cq.Columns.Count;
            if (columncount > 0)
            {
                dgvClientQuery.Rows.Add(columncount);
            }
            for (int i = 0; i < columncount; i++)
            {
                dgvClientQuery.Rows[i].Cells["Caption"].Value = ((QueryColumns)cq.Columns[i]).Caption;
                dgvClientQuery.Rows[i].Cells["Column"].Value = ((QueryColumns)cq.Columns[i]).Column;
                dgvClientQuery.Rows[i].Cells["ColumnType"].Value = ((QueryColumns)cq.Columns[i]).ColumnType;
                dgvClientQuery.Rows[i].Cells["Condition"].Value = ((QueryColumns)cq.Columns[i]).Condition;
                dgvClientQuery.Rows[i].Cells["DefaultValue"].Value = ((QueryColumns)cq.Columns[i]).DefaultValue;
                dgvClientQuery.Rows[i].Cells["Operator"].Value = ((QueryColumns)cq.Columns[i]).Operator;
                dgvClientQuery.Rows[i].Cells["NewLine"].Value = (object)((QueryColumns)cq.Columns[i]).NewLine.ToString();
                dgvClientQuery.Rows[i].Cells["TextAlign"].Value = ((QueryColumns)cq.Columns[i]).TextAlign;
                dgvClientQuery.Rows[i].Cells["TextWidth"].Value = ((QueryColumns)cq.Columns[i]).Width;
                if (((QueryColumns)cq.Columns[i]).InfoRefVal != null)
                {
                    try
                    {
                        dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value = ((QueryColumns)cq.Columns[i]).InfoRefVal.Site.Name;
                    }
                    catch
                    {
                        dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value = "(None)";
                    }
                }
                else
                {
                    dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value = "(None)";
                }
                dgvClientQuery.Rows[i].Cells["ExternalRefval"].Value = ((QueryColumns)cq.Columns[i]).ExternalRefVal;
                dgvClientQuery.Rows[i].Cells["ColumnVisible"].Value = ((QueryColumns)cq.Columns[i]).Visible;
            }

            foreach (IComponent cp in ict.Components)
            {
                if (cp is InfoBindingSource)
                {
                    cmbbindingsource.Items.Add((object)cp.Site.Name);
                }
            }
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int columncount = dgvClientQuery.Rows.Count;

            QueryColumns[] qc = new QueryColumns[columncount];
            InfoRefVal[] refval = new InfoRefVal[columncount];

            for (int i = 0; i < columncount - 1; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (dgvClientQuery.Rows[i].Cells[j].Value == null)
                    {
                        if (dgvClientQuery.Columns[j].Name != "DefaultValue")
                        {
                            MessageBox.Show(string.Format("Record {0}: Can't be null in {1}", i + 1, dgvClientQuery.Columns[j].HeaderText));
                            return;
                        }
                    }
                }

                if (dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value != null && dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString() != "(None)")
                {
                    if ((dgvClientQuery.Rows[i].Cells["ColumnType"].Value.ToString() != "ClientQueryComboBoxColumn")
                        && (dgvClientQuery.Rows[i].Cells["ColumnType"].Value.ToString() != "ClientQueryRefValColumn"))
                    {
                        MessageBox.Show(string.Format("Record {0} :\nInfoRefval can be set only when\ncolumntype is combobox & refval.",i + 1));
                        return;
                    }
                    else
                    {
                        try
                        {
                            refval[i] = (InfoRefVal)ict.Components[dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString()];
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("InfoRefVal {0} doesn't exist", dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString()));
                            return;
                        }
                    }

                }
            }

            try
            {
                cqeditor.BindingSource = (InfoBindingSource)ict.Components[cmbbindingsource.Text];
                cqeditor.Columns.Clear();
            }
            catch
            {

            }
            for (int i = 0; i < columncount - 1; i++)
            {
                qc[i] = new QueryColumns();
                qc[i].setcolumn(dgvClientQuery.Rows[i].Cells["Column"].Value.ToString());
                qc[i].Caption = dgvClientQuery.Rows[i].Cells["Caption"].Value.ToString();
                qc[i].ColumnType = dgvClientQuery.Rows[i].Cells["ColumnType"].Value.ToString();
                qc[i].Condition = dgvClientQuery.Rows[i].Cells["Condition"].Value.ToString();
                if (dgvClientQuery.Rows[i].Cells["DefaultValue"].Value != null)
                {
                    qc[i].DefaultValue = dgvClientQuery.Rows[i].Cells["DefaultValue"].Value.ToString();
                }
                else
                {
                    qc[i].DefaultValue = "";
                }
                qc[i].Operator = dgvClientQuery.Rows[i].Cells["Operator"].Value.ToString();
                qc[i].NewLine = Boolean.Parse(dgvClientQuery.Rows[i].Cells["NewLine"].Value.ToString());
                qc[i].TextAlign = dgvClientQuery.Rows[i].Cells["TextAlign"].Value.ToString();
                try
                {
                    qc[i].Width = int.Parse(dgvClientQuery.Rows[i].Cells["TextWidth"].Value.ToString());
                    if(qc[i].Width <= 0)
                    {
                        MessageBox.Show(string.Format("Record {0}: Width should be an positive integer", i + 1));
                        return;
                    }
                }
                catch
                { 
                    MessageBox.Show(string.Format("Record {0}: Width should be an positive integer", i + 1));
                    return;
                }
                if (dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value != null && dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString() != "(None)")
                {
                    qc[i].InfoRefVal = refval[i];
                }
                if (dgvClientQuery.Rows[i].Cells["ExternalRefval"].Value != null)
                {
                    qc[i].ExternalRefVal = dgvClientQuery.Rows[i].Cells["ExternalRefval"].Value.ToString();
                }
                if (dgvClientQuery.Rows[i].Cells["ColumnVisible"].Value != null)
                {
                    qc[i].Visible = (bool)dgvClientQuery.Rows[i].Cells["ColumnVisible"].Value;
                }
                cqeditor.Columns.Add(qc[i]);
            }

            //MessageBox.Show(string.Format("cqeditor has {0} columns", cqeditor.Columns.Count));

            //new add
            IComponentChangeService ComponentChangeService =
                this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            object oldValue = null;
            object newValue = null;



            //BindingSource

            PropertyDescriptor descBindingSource = TypeDescriptor.GetProperties(this.cqold)["BindingSource"];
            ComponentChangeService.OnComponentChanging(this.cqold, descBindingSource);
            oldValue = cqold.BindingSource;
            cqold.BindingSource = cqeditor.BindingSource;
            newValue = cqold.BindingSource;
            ComponentChangeService.OnComponentChanged(this.cqold, descBindingSource, oldValue, newValue);
            
            // Columns
            PropertyDescriptor descColumns = TypeDescriptor.GetProperties(this.cqold)["Columns"];
            ComponentChangeService.OnComponentChanging(this.cqold, descColumns);
            oldValue = cqold.Columns;
            cqold.Columns = cqeditor.Columns;
            newValue = cqold.Columns;
            ComponentChangeService.OnComponentChanged(this.cqold, descColumns, oldValue, newValue);

            this.Close();
        
        }

        private void cmbbindingsource_TextChanged(object sender, EventArgs e)
        {
            this.Column.Items.Clear();
            if (cmbbindingsource.Text != null)
            {
                try
                {
                    DsForDD.Tables.Clear();
                    InfoBindingSource ibs = (InfoBindingSource)ict.Components[cmbbindingsource.Text];
                    dgvClientQuery.Rows.Clear();
                    int cqcolumncount = ((InfoDataSet)ibs.DataSource).RealDataSet.Tables[ibs.DataMember].Columns.Count;
                    for (int i = 0; i < cqcolumncount; i++)
                    {
                        this.Column.Items.Add(((InfoDataSet)ibs.DataSource).RealDataSet.Tables[ibs.DataMember].Columns[i].ColumnName);
                    }
                }
                catch
                {
                    MessageBox.Show(string.Format("bindingsource {0} doesn't exist", cmbbindingsource.Text));
                }
            }
        }

        private void dgvClientQuery_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 0)
            {
       
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["ColumnType"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["ColumnType"].Value = "ClientQueryTextBoxColumn";
                }
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["Condition"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["Condition"].Value = "And";
                }
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["Operator"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["Operator"].Value = "=";
                }
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["NewLine"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["NewLine"].Value = "True";
                }
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["TextAlign"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["TextAlign"].Value = "Left";
                }
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["TextWidth"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["TextWidth"].Value = 120;
                }
                if (dgvClientQuery.Rows[e.RowIndex - 1].Cells["InfoRefVal"].Value == null)
                {
                    dgvClientQuery.Rows[e.RowIndex - 1].Cells["InfoRefVal"].Value = "(None)";
                }
            }
        }

        private void dgvClientQuery_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        
        #region method
        private DataSet DsForDD = new DataSet();

        private string GetHeaderText(string ColName)
        {
            DataSet ds = DsForDD;
            string strTableName = "";

            InfoBindingSource ibs = (InfoBindingSource)ict.Components[cmbbindingsource.Text];
            strTableName = ibs.DataMember;
            if (ds.Tables.Count == 0)
            {
                DsForDD = DBUtils.GetDataDictionary(ibs, true);
                ds = DsForDD;
            }

            string strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[strTableName].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[strTableName].Rows[j]["FIELD_NAME"].ToString(), ColName, true) == 0)//IgnoreCase
                    {
                        strHeaderText = ds.Tables[strTableName].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        #endregion

        private ClientQuery cqpreivewer = new ClientQuery();
        private frmClientQuery fcq = null;
        private void btnPreview_Click(object sender, EventArgs e)
        {

            int columncount = dgvClientQuery.Rows.Count;

            QueryColumns[] qc = new QueryColumns[columncount];
            InfoRefVal[] refval = new InfoRefVal[columncount];

            for (int i = 0; i < columncount - 1; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (dgvClientQuery.Rows[i].Cells[j].Value == null)
                    {
                        if (dgvClientQuery.Columns[j].Name != "DefaultValue")
                        {
                            MessageBox.Show(string.Format("Record {0}: Can't be null in {1}", i + 1, dgvClientQuery.Columns[j].HeaderText));
                            return;
                        }
                    }
                }

                if (dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value != null && dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString() != "(None)")
                {
                    if ((dgvClientQuery.Rows[i].Cells["ColumnType"].Value.ToString() != "ClientQueryComboBoxColumn")
                        && (dgvClientQuery.Rows[i].Cells["ColumnType"].Value.ToString() != "ClientQueryRefValColumn"))
                    {
                        MessageBox.Show(string.Format("Record {0} :\nInfoRefval can be set only when\ncolumntype is combobox & refval.", i + 1));
                        return;
                    }
                    else
                    {
                        try
                        {
                            refval[i] = (InfoRefVal)ict.Components[dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString()];
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("InfoRefVal {0} doesn't exist", dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString()));
                            return;
                        }
                    }

                }
            }

            try
            {
                cqpreivewer.BindingSource = (InfoBindingSource)ict.Components[cmbbindingsource.Text];
                cqpreivewer.Columns.Clear();
            }
            catch
            {

            }
            for (int i = 0; i < columncount - 1; i++)
            {
                qc[i] = new QueryColumns();
                qc[i].setcolumn(dgvClientQuery.Rows[i].Cells["Column"].Value.ToString());
                qc[i].Caption = dgvClientQuery.Rows[i].Cells["Caption"].Value.ToString();
                if (dgvClientQuery.Rows[i].Cells["DefaultValue"].Value != null)
                {
                    qc[i].DefaultValue = dgvClientQuery.Rows[i].Cells["DefaultValue"].Value.ToString();
                }
                else
                {
                    qc[i].DefaultValue = "";
                }
                qc[i].ColumnType = dgvClientQuery.Rows[i].Cells["ColumnType"].Value.ToString();
                qc[i].Condition = dgvClientQuery.Rows[i].Cells["Condition"].Value.ToString();
                qc[i].Operator = dgvClientQuery.Rows[i].Cells["Operator"].Value.ToString();
                qc[i].NewLine = Boolean.Parse(dgvClientQuery.Rows[i].Cells["NewLine"].Value.ToString());
                qc[i].TextAlign = dgvClientQuery.Rows[i].Cells["TextAlign"].Value.ToString();
                try
                {
                    qc[i].Width = int.Parse(dgvClientQuery.Rows[i].Cells["TextWidth"].Value.ToString());
                    if (qc[i].Width <= 0)
                    {
                        MessageBox.Show(string.Format("Record {0}: Width should be an positive integer", i + 1));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(string.Format("Record {0}: Width should be an positive integer", i + 1));
                    return;
                }
                if (dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value != null && dgvClientQuery.Rows[i].Cells["InfoRefVal"].Value.ToString() != "(None)")
                {
                    qc[i].InfoRefVal = refval[i];
                }
                cqpreivewer.Columns.Add(qc[i]);
            }
            cqpreivewer.Caption = cqold.Caption;
            cqpreivewer.Margin = cqold.Margin;
            cqpreivewer.Font = cqold.Font;
            cqpreivewer.ForeColor = cqold.ForeColor;
            cqpreivewer.TextColor = cqold.TextColor;

            fcq = new frmClientQuery(cqpreivewer,true);
            fcq.btnOk.Enabled = false;
            fcq.btnCancel.Enabled = false;
            fcq.ShowDialog();


        }

        private void dgvClientQuery_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvClientQuery_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dgvClientQuery.Columns["Column"].Index)
                {
                    //if (dgvClientQuery.Rows[e.RowIndex].Cells["Caption"].Value == null)
                    //{
                    dgvClientQuery.Rows[e.RowIndex].Cells["Caption"].Value = GetHeaderText(dgvClientQuery.Rows[e.RowIndex].Cells["Column"].Value.ToString());
                    if (dgvClientQuery.Rows[e.RowIndex].Cells["Caption"].Value == null || dgvClientQuery.Rows[e.RowIndex].Cells["Caption"].Value.ToString() == string.Empty)
                    {
                        dgvClientQuery.Rows[e.RowIndex].Cells["Caption"].Value = dgvClientQuery.Rows[e.RowIndex].Cells["Column"].Value;
                    }
                    //}

                }
            }
        }

        private void dgvClientQuery_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == dgvClientQuery.Columns.IndexOf(ExternalRefVal))
            {
                frmExternalRefvalEditor form = new frmExternalRefvalEditor();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    dgvClientQuery[e.ColumnIndex, e.RowIndex].Value = form.SelectedValue;
                }

                e.Cancel = true;
            }
        }
    }
}