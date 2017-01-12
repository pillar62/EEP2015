using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(MultiLanguage), "Resources.MultiLanguage.ico")]
    [Designer(typeof(ResourceManagerEditor), typeof(IDesigner))]
    public class MultiLanguage : InfoBaseComp, IGetValues, ISupportInitialize
    {
        public MultiLanguage()
        {
            _Identification = this.GetHashCode();
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

        public enum LanguageGroups
        { English, ChineseTra, ChineseSim, ChineseHK, Japanese, Korean, LANG1, LANG2 }

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

        private string _DataBase;
        [Category("Infolight"),
        Description("Specifies DataBase storing the data of MultiLanguage")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        private int _Identification;
        [Browsable(false)]
        public int Identification
        {
            get { return _Identification; }
            set { _Identification = value; }
        }

        [Browsable(false)]
        public InfoForm OwnerForm
        {
            get
            {
                return (InfoForm)Srvtools.DesignSupport.GetDesignRoot();
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
            Type myType = this.OwnerComp.GetType();
            FieldInfo[] myFields = myType.GetFields(BindingFlags.Instance
                                                 | BindingFlags.Public
                                                 | BindingFlags.NonPublic
                                                 | BindingFlags.DeclaredOnly);
            for (int i = 0; i < myFields.Length; i++)
            {
                object objValue = myFields[i].GetValue(this.OwnerComp);
                if (objValue != null && !(objValue is InfoDataSet))
                {
                    ApplyResources(objValue, myFields[i].Name);
                }
                if (objValue is DefaultValidate)
                {
                    SetCollectionValue(objValue, myFields[i].Name, "FieldItems", "WarningMsg", "Warningmsg");
                }
                else if (objValue is ClientQuery)
                {
                    SetCollectionValue(objValue,myFields[i].Name, "Columns", "Caption", "Caption");
                }
                else if (objValue is InfoNavigator)
                { 
                    SetCollectionValue(objValue,myFields[i].Name, "QueryFields", "Caption", "QueryField");
                }
                else if (objValue is ComboBox)
                {
                    SetCollectionValue(objValue, myFields[i].Name, "Items", null, "Item");
                }
                else if (objValue != null && objValue.GetType().Name == "EasilyReport")
                {
                    SetEasilyReportValue(objValue, myFields[i].Name);
                }
            }
            ApplyResources(this.OwnerComp, this.OwnerComp.GetType().Name);
            string text = GetValue("this.Text");
            if(text != null)
            {
                (this.OwnerComp as InfoForm).Text = text;
            }
        }

        public string GetValue(string key)
        {
            XmlNode nodelanguage = GetLanguageNode();
            if (nodelanguage.ChildNodes.Count > 0)
            {
                XmlNode nodevalue = nodelanguage.SelectSingleNode(string.Format("Language[@field='{0}']",key));
                if (nodevalue != null && nodevalue.Attributes["value"] != null)
                {
                    return nodevalue.Attributes["value"].Value;
                }
            }
            return null;
        }

        private void SetEasilyReportValue(object ct, String cName)
        {
            String controlname = cName;
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
                                else
                                {
                                    collection.GetType().GetProperty("Item").SetValue(collection, strvalue, new object[] { index });
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
            string xmlfile = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + this.Identification.ToString() + ".xml";

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

        private void ApplyResources(object value, string objName)
        {
            ToolTip tooltip = new ToolTip();

            string objectName = objName.TrimStart('_');//trim for vb
            if (value == null || objName == null)
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

                            if (string.Compare(strfield2, "tooltip", true) == 0)//tooltip不是属性不能赋值,只好这样
                            {
                                tooltip.SetToolTip(value as Control, strvalue);
                            }
                            else
                            {
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
                                        info = type1.GetProperty(strfield2, flags| BindingFlags.DeclaredOnly);
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
                    this.GroupIndex = LanguageGroups.English;
                    break;
                case SYS_LANGUAGE.TRA:
                    this.GroupIndex = LanguageGroups.ChineseTra;
                    break;
                case SYS_LANGUAGE.SIM:
                    this.GroupIndex = LanguageGroups.ChineseSim;
                    break;
                case SYS_LANGUAGE.HKG:
                    this.GroupIndex = LanguageGroups.ChineseHK;
                    break;
                case SYS_LANGUAGE.JPN:
                    this.GroupIndex = LanguageGroups.Japanese;
                    break;
                case SYS_LANGUAGE.LAN3:
                    this.GroupIndex = LanguageGroups.Korean;
                    break;
                case SYS_LANGUAGE.LAN1:
                    this.GroupIndex = LanguageGroups.LANG1;
                    break;
                case SYS_LANGUAGE.LAN2:
                    this.GroupIndex = LanguageGroups.LANG2;
                    break;
            }
        }

        internal List<DictionaryEntry> GetControlValues()
        {
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            if (this.DesignMode)
            {
                IResourceService service = this.GetService(typeof(IResourceService)) as IResourceService;
                IResourceReader reader = service.GetResourceReader(new CultureInfo(string.Empty));
                IDictionaryEnumerator enumerator = reader.GetEnumerator();
                string formName = string.Empty;
                string formText = string.Empty;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString().Equals(">>$this.Name"))
                    {
                        formName = enumerator.Value.ToString();
                    }
                    else if (enumerator.Key.ToString().Equals("$this.Text"))
                    {
                        formText = enumerator.Value.ToString();
                    }
                    else
                    {
                        string[] keys = enumerator.Key.ToString().Split('.');
                        if (keys.Length == 2)
                        {
                            object comp = this.Container.Components[keys[0]];
                            if (comp != null && !(comp is InfoNavigator) && !(comp is ToolStripTextBox) && comp.GetType().BaseType != typeof(ToolStripItem))
                            {
                                PropertyInfo pi = comp.GetType().GetProperty(keys[1], BindingFlags.Instance | BindingFlags.Public);
                                if ((pi != null && pi.PropertyType == typeof(string)) || string.Compare(keys[1], "tooltip", true) == 0)
                                {
                                    list.Add(new DictionaryEntry(enumerator.Key.ToString(), enumerator.Value.ToString()));
                                }
                            }
                        }
                    }
                }
                if (formName.Length > 0)
                {
                    list.Add(new DictionaryEntry("this.Text", formText.Length > 0 ? formText : formName));
                }

                ComponentCollection comps = this.Container.Components;

                for (int i = 0; i < comps.Count; i++)
                {
                    #region DefaultValidate
                    if (comps[i] is DefaultValidate)
                    {
                        DefaultValidate comp = comps[i] as DefaultValidate;
                        for (int j = 0; j < comp.FieldItems.Count; j++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Warningmsg{1}", comp.Site.Name, j), ((FieldItem)comp.FieldItems[j]).WarningMsg));
                        }
                    }
                    #endregion

                    #region ClientQuery
                    else if (comps[i] is ClientQuery)
                    {
                        ClientQuery comp = comps[i] as ClientQuery;
                        for (int j = 0; j < comp.Columns.Count; j++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Caption{1}", comp.Site.Name, j), ((QueryColumns)comp.Columns[j]).Caption));
                        }
                    }
                    #endregion

                    #region InfoNavigator
                    else if (comps[i] is InfoNavigator)
                    {
                        InfoNavigator comp = comps[i] as InfoNavigator;
                        for (int j = 0; j < comp.QueryFields.Count; j++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.QueryField{1}", comp.Site.Name, j), ((QueryField)comp.QueryFields[j]).Caption));
                        }
                    }
                    #endregion

                    else if (comps[i] is ComboBox)
                    {
                        ComboBox comp = comps[i] as ComboBox;
                        for (int j = 0; j < comp.Items.Count; j++)
                        {
                            list.Add(new DictionaryEntry(string.Format("{0}.Item{1}", comp.Site.Name, j), comp.Items[j].ToString()));
                        }
                    }

                    else if(comps[i].GetType().Name == "EasilyReport")
                    {
                        Type type = comps[i].GetType();
                        PropertyInfo infoHI = type.GetProperty("HeaderItems");
                        IList iHeaderItemsCollection = infoHI.GetValue(comps[i], null) as IList;
                        for (int x = 0; x < iHeaderItemsCollection.Count; x++)
                        {
                            String strFormat = (String)iHeaderItemsCollection[x].GetType().GetProperty("Format").GetValue(iHeaderItemsCollection[x], null);
                            if (iHeaderItemsCollection[x].GetType().GetProperty("Style") != null)
                            {
                                String strStyle = String.Empty;
                                strStyle = iHeaderItemsCollection[x].GetType().GetProperty("Style").GetValue(iHeaderItemsCollection[x], null).ToString();
                                list.Add(new DictionaryEntry(string.Format("{0}.HeaderItems.{1}.Format", comps[i].Site.Name, strStyle), strFormat));
                            }
                            else if (iHeaderItemsCollection[x].GetType().GetProperty("ColumnName") != null)
                            {
                                String strColumnName = String.Empty;
                                strColumnName = iHeaderItemsCollection[x].GetType().GetProperty("ColumnName").GetValue(iHeaderItemsCollection[x], null).ToString();
                                list.Add(new DictionaryEntry(string.Format("{0}.HeaderItems.{1}.Format", comps[i].Site.Name, strColumnName), strFormat));
                            }
                        }

                        PropertyInfo infoFI = type.GetProperty("FooterItems");
                        IList iFooterItemsCollection = infoFI.GetValue(comps[i], null) as IList;
                        for (int x = 0; x < iFooterItemsCollection.Count; x++)
                        {
                            String strFormat = iFooterItemsCollection[x].GetType().GetProperty("Format").GetValue(iFooterItemsCollection[x], null).ToString();
                            String strStyle = String.Empty;
                            if (iFooterItemsCollection[x].GetType().GetProperty("Style") != null)
                                strStyle = iFooterItemsCollection[x].GetType().GetProperty("Style").GetValue(iFooterItemsCollection[x], null).ToString();
                            list.Add(new DictionaryEntry(string.Format("{0}.FooterItems.{1}.Format", comps[i].Site.Name, strStyle), strFormat));
                        }

                        PropertyInfo infoFieldsI = type.GetProperty("FieldItems");
                        IList iFieldItemsCollection = infoFieldsI.GetValue(comps[i], null) as IList;
                        for (int x = 0; x < iFieldItemsCollection.Count; x++)
                        {
                            PropertyInfo infoFields = iFieldItemsCollection[x].GetType().GetProperty("Fields");
                            IList iFieldsCollection = infoFields.GetValue(iFieldItemsCollection[x], null) as IList;
                            for (int j = 0; j < iFieldsCollection.Count; j++)
                            {
                                String strCaption = iFieldsCollection[j].GetType().GetProperty("Caption").GetValue(iFieldsCollection[j], null).ToString();
                                String strColumnName = iFieldsCollection[j].GetType().GetProperty("ColumnName").GetValue(iFieldsCollection[j], null).ToString();
                                list.Add(new DictionaryEntry(string.Format("{0}.{1}.Caption", comps[i].Site.Name, strColumnName), strCaption));

                            }
                        }
                    }
                }
            }
            return list;
        }

        [Obsolete("The recommended alternative is Multilanguage.GetLastVersion().")]
        public void GetLastVision()
        {
            string filename = this.Identification.ToString() + ".xml";
            CliUtils.DownLoadModule(filename, true);
        }

        public void GetLastVersion()
        {
            string filename = this.Identification.ToString() + ".xml";
            CliUtils.DownLoadModule(filename, true);    
        }

        #region IGetValues
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

        #region ISupportInitialize Members

        public void BeginInit() { }

        public void EndInit()
        {
            if(!this.DesignMode)
            {
                GetLastVersion();
            }
        }

        #endregion

        public String TraditionalToSimplified(String str)
        {
            System.Globalization.CultureInfo chs = new System.Globalization.CultureInfo("zh-CN", false);
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, chs.LCID);
        }

        public String SimplifiedToTraditional(String str)
        {
            System.Globalization.CultureInfo cht = new System.Globalization.CultureInfo("zh-TW", false);
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, cht.LCID);
        }
    }

    public class ResourceManagerEditor : ComponentDesigner
    {
        // 在Component上双击后执行
        public override void DoDefaultAction()
        {
            if ((this.Component as MultiLanguage).DataBase != "" && (this.Component as MultiLanguage).DataBase != null && (this.Component as MultiLanguage).Active)
            {
                MultiLanguageEditorDialog form = new MultiLanguageEditorDialog(this.Component);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("the property of multilanguage hasn't be set correctly");
            }
        }
    }
}
