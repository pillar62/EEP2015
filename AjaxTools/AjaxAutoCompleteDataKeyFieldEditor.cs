using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Xml;
using Srvtools;
using System.Data;
using System.Collections.Generic;

namespace AjaxTools
{
    public class AjaxAutoCompleteDataKeyFieldEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (context != null)
            {
                object obj = context.Instance;
                if (obj is AjaxAutoComplete)
                {
                    AjaxAutoComplete autoComplete = (AjaxAutoComplete)obj;
                    string selAlias = autoComplete.SelectAlias;
                    if (selAlias != null && selAlias != "")
                    {
                        string conString = GloFix.GetConnString(selAlias);
                        int conType = GloFix.GetConnType(selAlias);

                        IDbConnection conn = GloFix.AllocateConn(conString, (ClientType)conType);
                        if (conn == null)
                            return null;
                        string cmdTable = autoComplete.CommandTable;
                        if (cmdTable != null && cmdTable != "")
                        {
                            String sQL = "select * from " + cmdTable;
                            IDbCommand cmd = GloFix.AllocateCommand(conn, sQL);
                            if (conn.State == ConnectionState.Closed)
                            { conn.Open(); }

                            IDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                            DataTable schemaTable = reader.GetSchemaTable();
                            conn.Close();
                            List<String> columnList = new List<string>();
                            foreach (DataRow r in schemaTable.Rows)
                            {
                                columnList.Add(r["ColumnName"].ToString());
                            }

                            if (edSvc != null)
                            {
                                StringListSelector mySelector = new StringListSelector(edSvc, columnList.ToArray());
                                string strValue = (string)value;
                                if (mySelector.Execute(ref strValue)) value = strValue;

                                return strValue;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
