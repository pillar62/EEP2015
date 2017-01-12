using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.IO;

namespace Srvtools
{
    #region INFO_BINDINGSOURCE
    [ToolboxBitmap(typeof(UpdateComponent), "Resources.InfoBindingSource.ico")]
    public class InfoBindingSource : BindingSource, IFindContainer
    {
        Dictionary<string, bool> relationActives = new Dictionary<string, bool>();

        private SYS_LANGUAGE language;
        // Add By Chenjian 2006-01-09
        private object EventEditBegining = new object();
        internal event EventHandler EditBeginning
        {
            add
            {
                Events.AddHandler(EventEditBegining, value);
            }
            remove
            {
                Events.RemoveHandler(EventEditBegining, value);
            }
        }

        internal void OnBeginEdit(EventArgs e)
        {
            EventHandler handler = Events[EventEditBegining] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private string _text;
        [Browsable(false)]
        public string text
        {
            get
            {
                return _text;
            }
            set
            {
                if (this.DesignMode)
                    value = this.Site.Name;
                _text = value;
            }
        }

        private bool _autorecordlock;
        [Category("Infolight"),
        Description("Specifies data lock to prevent from modifying by two different users")]
        public bool AutoRecordLock
        {
            get
            {
                return _autorecordlock;
            }
            set
            {
                if (value == true)
                {
                    _AutoDisibleControl = true;
                }
                _autorecordlock = value;
            }
        }

        public enum LockMode
        {
            ReLoad,
            NoneReload
        }

        private LockMode _autorecordlockmode;
        [Category("Infolight"),
        Description("Specifies the behaviour before modify data")]
        public LockMode AutoRecordLockMode
        {
            get
            {
                return _autorecordlockmode;
            }
            set
            {
                _autorecordlockmode = value;
            }
        }

        /// <summary>
        /// The flag indicates whether the bindingSource is being edited
        /// </summary>
        [Browsable(false)]
        public bool isEdited
        {
            get
            {
                return bEdited;
            }
        }

        private bool bEdited = false;
        public bool BeginEdit()
        {
            //if (AutoRecordLock && !IsAdd)
            //{
            //    if (!AddLock("Updating"))
            //    {
            //        return false;
            //    }
            //}
            //modified by lily 2007/4/2 解决如果query沒有資料，沒有按insert直接編輯存檔資料會報錯。
            if (this.Current == null)
                return false;
            if (!this.AllowUpdate)
            {
                DataRowView rowview = this.Current as DataRowView;
                if (rowview != null && (!rowview.IsNew && rowview.Row.RowState != DataRowState.Added))
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoBindingSource", "rightToUpdate");
                    MessageBox.Show(message);
                    return false;
                }
            }

            OnBeginEdit(new EventArgs());
            if (!bEdited)
            {
                OnBeforeEdit(new EventArgs());
                bEdited = true;
            }
            InfoBindingSource bindingSource = this.DataSource as InfoBindingSource;
            if (bindingSource != null)
            {
                bindingSource.BeginEdit();
            }
            return true;
        }

        public string Tablename = "";
        public string SQLText = "";
        private string strTableName = "";
        public string KeyFields = "";
        public ArrayList KeyValues = new ArrayList();
        public bool AddLock(string modifytype)
        {
            if (Tablename == "" || KeyFields == "" || SQLText == "")
            {

                string strModuleName = "";
                if (this.DataSource is InfoDataSet)
                {
                    strModuleName = ((InfoDataSet)this.DataSource).RemoteName.Substring(0, ((InfoDataSet)this.DataSource).RemoteName.IndexOf('.'));
                    strTableName = ((InfoDataSet)this.DataSource).RemoteName.Substring(((InfoDataSet)this.DataSource).RemoteName.IndexOf('.') + 1);
                }
                else
                {
                    InfoDataSet ids = this.GetDataSource() as InfoDataSet;
                    for (int i = 0; i < ids.RealDataSet.Relations.Count; i++)
                    {
                        strModuleName = ((InfoDataSet)this.GetDataSource()).RemoteName.Substring(0, ((InfoDataSet)this.GetDataSource()).RemoteName.IndexOf('.'));
                        if (ids.RealDataSet.Relations[i].RelationName == this.DataMember)
                        {
                            strTableName = ids.RealDataSet.Relations[i].ChildTable.TableName;
                        }
                    }

                }
                Tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
                SQLText = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
                DataTable table = (this.GetDataSource() as InfoDataSet).RealDataSet.Tables[strTableName];
                foreach (DataColumn key in table.PrimaryKey)
                {
                    KeyFields += key.ColumnName + ";";
                }
                if (KeyFields != "")
                {
                    KeyFields = KeyFields.Substring(0, KeyFields.LastIndexOf(";"));
                }
                else
                {
                    MessageBox.Show("no key avaliable");
                }
            }
            string[] arrkeyfields = KeyFields.Split(';');
            string keyvalue = "";
            foreach (string str in arrkeyfields)
            {
                if (this.Current != null)
                    keyvalue += ((DataRowView)this.Current).Row[str].ToString() + ";";
            }
            if (keyvalue != "")
            {
                keyvalue = keyvalue.Substring(0, keyvalue.LastIndexOf(";"));
            }
            if (KeyValues.Contains(keyvalue)) //data has locked
            {
                return true;
            }

            object[] retval = CliUtils.CallMethod("GLModule", "DoRecordLock", new object[] { CliUtils.fLoginDB, Tablename, KeyFields, keyvalue, CliUtils.fLoginUser, modifytype, SQLText, this.AutoRecordLockMode.ToString() });
            if (retval != null && (int)retval[0] == 0)
            {
                if ((int)retval[1] == 0)
                {
                    if (this.AutoRecordLockMode == LockMode.ReLoad)
                    {
                        //get latest data
                        string[] arrkeyvalues = keyvalue.Split(';');
                        InfoDataSet ids = this.GetDataSource() as InfoDataSet;
                        DataSet dsnew = ((DataSet)retval[2]);
                        int count = ids.RealDataSet.Tables[strTableName].Rows.Count;
                        if (dsnew.Tables[0].Rows.Count == 0)
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetServer", "RecordLock", "DataDeleted");
                            MessageBox.Show(message);
                            return false;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            bool equal = true;
                            for (int j = 0; j < arrkeyfields.Length; j++)
                            {
                                if (ids.RealDataSet.Tables[strTableName].Rows[i][arrkeyfields[j]].ToString() != arrkeyvalues[j])
                                {
                                    equal = false;
                                    break;
                                }
                            }
                            if (equal)
                            {
                                foreach (DataColumn dc in dsnew.Tables[0].Columns)
                                {
                                    if (ids.RealDataSet.Tables[strTableName].Columns.Contains(dc.ColumnName))
                                    {
                                        ids.RealDataSet.Tables[strTableName].Rows[i][dc.ColumnName] = dsnew.Tables[0].Rows[0][dc.ColumnName];
                                    }
                                }
                                ids.RealDataSet.AcceptChanges();
                                break;
                            }
                        }
                    }
                    if (!KeyValues.Contains(keyvalue))
                    {
                        KeyValues.Add(keyvalue);
                    }
                    return true;
                }
                else
                {
                    string message = "";
                    if (retval[2].ToString() == "Updating")
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetServer", "RecordLock", "DataUpdating");
                    }
                    else if (retval[2].ToString() == "Deleting")
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetServer", "RecordLock", "DataDeleting");
                    }
                    else
                    {
                        message = "Unknown Error";
                    }
                    MessageBox.Show(message);
                    return false;
                }
            }
            return false;
        }

        public void RemoveLock()
        {
            CliUtils.CallMethod("GLModule", "DoRecordLock", new object[] { CliUtils.fLoginDB, Tablename, KeyFields, KeyValues, CliUtils.fLoginUser, "Release" });
            KeyValues.Clear();
        }

        private object EventChanged = new object();
        internal event EventHandler Changed
        {
            add
            {
                Events.AddHandler(EventChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventChanged, value);
            }
        }

        internal void OnChanged(EventArgs e)
        {
            EventHandler handler = Events[EventChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void BeginChange()
        {
            OnChanged(new EventArgs());
        }
        // End Add

        private bool fInDelay = false;

        public InfoBindingSource(System.ComponentModel.IContainer container)
            : this()
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
        }

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            base.OnAddingNew(e);

        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            if (bEdited)
            {
                OnAfterEdit(new EventArgs());
            }
            bEdited = false;
        }

        bool keyFlag = false;

        new public void EndEdit()
        {
            try
            {
                base.EndEdit();
            }
            catch { }
            bEdited = false;
        }

        new public void CancelEdit()
        {
            //if (AutoRecordLock)
            //{
            //    RemoveLock();
            //}
            base.CancelEdit();
            OnCanceledEdit(new EventArgs());
            bEdited = false;
        }

        private object EventCanceledEdit = new object();
        internal event EventHandler CanceledEdit
        {
            add
            {
                Events.AddHandler(EventCanceledEdit, value);
            }
            remove
            {
                Events.RemoveHandler(EventCanceledEdit, value);
            }
        }

        protected void OnCanceledEdit(EventArgs e)
        {
            EventHandler handler = Events[EventCanceledEdit] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private Control _FocusedControl;
        [Category("Infolight"),
        Description("Specifies the control to focus when InfobindingSource loads")]
        public Control FocusedControl
        {
            get
            {
                return _FocusedControl;
            }
            set
            {
                _FocusedControl = value;
            }
        }

        private bool _DisableKeyFields = false;
        [Category("Infolight")]
        public bool DisableKeyFields
        {
            get
            {
                return _DisableKeyFields;
            }
            set
            {
                _DisableKeyFields = value;
            }
        }

        private InfoNavigator navigator;
        /// <summary>
        /// 取出绑定的navigator, closeprotect和addnew时用
        /// </summary>
        private InfoNavigator Navigator
        {
            get
            {
                if (navigator == null)
                {
                    if (OwnerComp != null && OwnerComp is Form)
                    {
                        Type type = ((Form)OwnerComp).GetType();
                        FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                            | BindingFlags.DeclaredOnly);
                        foreach (FieldInfo fi in fis)
                        {
                            if (fi.FieldType == typeof(InfoNavigator))
                            {
                                InfoNavigator nav = fi.GetValue(OwnerComp) as InfoNavigator;
                                if (nav.BindingSource != null && nav.BindingSource.GetDataSource() == this.GetDataSource())
                                {
                                    navigator = nav;
                                    break;
                                }
                            }
                        }
                    }
                }
                return navigator;
            }
        }


        private ArrayList keyCtrlList = new ArrayList();
        public override object AddNew()
        {
            if (!this.AllowAdd)
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoBindingSource", "rightToAdd");
                MessageBox.Show(message);

                InfoNavigator nav = Navigator;
                if (nav != null)
                {
                    nav.Focus();
                }
                return null;
            }
            if (this.DataSource is InfoBindingSource && this.AutoApplyMaster)
            {
                object obj = this.GetDataSource();
                if (obj is InfoDataSet)
                {
                    InfoDataSet ds = (InfoDataSet)obj;
                    int i = ds.RealDataSet.Relations.Count;
                    for (int j = 0; j < i; j++)
                    {
                        if (ds.RealDataSet.Relations[j].RelationName == this.DataMember
                            && this.List.Count == 0
                            && ds.IsInserted(ds.RealDataSet.Relations[j].ParentTable.TableName))
                        {
                            if (!ds.ApplyUpdates())
                            {
                                InfoNavigator nav = Navigator;
                                if (nav != null)
                                {
                                    nav.Focus();
                                }
                                return null;
                            }
                        }
                    }
                }
            }
            if (this.DisableKeyFields && !this.AutoDisableControl)
            {
                keyFlag = false;
                SetKeyCtrlsReadOnly(true);
            }
            if (this.FocusedControl != null)
            {
                FocusedControl.Focus();
            }

            object result = base.AddNew();
            bEdited = true;
            return result;
        }

        private void SetKeyCtrlsReadOnly(bool Enabled)
        {
            //if (keyCtrlList.Count == 0)
            //{
            //    AllControlsList.Clear();
            //    if (this.OwnerComp != null)
            //    {
            //        GetAllControls(((Form)this.OwnerComp).Controls);
            //    }

            //    foreach (Control ctrl in AllControlsList)
            //    {
            //        foreach (Binding binding in ctrl.DataBindings)
            //        {
            //            if (binding.DataSource == this && IsKeyField(binding.BindingMemberInfo.BindingField))
            //            {
            //                keyCtrlList.Add(ctrl);
            //                break;
            //            }
            //        }
            //        if (ctrl is InfoDataGridView && ((InfoDataGridView)ctrl).GetDataSource() == this.GetDataSource())
            //        {
            //            keyCtrlList.Add(ctrl);
            //        }
            //    }
            //}
            foreach (Control ctrl in keyCtrlList)
            {
                if (ctrl.GetType().BaseType == typeof(DataGridView))
                {
                    DataGridView dgView = (DataGridView)ctrl;
                    foreach (DataGridViewColumn column in dgView.Columns)
                    {
                        if (IsKeyField(column.DataPropertyName))
                            column.ReadOnly = !Enabled;
                    }
                }
                else
                {
                    if (this.AutoDisableStyle == AutoDisableStyleType.ReadOnly)
                    {
                        PropertyInfo pi = ctrl.GetType().GetProperty("ReadOnly");
                        if (pi != null)
                        {
                            pi.SetValue(ctrl, !Enabled, null);
                            continue;
                        }
                    }
                    ctrl.Enabled = Enabled;
                }
            }
        }

        private bool IsKeyField(string FieldName)
        {
            InfoDataSet infoDs = (InfoDataSet)this.GetDataSource();
            DataTable table = infoDs.RealDataSet.Tables[0];//现在只管主档
            foreach (DataColumn key in table.PrimaryKey)
            {
                if (key.ColumnName == FieldName)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Remove(object value)
        {
            if (!this.AllowDelete)
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoBindingSource", "rightToDelete");
                MessageBox.Show(message);
                return;
            }
            base.Remove(value);
        }

        public override void RemoveAt(int index)
        {
            if (!this.AllowDelete)
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoBindingSource", "rightToDelete");
                MessageBox.Show(message);
                return;
            }
            base.RemoveAt(index);
        }

        public bool bChk = false;
        public bool CheckSucess = true;
        public bool bDeplChk = false;
        public bool CheckDeplicateSucess = true;
        public bool InEndEdit(bool EndEditRowByRow)
        {
            bool bRet = true;
            DefaultValidate validator = null;
            foreach (IComponent comp in this.Site.Container.Components)
            {
                if (comp is DefaultValidate)
                {
                    if (((DefaultValidate)comp).BindingSource == this)
                    {
                        validator = (DefaultValidate)comp;
                        break;
                    }
                }
            }
            if (validator != null)
            {
                // 在此做DepCheckMode.ByLocal的代码
                if (validator.DuplicateCheck || validator.CheckKeyFieldEmpty)
                {
                    //if (validator.BindingSource != this)
                    //    ((DataRowView)validator.BindingSource.Current).BeginEdit();
                    DataRowView rowView = ((DataRowView)this.Current);
                    if (rowView != null)
                    {
                        rowView.BeginEdit();
                        try
                        {
                            //if (validator.BindingSource != this)
                            //    ((DataRowView)validator.BindingSource.Current).EndEdit();
                            string sort = null;
                            if (!string.IsNullOrEmpty(this.Sort))
                            {
                                sort = this.Sort;
                                this.Sort = string.Empty;
                            }
                            rowView.EndEdit();
                            if (sort != null)
                            {
                                this.Sort = sort;
                            }
                        }
                        catch (NoNullAllowedException)
                        {
                            DataSet dataset = DBUtils.GetDataDictionary(this, false);
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

                            //language = CliUtils.fClientLang;
                            //String message = e.Message;
                            //MessageBox.Show(string.Format(message, this.KeyFields));
                            return false;
                        }
                        catch
                        {
                            //language = CliSysMegLag.GetClientLanguage();
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "DefaultValidate", "DeplicateWarning");
                            MessageBox.Show(message);
                            return false;
                        }
                    }
                }
                base.EndEdit();
                if (!CheckDeplicateSucess)
                {
                    return false;
                }
                //DepCheckMode.ByWhere
                if (!bDeplChk)
                {
                    foreach (object obj in this.List)
                    {
                        DataRowView rowView = (DataRowView)obj;
                        if (rowView.Row.RowState == DataRowState.Added || rowView.Row.RowState == DataRowState.Modified)
                        {
                            bool isInsert = (rowView.Row.RowState == DataRowState.Added);
                            if (!validator.CheckDuplicate(rowView, isInsert))
                            {
                                return false;
                            }
                        }
                    }
                }

                bRet = this.CheckSucess;
                if (this.Site != null && !EndEditRowByRow) // EndEditRowByRow只是当前是否为autoapply,如果是则不必执行validate,会在beforeapply中去做validate
                {
                    if (validator.ValidActive == true)
                    {

                        for (int i = 0; i < this.List.Count; i++)
                        {
                            DataRowView rowView = this.List[i] as DataRowView;

                            if (rowView != null && !bChk && (rowView.Row.RowState == DataRowState.Modified || rowView.Row.RowState == DataRowState.Added))
                            {
                                bool CheckNullAndRangeSuccessful = validator.CheckNullAndRange(rowView);
                                bool ValidateSuccessful = validator.ValidateRow(i, rowView);
                                validator.ResetWarnging();
                                if (!CheckNullAndRangeSuccessful || !ValidateSuccessful)
                                {
                                    if (AutoApply)
                                    {
                                        int p = ((Form)this.OwnerComp).Controls.Count;
                                        for (int q = 0; q < p; q++)
                                        {
                                            if (((Form)this.OwnerComp).Controls[q] is InfoNavigator
                                                && ((InfoNavigator)((Form)this.OwnerComp).Controls[q]).BindingSource == this)
                                            {
                                                if (((InfoNavigator)((Form)this.OwnerComp).Controls[q]).GetCurrentState() != "Inserting")
                                                {
                                                    ((InfoNavigator)((Form)this.OwnerComp).Controls[q]).SetState("Editing");
                                                }
                                            }
                                        }
                                    }
                                    bRet = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                this.CheckSucess = bRet;//重新刷新标志,不然错一次后就不能再存档了
            }
            else
            {
                base.EndEdit();
            }
            return bRet;
        }

        public InfoDataSet GetDataSource()
        {
            object obj = this.DataSource;
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
            return (InfoDataSet)obj;
        }

        public object GetDataSource(bool TrackToDataSet)
        {
            InfoDataSet obj = GetDataSource();
            if (TrackToDataSet)
            {
                if (obj != null)
                {
                    return obj.RealDataSet;
                }
            }
            return obj;
        }

        public string GetTableName()
        {
            InfoDataSet obj = this.GetDataSource();
            if (obj != null)
            {
                DataSet dataset = obj.RealDataSet;
                if (dataset.Tables.Contains(this.DataMember))
                {
                    return this.DataMember;
                }
                else if (dataset.Relations.Contains(this.DataMember))
                {
                    return dataset.Relations[this.DataMember].ChildTable.TableName;
                }
            }
            return string.Empty;
        }

        [Category("Infolight"),
        Description("Specifies the interval of delay of relations")]
        public int DelayInterval
        {
            get
            {
                return DelayTimer.Interval;
            }
            set
            {
                DelayTimer.Interval = value;
            }
        }

        public InfoBindingSource()
        {
            fRelations = new InfoRelations(this, typeof(InfoRelation));
            InitializeComponent();
            this.DataSourceChanged += InternalDataSourceChanged;
            this.AllowAdd = true;
            this.AllowDelete = true;
            this.AllowUpdate = true;
            this.AllowPrint = true;
            this.AutoRecordLock = false;
            this.AutoRecordLockMode = LockMode.NoneReload;
            this.AutoDisableStyle = AutoDisableStyleType.Enabled;
            this.ListChanged += new ListChangedEventHandler(InfoBindingSource_ListChanged);
        }

        void InfoBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                OnAfterDelete(new EventArgs());
                // 解决detail删除之后无法改动master的navigator的状态问题
                if (this.DataSource is InfoBindingSource)
                {
                    //    bool bChangeEnable = true;
                    //    IInfoDataSet ds = (IInfoDataSet)this.GetDataSource();
                    //    int i = ds.RealDataSet.Relations.Count;
                    //    for (int j = 0; j < i; j++)
                    //    {
                    //        if (ds.RealDataSet.Relations[j].RelationName == this.DataMember)
                    //        {
                    //            bChangeEnable = false;
                    //        }
                    //    }
                    //    if (!bChangeEnable)
                    //    {
                    //        ((InfoBindingSource)this.DataSource).BeginEdit();
                    //    }
                    //}

                    InfoBindingSource parent = this.DataSource as InfoBindingSource;
                    while (true)
                    {
                        if (parent.DataSource is InfoBindingSource)
                        {
                            parent = parent.DataSource as InfoBindingSource;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (!parent.AutoDisableControl)
                    {
                        bool bChangeEnable = true;
                        IInfoDataSet ds = (IInfoDataSet)this.GetDataSource();
                        int i = ds.RealDataSet.Relations.Count;
                        for (int j = 0; j < i; j++)
                        {
                            if (ds.RealDataSet.Relations[j].RelationName == this.DataMember)
                            {
                                bChangeEnable = false;
                            }
                        }
                        if (!bChangeEnable)
                        {
                            ((InfoBindingSource)this.DataSource).BeginEdit();
                        }
                    }
                }
            }

            if (this.DisableKeyFields && !this.AutoDisableControl)
            {
                if (e.ListChangedType == ListChangedType.ItemAdded)
                {
                    if (keyFlag)
                        SetKeyCtrlsReadOnly(false);
                    keyFlag = true;
                }
                else
                {
                    SetKeyCtrlsReadOnly(false);
                }
            }
        }

        private bool fRelationDelay = false;
        [Category("Infolight"),
        Description("Indicates whether delay of relations is used to improve system's efficiency")]
        public bool RelationDelay
        {
            get
            {
                return fRelationDelay;
            }
            set
            {
                fRelationDelay = value;
            }
        }

        private InfoRelations fRelations;
        [Category("Infolight"),
        Description("Specifies the relations between InfoBindingSource and other DataSets")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InfoRelations Relations
        {
            get
            {
                if (relationActives.Count == 0 && !this.DesignMode)
                {
                    foreach (InfoRelation rel in fRelations)
                    {
                        relationActives.Add(rel.RelationDataSet.RemoteName, rel.Active);
                    }
                }
                return fRelations;
            }
            set
            {
                fRelations = value;
            }
        }

        private bool fAutoApply = false;
        [Category("Infolight"),
        Description("Indicate whether it will apply data to the database automatically after the data is modified")]
        public bool AutoApply
        {
            get
            {
                return fAutoApply;
            }
            set
            {
                fAutoApply = value;
            }
        }

        private bool _AutoApplyMaster;
        [Category("Infolight"),
        Description("Indicates whether the master table applies when the the data is insert to detail table")]
        public bool AutoApplyMaster
        {
            get
            {
                return _AutoApplyMaster;
            }
            set
            {
                _AutoApplyMaster = value;
            }
        }

        private bool _AllowAdd;
        [Category("Infolight"),
        Description("Indicates whether data can be added or not")]
        public bool AllowAdd
        {
            get
            {
                if (!this.DesignMode && this.DataSource is InfoBindingSource)
                {
                    InfoBindingSource ibs = this.DataSource as InfoBindingSource;
                    if (ibs.Current != null && ((ibs.Current as DataRowView).IsNew || (ibs.Current as DataRowView).Row.RowState == DataRowState.Added))
                    {
                        return _AllowAdd;
                    }
                    return _AllowAdd & ibs.AllowUpdate;
                }
                return _AllowAdd;
            }
            set
            {
                if (value != _AllowAdd)
                {
                    _AllowAdd = value;
                    OnAllowPropertyChanged(new AllowProperytyEventArgs(AllowProperytyEventArgs.PropertyName.Add));
                }

            }
        }

        private bool _AllowDelete;
        [Category("Infolight"),
        Description("Indicates whether data can be deleted or not")]
        public bool AllowDelete
        {
            get
            {
                if (!this.DesignMode && this.DataSource is InfoBindingSource)
                {
                    InfoBindingSource ibs = this.DataSource as InfoBindingSource;
                    if (ibs.Current != null && ((ibs.Current as DataRowView).IsNew || (ibs.Current as DataRowView).Row.RowState == DataRowState.Added))
                    {
                        return _AllowDelete;
                    }
                    return _AllowDelete & (ibs as InfoBindingSource).AllowUpdate;
                }
                return _AllowDelete;
            }
            set
            {
                if (value != _AllowDelete)
                {
                    _AllowDelete = value;
                    OnAllowPropertyChanged(new AllowProperytyEventArgs(AllowProperytyEventArgs.PropertyName.Delete));
                }

            }
        }

        private bool _AllowUpdate;
        [Category("Infolight"),
        Description("Indicates whether data can be updated or not")]
        public bool AllowUpdate
        {
            get
            {
                if (!this.DesignMode && this.DataSource is InfoBindingSource)
                {
                    InfoBindingSource ibs = this.DataSource as InfoBindingSource;
                    if (ibs.Current != null && ((ibs.Current as DataRowView).IsNew || (ibs.Current as DataRowView).Row.RowState == DataRowState.Added))
                    {
                        return _AllowUpdate;
                    }
                    return _AllowUpdate & (ibs as InfoBindingSource).AllowUpdate;
                }
                return _AllowUpdate;
            }
            set
            {
                if (value != _AllowUpdate)
                {
                    _AllowUpdate = value;
                    OnAllowPropertyChanged(new AllowProperytyEventArgs(AllowProperytyEventArgs.PropertyName.Update));
                }

            }
        }

        private bool _AllowPrint;
        [Category("Infolight"),
        Description("Indicates whether data can be printed or not")]
        public bool AllowPrint
        {
            get
            {
                return _AllowPrint;
            }
            set
            {
                if (value != _AllowPrint)
                {
                    _AllowPrint = value;
                    OnAllowPropertyChanged(new AllowProperytyEventArgs(AllowProperytyEventArgs.PropertyName.Print));
                }

            }
        }

        private bool serverModifyCache;

        public bool ServerModifyCache
        {
            get { return serverModifyCache; }
            set { serverModifyCache = value; }
        }


        public delegate void AllowPropertyChangedHandler(object sender, AllowProperytyEventArgs e);
        private object allowpropertychanged = new object();
        [Category("Infolight"),
        Description("Event of allow property changed")]
        public event AllowPropertyChangedHandler AllowProperyChanged
        {
            add { Events.AddHandler(allowpropertychanged, value); }
            remove { Events.RemoveHandler(allowpropertychanged, value); }
        }

        protected void OnAllowPropertyChanged(AllowProperytyEventArgs e)
        {
            AllowPropertyChangedHandler call = Events[allowpropertychanged] as AllowPropertyChangedHandler;
            if (call != null)
            {
                call(this, e);
            }
        }

        private ArrayList ctrlList = new ArrayList();
        private Hashtable ctrlstate = new Hashtable();
        private Timer DelayTimer;
        private IContainer components;
        private IDataModule fOwnerComp = null;
        [Browsable(false)]
        public IDataModule OwnerComp
        {
            get
            {
                return fOwnerComp;
            }
            set
            {
                DoBeforeSetOwner(fOwnerComp);
                fOwnerComp = value;
                if (fOwnerComp != null && fOwnerComp is Form)
                {
                    if (this.AutoDisableControl)
                    {
                        AllControlsList.Clear();
                        GetAllControls(((Form)fOwnerComp).Controls);
                        foreach (Control ctrl in AllControlsList)
                        {
                            if (ctrl is InfoRefvalBox && (InfoBindingSource)((InfoRefvalBox)ctrl).TextBoxBindingSource == this)
                            {
                                ctrlList.Add(ctrl);
                                if (DisableKeyFields && IsKeyField(((InfoRefvalBox)ctrl).TextBoxBindingMember))
                                {
                                    keyCtrlList.Add(ctrl);
                                }
                                continue;
                            }
                            foreach (Binding binding in ctrl.DataBindings)
                            {
                                if (binding.DataSource == this)
                                {
                                    if (!(ctrl.Parent is InfoRefvalBox))
                                    {
                                        ctrlList.Add(ctrl);
                                        if (DisableKeyFields && IsKeyField(binding.BindingMemberInfo.BindingField))
                                        {
                                            keyCtrlList.Add(ctrl);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (ctrl is InfoDataGridView && ((InfoDataGridView)ctrl).GetDataSource() == this.GetDataSource())
                            {
                                ctrlList.Add(ctrl);
                                if (DisableKeyFields)
                                {
                                    keyCtrlList.Add(ctrl);
                                }
                            }
                            if (ctrl is InfoRefButton)
                            {
                                InfoRefButton refButton = ctrl as InfoRefButton;
                                foreach (RefButtonMatch match in refButton.refButtonMatchs)
                                {
                                    string controlName = match.matchColumnName;
                                    Control matchControl = refButton.Parent.Controls[controlName];
                                    if (matchControl != null && matchControl.DataBindings.Count > 0 && matchControl.DataBindings[0].DataSource == this)
                                    {
                                        ctrlList.Add(ctrl);
                                        if (DisableKeyFields)
                                        {
                                            keyCtrlList.Add(ctrl);
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                        foreach (Control ctrl in ctrlList)
                        {
                            if (ctrl is DataGridView)
                            {
                                ctrlstate.Add(ctrl.Name, ((DataGridView)ctrl).ReadOnly);
                                foreach (DataGridViewColumn col in ((DataGridView)ctrl).Columns)
                                {
                                    ctrlstate.Add(col.Name, col.ReadOnly);
                                }
                                if (ctrl is InfoDataGridView)
                                {
                                    ((InfoDataGridView)ctrl).ReadOnly = true;
                                }
                                else
                                {
                                    ((DataGridView)ctrl).ReadOnly = true;
                                }
                            }
                            else if (ctrl.GetType() == typeof(DataGrid))
                            {
                                ctrlstate.Add(ctrl.Name, ((DataGrid)ctrl).ReadOnly);
                                ((DataGrid)ctrl).ReadOnly = true;
                            }
                            else
                            {
                                if (this.AutoDisableStyle == AutoDisableStyleType.ReadOnly)
                                {
                                    PropertyInfo pi = ctrl.GetType().GetProperty("ReadOnly");
                                    if (pi != null)
                                    {
                                        ctrlstate.Add(ctrl.Name, (bool)pi.GetValue(ctrl, null));
                                        pi.SetValue(ctrl, true, null);
                                        continue;
                                    }
                                }
                                ctrlstate.Add(ctrl.Name, !ctrl.Enabled);
                                ctrl.Enabled = false;
                            }
                        }
                    }
                    this.EnableFlagChanged += new EventHandler(InfoBindingSource_EnableFlagChanged);
                    ((Form)fOwnerComp).FormClosing += new FormClosingEventHandler(InfoBindingSource_FormClosing);
                    DoAfterSetOwner(fOwnerComp);
                }
            }
        }

        protected virtual void DoBeforeSetOwner(IDataModule value)
        {
        }

        protected virtual void DoAfterSetOwner(IDataModule value)
        {
            if (this.DisableKeyFields && !this.AutoDisableControl)
                SetKeyCtrlsReadOnly(false);
        }

        List<Control> AllControlsList = new List<Control>();
        private void GetAllControls(Control.ControlCollection controlCollection)
        {
            foreach (Control ctrl in controlCollection)
            {
                AllControlsList.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllControls(ctrl.Controls);
                }
            }
        }

        public bool keyFlag2 = false;
        private void InfoBindingSource_EnableFlagChanged(object sender, EventArgs e)
        {
            EnableControl(EnableFlag);
            if (this.EnableFlag && this.FocusedControl != null)
            {
                FocusedControl.Focus();
            }
        }

        //暂时提供的方法
        public void EnableControl(bool enable)
        {
            SetControlStatus(enable);
            if (this.DisableKeyFields)
            {
                if (!keyFlag2)
                {
                    SetKeyCtrlsReadOnly(false);
                }
            }
        }

        private void SetControlStatus(bool enable)
        {
            foreach (Control ctrl in ctrlList)
            {
                bool status = enable ? (bool)ctrlstate[ctrl.Name] : true;
                if (ctrl is DataGridView)
                {
                    if (ctrl is InfoDataGridView)
                    {
                        ((InfoDataGridView)ctrl).ReadOnly = status;
                    }
                    else
                    {
                        ((DataGridView)ctrl).ReadOnly = status;
                    }
                    foreach (DataGridViewColumn col in ((DataGridView)ctrl).Columns)
                    {
                        if (ctrlstate.Contains(col.Name))
                        {
                            status = enable ? (bool)ctrlstate[col.Name] : true;
                            col.ReadOnly = status;
                        }
                    }
                }
                else if (ctrl is DataGrid)
                {
                    ((DataGrid)ctrl).ReadOnly = status;
                }
                else
                {
                    if (this.AutoDisableStyle == AutoDisableStyleType.ReadOnly)
                    {
                        PropertyInfo pi = ctrl.GetType().GetProperty("ReadOnly");
                        if (pi != null)
                        {
                            pi.SetValue(ctrl, status, null);
                            continue;
                        }
                    }
                    ctrl.Enabled = !status;


                }
            }

            Type type = this.OwnerComp.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.Name == "InfoSecColumns")
                {
                    var secColumns = field.GetValue(this.OwnerComp);
                    MethodInfo doMethod = secColumns.GetType().GetMethod("Do", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    doMethod.Invoke(secColumns, null);
                }
            }
        }

        private bool _CloseProtect;
        [Category("Infolight"),
        Description("Enable Protection to prevent user from closing form without applying modified data")]
        public bool CloseProtect
        {
            get
            {
                return _CloseProtect;
            }
            set
            {
                _CloseProtect = value;
            }
        }

        private bool _AutoDisibleControl;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("The recommended alternative is AutoDisableControl")]
        public bool AutoDisibleControl
        {
            get
            {
                return _AutoDisibleControl;
            }
            set
            {
                if (this.DesignMode)
                {
                    if (this.AutoRecordLock)
                    {
                        _AutoDisibleControl = true;
                    }
                    else
                    {
                        _AutoDisibleControl = value;
                    }
                }
                else
                {
                    _AutoDisibleControl = value;
                }

            }
        }

        [Category("Infolight"),
        Description("Indicate whether the enable status of the controls bound to InfoBindingSource will be set as false automatically")]
        public bool AutoDisableControl
        {
            get
            {
                return _AutoDisibleControl;
            }
            set
            {
                if (this.DesignMode)
                {
                    if (this.AutoRecordLock)
                    {
                        _AutoDisibleControl = true;
                    }
                    else
                    {
                        _AutoDisibleControl = value;
                    }
                }
                else
                {
                    _AutoDisibleControl = value;
                }

            }
        }

        public enum AutoDisableStyleType
        {
            ReadOnly,
            Enabled
        }

        private AutoDisableStyleType _AutoDisableStyle;
        [Category("Infolight"),
        Description("Specifies the style of autodisablecontrol")]
        public AutoDisableStyleType AutoDisableStyle
        {
            get { return _AutoDisableStyle; }
            set { _AutoDisableStyle = value; }
        }


        private bool _EnableFlag;
        [Browsable(false)]
        public bool EnableFlag
        {
            get
            {
                return _EnableFlag;
            }
            set
            {
                bool btemp = _EnableFlag;
                _EnableFlag = value;
                if (_EnableFlag != btemp && this.AutoDisableControl)
                {
                    OnEnableFlagChanged(new EventArgs());
                }
            }
        }

        protected void OnEnableFlagChanged(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnEnableFlagChanged];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnEnableFlagChanged = new object();
        public event EventHandler EnableFlagChanged
        {
            add { base.Events.AddHandler(EventOnEnableFlagChanged, value); }
            remove { base.Events.RemoveHandler(EventOnEnableFlagChanged, value); }
        }

        protected void OnBeforeEdit(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnBeforeEdit];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnBeforeEdit = new object();
        public event EventHandler BeforeEdit
        {
            add { base.Events.AddHandler(EventOnBeforeEdit, value); }
            remove { base.Events.RemoveHandler(EventOnBeforeEdit, value); }
        }

        protected void OnAfterEdit(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterEdit];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnAfterEdit = new object();
        public event EventHandler AfterEdit
        {
            add { base.Events.AddHandler(EventOnAfterEdit, value); }
            remove { base.Events.RemoveHandler(EventOnAfterEdit, value); }
        }

        /*protected void OnBeforeDelete(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnBeforeDelete];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnBeforeDelete = new object();
        public event EventHandler BeforeDelete
        {
            add { base.Events.AddHandler(EventOnBeforeDelete, value); }
            remove { base.Events.RemoveHandler(EventOnBeforeDelete, value); }
        }*/

        protected void OnAfterDelete(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterDelete];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnAfterDelete = new object();
        public event EventHandler AfterDelete
        {
            add { base.Events.AddHandler(EventOnAfterDelete, value); }
            remove { base.Events.RemoveHandler(EventOnAfterDelete, value); }
        }


        private void InfoBindingSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.CloseProtect && fOwnerComp is Form && !CliUtils.applicationQuit)
            {
                /*DataRowView drv = (DataRowView)this.Current;
                if (drv != null)
                {
                    if (drv.Row.Table.GetChanges() != null)
                    {
                        MessageBox.Show(this.CloseProtectText);
                        e.Cancel = true;
                    }
                }*/
                Type type = ((Form)fOwnerComp).GetType();
                FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                    | BindingFlags.NonPublic
                                    | BindingFlags.DeclaredOnly);
                for (int i = 0; i < fi.Length; i++)
                {
                    if (fi[i].FieldType == typeof(InfoNavigator))
                    {
                        InfoNavigator navigator = fi[i].GetValue(fOwnerComp) as InfoNavigator;
                        if (navigator.BindingSource != null && navigator.BindingSource.GetDataSource() == this.GetDataSource())
                        {
                            if (isCloseProtectStates(navigator.GetCurrentState()))
                            {
                                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoBindingSource", "msg_CloseProtect");
                                DialogResult result = MessageBox.Show(message, "Waning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    ToolStripItem item = navigator.ApplyItem != null ? navigator.ApplyItem : navigator.OKItem;
                                    if (item != null)
                                    {
                                        item.PerformClick();
                                        if (!isCloseProtectStates(navigator.GetCurrentState()))
                                        {
                                            CliUtils.closeProtected = false;
                                            continue;
                                        }
                                    }
                                }
                                else if (result == DialogResult.No)
                                {
                                    CliUtils.closeProtected = false;
                                    continue;
                                }
                                CliUtils.closeProtected = true;
                                e.Cancel = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool isCloseProtectStates(string stateName)
        {
            if (stateName != "Initial" && stateName != "Browsed"
                && stateName != "Normal" && stateName != "Insert"
                && stateName != "Modify" && stateName != "Inquery"
                && stateName != "Prepare" && stateName != ""
                && stateName != null)
                return true;
            return false;
        }

        internal void InternalDataSourceChanged(object sender, EventArgs e)
        {
            if (DataSource != null)
            {
                //MessageBox.Show(((IInfoDataSet)DataSource).RealDataSet.Count.ToString());
                if ((DataMember != null) && (!DataMember.Equals("")))
                {
                    if (DataSource is IInfoDataSet)
                    {
                        //myView指向Master，我们把PositionChanged加上指挥DefaultView的动作...
                        PositionChanged += InternalPositionChanged;
                        ListChanged += InternalListChanged;
                    }
                }
            }
        }

        private Boolean fInEdit = false;
        private bool fEmptyViewMerge = false;
        public void Set_fEmptyViewMerge() //only use by coder
        {
            fInEdit = true;
        }

        public void Reset_fInEdit()
        {
            fInEdit = false;
        }

        private int fNewIndex = -1;
        private void InternalListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                fInEdit = true;
                fNewIndex = e.NewIndex;
            }
            else if (e.ListChangedType == ListChangedType.Reset && this.List.Count == 0 && !this.DesignMode)
            {
                DoDelay();
            }
        }

        private Boolean fInternalChange = false;


        private bool cancelPositionChanged;

        internal bool CancelPositionChanged
        {
            get { return cancelPositionChanged; }
            set { cancelPositionChanged = value; }
        }


        internal void InternalPositionChanged(object sender, EventArgs e)
        {
            if (CancelPositionChanged)
            {
                return;
            }
            if (AutoApply && DataSource is IInfoDataSet)
            {

                CancelPositionChanged = true;

                bool deletelastrow = false;//mark:this part is used for the problem of delete last row invoke internal index of bindingsource destroyed
                if (DataSource is InfoDataSet)
                {
                    DataTable table = (DataSource as InfoDataSet).RealDataSet.Tables[0];
                    if (table.Rows.Count > 0 && table.Rows[table.Rows.Count - 1].RowState == DataRowState.Deleted)
                    {
                        deletelastrow = true;
                    }
                }
                ((IInfoDataSet)DataSource).ApplyUpdates();

                if (deletelastrow)
                {
                    int lastindex = -1;
                    DataSet ds = null;

                    if (this.Count > 0 && DataSource is InfoDataSet)
                    {
                        ds = (DataSource as InfoDataSet).RealDataSet.Copy();
                    }
                    bool EofFlag = ((InfoDataSet)DataSource).Eof;    //modified by lily 2011/6/15重新Active會導致Eof從true變成false，會導致下面重新取資料，merge就會報錯。
                    ((IInfoDataSet)DataSource).Active = false;
                    ((IInfoDataSet)DataSource).Active = true;
                    ((InfoDataSet)DataSource).Eof = EofFlag;
                    if (ds != null)
                    {
                        (DataSource as InfoDataSet).RealDataSet.Merge(ds);
                    }
                    this.Position = this.Count - 1;
                    ((InfoDataSet)DataSource).LastIndex = this.Count - 1;
                }

                CancelPositionChanged = false;
            }

            if (Position == -1) return;
            if (fEmptyViewMerge)
            {
                fEmptyViewMerge = false;
                return;
            }

            if (DataSource is IInfoDataSet && ((IInfoDataSet)DataSource).RealDataSet.Tables.Count > 0)
            {
                if (DataMember.Equals(((IInfoDataSet)DataSource).RealDataSet.Tables[0].TableName))
                {
                    DataView defView = ((IInfoDataSet)DataSource).RealDataSet.Tables[0].DefaultView;
                    DataRowView myCur = (DataRowView)Current;

                    if (myCur.IsNew || fInternalChange) return;

                    if (fInEdit)
                    {
                        fInEdit = false;
                        return;
                    }

                    fInternalChange = true;
                    try
                    {
                        if (((IInfoDataSet)DataSource).PacketRecords != -1)//default view不是最后一笔...
                        {
                            while (myCur.DataView.Count - 1 == Position)//但是在 UI上已经到了最后一笔，也需要抓NextPacket...
                            {
                                //Remarked by Rax 2006/3/31
                                //为了解决通过程序往Active＝false的InfoDataSet新增数据会导致死循环
                                //新增触发PositionChanged事件，然后触发GetNextPackeds
                                /*((IInfoDataSet)DataSource).GetNextPacket();
                                 if (((IInfoDataSet)DataSource).Eof) break;*/
                                //Remarked by Rax 2006/3/31

                                //added by Rax 2006/3/31

                                if (!((InfoDataSet)DataSource).GetNextPacket()) break;
                                if (((InfoDataSet)DataSource).Eof) break;
                                //added by Rax 2006/3/31
                            }
                        }
                    }
                    finally
                    {
                        fInternalChange = false;
                    }
                }
            }

            //处理Relations
            if (!this.DesignMode)
            {
                if (DelayTimer == null || !RelationDelay)
                    DoDelay();
                else
                {
                    DelayTimer.Enabled = false;
                    DelayTimer.Enabled = true;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.DelayTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DelayTimer
            // 
            this.DelayTimer.Interval = 300;
            this.DelayTimer.Tick += new System.EventHandler(this.DelayTimer_Tick);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }

        private Hashtable relationsqlcmd = new Hashtable();
        public void DoDelay()
        {
            //处理Relations
            foreach (InfoRelation r in Relations)
            {
                if (relationActives.ContainsKey(r.RelationDataSet.RemoteName) && !relationActives[r.RelationDataSet.RemoteName])
                    continue;
                if (relationsqlcmd[r.RelationDataSet.RemoteName] == null)
                {
                    InfoDataSet ids = r.RelationDataSet as InfoDataSet;
                    string strModuleName = ids.RemoteName.Substring(0, (ids.RemoteName.IndexOf('.')));
                    string strTableName = ids.RemoteName.Substring((ids.RemoteName.IndexOf('.') + 1));
                    string sCurProject = CliUtils.fCurrentProject;
                    if (DesignMode)
                    {
                        sCurProject = EditionDifference.ActiveSolutionName();
                    }
                    string relsqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, sCurProject);
                    relationsqlcmd.Add(r.RelationDataSet.RemoteName, relsqlcmd);
                }
                if ((r.TargetKeyFields.Count == r.SourceKeyFields.Count) && (r.SourceKeyFields.Count > 0))
                {
                    // 2006/08/05 將View BindingSource.Relation.Active設  為False,Find()完後, 再設為True
                    if (!((InfoDataSet)r.RelationDataSet).RelationsActive)
                    {
                        r.Active = true;
                    }
                    // 2006/08/05
                    if (r.Active)
                    {
                        //string sWhere = "(";后面有加了，这里不加了
                        StringBuilder sWhere = new StringBuilder();
                        DataRowView drv = (DataRowView)this.Current;
                        if (drv == null)
                        {
                            sWhere.Append("1=0");
                        }
                        else
                        {
                            ClientType ct = CliUtils.GetDataBaseType();
                            DataRow arow = drv.Row;
                            for (int i = 0; i < r.TargetKeyFields.Count; i++)
                            {
                                string sField = r.TargetKeyFields[i].Name;
                                string sField1 = r.SourceKeyFields[i].Name;
                                System.Data.DataColumn aColumn = ((IInfoDataSet)DataSource).RealDataSet.Tables[0].Columns[sField1];
                                if (sWhere.Length > 0)
                                {
                                    sWhere.Append(" And ");
                                }
                                sWhere.Append(string.Format("{0} = "
                                    , CliUtils.GetTableNameForColumn(relationsqlcmd[r.RelationDataSet.RemoteName].ToString(), sField)));
                                string format = DBUtils.GetWhereFormat(aColumn.DataType, ct, OdbcDBType.None, 0);
                                sWhere.Append(string.Format(format, DBUtils.GetWhereValue(aColumn.DataType, arow[aColumn])));
                            }
                        }
                        //sWhere = sWhere + ")";
                        ((IInfoDataSet)(r.RelationDataSet)).SetWhere(sWhere.ToString());
                    }
                    // 2006/08/05 將View BindingSource.Relation.Active設  為False,Find()完後, 再設為True
                    //2007/3/23 remark by lily for servermodify.
                    //((InfoDataSet)r.RelationDataSet).RelationsActive = false;
                    // 2006/08/05
                }
            }
        }

        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            if (fInDelay || !DelayTimer.Enabled) return;
            fInDelay = true;
            try
            {
                DelayTimer.Enabled = false;
                DoDelay();
            }
            finally
            {
                fInDelay = false;
            }
        }

        public void Refresh()
        {
            if (this.Count == 0)
            {
                return;
            }
            DataRow xRow = ((DataRowView)Current).Row;
            if (DataSource is IInfoDataSet)
            {
                IInfoDataSet ds = (IInfoDataSet)DataSource;
                if (xRow != null)
                {
                    if (null != ds.RealDataSet.Tables[0].PrimaryKey)
                    {
                        string strModuleName = ds.RemoteName.Substring(0, (ds.RemoteName.IndexOf('.')));
                        string strTableName = ds.RemoteName.Substring((ds.RemoteName.IndexOf('.') + 1));
                        string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);

                        string wherePart = "";
                        DataColumn[] mydc = (DataColumn[])(ds.RealDataSet.Tables[0].PrimaryKey);
                        DataColumnCollection mydc2 = ds.RealDataSet.Tables[0].Columns;
                        object[] mykeys = new object[mydc.Length];
                        ClientType ct = CliUtils.GetDataBaseType();
                        for (int ix = 0; ix < mydc.Length; ix++)
                        {
                            mykeys[ix] = xRow[mydc[ix]];

                            // Create sql where part.
                            if (wherePart.Length != 0)
                                wherePart += " and ";

                            wherePart += CliUtils.GetTableNameForColumn(sqlcmd, ((DataColumn)mydc[ix]).ColumnName) + " = "
                                + string.Format(DBUtils.GetWhereFormat(((DataColumn)mydc[ix]).DataType, ct, OdbcDBType.None, 0)
                            , DBUtils.GetWhereValue(((DataColumn)mydc[ix]).DataType, xRow[mydc[ix]]));

                        }

                        IInfoDataSet ds2 = new InfoDataSet();
                        ds2.RemoteName = ds.RemoteName;
                        ds2.SetWhere(wherePart);
                        if (ds2 == null || ds2.RealDataSet == null || ds2.RealDataSet.Tables.Count == 0 ||
                            ds2.RealDataSet.Tables[0].Rows.Count == 0)
                        {
                            bool allowD = this.AllowDelete;
                            this.AllowDelete = true;
                            this.RemoveCurrent();
                            this.AllowDelete = allowD;
                            ds.RealDataSet.AcceptChanges();
                            return;
                        }
                        if (this.Container != null)
                        {
                            RegisterListEvent(false);
                            foreach (Component cp in this.Container.Components)
                            {
                                if (cp is InfoBindingSource && (cp as InfoBindingSource).DataSource == this)
                                {
                                    bool allowD = (cp as InfoBindingSource).AllowDelete;
                                    (cp as InfoBindingSource).AllowDelete = true;
                                    int count = (cp as InfoBindingSource).Count;
                                    for (int i = 0; i < count; i++)
                                    {
                                        (cp as InfoBindingSource).RemoveAt(0);
                                    }
                                    (cp as InfoBindingSource).AllowDelete = allowD;
                                }
                            }
                            RegisterListEvent(true);

                            ds.RealDataSet.AcceptChanges();
                        }
                        xRow = ds.RealDataSet.Tables[0].Rows.Find(mykeys);

                        if (this.ServerModifyCache)
                        {
                            this.CancelPositionChanged = true;
                            this.SuspendBinding();
                        }
                        // 同步Row。
                        SyncRow(mydc2, xRow, ds2.RealDataSet.Tables[0].Rows[0]);
                        if (this.ServerModifyCache)
                        {
                            this.CancelPositionChanged = false;
                            this.ResumeBinding();
                        }

                        //处理所有的Detail
                        for (int i = 1; i < ds.RealDataSet.Tables.Count; i++)
                        {
                            ds.RealDataSet.Tables[i].Merge(ds2.RealDataSet.Tables[i]);
                        }
                        ds.RealDataSet.AcceptChanges();

                        DataRowView drv = (DataRowView)Current;
                        DataView dv = drv.DataView;
                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (dv[i].Row == xRow)
                            {
                                Position = i;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ds.Active = false;
                    ds.Active = true;
                }
            }
        }

        private void RegisterListEvent(bool add)
        {
            foreach (Component cp in this.Container.Components)
            {
                if (cp is InfoBindingSource)
                {
                    if (add)
                    {
                        (cp as InfoBindingSource).ListChanged += (cp as InfoBindingSource).InfoBindingSource_ListChanged;
                    }
                    else
                    {
                        (cp as InfoBindingSource).ListChanged -= (cp as InfoBindingSource).InfoBindingSource_ListChanged;
                    }
                }
            }
        }

        // 为Refersh添加的。
        private String Mark(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        // 为Refersh添加的。
        private String Quote(String table_or_column)
        {
            object[] param = new object[1];
            param[0] = CliUtils.fLoginDB;
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", param);
            string type = "";
            if (myRet != null && (int)myRet[0] == 0)
                type = myRet[1].ToString();

            if (type == "1")
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (type == "3")
            {
                return table_or_column;
            }
            else if (type == "2")
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (type == "4")
            {
                return table_or_column;
            }
            else if (type == "5")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        // 为Refersh添加的。
        // r1被Sync的Row
        private void SyncRow(DataColumnCollection cs, DataRow r1, DataRow r2)
        {
            foreach (DataColumn c in cs)
            {
                if (r1.Table.Columns[c.ColumnName] != null && r2.Table.Columns[c.ColumnName] != null)
                    r1[c.ColumnName] = r2[c.ColumnName];
            }
        }

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
        private Char _marker = '\'';

        //added by lily 2007/10/9
        public object GetCurrentValue(string fName)
        {
            if (this.Count == 0 || this.Current == null)
            {
                return null;
            }

            return ((DataRowView)this.Current)[fName];
        }

        public object GetOldValue(string fName)
        {
            if (this.Count == 0 || this.Current == null)
            {
                return null;
            }
            return ((DataRowView)this.Current).Row[fName, DataRowVersion.Original];
        }
        //added by lily 2007/10/9
    }
    #endregion INFO_BINDINGSOURCE

    public class AllowProperytyEventArgs : EventArgs
    {
        public AllowProperytyEventArgs(PropertyName name)
        {
            _Name = name;
        }

        private PropertyName _Name;

        public PropertyName Name
        {
            get { return _Name; }
        }

        public enum PropertyName
        {
            Add,
            Update,
            Delete,
            Print
        }
    }

    #region INFO_RELATIONS
    public class InfoRelation : InfoOwnerCollectionItem
    {
        private string fName = "";
        override public string Name
        {
            get
            {
                if (RelationDataSet != null && ((Component)RelationDataSet).Site != null)
                    return ((Component)RelationDataSet).Site.Name;
                else
                    return fName;
            }
            set
            {
                fName = value;
            }
        }

        private bool fActive = false;
        public bool Active
        {
            get
            {
                return fActive;
            }
            set
            {
                fActive = value;
            }
        }

        private IInfoDataSet fRelationDataSet;
        public IInfoDataSet RelationDataSet
        {
            get
            {
                return fRelationDataSet;
            }
            set
            {
                fRelationDataSet = value;
            }
        }

        public InfoRelation()
        {
            fTargetKeyFields = new InfoKeyFields(this, typeof(InfoKeyField));
            fSourceKeyFields = new InfoKeyFields(this, typeof(InfoKeyField));
        }

        private InfoKeyFields fTargetKeyFields = null;
        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InfoKeyFields TargetKeyFields
        {
            get
            {
                return fTargetKeyFields;
            }
            set
            {
                fTargetKeyFields = value;
            }
        }

        private InfoKeyFields fSourceKeyFields = null;
        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InfoKeyFields SourceKeyFields
        {
            get
            {
                return fSourceKeyFields;
            }
            set
            {
                fSourceKeyFields = value;
            }
        }
    }
    public class InfoRelations : InfoOwnerCollection
    {
        public InfoRelations(object aOwner, Type aItemType)
            : base(aOwner, typeof(InfoRelation))
        {

        }

        new public InfoRelation this[int index]
        {
            get
            {
                return (InfoRelation)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is InfoRelation)
                    {
                        //原来的Collection设置为0
                        ((InfoRelation)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((InfoRelation)InnerList[index]).Collection = this;
                    }

            }
        }
    }
    #endregion INFO_RELATIONS

    #region INFO_KEYFIELDS
    public class InfoKeyField : InfoOwnerCollectionItem, IGetValues
    {
        private string fName = "";
        override public string Name
        {
            get
            {
                if (FieldName != "")
                    return FieldName;
                else
                    return fName;
            }
            set
            {
                fName = value;
            }
        }

        private string fFieldName = "";
        [Category("Infolight"),
        Description("Security Field of InfoCommand"),
        Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fFieldName;
            }
            set
            {
                fFieldName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            if (sKind.Equals("FieldName"))
            {
                InfoRelation r = (InfoRelation)Collection.Owner;
                InfoBindingSource bs = (InfoBindingSource)r.Collection.Owner;
                IInfoDataSet ds;
                if (Collection == r.TargetKeyFields)
                {
                    ds = (IInfoDataSet)r.RelationDataSet;
                }
                else//Collection == r.SourceKeyFields
                {
                    ds = (IInfoDataSet)bs.DataSource;
                }

                if ((ds == null) || (!ds.Active)) return new string[] { };
                int iCount = ds.RealDataSet.Tables[0].Columns.Count;
                string[] sRet = new string[iCount];
                for (int i = 0; i < iCount; i++)
                {
                    sRet[i] = ds.RealDataSet.Tables[0].Columns[i].ColumnName;
                }
                return sRet;
            }
            else
                return new string[] { };
        }
    }

    public class InfoKeyFields : InfoOwnerCollection
    {
        public InfoKeyFields(object aOwner, Type aItemType)
            : base(aOwner, typeof(InfoKeyField))
        {
        }

        new public InfoKeyField this[int index]
        {
            get
            {
                return (InfoKeyField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is InfoKeyField)
                    {
                        //原来的Collection设置为0
                        ((InfoKeyField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((InfoKeyField)InnerList[index]).Collection = this;
                    }

            }
        }
    }
    #endregion INFO_KEYFIELDS

    internal class FocusedControlEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> focusCtrls = new List<string>();
            if (context.Instance != null && context.Instance is InfoBindingSource)
            {
                InfoBindingSource bindingsource = (InfoBindingSource)context.Instance;
                List<Control> ctrls = new List<Control>(); ;
                foreach (IComponent comp in bindingsource.Container.Components)
                {
                    if (comp is Control)
                    {
                        ctrls.Add((Control)comp);
                    }
                }
                GetAllCtrls(ctrls);
                foreach (Control ctrl in AllCtrls)
                {
                    foreach (Binding binding in ctrl.DataBindings)
                    {
                        if (binding.DataSource is InfoBindingSource &&
                            binding.DataSource == bindingsource
                            && ((InfoBindingSource)binding.DataSource).DataMember == bindingsource.DataMember)
                        {
                            focusCtrls.Add(ctrl.Name);
                        }
                    }
                }
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, focusCtrls.ToArray());
                string strValue = ((Control)value).Name;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }

        private List<Control> AllCtrls = new List<Control>();
        private void GetAllCtrls(List<Control> ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                AllCtrls.Add(ctrl);
                List<Control> childCtrls = new List<Control>();
                foreach (Control childCtrl in ctrl.Controls)
                {
                    childCtrls.Add(childCtrl);
                }
                GetAllCtrls(childCtrls);
            }
        }
    }
}