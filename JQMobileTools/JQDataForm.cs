using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.Design;
using System.Data;
using System.Collections.Specialized;

namespace JQMobileTools
{
    [Designer(typeof(JQDataFormDesigner), typeof(IDesigner))]
    public class JQDataForm : WebControl, IJQDataSourceProvider, IColumnCaptions, IDetailObject, IThemeObject
    {
        public JQDataForm()
            : base()
        {
            columns = new JQCollection<JQFormColumn>(this);
            relationColumns = new JQCollection<JQRelationColumn>(this);
            Title = "JQDataForm";
            AppliedClose = true;
            HorizontalColumnsCount = 1;
        }

        private string theme;
        public string Theme
        {
            get
            {
                if (string.IsNullOrEmpty(theme))
                {
                    var scriptManager = this.Parent.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    return scriptManager != null ? scriptManager.Theme : string.Empty;
                }
                return theme;
            }
            set
            {
                theme = value;
            }
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

        public string Title { get; set; }

        string IDetailObject.ParentObjectID
        {
            get
            {
                var grid = this.Parent.Controls.OfType<JQDataGrid>().FirstOrDefault(c => c.EditFormID == this.ID);
                if (grid != null)
                {
                    var parentGrid = this.Parent.Controls.OfType<JQDataGrid>().FirstOrDefault(c => c.DetailObjectID == grid.ID);
                    if (parentGrid != null)
                    {
                        return parentGrid.ID;
                    }
                }
                return string.Empty;
            }
        }

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
        [Editor(typeof(JQAlignmentEditor), typeof(UITypeEditor))]
        public string CaptionAlignment { get; set; }


        [Category("Infolight")]
        public bool DuplicateCheck { get; set; }

        private bool isShowCloseButton = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool IsShowCloseButton {
            get
            {
                return isShowCloseButton;
            }
            set
            {
                isShowCloseButton = value;
            }
        }

        [Category("Infolight")]
        public bool AppliedClose { get; set; }
        [Category("Infolight")]
        public bool AutoSubmit { get; set; }
        [Category("Infolight")]
        public bool AutoPause { get; set; }

        [Category("Infolight")]
        public string OnLoadSuccess { get; set; }
        [Category("Infolight")]
        public string OnApply { get; set; }
        [Category("Infolight")]
        public string OnApplied { get; set; }
        [Category("Infolight")]
        public string OnCancel { get; set; }

        /// <summary>
        /// 是否显示Flow按钮
        /// </summary>
        [Category("Infolight")]
        public bool IsShowFlowIcon { get; set; }

        /// <summary>
        /// 是否關閉通知
        /// </summary>
        [Category("Infolight")]
        public bool IsNotifyOFF { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FormControlEditor), typeof(UITypeEditor))]
        public string ChainDataFormID { get; set; }
        [Category("Infolight")]
        public int HorizontalColumnsCount { get; set; }

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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderPage
        {
            get
            {
                return this.Parent == null || !(this.Parent is JQTabItem);
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

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                var formTheme = Theme;
                if (RenderPage)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                    writer.AddAttribute(JQProperty.DataOverlayTheme, Theme);
                    if (!IsShowCloseButton)
                        writer.AddAttribute("data-close-btn", "none");
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Page);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Header);
                    if (!string.IsNullOrEmpty(formTheme))
                    {
                        writer.AddAttribute(JQProperty.DataTheme, formTheme);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    if (!string.IsNullOrEmpty(Title))
                    {
                        if (QueryString["Cordova"] == "true")
                        {
                        }
                        else
                        {
                            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0px");
                        }
                        writer.RenderBeginTag(HtmlTextWriterTag.H1);
                        writer.Write(Title);
                        writer.RenderEndTag();
                    }

                    if (IsShowCloseButton)
                    {
                        //render back button
                        writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower().ToLower());
                        writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower().ToLower());
                        writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.CaratL);
                        writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
                        writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
                        writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.B);
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("javascript:$('#{0}').find('.info-form').form('close');", this.ID));
                        //writer.AddAttribute(JQProperty.DataRel, "back");
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write("Back");
                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();

                    if (!string.IsNullOrEmpty(formTheme))
                    {
                        writer.AddAttribute(JQProperty.DataTheme, formTheme);
                    }
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }

                if (!string.IsNullOrEmpty(formTheme))
                {
                    writer.AddAttribute(JQProperty.DataTheme, formTheme);
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Form);
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                if (IsShowFlowIcon && (QueryString["LISTID"] != null ||
                    QueryString["NAVMODE"] != null &&
                    (QueryString["NAVMODE"].ToString() == "Insert"
                    || QueryString["NAVMODE"].ToString() == "Modify"
                    || QueryString["NAVMODE"].ToString() == "Prepare"
                    || QueryString["NAVMODE"].ToString() == "Inquery")))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    String messageKey = "FLClientControls/FLNavigator/NavText";
                    EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                    string[] flowTexts = provider[messageKey].Split(';');

                    InitStates();

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    //writer.AddAttribute(HtmlTextWriterAttribute.Colspan, (HorizontalColumnsCount * 2).ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                    if (IsControlVisible("Submit"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //<a id="aInbox" data-role="button" data-theme="b" data-mini="true" onclick="gotoInbox()">待办事项</a>
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowSubmit");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[16]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "form-FlowSubmit");
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Submit");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:80px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowSubmit");
                        if (this.CurrentFLState == "Continue")
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doApprove('{0}', '{1}')", this.ID, flowTexts[17]));
                        }
                        else
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doSubmit('{0}', '{1}')", this.ID, flowTexts[16]));
                        }
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[16]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("Approve") && (string.IsNullOrEmpty(QueryString["PLUSROLES"]) || QueryString["PLUSROLES"].ToLower() == "null"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowApprove");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[17]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Approve");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowApprove");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doApprove('{0}', '{1}')", this.ID, flowTexts[17]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[17]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("Return") && QueryString["FLNAVIGATOR_MODE"] != "0" && (string.IsNullOrEmpty(QueryString["PLUSROLES"]) || QueryString["PLUSROLES"].ToLower() == "null"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowReturn");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[18]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Return");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowReturn");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doReturn('{0}', '{1}')", this.ID, flowTexts[18]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[18]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("Reject"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowReject");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[19]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Reject");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowPause");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doReject('{0}', '{1}')", this.ID, flowTexts[19]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[19]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("Plus") && (string.IsNullOrEmpty(QueryString["PLUSROLES"]) || QueryString["PLUSROLES"].ToLower() == "null"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowPlus");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[22]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Plus");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowPause");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "alert('Plus')");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doPlusApprove('{0}', '{1}')", this.ID, flowTexts[22]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[22]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (!IsNotifyOFF && IsControlVisible("Notify") && IsClose(QueryString["LISTID"]))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowNotify");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[20]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Notify");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowNotify");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doNotify('{0}', '{1}')", this.ID, flowTexts[20]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[20]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("FlowDelete"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowDelete");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[21]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-FlowDelete");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowDelete");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doFlowDelete('{0}', '{1}')", this.ID, flowTexts[21]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[21]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("Pause"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowPause");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[23]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Pause");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowPause");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doPause('{0}', '{1}')", this.ID, flowTexts[23]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[23]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }

                    if (IsControlVisible("Comment"))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "FlowComment");
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, flowTexts[24]);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                        writer.AddAttribute("data-role", "button");
                        writer.AddAttribute("data-theme", "b");
                        writer.AddAttribute("data-mini", "true");
                        writer.AddAttribute("iconcls", "icon-flow-Comment");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:inline-block;width:26px;height:26px;cursor:pointer;padding:2px;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-FlowComment");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("doComment('{0}', '{1}')", this.ID, flowTexts[24]));
                        //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(flowTexts[24]);
                        writer.RenderEndTag();
                        writer.RenderEndTag();//td
                    }
                    writer.RenderEndTag();//tr

                    writer.RenderEndTag();//Table
                }

                var jqDefault = this.Parent.Controls.OfType<JQDefault>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                var jqValidate = this.Parent.Controls.OfType<JQValidate>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                var jqAutoSeq = this.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                if (jqAutoSeq == null && this.Parent.Parent != null)
                {
                    jqAutoSeq = this.Parent.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                }

                var divClass = new string[] { "a", "b", "c", "d" };

                if (HorizontalColumnsCount > 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-grid-" + divClass[HorizontalColumnsCount - 2]);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }

                for (int i = 0; i < Columns.Count; i++)
                {
                    var column = Columns[i];

                    var relationColumn = this.RelationColumns.FirstOrDefault(c => c.FieldName == column.FieldName);
                    if (relationColumn != null)
                    {
                        column.Default = relationColumn.Value;
                    }
                    else if (jqDefault != null)
                    {
                        var defaultColumn = jqDefault.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                        if (defaultColumn != null)
                        {
                            column.Default = defaultColumn.Value;
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
                    if (jqAutoSeq != null)
                    {
                        if (jqAutoSeq.FieldName == column.FieldName)
                        {
                            column.AutoSeq = jqAutoSeq.Value;
                        }
                    }
                    if (HorizontalColumnsCount > 1)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-block-" + divClass[i % HorizontalColumnsCount]);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    }
                    column.Render(writer);
                    if (HorizontalColumnsCount > 1)
                    {
                        writer.RenderEndTag();
                    }
                }
                if (HorizontalColumnsCount > 1)
                {
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();


                if (this.Parent is JQTabItem)
                {
                    JQScriptManager.RenderPopup(writer, string.Format("{0}_popup", (this.Parent as JQTabItem).Parent.ID));
                }
                else
                {
                    JQScriptManager.RenderPopup(writer, string.Format("{0}_popup", ID));
                }
                //var popupID = string.Format("{0}_popup", ID);
                //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-content");
                //writer.AddAttribute(HtmlTextWriterAttribute.Id, popupID);
                //writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
                //writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
                //writer.AddAttribute(JQProperty.DataOverlayTheme, JQDataTheme.A);
                //writer.RenderBeginTag(HtmlTextWriterTag.Div);
                // //<a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                //writer.AddAttribute(JQProperty.DataRel, "back");
                //writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
                //writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Delete);

                //writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
                //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-btn-right");
                //writer.RenderBeginTag(HtmlTextWriterTag.A);
                //writer.Write("Close");
                //writer.RenderEndTag();
                //writer.RenderBeginTag(HtmlTextWriterTag.P);
                //writer.RenderEndTag();
                //writer.RenderEndTag();
                if (RenderPage)
                {
                    writer.RenderEndTag();

                    writer.RenderEndTag();
                }
                else
                {
                    writer.RenderEndTag();
                }

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
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("id: '{0}'", ID));
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("duplicateCheck:{0}", DuplicateCheck.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnLoadSuccess))
                {
                    options.Add(string.Format("onLoadSuccess:{0}", OnLoadSuccess));
                }
                if (!string.IsNullOrEmpty(ChainDataFormID))
                {
                    options.Add(string.Format("chainDataForm:'#{0}'", ChainDataFormID));
                }
                options.Add(string.Format("appliedClose:{0}", AppliedClose.ToString().ToLower()));
                options.Add(string.Format("autoSubmit:{0}", AutoSubmit.ToString().ToLower()));
                options.Add(string.Format("autoPause:{0}", AutoPause.ToString().ToLower()));
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
            if (flNavigatorMode != "" && navigatorMode != "")
            {
                string flMode = "", mode = "";
                switch (flNavigatorMode)
                {
                    case "0":
                    case "Submit":
                        flMode = "Submit";
                        if (listID != "" && flowFileName == "")
                            flMode = "Approve";
                        if (status == "NP" || status == "取回" || status == "Retake")
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
                        if (QueryString["PLUSAPPROVE"] != null && QueryString["PLUSAPPROVE"].ToString() == "1")
                            visible = true;
                        else
                            visible = false;
                        break;
                    case "Submit":
                    case "Return":
                    case "Notify":
                    case "Continue":
                    case "Inquery":
                    case "None":
                    case "Lock":
                    case "FSubmit":
                    case "RSubmit":
                        visible = false;
                        break;
                    case "Plus":
                        if (QueryString["STATUS"] != null && QueryString["STATUS"].ToString() == "AA")
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

    public class JQFormColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQFormColumn()
        {
            Alignment = "left";
            Width = 80;
            Editor = JQEditorControl.Text;
            EditorOptions = string.Empty;
            Visible = true;
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
        /// 编辑选项
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditorOptionsEditor), typeof(UITypeEditor))]
        public string EditorOptions { get; set; }

        [Category("Infolight")]
        public bool Visible { get; set; }
        [Category("Infolight")]
        public bool ReadOnly { get; set; }
        /// <summary>
        /// 最大输入字符数
        /// </summary>
        [Category("Infolight")]
        public int MaxLength { get; set; }
        [Category("Infolight")]
        public String PlaceHolder { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Default { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Validate { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AutoSeq { get; set; }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.FieldContain);
            if (!this.Visible)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            var contorlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
            var fieldID = string.Format("{0}_{1}", contorlID, this.FieldName);
            var dataForm = (this as IJQProperty).ParentProperty.Component as JQMobileTools.JQDataForm;
            if (this.Editor != JQEditorControl.RadioButtons && this.Editor != JQEditorControl.CheckBoxes)
            {
                writer.AddAttribute(JQProperty.For, fieldID);
                if (dataForm.CaptionAlignment != JQAlignment.Left)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, dataForm.CaptionAlignment);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(Caption);
                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, fieldID);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, FieldName);
            writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            if (this.MaxLength > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, this.MaxLength.ToString());
            }
            if (!string.IsNullOrEmpty(PlaceHolder))
                writer.AddAttribute("placeholder", PlaceHolder);
            var control = JQControl.CreateControl(this.Editor, this.EditorOptions);
            control.ID = fieldID;
            control.Caption = Caption;
            control.Theme = ((this as IJQProperty).ParentProperty.Component as JQDataForm).Theme;
            var dataOptions = DataOptions;
            if (!string.IsNullOrEmpty(control.EditorOptions))
            {
                dataOptions += string.Format(",{0}", control.EditorOptions);
            }
            writer.AddAttribute(JQProperty.DataOptions, dataOptions);
            control.Render(writer);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add(string.Format("width:{0}", Width));
                options.Add(string.Format("align:'{0}'", Alignment));
                options.Add(string.Format("readOnly:{0}", ReadOnly.ToString().ToLower()));
                if (!string.IsNullOrEmpty(Default))
                {
                    options.Add(string.Format("defaultValue:{{{0}}}", Default));
                }
                if (!string.IsNullOrEmpty(Validate))
                {
                    options.Add(string.Format("validate:{{{0}}}", Validate));
                }
                if (!string.IsNullOrEmpty(AutoSeq))
                {
                    options.Add(string.Format("autoSeq:[{{{0}}}]", AutoSeq));
                }
                return string.Join(",", options);
            }
        }

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
    }

    public class JQRelationColumn : JQCollectionItem, IJQDataSourceProvider
    {
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ParentFieldEditor), typeof(UITypeEditor))]
        public string ParentFieldName { get; set; }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                values.Add("type:'field'");
                values.Add(string.Format("value:['{0}']", ParentFieldName));
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
}
