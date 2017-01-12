using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Srvtools;

namespace Infolight.EasilyReportTools.Tools
{
    internal class DataSourceExchange
    {
        /// <summary>
        /// Get DataSet by DataSource
        /// </summary>
        /// <param name="dataSource">BindingSource、InfoBindingSource or WebDataSource</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(object dataSource)
        {
            DataSet ds = new DataSet();
            Type datasouceType = null;
            string tableName = String.Empty;
            DataTable dtDataSource = new DataTable();

            datasouceType = dataSource.GetType();
            switch (datasouceType.Name)
            {
                case "BindingSource":
                    ds = ((BindingSource)dataSource).DataSource as DataSet;
                    break;

                case "InfoBindingSource":
                    if (((InfoBindingSource)dataSource).DataSource.GetType().Name == ComponentInfo.InfoBindingSource)
                    {
                        tableName = ((InfoBindingSource)dataSource).DataMember;
                        dtDataSource = ((InfoDataSet)(((InfoBindingSource)((InfoBindingSource)dataSource).DataSource)).DataSource).RealDataSet.Relations[tableName].ChildTable;
                        ds.Tables.Add(dtDataSource.Copy());
                    }
                    else
                    {
                        ds = (((InfoBindingSource)dataSource).DataSource as InfoDataSet).RealDataSet;
                    }
                    break;

                case "WebDataSource":
                    ds = (dataSource as WebDataSource).InnerDataSet;
                    break;
            }

            return ds;
        }

        /// <summary>
        /// Get DataSet by DataSource
        /// </summary>
        /// <param name="dataSource">BindingSource、InfoBindingSource or WebDataSource</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(object dataSource, bool designMode)
        {
            DataSet ds = new DataSet();

            if (designMode)
            {
                if (dataSource.GetType().Name == ComponentInfo.WebDataSource)
                {
                    ds.Tables.Add(GetWebDataTable((WebDataSource)dataSource, designMode).Copy()); 
                }
                else
                {
                    ds = GetDataSet(dataSource);
                }
            }
            else
            {
                ds = GetDataSet(dataSource);
            }

            return ds;
        }

        public static DataTable GetWebDataTable(WebDataSource wds, bool designTime)
        {
            DataTable dtWebDataTable = null;
            WebDataSet wdt = null;

            if (designTime)
            {
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    dtWebDataTable = wds.CommandTable.Copy();
                }
                else
                {
                    wdt = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                    if (wdt != null)
                    {
                        dtWebDataTable = wdt.RealDataSet.Tables[wds.DataMember].Copy();
                    }
                    else
                    {
                        dtWebDataTable = new DataTable();
                    }
                }
            }
            else
            {
                dtWebDataTable = DataSourceExchange.GetDataView(wds).Table.Copy();
            }

            return dtWebDataTable;
        }

        /// <summary>
        /// Get DataView by DataSource
        /// </summary>
        /// <param name="dataSource">BindingSource、InfoBindingSource or WebDataSource</param>
        /// <returns>DataView</returns>
        public static DataView GetDataView(object dataSource)
        {
            DataView dv = new DataView();
            Type datasouceType = null;

            datasouceType = dataSource.GetType();
            switch (datasouceType.Name)
            {
                case "BindingSource":
                    dv = (DataView)((BindingSource)dataSource).List;
                    break;

                case "WebDataSource":
                    WebDataSource wds = (WebDataSource)dataSource;
                    if (wds.CommandTable == null && wds.InnerDataSet == null)
                    {
                        dv = GetWebDataTable(wds, true).DefaultView;
                    }
                    else
                    {
                        dv = (dataSource as WebDataSource).View;
                    }
                    break;

                case "InfoBindingSource":
                    dv = (DataView)((InfoBindingSource)dataSource).List;
                    break;
            }

            return dv;
        }

        /// <summary>
        /// Get sorted DataView
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="sortExpression">sort expression</param>
        /// <returns></returns>
        public static DataView GetSortedDataView(DataTable dt, string sortExpression)
        {
            DataView dv = null;
            
            dv = new DataView(dt);
            if (sortExpression != "")
            {
                dv.Sort = sortExpression;
            }

            return dv;
        }

        /// <summary>
        /// Get DataView by DataSource
        /// </summary>
        /// <param name="dataSource">BindingSource、InfoBindingSource or WebDataSource</param>
        /// <returns>DataView</returns>
        public static void GetAllData(object dataSource)
        {
            InfoDataSet infoDataSet = null;
            WebDataSet webDataSet = null;
            Type datasouceType = null;

            datasouceType = dataSource.GetType();
            switch (datasouceType.Name)
            {
                case "InfoBindingSource":
                    if (((InfoBindingSource)dataSource).DataSource.GetType().Name == "InfoDataSet")
                    {
                        infoDataSet = ((InfoBindingSource)dataSource).DataSource as InfoDataSet;
                        while (infoDataSet.GetNextPacket()) ;
                    }
                    break;

                case "BindingSource":
                    if (((BindingSource)dataSource).DataSource.GetType().Name == "InfoDataSet")
                    {
                        infoDataSet = ((BindingSource)dataSource).DataSource as InfoDataSet;
                        while (infoDataSet.GetNextPacket()) ;
                    }
                    break;

                case "WebDataSource":
                    if (((WebDataSource)dataSource).DataSource.GetType().Name == "WebDataSet")
                    {
                        webDataSet = ((WebDataSource)dataSource).DataSource as WebDataSet;
                        while (webDataSet.GetNextPacket()) ;
                    }
                    break;
            }
        }

        public static DataSet GetPreviewDataSet(DataSet ds)
        {
            #region Variable Definition
            DataRow dr = null;
            int rowCount = 5;
            int charUnit = 65;
            int numValue = 0;
            object objValue = null;
            #endregion

            ds.Tables[0].Clear();
            for (int i = 0; i < rowCount; i++)
            {
                dr = ds.Tables[0].NewRow();
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    switch (ds.Tables[0].Columns[j].DataType.Name)
                    {
                        case "String":
                            objValue = Convert.ToChar(charUnit).ToString().PadLeft(5, Convert.ToChar(charUnit));
                            break;

                        case "Int16":
                        case "Int32":
                        case "Int64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                            objValue = numValue;
                            break;

                        case "DateTime":
                            objValue = DateTime.Now.ToString();
                            break;

                        case "Boolean":
                            objValue = true;
                            break;
                    }

                    if (ds.Tables[0].Columns[j].DataType.Name != "Byte[]")
                    {
                        dr[j] = objValue;
                    }
                }
                charUnit++;
                numValue++;
                ds.Tables[0].Rows.Add(dr);
            }

            return ds;
        }

    }
}
