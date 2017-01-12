using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Data;

namespace JQClientTools
{
    [Designer(typeof(JQMultiLanguageEditor), typeof(IDesigner))]
    public class JQMultiLanguage : WebControl
    {
        public JQMultiLanguage()
        {

        }

        private bool _Active;
        [Category("Infolight"),
        Description("Indicates whether WebMultiLanguage is enabled or disabled")]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
            }
        }

        private bool _ReadFromTable;
        [Category("Infolight")]
        public bool ReadFromTable
        {
            get
            {
                if (EFClientTools.ClientUtility.ClientInfo != null && !String.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.SDDeveloperID))
                    return true;
                return _ReadFromTable;
            }
            set
            {
                _ReadFromTable = value;
            }
        }

        private LanguageGroups _GroupIndex;
        [Category("Infolight"),
        Description("Specifies the current language")]
        public LanguageGroups GroupIndex
        {
            get
            {
                return _GroupIndex;
            }
            set
            {
                _GroupIndex = value;
            }
        }

        private string _DBAlias;
        [Category("Infolight"),
        Description("Specifies DataBase storing the data of WebMultiLanguage")]
        [Editor(typeof(JQGetAlias), typeof(System.Drawing.Design.UITypeEditor))]
        public string DBAlias
        {
            get
            {
                return _DBAlias;
            }
            set
            {
                _DBAlias = value;
            }
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="SetDefault">是否设置成默认语言</param>
        public void SetLanguage(bool SetDefault)
        {
            if (SetDefault)
            {
                SetDefaultGroup();
            }
            else
            {
                switch (this.GroupIndex)
                {
                    case LanguageGroups.English:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "en-us";
                        break;
                    case LanguageGroups.ChineseTra:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "zh-tw";
                        break;
                    case LanguageGroups.ChineseSim:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "zh-cn";
                        break;
                    case LanguageGroups.ChineseHK:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "zh-hk";
                        break;
                    case LanguageGroups.Japanese:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "ja-jp";
                        break;
                    case LanguageGroups.Korean:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "ko-kr";
                        break;
                    case LanguageGroups.LANG1:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "lan1";
                        break;
                    case LanguageGroups.LANG2:
                        EFClientTools.ClientUtility.ClientInfo.Locale = "lan2";
                        break;
                }
            }
            if (this.Page.Master == null)
            {
                SetLanguage(this.Page.Form.Controls);
                string title = GetValue("This.Title");
                if (title != null)
                {
                    this.Page.Title = title;
                }
            }
            else
            {
                SetLanguage(this.Parent.Controls);
            }
        }

        private void SetLanguage(ControlCollection collection)
        {
            foreach (Control ct in collection)
            {
                if (!string.IsNullOrEmpty(ct.ID))
                {
                    if (ct is JQDataForm || ct is JQDataGrid || ct is JQDialog || ct is JQValidate ||
                        ct is JQCheckBox || ct is JQComboGrid || ct is JQRefval || ct is JQOptions)
                    {
                        ApplyColumnCaption(ct, ct.ID);
                        //SetCollectionValue(ct, ct.ID, "Columns", "Caption", "Caption");
                    }
                }

                if (ct.Controls.Count > 0)
                {
                    SetLanguage(ct.Controls);
                }
                if (!string.IsNullOrEmpty(ct.ID))
                {
                    ApplyResources(ct, ct.ID);
                }
            }
        }

        private XmlNode GetLanguageNode()
        {
            string xmlfile = this.Page.Request.PhysicalPath + ".xml";

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlfile);
            XmlNode nodelanguage = null;
            switch (this.GroupIndex)
            {
                case LanguageGroups.English: nodelanguage = xml.DocumentElement.SelectSingleNode("EN"); break;
                case LanguageGroups.ChineseTra: nodelanguage = xml.DocumentElement.SelectSingleNode("CHT"); break;
                case LanguageGroups.ChineseSim: nodelanguage = xml.DocumentElement.SelectSingleNode("CHS"); break;
                case LanguageGroups.ChineseHK: nodelanguage = xml.DocumentElement.SelectSingleNode("HK"); break;
                case LanguageGroups.Japanese: nodelanguage = xml.DocumentElement.SelectSingleNode("JA"); break;
                case LanguageGroups.Korean: nodelanguage = xml.DocumentElement.SelectSingleNode("KO"); break;
                case LanguageGroups.LANG1: nodelanguage = xml.DocumentElement.SelectSingleNode("LAN1"); break;
                case LanguageGroups.LANG2: nodelanguage = xml.DocumentElement.SelectSingleNode("LAN2"); break;
            }
            return nodelanguage;
        }

        public string GetValue(string key)
        {
            if (ReadFromTable)
            {
                DataSet dataSet = ReadFromDataBase();
                DataRow[] drs = dataSet.Tables[0].Select("KEYS='" + key + "'");
                if (drs.Length > 0)
                {
                    string strfield = drs[0]["KEYS"].ToString();
                    string language = "EN";
                    switch (this.GroupIndex)
                    {
                        case LanguageGroups.English: language = "EN"; break;
                        case LanguageGroups.ChineseTra: language = "CHT"; break;
                        case LanguageGroups.ChineseSim: language = "CHS"; break;
                        case LanguageGroups.ChineseHK: language = "HK"; break;
                        case LanguageGroups.Japanese: language = "JA"; break;
                        case LanguageGroups.Korean: language = "KO"; break;
                        case LanguageGroups.LANG1: language = "LAN1"; break;
                        case LanguageGroups.LANG2: language = "LAN2"; break;
                    }
                    return drs[0][language].ToString();
                }
            }
            else
            {
                XmlNode nodelanguage = GetLanguageNode();
                if (nodelanguage.ChildNodes.Count > 0)
                {
                    XmlNode nodevalue = nodelanguage.SelectSingleNode(string.Format("Language[@field='{0}']", key));
                    if (nodevalue != null && nodevalue.Attributes["value"] != null)
                    {
                        return nodevalue.Attributes["value"].Value;
                    }
                }
            }
            return null;
        }

        private void ApplyResources(object value, string objectName)
        {
            if (value == null || objectName == null)
            {
                return;
            }
            BindingFlags flags = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;

            if (ReadFromTable)
            {
                DataSet dataSet = ReadFromDataBase();
                if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    for (var i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        string strfield = dataSet.Tables[0].Rows[i]["KEYS"].ToString();
                        string language = "EN";
                        switch (this.GroupIndex)
                        {
                            case LanguageGroups.English: language = "EN"; break;
                            case LanguageGroups.ChineseTra: language = "CHT"; break;
                            case LanguageGroups.ChineseSim: language = "CHS"; break;
                            case LanguageGroups.ChineseHK: language = "HK"; break;
                            case LanguageGroups.Japanese: language = "JA"; break;
                            case LanguageGroups.Korean: language = "KO"; break;
                            case LanguageGroups.LANG1: language = "LAN1"; break;
                            case LanguageGroups.LANG2: language = "LAN2"; break;
                        }
                        string strvalue = dataSet.Tables[0].Rows[i][language].ToString();
                        if (string.CompareOrdinal(strfield, 0, objectName, 0, objectName.Length) == 0)
                        {
                            if (strfield.Length >= objectName.Length && strfield[objectName.Length] == '.')
                            {
                                string strfield2 = strfield.Substring(objectName.Length + 1);

                                PropertyInfo info = null;
                                try
                                {
                                    info = value.GetType().GetProperty(strfield2, flags);
                                }
                                catch (AmbiguousMatchException)
                                {
                                    Type type1 = value.GetType();
                                    do
                                    {
                                        info = type1.GetProperty(strfield2, flags | BindingFlags.DeclaredOnly);
                                        type1 = type1.BaseType;
                                        if ((info != null) || (type1 == null))
                                        {
                                            break;
                                        }
                                    }
                                    while (type1 != typeof(object));
                                }
                                if (((info != null) && info.CanWrite) && info.PropertyType == typeof(string))
                                {
                                    info.SetValue(value, strvalue, null);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                XmlNode nodelanguage = GetLanguageNode();
                if (nodelanguage != null && nodelanguage.ChildNodes.Count > 0)
                {
                    XmlNode nodefield = nodelanguage.FirstChild;
                    while (nodefield != null)
                    {
                        string strfield = nodefield.Attributes["field"].Value;
                        string strvalue = nodefield.Attributes["value"].Value;
                        if (string.CompareOrdinal(strfield, 0, objectName, 0, objectName.Length) == 0)
                        {
                            if (strfield.Length >= objectName.Length && strfield[objectName.Length] == '.')
                            {
                                string strfield2 = strfield.Substring(objectName.Length + 1);

                                PropertyInfo info = null;
                                try
                                {
                                    info = value.GetType().GetProperty(strfield2, flags);
                                }
                                catch (AmbiguousMatchException)
                                {
                                    Type type1 = value.GetType();
                                    do
                                    {
                                        info = type1.GetProperty(strfield2, flags | BindingFlags.DeclaredOnly);
                                        type1 = type1.BaseType;
                                        if ((info != null) || (type1 == null))
                                        {
                                            break;
                                        }
                                    }
                                    while (type1 != typeof(object));
                                }
                                if (((info != null) && info.CanWrite) && info.PropertyType == typeof(string))
                                {
                                    info.SetValue(value, strvalue, null);
                                }
                            }
                        }
                        nodefield = nodefield.NextSibling;
                    }
                }
            }
        }

        private void ApplyColumnCaption(object value, string objectName)
        {
            if (value == null || objectName == null)
            {
                return;
            }

            if (ReadFromTable)
            {
                DataSet dataSet = ReadFromDataBase();

                if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    for (var i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        string strfield = dataSet.Tables[0].Rows[i]["KEYS"].ToString();
                        string language = "EN";
                        switch (this.GroupIndex)
                        {
                            case LanguageGroups.English: language = "EN"; break;
                            case LanguageGroups.ChineseTra: language = "CHT"; break;
                            case LanguageGroups.ChineseSim: language = "CHS"; break;
                            case LanguageGroups.ChineseHK: language = "HK"; break;
                            case LanguageGroups.Japanese: language = "JA"; break;
                            case LanguageGroups.Korean: language = "KO"; break;
                            case LanguageGroups.LANG1: language = "LAN1"; break;
                            case LanguageGroups.LANG2: language = "LAN2"; break;
                        }
                        string strvalue = dataSet.Tables[0].Rows[i][language].ToString();
                        if (string.CompareOrdinal(strfield, 0, objectName, 0, objectName.Length) == 0)
                        {
                            if (strfield.Length >= objectName.Length && strfield[objectName.Length] == '.')
                            {
                                string strfield2 = strfield.Substring(objectName.Length + 1);
                                if (value is JQDataGrid)
                                {
                                    foreach (JQGridColumn dc in ((JQDataGrid)value).Columns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".Caption" == strfield)
                                        {
                                            dc.Caption = strvalue;
                                            break;
                                        }

                                        if (objectName + "." + dc.FieldName + ".PlaceHolder" == strfield)
                                        {
                                            dc.PlaceHolder = strvalue;
                                            break;
                                        }

                                        var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (strfield.StartsWith(objectName + "." + dc.FieldName) && keys.Length > 3)
                                        {
                                            switch (dc.Editor)
                                            {
                                                case "checkbox":
                                                    if (keys[3] == "Format")
                                                        dc.Format = strvalue;
                                                    break;
                                                case "infocombogrid":
                                                    JQComboGrid jcg = new JQComboGrid();
                                                    jcg.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jcg.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jcg.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jcg.Columns.Count; j++)
                                                        {
                                                            if (jcg.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jcg.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "inforefval":
                                                    JQRefval jr = new JQRefval();
                                                    jr.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jr.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jr.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jr.Columns.Count; j++)
                                                        {
                                                            if (jr.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "infooptions":
                                                    JQOptions jo = new JQOptions();
                                                    jo.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jo.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    foreach (JQToolItem dc in ((JQDataGrid)value).TooItems)
                                    {
                                        if (objectName + "." + dc.ID + ".ToolItemText" == strfield)
                                        {
                                            dc.Text = strvalue;
                                            foreach (DataRow drRow in dataSet.Tables[0].Rows)
                                            {
                                                if (drRow["KEYS"].ToString() == objectName + "." + dc.Icon + ".ToolItemText")
                                                {
                                                    drRow[language] = "ALREADY SET BY ID";
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        if (objectName + "." + dc.Icon + ".ToolItemText" == strfield && strvalue != "ALREADY SET BY ID")
                                        {
                                            dc.Text = strvalue;
                                            break;
                                        }
                                    }
                                    foreach (JQQueryColumn dc in ((JQDataGrid)value).QueryColumns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".QueryColumnCaption" + dc.Condition == strfield)
                                        {
                                            dc.Caption = strvalue;
                                            break;
                                        }
                                        var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (strfield.StartsWith(objectName + "." + dc.FieldName + ".QueryColumn") && keys.Length > 4)
                                        {
                                            switch (dc.Editor)
                                            {
                                                case "checkbox":
                                                    if (keys[4] == "Format")
                                                        dc.Format = strvalue;
                                                    break;
                                                case "infocombogrid":
                                                    JQComboGrid jcg = new JQComboGrid();
                                                    jcg.LoadProperties(dc.EditorOptions);
                                                    if (keys[4] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jcg.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jcg.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "Caption")
                                                    {
                                                        for (int j = 0; j < jcg.Columns.Count; j++)
                                                        {
                                                            if (jcg.Columns[j].FieldName == keys[5])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jcg.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "inforefval":
                                                    JQRefval jr = new JQRefval();
                                                    jr.LoadProperties(dc.EditorOptions);
                                                    if (keys[4] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jr.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jr.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    else if (keys[4] == "Caption")
                                                    {
                                                        for (int j = 0; j < jr.Columns.Count; j++)
                                                        {
                                                            if (jr.Columns[j].FieldName == keys[5])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "infooptions":
                                                    JQOptions jo = new JQOptions();
                                                    jo.LoadProperties(dc.EditorOptions);
                                                    if (keys[4] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jo.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    break;
                                            }
                                            break;
                                        }
                                    }

                                    if (objectName + "." + "Title" == strfield)
                                    {
                                        (value as JQDataGrid).Title = strvalue;
                                    }
                                    else if (objectName + "." + "QueryTitle" == strfield)
                                    {
                                        (value as JQDataGrid).QueryTitle = strvalue;
                                    }
                                    else if (objectName + "." + "TotalCaption" == strfield)
                                    {
                                        (value as JQDataGrid).TotalCaption = strvalue;
                                    }
                                }
                                else if (value is JQDataForm)
                                {
                                    foreach (JQFormColumn dc in ((JQDataForm)value).Columns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".Caption" == strfield)
                                        {
                                            dc.Caption = strvalue;
                                            break;
                                        }
                                        if (objectName + "." + dc.FieldName + ".PlaceHolder" == strfield)
                                        {
                                            dc.PlaceHolder = strvalue;
                                            break;
                                        }
                                        var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (strfield.StartsWith(objectName + "." + dc.FieldName) && keys.Length > 3)
                                        {
                                            switch (dc.Editor)
                                            {
                                                case "checkbox":
                                                    if (keys[3] == "Format")
                                                        dc.Format = strvalue;
                                                    break;
                                                case "infocombogrid":
                                                    JQComboGrid jcg = new JQComboGrid();
                                                    jcg.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jcg.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jcg.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jcg.Columns.Count; j++)
                                                        {
                                                            if (jcg.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jcg.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "inforefval":
                                                    JQRefval jr = new JQRefval();
                                                    jr.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jr.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jr.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jr.Columns.Count; j++)
                                                        {
                                                            if (jr.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "infooptions":
                                                    JQOptions jo = new JQOptions();
                                                    jo.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jo.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    foreach (JQToolItem dc in ((JQDataForm)value).TooItems)
                                    {
                                        if (objectName + "." + dc.ID + ".ToolItemText" == strfield)
                                        {
                                            dc.Text = strvalue;
                                            foreach (DataRow drRow in dataSet.Tables[0].Rows)
                                            {
                                                if (drRow["KEYS"].ToString() == objectName + "." + dc.Icon + ".ToolItemText")
                                                {
                                                    drRow[language] = "ALREADY SET BY ID";
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        if (objectName + "." + dc.Icon + ".ToolItemText" == strfield && strvalue != "ALREADY SET BY ID")
                                        {
                                            dc.Text = strvalue;
                                            break;
                                        }
                                    }
                                }
                                else if (value is JQValidate)
                                {
                                    foreach (JQValidateColumn dc in ((JQValidate)value).Columns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".ValidateMessage" == strfield)
                                        {
                                            dc.ValidateMessage = ReplaceSpecialCharacters(strvalue);
                                            break;
                                        }
                                    }
                                }
                                else if (value is JQDialog)
                                {
                                    if (objectName + "." + "Title" == strfield)
                                    {
                                        (value as JQDialog).Title = strvalue;
                                    }
                                }
                                else if (value is JQCheckBox)
                                {
                                    if (objectName + ".checkbox.On" == strfield)
                                        (value as JQCheckBox).On = strvalue;
                                    if (objectName + ".checkbox.Off" == strfield)
                                        (value as JQCheckBox).Off = strvalue;
                                }
                                else if (value is JQComboGrid)
                                {
                                    if (objectName + ".infocombogrid.textFieldCaption" == strfield)
                                        (value as JQComboGrid).DisplayMemberCaption = strvalue;
                                    if (objectName + ".infocombogrid.valueFieldCaption" == strfield)
                                        (value as JQComboGrid).ValueMemberCaption = strvalue;
                                    if (strfield.StartsWith(objectName + ".infocombogrid.Caption"))
                                    {
                                        for (int j = 0; j < (value as JQComboGrid).Columns.Count; j++)
                                        {
                                            if (strfield.Split('.')[3] == (value as JQComboGrid).Columns[j].FieldName)
                                            {
                                                (value as JQComboGrid).Columns[j].Caption = strvalue;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (value is JQRefval)
                                {
                                    if (objectName + ".inforefval.title" == strfield)
                                        (value as JQRefval).DialogTitle = strvalue;
                                    if (objectName + ".inforefval.textFieldCaption" == strfield)
                                        (value as JQRefval).DisplayMemberCaption = strvalue;
                                    if (objectName + ".inforefval.valueFieldCaption" == strfield)
                                        (value as JQRefval).ValueMemberCaption = strvalue;
                                    if (strfield.StartsWith(objectName + ".inforefval.Caption"))
                                    {
                                        for (int j = 0; j < (value as JQRefval).Columns.Count; j++)
                                        {
                                            if (strfield.Split('.')[3] == (value as JQRefval).Columns[j].FieldName)
                                            {
                                                (value as JQRefval).Columns[j].Caption = strvalue;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (value is JQOptions)
                                {
                                    if (objectName + ".infooptions.title" == strfield)
                                        (value as JQOptions).DialogTitle = strvalue;
                                }
                                else if (value is JQTab)
                                {
                                    //if (objectName + "." + "Title" == strfield)
                                    //{
                                    //    (value as JQTab).Title = strvalue;
                                    //}
                                }
                                else if (value is JQTabItem)
                                {
                                    if (objectName + "." + "Title" == strfield)
                                    {
                                        (value as JQTabItem).Title = strvalue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                XmlNode nodelanguage = GetLanguageNode();

                if (nodelanguage != null && nodelanguage.ChildNodes.Count > 0)
                {
                    XmlNode nodefield = nodelanguage.FirstChild;
                    while (nodefield != null)
                    {
                        string strfield = nodefield.Attributes["field"].Value;
                        string strvalue = nodefield.Attributes["value"].Value;
                        if (string.CompareOrdinal(strfield, 0, objectName, 0, objectName.Length) == 0)
                        {
                            if (strfield.Length >= objectName.Length && strfield[objectName.Length] == '.')
                            {
                                string strfield2 = strfield.Substring(objectName.Length + 1);
                                if (value is JQDataGrid)
                                {
                                    foreach (JQGridColumn dc in ((JQDataGrid)value).Columns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".Caption" == strfield)
                                        {
                                            dc.Caption = strvalue;
                                            break;
                                        }

                                        if (objectName + "." + dc.FieldName + ".PlaceHolder" == strfield)
                                        {
                                            dc.PlaceHolder = strvalue;
                                            break;
                                        }

                                        var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (strfield.StartsWith(objectName + "." + dc.FieldName) && keys.Length > 3)
                                        {
                                            switch (dc.Editor)
                                            {
                                                case "checkbox":
                                                    if (keys[3] == "Format")
                                                        dc.Format = strvalue;
                                                    break;
                                                case "infocombogrid":
                                                    JQComboGrid jcg = new JQComboGrid();
                                                    jcg.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jcg.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jcg.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jcg.Columns.Count; j++)
                                                        {
                                                            if (jcg.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jcg.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "inforefval":
                                                    JQRefval jr = new JQRefval();
                                                    jr.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jr.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jr.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jr.Columns.Count; j++)
                                                        {
                                                            if (jr.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "infooptions":
                                                    JQOptions jo = new JQOptions();
                                                    jo.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jo.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    foreach (JQToolItem dc in ((JQDataGrid)value).TooItems)
                                    {
                                        if (objectName + "." + dc.ID + ".ToolItemText" == strfield)
                                        {
                                            dc.Text = strvalue;

                                            XmlNode nodefieldForIcon = nodelanguage.FirstChild;
                                            while (nodefieldForIcon != null)
                                            {
                                                if (nodefieldForIcon.Attributes["field"].Value == objectName + "." + dc.Icon + ".ToolItemText")
                                                {
                                                    nodefieldForIcon.Attributes["value"].Value = "ALREADY SET BY ID";
                                                    break;
                                                }
                                                nodefieldForIcon = nodefieldForIcon.NextSibling;
                                            }
                                            break;
                                        }
                                        if (objectName + "." + dc.Icon + ".ToolItemText" == strfield && strvalue != "ALREADY SET BY ID")
                                        {
                                            dc.Text = strvalue;
                                            break;
                                        }
                                    }
                                    foreach (JQQueryColumn dc in ((JQDataGrid)value).QueryColumns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".QueryColumnCaption" + dc.Condition == strfield)
                                        {
                                            dc.Caption = strvalue;
                                            break;
                                        }
                                        var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (strfield.StartsWith(objectName + "." + dc.FieldName + ".QueryColumn") && keys.Length > 4)
                                        {
                                            switch (dc.Editor)
                                            {
                                                case "checkbox":
                                                    if (keys[4] == "Format")
                                                        dc.Format = strvalue;
                                                    break;
                                                case "infocombogrid":
                                                    JQComboGrid jcg = new JQComboGrid();
                                                    jcg.LoadProperties(dc.EditorOptions);
                                                    if (keys[4] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jcg.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jcg.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "Caption")
                                                    {
                                                        for (int j = 0; j < jcg.Columns.Count; j++)
                                                        {
                                                            if (jcg.Columns[j].FieldName == keys[5])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jcg.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "inforefval":
                                                    JQRefval jr = new JQRefval();
                                                    jr.LoadProperties(dc.EditorOptions);
                                                    if (keys[4] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jr.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jr.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[4] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    else if (keys[4] == "Caption")
                                                    {
                                                        for (int j = 0; j < jr.Columns.Count; j++)
                                                        {
                                                            if (jr.Columns[j].FieldName == keys[5])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "infooptions":
                                                    JQOptions jo = new JQOptions();
                                                    jo.LoadProperties(dc.EditorOptions);
                                                    if (keys[4] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jo.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    break;
                                            }
                                            break;
                                        }
                                    }

                                    if (objectName + "." + "Title" == strfield)
                                    {
                                        (value as JQDataGrid).Title = strvalue;
                                    }
                                    else if (objectName + "." + "QueryTitle" == strfield)
                                    {
                                        (value as JQDataGrid).QueryTitle = strvalue;
                                    }
                                    else if (objectName + "." + "TotalCaption" == strfield)
                                    {
                                        (value as JQDataGrid).TotalCaption = strvalue;
                                    }
                                }
                                else if (value is JQDataForm)
                                {
                                    foreach (JQFormColumn dc in ((JQDataForm)value).Columns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".Caption" == strfield)
                                        {
                                            dc.Caption = strvalue;
                                            break;
                                        }

                                        if (objectName + "." + dc.FieldName + ".PlaceHolder" == strfield)
                                        {
                                            dc.PlaceHolder = strvalue;
                                            break;
                                        }

                                        var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (strfield.StartsWith(objectName + "." + dc.FieldName) && keys.Length > 3)
                                        {
                                            switch (dc.Editor)
                                            {
                                                case "checkbox":
                                                    if (keys[3] == "Format")
                                                        dc.Format = strvalue;
                                                    break;
                                                case "infocombogrid":
                                                    JQComboGrid jcg = new JQComboGrid();
                                                    jcg.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jcg.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jcg.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jcg.Columns.Count; j++)
                                                        {
                                                            if (jcg.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jcg.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "inforefval":
                                                    JQRefval jr = new JQRefval();
                                                    jr.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "textFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("textFieldCaption:'{0}'", jr.DisplayMemberCaption), String.Format("textFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "valueFieldCaption")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("valueFieldCaption:'{0}'", jr.ValueMemberCaption), String.Format("valueFieldCaption:'{0}'", strvalue));
                                                    else if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    else if (keys[3] == "Caption")
                                                    {
                                                        for (int j = 0; j < jr.Columns.Count; j++)
                                                        {
                                                            if (jr.Columns[j].FieldName == keys[4])
                                                                dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jr.Columns[j].Caption), String.Format("title:'{0}'", strvalue));
                                                        }
                                                    }
                                                    break;
                                                case "infooptions":
                                                    JQOptions jo = new JQOptions();
                                                    jo.LoadProperties(dc.EditorOptions);
                                                    if (keys[3] == "title")
                                                        dc.EditorOptions = dc.EditorOptions.Replace(String.Format("title:'{0}'", jo.DialogTitle), String.Format("title:'{0}'", strvalue));
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    foreach (JQToolItem dc in ((JQDataForm)value).TooItems)
                                    {
                                        if (objectName + "." + dc.ID + ".ToolItemText" == strfield)
                                        {
                                            dc.Text = strvalue;

                                            XmlNode nodefieldForIcon = nodelanguage.FirstChild;
                                            while (nodefieldForIcon != null)
                                            {
                                                if (nodefieldForIcon.Attributes["field"].Value == objectName + "." + dc.Icon + ".ToolItemText")
                                                {
                                                    nodefieldForIcon.Attributes["value"].Value = "ALREADY SET BY ID";
                                                    break;
                                                }
                                                nodefieldForIcon = nodefieldForIcon.NextSibling;
                                            }
                                            break;
                                        }
                                        if (objectName + "." + dc.Icon + ".ToolItemText" == strfield && strvalue != "ALREADY SET BY ID")
                                        {
                                            dc.Text = strvalue;
                                            break;
                                        }
                                    }
                                }
                                else if (value is JQValidate)
                                {
                                    foreach (JQValidateColumn dc in ((JQValidate)value).Columns)
                                    {
                                        if (objectName + "." + dc.FieldName + ".ValidateMessage" == strfield)
                                        {
                                            dc.ValidateMessage = ReplaceSpecialCharacters(strvalue);
                                            break;
                                        }
                                    }
                                }
                                else if (value is JQDialog)
                                {
                                    if (objectName + "." + "Title" == strfield)
                                    {
                                        (value as JQDialog).Title = strvalue;
                                    }
                                }
                                else if (value is JQCheckBox)
                                {
                                    if (objectName + ".checkbox.On" == strfield)
                                        (value as JQCheckBox).On = strvalue;
                                    if (objectName + ".checkbox.Off" == strfield)
                                        (value as JQCheckBox).Off = strvalue;
                                }
                                else if (value is JQComboGrid)
                                {
                                    if (objectName + ".infocombogrid.textFieldCaption" == strfield)
                                        (value as JQComboGrid).DisplayMemberCaption = strvalue;
                                    if (objectName + ".infocombogrid.valueFieldCaption" == strfield)
                                        (value as JQComboGrid).ValueMemberCaption = strvalue;
                                    if (strfield.StartsWith(objectName + ".infocombogrid.Caption"))
                                    {
                                        for (int j = 0; j < (value as JQComboGrid).Columns.Count; j++)
                                        {
                                            if (strfield.Split('.')[3] == (value as JQComboGrid).Columns[j].FieldName)
                                            {
                                                (value as JQComboGrid).Columns[j].Caption = strvalue;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (value is JQRefval)
                                {
                                    if (objectName + ".inforefval.title" == strfield)
                                        (value as JQRefval).DialogTitle = strvalue;
                                    if (objectName + ".inforefval.textFieldCaption" == strfield)
                                        (value as JQRefval).DisplayMemberCaption = strvalue;
                                    if (objectName + ".inforefval.valueFieldCaption" == strfield)
                                        (value as JQRefval).ValueMemberCaption = strvalue;
                                    if (strfield.StartsWith(objectName + ".inforefval.Caption"))
                                    {
                                        for (int j = 0; j < (value as JQRefval).Columns.Count; j++)
                                        {
                                            if (strfield.Split('.')[3] == (value as JQRefval).Columns[j].FieldName)
                                            {
                                                (value as JQRefval).Columns[j].Caption = strvalue;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (value is JQOptions)
                                {
                                    if (objectName + ".infooptions.title" == strfield)
                                        (value as JQOptions).DialogTitle = strvalue;
                                }
                            }
                        }
                        nodefield = nodefield.NextSibling;
                    }
                }
            }
        }

        private String ReplaceSpecialCharacters(String oldStr)
        {
            String newStr = oldStr.Replace("\'", "\\\\'");
            return newStr;
        }

        private DataSet ReadFromDataBase()
        {
            DataSet res = null;
            try
            {
                //string XMLFile = string.Format("{0}.xml", EFClientTools.Design.DTE.ActiveDocumentFullName);
                string id = "";
                if (!String.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.SDDeveloperID))
                {
                    string formName = this.Page.Request.FilePath;
                    formName = formName.Substring(formName.LastIndexOf("/"));
                    formName = formName.Split('.')[0].Split('_')[2];
                    id = EFClientTools.ClientUtility.ClientInfo.Solution + "_" + formName;
                }
                else
                {
                    string XMLFile = string.Format("{0}.xml", this.Page.Request.PhysicalPath);
                    string strid = XMLFile + this.ID;
                    id = strid.GetHashCode().ToString();
                }
                res = EFClientTools.ClientUtility.ExecuteSQL(this.DBAlias, String.Format("SELECT * FROM SYS_LANGUAGE WHERE IDENTIFICATION='{0}'", id));
            }
            catch (Exception e)
            {

            }
            return res;
        }

        public void SetDefaultGroup()
        {
            String language = EFClientTools.ClientUtility.ClientInfo.Locale.ToLower();

            switch (language)
            {
                case "en-us":
                    this.GroupIndex = LanguageGroups.English;
                    break;
                case "zh-tw":
                    this.GroupIndex = LanguageGroups.ChineseTra;
                    break;
                case "zh-cn":
                    this.GroupIndex = LanguageGroups.ChineseSim;
                    break;
                case "zh-hk":
                    this.GroupIndex = LanguageGroups.ChineseHK;
                    break;
                case "ja-jp":
                    this.GroupIndex = LanguageGroups.Japanese;
                    break;
                case "ko-kr":
                    this.GroupIndex = LanguageGroups.Korean;
                    break;
                case "lan1":
                    this.GroupIndex = LanguageGroups.LANG1;
                    break;
                case "lan2":
                    this.GroupIndex = LanguageGroups.LANG2;
                    break;
            }
        }

        internal List<DictionaryEntry> GetControlValues()
        {
            List<DictionaryEntry> list = GetControlValues(Page.Controls);
            list.Add(new DictionaryEntry("This.Title", "Untitled Page"));
            return list;
        }

        private List<DictionaryEntry> GetControlValues(ControlCollection collection)
        {
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            if (this.DesignMode)
            {
                foreach (Control ct in collection)
                {
                    PropertyInfo[] pis = ct.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (pis[i].PropertyType == typeof(string))
                        {
                            if (string.Compare(pis[i].Name, "Text", true) == 0 || string.Compare(pis[i].Name, "ToolTip", true) == 0)
                            {
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}", ct.ID, pis[i].Name)
                                    , pis[i].GetValue(ct, null).ToString()));
                            }
                        }
                    }
                    if (ct is JQDataForm)
                    {
                        JQDataForm ctrl = ct as JQDataForm;
                        for (int i = 0; i < ctrl.Columns.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", ct.ID, ctrl.Columns[i].FieldName), new object[] { String.IsNullOrEmpty(ctrl.Columns[i].Caption) ? ctrl.Columns[i].FieldName : ctrl.Columns[i].Caption, ctrl.Columns[i].FieldName }));
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.PlaceHolder", ct.ID, ctrl.Columns[i].FieldName), ctrl.Columns[i].PlaceHolder));
                            switch (ctrl.Columns[i].Editor)
                            {
                                case "checkbox":
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.checkbox.Format", ct.ID, ctrl.Columns[i].FieldName), ctrl.Columns[i].Format));
                                    break;
                                case "infocombogrid":
                                    JQComboGrid jcg = new JQComboGrid();
                                    jcg.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.infocombogrid.textFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jcg.DisplayMemberCaption, jcg.DisplayMember }));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.infocombogrid.valueFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jcg.ValueMemberCaption, jcg.ValueMember }));
                                    for (int j = 0; j < jcg.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.infocombogrid.Caption.{2}", ct.ID, ctrl.Columns[i].FieldName, jcg.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jcg.Columns[j].Caption) ? jcg.Columns[j].FieldName : jcg.Columns[j].Caption, jcg.Columns[j].FieldName }));
                                    }
                                    break;
                                case "inforefval":
                                    JQRefval jr = new JQRefval();
                                    jr.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.title", ct.ID, ctrl.Columns[i].FieldName), jr.DialogTitle));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.textFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jr.DisplayMemberCaption, jr.DisplayMember }));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.valueFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jr.ValueMemberCaption, jr.DisplayMember }));
                                    for (int j = 0; j < jr.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.Caption.{2}", ct.ID, ctrl.Columns[i].FieldName, jr.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jr.Columns[j].Caption) ? jr.Columns[j].FieldName : jr.Columns[j].Caption, jr.Columns[j].FieldName }));
                                    }
                                    break;
                                case "infooptions":
                                    JQOptions jo = new JQOptions();
                                    jo.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.infooptions.title", ct.ID, ctrl.Columns[i].FieldName), jo.DialogTitle));
                                    break;
                            }
                        }

                        for (int i = 0; i < ctrl.TooItems.Count; i++)
                        {
                            if (!String.IsNullOrEmpty(ctrl.TooItems[i].ID))
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.ToolItemText", ct.ID, ctrl.TooItems[i].ID), ctrl.TooItems[i].Text));
                            else
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.ToolItemText", ct.ID, ctrl.TooItems[i].Icon), ctrl.TooItems[i].Text));
                        }
                    }
                    if (ct is JQDialog)
                    {
                        JQDialog ctrl = ct as JQDialog;
                        list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), ctrl.Title));
                    }
                    if (ct is JQDataGrid)
                    {
                        JQDataGrid ctrl = ct as JQDataGrid;
                        for (int i = 0; i < ctrl.Columns.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", ct.ID, ctrl.Columns[i].FieldName), new object[] { String.IsNullOrEmpty(ctrl.Columns[i].Caption) ? ctrl.Columns[i].FieldName : ctrl.Columns[i].Caption, ctrl.Columns[i].FieldName }));
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.PlaceHolder", ct.ID, ctrl.Columns[i].FieldName), ctrl.Columns[i].PlaceHolder));
                            switch (ctrl.Columns[i].Editor)
                            {
                                case "checkbox":
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.checkbox.Format", ct.ID, ctrl.Columns[i].FieldName), ctrl.Columns[i].Format));
                                    break;
                                case "infocombogrid":
                                    JQComboGrid jcg = new JQComboGrid();
                                    jcg.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.infocombogrid.textFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jcg.DisplayMemberCaption, jcg.DisplayMember }));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.infocombogrid.valueFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jcg.ValueMemberCaption, jcg.ValueMember }));
                                    for (int j = 0; j < jcg.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.infocombogrid.Caption.{2}", ct.ID, ctrl.Columns[i].FieldName, jcg.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jcg.Columns[j].Caption) ? jcg.Columns[j].FieldName : jcg.Columns[j].Caption, jcg.Columns[j].FieldName }));
                                    }
                                    break;
                                case "inforefval":
                                    JQRefval jr = new JQRefval();
                                    jr.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.title", ct.ID, ctrl.Columns[i].FieldName), jr.DialogTitle));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.textFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jr.DisplayMemberCaption, jr.DisplayMember }));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.valueFieldCaption", ct.ID, ctrl.Columns[i].FieldName), new object[] { jr.ValueMemberCaption, jr.ValueMember }));
                                    for (int j = 0; j < jr.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.inforefval.Caption.{2}", ct.ID, ctrl.Columns[i].FieldName, jr.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jr.Columns[j].Caption) ? jr.Columns[j].FieldName : jr.Columns[j].Caption, jr.Columns[j].FieldName }));
                                    }
                                    break;
                                case "infooptions":
                                    JQOptions jo = new JQOptions();
                                    jo.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.infooptions.title", ct.ID, ctrl.Columns[i].FieldName), jo.DialogTitle));
                                    break;
                            }
                        }
                        for (int i = 0; i < ctrl.TooItems.Count; i++)
                        {
                            if (!String.IsNullOrEmpty(ctrl.TooItems[i].ID))
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.ToolItemText", ct.ID, ctrl.TooItems[i].ID), ctrl.TooItems[i].Text));
                            else
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.ToolItemText", ct.ID, ctrl.TooItems[i].Icon), ctrl.TooItems[i].Text));
                        }
                        for (int i = 0; i < ctrl.QueryColumns.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumnCaption{2}", ct.ID, ctrl.QueryColumns[i].FieldName, ctrl.QueryColumns[i].Condition), new object[] { String.IsNullOrEmpty(ctrl.QueryColumns[i].Caption) ? ctrl.QueryColumns[i].FieldName : ctrl.QueryColumns[i].Caption, ctrl.QueryColumns[i].FieldName }));
                            switch (ctrl.QueryColumns[i].Editor)
                            {
                                case "checkbox":
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.checkbox.Format", ct.ID, ctrl.QueryColumns[i].FieldName), ctrl.QueryColumns[i].Format));
                                    break;
                                case "infocombogrid":
                                    JQComboGrid jcg = new JQComboGrid();
                                    jcg.LoadProperties(ctrl.QueryColumns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.infocombogrid.textFieldCaption", ct.ID, ctrl.QueryColumns[i].FieldName), new object[] { jcg.DisplayMemberCaption, jcg.DisplayMember }));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.infocombogrid.valueFieldCaption", ct.ID, ctrl.QueryColumns[i].FieldName), new object[] { jcg.ValueMemberCaption, jcg.ValueMember }));
                                    for (int j = 0; j < jcg.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.inforefval.Caption.{2}", ct.ID, ctrl.QueryColumns[i].FieldName, jcg.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jcg.Columns[j].Caption) ? jcg.Columns[j].FieldName : jcg.Columns[j].Caption, jcg.Columns[j].FieldName }));
                                    }
                                    break;
                                case "inforefval":
                                    JQRefval jr = new JQRefval();
                                    jr.LoadProperties(ctrl.QueryColumns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.inforefval.title", ct.ID, ctrl.QueryColumns[i].FieldName), jr.DialogTitle));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.inforefval.textFieldCaption", ct.ID, ctrl.QueryColumns[i].FieldName), new object[] { jr.DisplayMemberCaption, jr.DisplayMember }));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.inforefval.valueFieldCaption", ct.ID, ctrl.QueryColumns[i].FieldName), new object[] { jr.ValueMemberCaption, jr.ValueMember }));
                                    for (int j = 0; j < jr.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.inforefval.Caption.{2}", ct.ID, ctrl.QueryColumns[i].FieldName, jr.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jr.Columns[j].Caption) ? jr.Columns[j].FieldName : jr.Columns[j].Caption, jr.Columns[j].FieldName }));
                                    }
                                    break;
                                case "infooptions":
                                    JQOptions jo = new JQOptions();
                                    jo.LoadProperties(ctrl.QueryColumns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumn.infooptions.title", ct.ID, ctrl.QueryColumns[i].FieldName), jo.DialogTitle));
                                    break;
                            }
                        }
                        list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), ctrl.Title));
                        list.Add(new DictionaryEntry(string.Format("{0}.QueryTitle", ct.ID), ctrl.QueryTitle));
                        list.Add(new DictionaryEntry(string.Format("{0}.TotalCaption", ct.ID), ctrl.TotalCaption));
                    }
                    if (ct is JQValidate)
                    {
                        JQValidate ctrl = ct as JQValidate;
                        for (int i = 0; i < ctrl.Columns.Count; i++)
                        {
                            if (!String.IsNullOrEmpty(ctrl.Columns[i].CheckMethod))
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.ValidateMessage", ct.ID, ctrl.Columns[i].FieldName), ctrl.Columns[i].ValidateMessage));
                        }
                    }
                    if (ct is JQCheckBox)
                    {
                        list.Add(new DictionaryEntry(string.Format("{0}.checkbox.On", ct.ID), (ct as JQCheckBox).On));
                        list.Add(new DictionaryEntry(string.Format("{0}.checkbox.Off", ct.ID), (ct as JQCheckBox).Off));
                    }
                    if (ct is JQComboGrid)
                    {
                        JQComboGrid jcg = (JQComboGrid)ct;
                        list.Add(new DictionaryEntry(string.Format("{0}.infocombogrid.textFieldCaption", ct.ID), new object[] { jcg.DisplayMemberCaption, jcg.DisplayMember }));
                        list.Add(new DictionaryEntry(string.Format("{0}.infocombogrid.valueFieldCaption", ct.ID), new object[] { jcg.ValueMemberCaption, jcg.ValueMember }));
                        for (int j = 0; j < jcg.Columns.Count; j++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.infocombogrid.Caption.{1}", ct.ID, jcg.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jcg.Columns[j].Caption) ? jcg.Columns[j].FieldName : jcg.Columns[j].Caption, jcg.Columns[j].FieldName }));
                        }
                    }
                    if (ct is JQRefval)
                    {
                        JQRefval jr = (JQRefval)ct;
                        list.Add(new DictionaryEntry(string.Format("{0}.inforefval.title", ct.ID), jr.DialogTitle));
                        list.Add(new DictionaryEntry(string.Format("{0}.inforefval.textFieldCaption", ct.ID), new object[] { jr.DisplayMemberCaption, jr.DisplayMember }));
                        list.Add(new DictionaryEntry(string.Format("{0}.inforefval.valueFieldCaption", ct.ID), new object[] { jr.ValueMemberCaption, jr.ValueMember }));
                        for (int j = 0; j < jr.Columns.Count; j++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.inforefval.Caption.{1}", ct.ID, jr.Columns[j].FieldName), new object[] { String.IsNullOrEmpty(jr.Columns[j].Caption) ? jr.Columns[j].FieldName : jr.Columns[j].Caption, jr.Columns[j].FieldName }));
                        }
                    }
                    if (ct is JQOptions)
                    {
                        JQOptions jo = (JQOptions)ct;
                        list.Add(new DictionaryEntry(string.Format("{0}.infooptions.title", ct.ID), jo.DialogTitle));
                    }
                    if (ct is JQTab)
                    {
                        //list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), (ct as JQTab).Title));
                    }
                    if (ct is JQTabItem)
                    {
                        list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), (ct as JQTabItem).Title));
                    }

                    if (ct.Controls.Count > 0)
                    {
                        list.AddRange(GetControlValues(ct.Controls));
                    }
                }
            }
            return list;
        }
    }

    public class JQMultiLanguageEditor : DataSourceDesigner
    {
        private DesignerActionListCollection _actionLists;

        public JQMultiLanguageEditor()
        {
            DesignerVerb editVerb = new DesignerVerb("Edit", new EventHandler(OnEdit));
            this.Verbs.Add(editVerb);
        }

        public void OnEdit(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty((this.Component as JQMultiLanguage).DBAlias))
            {
                MultiLanguageEditorDialog form = new MultiLanguageEditorDialog(this.Component);
                form.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("the property of webmultilanguage hasn't be set correctly");
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new JQMultiLanguageActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class JQMultiLanguageActionList : DesignerActionList
    {
        private JQMultiLanguage wml;

        public JQMultiLanguageActionList(IComponent component)
            : base(component)
        {
            wml = component as JQMultiLanguage;
        }

        public void OnEdit()
        {
            MultiLanguageEditorDialog form = new MultiLanguageEditorDialog(wml);
            form.ShowDialog();
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnEdit", "Edit", "UserEdit", true));
            return items;
        }
    }

    public enum LanguageGroups
    { English, ChineseTra, ChineseSim, ChineseHK, Japanese, Korean, LANG1, LANG2 }
}
