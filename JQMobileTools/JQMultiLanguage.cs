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

namespace JQMobileTools
{
    [Designer(typeof(JQMultiLanguageEditor), typeof(IDesigner))]
    public class JQMultiLanguage : WebControl
    {
        public JQMultiLanguage()
        {

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
                    if (ct is JQDataForm || ct is JQDataGrid || ct is JQValidate)
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

                    //if (ct is WebGridView || ct is WebDetailsView || ct is WebFormView)
                    //{
                    //    ApplyViewHeadText(ct, ct.ID);
                    //}
                    //else if (ct is WebValidate)
                    //{
                    //    SetCollectionValue(ct, ct.ID, "Fields", "WarningMsg", "Warningmsg");
                    //}
                    //else if (ct is WebClientQuery)
                    //{
                    //    SetCollectionValue(ct, ct.ID, "Columns", "Caption", "Caption");
                    //}
                    //else if (ct is WebNavigator)
                    //{
                    //    SetCollectionValue(ct, ct.ID, "QueryFields", "Caption", "Caption");
                    //}
                    //else if (ct is DropDownList || ct is RadioButtonList || ct is CheckBoxList)
                    //{
                    //    SetCollectionValue(ct, ct.ID, "Items", null, "Item");
                    //}
                    //else if (ct is WebMultiViewCaptions && ct.ID != null)
                    //{
                    //    SetCollectionValue(ct, ct.ID, "Captions", "Caption", "Caption");
                    //}
                    //else if (ct.GetType().FullName == "AjaxTools.AjaxGridView")
                    //{
                    //    ApplyViewHeadText(ct, ct.ID);
                    //}
                    //else if (ct.GetType().FullName == "AjaxTools.AjaxFormView")
                    //{
                    //    ApplyViewHeadText(ct, ct.ID);
                    //}
                    //else if (ct.GetType().FullName == "AjaxTools.AjaxLayout")
                    //{
                    //    ApplyViewHeadText(ct, ct.ID);
                    //}
                    //else
                    //{
                    //    ApplyResources(ct, ct.ID);
                    //}
                }
                //if (!(ct is WebGridView))
                //{
                //    SetLanguage(ct.Controls);
                //}
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
                                    object field = (collection as IList)[index];
                                    field.GetType().GetProperty(propertyname).SetValue(field, strvalue, null);
                                }
                                else if (collection is ListItemCollection)
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

        private void ApplyColumnCaption(object value, string objectName)
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
                            if (value is JQDataGrid)
                            {
                                foreach (JQGridColumn dc in ((JQDataGrid)value).Columns)
                                {
                                    if (objectName + "." + dc.FieldName + ".Caption" == strfield)
                                    {
                                        dc.Caption = strvalue;
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
                                }

                                if (objectName + "." + "Title" == strfield)
                                {
                                    (value as JQDataGrid).Title = strvalue;
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
                                    var keys = strfield.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (strfield.StartsWith(objectName + "." + dc.FieldName) && keys.Length > 3)
                                    {
                                        switch (dc.Editor)
                                        {
                                            case "flipswitch":
                                                JQFlipSwitch jfs = new JQFlipSwitch();
                                                jfs.LoadProperties(dc.EditorOptions);
                                                if (keys[3] == "OnText")
                                                    dc.EditorOptions = dc.EditorOptions.Replace(String.Format("OnText:'{0}'", jfs.OnText), String.Format("OnText:'{0}'", strvalue));
                                                else if (keys[3] == "OffText")
                                                    dc.EditorOptions = dc.EditorOptions.Replace(String.Format("OffText:'{0}'", jfs.OffText), String.Format("OffText:'{0}'", strvalue));
                                                break;
                                            case "refval":
                                                JQRefval jr = new JQRefval();
                                                jr.LoadProperties(dc.EditorOptions);
                                                if (keys[3] == "DialogTitle")
                                                    dc.EditorOptions = dc.EditorOptions.Replace(String.Format("DialogTitle:'{0}'", jr.DialogTitle), String.Format("DialogTitle:'{0}'", strvalue));
                                                else if (keys[3] == "Caption")
                                                {
                                                    for (int j = 0; j < jr.Columns.Count; j++)
                                                    {
                                                        if (jr.Columns[j].FieldName == keys[4])
                                                            dc.EditorOptions = dc.EditorOptions.Replace(String.Format("Caption:'{0}'", jr.Columns[j].Caption), String.Format("Caption:'{0}'", strvalue));
                                                    }
                                                }
                                                break;
                                        }
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
                                        dc.ValidateMessage = strvalue;
                                        break;
                                    }
                                }
                            }
                            else if (value is JQTab)
                            {
                                if (objectName + "." + "Title" == strfield)
                                {
                                    (value as JQTab).Title = strvalue;
                                }
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
                    nodefield = nodefield.NextSibling;
                }
            }
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
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", ct.ID, ctrl.Columns[i].FieldName), String.IsNullOrEmpty(ctrl.Columns[i].Caption) ? ctrl.Columns[i].FieldName : ctrl.Columns[i].Caption));
                            switch (ctrl.Columns[i].Editor)
                            {
                                case "flipswitch":
                                    JQFlipSwitch jfs = new JQFlipSwitch();
                                    jfs.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.flipswitch.OnText", ct.ID, ctrl.Columns[i].FieldName), jfs.OnText));
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.flipswitch.OffText", ct.ID, ctrl.Columns[i].FieldName), jfs.OffText));
                                    break;
                                case "refval":
                                    JQRefval jr = new JQRefval();
                                    jr.LoadProperties(ctrl.Columns[i].EditorOptions);
                                    list.Add(new DictionaryEntry(string.Format("{0}.{1}.refval.DialogTitle", ct.ID, ctrl.Columns[i].FieldName), jr.DialogTitle));
                                    for (int j = 0; j < jr.Columns.Count; j++)
                                    {
                                        list.Add(new DictionaryEntry(string.Format("{0}.{1}.refval.Caption.{2}", ct.ID, ctrl.Columns[i].FieldName, jr.Columns[j].FieldName), String.IsNullOrEmpty(jr.Columns[j].Caption) ? jr.Columns[j].FieldName : jr.Columns[j].Caption));
                                    }
                                    break;
                            }
                        }
                    }
                    if (ct is JQDataGrid)
                    {
                        JQDataGrid ctrl = ct as JQDataGrid;
                        for (int i = 0; i < ctrl.Columns.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", ct.ID, ctrl.Columns[i].FieldName), String.IsNullOrEmpty(ctrl.Columns[i].Caption) ? ctrl.Columns[i].FieldName : ctrl.Columns[i].Caption));
                        }
                        for (int i = 0; i < ctrl.QueryColumns.Count; i++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.{1}.QueryColumnCaption{2}", ct.ID, ctrl.QueryColumns[i].FieldName, ctrl.QueryColumns[i].Condition), String.IsNullOrEmpty(ctrl.QueryColumns[i].Caption) ? ctrl.QueryColumns[i].FieldName : ctrl.QueryColumns[i].Caption));
                        }
                        list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), ctrl.Title));
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
                    if (ct is JQTab)
                    {
                        list.Add(new DictionaryEntry(string.Format("{0}.Title", ct.ID), (ct as JQTab).Title));
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
