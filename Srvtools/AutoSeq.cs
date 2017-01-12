using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(AutoSeq), "Resources.AutoSeq.ico")]
    public class AutoSeq : InfoBaseComp
    {
        private InfoBindingSource m_bindingSource;
        private string m_fieldName;
        private int m_numDig = 1;
        private int m_startValue;
        private int m_step = 1;
        private bool m_reNumber;
        private InfoBindingSource m_masterBindingSource;
        private bool m_active;
        //private int CurrentOffset = 0;

        public AutoSeq(System.ComponentModel.IContainer container)
            : base()
        {
            container.Add(this);
            //StartValue請默認為1. NumDig則為2,Active=True.
            this.StartValue = 1;
            this.NumDig = 2;
            this.Active = true;
            _getFixed = string.Empty;
        }

        #region Property
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public InfoBindingSource BindingSource
        {
            get
            {
                return m_bindingSource;
            }
            set
            {
                if (value == null || value is InfoBindingSource)
                {
                    m_bindingSource = value;
                    if (m_bindingSource != null)
                    {
                        m_bindingSource.ListChanged += delegate(object sender, ListChangedEventArgs e)
                        {
                            if (Active && this.FieldName != null && this.FieldName != "")
                            {
                                if (e.ListChangedType == ListChangedType.ItemAdded)
                                {
                                    DataRowView rowView = m_bindingSource.List[e.NewIndex] as DataRowView;
                                    if (rowView != null)
                                    {
                                        DataTable table = rowView.Row.Table;
                                        DataColumn column = table.Columns[this.FieldName];
                                        if (column != null && rowView.Row[column].ToString()=="")
                                        {
                                            rowView.Row[column] = this.GetValue();
                                        }
                                    }
                                }
                            }
                        };
                    }
                }
            }
        }

        [Category("Infolight"),
        Description("The column which AutoSeq is applied to")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return m_fieldName;
            }
            set
            {
                m_fieldName = value;
            }
        }

        [Category("Infolight"),
        Description("The digit of coding by AutoSeq")]
        public int NumDig
        {
            get
            {
                return m_numDig;
            }
            set
            {
                if (value > 0)
                {
                    m_numDig = value;
                }
            }
        }

        [Category("Infolight"),
        Description("The first number of each regular coding by AutoSeq")]
        public int StartValue
        {
            get
            {
                return m_startValue;
            }
            set
            {
                m_startValue = value;
            }
        }

        [Category("Infolight"),
        Description("The increment between the coding by AutoSeq")]
        public int Step
        {
            get
            {
                return m_step;
            }
            set
            {
                m_step = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether the data of the detail table need to be recoding after the masterbindingsource has applied")]
        public bool ReNumber
        {
            get
            {
                return m_reNumber;
            }
            set
            {
                m_reNumber = value;
            }
        }

        [Category("Infolight"),
        Description("The InfoBindingSource of the master table while the control is bound to an InfoBindingSource of the detail table")]
        public InfoBindingSource MasterBindingSource
        {
            get
            {
                return m_masterBindingSource;
            }
            set
            {
                if (value == null || value is InfoBindingSource)
                {
                    m_masterBindingSource = value;
                }
            }
        }

        [Category("Infolight"),
        Description("Indicates whether AutoSeq is enabled or disabled")]
        public bool Active
        {
            get
            {
                return m_active;
            }
            set
            {
                m_active = value;
            }
        }

        private string _getFixed;
        [Category("Infolight"),
        Description("Pre-code of AutoSeq")]
        public String GetFixed
        {
            get { return _getFixed; }
            set { _getFixed = value; }
        }

        #endregion

        internal string FixString
        {
            get
            {
                return (this.OwnerComp == null) ? string.Empty : CliUtils.GetValue(this.GetFixed, this.OwnerComp).ToString();
            }
        }

        internal string GetValue()
        {
            int num = this.StartValue;

            string fixstring = FixString;
            if (this.BindingSource.List.Count > 1)
            {
                for (int i = 0; i < this.BindingSource.List.Count; i++)
                {
                    object value = ((DataRowView)this.BindingSource.List[i]).Row[this.FieldName];
                    if (value != null && value != DBNull.Value)
                    {
                        string strvalue = value.ToString();
                        if (fixstring.Length > 0)
                        {
                            if (strvalue.StartsWith(fixstring))
                            {
                                strvalue = strvalue.Length > fixstring.Length ? strvalue.Substring(fixstring.Length) : string.Empty;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (strvalue.Length > 0)
                        {
                            try
                            {
                                int intvalue = Convert.ToInt32(strvalue);
                                if (intvalue >= num)
                                {
                                    num = intvalue + Step;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            return GetValue(num, fixstring);
        }

        internal string GetValue(int num, string fixString)
        {
            StringBuilder format = new StringBuilder(fixString);
            format.Append("{0:");
            format.Append('0', this.NumDig);
            format.Append("}");
            return string.Format(format.ToString(), num);
        }

        private class FieldNameEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService EditorService = null;
                if (provider != null)
                {
                    EditorService =
                        provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                }

                if (EditorService != null)
                {
                    ListBox ColumnList = new ListBox();
                    ColumnList.SelectionMode = SelectionMode.One;
                    ColumnList.Items.Add("( None )");

                    AutoSeq autoSeq = context.Instance as AutoSeq;
                    if (autoSeq != null && autoSeq.BindingSource != null)
                    {
                        InfoBindingSource bindingSource = autoSeq.BindingSource;
                        DataView dataView = bindingSource.List as DataView;
                        if (dataView != null)
                        {
                            foreach (DataColumn column in dataView.Table.Columns)
                            {
                                ColumnList.Items.Add(column.ColumnName);
                            }
                        }
                        //add by Rax
                        else
                        {
                            int iRelationPos = -1;
                            DataSet ds = ((InfoDataSet)autoSeq.BindingSource.GetDataSource()).RealDataSet;
                            for (int i = 0; i < ds.Relations.Count; i++)
                            {
                                if (autoSeq.BindingSource.DataMember == ds.Relations[i].RelationName)
                                {
                                    iRelationPos = i;
                                    break;
                                }
                            }
                            if (iRelationPos != -1)
                            {
                                foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                                {
                                    ColumnList.Items.Add(column.ColumnName);
                                }
                            }
                        }
                        // end add
                    }

                    ColumnList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                    {
                        int index = ColumnList.SelectedIndex;
                        if (index != -1)
                        {
                            if (index == 0)
                            {
                                value = "";
                            }
                            else
                            {
                                value = ColumnList.Items[index].ToString();
                            }
                        }
                        EditorService.CloseDropDown();
                    };

                    EditorService.DropDownControl(ColumnList);
                }
                return value;
            }
        }
    }
}
