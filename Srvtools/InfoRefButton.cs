using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Srvtools;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Data;

namespace Srvtools
{
    public partial class InfoRefButton : System.Windows.Forms.Button
    {
        public InfoRefButton()
        {
            _refButtonMatchs = new RefButtonMatchs(this, typeof(RefButtonMatch));
            _columns = new RefColumnsCollection(this, typeof(RefColumns));
            _searchColumns = new RefButtonSearchColumnCollection(this, typeof(RefButtonSearchColumnCollection));
            InitializeComponent();
        }

        public InfoRefButton(IContainer container)
        {
            container.Add(this);
            _refButtonMatchs = new RefButtonMatchs(this, typeof(RefButtonMatch));
            _columns = new RefColumnsCollection(this, typeof(RefColumns));
            _searchColumns = new RefButtonSearchColumnCollection(this, typeof(RefButtonSearchColumnCollection));
            InitializeComponent();
        }

        private Panel _panel;
        [Category("Infolight")]
        public Panel panel
        {
            get { return _panel; }
            set { _panel = value; }
        }

        private InfoTranslate _infoTranslate;
        [Category("Infolight")]
        public InfoTranslate infoTranslate
        {
            get { return _infoTranslate; }
            set { _infoTranslate = value; }
        }

        private RefButtonMatchs _refButtonMatchs;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[Category("Infolight"),
        //Description("The columns which InfoTranslate is applied to")]
        [Category("Infolight")]
        public RefButtonMatchs refButtonMatchs
        {
            get
            {
                return _refButtonMatchs;
            }
            set
            {
                _refButtonMatchs = value;
            }
        }

        private bool _autoPanel = false;
        [Category("Infolight"), DefaultValue(false)]
        public bool autoPanel
        {
            get { return _autoPanel; }
            set { _autoPanel = value; }
        }

        private RefColumnsCollection _columns;
        [Category("Infolight"),
        Description("Specifies the columns to display on the form")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RefColumnsCollection Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        //add  MultiSelect by luciferling 20080730
        private bool _multiSelect = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool multiSelect
        {
            get { return _multiSelect; }
            set { _multiSelect = value; }
        }

        private int formWidth = 600;
        [Category("Infolight"), DefaultValue(600)]
        public int FormWidth
        {
            get { return formWidth; }
            set { formWidth = value; }
        }

        private int formHeight = 350;
        [Category("Infolight"), DefaultValue(350)]
        public int FormHeight
        {
            get { return formHeight; }
            set { formHeight = value; }
        }

        private RefButtonSearchColumnCollection _searchColumns;
        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RefButtonSearchColumnCollection SearchColumns
        {
            get { return _searchColumns; }
            set { _searchColumns = value; }
        }

        private int _windowTop = 0;
        [Category("Infolight"), DefaultValue(0)]
        public int WindowTop
        {
            get { return _windowTop; }
            set { _windowTop = value; }
        }

        private int _windowleft = 0;
        [Category("Infolight"), DefaultValue(0)]
        public int WindowLeft
        {
            get { return _windowleft; }
            set { _windowleft = value; }
        }

        private bool _removeTitleBar = false;
        [Category("Infolight"), DefaultValue(false)]
        public bool RemoveTitleBar
        {
            get { return _removeTitleBar; }
            set { _removeTitleBar = value; }
        }

        InfoRefPanel innerPanel;
        protected override void OnClick(EventArgs e)
        {
            foreach (Control c in this.FindForm().Controls)
            {
                if (c is Panel || c is SplitterPanel || c is SplitContainer)
                {
                    if (FindNavigator(c) == false)
                    {
                        return;
                    }
                }
            }
            //add multiSelect by luciferling 20080730
            //当multiSelect 为 true autoPanel为 false 时，使user 设计的infodatagridview 设定成整行选择，方便复选行
            if (multiSelect & !autoPanel)
            {
                ((InfoDataGridView)this.panel.Controls[0]).SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            //end add
            innerPanel = new InfoRefPanel(this);//add multiSelect by luciferling 20080730
            innerPanel.AfterOK += new EventHandler<AfterOKEventArgs>(irp_AfterOK);
            innerPanel.Width = FormWidth;
            innerPanel.Height = FormHeight;
            if (this.WindowTop != 0 || this.WindowLeft != 0)
            {
                innerPanel.StartPosition = FormStartPosition.Manual;
                innerPanel.Location = new System.Drawing.Point(this.WindowTop, this.WindowLeft);
            }
            innerPanel.ControlBox = !this.RemoveTitleBar;
            innerPanel.ShowDialog();
            base.OnClick(e);
        }

        private bool FindNavigator(Control ctrl)
        {
            bool canedit = true;
            foreach (Control c in ctrl.Controls)
            {
                if (c is Panel || c is SplitterPanel || c is SplitContainer)
                {
                     canedit &= FindNavigator(c);
                }
                else
                {
                    foreach (RefButtonMatch rbm in this.refButtonMatchs)
                    {
                        if (rbm.Name == c.Name)
                        {
                            if (c.DataBindings.Count > 0 && c.DataBindings[0].DataSource is InfoBindingSource)
                            {
                                if ((c.DataBindings[0].DataSource as InfoBindingSource).BeginEdit() == false)
                                {
                                    canedit = false;
                                }
                            }
                        }
                    }
                }
            }
            return canedit;
        }

        void irp_AfterOK(object sender, AfterOKEventArgs e)
        {
            String[] strValue = new string[(sender as InfoRefPanel).value.Count];
            String[] strDisplay = new string[(sender as InfoRefPanel).display.Count];
            for (int i = 0; i < strValue.Length; i++)
            {
                strValue[i] = (sender as InfoRefPanel).value[i].ToString();
                strDisplay[i] = (sender as InfoRefPanel).display[i].ToString();
            }
            int num = 0;
            for (int j = 0; j < this.refButtonMatchs.Count; j++)
                for(int i = 0; i < this.Parent.Controls.Count; i++)
                    if ((refButtonMatchs[j] as RefButtonMatch).matchColumnName == this.Parent.Controls[i].Name)
                        if (num < strValue.Length)
                        {
                            if (this.Parent.Controls[i] is InfoRefbuttonBox)
                            {
                                (this.Parent.Controls[i] as InfoRefbuttonBox).Text = strDisplay[num];
                                (this.Parent.Controls[i] as InfoRefbuttonBox).RealValue = strValue[num];
                                (this.Parent.Controls[i] as InfoRefbuttonBox).Enter += new EventHandler(InfoRefButton_Enter);
                                (this.Parent.Controls[i] as InfoRefbuttonBox).Leave += new EventHandler(InfoRefButton_Leave);
                            }
                            else if (this.Parent.Controls[i] is InfoRefvalBox)
                            {
                                (this.Parent.Controls[i] as InfoRefvalBox).TextBoxSelectedValue = strValue[num];
                                if (this.Parent.Controls[i].GetType().GetInterface("IWriteValue", true) != null)
                                {
                                    ((IWriteValue)this.Parent.Controls[i]).WriteValue();
                                }
                            }
                            else
                            {
                                this.Parent.Controls[i].Text = strValue[num];
                                if (this.Parent.Controls[i].GetType().GetInterface("IWriteValue", true) != null)
                                {
                                    ((IWriteValue)this.Parent.Controls[i]).WriteValue();
                                }
                            }
                            num++;
                            continue;
                        }
        }

        void InfoRefButton_Enter(object sender, EventArgs e)
        {
            (sender as InfoRefbuttonBox).Text = innerPanel.value[0].ToString();
        }

        void InfoRefButton_Leave(object sender, EventArgs e)
        {
            (sender as InfoRefbuttonBox).Text = innerPanel.display[0].ToString();
        }
    }

    public class RefButtonMatchs : InfoOwnerCollection
    {
        public RefButtonMatchs(Object aOwner, Type aItemType)
            : base(aOwner, typeof(RefButtonMatch))
        {

        }
        public new RefButtonMatch this[int index]
        {
            get
            {
                return (RefButtonMatch)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is RefButtonMatch)
                    {
                        //原来的Collection设置为0
                        ((RefButtonMatch)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((RefButtonMatch)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class RefButtonMatch : InfoOwnerCollectionItem, IGetValues
    {
        public RefButtonMatch()
        {
            _matchColumnName = "";
        }

        private string _name;
        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _matchColumnName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string matchColumnName
        {
            get
            {
                return _matchColumnName;
            }
            set
            {
                _matchColumnName = value;
                if (value != null && value != "")
                {
                    _name = value;
                }
            }
        }

        public string[] GetValues(string sKind)
        {
            List<string> values = new List<string>();
            if (this.Owner is InfoRefButton)
            {
                if (string.Compare(sKind, "matchcolumnname", true) == 0)//IgnoreCase
                {
                    for (int i = 0; i < (this.Owner as InfoRefButton).Parent.Controls.Count; i++)
                    {
                        if ((this.Owner as InfoRefButton).Parent.Controls[i] is InfoTextBox || (this.Owner as InfoRefButton).Parent.Controls[i] is TextBox
                            || (this.Owner as InfoRefButton).Parent.Controls[i] is DateTimePicker || (this.Owner as InfoRefButton).Parent.Controls[i] is InfoDateTimePicker
                            || (this.Owner as InfoRefButton).Parent.Controls[i] is InfoMaskedTextBox || (this.Owner as InfoRefButton).Parent.Controls[i] is InfoRefvalBox)
                        {
                            values.Add((this.Owner as InfoRefButton).Parent.Controls[i].Name);
                        }
                    }
                }
            }
            else if (this.Owner is InfoDataGridViewRefButtonColumn)
            {
                if (string.Compare(sKind, "matchcolumnname", true) == 0)
                {
                    DataGridViewColumnCollection columns = (this.Owner as DataGridViewColumn).DataGridView.Columns;
                    foreach (DataGridViewColumn column in columns)
                    {
                        if (column != this.Owner)
                        {
                            values.Add(column.Name);
                        }
                    }
                }
            }
            return values.ToArray();
        }
    }

    public class RefButtonSearchColumnCollection : InfoOwnerCollection
    {
        public RefButtonSearchColumnCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(RefButtonSearchColumn))
        {

        }
        public new RefButtonSearchColumn this[int index]
        {
            get
            {
                return (RefButtonSearchColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is RefButtonSearchColumn)
                    {
                        //原来的Collection设置为0
                        ((RefButtonSearchColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((RefButtonSearchColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }

        public DataSet DsForDD = new DataSet();
    }

    public class RefButtonSearchColumn : InfoOwnerCollectionItem, IGetValues
    {
        public RefButtonSearchColumn()
        {
            _columnName = "";
        }

        private string _name;
        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _columnName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
                if (value != null && value != "")
                {
                    _name = value;
                }

                if (this.Owner != null && ((Component)this.Owner).Site.DesignMode)
                {
                    string header = GetHeaderText(_columnName);
                    if (header != "")
                    {
                        ColumnHeader = header;
                    }
                    else
                    {
                        ColumnHeader = _columnName;
                    }
                }
            }
        }

        private string _columnHeader;
        [NotifyParentProperty(true)]
        public string ColumnHeader
        {
            get
            {
                return _columnHeader;
            }
            set
            {
                _columnHeader = value;
            }
        }

        private String _type = "%";
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true), DefaultValue("%")]
        public String Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        private string GetHeaderText(string ColName)
        {
            DataSet ds = ((RefButtonSearchColumnCollection)this.Collection).DsForDD;
            if (ds.Tables.Count == 0)
            {
                if (this.Owner is InfoRefVal)
                {
                    if (((InfoRefVal)this.Owner).DataSource is InfoBindingSource)
                    {
                        ((RefButtonSearchColumnCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoRefVal)this.Owner).DataSource as InfoBindingSource, true);
                    }
                    else if (((InfoRefVal)this.Owner).DataSource is InfoDataSet)
                    {
                        ((RefButtonSearchColumnCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoRefVal)this.Owner).DataSource as InfoDataSet, null, true);
                    }
                }
                else if (this.Owner is InfoRefButton)
                {
                    if (((InfoRefButton)this.Owner).infoTranslate != null && ((InfoRefButton)this.Owner).infoTranslate.BindingSource != null)
                    {
                        ((RefButtonSearchColumnCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoRefButton)this.Owner).infoTranslate.BindingSource, true);
                    }
                }
                ds = ((RefButtonSearchColumnCollection)this.Collection).DsForDD;
            }
            string strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), ColName, true) == 0)//IgnoreCase
                    {
                        strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }


        public string[] GetValues(string sKind)
        {
            List<string> values = new List<string>();
            if (this.Owner is InfoRefButton)
            {
                if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
                {
                    if (((InfoRefButton)this.Owner).infoTranslate != null && ((InfoRefButton)this.Owner).infoTranslate.BindingSource != null)
                    {
                        string[] retItems = null;
                        InfoBindingSource bindingSource = ((InfoRefButton)this.Owner).infoTranslate.BindingSource;
                        InfoDataSet ds = bindingSource.GetDataSource();
                        string datamember = bindingSource.GetTableName();
                        retItems = new string[ds.RealDataSet.Tables[datamember].Columns.Count];
                        for (int i = 0; i < retItems.Length; i++)
                        {
                            retItems[i] = ds.RealDataSet.Tables[datamember].Columns[i].ColumnName;
                            values.Add(retItems[i]);
                        }
                    }
                }
                else if (string.Compare(sKind, "type", true) == 0)
                {
                    values.Add("%");
                    values.Add("%%");
                }
            }
            else if (this.Owner is InfoDataGridViewRefButtonColumn)
            {
                if (string.Compare(sKind, "matchcolumnname", true) == 0)
                {
                    DataGridViewColumnCollection columns = (this.Owner as DataGridViewColumn).DataGridView.Columns;
                    foreach (DataGridViewColumn column in columns)
                    {
                        if (column != this.Owner)
                        {
                            values.Add(column.Name);
                        }
                    }
                }
            }
            return values.ToArray();
        }
    }
}
