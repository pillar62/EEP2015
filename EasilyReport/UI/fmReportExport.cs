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
using System.Diagnostics;

namespace Infolight.EasilyReportTools.UI
{
    public partial class fmReportExport : Form
    {
        public fmReportExport()
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
        private string selectedTemplateName;

        public fmReportExport(IReport rpt)
        {
            InitializeComponent();
            designReport = rpt;
            tempReport = rpt.Copy();

            //ERptMultiLanguage.clientLanguageNum = 0;

            InitLanguage();
            InitComponentValue();
        }

       
        #region Report Header
        private void btAddHeaderItems_Click(object sender, EventArgs e)
        {
            ReportItem reportItem = null;

            if (this.CheckReportItem(cbHeaderItemType.SelectedValue.ToString()))
            {
                reportItem = (ReportItem)Activator.CreateInstance(Type.GetType(ComponentInfo.NameSpace + ".Report" + cbHeaderItemType.SelectedValue.ToString()));
                lbxHeaderItems.Items.Add(reportItem);

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
                HeaderItemPropertyChange((ReportItem)lbxHeaderItems.SelectedItem);
                SetEnable(ReportArea.Header, true);
            }
            else
            {
                HeaderItemPropertyChange(null);
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

        private void HeaderItemPropertyChange(ReportItem reportItem)
        {
            bool isEnable = true;
            if (reportItem != null)
            {
                isEnable = true;

                cbHeaderConstantItemContent.Enabled = !isEnable;
                cbHeaderDataSourceItemField.Enabled = !isEnable;
                mtbHeaderItemIndex.Enabled = !isEnable;
                cbHeaderConstantItemContent.Visible = !isEnable;
                cbHeaderDataSourceItemField.Visible = !isEnable;
                mtbHeaderItemIndex.Visible = !isEnable;
                btAddPictureH.Visible = !isEnable;
                btViewH.Visible = !isEnable;

                switch (reportItem.GetType().Name)
                {
                    case "ReportConstantItem":
                        lbHeaderConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbContent");
                        cbHeaderConstantItemContent.Enabled = isEnable;
                        cbHeaderConstantItemContent.Visible = isEnable;
                        cbHeaderConstantItemContent.SelectedValue = ((ReportConstantItem)reportItem).Style;
                        break;

                    case "ReportParameterItem":
                        lbHeaderConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbIndex");
                        mtbHeaderItemIndex.Enabled = isEnable;
                        mtbHeaderItemIndex.Visible = isEnable;
                        mtbHeaderItemIndex.Text = ((ReportParameterItem)reportItem).Index.ToString();
                        break;

                    case "ReportDataSourceItem":
                        lbHeaderConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbField");
                        cbHeaderDataSourceItemField.Enabled = isEnable;
                        cbHeaderDataSourceItemField.Visible = isEnable;
                        if (((ReportDataSourceItem)reportItem).ColumnName != null)
                        {
                            cbHeaderDataSourceItemField.SelectedValue = ((ReportDataSourceItem)reportItem).ColumnName;
                        }
                        else
                        {
                            cbHeaderDataSourceItemField.SelectedIndex = -1;
                        }
                        break;

                    case "ReportImageItem":
                        lbHeaderConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbIndex");
                        mtbHeaderItemIndex.Enabled = !isEnable;
                        mtbHeaderItemIndex.Visible = isEnable;
                        mtbHeaderItemIndex.Text = ((ReportImageItem)reportItem).Index.ToString();
                        btAddPictureH.Visible = isEnable;
                        btViewH.Visible = isEnable;
                        break;
                }

                cbHeaderConstantItemContentAlign.SelectedItem = reportItem.ContentAlignment;
                cbHeaderConstantItemNewLine.Checked = reportItem.NewLine;
                cbHeaderConstantItemPosition.SelectedItem = reportItem.Position;
                mtbHeaderConstantItemCells.Text = reportItem.Cells.ToString();
                tbHeaderConstantItemFormat.Text = reportItem.Format;
                lbHeaderConstantItemFont.Font = reportItem.Font;
            }
            else
            {
                isEnable = false;
                lbHeaderConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbContent");
                cbHeaderConstantItemContent.Visible = !isEnable;
                cbHeaderDataSourceItemField.Visible = isEnable;
                mtbHeaderItemIndex.Visible = isEnable;

                mtbHeaderItemIndex.Text = String.Empty;
                cbHeaderConstantItemContentAlign.SelectedIndex = 0;
                cbHeaderConstantItemNewLine.Checked = false;
                cbHeaderConstantItemPosition.SelectedIndex = 0;
                mtbHeaderConstantItemCells.Text = String.Empty;
                tbHeaderConstantItemFormat.Text = String.Empty;

                btAddPictureH.Visible = isEnable;
                btViewH.Visible = isEnable;
            }

            cbHeaderConstantItemContent.Enabled = isEnable;
            cbHeaderDataSourceItemField.Enabled = isEnable;
            cbHeaderConstantItemContentAlign.Enabled = isEnable;
            cbHeaderConstantItemNewLine.Enabled = isEnable;
            cbHeaderConstantItemPosition.Enabled = isEnable;
            mtbHeaderConstantItemCells.Enabled = isEnable;
            tbHeaderConstantItemFormat.Enabled = isEnable;
            btHeaderItemFont.Enabled = isEnable;
            plHeaderItemFontSample.Enabled = isEnable;
        }

        private void btAddPictureH_Click(object sender, EventArgs e)
        {
            fmPictures fm = new fmPictures(this.TempReport);
            if (fm.ShowDialog() == DialogResult.OK)
            {
                mtbHeaderItemIndex.Text = fm.SelectedIndex;

                ((ReportImageItem)lbxHeaderItems.SelectedItem).Index = Convert.ToInt32(mtbHeaderItemIndex.Text.Trim());
                this.RefreshListBox(ReportArea.Header);
            }
        }

        private void btViewH_Click(object sender, EventArgs e)
        {
            if (mtbHeaderItemIndex.Text != String.Empty && this.TempReport.Images.Count > 0)
            {
                fmPictureView fm = new fmPictureView(this.TempReport, Convert.ToInt32(mtbHeaderItemIndex.Text.Trim()));
                fm.ShowDialog();
            }
        }

        private void btHeaderItemFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.Font = lbHeaderConstantItemFont.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                ((ReportItem)lbxHeaderItems.SelectedItem).Font = fontDialog.Font;
                lbHeaderConstantItemFont.Font = fontDialog.Font;
            }
        }

        private void mtbHeaderItemIndex_Leave(object sender, EventArgs e)
        {
            int value = 0;
            string oldValue = String.Empty;
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                if (mtbHeaderItemIndex.Text.Trim() != String.Empty)
                {
                    value = Convert.ToInt32(mtbHeaderItemIndex.Text.Trim());

                    switch (lbxHeaderItems.SelectedItem.GetType().Name)
                    {
                        case "ReportParameterItem":
                            ((ReportParameterItem)lbxHeaderItems.SelectedItem).Index = value;
                            break;

                        case "ReportImageItem":
                            ((ReportImageItem)lbxHeaderItems.SelectedItem).Index = value;
                            break;
                    }
                    this.RefreshListBox(ReportArea.Header);
                }
            }
        }

        private void cbHeaderDataSourceItemField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1 && cbHeaderDataSourceItemField.SelectedIndex != -1)
            {
                if (lbxHeaderItems.SelectedItem.GetType().Name == "ReportDataSourceItem")
                {
                    ((ReportDataSourceItem)lbxHeaderItems.SelectedItem).ColumnName = cbHeaderDataSourceItemField.SelectedValue.ToString();
                    this.RefreshListBox(ReportArea.Header);
                }
            }
        }

        private void cbHeaderConstantItemContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1 && cbHeaderConstantItemContent.SelectedIndex != -1)
            {
                if (lbxHeaderItems.SelectedItem.GetType().Name == "ReportConstantItem")
                {
                    ((ReportConstantItem)lbxHeaderItems.SelectedItem).Style = (Infolight.EasilyReportTools.ReportConstantItem.StyleType)cbHeaderConstantItemContent.SelectedValue;
                    this.RefreshListBox(ReportArea.Header);
                }
            }
        }

        private void cbHeaderConstantItemContentAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxHeaderItems.SelectedItem).ContentAlignment = (HorizontalAlignment)cbHeaderConstantItemContentAlign.SelectedItem;
            }
        }

        private void cbHeaderConstantItemPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxHeaderItems.SelectedItem).Position = (ReportItem.PositionAlign)cbHeaderConstantItemPosition.SelectedItem;
            }
        }

        private void mtbHeaderConstantItemCells_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                if(mtbHeaderConstantItemCells.Text.Trim() != String.Empty)
                {
                    value = Convert.ToInt32(mtbHeaderConstantItemCells.Text.Trim());
                }
                ((ReportItem)lbxHeaderItems.SelectedItem).Cells = value;
            }
        }

        private void cbHeaderConstantItemNewLine_Click(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxHeaderItems.SelectedItem).NewLine = cbHeaderConstantItemNewLine.Checked;
            }
        }

        private void tbHeaderConstantItemFormat_TextChanged(object sender, EventArgs e)
        {
            if (lbxHeaderItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxHeaderItems.SelectedItem).Format = tbHeaderConstantItemFormat.Text.Trim();
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
                if (lbxSelectedFieldItems.SelectedIndex > 0)
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
                if (lbxSelectedFieldItems.SelectedIndex < lbxSelectedFieldItems.Items.Count - 1)
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
            PropertyChange();
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


        private void cbColumnGridLine_Click(object sender, EventArgs e)
        {
            TempReport.Format.ColumnGridLine = this.cbColumnGridLine.Checked;
        }

        private void cbColumnInsideLine_Click(object sender, EventArgs e)
        {
            TempReport.Format.ColumnInsideGridLine = this.cbColumnInsideLine.Checked;
        }

        private void cbRowGridLine_Click(object sender, EventArgs e)
        {
            TempReport.Format.RowGridLine = this.cbRowGridLine.Checked;
        }

        private void cbNewLine_Click(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.NewLine = cbFieldItemNewLine.Checked;
            }
            mtbNewLinePosition.Enabled = cbFieldItemNewLine.Checked;
        }

        private void cbSuppressIfDuplicated_Click(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.SuppressIfDuplicated = cbSuppressIfDuplicated.Checked;
            }
        }

        private void tbColumnCaption_TextChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.Caption = tbColumnCaption.Text.Trim();
            }
        }

        private void cbCaptionAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.CaptionAlignment = (HorizontalAlignment)cbCaptionAlignment.SelectedItem;
            }
        }

        private void cbColumnAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.ColumnAlignment = (HorizontalAlignment)cbColumnAlignment.SelectedItem;
            }
        }

        private void mtbWidth_TextChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                if (mtbWidth.Text.Trim().Length > 0)
                {
                    FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                    field.Width = Convert.ToInt32(mtbWidth.Text.Trim());
                }
            }
        }

        private void mtbNewLinePosition_TextChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                if (mtbNewLinePosition.Text.Trim().Length > 0)
                {
                    FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                    field.NewLinePostion = Convert.ToInt32(mtbNewLinePosition.Text.Trim());
                }
            }
        }

        private void mtbFieldCells_TextChanged(object sender, EventArgs e)
        {
            //int value = 0;

            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                if (mtbFieldCells.Text.Trim().Length > 0)
                {
                    FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                    field.Cells = Convert.ToInt32(mtbFieldCells.Text.Trim());
                }
            }
        }

        private void cbSumType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.Sum = (FieldItem.SumType)cbSumType.SelectedItem;
            }
        }

        private void tbGroupTotalCaption_TextChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.GroupTotalCaption = tbGroupTotalCaption.Text;
            }
        }

        private void tbTotalCaption_TextChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.TotalCaption = tbTotalCaption.Text;
            }
        }

        #endregion

        private void cbGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.Group = (FieldItem.GroupType)cbGroupType.SelectedItem;
            }
        }

        private void cbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSelectedFieldItems.SelectedIndex != -1)
            {
                FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                field.Order = (FieldItem.OrderType)cbOrderType.SelectedValue;
            }
        }

        private void FieldItemPropertyChange(FieldItem fieldItem)
        {
            bool isEnable = true;
            if (fieldItem != null)
            {
                tbColumnCaption.Text = fieldItem.Caption;
                cbCaptionAlignment.SelectedItem = fieldItem.CaptionAlignment;
                cbColumnAlignment.SelectedItem = fieldItem.ColumnAlignment;
                mtbWidth.Text = fieldItem.Width.ToString();
                cbFieldItemNewLine.Checked = fieldItem.NewLine;
                cbSuppressIfDuplicated.Checked = fieldItem.SuppressIfDuplicated;
                mtbNewLinePosition.Text = fieldItem.NewLinePostion.ToString();
                mtbFieldCells.Text = fieldItem.Cells.ToString();
                cbOrderType.SelectedValue = fieldItem.Order;
                cbGroupType.SelectedItem = fieldItem.Group;
                tbGroupTotalCaption.Text = fieldItem.GroupTotalCaption;
                cbSumType.SelectedItem = fieldItem.Sum;
                tbTotalCaption.Text = fieldItem.TotalCaption;

                isEnable = true;
            }
            else
            {
                tbColumnCaption.Text = String.Empty;
                cbCaptionAlignment.SelectedIndex = 0;
                cbColumnAlignment.SelectedIndex = 0;
                mtbWidth.Text = String.Empty;
                cbFieldItemNewLine.Checked = false;
                cbSuppressIfDuplicated.Checked = false;
                mtbNewLinePosition.Text = String.Empty;
                mtbFieldCells.Text = string.Empty;
                cbOrderType.SelectedIndex = 0;
                cbGroupType.SelectedIndex = 0;
                tbGroupTotalCaption.Text = string.Empty;
                cbSumType.SelectedIndex = 0;
                tbTotalCaption.Text = string.Empty;

                isEnable = false;
            }

            tbColumnCaption.Enabled = isEnable;
            cbCaptionAlignment.Enabled = isEnable;
            cbColumnAlignment.Enabled = isEnable;
            mtbWidth.Enabled = isEnable;
            cbFieldItemNewLine.Enabled = isEnable;
            cbSuppressIfDuplicated.Enabled = isEnable;
            mtbNewLinePosition.Enabled = isEnable;
            mtbFieldCells.Enabled = isEnable;
            cbOrderType.Enabled = isEnable;
            cbGroupType.Enabled = isEnable;
            tbGroupTotalCaption.Enabled = isEnable;
            cbSumType.Enabled = isEnable;
            tbTotalCaption.Enabled = isEnable;

        }

        private void PropertyChange()
        {
            if (lbxSelectedFieldItems.SelectedIndices.Count == 1)
            {
                if (lbxSelectedFieldItems.SelectedIndex != -1)
                {
                    SetEnable(ReportArea.DetailColumns, true);

                    FieldItem field = lbxSelectedFieldItems.SelectedItem as FieldItem;
                    FieldItemPropertyChange(field);
                }
                else
                {
                    SetEnable(ReportArea.DetailColumns, false);
                    FieldItemPropertyChange(null);
                }
            }
            else
            {
                SetEnable(ReportArea.DetailColumns, false);
                FieldItemPropertyChange(null);
            }

        }
        #endregion

        #region Report Footer
        private void lbxFooterItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                FooterItemPropertyChange((ReportItem)lbxFooterItems.SelectedItem);
                SetEnable(ReportArea.Footer, true);
            }
            else
            {
                FooterItemPropertyChange(null);
                SetEnable(ReportArea.Footer, false);
            }
        }

        private void btAddFooterItem_Click(object sender, EventArgs e)
        {
            ReportItem reportItem = null;
            if (this.CheckReportItem(cbFooterItemType.SelectedValue.ToString()))
            {
                reportItem = (ReportItem)Activator.CreateInstance(Type.GetType(ComponentInfo.NameSpace + ".Report" + cbFooterItemType.SelectedValue.ToString()));
                lbxFooterItems.Items.Add(reportItem);
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

        private void FooterItemPropertyChange(ReportItem reportItem)
        {
            bool isEnable = true;
            if (reportItem != null)
            {
                isEnable = true;

                cbFooterConstantItemContent.Enabled = !isEnable;
                cbFooterDataSourceItemField.Enabled = !isEnable;
                mtbFooterItemIndex.Enabled = !isEnable;
                cbFooterConstantItemContent.Visible = !isEnable;
                cbFooterDataSourceItemField.Visible = !isEnable;
                mtbFooterItemIndex.Visible = !isEnable;
                btAddPictureF.Visible = !isEnable;
                btViewF.Visible = !isEnable;

                switch (reportItem.GetType().Name)
                {
                    case "ReportConstantItem":
                        lbFooterConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbContent");
                        cbFooterConstantItemContent.Enabled = isEnable;
                        cbFooterConstantItemContent.Visible = isEnable;
                        cbFooterConstantItemContent.SelectedValue = ((ReportConstantItem)reportItem).Style;
                        break;

                    case "ReportParameterItem":
                        lbFooterConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbIndex");
                        mtbFooterItemIndex.Enabled = isEnable;
                        mtbFooterItemIndex.Visible = isEnable;
                        mtbFooterItemIndex.Text = ((ReportParameterItem)reportItem).Index.ToString();
                        break;

                    case "ReportDataSourceItem":
                        lbFooterConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbField");
                        cbFooterDataSourceItemField.Enabled = isEnable;
                        cbFooterDataSourceItemField.Visible = isEnable;
                        if (((ReportDataSourceItem)reportItem).ColumnName != null)
                        {
                            cbFooterDataSourceItemField.SelectedValue = ((ReportDataSourceItem)reportItem).ColumnName;
                        }
                        else
                        {
                            cbFooterDataSourceItemField.SelectedIndex = -1;
                        }
                        break;

                    case "ReportImageItem":
                        lbFooterConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbIndex");
                        mtbFooterItemIndex.Enabled = !isEnable;
                        mtbFooterItemIndex.Visible = isEnable;
                        mtbFooterItemIndex.Text = ((ReportImageItem)reportItem).Index.ToString();
                        btAddPictureF.Visible = isEnable;
                        btViewF.Visible = isEnable;
                        break;
                }

                cbFooterConstantItemContentAlign.SelectedItem = reportItem.ContentAlignment;
                cbFooterConstantItemNewLine.Checked = reportItem.NewLine;
                cbFooterConstantItemPosition.SelectedItem = reportItem.Position;
                mtbFooterConstantItemCells.Text = reportItem.Cells.ToString();
                tbFooterConstantItemFormat.Text = reportItem.Format;
                lbFooterConstantItemFont.Font = reportItem.Font;
            }
            else
            {
                isEnable = false;
                lbFooterConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbContent");
                cbFooterConstantItemContent.Visible = !isEnable;
                cbFooterDataSourceItemField.Visible = isEnable;
                mtbFooterItemIndex.Visible = isEnable;

                mtbFooterItemIndex.Text = String.Empty;
                cbFooterConstantItemContentAlign.SelectedIndex = 0;
                cbFooterConstantItemNewLine.Checked = false;
                cbFooterConstantItemPosition.SelectedIndex = 0;
                mtbFooterConstantItemCells.Text = String.Empty;
                tbFooterConstantItemFormat.Text = String.Empty;

                btAddPictureF.Visible = isEnable;
                btViewF.Visible = isEnable;
            }

            cbFooterConstantItemContent.Enabled = isEnable;
            cbFooterDataSourceItemField.Enabled = isEnable;
            cbFooterConstantItemContentAlign.Enabled = isEnable;
            cbFooterConstantItemNewLine.Enabled = isEnable;
            cbFooterConstantItemPosition.Enabled = isEnable;
            mtbFooterConstantItemCells.Enabled = isEnable;
            tbFooterConstantItemFormat.Enabled = isEnable;
            btFooterItemFont.Enabled = isEnable;
            plFooterItemFontSample.Enabled = isEnable;
        }

        private void btAddPictureF_Click(object sender, EventArgs e)
        {
            fmPictures fm = new fmPictures(this.TempReport);
            if (fm.ShowDialog() == DialogResult.OK)
            {
                mtbFooterItemIndex.Text = fm.SelectedIndex;

                ((ReportImageItem)lbxFooterItems.SelectedItem).Index = Convert.ToInt32(mtbFooterItemIndex.Text.Trim());
                this.RefreshListBox(ReportArea.Footer);
            }
        }

        private void btViewF_Click(object sender, EventArgs e)
        {
            if (mtbFooterItemIndex.Text != String.Empty && this.TempReport.Images.Count > 0)
            {
                fmPictureView fm = new fmPictureView(this.TempReport, Convert.ToInt32(mtbFooterItemIndex.Text.Trim()));
                fm.ShowDialog();
            }
        }

        private void cbFooterConstantItemContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1 && cbFooterConstantItemContent.SelectedIndex != -1)
            {
                if (lbxFooterItems.SelectedItem.GetType().Name == "ReportConstantItem")
                {
                    ((ReportConstantItem)lbxFooterItems.SelectedItem).Style = (Infolight.EasilyReportTools.ReportConstantItem.StyleType)cbFooterConstantItemContent.SelectedValue;
                    this.RefreshListBox(ReportArea.Footer);
                }
            }
        }

        private void cbFooterDataSourceItemField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1 && cbFooterDataSourceItemField.SelectedIndex != -1)
            {
                if (lbxFooterItems.SelectedItem.GetType().Name == "ReportDataSourceItem")
                {
                    ((ReportDataSourceItem)lbxFooterItems.SelectedItem).ColumnName = cbFooterDataSourceItemField.SelectedValue.ToString();
                    this.RefreshListBox(ReportArea.Footer);
                }
            }
        }

        private void mtbFooterItemIndex_Leave(object sender, EventArgs e)
        {
            int value = 0;
            string oldValue = String.Empty;
            if (lbxFooterItems.SelectedIndex != -1)
            {
                if (mtbFooterItemIndex.Text.Trim() != String.Empty)
                {
                    value = Convert.ToInt32(mtbFooterItemIndex.Text.Trim());

                    switch (lbxFooterItems.SelectedItem.GetType().Name)
                    {
                        case "ReportParameterItem":
                            ((ReportParameterItem)lbxFooterItems.SelectedItem).Index = value;
                            break;

                        case "ReportImageItem":
                            ((ReportImageItem)lbxFooterItems.SelectedItem).Index = value;
                            break;
                    }
                    this.RefreshListBox(ReportArea.Footer);
                }
            }
        }

        private void cbFooterConstantItemContentAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxFooterItems.SelectedItem).ContentAlignment = (HorizontalAlignment)cbFooterConstantItemContentAlign.SelectedItem;
            }
        }

        private void cbFooterConstantItemPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxFooterItems.SelectedItem).Position = (ReportItem.PositionAlign)cbFooterConstantItemPosition.SelectedItem;
            }
        }

        private void mtbFooterConstantItemCells_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            if (lbxFooterItems.SelectedIndex != -1)
            {
                if (mtbFooterConstantItemCells.Text.Trim() != String.Empty)
                {
                    value = Convert.ToInt32(mtbFooterConstantItemCells.Text.Trim());
                }
                ((ReportItem)lbxFooterItems.SelectedItem).Cells = value;
            }
        }

        private void cbFooterConstantItemNewLine_Click(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxFooterItems.SelectedItem).NewLine = cbFooterConstantItemNewLine.Checked;
            }
        }

        private void tbFooterConstantItemFormat_TextChanged(object sender, EventArgs e)
        {
            if (lbxFooterItems.SelectedIndex != -1)
            {
                ((ReportItem)lbxFooterItems.SelectedItem).Format = tbFooterConstantItemFormat.Text.Trim();
            }
        }

        private void btFooterItemFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.Font = lbFooterConstantItemFont.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                ((ReportItem)lbxFooterItems.SelectedItem).Font = fontDialog.Font;
                lbFooterConstantItemFont.Font = fontDialog.Font;
            }
        }
        #endregion

        #region Report Setting
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

        private void cbDefault_Click(object sender, EventArgs e)
        {
            tbSmtpServer.ReadOnly = cbDefault.Checked;
            if (cbDefault.Checked)
            {
                if (tbMailFrom.Text.Trim().Length == 0)
                {
                    tbSmtpServer.Text = string.Empty;
                }
                else
                {
                    string[] mailaddress = tbMailFrom.Text.Trim().Split('@');
                    tbSmtpServer.Text = "smtp." + mailaddress[1];
                }
            }
        }

        private void tbMailFrom_Leave(object sender, EventArgs e)
        {
            if (tbMailFrom.Text.Trim().Length == 0)
            {
                if (cbDefault.Checked)
                {
                    tbSmtpServer.Text = string.Empty;
                }
                return;
            }

            string[] mailaddress = tbMailFrom.Text.Trim().Split('@');

            if (mailaddress.Length != 2 || mailaddress[0].Length == 0 || mailaddress[1].Length == 0)
            {
                this.ShowMessage(MailMessageInfo.InvalidMailAddress, MsgMode.Error);
                tbMailFrom.Focus();
                return;
            }
            else if (cbDefault.Checked)
            {
                tbSmtpServer.Text = "smtp." + mailaddress[1];
            }
        }
        #endregion

        #region Common Function
        private void ShowMessage(string message, MsgMode msgMode)
        {
            switch (msgMode)
            {
                case MsgMode.Success:
                    MessageBox.Show(this, message, TitleMsgInfo.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MsgMode.Error:
                    MessageBox.Show(this, message, TitleMsgInfo.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case MsgMode.Warning:
                    MessageBox.Show(this, message, TitleMsgInfo.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private enum MsgMode
        {
            Success,
            Error,
            Warning
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

        DataTable dtField = new DataTable();
        private void InitField()
        {
            DataSourceItem fieldItem = TempReport.FieldItems[comboBoxField.SelectedIndex];

            DataSet ds = null;
            DataTable table = null;
            if (fieldItem.DataSource is InfoBindingSource)
            {
                InfoBindingSource ibs = fieldItem.DataSource as InfoBindingSource;
                DataView view = ibs.List as DataView;
                if (view != null)
                {
                    table = view.Table;
                }
                ds = DBUtils.GetDataDictionary(ibs, false);
            }
            else if (fieldItem.DataSource is WebDataSource)
            {
                WebDataSource wds = fieldItem.DataSource as WebDataSource;
                DataView view = wds.View;
                if (view != null)
                {
                    table = view.Table;
                }
                ds = DBUtils.GetDataDictionary(wds, false);
            }
            if (table != null && ds != null)
            {
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

            PropertyChange();
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


        private void Init()
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

            cbExportFormat.SelectedItem = this.TempReport.Format.ExportFormat;

            #region Header

            DictionaryEntry[] items = new DictionaryEntry[] 
            {
                new DictionaryEntry("ConstantItem",ERptMultiLanguage.GetLanValue("ConstantItem"))
                ,new DictionaryEntry("DataSourceItem",ERptMultiLanguage.GetLanValue("DataSourceItem"))
                ,new DictionaryEntry("ImageItem",ERptMultiLanguage.GetLanValue("ImageItem"))
            };
            cbHeaderItemType.DataSource = items;
            cbHeaderItemType.DisplayMember = "Value";
            cbHeaderItemType.ValueMember = "Key";
            cbHeaderItemType.SelectedIndex = 0;
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
                HeaderItemPropertyChange(null);
                SetEnable(ReportArea.Header, false);
            }
            else
            {
                SetEnable(ReportArea.Header, true);
            }
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
            cbFooterItemType.DataSource = items;
            cbFooterItemType.DisplayMember = "Value";
            cbFooterItemType.ValueMember = "Key";
            cbFooterItemType.SelectedIndex = 0;
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
                FooterItemPropertyChange(null);
                SetEnable(ReportArea.Footer, false);
            }
            else
            {
                SetEnable(ReportArea.Footer, true);
            }
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
            mtbPageHeight.Text = TempReport.Format.PageHeight.ToString("f2");



            //Output Mode
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
            tbMailFrom.Text = TempReport.MailSetting.MailFrom;
            tbMailTo.Text = TempReport.MailSetting.MailTo;
            tbSmtpServer.Text = TempReport.MailSetting.Host;
            tbPassword.Text = TempReport.MailSetting.Password;
            tbMailTitle.Text = TempReport.MailSetting.Subject;
            #endregion

            #region Page Margin for Pdf
            mtbLeft.Text = this.TempReport.Format.MarginLeft.ToString();
            mtbRight.Text = this.TempReport.Format.MarginRight.ToString();
            mtbTop.Text = this.TempReport.Format.MarginTop.ToString();
            mtbBottom.Text = this.TempReport.Format.MarginBottom.ToString();
            #endregion

            ShowStatusMsg();

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

        private void InitComponentValue()
        {
            #region Variable Definition
            DataSet dsReportItemDDColumns = null;
            DataTable dtReportItemDDColumns = null;
            DataRow drTemp = null;
            string tableName = String.Empty;
            ReportConstantItem reportConstantItem = new ReportConstantItem();
            List<DictionaryEntry> orderTypeList = new List<DictionaryEntry>();
            List<DictionaryEntry> styleTypeListHeader = new List<DictionaryEntry>();
            List<DictionaryEntry> styleTypeListFooter = new List<DictionaryEntry>();
            #endregion

            #region Report Header
            styleTypeListHeader = this.GetStyleTypeList();

            cbHeaderConstantItemContent.DataSource = styleTypeListHeader;
            cbHeaderConstantItemContent.DisplayMember = "Value";
            cbHeaderConstantItemContent.ValueMember = "Key";

            //cbHeaderConstantItemContent.SelectedIndex = -1;

            foreach (object item in Enum.GetValues(typeof(HorizontalAlignment)))
            {
                if (!cbHeaderConstantItemContentAlign.Items.Contains(item))
                {
                    cbHeaderConstantItemContentAlign.Items.Add(item);
                }
            }

            foreach (object item in Enum.GetValues(typeof(ReportItem.PositionAlign)))
            {
                if (!cbHeaderConstantItemPosition.Items.Contains(item))
                {
                    cbHeaderConstantItemPosition.Items.Add(item);
                }
            }

            if (TempReport.HeaderDataSource != null)
            {
                dsReportItemDDColumns = DBUtils.GetDataDictionary((InfoBindingSource)TempReport.HeaderDataSource, false);
                tableName = ((InfoBindingSource)TempReport.HeaderDataSource).DataMember;
                if (dsReportItemDDColumns.Tables[tableName].Rows.Count > 0)
                {
                    dsReportItemDDColumns.Tables[tableName].Rows.RemoveAt(0);
                }
                dtReportItemDDColumns = dsReportItemDDColumns.Tables[tableName].Clone();

                foreach (DataColumn dc in ((DataView)((InfoBindingSource)TempReport.HeaderDataSource).List).Table.Columns)
                {
                    if (dsReportItemDDColumns.Tables[tableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsReportItemDDColumns.Tables[tableName].Rows)
                        {
                            if (String.Compare(dr["FIELD_NAME"].ToString(), dc.Caption, true) == 0)
                            {
                                dtReportItemDDColumns.ImportRow(dr);
                                break;
                            }
                        }
                    }
                    else
                    {
                        drTemp = dtReportItemDDColumns.NewRow();
                        drTemp["CAPTION"] = dc.Caption.ToUpper();
                        drTemp["FIELD_NAME"] = dc.Caption.ToUpper();
                        drTemp["FIELD_LENGTH"] = dc.MaxLength;
                        dtReportItemDDColumns.Rows.Add(drTemp);
                    }
                }

                cbHeaderDataSourceItemField.DataSource = dtReportItemDDColumns;
                cbHeaderDataSourceItemField.DisplayMember = "CAPTION";
                cbHeaderDataSourceItemField.ValueMember = "FIELD_NAME";
            }

            cbHeaderDataSourceItemField.SelectedIndex = -1;
            cbHeaderConstantItemContentAlign.SelectedIndex = 0;
            cbHeaderConstantItemPosition.SelectedIndex = 0;
            #endregion

            #region Report Details
            #region Columns
            foreach (object item in Enum.GetValues(typeof(HorizontalAlignment)))
            {
                if (!cbCaptionAlignment.Items.Contains(item))
                {
                    cbCaptionAlignment.Items.Add(item);
                }

                if (!cbColumnAlignment.Items.Contains(item))
                {
                    cbColumnAlignment.Items.Add(item);
                }
            }

            foreach (object item in Enum.GetValues(typeof(FieldItem.SumType)))
            {
                if (!cbSumType.Items.Contains(item))
                {
                    cbSumType.Items.Add(item);
                }
            }

            cbCaptionAlignment.SelectedIndex = 0;
            cbColumnAlignment.SelectedIndex = 0;
            cbSumType.SelectedIndex = 0;
            #endregion

            #region Group Columns
            foreach (object item in Enum.GetValues(typeof(FieldItem.GroupType)))
            {

                if (!cbGroupType.Items.Contains(item))
                {
                    cbGroupType.Items.Add(item);
                }
            }

            orderTypeList.Clear();
            //orderTypeList.Add(new DictionaryEntry("None", ERptMultiLanguage.GetLanValue("None")));
            foreach (object item in Enum.GetValues(typeof(FieldItem.OrderType)))
            {
                orderTypeList.Add(new DictionaryEntry(item, ERptMultiLanguage.GetLanValue(item.ToString())));
            }

            cbOrderType.DataSource = orderTypeList;
            cbOrderType.DisplayMember = "Value";
            cbOrderType.ValueMember = "Key";

            cbGroupType.SelectedIndex = 0;
            cbOrderType.SelectedIndex = 0;
            #endregion
           // DisplayDetailsPage(tabPageColumnProperties);
            #endregion

            #region Report Footer
            styleTypeListFooter = this.GetStyleTypeList();
            cbFooterConstantItemContent.DataSource = styleTypeListFooter;
            cbFooterConstantItemContent.DisplayMember = "Value";
            cbFooterConstantItemContent.ValueMember = "Key";

            cbFooterConstantItemContent.SelectedIndex = -1;

            foreach (object item in Enum.GetValues(typeof(HorizontalAlignment)))
            {
                if (!cbFooterConstantItemContentAlign.Items.Contains(item))
                {
                    cbFooterConstantItemContentAlign.Items.Add(item);
                }
            }

            foreach (object item in Enum.GetValues(typeof(ReportItem.PositionAlign)))
            {
                if (!cbFooterConstantItemPosition.Items.Contains(item))
                {
                    cbFooterConstantItemPosition.Items.Add(item);
                }
            }

            if (TempReport.HeaderDataSource != null)
            {
                dsReportItemDDColumns = DBUtils.GetDataDictionary((InfoBindingSource)TempReport.HeaderDataSource, false);
                tableName = ((InfoBindingSource)TempReport.HeaderDataSource).DataMember;
                if (dsReportItemDDColumns.Tables[tableName].Rows.Count > 0)
                {
                    dsReportItemDDColumns.Tables[tableName].Rows.RemoveAt(0);
                }
                dtReportItemDDColumns = dsReportItemDDColumns.Tables[tableName].Clone();

                foreach (DataColumn dc in ((DataView)((InfoBindingSource)TempReport.HeaderDataSource).List).Table.Columns)
                {
                    if (dsReportItemDDColumns.Tables[tableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsReportItemDDColumns.Tables[tableName].Rows)
                        {
                            if (String.Compare(dr["FIELD_NAME"].ToString(), dc.Caption, true) == 0)
                            {
                                dtReportItemDDColumns.ImportRow(dr);
                                break;
                            }
                        }
                    }
                    else
                    {
                        drTemp = dtReportItemDDColumns.NewRow();
                        drTemp["CAPTION"] = dc.Caption.ToUpper();
                        drTemp["FIELD_NAME"] = dc.Caption.ToUpper();
                        drTemp["FIELD_LENGTH"] = dc.MaxLength;
                        dtReportItemDDColumns.Rows.Add(drTemp);
                    }
                }

                cbFooterDataSourceItemField.DataSource = dtReportItemDDColumns;
                cbFooterDataSourceItemField.DisplayMember = "CAPTION";
                cbFooterDataSourceItemField.ValueMember = "FIELD_NAME";
            }

            cbFooterDataSourceItemField.SelectedIndex = -1;
            cbFooterConstantItemContentAlign.SelectedIndex = 0;
            cbFooterConstantItemPosition.SelectedIndex = 0;
            #endregion

        }

        private List<DictionaryEntry> GetStyleTypeList()
        {
            List<DictionaryEntry> styleTypeList = new List<DictionaryEntry>();

            foreach (object item in Enum.GetValues(typeof(ReportConstantItem.StyleType)))
            {
                styleTypeList.Add(new DictionaryEntry(item,ERptMultiLanguage.GetLanValue(item.ToString())));
            }

            return styleTypeList;
        }

        private void InitLanguage()
        {
            #region MenuItem
            menuItemFile.Text = ERptMultiLanguage.GetLanValue("MenuFile");
            menuItemReadTemplate.Text = ERptMultiLanguage.GetLanValue("MenuReadTemplate");
            menuItemSaveTemplate.Text = ERptMultiLanguage.GetLanValue("MenuSaveTemplate");
            menuItemSaveAsTemplate.Text = ERptMultiLanguage.GetLanValue("MenuSaveAsTemplate");
            menuItemDeleteTemplate.Text = ERptMultiLanguage.GetLanValue("MenuDeleteTemplate");
            #endregion

            #region Tab Control
            tabPageReportHeader.Text = ERptMultiLanguage.GetLanValue("TabControlReportHeader");
            tabPageDetails.Text = ERptMultiLanguage.GetLanValue("TabControlReportDetails");
            tabPageReportFooter.Text = ERptMultiLanguage.GetLanValue("TabControlReportFooter");
            tabPageReportSetting.Text = ERptMultiLanguage.GetLanValue("TabControlReportSetting");
            //tabPageColumns.Text = ERptMultiLanguage.GetLanValue("TabControlColumns");
            //tabPageGroupColumns.Text = ERptMultiLanguage.GetLanValue("TabControlGroupColumns");
            //tabPageColumnProperties.Text = ERptMultiLanguage.GetLanValue("TabControlColumnProperties");
            //tabPageGroupColumnProperties.Text = ERptMultiLanguage.GetLanValue("TabControlGroupColumnProperties");
            tabPageHeaderItemProperties.Text = ERptMultiLanguage.GetLanValue("TabControlHeaderItemProperties");
            tabPageFooterItemProperties.Text = ERptMultiLanguage.GetLanValue("TabControlFooterItemProperties");
            #endregion

            #region Group Box
            gbContentSettingHeader.Text = ERptMultiLanguage.GetLanValue("gbContentSetting");
            gbContentSettingDetails.Text = ERptMultiLanguage.GetLanValue("gbContentSetting");
            gbContentSettingFooter.Text = ERptMultiLanguage.GetLanValue("gbContentSetting");
            gbContentSettingGroupColumns.Text = ERptMultiLanguage.GetLanValue("gbContentSetting");
            gbStyleSettingHeader.Text = ERptMultiLanguage.GetLanValue("gbStyleSetting");
            gbStyleSettingDetails.Text = ERptMultiLanguage.GetLanValue("gbStyleSetting");
            gbStyleSettingFooter.Text = ERptMultiLanguage.GetLanValue("gbStyleSetting");
            gbColumns.Text = ERptMultiLanguage.GetLanValue("gbColumns");
            gbGroupColumns.Text = ERptMultiLanguage.GetLanValue("gbGroupColumns");
            gbSelectedColumns.Text = ERptMultiLanguage.GetLanValue("gbSelectedColumns");
            gbSelectedGroupColumns.Text = ERptMultiLanguage.GetLanValue("gbSelectedGroupColumns");
            gbShowGridLine.Text = ERptMultiLanguage.GetLanValue("gbShowGridLine");
            gbPageSetting.Text = ERptMultiLanguage.GetLanValue("gbPageSetting");
            gbOutputSetting.Text = ERptMultiLanguage.GetLanValue("gbOutputSetting");
            gbPageSize.Text = ERptMultiLanguage.GetLanValue("gbPageSize");
            gbOrientation.Text = ERptMultiLanguage.GetLanValue("gbPrintOrientation");
            gbEmailConfig.Text = ERptMultiLanguage.GetLanValue("gbMailConfig");
            gbHeaderItems.Text = ERptMultiLanguage.GetLanValue("gbHeaderItems");
            gbFooterItems.Text = ERptMultiLanguage.GetLanValue("gbFooterItems");
            gbPageMargin.Text = ERptMultiLanguage.GetLanValue("gbPageMargin");
            #endregion

            #region Button
            btHeaderFont.Text = ERptMultiLanguage.GetLanValue("btHeaderFont");
            btFieldFont.Text = ERptMultiLanguage.GetLanValue("btFieldFont");
            btFooterFont.Text = ERptMultiLanguage.GetLanValue("btFooterFont");
            btAddHeaderItems.Text = ERptMultiLanguage.GetLanValue("btAdd");
            btRemoveHeaderItems.Text = ERptMultiLanguage.GetLanValue("btRemove");
            btAddFooterItem.Text = ERptMultiLanguage.GetLanValue("btAdd");
            btRemoveFooterItem.Text = ERptMultiLanguage.GetLanValue("btRemove");
            btPreview.Text = ERptMultiLanguage.GetLanValue("btPreview");
            btExport.Text = ERptMultiLanguage.GetLanValue("btExport");
            btSave.Text = ERptMultiLanguage.GetLanValue("btSave");
            btCancel.Text = ERptMultiLanguage.GetLanValue("btClose");
            btHeaderItemFont.Text = ERptMultiLanguage.GetLanValue("btFont");
            btFooterItemFont.Text = ERptMultiLanguage.GetLanValue("btFont");
            #endregion

            #region Check Box
            cbColumnGridLine.Text = ERptMultiLanguage.GetLanValue("cbColumnGridLine");
            cbColumnInsideLine.Text = ERptMultiLanguage.GetLanValue("cbColumnInsideLine");
            cbRowGridLine.Text = ERptMultiLanguage.GetLanValue("cbRowGridLine");
            cbGroupTotal.Text = ERptMultiLanguage.GetLanValue("cbGroupTotal");
            cbFieldItemNewLine.Text = ERptMultiLanguage.GetLanValue("cbNewLine");
            cbSuppressIfDuplicated.Text = ERptMultiLanguage.GetLanValue("cbSuppressIfDuplicated");
            cbHeaderConstantItemNewLine.Text = ERptMultiLanguage.GetLanValue("cbNewLine");
            cbFooterConstantItemNewLine.Text = ERptMultiLanguage.GetLanValue("cbNewLine");
            cbHeaderRepeat.Text = ERptMultiLanguage.GetLanValue("cbHeaderRepeat");
            #endregion

            #region Label
            lbHeaderItemType.Text = ERptMultiLanguage.GetLanValue("lbHeaderItemType");
            lbFooterItemType.Text = ERptMultiLanguage.GetLanValue("lbFooterItemType");
            lbDataSourceIndex.Text = ERptMultiLanguage.GetLanValue("lbDataSourceIndex");
            lbTotalCaption.Text = ERptMultiLanguage.GetLanValue("lbTotalCaption");
            lbGroupGap.Text = ERptMultiLanguage.GetLanValue("lbGroupGap");
            lbGroupTotalCaption.Text = ERptMultiLanguage.GetLanValue("lbGroupTotalCaption");
            lbColumnCaption.Text = ERptMultiLanguage.GetLanValue("lbColumnCaption");
            lbCaptionAlignment.Text = ERptMultiLanguage.GetLanValue("lbCaptionAlignment");
            lbCaptionStyle.Text = ERptMultiLanguage.GetLanValue("lbCaptionStyle");
            lbColumnAlignment.Text = ERptMultiLanguage.GetLanValue("lbColumnAlignment");
            lbWidth.Text = ERptMultiLanguage.GetLanValue("lbWidth");
            lbNewLinePosition.Text = ERptMultiLanguage.GetLanValue("lbNewLinePosition");
            lbOrderType.Text = ERptMultiLanguage.GetLanValue("lbOrderType");
            lbSumType.Text = ERptMultiLanguage.GetLanValue("lbSumType");
            lbPageRecords.Text = ERptMultiLanguage.GetLanValue("lbPageRecords");
            lbPageHeight.Text = ERptMultiLanguage.GetLanValue("lbPageHeight");
            lbOutputFileName.Text = ERptMultiLanguage.GetLanValue("lbOutputFileName");
            lbOutputFilePath.Text = ERptMultiLanguage.GetLanValue("lbOutputFilePath");
            lbOutputMode.Text = ERptMultiLanguage.GetLanValue("lbOutputMode");
            lbHeaderConstantItemContent.Text = ERptMultiLanguage.GetLanValue("lbContent");
            lbHeaderConstantItemContentAlign.Text = ERptMultiLanguage.GetLanValue("lbContentAlignment");
            lbFooterConstantItemContentAlign.Text = ERptMultiLanguage.GetLanValue("lbContentAlignment");
            lbHeaderConstantItemPosition.Text = ERptMultiLanguage.GetLanValue("lbPosition");
            lbFooterConstantItemPosition.Text = ERptMultiLanguage.GetLanValue("lbPosition");
            lbHeaderConstantItemCells.Text = ERptMultiLanguage.GetLanValue("lbCells");
            lbFooterConstantItemCells.Text = ERptMultiLanguage.GetLanValue("lbCells");
            lbFieldCells.Text = ERptMultiLanguage.GetLanValue("lbCells");
            lbHeaderConstantItemFormat.Text = ERptMultiLanguage.GetLanValue("lbFormat");
            lbFooterConstantItemFormat.Text = ERptMultiLanguage.GetLanValue("lbFormat");
            lbGroupType.Text = ERptMultiLanguage.GetLanValue("lbGroupType");
            lbMailTo.Text = ERptMultiLanguage.GetLanValue("lbMailTo");
            lbMailFrom.Text = ERptMultiLanguage.GetLanValue("lbMailFrom");
            lbServer.Text = ERptMultiLanguage.GetLanValue("lbSmtpServer");
            lbPassword.Text = ERptMultiLanguage.GetLanValue("lbPassword");
            lbExportFormat.Text = ERptMultiLanguage.GetLanValue("lbExportFormat");
            lbLeft.Text = ERptMultiLanguage.GetLanValue("lbLeft");
            lbRight.Text = ERptMultiLanguage.GetLanValue("lbRight");
            lbTop.Text = ERptMultiLanguage.GetLanValue("lbTop");
            lbBottom.Text = ERptMultiLanguage.GetLanValue("lbBottom");
            lbMailTitle.Text = ERptMultiLanguage.GetLanValue("lbMailTitle");
            
            #endregion

            #region Form
            this.Text = ERptMultiLanguage.GetLanValue("fmReportExport");
            #endregion
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
            }

            return result;
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
                case ReportArea.DetailGroupColumns:
                    btGroupColumnUp.Enabled = isEnable;
                    btGroupColumnDown.Enabled = isEnable;
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
            DetailGroupColumns,
            Footer
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
            SaveReportFooter();
            SaveReportSetting();
            SaveMailSetting();

            this.TempReport.CopyTo(this.DesignReport);
        }

        private void SaveMailSetting()
        {
            TempReport.MailSetting.MailFrom = tbMailFrom.Text.Trim();
            TempReport.MailSetting.MailTo = tbMailTo.Text.Trim();
            TempReport.MailSetting.Host = tbSmtpServer.Text.Trim();
            TempReport.MailSetting.Password = tbPassword.Text.Trim();
            TempReport.MailSetting.Subject = tbMailTitle.Text.Trim();
        }

        private void SaveReportSetting()
        {
            foreach (object pagetype in Enum.GetValues(typeof(ReportFormat.PageType)))
            {
                if (((RadioButton)this.Controls.Find("rb" + pagetype.ToString(), true)[0]).Checked)
                {
                    TempReport.Format.PageSize = (ReportFormat.PageType)pagetype;
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

            if (mtbPageRecords.Text.Trim() != "")
            {
                TempReport.Format.PageRecords = Convert.ToInt32(mtbPageRecords.Text.Trim());
            }

            if (mtbPageHeight.Text.Trim().Length > 0)
            {
                TempReport.Format.PageHeight = Convert.ToDouble(mtbPageHeight.Text.Trim());
            }

            TempReport.Format.ExportFormat = (ReportFormat.ExportType)cbExportFormat.SelectedItem;

            TempReport.HeaderRepeat = cbHeaderRepeat.Checked;

            TempReport.OutputMode = (OutputModeType)cbOutputMode.SelectedItem;
            
            
            TempReport.FilePath = MyStringConverter.GetFullFilePath(tbOutputFilePath.Text.Trim(), this.tbOutputFileName.Text.Trim(), this.TempReport.Format.ExportFormat);

            //if (this.report.GetType().Name == ComponentInfo.EasilyReport)
            //{
            //    ((EasilyReport)this.report).OutputMode = (OutputModeType)cbOutputMode.SelectedItem;

            //    ((EasilyReport)report).FilePath = MyStringConverter.GetFullFilePath(tbOutputFilePath.Text.Trim(), this.tbOutputFileName.Text.Trim(), this.report.Format.ExportFormat);
            //}

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

            TempReport.HeaderFont = lbHeaderFont.Font;
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
            //this.LoadTemplate(TemplateLoadMode.Delete);
        }

        private void LoadTemplate()
        {
            fmLoadTemplate fm = new fmLoadTemplate(this.TempReport, false);

            if (fm.ShowDialog(this) == DialogResult.OK)
            {
                this.selectedTemplateName = fm.TemplateName;

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

                case "Content":

                case "Index":
                    this.RefreshListBox(ReportArea.Header);
                    break;
            }
        }

        private void propertyGridReportFooterItem_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label)
            {
                case "Field":

                case "Content":

                case "Index":
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
                    tempItem = ((ReportItem)lbxHeaderItems.SelectedItem).Copy();
                    lbxHeaderItems.Items.Insert(selectedIndex + 1, tempItem);
                    lbxHeaderItems.Items.RemoveAt(selectedIndex);
                    lbxHeaderItems.SelectedIndex = selectedIndex;
                    break;
                case ReportArea.Footer:
                    selectedIndex = lbxFooterItems.SelectedIndex;
                    tempItem = ((ReportItem)lbxFooterItems.SelectedItem).Copy();
                    lbxFooterItems.Items.Insert(selectedIndex + 1, tempItem);
                    lbxFooterItems.Items.RemoveAt(selectedIndex);
                    lbxFooterItems.SelectedIndex = selectedIndex;
                    break;
            }
        }
        #endregion

        #region Event
        private void fmReportExport_Load(object sender, EventArgs e)
        {
            if (TempReport.DataSourceCount == 0)
            {
                this.ShowMessage(MessageInfo.DataSourceNull, MsgMode.Error);
                this.Close();
            }
            else
            { 
                this.LoadTemplate();
                if (string.IsNullOrEmpty(selectedTemplateName))
                {
                    Init();
                }
            }
        }

        private void btPreview_Click(object sender, EventArgs e)
        {
            IReportExport excelReport = null;
            ExecutionResult execResult;
            execResult = new ExecutionResult();

            this.Cursor = Cursors.WaitCursor;

            this.SaveReportConfig();

            if (this.TempReport.Format.ExportFormat == ReportFormat.ExportType.Excel)
            {
                excelReport = new ExcelReportExporter(this.TempReport, ExportMode.Preview, false);
            }
            else
            {
                excelReport = new PdfReportExporter(this.TempReport, ExportMode.Preview, false);
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

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            IReportExport reportExport = null;
            ExecutionResult execResult = new ExecutionResult();

            this.SaveReportConfig();

            if (this.TempReport.Format.ExportFormat == ReportFormat.ExportType.Excel)
            {
                reportExport = new ExcelReportExporter(this.TempReport, ExportMode.Export, false);
            }
            else
            {
                reportExport = new PdfReportExporter(this.TempReport, ExportMode.Export, false);
            }

            execResult = reportExport.CheckValidate();

            if (execResult.Status)
            {
                fmProgress fp = new fmProgress("Easily Report (" + this.TempReport.Format.ExportFormat.ToString() + " Format)", reportExport, this.TempReport);
                fp.Visible = false;
                fp.ShowDialog();
            }
            else
            {
                MessageBox.Show(execResult.Message, TitleMsgInfo.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabControleRpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowStatusMsg();
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

            cbGroupGap.Items.Clear();
            foreach (object gGapType in Enum.GetValues(typeof(DataSourceItem.GroupGapType)))
            {
                if (((ReportFormat.ExportType)cbExportFormat.SelectedItem) == ReportFormat.ExportType.Excel)
                {
                    if ((DataSourceItem.GroupGapType)gGapType != DataSourceItem.GroupGapType.SingleLine && (DataSourceItem.GroupGapType)gGapType != DataSourceItem.GroupGapType.DoubleLine)
                    {
                        cbGroupGap.Items.Add(gGapType);
                    }
                }
                else
                {
                    if ((DataSourceItem.GroupGapType)gGapType != DataSourceItem.GroupGapType.Sheet)
                    {
                        cbGroupGap.Items.Add(gGapType);
                    }
                }
            }
            cbGroupGap.SelectedItem = DataSourceItem.GroupGapType.None;

            cbGroupType.Items.Clear();
            foreach (object item in Enum.GetValues(typeof(FieldItem.GroupType)))
            {
                if (((ReportFormat.ExportType)cbExportFormat.SelectedItem) != ReportFormat.ExportType.Excel)
                {
                    if (((FieldItem.GroupType)item) != FieldItem.GroupType.Excel)
                    {
                        cbGroupType.Items.Add(item);
                    }
                }
                else
                {
                    cbGroupType.Items.Add(item);
                }
            }
            cbGroupType.SelectedIndex = 0;

            tbOutputFileName.Text = MyStringConverter.GetFullFileName(tbOutputFileName.Text.Trim(), ((ReportFormat.ExportType)cbExportFormat.SelectedItem));
        }

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

        #endregion

      
 
    }
}