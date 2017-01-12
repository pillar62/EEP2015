using System;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class InfoSecurityEdit : ComponentDesigner
    {
        private frmInfoSecurityEdit editor = null;
        private SYS_LANGUAGE language;

        public InfoSecurityEdit()
        {
        }

        public override void DoDefaultAction()
        {
            InfoSecurity ifs = (InfoSecurity)this.Component;
            if (ifs.DBAlias == null || ifs.DBAlias == "" || ifs.DBAlias == "(none)")
            {
                //language = CliSysMegLag.GetClientLanguage();
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsDBAlias");
                MessageBox.Show(message);
            }
            else if (editor == null)
            {
                editor = new frmInfoSecurityEdit(this.Component as InfoSecurity, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                editor.ShowDialog();
            }
            //editor.Dispose();
            editor = null;
        }

    }
}
