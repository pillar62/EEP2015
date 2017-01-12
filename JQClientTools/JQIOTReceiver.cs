using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace JQClientTools
{
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public class JQIOTReceiver : WebControl, IJQDataSourceProvider
    {
        protected override void Render(HtmlTextWriter writer)
        {

        }

        public JQIOTReceiver()
        {

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
                dataMember = value;
            }
        }
    }
}
