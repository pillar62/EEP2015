using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;

namespace Srvtools
{
    #region InfoDataGridViewExpressionColumn
    public class InfoDataGridViewExpressionColumn : DataGridViewTextBoxColumn, ISupportInitialize
    {

        public InfoDataGridViewExpressionColumn()
            : base()
        {
            base.CellTemplate = new InfoDataGridViewExpressionCell();
        }

        public void BeginInit()
        {
        }

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }

        public void EndInit()
        {
            if (DataGridView != null)
            {
                ISite site = this.DataGridView.Site;
                if ((site == null) || (!site.DesignMode))
                {
                    InfoDataSet objDataSource = (InfoDataSet)((InfoDataGridView)this.DataGridView).GetDataSource();

                    string strDataMember = ((InfoDataGridView)this.DataGridView).GetDataMember();
                    if (_Expression != "")
                    {
                        if (ExpressionColumn == null)
                        {
                            ExpressionColumn = new DataColumn();
                            ExpressionColumn.ColumnName = _Expression;
                            ExpressionColumn.Expression = _Expression;

                            bool bDetail = false;
                            int relationPos = -1;
                            int i = objDataSource.RealDataSet.Relations.Count;
                            if (i > 0)
                            {
                                for (int j = 0; j < i; j++)
                                {
                                    if (strDataMember == objDataSource.RealDataSet.Relations[j].RelationName
                                        || strDataMember.Substring(strDataMember.IndexOf('.') + 1) == objDataSource.RealDataSet.Relations[j].RelationName)
                                    {
                                        bDetail = true;
                                        relationPos = j;
                                        break;
                                    }
                                }
                                if (bDetail)
                                {
                                    if (!(objDataSource.RealDataSet.Relations[relationPos].ChildTable.Columns.Contains(_Expression)))
                                        objDataSource.RealDataSet.Relations[relationPos].ChildTable.Columns.Add(ExpressionColumn);
                                    else
                                    {
                                        ExpressionColumn = objDataSource.RealDataSet.Relations[relationPos].ChildTable.Columns[_Expression];
                                    }
                                }
                                else if (!(objDataSource.RealDataSet.Tables[strDataMember].Columns.Contains(_Expression)))
                                    objDataSource.RealDataSet.Tables[strDataMember].Columns.Add(ExpressionColumn);
                                else
                                {
                                    ExpressionColumn = objDataSource.RealDataSet.Tables[strDataMember].Columns[_Expression];
                                }
                            }
                            else if (!(objDataSource.RealDataSet.Tables[strDataMember].Columns.Contains(_Expression)))
                                objDataSource.RealDataSet.Tables[strDataMember].Columns.Add(ExpressionColumn);
                            else
                            {
                                ExpressionColumn = objDataSource.RealDataSet.Tables[strDataMember].Columns[_Expression];
                            }
                        }
                    }
                    else
                    {
                        if (ExpressionColumn != null)
                        {
                            string columnName = ExpressionColumn.ColumnName;
                            if (objDataSource.RealDataSet.Relations.Count > 0
                                && objDataSource.RealDataSet.Relations.Contains(strDataMember))
                            {
                                if (objDataSource.RealDataSet.Relations[strDataMember].ChildTable.Columns.Contains(ExpressionColumn.ColumnName))
                                {
                                    ExpressionColumn = null;
                                    objDataSource.RealDataSet.Relations[strDataMember].ChildTable.Columns.Remove(columnName);

                                    return;
                                }
                            }
                            if (objDataSource.RealDataSet.Tables[strDataMember].Columns.Contains(ExpressionColumn.ColumnName))
                            {

                                ExpressionColumn = null;
                                objDataSource.RealDataSet.Tables[strDataMember].Columns.Remove(columnName);
                            }
                        }
                    }
                }
            }
        }

        [Category("InfoLight")]
        private string _Expression = "";
        public string Expression
        {
            get
            {
                return _Expression;
            }
            set
            {
                _Expression = value;
            }
        }

        [Category("InfoLight")]
        [Description("各字段用分号隔开")]
        private string _EffectColumnNames;
        public string EffectColumnNames
        {
            get
            {
                return _EffectColumnNames;
            }
            set
            {
                _EffectColumnNames = value;
            }
        }

        public List<string> GetEffectColumnList()
        {
            List<string> columns = new List<string>();
            string effectColumnNames = this.EffectColumnNames.Trim();
            string[] list = effectColumnNames.Split(';');
            int i = list.Length;
            for (int j = 0; j < i; j++)
            {
                columns.Add(list[j].Trim());
            }
            return columns;
        }

        [Browsable(false)]
        public override bool ReadOnly
        {
            get
            {
                return true;
            }
        }

        [Browsable(false)]
        public new string DataPropertyName
        {
            get { return base.DataPropertyName; }
            set 
            {
                base.DataPropertyName = string.Empty;
            }
        }

        public override object Clone()
        {
            InfoDataGridViewExpressionColumn column = base.Clone() as InfoDataGridViewExpressionColumn;
            column._Expression = this._Expression;
            column._EffectColumnNames = this._EffectColumnNames;
            return column;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value == null || value is InfoDataGridViewExpressionCell)
                {
                    base.CellTemplate = value;
                }
                else
                {
                    throw new Exception("Must be a InfoDataGridViewExpressionCell");
                }
            }
        }

        #region GetExpression(int RowPos)
        //private int GetRowPosInDataSet(int RowPosInDataGridView)
        //{
        //    int RowPosInDataSet = -1;
        //    InfoDataSet objDataSource = (InfoDataSet)((InfoDataGridView)this.DataGridView).GetDataSource();
        //    string strDataMember = ((InfoDataGridView)this.DataGridView).GetDataMember();

        //    bool bDetail = false;
        //    int relationPos = -1;
        //    int i = objDataSource.RealDataSet.Relations.Count;
        //    DataRow dr = ((DataRowView)this.DataGridView.Rows[RowPosInDataGridView].DataBoundItem).Row;
        //    if (i > 0) // 是否存在relation
        //    {
        //        for (int j = 0; j < i; j++)
        //        {
        //            if (strDataMember == objDataSource.RealDataSet.Relations[j].RelationName
        //                || strDataMember.Substring(strDataMember.IndexOf('.') + 1) == objDataSource.RealDataSet.Relations[j].RelationName)
        //            // 当前datasource是否为relation
        //            {
        //                bDetail = true;
        //                relationPos = j;
        //                break;
        //            }
        //        }
        //        if (bDetail)
        //        {
        //            // 当前datasource是relation
        //            RowPosInDataSet = objDataSource.RealDataSet.Relations[relationPos].ChildTable.Rows.IndexOf(dr);
        //        }
        //        else
        //        {
        //            RowPosInDataSet = objDataSource.RealDataSet.Tables[strDataMember].Rows.IndexOf(dr);
        //        }
        //    }
        //    else
        //    {
        //        RowPosInDataSet = objDataSource.RealDataSet.Tables[strDataMember].Rows.IndexOf(dr);
        //    }
        //    return RowPosInDataSet;
        //}

        private DataColumn ExpressionColumn = null;
        public string GetExpression(int RowPos)
        {
            //InfoDataSet objDataSource = (InfoDataSet)((InfoDataGridView)this.DataGridView).GetDataSource();
            //string strDataMember = ((InfoDataGridView)this.DataGridView).GetDataMember();
            //string strExpValue = string.Empty;

            if (ExpressionColumn != null)
            {
                DataRow dr = (this.DataGridView.Rows[RowPos].DataBoundItem as DataRowView).Row;
                object value = dr[ExpressionColumn];
                if (!string.IsNullOrEmpty(this.DefaultCellStyle.Format))
                {
                    try
                    {
                        double number = Convert.ToDouble(value);
                        return number.ToString(this.DefaultCellStyle.Format);
                    }
                    catch
                    {
                        return value.ToString();
                    }
                }
                else
                {
                    return value.ToString();
                }
            }
            return string.Empty;
            //    bool bDetail = false;
            //    int relationPos = -1;
            //    int i = objDataSource.RealDataSet.Relations.Count;
            //    if (i > 0)
            //    {
            //        for (int j = 0; j < i; j++)
            //        {
            //            if (strDataMember == objDataSource.RealDataSet.Relations[j].RelationName
            //                || strDataMember.Substring(strDataMember.IndexOf('.') + 1) == objDataSource.RealDataSet.Relations[j].RelationName)
            //            {
            //                bDetail = true;
            //                relationPos = j;
            //                break;
            //            }
            //        }
            //        if (bDetail)
            //        {
            //            if (objDataSource.RealDataSet.Relations[relationPos].ChildTable.Columns.Contains(ExpressionColumn.ColumnName))
            //            {
            //                if (RowPos < objDataSource.RealDataSet.Relations[relationPos].ChildTable.Rows.Count && GetRowPosInDataSet(RowPos) != -1)
            //                    strExpValue = objDataSource.RealDataSet.Relations[relationPos].ChildTable.Rows[GetRowPosInDataSet(RowPos)][ExpressionColumn].ToString();
            //                else
            //                    strExpValue = "";
            //                return strExpValue;
            //            }
            //            else
            //                return "null";
            //        }
            //        else if (objDataSource.RealDataSet.Tables[strDataMember].Columns.Contains(ExpressionColumn.ColumnName))
            //        {
            //            if (RowPos < objDataSource.RealDataSet.Tables[strDataMember].Rows.Count && GetRowPosInDataSet(RowPos) != -1)
            //                strExpValue = objDataSource.RealDataSet.Tables[strDataMember].Rows[GetRowPosInDataSet(RowPos)][ExpressionColumn].ToString();
            //            else
            //                strExpValue = "";
            //            return strExpValue;
            //        }
            //        else
            //            return "null";
            //    }
            //    else if (objDataSource.RealDataSet.Tables[strDataMember].Columns.Contains(ExpressionColumn.ColumnName))
            //    {
            //        if (RowPos < objDataSource.RealDataSet.Tables[strDataMember].Rows.Count && GetRowPosInDataSet(RowPos) != -1)
            //            strExpValue = objDataSource.RealDataSet.Tables[strDataMember].Rows[GetRowPosInDataSet(RowPos)][ExpressionColumn].ToString();
            //        else
            //            strExpValue = "";
            //        return strExpValue;
            //    }
            //    else
            //        return "null";
            //}
            //else
            //    return "null";
        }
        #endregion

        #region InfoDataGridViewExpressionCell
        public class InfoDataGridViewExpressionCell : DataGridViewTextBoxCell
        {
            public InfoDataGridViewExpressionCell()
                : base()
            {
            }

            protected override void Paint(Graphics graphics,
                Rectangle clipBounds,
                Rectangle cellBounds,
                int rowIndex,
                DataGridViewElementStates cellState,
                object value,
                object formattedValue,
                string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                int rowCnt = -1;
                if (this.DataGridView.AllowUserToAddRows)
                    rowCnt = this.DataGridView.Rows.Count - 1;
                else
                    rowCnt = this.DataGridView.Rows.Count;
                if (rowIndex < rowCnt)
                {
                    if (this.DataGridView == null)
                        formattedValue = "";
                    else
                    {
                        ISite site = this.DataGridView.Site;
                        if ((site != null))
                        {
                            formattedValue = "<CalField>";
                        }
                        //2006/05/25 为解GridView的Columns超出右邊界, TotalChange會有問題
                        //else
                        //{
                        //    DataRowView drv = (DataRowView)this.DataGridView.Rows[rowIndex].DataBoundItem;
                        //    if (!drv.IsNew)
                        //    {
                        //        DataRow dr = drv.Row;
                        //        if (((InfoDataGridViewExpressionColumn)this.OwningColumn).Expression == null)
                        //        {
                        //            //value = "";
                        //            formattedValue = "";
                        //        }
                        //        else
                        //        {
                        //            //value = ((InfoDataGridViewExpressionColumn)this.OwningColumn).GetExpression(rowIndex);
                        //            formattedValue = ((InfoDataGridViewExpressionColumn)this.OwningColumn).GetExpression(rowIndex);
                        //            try
                        //            {
                        //                double d = Convert.ToDouble(formattedValue);
                        //                string format = this.OwningColumn.DefaultCellStyle.Format;
                        //                formattedValue = d.ToString(format);
                        //            }
                        //            catch
                        //            {

                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        //value = "";
                        //        formattedValue = "";
                        //    }
                        //    //double d = new double();
                        //    //try
                        //    //{
                        //    //    if (value != null && value.ToString() != "")
                        //    //    {
                        //    //        d = Convert.ToDouble(value);
                        //    //    }
                        //    //}
                        //    //catch (Exception ex)
                        //    //{
                        //    //    MessageBox.Show(ex.Message);
                        //    //}
                        //    //this.SetValue(rowIndex, d);
                        //}
                    }
                }
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            }
        }
        #endregion
    }
    #endregion
}
