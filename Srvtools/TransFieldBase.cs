using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public class TransFieldBase : InfoOwnerCollectionItem , IGetValues
    {
        #region Constructor

        public TransFieldBase() : this("")
        {
            
        }

        public TransFieldBase(String desField)
        {
            _desField = desField;
        }

        #endregion

        #region Propeties

        [Category("Data"), Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String DesField
        {
            set { _desField = value; }
            get { return _desField; }
        }

        [Category("Data"), Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String SrcField
        {
            set { _srcField = value; }
            get { return _srcField; }
        }

        [Category("Data")]
        public String SrcGetValue
        {
            set { _srcGetValue = value; }
            get { return _srcGetValue; }
        }

        [Browsable(false)]
        public String FieldType
        {
            set { _fieldType = value; }
            get { return _fieldType; }
        }

        [Browsable(false)]
        public String FieldTypeName
        {
            set { _fieldTypeName = value; }
            get { return _fieldTypeName; }
        }

        [Browsable(false)]
        public Boolean ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        [Browsable(false)]
        public Object SrcValue
        {
            set { _srcValue = value; }
            get { return _srcValue; }
        }

        [Browsable(false)]
        public Object DesValue
        {
            set { _desValue = value; }
            get { return _desValue; }
        }

        [Browsable(false)]
        public override string Name
        {
            get { return _desField; }
            set { _desField = value; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return _desField; ;
        }

        public string[] GetValues(string sKind)
        {
            Transaction trans = (Transaction)Owner;
            InfoTransaction infoTrans = (InfoTransaction)trans.Owner;
            if (infoTrans.UpdateComp == null)
            { throw new Exception("The 'UpdateComp' property is null."); }

            if (infoTrans.UpdateComp.SelectCmd == null)
            { throw new Exception("The 'UpdateComp.SelectCmd' property is null."); }
            if (string.Compare(sKind, "SrcField", true) == 0)
            {
                return infoTrans.UpdateComp.SelectCmd.GetFields();
            }
            else if (string.Compare(sKind, "DesField", true) == 0)
            {
                if (!string.IsNullOrEmpty(trans.TransTableName))
                {
                    IDbConnection conn = infoTrans.UpdateComp.SelectCmd.InfoConnection;
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = string.Format("Select * from {0}"
                            , DBUtils.QuoteWords(trans.TransTableName, DBUtils.GetDatabaseType(conn)));
                        IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.FillSchema(ds, SchemaType.Mapped);
                        List<string> list = new List<string>();
                        foreach (DataColumn column in ds.Tables[0].Columns)
                        {
                            list.Add(column.ColumnName);
                        }
                        return list.ToArray();
                    }
                }
            }
            return new string[] { };
        }

        #endregion

        #region Vars

        private String _desField;
        private Object _desValue;

        private String _srcField;
        private Object _srcValue;

        private String _srcGetValue;
        private String _fieldType;
        private Boolean _readOnly;
        private String _fieldTypeName;

        #endregion
    }
}
