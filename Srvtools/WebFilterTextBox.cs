using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace Srvtools
{
    public class WebFilterTextBox : TextBox, ICallbackEventHandler
    {
        public WebFilterTextBox()
        { 
        }

        private SYS_LANGUAGE language;

        [Category("InfoLight")]
        [Editor(typeof(FilterObjectEditor), typeof(UITypeEditor))]
        public string FilterObject
        {
            get
            {
                object obj = this.ViewState["FilterObject"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["FilterObject"] = value;
            }
        }

        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            if (eventArgument != null)
            {
                Control objSelect = this.NamingContainer.FindControl(this.FilterObject);
                if (objSelect != null)
                {
                    if (objSelect is WebDropDownList)
                    {
                        WebDropDownList ddl = (WebDropDownList)objSelect;
                        object objDs = this.GetObjByID(ddl.DataSourceID);
                        if (objDs != null && objDs is WebDataSource)
                        {
                            WebDataSource wds = (WebDataSource)objDs;
                            string type = wds.InnerDataSet.Tables[wds.DataMember].Columns[ddl.DataValueField].DataType.ToString().ToLower();
                            if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                            || type == "system.uint64" || type == "system.int" || type == "system.int16"
                            || type == "system.int32" || type == "system.int64" || type == "system.single"
                            || type == "system.double" || type == "system.decimal")
                            {
                                wds.SetWhere(ddl.DataValueField + "=" + eventArgument);
                            }
                            else
                            {
                                wds.SetWhere(ddl.DataValueField + "='" + eventArgument + "'");
                            }
                            callBackRetVal = "var filterSelect = document.getElementById('" + ddl.UniqueID + "');filterSelect.options.length=0;";
                            if (ddl.AutoInsertEmptyData)
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDropDownList", "EmptyData", true);
                                callBackRetVal += "filterSelect.options.add(new Option('" + message + "',''));";
                            }
                            DataTable table = wds.InnerDataSet.Tables[wds.DataMember];
                            int i = table.Rows.Count;
                            for (int j = 0; j < i; j++)
                            {
                                callBackRetVal += "filterSelect.options.add(new Option('" + 
                                    table.Rows[j][ddl.DataTextField].ToString() + "', '" + table.Rows[j][ddl.DataValueField].ToString() + "'));";
                            }
                        }
                    }
                }
            }
        }

        public string GetCallbackResult()
        {
            return callBackRetVal;
        }

        private string callBackRetVal = "";

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            string callbackScript = "function ReceiveServerData(arg){eval(arg);}";
            string changeScript = csm.GetCallbackEventReference(this, "this.value", callbackScript, "", true);
            writer.AddAttribute("onchange", changeScript, true);
            base.Render(writer);
        }
    }

    public class FilterObjectEditor : UITypeEditor
    {
        public FilterObjectEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is WebFilterTextBox)
            {
                WebFilterTextBox filterTxt = (WebFilterTextBox)context.Instance;
                bool findControl = false;
                string Temp = "";
                foreach (Control ctrl in filterTxt.Page.Controls)
                {
                    if (ctrl is WebDetailsView)
                    {
                        #region DetailsView
                        WebDetailsView detView = (WebDetailsView)ctrl;
                        foreach (DataControlField field in detView.Fields)
                        {
                            if (field is TemplateField)
                            {
                                TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                bool bInEditTemplate = false;
                                if (EditBuilder != null)
                                {
                                    findControl = HasFilterTextBox(detView, EditBuilder.Text, filterTxt.ID);
                                    Temp = "EditItemTemplate";
                                    bInEditTemplate = findControl;
                                }
                                if (!bInEditTemplate)
                                {
                                    findControl = HasFilterTextBox(detView, InsertBuilder.Text, filterTxt.ID);
                                    Temp = "InsertItemTemplate";
                                }
                                if (findControl)
                                {
                                    break;
                                }
                            }
                        }
                        if (findControl)
                        {
                            foreach (DataControlField field in detView.Fields)
                            {
                                if (field is TemplateField)
                                {
                                    if (Temp == "EditItemTemplate")
                                    {
                                        TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                        if (EditBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(detView, EditBuilder.Text, filterTxt.ID));
                                        }
                                    }
                                    else if (Temp == "InsertItemTemplate")
                                    {
                                        TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                        if (InsertBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(detView, InsertBuilder.Text, filterTxt.ID));
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        #endregion
                    }
                    else if (ctrl is WebFormView)
                    {
                        #region FormView
                        WebFormView frmView = (WebFormView)ctrl;
                        TemplateBuilder EditBuilder = (TemplateBuilder)frmView.EditItemTemplate;
                        TemplateBuilder InsertBuilder = (TemplateBuilder)frmView.InsertItemTemplate;
                        bool bInEditTemplate = false;
                        if (EditBuilder != null)
                        {
                            findControl = HasFilterTextBox(frmView, EditBuilder.Text, filterTxt.ID);
                            Temp = "EditItemTemplate";
                            bInEditTemplate = findControl;
                        }
                        if (!bInEditTemplate)
                        {
                            findControl = HasFilterTextBox(frmView, InsertBuilder.Text, filterTxt.ID);
                            Temp = "InsertItemTemplate";
                        }
                        if (findControl)
                        {
                            if (Temp == "EditItemTemplate")
                            {
                                objName = GetControlNames(frmView, EditBuilder.Text, filterTxt.ID);
                            }
                            else if (Temp == "InsertItemTemplate")
                            {
                                objName = GetControlNames(frmView, InsertBuilder.Text, filterTxt.ID);
                            }
                            break;
                        }
                        #endregion
                    }
                    else if (ctrl is WebGridView)
                    {
                        #region GridView
                        WebGridView gdView = (WebGridView)ctrl;
                        foreach (DataControlField field in gdView.Columns)
                        {
                            if (field is TemplateField)
                            {
                                TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                bool bInEditTemplate = false;
                                if (EditBuilder != null)
                                {
                                    findControl = HasFilterTextBox(gdView, EditBuilder.Text, filterTxt.ID);
                                    Temp = "EditItemTemplate";
                                    bInEditTemplate = findControl;
                                }
                                if (!bInEditTemplate && FooterBuilder != null)
                                {
                                    findControl = HasFilterTextBox(gdView, FooterBuilder.Text, filterTxt.ID);
                                    Temp = "FooterTemplate";
                                }
                                if (findControl)
                                {
                                    break;
                                }
                            }
                        }
                        if (findControl)
                        {
                            foreach (DataControlField field in gdView.Columns)
                            {
                                if (field is TemplateField)
                                {
                                    if (Temp == "EditItemTemplate")
                                    {
                                        TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                        if (EditBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(gdView, EditBuilder.Text, filterTxt.ID));
                                        }
                                    }
                                    else if (Temp == "FooterTemplate")
                                    {
                                        TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                        if (FooterBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(gdView, FooterBuilder.Text, filterTxt.ID));
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        #endregion
                    }
                }
            }
            else if (context.Instance is WebDropDownList)
            {
                WebDropDownList ddl = (WebDropDownList)context.Instance;
                bool findControl = false;
                string Temp = "";
                foreach (Control ctrl in ddl.Page.Controls)
                {
                    if (ctrl is WebDetailsView)
                    {
                        #region DetailsView
                        WebDetailsView detView = (WebDetailsView)ctrl;
                        foreach (DataControlField field in detView.Fields)
                        {
                            if (field is TemplateField)
                            {
                                TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                bool bInEditTemplate = false;
                                if (EditBuilder != null)
                                {
                                    findControl = HasFilterTextBox(detView, EditBuilder.Text, ddl.ID);
                                    Temp = "EditItemTemplate";
                                    bInEditTemplate = findControl;
                                }
                                if (!bInEditTemplate)
                                {
                                    findControl = HasFilterTextBox(detView, InsertBuilder.Text, ddl.ID);
                                    Temp = "InsertItemTemplate";
                                }
                                if (findControl)
                                {
                                    break;
                                }
                            }
                        }
                        if (findControl)
                        {
                            foreach (DataControlField field in detView.Fields)
                            {
                                if (field is TemplateField)
                                {
                                    if (Temp == "EditItemTemplate")
                                    {
                                        TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                        if (EditBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(detView, EditBuilder.Text, ddl.ID));
                                        }
                                    }
                                    else if (Temp == "InsertItemTemplate")
                                    {
                                        TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                        if (InsertBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(detView, InsertBuilder.Text, ddl.ID));
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        #endregion
                    }
                    else if (ctrl is WebFormView)
                    {
                        #region FormView
                        WebFormView frmView = (WebFormView)ctrl;
                        TemplateBuilder EditBuilder = (TemplateBuilder)frmView.EditItemTemplate;
                        TemplateBuilder InsertBuilder = (TemplateBuilder)frmView.InsertItemTemplate;
                        bool bInEditTemplate = false;
                        if (EditBuilder != null)
                        {
                            findControl = HasFilterTextBox(frmView, EditBuilder.Text, ddl.ID);
                            Temp = "EditItemTemplate";
                            bInEditTemplate = findControl;
                        }
                        if (!bInEditTemplate)
                        {
                            findControl = HasFilterTextBox(frmView, InsertBuilder.Text, ddl.ID);
                            Temp = "InsertItemTemplate";
                        }
                        if (findControl)
                        {
                            if (Temp == "EditItemTemplate")
                            {
                                objName = GetControlNames(frmView, EditBuilder.Text, ddl.ID);
                            }
                            else if (Temp == "InsertItemTemplate")
                            {
                                objName = GetControlNames(frmView, InsertBuilder.Text, ddl.ID);
                            }
                            break;
                        }
                        #endregion
                    }
                    else if (ctrl is WebGridView)
                    {
                        #region GridView
                        WebGridView gdView = (WebGridView)ctrl;
                        foreach (DataControlField field in gdView.Columns)
                        {
                            if (field is TemplateField)
                            {
                                TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                bool bInEditTemplate = false;
                                if (EditBuilder != null)
                                {
                                    findControl = HasFilterTextBox(gdView, EditBuilder.Text, ddl.ID);
                                    Temp = "EditItemTemplate";
                                    bInEditTemplate = findControl;
                                }
                                if (!bInEditTemplate && FooterBuilder != null)
                                {
                                    findControl = HasFilterTextBox(gdView, FooterBuilder.Text, ddl.ID);
                                    Temp = "FooterTemplate";
                                }
                                if (findControl)
                                {
                                    break;
                                }
                            }
                        }
                        if (findControl)
                        {
                            foreach (DataControlField field in gdView.Columns)
                            {
                                if (field is TemplateField)
                                {
                                    if (Temp == "EditItemTemplate")
                                    {
                                        TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                        if (EditBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(gdView, EditBuilder.Text, ddl.ID));
                                        }
                                    }
                                    else if (Temp == "FooterTemplate")
                                    {
                                        TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                        if (FooterBuilder != null)
                                        {
                                            objName.AddRange(this.GetControlNames(gdView, FooterBuilder.Text, ddl.ID));
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        #endregion
                    }
                }
            }
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }

        private bool HasFilterTextBox(Control Container, string BuilderText, string txtID)
        {
            IDesignerHost host = (IDesignerHost)Container.Site.GetService(typeof(IDesignerHost));
            Control[] ctrls = ControlParser.ParseControls(host, BuilderText);
            int i = ctrls.Length;
            for (int j = 0; j < i; j++)
            {
                if ((ctrls[j] is WebFilterTextBox || ctrls[j] is WebDropDownList) && ctrls[j].ID == txtID)
                {
                    return true;
                }
            }
            return false;
        }

        private List<string> GetControlNames(Control Container, string BuilderText, string txtID)
        {
            IDesignerHost host = (IDesignerHost)Container.Site.GetService(typeof(IDesignerHost));
            Control[] ctrls = ControlParser.ParseControls(host, BuilderText);
            List<string> ctrlNames = new List<string>();
            int i = ctrls.Length;
            for (int j = 0; j < i; j++)
            {
                if (ctrls[j] is WebDropDownList && ctrls[j].ID != txtID)
                {
                    ctrlNames.Add(ctrls[j].ID);
                }
            }
            return ctrlNames;
        }
    }
}
