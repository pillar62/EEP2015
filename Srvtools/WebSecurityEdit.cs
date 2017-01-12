using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Srvtools
{
    class WebSecurityEdit : DataSourceDesigner
    {
        private frmWebSecurity Export = null;
        private SYS_LANGUAGE language;
        private DesignerActionListCollection _actionLists;

        public WebSecurityEdit()
        {
            DesignerVerb ExportVerb = new DesignerVerb("Export", new EventHandler(OnExport));
            this.Verbs.Add(ExportVerb);
        }

        public WebSecurityEdit(IContainer container)
        {

        }

        public void OnExport(object sender, EventArgs e)
        {
            WebSecurity ws = (WebSecurity)this.Component;
            if (ws.DBAlias == null || ws.DBAlias == "" || ws.DBAlias == "(none)")
            {
                //language = CliSysMegLag.GetClientLanguage();
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsDBAlias", true);
                MessageBox.Show(message);
            }
            else if (Export == null)
            {
                Export = new frmWebSecurity(this.Component as WebSecurity, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                Export.ShowDialog();
            }
            Export = null;
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new WebSecurityActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class WebSecurityActionList : DesignerActionList
    {
        private WebSecurity ws;
        private frmWebSecurity Export = null;
        private SYS_LANGUAGE language;

        //public WebClientQueryActionList()
        //{ 

        //}

        public WebSecurityActionList(IComponent component)
            : base(component)
        {
            ws = component as WebSecurity;
        }

        public void OnExport()
        {
            if (ws.DBAlias == null || ws.DBAlias == "" || ws.DBAlias == "(none)")
            {
                //language = CliSysMegLag.GetClientLanguage();
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsDBAlias", true);
                MessageBox.Show(message);
            }
            else if (Export == null)
            {
                Export = new frmWebSecurity(ws, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                Export.ShowDialog();
            }
            Export = null;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnExport", "Export", "UsePreview", true));
            return items;
        }
    }
}
