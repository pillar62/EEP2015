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

    public class JQDefaultColumn : JQCollectionItem, IJQDataSourceProvider, ICloneable
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

        /// <summary>
        /// 是否代入上一笔资料
        /// </summary>
        [Category("Infolight")]
        public bool CarryOn { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                //var valueBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(this.DefaultValue))
                //{
                //    valueBuilder.Append(DefaultValue.Replace("'", "\'"));
                //}
                //else
                //{
                //    if (RemoteMethod)
                //    {
                //        valueBuilder.AppendFormat("remote[{0}]",DefaultMethod);
                //    }
                //    else
                //    {
                //        valueBuilder.AppendFormat("client[{0}]", DefaultMethod);
                //    }
                //}
                //return valueBuilder.ToString().TrimEnd(',');
                var values = new List<string>();
                if (!string.IsNullOrEmpty(this.DefaultValue))
                {
                    if (this.DefaultValue.StartsWith("_"))
                    {
                        values.Add(string.Format("remote[{0}]", DefaultValue));
                    }
                    else
                    {
                        values.Add(DefaultValue.Replace("'", "\'"));
                    }
                }
                else if (!string.IsNullOrEmpty(DefaultMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add(string.Format("remote[{0}]", DefaultMethod));
                    }
                    else
                    {
                        values.Add(string.Format("client[{0}]", DefaultMethod));
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

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
