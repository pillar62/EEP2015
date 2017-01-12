using System;
using System.Collections.Generic;
using System.Text;
using Srvtools;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Infolight.EasilyReportTools.Tools;
using System.Collections;
using System.Drawing;
using System.Data.OleDb;


namespace Infolight.EasilyReportTools.DataCenter
{
    internal class DBGateway
    {
        private IReport report;
        public DBGateway(IReport rpt)
        {
            report = rpt;

        }

        public bool LoadTemplate(string fileName)
        {
            return LoadTemplate(report.ReportID, fileName);
        }

        public bool LoadTemplate(string reportID, string fileName)
        {
            string sql = GetSelectTemplateSql(reportID, fileName);
            DataSet ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, true, CliUtils.fCurrentProject);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                BinarySerialize serializer = new BinarySerialize();
                report.ReportName = dr[SysRptDB.ReportName].ToString();
                report.Description = dr[SysRptDB.Description].ToString();
                report.FilePath = dr[SysRptDB.FilePath].ToString();
                report.OutputMode = (OutputModeType)Enum.Parse(typeof(OutputModeType), dr[SysRptDB.OutputMode].ToString());
                string repeater = dr[SysRptDB.HeaderRepeat].ToString();
                if (string.IsNullOrEmpty(repeater))
                {
                    report.HeaderRepeat = true;
                }
                else
                {
                    report.HeaderRepeat = Boolean.Parse(repeater);
                }

                report.HeaderFont = (Font)serializer.DeSerialize((byte[])dr[SysRptDB.HeaderFont]);
                ReportItemCollection headerItems = (ReportItemCollection)serializer.DeSerialize(((byte[])dr[SysRptDB.HeaderItems]));
                report.HeaderItems.Clear();
                foreach (ReportItem item in headerItems)
                {
                    report.HeaderItems.Add(item.Copy());
                }

                report.FieldFont = (Font)serializer.DeSerialize((byte[])dr[SysRptDB.FieldFont]);
                DataSourceItemCollection fieldItems = (DataSourceItemCollection)serializer.DeSerialize(((byte[])dr[SysRptDB.FieldItems]));
                report.FieldItems.Clear();
                foreach (DataSourceItem item in fieldItems)
                {
                    report.FieldItems.Add(item.Copy());
                }


                report.FooterFont = (Font)serializer.DeSerialize((byte[])dr[SysRptDB.FooterFont]);
                ReportItemCollection FooterItems = (ReportItemCollection)serializer.DeSerialize(((byte[])dr[SysRptDB.FooterItems]));
                report.FooterItems.Clear();
                foreach (ReportItem item in FooterItems)
                {
                    report.FooterItems.Add(item.Copy());
                }

                ParameterItemCollection parameters = (ParameterItemCollection)serializer.DeSerialize((byte[])dr[SysRptDB.Parameters]);
                report.Parameters.Clear();
                foreach (ParameterItem item in parameters)
                {
                    report.Parameters.Add(item.Copy());
                }

                ImageItemCollection images = (ImageItemCollection)serializer.DeSerialize((byte[])dr[SysRptDB.Images]);
                report.Images.Clear();
                foreach (ImageItem item in images)
                {
                    report.Images.Add(item.Copy());
                }

                ((ReportFormat)serializer.DeSerialize((byte[])dr[SysRptDB.Format])).CopyTo(this.report.Format);
                ((MailConfig)serializer.DeSerialize((byte[])dr[SysRptDB.MailSetting])).CopyTo(this.report.MailSetting);
                return true;
            }
            else
            {
                return false;
            }
        }

        public DictionaryEntry[] GetTemplates(bool designMode)
        {
            StringBuilder sql = new StringBuilder("Select REPORTID, FILENAME from SYS_REPORT");
            if (!designMode)
            {
                sql.Append(string.Format(" Where REPORTID='{0}'", report.ReportID));
            }
            DataSet ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql.ToString(), true, CliUtils.fCurrentProject);
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new DictionaryEntry(ds.Tables[0].Rows[i]["REPORTID"].ToString(), ds.Tables[0].Rows[i]["FILENAME"].ToString()));
                }
            
            }
            return list.ToArray();
        }

        public ExecutionResult SaveTemplate(string fileName)
        {


            #region Variable Definition
            ExecutionResult execRes = new ExecutionResult();
            BinarySerialize serializer = new BinarySerialize();
            byte[] buffer = null;
            Hashtable values = new Hashtable();
            #endregion
            //IDbConnection iDbConn = this.GetConnection(this.report.EEPAlias);
            try
            {
                #region Get serialized values
                //ReportID
                if (string.IsNullOrEmpty(this.report.ReportID))
                {
                    values.Add(SysRptDB.ReportID, SysRptDB.SysReportID);
                }
                else
                {
                    values.Add(SysRptDB.ReportID, this.report.ReportID);
                }
                //FileName
                //values.Add(Path.GetFileName(report.FilePath));
                values.Add(SysRptDB.FileName, fileName);

                //ReportName
                values.Add(SysRptDB.ReportName, report.ReportName);

                //Description
                values.Add(SysRptDB.Description, report.Description);

                //FilePath
                values.Add(SysRptDB.FilePath, report.FilePath);


                //OutputMode
                values.Add(SysRptDB.OutputMode, report.OutputMode.ToString());
                
                //HeaderRepeat
                values.Add(SysRptDB.HeaderRepeat, report.HeaderRepeat.ToString());

                //HeaderFont
                buffer = serializer.Serialize(report.HeaderFont);
                values.Add(SysRptDB.HeaderFont, buffer);

                //HeaderItems
                buffer = serializer.Serialize(report.HeaderItems);
                values.Add(SysRptDB.HeaderItems, buffer);

                //FooterFont
                buffer = serializer.Serialize(report.FooterFont);
                values.Add(SysRptDB.FooterFont, buffer);

                //FooterItems
                buffer = serializer.Serialize(report.FooterItems);
                values.Add(SysRptDB.FooterItems, buffer);

                //FieldFont
                buffer = serializer.Serialize(report.FieldFont);
                values.Add(SysRptDB.FieldFont, buffer);

                //FieldItems
                buffer = serializer.Serialize(report.FieldItems);
                values.Add(SysRptDB.FieldItems, buffer);

                ////Setting
                //buffer = serializer.Serialize(report.Setting);
                //values.Add(SysRptDB.Setting, buffer);

                //Format
                buffer = serializer.Serialize(report.Format);
                values.Add(SysRptDB.Format, buffer);

                //Parameters
                buffer = serializer.Serialize(report.Parameters);
                values.Add(SysRptDB.Parameters, buffer);

                //Images
                buffer = serializer.Serialize(report.Images);
                values.Add(SysRptDB.Images, buffer);

                //Mail Setting
                buffer = serializer.Serialize(report.MailSetting);
                values.Add(SysRptDB.MailSetting, buffer);
                #endregion


                if (this.report.ReportID == null || this.report.ReportID == String.Empty)
                {
                    execRes = this.ExecuteSql(GetSelectTemplateSql(fileName), ExecuteMode.Select);
                }
                else
                {
                    execRes = this.ExecuteSql(GetSelectTemplateSql(this.report.ReportID, fileName), ExecuteMode.Select);
                }

                if (execRes.Status)
                {
                    if (((DataTable)execRes.Anything).Rows.Count == 0)
                    {
                        execRes = this.ExecuteSql(GetInsertSysRptTableSql(values), ExecuteMode.Insert, values);
                    }
                    else
                    {
                        //if (saveMode == SaveMode.Save)
                        //{
                        execRes = this.ExecuteSql(GetUpdateSysRptTableSql(values), ExecuteMode.Update, values);
                        //}
                        //else
                        //{
                        //  execRes.Status = false;
                        //    execRes.Message = MessageInfo.TemplateExist;
                        //}
                    }
                }

                if (execRes.Status)
                {
                    execRes.Message = MessageInfo.SaveSuccess;
                }
            }
            catch (Exception ex)
            {
                execRes.Status = false;
                execRes.Message = ex.Message;
            }
            finally 
            {
            }

            return execRes;
        }

        public void DeleteTemplate(string fileName)
        {
            DeleteTemplate(report.ReportID, fileName);
        }

        public void DeleteTemplate(string reportID, string fileName)
        {
            //IDbConnection iDbConn = this.GetConnection(this.report.EEPAlias);
            try
            {
               
                this.ExecuteSql(GetDeleteTemplateSql(reportID, fileName), ExecuteMode.Delete);
                
            }
            catch (Exception ex)
            {
                //log.WriteExceptionInfo(ex);
                throw ex;
            }
            finally
            {
            }
        
        }

        public enum SaveMode
        { 
            Save,
            SaveAs
        }


        public ExecutionResult ExecuteSql(string sql, ExecuteMode executeMode)
        {
            return this.ExecuteSql(sql, executeMode, null);
        }

        public ExecutionResult ExecuteSql(string sql, ExecuteMode executeMode, Hashtable htValues)
        {
            ExecutionResult execRes = new ExecutionResult();
            bool designMode = string.IsNullOrEmpty(CliUtils.fLoginDB);
            if (designMode)
            {
                return ExecuteSqlDesign(sql, executeMode, htValues);
            }
            string database = designMode ? report.EEPAlias : CliUtils.fLoginDB;
            CliUtils.fLoginDB = database;
            ClientType clientType = CliUtils.GetDataBaseType(database);

            DBParameter dbParameter = new DBParameter(clientType);
            try
            {
                ArrayList parameters = new ArrayList();
                if (htValues != null)
                {
                    foreach (DictionaryEntry de in htValues)
                    {
                        IDbDataParameter dbParam = dbParameter.CreateParameter();
                        dbParam.ParameterName = dbParameter.ParamTag + de.Key;
                        dbParam.Value = de.Value;
                        if (de.Value != null)
                        {
                            if (de.Value.GetType().Name == "Byte[]")
                            {
                                if (dbParam is OleDbParameter)
                                    (dbParam as OleDbParameter).OleDbType = OleDbType.LongVarBinary;
                                else
                                    dbParam.DbType = DbType.Binary;
                            }
                            else
                            {
                                if (dbParam is OleDbParameter)
                                    (dbParam as OleDbParameter).OleDbType = OleDbType.VarChar;
                                else
                                    dbParam.DbType = DbType.String;
                            }
                        }
                        else
                        {
                            if (dbParam is OleDbParameter)
                                (dbParam as OleDbParameter).OleDbType = OleDbType.VarChar;
                            else
                                dbParam.DbType = DbType.String;
                            dbParam.Value = "";
                        }
                        parameters.Add(dbParam);
                    }
                }

                DataSet ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, database, executeMode == ExecuteMode.Select, CliUtils.fCurrentProject, null, parameters);

                if (executeMode == ExecuteMode.Select)
                {
                    execRes.Anything = ds.Tables[0];
                }

                //if (executeMode == ExecuteMode.Select)
                //{
                //    iDbDataAdapter = DBUtils.CreateDbDataAdapter(iDbCommand);
                //    if (iDbCommand.Connection is OleDbConnection)
                //    {
                //        DataTable dt = new DataTable();
                //        (iDbDataAdapter as OleDbDataAdapter).Fill(dt);
                //        ds.Tables.Add(dt);
                //    }
                //    else
                //        iDbDataAdapter.Fill(ds);
                //    //iDbDataAdapter.Fill(ds);
                //    execRes.Anything = ds.Tables[0];
                //}
                //else
                //{
                //    iDbCommand.ExecuteNonQuery();
                //}
                execRes.Status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (designMode)
                {
                    CliUtils.fLoginDB = string.Empty;
                }
            }

            return execRes;
        }


        /// <summary>
        /// Used for DesignTime
        /// </summary>
        /// <param name="dbAlias"></param>
        /// <returns></returns>
        private IDbConnection GetConnection(string dbAlias)
        {
            #region Variable Definition
            DbConnectionSet.DbConnection dbConn = null;
            #endregion

            dbConn = DbConnectionSet.GetDbConn(dbAlias);
            return dbConn.CreateConnection();

        }

        public ExecutionResult ExecuteSqlDesign(string sql, ExecuteMode executeMode, Hashtable htValues)
        {
            IDbConnection iDbConn = GetConnection(report.EEPAlias);
            #region Variable Definition
            ExecutionResult execRes = new ExecutionResult();
            //IDbConnection iDbConn = null;
            IDbCommand iDbCommand = null;
            IDbDataAdapter iDbDataAdapter = null;
            DataSet ds = new DataSet();
            IDbDataParameter dbParam = null;
            #endregion

            DBParameter dbParameter = new DBParameter(DBUtils.GetDatabaseType(iDbConn));
            try
            {

                if (iDbConn.State == ConnectionState.Closed)
                {
                    iDbConn.Open();
                }
                iDbCommand = iDbConn.CreateCommand();
                iDbCommand.CommandText = sql;
                if (htValues != null)
                {
                    foreach (DictionaryEntry de in htValues)
                    {
                        dbParam = iDbCommand.CreateParameter();
                        dbParam.ParameterName = dbParameter.ParamTag + de.Key;
                        dbParam.Value = de.Value;
                        if (de.Value != null)
                        {
                            if (de.Value.GetType().Name == "Byte[]")
                            {
                                if (iDbCommand.Connection is OleDbConnection)
                                    (dbParam as OleDbParameter).OleDbType = OleDbType.LongVarBinary;
                                else
                                    dbParam.DbType = DbType.Binary;
                            }
                            else
                            {
                                if (iDbCommand.Connection is OleDbConnection)
                                    (dbParam as OleDbParameter).OleDbType = OleDbType.VarChar;
                                else
                                    dbParam.DbType = DbType.String;
                            }
                        }
                        else
                        {
                            if (iDbCommand.Connection is OleDbConnection)
                                (dbParam as OleDbParameter).OleDbType = OleDbType.VarChar;
                            else
                                dbParam.DbType = DbType.String;
                            dbParam.Value = "";
                        }
                        iDbCommand.Parameters.Add(dbParam);
                    }
                }



                if (executeMode == ExecuteMode.Select)
                {
                    iDbDataAdapter = DBUtils.CreateDbDataAdapter(iDbCommand);
                    DataTable dt = new DataTable();
                    if (iDbCommand.Connection is OleDbConnection)
                    {
                        (iDbDataAdapter as OleDbDataAdapter).Fill(dt);
                        ds.Tables.Add(dt);
                    }
                    else
                        iDbDataAdapter.Fill(ds);
                    //iDbDataAdapter.Fill(ds);
                    execRes.Anything = ds.Tables[0];
                }
                else
                {
                    iDbCommand.ExecuteNonQuery();
                }

                execRes.Status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                iDbConn.Close();
                //iDbConn.Dispose();


            }

            return execRes;
        }
      

        public enum ExecuteMode
        { 
            Insert,
            Update,
            Select,
            Delete
        }

        #region Sql Command
        private string GetLoadTemplateSql()
        {
            string sql = "";
            //if (designTime)
            //{
            //    //sql = "select * from " + SysRptDB.TableName + " where " + SysRptDB.ReportID + " = '" + SysRptDB.SysReportID + "'";
            //    sql = "select * from " + SysRptDB.TableName;
            //}
            //else
            //{
                sql = "select * from " + SysRptDB.TableName + " where " + SysRptDB.ReportID + " != '" + SysRptDB.SysReportID + "'";
            //}
            return sql;
        }

        private string GetInsertSysRptTableSql(Hashtable values)
        {
            string database = string.IsNullOrEmpty(CliUtils.fLoginDB) ? report.EEPAlias : CliUtils.fLoginDB;
            ClientType clientType = CliUtils.GetDataBaseType(database);
            DBParameter dbParameter = new DBParameter(clientType);
            string sql = "";
            sql = "insert into " + SysRptDB.TableName + " (";

            foreach (DictionaryEntry de in values)
            {
                sql += de.Key + ",";
            }
            sql = sql.Remove(sql.Length - 1);
            sql += ")";

            sql += " values(";
            foreach (DictionaryEntry de in values)
            {
                if (clientType == ClientType.ctOleDB || clientType == ClientType.ctInformix)
                    sql += dbParameter.ParamTag + ",";
                else
                    sql += dbParameter.ParamTag + de.Key + ",";
            }
            sql = sql.Remove(sql.Length - 1);
            sql += ")";
            return sql;
        }

        private string GetUpdateSysRptTableSql(Hashtable values)
        {
            string database = string.IsNullOrEmpty(CliUtils.fLoginDB) ? report.EEPAlias : CliUtils.fLoginDB;
            ClientType clientType = CliUtils.GetDataBaseType(database);
            DBParameter dbParameter = new DBParameter(clientType);
            string sql = "";
            sql = "Update " + SysRptDB.TableName;
            sql += " set ";
            for (int i = 2; i < SysRptDB.sysTableColumns.Count; i++)
            {
                foreach (DictionaryEntry de in values)
                {
                    if (de.Key == SysRptDB.sysTableColumns[i])
                    {
                        if (clientType == ClientType.ctOleDB || clientType == ClientType.ctInformix)
                            sql += SysRptDB.sysTableColumns[i] + " = " + dbParameter.ParamTag + ",";
                        else
                            sql += SysRptDB.sysTableColumns[i] + " = " + dbParameter.ParamTag + de.Key + ",";
                    }
                }
            }
            sql = sql.Remove(sql.Length - 1);

            sql += " where ";
            for (int i = 0; i < 2; i++)
            {
                foreach (DictionaryEntry de in values)
                {
                    if (de.Key == SysRptDB.sysTableColumns[i])
                    {
                        if (clientType == ClientType.ctOleDB || clientType == ClientType.ctInformix)
                            sql += SysRptDB.sysTableColumns[i] + " = " + dbParameter.ParamTag + " and ";
                        else
                            sql += SysRptDB.sysTableColumns[i] + " = " + dbParameter.ParamTag + de.Key + " and ";
                    }
                }

            }
            sql = sql.Remove(sql.Length - 4, 4);
            return sql;
        }

        private string GetSelectTemplateSql(string fileName)
        {
            return this.GetSelectTemplateSql(SysRptDB.SysReportID, fileName);
        }

        private string GetSelectTemplateSql(string reportID, string fileName)
        {
            string sql = "";
            sql = "select * from " + SysRptDB.TableName + " where " + SysRptDB.ReportID + " = '" + reportID +
                    "' and " + SysRptDB.FileName + " = '" + fileName + "'";
            return sql;
        }

        private string GetDeleteTemplateSql(string reportID, string fileName)
        {
            string sql = "";
            sql = "delete from " + SysRptDB.TableName + " where " + SysRptDB.ReportID + " = '" + reportID +
                    "' and " + SysRptDB.FileName + " = '" + fileName + "'";
            return sql;
        }
        #endregion

      
    }
}
