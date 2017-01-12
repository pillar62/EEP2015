using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Resources;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebValidateBox), "Resources.WebValidateBox.ico")]
    public class  WebValidateBox : TextBox, ICallbackEventHandler, IGetValues
    {
        public WebValidateBox()
        {
        }

        [Category("InfoLight"),
        Editor(typeof(ValidateEditor), typeof(UITypeEditor))]
        public string WebValidateID
        {
            get
            {
                object obj = this.ViewState["WebValidateID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["WebValidateID"] = value;
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ValidateField
        {
            get
            {
                object obj = this.ViewState["ValidateField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["ValidateField"] = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "validatefield", true) == 0)//IgnoreCase
            {
                if (this.WebValidateID != null && this.WebValidateID != "")
                {
                    object obj = this.GetObjByID(this.WebValidateID);
                    if (obj != null && obj is WebValidate)
                    {
                        WebValidate validate = (WebValidate)obj;
                        foreach (ValidateFieldItem item in validate.Fields)
                        {
                            if (item.FieldName != null && item.FieldName != "")
                            {
                                values.Add(item.FieldName);
                            }
                        }
                    }
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
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

        private string ValMessage = "";
        public void RaiseCallbackEvent(string eventArgument)
        {
            bool bValidateSucess = true;
            ValMessage = "";
            object obj = this.GetObjByID(this.WebValidateID);
            if (obj != null && obj is WebValidate)
            {
                WebValidate validate = (WebValidate)obj;
                object[] valStates = validate.CheckValidate(this.ValidateField, eventArgument);
                if (valStates.Length == 4)
                {
                    bValidateSucess = (bool)valStates[0];
                    if (!bValidateSucess)
                    {
                        if (valStates[1].ToString() != "")
                        {
                            ValMessage += valStates[1].ToString() + ";";
                        }
                        if (valStates[2].ToString() != "")
                        {
                            ValMessage += valStates[2].ToString() + ";";
                        }
                        if (valStates[3].ToString() != "")
                        {
                            ValMessage += valStates[3].ToString() + ";";
                        }
                    }
                }
            }
            if (ValMessage != "")
            {
                ValMessage = ValMessage.Substring(0, ValMessage.LastIndexOf(';'));
            }
        }

        public string GetCallbackResult()
        {
            return ValMessage;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.CssClass != "") { writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass); }
            #region TextBox
            if (!this.DesignMode && this.WebValidateID != null && this.WebValidateID != "" && this.ValidateField != null && this.ValidateField != "")
            {
                ClientScriptManager csm = Page.ClientScript;
                string callBackSucess =
                    "function ReceiveServerData(arg)" +
                    "{" +
                        "var alertTable = document.getElementById('" + this.ClientID + "').parentElement;" + 
                        "if(arg != '')" +
                        "{" +
                            "var popupTableBody = document.createElement('tbody');" +
                            "var popupTableRow = document.createElement('tr');" +
                            "var calloutCell = document.createElement('td');" +
                            "var calloutTable = document.createElement('table');" +
                            "var calloutTableBody = document.createElement('tbody');" +
                            "var calloutTableRow = document.createElement('tr');" +
                            "var iconCell = document.createElement('td');" +
                            "var closeCell = document.createElement('td');" +
                            "var popupTable = document.createElement('table');" +
                            "var calloutArrowCell = document.createElement('td');" +
                            "var warningIconImage = document.createElement('img');" +
                            "var closeImage = document.createElement('img');" +
                            "var errorMessageCell = document.createElement('td');" +
                            // popupTable
                            "popupTable.id = 'popupTable';" +
                            "popupTable.style.zIndex = '2';" +
                            "popupTable.style.position = 'absolute';" + 
                            "popupTable.cellPadding = 0;" +
                            "popupTable.cellSpacing = 0;" +
                            "popupTable.border = 0;" +
                            "popupTable.width = '180px';" +
                            // popupTableRow
                            "popupTableRow.vAlign = 'top';" +
                            "popupTableRow.style.height = '100%';" +
                            // calloutCell
                            "calloutCell.width = '20px';" +
                            "calloutCell.align = 'right';" +
                            "calloutCell.style.height = '100%';" +
                            "calloutCell.style.verticalAlign = 'top';" +
                            // calloutTable
                            "calloutTable.cellPadding = 0;" +
                            "calloutTable.cellSpacing = 0;" +
                            "calloutTable.border = 0;" +
                            "calloutTable.style.height = '100%';" +
                            // _calloutArrowCell
                            "calloutArrowCell.align = 'right';" +
                            "calloutArrowCell.vAlign = 'top';" +
                            "calloutArrowCell.style.fontSize = '1px';" +
                            "calloutArrowCell.style.paddingTop = '8px';" +
                            // iconCell
                            "iconCell.width = '20px';" +
                            "iconCell.style.borderTop = '1px solid black';" +
                            "iconCell.style.borderLeft = '1px solid black';" +
                            "iconCell.style.borderBottom = '1px solid black';" +
                            "iconCell.style.padding = '5px';" +
                            "iconCell.style.backgroundColor = 'LemonChiffon';" +
                            // warningIconImage
                            "warningIconImage.border = 0;" +
                            "warningIconImage.src = '../Image/Ajax/alert-large.gif';" +
                            // errorMessageCell
                            "errorMessageCell.style.backgroundColor = 'LemonChiffon';" +
                            "errorMessageCell.style.fontFamily = 'verdana';" +
                            "errorMessageCell.style.fontSize = '12px';" +
                            "errorMessageCell.style.padding = '5px';" +
                            "errorMessageCell.style.borderTop = '1px solid black';" +
                            "errorMessageCell.style.borderBottom = '1px solid black';" +
                            "errorMessageCell.width = '100%';" +
                            "errorMessageCell.innerHTML = arg;" +
                            // closeCell
                            "closeCell.style.borderTop = '1px solid black';" +
                            "closeCell.style.borderRight = '1px solid black';" +
                            "closeCell.style.borderBottom = '1px solid black';" +
                            "closeCell.style.backgroundColor = 'lemonchiffon';" +
                            "closeCell.style.verticalAlign = 'top';" +
                            "closeCell.style.textAlign = 'right';" +
                            "closeCell.style.padding = '2px';" +
                            // closeImage
                            "closeImage.src = '../Image/Ajax/close.gif';" +
                            "closeImage.style.cursor = 'pointer';" +
                            "closeImage.onclick = function()" +
                            "{" +
                                "popupTable.style.display = 'none';" +
                            "};" +
                            "if(alertTable.childNodes['popupTable'] != undefined)" +
                            "{" +
                                "alertTable.removeChild(popupTable);" + //清空节点内容
                            "}" + 
                            // Create the DOM tree
                            "alertTable.appendChild(popupTable);" +
                            "popupTable.appendChild(popupTableBody);" +
                            "popupTableBody.appendChild(popupTableRow);" +
                            "popupTableRow.appendChild(calloutCell);" +
                            "calloutCell.appendChild(calloutTable);" +
                            "calloutTable.appendChild(calloutTableBody);" +
                            "calloutTableBody.appendChild(calloutTableRow);" +
                            "calloutTableRow.appendChild(calloutArrowCell);" +
                            "popupTableRow.appendChild(iconCell);" +
                            "iconCell.appendChild(warningIconImage);" +
                            "popupTableRow.appendChild(errorMessageCell);" +
                            "popupTableRow.appendChild(closeCell);" +
                            "closeCell.appendChild(closeImage);" +

                            // initialize callout arrow
                            "var div = document.createElement('div');" +
                            "div.style.fontSize = '1px';" +
                            "div.style.position = 'relative';" +
                            "div.style.left = '1px';" +
                            "div.style.borderTop = '1px solid black';" +
                            "div.style.width = '15px';" +
                            "calloutArrowCell.appendChild(div);" +
                            "for(var i = 14; i > 0; i--)" +
                            "{" +
                                "var line = document.createElement('div');" +
                                "line.style.width = i.toString() + 'px';" +
                                "line.style.height = '1px';" +
                                "line.style.overflow = 'hidden';" +
                                "line.style.backgroundColor = 'LemonChiffon';" +
                                "line.style.borderLeft = '1px solid black';" +
                                "div.appendChild(line);" +
                            "}" +

                            this.UniqueID + ".focus();" +
                            this.UniqueID + ".select();" +
                        "}" +
                        "else" + 
                        "{" +
                            "for(var i=0;i<alertTable.childNodes.length;i++)" +
                            "{"+
                                "if(alertTable.childNodes[i].id=='popupTable')" +
                                "{" +
                                    "alertTable.childNodes[i].style.display='none';" +
                                "}" +
                            "}"+
                        "}" + 
                    "}";
                string callBackFail =
                    "function CallBackFailed(errorMessage)" +
                    "{" +
                        "return;" +
                    "}";
                string argument = this.UniqueID + ".value";
                string changeScript = csm.GetCallbackEventReference(this, argument, callBackSucess, null, callBackFail, true);
                writer.AddAttribute("onchange", changeScript, true);
            }
            this.RenderBeginTag(writer);
            base.RenderContents(writer);
            this.RenderEndTag(writer);
            #endregion
        }
    }

    public class ValidateEditor : UITypeEditor
    {
        public ValidateEditor()
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
            if (context.Instance is WebValidateBox)
            {
                ControlCollection ctrlList = ((WebValidateBox)context.Instance).Page.Controls;

                foreach (Control ctrl in ctrlList)
                {
                    Control c = GetWebValidateBox(ctrl);
                    if (c != null)
                        objName.Add(c.ID);
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

        private Control GetWebValidateBox(Control ct)
        {
            if (ct is WebValidate)
            {
                return ct;
            }
            else if (ct.HasControls())
            {
                foreach (Control ctchild in ct.Controls)
                {
                    if (ctchild is WebValidate)
                    {
                        return ctchild;
                    }
                    else
                    {
                        GetWebValidateBox(ctchild);
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

}
