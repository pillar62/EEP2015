using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Srvtools
{
    public class WebCustomValidator : CustomValidator, IBaseWebControl
    {
        public WebCustomValidator()
        { 
        }

        [Category("InfoLight")]
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

        [Category("InfoLight")]
        public string FieldName
        {
            get
            {
                object obj = this.ViewState["FieldName"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["FieldName"] = value;
            }
        }

        internal Control FindChildControl(string strid, Control ct)
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
                        Control ctrtn = FindChildControl(strid, ctchild);
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
                return FindChildControl(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return FindChildControl(ObjID, this.Page.Form);
                else
                    return FindChildControl(ObjID, this.Page);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            ClientScriptManager csm = Page.ClientScript;
        }

        protected override bool OnServerValidate(string value)
        {
            return ValidateAction(value);
        }

        public bool ValidateAction(string value)
        {
            object obj = this.GetObjByID(this.WebValidateID);
            if (obj != null && obj is WebValidate)
            {
                WebValidate validate = (WebValidate)obj;
                object[] validateRet = validate.CheckValidate(this.FieldName, value);
                this.ErrorMessage = validateRet[1].ToString() + "<br />" + validateRet[2].ToString() + "<br />" + validateRet[3].ToString();
                return (bool)validateRet[0];
            }
            return true;
        }
    }
}
