using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Xml;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoComboBox), "Resources.InfoComboBox.ico")]
    public partial class InfoComboBox : ComboBox, ISupportInitialize, IUseSelectCommand,IWriteValue
	{
        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (this.SelectCommand != null && this.SelectCommand != "")
            {
                if (this.SelectTop != null && this.SelectTop != "")
                {
                    StringBuilder sb = new StringBuilder(this.SelectCommand);
                    if (this.SelectCommand.IndexOf(" top ", StringComparison.OrdinalIgnoreCase) != -1)//IgnoreCase
                    {
                        string oldValue = "";
                        string[] parts = this.SelectCommand.Split(' ');
                        int i = parts.Length;
                        for (int j = 0; j < i; j++)
                        {
                            if (string.Compare(parts[j], "top", true) == 0 && j != i - 1)//IgnoreCase
                            {
                                oldValue = parts[j + 1];
                                break;
                            }
                        }
                        if (oldValue != "")
                        {
                            int lenth = this.SelectCommand.IndexOf(oldValue) + oldValue.Length;
                            sb.Replace(oldValue, this.SelectTop, 0, lenth);
                        }
                    }
                    else
                    {
                        sb.Insert(this.SelectCommand.Trim().IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6, " top " + this.SelectTop);//IgnoreCase
                    }
                    this.SelectCommand = sb.ToString();
                }
                if (!this.DesignMode)
                {
                    InnerDs.PacketRecords = -1;
                    InnerDs.RemoteName = "GLModule.cmdRefValUse";
                    InnerDs.Execute(this.SelectCommand, true);

                    InnerBs.DataSource = InnerDs;
                    InnerBs.DataMember = "cmdRefValUse";
                    this.DataSource = InnerBs;
                }
            }
        }

        public InfoComboBox()
		{
			InitializeComponent();
            language = CliUtils.fClientLang;
            this.KeyDown += new KeyEventHandler(InfoComboBox_KeyDown);
        }

        private bool _EnterEnable;
        [Category("Infolight"),
        Description("Indicate whether user can leave the control by pressing key of enter")]
        public bool EnterEnable
        {
            get { return _EnterEnable; }
            set { _EnterEnable = value; }
        }

        private bool _DisplayMemberOnly = false; 
        [Category("Infolight"), DefaultValue(false)]
        public bool DisplayMemberOnly
        {
            get { return _DisplayMemberOnly; }
            set { _DisplayMemberOnly = value; }
        }

        void InfoComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.EnterEnable && e.KeyCode == Keys.Enter)
            {
                if (!e.Shift)
                {
                    SendKeys.Send("{Tab}");
                }
                else
                {
                    SendKeys.Send("+{Tab}");
                }
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            BindingSourceBeginEdit();
        }

        private void BindingSourceBeginEdit()
        {
            Binding TextBinding = this.DataBindings["Text"];
            Binding SelectedValueBinding = this.DataBindings["SelectedValue"];
            if (TextBinding != null)
            {
                InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    if (bindingSource.BeginEdit() == false)
                    {
                        TextBinding.ReadValue();
                    }
                }
            }
            if (SelectedValueBinding != null)
            {
                InfoBindingSource bindingSource = SelectedValueBinding.DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    if (bindingSource.BeginEdit() == false)
                    {
                        SelectedValueBinding.ReadValue();
                    }
                }
            }
        }

        private SYS_LANGUAGE language;
		private DataColumn column;
        private DataTable table;
        private string oldDisplayMember = "";

        public object GetDataSource()
        {
            object obj = new object();
            if (this.SelectCommand != null && this.SelectCommand != "")
            {
                obj = this.InnerDs;
            }
            else
            {
                obj = this.DataSource;
                while (!(obj is InfoDataSet))
                {
                    if (obj is InfoBindingSource)
                    {
                        obj = ((InfoBindingSource)obj).DataSource;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return obj;
        }

        public string GetDataMember()
        {
            string strDataMember = "";
            object obj = this.DataSource;
            if (obj is InfoDataSet && this.ValueMember.IndexOf('.') != -1)
            {
                strDataMember = this.ValueMember.Substring(0, this.ValueMember.IndexOf('.'));
            }
            else
            {
                int i = 0;
                while (strDataMember == "" && i < 5)
                {
                    if (obj is InfoBindingSource)
                    {
                        strDataMember = ((InfoBindingSource)obj).DataMember.ToString();
                        obj = ((InfoBindingSource)obj).DataSource;
                    }
                    i++;
                }
            }
            return strDataMember;
        }

        public string GetDataTable()
        {
            string strTableName = "";
            if (this.SelectCommand != null && this.SelectCommand != "")
            {
                strTableName = this.InnerDs.RemoteName;
                strTableName = strTableName.Substring(strTableName.IndexOf('.') + 1);
            }
            else
            {
                strTableName = this.GetDataMember();
                InfoDataSet infoDs = (InfoDataSet)this.GetDataSource();
                if (infoDs != null)
                {
                    if (infoDs.RealDataSet.Relations != null)
                    {
                        int i = infoDs.RealDataSet.Relations.Count;
                        for (int j = 0; j < i; j++)
                        {
                            if (infoDs.RealDataSet.Relations[j].RelationName == strTableName)
                            {
                                strTableName = infoDs.RealDataSet.Relations[j].ChildTable.TableName;
                                break;
                            }
                        }
                    }
                }
            }
            return strTableName;
        }

        private string ValueMemberBeforeDropDown;
        protected override void OnDropDown(EventArgs e)
        {
            if (this.Expression != null && this.Expression != "")
            {
                table = new DataTable();
                table.TableName = this.GetDataTable();
                column = new DataColumn();
                column.Expression = this.Expression;
                column.ColumnName = this.Expression;
                InfoDataSet ds = (InfoDataSet)this.GetDataSource();
                if (!ds.RealDataSet.Tables[table.TableName].Columns.Contains(column.ColumnName))
                {
                    ds.RealDataSet.Tables[table.TableName].Columns.Add(column);
                }
                else
                {
                    bExpColExist = true;
                }

                oldDisplayMember = this.DisplayMember;
                if (this.DataSource is InfoDataSet)
                    this.DisplayMember = table.TableName + "." + this.Expression;
                else if (this.DataSource is InfoBindingSource)
                    this.DisplayMember = this.Expression;
            }

            if (DisplayMemberOnly)
            {
                ValueMemberBeforeDropDown = this.ValueMember;
                if (DisplayMemberBeforeEnter != null && DisplayMemberBeforeEnter != String.Empty)
                    this.DisplayMember = DisplayMemberBeforeEnter;
                object selectedValue = new object();
                if (this.DataBindings["SelectedValue"] != null)
                {
                    selectedValue = this.SelectedValue;
                    this.SetFilter();
                    this.SelectedValue = selectedValue;
                    if (this.SelectedValue == null)
                    {
                        this.SelectedIndex = 0;
                    }
                }
            }

            base.OnDropDown(e);
        }

        bool bExpColExist = false;
        protected override void OnDropDownClosed(EventArgs e)
        {
            if (this.Expression != null && this.Expression != "")
            {
                object selectValue = base.SelectedValue;
                if (!bExpColExist)
                {
                    ((InfoDataSet)this.GetDataSource()).RealDataSet.Tables[table.TableName].Columns.Remove(column.ColumnName);
                }
                //2007/4/29修改，如果DisplayMember和Expression相同，無法觸發BeginEdit。
                if (this.DisplayMember == oldDisplayMember)
                {
                    this.DisplayMember = "";
                }
                this.DisplayMember = oldDisplayMember;
                if (base.SelectedValue != selectValue)
                {
                    if (selectValue != null)//can not set selectedvalue to be null
                    {
                        base.SelectedValue = selectValue;
                        BindingSourceBeginEdit();
                    }
                    else
                    {
                        SelectedIndex = -1;
                    }
                }
            }
            else if (DataSource == null)
            {
                BindingSourceBeginEdit();
            }

            if (DisplayMemberOnly)
            {
                DisplayMemberBeforeEnter = this.DisplayMember;

                object selectedValue = this.SelectedValue;
                if (this.Text == "")
                {
                    this.DisplayMember = ValueMemberBeforeDropDown;
                    this.SelectedIndex = -1;
                }
                else
                {
                    this.DisplayMember = ValueMemberBeforeDropDown;
                }

                if (this.DataBindings["SelectedValue"] != null)
                {
                    this.ClearFilter();
                    this.SelectedValue = selectedValue;
                    this.DataBindings["SelectedValue"].WriteValue();
                }
                else
                {
                    this.SelectedValue = selectedValue;
                }
            }

            base.OnDropDownClosed(e);
        }

        private string DisplayMemberBeforeEnter;
        protected override void OnEnter(EventArgs e)
        {
            if (!DesignMode)
            {
                object selectedValue = this.SelectedValue;

                DisplayMemberBeforeEnter = this.DisplayMember;
                this.DisplayMember = this.ValueMember;
                if (this.DataBindings["SelectedValue"] != null)
                {
                    this.SetFilter();
                    if (selectedValue == null) this.SelectedIndex = -1;
                    else this.SelectedValue = selectedValue;
                }
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (!DesignMode)
            {
                object selectedValue = this.Text.Clone();
                this.DisplayMember = DisplayMemberBeforeEnter;
                if (this.DataBindings["SelectedValue"] != null)
                {
                    this.ClearFilter();
                    this.SelectedValue = selectedValue;
                    this.DataBindings["SelectedValue"].WriteValue();
                }
            }
            //if (this.Text == "")
            //    this.SelectedIndex = -1;
        }

		private string expr;

        [Category("Infolight"),
        Description("Expression of display data")]
		public string Expression
		{
			get
			{
				return expr;
			}
			set
			{
				expr = value;
			}
		}

        [Category("Infolight"),
        Description("Value of the seleced item")]
        public new object SelectedValue
        {
            get
            {
                return base.SelectedValue;
            }
            set
            {
                if (null != value)
                {
                    base.SelectedValue = value;
                }
                if (this.DataBindings["SelectedValue"] != null)
                {
                    this.DataBindings["SelectedValue"].DataSourceUpdateMode = DataSourceUpdateMode.Never;
                }
            }
        }

        private string _Filter = "";
        [Category("Infolight"),
        Description("Specifies the filter to get data")]
        public string Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                _Filter = value;
            }
        }

        public void SetFilter()
        {
            if (this.DataSource is BindingSource)
            {
                ((BindingSource)this.DataSource).Filter = this.Filter;
            }
        }

        public void ClearFilter()
        {
            /*InfoDataSet dsAllData = (InfoDataSet)this.GetDataSource();
            if (this.SelectAlias != null && this.SelectCommand != null && this.SelectAlias != "" && this.SelectCommand != "")
            {
                dsAllData.Execute(this.SelectCommand, this.SelectAlias, true);
            }
            else
            {
                dsAllData.ClearWhere();
            }*/
            if (this.DataSource is BindingSource)
            {
                ((BindingSource)this.DataSource).Filter = "";
            }
        }

        internal InfoDataSet InnerDs = new InfoDataSet();
        internal InfoBindingSource InnerBs = new InfoBindingSource();

        private string _SelectAlias;
        [Category("Infolight"),
        Description("Specifies database")]
        [Editor(typeof(ComboConnectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SelectAlias
        {
            get
            {
                return _SelectAlias;
            }
            set
            {
                _SelectAlias = value;
            }
        }

        private string _SelectTop;
        [Category("Data")]
        public string SelectTop
        {
            get
            {
                return _SelectTop;
            }
            set
            {
                _SelectTop = value;
            }
        }

        private string _SelectCommand;
        [Category("Infolight"),
        Description("Specifies the SQL command to get data")]
        [Editor(typeof(SelectCommandEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SelectCommand
        {
            get
            {
                return _SelectCommand;
            }
            set
            {
                _SelectCommand = value;
                if (this.Site != null && _SelectAlias != null && _SelectAlias != "" && _SelectCommand != null && _SelectCommand != "")
                {
                    InnerDs.RemoteName = "GLModule.cmdRefValUse";
                    InnerDs.Execute(_SelectCommand, _SelectAlias, true);
                    InnerBs.DataSource = InnerDs;
                    InnerBs.DataMember = "cmdRefValUse";
                    this.DataSource = InnerBs;
                }
            }
        }

        #region IWriteValue Members

        public void WriteValue()
        {
            if (this.DataBindings["SelectedValue"] != null)
            {
                this.DataBindings["SelectedValue"].WriteValue();
            }
        }

        public void WriteValue(object value)
        {
            this.SelectedValue = value;
            WriteValue();
        }

        #endregion
    }

    public class ComboConnectionEditor : System.Drawing.Design.UITypeEditor
    {
        public ComboConnectionEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService EditorService = null;
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                ListBox ColumnList = new ListBox();
                ColumnList.SelectionMode = SelectionMode.One;
                InfoComboBox combo = context.Instance as InfoComboBox;
                if (combo != null)
                {
                    XmlDocument DBXML = new XmlDocument();
                    if (File.Exists(SystemFile.DBFile))
                    {
                        DBXML.Load(SystemFile.DBFile);
                        XmlNode aNode = DBXML.DocumentElement.FirstChild;

                        while (aNode != null)
                        {
                            if (string.Compare(aNode.Name, "DATABASE", true) == 0)//IgnoreCase
                            {
                                XmlNode bNode = aNode.FirstChild;
                                while (bNode != null)
                                {
                                    ColumnList.Items.Add(bNode.LocalName);
                                    bNode = bNode.NextSibling;
                                }
                            }
                            aNode = aNode.NextSibling;
                        }
                    }
                }
                ColumnList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = ColumnList.SelectedIndex;
                    if (index != -1)
                    {
                        value = ColumnList.Items[index].ToString();
                    }
                    EditorService.CloseDropDown();
                };
                EditorService.DropDownControl(ColumnList);
            }
            return value;
        }
    }
}