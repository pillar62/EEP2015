using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
using System.Windows.Forms;

namespace JQClientTools
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
            WriteToDataBase();
        }

        private void InitialParameter()
        {
            if (EditingControl is JQMultiLanguage)
            {
                JQMultiLanguage control = EditingControl as JQMultiLanguage;
                _Alias = control.DBAlias;
                _XMLFile = string.Format("{0}.xml", EFClientTools.Design.DTE.ActiveDocumentFullName);
                //string strid = XMLFile + control.Site.Name;
                string strid = XMLFile + control.ID;
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
            SelectLanguageForm form = new SelectLanguageForm(Languages);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Languages = form.Languages;
            }
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
            //MultiLanguageTableNameDialog aMultiLanguageTableNameDialog = new MultiLanguageTableNameDialog();
            //DialogResult drTableName = aMultiLanguageTableNameDialog.ShowDialog();
            //if (drTableName == System.Windows.Forms.DialogResult.OK)
            //{
            //    sTableName = aMultiLanguageTableNameDialog.TableName;
            //}
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            EFClientTools.DesignClientUtility.ClientInfo.Database = (EditingControl as JQMultiLanguage).DBAlias;
            EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
            if (EditingControl is JQMultiLanguage)
            {
                JQMultiLanguage control = EditingControl as JQMultiLanguage;
                list = control.GetControlValues();
            }
            try
            {
                foreach (DictionaryEntry entry in list)
                {
                    String sTableName = RemoveQuote(GetTableName(entry.Key));

                    DataRow[] drs = dataSet.Tables["Table"].Select(string.Format("KEYS = '{0}'", entry.Key));
                    if (drs.Length == 0)
                    {
                        DataRow dr = dataSet.Tables["Table"].NewRow();
                        dr["IDENTIFICATION"] = ID.ToString();
                        dr["KEYS"] = entry.Key;
                        string value = string.Empty;
                        string fieldName = string.Empty;
                        if (entry.Value is object[])
                        {
                            value = ((object[])entry.Value)[0].ToString();
                            fieldName = ((object[])entry.Value)[1].ToString();
                        }
                        else
                        {
                            value = entry.Value == null ? string.Empty : entry.Value.ToString();
                        }

                        if (fieldName != String.Empty)
                        {
                            List<object> param = new List<object>();
                            param.Add("COLDEF");
                            //暂时先改成以FieldName取DD
                            if (String.IsNullOrEmpty(sTableName))
                                param.Add(String.Format("FIELD_NAME='{0}'", fieldName));
                            else
                                param.Add(String.Format("FIELD_NAME='{0}' AND TABLE_NAME='{1}'", fieldName, sTableName));
                            var res = EFClientTools.DesignClientUtility.GetDataByTableNameWhere(param);

                            if (res != null && res.Count > 0)
                            {
                                int flag = 1;
                                for (int i = 0; i < 8; i++)
                                {
                                    if (((int)_Languages & flag) > 0)
                                    {
                                        System.Reflection.PropertyInfo piCaption = res[0].GetType().GetProperty(string.Format("CAPTION{0}", i + 1));
                                        object ddvalue = null;
                                        if (piCaption != null)
                                            ddvalue = piCaption.GetValue(res[0], null);
                                        if (ddvalue == null || ddvalue == DBNull.Value || ddvalue.ToString().Trim() == String.Empty)
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
                        dataSet.Tables["Table"].Rows.Add(dr);
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(this, e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;
        }

        private string GetTableName(object key)
        {
            String tableName = String.Empty;
            if (key != null && key.ToString() != String.Empty)
            {
                JQMultiLanguage control = EditingControl as JQMultiLanguage;
                String controlId = key.ToString().Split('.')[0];
                tableName = FindTableName(control.Page.Controls, controlId);
            }

            return tableName;
        }

        private String FindTableName(System.Web.UI.ControlCollection controls, string controlId)
        {
            String tableName = String.Empty;
            foreach (System.Web.UI.Control ct in controls)
            {
                if (ct is JQDataGrid)
                {
                    JQDataGrid ctrl = ct as JQDataGrid;
                    if (ctrl.ID == controlId)
                    {
                        if (!String.IsNullOrEmpty(ctrl.RemoteName))
                        {
                            String moduleName = ctrl.RemoteName.Split('.')[0];
                            String commandName = ctrl.RemoteName.Split('.')[1];
                            tableName = EFClientTools.DesignClientUtility.Client.GetObjectClassName(EFClientTools.DesignClientUtility.ClientInfo, moduleName, commandName, null);
                            break;
                        }
                    }
                }
                else if (ct is JQDataForm)
                {
                    JQDataForm ctrl = ct as JQDataForm;
                    if (ctrl.ID == controlId)
                    {
                        if (!String.IsNullOrEmpty(ctrl.RemoteName))
                        {
                            String moduleName = ctrl.RemoteName.Split('.')[0];
                            String commandName = ctrl.RemoteName.Split('.')[1];
                            tableName = EFClientTools.DesignClientUtility.Client.GetObjectClassName(EFClientTools.DesignClientUtility.ClientInfo, moduleName, commandName, null);
                            break;
                        }
                    }
                }
                if (ct.Controls.Count > 0)
                {
                    return FindTableName(ct.Controls, controlId);
                }
            }
            return tableName;
        }

        private string[] QuoteList = new string[] { "[", "]" };
        private string RemoveQuote(string value)
        {
            string rtn = value;
            foreach (string str in QuoteList)
            {
                rtn = rtn.Replace(str, string.Empty);
            }
            return rtn;
        }

        private void ReadFromDataBase()
        {
            EFClientTools.DesignClientUtility.ClientInfo.Database = (EditingControl as JQMultiLanguage).DBAlias;
            EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
            try
            {
                List<object> param = new List<object>();
                param.Add("SYS_LANGUAGE");
                param.Add(String.Format("IDENTIFICATION='{0}'", ID));
                var res = EFClientTools.DesignClientUtility.GetDataByTableNameWhere(param);

                dataSet.Tables["Table"].Rows.Clear();
                if (res != null && res.Count > 0)
                {
                    foreach (EFClientTools.EFServerReference.SYS_LANGUAGE item in res)
                    {
                        DataRow row = dataSet.Tables["Table"].NewRow();
                        //row["ID"] = item.ID;
                        row["IDENTIFICATION"] = item.IDENTIFICATION;
                        row["KEYS"] = item.KEYS;
                        row["EN"] = item.EN;
                        row["CHT"] = item.CHT;
                        row["CHS"] = item.CHS;
                        row["HK"] = item.HK;
                        row["JA"] = item.JA;
                        row["KO"] = item.KO;
                        row["LAN1"] = item.LAN1;
                        row["LAN2"] = item.LAN2;
                        dataSet.Tables["Table"].Rows.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;
        }

        private void WriteToDataBase()
        {
            this.buttonWriteDB.Enabled = false;
            EFClientTools.DesignClientUtility.ClientInfo.Database = (EditingControl as JQMultiLanguage).DBAlias;
            EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
            foreach (DataRow row in dataSet.Tables["Table"].Rows)
            {
                List<object> SYS_LANGUAGEs = new List<object>();
                EFClientTools.EFServerReference.SYS_LANGUAGE aSYS_LANGUAGE = new EFClientTools.EFServerReference.SYS_LANGUAGE();
                //if (row["ID"] != null)
                //    aSYS_LANGUAGE.ID = (int)row["ID"];
                if (row["IDENTIFICATION"] != null)
                    aSYS_LANGUAGE.IDENTIFICATION = row["IDENTIFICATION"].ToString();
                if (row["KEYS"] != null)
                    aSYS_LANGUAGE.KEYS = row["KEYS"].ToString();
                if (row["EN"] != null)
                    aSYS_LANGUAGE.EN = row["EN"].ToString();
                if (row["CHT"] != null)
                    aSYS_LANGUAGE.CHT = row["CHT"].ToString();
                if (row["CHS"] != null)
                    aSYS_LANGUAGE.CHS = row["CHS"].ToString();
                if (row["HK"] != null)
                    aSYS_LANGUAGE.HK = row["HK"].ToString();
                if (row["JA"] != null)
                    aSYS_LANGUAGE.JA = row["JA"].ToString();
                if (row["KO"] != null)
                    aSYS_LANGUAGE.KO = row["KO"].ToString();
                if (row["LAN1"] != null)
                    aSYS_LANGUAGE.LAN1 = row["LAN1"].ToString();
                if (row["LAN2"] != null)
                    aSYS_LANGUAGE.LAN2 = row["LAN2"].ToString();
                SYS_LANGUAGEs.Add(aSYS_LANGUAGE);
                EFClientTools.DesignClientUtility.SaveDataToTable(SYS_LANGUAGEs, "SYS_LANGUAGE");
            }
            EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;
            this.buttonWriteDB.Enabled = true;
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
            catch (Exception e)
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