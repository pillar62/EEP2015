using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Infolight.EasilyReportTools.Tools;
using Srvtools;
using System.ComponentModel.Design;
using Infolight.EasilyReportTools;
using Infolight.EasilyReportTools.DataCenter;
using System.IO;

namespace Infolight.EasilyReportTools.UI
{
    public partial class fmEasilyReportDesigner : Form
    {
        public fmEasilyReportDesigner()
        {
            InitializeComponent();
        }

        private IReport tempReport;
        /// <summary>
        /// Get the copy of report
        /// </summary>
        public IReport TempReport
        {
            get { return tempReport; }
        }

        private IReport designReport;
        public IReport DesignReport
        {
            get { return designReport; }
        }

        IComponentChangeService componentChangeService;
	       
        private string selectedTemplateName;

        public fmEasilyReportDesigner(IReport rpt, IDesignerHost designerHost)
        {
            InitializeComponent();
            designReport = rpt;
            tempReport = rpt.Copy();

            componentChangeService = designerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
        }

        private void fmEasilyReportDesigner_Load(object sender, EventArgs e)
        {
            if (TempReport.DataSourceCount == 0)
            {
                this.ShowMessage(MessageInfo.DataSourceNull, MsgMode.Error);
                this.Close();
            }
            else if (this.TempReport.EEPAlias == null || this.TempReport.EEPAlias == "")
            {
                this.ShowMessage(MessageInfo.EEPAliasNull, MsgMode.Error);
                this.Close();
            }
            else
            {
                //打开时跳过加载模板
                //this.LoadTemplate();
                if (string.IsNullOrEmpty(selectedTemplateName))
                {
                    Init();
                }
            }
        }

        private bool CheckReportItem(string itemType)
        {
            bool result = true;
            switch (itemType)
            {
                case "DataSourceItem":
                    if (TempReport.HeaderDataSource == null)
                    {
                        this.ShowMessage(MessageInfo.HeaderDataSourceNull, MsgMode.Error);
                        result = false;
                    }
                    break;

                case "ParameterItem":
                    if (TempReport.Parameters.Count == 0)
                    {
                        this.ShowMessage(MessageInfo.ParameterItemsNull, MsgMode.Error);
                        result = false;
                    }
                    break;

                case "ImageItem":
                    if (TempReport.Images.Count == 0)
                    {
                        this.ShowMessage(MessageInfo.ImageItemsNull, MsgMode.Error);
                        result = false;
                    }
                    break;
            }

            return result;
        }

        #region Report Header
        private void btAddHeaderItems_Click(object sender, EventArgs e)
        {
            ReportItem reportItem = null;

            if (this.CheckReportItem(cbHeaderItemType.SelectedItem.ToString()))
            {
                reportItem = (ReportItem)Activator.CreateInstance(Type.GetType(ComponentInfo.NameSpace + ".Report" + cbHeaderItemType.SelectedItem.ToString()));
                lbxHeaderItems.Items.Add(reportItem);
                TempReport.HeaderItems.Add(reportItem);//to get collection

                lbxHeaderItems.SelectedIndex = lbxHeaderItems.Items.IndexOf(reportItem);
            }
        }

        private void btRemoveHeaderItems_Click(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                lbxHeaderItems.Items.Remove(lbxHeaderItems.SelectedItem);
                if (lbxHeaderItems.Items.Count > 0)
                {
                    lbxHeaderItems.SelectedIndex = 0;
                }
            }
        }

        private void lbxHeaderItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                propertyGridReportHeaderItem.SelectedObject = lbxHeaderItems.SelectedItem;
                SetEnable(ReportArea.Header, true);
            }
            else
            {
                propertyGridReportHeaderItem.SelectedObject = null;
                SetEnable(ReportArea.Header, false);
               
            }
        }

        private void btHeaderFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.Font = lbHeaderFont.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                lbHeaderFont.Font = fontDialog.Font;
            }
        }

        private void btHeaderUp_Click(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                if (lbxHeaderItems.SelectedIndex > 0)
                {
                    int selectedIndex = lbxHeaderItems.SelectedIndex;
                    object tempItem = lbxHeaderItems.Items[selectedIndex - 1];
                    lbxHeaderItems.Items.RemoveAt(selectedIndex - 1);
                    lbxHeaderItems.Items.Insert(selectedIndex, tempItem);
                    lbxHeaderItems.SelectedIndex = selectedIndex - 1;
                }
            }
        }

        private void btHeaderDown_Click(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                if (lbxHeaderItems.SelectedIndex < lbxHeaderItems.Items.Count - 1)
                {
                    int selectedIndex = lbxHeaderItems.SelectedIndex;
                    object tempItem = lbxHeaderItems.SelectedItem;
                    lbxHeaderItems.Items.RemoveAt(selectedIndex);
                    lbxHeaderItems.Items.Insert(selectedIndex + 1, tempItem);
                    lbxHeaderItems.SelectedIndex = selectedIndex + 1;
                }
            }
        }
        #endregion

        #region Report Details
        #region Columns

        private void AddFieldItem(FieldItem field)
        {
            lbxSelectedFieldItems.Items.Add(field);

            DataRow[] dr = dtField.Select(string.Format("FIELD_NAME='{0}'", field.ColumnName.Replace("'", "''")));
            if (dr.Length > 0)
            {
                dr[0].Delete();
            }
        }

        private void DeleteFieldItem(FieldItem field)
        {
            lbxSelectedFieldItems.Items.Remove(field);
            for (int i = 0; i < dtField.Rows.Count; i++)
            {
                if (dtField.Rows[i].RowState == DataRowState.Deleted)
                {
                    if (dtField.Rows[i]["FIELD_NAME", DataRowVersion.Original].Equals(field.ColumnName))
                    {
                        dtField.Rows[i].RejectChanges();
                    }
                }
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (lbxFieldItems.SelectedIndex != -1)
            {
                List<FieldItem> list = new List<FieldItem>();
                foreach (DataRowView rowView in lbxFieldItems.SelectedItems)
                {
                    FieldItem field = new FieldItem();
                    field.ColumnName = rowView.Row["FIELD_NAME"].ToString();
                    field.Caption = rowView.Row["CAPTION"].ToString();
                    list.Add(field);
                }
                foreach (FieldItem field in list)
                {
                    AddFieldItem(field);
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                List<FieldItem> list = new List<FieldItem>();
                foreach (FieldItem field in lbxSelectedFieldItems.SelectedItems)
                {
                    list.Add(field);
                }
                foreach (FieldItem field in list)
                {
                    DeleteFieldItem(field);
                }
            }
        }

        private void btAddAll_Click(object sender, EventArgs e)
        {
            List<FieldItem> list = new List<FieldItem>();
            foreach (DataRowView rowView in lbxFieldItems.Items)
            {
                FieldItem field = new FieldItem();
                field.ColumnName = rowView.Row["FIELD_NAME"].ToString();
                field.Caption = rowView.Row["CAPTION"].ToString();
                list.Add(field);
            }
            foreach (FieldItem field in list)
            {
                AddFieldItem(field);
            }
        }

        private void btDeleteAll_Click(object sender, EventArgs e)
        {
            lbxSelectedFieldItems.Items.Clear();
            dtField.RejectChanges();
        }


        private void btFieldUp_Click(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                if (lbxSelectedFieldItems.SelectedIndices.Count == 1 && lbxSelectedFieldItems.SelectedIndex > 0)
                {
                    int selectedIndex = lbxSelectedFieldItems.SelectedIndex;
                    object item = lbxSelectedFieldItems.SelectedItem;
                    lbxSelectedFieldItems.Items.RemoveAt(selectedIndex);
                    lbxSelectedFieldItems.Items.Insert(selectedIndex - 1, item);
                    lbxSelectedFieldItems.SelectedIndex = selectedIndex - 1;
                }
            }
        }

        private void btFieldDown_Click(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                if (lbxSelectedFieldItems.SelectedIndices.Count == 1 && lbxSelectedFieldItems.SelectedIndex < lbxSelectedFieldItems.Items.Count - 1)
                {
                    int selectedIndex = lbxSelectedFieldItems.SelectedIndex;
                    object item = lbxSelectedFieldItems.SelectedItem;
                    lbxSelectedFieldItems.Items.RemoveAt(selectedIndex);
                    lbxSelectedFieldItems.Items.Insert(selectedIndex + 1, item);
                    lbxSelectedFieldItems.SelectedIndex = selectedIndex + 1;
                }
            }
        }

        private void lbxSelectedFieldItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                propertyGridFieldItem.SelectedObject = lbxSelectedFieldItems.SelectedItem;
                SetEnable(ReportArea.DetailColumns, true);
            }
            else
            {
                propertyGridFieldItem.SelectedObject = null;
                SetEnable(ReportArea.DetailColumns, false);
            }
        }

        private void btFieldFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.Font = lbFieldFont.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                lbFieldFont.Font = fontDialog.Font;
            }
        }

        #endregion


        #endregion

        #region Report Footer
        private void lbxFooterItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                propertyGridReportFooterItem.SelectedObject = lbxFooterItems.SelectedItem;
                SetEnable(ReportArea.Footer, true);
            }
            else
            {
                SetEnable(ReportArea.Footer, false);
                propertyGridReportFooterItem.SelectedObject = null;
            }
        }

        private void btAddFooterItem_Click(object sender, EventArgs e)
        {
            if (this.CheckReportItem(cbFooterItemType.SelectedItem.ToString()))
            {
                ReportItem reportItem = (ReportItem)Activator.CreateInstance(Type.GetType(ComponentInfo.NameSpace + ".Report" + cbFooterItemType.SelectedItem.ToString()));
                lbxFooterItems.Items.Add(reportItem);
                TempReport.FooterItems.Add(reportItem);//to get collection
                lbxFooterItems.SelectedIndex = lbxFooterItems.Items.IndexOf(reportItem);
            }
        }

        private void btRemoveFooterItem_Click(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                lbxFooterItems.Items.Remove(lbxFooterItems.SelectedItem);
                if (lbxFooterItems.Items.Count > 0)
                {
                    lbxFooterItems.SelectedIndex = 0;
                }
            }
        }

        private void btFooterFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.Font = lbFooterFont.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                lbFooterFont.Font = fontDialog.Font;
            }
        }

        private void btFooterUp_Click(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                if (lbxFooterItems.SelectedIndex > 0)
                {
                    int selectedIndex = lbxFooterItems.SelectedIndex;
                    object tempItem = lbxFooterItems.Items[selectedIndex - 1];
                    lbxFooterItems.Items.RemoveAt(selectedIndex - 1);
                    lbxFooterItems.Items.Insert(selectedIndex, tempItem);

                    lbxFooterItems.SelectedIndex = selectedIndex - 1;
                }
            }
        }

        private void btFooterDown_Click(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                if (lbxFooterItems.SelectedIndex < lbxFooterItems.Items.Count - 1)
                {
                    int selectedIndex = lbxFooterItems.SelectedIndex;
                    object tempItem = lbxFooterItems.SelectedItem;
                    lbxFooterItems.Items.RemoveAt(selectedIndex);
                    lbxFooterItems.Items.Insert(selectedIndex + 1, tempItem);

                    lbxFooterItems.SelectedIndex = selectedIndex + 1;
                }
            }
        }

        #endregion

        #region Report Setting
        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (tbEmailAddressDetail.Visible)
            {
                tbEmailAddress.Text = tbEmailAddressDetail.Text.Replace("\r\n", ";");
            }
            else
            {
                tbEmailAddressDetail.Text = tbEmailAddress.Text.Replace(";", "\r\n");
            }
            tbEmailAddressDetail.Visible = !tbEmailAddressDetail.Visible;
        }

        #endregion

        #region Common Function
        private void btnDone_Click(object sender, EventArgs e)
        {
            IReportExport excelReport = null;
            ExecutionResult execResult;
            execResult = new ExecutionResult();

            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.SaveReportConfig();

                if (this.TempReport.Format.ExportFormat == ReportFormat.ExportType.Excel)
                {
                    excelReport = new ExcelReportExporter(this.TempReport, ExportMode.Preview, true);
                }
                else
                {
                    excelReport = new PdfReportExporter(this.TempReport, ExportMode.Preview, true);
                }

                execResult = excelReport.CheckValidate();

                if (execResult.Status)
                {
                    excelReport.View();
                }
                else
                {
                    MessageBox.Show(execResult.Message, "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowMessage(string message, MsgMode msgMode)
        {
            switch (msgMode)
            {
                case MsgMode.Success:
                    MessageBox.Show(this, message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MsgMode.Error:
                    MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case MsgMode.Warning:
                    MessageBox.Show(this, message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private enum MsgMode
        {
            Success,
            Error,
            Warning
        }

        private void tabControleRpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowStatusMsg();
        }

        private void ShowStatusMsg()
        {
            switch (tabControleRpt.SelectedTab.Name)
            {
                case "tabPageReportHeader":
                    statusStripeRpt.Items["tsslMsg"].Text = StatusMsgInfo.HeaderItemAdd;
                    break;

                case "tabPageDetails":
                    statusStripeRpt.Items["tsslMsg"].Text = StatusMsgInfo.ReportDetails;
                    break;

                case "tabPageReportFooter":
                    statusStripeRpt.Items["tsslMsg"].Text = StatusMsgInfo.FooterItemAdd;
                    break;

                case "tabPageReportSetting":
                    statusStripeRpt.Items["tsslMsg"].Text = StatusMsgInfo.ReportSetting;
                    break;
            }
        }

        private void Init()
        {
            try
            {
                lastindex = -1;
                cbCaptionStyle.Items.Clear();
                foreach (object item in Enum.GetValues(typeof(DataSourceItem.CaptionStyleType)))
                {
                    cbCaptionStyle.Items.Add(item);
                }

                cbGroupGap.Items.Clear();
                foreach (object item in Enum.GetValues(typeof(DataSourceItem.GroupGapType)))
                {
                    cbGroupGap.Items.Add(item);
                }


                cbExportFormat.Items.Clear();
                foreach (object item in Enum.GetValues(typeof(ReportFormat.ExportType)))
                {
                    cbExportFormat.Items.Add(item);
                }
                cbExportFormat.SelectedItem = TempReport.Format.ExportFormat;

                #region Header
                cbHeaderItemType.SelectedItem = cbHeaderItemType.Items[0];
                if (TempReport.HeaderFont != null)
                {
                    lbHeaderFont.Font = TempReport.HeaderFont;
                }

                lbxHeaderItems.Items.Clear();
                foreach (object reportItem in TempReport.HeaderItems)
                {
                    lbxHeaderItems.Items.Add(reportItem);
                }

                if (lbxHeaderItems.Items.Count == 0 || lbxHeaderItems.SelectedIndex == -1)
                {
                    SetEnable(ReportArea.Header, false);
                }
                else
                {
                    SetEnable(ReportArea.Header, true);
                }

                propertyGridReportHeaderItem.SelectedObject = null;
                #endregion

                if (TempReport.FieldFont != null)
                {
                    lbFieldFont.Font = TempReport.FieldFont;
                }

                int count = TempReport.DataSourceCount - TempReport.FieldItems.Count;
                for (int i = 0; i < count; i++)
                {
                    TempReport.FieldItems.Add(new DataSourceItem());
                }

                comboBoxField.Items.Clear();
                for (int i = 0; i < TempReport.FieldItems.Count; i++)
                {
                    comboBoxField.Items.Add(i);
                }
                if (comboBoxField.Items.Count > 0)
                {
                    comboBoxField.SelectedIndex = 0;
                }

                #region Footer
                cbFooterItemType.SelectedItem = cbFooterItemType.Items[0];

                if (TempReport.FooterFont != null)
                {
                    lbFooterFont.Font = TempReport.FooterFont;
                }

                lbxFooterItems.Items.Clear();
                foreach (object reportItem in TempReport.FooterItems)
                {
                    lbxFooterItems.Items.Add(reportItem);
                }

                if (lbxFooterItems.Items.Count == 0 || lbxFooterItems.SelectedIndex == -1)
                {
                    SetEnable(ReportArea.Footer, false);
                }
                else
                {
                    SetEnable(ReportArea.Footer, true);
                }

                propertyGridReportFooterItem.SelectedObject = null;
                #endregion

                #region Report Format

                this.cbColumnGridLine.Checked = TempReport.Format.ColumnGridLine;
                this.cbColumnInsideLine.Checked = TempReport.Format.ColumnInsideGridLine;
                this.cbRowGridLine.Checked = TempReport.Format.RowGridLine;

                //Page Size
                foreach (Control ctl in this.Controls.Find("rb" + TempReport.Format.PageSize.ToString(), true))
                {
                    ((RadioButton)ctl).Checked = true;
                }
                //Print Orientation
                foreach (Control ctl in this.Controls.Find("rb" + TempReport.Format.Orientation.ToString(), true))
                {
                    ((RadioButton)ctl).Checked = true;
                }
                //Page Records
                mtbPageRecords.Text = TempReport.Format.PageRecords.ToString();

                //Page Height
                tbPageHeight.Text = TempReport.Format.PageHeight.ToString("f2");

                cbOutputMode.Items.Clear();
                foreach (object outputMode in Enum.GetValues(typeof(OutputModeType)))
                {
                    cbOutputMode.Items.Add(outputMode);
                }
                cbOutputMode.SelectedItem = this.TempReport.OutputMode;
                cbHeaderRepeat.Checked = this.TempReport.HeaderRepeat;


                if (!string.IsNullOrEmpty(this.TempReport.FilePath))
                {
                    tbOutputFileName.Text = Path.GetFileName(TempReport.FilePath);
                    tbOutputFilePath.Text = Path.GetDirectoryName(TempReport.FilePath);
                }
                #endregion

                #region MailSetting
                tbEmailAddress.Text = TempReport.MailSetting.MailTo;
                tbEmailTitle.Text = TempReport.MailSetting.Subject;
                #endregion

                #region Page Margin for Pdf
                mtbLeft.Text = TempReport.Format.MarginLeft.ToString();
                mtbRight.Text = TempReport.Format.MarginRight.ToString();
                mtbTop.Text = TempReport.Format.MarginTop.ToString();
                mtbBottom.Text = TempReport.Format.MarginBottom.ToString();
                #endregion

                ShowStatusMsg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int lastindex = -1;
        private void comboBoxField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastindex != -1)
            {
                SaveField(lastindex);
            }
            InitField();
            lastindex = comboBoxField.SelectedIndex;
        }

        DataTable dtField = new DataTable();
        private void InitField()
        { 
            //需要写回上一笔的值
            DataSourceItem fieldItem = TempReport.FieldItems[comboBoxField.SelectedIndex];

            DataSet ds = null;
            DataView view = null;
            if (fieldItem.DataSource is InfoBindingSource)
            {
                InfoBindingSource ibs = fieldItem.DataSource as InfoBindingSource;

                InfoDataSet ids = ibs.GetDataSource();
                if (ids.RealDataSet.Tables.Contains(ibs.DataMember))
                {
                    view = ids.RealDataSet.Tables[ibs.DataMember].DefaultView;
                }
                if (ids.RealDataSet.Relations.Contains(ibs.DataMember))
                {
                    view = ids.RealDataSet.Relations[ibs.DataMember].ChildTable.DefaultView;
                }

                ds = DBUtils.GetDataDictionary(ibs, true);
            }
            else if (fieldItem.DataSource is WebDataSource)
            {
                WebDataSource wds = fieldItem.DataSource as WebDataSource;
                if (string.IsNullOrEmpty(wds.SelectCommand) || string.IsNullOrEmpty(wds.SelectAlias))
                {
                    if (wds.DesignDataSet == null)
                    {
                        WebDataSet webDataSet = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                        if (webDataSet.RealDataSet != null)
                        {
                            wds.DesignDataSet = webDataSet.RealDataSet;
                        }
                    }
                    if (wds.DesignDataSet != null && wds.DesignDataSet.Tables.Contains(wds.DataMember))
                    {
                        view = wds.DesignDataSet.Tables[wds.DataMember].DefaultView;
                    }
                }
                else
                {
                    DataTable commandTable = wds.CommandTable;
                    if (commandTable != null)
                    {
                        view = commandTable.DefaultView;
                    }
                }
                ds = DBUtils.GetDataDictionary(wds, true);
            }
            if (view != null && ds != null)
            {
                DataTable table = view.Table;

                dtField = ds.Tables[0].Clone();

                foreach (DataColumn column in table.Columns)
                {
                    DataRow[] drDD = ds.Tables[0].Select(string.Format("FIELD_NAME='{0}'", column.ColumnName.Replace("'", "''")));

                    if (drDD.Length > 0)
                    {
                        dtField.ImportRow(drDD[0]);
                    }
                    else
                    {
                        DataRow row = dtField.NewRow();
                        row["CAPTION"] = column.Caption;
                        row["FIELD_NAME"] = column.ColumnName;
                        dtField.Rows.Add(row);
                    }
                }
            }
            lbxFieldItems.DataSource = dtField;
            lbxFieldItems.DisplayMember = "CAPTION";
            lbxFieldItems.ValueMember = "FIELD_NAME";

            lbxSelectedFieldItems.Items.Clear();
            foreach (FieldItem field in fieldItem.Fields)
            {
                AddFieldItem(field);
            }

            cbCaptionStyle.SelectedItem = fieldItem.CaptionStyle;
            cbGroupGap.SelectedItem = fieldItem.GroupGap;
            cbGroupTotal.Checked = fieldItem.GroupTotal;
        }

        private void SaveField(int index)
        {
            DataSourceItem fieldItem = TempReport.FieldItems[index];
            fieldItem.Fields.Clear();
            for (int i = 0; i < lbxSelectedFieldItems.Items.Count; i++)
            {
                fieldItem.Fields.Add((FieldItem)lbxSelectedFieldItems.Items[i]);
            }
            fieldItem.CaptionStyle = (DataSourceItem.CaptionStyleType)cbCaptionStyle.SelectedItem;
            fieldItem.GroupGap = (DataSourceItem.GroupGapType)cbGroupGap.SelectedItem;
            fieldItem.GroupTotal = cbGroupTotal.Checked;
        }

        private void SetEnable(ReportArea reportArea, bool isEnable)
        {
            switch (reportArea)
            {
                case ReportArea.Header:
                    btHeaderUp.Enabled = isEnable;
                    btHeaderDown.Enabled = isEnable;
                    break;
                case ReportArea.DetailColumns:
                    btFieldUp.Enabled = isEnable;
                    btFieldDown.Enabled = isEnable;
                    break;
                case ReportArea.Footer:
                    btFooterUp.Enabled = isEnable;
                    btFooterDown.Enabled = isEnable;
                    break;
            }
        }

        private enum ReportArea
        { 
            Header,
            DetailColumns,
            Footer
        }

        private void cbOutputMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOutputMode.SelectedItem.ToString() != "None" && cbOutputMode.SelectedItem.ToString() != "Launch")
            {
                gbEmailConfig.Enabled = true;
            }
            else
            {
                gbEmailConfig.Enabled = false;
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveReportConfig();
        }

        #region Save Config Function
        private void SaveReportConfig()
        {
            SaveReportHeader();
            if (lastindex != -1)
            {
                SaveField(lastindex);
            }
            TempReport.FieldFont = lbFieldFont.Font;

            SaveReportFooter();
            SaveReportSetting();
            SaveMailSetting();

            TempReport.CopyTo(this.DesignReport);
            this.RaiseDesignSourceChanged("Description", this.DesignReport.Description);
            this.RaiseDesignSourceChanged("FieldFont", this.DesignReport.FieldFont);
            this.RaiseDesignSourceChanged("FieldItems", this.DesignReport.FieldItems);
            this.RaiseDesignSourceChanged("FilePath", this.DesignReport.FilePath);
            this.RaiseDesignSourceChanged("FooterFont", this.DesignReport.FooterFont);
            this.RaiseDesignSourceChanged("FooterItems", this.DesignReport.FooterItems);
            this.RaiseDesignSourceChanged("Format", this.DesignReport.Format);
            this.RaiseDesignSourceChanged("HeaderFont", this.DesignReport.HeaderFont);
            this.RaiseDesignSourceChanged("HeaderItems", this.DesignReport.HeaderItems);
            this.RaiseDesignSourceChanged("Images", this.DesignReport.Images);
            this.RaiseDesignSourceChanged("MailSetting", this.DesignReport.MailSetting);
            this.RaiseDesignSourceChanged("OutputMode", this.DesignReport.OutputMode);
            this.RaiseDesignSourceChanged("Parameters", this.DesignReport.Parameters);
            this.RaiseDesignSourceChanged("ReportID", this.DesignReport.ReportID);
            this.RaiseDesignSourceChanged("ReportName", this.DesignReport.ReportName);
        }

        private void SaveMailSetting()
        {
            TempReport.MailSetting.MailTo = tbEmailAddress.Text.Trim();
            TempReport.MailSetting.Subject = tbEmailTitle.Text.Trim();
        }

        private void SaveReportSetting()
        {
             TempReport.Format.ColumnGridLine = cbColumnGridLine.Checked;
             TempReport.Format.ColumnInsideGridLine = cbColumnInsideLine.Checked;
             TempReport.Format.RowGridLine = cbRowGridLine.Checked;

            foreach (object pageType in Enum.GetValues(typeof(ReportFormat.PageType)))
            {
                if (((RadioButton)this.Controls.Find("rb" + pageType.ToString(), true)[0]).Checked)
                {
                    TempReport.Format.PageSize = (ReportFormat.PageType)pageType;
                }
            }
            
            if (rbHorizontal.Checked)
            {
                TempReport.Format.Orientation = Orientation.Horizontal;
            }
            else
            {
                TempReport.Format.Orientation = Orientation.Vertical;
            }

            if (mtbPageRecords.Text.Trim().Length > 0)
            {
                TempReport.Format.PageRecords = Convert.ToInt32(mtbPageRecords.Text.Trim());
            }

            if (tbPageHeight.Text.Trim().Length > 0)
            {
                TempReport.Format.PageHeight = Convert.ToDouble(tbPageHeight.Text.Trim());
            }

            TempReport.Format.ExportFormat = (ReportFormat.ExportType)cbExportFormat.SelectedItem;

            TempReport.OutputMode = (OutputModeType)cbOutputMode.SelectedItem;

            TempReport.HeaderRepeat = cbHeaderRepeat.Checked;

            TempReport.FilePath = MyStringConverter.GetFullFilePath(tbOutputFilePath.Text.Trim(), this.tbOutputFileName.Text.Trim(), this.TempReport.Format.ExportFormat);

            if (this.TempReport.Format.ExportFormat == ReportFormat.ExportType.Pdf)
            {
                TempReport.Format.MarginLeft = mtbLeft.Text.Trim().Length > 0 ? Convert.ToDouble(mtbLeft.Text.Trim()) : 0;
                TempReport.Format.MarginRight = mtbRight.Text.Trim().Length > 0 ? Convert.ToDouble(mtbRight.Text.Trim()) : 0;
                TempReport.Format.MarginTop = mtbTop.Text.Trim().Length > 0 ? Convert.ToDouble(mtbTop.Text.Trim()) : 0;
                TempReport.Format.MarginBottom = mtbBottom.Text.Trim().Length > 0 ? Convert.ToDouble(mtbBottom.Text.Trim()) : 0;
            }
        }

        private void SaveReportHeader()
        {
            TempReport.HeaderItems.Clear();
            for (int i = 0; i < lbxHeaderItems.Items.Count; i++)
            {
                TempReport.HeaderItems.Add((ReportItem)lbxHeaderItems.Items[i]);
            }

            //this.RaiseDesignSourceChanged("HeaderItems", report.HeaderItems);
            TempReport.HeaderFont = lbHeaderFont.Font;
            //this.RaiseDesignSourceChanged("HeaderFont", report.HeaderFont);
        }

        private void SaveReportFooter()
        {
            TempReport.FooterItems.Clear();
            for (int i = 0; i < lbxFooterItems.Items.Count; i++)
            {
                TempReport.FooterItems.Add((ReportItem)lbxFooterItems.Items[i]);
            }
            TempReport.FooterFont = lbFooterFont.Font;
        }
     
        #endregion
        
        #endregion

        #region Component Designer Function
        private void RaiseDesignSourceChanged(string propertyName, object newValue)
        {
            this.RaiseDesignSourceChanged(this.DesignReport, propertyName, newValue);
        }

        private void RaiseDesignSourceChanged(object component, string propertyName, object newValue)
        {
            PropertyDescriptor itemProperty = TypeDescriptor.GetProperties(component)[propertyName];
            componentChangeService.OnComponentChanging(component, itemProperty);
            componentChangeService.OnComponentChanged(component, itemProperty, null, newValue);
        }
        #endregion

        #region Menu Operation
        private void menuItemReadTemplate_Click(object sender, EventArgs e)
        {
            LoadTemplate();
        }

        private void menuItemSaveTemplate_Click(object sender, EventArgs e)
        {
            this.SaveReportConfig();
            fmSaveAsTemplate fm = new fmSaveAsTemplate(TempReport, true);
            if (string.IsNullOrEmpty(selectedTemplateName))
            {
                if (fm.ShowDialog(this) == DialogResult.OK)
                {
                    this.ShowMessage(MessageInfo.SaveSuccess, MsgMode.Success);
                }
            }
            else
            {
                ExecutionResult execResult = fm.SaveTemplate(selectedTemplateName);
                if (execResult.Status)
                {
                    this.ShowMessage(execResult.Message, MsgMode.Success);
                }
                else
                {
                    this.ShowMessage(execResult.Message, MsgMode.Error);
                }
            }
        }

        private void menuItemSaveAsTemplate_Click(object sender, EventArgs e)
        {
            this.SaveReportConfig();
            fmSaveAsTemplate fm = new fmSaveAsTemplate(TempReport, true);
            if (fm.ShowDialog(this) == DialogResult.OK)
            {
                this.ShowMessage(MessageInfo.SaveSuccess, MsgMode.Success);
            }
        }

        private void menuItemDeleteTemplate_Click(object sender, EventArgs e)
        {
           // this.LoadTemplate(TemplateLoadMode.Delete);
        }

        private void LoadTemplate()
        {
            fmLoadTemplate fm = new fmLoadTemplate(this.TempReport, true);

            if (fm.ShowDialog(this) == DialogResult.OK)
            {
                selectedTemplateName = fm.TemplateName;
                Init();
            }
            else
            {
                this.selectedTemplateName = String.Empty;
            }
        }

        #endregion

        #region lbxHeaderItems and lbxFooterItems Refresh
        private void propertyGridReportHeaderItem_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label)
            {
                case "Field":

                case "Style":

                case "Index":

                case "ColumnName":
                    this.RefreshListBox(ReportArea.Header);
                    break;
            }
        }

        private void propertyGridReportFooterItem_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label)
            {
                case "Field":

                case "Style":

                case "Index":

                case "ColumnName":
                    this.RefreshListBox(ReportArea.Footer);
                    break;
            }
            
        }

        private void RefreshListBox(ReportArea reportArea)
        {
            #region Variable Definition
            ReportItem tempItem = null;
            int selectedIndex = -1;
            #endregion

            switch (reportArea)
            {
                case ReportArea.Header:
                    selectedIndex = lbxHeaderItems.SelectedIndex;
                    tempItem = ((ReportItem)lbxHeaderItems.SelectedItem);
                    lbxHeaderItems.Items.RemoveAt(selectedIndex);
                    lbxHeaderItems.Items.Insert(selectedIndex, tempItem);
                    lbxHeaderItems.SelectedIndex = selectedIndex;
                    break;
                case ReportArea.Footer:
                    selectedIndex = lbxFooterItems.SelectedIndex;
                    tempItem = ((ReportItem)lbxFooterItems.SelectedItem);
                    lbxFooterItems.Items.RemoveAt(selectedIndex);
                    lbxFooterItems.Items.Insert(selectedIndex, tempItem);
                    lbxFooterItems.SelectedIndex = selectedIndex;
                    break;
            }
        }
        #endregion

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (Directory.Exists(tbOutputFilePath.Text))
            {
                fbd.SelectedPath = tbOutputFilePath.Text;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbOutputFilePath.Text = fbd.SelectedPath;
            }
        }

        private void cbExportFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ReportFormat.ExportType)cbExportFormat.SelectedItem) == ReportFormat.ExportType.Excel)
            {
                gbPageSize.Enabled = false;
                //gbOrientation.Enabled = false;
                gbPageMargin.Enabled = false;
            }
            else
            {
                gbPageSize.Enabled = true;
                //gbOrientation.Enabled = true;
                gbPageMargin.Enabled = true;
            }

            tbOutputFileName.Text = MyStringConverter.GetFullFileName(tbOutputFileName.Text.Trim(), ((ReportFormat.ExportType)cbExportFormat.SelectedItem));
        }
    }
}