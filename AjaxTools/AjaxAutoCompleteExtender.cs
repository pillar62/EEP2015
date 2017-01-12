// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Resources;
using System.Web.Script;
using AjaxControlToolkit;

#if VS90
[assembly: System.Web.UI.WebResource("AjaxTools.AjaxAutoCompleteBehaviorVS90.js", "text/javascript")]
#else
[assembly: System.Web.UI.WebResource("AjaxTools.AjaxAutoCompleteBehavior.js", "text/javascript")]
#endif

namespace AjaxTools
{
    [Designer(typeof(AjaxAutoCompleteExtenderDesigner))]
#if VS90
    [ClientScriptResource("AjaxTools.AjaxAutoCompleteBehavior", "AjaxTools.AjaxAutoCompleteBehaviorVS90.js")]
#else
    [ClientScriptResource("AjaxTools.AjaxAutoCompleteBehavior", "AjaxTools.AjaxAutoCompleteBehavior.js")]
#endif
    public class AjaxAutoCompleteExtender : AutoCompleteExtender
    {
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("commandTable")]
        public virtual string CommandTable
        {
            get { return GetPropertyValue("CommandTable", String.Empty); }
            set { SetPropertyValue("CommandTable", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("dataKeyField")]
        public virtual string DataKeyField
        {
            get { return GetPropertyValue("DataKeyField", String.Empty); }
            set { SetPropertyValue("DataKeyField", value); }
        }

        [DefaultValue(",;")]
        [ExtenderControlProperty]
        [ClientPropertyName("delimiterCharacters")]
        public virtual string DelimiterCharacters
        {
            get { return GetPropertyValue("DelimiterCharacters", String.Empty); }
            set { SetPropertyValue("DelimiterCharacters", value); }

        }

        [DefaultValue("")]
        [Browsable(false)]
        [ExtenderControlProperty]
        [ClientPropertyName("userId")]
        public virtual string UserID
        {
            get { return GetPropertyValue("UserID", String.Empty); }
            set { SetPropertyValue("UserID", value); }
        }

        [DefaultValue("")]
        [Browsable(false)]
        [ExtenderControlProperty]
        [ClientPropertyName("password")]
        public virtual string Password
        {
            get { return GetPropertyValue("Password", String.Empty); }
            set { SetPropertyValue("Password", value); }
        }

        [DefaultValue("")]
        [Browsable(false)]
        [ExtenderControlProperty]
        [ClientPropertyName("dataBase")]
        public virtual string DataBase
        {
            get { return GetPropertyValue("DataBase", String.Empty); }
            set { SetPropertyValue("DataBase", value); }
        }

        [DefaultValue("")]
        [Browsable(false)]
        [ExtenderControlProperty]
        [ClientPropertyName("solution")]
        public virtual string Solution
        {
            get { return GetPropertyValue("Solution", String.Empty); }
            set { SetPropertyValue("Solution", value); }
        }
    }
}