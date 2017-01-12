using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using System.Reflection;

namespace Srvtools
{
    [ToolboxItem(true)]
    public class InfoSecColumns : InfoBaseComp
    {
        public InfoSecColumns()
        {
            _Columns = new SecColumnsCollection(this, typeof(SecColumn));
        }

        public InfoSecColumns(IContainer container)
        {
            container.Add(this);
            _Columns = new SecColumnsCollection(this, typeof(SecColumn));
        }

        private List<Control> formControls = new List<Control>();
        private void GetFormCtrls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                formControls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetFormCtrls(ctrl.Controls);
                }
            }
        }

        private string _Name;
        [Browsable(false)]
        public string Name
        {
            get { return _Name; }
            set
            {
                if (this.DesignMode)
                {
                    value = this.Site.Name;
                }
                _Name = value;
            }
        }

        protected override void DoAfterSetOwner(IDataModule value)
        {
            Do();
        }

        private void Do()
        {
            if (this.OwnerComp != null && this.OwnerComp is InfoForm)
            {
                InfoForm form = (InfoForm)this.OwnerComp;
                formControls.Clear();
                this.GetFormCtrls(form.Controls);
                Control ctrl;
                foreach (Control ctrlTemp in formControls)
                {
                    ctrl = ctrlTemp;
                    if (ctrl is DataGridView && ((DataGridView)ctrl).DataSource == this.BindingSource)
                    {
                        DataGridView grid = (DataGridView)ctrl;
                        foreach (DataGridViewColumn dataColumn in grid.Columns)
                        {
                            foreach (SecColumn secColumn in this.Columns)
                            {
                                if (dataColumn.DataPropertyName == secColumn.ColumnName)
                                {
                                    if (!Visible)
                                    {
                                        dataColumn.Visible = this.Visible;
                                    }
                                    if (ReadOnly)
                                    {
                                        dataColumn.ReadOnly = this.ReadOnly;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ControlBindingsCollection ctrlBindings = ctrl.DataBindings;
                        foreach (Binding binding in ctrlBindings)
                        {
                            if (binding.DataSource == this.BindingSource)
                            {
                                foreach (SecColumn secColumn in this.Columns)
                                {
                                    if (binding.BindingMemberInfo.BindingField == secColumn.ColumnName)
                                    {
                                        if (ctrl.Parent != null && ctrl.Parent is InfoRefvalBox) ctrl = ctrl.Parent;
                                        if (!Visible)
                                        {
                                            ctrl.Visible = this.Visible;
                                        }
                                        if (ReadOnly)
                                        {
                                            Type type = ctrl.GetType();
                                            PropertyInfo propInfo = type.GetProperty("ReadOnly");
                                            if (propInfo != null)
                                            {
                                                propInfo.SetValue(ctrl, this.ReadOnly, null);
                                            }
                                            else
                                            {
                                                propInfo = type.GetProperty("Enabled");
                                                if (propInfo != null) propInfo.SetValue(ctrl, !this.ReadOnly, null);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (ctrlTemp is Label)
                        {
                            foreach (SecColumn secColumn in this.Columns)
                            {
                                if ((ctrlTemp as Label).Name == secColumn.ColumnLabelName)
                                {
                                    if (!Visible)
                                    {
                                        ctrl.Visible = this.Visible;
                                    }
                                    if (ReadOnly)
                                    {
                                        Type type = ctrl.GetType();
                                        PropertyInfo propInfo = type.GetProperty("ReadOnly");
                                        if (propInfo != null)
                                        {
                                            propInfo.SetValue(ctrl, this.ReadOnly, null);
                                        }
                                        else
                                        {
                                            propInfo = type.GetProperty("Enabled");
                                            if (propInfo != null) propInfo.SetValue(ctrl, !this.ReadOnly, null);
                                        }
                                        break;
                                    }
                                }


                            }
                        }

                    }
                }
            }
        }

        private InfoBindingSource _BindingSource = null;
        [Category("InfoLight")]
        public InfoBindingSource BindingSource
        {
            get
            {
                return _BindingSource;
            }
            set
            {
                _BindingSource = value;
            }
        }

        private bool _ReadOnly = false;
        [Category("InfoLight")]
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                if (!this.DesignMode)
                    Do();
            }
        }

        private bool _Visible = true;
        [Category("InfoLight")]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
                if (!this.DesignMode)
                    Do();
            }
        }

        private SecColumnsCollection _Columns;
        [Category("InfoLight"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SecColumnsCollection Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }
    }

    public class SecColumnsCollection : InfoOwnerCollection
    {
        public SecColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(SecColumn))
        {
        }

        public new SecColumn this[int index]
        {
            get
            {
                return (SecColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is SecColumn)
                    {
                        //原来的Collection设置为0
                        ((SecColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((SecColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class SecColumn : InfoOwnerCollectionItem, IGetValues
    {
        public SecColumn()
            : this("")
        {
        }

        public SecColumn(string columnName)
        {
            _ColumnName = columnName;
        }

        public override string Name
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public override string ToString()
        {
            return _ColumnName;
        }

        private string _ColumnName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        private string _ColumnLabelName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnLabelName
        {
            get
            {
                return _ColumnLabelName;
            }
            set
            {
                _ColumnLabelName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retItems = null;
            if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is InfoSecColumns)
                {
                    InfoSecColumns scs = (InfoSecColumns)this.Owner;
                    if (scs.BindingSource != null)
                    {
                        object obj = scs.BindingSource.GetDataSource();
                        if (obj != null && obj is InfoDataSet)
                        {
                            DataSet ds = ((InfoDataSet)obj).RealDataSet;
                            string dm = scs.BindingSource.DataMember;
                            DataTable table = null;
                            foreach (DataRelation relation in ds.Relations)
                            {
                                if (relation.RelationName == dm)
                                {
                                    table = relation.ChildTable;
                                    break;
                                }
                            }
                            if (table == null)
                            {
                                table = ds.Tables[dm];
                            }
                            List<string> arrLst = new List<string>();
                            if (table != null)
                            {
                                foreach (DataColumn column in table.Columns)
                                {
                                    arrLst.Add(column.ColumnName);
                                }
                                retItems = arrLst.ToArray();
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "columnlabelname", true) == 0)//IgnoreCase
            {
                if (this.Owner is InfoSecColumns)
                {
                    InfoSecColumns scs = (InfoSecColumns)this.Owner;
                    if (scs.BindingSource != null)
                    {
                        List<string> arrLst = new List<string>();
                        for (int i = 0; i < scs.Container.Components.Count; i++)
                        {
                            if (scs.Container.Components[i] is Label)
                                arrLst.Add((scs.Container.Components[i] as Label).Name);
                        }
                        retItems = arrLst.ToArray();
                    }
                }
            }
            return retItems;
        }
    }
}
