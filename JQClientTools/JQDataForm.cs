using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace JQClientTools
{
    [Designer(typeof(JQDataFormDesigner), typeof(IDesigner))]
    public class JQDataForm : WebControl, IJQDataSourceProvider, IDetailObject, IColumnCaptions
    {
        public JQDataForm()
        {
            columns = new JQCollection<JQFormColumn>(this);
            relationColumns = new JQCollection<JQRelationColumn>(this);
            HorizontalColumnsCount = 1;
            ValidateStyle = ValidateStyleType.Hint;
            toolitems = new JQCollection<JQToolItem>(this);
        }

        private string remoteName;
        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return remoteName;
            }
            set
            {
                if (this.DesignMode && value != remoteName)
                {
                    columnCaptions = null;
                }
                remoteName = value;
            }
        }

        private string currentFLState;
        [Category("Infolight"), Browsable(false)]
        public string CurrentFLState
        {
            get
            {
                return currentFLState;
            }
            set
            {
                currentFLState = value;
            }
        }


        private string dataMember;
        /// <summary>
        /// 表名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return dataMember;
            }
            set
            {
                if (this.DesignMode && value != dataMember)
                {
                    columnCaptions = null;
                }
                dataMember = value;
            }
        }

        private JQCollection<JQFormColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQFormColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        /// <summary>
        /// 是否关闭
        /// </summary>
        [Category("Infolight")]
        [Browsable(false)]
        public bool Closed { get; set; }

        [Category("Infolight")]
        public int HorizontalColumnsCount { get; set; }


        [Category("Infolight")]
        [Editor(typeof(JQAlignmentEditor), typeof(UITypeEditor))]
        public string CaptionAlignment { get; set; }

        /// <summary>
        /// 是否DuplicateCheck
        /// </summary>
        [Category("Infolight")]
        public bool DuplicateCheck { get; set; }

        /// <summary>
        /// 是否显示Flow按钮
        /// </summary>
        [Category("Infolight")]
        public bool IsShowFlowIcon { get; set; }

        /// <summary>
        /// 是否自动提交
        /// </summary>
        [Category("Infolight")]
        public bool IsAutoSubmit { get; set; }

        [Category("Infolight")]
        public bool IsAutoPause { get; set; }

        /// <summary>
        /// 是否送出流程成功後, 自動將頁面關閉
        /// </summary>
        [Category("Infolight")]
        public bool IsAutoPageClose { get; set; }

        [Category("Infolight")]
        public bool IsRejectON { get; set; }

        /// <summary>
        /// 是否關閉通知
        /// </summary>
        [Category("Infolight")]
        public bool IsNotifyOFF { get; set; }

        /// <summary>
        /// 是否连续新增
        /// </summary>
        [Category("Infolight")]
        public bool ContinueAdd { get; set; }

        [Category("Infolight")]
        public bool ShowApplyButton { get; set; }

        [Category("Infolight")]
        public string OnLoadSuccess { get; set; }
        [Category("Infolight")]
        public string OnBeforeValidate { get; set; }
        [Category("Infolight")]
        public string OnApply { get; set; }
        [Category("Infolight")]
        public string OnApplied { get; set; }
        [Category("Infolight")]
        public string OnCancel { get; set; }


        [Category("Infolight")]
        public bool IsRejectNotify { get; set; }

        [Category("Infolight")]
        public bool disapply { get; set; }

        /// <summary>
        /// 父控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string ParentObjectID { get; set; }

        private JQCollection<JQRelationColumn> relationColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQRelationColumn> RelationColumns
        {
            get
            {
                return relationColumns;
            }
        }

        [Category("Infolight")]
        public int HorizontalGap { get; set; }
        [Category("Infolight")]
        public int VerticalGap { get; set; }

        [Category("Infolight")]
        public bool DivFramed { get; set; }
        [Category("Infolight")]
        public string DivTitle { get; set; }

        [Category("Infolight")]
        public ValidateStyleType ValidateStyle { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FormControlEditor), typeof(UITypeEditor))]
        public string ChainDataFormID { get; set; }

        [Category("Infolight")]
        public bool AlwaysReadOnly { get; set; }

        private JQCollection<JQToolItem> toolitems;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQToolItem> TooItems
        {
            get
            {
                return toolitems;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NameValueCollection QueryString
        {
            get
            {
                if (this.Page != null)
                {
                    var scriptManager = this.Page.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    if (scriptManager == null)
                    {
                        scriptManager = this.Page.Form.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    }
                    if (scriptManager != null)
                    {
                        return scriptManager.QueryString;
                    }
                }
                return new NameValueCollection();
            }
        }


        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(RemoteName))
            {
                throw new JQProperyNullException(this.ID, typeof(JQDataForm), "RemoteName");
            }
            foreach (var column in this.Columns)
            {
                column.CheckProperties();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8100;
                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                //if (this.Closed)
                //{
                //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
                //}
                if (this.DivFramed)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Panel);
                    var dataO = string.Format("title:'{0}',collapsible:true", DivTitle);
                    var parent = this.Parent;
                    while (parent != null && !(parent is JQDialog))
                    {
                        parent = parent.Parent;
                    }
                    if (parent != null)
                    {
                        if (this.AlwaysReadOnly)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px", (parent as JQDialog).Width.Value));
                        }
                        else
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px", (parent as JQDialog).Width.Value - 60));
                        }
                        if (!String.IsNullOrEmpty((parent as JQDialog).HelpLink))
                        {
                            dataO += string.Format(",tools:[{{iconCls:'icon-help',handler:function(){{window.open('{0}');}}}}]", (parent as JQDialog).HelpLink);
                        }
                    }
                     writer.AddAttribute(JQProperty.DataOptions, dataO);

                }
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);



                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("border-spacing:{0}px {1}px;", HorizontalGap, VerticalGap));
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                if (IsShowFlowIcon && (QueryString["LISTID"] != null ||
                    QueryString["NAVMODE"] != null &&
                    (QueryString["NAVMODE"] == "Insert"
                    || QueryString["NAVMODE"] == "Modify"
                    || QueryString["NAVMODE"] == "Prepare"
                    || QueryString["NAVMODE"] == "Inquery")))
                {
                    var parent = this.Parent;
                    while (parent != null && !(parent is JQDialog))
                    {
                        parent = parent.Parent;
                    }
                    if (parent != null && ((parent as JQDialog).EditMode == EidtModeType.Continue || (parent as JQDialog).EditMode == EidtModeType.Expand))
                    {
                        throw new Exception("無法使用Continue或Expand模式呈送Flow單據");
                    }

                    String messageKey = "FLClientControls/FLNavigator/NavText";
                    //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                    var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                    string[] flowTexts = provider[messageKey].Split(';');

                    InitStates();

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, (HorizontalColumnsCount * 2).ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    if (QueryString["NAVMODE"] != null && (QueryString["NAVMODE"] == "Insert" || QueryString["NAVMODE"] == "Modify") ||
                        QueryString["NAVIGATOR_MODE"] != null && (QueryString["NAVIGATOR_MODE"] == "1" || QueryString["NAVIGATOR_MODE"] == "2"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowEdit");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[6]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doFlowEdit('{0}', 'updated', '{1}');", this.ID, (parent as JQDialog).ID));
                        writer.AddAttribute("iconcls", "icon-edit");
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[6]);
                        writer.RenderEndTag();
                    }

                    string navigatorMode = QueryString["NAVIGATOR_MODE"];
                    if (String.IsNullOrEmpty(navigatorMode))
                    {
                        navigatorMode = QueryString["NAVMODE"];
                    }
                    if (IsControlVisible("Submit") && (!IsAutoSubmit || navigatorMode == "Inquery" || this.CurrentFLState == "Continue"))
                    {
                        String sTitle = flowTexts[16];
                        //'<span class="icon-flow-Select" title="Select" style="display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;" />';
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowSubmit");
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:80px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowSubmit");

                        if (this.CurrentFLState == "Continue")
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doApprove('winApprove', '{0}', '{1}')", this.ID, flowTexts[17]));
                            sTitle = flowTexts[17];
                            writer.AddAttribute("iconcls", "icon-flow-Approve");
                        }
                        else
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doSubmit('winSubmit', '{0}', '{1}')", this.ID, flowTexts[16]));
                            sTitle = flowTexts[16];
                            writer.AddAttribute("iconcls", "icon-flow-Submit");
                        }
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, sTitle);
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(sTitle);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("Approve") && (string.IsNullOrEmpty(QueryString["PLUSROLES"]) || QueryString["PLUSROLES"].ToLower() == "null"))
                    {
                        string approveText = flowTexts[17];
                        if (this.CurrentFLState == "FSubmit" || this.CurrentFLState == "RSubmit")
                            approveText = flowTexts[16];

                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowApprove");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, approveText);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Approve");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowApprove");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doApprove('winApprove', '{0}', '{1}')", this.ID, approveText));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(approveText);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("Return") && QueryString["FLNAVIGATOR_MODE"] != "0" && (string.IsNullOrEmpty(QueryString["PLUSROLES"]) || QueryString["PLUSROLES"].ToLower() == "null"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowReturn");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[18]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Return");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowReturn");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doReturn('winReturn', '{0}', '{1}')", this.ID, flowTexts[18]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[18]);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("Reject") || ((IsControlVisible("Approve") || IsControlVisible("Submit")) && IsRejectON))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowReject");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[19]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Reject");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowPause");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doReject('winNotify', '{0}', '{1}')", this.ID, flowTexts[19]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[19]);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("Plus") && (string.IsNullOrEmpty(QueryString["PLUSROLES"]) || QueryString["PLUSROLES"].ToLower() == "null"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowPlus");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[22]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Plus");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowPause");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "alert('Plus')");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doPlusApprove('winPlusApprove', '{0}', '{1}')", this.ID, flowTexts[22]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[22]);
                        writer.RenderEndTag();
                    }

                    if (!IsNotifyOFF && IsControlVisible("Notify") && IsClose(QueryString["LISTID"]))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowNotify");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[20]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Notify");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowNotify");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doNotify('winNotify', '{0}', '{1}')", this.ID, flowTexts[20]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[20]);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("FlowDelete"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowDelete");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[21]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-FlowDelete");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowDelete");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doFlowDelete('winNotify', '{0}', '{1}')", this.ID, flowTexts[21]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[21]);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("Pause"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowPause");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[23]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Pause");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowPause");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doPause('winNotify', '{0}', '{1}')", this.ID, flowTexts[23]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[23]);
                        writer.RenderEndTag();
                    }

                    if (IsControlVisible("Comment"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowComment");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[24]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("iconcls", "icon-flow-Comment");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowComment");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doComment('winComment', '{0}', '{1}')", this.ID, flowTexts[24]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[24]);
                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

                if (AlwaysReadOnly && this.TooItems.Count > 0)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, (HorizontalColumnsCount * 2).ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    foreach (var toolItem in this.toolitems)
                    {
                        if (toolItem.Visible)
                        {
                            if (toolItem.ID != "")
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Id, toolItem.ID);
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, toolItem.ItemType);
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$('#{0}').infoform('callfunction','{1}');", this.ID, toolItem.OnClick));
                            writer.AddAttribute("iconcls", toolItem.Icon);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(toolItem.Text);
                            writer.RenderEndTag();
                        }
                    }
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

                var jqDefault = this.Parent.Controls.OfType<JQDefault>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                if (jqDefault == null && this.Parent.Parent != null)
                {
                    jqDefault = this.Parent.Parent.Controls.OfType<JQDefault>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                }
                var jqValidate = this.Parent.Controls.OfType<JQValidate>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                if (jqValidate == null && this.Parent.Parent != null)
                {
                    jqValidate = this.Parent.Parent.Controls.OfType<JQValidate>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                }

                for (int i = 0; i < Columns.Count; i++)
                {
                    var column = Columns[i];


                    if (jqDefault != null)
                    {
                        var defaultColumn = jqDefault.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                        if (defaultColumn != null)
                        {
                            //column.Default = EFClientTools.ClientUtility.GetSysValue(defaultColumn.Value);
                            column.Default = defaultColumn.Value;
                            column.CarryOn = defaultColumn.CarryOn;
                        }
                    }
                    if (jqValidate != null)
                    {
                        var validateColumn = jqValidate.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                        if (validateColumn != null)
                        {
                            column.Validate = validateColumn.Value;
                        }
                    }
                    var jqAutoSeq = this.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID && c.FieldName == column.FieldName);
                    if (jqAutoSeq == null && this.Parent.Parent != null)
                    {
                        jqAutoSeq = this.Parent.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID && c.FieldName == column.FieldName);
                    }

                    if (jqAutoSeq != null)
                    {
                        column.AutoSeq = jqAutoSeq.Value;
                    }
                }

                var visibleColumns = Columns.Where(c => c.Visible).ToList();

                int columnPosition = 0;
                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    var column = visibleColumns[i];
                    //if (i % HorizontalColumnsCount == 0)
                    //{
                    //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    //}
                    if (columnPosition == 0)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    }
                    if (i > 0 && column.NewRow) //属性换行
                    {
                        writer.RenderEndTag();
                        columnPosition = 0;
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    }
                    //重设span
                    else if (columnPosition + column.Span > HorizontalColumnsCount) //自动换行
                    {
                        writer.RenderEndTag();
                        columnPosition = 0;
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    }
                    column.Render(writer);
                    columnPosition += column.Span;

                    if (columnPosition >= HorizontalColumnsCount || i == visibleColumns.Count - 1)
                    {
                        writer.RenderEndTag();
                        columnPosition = 0;
                    }
                    //if (i == Columns.Count - 1 || (i + 1) % HorizontalColumnsCount == 0)
                    //{
                    //    writer.RenderEndTag();
                    //}
                }

                var hiddenColumns = Columns.Where(c => !c.Visible);
                if (hiddenColumns.Count() > 0)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    foreach (var column in hiddenColumns)
                    {
                        column.Render(writer);
                    }
                    writer.RenderEndTag();
                }

                if (ShowApplyButton)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, (HorizontalColumnsCount * 2).ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-s");

                    if (this.IsShowFlowIcon && this.IsControlVisible("Submit"))
                    {
                        if (this.IsAutoPause)
                        {
                            String messageKey = "FLClientControls/FLNavigator/NavText";
                            //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                            var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                            string[] flowTexts = provider[messageKey].Split(';');
                            string onClick = "submitForm('#" + this.ID + "', undefined, function(){ ";
                            if (this.CurrentFLState != "Continue")
                            {
                                onClick += string.Format("doPause('winNotify', '{0}', '{1}',undefined, {2})", this.ID, flowTexts[23], this.IsAutoSubmit.ToString().ToLower());
                            }
                            onClick += "});";
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
                        }
                        else if (this.IsAutoSubmit)
                        {
                            String messageKey = "FLClientControls/FLNavigator/NavText";
                            //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                            var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                            string[] flowTexts = provider[messageKey].Split(';');

                            string approveText = flowTexts[17];
                            if (this.CurrentFLState == "FSubmit" || this.CurrentFLState == "RSubmit")
                                approveText = flowTexts[16];
                            string onClick = "submitForm('#" + this.ID + "', undefined, function(){ ";
                            if (this.CurrentFLState == "Continue")
                            {
                                onClick += string.Format("doApprove('winApprove', '{0}', '{1}');", this.ID, approveText);
                            }
                            else
                            {
                                onClick += string.Format("doSubmit('winSubmit', '{0}', '{1}');", this.ID, flowTexts[16]);
                            }
                            onClick += "});";
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
                        }
                    }
                    else if (this.IsAutoSubmit)
                    {
                        String messageKey = "FLClientControls/FLNavigator/NavText";
                        //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                        var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                        string[] flowTexts = provider[messageKey].Split(';');

                        string approveText = flowTexts[17];
                        if (this.CurrentFLState == "FSubmit" || this.CurrentFLState == "RSubmit")
                            approveText = flowTexts[16];
                        string onClick = "submitForm('#" + this.ID + "', undefined, function(){ ";
                        if (this.IsControlVisible("Submit"))
                        {
                            if (this.CurrentFLState == "Continue")
                            {
                                onClick += string.Format("doApprove('winApprove', '{0}', '{1}');", this.ID, approveText);
                            }
                            else
                            {
                                onClick += string.Format("doSubmit('winSubmit', '{0}', '{1}');", this.ID, flowTexts[16]);
                            }
                        }
                        if (this.IsControlVisible("Approve"))
                        {
                            onClick += string.Format("doApprove('winApprove', '{0}', '{1}');", this.ID, approveText);
                        }
                        onClick += "});";
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
                    }
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("applyForm('#{0}')", this.ID));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Submit");
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        private bool IsClose(String listId)
        {
            EFClientTools.EFServerReference.FlowDataParameter fdp = new EFClientTools.EFServerReference.FlowDataParameter();
            fdp.ListID = new Guid(listId);
            var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, EFClientTools.EFServerReference.FlowDataType.History, fdp);
            DataSet ds = Deserialize<DataSet>(ds1);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                String status = item["STATUS"].ToString();
                if (status == "Z" || status == "结案" || status == "結案" || status == "X" || status == "作废" || status == "作廢")
                {
                    return false;
                }
            }
            return true;
        }

        private static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
            // No settings need modifying here      
            using (System.IO.StringReader textReader = new System.IO.StringReader(xml))
            {
                using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string InfolightOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("remoteName:'{0}'", RemoteName);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("tableName:'{0}'", DataMember);
                //return optionBuilder.ToString();

                var options = new List<string>();
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                if (!string.IsNullOrEmpty(ParentObjectID))
                {
                    options.Add(string.Format("parent:'{0}'", ParentObjectID));

                    var relations = new List<string>();
                    foreach (var column in this.RelationColumns)
                    {
                        relations.Add(string.Format("{{field:'{0}',parentField:'{1}'}}", column.FieldName, column.ParentFieldName));
                    }
                    options.Add(string.Format("parentRelations:[{0}]", string.Join(",", relations)));
                }
                if (!string.IsNullOrEmpty(ChainDataFormID))
                {
                    options.Add(string.Format("chainDataForm:'#{0}'", ChainDataFormID));
                }
                options.Add(string.Format("duplicateCheck:{0}", DuplicateCheck.ToString().ToLower()));
                options.Add(string.Format("continueAdd:{0}", ContinueAdd.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnLoadSuccess))
                {
                    options.Add(string.Format("onLoadSuccess:{0}", OnLoadSuccess));
                }
                if (!string.IsNullOrEmpty(OnBeforeValidate))
                {
                    options.Add(string.Format("onBeforeValidate:{0}", OnBeforeValidate));
                }
                if (!string.IsNullOrEmpty(OnApply))
                {
                    options.Add(string.Format("onApply:{0}", OnApply));
                }
                if (!string.IsNullOrEmpty(OnApplied))
                {
                    options.Add(string.Format("onApplied:{0}", OnApplied));
                }
                if (!string.IsNullOrEmpty(OnCancel))
                {
                    options.Add(string.Format("onCancel:{0}", OnCancel));
                }
                options.Add(string.Format("rejectNotify:{0}", IsRejectNotify.ToString().ToLower()));
                options.Add(string.Format("isAutoPageClose:{0}", IsAutoPageClose.ToString().ToLower()));
                options.Add(string.Format("validateStyle:'{0}'", ValidateStyle.ToString().ToLower()));
                options.Add(string.Format("isshowflowicon:'{0}'", IsShowFlowIcon.ToString().ToLower()));
                options.Add(string.Format("disapply:{0}", disapply.ToString().ToLower()));
                options.Add(string.Format("alwaysReadOnly:{0}", AlwaysReadOnly.ToString().ToLower()));
                return string.Join(",", options);
            }
        }

        private Dictionary<string, string> columnCaptions;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Dictionary<string, string> IColumnCaptions.ColumnCaptions
        {
            get
            {
                if (this.DesignMode)
                {
                    if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember))
                    {
                        return null;
                    }
                    if (columnCaptions == null)
                    {
                        columnCaptions = new Dictionary<string, string>();
                        var assemblyName = RemoteName.Split('.')[0];
                        var commandName = RemoteName.Split('.')[1];
                        var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                        clientInfo.UseDataSet = true;
                        var columnDefinations = EFClientTools.DesignClientUtility.Client.GetColumnDefination(clientInfo, assemblyName, DataMember, null)
                            .OfType<EFClientTools.EFServerReference.COLDEF>();
                        foreach (var columnDefination in columnDefinations)
                        {
                            columnCaptions.Add(columnDefination.FIELD_NAME
                                , string.IsNullOrEmpty(columnDefination.CAPTION) ? columnDefination.FIELD_NAME : columnDefination.CAPTION);
                        }
                    }
                }
                return columnCaptions;
            }
        }

        private void InitStates()
        {
            string listID = QueryString["LISTID"];
            string flowFileName = QueryString["FLOWFILENAME"];
            string status = QueryString["STATUS"];
            string flNavigatorMode = QueryString["FLNAVMODE"];
            if (String.IsNullOrEmpty(flNavigatorMode))
            {
                flNavigatorMode = QueryString["FLNAVIGATOR_MODE"];
            }
            string navigatorMode = QueryString["NAVIGATOR_MODE"];
            if (String.IsNullOrEmpty(navigatorMode))
            {
                navigatorMode = QueryString["NAVMODE"];
            }
            if (flNavigatorMode != "" || navigatorMode != "")
            {
                string flMode = "", mode = "";
                switch (flNavigatorMode)
                {
                    case "0":
                    case "Submit":
                        flMode = "Submit";
                        if (listID != "" && String.IsNullOrEmpty(flowFileName))
                            flMode = "Approve";
                        if (status == "NF" || status == "取回" || status == "Retake")
                            flMode = "FSubmit";
                        else if (status == "NR" || status == "退回" || status == "Return")
                            flMode = "RSubmit";
                        break;
                    case "1":
                        flMode = "Approve";
                        break;
                    case "2":
                        flMode = "Return";
                        break;
                    case "3":
                        flMode = "Notify";
                        break;
                    case "4":
                        flMode = "Inquery";
                        break;
                    case "5":
                        flMode = "Continue";
                        break;
                    case "6":
                        flMode = "None";
                        break;
                    case "7":
                        flMode = "Plus";
                        break;
                    case "8":
                        flMode = "Lock";
                        break;
                    default:
                        flMode = "Inquery";
                        break;
                }
                if (flMode != "")
                {
                    this.SetFLState(flMode);
                }
                switch (navigatorMode)
                {
                    case "0":
                        mode = "Normal";
                        break;
                    case "1":
                        mode = "Insert";
                        break;
                    case "2":
                        mode = "Modify";
                        break;
                    case "3":
                        mode = "Inquery";
                        break;
                    case "4":
                        mode = "Prepare";
                        break;
                    /*以下状态为系统内定,不允许用户自行设置*/
                    case "5":
                        mode = "PreInsert";
                        break;
                    default:
                        mode = navigatorMode;
                        break;
                }
                if (mode != "")
                {
                    //if (mode == "Inquery" || mode == "Prepare")
                    //{
                    //    WebDataSource wds = this.GetDataSource() as WebDataSource;
                    //    if (wds != null)
                    //    {
                    //        if (wds.IsEmpty)
                    //        {
                    //            mode = "Inquery";
                    //        }
                    //        else
                    //        {
                    //            int record = wds.InnerDataSet.Tables[0].Rows.Count;
                    //            if (record == 1)
                    //            {
                    //                mode = string.Format("{0}Single", mode);
                    //            }
                    //            else
                    //            {
                    //                mode = string.Format("{0}Multi", mode);
                    //            }
                    //        }
                    //    }
                    //}
                    //this.SetNavState(mode);
                }
            }
        }

        public void SetFLState(string flStateText)
        {
            this.CurrentFLState = flStateText;
        }

        public bool IsControlVisible(string ctrlName)
        {
            bool visible = true;
            if (ctrlName == "Submit")
            {
                switch (this.CurrentFLState)
                {
                    case "Submit":
                    case "Return":
                    case "Continue":
                        visible = true;
                        break;
                    case "Approve":
                    case "Inquery":
                    case "Notify":
                    case "None":
                    case "Plus":
                    case "Lock":
                    case "FSubmit":
                    case "RSubmit":
                        visible = false;
                        break;
                }
            }
            else if (ctrlName == "Approve")
            {
                switch (this.CurrentFLState)
                {
                    case "Approve":
                    case "Plus":
                    case "FSubmit":
                    case "RSubmit":
                        visible = true;
                        break;
                    case "Submit":
                    case "Return":
                    case "Continue":
                    case "Inquery":
                    case "Notify":
                    case "None":
                    case "Lock":
                        visible = false;
                        break;

                }
            }
            else if (ctrlName == "Return")
            {
                switch (this.CurrentFLState)
                {
                    case "Approve":
                    case "Plus":
                        visible = true;
                        break;
                    case "RSubmit":
                    case "Submit":
                    case "Return":
                    case "Continue":
                    case "Inquery":
                    case "Notify":
                    case "None":

                    case "Lock":
                    case "FSubmit":
                        visible = false;
                        break;
                }
            }
            else if (ctrlName == "Reject")
            {
                switch (this.CurrentFLState)
                {
                    case "Return":
                    case "FSubmit":
                    case "RSubmit":
                        visible = true;
                        break;
                    case "Submit":
                    case "Approve":
                    case "Continue":
                    case "Inquery":
                    case "Notify":
                    case "None":
                    case "Plus":
                    case "Lock":
                        visible = false;
                        break;
                }
            }
            else if (ctrlName == "Notify")
            {
                switch (this.CurrentFLState)
                {
                    case "Notify":
                    case "Approve":
                    case "Return":
                    case "Continue":
                    case "Plus":
                    case "Lock":
                    case "FSubmit":
                    case "RSubmit":
                        visible = true;
                        break;
                    case "Submit":
                    case "Inquery":
                    case "None":
                        visible = false;
                        break;
                }
            }
            else if (ctrlName == "FlowDelete")
            {
                switch (this.CurrentFLState)
                {
                    case "Notify":
                        visible = true;
                        break;
                    case "Submit":
                    case "Approve":
                    case "Return":
                    case "Continue":
                    case "Inquery":
                    case "None":
                    case "Plus":
                    case "Lock":
                    case "FSubmit":
                    case "RSubmit":
                        visible = false;
                        break;
                }
            }
            else if (ctrlName == "Plus")
            {
                switch (this.CurrentFLState)
                {
                    case "Approve":
                    case "Continue":
                        if (QueryString["PLUSAPPROVE"] != null && QueryString["PLUSAPPROVE"] == "1")
                            visible = true;
                        else
                            visible = false;
                        break;
                    case "Submit":
                    case "Return":
                    case "Notify":
                    case "Inquery":
                    case "None":
                    case "Lock":
                    case "FSubmit":
                    case "RSubmit":
                        visible = false;
                        break;
                    case "Plus":
                        if (QueryString["STATUS"] != null && QueryString["STATUS"] == "AA")
                            visible = true;
                        else
                            visible = false;
                        break;
                }
            }
            else if (ctrlName == "Pause")
            {
                switch (this.CurrentFLState)
                {
                    case "Submit":
                        visible = true;
                        break;
                    case "Notify":
                    case "Approve":
                    case "Return":
                    case "Continue":
                    case "Inquery":
                    case "None":
                    case "Plus":
                    case "Lock":
                    case "FSubmit":
                    case "RSubmit":
                        visible = false;
                        break;
                }
            }
            else if (ctrlName == "Comment")
            {
                switch (this.CurrentFLState)
                {
                    case "Approve":
                    case "Notify":
                    case "Inquery":
                    case "Continue":
                    case "Plus":
                    case "None":
                    case "RSubmit":
                    case "FSubmit":
                        visible = true;
                        break;
                    case "Submit":
                    case "Return":
                    case "Lock":
                        visible = false;
                        break;
                }
            }
            return visible;
        }
    }

    public enum ValidateStyleType
    {
        Hint,
        Dialog,
        Both
    }


    public class JQFormColumn : JQCollectionItem, IJQDataSourceProvider, ICloneable
    {
        public JQFormColumn()
        {
            Alignment = "left";
            Width = 80;
            Editor = JQEditorControl.TextBox;
            Span = 1;
            RowSpan = 1;
        }

        private string fieldName;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;

                if (Component != null && Component.ColumnCaptions != null)
                {
                    if (string.IsNullOrEmpty(caption) && Component.ColumnCaptions.ContainsKey(fieldName))
                    {
                        Caption = Component.ColumnCaptions[fieldName];
                    }
                }
            }
        }

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

        /// <summary>
        /// 对齐
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(JQAlignmentEditor), typeof(UITypeEditor))]
        public string Alignment { get; set; }

        

        /// <summary>
        /// 宽度
        /// </summary>
        [Category("Infolight")]
        public int Width { get; set; }
        /// <summary>
        /// 编辑器
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditControlEditor), typeof(UITypeEditor))]
        public string Editor { get; set; }


        /// <summary>
        /// 格式
        /// </summary>
        [Category("Infolight")]
        public string Format { get; set; }

        /// <summary>
        /// 编辑选项
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditorOptionsEditor), typeof(UITypeEditor))]
        public string EditorOptions { get; set; }
        /// <summary>
        /// 最大输入字符数
        /// </summary>
        [Category("Infolight")]
        public int MaxLength { get; set; }

        private bool _ReadOnly = false;
        /// <summary>
        /// ReadOnly Column
        /// </summary>
        [Category("Infolight")]
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
            }
        }

        [Category("Infolight")]
        public int Span { get; set; }

        [Category("Infolight")]
        public int RowSpan { get; set; }
        [Category("Infolight")]
        public bool NewRow { get; set; }

        [Category("Infolight")]
        public String PlaceHolder { get; set; }

        private bool _Visible = true;
        /// <summary>
        /// Visible Column
        /// </summary>
        [Category("Infolight")]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        [Category("Infolight")]
        public string OnBlur { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Class
        {
            get
            {
                if (!string.IsNullOrEmpty(Editor))
                {
                    switch (Editor)
                    {
                        case JQEditorControl.ComboBox:
                            return JQControl.ComboBox;
                        case JQEditorControl.ComboGrid:
                            return JQControl.ComboGrid;
                        case JQEditorControl.DateBox:
                            return JQControl.DateBox;
                        case JQEditorControl.TimeSpinner:
                            return JQControl.TimeSpinner;
                        case JQEditorControl.NumberBox:
                            return JQControl.NumberBox;
                        case JQEditorControl.TextBox:
                        case JQEditorControl.TextArea:
                        case JQEditorControl.Password:
                            if (!string.IsNullOrEmpty(Validate))
                            {
                                return JQControl.ValidateBox;
                            }
                            break;
                        case JQEditorControl.RefValBox:
                            return JQControl.RefValBox;
                        case JQEditorControl.FileUpload:
                            return JQControl.FileUpload;
                        case JQEditorControl.AutoComplete:
                            return JQControl.AutoComplete;
                        case JQEditorControl.Options:
                            return JQControl.Options;
                        case JQEditorControl.Qrcode:
                            return JQControl.Qrcode;
                        case JQEditorControl.YearMonth:
                            return JQControl.YearMonth;
                        default: break;
                    }
                }
                return string.Empty;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Type
        {
            get
            {
                if (!string.IsNullOrEmpty(Editor))
                {
                    switch (Editor)
                    {
                        case JQEditorControl.CheckBox:
                            return "checkbox";
                        case JQEditorControl.Password:
                            return "password";
                        default: break;
                    }
                }
                return "text";
            }
        }

        internal void CheckProperties()
        {
            var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
            if (string.IsNullOrEmpty(FieldName) && this.Editor != JQEditorControl.FileUpload)
            {
                throw new JQProperyNullException(controlID, typeof(JQDataForm), "Columns.FieldName");
            }
            if (this.Editor == JQEditorControl.RefValBox)
            {
                var editor = new JQRefval() { ID = string.Format("{0}_{1}", controlID, FieldName) };
                editor.LoadProperties(EditorOptions);
                editor.CheckProperties();
            }
            else if (this.Editor == JQEditorControl.ComboGrid)
            {
                var editor = new JQComboGrid() { ID = string.Format("{0}_{1}", controlID, FieldName) };
                editor.LoadProperties(EditorOptions);
                editor.CheckProperties();
            }
            else if (this.Editor == JQEditorControl.ComboBox)
            {
                var editor = new JQComboBox() { ID = string.Format("{0}_{1}", controlID, FieldName) };
                editor.LoadProperties(EditorOptions);
                editor.CheckProperties();
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                if (this.RowSpan > 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, this.RowSpan.ToString());
                }
                var dataForm = (this as IJQProperty).ParentProperty.Component as JQClientTools.JQDataForm;

                if (dataForm.CaptionAlignment != JQAlignment.Left)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, dataForm.CaptionAlignment);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(Caption);
                writer.RenderEndTag();

                if (this.Span > 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, ((this.Span - 1) * 2 + 1).ToString());
                }
                if (this.RowSpan > 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, this.RowSpan.ToString());
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                var contorlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
            


                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}{1}", contorlID, this.FieldName));
                if (Editor != JQEditorControl.ComboGrid)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, this.FieldName);
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, this.Type);
                }
                writer.AddAttribute("placeholder", PlaceHolder);
                if (this.MaxLength > 0)
                {
                    writer.AddAttribute("maxlength", this.MaxLength.ToString());
                }
                var styles = new List<string>();

                styles.Add(string.Format("width:{0}px", Width));

                if (this.Alignment.ToLower() == "right" || this.Alignment.ToLower() == "center")
                {
                    styles.Add(string.Format("text-align:{0}", this.Alignment.ToLower()));
                }

                if (Editor == JQEditorControl.TextArea)
                {
                    var textarea = new JQTextArea();
                    textarea.LoadProperties(EditorOptions);
                    if (textarea.Height.Type == UnitType.Pixel && textarea.Height.Value > double.Epsilon)
                    {
                        styles.Add(string.Format("height:{0}px", textarea.Height.Value));
                    }
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }


                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + this.Width.ToString() + "px");
                if (this.ReadOnly)
                {
                    if (Editor == JQEditorControl.TextArea)
                        writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");
                    else 
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                }
                if (string.Equals(this.Editor, JQEditorControl.ComboBox.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    var editoptions = this.EditorOptions;
                    var combobox = new JQComboBox();
                    combobox.LoadProperties(editoptions);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.ComboBox);

                    if (combobox.RemoteName != null && combobox.RemoteName != "")
                    {
                        writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                        writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                        writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    }
                    else
                    {
                        writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                        writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                        writer.RenderBeginTag(HtmlTextWriterTag.Select);
                        if (combobox.Items != null)
                        {
                            foreach (JQComboItem item in combobox.Items)
                            {
                                if (item.Selected)
                                {
                                    writer.Write("<option value=\"" + item.Value + "\" selected=\"true\">" + item.Text + "</option>");
                                }
                                else
                                    writer.Write("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                            }
                        }
                    }
                    writer.RenderEndTag();
                }
                else if (Editor == JQEditorControl.TextArea)
                {
                    if (!string.IsNullOrEmpty(this.Class))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    }
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    if (!string.IsNullOrEmpty(OnBlur))
                    {
                        writer.AddAttribute("onblur", OnBlur + "()");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                    writer.RenderEndTag();
                }
                else if (Editor == JQEditorControl.DateBox)
                {
                    var editoptions = this.EditorOptions;
                    var dateBox = new JQDateBox();
                    dateBox.LoadProperties(editoptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    if (dateBox.ShowTimeSpinner)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.DateTimeBox);
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                        if (string.IsNullOrEmpty(Validate) || !Validate.Contains("validType"))
                        {
                            if (!string.IsNullOrEmpty(Validate))
                            {
                                Validate += ",";
                            }
                            if (!string.IsNullOrEmpty(Format) && Format.IndexOf("YY") == 0)
                            {
                                Validate += "validType:'rocDatetime'";
                            }
                            else
                            {
                                Validate += "validType:'datetime'";
                            }
                        }
                    }

                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);

                    if (!string.IsNullOrEmpty(OnBlur))
                    {
                        writer.AddAttribute("onblur", OnBlur + "()");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                else if (Editor == JQEditorControl.Qrcode)
                {
                    if (!string.IsNullOrEmpty(this.Class))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    }
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderEndTag();
                }
                else if (Editor == JQEditorControl.ComboGrid || Editor == JQEditorControl.YearMonth)
                {
                    if (!string.IsNullOrEmpty(this.Class))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    }
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    writer.RenderBeginTag(HtmlTextWriterTag.Select);
                    writer.RenderEndTag();
                }
                else if (Editor == JQEditorControl.TextBox)
                {
                    if (!string.IsNullOrEmpty(this.Class))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    }
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    if (!string.IsNullOrEmpty(OnBlur))
                    {
                        writer.AddAttribute("onblur", OnBlur + "()");
                    }
                    var textbox = new JQTextBox();
                    textbox.LoadProperties(this.InfolightOptions);
                    if (textbox.CapsLock != CapsLockEnum.None)
                    {
                        writer.AddAttribute("onKeyUp", string.Format("$.changeCapsLock2($(this), '{0}');", textbox.CapsLock.ToString().ToLower()));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                else if (Editor == JQEditorControl.FixTextBox)
                {
                    if (!string.IsNullOrEmpty(this.Class))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    }
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    if (!string.IsNullOrEmpty(OnBlur))
                    {
                        writer.AddAttribute("onblur", OnBlur + "()");
                    }
                    var textbox = new JQTextBox();
                    textbox.LoadProperties(this.InfolightOptions);
                    if (textbox.CapsLock != CapsLockEnum.None)
                    {
                        writer.AddAttribute("onKeyUp", string.Format("$.changeCapsLock2($(this), '{0}');", textbox.CapsLock.ToString().ToLower()));
                    }
                    writer.AddAttribute("onKeyUp", string.Format("$.gotoNextControl($(this), '{0}');", contorlID));
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.Class))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    }
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    if (!string.IsNullOrEmpty(OnBlur))
                    {
                        writer.AddAttribute("onblur", OnBlur + "()");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                var contorlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}{1}", contorlID, this.FieldName));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.FieldName);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                //Andy提出JQDataForm中numberbox 如果在 Columns 設定 visible = false , 就會變回一般的 Text , 造成 .numberBox('setValue') 失敗, 就不能用到 Precision 小數點4捨5入的功能。
                if (Editor == JQEditorControl.NumberBox)
                {
                    writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();

                //if (!string.IsNullOrEmpty(Validate))
                //{
                //    optionBuilder.Append(Validate);
                //    optionBuilder.Append(",");
                //}
                //if (this.Editor == JQEditorControl.RefValBox)
                //{

                //}
                //else
                //{
                //    optionBuilder.Append(EditorOptions);
                //}
                //return optionBuilder.ToString().TrimEnd(',');
                var dataForm = (this as IJQProperty).ParentProperty.Component as JQDataForm;
                var options = new List<string>();

                if (dataForm.ValidateStyle == ValidateStyleType.Hint)
                {
                    if (!string.IsNullOrEmpty(Validate))
                    {
                        options.Add(Validate);
                        //if (Editor == JQEditorControl.DateBox && !Validate.Contains("validType"))
                        //{
                        //    if (!string.IsNullOrEmpty(Format) && Format.IndexOf("YY") == 0)
                        //    {
                        //        options.Add("validType:'rocDatetime'");
                        //    }
                        //    else
                        //    {
                        //        options.Add("validType:'datetime'");
                        //    }
                        //}
                    }
                    else
                    {
                        //if (Editor == JQEditorControl.DateBox)
                        //{
                        //    if (!string.IsNullOrEmpty(Format) && Format.IndexOf("YY") == 0)
                        //    {
                        //        options.Add("validType:'rocDatetime'");
                        //    }
                        //    else
                        //    {
                        //        options.Add("validType:'datetime'");
                        //    }
                        //}
                    }
                }

                if (this.Editor == JQEditorControl.RefValBox || this.Editor == JQEditorControl.ComboGrid || this.Editor == JQEditorControl.ComboBox || this.Editor == JQEditorControl.FileUpload || this.Editor == JQEditorControl.AutoComplete)
                {

                }
                else
                {
                    if (!string.IsNullOrEmpty(EditorOptions))
                    {
                        options.Add(EditorOptions);
                    }
                }
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string InfolightOptions
        {
            get
            {
                var dataForm = (this as IJQProperty).ParentProperty.Component as JQDataForm;
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("field:'{0}'", FieldName);
                //optionBuilder.Append(",");
                //if (!string.IsNullOrEmpty(Default))
                //{
                //    optionBuilder.AppendFormat("defaultValue:'{0}'", Default);
                //    optionBuilder.Append(",");
                //}
                //optionBuilder.AppendFormat("form:'{0}'", dataForm.ID);
                //optionBuilder.Append(",");
                //if (this.Editor == JQEditorControl.RefValBox)
                //{
                //    optionBuilder.Append(EditorOptions);
                //}
                //return optionBuilder.ToString().TrimEnd(',');
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                if (!string.IsNullOrEmpty(Format))
                {
                    if (Format.IndexOf("image", StringComparison.CurrentCultureIgnoreCase) == 0 || Format.IndexOf("download", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        if (Format.IndexOf("image", StringComparison.CurrentCultureIgnoreCase) == 0)
                            options.Add("format:'image'");
                        else if (Format.IndexOf("download", StringComparison.CurrentCultureIgnoreCase) == 0)
                            options.Add("format:'download'");

                        string[] formats = Format.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var eachformat in formats)
                        {
                            string[] eachformats = eachformat.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (eachformats.Length == 2)
                            {
                                options.Add(string.Format("{0}:'{1}'", eachformats[0].ToLower(), eachformats[1]));
                            }
                        }
                    }
                    else
                    {
                        options.Add(string.Format("format:'{0}'", Format));
                    }
                }
                if (!string.IsNullOrEmpty(Default))
                {
                    options.Add(string.Format("defaultValue:'{0}'", Default));
                }
                if (CarryOn)
                {
                    options.Add(string.Format("carryOn:{0}", CarryOn.ToString().ToLower()));
                }
                if (!string.IsNullOrEmpty(AutoSeq))
                {
                    options.Add(string.Format("autoSeq:[{{{0}}}]", AutoSeq));
                }
                //if (dataForm.ValidateStyle == ValidateStyleType.Dialog)
                //{
                if (!string.IsNullOrEmpty(Validate))
                {
                    options.Add(Validate);
                }
                if (!string.IsNullOrEmpty(PlaceHolder))
                {
                    options.Add(string.Format("placeholder:'{0}'", PlaceHolder));
                }
                //}
                options.Add(string.Format("form:'{0}'", dataForm.ID));
                if (this.Editor == JQEditorControl.RefValBox || this.Editor == JQEditorControl.ComboGrid || this.Editor == JQEditorControl.ComboBox
                    || this.Editor == JQEditorControl.FileUpload || this.Editor == JQEditorControl.AutoComplete || this.Editor == JQEditorControl.Options
                    || this.Editor == JQEditorControl.Qrcode || this.Editor == JQEditorControl.YearMonth || this.Editor == JQEditorControl.TextBox)
                {
                    if (!string.IsNullOrEmpty(EditorOptions))
                    {
                        options.Add(EditorOptions);
                    }
                }
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Default { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Validate { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CarryOn { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AutoSeq { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IColumnCaptions Component
        {
            get
            {
                if ((this as IJQProperty).ParentProperty != null && (this as IJQProperty).ParentProperty.Component != null)
                {
                    return (this as IJQProperty).ParentProperty.Component as IColumnCaptions;
                }
                return null;
            }
        }

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

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
