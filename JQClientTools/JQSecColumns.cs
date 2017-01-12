using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Collections;
using System.Web.UI.WebControls;

namespace JQClientTools
{
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public sealed class JQSecColumns : WebControl, IJQDataSourceProvider
    {
        #region Fields
        private JQCollection<JQSecColumnItem> _columns;
        private bool _visible = true;
        private bool _readOnly = false;
        #endregion

        #region Properties
        /// <summary>
        /// 绑定控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string BindingObjectID { get; set; }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQSecColumnItem> Columns
        {
            get
            {
                return _columns;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public new bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }
        #endregion

        #region Methods
        public JQSecColumns()
        {
            _columns = new JQCollection<JQSecColumnItem>(this);
        }

        protected override void Render(HtmlTextWriter writer)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            Do();
            base.OnLoad(e);

            //RenderJS();
        }

        public void Do()
        {
            ControlCollection collection = null;
            if (this.Page.Master == null)
                collection = this.Page.Form.Controls;
            else
                collection = this.Parent.Controls;
            foreach (Control ct in collection)
            {
                if (!string.IsNullOrEmpty(ct.ID))
                {
                    if (ct is JQDataForm)
                    {
                        if ((ct as JQDataForm).ID == this.BindingObjectID)
                            SetToExtFormView(ct);
                    }
                    else if (ct is JQDataGrid)
                    {
                        if ((ct as JQDataGrid).ID == this.BindingObjectID)
                            SetToExtGridView(ct);
                    }
                    if (ct.Controls.Count > 0)
                    {
                        InitControls(ct);
                    }
                }
            }
        }

        private void RenderJS()
        {
            StringBuilder renderScript = new StringBuilder();
            renderScript.Append("document.onready = function () {");
            renderScript.Append("\r\n");
            renderScript.Append("var columns = [];");
            foreach (JQSecColumnItem esc in this.Columns)
            {
                if (esc.ColumnType == SecColumnType.FieldName)
                {
                    renderScript.Append("columns.push({ field:'" + esc.ColumnName + "',visible:" + this.Visible.ToString().ToLower() + "});");
                }
            }
            renderScript.AppendFormat("var dg = $('#{0}');", this.BindingObjectID);
            renderScript.Append("if(dg != null){");
            renderScript.Append("var dgColumns = dg.datagrid('getColumnFields');");
            renderScript.Append("for (var i = 0; i < dgColumns.length; i++){");
            renderScript.Append("for (var j = 0; j < columns.length; j++){");
            renderScript.Append("if(dgColumns[i] == columns[j].field){");
            renderScript.Append("if(columns[j].visible){");
            renderScript.Append("dg.datagrid('showColumn', dgColumns[i]);");
            renderScript.Append("}");//endif columns[j]
            renderScript.Append("else{");
            renderScript.Append("dg.datagrid('hideColumn', dgColumns[i]);");
            renderScript.Append("}");//else
            renderScript.Append("}");//dgColumns[i]
            renderScript.Append("}");//endfor j
            renderScript.Append("}");//endfor i
            renderScript.Append("}");//endif dg
            renderScript.Append("}");//end document.onready
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), renderScript.ToString(), true);
        }

        private void InitControls(Control control)
        {
            foreach (Control ct in control.Controls)
            {
                if (!string.IsNullOrEmpty(ct.ID))
                {
                    if (ct is JQDataForm)
                    {
                        if ((ct as JQDataForm).ID == this.BindingObjectID)
                            SetToExtFormView(ct);
                    }
                    else if (ct is JQDataGrid)
                    {
                        if ((ct as JQDataGrid).ID == this.BindingObjectID)
                            SetToExtGridView(ct);
                    }
                    if (ct.Controls.Count > 0)
                    {
                        InitControls(ct);
                    }
                }
            }
        }

        private void SetToExtFormView(object value)
        {
            Type type = value.GetType();
            PropertyInfo info = type.GetProperty("Columns");
            IList iExtFormColumnCollection = info.GetValue(value, null) as IList;
            foreach (JQSecColumnItem esc in this.Columns)
            {
                if (esc.ColumnType == SecColumnType.ColumnName)
                {
                    for (int i = 0; i < iExtFormColumnCollection.Count; i++)
                    {
                        String strColumnName = iExtFormColumnCollection[i].GetType().GetProperty("FieldName").GetValue(iExtFormColumnCollection[i], null).ToString();
                        if (strColumnName == esc.ColumnName)
                        {
                            iExtFormColumnCollection[i].GetType().GetProperty("ReadOnly").SetValue(iExtFormColumnCollection[i], this.ReadOnly, null);
                            iExtFormColumnCollection[i].GetType().GetProperty("Visible").SetValue(iExtFormColumnCollection[i], this.Visible, null);
                            break;
                        }
                    }
                }
            }
        }

        private void SetToExtGridView(object value)
        {
            Type type = value.GetType();
            PropertyInfo info = type.GetProperty("Columns");
            IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
            foreach (JQSecColumnItem esc in this.Columns)
            {
                if (esc.ColumnType == SecColumnType.ColumnName)
                {
                    for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                    {
                        String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("FieldName").GetValue(iExtGridColumnCollection[i], null).ToString();
                        if (strColumnName == esc.ColumnName)
                        {
                            iExtGridColumnCollection[i].GetType().GetProperty("ReadOnly").SetValue(iExtGridColumnCollection[i], this.ReadOnly, null);
                            iExtGridColumnCollection[i].GetType().GetProperty("Visible").SetValue(iExtGridColumnCollection[i], this.Visible, null);
                            break;
                        }
                    }
                }
            }
        }
        #endregion


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

    public sealed class JQSecColumnItem : JQCollectionItem, IJQDataSourceProvider
    {
        #region Fields
        private String _columnName;
        private SecColumnType _ColumnType = SecColumnType.ColumnName;
        #endregion

        #region Properties
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public String ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
            }
        }

        [Category("Infolight"), Browsable(false)]
        public SecColumnType ColumnType
        {
            get
            {
                return _ColumnType;
            }
            set
            {
                _ColumnType = value;
            }
        }
        #endregion

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ColumnName))
            {
                return this.ColumnName;
            }
            else
            {
                return base.ToString();
            }
        }

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set
            {

            }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set
            {

            }
        }
    }

    public enum SecColumnType
    {
        ColumnName,
        FieldName
    }
}