using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;

namespace Srvtools
{
    public class RefColumns : InfoOwnerCollectionItem, IGetValues
    {
        public RefColumns()
            : this("", "", 100, new DataGridViewCellStyle(), true)
        {
        }

        public RefColumns(string column, 
                         string headerText, 
                         int width, 
                         DataGridViewCellStyle defaultCellStyle,
                         bool visible)
        {
            _Column = column;
            _HeaderText = headerText;
            _Width = width;
            _Visible = visible;
            _DefaultCellStyle = defaultCellStyle;
        }

        public override string Name
        {
            get { return _HeaderText; }
            set { _HeaderText = value; }
        }

        public override string ToString()
        {
            return _HeaderText;
        }

        private string _Column;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Column
        {
            get
            {
                return _Column;
            }
            set
            {
                _Column = value;
                if (this.Owner != null && ((Component)this.Owner).Site.DesignMode)
                {
                    string header = GetHeaderText(_Column);
                    if (header != "")
                    {
                        HeaderText = header;
                    }
                    else
                    {
                        HeaderText = _Column;
                    }
                }
            }
        }

        private string _HeaderText;
        public string HeaderText
        {
            get
            {
                return _HeaderText;
            }
            set
            {
                _HeaderText = value;
            }
        }

        private int _Width;
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        private bool _Visible = true;
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        private bool isNvarChar;
        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }

        private DataGridViewCellStyle _DefaultCellStyle;
        public DataGridViewCellStyle DefaultCellStyle
        {
            get
            {
                return _DefaultCellStyle;
            }
            set
            {
                _DefaultCellStyle = value;
            }
        }

        private string GetHeaderText(string ColName)
        {
            DataSet ds = ((RefColumnsCollection)this.Collection).DsForDD;
            if (ds.Tables.Count == 0) 
            {
                if (this.Owner is InfoRefVal)
                {
                    if (((InfoRefVal)this.Owner).DataSource is InfoBindingSource)
                    {
                        ((RefColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoRefVal)this.Owner).DataSource as InfoBindingSource, true);
                    }
                    else if (((InfoRefVal)this.Owner).DataSource is InfoDataSet)
                    {
                        ((RefColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoRefVal)this.Owner).DataSource as InfoDataSet, null, true);
                    }
                }
                else if(this.Owner is InfoRefButton)
                {
                    if (((InfoRefButton)this.Owner).infoTranslate != null && ((InfoRefButton)this.Owner).infoTranslate.BindingSource != null)
                    {
                        ((RefColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoRefButton)this.Owner).infoTranslate.BindingSource, true);
                    }
                }
                ds = ((RefColumnsCollection)this.Collection).DsForDD;
            }
            string strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), ColName, true) == 0)//IgnoreCase
                    {
                        strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        /*private IDbConnection AllocateConnection(string ConnectionType)
        {
            IDbConnection con = null;
            switch (ConnectionType)
            {
                case "1":
                    con = new SqlConnection(((InfoRefVal)this.Owner).SelectAlias);
                    break;
                case "2":
                    con = new OleDbConnection(((InfoRefVal)this.Owner).SelectAlias);
                    break;
                case "3":
                    con = new OracleConnection(((InfoRefVal)this.Owner).SelectAlias);
                    break;
                case "4":
                    con = new OdbcConnection(((InfoRefVal)this.Owner).SelectAlias);
                    break;
            }
            return con;
        }

        private IDbDataAdapter AllocateDataAdapter(string ConnectionType)
        {
            IDbDataAdapter da = null;
            switch (ConnectionType)
            {
                case "1":
                    da = new SqlDataAdapter();
                    break;
                case "2":
                    da = new OleDbDataAdapter();
                    break;
                case "3":
                    da = new OracleDataAdapter();
                    break;
                case "4":
                    da = new OdbcDataAdapter();
                    break;
            }
            return da;
        }*/

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retItems = null;
            if (string.Compare(sKind, "column", true) == 0)//IgnoreCase
            {
                if (this.Owner is InfoRefVal)
                {
                    if (((InfoRefVal)this.Owner).SelectCommand == null || ((InfoRefVal)this.Owner).SelectCommand == "")
                    {
                        if (((InfoRefVal)this.Owner).GetDataSource() != null)
                        {
                            InfoDataSet objDataSource = (InfoDataSet)((InfoRefVal)this.Owner).GetDataSource();
                            string strTabName = ((InfoRefVal)this.Owner).GetDataMember();
                            int i = objDataSource.RealDataSet.Tables[strTabName].Columns.Count;
                            string[] arrItems = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                arrItems[j] = objDataSource.GetRealDataSet().Tables[strTabName].Columns[j].ColumnName;
                            }
                            retItems = arrItems;
                        }
                    }
                    else
                    {
                        if (((InfoRefVal)this.Owner).InnerBs != null && ((InfoRefVal)this.Owner).InnerBs.DataSource != null
                            && ((InfoDataSet)((InfoRefVal)this.Owner).InnerBs.DataSource).RealDataSet.Tables.Count > 0)
                        {
                            DataColumnCollection dcc = ((InfoDataSet)((InfoRefVal)this.Owner).InnerBs.DataSource).RealDataSet.Tables[0].Columns;
                            int i = dcc.Count;
                            string[] arrItems = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                arrItems[j] = dcc[j].ColumnName;
                            }
                            retItems = arrItems;
                        }
                    }
                }
                else if(this.Owner is InfoRefButton)
                {
                    if (((InfoRefButton)this.Owner).infoTranslate != null && ((InfoRefButton)this.Owner).infoTranslate.BindingSource != null)
                    {
                        InfoBindingSource bindingSource = ((InfoRefButton)this.Owner).infoTranslate.BindingSource;
                        InfoDataSet ds = bindingSource.GetDataSource();
                        string datamember = bindingSource.GetTableName();
                        retItems = new string[ds.RealDataSet.Tables[datamember].Columns.Count];
                        for (int i = 0; i < retItems.Length; i++)
                        {
                            retItems[i] = ds.RealDataSet.Tables[datamember].Columns[i].ColumnName;
                        }
                    }
                }
            }
            return retItems;
        }
        #endregion
    }
}
