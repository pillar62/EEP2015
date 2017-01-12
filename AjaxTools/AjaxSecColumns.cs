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

namespace AjaxTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public sealed class AjaxSecColumns : AjaxBaseControl, IAjaxDataSource
    {
        #region Fields
        private String _dataSourceId;
        private AjaxSecColumnCollection _columns;
        private bool _visible = true;
        private bool _readOnly = false;
        #endregion

        #region Properties
        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public String DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public AjaxSecColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                    _columns = new AjaxSecColumnCollection(this);
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
        public AjaxSecColumns()
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            Do();
            base.OnLoad(e);
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
                    if (ct.GetType().FullName == "AjaxTools.AjaxFormView")
                    {
                        SetToExtFormView(ct);
                    }
                    else if (ct.GetType().FullName == "AjaxTools.AjaxGridView")
                    {
                        SetToExtGridView(ct);
                    }
                }
            }
        }

        private void SetToExtFormView(object value)
        {
            if (value.GetType().FullName == "AjaxTools.AjaxFormView")
            {
                Type type = value.GetType();
                PropertyInfo info = type.GetProperty("Fields");
                IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
                foreach (AjaxSecColumnItem esc in this.Columns)
                {
                    for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                    {
                        String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
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

        private void SetToExtGridView(object value)
        {
            if (value.GetType().FullName == "AjaxTools.AjaxGridView")
            {
                Type type = value.GetType();
                PropertyInfo info = type.GetProperty("Columns");
                IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
                foreach (AjaxSecColumnItem esc in this.Columns)
                {
                    for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                    {
                        String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
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

    }

    public class AjaxSecColumnCollection : InfoOwnerCollection
    {
        public AjaxSecColumnCollection(object owner)
            : base(owner)
        {
        }

        public AjaxSecColumnCollection(object owner, Type itemType)
            : base(owner, typeof(AjaxSecColumnItem))
        {
        }

        public new AjaxSecColumnItem this[int index]
        {
            get
            {
                return (AjaxSecColumnItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is AjaxSecColumnItem)
                    {
                        ((AjaxSecColumnItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((AjaxSecColumnItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }


    public sealed class AjaxSecColumnItem : InfoOwnerCollectionItem
    {
        #region Fields
        private String _columnName;
        #endregion

        #region Properties
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
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
        #endregion

        [Browsable(false)]
        public override string Name
        {
            get
            {
                return "";
            }
            set
            {
                value = "";
            }
        }
    }
}