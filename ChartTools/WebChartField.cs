using System;
using System.ComponentModel;
using Srvtools;
using System.Data;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Web.UI;
using System.Drawing.Design;

namespace ChartTools
{
    public class WebChartField : InfoOwnerCollectionItem, IGetValues, IChartField
    {
        public WebChartField()
        { }

        public WebChartField(string fieldName, string fieldCaption)
        {
            this._fieldName = fieldName;
            this._fieldCaption = fieldCaption;
        }

        public override string Name
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private string _fieldName = "";
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string FieldName
        {
            get 
            { 
                return _fieldName; 
            }
            set 
            {
                _fieldName = value; 
                if (this.Owner != null && ((WebChartBase)this.Owner).Site.DesignMode && _fieldName != null && _fieldName != "")
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

        private string _fieldCaption = "";
        [NotifyParentProperty(true)]
        [DefaultValue("")]
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

        private string _captionFieldName = "";
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
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
            string strCaption = "";
            DataSet ds = ((WebChartFieldsCollection)this.Collection).DsForDD;
            WebChartBase chart = (WebChartBase)this.Owner;
            string strTableName = ((WebDataSource)chart.GetObjByID(chart.DataSourceID)).DataMember;
            if (ds.Tables.Count == 0)
            {
                ((WebChartFieldsCollection)this.Collection).DsForDD = this.GetDD(strTableName);
                ds = ((WebChartFieldsCollection)this.Collection).DsForDD;
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == fieldName.ToLower())
                    {
                        strCaption = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                        break;
                    }
                }
            }
            return strCaption;
        }

        private DataSet GetDD(string strTableName)
        {
            DataSet dsDD = new DataSet();
            string sCurProject = EditionDifference.ActiveSolutionName();
            string tabName = "";
            WebChartBase chart = (WebChartBase)this.Owner;
            WebDataSource wds = (WebDataSource)chart.GetObjByID(chart.DataSourceID);
            ///////////////////////////////////////////////////////////////////////////////////////////
            if (wds.SelectAlias != null && wds.SelectAlias != "" && wds.SelectCommand != null && wds.SelectCommand != "")
            {
                tabName = CliUtils.GetTableName(wds.SelectCommand, true);
                string strSql = "select * from COLDEF where TABLE_NAME = '" + tabName + "'";
                dsDD = CliUtils.ExecuteSql("GLModule", "cmdDDUse", strSql, wds.SelectAlias, true, sCurProject);
            }
            else
            {
                string strModuleName = WebDataSet.GetRemoteName(wds.WebDataSetID);
                if (strModuleName.IndexOf('.') != -1)
                {
                    strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
                    tabName = CliUtils.GetTableName(strModuleName, strTableName, sCurProject, "", true);
                }
                string strSql = "select * from COLDEF where TABLE_NAME = '" + tabName + "'";
                dsDD = CliUtils.ExecuteSql(strModuleName, strTableName, strSql, true, sCurProject);
            }
            return dsDD;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            List<string> values = new List<string>();
            if (this.Owner is WebChartBase)
            {
                if (sKind.ToLower().Equals("fieldname") || sKind.ToLower().Equals("captionfieldname"))
                {
                    WebChartBase chart = (WebChartBase)this.Owner;
                    if (chart.Page != null && chart.DataSourceID != null && chart.DataSourceID != "")
                    {
                        foreach (Control ctrl in chart.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == chart.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.SelectAlias != null && ds.SelectAlias != "" && ds.SelectCommand != null && ds.SelectCommand != "")
                                {
                                    DataTable table = ds.CommandTable;
                                    foreach (DataColumn column in table.Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                    break;
                                }
                                else
                                {
                                    if (ds.DesignDataSet == null)
                                    {
                                        WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                        if (wds != null)
                                        {
                                            ds.DesignDataSet = wds.RealDataSet;
                                        }
                                    }
                                    if (ds.DesignDataSet != null)
                                    {
                                        foreach (DataColumn column in ds.DesignDataSet.Tables[((WebDataSource)ctrl).DataMember].Columns)
                                        {
                                            values.Add(column.ColumnName);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return values.ToArray();
        }
        #endregion
    }
}
