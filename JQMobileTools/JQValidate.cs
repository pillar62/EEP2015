using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace JQMobileTools
{
    [Designer(typeof(JQValidateDesigner), typeof(IDesigner))]
    public class JQValidate : WebControl, IJQDataSourceProvider
    {
        public JQValidate()
        {
            columns = new JQCollection<JQValidateColumn>(this);
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string BindingObjectID { get; set; }

        private JQCollection<JQValidateColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQValidateColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        [Category("Infolight")]
        public bool DuplicateCheck { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {

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
    }

    public class JQValidateColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQValidateColumn()
        {
            CheckNull = true;
            RemoteMethod = true;
        }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        /// <summary>
        /// 检查空白
        /// </summary>
        [Category("Infolight")]
        public bool CheckNull { get; set; }

        /// <summary>
        /// 检查的方法
        /// </summary>
        [Category("Infolight")]
        public string CheckMethod { get; set; }

        /// <summary>
        /// 显示的信息
        /// </summary>
        [Category("Infolight")]
        public string ValidateMessage { get; set; }

        [Category("Infolight")]
        public string RangeFrom { get; set; }
        
        [Category("Infolight")]
        public string RangeTo { get; set; }

        /// <summary>
        /// 是否后台方法
        /// </summary>
        [Category("Infolight")]
        public bool RemoteMethod { get; set; }

        [Category("Infolight"), Bindable(false)]
        public ValidateMode ValidateType { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                if (this.CheckNull)
                {
                    values.Add("required:true");
                }
                if (!string.IsNullOrEmpty(RangeFrom) || !string.IsNullOrEmpty(RangeTo))
                {
                    if (string.IsNullOrEmpty(RangeTo))
                    {
                        values.Add("type:'greater'");
                        values.Add(string.Format("value:['{0}']", RangeFrom));
                    }
                    else if (string.IsNullOrEmpty(RangeFrom))
                    {
                        values.Add("type:'less'");
                        values.Add(string.Format("value:['{0}']", RangeTo));
                    }
                    else
                    {
                        values.Add("type:'range'");
                        values.Add(string.Format("value:['{0}','{1}']",RangeFrom, RangeTo));
                    }
                }
                else if (!string.IsNullOrEmpty(CheckMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add("type:'remote'");
                        values.Add(string.Format("value:['{0}']", CheckMethod));
                        
                    }
                    else
                    {
                        values.Add("type:'client'");
                        values.Add(string.Format("value:['{0}']", CheckMethod));
                    }
                    values.Add(string.Format("message:'{0}'", ValidateMessage));
                }
                if (ValidateType == ValidateMode.IdCard)
                {
                    values.Add(string.Format("type:'idCard'"));
                }
                else if (ValidateType == ValidateMode.URL)
                {
                    values.Add(string.Format("type:'url'"));
                }
                else if (ValidateType == ValidateMode.EMail)
                {
                    values.Add(string.Format("type:'email'"));
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

    public enum ValidateMode
    {
        None,
        IdCard,
        URL,
        EMail
    }
}
