using System;
using System.Collections.Generic;
using System.Text;

namespace Infolight.EasilyReportTools.Config
{
    internal class CellCssClass
    {
        internal CellCssClass(int rowIndex, int columnIndex, string cssClass)
        {
            _rowIndex = rowIndex;
            _colIndex = columnIndex;
            _cssClass = cssClass;
        }

        int _rowIndex = -1;
        int _colIndex = -1;
        string _cssClass = "";

        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        public int ColumnIndex
        {
            get { return _colIndex; }
            set { _colIndex = value; }
        }

        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }
    }
}
