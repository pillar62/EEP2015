using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Xml;
using Microsoft.Win32;
using Srvtools;
using System.Collections.Generic;

namespace AjaxTools
{
    public class AjaxAutoComplete : TextBox, ICallbackEventHandler, INamingContainer, IGetSelectAlias
    {
        public AjaxAutoComplete()
        {
            if (this.ServicePath == null || this.ServicePath == "")
                this.ServicePath = "~/AutoComplete.asmx";
            if (this.ServiceMethod == null || this.ServiceMethod == "")
                this.ServiceMethod = "GetCompletionList";
        }

        private AjaxAutoCompleteExtender _autoCompleteExtender = new AjaxAutoCompleteExtender();

        #region Properties
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                EnsureChildControls();
                this._autoCompleteExtender.ID = "_autoCompleteExtender";
                this._autoCompleteExtender.TargetControlID = this.ClientID;
            }
        }

        [Category("Infolight")]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Category("Infolight")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.BehaviorID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(1000)]
        public int CompletionInterval
        {
            get 
            {
                EnsureChildControls();
                return this._autoCompleteExtender.CompletionInterval;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.CompletionInterval = value; 
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string CompletionListElementID
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.CompletionListElementID;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.CompletionListElementID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(10)]
        public int CompletionSetCount
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.CompletionSetCount;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.CompletionSetCount = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool EnableCaching
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.EnableCaching;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.EnableCaching = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(3)]
        public int MinimumPrefixLength
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.MinimumPrefixLength;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.MinimumPrefixLength = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("GetCompletionList")]
        public string ServiceMethod
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.ServiceMethod;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.ServiceMethod = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("~/AutoComplete.asmx")]
        public string ServicePath
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.ServicePath;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.ServicePath = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(AjaxAutoCompleteSelectAliasEditor), typeof(UITypeEditor))]
        public string SelectAlias
        {
            get
            {
                object obj = this.ViewState["SelectAlias"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["SelectAlias"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(AjaxAutoCompleteCommandTableEditor), typeof(UITypeEditor))]
        public string CommandTable
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.CommandTable;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.CommandTable = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(AjaxAutoCompleteDataKeyFieldEditor), typeof(UITypeEditor))]
        public string DataKeyField
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.DataKeyField;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.DataKeyField = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(",;")]
        public string DelimiterCharacters
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.DelimiterCharacters;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.DelimiterCharacters = value;
            }
        }

        [Category("Infolight")]
        [Browsable(false)]
        public string UserId
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.UserID;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.UserID = value;
            }
        }

        [Category("Infolight")]
        [Browsable(false)]
        public string Password
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.Password;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.Password = value;
            }
        }

        [Category("Infolight")]
        [Browsable(false)]
        public string DataBase
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.DataBase;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.DataBase = value;
            }
        }

        [Category("Infolight")]
        [Browsable(false)]
        public string Solution
        {
            get
            {
                EnsureChildControls();
                return this._autoCompleteExtender.Solution;
            }
            set
            {
                EnsureChildControls();
                this._autoCompleteExtender.Solution = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool AutoInsert
        {
            get
            {
                object obj = this.ViewState["AutoInsert"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["AutoInsert"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(4)]
        public int AutoInsertLength
        {
            get
            {
                object obj = this.ViewState["AutoInsertLength"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 4;
            }
            set
            {
                this.ViewState["AutoInsertLength"] = value;
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.UserId = CliUtils.fLoginUser;
            this.Password = CliUtils.fLoginPassword;
            this.DataBase = CliUtils.fLoginDB;
            this.Solution = CliUtils.fCurrentProject;
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!this.DesignMode)
            {
                _autoCompleteExtender = new AjaxAutoCompleteExtender();
                _autoCompleteExtender.ID = "_autoCompleteExtender";
                this.Controls.Add(_autoCompleteExtender);
            }
        }

        public string[] GetSelectAlias()
        {
            List<String> aliasList = new List<String>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNodeList nodeList = xmlDoc.FirstChild.FirstChild.ChildNodes;
            foreach (XmlNode n in nodeList)
            {
                aliasList.Add(n.Name);
            }

            return aliasList.ToArray();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            if (!string.IsNullOrEmpty(this.DataKeyField) && !string.IsNullOrEmpty(this.CommandTable) && eventArgument.Length >= this.AutoInsertLength)
            {
                string sql = "select " + this.DataKeyField + " from " + this.CommandTable;
                DataTable table = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sql, true, CliUtils.fCurrentProject).Tables[0];
                if (table.Columns.Contains(this.DataKeyField))
                {
                    DataRow[] rows = null;
                    Type type = table.Columns[this.DataKeyField].DataType;
                    if (GloFix.IsNumeric(type))
                    {
                        rows = table.Select(this.DataKeyField + "=" + eventArgument);
                    }
                    else
                    {
                        rows = table.Select(this.DataKeyField + "='" + eventArgument + "'");
                    }
                    if (rows == null || rows.Length == 0)
                    {
                        string value = "";
                        if (GloFix.IsNumeric(type))
                        {
                            value = eventArgument;
                        }
                        else
                        {
                            value = "'" + eventArgument + "'";
                        }
                        sql = "insert into " + this.CommandTable + " (" + this.DataKeyField + ") values (" + value + ")";
                        CliUtils.ExecuteSql("GLModule", "cmdDDUse", sql, true, CliUtils.fCurrentProject);
                    }
                }
            }
        }

        public string GetCallbackResult()
        {
            return "";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.AutoInsert)
            {
                ClientScriptManager csm = Page.ClientScript;
                string callbackScript =
                    "function ReceiveServerData(arg)" +
                    "{" +
                    "}";
                string blurScript = csm.GetCallbackEventReference(this, this.UniqueID + ".value", callbackScript, "", true);
                writer.AddAttribute("onblur", blurScript, true);
            }
            base.Render(writer);
            if (!this.DesignMode)
            {
                _autoCompleteExtender.RenderControl(writer);
            }
        }
    }
}
