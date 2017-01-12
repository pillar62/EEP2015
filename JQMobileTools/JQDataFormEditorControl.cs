using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text.RegularExpressions;

namespace JQMobileTools
{

    public class JQDataFormEditorAttribute : Attribute
    {
        public JQDataFormEditorAttribute(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            TypeName = typeName;
        }

        public string TypeName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is JQDataFormEditorAttribute)
            {
                return this.TypeName.Equals((obj as JQDataFormEditorAttribute).TypeName);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.TypeName.GetHashCode();
        }
    }

    public class JQControl: IThemeObject
    {
        public virtual void Render(HtmlTextWriter writer) { }

        public static JQControl CreateControl(string typeName, string options)
        {
            var types = typeof(JQControl).Assembly.GetTypes()
                .Where(c => typeof(JQControl).IsAssignableFrom(c));
            foreach (var type in types)
            {
                var jqAttribute = type.GetCustomAttributes(typeof(JQDataFormEditorAttribute), true).OfType<JQDataFormEditorAttribute>().FirstOrDefault(c=> c.TypeName == typeName);
                if (jqAttribute != null)
                {
                    //var control = (JQDataFormEditorControl)Activator.CreateInstance(type);
                    //control.Options = options;
                    //return control;
                    var control = (JQControl)Deserailze(options, type, null);
                    return control;
                }
            }
            return null;
        }

        internal ITypeDescriptorContext Context { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Options
        {
            get
            {
                return Serialize(this);
            }
        }
        [Browsable(false)]
        public string ID { get; set; }
        [Browsable(false)]
        public string Caption { get; set; }
        [Browsable(false)]
        public string Theme { get; set; }

        //runtime property to create items 
        public virtual string EditorOptions 
        { 
            get
            { 
                return string.Empty; 
            } 
        }

        private static string Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var objType = obj.GetType();
            if (objType == typeof(string) || objType == typeof(Guid))
            {
                return string.Format("'{0}'", obj);
            }
            else if (typeof(IList).IsAssignableFrom(objType))
            {
                var listItems = new List<string>();
                foreach (var item in (IList)obj)
	            {
		            listItems.Add(Serialize(item));
                }
                return string.Format("[{0}]", string.Join(",", listItems));
            }
            else if (objType.IsClass)
            {
                var listProperties = new List<string>();
                foreach (var property in objType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        if (property.GetCustomAttributes(typeof(BrowsableAttribute), true).OfType<BrowsableAttribute>()
                            .FirstOrDefault(c => !c.Browsable) != null)
                        {
                            continue;
                        }
                        var value = property.GetValue(obj, null);
                        if (value != null)
                        {
                            listProperties.Add(string.Format("{0}:{1}", property.Name, Serialize(value)));
                        }
                    }
                }
                return string.Format("{{{0}}}", string.Join(",", listProperties));
            }
            else
            {
                return obj.ToString();
            }
        }

        private static object Deserailze(string option, Type objType, object parent /*for collection*/)
        {
            if (objType == typeof(string))
            {
                return option.Trim('\'');
            }
            else if (objType == typeof(Guid))
            {
                return new Guid(option.Trim('\''));
            }
            else if (typeof(IList).IsAssignableFrom(objType))
            {
                IList list = null;
                var constructor = objType.GetConstructor(new Type[] { }); 
                if(constructor == null)
                {
                    constructor = objType.GetConstructor(new Type[] { typeof(object) }); 
                    list = (IList)constructor.Invoke(new object[] { parent });
                }
                else
                {
                    list = (IList)constructor.Invoke(new object[] { });
                }
                if (objType.IsGenericType)
                {
                    var itemType = objType.GetGenericArguments()[0];
                    foreach (var itemOption in Split(option.Trim("[]".ToCharArray())))
                    {
                        list.Add(Deserailze(itemOption, itemType, null));
                    }
                }
                return list;
            }
            else if (objType.IsClass)
            {
                object obj = null;
                var constructor = objType.GetConstructors()[0];
                obj = constructor.Invoke(new object[] { });
                foreach (var propertyOption in Split(option.Trim("{}".ToCharArray())))
                {
                    var index = propertyOption.IndexOf(':');
                    if (index > 0 && index < propertyOption.Length - 1)
                    {
                        var propertyName = propertyOption.Substring(0, index);
                        var propertyValue = propertyOption.Substring(index + 1);
                        var property = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        if (property != null && property.CanWrite)
                        {
                            property.SetValue(obj, Deserailze(propertyValue, property.PropertyType, obj), null);
                        }
                    }
                }
                return obj;
            }
            else if (objType.IsEnum)
            {
                return Enum.Parse(objType, option);
            }
            else
            {
                return Convert.ChangeType(option, objType);
            }
        }

        private static List<string> Split(string option)
        {
            var listOptions = new List<string>();
            var temp = new StringBuilder(); ;
            foreach (var op in option.Split(','))
            {
                if (temp.Length > 0)
                {
                    temp.Append(",");
                }
                temp.Append(op);
                if (!IsInBracketOrQuote(temp.ToString()))
                {
                    if (temp.Length > 0)
                    {
                        listOptions.Add(temp.ToString());
                        temp.Clear();
                    }
                }
            }
            
            return listOptions;
        }

        private static bool IsInBracketOrQuote(string str)
        {
            if (str.Split('{').Length != str.Split('}').Length)
            {
                return true;
            }
            if (str.Split('[').Length != str.Split(']').Length)
            {
                return true;
            }
            if (str.Split('\'').Length % 2 == 0)
            {
                return true;
            }
            return false;
        }
    }
    [JQDataFormEditorAttribute(JQEditorControl.Text)]
    public class JQTextBox : JQControl
    {
        public JQTextBox()
        {
            InputType = InputTypeMode.text;
        }
        public override void Render(HtmlTextWriter writer)
        {
            if (InputType != InputTypeMode.text)
                writer.AddAttribute(HtmlTextWriterAttribute.Type, InputType.ToString().ToLower());
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("inputType:'{0}'", InputType.ToString()));
                //options.Add(string.Format("mode:'{0}'", Mode.ToString()));
                return string.Join(",", options);
            }
        }
        [Category("Infolight")]
        public InputTypeMode InputType { get; set; }
        public enum InputTypeMode { text, email, url, tel, number, date, time, datetime, month }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Selects)]
    public class JQSelects : JQControl, IJQDataSourceProvider
    {
        public JQSelects()
        {
            Items = new JQCollection<JQDataItem>(this);
            Mode = JQSelectMode.dialog;
            CacheMode = CacheModeType.None;
            CacheGlobal = false;
        }

        [Category("Infolight")]
        public JQCollection<JQDataItem> Items { get; set; }

        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    var remoteNames = RemoteName.Split('.');
                    if (remoteNames.Length == 2)
                    {
                        return remoteNames[1];
                    }
                }
                return string.Empty;
            }
            set { }
        }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DisplayMember { get; set; }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }
        [Category("Infolight")]
        public JQSelectMode Mode { get; set; }

        [Category("Infolight")]
        public string OnSelect { get; set; }

        [Category("Infolight")]
        public CacheModeType CacheMode { get; set; }

        [Category("Infolight")]
        public bool CacheGlobal { get; set; }

        private string cacheDateTimeField;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string CacheDateTimeField
        {
            get
            {
                return cacheDateTimeField;
            }
            set
            {
                cacheDateTimeField = value;
            }
        }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Selects);
            if (Mode == JQSelects.JQSelectMode.dropdown)
            {
                writer.AddAttribute(JQProperty.DataNativeMenu, bool.TrueString.ToLower());
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "font-size:large");
            }
            else
            {
                writer.AddAttribute(JQProperty.DataNativeMenu, bool.FalseString.ToLower());
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Select);
            foreach (var item in Items)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Value, item.Value);
                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                writer.Write(item.Text);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    options.Add(string.Format("remoteName:'{0}'", RemoteName));
                    options.Add(string.Format("tableName:'{0}'", DataMember));
                    options.Add(string.Format("displayMember:'{0}'", DisplayMember));
                    options.Add(string.Format("valueMember:'{0}'", ValueMember));
                    options.Add(string.Format("cacheMode:'{0}'", CacheMode.ToString().ToLower()));
                    options.Add(string.Format("cacheGlobal:{0}", CacheGlobal.ToString().ToLower()));
                    if (!string.IsNullOrEmpty(CacheDateTimeField))
                    {
                        options.Add(string.Format("cacheDateTimeField:'{0}'", CacheDateTimeField));
                    }
                }
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                //options.Add(string.Format("mode:'{0}'", Mode.ToString()));
                return string.Join(",", options);
            }
        }
        public enum JQSelectMode
        {
            dialog, dropdown
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Date)]
    public class JQDate : JQControl
    {
        public JQDate()
        {
            Format = "yyyy-mm-dd";
        }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataTheme, "b");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.DateBox);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Datebox);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "date");
            var sourceDataOptions = string.Format("{{\"mode\":\"calbox\", \"overrideDateFormat\":\"{0}\",\"lockInput\":{1}{2}}}", ChangeFormat(Format), LockInput.ToString().ToLower(),(ShowYearSelect?",\"calUsePickers\": true, \"calNoHeader\": true":""));
            writer.AddAttribute(JQProperty.SourceDataOptions, sourceDataOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        private string ChangeFormat(string format)
        {
            return format.Replace("yyyy", "%Y").Replace("mm", "%m").Replace("dd", "%d");
        }
       
        public JQDateMode Mode { get; set; }
        [Category("Infolight")]
        public string Format { get; set; }

        private bool _lockInput = true;
        [Category("Infolight")]
        public bool LockInput
        {
            get { return _lockInput; }
            set { _lockInput = value; }
        }
        private bool _showYearSelect = false;
        [Category("Infolight")]
        public bool ShowYearSelect
        {
            get { return _showYearSelect; }
            set { _showYearSelect = value; }
        }
        private JQDateTimeDataType _DataType = JQDateTimeDataType.datetime;
        [Category("Infolight")]
        public JQDateTimeDataType DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }
        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("format:'{0}'", Format));
                options.Add(string.Format("lockInput:{0}", LockInput.ToString().ToLower()));
                options.Add(string.Format("showYearSelect:{0}", ShowYearSelect.ToString().ToLower()));
                options.Add(string.Format("dataType:'{0}'", DataType.ToString().ToLower())); 
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.YearMonth)]
    public class JQYearMonth: JQControl
    {
        public JQYearMonth()
        {
            Format = "yyyy-mm";
        }

        private string ChangeFormat(string format)
        {
            return format.Replace("yyyy", "%Y").Replace("mm", "%m").Replace("dd", "%d");
        }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataTheme, "b");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.DateBox);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Datebox);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "date");
            var sourceDataOptions = string.Format("{{\"mode\":\"datebox\", \"overrideDateFormat\": \"{0}\",\"overrideDateFieldOrder\":[\"y\", \"m\"]}}",  ChangeFormat(Format));
            writer.AddAttribute(JQProperty.SourceDataOptions, sourceDataOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        [Category("Infolight")]
        public string Format { get; set; }

        public override string EditorOptions
        {
            get
            {
                
                return "";
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.TimeSpinner)]
    public class JQTimeSpinner : JQControl
    {
        public JQTimeSpinner()
        {
        }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataTheme, "b");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.TimeSpinner);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Datebox);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "date");
            //var sourceDataOptions = string.Format("{{\"mode\":\"timebox\", \"lockInput\":{0}}}", LockInput.ToString().ToLower());
            var sourceDataOptions = "{\"mode\":\"timeflipbox\", \"overrideTimeFormat\": 24,\"timeOutput\":\"%k:%M\"}";
            writer.AddAttribute(JQProperty.SourceDataOptions, sourceDataOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
        //private bool _lockInput = true;
        //[Category("Infolight")]
        //public bool LockInput
        //{
        //    get { return _lockInput; }
        //    set { _lockInput = value; }
        //}
        public override string EditorOptions
        {
            get
            {
                //var options = new List<string>();
                //options.Add(string.Format("lockInput:{0}", LockInput.ToString().ToLower()));
                //return string.Join(",", options);
                return "";
            }
        }
    }

    public enum JQDateMode
    {
        date, time
    }
    public enum JQDateTimeDataType
    {
        datetime, varchar8
    }
    [JQDataFormEditorAttribute(JQEditorControl.FlipSwitch)]
    public class JQFlipSwitch : JQControl
    {
        public JQFlipSwitch()
        {
            OnText = "Yes";
            OnValue = "true";
            OffText = "No";
            OffValue = "false";
        }

        [Category("Infolight")]
        public string OnText { get; set; }
        [Category("Infolight")]
        public string OnValue { get; set; }
        [Category("Infolight")]
        public string OffText { get; set; }
        [Category("Infolight")]
        public string OffValue { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.FlipSwitch);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Slider);
            writer.RenderBeginTag(HtmlTextWriterTag.Select);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, OffValue);
            writer.RenderBeginTag(HtmlTextWriterTag.Option);
            writer.Write(OffText);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Value, OnValue);
            writer.RenderBeginTag(HtmlTextWriterTag.Option);
            writer.Write(OnText);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        internal void LoadProperties(string value)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                var options = ((string)value).Split(',');
                foreach (var op in options)
                {
                    var parts = op.Split(':');
                    if (parts.Length == 2)
                    {
                        var pname = parts[0].Trim();
                        var pvalue = parts[1].Trim();
                        if (pname == "onvalue")
                        {
                            this.OnValue = pvalue;
                        }
                        else if (pname == "offvalue")
                        {
                            this.OffValue = pvalue;
                        }
                        else if (pname == "ontext")
                        {
                            this.OnText = pvalue;
                        }
                        else if (pname == "offtext")
                        {
                            this.OffText = pvalue;
                        }
                    }
                }
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.TextArea)]
    public class JQTextArea : JQControl
    {
        public JQTextArea()
        {
            Rows = 4;        
        }

        [Category("Infolight")]
        public int Rows { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Cols, "40");
            writer.AddAttribute(HtmlTextWriterAttribute.Rows, Rows.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
            writer.RenderEndTag();
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Password)]
    public class JQPassword : JQControl
    {
        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "password");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.RadioButtons)]
    public class JQRadioButtons : JQControl, IJQDataSourceProvider
    {
        public JQRadioButtons() 
        {
            Items = new JQCollection<JQDataItem>(this);
        }

        [Category("Infolight")]
        public JQCollection<JQDataItem> Items { get; set; }
        
        [Category("Infolight")]
        public bool Horizontal { get; set; }

        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    var remoteNames = RemoteName.Split('.');
                    if (remoteNames.Length == 2)
                    {
                        return remoteNames[1];
                    }
                }
                return string.Empty;
            }
            set { }
        }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DisplayMember { get; set; }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }

        [Category("Infolight")]
        public string OnSelect { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.RadioButtons);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.ControlGroup);
            if (Horizontal)
            {
                writer.AddAttribute(JQProperty.DataType, "horizontal");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Fieldset);
            if (!string.IsNullOrEmpty(Caption))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Legend);
                writer.Write(Caption);
                writer.RenderEndTag();
            }
            for (int i = 0; i < Items.Count; i++)
            {
                var itemID = string.Format("{0}_{1}", ID, i);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, itemID);
                writer.AddAttribute(HtmlTextWriterAttribute.Name, string.Format("{0}_choice", ID));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                writer.AddAttribute(HtmlTextWriterAttribute.Value,  Items[i].Value);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.AddAttribute(JQProperty.For, itemID);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(Items[i].Text);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    options.Add(string.Format("remoteName:'{0}'", RemoteName));
                    options.Add(string.Format("tableName:'{0}'", DataMember));
                    options.Add(string.Format("displayMember:'{0}'", DisplayMember));
                    options.Add(string.Format("valueMember:'{0}'", ValueMember));
                }
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.CheckBoxes)]
    public class JQCheckBoxes : JQControl, IJQDataSourceProvider
    {
        public JQCheckBoxes()
        {
            Items = new JQCollection<JQDataItem>(this);
        }

        [Category("Infolight")]
        public JQCollection<JQDataItem> Items { get; set; }

        [Category("Infolight")]
        public bool Horizontal { get; set; }

        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    var remoteNames = RemoteName.Split('.');
                    if (remoteNames.Length == 2)
                    {
                        return remoteNames[1];
                    }
                }
                return string.Empty;
            }
            set { }
        }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DisplayMember { get; set; }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }

        [Category("Infolight")]
        public string OnSelect { get; set; }
        
        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Checkboxes);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.ControlGroup);
            if (Horizontal)
            {
                writer.AddAttribute(JQProperty.DataType, "horizontal");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Fieldset);
            if (!string.IsNullOrEmpty(Caption))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Legend);
                writer.Write(Caption);
                writer.RenderEndTag();
            }
            for (int i = 0; i < Items.Count; i++)
            {
                var itemID = string.Format("{0}_{1}", ID, i);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, itemID);
                writer.AddAttribute(HtmlTextWriterAttribute.Name, itemID);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, Items[i].Value);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.AddAttribute(JQProperty.For, itemID);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(Items[i].Text);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    options.Add(string.Format("remoteName:'{0}'", RemoteName));
                    options.Add(string.Format("tableName:'{0}'", DataMember));
                    options.Add(string.Format("displayMember:'{0}'", DisplayMember));
                    options.Add(string.Format("valueMember:'{0}'", ValueMember));
                }
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                } 
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.File)]
    public class JQFile : JQControl
    {
        public JQFile()
        {
            //Filter = "image/*";
            UploadFolder = "Image";
            FileSizeLimited = 500;
        }

        [Category("Infolight")]
        public string UploadFolder { get; set; }
        [Category("Infolight")]
        public string Filter { get; set; }

        [Category("Infolight")]
        public int FileSizeLimited { get; set; }

        [Category("Infolight")]
        public string OnSuccess { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.File);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "file");
            //if (!string.IsNullOrEmpty(Filter))
            //{
            //    writer.AddAttribute("accept", Filter);
            //}
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(UploadFolder))
                {
                    options.Add(string.Format("directory:'{0}'", UploadFolder));
                }
                if (!string.IsNullOrEmpty(Filter))
                {
                    options.Add(string.Format("filter:'{0}'", Filter));
                }
                if (FileSizeLimited > 0)
                {
                    options.Add(string.Format("fileSizeLimited:{0}", FileSizeLimited));
                }
                if (!string.IsNullOrEmpty(OnSuccess))
                {
                    options.Add(string.Format("onSuccess:{0}", OnSuccess));
                }
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Sliders)]
    public class JQSliders : JQControl
    {
        public JQSliders()
        {
            MinValue = 0;
            MaxValue = 100;
        }

        [Category("Infolight")]
        public int MinValue { get; set; }
        [Category("Infolight")]
        public int MaxValue { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("min",MinValue.ToString());
            writer.AddAttribute("max", MaxValue.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "range");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Qrcode)]
    public class JQQrcode : JQControl
    {
        public JQQrcode()
        {
            renderMode = JQQrcodeMode.table;
            //Width = 120;
            //Height = 120;
            Size = 120;
        }
        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "info-qrcode");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }
        [Category("Infolight")]
        public JQQrcodeMode renderMode { get; set; }
        //[Category("Infolight")]
        //public UInt32 Width { get; set; }
        //[Category("Infolight")]
        //public UInt32 Height { get; set; }
        [Category("Infolight")]
        public UInt32 Size { get; set; }
        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("render:'{0}'", renderMode.ToString()));
                //if(Width != 0)
                //    options.Add(string.Format("width:'{0}'", Width.ToString()));
                //if(Height != 0)
                //    options.Add(string.Format("height:'{0}'", Height.ToString()));
                if (Size != 0)
                {
                    options.Add(string.Format("width:{0}", Size.ToString()));
                    options.Add(string.Format("height:{0}", Size.ToString()));
                }
                return string.Join(",", options);
            }
        }
        public enum JQQrcodeMode
        {
            table, canvas
        }
    }

    public class JQDataItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Refval)]
    public class JQRefval : JQControl, IJQDataSourceProvider, IQueryObject
    {
        public JQRefval()
        {
            columns = new JQCollection<JQRefValColumn>(this);
            whereItems = new JQCollection<JQWhereItem>(this);
            columnMatches = new JQCollection<JQColumnMatch>(this);
            queryColumns = new JQCollection<JQQueryColumn>(this);
            PageSize = 20;
            DialogTitle = "Select Item";
            DialogWidth = 300;
            CacheMode = CacheModeType.None;
            CacheGlobal = false;
        }


        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    var remoteNames = RemoteName.Split('.');
                    if (remoteNames.Length == 2)
                    {
                        return remoteNames[1];
                    }
                }
                return string.Empty;
            }
            set { }
        }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DisplayMember { get; set; }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }

        private JQCollection<JQRefValColumn> columns;
        [Category("Infolight")]
        public JQCollection<JQRefValColumn> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        private JQCollection<JQWhereItem> whereItems;
        [Category("Infolight")]
        public JQCollection<JQWhereItem> WhereItems
        {
            get
            {
                return whereItems;
            }
            set
            {
                whereItems = value;
            }
        }

        private JQCollection<JQColumnMatch> columnMatches;
        [Category("Infolight")]
        public JQCollection<JQColumnMatch> ColumnMatches
        {
            get
            {
                return columnMatches;
            }
            set
            {
                columnMatches = value;
            }
        }

        private JQCollection<JQQueryColumn> queryColumns;

        [Category("Infolight")]
        public JQCollection<JQQueryColumn> QueryColumns
        {
            get
            {
                return queryColumns;
            }
            set
            {
                queryColumns = value;
            }
        }

        [Category("Infolight")]
        public int PageSize { get; set; }

        [Category("Infolight")]
        public string DialogTitle { get; set; }
        [Category("Infolight")]
        public int DialogWidth { get; set; }
        [Category("Infolight")]
        public string OnSelect { get; set; }

        [Category("Infolight")]
        public CacheModeType CacheMode { get; set; }

        [Category("Infolight")]
        public bool CacheGlobal { get; set; }

        private string cacheDateTimeField;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string CacheDateTimeField
        {
            get
            {
                return cacheDateTimeField;
            }
            set
            {
                cacheDateTimeField = value;
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string QueryObjectID
        {
            get
            {
                return string.Format("{0}_query", this.ID);
            }
        }

        public override void Render(HtmlTextWriter writer)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-grid-a");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-a");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            //writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            //writer.RenderBeginTag(HtmlTextWriterTag.Input);
            //writer.RenderEndTag();
            //writer.RenderEndTag();

            //var popupID = string.Format("{0}_popup", ID);
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-b");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
            //writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            //writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
            //writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Star);
            //writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
            //writer.AddAttribute(JQProperty.DataRel, JQDataRole.Popup);
            //writer.AddAttribute(JQProperty.DataTransition, JQDataTransition.Pop);
            ////writer.AddAttribute(JQProperty.For, ID);
            //writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("#{0}", popupID));
            //writer.RenderBeginTag(HtmlTextWriterTag.A);
            //writer.Write("Open");
            //writer.RenderEndTag();

            //writer.AddAttribute(HtmlTextWriterAttribute.Id, popupID);
            //writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
            //writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            //writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
            //writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Delete);
            //writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
            //writer.AddAttribute(JQProperty.DataRel, "back");
            //writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-btn-right");
            //writer.RenderBeginTag(HtmlTextWriterTag.A);
            //writer.Write("Close");
            //writer.RenderEndTag();
            //writer.AddAttribute(JQProperty.DataRole, JQDataRole.ListView);
            //writer.AddAttribute(JQProperty.DataInSet, bool.TrueString.ToLower());
            //writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            //writer.RenderEndTag();
            //writer.RenderEndTag();

            //writer.RenderEndTag();

            //writer.RenderEndTag();
            var popupID = string.Format("{0}_popup", ID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Refval);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$('#{0}').refval('open');", ID));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, popupID);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px", DialogWidth));
            writer.AddAttribute(JQProperty.DataOverlayTheme, JQDataTheme.A);
            writer.AddAttribute(JQProperty.DataDismissible, bool.FalseString.ToLower());
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-all");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);


            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
            writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Delete);
            writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
            writer.AddAttribute(JQProperty.DataRel, "back");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-btn-right");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("Close");
            writer.RenderEndTag();

            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Header);
            writer.AddAttribute(JQProperty.DataTheme, Theme);
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-top");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align:left");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(DialogTitle);
            writer.RenderEndTag();
            writer.RenderEndTag();


            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
            writer.AddAttribute(JQProperty.DataTheme, Theme);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-top:0px");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-bottom");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //add toolitem
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:50px;");
            writer.AddAttribute(HtmlTextWriterAttribute.Class,"ui-grid-c");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "80px");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-a");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            var previousPageItem = new JQToolItem() { Name = "grid-previous", Text = "Previous page", Icon = JQDataIcon.ArrowL, DataRole = JQDataRole.Button };
            previousPageItem.Render(writer);
            var nextPageItem = new JQToolItem() { Name = "grid-next", Text = "Next page", Icon = JQDataIcon.ArrowR, DataRole = JQDataRole.Button };
            nextPageItem.Render(writer);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-b");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //Andy说Mobile的RefVal,上方的Query Icon原來會打開Div，請在Icon左方增加一個Text，讓User可以輸入查詢條件(Fuzzy方式),Icon按下即可進行查詢。DIV不打开
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "fuzzy-query");
            writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-c");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "40px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            var queryItem = new JQToolItem() { Name = "grid-query", Text = "Query", Icon = JQDataIcon.Search, DataRole = JQDataRole.Button };
            queryItem.Render(writer);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-d");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "40px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //var clearItem = new JQToolItem() { Name = "grid-clear", Text = "Clear", Icon = JQDataIcon.Delete, DataRole = JQDataRole.Button };
            //clearItem.Render(writer);
            writer.RenderEndTag();
            
            writer.RenderEndTag();

            if (this.QueryColumns.Count == 0)
            {
                var clientInfo = (EFClientTools.EFServerReference.ClientInfo)System.Web.HttpContext.Current.Session["ClientInfo"];
                if (clientInfo != null && clientInfo.LogonResult == EFClientTools.EFServerReference.LogonResult.Logoned)
                {
                    var assemblyName = RemoteName.Split('.')[0];
                    //get dd
                    var columnDefinations = EFClientTools.ClientUtility.Client.GetColumnDefination(EFClientTools.ClientUtility.ClientInfo, assemblyName, DataMember, null)
                         .OfType<EFClientTools.EFServerReference.COLDEF>();
                    var caption = this.ValueMember;
                    var column = columnDefinations.FirstOrDefault(c => c.FIELD_NAME == this.ValueMember);
                    if (column != null && !string.IsNullOrEmpty(column.CAPTION))
                    {
                        caption = column.CAPTION;
                    }

                    this.QueryColumns.Add(new JQQueryColumn() { FieldName = this.ValueMember, Caption = caption });

                    caption = this.DisplayMember;
                    column = columnDefinations.FirstOrDefault(c => c.FIELD_NAME == this.DisplayMember);
                    if (column != null && !string.IsNullOrEmpty(column.CAPTION))
                    {
                        caption = column.CAPTION;
                    }
                    this.QueryColumns.Add(new JQQueryColumn() { FieldName = this.DisplayMember, Caption = caption });
                }
            }
            if (this.QueryColumns.Count > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.QueryObjectID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Query);
                writer.AddAttribute(JQProperty.DataTheme, Theme);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin-bottom:15px;margin-top:-20px;display:none");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                foreach (var column in this.QueryColumns)
                {
                    column.Render(writer);
                }
                writer.RenderEndTag();
            }

            writer.AddAttribute(JQProperty.DataRole, JQDataRole.ListView);
            writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
            //writer.AddAttribute(JQProperty.DataDividerTheme, Theme); 
            //writer.AddAttribute(JQProperty.DataInSet, bool.TrueString.ToLower());
            //writer.AddAttribute(JQProperty.DataFilter, bool.TrueString.ToLower());
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:30px;margin-top:20px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            previousPageItem.Render(writer);
            nextPageItem.Render(writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("displayMember:'{0}'", DisplayMember));
                options.Add(string.Format("valueMember:'{0}'", ValueMember));
                options.Add(string.Format("pageSize:{0}", PageSize));
                options.Add(string.Format("cacheMode:'{0}'", CacheMode.ToString().ToLower()));
                options.Add(string.Format("cacheGlobal:{0}", CacheGlobal.ToString().ToLower()));
                if (!string.IsNullOrEmpty(CacheDateTimeField))
                {
                    options.Add(string.Format("cacheDateTimeField:'{0}'", CacheDateTimeField));
                }

                var columnOptions = new List<string> { };
                foreach (var column in Columns)
                {
                    columnOptions.Add(string.Format("{{field:'{0}',caption:'{1}'}}", column.FieldName, column.Caption));    
                }
                options.Add(string.Format("columns:[{0}]", string.Join(",", columnOptions)));


                var whereItemsOptions = new List<string> { };
                foreach (var item in WhereItems)
                {
                    whereItemsOptions.Add(string.Format("{{field:'{0}',whereValue:{{{1}}}}}", item.FieldName, item.Value));
                }
                options.Add(string.Format("whereItems:[{0}]", string.Join(",", whereItemsOptions)));

                var columnMatchOptions = new List<string> { };
                foreach (var column in ColumnMatches)
                {
                    columnMatchOptions.Add(string.Format("{{field:'{0}',matchValue:{{{1}}}}}", column.TargetFieldName, column.Value));
                }
                options.Add(string.Format("columnMatches:[{0}]", string.Join(",", columnMatchOptions)));

                //if (this.QueryColumns.Count > 0)
                //{
                options.Add(string.Format("queryObjectID:'#{0}'", QueryObjectID));
                //}
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                return string.Join(",", options);
            }
        }

        internal void LoadProperties(string value)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                value = value.Substring(1, value.Length - 2);
                var options = ((string)value).Split(',');
                var op = string.Empty;
                foreach (var option in options)
                {
                    if (op.Length > 0)
                    {
                        op += ',';
                    }
                    op += option;
                    if (op.Split('{').Length != op.Split('}').Length)
                    {
                        continue;
                    }
                    if (op.Split('[').Length != op.Split(']').Length)
                    {
                        continue;
                    }
                    var index = op.IndexOf(':');

                    if (index > 0)
                    {
                        var pname = op.Substring(0, index).Trim();
                        var pvalue = op.Substring(index + 1).Trim('\'');
                        if (pname == "DialogTitle")
                        {
                            this.DialogTitle = pvalue;
                        }
                        else if (pname == "DialogWidth")
                        {
                            try
                            {
                                this.DialogWidth = int.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "RemoteName")
                        {
                            this.RemoteName = pvalue;
                        }
                        else if (pname == "ValueMember")
                        {
                            this.ValueMember = pvalue;
                        }
                        else if (pname == "DisplayMember")
                        {
                            this.DisplayMember = pvalue;
                        }
                        else if (pname == "Columns")
                        {
                            var columns = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(columns, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    var column = new JQRefValColumn();
                                    var columnOptions = match.Value.Split(',');
                                    foreach (var cop in columnOptions)
                                    {
                                        var cparts = cop.Split(':');
                                        if (cparts.Length == 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cparts[1].Trim('\'');
                                            if (cpname == "FieldName")
                                            {
                                                column.FieldName = cpvalue;
                                            }
                                            else if (cpname == "Caption")
                                            {
                                                column.Caption = cpvalue;
                                            }
                                        }
                                    }
                                    this.Columns.Add(column);
                                }
                            }
                        }
                        else if (pname == "columnMatches")
                        {
                            var columnMatches = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(columnMatches, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {

                                foreach (Match match in matches)
                                {
                                    var columnMatch = new JQColumnMatch();
                                    var columnOptions = match.Value.Split(',');
                                    foreach (var cop in columnOptions)
                                    {
                                        var cparts = cop.Split(':');
                                        if (cparts.Length == 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cparts[1].Trim('\'');
                                            if (cpname == "TargetFieldName")
                                            {
                                                columnMatch.TargetFieldName = cpvalue;
                                            }
                                            else if (cpname == "value")
                                            {
                                                if (cpvalue.StartsWith("remote["))
                                                {
                                                    columnMatch.RemoteMethod = true;
                                                    columnMatch.SourceMethod = cpvalue.Replace("remote[", "").Replace("]", "");
                                                }
                                                else if (cpvalue.StartsWith("client["))
                                                {
                                                    columnMatch.RemoteMethod = false;
                                                    columnMatch.SourceMethod = cpvalue.Replace("client[", "").Replace("]", "");
                                                }
                                                else
                                                {
                                                    columnMatch.SourceFieldName = cpvalue;
                                                }
                                            }
                                        }
                                    }
                                    this.ColumnMatches.Add(columnMatch);
                                }

                            }
                        }
                        else if (pname == "whereItems")
                        {
                            var whereItems = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(whereItems, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {

                                foreach (Match match in matches)
                                {
                                    var whereItem = new JQWhereItem();
                                    var columnOptions = match.Value.Split(',');
                                    var columnOp = "";
                                    foreach (var cop in columnOptions)
                                    {
                                        if (columnOp.Length > 0)
                                        {
                                            columnOp += ',';
                                        }
                                        columnOp += cop;
                                        if (columnOp.Split('\'').Length % 2 == 0)
                                        {
                                            continue;
                                        }


                                        var cparts = columnOp.Split(':');
                                        if (cparts.Length == 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cparts[1].Trim('\'');
                                            if (cpname == "FieldName")
                                            {
                                                whereItem.FieldName = cpvalue;
                                            }
                                            else if (cpname == "value")
                                            {
                                                if (cpvalue.StartsWith("remote["))
                                                {
                                                    whereItem.RemoteMethod = true;
                                                    cpvalue = cpvalue.Replace("remote[", "").Replace("]", "");
                                                    if (cpvalue.StartsWith("_"))
                                                    {
                                                        whereItem.WhereValue = cpvalue;
                                                    }
                                                    else
                                                    {
                                                        whereItem.WhereMethod = cpvalue;
                                                    }
                                                }
                                                else if (cpvalue.StartsWith("client["))
                                                {
                                                    whereItem.RemoteMethod = false;
                                                    whereItem.WhereMethod = cpvalue.Replace("client[", "").Replace("]", "");
                                                }
                                                else if (cpvalue.StartsWith("row["))
                                                {
                                                    whereItem.RemoteMethod = false;
                                                    whereItem.WhereField = cpvalue.Replace("row[", "").Replace("]", "");
                                                }
                                                else
                                                {
                                                    whereItem.WhereValue = cpvalue;
                                                }
                                            }
                                        }
                                        columnOp = string.Empty;
                                    }
                                    this.WhereItems.Add(whereItem);
                                }

                            }
                        }
                    }
                    op = string.Empty;
                }
            }
        }
    }

    public class JQRefValColumn : JQCollectionItem, IJQDataSourceProvider
    {
        private string caption;
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Caption
        {
            get
            {
                if (string.IsNullOrEmpty(caption))
                {
                    return FieldName;
                }
                else
                {
                    return caption;
                }
            }
            set
            {
                caption = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                return this.FieldName;
            }
            else
            {
                return base.ToString();
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }

    public class JQWhereItem : JQCollectionItem, IJQDataSourceProvider
    {
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        [Category("Infolight")]
        public string WhereValue { get; set; }

        [Category("Infolight")]
        public string WhereMethod { get; set; }

        [Category("Infolight")]
        public bool RemoteMethod { get; set; }

        [Category("Infolight")]
        [Editor(typeof(ParentFieldEditor), typeof(UITypeEditor))]
        public string WhereField { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                if (!string.IsNullOrEmpty(this.WhereValue))
                {
                    if (this.WhereValue.StartsWith("_"))
                    {
                        values.Add("type:'remote'");
                        values.Add(string.Format("value:['{0}']", WhereValue));
                    }
                    else
                    {
                        values.Add("type:'constant'");
                        values.Add(string.Format("value:['{0}']", WhereValue));
                    }
                }
                else if (!string.IsNullOrEmpty(WhereField))
                {
                    values.Add("type:'row'");
                    values.Add(string.Format("value:['{0}']", WhereField));
                }
                else if (!string.IsNullOrEmpty(WhereMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add("type:'remote'");
                        values.Add(string.Format("value:['{0}']", WhereMethod));
                    }
                    else
                    {
                        values.Add("type:'client'");
                        values.Add(string.Format("value:['{0}']", WhereMethod));
                    }
                }
                return string.Join(",", values);
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }

    public class JQColumnMatch : JQCollectionItem, IJQDataSourceProvider
    {
        public JQColumnMatch()
        {
            RemoteMethod = true;
        }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string SourceFieldName { get; set; }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ParentFieldEditor), typeof(UITypeEditor))]
        public string TargetFieldName { get; set; }


        /// <summary>
        /// 方法
        /// </summary>
        [Category("Infolight")]
        public string SourceMethod { get; set; }

        /// <summary>
        /// 是否后台方法
        /// </summary>
        [Category("Infolight")]
        public bool RemoteMethod { get; set; }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                if (!string.IsNullOrEmpty(this.SourceFieldName))
                {
                    values.Add("type:'row'");
                    values.Add(string.Format("value:['{0}']", SourceFieldName));
                }
                else
                {
                    if (RemoteMethod)
                    {
                        values.Add("type:'remote'");
                        values.Add(string.Format("value:['{0}']", SourceMethod));
                    }
                    else
                    {
                        values.Add("type:'client'");
                        values.Add(string.Format("value:['{0}']", SourceMethod));
                    }
                }
                return string.Join(",", values);
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(TargetFieldName))
            {
                return this.TargetFieldName;
            }
            else
            {
                return base.ToString();
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }

    [JQDataFormEditorAttribute(JQEditorControl.Geolocation)]
    public class JQGeolocation : JQControl
    {
        //[Category("Infolight")]
        public bool EnableHighAcuracy { get; set; }


        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Geolocation);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("enableHighAcuracy:{0}", EnableHighAcuracy.ToString().ToLower()));
                options.Add(string.Format("geolocation:{0}", bool.TrueString.ToLower()));
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Map)]
    public class JQMap : JQControl
    {
        public JQMap()
        {
            Geolocation = true;
        }

        //[Category("Infolight")]
        public bool EnableHighAcuracy { get; set; }

        [Category("Infolight")]
        public bool Geolocation { get; set; }

        [Category("Infolight")]
        public bool Address { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Map);
            //writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            //writer.RenderBeginTag(HtmlTextWriterTag.Input);
            //writer.RenderEndTag();


            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Map);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.None);
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:300px;height:200px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("enableHighAcuracy:{0}", EnableHighAcuracy.ToString().ToLower()));
                options.Add(string.Format("geolocation:{0}", Geolocation.ToString().ToLower()));
                options.Add(string.Format("address:{0}", Address.ToString().ToLower()));
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Scan)]
    public class JQScan : JQControl
    {
        [Category("Infolight")]
        public bool AutoOpen { get; set; }
        [Category("Infolight")]
        public string OnScan { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Scan);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("autoOpen:{0}", AutoOpen.ToString().ToLower()));
                options.Add(string.Format("onScan:{0}", OnScan));
                return string.Join(",", options);
            }
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Call)]
    public class JQCall : JQControl
    {
        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Call);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }
    


    [JQDataFormEditorAttribute(JQEditorControl.Capture)]
    public class JQCapture : JQControl
    {
        public JQCapture()
        {
            UploadFolder = "upload_files";
            FileSizeLimited = 1000;
            Quality = 100;
        }

        [Category("Infolight")]
        public String UploadFolder { get; set; }
        [Category("Infolight")]
        public int FileSizeLimited { get; set; }
        [Category("Infolight")]
        public int Quality { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "info-capture");
            writer.AddAttribute("uploadFolder", this.UploadFolder);
            writer.AddAttribute("fileSizeLimited", this.FileSizeLimited.ToString());
            writer.AddAttribute("quality", this.Quality.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Place)]
    public class JQPlace : JQControl
    {
        public JQPlace()
        {
            GeoFormat = GeoFormatFormat.Geo;
        }
        public enum GeoFormatFormat
        {
            Geo, String
        }

        [Category("Infolight")]
        public GeoFormatFormat GeoFormat { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            var popupID = string.Format("{0}_popup", ID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Place);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$('#{0}').place('open');", ID));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, popupID);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
            writer.AddAttribute(JQProperty.DataOverlayTheme, JQDataTheme.A);
            writer.AddAttribute(JQProperty.DataDismissible, bool.FalseString.ToLower());
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-all");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            #region back button
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
            writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Delete);
            writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
            writer.AddAttribute(JQProperty.DataRel, "back");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-btn-right");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("Close");
            writer.RenderEndTag();
            #endregion
            #region header
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Header);
            writer.AddAttribute(JQProperty.DataTheme, Theme);
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-top");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align:left");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write("JQPlace");
            writer.RenderEndTag();
            writer.RenderEndTag();
            #endregion

            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
            writer.AddAttribute(JQProperty.DataTheme, Theme);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-top:0px");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-bottom");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            #region searchbox
            var searchID = string.Format("{0}_search", ID);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, searchID);
            writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "search");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            #endregion
            #region mapdiv
            var mapID = string.Format("{0}_map", ID);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, mapID);
            writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            #endregion
            #region ul
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.ListView);
            writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.RenderEndTag();
            #endregion

            writer.RenderEndTag();
            writer.RenderEndTag();

        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("geoFormat:'{0}'", GeoFormat.ToString()));
                return string.Join(",", options);
            }
        }

        internal void LoadProperties(string value)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                value = value.Substring(1, value.Length - 2);
                var options = ((string)value).Split(',');
                var op = string.Empty;
                foreach (var option in options)
                {
                    if (op.Length > 0)
                    {
                        op += ',';
                    }
                    op += option;
                    if (op.Split('{').Length != op.Split('}').Length)
                    {
                        continue;
                    }
                    if (op.Split('[').Length != op.Split(']').Length)
                    {
                        continue;
                    }
                    var index = op.IndexOf(':');

                    if (index > 0)
                    {
                        var pname = op.Substring(0, index).Trim();
                        var pvalue = op.Substring(index + 1).Trim('\'');
                    }
                    op = string.Empty;
                }
            }
        }
    }


    [JQDataFormEditorAttribute(JQDisplayControl.Relation)]
    public class JQRelation : JQControl, IJQDataSourceProvider
    {
        public JQRelation()
        {
            Items = new JQCollection<JQDataItem>(this);
            WhereItems = new JQCollection<JQWhereItem>(this);
        }

        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    var remoteNames = RemoteName.Split('.');
                    if (remoteNames.Length == 2)
                    {
                        return remoteNames[1];
                    }
                }
                return string.Empty;
            }
            set { }
        }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DisplayMember { get; set; }
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }

        [Category("Infolight")]
        public JQCollection<JQDataItem> Items { get; set; }


        [Category("Infolight")]
        public JQCollection<JQWhereItem> WhereItems { get; set; }
    }

    [JQDataFormEditorAttribute(JQEditorControl.Contacts)]
    public class JQContacts : JQControl
    {
        public enum ValueType
        {
            PhoneNumber, ContactName, All
        }

        public JQContacts()
        {
            DialogTitle = "Contacts";
            DialogWidth = 250;
            ValueMemberType = ValueType.PhoneNumber;
        }

        [Category("Infolight")]
        public string DialogTitle { get; set; }
        [Category("Infolight")]
        public int DialogWidth { get; set; }
        [Category("Infolight")]
        public ValueType ValueMemberType { get; set; }

        [Category("Infolight")]
        public bool ReadOnly { get; set; }

         public override void Render(HtmlTextWriter writer)
        {
            var popupID = string.Format("{0}_popup", ID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Contacts);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$('#{0}').contacts('open');", ID));
            writer.AddAttribute("valueMemberType", ValueMemberType.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, popupID);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px", DialogWidth));
            writer.AddAttribute(JQProperty.DataOverlayTheme, JQDataTheme.A);
            writer.AddAttribute(JQProperty.DataDismissible, bool.FalseString.ToLower());
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-all");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);


            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
            writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
            writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Delete);
            writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
            writer.AddAttribute(JQProperty.DataRel, "back");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-btn-right");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("Close");
            writer.RenderEndTag();

            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Header);
            writer.AddAttribute(JQProperty.DataTheme, Theme);
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-top");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align:left");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(DialogTitle);
            writer.RenderEndTag();
            writer.RenderEndTag();


            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
            writer.AddAttribute(JQProperty.DataTheme, Theme);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-top:0px");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-corner-bottom");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(JQProperty.DataRole, JQDataRole.ListView);
            writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
            writer.AddAttribute("data-filter", "true");
            //writer.AddAttribute(JQProperty.DataDividerTheme, Theme); 
            //writer.AddAttribute(JQProperty.DataInSet, bool.TrueString.ToLower());
            //writer.AddAttribute(JQProperty.DataFilter, bool.TrueString.ToLower());
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
    [JQDataFormEditorAttribute(JQEditorControl.Signature)]
    public class JQSignature : JQControl
    {
        public JQSignature()
        {
            Format = SignatureFormat.Image;
            Height = 40;
        }

        public enum SignatureFormat
        {
            Image, svg, /*base30,*/ svgbase64
        }

        [Category("Infolight")]
        public UInt32 Height { get; set; }

        [Category("Infolight")]
        public SignatureFormat Format { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Signature + " ui-input-text ui-shadow-inset ui-corner-all ui-btn-shadow ui-body-b ui-mini");
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.None);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString() + "px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }

        public override string EditorOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("format:'{0}'", Format.ToString().ToLower()));
                options.Add(string.Format("height:{0}", Height.ToString()));
                return string.Join(",", options);
            }
        }
    }
}
