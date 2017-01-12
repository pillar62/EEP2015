using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Srvtools
{
    public class IndentityField : DataControlField
    {
        private WebGridView gridView = null;

        public IndentityField()
        { 
        }

        protected override DataControlField CreateField()
        {
            return new IndentityField();
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cellType == DataControlCellType.DataCell)
            {
                Label lbl = new Label();
                int dataIndex = rowIndex + 1;
                if (gridView != null)
                {
                    dataIndex += gridView.PageIndex * gridView.PageSize;
                }
                lbl.Text = dataIndex.ToString();
                cell.Controls.Add(lbl);
            }
        }

        public override bool Initialize(bool sortingEnabled, Control control)
        {
            gridView = (WebGridView)control;
            return base.Initialize(sortingEnabled, control);
        }
    }
}

