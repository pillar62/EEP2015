using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Xml;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
#if MySql
using MySql.Data.MySqlClient;
#endif
using System.Data.OleDb;

namespace Srvtools
{
    //only for design
    public partial class MultiLanguageEditorDialog : Form
    {
        public MultiLanguageEditorDialog(object editingcontrol)
        {
            InitializeComponent();
            _EditingControl = editingcontrol;
            InitialParameter();
        }

        private object _EditingControl;

        public object EditingControl
        {
            get { return _EditingControl; }
        }

        private int _ID;

        public int ID
        {
            get { return _ID; }
        }

        private string _Alias;

        public string Alias
        {
            get { return _Alias; }
        }
	
        private string _XMLFile;

        public string XMLFile
        {
            get { return _XMLFile; }
        }

        private SelectedLanguages _Languages;

        public SelectedLanguages Languages
        {
            get { return _Languages; }
            set
            { 
                _Languages = value;
                int flag = 1;
                for (int i = 0; i < 8; i++)
                {
                    if (((int)_Languages & flag) > 0)
                    {
                        this.dataGridView.Columns[i + 2].Visible = true;
                    }
                    else
                    {
                        this.dataGridView.Columns[i + 2].Visible = false;
                    }
                    flag *= 2;
                }
            }
        }
	
        private void BaseMultiLanguageEditorDialog_Load(object sender, EventArgs e)
        {
            if (!LoadXml())
            {
                SelectLanguages();
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshControls();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            SelectLanguages();
        }

        private void buttonReadDB_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Replace multilanguage defination from database?", "Confirm"
                , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ReadFromDataBase();
            }
        }

        private void buttonWriteDB_Click(object sender, EventArgs e)
        {
            WriteToDataBase();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!SaveXml())
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private void InitialParameter()
        {
            string sCurProject = EditionDifference.ActiveSolutionName();

            if (EditingControl is MultiLanguage)
            {
                MultiLanguage control = EditingControl as MultiLanguage;
                _ID = control.Identification;
                _Alias = control.DataBase;
                _XMLFile = string.Format("{0}\\{1}\\{2}.xml", EEPRegistry.Client, sCurProject, ID);

            }
            else
            {
                WebMultiLanguage control = EditingControl as WebMultiLanguage;
                _Alias = control.DataBase;
                _XMLFile = string.Format("{0}.xml", EditionDifference.ActiveDocumentFullName());
                string strid = XMLFile + control.Site.Name;
                _ID = strid.GetHashCode();
            }
            if (string.IsNullOrEmpty(Alias))
            {
                groupBoxDB.Enabled = false;
            }
            this.Text = string.Format("MultiLanguage Editor({0})", ID);
        }

        private void SelectLanguages()
        {
            frmSelectLanguage form = new frmSelectLanguage(Languages);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Languages = form.Languages;
            }
        }

        private DbConnection CreateConnection(string alias)
        {
            try
            {
                DbConnectionSet.DbConnection db = DbConnectionSet.GetDbConn(alias);
                if (db == null)
                {
                    throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, this.GetType(), null, "alias", alias);
                }
                else
                {
                    DbConnection conn = (DbConnection)db.CreateConnection();
                    conn.Open();
                    return conn;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

//            DbConnection connection = null;
//            XmlDocument xml = new XmlDocument();
//            try
//            {
//                xml.Load(SystemFile.DBFile);
//                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", Alias));
//                if (node != null)
//                {
//                    if(node.Attributes["String"] != null)
//                    {
//                        string password = node.Attributes["Password"] == null? 
//                            string.Empty: string.Format("password={0}", GetPwdString(node.Attributes["Password"].Value));
//                        string connectionstring = string.Format("{0};{1}", node.Attributes["String"].Value, password);
//                        string type = (node.Attributes["Type"] == null) ? "1" : node.Attributes["Type"].Value;
//                        switch (type)
//                        {
//                            case "1": connection = new System.Data.SqlClient.SqlConnection(connectionstring); break;
//                            case "2": connection = new System.Data.OleDb.OleDbConnection(connectionstring); break;
//                            case "3": connection = new System.Data.OracleClient.OracleConnection(connectionstring); break;
//                            case "4": connection = new System.Data.Odbc.OdbcConnection(connectionstring); break;
//#if MySql
//                            case "5": connection = new MySql.Data.MySqlClient.MySqlConnection(connectionstring); break;
//#endif
//                        }
//                        connection.Open();
//                    }
//                    else
//                    {
//                        MessageBox.Show(this, string.Format("ConnectString of Database: {0} hasn't been defined",Alias) 
//                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    }
//                }
//                else
//                {
//                    MessageBox.Show(this, string.Format("Database: {0} hasn't been defined",Alias) 
//                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//            catch(Exception e)
//            {
//                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return null;
//            }

//            return connection;
        }

        private static string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        private void RefreshControls()
        {
            using (DbConnection connection = CreateConnection(Alias))
            {
                List<DictionaryEntry> list = new List<DictionaryEntry>();
                DbCommand command = null;
                if (connection != null)
                {
                    command = connection.CreateCommand();
                    string strparamter = GetParameter("KEYS", connection);
                    command.CommandText = string.Format("SELECT * FROM COLDEF WHERE FIELD_NAME = {0}", strparamter);
                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = strparamter;
                    if (parameter is System.Data.OleDb.OleDbParameter)
                    {
                        (parameter as System.Data.OleDb.OleDbParameter).OleDbType = System.Data.OleDb.OleDbType.VarChar;
                    }
                    else
                    {
                        parameter.DbType = DbType.String;
                    }
                    parameter.Size = 80;
                    command.Parameters.Add(parameter);
                    command.Prepare();
                }
                if (EditingControl is MultiLanguage)
                {
                    MultiLanguage control = EditingControl as MultiLanguage;
                    list = control.GetControlValues();
                }
                else
                {
                    WebMultiLanguage control = EditingControl as WebMultiLanguage;
                    list = control.GetControlValues();
                }
                try
                {
                    foreach (DictionaryEntry entry in list)
                    {
                        DataRow[] drs = dataSet.Tables["Table"].Select(string.Format("KEYS = '{0}'", entry.Key));
                        if (drs.Length == 0)
                        {
                            DataRow dr = dataSet.Tables["Table"].NewRow();
                            dr["IDENTIFICATION"] = ID.ToString();
                            dr["KEYS"] = entry.Key;
                            string value = entry.Value == null? string.Empty: entry.Value.ToString();
                            if (command != null)
                            {
                                command.Parameters[0].Value = value;
                                DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);
                                if (reader.Read())
                                {
                                    int flag = 1;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        if (((int)_Languages & flag) > 0)
                                        {
                                            object ddvalue = reader[string.Format("CAPTION{0}", i + 1)];
                                            if (ddvalue == DBNull.Value)
                                            {
                                                dr[((SelectedLanguages)flag).ToString()] = value;
                                            }
                                            else
                                            {
                                                dr[((SelectedLanguages)flag).ToString()] = ddvalue;
                                            }
                                        }
                                        flag *= 2;
                                    }
                                }
                                else
                                {
                                    int flag = 1;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        if (((int)_Languages & flag) > 0)
                                        {
                                            dr[((SelectedLanguages)flag).ToString()] = value;
                                        }
                                        flag *= 2;
                                    }
                                }
                                if (command is OleDbCommand)
                                    command.Cancel();
                                reader.Close();
                            }
                            dataSet.Tables["Table"].Rows.Add(dr);
                        }
                    }
                }   
                catch(Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
         
        }

        private void ReadFromDataBase()
        {
            using (DbConnection connection = CreateConnection(Alias))
            {
                if (connection != null)
                {
                    DbCommand command = connection.CreateCommand();
                    try
                    {
                        command.CommandText = string.Format("SELECT * FROM SYS_LANGUAGE WHERE IDENTIFICATION = '{0}'", ID);
                        DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        dataSet.Tables["Table"].Rows.Clear();
                        while (reader.Read())
                        {
                            DataRow row = dataSet.Tables["Table"].NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader[i];
                            }
                            dataSet.Tables["Table"].Rows.Add(row);
                        }
                        command.Cancel();
                        reader.Close();
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    groupBoxDB.Enabled = false;
                }
            }
        }

        private void WriteToDataBase()
        {
            using (DbConnection connection = CreateConnection(Alias))
            {
                if (connection != null)
                {
                    DbTransaction transaction = connection.BeginTransaction();
                    DbCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    try
                    {
                        command.CommandText = string.Format("DELETE FROM SYS_LANGUAGE WHERE IDENTIFICATION = '{0}'", ID);
                        command.ExecuteNonQuery();

                        StringBuilder sbuilder = new StringBuilder();
                        StringBuilder parambuilder = new StringBuilder();
                        DbParameter[] parameters = new DbParameter[dataSet.Tables["Table"].Columns.Count - 1];
                        sbuilder.Append("INSERT INTO SYS_LANGUAGE (");
                        parambuilder.Append("VALUES (");
#if MySql
                        if (connection is MySql.Data.MySqlClient.MySqlConnection)
                        {
                            for (int i = 1; i < dataSet.Tables["Table"].Columns.Count; i++)
                            {
                                if (dataSet.Tables["Table"].Columns[i].ColumnName == "KEYS")
                                    sbuilder.Append("KEY_S");
                                else
                                    sbuilder.Append(dataSet.Tables["Table"].Columns[i].ColumnName);
                                if (i != dataSet.Tables["Table"].Columns.Count - 1)
                                {
                                    sbuilder.Append(", ");
                                }
                                else
                                {
                                    sbuilder.Append(") ");
                                }
                            }

                            for (int i = 0; i < dataSet.Tables["Table"].Rows.Count; i++)
                            {
                                parambuilder = new StringBuilder();
                                parambuilder.Append("VALUES (");
                                //parambuilder.Append("0, ");
                                for (int j = 1; j < dataSet.Tables["Table"].Columns.Count; j++)
                                {
                                    parambuilder.Append("'" + dataSet.Tables["Table"].Rows[i][j].ToString() + "'");

                                    if (j != dataSet.Tables["Table"].Columns.Count - 1)
                                    {
                                        parambuilder.Append(", ");
                                    }
                                    else
                                    {
                                        parambuilder.Append(") ");
                                    }
                                }

                                command.CommandText = string.Format("{0}{1}", sbuilder, parambuilder);
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            MessageBox.Show(this, "Write to Database successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
#endif

                        for (int i = 1; i < dataSet.Tables["Table"].Columns.Count; i++)
                        {
                            sbuilder.Append(dataSet.Tables["Table"].Columns[i].ColumnName);
                            string parametername = GetParameter(dataSet.Tables["Table"].Columns[i].ColumnName, connection);
                            parambuilder.Append(parametername);
                            parameters[i - 1] = command.CreateParameter();
                            if (!(parameters[i - 1] is System.Data.Odbc.OdbcParameter))
                            {
                                parameters[i - 1].ParameterName = parametername;
                            }
                            if (parameters[i - 1] is OleDbParameter)
                            {
                                (parameters[i - 1] as OleDbParameter).OleDbType = OleDbType.VarChar;
                            }
                            else
                                parameters[i - 1].DbType = DbType.String;
                            parameters[i - 1].Size = 80;

                            if (i != dataSet.Tables["Table"].Columns.Count - 1)
                            {
                                sbuilder.Append(", ");
                                parambuilder.Append(", ");
                            }
                            else
                            {
                                sbuilder.Append(") ");
                                parambuilder.Append(") ");
                            }
                        }
                        command.CommandText = string.Format("{0}{1}", sbuilder, parambuilder);
                        command.Parameters.AddRange(parameters);
                        command.Prepare();

                        for (int i = 0; i < dataSet.Tables["Table"].Rows.Count; i++)
                        {
                            for (int j = 1; j < dataSet.Tables["Table"].Columns.Count; j++)
                            {
                                if (command is OleDbCommand)
                                {
                                    command.Parameters[j - 1].Value = dataSet.Tables["Table"].Rows[i][j];
                                }
                                else
                                {
                                    string parametername = GetParameter(dataSet.Tables["Table"].Columns[j].ColumnName, connection);
                                    command.Parameters[parametername].Value = dataSet.Tables["Table"].Rows[i][j];
                                }
                            }

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        MessageBox.Show(this, "Write to Database successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    groupBoxDB.Enabled = false;
                }
            }
        }

        private string GetParameter(string columnname, DbConnection connection)
        {
            string parameter = string.Empty;
            if (connection is System.Data.OracleClient.OracleConnection)
            {
                parameter = string.Format(":{0}", columnname);
            }
            else if (connection is System.Data.Odbc.OdbcConnection)
            {
                parameter = "?";
            }
            else if (connection is System.Data.OleDb.OleDbConnection)
            {
                parameter = "?";
            }
#if MySql
            else if (connection is MySql.Data.MySqlClient.MySqlConnection)
            {
                parameter = string.Format("@{0}", columnname);
            }
#endif
            else
            {
                parameter = string.Format("@{0}", columnname);
            }
            return parameter;
        }

        private bool LoadXml()
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(XMLFile);
                XmlNode nodelanguage = xml.SelectSingleNode("Infolight/MultiLanguage");
                if (nodelanguage == null || nodelanguage.Attributes["language"] == null)
                {
                    return false;
                }
                Languages = ParseLanguage(nodelanguage.Attributes["language"].Value);
                if (Languages == SelectedLanguages.None)
                {
                    return false;
                }
                dataSet.Tables["Table"].Rows.Clear();
                while (nodelanguage.NextSibling != null)
                {
                    nodelanguage = nodelanguage.NextSibling;
                    ParseLanguageText(nodelanguage);
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        private SelectedLanguages ParseLanguage(string text)
        {
            SelectedLanguages languages = SelectedLanguages.None;
            string[] arrtext = text.Split(';');
            foreach (string str in arrtext)
            {
                try
                {
                    languages |= (SelectedLanguages)Enum.Parse(typeof(SelectedLanguages), str, true);
                }
                catch (ArgumentException) { }
            }
            return languages;
        }

        private void ParseLanguageText(XmlNode nodelanguage)
        {
            if (dataSet.Tables["Table"].Columns.Contains(nodelanguage.Name))
            {
                foreach (XmlNode node in nodelanguage.ChildNodes)
                {
                    if (node.Attributes["field"] != null && node.Attributes["value"] != null)
                    {
                        string field = node.Attributes["field"].Value;
                        string value = node.Attributes["value"].Value;
                        DataRow[] drs = dataSet.Tables["Table"].Select(string.Format("KEYS = '{0}'", field));
                        DataRow dr = null;
                        if (drs.Length == 0)
                        {
                            dr = dataSet.Tables["Table"].NewRow();
                            dr["IDENTIFICATION"] = ID.ToString();
                            dr["KEYS"] = field;
                            dataSet.Tables["Table"].Rows.Add(dr);
                        }
                        else
                        {
                            dr = drs[0];
                        }
                        dr[nodelanguage.Name] = value;
                    }
                }
            }
        }

        private bool SaveXml()
        {
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            xml.AppendChild(xml.CreateElement("Infolight"));
            SaveLanguageNode(xml);
            try
            {
                xml.Save(XMLFile);
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SaveLanguageNode(XmlDocument xml)
        {
            XmlNode node = xml.CreateElement("MultiLanguage");
            xml.DocumentElement.AppendChild(node);
            XmlAttribute attribute = xml.CreateAttribute("language");

            StringBuilder sBuilder = new StringBuilder();
            int flag = 1;
            for (int i = 0; i < 8; i++)
            {
                string strlanguage = ((SelectedLanguages)flag).ToString();
                XmlNode nodelanguage = xml.CreateElement(strlanguage);
                xml.DocumentElement.AppendChild(nodelanguage);
                if (((int)_Languages & flag) > 0)
                {
                    if (sBuilder.Length > 0)
                    {
                        sBuilder.Append(";");
                    }
                    sBuilder.Append(strlanguage);
                    for (int j = 0; j < dataSet.Tables["Table"].Rows.Count; j++)
                    {
                        XmlNode nodetext = xml.CreateElement("Language");
                        XmlAttribute attfield = xml.CreateAttribute("field");
                        attfield.Value = dataSet.Tables["Table"].Rows[j]["KEYS"].ToString();
                        nodetext.Attributes.Append(attfield);
                        XmlAttribute attvalue = xml.CreateAttribute("value");
                        attvalue.Value = dataSet.Tables["Table"].Rows[j][strlanguage].ToString();
                        nodetext.Attributes.Append(attvalue);
                        nodelanguage.AppendChild(nodetext);
                    }
                }
                flag *= 2;
            }
            attribute.Value = sBuilder.ToString();
            node.Attributes.Append(attribute);
        }
    }

    [Flags]
    public enum SelectedLanguages
    { 
        None = 0,
        EN = 1,
        CHT = 2,
        CHS = 4,
        HK = 8,
        JA = 16,
        KO = 32,
        LAN1 = 64,
        LAN2 = 128
    }
}