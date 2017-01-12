using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Srvtools
{
    // Add By Chenjian 2006-01-09
    public delegate void InfoDataSetEventHandler(object sender, EventArgs e);
    // End Add

	/// <summary>
	/// Summary description for Component.
	/// </summary>
    [ToolboxItem(true)]
    [Designer(typeof(InfoDataSetEditor), typeof(IDesigner))]
    [ToolboxBitmap(typeof(InfoDataSet), "Resources.InfoDataSet.ico")]
	public class InfoDataSet : InfoBaseComp, IListSource, IGetValues, IFindContainer, IInfoDataSet, ISupportInitialize
	{
        private bool _isDesignMode;

        // Add By Chenjian 2006-01-09
        private object EventFillData = new object();
        public event InfoDataSetEventHandler DataFilled
        {
            add
            {
                Events.AddHandler(EventFillData, value);
            }
            remove
            {
                Events.RemoveHandler(EventFillData, value);
            }
        }

        protected void OnDataFilled(EventArgs e)
        {
            InfoDataSetEventHandler handler = Events[EventFillData] as InfoDataSetEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        // End Add

		private System.ComponentModel.Container components = null;
		private System.Data.DataSet fRealDataSet;

        public CurrencyManager myManager = null;
        //private Component fOwnerComp = null;

        // Summary:
        //     Signals the object that initialization is starting.
        private bool fInit = false;
        private bool fInitActive = false;
        private bool fServerModify = false;

        [Browsable(false)]
        public System.Data.DataSet RealDataSet
        {
            get { return fRealDataSet; }
            set { fRealDataSet = value; }
        }

        private CultureInfo locale = CultureInfo.CurrentCulture;

        public CultureInfo Locale 
        {
            get { return locale; }
            set { locale = value; }
        }

        [Category("Infolight"),
        Description("Indicates whether server transfers the lastest date to client automatically after the data in database changes")]
        public bool ServerModify
        {
            get { return fServerModify; }
            set { fServerModify = value; }
        }

        public void BeginInit()
        {
            fInit = true;
        }
        //
        // Summary:
        //     Signals the object that initialization is complete.
        public void EndInit()
        {
            fInit = false;
            if (fInitActive)
                fInitActive = false;
            fRealDataSet.Locale = Locale;
            //OnInitialized
        }

        public DataSet GetRealDataSet()
        {
            return this.fRealDataSet;
        }

        public string[] GetValues(string sKind)
        {
            if (sKind.Equals("RemoteName"))
            {
                string s = EditionDifference.ActiveSolutionName();

                EEPRemoteModule remoteObject = new EEPRemoteModule();
                object[] myRet = remoteObject.GetSqlCommandList(new object[] { (object)"", (object)"", (object)"", (object)"", (object)"", (object)"", (object)s });
                if ((null != myRet))
                {
                    if (0 == (int)(myRet[0]))
                    {
                        string[] sList = (string[])(myRet[1]);
                        return sList;
                    }
                    else
                    {
                        return new string[0] { };
                    }
                } else
                    return new string[0] { };
            }
            else
                return new string[0] { };
        }

        public ArrayList GetKeyFields()
        {
            return GetKeyFields("");
        }

        public ArrayList GetKeyFields(string sDataSetName)
        {
            if (string.IsNullOrEmpty(sDataSetName))
            {
                sDataSetName = this.RealDataSet.Tables[0].TableName;
            }
            ArrayList list = new ArrayList();
            if (this.RealDataSet.Tables.Contains(sDataSetName))
            {
                DataTable table = this.RealDataSet.Tables[sDataSetName];
                foreach (DataColumn column in table.PrimaryKey)
                {
                    list.Add(column.ColumnName);
                }
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, this.GetType(), null, "sDataSetName", sDataSetName);
            }
            return list;
        }

        /*[Browsable(false)]
        public Component OwnerComp
        {
            get
            {
                return fOwnerComp;
            }
            set
            {
                fOwnerComp = value;
                //AUTO GET NEXT PACKET
                if ((null != fOwnerComp) && (RealDataSet.Tables.Count > 0))
                {
                    myManager = (CurrencyManager)((Control)fOwnerComp).BindingContext[this, RealDataSet.Tables[0].TableName];
                    myManager.PositionChanged += new EventHandler(myManager_PositionChanged);
                }
                //AUTO GET NEXT PACKET
            }
        }*/
        
        #region navigator
        [Browsable(false)]
        public int Position
        {
            get
            {
                if (null != myManager)
                    return myManager.Position;
                else
                    return -1;
            }
            set
            {
                if (null != myManager)
                    myManager.Position = value;
            }
        }

        public bool First()
        {
            bool bRet = false;
            if (null != myManager)
            {
                myManager.Position = 0;
                bRet = true;
            }
            return bRet;
        }

        public bool Prior()
        {
            bool bRet = false;
            if (null != myManager)
            {
                myManager.Position--;
                bRet = true;
            }
            return bRet;
        }

        public bool Next()
        {
            bool bRet = false;
            if (null != myManager)
            {
                myManager.Position++;
                bRet = true;
            }
            return bRet;
        }

        public bool Last()
        {
            bool bRet = false;
            if (null != myManager)
            {
                myManager.Position = fRealDataSet.Tables[0].DefaultView.Count - 1;
                bRet = true;
            }
            return bRet;
        }

        public bool MoveBy(int iSkip)
        {
            bool bRet = false;
            if (null != myManager)
            {
                if (iSkip > 0)
                    for (int i = 0; i < iSkip; i++)
                        myManager.Position++;
                else
                    for (int i = 0; i > iSkip; i--)
                        myManager.Position--;
                bRet = true;
            }
            return bRet;
        }
        #endregion navigator

        public enum state
        { Inserted, Edited, Deleted }

        public state[] State(string strTalbeName)
        {
            DataTableCollection Tables = this.fRealDataSet.Tables;
            DataTable tab = null;
            int i = Tables.Count;
            for (int j = 0; j < i; j++)
            {
                if (string.Compare(Tables[j].TableName, strTalbeName, true) == 0)//IgnoreCase
                {
                    tab = Tables[j];
                    break;
                }
            }
            ArrayList states = new ArrayList();
            if (tab != null)
            {
                if (tab.GetChanges(DataRowState.Added) != null)
                {
                    states.Add(state.Inserted);
                }
                if (tab.GetChanges(DataRowState.Deleted) != null)
                {
                    states.Add(state.Deleted);
                }
                if (tab.GetChanges(DataRowState.Modified) != null)
                {
                    states.Add(state.Edited);
                }
            }
            int m = states.Count;
            state[] myState = new state[m];
            for (int n = 0; n < m; n++)
            {
                myState[n] = (state)states[n];
            }
            return myState;
        }

        public bool IsInserted(string TalbeName)
        {
            bool isInserted = false;
            state[] s = this.State(TalbeName);
            int i = s.Length;
            for (int j = 0; j < i; j++)
            {
                if (s[j] == state.Inserted)
                {
                    isInserted = true;
                    break;
                }
            }
            return isInserted;
        }

        public bool IsUpdated(string TalbeName)
        {
            bool isUpdated = false;
            state[] s = this.State(TalbeName);
            int i = s.Length;
            for (int j = 0; j < i; j++)
            {
                if (s[j] == state.Edited)
                {
                    isUpdated = true;
                    break;
                }
            }
            return isUpdated;
        }

        public bool IsDeleted(string TalbeName)
        {
            bool isDeleted = false;
            state[] s = this.State(TalbeName);
            int i = s.Length;
            for (int j = 0; j < i; j++)
            {
                if (s[j] == state.Deleted)
                {
                    isDeleted = true;
                    break;
                }
            }
            return isDeleted;
        }

        public InfoDataSet(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        public InfoDataSet(bool isDesignMode)
        {
            _isDesignMode = isDesignMode;

            InitializeComponent();
        }

		public InfoDataSet()
		{
			///
			/// This call is required by the Windows.Forms Designer.
            /// 
			InitializeComponent();
			///
			/// TODO: Add any constructor code after InitializeComponent call
			///
		}
        [Browsable(false)]
        public bool ContainsListCollection
		{
			get
			{
				return true;
			}
		}

		public IList GetList()
		{
			return ((IListSource)fRealDataSet).GetList();
		}

        private string fRemoteName;
        [Category("Infolight"),
        Description("Remote Name of InfoDataSet"),
        Editor(typeof(RemoteNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return fRemoteName;
            }
            set
            {
                if (!fInit && Active && (!fRemoteName.Trim().Equals(value.Trim())))
                {
                    fRealDataSet.Clear();
                    if (DesignMode)
                    {
                        fRealDataSet.Reset();
                    }
                    Active = false;
                }
                    
                fRemoteName = value;
                if (fInit)
                {
                    Active = fStreamedActive;
                }
            }
        }

        private bool fAlwaysClose = false;
        [Category("Infolight"),
        Description("Indicate whether the data is get when the form loads")]
        public bool AlwaysClose
        {
            get
            {
                return fAlwaysClose;
            }
            set
            {
                fAlwaysClose = value;
            }
        }

        private bool fDeleteIncomplete = true;
        [Category("Infolight")]
        public bool DeleteIncomplete
        {
            get
            {
                return fDeleteIncomplete;
            }
            set
            {
                fDeleteIncomplete = value;
            }
        }

        private object fLastKeyValues = null;
        [Browsable(false)]
        public object LastKeyValues
        {
            get
            {
                return fLastKeyValues;
            }
            set
            {
                fLastKeyValues = value;
            }
        }

        //private bool _supportCurrentMasterRow = false;
        //public bool SupportCurrentMasterRow
        //{
        //    get
        //    {
        //        return _supportCurrentMasterRow;
        //    }
        //    set
        //    {
        //        _supportCurrentMasterRow = value;
        //    }
        //}

        public int LastIndex = -1;
        public string CommandText = "";

        private int fPacketRecords = 100;
        [Category("Infolight"),
        Description("Specifies the amount of data downloaded in each transfer")]
        public int PacketRecords
        {
            get
            {
                return fPacketRecords;
            }
            set
            {
                fPacketRecords = value;
            }
        }

        public bool Eof = false;
        //[Browsable(false)]
        //public bool Eof
        //{
        //    get
        //    {
        //        return fEof;
        //    }
        //    set
        //    {
        //        fEof = value;
        //    }
        //}

        public string WhereStr = "";
        public ArrayList WhereParam = new ArrayList();
        public string OrderStr = string.Empty;
        //private string fWhereStr = "";
        //[Browsable(false)]
        //public string WhereStr
        //{
        //    get
        //    {
        //        return fWhereStr;
        //    }
        //    set
        //    {
        //        fWhereStr = value;
        //    }
        //}

        //private DataRow lastrow = null;
        private bool fActive;
        private bool fStreamedActive = false;
        private bool fInitSetWhere = false;
        [Category("Infolight"),
        Description("Activate InfoDataSet to get data")]
        public bool Active
        {
            get
            {
                return fActive;
            }
            set
            {
                if (fInit && !value)
                    fInitActive = true;

                if ((null != RemoteName) && !RemoteName.Equals(""))
                {
                    if (fInit && !value && !fInitSetWhere)
                    {
                        fInitSetWhere = true;
                        try
                        {
                            try
                            {
                                SetWhere("1 = 0");
                            }
                            catch
                            {
                            }
                            try
                            {
                                fActive = false;
                                //modified by andy 2007/3/28 false -> True ,for when active=false sql will be set one more time
                                Eof = true;
                                // fLastKeyValues = null;
                                LastIndex = -1;
                            }
                            finally
                            {
                                WhereStr = "";
                            }
                        }
                        finally
                        {
                            fInitSetWhere = false;
                        }
                    }
                    else
                    {
                        if (value != fActive)
                        {
                            //modified by ccm 2008/8/7 active属性值变化时，都需要对Eof进行初始化
                            Eof = false;
                            if (!value)
                            {
                                fRealDataSet.Clear();
                                fActive = value;
                                // fLastKeyValues = null;
                                LastIndex = -1;
                            }
                            else
                            {
                                fActive = value;
                                string s = RemoteName;
                                int iPos = s.IndexOf('.');
                                string s1 = s.Substring(0, iPos);
                                string s2 = s.Substring(iPos + 1, s.Length - iPos - 1);
                                DataSet aDataSet;

                                if (!DesignMode && !FWizardDesignMode && !_isDesignMode)
                                {
                                    if (fInit && fAlwaysClose)
                                        WhereStr = "1=0";

                                    aDataSet = CliUtils.GetSqlCommand(s1, s2, this, WhereStr, "", CommandText, WhereParam, OrderStr);
                                    OnDataFilled(new EventArgs());
                                }
                                else
                                {
                                    //if (fAlwaysClose)
                                        WhereStr = "1=0";

                                    //string sCurProject = CliUtils.fCurrentProject;
                                    //if (this.DesignMode || _isDesignMode || FWizardDesignMode)
                                    //{
                                        string sCurProject = EditionDifference.ActiveSolutionName();
                                    //}

                                    aDataSet = CliUtils.GetSqlCommand(s1, s2, this, WhereStr, sCurProject, CommandText, WhereParam, OrderStr);
                                }

                                if (-1 != PacketRecords)
                                {
                                    //DataRow lastrow = null;
                                    if (aDataSet != null && aDataSet.Tables.Count > 0)
                                    {
                                        //if (aDataSet.Tables[0].Rows.Count >= 1)
                                        //{
                                        //    lastrow = aDataSet.Tables[0].Rows[aDataSet.Tables[0].Rows.Count - 1];
                                        //    if (null != aDataSet.Tables[0].PrimaryKey)
                                        //    {
                                        //        DataColumn[] mydc = (DataColumn[])(aDataSet.Tables[0].PrimaryKey);
                                        //        object[] mykeys = new object[mydc.Length];
                                        //        for (int ix = 0; ix < mydc.Length; ix++)
                                        //        {
                                        //            mykeys[ix] = lastrow[mydc[ix]];
                                        //        }

                                        //        if (!DesignMode)
                                        //        {
                                        //            fLastKeyValues = mykeys;
                                        //        }
                                        //    }
                                        //}

                                        if (aDataSet.Tables[0].Rows.Count < PacketRecords)
                                            Eof = true;

                                        LastIndex += aDataSet.Tables[0].Rows.Count;
                                    }
                                }
                                else
                                {
                                    Eof = true;
                                }
                                fRealDataSet.Clear();
                                if (aDataSet != null)
                                {
                                    #region add by Rax 2007/01/10
                                    fRealDataSet.CaseSensitive = true;
                                    #endregion
                                    fRealDataSet.Merge(aDataSet);
                                }
                            }

                            if (fActive && fInitActive)
                                fActive = false;
                        }
                    }
                }
                else
                {
                    fStreamedActive = value;
                }
            }
        }


        private bool dataCompressed;
        [Category("Infolight"),
        Description("Specifies whether compress data")]
        public bool DataCompressed
        {
            get { return dataCompressed; }
            set { dataCompressed = value; }
        }
	

        #region Data_Method

        public bool GetNextPacket()
        {
            if (Eof) return false;
            // if (!Active) return false;
            
            if ((null != RemoteName) && !RemoteName.Equals(""))
            {
                PacketEventArgs e = new PacketEventArgs(PacketEventArgs.PacketState.Before);
                OnNextPacket(e);
                if (e.Cancel)
                {
                    return false;
                }
                string s = RemoteName;
                int iPos = s.IndexOf('.');
                string s1 = s.Substring(0, iPos);
                string s2 = s.Substring(iPos + 1, s.Length - iPos - 1);
                DataSet aDataSet = null;
                if (s1 == "GLModule" && s2 == "cmdRefValUse")
                {
                    aDataSet = (DataSet)(CliUtils.ExecuteSql(s1, s2, refCommandText, refDBAlias, true, CliUtils.fCurrentProject, new object[] { this.PacketRecords, LastIndex }));
                }
                else
                {
                    aDataSet = (DataSet)(CliUtils.GetSqlCommand(s1, s2, this, WhereStr, "", CommandText, WhereParam, OrderStr));//更改过排序后, 取一批也要保持同一排序
                }
                if (-1 != PacketRecords)
                {
                    if (aDataSet.Tables[0].Rows.Count < PacketRecords)
                        Eof = true;

                    LastIndex += aDataSet.Tables[0].Rows.Count;
                }

                fRealDataSet.Merge(aDataSet);
                OnNextPacket(new PacketEventArgs(PacketEventArgs.PacketState.After));
                return true;
            }

            return false;
        }

        public bool GetAllPacket(int records)
        {
            if (records <= 0)
            {
                return false;
            }
            int tempRecords = PacketRecords;
            PacketRecords = records;
            while (!Eof)
            {
                GetNextPacket();
            }
            PacketRecords = tempRecords;
            return true;
        }

        public bool GetAllPacket()
        {
            return GetAllPacket(PacketRecords);
        }

        public bool SetWhere(string strWhere)
        {
            return SetWhere(strWhere, null);
        }

        public bool SetWhere(String strWhere, ArrayList param)
        {
            WhereStr = strWhere;
            OrderStr = string.Empty;
            Active = false;
            LastIndex = -1;
            //if (WhereParam != null)
            //    WhereParam.Clear();
            WhereParam = param;
            Active = true;
            return true;
        }

        public void SetOrder(string strOrder)
        {
            OrderStr = strOrder;
            Active = false;
            LastIndex = -1;
            Active = true;
        }

        public bool ClearWhere()
        {
            return SetWhere("");
        }

        #endregion Data_Method

        #region Component Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            fRealDataSet = new DataSet();
			((System.ComponentModel.ISupportInitialize)(this.fRealDataSet)).BeginInit();
			// 
			// RealDataSet
			// 
			this.fRealDataSet.DataSetName = "NewDataSet";
			((System.ComponentModel.ISupportInitialize)(this.fRealDataSet)).EndInit();
		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

        internal bool RelationsActive = false;
        protected void OnBeforeApplyUpdates(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnBeforeApplyUpdates];
            if (handler != null)
            {
                handler(this, value);
            }

            DataTable mergeTab = new DataTable();
            DataSet mergedataset = new DataSet();
            DataSet ds = new DataSet();
            if (this.Container != null)
            {
                int i = this.Container.Components.Count;
                for (int j = 0; j < i; j++)
                {
                    if (this.Container.Components[j] is InfoBindingSource)
                    {
                        InfoBindingSource bs = (InfoBindingSource)this.Container.Components[j];
                        int m = bs.Relations.Count;
                        for (int n = 0; n < m; n++)
                        {
                            InfoRelation infoRel = bs.Relations[n];
                            ds = this.fRealDataSet;
                            if (infoRel.RelationDataSet.GetRealDataSet() == ds)
                            {
                                if (ds.HasChanges())
                                {
                                    string strTabName = this.RemoteName.Substring(this.RemoteName.IndexOf('.') + 1);
                                    mergeTab = ds.GetChanges().Tables[strTabName];
                                    mergedataset = ds.GetChanges();
                                    if (((InfoDataSet)bs.GetDataSource()).RealDataSet.Tables[0].Rows.Count == 0)
                                    {
                                        bs.Set_fEmptyViewMerge();
                                    }
                                    //2008/8/4 by ccm, 解决最后一笔无法删除的问题，取消DoDelay事件，避免触发使master重新取资料变为unchanged，导致无法删除。
                                    bs.CancelPositionChanged = true;
                                    ((InfoDataSet)bs.GetDataSource()).RealDataSet.Tables[0].Merge(mergeTab);
                                    bs.CancelPositionChanged = false;
                                  
                                    //2008-9-9 Modified by lily 最後一筆master無法删除的問題。View在merge后master的資料會不見，重新merge一次master。
                                    if (mergeTab.Rows.Count > 0 && mergeTab.Rows[0].RowState == DataRowState.Deleted)
                                    {
                                        //2007/01/12 Master-Detail-View因为无法删除最后一笔资料而新增 by Rax
                                        ds.Tables[strTabName].Merge(mergeTab);

                                        foreach (DataTable tb in ds.Tables)
                                        {
                                            if (mergedataset.Tables[tb.TableName] != null)
                                            {
                                                tb.Merge(mergedataset.Tables[tb.TableName]);
                                            }
                                        }
                                        //2007/01/12 end 
                                    }
                                    ArrayList keyFields = this.GetKeyFields();
                                    int p = mergeTab.Rows.Count - 1;
                                    if (p >= 0 && (mergeTab.Rows[p].RowState == DataRowState.Added || mergeTab.Rows[p].RowState == DataRowState.Modified))
                                    {
                                        int x = keyFields.Count;
                                        object[] keyValues = new object[x];
                                        for (int y = 0; y < x; y++)
                                        {
                                            keyValues[y] = mergeTab.Rows[p][keyFields[y].ToString()];
                                        }
                                        DataRow locRow = ((InfoDataSet)bs.GetDataSource()).RealDataSet.Tables[0].Rows.Find(keyValues);
                                        if (locRow != null)
                                        {
                                            int a = bs.List.Count;
                                            for (int b = 0; b < a; b++)
                                            {
                                                if (((DataRowView)bs.List[b]).Row == locRow)
                                                {
                                                    // 2006/08/05 將View BindingSource.Relation.Active設  為False,Find()完後, 再設為True
                                                    if (infoRel.Active)
                                                    {
                                                        RelationsActive = true;
                                                        infoRel.Active = false;
                                                    }
                                                    // 2006/08/05
                                                    
                                                    //2008/9/4 modified by ccm在新增的時候會插入最後一筆，導致取下一批資料
                                                    bs.CancelPositionChanged = true;
                                                    bs.Position = b;
                                                    bs.CancelPositionChanged = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        internal static readonly object EventOnBeforeApplyUpdates = new object();
        public event EventHandler BeforeApplyUpdates
        {
            add { base.Events.AddHandler(EventOnBeforeApplyUpdates, value); }
            remove { base.Events.RemoveHandler(EventOnBeforeApplyUpdates, value); }
        }

        protected void OnAfterApplyUpdates(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterApplyUpdates];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnAfterApplyUpdates = new object();
        public event EventHandler AfterApplyUpdates
        {
            add { base.Events.AddHandler(EventOnAfterApplyUpdates, value); }
            remove { base.Events.RemoveHandler(EventOnAfterApplyUpdates, value); }
        }


        public delegate void ApplyErrorEventHandler(object sender, ApplyErrorEventArgs e);
        internal static readonly object EventOnApplyError = new object();
        /// <summary>
        /// The event ocured when apply data encounter errors
        /// </summary>
        [Category("Infolight"),
        Description("The event ocured when apply data encounter errors")]
        public event ApplyErrorEventHandler ApplyError
        {
            add { base.Events.AddHandler(EventOnApplyError, value); }
            remove { base.Events.RemoveHandler(EventOnApplyError, value); }
        }

        protected void OnApplyError(ApplyErrorEventArgs value)
        {
            ApplyErrorEventHandler handler = (ApplyErrorEventHandler)base.Events[EventOnApplyError];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public delegate void PacketEventHandler(object sender, PacketEventArgs e);
        internal static readonly object EventOnNextPacket = new object();
        /// <summary>
        /// The event ocured when get next packet
        /// </summary>
        [Category("Infolight"),
        Description("The event ocured when get next packet")]
        public event PacketEventHandler NextPacket
        {
            add { base.Events.AddHandler(EventOnNextPacket, value); }
            remove { base.Events.RemoveHandler(EventOnNextPacket, value); }
        }

        protected void OnNextPacket(PacketEventArgs value)
        {
            PacketEventHandler handler = (PacketEventHandler)base.Events[EventOnNextPacket];
            if (handler != null)
            {
                handler(this, value);
            }
        }


        public bool bChkSucess = true;

        public virtual bool ApplyUpdates()
        {
            return ApplyUpdates(true);
        }

        // Modified by yangdong 2006-02-15.
        // 为解决提交不成功，InfoNavigator的状态不对的情况。
        public virtual bool ApplyUpdates(bool NeedToValidate)
        {
            bool blapplysuccess = true;
            if (fRealDataSet.HasChanges())
            {
                try
                {
                    OnBeforeApplyUpdates(new EventArgs());
                    // Add By Chenjian
                    // Validate modified data
                    if (this.Site != null /*&& NeedToValidate*/)
                    {
                        foreach (IComponent comp in this.Site.Container.Components)
                        {
                            if (comp is DefaultValidate)
                            {
                                DefaultValidate validator = comp as DefaultValidate;
                                InfoBindingSource bindingSource = validator.BindingSource;
                                if (bindingSource != null)
                                {
                                    if (bindingSource.GetDataSource() != null && bindingSource.GetDataSource() is InfoDataSet
                                        && (InfoDataSet)bindingSource.GetDataSource() == this)
                                    {
                                        bool isDetail = false;
                                        foreach (DataRelation relation in this.RealDataSet.Relations)
                                        {
                                            if (bindingSource.DataMember == relation.RelationName)
                                            {
                                                isDetail = true;
                                                break;
                                            }
                                        }
                                        if (NeedToValidate || isDetail)
                                        {
                                            for (int i = 0; i < validator.BindingSource.List.Count; ++i)
                                            {
                                                DataRowView rowView = validator.BindingSource.List[i] as DataRowView;

                                                if (rowView.Row.RowState == DataRowState.Modified || rowView.Row.RowState == DataRowState.Added)
                                                {
                                                    bool isInsert = rowView.Row.RowState == DataRowState.Added;
                                                    bool CheckDeplicateSucessful = validator.CheckDuplicate(rowView, isInsert);
                                                    if (!CheckDeplicateSucessful)
                                                    {
                                                        return false;
                                                    }
                                                    if (validator.ValidActive == true)
                                                    {
                                                        bool CheckNullAndRangeSuccessful = validator.CheckNullAndRange(rowView);
                                                        bool ValidateSuccessful = validator.ValidateRow(i, rowView);
                                                        validator.ResetWarnging();
                                                        if (!CheckNullAndRangeSuccessful || !ValidateSuccessful)
                                                        {
                                                            return false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (DeleteIncomplete)
                    {
                        foreach (DataTable table in fRealDataSet.Tables)
                        {
                            ArrayList keys = GetKeyFields(table.TableName);
                            ArrayList rows = new ArrayList();
                            foreach (DataRow row in table.Rows)
                            {
                                if (row.RowState != DataRowState.Added)
                                {
                                    continue;
                                }

                                bool keyIsNull = false;
                                foreach (object obj in keys)
                                {
                                    if (row[(string)obj] == null || row[(string)obj] == DBNull.Value)
                                    {
                                        keyIsNull = true;
                                        break;
                                    }
                                }

                                if (keyIsNull)
                                {
                                    rows.Add(row);
                                }
                            }

                            foreach (DataRow row in rows)
                            {
                                table.Rows.Remove(row);
                            }
                        }
                    }

                    // End Add
                    DataSet custDS = new DataSet();
                    //if (_supportCurrentMasterRow)
                    //{
                    //    custDS.Merge(fRealDataSet);
                    //}
                    //else
                    //{
                        if (fRealDataSet.GetChanges() != null)
                        {
                            custDS.Merge(fRealDataSet.GetChanges());//tables...
                        }
                    //}

                    string s = RemoteName;
                    int iPos = s.IndexOf('.');
                    string s1 = s.Substring(0, iPos);
                    string s2 = s.Substring(iPos + 1, s.Length - iPos - 1);

                    DataSet aDS = new DataSet();
                    if (this.CommandText != null && this.CommandText.Length != 0)
                        aDS = CliUtils.UpdateDataSet(s1, s2, custDS, this.CommandText);
                    else
                        aDS = CliUtils.UpdateDataSet(s1, s2, custDS, "");

                    if (aDS == null || aDS.Tables == null)
                    { return false; }

                 
                    // End Add

                    //if (fServerModify)
                    //{
                    //    if (aDS.Tables[0].Rows.Count != 1 || aDS.Tables[0].Rows[0].RowState != DataRowState.Added)
                    //    { goto Label1; }

                    //    DataRow needModifyRow = fRealDataSet.Tables[0].Rows[0];
                    //    foreach (DataRow r in fRealDataSet.Tables[0].Rows)
                    //    {
                    //        if (r.RowState == DataRowState.Added)
                    //        { needModifyRow = r; }
                    //    }

                    //    foreach (DataColumn c in needModifyRow.Table.Columns)
                    //    {
                    //        needModifyRow[c.ColumnName] = aDS.Tables[0].Rows[0][c.ColumnName];
                    //    }
                    //}
                    InfoBindingSource ibsview = null;
                    InfoBindingSource ibsmaster = null;
                    if (this.Container != null)
                    {
                        foreach (IComponent comp in this.Container.Components)
                        {
                            if (comp is InfoBindingSource)
                            {
                                InfoBindingSource ibs = comp as InfoBindingSource;
                                foreach (InfoRelation relation in ibs.Relations)
                                {
                                    if (relation.RelationDataSet == this)// find view
                                    {
                                        ibsview = ibs;
                                        break;
                                    }
                                }
                                if (ibs.DataSource == this)
                                {
                                    ibsmaster = ibs;
                                }
                            }
                        }
                    }

                    if (fServerModify)
                    {
                        int m = 0;
                        foreach (DataRow row in aDS.Tables[0].Rows)
                        {
                            if (row.RowState == DataRowState.Added)
                            {
                                break;
                            }
                            m++;
                        }
                        if (m < aDS.Tables[0].Rows.Count)
                        {
                            DataRow needModifyRow = fRealDataSet.Tables[0].Rows[m];
                            foreach (DataRow r in fRealDataSet.Tables[0].Rows)
                            {
                                if (r.RowState == DataRowState.Added)
                                { needModifyRow = r; }
                            }
                            /*挂起绑定,减少更改值需要的时间*/
                            int postion = 0;
                            if (ibsmaster != null && ibsmaster.ServerModifyCache)
                            {
                                postion = ibsmaster.Position;
                                ibsmaster.CancelPositionChanged = true;
                                ibsmaster.SuspendBinding();
                            }
                            /******************************/

                            foreach (DataColumn c in needModifyRow.Table.Columns)
                            {
                                if (!c.ReadOnly)
                                {
                                    needModifyRow[c.ColumnName] = aDS.Tables[0].Rows[m][c.ColumnName];
                                }
                            }

                            if (ibsmaster != null && ibsmaster.ServerModifyCache)
                            {
                                ibsmaster.ResumeBinding();
                                ibsmaster.Position = postion;
                                ibsmaster.CancelPositionChanged = false;
                            }

                            if (ibsview != null)
                            {
                                DataSet realDs = ((InfoDataSet)ibsview.GetDataSource()).RealDataSet;
                                DataSet ds = realDs.GetChanges(DataRowState.Added);
                                if (ds != null)
                                {
                                    DataColumn[] cs = realDs.Tables[0].PrimaryKey;
                                    DataRow row = ds.Tables[0].Rows[0];

                                    if (cs.Length != 0)
                                    {
                                        List<object> os = new List<object>();
                                        foreach (DataColumn c in cs)
                                        {
                                            os.Add(row[c.ColumnName]);
                                        }

                                        DataRow replaceRow = realDs.Tables[0].Rows.Find(os.ToArray());
                                        foreach (DataColumn c in needModifyRow.Table.Columns)
                                        {
                                            if (replaceRow.Table.Columns.Contains(c.ColumnName) && !c.ReadOnly)
                                            {
                                                replaceRow[c.ColumnName] = needModifyRow[c.ColumnName];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (ibsview != null)
                    {
                        (ibsview.DataSource as InfoDataSet).RealDataSet.AcceptChanges();
                        ibsview.Reset_fInEdit();
                    }
                   
                    fRealDataSet.AcceptChanges();
                    OnAfterApplyUpdates(new EventArgs());
                }
                catch (Exception e)
                {
                    if (fRealDataSet.GetChanges(DataRowState.Deleted) !=null 
                        && fRealDataSet.GetChanges(DataRowState.Deleted).Tables[0].Rows.Count > 0 && this.Container != null)
                    {
                        bool autoapply = false;
                        InfoBindingSource ibsview = null;
                        if (this.Container != null)
                        {
                            foreach (IComponent comp in this.Container.Components)
                            {
                                if (comp is InfoBindingSource)
                                {
                                    InfoBindingSource ibs = comp as InfoBindingSource;
                                    if (ibs.DataSource == this) //find bindingsource of master
                                    {
                                        if (ibs.AutoApply)
                                        {
                                            autoapply = true;
                                            continue;
                                        }
                                        else
                                        {
                                            autoapply = false;
                                            break;
                                        }
                                    }
                                    foreach (InfoRelation relation in ibs.Relations)
                                    {
                                        if (relation.RelationDataSet == this)// find view
                                        {
                                            ibsview = ibs;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (autoapply)
                            {
                                fRealDataSet.RejectChanges();
                                if (ibsview != null)
                                {
                                    (ibsview.DataSource as InfoDataSet).RealDataSet.RejectChanges();
                                    //clear edit flag...
                                    ibsview.Reset_fInEdit();
                                }
                            }
                        }
                    }
                    ApplyErrorEventArgs args = new ApplyErrorEventArgs(e);
                    OnApplyError(args);
                    blapplysuccess = false;
                    if (!args.Cancel)
                    {
                        throw e;
                    }
                }
                finally
                {
                    if (this.Container != null)
                    {
                        foreach (IComponent comp in this.Container.Components)
                        {
                            if (comp is InfoBindingSource)
                            {
                                InfoBindingSource bs = (InfoBindingSource)comp;
                                DataSet ds = new DataSet();
                                int m = bs.Relations.Count;
                                for (int n = 0; n < m; n++)
                                {
                                    InfoRelation infoRel = bs.Relations[n];
                                    ds = this.fRealDataSet;
                                    if (infoRel.RelationDataSet.GetRealDataSet() == ds)
                                    {
                                        if (!infoRel.Active)
                                        {
                                            infoRel.Active = this.RelationsActive;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return blapplysuccess;
        }

        //private String Quote(String table_or_column)
        //{
        //    if (_quotePrefix == null || _quoteSuffix == null)
        //        return table_or_column;
        //    return _quotePrefix + table_or_column + _quoteSuffix;
        //}

        //private Object TransformMarkerInColumnValue(String typeName, Object columnValue)
        //{
        //    if (Type.GetType(typeName).Equals(typeof(Char)) || Type.GetType(typeName).Equals(typeof(String)))
        //    {
                //StringBuilder sb = new StringBuilder();
        //        if (columnValue.ToString().Length > 0)
        //        {
        //            Char[] cVChars = columnValue.ToString().ToCharArray();

        //            foreach (Char cVChar in cVChars)
        //            {
        //                if (cVChar == _marker)
        //                { sb.Append(cVChar.ToString()); }

        //                sb.Append(cVChar.ToString());
        //            }
        //        }
        //        return sb.ToString();
        //    }
        //    else
        //    { return columnValue; }
        //}

        //private Char _marker = '\'';
        //private String Mark(String typeName, Object columnValue)
        //{
        //    if (Type.GetType(typeName).Equals(typeof(Char)) || Type.GetType(typeName).Equals(typeof(String))
        //        || Type.GetType(typeName).Equals(typeof(DateTime)))
        //    {
        //        return _marker.ToString() + columnValue.ToString() + _marker.ToString();
        //    }
        //    else if (Type.GetType(typeName).Equals(typeof(Byte[])))
        //    {
        //        StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
        //        foreach (Byte b in (Byte[])columnValue)
        //        { builder.Append(Convert.ToString(b, 16)); }
        //        return builder.ToString();
        //    }
        //    else
        //    {
        //        return columnValue.ToString();
        //    }
        //}

        private bool FWizardDesignMode = false;
        public void SetWizardDesignMode(bool value)
        {
            FWizardDesignMode = value;
        }

        public DataSet Execute(string sCommandText)
        {
            return Execute(sCommandText, false);
        }

        public DataSet Execute(string sCommandText, ArrayList paramWhere)
        {
            return Execute(sCommandText, null, false, paramWhere);
        }

        private string refCommandText;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RefCommandText
        {
            get { return refCommandText; }
            set { refCommandText = value; }
        }

        private string refDBAlias;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RefDBAlias
        {
            get { return refDBAlias; }
            set { refDBAlias = value; }
        }
	
        public DataSet Execute(string sCommandText, string DBAlias, bool isReplace)
        {
            return Execute(sCommandText, DBAlias, isReplace, null);
        }

        public DataSet Execute(String sCommandText, String DBAlias, bool isReplace, ArrayList paramWhere)
        {
            DataSet dsRet = null;

            int iPos = RemoteName.IndexOf('.');
            if (iPos > 0)
            {
                string sModule = RemoteName.Substring(0, iPos);
                string sDataSet = RemoteName.Substring(iPos + 1);
                if (isReplace)                                  //替换时才记录
                {
                    refCommandText = sCommandText;
                    refDBAlias = DBAlias;
                }
                LastIndex = -1;
                Eof = false;
                dsRet = CliUtils.ExecuteSql(sModule, sDataSet, sCommandText, DBAlias, true, CliUtils.fCurrentProject, new object[2] { this.PacketRecords, LastIndex }, paramWhere);
                LastIndex += this.PacketRecords;
            }

            if (isReplace)
            {
                fRealDataSet.Clear();
                fRealDataSet.Merge(dsRet);
                fRealDataSet.AcceptChanges();
            }

            return dsRet;
        }

        public int GetRecordsCount(string strWhere)
        {
            int i = 0;
            int iPos = RemoteName.IndexOf('.');
            if (iPos > 0)
            {
                string sModule = RemoteName.Substring(0, iPos);
                string sDataSet = RemoteName.Substring(iPos + 1);
                i = CliUtils.GetRecordsCount(sModule, sDataSet,strWhere, CliUtils.fCurrentProject);
            }

            return i;
        }

        public int GetRecordsCount()
        {
            return GetRecordsCount("");
        }

        public DataSet Execute(string sCommandText, bool isReplace)
        {
            return Execute(sCommandText, null, isReplace);
        }

        public bool ToExcel(int tableIndex, string xLSFileName)
        {
            return ToExcel(tableIndex, xLSFileName, string.Empty, string.Empty, string.Empty);
        }

        public bool ToExcel(int tableIndex, string xLSFileName, string Title)
        {
            return ToExcel(tableIndex, xLSFileName, Title, string.Empty, string.Empty);
        }

        public bool ToExcel(int tableIndex, string xLSFileName, string Title, string Filter)
        {
            return ToExcel(tableIndex, xLSFileName, Title, Filter, string.Empty);
        }

        public bool ToExcel(int tableIndex, string xLSFileName, string Title, string Filter, string Sort)
        {
            return ToExcel(tableIndex, xLSFileName, Title, Filter, Sort, null);
        }

        public bool ToExcel(int tableIndex, string xLSFileName, string Title, string Filter, string Sort, List<string> IgnoreColumns)
        {
            bool rtn = false;

            DataSetToExcel dte = new DataSetToExcel();
            dte.DataSet = this;
            dte.Excelpath = xLSFileName;
            dte.Title = Title;
            dte.Filter = Filter;
            dte.Sort = Sort;
            dte.IgnoreColumns = IgnoreColumns;
            if (tableIndex >= 0 && tableIndex < this.RealDataSet.Tables.Count)
            {
                rtn = dte.Export(tableIndex);
            }
            else
            {
                rtn = false;
            }

            return rtn;
        }

        public bool ToExcel(string tableName, string xLSFileName)
        {
            return ToExcel(tableName, xLSFileName, string.Empty, string.Empty, string.Empty);
        }

        public bool ToExcel(string tableName, string xLSFileName, string Title)
        {
            return ToExcel(tableName, xLSFileName, Title, string.Empty, string.Empty);
        }

        public bool ToExcel(string tableName, string xLSFileName, string Title, string Filter)
        {
            return ToExcel(tableName, xLSFileName, Title, Filter, string.Empty);
        }

        public bool ToExcel(string tableName, string xLSFileName, string Title, string Filter, string Sort)
        {
            return ToExcel(tableName, xLSFileName, Title, Filter, string.Empty, null);
        }

        public bool ToExcel(string tableName, string xLSFileName, string Title, string Filter, string Sort, List<string> IgnoreColumns)
        {
            bool rtn = false;

            DataSetToExcel dte = new DataSetToExcel();
            dte.DataSet = this;
            dte.Excelpath = xLSFileName;
            dte.Title = Title;
            dte.Filter = Filter;
            dte.Sort = Sort;
            dte.IgnoreColumns = IgnoreColumns;
            if (this.RealDataSet.Tables.Contains(tableName))
            {
                rtn = dte.Export(tableName);
            }
            else
            {
                rtn = false;
            }

            return rtn;
        }

        public bool ToCSV(int tableIndex, string xLSFileName)
        {
            bool rtn = false;

            DataSetToExcel dte = new DataSetToExcel();
            dte.DataSet = this;
            dte.Excelpath = xLSFileName;
            if (tableIndex >= 0 && tableIndex < this.RealDataSet.Tables.Count)
            {
                rtn = dte.ExportCSV(tableIndex);
            }
            else
            {
                rtn = false;
            }

            return rtn;
        }

        public bool ToCSV(string tableName, string xLSFileName)
        {
            bool rtn = false;

            DataSetToExcel dte = new DataSetToExcel();
            dte.DataSet = this;
            dte.Excelpath = xLSFileName;
            if (this.RealDataSet.Tables.Contains(tableName))
            {
                rtn = dte.ExportCSV(tableName);
            }
            else
            {
                rtn = false;
            }

            return rtn;
        }

        public void ReadFromTxt(int tableIndex, string txtFileName)
        {
            DataTable table = this.RealDataSet.Tables[tableIndex];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Txt);
            reader.Read(txtFileName,0,0);
        }

        public void ReadFromTxt(string tableName, string txtFileName)
        {
            DataTable table = this.RealDataSet.Tables[tableName];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Txt);
            reader.Read(txtFileName,0,0);
        }

        public void ReadFromXls(int tableIndex, string xlsFileName)
        {
            ReadFromXls(tableIndex, xlsFileName, 0, 0);
        }

        public void ReadFromXls(int tableIndex, string xlsFileName, int beginrow, int begincell)
        {
            DataTable table = this.RealDataSet.Tables[tableIndex];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Xls);
            reader.Read(xlsFileName, beginrow, begincell);
        }

        public void ReadFromXls(string tableName, string xlsFileName)
        {
            ReadFromXls(tableName, xlsFileName, 0, 0);
        }

        public void ReadFromXls(string tableName, string xlsFileName, int beginrow, int begincell)
        {
            DataTable table = this.RealDataSet.Tables[tableName];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Xls);
            reader.Read(xlsFileName, beginrow, begincell);
        }

        public void GetERModel(Graphics graph)
        {
            ERModel model = new ERModel(this.fRealDataSet);
            model.Graph = graph;
            model.Paint();
        }

        public void GetERModel(string filename)
        {
            GetERModel(filename, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public void GetERModel(string filename, System.Drawing.Imaging.ImageFormat format)
        {
            GetERModel(filename, System.Drawing.Imaging.ImageFormat.Bmp, Size.Empty);
        }

        public void GetERModel(string filename, System.Drawing.Imaging.ImageFormat format, Size size)
        {
            ERModel model = new ERModel(this.fRealDataSet);
            Size sizeGraph = Size.Ceiling(model.GraphSize);
            Bitmap bmp = new Bitmap(Math.Max(sizeGraph.Width, size.Width), Math.Max(sizeGraph.Height, size.Height));
            Graphics graph = Graphics.FromImage(bmp);
            model.Graph = graph;
            model.Paint();
            bmp.Save(filename, format);
        }

        public static void GetERModel(List<DataSet> list, Graphics graph)
        {
            ERModel model = new ERModel(list);
            model.Graph = graph;
            model.Paint();
        }

        public static void GetERModel(List<DataSet> list, string filename)
        {
            GetERModel(list, filename, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public static void GetERModel(List<DataSet> list, string filename, System.Drawing.Imaging.ImageFormat format)
        {
            GetERModel(list, filename, System.Drawing.Imaging.ImageFormat.Bmp, Size.Empty);
        }

        public static void GetERModel(List<DataSet> list, string filename, System.Drawing.Imaging.ImageFormat format, Size size)
        {
            ERModel model = new ERModel(list);
            Size sizeGraph = Size.Ceiling(model.GraphSize);
            Bitmap bmp = new Bitmap(Math.Max(sizeGraph.Width, size.Width), Math.Max(sizeGraph.Height, size.Height));
            Graphics graph = Graphics.FromImage(bmp);
            model.Graph = graph;
            model.Paint();
            bmp.Save(filename, format);
        }
    }

    public sealed class ApplyErrorEventArgs : EventArgs
    {
        public ApplyErrorEventArgs(Exception e)
        {
            _Exception = e;
            _Cancel = false;
        }

        private Exception _Exception;
        /// <summary>
        /// The exception throw during the apply process
        /// </summary>
        public Exception Exception
        {
            get { return _Exception; }
        }

        private bool _Cancel;
        /// <summary>
        /// The value indicates whether to throw the exception ,false to throw
        /// </summary>
        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }
    }

    public sealed class PacketEventArgs: EventArgs
    {
        public PacketEventArgs(PacketState state)
        {
            _State = state;
            _Cancel = false;
        }

        private PacketState _State;
	    public PacketState State
	    {
		    get { return _State;}
	    }
    
        public enum PacketState
        {
            Before,
            After
        }

        private bool _Cancel;

        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }
	
    }

    public class RemoteNameEditor : System.Drawing.Design.UITypeEditor
    {
        public RemoteNameEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                RemoteNameSelector mySelector = new RemoteNameSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }

            return value;
        }
    }
}
