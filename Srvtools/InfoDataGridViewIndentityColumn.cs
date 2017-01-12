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
    public class InfoDataGridViewIndentityColumn : DataGridViewTextBoxColumn
    {
        public InfoDataGridViewIndentityColumn()
            : base()
        {
            base.CellTemplate = new InfoDataGridViewIndentityCell();
        }

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value == null || value is InfoDataGridViewIndentityCell)
                {
                    base.CellTemplate = value;
                }
                else
                {
                    throw new Exception("Must be a InfoDataGridViewIndentityCell");
                }
            }
        }

        [Browsable(false)]
        public override bool ReadOnly
        {
            get
            {
                return true;
            }
        }

        public class InfoDataGridViewIndentityCell : DataGridViewTextBoxCell
        {
            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, 
                int rowIndex,DataGridViewElementStates cellState, object value, object formattedValue, 
                string errorText,DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, 
                DataGridViewPaintParts paintParts)
            {
                int rowCnt = -1;
                if (this.DataGridView.AllowUserToAddRows)
                    rowCnt = this.DataGridView.Rows.Count - 1;
                else
                    rowCnt = this.DataGridView.Rows.Count;
                if (rowIndex < rowCnt)
                {
                    formattedValue = (rowIndex + 1).ToString();
                }
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}
