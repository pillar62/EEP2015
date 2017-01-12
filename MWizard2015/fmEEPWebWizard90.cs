using System;
using System.Collections.Generic;
using System.Text;
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Srvtools;
using System.Data;

namespace MWizard2015
{
    //这个文件为兼容VS2008增加的,只考虑实现功能，不考虑代码优化
    public partial class TWebClientGenerator
    {
#if VS90
        /// <summary>
        /// "Infolight"字串
        /// </summary>
        const string INFOLIGHTMARK = "Infolight";

        private void GenMainBlockControl_3(TBlockItem blockItem, String formViewName)
        {
            WebDevPage.IHTMLElement Master = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("Master", 0);
            string mastertablename = string.Empty;
            if (Master != null)
            {
                mastertablename = FClientData.ProviderName.Split('.')[1];
                Master.setAttribute("DataMember", mastertablename, 0);
                if (formViewName == "wfvMaster")
                {
                    Master.setAttribute("AutoApply", "true", 0);
                }
            }
            blockItem.wDataSource = new WebDataSource();
            WebDevPage.IHTMLElement Page = FDesignerDocument.pageContentElement;

            WebDefault Default = new WebDefault();
            Default.ID = "wd" + WzdUtils.RemoveSpecialCharacters(blockItem.TableName);
            Default.DataSourceID = Master.getAttribute("ID", 0).ToString();
            Default.DataMember = mastertablename;

            WebValidate Validate = new WebValidate();
            Validate.ID = "wv" + WzdUtils.RemoveSpecialCharacters(blockItem.TableName);
            Validate.DataSourceID = Master.getAttribute("ID", 0).ToString();
            Validate.DataMember = mastertablename;

            WebQueryFiledsCollection QueryFields = new WebQueryFiledsCollection(null, typeof(QueryField));
            WebQueryColumnsCollection QueryColumns = new WebQueryColumnsCollection(null, typeof(QueryColumns));
            foreach (TBlockFieldItem fielditem in blockItem.BlockFieldItems)
            {
                GenDefault(fielditem, Default, Validate);
                GenQuery(fielditem, QueryFields, QueryColumns, blockItem.TableName);
            }

            InsertControl(Page, Default);
            InsertControl(Page, Validate);

            foreach (TBlockFieldItem fielditem in blockItem.BlockFieldItems)
            {
                foreach (WebQueryColumns wqc in QueryColumns)
                {
                    if (wqc.ColumnType == "ClientQueryRefValColumn" && wqc.Column == fielditem.DataField)
                    {
                        WebDataSource aWebDataSource = new WebDataSource();
                        InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
                        aInfoCommand.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
                        //aInfoCommand.Connection = FClientData.Owner.GlobalConnection;
                        IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                        if (FSYS_REFVAL != null)
                            FSYS_REFVAL.Dispose();
                        FSYS_REFVAL = new DataSet();
                        aInfoCommand.CommandText = String.Format("Select * from SYS_REFVAL where REFVAL_NO = '{0}'", fielditem.RefValNo);
                        WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, FSYS_REFVAL, fielditem.RefValNo);

                        WebRefVal aWebRefVal = new WebRefVal();
                        aWebRefVal.ID = wqc.WebRefVal;
                        aWebRefVal.DataTextField = FSYS_REFVAL.Tables[0].Rows[0]["DISPLAY_MEMBER"].ToString();
                        aWebRefVal.DataValueField = FSYS_REFVAL.Tables[0].Rows[0]["VALUE_MEMBER"].ToString();
                        aWebRefVal.DataSourceID = String.Format("wds{0}{1}", WzdUtils.RemoveSpecialCharacters(blockItem.TableName), wqc.Column);
                        aWebRefVal.Visible = false;
                        InsertControl(Page, aWebRefVal);
                        break;
                    }
                }
            }

            WebDevPage.IHTMLElement Navigator = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("WebNavigator1", 0);
            if (Navigator != null)
            {
                SetCollectionValue(Navigator, typeof(WebNavigator).GetProperty("QueryFields"), QueryFields);
            }
            WebDevPage.IHTMLElement ClientQuery = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("WebClientQuery1", 0);
            if (ClientQuery != null)
            {
                SetCollectionValue(ClientQuery, typeof(WebClientQuery).GetProperty("Columns"), QueryColumns);
            }
            //刷不了Schema所以直接用写的

            WebDevPage.IHTMLElement FormView = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item(formViewName, 0);
            if (FormView != null)
            {
                RefreshFormView(FormView, blockItem);
            }
            WebDevPage.IHTMLElement AjaxFromView1 = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("AjaxFormView1", 0);
            if (AjaxFromView1 != null)
            {
                AjaxTools.AjaxFormFieldCollection aAjaxFormFieldCollection = new AjaxTools.AjaxFormFieldCollection(new AjaxTools.AjaxFormView(), typeof(AjaxTools.AjaxFormField));
                DataTable srcTable = FWizardDataSet.RealDataSet.Tables[blockItem.TableName];
                bool flag = true;
                foreach (TBlockFieldItem BFI in blockItem.BlockFieldItems)
                {
                    AjaxTools.AjaxFormField extCol = new AjaxTools.AjaxFormField();
                    if (BFI.CheckNull == "Y")
                        extCol.AllowNull = false;
                    else
                        extCol.AllowNull = true;
                    if (BFI.Description != null && BFI.Description != String.Empty)
                        extCol.Caption = BFI.Description;
                    else
                        extCol.Caption = BFI.DataField;
                    extCol.DataField = BFI.DataField;
                    extCol.DefaultValue = BFI.DefaultValue;
                    extCol.EditControlId = null;
                    extCol.FieldControlId = string.Format("ctrl{0}", BFI.DataField);
                    extCol.IsKeyField = IsKeyField(BFI.DataField, srcTable.PrimaryKey);
                    extCol.NewLine = flag;
                    //extCol.Resizable = true;
                    //extCol.TextAlign = "left";
                    //extCol.Visible = true;
                    extCol.Width = 140;
                    if ((BFI.RefValNo != null && BFI.RefValNo != "") || BFI.RefField != null)
                    {
                        String DataSourceID = GenWebDataSource(BFI, WzdUtils.RemoveSpecialCharacters(blockItem.TableName), "RefVal", "", true);
                        String extComboBox = GenExtComboBox(BFI, WzdUtils.RemoveSpecialCharacters(blockItem.TableName), "ExtRefVal", "", DataSourceID);
                        try
                        {
                            String str = AjaxFromView1.innerHTML;
                        }
                        catch
                        {
                            AjaxFromView1 = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("AjaxFormView1", 0);
                        }
                        extCol.EditControlId = extComboBox;
                        extCol.Editor = AjaxTools.ExtGridEditor.ComboBox;
                    }
                    else if (BFI.ControlType == "ComboBox")
                    {
                        String DataSourceID = GenWebDataSource(BFI, WzdUtils.RemoveSpecialCharacters(BFI.ComboEntityName), "ComboBox", "", true);
                        String extComboBox = GenExtComboBox(BFI, WzdUtils.RemoveSpecialCharacters(blockItem.TableName), "ExtComboBox", "", DataSourceID);
                        try
                        {
                            String str = AjaxFromView1.innerHTML;
                        }
                        catch
                        {
                            AjaxFromView1 = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("AjaxFormView1", 0);
                        } extCol.EditControlId = extComboBox;
                        extCol.Editor = AjaxTools.ExtGridEditor.ComboBox;
                    }
                    this.FieldTypeSelector(BFI.DataType, extCol, BFI.ControlType);
                    aAjaxFormFieldCollection.Add(extCol);
                    flag = !flag;
                }

                SetCollectionValue(AjaxFromView1, typeof(AjaxTools.AjaxFormView).GetProperty("Fields"), aAjaxFormFieldCollection);
            }
        }

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

                    if (string.Compare(fieldItem.ControlType, "textbox", true) == 0)
                    {
                        field.Mode = "TextBox";
                    }
                    else if (string.Compare(fieldItem.ControlType, "combobox", true) == 0)
                    {
                        field.Mode = "ComboBox";
                        field.RefVal = string.Format("wrv{0}{1}QF", tableName, fieldItem.DataField);
                    }
                    else if (string.Compare(fieldItem.ControlType, "refvalbox", true) == 0)
                    {
                        if (fieldItem.RefValNo == String.Empty)
                            field.Mode = "TextBox";
                        else
                        {
                            field.Mode = "RefVal";
                            field.RefVal = string.Format("wrv{0}{1}QF", tableName, fieldItem.DataField);
                        }
                    }
                    else if (string.Compare(fieldItem.ControlType, "datetimebox", true) == 0)
                    {
                        field.Mode = "Calendar";
                    }
                    if (string.Compare(fieldItem.QueryMode, "normal", true) == 0)
                    {
                        field.Condition = (fieldItem.DataType == typeof(string)) ? "%" : "=";
                    }
                    else
                    {
                        WebQueryField fieldrev = new WebQueryField();
                        fieldrev.FieldName = field.FieldName;
                        fieldrev.Caption = field.Caption;
                        fieldrev.Mode = field.Mode;
                        fieldrev.RefVal = field.RefVal;
                        fieldrev.Condition = ">=";
                        queryFields.Add(fieldrev);
                        field.Condition = "<=";
                    }
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

        /// <summary>
        /// 刷新formview的内容
        /// </summary>
        /// <param name="formViewElement">formview的结构</param>
        /// <param name="blockItem">blockItem</param>
        private void RefreshFormView(WebDevPage.IHTMLElement formViewElement, TBlockItem blockItem)
        {
            if (formViewElement != null)
            {
                StringBuilder builderEditTemplate = new StringBuilder("<EditItemTemplate>\r\n\t<table class=\"container_table\">");
                StringBuilder builderInsertTemplate = new StringBuilder("<InsertItemTemplate>\r\n\t<table class=\"container_table\">");
                StringBuilder builderItemTemplate = new StringBuilder("<ItemTemplate>\r\n\t<table class=\"container_table\">");
                FormViewFieldsCollection fields = new FormViewFieldsCollection(null, typeof(FormViewField));
                int layoutcolnum = int.Parse(formViewElement.getAttribute("LayOutColNum", 0).ToString());
                for (int i = 0; i < blockItem.BlockFieldItems.Count; i++)
                {
                    TBlockFieldItem item = (TBlockFieldItem)blockItem.BlockFieldItems[i];
                    string controlid = string.Empty;
                    if (i % layoutcolnum == 0 || layoutcolnum == 1)
                    {
                        builderEditTemplate.AppendLine("\t\t<tr>");
                        builderInsertTemplate.AppendLine("\t\t<tr>");
                        builderItemTemplate.AppendLine("\t\t<tr>");
                    }
                    builderEditTemplate.AppendLine("\t\t\t<td class=\"caption_td\">");
                    builderEditTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetCaptionLabelXml(item, true)));
                    builderEditTemplate.AppendLine("\t\t\t</td>");
                    builderEditTemplate.AppendLine("\t\t\t<td class=\"value_td\">");
                    controlid = "E";
                    builderEditTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetControlXml(item, WzdUtils.RemoveSpecialCharacters(blockItem.TableName), ref controlid)));
                    builderEditTemplate.AppendLine("\t\t\t</td>");

                    builderInsertTemplate.AppendLine("\t\t\t<td class=\"caption_td\">");
                    builderInsertTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetCaptionLabelXml(item, true)));
                    builderInsertTemplate.AppendLine("\t\t\t</td>");
                    builderInsertTemplate.AppendLine("\t\t\t<td class=\"value_td\">");
                    controlid = "I";
                    builderInsertTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetControlXml(item, WzdUtils.RemoveSpecialCharacters(blockItem.TableName), ref controlid)));
                    builderInsertTemplate.AppendLine("\t\t\t</td>");

                    builderItemTemplate.AppendLine("\t\t\t<td class=\"caption_td\">");
                    builderItemTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetCaptionLabelXml(item, false)));
                    builderItemTemplate.AppendLine("\t\t\t</td>");

                    builderItemTemplate.AppendLine("\t\t\t<td class=\"value_td\">");
                    builderItemTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetLabelXml(item, WzdUtils.RemoveSpecialCharacters(blockItem.TableName), ref controlid)));
                    builderItemTemplate.AppendLine("\t\t\t</td>");
                    if (i % layoutcolnum == layoutcolnum - 1 || layoutcolnum == 1 || i == blockItem.BlockFieldItems.Count - 1)
                    {
                        builderEditTemplate.AppendLine("\t\t</tr>");
                        builderInsertTemplate.AppendLine("\t\t</tr>");
                        builderItemTemplate.AppendLine("\t\t</tr>");
                    }

                    FormViewField field = new FormViewField();
                    field.FieldName = item.DataField;
                    field.ControlID = controlid;
                    fields.Add(field);
                }
                builderEditTemplate.AppendLine("\t</table>\r\n</EditItemTemplate>");
                builderInsertTemplate.AppendLine("\t</table>\r\n</InsertItemTemplate>");
                builderItemTemplate.AppendLine("\t</table>\r\n</ItemTemplate>");

                SetCollectionValue(formViewElement, typeof(WebFormView).GetProperty("Fields"), fields);
                SetTemplateValue(formViewElement, builderItemTemplate.ToString(), "ItemTemplate");
                SetTemplateValue(formViewElement, builderInsertTemplate.ToString(), "InsertItemTemplate");
                SetTemplateValue(formViewElement, builderEditTemplate.ToString(), "EditItemTemplate");
            }
        }

        /// <summary>
        /// 在page中插入Control
        /// </summary>
        /// <param name="pageElement">Page的Element</param>
        /// <param name="controltype">Control的Type</param>
        /// <param name="id">Control的ID</param>
        private void InsertControl(WebDevPage.IHTMLElement pageElement, Type controltype, string id)
        {
            if (pageElement != null)
            {
                StringBuilder builder = new StringBuilder();
                if (controltype.Namespace == "Srvtools")
                {
                    builder.AppendLine(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\">", INFOLIGHTMARK, controltype.Name, id));
                    builder.AppendLine(string.Format("</{0}:{1}>", INFOLIGHTMARK, controltype.Name));
                }
                else
                {
                    builder.AppendLine(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\">", "Asp", controltype.Name, id));
                    builder.AppendLine(string.Format("</{0}:{1}>", "Asp", controltype.Name));
                }
                string html = pageElement.innerHTML;
                int index = IndexOfEndTag(html, "form");
                if (index != -1)
                {
                    pageElement.innerHTML = html.Insert(index, builder.ToString());
                }
            }
        }

        /// <summary>
        /// 在page中插入Control
        /// </summary>
        /// <param name="pageElement">Page的Element</param>
        /// <param name="control">Control</param>
        private void InsertControl(WebDevPage.IHTMLElement pageElement, WebControl control)
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
                    int index = IndexOfEndTag(html, "form");
                    if (index != -1)
                    {
                        pageElement.innerHTML = html.Insert(index, controlxml);
                    }
                }
            }
        }

        /// <summary>
        /// 在page中插入Control
        /// </summary>
        /// <param name="pageElement">Page的Element</param>
        /// <param name="control">Control</param>
        private void InsertControl(WebDevPage.IHTMLElement pageElement, System.Web.UI.Control control)
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
                    //int index = IndexOfEndTag(html, "form");
                    //if (index != -1)
                    //{
                    //    pageElement.innerHTML = html.Insert(index, controlxml);
                    //}
                    int index = IndexOfEndTag(pageElement.innerHTML, "form");
                    if (index != -1)
                    {
                        pageElement.innerHTML = pageElement.innerHTML.Insert(index, controlxml);
                    }
                }
            }
        }

        /// <summary>
        /// 设置Control的Collection属性
        /// </summary>
        /// <param name="controlElement">Control的Element</param>
        /// <param name="prop">Collection的propinfo</param>
        /// <param name="collection">Collection的value</param>
        private void SetCollectionValue(WebDevPage.IHTMLElement controlElement, PropertyInfo prop, InfoOwnerCollection collection)
        {
            if (controlElement != null)
            {
                string collectionxml = GetCollectionXml(prop, collection);
                if (collectionxml.Length > 0)
                {
                    string html = controlElement.innerHTML;
                    int index = IndexOfBeginTag(html, prop.Name);
                    int length;
                    if (index > 0)
                    {
                        int indexend = IndexOfEndTag(html, prop.Name, out length);
                        html = html.Remove(indexend - index - length);
                    }
                    else
                    {
                        index = 0;
                    }
                    controlElement.innerHTML = html.Insert(index, collectionxml);
                }
            }
        }

        private void SetCollectionValue(WebDevPage.IHTMLElement controlElement, PropertyInfo prop, AjaxTools.ExtGridColumnCollection collection)
        {
            if (controlElement != null)
            {
                string collectionxml = GetCollectionXml(prop, collection);
                if (collectionxml.Length > 0)
                {
                    string html = controlElement.innerHTML;
                    int index = IndexOfBeginTag(html, prop.Name);
                    int length;
                    if (index > 0)
                    {
                        int indexend = IndexOfEndTag(html, prop.Name, out length);
                        html = html.Remove(indexend - index - length);
                    }
                    else
                    {
                        index = 0;
                    }
                    controlElement.innerHTML = html.Insert(index, collectionxml);
                }
            }
        }

        /// <summary>
        /// 设置FormView的Template
        /// </summary>
        /// <param name="viewElement">FormView的Element</param>
        /// <param name="templatexml">Template的名字</param>
        /// <param name="templatename">Template的内容</param>
        private void SetTemplateValue(WebDevPage.IHTMLElement viewElement, string templatexml, string templatename)
        {
            if (viewElement != null)
            {
                if (templatexml.Length > 0)
                {
                    string html = viewElement.innerHTML;
                    int index = IndexOfBeginTag(html, templatename);
                    int length;
                    if (index > 0)
                    {
                        int indexend = IndexOfEndTag(html, templatename, out length);
                        html = html.Remove(indexend - index - length);
                    }
                    else
                    {
                        index = 0;
                    }
                    viewElement.innerHTML = html.Insert(index, templatexml);
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
        /// <returns>xml</returns>
        private string GetControlXml(System.Web.UI.Control control)
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
                    builder.Append(string.Format("<{0}:{1} runat=\"server\" ", "Asp", controltype.Name));
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
                            else if (infos[i].PropertyType.BaseType == typeof(InfoOwnerCollection))
                            {
                                string collectionxml = GetCollectionXml(infos[i], (InfoOwnerCollection)infos[i].GetValue(control, null));
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
                    builder.AppendLine(string.Format("</{0}:{1}>", "Asp", controltype.Name));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 取得Control对应的xml
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="prop">绑定Control的属性</param>
        /// <param name="field">绑定的字段</param>
        /// <param name="format">绑定的格式</param>
        /// <returns>xml</returns>
        private string GetControlXml(System.Web.UI.Control control, PropertyInfo prop, string field, string format)
        {
            StringBuilder builder = new StringBuilder();
            if (control != null)
            {
                StringBuilder builderInnerHtml = new StringBuilder();
                Type controltype = control.GetType();
                if (controltype.Namespace == "Srvtools")
                {
                    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", INFOLIGHTMARK, controltype.Name, control.ID));
                }
                else if (controltype.Namespace == "AjaxTools")
                {
                    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", "ajaxtools", controltype.Name, control.ID));
                }
                else
                {
                    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", "Asp", controltype.Name, control.ID));
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

                PropertyInfo[] infos = controltype.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                for (int i = 0; i < infos.Length; i++)
                {
                    if (prop == null || (prop != null && prop.Name != infos[i].Name))
                    {
                        if (!IsVisibilityHidden(infos[i]))
                        {
                            if (infos[i].PropertyType == typeof(string) || infos[i].PropertyType == typeof(int) || infos[i].PropertyType == typeof(bool)
                                || infos[i].PropertyType.BaseType == typeof(Enum))
                            {
                                object value = infos[i].GetValue(control, null);
                                object defaultvalue = GetDefaultValue(infos[i]);
                                if (infos[i].CanWrite && value != defaultvalue)
                                {
                                    builder.Append(string.Format("{0}=\"{1}\" ", infos[i].Name, value));
                                }
                            }
                            else if (infos[i].PropertyType.BaseType == typeof(InfoOwnerCollection))
                            {
                                string collectionxml = GetCollectionXml(infos[i], (InfoOwnerCollection)infos[i].GetValue(control, null));
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
                else if (controltype.Namespace == "AjaxTools")
                {
                    builder.AppendLine(string.Format("</{0}:{1}>", "ajaxtools", controltype.Name));
                }
                else
                {
                    builder.AppendLine(string.Format("</{0}:{1}>", "Asp", controltype.Name));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 取得Control对应的xml
        /// </summary>
        /// <param name="item">blockfielditem</param>
        /// <param name="tableName">tablename</param>
        /// <param name="id">控件的id</param>
        /// <returns>xml</returns>
        private string GetControlXml(TBlockFieldItem item, string tableName, ref string id)
        {
            WebControl control = null;
            PropertyInfo info = null;

            #region DropDownList
            if (string.Compare(item.ControlType, "combobox", true) == 0)
            {
                control = new WebDropDownList();
                control.ID = string.Format("{0}DropDownList", WzdUtils.RemoveSpecialCharacters(item.DataField));
                control.Width = new Unit(130, UnitType.Pixel);
                (control as WebDropDownList).DataSourceID = GenWebDataSource(item, tableName, "ComboBox", string.Empty);
                (control as WebDropDownList).DataMember = item.ComboEntityName;
                (control as WebDropDownList).DataTextField = item.ComboTextField;
                (control as WebDropDownList).DataValueField = item.ComboValueField;
                info = control.GetType().GetProperty("SelectedValue");
            }
            #endregion

            #region RefVal
            else if (string.Compare(item.ControlType, "refvalbox", true) == 0)
            {
#warning GenWebDataSource未完成FSYS_REFVAL部分
                control = new WebRefVal();
                control.ID = string.Format("{0}RefVal", WzdUtils.RemoveSpecialCharacters(item.DataField));
                if (!string.IsNullOrEmpty(item.RefValNo) || (item.RefField != null))
                {
                    (control as WebRefVal).DataSourceID = GenWebDataSource(item, tableName, "RefVal", string.Empty);
                    (control as WebRefVal).DataBindingField = item.DataField;
                    (control as WebRefVal).DataTextField = FSYS_REFVAL.Tables[0].Rows[0]["DISPLAY_MEMBER"].ToString();
                    (control as WebRefVal).DataValueField = FSYS_REFVAL.Tables[0].Rows[0]["VALUE_MEMBER"].ToString();
                    if (!string.IsNullOrEmpty(item.RefValNo))
                    {
                        IDbConnection conn = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, false);
                        InfoCommand command = new InfoCommand(FClientData.DatabaseType);
                        command.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
                        //command.Connection = conn;
                        command.CommandText = String.Format("Select * from SYS_REFVAL_D1 where REFVAL_NO = '{0}'", item.RefValNo);
                        IDbDataAdapter adapter = WzdUtils.AllocateDataAdapter(FClientData.DatabaseType);
                        adapter.SelectCommand = command.GetInternalCommand();
                        DataSet dataset = new DataSet();
                        WzdUtils.FillDataAdapter(FClientData.DatabaseType, adapter, dataset, item.RefValNo);
                        if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow DR in dataset.Tables[0].Rows)
                            {
                                WebRefColumn refcolumn = new WebRefColumn();
                                refcolumn.ColumnName = DR["FIELD_NAME"].ToString();
                                refcolumn.HeadText = DR["HEADER_TEXT"].ToString();
                                refcolumn.Width = 100;
                                (control as WebRefVal).Columns.Add(refcolumn);
                            }
                        }
                    }
                    info = control.GetType().GetProperty("BindingValue");
                }
                else
                {
                    control = new TextBox();
                    control.ID = string.Format("{0}TextBox", WzdUtils.RemoveSpecialCharacters(item.DataField));
                    (control as TextBox).MaxLength = item.Length;
                    info = control.GetType().GetProperty("Text");
                }

            }
            #endregion

            #region DateTimePicker
            else if (string.Compare(item.ControlType, "datetimebox", true) == 0)
            {
                control = new WebDateTimePicker();
                control.ID = string.Format("{0}DateTimePicker", WzdUtils.RemoveSpecialCharacters(item.DataField));
                (control as WebDateTimePicker).MaxLength = item.Length;
                if (string.IsNullOrEmpty(item.EditMask))
                {
                    (control as WebDateTimePicker).DateFormat = dateFormat.ShortDate;
                }
                if (item.DataType == typeof(DateTime))
                {
                    info = control.GetType().GetProperty("Text");
                }
                else if (item.DataType == typeof(string))
                {
                    (control as WebDateTimePicker).DateTimeType = dateTimeType.VarChar;
                    info = control.GetType().GetProperty("DataString");
                }
            }
            #endregion

            #region ValidateBox
            else if (string.Compare(item.ControlType, "validatebox", true) == 0)
            {
                control = new WebValidateBox();
                control.ID = string.Format("{0}ValidateBox", WzdUtils.RemoveSpecialCharacters(item.DataField));
                (control as WebValidateBox).WebValidateID = string.Format("wv{0}", tableName);
                (control as WebValidateBox).ValidateField = item.DataField;
                (control as WebValidateBox).MaxLength = item.Length;
                info = control.GetType().GetProperty("Text");
            }
            #endregion

            #region CheckBox
            else if (string.Compare(item.ControlType, "checkbox", true) == 0)
            {
                control = new CheckBox();
                control.ID = string.Format("{0}CheckBox", WzdUtils.RemoveSpecialCharacters(item.DataField));
                info = control.GetType().GetProperty("Checked");
            }
            #endregion

            #region TextBox
            else
            {
                control = new TextBox();
                control.ID = string.Format("{0}TextBox", WzdUtils.RemoveSpecialCharacters(item.DataField));
                (control as TextBox).MaxLength = item.Length;
                info = control.GetType().GetProperty("Text");
            }
            #endregion

            control.ID = WzdUtils.RemoveSpecialCharacters(string.Format("{0}{1}", control.ID, id));
            id = control.ID;
            item.EditMask = FormatEditMask(item.EditMask);
            return GetControlXml(control, info, item.DataField, item.EditMask);
        }

        /// <summary>
        /// 取得label对应的xml
        /// </summary>
        /// <param name="item">blockfielditem</param>
        /// <returns>xml</returns>
        private string GetLabelXml(TBlockFieldItem item, String tableName, ref String id)
        {
            String strLabel = String.Empty;
            if (string.Compare(item.ControlType, "refvalbox", true) == 0)
            {
                WebControl control = null;
                PropertyInfo info = null;
                if (!string.IsNullOrEmpty(item.RefValNo) || (item.RefField != null))
                {
                    control = new WebRefVal();
                    control.ID = string.Format("{0}RefVal", WzdUtils.RemoveSpecialCharacters(item.DataField));
                    (control as WebRefVal).DataSourceID = GenWebDataSource(item, tableName, "RefVal", string.Empty);
                    (control as WebRefVal).DataBindingField = item.DataField;
                    (control as WebRefVal).DataTextField = FSYS_REFVAL.Tables[0].Rows[0]["DISPLAY_MEMBER"].ToString();
                    (control as WebRefVal).DataValueField = FSYS_REFVAL.Tables[0].Rows[0]["VALUE_MEMBER"].ToString();
                    (control as WebRefVal).BackColor = System.Drawing.Color.Transparent;
                    (control as WebRefVal).BorderStyle = BorderStyle.None;
                    (control as WebRefVal).ReadOnly = true;
                    (control as WebRefVal).Width = 100;
                    info = control.GetType().GetProperty("BindingValue");
                    //id = control.ID;
                    strLabel = GetControlXml(control, info, item.DataField, item.EditMask);
                }
                else
                {
                    control = new Label();
                    control.ID = string.Format("{0}Label", WzdUtils.RemoveSpecialCharacters(item.DataField));
                    strLabel = GetControlXml(control, control.GetType().GetProperty("Text"), item.DataField, item.EditMask);
                }
                //control.ID = string.Format("{0}{1}", control.ID, id);

                int i = strLabel.IndexOf("runat=\"server\"");
                strLabel = strLabel.Insert(i + 14, " Width=\"100\" BackColor=\"Transparent\"");
            }
            else
            {
                //用上面的方法实现
                Label label = new Label();
                label.ID = string.Format("{0}Label", WzdUtils.RemoveSpecialCharacters(item.DataField));
                strLabel = GetControlXml(label, label.GetType().GetProperty("Text"), item.DataField, item.EditMask);
            }
            return strLabel;
        }

        /// <summary>
        /// 取得CaptionLabel对应的xml
        /// </summary>
        /// <param name="item"><blockfielditem/param>
        /// <returns>xml</returns>
        private string GetCaptionLabelXml(TBlockFieldItem item, bool b)
        {
            Label label = new Label();
            label.Text = string.IsNullOrEmpty(item.Description) ? item.DataField : item.Description;
            if (b)
            {
                label.ID = string.Format("Caption{0}", WzdUtils.RemoveSpecialCharacters(item.DataField));
            }
            else
            {
                label.ID = string.Format("Caption{0}Label", WzdUtils.RemoveSpecialCharacters(item.DataField));
            }
            return GetControlXml(label);
        }

        /// <summary>
        /// 取得Collecion对应的xml
        /// </summary>
        /// <param name="prop">Collection的propinfo</param>
        /// <param name="collection">Collection的value</param>
        /// <returns>xml</returns>
        private string GetCollectionXml(PropertyInfo prop, InfoOwnerCollection collection)
        {
            //if (collection is AjaxTools.ExtGridColumnCollection)
            //    INFOLIGHTMARK = "ajaxTools";
            StringBuilder builder = new StringBuilder();
            if (prop != null && collection != null && prop.PropertyType == collection.GetType())
            {
                if (collection.Count > 0)
                {
                    builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                    for (int i = 0; i < collection.Count; i++)
                    {
                        PropertyInfo[] infos = null;
                        if (collection is AjaxTools.ExtGridColumnCollection || collection is AjaxTools.AjaxFormFieldCollection
                            || collection is AjaxTools.ExtSimpleColumnCollection || collection is AjaxTools.ExtQueryFieldCollection)
                        {
                            builder.Append(string.Format("\t\t<{0}:{1} ", "ajaxTools", collection.ItemType.Name));
                            infos = collection.ItemType.GetProperties();
                        }
                        else
                        {
                            builder.Append(string.Format("\t\t<{0}:{1} ", INFOLIGHTMARK, collection.ItemType.Name));
                            infos = collection.ItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        }
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
        /// 取得tag在html的最始位置，如&lt;form&gt;
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="tag">tag</param>
        /// <returns>最始位置</returns>
        private int IndexOfBeginTag(string html, string tag)
        {
            Match mc = Regex.Match(html, string.Format(@"<{0}\s*>", tag));
            if (mc.Success)
            {
                return mc.Index;
            }
            else
            {
                return -1;
            }
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
#endif
    }
}
