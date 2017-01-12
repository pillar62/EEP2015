using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Web.UI.Design;
namespace JQClientTools
{
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public class JQAutoSeq : WebControl, IJQDataSourceProvider
    {
        public JQAutoSeq()
        {
            NumDig = 3;
            StartValue = 1;
            Step = 1;
        }
        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.BindingObjectID))
                {
                    var bindingObject = this.Parent.FindControl(this.BindingObjectID);
                    if (bindingObject is IJQDataSourceProvider)
                    {
                        return (bindingObject as IJQDataSourceProvider).RemoteName;
                    }
                }
                return string.Empty;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(this.BindingObjectID))
                {
                    var bindingObject = this.Parent.FindControl(this.BindingObjectID);
                    if (bindingObject is IJQDataSourceProvider)
                    {
                        return (bindingObject as IJQDataSourceProvider).DataMember;
                    }
                }
                return string.Empty;
            }
            set { }
        }

        #endregion
        /// <summary>
        /// 绑定控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string BindingObjectID { get; set; }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        /// <summary>
        /// 自動序號的位數
        /// </summary>
        [Category("Infolight")]
        public int NumDig { get; set; }
        /// <summary>
        /// 自動序號的起始數
        /// </summary>
        [Category("Infolight")]
        public int StartValue { get; set; }
        /// <summary>
        /// 每次間隔
        /// </summary>
        [Category("Infolight")]
        public int Step { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                values.Add(string.Format("numDig:'{0}'", NumDig));
                values.Add(string.Format("startValue:'{0}'", StartValue));
                values.Add(string.Format("step:'{0}'", Step));
                return string.Join(",", values);
            }
        }
        protected override void Render(HtmlTextWriter writer) { }
    }
}
