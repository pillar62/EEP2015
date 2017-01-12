using System;
using System.Collections.Generic;
using System.Text;
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Srvtools;
using System.Data;
using JQMobileTools;
using System.Linq;

namespace MWizard2015
{
    //这个文件为兼容VS2008增加的,只考虑实现功能，不考虑代码优化
    public partial class TJQMobileFormGenerator
    {
        /// <summary>
        /// "Infolight"字串
        /// </summary>
        const string INFOLIGHTMARK = "Infolight";

        /// <summary>
        /// 加入查询
        /// </summary>
        /// <param name="fieldItem">blockfielditem</param>
        /// <param name="queryFields">Navigator的查询集合</param>
        /// <param name="queryColumns">ClientQuery的查询集合</param>
        /// <param name="tableName">tablename</param>
        private void GenQuery(TBlockFieldItem fieldItem, WebQueryFiledsCollection queryFields, WebQueryColumnsCollection queryColumns
            , string tableName)
        {
            if (string.Compare(fieldItem.QueryMode, "normal", true) == 0
                  || string.Compare(fieldItem.QueryMode, "range", true) == 0)
            {
                if (queryFields != null)
                {
                    WebQueryField field = new WebQueryField();
                    field.FieldName = fieldItem.DataField;
                    field.Caption = string.IsNullOrEmpty(fieldItem.Description) ? fieldItem.DataField : fieldItem.Description;

                    if (string.Compare(fieldItem.QueryMode, "normal", true) == 0)
                    {
                        field.Condition = (fieldItem.DataType == typeof(string)) ? "%" : "=";
                    }
                    else
                    {
                        WebQueryField fieldrev = new WebQueryField();
                        fieldrev.FieldName = field.FieldName;
                        fieldrev.Caption = field.Caption;
                        fieldrev.RefVal = field.RefVal;
                        fieldrev.Condition = ">=";
                        fieldrev.Mode = InitQueryField(fieldItem, fieldrev);
                        queryFields.Add(fieldrev);
                        field.Condition = "<=";
                    }

                    field.Mode = InitQueryField(fieldItem, field);
                    queryFields.Add(field);
                }
                if (queryColumns != null)
                {
                    WebQueryColumns column = new WebQueryColumns();
                    column.Column = fieldItem.DataField;
                    column.Caption = string.IsNullOrEmpty(fieldItem.Description) ? fieldItem.DataField : fieldItem.Description;
                    if (string.Compare(fieldItem.ControlType, "textbox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryTextBoxColumn";
                    }
                    else if (string.Compare(fieldItem.ControlType, "combobox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryComboBoxColumn";
                        column.WebRefVal = string.Format("wrv{0}{1}QF", tableName, fieldItem.DataField);
                    }
                    else if (string.Compare(fieldItem.ControlType, "refvalbox", true) == 0)
                    {
                        if (fieldItem.RefValNo == String.Empty)
                            column.ColumnType = "ClientQueryTextBoxColumn";
                        else
                        {
                            column.ColumnType = "ClientQueryRefValColumn";
                            column.WebRefVal = string.Format("wrv{0}{1}QF", tableName, fieldItem.DataField);
                        }
                    }
                    else if (string.Compare(fieldItem.ControlType, "datetimebox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryCalendarColumn";
                    }
                    if (string.Compare(fieldItem.QueryMode, "normal", true) == 0)
                    {
                        column.Operator = (fieldItem.DataType == typeof(string)) ? "%" : "=";
                    }
                    else
                    {
                        WebQueryColumns columnrev = new WebQueryColumns();
                        columnrev.Column = column.Column;
                        columnrev.Caption = column.Caption;
                        columnrev.ColumnType = column.ColumnType;
                        columnrev.WebRefVal = column.WebRefVal;
                        columnrev.Operator = ">=";
                        queryColumns.Add(columnrev);
                        column.Operator = "<=";
                    }
                    queryColumns.Add(column);
                }
            }
        }

        private String InitQueryField(TBlockFieldItem fieldItem, WebQueryField field)
        {
            String returnValue = "";

            if (string.Compare(fieldItem.ControlType, "combobox", true) == 0)
            {
                returnValue = String.Format("<JQMobileTools:JQQueryColumn Caption=\"{0}\" Condition=\"{1}\" Editor=\"selects\" FieldName=\"{2}\" " +
                                            "EditorOptions=\"{{DialogTitle:'Select Item',DialogWidth:250,PageSize:20,ColumnMatches:[],ValueMember:'{3}',DisplayMember:'{4}',RemoteName:'{5}',TableName:'{6}'}}\"/>",
                                             field.Caption, field.Condition, fieldItem.DataField, fieldItem.ComboValueField, fieldItem.ComboTextField, fieldItem.ComboRemoteName, fieldItem.ComboEntityName);
            }
            else if (string.Compare(fieldItem.ControlType, "refvalbox", true) == 0)
            {
                String columns = "{field:'" + fieldItem.ComboValueField + "',title:'" + fieldItem.ComboValueFieldCaption + "',width:80,align:'left'}";
                if (fieldItem.ComboValueField != fieldItem.ComboTextField)
                {
                    columns += ",{field:'" + fieldItem.ComboTextField + "',title:'" + fieldItem.ComboTextFieldCaption + "',width:80,align:'left'}";
                }
                returnValue = String.Format("<JQMobileTools:JQQueryColumn Caption=\"{0}\" Condition=\"{1}\" Editor=\"refval\" FieldName=\"{2}\" " +
                                        "EditorOptions=\"{{DialogTitle:'Select Item',DialogWidth:250,PageSize:20,ColumnMatches:[],ValueMember:'{3}',DisplayMember:'{4}',RemoteName:'{5}',TableName:'{6}',Columns:[{7}]}}\"/>",
                                        field.Caption, field.Condition, fieldItem.DataField, fieldItem.ComboValueField, fieldItem.ComboTextField, fieldItem.ComboRemoteName, fieldItem.ComboEntityName, columns);

            }
            else if (string.Compare(fieldItem.ControlType, "datetimebox", true) == 0)
            {
                returnValue = String.Format("<JQMobileTools:JQQueryColumn Caption=\"{0}\" Condition=\"{1}\" Editor=\"date\" FieldName=\"{2}\" />", field.Caption, field.Condition, fieldItem.DataField);
            }
            else //(string.Compare(fieldItem.ControlType, "textbox", true) == 0)
            {
                returnValue = String.Format("<JQMobileTools:JQQueryColumn Caption=\"{0}\" Condition=\"{1}\" Editor=\"text\" FieldName=\"{2}\" />", field.Caption, field.Condition, fieldItem.DataField);
            }
            //else if (string.Compare(fieldItem.ControlType, "combogrid", true) == 0)
            //{
            //    String columns = "{field:'" + fieldItem.ComboValueField + "',title:'" + fieldItem.ComboValueFieldCaption + "',width:80,align:'left'}";
            //    if (fieldItem.ComboValueField != fieldItem.ComboTextField)
            //    {
            //        columns += ",{field:'" + fieldItem.ComboTextField + "',title:'" + fieldItem.ComboTextFieldCaption + "',width:80,align:'left'}";
            //    }
            //    returnValue = String.Format("<JQMobileTools:JQQueryColumn Caption=\"{0}\" Condition=\"{1}\" Editor=\"infocombogrid\" FieldName=\"{2}\" " +
            //                            "EditorOptions=\"panelWidth:450,columnMatches:[],valueField:'{3}',textField:'{4}',remoteName:'{5}',tableName:'{6}',columns:[{7}]\"/>",
            //                            field.Caption, field.Condition, fieldItem.DataField, fieldItem.ComboValueField, fieldItem.ComboTextField, fieldItem.ComboRemoteName, fieldItem.ComboEntityName, columns);

            //}
            //else if (string.Compare(fieldItem.ControlType, "numberbox", true) == 0)
            //{
            //    returnValue = String.Format("<JQMobileTools:JQQueryColumn Caption=\"{0}\" Condition=\"{1}\"  Editor=\"numberbox\" FieldName=\"{2}\" />", field.Caption, field.Condition, fieldItem.DataField);
            //}
            return returnValue;
        }

        /// <summary>
        /// 在page中插入Control
        /// </summary>
        /// <param name="pageElement">Page的Element</param>
        /// <param name="control">Control</param>
        private void InsertControl(WebDevPage.IHTMLElement pageElement, WebControl control, String position = "")
        {
            if (pageElement != null)
            {
                string controlxml = GetControlXml(control);

                if (control is WebRefVal)
                {
                    int i = controlxml.IndexOf("runat=\"server\"");
                    controlxml = controlxml.Insert(i + 14, " Visible=\"False\" ");
                }
                if (controlxml.Length > 0)
                {
                    string html = pageElement.innerHTML;
                    if (position == String.Empty)
                        position = "form";
                    int index = IndexOfEndTag(html, position);
                    if (index != -1)
                    {
                        pageElement.innerHTML = html.Insert(index, controlxml);
                    }
                }
            }
        }

        /// <summary>
        /// 取得Control对应的xml
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>xml</returns>
        private string GetControlXml(WebControl control)
        {
            return GetControlXml(control, null, string.Empty, string.Empty);
        }

        /// <summary>
        /// 取得Control对应的xml
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="prop">绑定Control的属性</param>
        /// <param name="field">绑定的字段</param>
        /// <param name="format">绑定的格式</param>
        /// <returns>xml</returns>
        private string GetControlXml(WebControl control, PropertyInfo prop, string field, string format)
        {
            StringBuilder builder = new StringBuilder();
            //builder.Append("<br/>");
            if (control != null)
            {
                StringBuilder builderInnerHtml = new StringBuilder();
                Type controltype = control.GetType();
                //if (controltype.Namespace == "Srvtools")
                //{
                //    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", INFOLIGHTMARK, controltype.Name, control.ID));
                //}
                //else
                //{
                //    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", "Asp", controltype.Name, control.ID));
                //} 
                if (controltype.Namespace == "Srvtools")
                {
                    builder.Append(string.Format("<{0}:{1} runat=\"server\" ", INFOLIGHTMARK, controltype.Name));
                }
                else
                {
                    builder.Append(string.Format("<{0}:{1} runat=\"server\" ", "JQMobileTools", controltype.Name));
                }
                if (prop != null)
                {
                    builder.Append(string.Format("{0}='<%# Bind(\"{1}\"", prop.Name, field, format));
                    if (!string.IsNullOrEmpty(format))
                    {
                        builder.Append(string.Format("{0}", format));
                    }
                    builder.Append(") %>' ");
                }

                //PropertyInfo[] infos = controltype.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                PropertyInfo[] infos = controltype.GetProperties();
                List<String> listTemp = new List<string>();
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Name == "ValidateRequestMode")
                        continue;
                    if (prop == null || (prop != null && prop.Name != infos[i].Name))
                    {
                        if (listTemp.Contains(infos[i].Name))
                            continue;
                        listTemp.Add(infos[i].Name);
                        if (!IsVisibilityHidden(infos[i]))
                        {
                            if (infos[i].PropertyType == typeof(string) || infos[i].PropertyType == typeof(int) || infos[i].PropertyType == typeof(bool)
                                || infos[i].PropertyType.BaseType == typeof(Enum))
                            {
                                object value = infos[i].GetValue(control, null);
                                object defaultvalue = GetDefaultValue(infos[i]);
                                if (infos[i].CanWrite && value != null && !String.IsNullOrEmpty(value.ToString())
                                    && value != defaultvalue && infos[i].Name != "Visible")
                                {
                                    builder.Append(string.Format("{0}=\"{1}\" ", infos[i].Name, value));
                                }
                            }
                            else if (infos[i].PropertyType.Name == "JQCollection`1")
                            {
                                string collectionxml = GetCollectionXml(infos[i], (IJQProperty)infos[i].GetValue(control, null));
                                if (collectionxml.Length > 0)
                                {
                                    builderInnerHtml.AppendLine(collectionxml);
                                }
                            }
                        }
                    }
                }
                builder.AppendLine(">");
                builder.Append(builderInnerHtml.ToString());
                if (controltype.Namespace == "Srvtools")
                {
                    builder.AppendLine(string.Format("</{0}:{1}>", INFOLIGHTMARK, controltype.Name));
                }
                else
                {
                    builder.AppendLine(string.Format("</{0}:{1}>", "JQMobileTools", controltype.Name));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 取得Collecion对应的xml
        /// </summary>
        /// <param name="prop">Collection的propinfo</param>
        /// <param name="collection">Collection的value</param>
        /// <returns>xml</returns>
        private string GetCollectionXml(PropertyInfo prop, IJQProperty c)
        {
            StringBuilder builder = new StringBuilder();
            if (c is JQCollection<JQDefaultColumn>)
            {
                JQCollection<JQDefaultColumn> collection = c as JQCollection<JQDefaultColumn>;
                if (prop != null && collection != null && prop.PropertyType == collection.GetType())
                {
                    if (collection.Count > 0)
                    {
                        builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                        for (int i = 0; i < collection.Count; i++)
                        {
                            PropertyInfo[] infos = null;
                            builder.Append(string.Format("\t\t<{0}:{1} ", "JQMobileTools", collection[i].GetType().Name));
                            infos = collection[i].GetType().GetProperties();

                            for (int j = 0; j < infos.Length; j++)
                            {
                                if (!IsVisibilityHidden(infos[j]))
                                {
                                    if (infos[j].PropertyType == typeof(string) || infos[j].PropertyType == typeof(int) || infos[j].PropertyType == typeof(bool)
                                        || infos[j].PropertyType.BaseType == typeof(Enum))
                                    {
                                        if (!infos[j].Name.Equals("Name"))
                                        {
                                            object value = infos[j].GetValue(collection[i], null);
                                            object defaultvalue = GetDefaultValue(infos[j]);
                                            if (infos[j].CanWrite && value != defaultvalue)
                                            {
                                                builder.Append(string.Format("{0}=\"{1}\" ", infos[j].Name, value));
                                            }
                                        }
                                    }
                                }
                            }
                            builder.AppendLine("/>");
                        }
                        builder.AppendLine(string.Format("\t</{0}>", prop.Name));
                    }
                }
            }
            else if (c is JQCollection<JQValidateColumn>)
            {
                JQCollection<JQValidateColumn> collection = c as JQCollection<JQValidateColumn>;
                if (prop != null && collection != null && prop.PropertyType == collection.GetType())
                {
                    if (collection.Count > 0)
                    {
                        builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                        for (int i = 0; i < collection.Count; i++)
                        {
                            PropertyInfo[] infos = null;
                            builder.Append(string.Format("\t\t<{0}:{1} ", "JQMobileTools", collection[i].GetType().Name));
                            infos = collection[i].GetType().GetProperties();

                            for (int j = 0; j < infos.Length; j++)
                            {
                                if (!IsVisibilityHidden(infos[j]))
                                {
                                    if (infos[j].PropertyType == typeof(string) || infos[j].PropertyType == typeof(int) || infos[j].PropertyType == typeof(bool)
                                        || infos[j].PropertyType.BaseType == typeof(Enum))
                                    {
                                        if (!infos[j].Name.Equals("Name"))
                                        {
                                            object value = infos[j].GetValue(collection[i], null);
                                            object defaultvalue = GetDefaultValue(infos[j]);
                                            if (infos[j].CanWrite && value != defaultvalue)
                                            {
                                                builder.Append(string.Format("{0}=\"{1}\" ", infos[j].Name, value));
                                            }
                                        }
                                    }
                                }
                            }
                            builder.AppendLine("/>");
                        }
                        builder.AppendLine(string.Format("\t</{0}>", prop.Name));
                    }
                }
            }

            return builder.ToString();
        }

        private bool IsVisibilityHidden(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(System.ComponentModel.DesignerSerializationVisibilityAttribute), true);
            if (attributes != null && attributes.Length > 0)
            {
                if (((System.ComponentModel.DesignerSerializationVisibilityAttribute)attributes[0]).Visibility
                    == System.ComponentModel.DesignerSerializationVisibility.Hidden)
                {
                    return true;
                }
            }
            return false;
        }

        private object GetDefaultValue(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(System.ComponentModel.DefaultValueAttribute), true);
            if (attributes != null && attributes.Length > 0)
            {
                return ((System.ComponentModel.DefaultValueAttribute)attributes[0]).Value;
            }
            return null;
        }

        /// <summary>
        /// 取得tag在html的最末位置，如&lt;/form&gt;
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="tag">tag</param>
        /// <returns>最末位置</returns>
        private int IndexOfEndTag(string html, string tag)
        {
            int length;
            return IndexOfEndTag(html, tag, out length);
        }

        /// <summary>
        /// 取得tag在html的最末位置，如&lt;/form&gt;
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="tag">tag</param>
        /// <param name="length">tag的长度</param>
        /// <returns>最末位置</returns>
        private int IndexOfEndTag(string html, string tag, out int length)
        {
            Match mc = Regex.Match(html, string.Format(@"</{0}\s*>", tag), RegexOptions.RightToLeft);
            if (mc.Success)
            {
                length = mc.Length;
                return mc.Index;
            }
            else
            {
                length = 0;
                return -1;
            }
        }
    }
}
