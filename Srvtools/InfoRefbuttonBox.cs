using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Srvtools
{
    [ToolboxItem(false)]
    class InfoRefbuttonBox : TextBox
    {
        private String _RealValue = String.Empty;
        public String RealValue
        {
            set
            {
                _RealValue = value;
            }
            get
            {
                return _RealValue;
            }
        }
    }
}
