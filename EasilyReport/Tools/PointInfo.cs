using System;
using System.Collections.Generic;
using System.Text;

namespace Infolight.EasilyReportTools.Tools
{
    internal class PointInfo
    {
        public PointInfo()
        {
            Reset();
            this.sheetWidth = 0;
        }

        public PointInfo(int rowindex, int columnindex)
        {
            rowIndex = rowindex;
            columnIndex = columnindex;
        }

        private int sheetWidth;
        /// <summary>
        /// excel sheet width (cell count, n times as great as three)
        /// </summary>
        public int SheetWidth
        {
            get { return sheetWidth; }
            set { sheetWidth = value; }
        }

        private int rowIndex;
        /// <summary>
        /// row index
        /// </summary>
        public int RowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }

        private int columnIndex;
        /// <summary>
        /// column index
        /// </summary>
        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        /// <summary>
        /// rowIndex and columnIndex reset to 0
        /// </summary>
        public void Reset()
        {
            this.rowIndex = 0;
            this.columnIndex = 0;
        }

        /// <summary>
        /// rowIndex reset to 0
        /// </summary>
        public void ResetRowIndex()
        {
            this.rowIndex = 0;
        }

        /// <summary>
        /// columnIndex reset to 0
        /// </summary>
        public void ResetColumnIndex()
        {
            this.columnIndex = 0;
        }

        /// <summary>
        /// Tab
        /// </summary>
        public void Tab()
        {
            switch (this.SheetWidth % 3)
            {
                case 0:
                    this.ColumnIndex += this.SheetWidth / 3;
                    break;

                case 1:
                    if (this.ColumnIndex == 0 || this.ColumnIndex == ((this.SheetWidth / 3) * 2 + 1))
                    {
                        this.ColumnIndex += this.SheetWidth / 3;
                    }
                    else if (this.ColumnIndex == this.SheetWidth / 3)
                    {
                        this.ColumnIndex += this.SheetWidth / 3 + 1;
                    }
                    break;

                case 2:
                    if (this.ColumnIndex == 0 || this.ColumnIndex == ((this.SheetWidth / 3) * 2 + 1))
                    {
                        this.ColumnIndex += this.SheetWidth / 3 + 1;
                    }
                    else if (this.ColumnIndex == this.SheetWidth / 3)
                    {
                        this.ColumnIndex += this.SheetWidth / 3;
                    }
                    break;
            }
        }

        /// <summary>
        /// Next row (column index reset to 0)
        /// </summary>
        public void Enter()
        {
            this.rowIndex += 1;
            ResetColumnIndex();
        }

        /// <summary>
        /// Next column
        /// </summary>
        public void NextColumn()
        {
            this.columnIndex += 1;
        }

        /// <summary>
        /// Previous column
        /// </summary>
        public void PreviousColumn()
        {
            this.columnIndex += -1;
        }

        /// <summary>
        /// Next row
        /// </summary>
        public void NextRow()
        {
            this.rowIndex += 1;
        }

        /// <summary>
        /// Move down row
        /// </summary>
        public void MoveDownRow(int count)
        {
            this.rowIndex += count;
        }

        /// <summary>
        /// Move up row
        /// </summary>
        public void MoveUpRow(int count)
        {
            this.rowIndex -= count;
        }

        /// <summary>
        /// Move left column
        /// </summary>
        /// <param name="count"></param>
        public void MoveLeftColumn(int count)
        {
            this.columnIndex -= count;
        }

        /// <summary>
        /// Move right column
        /// </summary>
        /// <param name="count"></param>
        public void MoveRightColumn(int count)
        {
            this.columnIndex += count;
        }

        /// <summary>
        /// Previous row
        /// </summary>
        public void PreviousRow()
        {
            this.rowIndex += -1;
        }

        public void EndArea()
        {
            switch (this.SheetWidth % 3)
            {
                case 0:
                    this.ColumnIndex += this.SheetWidth / 3 - 1;
                    break;

                case 1:
                    if (this.ColumnIndex == 0 || this.ColumnIndex == ((this.SheetWidth / 3) * 2 + 1))
                    {
                        this.ColumnIndex += this.SheetWidth / 3 - 1;
                    }
                    else if (this.ColumnIndex == this.SheetWidth / 3)
                    {
                        this.ColumnIndex += this.SheetWidth / 3;
                    }
                    break;

                case 2:
                    if (this.ColumnIndex == 0 || this.ColumnIndex == ((this.SheetWidth / 3) * 2 + 1))
                    {
                        this.ColumnIndex += this.SheetWidth / 3;
                    }
                    else if (this.ColumnIndex == this.SheetWidth / 3)
                    {
                        this.ColumnIndex += this.SheetWidth / 3 - 1;
                    }
                    break;
            }
        }

        public void EndHalfPageWidth()
        {
            switch (this.SheetWidth % 2)
            {
                case 0:
                    this.ColumnIndex += this.SheetWidth / 2 - 1;
                    break;

                case 1:
                    this.ColumnIndex += this.SheetWidth / 2 - 1;
                    break;
            }
        }

        public void NextHalfPage()
        {
            switch (this.SheetWidth % 2)
            {
                case 0:
                    this.ColumnIndex += this.SheetWidth / 2;
                    break;

                case 1:
                    this.ColumnIndex += this.SheetWidth / 2;
                    break;
            }
        }

        public void EndPageWidth()
        {
            this.ColumnIndex = this.SheetWidth - 1;
        }

        public void LastColumn()
        {
            if (this.ColumnIndex < this.SheetWidth - 1)
            {
                this.ColumnIndex = this.SheetWidth - 1;
            }
        }
    }
}
