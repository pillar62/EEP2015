using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.IO;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoNavigator), "Resources.InfoNavigator.ico")]
    public class InfoNavigator : BindingNavigator, ISupportInitialize, IGetValues
    {
        // Add By Chenjian 2006-01-06
        #region Constant
        private const int WM_ShowWindow = 0x0018;
        #endregion Constant
        // End Add

        // Add By Chenjian 2006-01-07
        #region ISupportInitialalize interface
        void ISupportInitialize.BeginInit()
        {
            base.BeginInit();
        }

        public virtual void InitializeStates()
        {
            foreach (StateItem stateItem in this.States)
            {
                if (!stateItem.EnabledControlsEdited)
                {
                    if (stateItem.EnabledControls == null)
                    {
                        stateItem.EnabledControls = new List<string>();
                    }
                    switch (stateItem.StateText)
                    {
                        case "Initial":
                        case "Browsed":
                            //(1) initial : Disible: OK,Cancel,Apply,Abort,其餘Enable. 
                            //(2) Browsed : 同Initil
                            if (stateItem.EnabledControls != null)
                            {
                                stateItem.EnabledControls.Clear();
                            }
                            foreach (ToolStripItem item in this.Items)
                            {
                                if (!(item is ToolStripSeparator)
                                    && item.Name != null && item.Name.Trim() != "")
                                {
                                    if (this.OKItem != item
                                        && this.CancelItem != item
                                        && this.ApplyItem != item
                                        && this.AbortItem != item)
                                    {
                                        AddItemToList(stateItem.EnabledControls, item);
                                    }
                                }
                            }
                            break;
                        case "Inserting":
                        case "Editing":
                            //(3) Inserting: Enable: OK,Cancel,Apply,Abort,其餘Disible. 
                            //(4) Editing: 同inserting. 
                            if (stateItem.EnabledControls != null)
                            {
                                stateItem.EnabledControls.Clear();
                            }
                            AddItemToList(stateItem.EnabledControls, this.OKItem);
                            AddItemToList(stateItem.EnabledControls, this.CancelItem);
                            AddItemToList(stateItem.EnabledControls, this.ApplyItem);
                            AddItemToList(stateItem.EnabledControls, this.AbortItem);
                            break;
                        case "Changing":
                            //(6) Changing: Disible: OK,Cancel,Refresh,Query,Print, 其餘Enable. 
                            if (stateItem.EnabledControls != null)
                            {
                                stateItem.EnabledControls.Clear();
                            }
                            foreach (ToolStripItem item in this.Items)
                            {
                                if (!(item is ToolStripSeparator)
                                    && item.Name != null && item.Name.Trim() != "")
                                {
                                    if (this.OKItem != item
                                        && this.CancelItem != item
                                        && this.ViewRefreshItem != item
                                        && this.ViewQueryItem != item
                                        && this.PrintItem != item
                                        && this.ExportItem != item
                                        && this.CopyItem != item)
                                    {
                                        AddItemToList(stateItem.EnabledControls, item);
                                    }
                                }
                            }
                            break;
                        case "Querying":
                        case "Printing":
                        case "Applying":
                            //(5) Applying: 全部Disible. 
                            //(7) Querying: 全部Disible. 
                            //(8) Printing: 全部Disible.
                            if (stateItem.EnabledControls != null)
                            {
                                stateItem.EnabledControls.Clear();
                            }
                            break;
                    }
                }
            }
        }

        public void AddItemToList(List<string> list, ToolStripItem item)
        {
            if (item != null && list != null)
            {
                list.Add(item.Name);
            }
        }

        void ISupportInitialize.EndInit()
        {
            OnBeforeEndInit();
            base.EndInit();
            OnAfterEndInit();
        }

        protected virtual void OnBeforeEndInit()
        {
            if (this.GetServerText)
            {
                string message;
                message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "NavText");
                string[] texts = message.Split(';');
                if (ViewMoveFirstItem != null)
                    ViewMoveFirstItem.Text = texts[0];
                if (ViewMovePreviousItem != null)
                    ViewMovePreviousItem.Text = texts[1];
                if (ViewMoveNextItem != null)
                    ViewMoveNextItem.Text = texts[2];
                if (ViewMoveLastItem != null)
                    ViewMoveLastItem.Text = texts[3];
                if (AddNewItem != null)
                    AddNewItem.Text = texts[4];
                if (DeleteItem != null)
                    DeleteItem.Text = texts[5];
                if (EditItem != null)
                    EditItem.Text = texts[6];
                if (OKItem != null)
                    OKItem.Text = texts[7];
                if (CancelItem != null)
                    CancelItem.Text = texts[8];
                if (ApplyItem != null)
                    ApplyItem.Text = texts[9];
                if (AbortItem != null)
                    AbortItem.Text = texts[10];
                if (ViewRefreshItem != null)
                    ViewRefreshItem.Text = texts[11];
                if (ViewQueryItem != null)
                    ViewQueryItem.Text = texts[12];
                if (PrintItem != null)
                    PrintItem.Text = texts[13];
                if (ExportItem != null)
                    ExportItem.Text = texts[14];
                if (ViewCountItem != null)
                    ViewCountItem.Text = texts[15];
                ViewCountItemFormat = texts[15];

                message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "NavStates");
                string[] states = message.Split(';');
                foreach (StateItem item in m_states)
                {
                    switch (item.StateText)
                    {
                        case "Initial":
                            item.Description = states[0];
                            break;
                        case "Browsed":
                            item.Description = states[1];
                            break;
                        case "Inserting":
                            item.Description = states[2];
                            break;
                        case "Editing":
                            item.Description = states[3];
                            break;
                        case "Applying":
                            item.Description = states[4];
                            break;
                        case "Changing":
                            item.Description = states[5];
                            break;
                        case "Querying":
                            item.Description = states[6];
                            break;
                        case "Printing":
                            item.Description = states[7];
                            break;
                    }
                }
            }

            // 设置Items的初始状态
            InitializeStates();

            m_sureDeleteText = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "sureDeleteText");
            m_sureInsertText = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "sureInsertText");

            if (this.BindingSource != null)
            {
                this.BindingSource.AddingNew += delegate(object sender, AddingNewEventArgs e)
                {
                    // (4 : 1 of 1) 在Insert時, SetState(Inserting)
                    this.SetState("Inserting");
                    this.BindingSource.EnableFlag = true;
                };

                SetItemEnable(this.AddNewItem, BindingSource.AllowAdd);
                SetItemEnable(this.EditItem, BindingSource.AllowUpdate);
                SetItemEnable(this.DeleteItem, BindingSource.AllowDelete);
                SetItemEnable(this.PrintItem, BindingSource.AllowPrint);
                this.BindingSource.AllowProperyChanged += delegate(object sender, AllowProperytyEventArgs e)
                {
                    switch (e.Name)
                    {
                        case AllowProperytyEventArgs.PropertyName.Add: SetItemEnable(this.AddNewItem, BindingSource.AllowAdd); break;
                        case AllowProperytyEventArgs.PropertyName.Update: SetItemEnable(this.EditItem, BindingSource.AllowUpdate); break;
                        case AllowProperytyEventArgs.PropertyName.Delete: SetItemEnable(this.DeleteItem, BindingSource.AllowDelete); break;
                        case AllowProperytyEventArgs.PropertyName.Print: SetItemEnable(this.PrintItem, BindingSource.AllowPrint); break;
                    }
                };

                InfoBindingSource infoBindingSource = this.BindingSource as InfoBindingSource;
                if (infoBindingSource != null)
                {
                    infoBindingSource.EditBeginning += delegate(object sender, EventArgs e) // 2006/07/18 改成BeforeEdit
                    {
                        // (5 : 1 of 1) 在Update時: SetState(Editing)
                        if (this.CurrentStateItem != null && this.CurrentStateItem.StateText != "Editing"
                            //add by Rax, 解决新增状态下，会自动切换到编辑状态的问题
                            && this.CurrentStateItem.StateText != "Inserting")
                        // end add
                        {
                            this.SetState("Editing");
                        }
                    };

                    infoBindingSource.Changed += delegate(object sender, EventArgs e)
                    {
                        // Add By Chenjian 2006-01-09
                        // (7 : 1 of 2) 类似于: 在OK時: SetState(Changing)
                        //modified by lily 2007/4/26 for autoapply=true 不存在chaging狀態
                        if (!infoBindingSource.AutoApply)
                        {
                            this.SetState("Changing");
                        }
                        // End Add
                    };
                }

                InfoBindingSource bindingSource = this.BindingSource;
                string dataMember = this.BindingSource.DataMember;
                while (bindingSource.DataSource is BindingSource)
                {
                    if (bindingSource.DataMember != null && bindingSource.DataMember.Trim() != "")
                    {
                        dataMember = bindingSource.DataMember;
                    }
                    bindingSource = bindingSource.DataSource as InfoBindingSource;
                }

                if (dataMember != null && dataMember != "")
                {
                    InfoDataSet dataSet = bindingSource.DataSource as InfoDataSet;
                    if (dataSet != null)
                    {
                        //dataSet.DataFilled += delegate(object sender, EventArgs e)
                        //{
                        //    // (2: 1 of 1) 在BindingSource FillData時: SetState(Browsed)
                        //    this.SetState("Browsed");
                        //};

                        DataTable table = dataSet.RealDataSet.Tables[dataMember];
                        if (table == null)
                        {
                            DataRelation relation = dataSet.RealDataSet.Relations[dataMember];
                            if (relation != null)
                            {
                                table = relation.ChildTable;
                            }
                        }

                        if (table != null)
                        {
                            table.RowDeleting += delegate(object sender, DataRowChangeEventArgs e)
                            {
                                if (e.Action == DataRowAction.Delete)
                                {
                                    // (6 : 1 of 1) Delete後: SetState(Changing).
                                    //modified by lily 2007/4/26
                                    if (!bindingSource.AutoApply)
                                    {
                                        this.SetState("Changing");
                                    }
                                }
                            };
                        }
                    }
                }
            }
            // ViewBindingSource的countitem显示

            InfoBindingSource ibs = this.ViewBindingSource == null ? this.BindingSource : this.ViewBindingSource;
            if (ibs != null)
            {
                BindingSource_ListChanged(ibs, new ListChangedEventArgs(ListChangedType.Reset, 0));
                ibs.ListChanged += new ListChangedEventHandler(BindingSource_ListChanged);

            }

        }

        protected virtual void OnAfterEndInit()
        {
            if (this.ViewMoveFirstItem != null
                    && (this.BindingSource != null || this.ViewBindingSource != null))
            {
                this.ViewMoveFirstItem.Click += new EventHandler(ViewMoveFirstItem_Click);
            }
            if (this.ViewMovePreviousItem != null
                && (this.BindingSource != null || this.ViewBindingSource != null))
            {
                this.ViewMovePreviousItem.Click += new EventHandler(ViewMovePreviousItem_Click);
            }
            if (this.ViewMoveNextItem != null
                && (this.BindingSource != null || this.ViewBindingSource != null))
            {
                this.ViewMoveNextItem.Click += new EventHandler(ViewMoveNextItem_Click);
            }
            if (this.ViewMoveLastItem != null
                && (this.BindingSource != null || this.ViewBindingSource != null))
            {
                this.ViewMoveLastItem.Click += new EventHandler(ViewMoveLastItem_Click);
            }
            if (null != this.ViewQueryItem && InternalQuery)
            {
                _ViewQueryItem.Click += new EventHandler(OnViewQueryClilk);
            }
            if (null != this.AddNewItem)
            {
                this.AddNewItem.Click += new EventHandler(OnAddClilk);
            }
            if (null != this.DeleteItem)
            {
                this.DeleteItem.Click += new EventHandler(OnDeleteClick);
            }
            if (null != this.EditItem)
            {
                this.EditItem.Click += new EventHandler(OnEditClilk);
            }
            if (null != this.OKItem)
            {
                this.OKItem.Click += new EventHandler(OnOKClick);
            }
            if (null != this.CancelItem)
            {
                this.CancelItem.Click += new EventHandler(OnCancelClick);
            }
            if (null != this.ApplyItem)
            {
                this.ApplyItem.Click += new EventHandler(OnApplyClilk);
            }
            if (null != this.AbortItem)
            {
                this.AbortItem.Click += new EventHandler(OnAbortClick);
            }
            if (null != this.PrintItem)
            {
                this.PrintItem.Click += new EventHandler(OnPrintClick);
            }
            if (null != this.ViewRefreshItem)
            {
                this.ViewRefreshItem.Click += new EventHandler(OnViewRefreshClilk);
            }
            if (null != this.ExportItem)
            {
                this.ExportItem.Click += new EventHandler(OnExportClick);
            }
            if (null != this.CopyItem)
            {
                this.CopyItem.Click += new EventHandler(OnCopyClick);
            }
        }

        private void SetItemEnable(ToolStripItem item, bool enable)
        {
            if (item != null)
            {
                if (enable)
                {
                    item.EnabledChanged -= new EventHandler(Item_EnabledChanged);
                }
                else
                {
                    item.EnabledChanged += new EventHandler(Item_EnabledChanged);
                }
                item.Enabled = enable;
            }
        }

        private void Item_EnabledChanged(object sender, EventArgs e)
        {
            (sender as ToolStripItem).Enabled = false;
        }

        #endregion ISupportInitialalize interface
        // End Add

        public InfoNavigator()
        {
            this.SureInsert = false;
            this.SureDelete = true;
            this.QuerySQLSend = true;
            this.InternalQuery = true;
            this._ViewScrollProtect = false;

            m_states = new StateCollection(this, typeof(StateItem));
            //m_sureDeleteText = "Are you sure to delete current record?";
            //m_sureInsertText = "Are you sure to insert record?";

            this.LayoutCompleted += new EventHandler(InfoNavigator_LayoutCompleted);
            _QueryFields = new QueryFieldCollection(this, typeof(QueryField));
            _QueryMode = QueryModeType.ClientQuery;
            _QueryFont = new Font("SimSun", 9.0f);
        }

        #region Propeties
        private bool _HideItemStates;
        [Category("Infolight")]
        public bool HideItemStates
        {
            get
            {
                return _HideItemStates;
            }
            set
            {
                _HideItemStates = value;
            }
        }

        private Font _QueryFont;
        [Category("Infolight"),
        Description("Font of form of query")]
        public Font QueryFont
        {
            get { return _QueryFont; }
            set { _QueryFont = value; }
        }

        private bool _InternalQuery;
        [Category("Infolight")]
        public bool InternalQuery
        {
            get
            {
                return _InternalQuery;
            }
            set
            {
                _InternalQuery = value;
            }
        }



        private ToolStripItem _ViewMoveFirstItem;
        [Category("Infolight"),
        Description("The button used to move to first record of data")]
        public ToolStripItem ViewMoveFirstItem
        {
            get
            {
                return _ViewMoveFirstItem;
            }
            set
            {
                _ViewMoveFirstItem = value;
            }
        }

        private void ViewMoveFirstItem_Click(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("First");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoViewMoveFirst();
                OnAfterItemClick(new AfterItemClickEventArgs("First"));
            }

        }

        private void DoViewMoveFirst()
        {
            this.Focus();
            if (this.ViewBindingSource != null)
            {
                this.ViewBindingSource.MoveFirst();
                MoveEnableControl(this.ViewBindingSource);
            }
            else if (this.BindingSource != null)
            {
                this.BindingSource.MoveFirst();
                MoveEnableControl(this.BindingSource);
            }
        }

        private ToolStripItem _ViewMovePreviousItem;
        [Category("Infolight"),
        Description("The button used to move to previous record of data")]
        public ToolStripItem ViewMovePreviousItem
        {
            get
            {
                return _ViewMovePreviousItem;
            }
            set
            {
                _ViewMovePreviousItem = value;
            }
        }

        private InfoBindingSource _DetailBindingSource;
        [Category("Infolight")]
        public InfoBindingSource DetailBindingSource
        {
            get
            {
                return _DetailBindingSource;
            }
            set
            {
                _DetailBindingSource = value;
            }
        }

        private String _MasterDetailField;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String MasterDetailField
        {
            get
            {
                return _MasterDetailField;
            }
            set
            {
                _MasterDetailField = value;
            }
        }

        private String _DetailKeyField;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String DetailKeyField
        {
            get
            {
                return _DetailKeyField;
            }
            set
            {
                _DetailKeyField = value;
            }
        }

        private void ViewMovePreviousItem_Click(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Previous");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoViewMovePrevious();
                OnAfterItemClick(new AfterItemClickEventArgs("Previous"));
            }
        }

        private void DoViewMovePrevious()
        {
            this.Focus();
            if (this.ViewBindingSource != null)
            {
                this.ViewBindingSource.MovePrevious();
                MoveEnableControl(this.ViewBindingSource);
            }
            else if (this.BindingSource != null)
            {
                this.BindingSource.MovePrevious();
                MoveEnableControl(this.BindingSource);
            }
        }

        private ToolStripItem _ViewMoveNextItem;
        [Category("Infolight"),
        Description("The button used to move to next record of data")]
        public ToolStripItem ViewMoveNextItem
        {
            get
            {
                return _ViewMoveNextItem;
            }
            set
            {
                _ViewMoveNextItem = value;
            }
        }
        private void ViewMoveNextItem_Click(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Next");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoViewMoveNext();
                OnAfterItemClick(new AfterItemClickEventArgs("Next"));
            }
        }

        private void DoViewMoveNext()
        {
            this.Focus();
            if (this.ViewBindingSource != null)
            {
                this.ViewBindingSource.MoveNext();
                MoveEnableControl(this.ViewBindingSource);
            }
            else if (this.BindingSource != null)
            {
                this.BindingSource.MoveNext();
                MoveEnableControl(this.BindingSource);
            }
        }

        private ToolStripItem _ViewMoveLastItem;
        [Category("Infolight"),
        Description("The button used to move to last record of data")]
        public ToolStripItem ViewMoveLastItem
        {
            get
            {
                return _ViewMoveLastItem;
            }
            set
            {
                _ViewMoveLastItem = value;
            }
        }
        private void ViewMoveLastItem_Click(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Last");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoViewMoveLast();
                OnAfterItemClick(new AfterItemClickEventArgs("Last"));
            }
        }

        private void DoViewMoveLast()
        {
            this.Focus();
            if (this.ViewBindingSource != null)
            {
                this.ViewBindingSource.MoveLast();
                MoveEnableControl(this.ViewBindingSource);
            }
            else if (this.BindingSource != null && this.ViewBindingSource == null)
            {
                this.BindingSource.MoveLast();
                MoveEnableControl(this.BindingSource);
            }
        }

        private bool _GetRealRecordsCount;
        [Category("Infolight"),
        Description("Specifies whether to display the real count of data")]
        public bool GetRealRecordsCount
        {
            get { return _GetRealRecordsCount; }
            set { _GetRealRecordsCount = value; }
        }


        private ToolStripItem _ViewCountItem;
        [Category("Infolight"),
        Description("The label used to display the amount of data in table")]
        public ToolStripItem ViewCountItem
        {
            get
            {
                return _ViewCountItem;
            }
            set
            {
                _ViewCountItem = value;
            }
        }

        private string _ViewCountItemFormat;
        [Category("Infolight"),
        Description("Format of ViewCountItem")]
        public string ViewCountItemFormat
        {
            get
            {
                return _ViewCountItemFormat;
            }
            set
            {
                if (_ViewCountItemFormat != value)
                {
                    _ViewCountItemFormat = value;
                }
            }
        }

        private String _AnyQueryID = String.Empty;
        [Category("Infolight")]
        public String AnyQueryID
        {
            get
            {
                return _AnyQueryID;
            }
            set
            {
                _AnyQueryID = value;
            }
        }

        private AnyQueryColumnMode _QueryColumnMode = AnyQueryColumnMode.ByBindingSource;
        [Category("Infolight"), DefaultValue(AnyQueryColumnMode.ByBindingSource)]
        public AnyQueryColumnMode QueryColumnMode
        {
            get
            {
                return _QueryColumnMode;
            }
            set
            {
                _QueryColumnMode = value;
            }
        }

        private Margins _margin = new Margins(100, 30, 10, 30);
        [Category("Infolight"),
        Description("Specifies space between the controls ClientQuery created and the border of the form or panel")]
        public Margins QueryMargin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
            }
        }


        private bool _AutoDisableColumns = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool AutoDisableColumns
        {
            get
            {
                return _AutoDisableColumns;
            }
            set
            {
                _AutoDisableColumns = value;
            }
        }

        private int _MaxColumnCount = 10;
        [Category("Infolight"), DefaultValue(10)]
        public int MaxColumnCount
        {
            get
            {
                return _MaxColumnCount;
            }
            set
            {
                _MaxColumnCount = value;
            }
        }

        private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.ViewCountItem != null)
            {
                InfoBindingSource ibs = sender as InfoBindingSource;
                if (DesignMode)
                {
                    this.ViewCountItem.Text = this.ViewCountItemFormat;
                }
                else
                {
                    if (GetRealRecordsCount)
                    {

                        InfoDataSet ids = ibs.GetDataSource() as InfoDataSet;
                        string[] module = ids.RemoteName.Split('.');
                        int count = CliUtils.GetRecordsCount(module[0], module[1], ids.WhereStr, CliUtils.fCurrentProject);
                        this.ViewCountItem.Text = string.Format(this.ViewCountItemFormat, count);

                    }
                    else
                    {
                        this.ViewCountItem.Text = string.Format(this.ViewCountItemFormat, ibs.Count);
                    }
                }
            }
        }

        private ToolStripItem _ViewPositionItem;
        [Category("Infolight"),
        Description("The label used to display the positon of data in table")]
        public ToolStripItem ViewPositionItem
        {
            get
            {
                return _ViewPositionItem;
            }
            set
            {
                _ViewPositionItem = value;
                if (_ViewPositionItem != null)
                {
                    if (this.ViewBindingSource != null)
                    {
                        this.ViewBindingSource.PositionChanged += new EventHandler(BindingSource_PositionChanged);
                    }
                    else if (this.BindingSource != null)
                    {
                        this.BindingSource.PositionChanged += new EventHandler(BindingSource_PositionChanged);
                    }
                    ((ToolStripControlHost)_ViewPositionItem).Leave += new EventHandler(host_Leave);
                }
            }
        }


        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            InfoBindingSource bindingSource = sender as InfoBindingSource;
            if (this.ViewPositionItem != null)
            {
                if (bindingSource.Current != null)
                {
                    DataRowView rowView = bindingSource.Current as DataRowView;
                    if (rowView.IsNew)
                    {
                        return;
                    }
                }
                int i = bindingSource.Position + 1;
                this.ViewPositionItem.Text = i.ToString();
                this.MoveEnableControl(bindingSource);
            }
        }

        private void host_Leave(object sender, EventArgs e)
        {
            if (this.ViewBindingSource != null)
            {
                this.ViewBindingSource.Position = Convert.ToInt32(this.ViewPositionItem.Text) - 1;
            }
            else if (this.BindingSource != null)
            {
                this.BindingSource.Position = Convert.ToInt32(this.ViewPositionItem.Text) - 1;
            }
        }

        private ToolStripItem _ViewQueryItem = null;
        [Category("Infolight"),
        Description("The button used to view query")]
        public ToolStripItem ViewQueryItem
        {
            get
            {
                return _ViewQueryItem;
            }
            set
            {
                _ViewQueryItem = value;
            }
        }

        private ToolStripItem _AddNewItem = null;
        [Category("Infolight"),
        Description("The button used to add")]
        public new ToolStripItem AddNewItem
        {
            get
            {
                return _AddNewItem;
            }
            set
            {
                _AddNewItem = value;
            }
        }


        private ToolStripItem _DeleteItem = null;
        [Category("Infolight"),
        Description("The button used to delete")]
        public new ToolStripItem DeleteItem
        {
            get
            {
                return _DeleteItem;
            }
            set
            {
                _DeleteItem = value;
            }
        }

        private ToolStripItem _EditItem = null;
        [Category("Infolight"),
        Description("The button used to edit")]
        public ToolStripItem EditItem
        {
            get
            {
                return _EditItem;
            }
            set
            {
                _EditItem = value;
            }
        }

        private ToolStripItem _OKItem = null;
        [Category("Infolight"),
        Description("The button used to ok")]
        public ToolStripItem OKItem
        {
            get
            {
                return _OKItem;
            }
            set
            {
                _OKItem = value;
            }
        }

        private ToolStripItem _CancelItem = null;
        [Category("Infolight"),
        Description("The button used to cancel")]
        public ToolStripItem CancelItem
        {
            get
            {
                return _CancelItem;
            }
            set
            {
                _CancelItem = value;
            }
        }

        private ToolStripItem _ApplyItem = null;
        [Category("Infolight"),
        Description("The button used to apply")]
        public ToolStripItem ApplyItem
        {
            get
            {
                return _ApplyItem;
            }
            set
            {
                _ApplyItem = value;
            }
        }

        private ToolStripItem _AbortItem = null;
        [Category("Infolight"),
        Description("The button used to abort")]
        public ToolStripItem AbortItem
        {
            get
            {
                return _AbortItem;
            }
            set
            {
                _AbortItem = value;
            }
        }

        private ToolStripItem _ExportItem = null;
        [Category("Infolight"),
        Description("The button used to export")]
        public ToolStripItem ExportItem
        {
            get
            {
                return _ExportItem;
            }
            set
            {
                _ExportItem = value;
            }
        }

        private ToolStripItem copyItem;

        public ToolStripItem CopyItem
        {
            get { return copyItem; }
            set { copyItem = value; }
        }

        private ToolStripItemDisplayStyle _DisplayStyle;
        [Category("Infolight"),
        Description("Style of display")]
        [DefaultValue(ToolStripItemDisplayStyle.None)]
        public ToolStripItemDisplayStyle DisplayStyle
        {
            get
            {
                return _DisplayStyle;
            }
            set
            {
                _DisplayStyle = value;
                foreach (ToolStripItem item in this.Items)
                {
                    item.DisplayStyle = _DisplayStyle;
                }
            }
        }

        private TextImageRelation _TextImageRelation;
        [Category("Infolight"),
        Description("The mode of display of image and text")]
        [DefaultValue(TextImageRelation.ImageBeforeText)]
        public TextImageRelation TextImageRelation
        {
            get
            {
                return _TextImageRelation;
            }
            set
            {
                _TextImageRelation = value;
                foreach (ToolStripItem item in this.Items)
                {
                    item.TextImageRelation = _TextImageRelation;
                }
            }
        }

        private Color _ForeColors;
        [Category("Infolight"),
        Description("The color of the text on InfoNavigator")]
        public Color ForeColors
        {
            get
            {
                return _ForeColors;
            }
            set
            {
                _ForeColors = value;
                foreach (ToolStripItem item in this.Items)
                {
                    item.ForeColor = _ForeColors;
                }
            }
        }

        public enum QueryModeType
        {
            Normal,
            ClientQuery,
            AnyQuery
        }

        private QueryModeType _QueryMode;
        [Category("Infolight"),
        Description("The mode of query after the query button clicked")]
        public QueryModeType QueryMode
        {
            get { return _QueryMode; }
            set { _QueryMode = value; }
        }

        private bool _DisplayAllOperator;
        [Category("Infolight"), DefaultValue(false)]
        public bool DisplayAllOperator
        {
            get
            {
                return _DisplayAllOperator;
            }
            set
            {
                _DisplayAllOperator = value;
            }
        }

        private QueryFieldCollection _QueryFields;
        [Category("Infolight"),
        Description("Specifies the colounms to excute query")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QueryFieldCollection QueryFields
        {
            get
            {
                return _QueryFields;
            }
            set
            {
                _QueryFields = value;
            }
        }

        private ToolStripItem _PrintItem = null;
        [Category("Infolight"),
        Description("The button used to print")]
        public ToolStripItem PrintItem
        {
            get
            {
                return _PrintItem;
            }
            set
            {
                _PrintItem = value;
            }
        }

        private bool _SureInsert;
        [Category("Infolight"),
        Description("Indicate whether user need to confirm to insert data")]
        public bool SureInsert
        {
            get
            {
                return _SureInsert;
            }
            set
            {
                _SureInsert = value;
            }
        }

        private bool _SureDelete;
        [Category("Infolight"),
        Description("Indicate whether user need to confirm to delete data")]
        public bool SureDelete
        {
            get
            {
                return _SureDelete;
            }
            set
            {
                _SureDelete = value;
            }
        }

        private bool _SureAbort;
        [Category("Infolight"),
        Description("Indicate whether user need to confirm to abort")]
        public bool SureAbort
        {
            get { return _SureAbort; }
            set { _SureAbort = value; }
        }


        private InfoBindingSource _ViewBindingSource;
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to for view")]
        public InfoBindingSource ViewBindingSource
        {
            get
            {
                return _ViewBindingSource;
            }
            set
            {
                _ViewBindingSource = value;
            }
        }

        private ToolStripItem _ViewRefreshItem = null;
        [Category("Infolight"),
        Description("The button used to refresh")]
        public ToolStripItem ViewRefreshItem
        {
            get
            {
                return _ViewRefreshItem;
            }
            set
            {
                _ViewRefreshItem = value;
            }
        }

        // Add By Chenjian 2006-01-05
        private StateCollection m_states = null;
        [Category("Infolight"),
        Description("Specifies the states of InfoNavigator and enable status of associated buttons")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StateCollection States
        {
            get
            {
                return m_states;
            }
            set
            {
                m_states = value;
            }
        }

        private InfoBindingSource _BindingSource;
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        new public InfoBindingSource BindingSource
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

        private ToolStripLabel m_descriptionItem = null;
        [Category("Infolight"),
        Description("The label used to display description")]
        public ToolStripLabel DescriptionItem
        {
            get
            {
                return m_descriptionItem;
            }
            set
            {
                m_descriptionItem = value;
                if (m_descriptionItem != null && !this.Items.Contains(m_descriptionItem))
                {
                    this.Items.Add(m_descriptionItem);
                }
            }
        }

        private string m_sureInsertText;
        [Category("Infolight"),
        Description("The message of SureInsert")]
        [Browsable(false)]
        public string SureInsertText
        {
            get
            {
                return this.m_sureInsertText;
            }
            set
            {
                this.m_sureInsertText = value;
            }
        }

        private string m_sureDeleteText;
        [Category("Infolight"),
        Description("The message of SureDelete")]
        [Browsable(false)]
        public string SureDeleteText
        {
            get
            {
                return this.m_sureDeleteText;
            }
            set
            {
                this.m_sureDeleteText = value;
            }
        }

        private bool _GetServerText;
        [Category("Infolight"),
         Description("Indicates whether the caption of InfoNavigator's items use server settings automatically")]
        public bool GetServerText
        {
            get
            {
                return _GetServerText;
            }
            set
            {
                _GetServerText = value;
                if (_GetServerText)
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "NavStates");
                    string[] states = message.Split(';');
                    foreach (StateItem item in m_states)
                    {
                        switch (item.StateText)
                        {
                            case "Initial":
                                item.Description = states[0];
                                break;
                            case "Browsed":
                                item.Description = states[1];
                                break;
                            case "Inserting":
                                item.Description = states[2];
                                break;
                            case "Editing":
                                item.Description = states[3];
                                break;
                            case "Applying":
                                item.Description = states[4];
                                break;
                            case "Changing":
                                item.Description = states[5];
                                break;
                            case "Querying":
                                item.Description = states[6];
                                break;
                            case "Printing":
                                item.Description = states[7];
                                break;
                        }
                    }
                }
            }
        }

        private InfoStatusStrip _StatusStrip;
        [Category("Infolight"),
        Description("StatusStrip to display the status of InfoNavigator")]
        public InfoStatusStrip StatusStrip
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        private bool _QuerySQLSend;
        [Category("Infolight"),
        Description("Indicates whether Sql string is used to get data automatically")]
        public bool QuerySQLSend
        {
            get
            {
                return _QuerySQLSend;
            }
            set
            {
                _QuerySQLSend = value;
            }
        }

        private bool _QueryKeepCondition;
        [Category("Infolight"),
        Description("Indicates whether the query text will be cleared after excute query")]
        public bool QueryKeepCondition
        {
            get
            {
                return _QueryKeepCondition;
            }
            set
            {
                _QueryKeepCondition = value;
            }
        }

        private bool _MultiLanguage;
        [Category("Infolight")]
        public bool MultiLanguage
        {
            get
            {
                return _MultiLanguage;
            }
            set
            {
                _MultiLanguage = value;
            }
        }


        // End Add
        #endregion

        #region Events
        private void InfoNavigator_LayoutCompleted(object sender, EventArgs e)
        {
            InitializeStates();
        }

        private void OnAddClilk(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Add");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoAdd();
                OnAfterItemClick(new AfterItemClickEventArgs("Add"));
            }
        }

        private void DoAdd()
        {
            this.BindingSource.keyFlag2 = true;
            if (this.SureInsert)
            {
                if (MessageBox.Show(this.SureInsertText, "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.BindingSource.AddNew();
                }
            }
            else
            {
                this.BindingSource.AddNew();
            }
          

            if (this.BindingSource != null)
            {
                if (((InfoBindingSource)this.BindingSource).AllowAdd == false)
                    ((InfoBindingSource)this.BindingSource).EnableFlag = false;
                else
                    ((InfoBindingSource)this.BindingSource).EnableFlag = true;
            }
            this.BindingSource.keyFlag2 = false;
        }

        private void OnCopyClick(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Copy");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoCopy();
                OnAfterItemClick(new AfterItemClickEventArgs("Copy"));
            }
        }

        private void DoCopy()
        {
            if (this.BindingSource != null && this.BindingSource.Current != null)
            {
                frmNavigatorCopy form = new frmNavigatorCopy(this.BindingSource);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    object[] values = form.Values;
                    DataRow dr = (this.BindingSource.Current as DataRowView).Row;
                    object newrowview = this.BindingSource.AddNew();
                    if (newrowview != null)
                    {
                        DataRow newdr = (newrowview as DataRowView).Row;
                        newdr.ItemArray = dr.ItemArray;
                        for (int i = 0; i < values.Length; i++)
                        {
                            newdr[newdr.Table.PrimaryKey[i]] = values[i];
                        }
                        this.BindingSource.EndEdit();
                        CopyRow(dr, newdr);
                    }
                }
            }
        }

        private void CopyRow(DataRow dr, DataRow newdr)
        {
            for (int i = 0; i < dr.Table.ChildRelations.Count; i++)
            {
                DataRow[] drchildren = dr.GetChildRows(dr.Table.ChildRelations[i]);
                object[] newvalues = new object[dr.Table.ChildRelations[i].ParentColumns.Length];
                for (int j = 0; j < newvalues.Length; j++)
                {
                    newvalues[j] = newdr[dr.Table.ChildRelations[i].ParentColumns[j]];
                }
                DataColumn[] columns = dr.Table.ChildRelations[i].ChildColumns;
                for (int j = 0; j < drchildren.Length; j++)
                {
                    DataRow newdrchild = CopyRow(drchildren[j], columns, newvalues);
                    CopyRow(drchildren[j], newdrchild);
                }
            }
        }

        private DataRow CopyRow(DataRow dr, DataColumn[] columns, object[] newvalues)
        {
            if (columns.Length == newvalues.Length)
            {
                DataRow newrow = dr.Table.NewRow();
                newrow.ItemArray = dr.ItemArray;
                for (int i = 0; i < columns.Length; i++)
                {
                    newrow[columns[i]] = newvalues[i];
                }
                dr.Table.Rows.Add(newrow);
                return newrow;
            }
            return null;
        }

        private void OnExportClick(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Export");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoExport();
                OnAfterItemClick(new AfterItemClickEventArgs("Export"));
            }
        }

        private object EventBeforeExport = new object();

        public event BeforeExportEventHandler BeforeExport
        {
            add { Events.AddHandler(EventBeforeExport, value); }
            remove { Events.RemoveHandler(EventBeforeExport, value); }
        }

        protected void OnBeforeExport(ExportArgs e)
        {
            BeforeExportEventHandler handler = Events[EventBeforeExport] as BeforeExportEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void DoExport()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file(*.xls)|*.xls";
            sfd.Title = "Export to File";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                InfoDataSet ids = new InfoDataSet();
                if (this.ViewBindingSource != null)
                {
                    ids = this.ViewBindingSource.GetDataSource() as InfoDataSet;
                }
                else
                {
                    ids = this.BindingSource.GetDataSource() as InfoDataSet;
                }
                DataSetToExcel dste = new DataSetToExcel();
                dste.Excelpath = sfd.FileName;
                dste.DataSet = ids;
                dste.BeforeExport = new BeforeExportEventHandler(BeforeExportXLS);
                dste.Export();
                // toexcel;
            }
        }

        private void BeforeExportXLS(object sender, ExportArgs e)
        {
            OnBeforeExport(e);
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Delete");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoDelete();
                OnAfterItemClick(new AfterItemClickEventArgs("Delete"));
            }
        }

        private void DoDelete()
        {
            if (this.BindingSource.Current != null)
            {
                //bool lastPos = false; 从未被使用过，只是赋了值，所以先注释掉了
                if (this.ViewBindingSource != null)
                {
                    if (this.ViewBindingSource.Position == this.ViewBindingSource.List.Count - 1 && this.ViewBindingSource.List.Count != 1)
                    {
                        //lastPos = true;从未被使用过，只是赋了值，所以先注释掉了
                    }
                }
                else
                {
                    if (this.BindingSource.Position == this.BindingSource.List.Count - 1 && this.BindingSource.List.Count != 1)
                    {
                        //lastPos = true;从未被使用过，只是赋了值，所以先注释掉了
                    }
                }

                if (this.SureDelete)
                {
                    if (!this.BindingSource.AllowDelete)
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoBindingSource", "rightToDelete");
                        MessageBox.Show(message);
                        return;
                    }
                    else if (MessageBox.Show(this.SureDeleteText, "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (this.BindingSource.AutoRecordLock)
                        {
                            if (!this.BindingSource.AddLock("Deleting"))
                            {
                                return;
                            }
                        }
                        this.BindingSource.RemoveCurrent();
                    }
                }
                else
                {
                    if (this.BindingSource.AutoRecordLock)
                    {
                        if (!this.BindingSource.AddLock("Deleting"))
                        {
                            return;
                        }
                    }
                    this.BindingSource.RemoveCurrent();
                }

                if (this.ViewBindingSource != null)
                {
                    if (this.ViewBindingSource.List.Count > 0)
                    {
                        //if (!lastPos)
                        //{

                        //    this.ViewBindingSource.Position += 1;
                        //    this.ViewBindingSource.Position -= 1;
                        //    this.ViewBindingSource.DoDelay();
                        //}
                        //else
                        //{
                        //    this.ViewBindingSource.Position -= 1;
                        //    this.ViewBindingSource.Position += 1;
                        //}
                        this.ViewBindingSource.DoDelay();
                    }
                }
                else
                {
                    if (this.BindingSource.List.Count > 0)
                    {
                        this.BindingSource.InternalPositionChanged(this.BindingSource, new EventArgs());
                        //if (!lastPos)
                        //{
                        //    this.BindingSource.Position += 1;
                        //    this.BindingSource.Position -= 1;
                        //}
                        //else
                        //{
                        //    this.BindingSource.Position -= 1;
                        //    this.BindingSource.Position += 1;
                        //}
                    }
                }

                //2006/08/02
                if (this.BindingSource.AutoApply)
                {
                    this.SetState("Browsed");
                }
                //2006/08/02
            }
        }

        private void OnEditClilk(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs args = new BeforeItemClickEventArgs("Edit");
            OnBeforeItemClick(args);
            if (!args.Cancel)
            {
                DoEdit();
                OnAfterItemClick(new AfterItemClickEventArgs("Edit"));
            }

        }

        private void DoEdit()
        {
            if (!this.BindingSource.AllowUpdate)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoBindingSource", "rightToUpdate");
                MessageBox.Show(message);
                return;
            }
            if (this.BindingSource != null)
            {
                ((InfoBindingSource)this.BindingSource).EnableFlag = true;
                if (((InfoBindingSource)this.BindingSource).AutoRecordLock)
                {
                    if (!((InfoBindingSource)this.BindingSource).AddLock("Updating"))
                    {
                        ((InfoBindingSource)this.BindingSource).EnableFlag = false;
                        return;
                    }
                }
            }
            this.SetState("Editing");
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            this.Focus();
            if (!this.Focused)
            {
                //modified by ccm 2009/04/22 如果没有取得焦点说明有控件验证不过,取消保存
                return;
            }
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("OK");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoOK(sender);
                OnAfterItemClick(new AfterItemClickEventArgs("OK"));
            }
        }

        protected virtual void DoOK(object sender)
        {
            Type type = this.FindForm().GetType();
            FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                            | BindingFlags.NonPublic
                                            | BindingFlags.Public);
            for (int i = 0; i < fi.Length; i++)
            {
                if (fi[i].GetValue(this.FindForm()) is AutoSeq)
                {
                    AutoSeq aSeq = (AutoSeq)fi[i].GetValue(this.FindForm());
                    if (this.BindingSource == aSeq.MasterBindingSource && aSeq.ReNumber)
                    {
                        string fixstring = aSeq.FixString;
                        for (int n = 0; n < aSeq.BindingSource.List.Count; n++)
                        {
                            if (((DataRowView)aSeq.BindingSource.List[n]).Row.RowState != DataRowState.Deleted //2008.10.13 大陸Lily加入這二行的判斷, memo by Ruth Hu
                                    && ((DataRowView)aSeq.BindingSource.List[n]).Row.RowState != DataRowState.Detached)
                            {
                                ((DataRowView)aSeq.BindingSource.List[n]).Row[aSeq.FieldName] = aSeq.GetValue(aSeq.StartValue + n * aSeq.Step, fixstring);
                            }
                        }
                    }
                }
                if (fi[i].GetValue(this.FindForm()) is InfoDataGridView
                    && ((InfoDataGridView)fi[i].GetValue(this.FindForm())).GetDataSource() == this.BindingSource.GetDataSource())
                {
                    ((InfoDataGridView)fi[i].GetValue(this.FindForm())).EndEdit();
                    ((InfoDataGridView)fi[i].GetValue(this.FindForm())).Changed = false;
                }
            }
            for (int i = 0; i < fi.Length; i++)
            {
                if (fi[i].GetValue(this.FindForm()) is InfoBindingSource
                    && (InfoBindingSource)fi[i].GetValue(this.FindForm()) != this.BindingSource
                    && ((InfoBindingSource)fi[i].GetValue(this.FindForm())).GetDataSource() == this.BindingSource.GetDataSource())
                {
                    ((InfoBindingSource)fi[i].GetValue(this.FindForm())).EndEdit();
                }
            }
            //this.BindingSource.EndEdit();//在下面的this.BindingSource.InEndEdit(EndValidRowByRow)函数中会进调用,2007/01/24
            if (sender != null)
            {
                this.BindingSource.bChk = false;
                this.BindingSource.bDeplChk = false;
                this.BindingSource.CheckDeplicateSucess = true;
                bool b = this.BindingSource.InEndEdit(EndValidRowByRow);

                // andy modified 2007.04.04 move CheckSucess to here 
                bool oldCheckSucess = this.BindingSource.CheckSucess;
                this.BindingSource.CheckSucess = true;

                EndValidRowByRow = false;
                if (b && oldCheckSucess)
                {
                    if (this.BindingSource.AutoApply)
                    {
                        bool bApplySuccess = ((InfoDataSet)this.BindingSource.GetDataSource()).ApplyUpdates(false);
                        if (bApplySuccess)
                        {
                            if (((InfoBindingSource)this.BindingSource).AutoRecordLock)
                            {
                                ((InfoBindingSource)this.BindingSource).RemoveLock();
                            }
                            //added by lily 2007/5/22 如果先有資料編輯存檔，再有資料編輯取消，view的資料會將原來已經存檔的資料也取消
                            if (this.ViewBindingSource != null)
                            {
                                ((InfoDataSet)this.ViewBindingSource.GetDataSource()).RealDataSet.AcceptChanges();
                                this.ViewBindingSource.DoDelay();
                            }
                            ((InfoBindingSource)this.BindingSource).EnableFlag = false;
                            //added by lily 2007/5/22
                            this.SetState("Browsed");
                        }
                        else
                        {
                            //remarked by lily 2007/4/2 如果新增没有成功会改变状态，会导致ok键enable＝false。
                            //this.SetState("Changing");
                        }
                    }
                    //modified by lily 2007/4/2 如果新增没有成功会改变状态，会导致ok键enable＝false。
                    else
                        this.SetState("Changing");
                }
                if (sender == ApplyItem && BindingSource.AutoApply)  //未设autoapply时,第一次验证未过,再按存档无法清除这个flag
                    BindingSource.CheckSucess = oldCheckSucess;
            }
            else
            {
                this.BindingSource.EndEdit();
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            if (SureAbort && this.BindingSource.AutoApply)
            {
                string aborttext = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "sureAborText");
                if (MessageBox.Show(aborttext, "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Cancel");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoCancel();
                OnAfterItemClick(new AfterItemClickEventArgs("Cancel"));
            }
        }

        protected virtual void DoCancel()
        {
            bool isModified = false;
            BindingSource bindingSource = this.BindingSource;
            while (bindingSource.DataSource is BindingSource)
            {
                bindingSource = bindingSource.DataSource as BindingSource;
            }
            this.Focus();
            ((InfoBindingSource)this.BindingSource).CancelEdit();

            InfoDataSet dataSet = bindingSource.DataSource as InfoDataSet;
            if (dataSet != null)
            {
                isModified = dataSet.RealDataSet.HasChanges();
            }

            if (isModified == true)
            {
                //modified by lily 2007/4/26 for autoapply＝true時沒有changing狀態
                if (!((InfoBindingSource)this.BindingSource).AutoApply)
                {
                    this.SetState("Changing");
                }
            }
            else
            {
                this.SetState("Browsed");
            }

            if (this.ViewBindingSource == null)
            {
                ((InfoBindingSource)this.BindingSource).EnableFlag = false;
            }

            Type type = this.FindForm().GetType();
            FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                            | BindingFlags.NonPublic
                                            | BindingFlags.Public);
            for (int i = 0; i < fi.Length; i++)
            {
                if (fi[i].GetValue(this.FindForm()) is InfoBindingSource
                    && ((InfoBindingSource)fi[i].GetValue(this.FindForm())).GetDataSource() == this.BindingSource.GetDataSource())
                {
                    ((InfoBindingSource)fi[i].GetValue(this.FindForm())).CancelEdit();
                }
            }
            for (int i = 0; i < fi.Length; i++)
            {
                if (fi[i].GetValue(this.FindForm()) is InfoDataGridView
                    && ((InfoDataGridView)fi[i].GetValue(this.FindForm())).GetDataSource() == this.BindingSource.GetDataSource())
                {
                    ((InfoDataGridView)fi[i].GetValue(this.FindForm())).CancelEdit();
                    ((InfoDataGridView)fi[i].GetValue(this.FindForm())).Changed = false;
                }
            }
            
            if (this.BindingSource.AutoApply)
            {
                bCallFromCancel = true;
                DoAbort();
                bCallFromCancel = false;
            }
        }

        public bool EndValidRowByRow = false;
        private void OnApplyClilk(object sender, EventArgs e)
        {
            this.Focus();
            if (!this.Focused)
            {
                //modified by ccm 2009/04/22 如果没有取得焦点说明有控件验证不过,取消保存
                return;
            }
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Apply");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoApply();
                OnAfterItemClick(new AfterItemClickEventArgs("Apply"));
            }
        }

        protected virtual void DoApply()
        {
            if (this.BindingSource != null && this.BindingSource.AutoApply && this.OKItem != null)
            {
                DoOK(this);
                return;
            }
            //modified by lily 2007/10/29 如果 ok/cancel button 删除，detail的最後一筆無法存檔除非新增后移動到別筆
            if (CurrentStateItem != null
               && (CurrentStateItem.StateText == "Inserting" || CurrentStateItem.StateText == "Editing" || CurrentStateItem.StateText == "Changing"))
            //modified by lily 2007/10/29 如果 ok/cancel button 删除，detail的最後一筆無法存檔除非新增后移動到別筆
            {
                //EndValidRowByRow = true;
                try
                {
                    if (this.BindingSource.Current != null)//删除最后一笔后Current为null
                    {
                        (this.BindingSource.Current as DataRowView).BeginEdit();
                        (this.BindingSource.Current as DataRowView).EndEdit();
                    }
                }
                catch (NoNullAllowedException)
                {
                    DataSet dataset = DBUtils.GetDataDictionary(this.BindingSource, false);
                    DataRowView rowView = this.BindingSource.Current as DataRowView;
                    DataColumn[] keys = rowView.Row.Table.PrimaryKey;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (rowView.Row[keys[i]] == DBNull.Value)
                        {
                            string name = keys[i].ColumnName;
                            if (dataset.Tables.Count > 0)
                            {
                                DataRow[] dr = dataset.Tables[0].Select(string.Format("FIELD_NAME = '{0}'", keys[i].ColumnName));
                                if (dr.Length > 0)
                                {
                                    if (dr[0]["CAPTION"] != DBNull.Value && dr[0]["CAPTION"].ToString().Length > 0)
                                    {
                                        name = dr[0]["CAPTION"].ToString();
                                    }
                                }
                            }
                            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "msg_DefaultValidateCheckNull");
                            MessageBox.Show(string.Format(message, name));
                            break;
                        }
                    }
                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
                //DoOK(null);
                if (OKItem != null && OKItem.Enabled)
                {
                    EndValidRowByRow = false;
                    DoOK(ApplyItem);
                    if (!BindingSource.CheckSucess)
                        return;
                }
                else
                {
                    EndValidRowByRow = true;
                    DoOK(null);
                }

            }
            this.SetState("Applying");
            try
            {
                if (this.BindingSource != null)
                {
                    bool b = ((InfoDataSet)this.BindingSource.GetDataSource()).ApplyUpdates();
                    if (b)
                    {
                        ((InfoBindingSource)this.BindingSource).EnableFlag = false;
                        if (((InfoBindingSource)this.BindingSource).AutoRecordLock)
                        {
                            ((InfoBindingSource)this.BindingSource).RemoveLock();
                        }
                        //added by lily 2007/5/22 如果先有資料編輯存檔，再有資料編輯取消，view的資料會將原來已經存檔的資料也取消
                        if (this.ViewBindingSource != null)
                        {
                            ((InfoDataSet)this.ViewBindingSource.GetDataSource()).RealDataSet.AcceptChanges();
                        }
                        this.SetState("Browsed");
                        //added by lily 2005/5/22
                    }
                    else
                    {
                        //modified by lily 2007/4/27 for autoapply=true no changing
                        if (!this.BindingSource.AutoApply)
                        {
                            this.SetState("Changing");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                //modified by lily 2007/4/27 for autoapply=true no changing
                if (!this.BindingSource.AutoApply)
                {
                    this.SetState("Changing");
                }
                //remarked by lily 2007/7/18 for 錯誤訊息太長會導致整個StatusStip不可見，索性全部改用Message來Show錯誤信息。
                //if (this.StatusStrip != null)
                //{
                //    if (this.StatusStrip.itemlist[0] == null)
                //    {
                //        ToolStripStatusLabel tsslNavigator = new ToolStripStatusLabel(err.Message);
                //        this.StatusStrip.itemlist[0] = tsslNavigator;
                //        tsslNavigator.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                //                                | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                //                                | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
                //        tsslNavigator.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;

                //        this.StatusStrip.Items.Clear();
                //        for (int i = 0; i < 7; i++)
                //            if (this.StatusStrip.itemlist[i] != null)
                //                this.StatusStrip.Items.Add(this.StatusStrip.itemlist[i]);
                //    }
                //    else
                //    {
                //        this.StatusStrip.itemlist[0].Text = err.Message;
                //    }
                //}
                //else
                //{
                MessageBox.Show(err.Message);
                //}
            }
        }

        private bool bCallFromCancel = false;
        private void OnAbortClick(object sender, EventArgs e)
        {
            if (SureAbort)
            {
                string aborttext = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "sureAborText");
                if (MessageBox.Show(aborttext, "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Abort");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoAbort();
                OnAfterItemClick(new AfterItemClickEventArgs("Abort"));
            }
        }

        protected virtual void DoAbort()
        {
            if (this.BindingSource != null)
            {
                //if (this.BindingSource.AutoApply && this.CancelItem != null)
                //{
                //    this.CancelItem.PerformClick();
                //    return;
                //}

                // Add By Chenjian 2006-01-06
                // (10 : 1 of 1) 在Abort時: 如果在State為inserting/Editing時, 請先執行'CANCEL'的程式.
                //  SetState(Browsed).
                //modified by lily 2007/10/29 如果 ok/cancel button 删除，detail的最後一筆無法存檔除非新增后移動到別筆
                if (CurrentStateItem != null
                    && (CurrentStateItem.StateText == "Inserting" || CurrentStateItem.StateText == "Editing" || CurrentStateItem.StateText == "Changing")
                    && !bCallFromCancel)
                //modified by lily 2007/10/29 如果 ok/cancel button 删除，detail的最後一筆無法存檔除非新增后移動到別筆
                {
                    DoCancel();
                    bCallFromCancel = false;
                    if (this.BindingSource.AutoApply)
                    {
                        return;
                    }
                }
                // End Add

                this.Focus();

                
                //add by lily 2007/4/4 取消時，如果有view，view也要取消變化，并重新關聯master
                if (this.ViewBindingSource != null && this.ViewBindingSource is InfoBindingSource)
                {
                    ((InfoDataSet)this.ViewBindingSource.GetDataSource()).RealDataSet.RejectChanges();
                    this.BindingSource.CancelPositionChanged = true;  //只删除detail会导致存档
                    this.ViewBindingSource.DoDelay();
                    this.BindingSource.CancelPositionChanged = false;
                }

                if (this.BindingSource != null)
                {
                    ((InfoDataSet)this.BindingSource.DataSource).RealDataSet.RejectChanges();
                    if (this.BindingSource.AutoRecordLock)
                    {
                        ((InfoBindingSource)this.BindingSource).RemoveLock();
                    }
                    ((InfoBindingSource)this.BindingSource).EnableFlag = false;
                }
                this.SetState("Browsed");
            }
        }

        private void OnViewRefreshClilk(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Refresh");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoViewRefresh();
                OnAfterItemClick(new AfterItemClickEventArgs("Refresh"));
            }
        }

        private void DoViewRefresh()
        {
            if (this.ViewBindingSource != null)
            {
                this.Focus();
                if (this.ViewBindingSource is InfoBindingSource)
                {
                    ((InfoBindingSource)this.ViewBindingSource).Refresh();
                    this.ViewBindingSource.DoDelay();
                }
                else
                {
                    ((InfoDataSet)this.ViewBindingSource.DataSource).Active = false;
                    ((InfoDataSet)this.ViewBindingSource.DataSource).Active = true;
                }
            }
            else if (this.BindingSource != null && this.ViewBindingSource == null)
            {
                this.Focus();
                if (this.BindingSource is InfoBindingSource)
                {
                    ((InfoBindingSource)this.BindingSource).Refresh();
                }
                else
                {
                    ((InfoDataSet)this.BindingSource.DataSource).Active = false;
                    ((InfoDataSet)this.BindingSource.DataSource).Active = true;
                }
            }
        }

        private void OnViewQueryClilk(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Query");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoViewQuery();
                OnAfterItemClick(new AfterItemClickEventArgs("Query"));
            }
        }

        ClientQuery cqform = null;

        protected virtual void DoViewQuery()
        {
            // (3 : 1 of 2) 在Query時: SetState(Querying);
            this.SetState("Querying");
            //string strQueryFields = _QueryFields;
            InfoBindingSource bSource = new InfoBindingSource();
            if (this.ViewBindingSource != null)
            {
                bSource = this.ViewBindingSource;
            }
            else if (this.BindingSource != null && this.ViewBindingSource == null)
            {
                bSource = this.BindingSource;
            }
            this.Focus();

            if (this.QueryMode == QueryModeType.Normal)
            {
                Form frmNavQuery = new frmNavQuery(this.QueryFields, bSource, this);
                frmNavQuery.ShowDialog();
            }
            else if (this.QueryMode == QueryModeType.ClientQuery)
            {
                if (this.QueryFields.Count > 0)
                {
                    if (cqform == null)
                    {
                        cqform = CopyQueryFileds(1);
                    }
                    string strwhere = string.Empty;
                    try
                    {
                        strwhere = cqform.Execute(false);
                        NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strwhere);
                        OnQueryWhere(args);
                        if (!args.Cancel)
                        {
                            strwhere = args.WhereString;
                            if (strwhere != null)
                            {
                                if (this.StatusStrip != null && this.StatusStrip.ShowProgress)
                                {
                                    this.StatusStrip.ShowProgressBar();
                                }
                                ((InfoDataSet)bSource.GetDataSource()).SetWhere(strwhere);
                                if (this.StatusStrip != null && this.StatusStrip.ShowProgress)
                                {
                                    this.StatusStrip.HideProgressBar();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                else
                {
                    Form frmNavQuery = new frmNavQuery(this.QueryFields, bSource, this);
                    frmNavQuery.ShowDialog();
                }
            }
            else if (this.QueryMode == QueryModeType.AnyQuery)
            {
                if (aq == null)
                    aq = CopyAnyQueryFileds();
                String strwhere = aq.Execute(this, false);
                NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strwhere);
                OnQueryWhere(args);
                args.Cancel = !this.QuerySQLSend;

                if (!args.Cancel)
                {
                    strwhere = args.WhereString;
                    if (strwhere != null)
                    {
                        if (this.StatusStrip != null && this.StatusStrip.ShowProgress)
                        {
                            this.StatusStrip.ShowProgressBar();
                        }
                        ((InfoDataSet)bSource.GetDataSource()).SetWhere(strwhere);
                        if (this.StatusStrip != null && this.StatusStrip.ShowProgress)
                        {
                            this.StatusStrip.HideProgressBar();
                        }
                    }
                }
            }

            // (3 : 2 of 2) Query後: SetState(Browsed).
            this.SetState("Browsed");
        }

        private List<string> _PreQueryField = new List<string>();
        [Browsable(false)]
        public List<string> PreQueryField
        {
            get
            {
                return _PreQueryField;
            }
            set
            {
                _PreQueryField = value;
            }
        }

        private List<string> _PreQueryCondition = new List<string>();
        [Browsable(false)]
        public List<string> PreQueryCondition
        {
            get
            {
                return _PreQueryCondition;
            }
            set
            {
                _PreQueryCondition = value;
            }
        }

        private List<string> _PreQueryValue = new List<string>();
        [Browsable(false)]
        public List<string> PreQueryValue
        {
            get
            {
                return _PreQueryValue;
            }
            set
            {
                _PreQueryValue = value;
            }
        }

        private bool _ViewScrollProtect;
        [Category("Infolight"),
        Description("Indicate whether the View will be Locked during edit")]
        public bool ViewScrollProtect
        {
            get { return _ViewScrollProtect; }
            set { _ViewScrollProtect = value; }
        }


        private void OnPrintClick(object sender, EventArgs e)
        {
            BeforeItemClickEventArgs argsbeforeclick = new BeforeItemClickEventArgs("Print");
            OnBeforeItemClick(argsbeforeclick);
            if (!argsbeforeclick.Cancel)
            {
                DoPrint();
                OnAfterItemClick(new AfterItemClickEventArgs("Print"));
            }
        }

        private void DoPrint()
        {
            if (!this.BindingSource.AllowPrint)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoBindingSource", "rightToPrint");
                MessageBox.Show(message);
                return;
            }
            /* print后不能讲state改为browsed状态,否则workflow状态有问题
            // (11 : 1 of 2) 在Print時: SetState(Printing), Print後: SetState(Browsed).
            this.SetState("Printing");

            this.Focus();

            // (11 : 2 of 2) Print後: SetState(Browsed).
            this.SetState("Browsed");
            */
        }

        // Add By Chenjian 2006-01-06
        private object EventStateChanged = new object();
        public event InfoNavigatorStateChangedEventHandler StateChanged
        {
            add
            {
                Events.AddHandler(EventStateChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventStateChanged, value);
            }
        }

        virtual protected void OnStateChanged(InfoNavigatorStateChangedEventArgs e)
        {
            InfoNavigatorStateChangedEventHandler handler =
                Events[EventStateChanged] as InfoNavigatorStateChangedEventHandler;
            if (handler != null && e is InfoNavigatorStateChangedEventArgs)
            {
                handler(this, e);
            }
            if (ViewScrollProtect && this.ViewBindingSource != null)
            {
                Form frm = this.FindForm();
                List<Control> datagridviewcollection = new List<Control>();
                FindControl(frm, typeof(InfoDataGridView), datagridviewcollection);
                for (int i = 0; i < datagridviewcollection.Count; i++)
                {
                    if ((datagridviewcollection[i] as InfoDataGridView).DataSource == this.ViewBindingSource)
                    {
                        (datagridviewcollection[i] as InfoDataGridView).SuspendLayout();//挂起UI, 避免scrollbar显示不正确
                        if (e.NewState.StateText == "Editing" || e.NewState.StateText == "Inserting" || e.NewState.StateText == "Changing")
                        {
                            (datagridviewcollection[i] as InfoDataGridView).Enabled = false;
                        }
                        else
                        {
                            (datagridviewcollection[i] as InfoDataGridView).Enabled = true;
                        }
                        (datagridviewcollection[i] as InfoDataGridView).ResumeLayout();
                    }
                }
            }
        }
        /// <summary>
        /// Find Control(Recursive)
        /// </summary>
        /// <param name="startControl">Control to start</param>
        /// <param name="controlType">Type of control to find</param>
        /// <param name="controlCollection">List of control found</param>
        public void FindControl(Control startControl, Type controlType, List<Control> controlCollection)
        {
            if (startControl.GetType() == controlType)
            {
                controlCollection.Add(startControl);
            }
            if (startControl.Controls.Count == 0)
            {
                return;
            }
            else
            {
                foreach (Control ct in startControl.Controls)
                {
                    FindControl(ct, controlType, controlCollection);
                }
            }
        }

        // End Add
        private object EventQueryConfirm = new object();
        public event QueryConfirmEventHandler QueryConfirm
        {
            add
            {
                Events.AddHandler(EventQueryConfirm, value);
            }
            remove
            {
                Events.RemoveHandler(EventQueryConfirm, value);
            }
        }

        public virtual void OnQueryConfirm(QueryConfirmEventArgs e)
        {
            QueryConfirmEventHandler handler =
                Events[EventQueryConfirm] as QueryConfirmEventHandler;
            if (handler != null && e is QueryConfirmEventArgs)
            {
                handler(this, e);
            }
        }

        private object EventQueryWhere = new object();
        [Category("Infolight"),
        Description("The event ocured before query")]
        public event NavigatorQueryWhereEventHandler QueryWhere
        {
            add
            {
                Events.AddHandler(EventQueryWhere, value);
            }
            remove
            {
                Events.RemoveHandler(EventQueryWhere, value);
            }
        }

        public void OnQueryWhere(NavigatorQueryWhereEventArgs value)
        {
            NavigatorQueryWhereEventHandler handler = (NavigatorQueryWhereEventHandler)Events[EventQueryWhere];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        private object EventBeforeItemClick = new object();
        [Category("Infolight"),
        Description("The event ocured before item clicked")]
        public event BeforeItemClickEventHandler BeforeItemClick
        {
            add
            {
                Events.AddHandler(EventBeforeItemClick, value);
            }
            remove
            {
                Events.RemoveHandler(EventBeforeItemClick, value);
            }
        }

        public void OnBeforeItemClick(BeforeItemClickEventArgs value)
        {
            BeforeItemClickEventHandler handler = (BeforeItemClickEventHandler)Events[EventBeforeItemClick];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        private object EventAfterItemClick = new object();
        [Category("Infolight"),
        Description("The event ocured after item clicked")]
        public event AfterItemClickEventHandler AfterItemClick
        {
            add
            {
                Events.AddHandler(EventAfterItemClick, value);
            }
            remove
            {
                Events.RemoveHandler(EventAfterItemClick, value);
            }
        }

        public void OnAfterItemClick(AfterItemClickEventArgs value)
        {
            AfterItemClickEventHandler handler = (AfterItemClickEventHandler)Events[EventAfterItemClick];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        #endregion

        protected void OnValueControlEnter(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnValueControlEnter];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnValueControlEnter = new object();
        [Category("Infolight")]
        public event EventHandler ValueControlEnter
        {
            add { base.Events.AddHandler(EventOnValueControlEnter, value); }
            remove { base.Events.RemoveHandler(EventOnValueControlEnter, value); }
        }

        #region Method
        public override void AddStandardItems()
        {
            this.ViewMoveFirstItem = new ToolStripButton();
            this.ViewMovePreviousItem = new ToolStripButton();
            this.ViewMoveNextItem = new ToolStripButton();
            this.ViewMoveLastItem = new ToolStripButton();
            this.ViewPositionItem = new ToolStripTextBox();
            this.ViewCountItem = new ToolStripLabel();
            this.AddNewItem = new ToolStripButton();
            this.DeleteItem = new ToolStripButton();
            this.EditItem = new ToolStripButton();
            ToolStripSeparator separator1 = new ToolStripSeparator();
            ToolStripSeparator separator2 = new ToolStripSeparator();
            ToolStripSeparator separator3 = new ToolStripSeparator();
            ToolStripSeparator separator4 = new ToolStripSeparator();
            this.OKItem = new ToolStripButton();
            this.CancelItem = new ToolStripButton();
            this.ApplyItem = new ToolStripButton();
            this.AbortItem = new ToolStripButton();
            this.ViewRefreshItem = new ToolStripButton();
            this.ViewQueryItem = new ToolStripButton();
            this.PrintItem = new ToolStripButton();
            this.ExportItem = new ToolStripButton();
            this.CopyItem = new ToolStripButton();
            this.ViewMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.ViewMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.ViewMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.ViewMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.ViewPositionItem.Name = "bindingNavigatorPositionItem";
            this.ViewCountItem.Name = "bindingNavigatorCountItem";
            this.AddNewItem.Name = "bindingNavigatorAddNewItem";
            this.DeleteItem.Name = "bindingNavigatorDeleteItem";
            separator1.Name = "bindingNavigatorSeparator1";
            separator2.Name = "bindingNavigatorSeparator2";
            separator3.Name = "bindingNavigatorSeparator3";
            separator4.Name = "bindingNavigatorSeparator4";
            this.EditItem.Name = "bindingNavigatorEditItem";
            this.OKItem.Name = "bindingNavigatorOKItem";
            this.CancelItem.Name = "bindingNavigatorCancelItem";
            this.ApplyItem.Name = "bindingNavigatorApplyItem";
            this.AbortItem.Name = "bindingNavigatorAbortItem";
            this.ViewRefreshItem.Name = "bindingNavigatorRefreshItem";
            this.ViewQueryItem.Name = "bindingNavigatorQueryItem";
            this.PrintItem.Name = "bindingNavigatorPrintItem";
            this.ExportItem.Name = "bindingNavigatorExportItem";
            this.CopyItem.Name = "bindingNavigatorCopyItem";
            ViewMoveFirstItem.Text = "first";
            ViewMovePreviousItem.Text = "previous";
            ViewMoveNextItem.Text = "next";
            ViewMoveLastItem.Text = "last";
            AddNewItem.Text = "add";
            DeleteItem.Text = "delete";
            EditItem.Text = "edit";
            OKItem.Text = "ok";
            CancelItem.Text = "cancel";
            ApplyItem.Text = "apply";
            AbortItem.Text = "abort";
            ViewRefreshItem.Text = "refresh";
            ViewQueryItem.Text = "query";
            PrintItem.Text = "print";
            ExportItem.Text = "export";
            CopyItem.Text = "copy";
            ViewCountItem.Text = "of {0}";
            ViewCountItemFormat = "of {0}";
            this.ViewCountItem.ToolTipText = "count";
            this.ViewPositionItem.ToolTipText = "position";
            this.ViewCountItem.AutoToolTip = false;
            this.ViewPositionItem.AutoToolTip = false;
            //////////////////////////////////////////////////////////////////////////////////////
            Bitmap bitmap1 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveFirst.bmp");
            Bitmap bitmap2 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MovePrevious.bmp");
            Bitmap bitmap3 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveNext.bmp");
            Bitmap bitmap4 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveLast.bmp");
            Bitmap bitmap5 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.AddNew.bmp");

            Assembly assembly = this.GetType().Assembly;
            string resDir = this.ResourceDir();
            Bitmap bitmap6 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Delete.png"));
            Bitmap bitmap7 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Edit.png"));
            Bitmap bitmap8 = new Bitmap(assembly.GetManifestResourceStream(resDir + "OK.png"));
            Bitmap bitmap9 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Cancel.png"));
            Bitmap bitmap10 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Refresh.png"));
            Bitmap bitmap11 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Abort.png"));
            Bitmap bitmap12 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Apply.png"));
            Bitmap bitmap13 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Print.png"));
            Bitmap bitmap14 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Query.png"));
            Bitmap bitmap15 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Export.png"));
            Bitmap bitmap16 = new Bitmap(assembly.GetManifestResourceStream(resDir + "Copy.png"));
            //////////////////////////////////////////////////////////////////////////////////////
            bitmap1.MakeTransparent(Color.Magenta);
            bitmap2.MakeTransparent(Color.Magenta);
            bitmap3.MakeTransparent(Color.Magenta);
            bitmap4.MakeTransparent(Color.Magenta);
            bitmap5.MakeTransparent(Color.Magenta);
            bitmap6.MakeTransparent(Color.Magenta);
            bitmap7.MakeTransparent(Color.Magenta);
            bitmap8.MakeTransparent(Color.Magenta);
            bitmap9.MakeTransparent(Color.Magenta);
            bitmap10.MakeTransparent(Color.Magenta);
            bitmap11.MakeTransparent(Color.Magenta);
            bitmap12.MakeTransparent(Color.Magenta);
            bitmap13.MakeTransparent(Color.Magenta);
            bitmap14.MakeTransparent(Color.Magenta);
            bitmap15.MakeTransparent(Color.Magenta);
            bitmap16.MakeTransparent(Color.Magenta);

            //////////////////////////////////////////////////////////////////////////////////////
            this.ViewMoveFirstItem.Image = bitmap1;
            this.ViewMovePreviousItem.Image = bitmap2;
            this.ViewMoveNextItem.Image = bitmap3;
            this.ViewMoveLastItem.Image = bitmap4;
            this.AddNewItem.Image = bitmap5;
            this.DeleteItem.Image = bitmap6;
            this.EditItem.Image = bitmap7;
            this.OKItem.Image = bitmap8;
            this.CancelItem.Image = bitmap9;
            this.ViewRefreshItem.Image = bitmap10;
            this.AbortItem.Image = bitmap11;
            this.ApplyItem.Image = bitmap12;
            this.PrintItem.Image = bitmap13;
            this.ViewQueryItem.Image = bitmap14;
            this.ExportItem.Image = bitmap15;
            this.CopyItem.Image = bitmap16;
            //////////////////////////////////////////////////////////////////////////////////////
            this.ViewMoveFirstItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ViewMovePreviousItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ViewMoveNextItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ViewMoveLastItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.AddNewItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.DeleteItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.EditItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.OKItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.CancelItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ApplyItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.AbortItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ViewRefreshItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ViewQueryItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.PrintItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.ExportItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.CopyItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //////////////////////////////////////////////////////////////////////////////////////

            this.ViewPositionItem.Size = new Size(50, 25);
            ToolStripItem[] itemArray1 = new ToolStripItem[] { this.ViewMoveFirstItem, 
                                                                this.ViewMovePreviousItem, 
                                                                this.ViewMoveNextItem, 
                                                                this.ViewMoveLastItem, 
                                                                separator1,
                                                                this.AddNewItem, 
                                                                this.DeleteItem, 
                                                                this.EditItem, 
                                                                separator2, 
                                                                this.OKItem,
                                                                this.CancelItem,
                                                                this.ApplyItem,
                                                                this.AbortItem,
                                                                separator3, 
                                                                this.ViewRefreshItem,
                                                                this.ViewQueryItem,
                                                                this.PrintItem,
                                                                this.ExportItem,
                                                                this.CopyItem,
                                                                separator4, 
                                                                this.ViewPositionItem,
                                                                this.ViewCountItem };
            this.Items.AddRange(itemArray1);
        }
        protected virtual string ResourceDir()
        {
            return "Srvtools.InfonavigatorImage.";
        }

        private void MoveEnableControl(InfoBindingSource bindingSource)
        {
            MoveEnableControl(bindingSource, true);
        }

        private void MoveEnableControl(InfoBindingSource bindingSource, bool move)
        {
            if (bindingSource != null)
            {
                if (move)
                {
                    if (this.ViewMoveFirstItem != null)
                    {
                        this.ViewMoveFirstItem.Enabled = true;
                    }
                    if (this.ViewMovePreviousItem != null)
                    {
                        this.ViewMovePreviousItem.Enabled = true;
                    }
                    if (this.ViewMoveNextItem != null)
                    {
                        this.ViewMoveNextItem.Enabled = true;
                    }
                    if (this.ViewMoveLastItem != null)
                    {
                        this.ViewMoveLastItem.Enabled = true;
                    }
                }

                if (bindingSource.Position <= 0)
                {
                    if (this.ViewMoveFirstItem != null)
                    {
                        this.ViewMoveFirstItem.Enabled = false;
                    }
                    if (this.ViewMovePreviousItem != null)
                    {
                        this.ViewMovePreviousItem.Enabled = false;
                    }
                }
                if (bindingSource.Position == bindingSource.Count - 1)
                {
                    if (this.ViewMoveNextItem != null)
                    {
                        this.ViewMoveNextItem.Enabled = false;
                    }
                    if (this.ViewMoveLastItem != null)
                    {
                        this.ViewMoveLastItem.Enabled = false;
                    }
                }
            }
        }

        // Add By Chenjian 2006-01-05
        protected StateItem PrevousStateItem = null;
        protected StateItem CurrentStateItem = null;
        public void SetState(string stateText)
        {
            InnerSetState(stateText, true);
            MoveEnableControl(this.ViewBindingSource == null ? this.BindingSource : this.ViewBindingSource, false);
            //remarked by lily 2009-4-21 將下面程式移動到InnerSetState中，否則在StateChanged事件讀取CurrentState會是舊的狀態
            //_CurrentState = stateText;
        }
        //modified by lily 2009-4-29 給flnavigator使用。
        protected string _CurrentState;
        [Browsable(false)]
        public string CurrentState
        {
            get { return _CurrentState; }
        }

        protected virtual void InnerSetState(string stateText, bool raiseStateChangedEvent)
        {
            foreach (StateItem stateItem in States)
            {
                if (stateItem.StateText == stateText)
                {
                    if (this.StatusStrip != null && this.StatusStrip.ShowNavigatorStatus == true)
                    {
                        string strState = "";
                        foreach (StateItem item in m_states)
                        {
                            if (item.StateText == stateText)
                            {
                                if (item.Description != null)
                                {
                                    strState = item.Description;
                                }
                                else
                                {
                                    strState = item.StateText;
                                }
                                break;
                            }
                        }
                        StatusStrip.SetNavigatorStatus(strState);
                    }
                    //Modified by lily 2007/3/10 when state is changed to set the buttons.
                    PrevousStateItem = CurrentStateItem;
                    CurrentStateItem = stateItem;
                    if ((PrevousStateItem == null) || (PrevousStateItem != CurrentStateItem))
                    {
                        foreach (ToolStripItem toolItem in this.Items)
                        {
                            if (stateItem.EnabledControls != null)
                            {
                                if (stateItem.EnabledControls.Contains(toolItem.Name))
                                {
                                    if (!this.HideItemStates)
                                        toolItem.Enabled = true;
                                    else
                                        toolItem.Visible = true;
                                }
                                else
                                {
                                    if (!this.HideItemStates)
                                        toolItem.Enabled = false;
                                    else
                                        toolItem.Visible = false;
                                }
                            }
                        }
                    }
                    if (stateText == "Browsed" || stateText == "Initial")    //没有资料时将EditItem disable
                    {
                        int count = this.ViewBindingSource != null ?
                            ViewBindingSource.Count : (this.BindingSource != null ? this.BindingSource.Count : 0);
                        if (count == 0)
                        {
                            if (this.EditItem != null)
                            {
                                if (!this.HideItemStates)
                                    EditItem.Enabled = false;
                                else
                                    EditItem.Visible = false;
                            }
                            if (this.DeleteItem != null)
                            {
                                if (!this.HideItemStates)
                                    DeleteItem.Enabled = false;
                                else
                                    DeleteItem.Visible = false;
                            }
                            if (this.ViewRefreshItem != null)
                            {
                                if (!this.HideItemStates)
                                    ViewRefreshItem.Enabled = false;
                                else
                                    ViewRefreshItem.Visible = false;
                            }
                            if (this.ExportItem != null)
                            {
                                if (!this.HideItemStates)
                                    ExportItem.Enabled = false;
                                else
                                    ExportItem.Visible = false;
                            }
                            if (this.CopyItem != null)
                            {
                                if (!this.HideItemStates)
                                    CopyItem.Enabled = false;
                                else
                                    CopyItem.Visible = false;
                            }

                        }

                    }
                    if (this.DescriptionItem != null)
                    {
                        this.DescriptionItem.Text = stateItem.Description;
                    }
                    //added by lily 2009-4-21 ，否則在StateChanged事件讀取CurrentState會是舊的狀態 
                    _CurrentState = stateText;
                    // Raise StateChanged Event
                    if (raiseStateChangedEvent && PrevousStateItem != CurrentStateItem)
                    {
                        OnStateChanged(new InfoNavigatorStateChangedEventArgs(CurrentStateItem, PrevousStateItem));
                    }
                    break;
                }
            }
        }

        public string GetCurrentState()
        {
            return CurrentState;
        }

        public void SetLanguage(SYS_LANGUAGE languageID)
        {
            if (this.GetServerText)
            {
                string message = SysMsg.GetSystemMessage(languageID, "Srvtools", "InfoNavigator", "NavText");
                string[] texts = message.Split(';');
                if (ViewMoveFirstItem != null)
                    ViewMoveFirstItem.Text = texts[0];
                if (ViewMovePreviousItem != null)
                    ViewMovePreviousItem.Text = texts[1];
                if (ViewMoveNextItem != null)
                    ViewMoveNextItem.Text = texts[2];
                if (ViewMoveLastItem != null)
                    ViewMoveLastItem.Text = texts[3];
                if (AddNewItem != null)
                    AddNewItem.Text = texts[4];
                if (DeleteItem != null)
                    DeleteItem.Text = texts[5];
                if (EditItem != null)
                    EditItem.Text = texts[6];
                if (OKItem != null)
                    OKItem.Text = texts[7];
                if (CancelItem != null)
                    CancelItem.Text = texts[8];
                if (ApplyItem != null)
                    ApplyItem.Text = texts[9];
                if (AbortItem != null)
                    AbortItem.Text = texts[10];
                if (ViewRefreshItem != null)
                    ViewRefreshItem.Text = texts[11];
                if (ViewQueryItem != null)
                    ViewQueryItem.Text = texts[12];
                if (PrintItem != null)
                    PrintItem.Text = texts[13];
                if (ExportItem != null)
                    ExportItem.Text = texts[14];
                if (ViewCountItem != null)
                    ViewCountItem.Text = texts[15];
                ViewCountItemFormat = texts[15];

                message = SysMsg.GetSystemMessage(languageID, "Srvtools", "InfoNavigator", "NavStates");
                string[] states = message.Split(';');
                foreach (StateItem item in m_states)
                {
                    switch (item.StateText)
                    {
                        case "Initial":
                            item.Description = states[0];
                            break;
                        case "Browsed":
                            item.Description = states[1];
                            break;
                        case "Inserting":
                            item.Description = states[2];
                            break;
                        case "Editing":
                            item.Description = states[3];
                            break;
                        case "Applying":
                            item.Description = states[4];
                            break;
                        case "Changing":
                            item.Description = states[5];
                            break;
                        case "Querying":
                            item.Description = states[6];
                            break;
                        case "Printing":
                            item.Description = states[7];
                            break;
                    }
                }

                InnerSetState(this.CurrentState, false);

                m_sureDeleteText = SysMsg.GetSystemMessage(languageID, "Srvtools", "InfoNavigator", "sureDeleteText");
                m_sureInsertText = SysMsg.GetSystemMessage(languageID, "Srvtools", "InfoNavigator", "sureInsertText");
            }
        }

        private ClientQuery CopyQueryFileds(int columncount)
        {
            ClientQuery cq = new ClientQuery();
            cq.OwnerComp = (IDataModule)this.FindForm();
            if (this.QueryFields.Count > 0)
            {
                InfoBindingSource ibs = null;
                if (this.ViewBindingSource != null)
                {
                    ibs = this.ViewBindingSource;
                }
                else if (this.BindingSource != null)
                {
                    ibs = this.BindingSource;
                }
                if (ibs == null)
                {
                    throw new Exception("Can't find InfoBindingSource");
                }
                cq.BindingSource = ibs;
                cq.GapVertical = 8;
                cq.KeepCondition = this.QueryKeepCondition;
                cq.Margin = this.QueryMargin;
                cq.Font = this.QueryFont;
                int columnindex = 0;
                //int index = 0;
                foreach (QueryField qf in this.QueryFields)
                {
                    QueryColumns qc = new QueryColumns(qf.Name, true, qf.FieldName, qf.Caption, 120);
                    //if (PreQueryValue.Count > 0)
                    //{
                    //    qc.Text = PreQueryValue[index];
                    //    index++;
                    //}

                    if (columnindex == columncount)
                    {
                        qc.NewLine = true;
                        columnindex = 1;
                    }
                    else
                    {
                        qc.NewLine = false;
                        columnindex++;
                    }
                    qc.DefaultValue = qf.DefaultValue;
                    if (qf.Mode == "")
                    {
                        qc.ColumnType = "ClientQueryTextBoxColumn";
                    }
                    else
                    {
                        qc.ColumnType = "ClientQuery" + qf.Mode + "Column";
                    }
                    if (qf.Condition == "")
                    {
                        Type tp = (ibs.GetDataSource() as InfoDataSet).RealDataSet.Tables[ibs.DataMember].Columns[qf.FieldName].DataType;
                        if (tp == typeof(string))
                        {
                            qc.Operator = "%";
                        }

                        else
                        {
                            qc.Operator = "=";
                        }
                    }
                    else
                    {
                        qc.Operator = qf.Condition;
                    }
                    qc.Width = qf.Width;
                    qc.InfoRefVal = qf.RefVal;
                    qc.InfoRefButtonAutoPanel = qf.RefButtonAutoPanel;
                    qc.InfoRefButtonPanel = qf.RefButtonPanel;
                    qc.IsNvarChar = qf.IsNvarChar;
                    cq.Columns.Add(qc);
                }
            }
            else
            {
                throw new Exception("No QueryFields in InfoNavigator");
            }
            return cq;

        }

        AnyQuery aq;
        private AnyQuery CopyAnyQueryFileds()
        {
            AnyQuery aAnyQuery = new AnyQuery();
            Control baseForm = this.Parent;
            while (baseForm != null)
            {
                if (baseForm is InfoForm)
                {
                    aAnyQuery.OwnerComp = baseForm as InfoForm;
                    break;
                }
                else
                {
                    baseForm = baseForm.Parent;
                }
            }
            aAnyQuery.ValueControlEnter += new EventHandler(aq_ValueControlEnter);
            InfoBindingSource ibs = null;
            if (this.ViewBindingSource != null && this.DetailBindingSource == null)
            {
                ibs = this.ViewBindingSource;
            }
            else if (this.BindingSource != null)
            {
                ibs = this.BindingSource;
            }
            if (ibs == null)
            {
                throw new Exception("Can't find InfoBindingSource");
            }

            aAnyQuery.BindingSource = ibs;
            foreach (QueryField qf in this.QueryFields)
            {
                AnyQueryColumns qc = new AnyQueryColumns(qf.Name, qf.FieldName, qf.Caption, 120);

                qc.DefaultValue = qf.DefaultValue;
                if (qf.Mode == "")
                {
                    qc.ColumnType = "AnyQueryTextBoxColumn";
                }
                else
                {
                    qc.ColumnType = "AnyQuery" + qf.Mode + "Column";
                }
                if (qf.Condition == "" && (ibs.GetDataSource() as InfoDataSet).RealDataSet.Tables[ibs.DataMember].Columns[qf.FieldName] != null)
                {
                    Type tp = (ibs.GetDataSource() as InfoDataSet).RealDataSet.Tables[ibs.DataMember].Columns[qf.FieldName].DataType;
                    if (tp == typeof(string))
                    {
                        qc.Operator = "%";
                    }
                    else
                    {
                        qc.Operator = "=";
                    }
                }
                else
                {
                    qc.Operator = qf.Condition;
                }
                qc.InfoRefVal = qf.RefVal;
                qc.InfoRefButtonAutoPanel = qf.RefButtonAutoPanel;
                qc.InfoRefButtonPanel = qf.RefButtonPanel;
                qc.Enabled = qf.Enabled;
                qc.AutoSelect = qf.AutoSelect;
                qc.Items = qf.Items;
                qc.DateConver = qf.DateConver;
                qc.Width = qf.Width;
                qc.ColumnWidth = qf.ColumnWidth;
                qc.IsNvarChar = qf.IsNvarChar;
                aAnyQuery.Columns.Add(qc);
            }
            if (this.AnyQueryID == String.Empty)
                aAnyQuery.PackageForm = (this.Parent as InfoForm).PackageName + "." + (this.Parent as InfoForm).FormName;
            aAnyQuery.AnyQueryID = this.AnyQueryID;
            aAnyQuery.AutoDisableColumns = this.AutoDisableColumns;
            aAnyQuery.MaxColumnCount = this.MaxColumnCount;
            aAnyQuery.QueryColumnMode = this.QueryColumnMode;
            aAnyQuery.DetailBindingSource = this.DetailBindingSource;
            aAnyQuery.MasterDetailField = this.MasterDetailField;
            aAnyQuery.DetailKeyField = this.DetailKeyField;
            aAnyQuery.KeepCondition = this.QueryKeepCondition;
            aAnyQuery.DisplayAllOperator = this.DisplayAllOperator;
            return aAnyQuery;
        }

        public String[] GetControlValues(int count)
        {
            return aq.GetControlValues(count);
        }

        void aq_ValueControlEnter(object sender, EventArgs e)
        {
            OnValueControlEnter(e);
        }

        ClientQuery cqpanel = null;

        public void Show(Panel pn)
        {
            this.Show(pn, 1);
        }

        public void Show(Panel pn, int Columns)
        {
            if (Columns < 1)
            {
                throw new Exception("Parameter Columns of InfoNavigator.Show() should larger than 1");
            }
            if (cqpanel == null)
            {
                cqpanel = this.CopyQueryFileds(Columns);
            }
            cqpanel.Show(pn);
        }

        public void Clear(Panel pn)
        {
            if (cqpanel != null)
            {
                cqpanel.Clear(pn);
            }
        }

        public string GetWhere(Panel pn)
        {
            if (cqpanel != null)
            {
                return cqpanel.GetWhere(pn);
            }
            else
            {
                return string.Empty;
            }
        }

        public void Execute(Panel pn)
        {
            string strwhere = GetWhere(pn);
            NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strwhere);
            OnQueryWhere(args);
            if (!args.Cancel)
            {
                strwhere = args.WhereString;
                InfoBindingSource ibs = null;
                if (this.ViewBindingSource != null)
                {
                    ibs = this.ViewBindingSource;
                }
                else
                {
                    ibs = this.BindingSource;
                }
                ((InfoDataSet)ibs.GetDataSource()).SetWhere(strwhere);
            }
        }

        public string GetWhereText(Panel pn)
        {
            ClientQuery cq = this.CopyQueryFileds(1);
            cq.isShow.Add(pn.Name);
            cq.isShowInsp = true;
            cq.GetWhere(pn);
            return QueryTranslate.Translate(cq);
        }

        public string GetWhereText()
        {
            ClientQuery cq = this.CopyQueryFileds(1);
            return QueryTranslate.Translate(cq);
        }


        protected override void WndProc(ref Message m)
        {
            //(1: 1 of 1) 打開Form時: SetState(Initial)
            if (!this.DesignMode && m.Msg == WM_ShowWindow)
            {
                this.SetState("Initial");
            }
            base.WndProc(ref m);
        }

        protected void WndProc2(ref Message m)
        {
            base.WndProc(ref m);
        }

        // End Add

        #endregion

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        #region IGetValues 成员

        public string[] GetValues(string sKind)
        {
            List<String> values = new List<string>();
            if (sKind == "DetailKeyField")
            {
                if (this.DetailBindingSource != null)
                {
                    DataView dataView = this.DetailBindingSource.List as DataView;

                    if (dataView != null)
                    {
                        foreach (DataColumn column in dataView.Table.Columns)
                        {
                            values.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet ds = ((InfoDataSet)this.DetailBindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < ds.Relations.Count; i++)
                        {
                            if (this.DetailBindingSource.DataMember == ds.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
                }
            }
            else if (sKind == "MasterDetailField")
            {
                if (this.BindingSource != null)
                {
                    DataView dataView = this.BindingSource.List as DataView;

                    if (dataView != null)
                    {
                        foreach (DataColumn column in dataView.Table.Columns)
                        {
                            values.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet ds = ((InfoDataSet)this.BindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < ds.Relations.Count; i++)
                        {
                            if (this.BindingSource.DataMember == ds.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
                }
            }
            return values.ToArray();
        }

        #endregion
    }

    #region InfoNavigatorStateChangedEventArgs
    public class InfoNavigatorStateChangedEventArgs : EventArgs
    {
        private StateItem m_newState = null;
        private StateItem m_oldState = null;

        public InfoNavigatorStateChangedEventArgs(StateItem newState, StateItem oldState)
        {
            m_newState = newState;
            m_oldState = oldState;
        }

        public StateItem NewState
        {
            get
            {
                return m_newState;
            }
        }

        [Description("Can be null")]
        public StateItem OldState
        {
            get
            {
                return m_oldState;
            }
        }
    }


    public delegate void InfoNavigatorStateChangedEventHandler(object sender, InfoNavigatorStateChangedEventArgs e);
    #endregion InfoNavigatorStateChangedEventArgs

    #region InfoNavigatorBeforeQueryEventArgs
    public class NavigatorQueryWhereEventArgs : EventArgs
    {

        public NavigatorQueryWhereEventArgs(string querytext)
            : this(querytext, false)
        { }

        public NavigatorQueryWhereEventArgs(string querytext, bool uFilter)
        {
            _WhereString = querytext;
            useFilter = uFilter;
        }

        private string _WhereString;
        /// <summary>
        /// The text of query
        /// </summary>
        public string WhereString
        {
            get { return _WhereString; }
            set { _WhereString = value; }
        }

        private bool _Cancel;
        /// <summary>
        /// Cancel the query event
        /// </summary>
        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }

        private bool useFilter;

	    public bool UseFilter
	    {
            get { return useFilter; }
	    }
	
    }

    public delegate void NavigatorQueryWhereEventHandler(object sender, NavigatorQueryWhereEventArgs e);
    #endregion

    #region BeforeItemClick
    public class BeforeItemClickEventArgs : EventArgs
    {
        public BeforeItemClickEventArgs(string itemname)
        {
            _ItemName = itemname;
            _Cancel = false;
        }

        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { _ItemName = value; }
        }

        private bool _Cancel;

        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }
    }

    public delegate void BeforeItemClickEventHandler(object sender, BeforeItemClickEventArgs e);
    #endregion

    #region AfterItemClick
    public class AfterItemClickEventArgs : EventArgs
    {
        public AfterItemClickEventArgs(string itemname)
        {
            _ItemName = itemname;
        }

        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { _ItemName = value; }
        }
    }

    public delegate void AfterItemClickEventHandler(object sender, AfterItemClickEventArgs e);
    #endregion

    #region QueryConfirmEventArgs
    public class QueryConfirmEventArgs : EventArgs
    {
        private string _Filter = "";

        public QueryConfirmEventArgs(string filter)
        {
            _Filter = filter;
        }

        public string Filter
        {
            get
            {
                return _Filter;
            }
        }
    }
    public delegate void QueryConfirmEventHandler(object sender, QueryConfirmEventArgs e);
    #endregion

    #region ExportArgs
    public class ExportArgs : EventArgs
    {
        public ExportArgs(string tablename, string filter, string sort)
        {
            _TableName = tablename;
            _Filter = filter;
            _Sort = sort;
        }

        private string _Filter;

        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        private string _Sort;

        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }

        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

    }

    public delegate void BeforeExportEventHandler(object sender, ExportArgs e);
    #endregion

    #region StateCollection
    [Editor(typeof(StateCollectionEditor), typeof(UITypeEditor))]
    public class StateCollection : InfoOwnerCollection
    {
        public StateCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(Transaction))
        {
            StateItem InitStateItem = new StateItem();
            base.Add(InitStateItem);
            InitStateItem.StateText = "Initial";

            StateItem BrowsedStateItem = new StateItem();
            base.Add(BrowsedStateItem);
            BrowsedStateItem.StateText = "Browsed";

            StateItem InsertingStateItem = new StateItem();
            base.Add(InsertingStateItem);
            InsertingStateItem.StateText = "Inserting";

            StateItem EditingStateItem = new StateItem();
            base.Add(EditingStateItem);
            EditingStateItem.StateText = "Editing";

            StateItem ApplyingStateItem = new StateItem();
            base.Add(ApplyingStateItem);
            ApplyingStateItem.StateText = "Applying";

            StateItem ChangingStateItem = new StateItem();
            base.Add(ChangingStateItem);
            ChangingStateItem.StateText = "Changing";

            StateItem QueryingStateItem = new StateItem();
            base.Add(QueryingStateItem);
            QueryingStateItem.StateText = "Querying";

            StateItem PrintingStateItem = new StateItem();
            base.Add(PrintingStateItem);
            PrintingStateItem.StateText = "Printing";
        }

        public new StateItem this[int index]
        {
            get
            {
                return (StateItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is StateItem)
                    {
                        //原来的Collection设置为0
                        ((StateItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((StateItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }

        new public virtual void Remove(object value)
        {
            StateItem stateItem = value as StateItem;
            if (stateItem != null)
            {
                if (stateItem.StateText == "Initial"
                    || stateItem.StateText == "Browsed"
                    || stateItem.StateText == "Inserting"
                    || stateItem.StateText == "Editing"
                    || stateItem.StateText == "Applying"
                    || stateItem.StateText == "Changing"
                    || stateItem.StateText == "Querying"
                    || stateItem.StateText == "Printing")
                {
                    throw new Exception("Default StateItem can not be removed");
                }
                else
                {
                    base.Remove(value);
                }
            }
        }

        new public virtual void RemoveAt(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                StateItem stateItem = this[index];
                if (stateItem.StateText == "Initial"
                    || stateItem.StateText == "Browsed"
                    || stateItem.StateText == "Inserting"
                    || stateItem.StateText == "Editing"
                    || stateItem.StateText == "Applying"
                    || stateItem.StateText == "Changing"
                    || stateItem.StateText == "Querying"
                    || stateItem.StateText == "Printing")
                {
                    throw new Exception("Default StateItem can not be removed");
                }
                else
                {
                    base.RemoveAt(index);
                }
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        new public void Clear()
        {
            this.ClearExceptDefaultStateItem();
        }

        new public void Add(object value)
        {
            StateItem stateItem = value as StateItem;
            if (stateItem != null)
            {
                foreach (StateItem si in this)
                {
                    if (si.StateText == stateItem.StateText)
                    {
                        si.EnabledControls = stateItem.EnabledControls;
                        si.Description = stateItem.Description;
                        si.EnabledControlsEdited = stateItem.EnabledControlsEdited;
                        return;
                    }
                }

                base.Add(stateItem);
            }
        }

        public virtual void ClearExceptDefaultStateItem()
        {
            // The number of Default StateItem is 8
            while (this.Count > 8)
            {
                base.RemoveAt(8);
            }
        }
    }
    #endregion StateCollection

    #region StateCollectionEditor
    internal class StateCollectionEditor : UITypeEditor
    {
        IWindowsFormsEditorService EditorService = null;
        public StateCollectionEditor()
            : base()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService))
                    as IWindowsFormsEditorService;
            }

            InfoNavigator navigator = context.Instance as InfoNavigator;

            if (navigator != null && EditorService != null)
            {
                if (value is StateCollection)
                {
                    StateCollectionEditorDialog editorDialog =
                        new StateCollectionEditorDialog(value as StateCollection);
                    if (DialogResult.OK ==
                        EditorService.ShowDialog(editorDialog))
                    {
                        IComponentChangeService ComponentChangeService =
                            provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

                        object oldValue = null;
                        object newValue = null;

                        // States changed
                        PropertyDescriptor descStates = TypeDescriptor.GetProperties(navigator)["States"];
                        ComponentChangeService.OnComponentChanging(navigator, descStates);
                        oldValue = value;

                        value = editorDialog.Collection;

                        newValue = value;
                        ComponentChangeService.OnComponentChanged(navigator, descStates, oldValue, newValue);
                    }
                }
            }

            return value;
        }
    }
    #endregion StateCollectionEditor

    #region StateItem
    public class StateItem : InfoOwnerCollectionItem
    {
        private string m_name;
        private string m_stateText;
        private string m_description;
        private bool m_enabledControlsEdited = false;
        private List<string> m_enabledControls = new List<string>();

        public StateItem()
        {
            //////////////////////////////////////
            m_enabledControls = new List<string>();
            //////////////////////////////////////
        }

        public override string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public string StateText
        {
            get
            {
                return m_stateText;
            }
            set
            {
                if (m_stateText == "Initial"
                    || m_stateText == "Browsed"
                    || m_stateText == "Inserting"
                    || m_stateText == "Editing"
                    || m_stateText == "Applying"
                    || m_stateText == "Changing"
                    || m_stateText == "Querying"
                    || m_stateText == "Printing")
                {
                    throw new Exception("Default StateText Can not be Changed");
                }
                else if (value == null || value.Trim() == "")
                {
                    throw new Exception("Empty StateText not allowed");
                }
                else
                {
                    StateCollection stateCollection = this.Collection as StateCollection;
                    if (stateCollection != null)
                    {
                        foreach (StateItem stateItem in stateCollection)
                        {
                            if (stateItem.StateText == value.Trim())
                            {
                                throw new Exception("StateText already exists");
                            }
                        }
                    }
                    m_stateText = value.Trim();
                }
            }
        }

        public string Description
        {
            get
            {
                return m_description;
            }
            set
            {
                m_description = value;
            }
        }

        [Browsable(false)]
        public bool EnabledControlsEdited
        {
            get
            {
                return m_enabledControlsEdited;
            }
            set
            {
                m_enabledControlsEdited = value;
            }
        }

        [Editor(typeof(EnabledControlsEditor), typeof(UITypeEditor))]
        public List<string> EnabledControls
        {
            get
            {
                return m_enabledControls;
            }
            set
            {
                if (value == null || value is List<string>)
                {
                    m_enabledControls = value;
                }
            }
        }
    }
    #endregion StateItem

    #region EnabledControlsEditor
    internal class EnabledControlsEditor : UITypeEditor
    {
        IWindowsFormsEditorService EditorService = null;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService))
                    as IWindowsFormsEditorService;
            }
            StateItem stateItem = context.Instance as StateItem;
            InfoNavigator navigator = stateItem.Owner as InfoNavigator;

            if (EditorService != null && navigator != null)
            {
                if (value is List<string>)
                {
                    EnabledControlsEditorDialog editorDialog =
                        new EnabledControlsEditorDialog(value as List<string>, navigator);
                    if (DialogResult.OK ==
                        EditorService.ShowDialog(editorDialog))
                    {
                        stateItem.EnabledControlsEdited = true;
                        value = editorDialog.EnabledControls;
                    }
                }
            }
            return value;
        }
    }
    #endregion EnabledControlsEditor

    #region QueryFieldCollection class
    public class QueryFieldCollection : InfoOwnerCollection
    {
        public QueryFieldCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(QueryField))
        {

        }

        public DataSet DsForDD = new DataSet();
        public new QueryField this[int index]
        {
            get
            {
                return (QueryField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is QueryField)
                    {
                        //原来的Collection设置为0
                        ((QueryField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((QueryField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region QueryField class
    public class QueryField : InfoOwnerCollectionItem, IGetValues
    {
        public QueryField()
            : this("", "", "")
        {
            _Mode = "";
            _DefaultValue = "";
        }

        public QueryField(string fieldName, string caption, string condition)
        {
            _FieldName = fieldName;
            _Caption = caption;
            _Condition = condition;
        }

        public override string Name
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public override string ToString()
        {
            return _FieldName;
        }

        private string _FieldName;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
                //if (this.Owner != null && ((InfoNavigator)this.Owner).Site.DesignMode)
                //{
                //    string caption = GetHeaderText(_FieldName);
                //    if (caption != "")
                //    {
                //        this.Caption = caption;
                //    }
                //    else
                //    {
                //        this.Caption = _FieldName;
                //    }
                //}

                if (this.Owner != null)
                {
                    String columnName = _FieldName;
                    if (_FieldName.StartsWith("Detail."))
                    {
                        columnName = columnName.Substring(columnName.IndexOf('.') + 1);

                        if (((InfoNavigator)this.Owner).Site == null)
                        {
                            this.Caption = GetDetailHeaderText(columnName);
                        }
                        else if (((InfoNavigator)this.Owner).Site.DesignMode)
                        {
                            this.Caption = GetDetailHeaderText(columnName);
                        }

                        //if (!this.Caption.StartsWith("Detail."))
                        //    this.Caption = "Detail." + this.Caption;
                    }
                    else
                    {
                        if (((InfoNavigator)this.Owner).Site == null)
                        {
                            this.Caption = GetHeaderText(columnName);
                        }
                        else if (((InfoNavigator)this.Owner).Site.DesignMode)
                        {
                            this.Caption = GetHeaderText(columnName);
                        }
                    }
                }
            }
        }

        private string _Caption;
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
            }
        }

        private string _Condition;
        [Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
        public string Condition
        {
            get
            {
                return _Condition;
            }
            set
            {
                _Condition = value;
            }
        }

        private string _DefaultValue;

        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }


        private string _Mode;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Mode
        {
            get { return _Mode; }
            set
            {
                _Mode = value;
                if (_Mode == "RefButton")
                {
                    RefButtonAutoPanel = true;
                }
                else
                {
                    RefButtonAutoPanel = false;
                }
            }
        }

        private InfoRefVal _RefVal;
        public InfoRefVal RefVal
        {
            get { return _RefVal; }
            set
            {
                if (_Mode != "RefButton" && _Mode != "RefVal" && _Mode != "ComboBox" && value != null)
                {
                    MessageBox.Show("InfoRefValue can be set only when\nMode is refbutton or refval.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _RefVal = value;
                }
            }
        }

        private Panel _refButtonPanel;
        public Panel RefButtonPanel
        {
            get
            {
                return _refButtonPanel;
            }
            set
            {
                if (_Mode != "RefButton" && value != null)
                {
                    MessageBox.Show("InfoRefButtonPanel can be set only when\nMode is refbutton.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _refButtonPanel = value;
                }
            }
        }

        private bool _refButtonAutoPanel;
        public bool RefButtonAutoPanel
        {
            get
            {
                return _refButtonAutoPanel;
            }
            set
            {
                if (_Mode != "RefButton" && value != false)
                {
                    MessageBox.Show("InfoRefButtonAutoPanel can be set only when\ncolumntype is refbutton.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _refButtonAutoPanel = value;
                }
            }
        }

        private bool _enabled = true;
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        private bool _autoSelect = false;
        [DefaultValue(false)]
        public bool AutoSelect
        {
            get
            {
                return _autoSelect;
            }
            set
            {
                _autoSelect = value;
            }
        }

        private bool isNvarChar;

        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }
	

        private String[] _items;
        public String[] Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        private bool _dateConver;
        [DefaultValue(false)]
        public bool DateConver
        {
            get
            {
                return _dateConver;
            }
            set
            {
                _dateConver = value;
            }
        }

        private int _width = 120;
        [DefaultValue(120)]
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private int _columnWidth = 120;
        [DefaultValue(120)]
        public int ColumnWidth
        {
            get
            {
                return _columnWidth;
            }
            set
            {
                _columnWidth = value;
            }
        }

        private string GetHeaderText(string ColName)
        {
            DataSet ds = ((QueryFieldCollection)this.Collection).DsForDD;
            string strTableName = "";

            strTableName = ((InfoNavigator)this.Owner).BindingSource.DataMember;
            if (ds.Tables.Count == 0)
            {
                ((QueryFieldCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((InfoNavigator)this.Owner).BindingSource, true);
                ds = ((QueryFieldCollection)this.Collection).DsForDD;
            }

            string strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[strTableName].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[strTableName].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        strHeaderText = ds.Tables[strTableName].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        private string GetDetailHeaderText(string ColName)
        {
            DataSet ds =  DBUtils.GetDataDictionary(((InfoNavigator)this.Owner).DetailBindingSource, true);

            string strHeaderText = ColName;
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        if (ds.Tables[0].Rows[j]["CAPTION"].ToString() != String.Empty)
                            strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            List<string> retList = new List<string>();
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is InfoNavigator)
                {
                    InfoNavigator nav = (InfoNavigator)this.Owner;

                    InfoBindingSource bindingsource = nav.BindingSource;
                    if (nav.ViewBindingSource != null)
                    {
                        bindingsource = nav.ViewBindingSource;
                    }
                    string tabName = bindingsource.DataMember;
                    InfoDataSet ds = (InfoDataSet)bindingsource.GetDataSource();
                    int i = ds.RealDataSet.Tables[tabName].Columns.Count;
                    for (int j = 0; j < i; j++)
                    {
                        retList.Add(ds.RealDataSet.Tables[tabName].Columns[j].ColumnName);
                    }

                    if ((this.Owner as InfoNavigator).DetailBindingSource != null)
                    {
                        DataView dataView = (this.Owner as InfoNavigator).DetailBindingSource.List as DataView;

                        if (dataView != null)
                        {
                            foreach (DataColumn column in dataView.Table.Columns)
                            {
                                retList.Add("Detail." + column.ColumnName);
                            }
                        }
                        else
                        {
                            int iRelationPos = -1;
                            DataSet dsDetail = ((InfoDataSet)(this.Owner as InfoNavigator).DetailBindingSource.GetDataSource()).RealDataSet;
                            for (int j = 0; j < dsDetail.Relations.Count; i++)
                            {
                                if ((this.Owner as InfoNavigator).DetailBindingSource.DataMember == dsDetail.Relations[j].RelationName)
                                {
                                    iRelationPos = j;
                                    break;
                                }
                            }
                            if (iRelationPos != -1)
                            {
                                foreach (DataColumn column in dsDetail.Relations[iRelationPos].ChildTable.Columns)
                                {
                                    retList.Add("Detail." + column.ColumnName);
                                }
                            }
                        }
                    }

                }
            }
            else if (string.Compare(sKind, "mode", true) == 0)//IgnoreCase
            {
                retList.AddRange(new string[6] { "TextBox", "ComboBox", "RefVal", "Calendar", "CheckBox", "RefButton" });
            }
            return retList.ToArray();
        }
        #endregion
    }
    #endregion

    #region ConditionEditor class
    public class ConditionEditor : UITypeEditor
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
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }
            if (EditorService != null)
            {
                ListBox ColumnList = new ListBox();
                ColumnList.SelectionMode = SelectionMode.One;
                if (((context.Instance as QueryField).Owner as InfoNavigator).QueryMode == InfoNavigator.QueryModeType.ClientQuery)
                    ColumnList.Items.AddRange(new object[] { "<=", "<", "=", "!=", ">", ">=", "%", "%%" });
                else if (((context.Instance as QueryField).Owner as InfoNavigator).QueryMode == InfoNavigator.QueryModeType.AnyQuery)
                    ColumnList.Items.AddRange(new object[] { "<=", "<", "=", "!=", ">", ">=", "%**", "**%", "%%", "!%%", "<->", "!<->", "IN", "NOT IN" });
                else
                    ColumnList.Items.AddRange(new object[] { "<=", "<", "=", "!=", ">", ">=", "%", "%%" });

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
    #endregion
}