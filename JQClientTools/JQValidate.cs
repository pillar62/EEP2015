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

namespace JQClientTools
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

    public class JQValidateColumn : JQCollectionItem, IJQDataSourceProvider, ICloneable
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

        [Category("Infolight")]
        public ValidateMode ValidateType { get; set; }

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
                //var valueBuilder = new StringBuilder();
                //if (this.CheckNull)
                //{
                //    valueBuilder.Append("required:'required'");
                //    valueBuilder.Append(",");
                //}
                //if (!string.IsNullOrEmpty(CheckMethod))
                //{
                //    if (RemoteMethod)
                //    {
                //        valueBuilder.AppendFormat("validType:'remote[\\'{0}\\', \\'{1}\\']'", CheckMethod, ValidateMessage);
                //    }
                //    else
                //    {
                //        valueBuilder.AppendFormat("validType:'client[\\'{0}\\', \\'{1}\\']'", CheckMethod, ValidateMessage);
                //    }
                //}
                //return valueBuilder.ToString().TrimEnd(',');


                var values = new List<string>();
                if (this.CheckNull)
                {
                    values.Add("required:'required'");
                }
                if (!string.IsNullOrEmpty(RangeFrom) || !string.IsNullOrEmpty(RangeTo))
                {
                    if (string.IsNullOrEmpty(RangeTo))
                    {
                        values.Add(string.Format("validType:'greater[\\'{0}\\']'", RangeFrom));
                    }
                    else if (string.IsNullOrEmpty(RangeFrom))
                    {
                        values.Add(string.Format("validType:'less[\\'{0}\\']'", RangeTo));
                    }
                    else
                    {
                        values.Add(string.Format("validType:'range[\\'{0}\\', \\'{1}\\']'", RangeFrom, RangeTo));
                    }
                }
                if (!string.IsNullOrEmpty(CheckMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add(string.Format("validType:'remote[\\'{0}\\', \\'{1}\\']'", CheckMethod, ReplaceSpecialCharacters(ValidateMessage)));
                    }
                    else
                    {
                        values.Add(string.Format("validType:'client[\\'{0}\\', \\'{1}\\']'", CheckMethod, ReplaceSpecialCharacters(ValidateMessage)));
                    }
                }
                if (ValidateType == ValidateMode.IdCard)
                {
                    values.Add(string.Format("validType:'idCard[]'"));
                }
                else if (ValidateType == ValidateMode.URL)
                {
                    values.Add(string.Format("validType:'url[]'"));
                }
                else if (ValidateType == ValidateMode.EMail)
                {
                    values.Add(string.Format("validType:'email[]'"));
                }
                return string.Join(",", values);
            }
        }

        private String ReplaceSpecialCharacters(String oldStr)
        {
            String newStr = oldStr.Replace("\'", "\\\\\\\'");
            return newStr;
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

    public enum ValidateMode
    {
        None,
        IdCard, 
        URL, 
        EMail
    }
}
