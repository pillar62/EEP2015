using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Srvtools;

namespace Infolight.EasilyReportTools.Tools
{
    internal class DDProvider
    {
        private DataTable dtDDInfo;
        
        public DDProvider(object dataSource, bool designTime)
        {
            string tableName = String.Empty;
            DataSet dsDDInfo = null;
            DataView dvDataSource = null;
            DataTable dtDataSource = null;

            switch (dataSource.GetType().Name)
            {
                case ComponentInfo.InfoBindingSource:
                    if (designTime && ((InfoBindingSource)dataSource).DataSource.GetType().Name == ComponentInfo.InfoBindingSource)
                    {
                        tableName = ((InfoBindingSource)dataSource).DataMember;
                        dtDataSource = ((InfoDataSet)(((InfoBindingSource)((InfoBindingSource)dataSource).DataSource)).DataSource).RealDataSet.Relations[tableName].ChildTable;
                    }
                    else
                    {
                        dvDataSource = DataSourceExchange.GetDataView(dataSource);
                        dtDataSource = dvDataSource.Table;
                    }
                    dsDDInfo = DBUtils.GetDataDictionary((InfoBindingSource)dataSource, designTime);
                    break;

                case ComponentInfo.WebDataSource:
                    dtDataSource = DataSourceExchange.GetWebDataTable((WebDataSource)dataSource, designTime);
                    dvDataSource = dtDataSource.DefaultView;
                    dsDDInfo = DBUtils.GetDataDictionary((WebDataSource)dataSource, designTime);
                    break;
            }

            dtDDInfo = dsDDInfo.Tables[0];
        }

        

        public object GetDDValue(string srcColumnName, string ddColumnName)
        {
            object resValue = string.Empty;

            if (dtDDInfo.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDDInfo.Rows)
                {
                    if (string.Compare(dr[DDInfo.FieldName].ToString(), srcColumnName, true) == 0)
                    {
                        resValue = dr[ddColumnName];
                    }
                }
            }

            return resValue;
        }
    }

    internal class DDInfo
    {
        public const string ddTableName = "COLDEF";
        public const string FieldName = "FIELD_NAME";
        public const string FieldType = "FIELD_TYPE";
        public const string FieldLength = "FIELD_LENGTH";
        public const string FieldCaption = "CAPTION";
        public const string EditMask = "EDITMASK";
    }
}
