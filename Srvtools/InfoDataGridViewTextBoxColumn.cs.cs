using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Srvtools
{
    public class InfoDataGridViewTextBoxColumn: DataGridViewTextBoxColumn
    {
        public InfoDataGridViewTextBoxColumn()
            : base()
        { }

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }
    }
}
