using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace Srvtools
{
    public class InfoCheckBox : CheckBox, ISupportInitialize
    {
        public InfoCheckBox()
            : base()
        {
            this.Leave += new EventHandler(InfoCheckBox_Leave);
        }



        private object checkBinding;
        [Bindable(true)]
        [Browsable(false)]
        public object CheckBinding
        {
            get { return checkBinding; }
            set { checkBinding = value; }
        }

        private Type bindingType;
        private Type BindingType
        {
            get
            {
                if (bindingType != null)
                {
                    return bindingType;
                }
                else
                {
                    if (this.DataBindings["CheckBinding"] != null)
                    {
                        InfoBindingSource ibs = this.DataBindings["CheckBinding"].DataSource as InfoBindingSource;
                        if (ibs != null)
                        {
                            if (ibs.List as DataView != null)
                            {
                                DataTable table = (ibs.List as DataView).Table;
                                if (table != null)
                                {
                                    string field = this.DataBindings["CheckBinding"].BindingMemberInfo.BindingField;
                                    if (table.Columns.Contains(field))
                                    {
                                        bindingType = table.Columns[field].DataType;
                                        return bindingType;
                                    }
                                }
                            }
                        }
                    }
                    return typeof(string);
                }
            }
        }

        private bool isBinding;

        #region ISupportInitialize Members

        public void BeginInit() { }

        public void EndInit()
        {
            if (this.DataBindings["CheckBinding"] != null)
            {
                this.DataBindings["CheckBinding"].DataSourceUpdateMode = DataSourceUpdateMode.Never;
                this.DataBindings["CheckBinding"].BindingComplete += delegate(object sender, BindingCompleteEventArgs e)
                {
                    isBinding = true;
                    if (this.CheckBinding == null || this.CheckBinding == DBNull.Value)
                    {
                        this.Checked = false;
                    }
                    else
                    {
                        Type checktype = this.BindingType;
                        if (checktype == typeof(string))
                        {
                            this.Checked = (string.Compare(this.CheckBinding.ToString(), 0, "y", 0, 1, true) == 0);
                        }
                        else if (checktype == typeof(bool))
                        {
                            this.Checked = (bool)this.CheckBinding;
                        }
                        else if (checktype == typeof(int) || checktype == typeof(decimal))
                        {
                            this.Checked = (string.Compare(this.CheckBinding.ToString(), 0, "1", 0, 1, true) == 0);
                        }
                        else
                        {
                            throw new Exception(string.Format("Type:{0} is not supported", checktype.Name));
                        }
                    }
                    isBinding = false;

                };
                this.CheckedChanged += delegate(object sender, EventArgs e)
                {
                    if (!isBinding)
                    {
                        InfoBindingSource ibs = this.DataBindings["CheckBinding"].DataSource as InfoBindingSource;
                        if (ibs != null)
                        {
                            if (ibs.BeginEdit() == false)
                            {
                                isBinding = true;
                                this.Checked = !this.Checked;
                                isBinding = false;
                                return;
                            }
                        }
                        Type checktype = this.BindingType;
                        if (checktype == typeof(string))
                        {
                            this.CheckBinding = this.Checked ? "Y" : "N";
                        }
                        else if (checktype == typeof(bool))
                        {
                            this.CheckBinding = this.Checked;
                        }
                        else if (checktype == typeof(int) || checktype == typeof(decimal))
                        {
                            this.CheckBinding = this.Checked ? 1 : 0;
                        }
                        else
                        {
                            throw new Exception(string.Format("Type:{0} is not supported", checktype.Name));
                        }
                        this.DataBindings["CheckBinding"].WriteValue();
                    }
                };
            }

            if (this.DataBindings["Checked"] != null)
            {
                this.DataBindings["Checked"].DataSourceUpdateMode = DataSourceUpdateMode.Never;
            }

        }
        #endregion

        void InfoCheckBox_Leave(object sender, EventArgs e)
        {
            if (this.DataBindings["Checked"] != null)
            {
                this.DataBindings["Checked"].WriteValue();
            }
        }
    }
}
