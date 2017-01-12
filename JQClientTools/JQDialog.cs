using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;

namespace JQClientTools
{
    public class JQDialog : Panel
    {
        public JQDialog()
        {
            Closed = true;
            Width = new Unit(600, UnitType.Pixel);
            Title = "JQDialog";
            DialogLeft = new Unit(100, UnitType.Pixel);
            DialogTop = new Unit(100, UnitType.Pixel);
            ShowModal = true;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Title { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(IconEditor), typeof(UITypeEditor))]
        public string Icon { get; set; }

        /// <summary>
        /// 绑定控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string BindingObjectID { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        [Category("Infolight")]
        public bool Closed { get; set; }

        /// <summary>
        /// 编辑模式
        /// </summary>
        [Category("Infolight")]
        public EidtModeType EditMode { get; set; }

        /// <summary>
        /// Show Modal mode or not
        /// </summary>
        [Category("Infolight")]
        public bool ShowModal { get; set; }
        /// <summary>
        /// modal left
        /// </summary>
        [Category("Infolight")]
        public Unit DialogLeft { get; set; }
        /// <summary>
        /// Modal top
        /// </summary>
        [Category("Infolight")]
        public Unit DialogTop { get; set; }

        [Category("Infolight")]
        public string DataFormTabID { get; set; }

        [Category("Infolight")]
        public string DataGridTabID { get; set; }

        [Category("Infolight")]
        public string OnSubmited { get; set; }

        [Category("Infolight")]
        public string HelpLink { get; set; }

        [Category("Infolight")]
        public bool DialogCenter { get; set; }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Dialog);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.RenderEndTag();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8140;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);

                var styles = new List<string>();
                styles.Add(string.Format("width:{0}px", Width.Value));
                if (this.BindingObjectID != null && this.BindingObjectID != "")
                {
                    var bindingObject2 = this.Page.FindControl(this.BindingObjectID);
                    if (bindingObject2 != null )
                    {
                        JQDataForm dataForm = (JQDataForm)bindingObject2;
                        if (dataForm.AlwaysReadOnly)
                        {
                            styles.Add("padding:0px");
                        }
                        else
                            styles.Add("padding:10px");
                    }
                }
                else
                    styles.Add("padding:10px");
                if (EditMode == EidtModeType.Switch || EditMode == EidtModeType.Expand)
                {
                    styles.Add("display:none");
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }

                if (EditMode == EidtModeType.Switch)
                {
                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px;display:none", Width.Value));
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "info-dialog");
                }
                else if (EditMode == EidtModeType.Expand)
                {
                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px;display:none", Width.Value));
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "info-dialog");
                }
                else if (EditMode == EidtModeType.Continue)
                {
                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px;", Width.Value));
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "info-dialog");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Dialog);
                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);

                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if (this.BindingObjectID != null && this.BindingObjectID != "")
                {
                    var bindingObject2 = this.Page.FindControl(this.BindingObjectID);
                    JQDataForm dataForm = (JQDataForm)bindingObject2;
                    if (dataForm.AlwaysReadOnly)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: 0px");
                    }
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: 5px 0 5px 20px");
                }
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: 5px 0 5px 20px");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();

                //选择第一个内部的datagrid,给submit和cancel的div加上id，infolight.js里面会处理这个，有多层的时候会隐藏当前层的submit div，命名规则是andy定的
                foreach (var control in this.Controls)
                {
                    if (control.GetType() == typeof(JQDataGrid))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, (control as JQDataGrid).ID + "-SubmitDiv");
                        break;
                    }
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align: center; padding: 5px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "DialogSubmit");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-s");
                //JQDataForm dataForm = this.Page.FindControl(this.BindingObjectID) as JQDataForm;
                bool autoSubmit = false;
                Control bindingObject = null;
                if (this.BindingObjectID != null && this.BindingObjectID != "")
                {
                    bindingObject = this.Page.FindControl(this.BindingObjectID);
                    JQDataForm dataForm = (JQDataForm)bindingObject;
                    if (dataForm.IsControlVisible("Submit"))
                    {
                        if (bindingObject != null && bindingObject.GetType() == typeof(JQDataForm) && ((JQDataForm)bindingObject).IsAutoPause)
                        {
                            autoSubmit = true;
                            String messageKey = "FLClientControls/FLNavigator/NavText";
                            //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                            var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                            string[] flowTexts = provider[messageKey].Split(';');
                            string onClick = "submitForm('#" + this.ID + "', undefined, function(){ ";
                            if (dataForm.CurrentFLState != "Continue")
                            {
                                onClick += string.Format("doPause('winNotify', '{0}', '{1}', undefined, {2})", dataForm.ID, flowTexts[23], dataForm.IsAutoSubmit.ToString().ToLower());
                            }
                            onClick += "});";
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
                        }
                        else if (bindingObject != null && bindingObject.GetType() == typeof(JQDataForm) && ((JQDataForm)bindingObject).IsAutoSubmit)
                        {
                            autoSubmit = true;
                            String messageKey = "FLClientControls/FLNavigator/NavText";
                            //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                            var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                            string[] flowTexts = provider[messageKey].Split(';');

                            string approveText = flowTexts[17];
                            if (dataForm.CurrentFLState == "FSubmit" || dataForm.CurrentFLState == "RSubmit")
                                approveText = flowTexts[16];
                            string onClick = "submitForm('#" + this.ID + "', undefined, function(){ ";

                            if (dataForm.CurrentFLState == "Continue")
                            {
                                onClick += string.Format("doApprove('winApprove', '{0}', '{1}');", dataForm.ID, approveText);
                            }
                            else
                            {
                                onClick += string.Format("doSubmit('winSubmit', '{0}', '{1}');", dataForm.ID, flowTexts[16]);
                            }
                            onClick += "});";
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
                        }
                    }

                    else if (bindingObject != null && bindingObject.GetType() == typeof(JQDataForm) && ((JQDataForm)bindingObject).IsAutoSubmit)
                    //if (dataForm != null && dataForm.IsAutoSubmit)
                    {
                        autoSubmit = true;
                        String messageKey = "FLClientControls/FLNavigator/NavText";
                        //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                        var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                        string[] flowTexts = provider[messageKey].Split(';');

                        string approveText = flowTexts[17];
                        if (dataForm.CurrentFLState == "FSubmit" || dataForm.CurrentFLState == "RSubmit")
                            approveText = flowTexts[16];
                        string onClick = "submitForm('#" + this.ID + "', undefined, function(){ ";
                        if (dataForm.IsControlVisible("Submit"))
                        {
                            if (dataForm.CurrentFLState == "Continue")
                            {
                                onClick += string.Format("doApprove('winApprove', '{0}', '{1}');", dataForm.ID, approveText);
                            }
                            else
                            {
                                onClick += string.Format("doSubmit('winSubmit', '{0}', '{1}');", dataForm.ID, flowTexts[16]);
                            }
                        }
                        if (dataForm.IsControlVisible("Approve"))
                        {
                            onClick += string.Format("doApprove('winApprove', '{0}', '{1}');", dataForm.ID, approveText);
                        }
                        onClick += "});";
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
                    }

                }
                if (!autoSubmit)
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("submitForm('#{0}')", this.ID));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("Submit");
                writer.RenderEndTag();
                if (bindingObject != null && bindingObject.GetType() == typeof(JQDataForm) && !((JQDataForm)bindingObject).IsAutoPageClose)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-c");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("closeForm('#{0}')", this.ID));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Close");
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();

                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(Icon))
                //{
                //    optionBuilder.AppendFormat("iconCls:'{0}'", Icon);
                //    optionBuilder.Append(",");
                //}
                //optionBuilder.AppendFormat("closed:'{0}'", Closed);
                //return optionBuilder.ToString();
                var options = new List<string>();
                if (!string.IsNullOrEmpty(Icon))
                {
                    options.Add(string.Format("iconCls:'{0}'", Icon));
                }
                options.Add(string.Format("closed:'{0}'", Closed));
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
                }
                if (this.EditMode == EidtModeType.Dialog)
                {
                    options.Add(string.Format("modal:{0}", ShowModal.ToString().ToLower()));
                }
                if (!string.IsNullOrEmpty(HelpLink))
                {
                    options.Add(string.Format("tools:[{{iconCls:'icon-help',handler:function(){{window.open('{0}');}}}}]", HelpLink));
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
                //var optionBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(BindingObjectID))
                //{
                //    optionBuilder.AppendFormat("containForm:'#{0}'", BindingObjectID);
                //}
                //return optionBuilder.ToString();
                var options = new List<string>();
                if (!string.IsNullOrEmpty(BindingObjectID))
                {
                    options.Add(string.Format("containForm:'#{0}'", BindingObjectID));
                }
                if (this.DialogLeft.Type == UnitType.Pixel && !this.DialogLeft.IsEmpty)
                {
                    options.Add(string.Format("dialogLeft:{0}", this.DialogLeft.Value));
                }
                if (this.DialogTop.Type == UnitType.Pixel && !this.DialogTop.IsEmpty)
                {
                    options.Add(string.Format("dialogTop:{0}", this.DialogTop.Value));
                }
                if (!string.IsNullOrEmpty(DataFormTabID) && !string.IsNullOrEmpty(DataGridTabID))
                {
                    options.Add(string.Format("dataFormTabID:'{0}'", DataFormTabID));
                    options.Add(string.Format("dataGridTabID:'{0}'", DataGridTabID));
                }
                if (!string.IsNullOrEmpty(OnSubmited))
                {
                    options.Add(string.Format("onSubmited:{0}", OnSubmited));
                }
                options.Add(string.Format("dialogCenter:{0}", DialogCenter.ToString().ToLower()));

                return string.Join(",", options);
            }
        }
    }
}
