using System;
using System.ComponentModel;
using Srvtools;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ChartTools
{
    public class ChartField : InfoOwnerCollectionItem, IGetValues, IChartField
    {
        public ChartField()
        { 
        }

        public ChartField(string fieldName, string fieldCaption)
        {
            this._fieldName = fieldName;
            this._fieldCaption = fieldCaption;
        }

        public override string Name
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private string _fieldName;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
                if (this.Owner != null && ((ChartBase)this.Owner).Site.DesignMode && _fieldName != null && _fieldName != "")
                {
                    string caption = GetCaption(_fieldName);
                    if (caption != "")
                    {
                        FeildCaption = caption;
                    }
                    else
                    {
                        FeildCaption = _fieldName;
                    }
                }
            }
        }

        private string _fieldCaption;
        [NotifyParentProperty(true)]
        public string FeildCaption
        {
            get 
            {
                return _fieldCaption; 
            }
            set 
            { 
                _fieldCaption = value;
                //if (!string.IsNullOrEmpty(_fieldCaption))
                //{
                //    CaptionFieldName = "";
                //}
            }
        }

        private string _captionFieldName;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string CaptionFieldName
        {
            get 
            {
                return _captionFieldName; 
            }
            set 
            {
                _captionFieldName = value;
                //if (!string.IsNullOrEmpty(_captionFieldName))
                //{
                //    FeildCaption = "";
                //}
            }
        }

        public override string ToString()
        {
            return _fieldName;
        }

        private string GetCaption(string fieldName)
        {
            DataSet ds = ((ChartFieldsCollection)this.Collection).DsForDD;
            string strTableName = ((ChartBase)this.Owner).BindingSource.DataMember;
            if (ds.Tables.Count == 0 || ds.Tables[strTableName] == null)
            {
                ((ChartFieldsCollection)this.Collection).DsForDD = this.GetDD(strTableName);
                ds = ((ChartFieldsCollection)this.Collection).DsForDD;
            }
            string caption = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[strTableName].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[strTableName].Rows[j]["FIELD_NAME"].ToString(), fieldName, true) == 0)//IgnoreCase
                    {
                        caption = ds.Tables[strTableName].Rows[j]["CAPTION"].ToString();
                        break;
                    }
                }
            }
            return caption;
        }

        private DataSet GetDD(string strTableName)
        {
            string strModuleName = ((InfoDataSet)((ChartBase)this.Owner).BindingSource.GetDataSource()).RemoteName;
            strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
            DataSet dsDD = new DataSet();
            string sCurProject = EditionDifference.ActiveSolutionName();
            string tabName = CliUtils.GetTableName(strModuleName, strTableName, sCurProject, "", true);
            string strSql = "select * from COLDEF where TABLE_NAME = '" + tabName + "'";
            return CliUtils.ExecuteSql(strModuleName, strTableName, strSql, true, sCurProject);
        }


        #region IGetValues Members
        public string[] GetValues(string sKind)
        {
            List<string> values = new List<string>();
            if (this.Owner is ChartBase)
            {
                if (string.Compare(sKind, "fieldname", true) == 0 || string.Compare(sKind, "captionfieldname", true) == 0)
                {
                    DataTable dt = ((ChartBase)this.Owner).GetDt();
                    foreach (DataColumn column in dt.Columns)
                    {
                        values.Add(column.ColumnName);
                    }
                }
            }
            return values.ToArray();
        }
        #endregion
    }
}
