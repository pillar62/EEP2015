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
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebDropDownList), "Resources.WebDropDownList.ico")]
    public class WebDropDownList : DropDownList, ICallbackEventHandler
    {
        public WebDropDownList()
        {
        }

        private SYS_LANGUAGE language;

        [Category("InfoLight"),
        Description("Specifies the filter to get data")]
        public string Filter
        {
            get
            {
                object obj = this.ViewState["Filter"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Filter"] = value;
            }
        }

        /*[Category("InfoLight")]
        public bool MultiLevel
        {
            get
            {
                object obj = this.ViewState["MultiLevel"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["MultiLevel"] = value;
            }
        }*/

        [Category("InfoLight"),
        Description("add an empty data into the list")]
        public bool AutoInsertEmptyData
        {
            get
            {
                object obj = this.ViewState["AutoInsertEmptyData"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["AutoInsertEmptyData"] = value;
            }
        }

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

        [Category("InfoLight")]
        [Editor(typeof(FilterObjectEditor), typeof(UITypeEditor))]
        public string DriverObject
        {
            get
            {
                object obj = this.ViewState["DriverObject"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DriverObject"] = value;
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
            string[] args = eventArgument.Split(';');
            string whereStr = "";
            Control container = this.NamingContainer;
            if (container != null)
            {
                if (this.DriverObject != null && this.DriverObject != "")
                {
                    Control objDriver = container.FindControl(this.DriverObject);
                    int i = 0;
                    while (objDriver != null && objDriver is WebDropDownList)
                    {
                        WebDropDownList driver = (WebDropDownList)objDriver;
                        whereStr += driver.DataValueField + "='" + args[i] + "' and ";
                        objDriver = container.FindControl(driver.DriverObject);
                        i++;
                    }
                    if (args[i] != "")
                    {
                        whereStr += this.DataValueField + "='" + args[i] + "'";
                    }
                }
                else if (args.Length == 1)
                {
                    if (args[0] != "")
                    {
                        whereStr += this.DataValueField + "='" + args[0] + "'";
                    }
                    else
                    {
                        Control objDdl = container.FindControl(this.FilterObject);
                        while (objDdl != null && objDdl is WebDropDownList)
                        {
                            WebDropDownList ddl = (WebDropDownList)objDdl;
                            callBackRetVal += "filterSelect = document.getElementById('" + ddl.UniqueID + "');filterSelect.options.length=0;" + SetEmptyData(ddl) + "filterSelect.disabled=true;";
                            objDdl = container.FindControl(ddl.FilterObject);
                        }
                    }
                }

                if (whereStr != "")
                {
                    Control objDdl = container.FindControl(this.FilterObject);
                    if (objDdl != null && objDdl is WebDropDownList)
                    {
                        WebDropDownList ddl = (WebDropDownList)objDdl;
                        object objDs = this.GetObjByID(ddl.DataSourceID);
                        if (objDs != null && objDs is WebDataSource)
                        {
                            callBackRetVal = "var filterSelect = document.getElementById('" + ddl.UniqueID + "');filterSelect.options.length=0;" + SetEmptyData(ddl);
                            if (!whereStr.EndsWith(" and "))
                            {
                                WebDataSource wds = (WebDataSource)objDs;
                                wds.SetWhere(whereStr);
                                DataTable table = wds.View.Table;
                                int i = table.Rows.Count;
                                for (int j = 0; j < i; j++)
                                {
                                    callBackRetVal += "filterSelect.options.add(new Option('" + table.Rows[j][ddl.DataTextField].ToString() + "', '" + table.Rows[j][ddl.DataValueField].ToString() + "'));";
                                }
                                callBackRetVal += "filterSelect.disabled=false;";
                            }
                            else
                            {
                                callBackRetVal += "filterSelect.disabled=true;";
                                objDdl = container.FindControl(ddl.FilterObject);
                                while (objDdl != null && objDdl is WebDropDownList)
                                {
                                    ddl = (WebDropDownList)objDdl;
                                    callBackRetVal += "filterSelect = document.getElementById('" + ddl.UniqueID + "');filterSelect.options.length=0;" + SetEmptyData(ddl) + "filterSelect.disabled=true;";
                                    objDdl = container.FindControl(ddl.FilterObject);
                                }
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

        private string SetEmptyData(WebDropDownList ddl)
        {
            string ret = "";
            if (ddl.AutoInsertEmptyData)
            {
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDropDownList", "EmptyData", true);
                ret = "filterSelect.options.add(new Option('" + message + "',''));";
            }
            return ret;
        }

        /*protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode && ((this.FilterObject != null && this.FilterObject != "") || (this.DriverObject != null && this.DriverObject != "")))
            {
                if (this.SelectedValue == "")
                {
                    Control objFilter = this.NamingContainer.FindControl(this.FilterObject);
                    while (objFilter != null && objFilter is WebDropDownList)
                    {
                        WebDropDownList filter = (WebDropDownList)objFilter;
                        filter.SelectedIndex = -1;
                        objFilter = this.NamingContainer.FindControl(filter.FilterObject);
                    }
                }
            }
            base.OnLoad(e);
        }*/

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode && ((this.FilterObject != null && this.FilterObject != "") || (this.DriverObject != null && this.DriverObject != "")))
            {
                ClientScriptManager csm = this.Page.ClientScript;
                string arg = "";
                if (this.DriverObject != null && this.DriverObject != "")
                {
                    Control objDriver = this.NamingContainer.FindControl(this.DriverObject);
                    while (objDriver != null && objDriver is WebDropDownList)
                    {
                        WebDropDownList driver = (WebDropDownList)objDriver;
                        arg += driver.UniqueID + ".value+';'+";
                        objDriver = this.NamingContainer.FindControl(driver.DriverObject);
                    }
                    arg += "this.value";
                }
                else if (this.FilterObject != null && this.FilterObject != "")
                {
                    arg = "this.value";
                    //if (this.SelectedValue == "")
                    //{
                    this.SelectedIndex = -1;
                    Control objFilter = this.NamingContainer.FindControl(this.FilterObject);
                    while (objFilter != null && objFilter is WebDropDownList)
                    {
                        WebDropDownList filter = (WebDropDownList)objFilter;
                        filter.SelectedIndex = -1;
                        filter.Enabled = false;
                        objFilter = this.NamingContainer.FindControl(filter.FilterObject);
                    }
                    //}
                }

                if (arg != "")
                {
                    string callbackScript =
                        "function ReceiveServerData(arg)" +
                        "{" +
                            "eval(arg);" +
                        "}";
                    string changeScript = csm.GetCallbackEventReference(this, arg, callbackScript, null, true);
                    writer.AddAttribute("onchange", changeScript, true);
                }
            }
            base.Render(writer);
        }

        public void SetFilter()
        {
            if (this.Filter == null || this.Filter == "")
                return;
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                wds.SetWhere(this.Filter);
            }
        }

        public void ClearFilter()
        {
            if (this.Filter == null || this.Filter == "")
                return;
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                wds.SetWhere(string.Empty);
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            this.SetFilter();
            if (this.AutoInsertEmptyData)
            {
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDropDownList", "EmptyData", true);
                ListItem item = new ListItem(message, "");
                if (!this.Items.Contains(item))
                    this.Items.Insert(0, item);
            }
            try
            {
                base.OnDataBinding(e);
            }
            catch (Exception ex)
            {
                if (!(ex is InvalidOperationException))
                    this.SelectedIndex = 0;
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            this.ClearFilter();
        }
    }
}
