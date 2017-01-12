
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Collections;
using System.Drawing;
using System.IO;

namespace Srvtools
{
    #region InfoDataGridViewRefValColumn
    public class InfoDataGridViewHyperLinkColumn : DataGridViewLinkColumn
    {
        public InfoDataGridViewHyperLinkColumn()
            : base()
        {
            base.CellTemplate = new InfoDataGridViewHyperLinkCell();
        }

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }

        [Category("Data")]
        public InfoHyperLink HyperLink
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewHyperLinkCell).HyperLink;
            }
            set
            {
                InfoDataGridViewHyperLinkCell template = this.CellTemplate as InfoDataGridViewHyperLinkCell;
                if (template != null)
                {
                    template.HyperLink = value;
                }
                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewHyperLinkCell cell =
                            row.Cells[base.Index] as InfoDataGridViewHyperLinkCell;
                        if (cell != null)
                        {
                            cell.HyperLink = value;
                        }
                    }
                }
            }
        }

      
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value == null || value is InfoDataGridViewHyperLinkCell)
                {
                    base.CellTemplate = value;
                }
                else
                {
                    throw new Exception("Must be a DataGridViewInfoRefValCell");
                }
            }
        }

        #region InfoDataGridViewRefValCell
        public class InfoDataGridViewHyperLinkCell : DataGridViewLinkCell
        {
            public InfoDataGridViewHyperLinkCell()
                : base()
            {
            }

            //protected override void Paint(System.Drawing.Graphics graphics,
            //                              System.Drawing.Rectangle clipBounds,
            //                              System.Drawing.Rectangle cellBounds,
            //                              int rowIndex,
            //                              DataGridViewElementStates cellState,
            //                              object value,
            //                              object formattedValue,
            //                              string errorText,
            //                              DataGridViewCellStyle cellStyle,
            //                              DataGridViewAdvancedBorderStyle advancedBorderStyle,
            //                              DataGridViewPaintParts paintParts)
            //{
            //    int rowCnt = -1;
            //    if (this.DataGridView.AllowUserToAddRows)
            //        rowCnt = this.DataGridView.Rows.Count - 1;
            //    else
            //        rowCnt = this.DataGridView.Rows.Count;

            //    if (rowIndex < rowCnt)
            //    {
            //        if (this.DataGridView.Site == null)
            //        {
            //            object[] obj = this.RefValue.CheckValid_And_ReturnDisplayValue(value.ToString(), false, false);
            //            if (obj[1] != null)
            //            {
            //                formattedValue = (string)obj[1];
            //            }
            //        }
            //    }
            //    base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            //    Rectangle rect = cellBounds;
            //    Point buttonLocation = new Point();
            //    buttonLocation.Y = cellBounds.Y;
            //    buttonLocation.X = cellBounds.X;
            //    Pen pen1 = new Pen(Brushes.White, 1);
            //    Pen pen2 = new Pen(Brushes.Black, 1);
            //    Pen pen3 = new Pen(Brushes.DimGray, 1);
            //    int i = this.DataGridView[this.ColumnIndex, rowIndex].Size.Width;

            //    SolidBrush myBrush = new SolidBrush(SystemColors.Control);
            //    graphics.FillRectangle(myBrush, buttonLocation.X + i - 21, buttonLocation.Y + 3, 18, 18);
            //    graphics.DrawLine(pen1, buttonLocation.X + i - 21, buttonLocation.Y + 3, buttonLocation.X + i - 3, buttonLocation.Y + 3); //top
            //    graphics.DrawLine(pen1, buttonLocation.X + i - 21, buttonLocation.Y + 3, buttonLocation.X + i - 21, buttonLocation.Y + 21); // left
            //    graphics.DrawLine(pen2, buttonLocation.X + i - 3, buttonLocation.Y + 21, buttonLocation.X + i - 21, buttonLocation.Y + 21); // Bottom
            //    graphics.DrawLine(pen3, buttonLocation.X + i - 4, buttonLocation.Y + 20, buttonLocation.X + i - 20, buttonLocation.Y + 20);
            //    graphics.DrawLine(pen2, buttonLocation.X + i - 3, buttonLocation.Y + 20, buttonLocation.X + i - 3, buttonLocation.Y + 3); // right
            //    graphics.DrawLine(pen3, buttonLocation.X + i - 4, buttonLocation.Y + 21, buttonLocation.X + i - 4, buttonLocation.Y + 4);
            //    StringFormat sf = new StringFormat();
            //    sf.Alignment = StringAlignment.Center;
            //    sf.LineAlignment = StringAlignment.Center;
            //    graphics.DrawString("...", new Font("SimSun", 9), Brushes.Black, buttonLocation.X + i - 10, buttonLocation.Y + 10, sf);

            //}

            //bool bShowRefance = false;
            protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
            {
                base.OnMouseDown(e);
            }

            protected override void OnContentClick(DataGridViewCellEventArgs e)
            {
                base.OnContentClick(e);

                ArrayList text = new ArrayList();
                int rowindex = e.RowIndex;
                foreach (LinkColumn lc in HyperLink.SourceColumns)
                {
                    int columnindex = -1;
                    for (int i = 0; i < this.DataGridView.Columns.Count; i++)
                    {
                        if (this.DataGridView.Columns[i].DataPropertyName == lc.ColumnName)
                        {
                            columnindex = i;
                            break;
                        }
                    }
                    if (columnindex != -1)
                    {
                        text.Add(this.DataGridView.Rows[rowindex].Cells[columnindex].Value.ToString());
                    }
                }

                HyperLink.ColumnText = text;
                HyperLink.DoClick();
                
            }

            //public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            //{
            //    base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            //    if (bShowRefance)
            //    {
            //        SendKeys.Send("{F4}");
            //        bShowRefance = false;
            //    }
            //}

            private InfoHyperLink m_hyperlink = null;
            public InfoHyperLink HyperLink
            {
                get
                {
                    return m_hyperlink;
                }
                set
                {
                    m_hyperlink = value;
                }
            }

            //public override Type EditType
            //{
            //    get
            //    {
            //        return typeof(InfoDataGridViewRefValCellEditingControl);
            //    }
            //}

            public override object Clone()
            {
                InfoDataGridViewHyperLinkCell cell = base.Clone() as InfoDataGridViewHyperLinkCell;
                cell.HyperLink = this.HyperLink;
                return cell;
            }
        }

        #endregion
      
    }
    #endregion
}