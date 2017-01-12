using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using EFClientTools.EFServerReference;
using FLCore;
using JQChartTools;

public class JsonHelper
{
    public static JObject CreateTreeNodeObject(string id, string text, string iconCls, bool open, params DictionaryEntry[] parameters)
    {
        var node = new JObject();
        if (!string.IsNullOrEmpty(id))
        {
            node["id"] = new JValue(id);
        }
        node["text"] = new JValue(text);
        node["iconCls"] = new JValue(iconCls);
        if (open)
        {
            node["state"] = new JValue("open");
        }
        else
        {
            node["state"] = new JValue("closed");
        }
        if (parameters.Length > 0)
        {
            var attributes = new JObject();
            for (int i = 0; i < parameters.Length; i++)
            {
                attributes[parameters[i].Key.ToString()] = parameters[i].Value.ToString();
            }
            node["attributes"] = attributes;
        }
        return node;
    }

    public static JArray CreateComboItems(List<string> list)
    {
        var items = new JArray();
        foreach (var str in list)
        {
            var item = new JObject();
            item["value"] = str;
            items.Add(item);
        }
        return items;
    }

    public static JArray CreateComboItems(List<JObject> list)
    {
        var items = new JArray();
        foreach (var obj in list)
        {
            var item = new JObject();
            foreach (var p in obj.Properties())
            {
                item[p.Name] = obj[p.Name];
            }
            items.Add(item);
        }
        return items;
    }

    const string KEY_PREFIX = "CopyKeyOfTable_";

    public static string CreateGridItems(System.Data.DataTable dataTable, int rowCount)
    {
        //主键可修改功能
        foreach (var key in dataTable.PrimaryKey)
        {
            dataTable.Columns.Add(new System.Data.DataColumn() { ColumnName = string.Format("{0}{1}", KEY_PREFIX, key.ColumnName), DataType = key.DataType, Expression = key.ColumnName });
        }
        var json = string.Format("{{\"total\":{0},\"rows\":{1}}}", rowCount, JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented));
        return json;
    }

    public static UpdateRow CreateUpdateRow(JObject row, DataRowState rowState)
    {
        //主键可修改功能
        var updateRow = new UpdateRow()
        {
            RowState = rowState,
            NewValues = new Dictionary<string, object>(),
            OldValues = new Dictionary<string, object>()
        };
        foreach (var property in row.Properties())
        {
            if (rowState != DataRowState.Added)
            {
                if (row[string.Format("{0}{1}", KEY_PREFIX, property.Name)] != null)
                {
                    updateRow.OldValues.Add(property.Name, (row[string.Format("{0}{1}", KEY_PREFIX, property.Name)] as JValue).Value);
                }
                else
                {
                    updateRow.OldValues.Add(property.Name, (row[property.Name] as JValue).Value);
                }
            }
            if (rowState != DataRowState.Deleted)
            {
                updateRow.NewValues.Add(property.Name, (row[property.Name] as JValue).Value);
            }
        }
        return updateRow;
    }

    public static JArray CreatePropertyItems(string assemblyName, string typeName, string parentPropertyName)
    {
        return CreatePropertyItems(assemblyName, typeName, parentPropertyName, null);
    }

    public static JArray CreatePropertyItems(string assemblyName, string typeName, string parentPropertyName, Dictionary<string, object> defaultValues)
    {
        var type = GetType(assemblyName, typeName);
        return CreatePropertyItems(type, parentPropertyName, defaultValues);
    }

    public static JArray CreatePropertyItems(string editor, string parentPropertyType)
    {
        var type = GetEditControlType(editor, parentPropertyType);
        if (type != null)
        {
            return CreatePropertyItems(type, "EditOptions", null);
        }
        else
        {
            return new JArray();
        }
    }

    private static JArray CreatePropertyItems(Type type, string parentPropertyName, Dictionary<string, object> defaultValues)
    {
        var array = new JArray();
        var obj = type.GetConstructor(new Type[] { }).Invoke(null);
        if (defaultValues != null)
        {
            foreach (var defaultValue in defaultValues)
            {
                var property = type.GetProperty(defaultValue.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                if (property != null && property.CanWrite)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(obj, Enum.Parse(property.PropertyType, defaultValue.Value.ToString()), null);
                    }
                    else
                    {
                        property.SetValue(obj, Convert.ChangeType(defaultValue.Value, property.PropertyType), null);
                    }
                }
            }
        }
        if (string.IsNullOrEmpty(parentPropertyName))
        {
            array.Add(GetIDObject());
        }
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            if (CanEdit(property))
            {
                var propertyObj = new JObject();
                propertyObj["name"] = property.Name;
                propertyObj["value"] = GetDefaultValue(obj, property);
                propertyObj["group"] = GetGroup(property);
                propertyObj["editor"] = GetEditor(property, parentPropertyName);
                array.Add(propertyObj);
            }
        }
        if (string.IsNullOrEmpty(parentPropertyName))
        {
            if (typeof(System.Web.UI.WebControls.WebControl).IsAssignableFrom(type))
            {
                var webControlProperties = new string[] { "Width", "Height" };
                foreach (var webControlProperty in webControlProperties)
                {
                    var property = type.GetProperty(webControlProperty, BindingFlags.Instance | BindingFlags.Public);
                    var overrideProperty = type.GetProperty(webControlProperty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    if (property != null && overrideProperty == null)
                    {
                        var propertyObj = new JObject();
                        propertyObj["name"] = property.Name;
                        propertyObj["value"] = GetDefaultValue(obj, property);
                        propertyObj["group"] = GetGroup(property);
                        propertyObj["editor"] = GetEditor(property, parentPropertyName);
                        array.Add(propertyObj);
                    }
                }
            }
        }

        if (type.BaseType != null && type.BaseType.Namespace == type.Namespace)
        {
            foreach (var property in type.BaseType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                var overrideProperty = type.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                if (CanEdit(property) && overrideProperty == null)
                {
                    var propertyObj = new JObject();
                    propertyObj["name"] = property.Name;
                    propertyObj["value"] = GetDefaultValue(obj, property);
                    propertyObj["group"] = GetGroup(property);
                    propertyObj["editor"] = GetEditor(property, parentPropertyName);
                    array.Add(propertyObj);
                }
            }
        }

        foreach (var eventInfo in type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            var propertyObj = new JObject();
            propertyObj["name"] = eventInfo.Name;
            propertyObj["value"] = new JValue("");
            propertyObj["group"] = GetGroup(eventInfo);
            propertyObj["editor"] = GetEventEditor(eventInfo);
            array.Add(propertyObj);
        }
        return array;
    }

    private static Assembly GetAssembly(string assemblyName)
    {
        if (string.IsNullOrEmpty(assemblyName))
        {
            throw new ArgumentNullException("assemblyName");
        }
        var serverAssembly = Assembly.GetExecutingAssembly();
        if (serverAssembly == null)
        {
            throw new NotSupportedException("Design time not support.");
        }
        var referenceName = serverAssembly.GetReferencedAssemblies().FirstOrDefault(c => c.Name.Equals(assemblyName));
        if (referenceName == null)
        {
            throw new Exception(string.Format("assembly:'{0}' not found.", assemblyName));
        }
        else
        {
            var assembly = Assembly.Load(referenceName);
            return assembly;
        }
    }

    public static Type GetType(string assemblyName, string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            throw new ArgumentNullException("typeName");
        }
        var assembly = GetAssembly(assemblyName);
        return assembly.GetType(typeName, true);
    }

    public static Type GetEditControlType(string editor, string parentPropertyType)
    {
        var assemblyName = parentPropertyType.Split('.')[0];
        if (assemblyName == "JQClientTools")
        {
            switch (editor)
            {
                case JQClientTools.JQEditorControl.AutoComplete: return typeof(JQClientTools.JQAutoComplete);
                case JQClientTools.JQEditorControl.CheckBox: return typeof(JQClientTools.JQCheckBox);
                case JQClientTools.JQEditorControl.ComboBox: return typeof(JQClientTools.JQComboBox);
                case JQClientTools.JQEditorControl.ComboGrid: return typeof(JQClientTools.JQComboGrid);
                case JQClientTools.JQEditorControl.DateBox: return typeof(JQClientTools.JQDateBox);
                case JQClientTools.JQEditorControl.FileUpload: return typeof(JQClientTools.JQFileUpload);
                case JQClientTools.JQEditorControl.Options: return typeof(JQClientTools.JQOptions);
                case JQClientTools.JQEditorControl.Qrcode: return typeof(JQClientTools.JQQrcode);
                case JQClientTools.JQEditorControl.RefValBox: return typeof(JQClientTools.JQRefval);
                case JQClientTools.JQEditorControl.TextArea: return typeof(JQClientTools.JQTextArea);
                case JQClientTools.JQEditorControl.TimeSpinner: return typeof(JQClientTools.JQTimeSpinner);
            }
        }
        else if (assemblyName == "JQMobileTools")
        {
            if (parentPropertyType == "JQMobileTools.JQGridColumn")
            {
                return typeof(JQMobileTools.JQRelation);
            }
            else
            {
                switch (editor)
                {
                    case JQMobileTools.JQEditorControl.CheckBoxes: return typeof(JQMobileTools.JQCheckBoxes);
                    case JQMobileTools.JQEditorControl.Date: return typeof(JQMobileTools.JQDate);
                    case JQMobileTools.JQEditorControl.File: return typeof(JQMobileTools.JQFile);
                    case JQMobileTools.JQEditorControl.FlipSwitch: return typeof(JQMobileTools.JQFlipSwitch);
                    case JQMobileTools.JQEditorControl.Geolocation: return typeof(JQMobileTools.JQGeolocation);
                    case JQMobileTools.JQEditorControl.Map: return typeof(JQMobileTools.JQMap);
                    case JQMobileTools.JQEditorControl.Password: return typeof(JQMobileTools.JQPassword);
                    case JQMobileTools.JQEditorControl.Qrcode: return typeof(JQMobileTools.JQQrcode);
                    case JQMobileTools.JQEditorControl.RadioButtons: return typeof(JQMobileTools.JQRadioButtons);
                    case JQMobileTools.JQEditorControl.Refval: return typeof(JQMobileTools.JQRefval);
                    case JQMobileTools.JQEditorControl.Selects: return typeof(JQMobileTools.JQSelects);
                    case JQMobileTools.JQEditorControl.Sliders: return typeof(JQMobileTools.JQSliders);
                    case JQMobileTools.JQEditorControl.TextArea: return typeof(JQMobileTools.JQTextArea);
                }
            }
        }
        return null;
    }

    private static Type[] GetTypes(string assemblyName, params Type[] baseTyps)
    {
        var assembly = GetAssembly(assemblyName);
        return assembly.GetTypes().Where(c =>
        {
            if (c.IsPublic)
            {
                foreach (var baseType in baseTyps)
                {
                    if (!baseType.IsAssignableFrom(c))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }).ToArray();
    }

    private static JObject GetIDObject()
    {
        var propertyObj = new JObject();
        propertyObj["name"] = "ID";
        propertyObj["value"] = string.Empty;
        propertyObj["group"] = "Default";
        var editor = new JObject();
        editor["type"] = "validatebox";
        var options = new JObject();
        options["required"] = new JValue(true);
        options["tipPosition"] = new JValue("left");
        editor["options"] = options;
        propertyObj["editor"] = editor;
        return propertyObj;
    }

    public static bool CanEdit(PropertyInfo property)
    {
        if (!property.CanRead)
        {
            return false;
        }

        var browsableAttribute = (BrowsableAttribute)Attribute.GetCustomAttribute(property, typeof(BrowsableAttribute), false);
        if (browsableAttribute != null && !browsableAttribute.Browsable)
        {
            return false;
        }

        var designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)Attribute.GetCustomAttribute(property, typeof(DesignerSerializationVisibilityAttribute), false);
        if (designerSerializationVisibilityAttribute != null && designerSerializationVisibilityAttribute.Visibility == DesignerSerializationVisibility.Hidden)
        {
            return false;
        }

        if (!property.CanWrite && !typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
        {
            return false;
        }

        return true;
    }

    private static JToken GetDefaultValue(object obj, PropertyInfo property)
    {
        object value = null;
        try
        {
            value = property.GetValue(obj, null);
        }
        catch { }
        if (value == null)
        {
            return new JValue("");
        }
        else if (property.PropertyType == typeof(int))
        {
            return new JValue((int)value);
        }
        else if (property.PropertyType == typeof(bool))
        {
            return new JValue(value.ToString().ToLower());
        }
        else if (property.PropertyType == typeof(System.Drawing.Font))
        {
            var font = (System.Drawing.Font)value;
            var fontValue = new List<string>();
            fontValue.Add(font.OriginalFontName);
            fontValue.Add(string.Format("{0}pt", font.Size));
            if (font.Style != System.Drawing.FontStyle.Regular)
            {
                fontValue.Add(font.Style.ToString());
            }
            return new JValue(string.Join(",", fontValue));
        }
        else if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
        {
            var items = new JArray();
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericArguments().Length == 1)
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];
                var toolItems = new ArrayList();
                if (itemType == typeof(JQClientTools.JQToolItem))
                {
                    //增加grid的默认toolitems
                    toolItems.AddRange(new JQClientTools.JQToolItem[] { JQClientTools.JQToolItem.InsertItem, JQClientTools.JQToolItem.UpdateItem, JQClientTools.JQToolItem.DeleteItem
                        , JQClientTools.JQToolItem.ApplyItem, JQClientTools.JQToolItem.CancelItem, JQClientTools.JQToolItem.QueryItem });
                }
                else if (itemType == typeof(JQMobileTools.JQToolItem))
                {
                    //增加grid的默认toolitems
                    toolItems.AddRange(new JQMobileTools.JQToolItem[] { JQMobileTools.JQToolItem.InsertItem, JQMobileTools.JQToolItem.PreviousPageItem,  JQMobileTools.JQToolItem.NextPageItem
                        , JQMobileTools.JQToolItem.QueryItem, JQMobileTools.JQToolItem.RefreshItem, JQMobileTools.JQToolItem.BackItem });
                }
                foreach (var toolItem in toolItems)
                {
                    var item = new JObject();
                    var itemProperties = toolItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    foreach (var itemProperty in itemProperties)
                    {
                        if (CanEdit(itemProperty))
                        {
                            item[itemProperty.Name] = GetDefaultValue(toolItem, itemProperty);
                        }
                    }
                    items.Add(item);
                }
            }
            else if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
            {
                CodeParser helper = new CodeParser();
                foreach (var collectionItem in (IEnumerable)value)
                {
                    items.Add(helper.GetItemProperties(collectionItem));
                }
            }
            return items;
        }
        else
        {
            return new JValue(value.ToString());
        }
    }

    private static JToken GetGroup(PropertyInfo property)
    {
        var categoryAttribute = (CategoryAttribute)Attribute.GetCustomAttribute(property, typeof(CategoryAttribute), false);
        if (categoryAttribute != null)
        {
            return new JValue(categoryAttribute.Category);
        }
        else
        {
            return new JValue("Misc");
        }
    }

    private static JToken GetGroup(EventInfo eventInfo)
    {
        return new JValue("Event");
    }

    private static JObject GetEditor(PropertyInfo property, string parentPropertyName)
    {
        var obj = new JObject();
        obj["type"] = new JValue("validatebox");
        var options = new JObject();
        options["validType"] = new JValue("crossSiteScript");
        options["tipPosition"] = new JValue("left");
        obj["options"] = options;
        if (property.PropertyType == typeof(int))
        {
            obj["type"] = new JValue("numberbox");
        }
        else if (property.PropertyType == typeof(bool))
        {
            obj["type"] = new JValue("checkbox");
            //var options = new JObject();
            options["on"] = new JValue(bool.TrueString.ToLower());
            options["off"] = new JValue(bool.FalseString.ToLower());
            obj["options"] = options;
        }
        else if (property.PropertyType == typeof(System.Drawing.Font))
        {
            obj["type"] = new JValue("font");
        }
        else if (property.PropertyType.IsEnum)
        {
            var items = new List<string>();
            foreach (var fi in property.PropertyType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (Attribute.GetCustomAttribute(fi, typeof(ObsoleteAttribute), false) == null)
                {
                    items.Add(fi.Name);
                }
            };
            obj = GetComboBoxEditor(items, false);
        }
        else if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
        {
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericArguments().Length == 1)
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];

                obj = GetCollectionEditor(itemType, property.Name);
            }
            else if (typeof(InfoOwnerCollection).IsAssignableFrom(property.PropertyType))
            {
                var item = property.PropertyType.GetProperty("Item", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (item != null)
                {
                    var itemType = item.PropertyType;
                    obj = GetCollectionEditor(itemType, property.Name);
                }
            }
            else if (typeof(ApproveRightCollection).IsAssignableFrom(property.PropertyType))
            {
                var item = property.PropertyType.GetProperty("Item", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (item != null)
                {
                    var itemType = item.PropertyType;
                    obj = GetCollectionEditor(itemType, property.Name);
                }
            }
        }
        else if (typeof(Component).IsAssignableFrom(property.PropertyType))
        {
            obj = GetControlEditor(new Type[] { property.PropertyType });
        }
        else if (property.PropertyType.IsInterface)
        {
            var controlTypes = GetTypes("Srvtools", new Type[] { property.PropertyType, typeof(Component) });
            obj = GetControlEditor(controlTypes);
        }
        else if (property.PropertyType == typeof(string))
        {
            var attribute = (EditorAttribute)Attribute.GetCustomAttribute(property, typeof(EditorAttribute), false);
            if (attribute != null)
            {
                var editorType = Type.GetType(attribute.EditorTypeName);
                if (typeof(JQClientTools.PropertyDropDownEditor).IsAssignableFrom(editorType) || typeof(JQMobileTools.PropertyDropDownEditor).IsAssignableFrom(editorType) || typeof(JQChartTools.PropertyDropDownEditor).IsAssignableFrom(editorType))
                {
                    if (editorType == typeof(JQClientTools.DataMemberEditor) || editorType == typeof(JQMobileTools.DataMemberEditor) || editorType == typeof(JQChartTools.DataMemberEditor))
                    {
                        obj = GetSchemaEditor("DataMember");
                    }
                    else if (editorType == typeof(JQClientTools.FieldEditor) || editorType == typeof(JQMobileTools.FieldEditor) || editorType == typeof(JQChartTools.FieldEditor))
                    {
                        obj = GetSchemaEditor("Field", string.Empty);
                    }
                    else if (editorType == typeof(JQClientTools.ParentFieldEditor) || editorType == typeof(JQMobileTools.ParentFieldEditor))
                    {
                        obj = GetSchemaEditor("Field", "parent");
                    }
                    else if (editorType == typeof(JQClientTools.DataControlEditor))
                    {
                        obj = GetControlEditor(new Type[] { typeof(JQClientTools.JQDataGrid), typeof(JQClientTools.JQDataForm) });
                    }
                    else if (editorType == typeof(JQMobileTools.DataControlEditor))
                    {
                        obj = GetControlEditor(new Type[] { typeof(JQMobileTools.JQDataGrid), typeof(JQMobileTools.JQDataForm) });
                    }
                    else if (editorType == typeof(JQClientTools.FormControlEditor))
                    {
                        obj = GetControlEditor(new Type[] { typeof(JQClientTools.JQDataForm) });
                    }
                    else if (editorType == typeof(JQMobileTools.FormControlEditor))
                    {
                        obj = GetControlEditor(new Type[] { typeof(JQMobileTools.JQDataForm) });
                    }
                    else if (editorType == typeof(JQMobileTools.GridControlEditor))
                    {
                        obj = GetControlEditor(new Type[] { typeof(JQMobileTools.JQDataGrid) });
                    }
                    else if (editorType == typeof(JQClientTools.WindowControlEditor))
                    {
                        obj = GetControlEditor(new Type[] { typeof(JQClientTools.JQDialog) });
                    }
                    else
                    {
                        try
                        {
                            var items = (List<string>)editorType.GetMethod("GetListOfValues", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .Invoke(editorType.GetConstructors()[0].Invoke(null), new object[] { null });
                            obj = GetComboBoxEditor(items, true);
                        }
                        catch { }
                    }
                }
                else if (editorType == typeof(JQClientTools.RemoteNameEditor) || editorType == typeof(JQMobileTools.RemoteNameEditor) || editorType == typeof(JQChartTools.RemoteNameEditor))
                {
                    obj["type"] = new JValue("command");
                }
                else if (editorType == typeof(JQClientTools.EditorOptionsEditor) || editorType == typeof(JQMobileTools.EditorOptionsEditor))
                {
                    obj = GetEditorOptionEditor(property.DeclaringType.FullName);
                }
                else if (editorType == typeof(JQClientTools.ColorEditor))
                {
                    obj["type"] = new JValue("color");
                }
                else if (editorType == typeof(Srvtools.PropertyDropDownEditor))
                {
                    if (property.DeclaringType == typeof(Srvtools.ColumnItem))
                    {
                        if (parentPropertyName == "MasterColumns")
                        {
                            obj = GetSchemaEditor("Field", "master");
                        }
                        else if (parentPropertyName == "DetailColumns")
                        {
                            obj = GetSchemaEditor("Field", "detail");
                        }
                    }
                    else if (property.DeclaringType == typeof(Srvtools.TransFieldBase) && property.Name == "DesField")
                    {
                        obj = GetSchemaEditor("Field", "target");
                    }
                    else if (property.DeclaringType == typeof(Srvtools.InfoCommand) && property.Name == "EEPAlias")
                    {

                    }
                    else
                    {
                        obj = GetSchemaEditor("Field");
                    }
                }
                else if (editorType == typeof(Srvtools.TransTableNameEditor))
                {
                    obj["type"] = new JValue("tablename");
                }
                else if (editorType == typeof(Srvtools.CommandTextEditor))
                {
                    obj["type"] = new JValue("sql");
                }
                else if (editorType == typeof(FLTools.Base.WebFormNameEditor))
                {
                    obj["type"] = new JValue("webformname");
                }

                if (property.Name == "SendToField" || property.Name == "ExtGroupField" || property.Name == "ExtValueField"
                    || property.Name == "FLNavigatorField" || property.Name == "ParallelField" || property.Name == "SendToMasterField")
                {
                    obj["type"] = new JValue("schema");
                }
                else if (property.Name == "SendToRole" || property.Name == "ErrorToRole")
                {
                    obj["type"] = new JValue("sendtorole");
                }
                else if (property.Name == "SendToUser")
                {
                    obj["type"] = new JValue("sendtouser");
                }
                else if (property.Name == "Grade")
                {
                    obj["type"] = new JValue("orglevel");
                }
                else if (property.Name == "DetailsTableName")
                {
                    obj["type"] = new JValue("tablename");
                }
                else if (property.Name == "MenuID")
                {
                    obj["type"] = new JValue("menuid");
                }
                else if (property.Name == "DBAlias")
                {
                    obj["type"] = new JValue("dbalias");
                }
            }
        }
        return obj;
    }

    private static JObject GetCollectionEditor(Type itemType, string parentPropertyName)
    {
        var obj = new JObject();
        obj["type"] = new JValue("collection");
        var options = new JObject();
        options["assembly"] = new JValue(itemType.Assembly.FullName.Split(',')[0]);
        options["type"] = new JValue(itemType.FullName);
        options["parentPropertyName"] = parentPropertyName;
        options["captionfield"] = GetCaptionField(itemType);
        obj["options"] = options;
        return obj;
    }

    private static JToken GetCaptionField(Type itemType)
    {
        if (itemType == typeof(Srvtools.KeyItem))
        {
            return new JValue("KeyName");
        }
        else if (itemType == typeof(Srvtools.FieldAttr))
        {
            return new JValue("DataField");
        }
        else if (itemType == typeof(Srvtools.ServerModifyColumn))
        {
            return new JValue("ColumnName");
        }
        else if (itemType == typeof(JQClientTools.JQToolItem))
        {
            return new JValue("Text");
        }
        else if (itemType == typeof(JQMobileTools.JQToolItem))
        {
            return new JValue("Text");
        }
        return new JValue("FieldName");
    }

    private static JObject GetComboBoxEditor(List<string> items, bool editable)
    {
        var obj = new JObject();
        obj["type"] = new JValue("combobox");

        var options = new JObject();
        options["valueField"] = new JValue("value");
        options["textField"] = new JValue("value");
        options["panelHeight"] = new JValue("auto");
        options["editable"] = new JValue(editable);
        options["validType"] = new JValue("crossSiteScript");
        if (items != null)
        {
            var data = JsonHelper.CreateComboItems(items);
            options["data"] = data;
        }

        obj["options"] = options;
        return obj;
    }

    private static JObject GetEditorOptionEditor(string parentPropertyType)
    {
        var obj = new JObject();
        obj["type"] = new JValue("editoroption");
        var options = new JObject();
        options["parentPropertyType"] = parentPropertyType;
        obj["options"] = options;
        return obj;
    }

    private static JObject GetSchemaEditor(string type)
    {
        return GetSchemaEditor(type, string.Empty);
    }

    private static JObject GetSchemaEditor(string type, string source)
    {
        var obj = new JObject();
        obj["type"] = new JValue("schema");
        var options = new JObject();
        options["type"] = new JValue(type);
        options["source"] = new JValue(source);
        obj["options"] = options;
        return obj;
    }

    private static JObject GetControlEditor(Type[] controlTypes)
    {
        var obj = new JObject();
        obj["type"] = new JValue("control");
        var options = new JObject();
        var types = new JArray();
        foreach (var controlType in controlTypes)
        {
            types.Add(new JValue(controlType.FullName));
        }
        options["types"] = types;
        obj["options"] = options;
        return obj;
    }

    private static JObject GetEventEditor(EventInfo eventInfo)
    {
        var obj = new JObject();
        obj["type"] = new JValue("event");
        var options = new JObject();
        options["argType"] = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType.FullName;
        obj["options"] = options;
        return obj;
    }

    private static JObject GetMenuEditor(string type)
    {
        var obj = new JObject();
        obj["type"] = new JValue("menu");
        var options = new JObject();
        options["type"] = type;
        obj["options"] = options;
        return obj;
    }
}