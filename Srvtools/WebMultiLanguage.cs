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

namespace Srvtools
{
    [Designer(typeof(WebMultiLanguageEditor), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebMultiLanguage), "Resources.WebMultiLanguage.ico")]
    public class WebMultiLanguage : WebControl, IGetValues
    {
        public WebMultiLanguage()
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

        private Srvtools.MultiLanguage.LanguageGroups _GroupIndex;
        [Category("Infolight"),
        Description("Specifies the current language")]
        public Srvtools.MultiLanguage.LanguageGroups GroupIndex
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

        private string _DataBase;
        [Category("Infolight"),
        Description("Specifies DataBase storing the data of WebMultiLanguage")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DataBase
        {
            get
            {
                return _DataBase;
            }
            set
            {
                _DataBase = value;
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
                    if (ct is WebGridView || ct is WebDetailsView || ct is WebFormView)
                    {
                        ApplyViewHeadText(ct, ct.ID);
                    }
                    else if (ct is WebValidate)
                    {
                        SetCollectionValue(ct, ct.ID, "Fields", "WarningMsg", "Warningmsg");
                    }
                    else if (ct is WebClientQuery)
                    {
                        SetCollectionValue(ct, ct.ID, "Columns", "Caption", "Caption");
                    }
                    else if (ct is WebNavigator)
                    {
                        SetCollectionValue(ct, ct.ID, "QueryFields", "Caption", "Caption");
                    }
                    else if (ct is DropDownList || ct is RadioButtonList || ct is CheckBoxList)
                    {
                        SetCollectionValue(ct, ct.ID, "Items", null, "Item");
                    }
                    else if (ct is WebMultiViewCaptions && ct.ID != null)
                    {
                        SetCollectionValue(ct, ct.ID, "Captions", "Caption", "Caption");
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxGridView")
                    {
                        ApplyViewHeadText(ct, ct.ID);
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxFormView")
                    {
                        ApplyViewHeadText(ct, ct.ID);
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxLayout")
                    {
                        ApplyViewHeadText(ct, ct.ID);
                    }
                    else if (ct.GetType().Name == "WebEasilyReport")
                    {
                        SetEasilyReportValue(ct);
                    }
                    else
                    {
                        ApplyResources(ct, ct.ID);
                    } 
                }
                if (!(ct is WebGridView))
                {
                    SetLanguage(ct.Controls);
                }
            }
        }

        private void SetCollectionValue(object ct, string ctname, string collectionname, string propertyname, string xmlname)
        {
            string controlname = ctname.TrimStart('_');//trim for vb
            XmlNode nodelanguage = GetLanguageNode();
            if (nodelanguage != null && nodelanguage.ChildNodes.Count > 0)
            {
                XmlNode nodefield = nodelanguage.FirstChild;
                while (nodefield != null)
                {
                    string strfield = nodefield.Attributes["field"].Value;
                    string strvalue = nodefield.Attributes["value"].Value;
                    if (string.CompareOrdinal(strfield, 0, controlname + "." + xmlname, 0, controlname.Length + 1 + xmlname.Length) == 0)
                    {
                        if (strfield.Length > controlname.Length + 1 + xmlname.Length)
                        {
                            string strfield2 = strfield.Substring(controlname.Length + 1 + xmlname.Length);
                            try
                            {
                                int index = int.Parse(strfield2);
                                Type controltype = ct.GetType();
                                object collection = controltype.GetProperty(collectionname).GetValue(ct, null);
                                if (propertyname != null)
                                {
                                    object field = (collection as InfoOwnerCollection)[index];
                                    field.GetType().GetProperty(propertyname).SetValue(field, strvalue, null);
                                }
                                else if(collection is ListItemCollection)
                                {
                                    (collection as ListItemCollection)[index].Text = strvalue;
                                }
                            }
                            catch { }
                        }
                    }
                    nodefield = nodefield.NextSibling;
                }
            }
        }

        private void SetEasilyReportValue(object ct)
        {
            String controlname = (ct as WebControl).ID;
            String xmlname = String.Empty;
            String propertyname = String.Empty;
            String collectionname = String.Empty;
            XmlNode nodelanguage = GetLanguageNode();
            if (nodelanguage != null && nodelanguage.ChildNodes.Count > 0)
            {
                XmlNode nodefield = nodelanguage.FirstChild;
                while (nodefield != null)
                {
                    string strfield = nodefield.Attributes["field"].Value;
                    string strvalue = nodefield.Attributes["value"].Value;

                    Type type = ct.GetType(); 
                    if (strfield.Contains("HeaderItems"))
                    {
                        PropertyInfo infoHI = type.GetProperty("HeaderItems");
                        IList iHeaderItemsCollection = infoHI.GetValue(ct, null) as IList;
                        for (int i = 0; i < iHeaderItemsCollection.Count; i++)
                        {
                            if (iHeaderItemsCollection[i].GetType().GetProperty("Style") != null)
                            {
                                String strStyle = iHeaderItemsCollection[i].GetType().GetProperty("Style").GetValue(iHeaderItemsCollection[i], null).ToString();
                                xmlname = "HeaderItems." + strStyle + ".Format";
                                if (string.CompareOrdinal(strfield, 0, controlname + "." + xmlname, 0, controlname.Length + 1 + xmlname.Length) == 0)
                                {
                                    iHeaderItemsCollection[i].GetType().GetProperty("Format").SetValue(iHeaderItemsCollection[i], strvalue, null);
                                    break;
                                }
                            }
                            else if (iHeaderItemsCollection[i].GetType().GetProperty("ColumnName") != null)
                            {
                                String strColumnName = iHeaderItemsCollection[i].GetType().GetProperty("ColumnName").GetValue(iHeaderItemsCollection[i], null).ToString();
                                xmlname = "HeaderItems." + strColumnName + ".Format";
                                if (string.CompareOrdinal(strfield, 0, controlname + "." + xmlname, 0, controlname.Length + 1 + xmlname.Length) == 0)
                                {
                                    iHeaderItemsCollection[i].GetType().GetProperty("Format").SetValue(iHeaderItemsCollection[i], strvalue, null);
                                    break;
                                }
                            }
                        }
                    }
                    else if (strfield.Contains("FooterItems"))
                    {
                        PropertyInfo infoFI = type.GetProperty("FooterItems");
                        IList iFooterItemsCollection = infoFI.GetValue(ct, null) as IList;
                        for (int i = 0; i < iFooterItemsCollection.Count; i++)
                        {
                            String strStyle = String.Empty;
                            if (iFooterItemsCollection[i].GetType().GetProperty("Style") != null)
                                strStyle = iFooterItemsCollection[i].GetType().GetProperty("Style").GetValue(iFooterItemsCollection[i], null).ToString();
                            xmlname = "FooterItems." + strStyle + ".Format";
                            if (string.CompareOrdinal(strfield, 0, controlname + "." + xmlname, 0, controlname.Length + 1 + xmlname.Length) == 0)
                            {
                                iFooterItemsCollection[i].GetType().GetProperty("Format").SetValue(iFooterItemsCollection[i], strvalue, null);
                                break;
                            }
                        }
                    }
                    else if (strfield.Contains("Caption"))
                    {
                        PropertyInfo infoFieldsI = type.GetProperty("FieldItems");
                        IList iFieldItemsCollection = infoFieldsI.GetValue(ct, null) as IList;
                        for (int i = 0; i < iFieldItemsCollection.Count; i++)
                        {
                            PropertyInfo infoFields = iFieldItemsCollection[i].GetType().GetProperty("Fields");
                            IList iFieldsCollection = infoFields.GetValue(iFieldItemsCollection[i], null) as IList;
                            for (int j = 0; j < iFieldsCollection.Count; j++)
                            {
                                String strColumnName = iFieldsCollection[j].GetType().GetProperty("ColumnName").GetValue(iFieldsCollection[j], null).ToString();
                                xmlname = strColumnName + ".Caption";
                                if (string.CompareOrdinal(strfield, 0, controlname + "." + xmlname, 0, controlname.Length + 1 + xmlname.Length) == 0)
                                {
                                    iFieldsCollection[i].GetType().GetProperty("Caption").SetValue(iFieldsCollection[j], strvalue, null);
                                    break;
                                }
                            }
                        }
                    }

                    nodefield = nodefield.NextSibling;
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
                case MultiLanguage.LanguageGroups.English: nodelanguage = xml.DocumentElement.SelectSingleNode("EN"); break;
                case MultiLanguage.LanguageGroups.ChineseTra: nodelanguage = xml.DocumentElement.SelectSingleNode("CHT"); break;
                case MultiLanguage.LanguageGroups.ChineseSim: nodelanguage = xml.DocumentElement.SelectSingleNode("CHS"); break;
                case MultiLanguage.LanguageGroups.ChineseHK: nodelanguage = xml.DocumentElement.SelectSingleNode("HK"); break;
                case MultiLanguage.LanguageGroups.Japanese: nodelanguage = xml.DocumentElement.SelectSingleNode("JA"); break;
                case MultiLanguage.LanguageGroups.Korean: nodelanguage = xml.DocumentElement.SelectSingleNode("KO"); break;
                case MultiLanguage.LanguageGroups.LANG1: nodelanguage = xml.DocumentElement.SelectSingleNode("LAN1"); break;
                case MultiLanguage.LanguageGroups.LANG2: nodelanguage = xml.DocumentElement.SelectSingleNode("LAN2"); break;
            }
            return nodelanguage;
        }

        public string GetValue(string key)
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
            return null;
        }

        private void ApplyResources(object value, string objectName)
        {
            if (value == null || objectName == null)
            {
                return;
            }
            BindingFlags flags = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;

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

        private void ApplyViewHeadText(object value, string objectName)
        {
            if (value == null || objectName == null)
            {
                return;
            }

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
                            if (value is WebGridView)
                            {
                                foreach (DataControlField dc in ((WebGridView)value).Columns)
                                {
                                    if (dc is BoundField && ((BoundField)dc).DataField + ".HeadText" == strfield2)
                                    {
                                        ((BoundField)dc).HeaderText = strvalue;
                                    }
                                    else if (dc.SortExpression != null && dc.SortExpression != ""
                                        && dc.SortExpression + ".HeadText" == strfield2)
                                    {
                                        dc.HeaderText = strvalue;
                                    }
                                }
                            }
                            if (value is WebDetailsView)
                            {
                                foreach (DataControlField dc in ((WebDetailsView)value).Fields)
                                {
                                    if (dc is BoundField && ((BoundField)dc).DataField + ".HeadText" == strfield2)
                                    {
                                        ((BoundField)dc).HeaderText = strvalue;
                                    }
                                    else if (dc.SortExpression != null && dc.SortExpression != ""
                                        && dc.SortExpression + ".HeadText" == strfield2)
                                    {
                                        dc.HeaderText = strvalue;
                                    }
                                }
                            }
                            if (value is WebFormView)
                            {
                                string labelname = strfield2.Replace(".HeadText", "");
                                object obj = ((WebFormView)value).FindControl(labelname);
                                if (obj != null && obj is System.Web.UI.WebControls.Label)
                                {
                                    (obj as System.Web.UI.WebControls.Label).Text = strvalue;
                                }
                                else// modified by ccm for labels in formview itemtemplate have been renamed with 'CaptionIxxx'
                                {
                                    labelname = System.Text.RegularExpressions.Regex.Replace(labelname, @"^CaptionI", "Caption");
                                    obj = ((WebFormView)value).FindControl(labelname);
                                    if (obj != null && obj is System.Web.UI.WebControls.Label)
                                    {
                                        (obj as System.Web.UI.WebControls.Label).Text = strvalue;
                                    }
                                }
                            }
                            if (value.GetType().FullName == "AjaxTools.AjaxGridView")
                            {
                                Type type = value.GetType();
                                String columnName = strfield2.Replace(".HeadText", "");
                                PropertyInfo info = type.GetProperty("Columns");
                                IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
                                for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                                {
                                    String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
                                    if (strColumnName == columnName)
                                    {
                                        iExtGridColumnCollection[i].GetType().GetProperty("HeaderText").SetValue(iExtGridColumnCollection[i], strvalue, null);
                                        break;
                                    }
                                }

                                columnName = strfield2.Replace(".Caption", "");
                                info = type.GetProperty("QueryFields");
                                iExtGridColumnCollection = info.GetValue(value, null) as IList;
                                for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                                {
                                    String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("Id").GetValue(iExtGridColumnCollection[i], null).ToString();
                                    if (strColumnName == columnName)
                                    {
                                        iExtGridColumnCollection[i].GetType().GetProperty("Caption").SetValue(iExtGridColumnCollection[i], strvalue, null);
                                        break;
                                    }
                                }
                            }
                            if (value.GetType().FullName == "AjaxTools.AjaxFormView")
                            {
                                Type type = value.GetType();
                                String columnName = strfield2.Replace(".HeadText", "");
                                PropertyInfo info = type.GetProperty("Fields");
                                IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
                                for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                                {
                                    String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
                                    if (strColumnName == columnName)
                                    {
                                        iExtGridColumnCollection[i].GetType().GetProperty("Caption").SetValue(iExtGridColumnCollection[i], strvalue, null);
                                        break;
                                    }
                                }
                            }
                            if (value.GetType().FullName == "AjaxTools.AjaxLayout")
                            {                                
                                Type type = value.GetType();
                                String columnName = strfield2.Replace(".Title", "");
                                PropertyInfo info = type.GetProperty("Masters");
                                IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
                                for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                                {
                                    String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("ControlId").GetValue(iExtGridColumnCollection[i], null).ToString();
                                    if (strColumnName == columnName)
                                    {
                                        iExtGridColumnCollection[i].GetType().GetProperty("Title").SetValue(iExtGridColumnCollection[i], strvalue, null);
                                        break;
                                    }
                                }

                                info = type.GetProperty("Details");
                                iExtGridColumnCollection = info.GetValue(value, null) as IList;
                                for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                                {
                                    String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("ControlId").GetValue(iExtGridColumnCollection[i], null).ToString();
                                    if (strColumnName == columnName)
                                    {
                                        iExtGridColumnCollection[i].GetType().GetProperty("Title").SetValue(iExtGridColumnCollection[i], strvalue, null); 
                                        break;
                                    }
                                }

                                if (objectName + ".Title" == strfield)
                                {
                                    info = type.GetProperty("Title");
                                    info.SetValue(value, strvalue, null);
                                }

                                if (objectName + ".ViewTitle" == strfield)
                                {
                                    info = type.GetProperty("ViewTitle");
                                    info.SetValue(value, strvalue, null);
                                }
                            }
                        }
                    }
                    nodefield = nodefield.NextSibling;
                }
            }
        }

        public void SetDefaultGroup()
        {
            //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
            SYS_LANGUAGE language = CliUtils.fClientLang;

            switch (language)
            {
                case SYS_LANGUAGE.ENG:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.English;
                    break;
                case SYS_LANGUAGE.TRA:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.ChineseTra;
                    break;
                case SYS_LANGUAGE.SIM:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.ChineseSim;
                    break;
                case SYS_LANGUAGE.HKG:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.ChineseHK;
                    break;
                case SYS_LANGUAGE.JPN:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.Japanese;
                    break;
                case SYS_LANGUAGE.LAN3:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.Korean;
                    break;
                case SYS_LANGUAGE.LAN1:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.LANG1;
                    break;
                case SYS_LANGUAGE.LAN2:
                    this.GroupIndex = Srvtools.MultiLanguage.LanguageGroups.LANG2;
                    break;
            }
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            ArrayList DBNameList = new ArrayList();

            XmlDocument DBXML = new XmlDocument();
            if (File.Exists(SystemFile.DBFile))
            {
                DBXML.Load(SystemFile.DBFile);
                XmlNode aNode = DBXML.DocumentElement.FirstChild.FirstChild;
                while (aNode != null)
                {
                    DBNameList.Add(aNode.LocalName);
                    aNode = aNode.NextSibling;
                }
            }
            int i = DBNameList.Count;
            string[] strDBName = new string[i];
            for (int j = 0; j < i; j++)
            {
                strDBName[j] = DBNameList[j].ToString();
            }

            return strDBName;
        }

        #endregion

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
                    if (ct is WebValidate)
                    {
                        WebValidate ctrl = ct as WebValidate;
                        for (int i = 0; i < ctrl.Fields.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Warningmsg{1}", ct.ID, i), ((ValidateFieldItem)ctrl.Fields[i]).WarningMsg));
                        }
                    }
                    else if (ct is WebClientQuery)
                    {
                        WebClientQuery ctrl = ct as WebClientQuery;
                        for (int i = 0; i < ctrl.Columns.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Caption{1}", ct.ID, i), ((WebQueryColumns)ctrl.Columns[i]).Caption));
                        }
                    }
                    else if(ct is WebNavigator)
                    {
                        WebNavigator ctrl = ct as WebNavigator;
                        for (int i = 0; i < ctrl.QueryFields.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Caption{1}", ct.ID, i), ((WebQueryField)ctrl.QueryFields[i]).Caption));
                        }
                    }
                    else if (ct is WebMultiViewCaptions)
                    {
                        WebMultiViewCaptions ctrl = ct as WebMultiViewCaptions;
                        for (int i = 0; i < ctrl.Captions.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Caption{1}", ct.ID, i), ((WebMultiViewCaption)ctrl.Captions[i]).Caption));
                        }
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxCollapsiblePanel")
                    {
                        Type type = ct.GetType();
                        PropertyInfo info = type.GetProperty("ExpandedText");
                        list.Add(new DictionaryEntry(string.Format("{0}.ExpandedText", ct.ID), info.GetValue(ct, null).ToString()));
                        info = type.GetProperty("CollapsedText");
                        list.Add(new DictionaryEntry(string.Format("{0}.CollapsedText", ct.ID), info.GetValue(ct, null).ToString()));
                    }
                    else if (ct is WebGridView)
                    {
                        WebGridView ctrl = ct as WebGridView;
                        for (int i = 0; i < ctrl.Columns.Count; i++)
                        {
                            string value = string.Empty;
                            if (ctrl.Columns[i] is BoundField)
                            {
                                value = ((BoundField)ctrl.Columns[i]).DataField;
                            }
                            else if (!string.IsNullOrEmpty(ctrl.Columns[i].SortExpression))
                            {
                                value = ctrl.Columns[i].SortExpression;
                            }
                            else
                            {
                                continue;
                            }
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.HeadText", ct.ID, value), ctrl.Columns[i].HeaderText));
                        }
                    }
                    else if (ct is WebDetailsView)
                    {
                        WebDetailsView ctrl = ct as WebDetailsView;
                        for (int i = 0; i < ctrl.Fields.Count; i++)
                        {
                            string value = string.Empty;
                            if (ctrl.Fields[i] is BoundField)
                            {
                                value = ((BoundField)ctrl.Fields[i]).DataField;
                            }
                            else if (!string.IsNullOrEmpty(ctrl.Fields[i].SortExpression))
                            {
                                value = ctrl.Fields[i].SortExpression;
                            }
                            else
                            {
                                continue;
                            }
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.HeadText", ct.ID, value), ctrl.Fields[i].HeaderText));
                        }
                    }
                    else if (ct is WebFormView)
                    {
                        string strtemp = EditionDifference.ActiveDocumentFullName() + ".tmp";
                        if (File.Exists(strtemp))
                        {
                            Hashtable ht = new Hashtable();
                            using (StreamReader sr = new StreamReader(strtemp, Encoding.UTF8))
                            {
                                while (sr.Peek() != -1)
                                {
                                    string line = sr.ReadLine();
                                    string name = line.Substring(0, line.IndexOf('='));
                                    string value = line.Substring(line.IndexOf('=') + 1);
                                    ht.Add(name, value);
                                }
                                foreach (string str in ht.Keys)
                                {
                                    list.Add(new DictionaryEntry(str, ht[str].ToString()));
                                }
                                sr.Close();
                            }
                        }
                    }
                    else if (ct is DropDownList)
                    {
                        DropDownList ctrl = ct as DropDownList;
                        for (int i = 0; i < ctrl.Items.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Item{1}", ct.ID, i), ctrl.Items[i].Text));
                        }
                    }
                    else if (ct is RadioButtonList)
                    {
                        RadioButtonList ctrl = ct as RadioButtonList;
                        for (int i = 0; i < ctrl.Items.Count; i++)
                        {
                             list.Add(new DictionaryEntry(string.Format("{0}.Item{1}", ct.ID, i), ctrl.Items[i].Text));
                        }
                    }
                    else if (ct is CheckBoxList)
                    {
                        CheckBoxList ctrl = ct as CheckBoxList;
                        for (int i = 0; i < ctrl.Items.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Item{1}", ct.ID, i), ctrl.Items[i].Text));
                        }
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxGridView")
                    {
                        Type type = ct.GetType();
                        PropertyInfo info = type.GetProperty("Columns");
                        IList iExtGridColumnCollection = info.GetValue(ct, null) as IList;
                        for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                        {
                            String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
                            String strHeadText = iExtGridColumnCollection[i].GetType().GetProperty("HeaderText").GetValue(iExtGridColumnCollection[i], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.HeadText", ct.ID, strColumnName), strHeadText));
                        }

                        info = type.GetProperty("QueryFields");
                        iExtGridColumnCollection = info.GetValue(ct, null) as IList;
                        for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                        {
                            String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("Id").GetValue(iExtGridColumnCollection[i], null).ToString();
                            String strHeadText = iExtGridColumnCollection[i].GetType().GetProperty("Caption").GetValue(iExtGridColumnCollection[i], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", ct.ID, strColumnName), strHeadText));
                        }
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxFormView")
                    {
                        Type type = ct.GetType();
                        PropertyInfo info = type.GetProperty("Fields");
                        IList iExtGridColumnCollection = info.GetValue(ct, null) as IList;
                        for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                        {
                            String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
                            String strHeadText = iExtGridColumnCollection[i].GetType().GetProperty("Caption").GetValue(iExtGridColumnCollection[i], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.HeadText", ct.ID, strColumnName), strHeadText));
                        }
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxLayout")
                    {
                        Type type = ct.GetType();
                        PropertyInfo info = type.GetProperty("Masters");
                        IList iExtGridColumnCollection = info.GetValue(ct, null) as IList;
                        for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                        {
                            String strControlId = iExtGridColumnCollection[i].GetType().GetProperty("ControlId").GetValue(iExtGridColumnCollection[i], null).ToString();
                            String strTitle = iExtGridColumnCollection[i].GetType().GetProperty("Title").GetValue(iExtGridColumnCollection[i], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Title", ct.ID, strControlId), strTitle));
                        }

                        info = type.GetProperty("Details");
                        iExtGridColumnCollection = info.GetValue(ct, null) as IList;
                        for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                        {
                            String strControlId = iExtGridColumnCollection[i].GetType().GetProperty("ControlId").GetValue(iExtGridColumnCollection[i], null).ToString();
                            String strTitle = iExtGridColumnCollection[i].GetType().GetProperty("Title").GetValue(iExtGridColumnCollection[i], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Title", ct.ID, strControlId), strTitle));
                        }

                        info = type.GetProperty("Title");
                        String title = info.GetValue(ct, null).ToString();
                        list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), title));

                        info = type.GetProperty("ViewTitle");
                        String viewTitle = info.GetValue(ct, null).ToString();
                        list.Add(new DictionaryEntry(string.Format("{0}.ViewTitle", ct.ID), viewTitle));

                    }
                    else if (ct.GetType().Name == "WebEasilyReport")
                    {
                        Type type = ct.GetType();
                        PropertyInfo infoHI = type.GetProperty("HeaderItems");
                        IList iHeaderItemsCollection = infoHI.GetValue(ct, null) as IList;
                        for (int i = 0; i < iHeaderItemsCollection.Count; i++)
                        {
                            String strFormat = (String)iHeaderItemsCollection[i].GetType().GetProperty("Format").GetValue(iHeaderItemsCollection[i], null);
                            if (iHeaderItemsCollection[i].GetType().GetProperty("Style") != null)
                            {
                                String strStyle = String.Empty;
                                strStyle = iHeaderItemsCollection[i].GetType().GetProperty("Style").GetValue(iHeaderItemsCollection[i], null).ToString();
                                list.Add(new DictionaryEntry(string.Format("{0}.HeaderItems.{1}.Format", ct.ID, strStyle), strFormat));
                            }
                            else if (iHeaderItemsCollection[i].GetType().GetProperty("ColumnName") != null)
                            {
                                String strColumnName = String.Empty;
                                strColumnName = iHeaderItemsCollection[i].GetType().GetProperty("ColumnName").GetValue(iHeaderItemsCollection[i], null).ToString();
                                list.Add(new DictionaryEntry(string.Format("{0}.HeaderItems.{1}.Format", ct.ID, strColumnName), strFormat));
                            }
                        }

                        PropertyInfo infoFI = type.GetProperty("FooterItems");
                        IList iFooterItemsCollection = infoFI.GetValue(ct, null) as IList;
                        for (int i = 0; i < iFooterItemsCollection.Count; i++)
                        {
                            String strFormat = iFooterItemsCollection[i].GetType().GetProperty("Format").GetValue(iFooterItemsCollection[i], null).ToString();
                            String strStyle = String.Empty;
                            if (iFooterItemsCollection[i].GetType().GetProperty("Style") != null)
                                strStyle = iFooterItemsCollection[i].GetType().GetProperty("Style").GetValue(iFooterItemsCollection[i], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.FooterItems.{1}.Format", ct.ID, strStyle), strFormat));
                        }

                        PropertyInfo infoFieldsI = type.GetProperty("FieldItems");
                        IList iFieldItemsCollection = infoFieldsI.GetValue(ct, null) as IList;
                        for (int i = 0; i < iFieldItemsCollection.Count; i++)
                        {
                            PropertyInfo infoFields = iFieldItemsCollection[i].GetType().GetProperty("Fields");
                            IList iFieldsCollection = infoFields.GetValue(iFieldItemsCollection[i], null) as IList;
                            for (int j = 0; j < iFieldsCollection.Count; j++)
                            {
                                String strCaption = iFieldsCollection[j].GetType().GetProperty("Caption").GetValue(iFieldsCollection[j], null).ToString();
                                String strColumnName = iFieldsCollection[j].GetType().GetProperty("ColumnName").GetValue(iFieldsCollection[j], null).ToString();
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", ct.ID, strColumnName), strCaption));

                            }
                        }
                    }

                    if (!(ct is WebGridView))
                    {
                        List<DictionaryEntry> listchild = GetControlValues(ct.Controls);
                        list.AddRange(listchild);
                    }
                }
            }
            return list;
        }
    }

    public class WebMultiLanguageEditor : DataSourceDesigner
    {
        private DesignerActionListCollection _actionLists;

        public WebMultiLanguageEditor()
        {
            DesignerVerb editVerb = new DesignerVerb("Edit", new EventHandler(OnEdit));
            this.Verbs.Add(editVerb);
        }

        public void OnEdit(object sender, EventArgs e)

        {
            if ((this.Component as WebMultiLanguage).DataBase != "" && (this.Component as WebMultiLanguage).DataBase != null && (this.Component as WebMultiLanguage).Active)
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
                    _actionLists.Add(new WebMultiLanguageActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class WebMultiLanguageActionList : DesignerActionList
    {
        private WebMultiLanguage wml;

        public WebMultiLanguageActionList(IComponent component)
            : base(component)
        {
            wml = component as WebMultiLanguage;
        }

        public void OnEdit()
        {
            if (wml.DataBase != "" && wml.DataBase != null && wml.Active)
            {
                MultiLanguageEditorDialog form = new MultiLanguageEditorDialog(wml);
                form.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("the property of webmultilanguage hasn't be set correctly");
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnEdit", "Edit", "UserEdit", true));
            return items;
        }
    }
}
