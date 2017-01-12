using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Text.RegularExpressions;

using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;

namespace Srvtools
{
    public class ServerModifyColumn : InfoOwnerCollectionItem, IGetValues
    {
        private string _columnName;

        [Editor(typeof(PropertyDropDownEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        [Browsable(false)]
        public override string Name
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public override string ToString()
        {
            return _columnName;
        }

        #region IGetValues

        public string[] GetValues(string sKind)
        {
            UpdateComponent updateComp = (UpdateComponent)Owner;
            // MessageBox.Show((updateComp.SelectCmd == null).ToString());

            InfoCommand myCmd = updateComp.SelectCmd;
            return myCmd.GetFields();
        }

        #endregion
    }
}
