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

namespace JQMobileTools
{
    [Designer(typeof(JQDefaultDesigner), typeof(IDesigner))]
    public class JQDefault : WebControl, IJQDataSourceProvider
    {
        public JQDefault()
        {
            columns = new JQCollection<JQDefaultColumn>(this);
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string BindingObjectID { get; set; }

        private JQCollection<JQDefaultColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQDefaultColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        protected override void Render(HtmlTextWriter writer) { }

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
    }

    public class JQDefaultColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQDefaultColumn()
        {
            RemoteMethod = true;
        }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Category("Infolight")]
        public string DefaultValue { get; set; }


        /// <summary>
        /// 默认方法
        /// </summary>
        [Category("Infolight")]
        public string DefaultMethod { get; set; }

        /// <summary>
        /// 是否后台方法
        /// </summary>
        [Category("Infolight")]
        public bool RemoteMethod { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();


                //{type:'remote', value:'test'}


                if (!string.IsNullOrEmpty(this.DefaultValue))
                {
                    if (this.DefaultValue.StartsWith("_"))
                    {
                        values.Add("type:'remote'");
                        values.Add(string.Format("value:['{0}']", DefaultValue));
                    }
                    else
                    {
                        values.Add("type:'constant'");
                        values.Add(string.Format("value:['{0}']", DefaultValue));
                    }
                }
                else if (!string.IsNullOrEmpty(DefaultMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add("type:'remote'");
                        values.Add(string.Format("value:['{0}']", DefaultMethod));
                    }
                    else
                    {
                        values.Add("type:'client'");
                        values.Add(string.Format("value:['{0}']", DefaultMethod));
                    }
                }
                return string.Join(",", values);
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                return this.FieldName;
            }
            else
            {
                return base.ToString();
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }
}
