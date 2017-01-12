using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(InfoRefVal), "Resources.InfoRefVal.png")]
    public class InfoRefVal : InfoBaseComp, ISupportInitialize, IUseSelectCommand
    {
        private InfoTextBox infotxt;
        private DataGridView infodgv;

        private object objDataSource = null;
        private object objDataBinding = null;
        private string strSourceTab = string.Empty;
        private string strBindingTab = string.Empty;
        string strValField = string.Empty;
        string strDisField = string.Empty;
        private string strBindingField = string.Empty;
        internal InfoDataSet InnerDs = new InfoDataSet();
        internal InfoBindingSource InnerBs = new InfoBindingSource();

        public InfoRefVal()
        {
            _whereItem = new WhereItemCollection(this, typeof(WhereItem));
            _columnMatch = new ColumnMatchCollection(this, typeof(ColumnMatch));
            _columns = new RefColumnsCollection(this, typeof(RefColumns));
            _Font = new Font("SimSun", 9.0f);
            //_SortMember = string.Empty;
        }

        protected override void DoAfterSetOwner(IDataModule value)
        {
            if (AllCtrls.Count > 0)
            {
                AllCtrls.Clear();
            }
            GetAllCtrls(((Form)this.OwnerComp).Controls);

            foreach (Control ctrl in AllCtrls)
            {
                if (ctrl is InfoRefvalBox && ((InfoRefvalBox)ctrl).RefVal == this)
                {
                    string selValue = ((InfoRefvalBox)ctrl).TextBoxSelectedValue;
                    object[] obj = this.CheckValid_And_ReturnDisplayValue(ref selValue, false, false);
                    ((InfoRefvalBox)ctrl).TextBoxText = (string)obj[1];
                }
            }
        }

        #region ISupportInitialize
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
                        sb.Insert(this.SelectCommand.Trim().IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6
                            , " top " + this.SelectTop);//IgnoreCase
                    }
                    this.SelectCommand = sb.ToString();
                }
                if (this.Site == null)
                {
                    string command = this.SelectCommand;
                    if (AlwaysClose)
                    {
                        command = CliUtils.InsertWhere(command, "1=0");
                    }

                    InnerDs.RemoteName = "GLModule.cmdRefValUse";
                    InnerDs.Execute(command, true);

                    InnerBs.DataMember = "cmdRefValUse";
                    InnerBs.DataSource = InnerDs;
                    this.DataSource = InnerBs;
                }
            }
        }
        #endregion

        public string GetValue(string value)
        {
            return CliUtils.GetValue(value, this.OwnerComp).ToString();
        }

        public object GetDataSource()
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
            return obj;
        }

        public string GetDataMember()
        {
            // 因为InfoRefVal的DataMember不可能为relation
            // 所以不考虑做GetDataTable(),这边一定取得到
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

        public string GetDisplayMember()
        {
            string strDisplayMember = this.DisplayMember;
            if (this.DataSource is InfoDataSet)
            {
                strDisplayMember = strDisplayMember.Substring(strDisplayMember.IndexOf('.') + 1);
            }
            return strDisplayMember;
        }

        public string GetValueMember()
        {
            string strValueMember = this.ValueMember;
            if (this.DataSource is InfoDataSet)
            {
                strValueMember = strValueMember.Substring(strValueMember.IndexOf('.') + 1);
            }
            return strValueMember;
        }

        private string m_editingDisplayMember = null;
        [Category("Data")]
        [Browsable(false)]
        public string EditingDisplayMember
        {
            get { return m_editingDisplayMember; }
            set { m_editingDisplayMember = value; }
        }

        private object _DataSource;
        [Category("Infolight"),
        Description("The DataSource which the control is bound to")]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        public object DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        private string _DisplayMember;
        [Category("Infolight"),
        Description("indicates the property to use as the actual vale for the items in the control")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor,System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public string DisplayMember
        {
            get { return _DisplayMember; }
            set { _DisplayMember = value; }
        }

        private string _ValueMember;
        [Category("Infolight"),
        Description("indicates the property to display for the items in this control")]    
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor,System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public string ValueMember
        {
            get
            {
                return _ValueMember;
            }
            set
            {
                _ValueMember = value;
                if (_ValueMember != null && _ValueMember != "")
                {
                    this.EditingDisplayMember = _ValueMember;
                }
            }
        }
        private string _LinkDisplayMember;
        [Category("Infolight"),
        Description("indicates the property to display for the items in this control")]    
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor,System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public string LinkDisplayMember
        {
            get
            {
                return _LinkDisplayMember;
            }
            set
            {
                _LinkDisplayMember = value;
            }
        }
        private string _LinkObject;
        [Category("Infolight"),
        Description("RefValBox一律只顯示ID, 然後在右邊貼上一個TextBox用來顯示名稱")]    
        public string LinkObject
        {
            get
            {
                return _LinkObject;
            }
            set
            {
                _LinkObject = value;
            }
        }
        
        //private string _SortMember;
        //[Category("Infolight"),
        //Description("indicates the property to display for the items in this control")]    
        //[Editor("System.Windows.Forms.Design.DataMemberFieldEditor,System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        //public string SortMember
        //{
        //    get { return _SortMember; }
        //    set { _SortMember = value; }
        //}
	
        private bool _IgnoreCase = false;
        [Category("Infolight"),
        Description("Ignore up case and low case")]
        public bool IgnoreCase
        {
            get
            {
                return _IgnoreCase;
            }
            set
            {
                _IgnoreCase = value;
            }
        }

        private Font _Font;
        [Category("Infolight"),
        Description("Font of form of refval")]
        public Font Font
        {
            get { return _Font; }
            set { _Font = value; }
        }
	

        public enum ShowStyle
        {
            gridStyle = 0
        }

        private ShowStyle _Styles;
        [Category("Infolight"),
        Description("Style of display")]
        public ShowStyle Styles
        {
            get { return _Styles; }
            set { _Styles = value; }
        }

        private string _Caption;
        [Category("Infolight"),
        Description("The caption of the form of InfoRefVal")]
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        private bool _AutoGridSize;
        [Category("Infolight"),
        Description("Enables automatic resizing the form based on grid size")]
        public bool AutoGridSize
        {
            get { return _AutoGridSize; }
            set { _AutoGridSize = value; }
        }

        private Size formSize = new Size(410, 255);

        public Size FormSize
        {
            get { return formSize; }
            set { formSize = value; }
        }	

        private WhereItemCollection _whereItem;
        [Category("Infolight"),
        Description("Specifies the columns in where part to get data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WhereItemCollection whereItem
        {
            get { return _whereItem; }
            set { _whereItem = value; }
        }

        private ColumnMatchCollection _columnMatch;
        [Category("Infolight"),
        Description("Specifies the columns in which data can be copied from source table to destination table")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColumnMatchCollection columnMatch
        {
            get { return _columnMatch; }
            set { _columnMatch = value; }
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

        private string  _FLookupValue = string.Empty;
        [Browsable(false)]
        public string  FLookupValue
        {
            get { return _FLookupValue; }
            set { _FLookupValue  = value; }
        }

        private bool _whereItemCache = true;

        public bool WhereItemCache
        {
            get { return _whereItemCache; }
            set { _whereItemCache = value; }
        }
	
	

        public void Active(OnActiveEventArgs value)
        {
            OnActiveEventHandler handler = (OnActiveEventHandler)Events[EventOnActive];
            if ((handler != null) && (value is OnActiveEventArgs))
            {
                handler(this, (OnActiveEventArgs)value);
            }
        }

        internal static readonly object EventOnActive = new object();
        public event OnActiveEventHandler OnActive
        {
            add { Events.AddHandler(EventOnActive, value); }
            remove { Events.RemoveHandler(EventOnActive, value); }
        }

        public void Close(OnCloseEventArgs value)
        {
            OnCloseEventHandler handler = (OnCloseEventHandler)Events[EventOnClose];
            if ((handler != null) && (value is OnCloseEventArgs))
            {
                handler(this, (OnCloseEventArgs)value);
            }
        }

        internal static readonly object EventOnClose = new object();
        public event OnCloseEventHandler OnClose
        {
            add { Events.AddHandler(EventOnClose, value); }
            remove { Events.RemoveHandler(EventOnClose, value); }
        }

        public void Return(OnReturnEventArgs value)
        {
            OnReturnEventHandler handler = (OnReturnEventHandler)Events[EventOnReturn];
            if ((handler != null) && (value is OnReturnEventArgs))
            {
                handler(this, (OnReturnEventArgs)value);
            }
        }
        internal static readonly object EventOnReturn = new object();
        public event OnReturnEventHandler OnReturn
        {
            add { Events.AddHandler(EventOnReturn, value); }
            remove { Events.RemoveHandler(EventOnReturn, value); }
        }

        internal static readonly object EventQueryWhere = new object();
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

        public void SetToTxt(InfoTextBox childTextBox)
        {
            infotxt = childTextBox;
        }

        public void SetToDgv(DataGridView childDataGridView)
        {
            infodgv = childDataGridView;
        }

        private bool _RefByWhere;
        [Browsable(false)]
        public bool RefByWhere
        {
            get
            {
                return _RefByWhere;
            }
            set
            {
                _RefByWhere = value;
            }
        }

        private string _SelectTop;
        [Browsable(false)]
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

        [Category("Infolight")]
        public int PacketRecords
        {
            get
            {
                return InnerDs.PacketRecords;
            }
            set
            {
                InnerDs.PacketRecords = value;
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
                    string cmd = _SelectCommand.Clone().ToString();
                    if (_AlwaysClose)
                    {
                        cmd = CliUtils.InsertWhere(cmd, "1=0");
                    }
                    InnerDs.Execute(cmd, _SelectAlias, true);
                    InnerBs.DataSource = InnerDs;
                    InnerBs.DataMember = "cmdRefValUse";
                    this.DataSource = InnerBs;
                }
            }
        }

        private string _SelectAlias;
        [Category("Infolight"),
        Description("Specifies database")]
        [Editor(typeof(RefConnectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        private bool _CheckData = true;
        [Category("Infolight"),
        Description("Indicate whether the data is checked if exsit in table when user inputs it")]
        public bool CheckData
        {
            get
            {
                return _CheckData;
            }
            set
            {
                _CheckData = value;
            }
        }

        private bool _AlwaysClose = false;
        [Category("Infolight"),
        Description("Indicate whether the data is get when the form loads")]
        public bool AlwaysClose 
        {
            get
            {
                return _AlwaysClose;
            }
            set
            {
                _AlwaysClose = value;
            }
        }

        private bool _AllowAddData = false;
        [Category("Infolight")]
        public bool AllowAddData
        {
            get
            {
                return _AllowAddData;
            }
            set
            {
                _AllowAddData = value;
            }
        }

        private InfoRefvalBox _ActiveBox = null;
        [Category("Infolight")]
        public InfoRefvalBox ActiveBox
        {
            get
            {
                return _ActiveBox;
            }
            set
            {
                _ActiveBox = value;
            }
        }

        private InfoDataGridViewRefValColumn _ActiveColumn;
        [Browsable(false)]
        public InfoDataGridViewRefValColumn ActiveColumn
        {
            get { return _ActiveColumn; }
            set { _ActiveColumn = value; }
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

        private bool _AutoLocate = true;
        [Category("Infolight"),
        Description("Indicate whether user can allocate data with key press")]
        public bool AutoLocate
        {
            get { return _AutoLocate; }
            set { _AutoLocate = value; }
        }

        private void SetAll()
        {
            if (null != infotxt && this.infotxt.DataBindings["SelectedValue"] != null)
            {
                objDataBinding = this.infotxt.GetDataSource();
                strBindingTab = this.infotxt.GetBindingMember();
                strBindingField = this.infotxt.GetBindingField();
            }
            objDataSource = this.GetDataSource();
            strSourceTab = this.GetDataMember();
            strValField = this.GetValueMember();
            strDisField = this.GetDisplayMember();
        }

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
                if (table_or_column.IndexOf(_quotePrefix) == 0 && table_or_column.IndexOf(_quoteSuffix) == table_or_column.Length - 1)
                {
                    return table_or_column;
                }
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (type == "3")
            {
                return table_or_column;
            }
            else if (type == "2")
            {
                if (table_or_column.IndexOf(_quotePrefix) == 0 && table_or_column.IndexOf(_quoteSuffix) == table_or_column.Length - 1)
                {
                    return table_or_column;
                }
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
            else if (type == "6")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";

        public object[] CheckValid_And_ReturnDisplayValue(object Value, bool bReport, bool Repaint)
        {
            string strValue = Value != null ? Value.ToString() : string.Empty;
            return CheckValid_And_ReturnDisplayValue(ref strValue, bReport, Repaint);
        }

        private string notFoundValue = "";

        /// <summary>
        /// check validate of InfoRefVal's value
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns>The first object is typeof bool, indicating the value is exist or not. The second object is typeof string, it's the displayValue of it's value</returns>
        public object[] CheckValid_And_ReturnDisplayValue(ref string strValue, bool bReport, bool Repaint)
        {
            //2006/8/4 add by lily for desing-time 不去取值 
            if (this.Site != null)
            {
                return new object[] { false, strValue };
            }
            //2006/8/4 add by lily for desing-time 不去取值 

            if (strValue == null || strValue == "")
            {
                //Modified by lily false to true解决新增时会显示其他值而不是空白的问题
                return new object[] { true, "", null };
            }
            object[] obj = new object[] { false, "",null };
            SetAll();
            if (strDisField == strValField && !CheckData && columnMatch.Count == 0)
            {
                return new object[] { true, strValue };//ValueMember和DisplayMember相同时直接返回
            }
            if (objDataSource != null && strSourceTab != "" && strValField != null && strValField != "" && strDisField != null && strDisField != "")
            {
                //////////// 2006/07/07 ////////////
                bool InWhere = false;
                bool bInOrignDs = false;
                DataTable table = ((InfoDataSet)objDataSource).RealDataSet.Tables[strSourceTab];
                DataRow[] whereRow = table.Select(this.WhereString());
                int x = whereRow.Length;
                for (int y = 0; y < x; y++)
                {
                    if (!this.IgnoreCase)
                    {
                        if (strValue == whereRow[y][strValField].ToString())
                        {
                            obj[0] = true;
                            obj[1] = whereRow[y][strDisField].ToString();
                            obj[2] = whereRow[y];
                            InWhere = true;
                            break;
                        }
                    }
                    else
                    {
                        if (string.Compare(strValue, whereRow[y][strValField].ToString(), true) == 0)//IgnoreCase
                        {
                            obj[0] = true;
                            obj[1] = whereRow[y][strDisField].ToString();
                            obj[2] = whereRow[y];
                            strValue = whereRow[y][strValField].ToString();
                            InWhere = true;
                            break;
                        }
                    }
                }
                //////////// 2006/07/07 ////////////
                if (!false)
                {
                    int i = table.Rows.Count;
                    for (int j = 0; j < i; j++)
                    {
                        if (!this.IgnoreCase)
                        {
                            if (strValue == table.Rows[j][strValField].ToString())
                            {
                                obj[0] = true;
                                obj[1] = table.Rows[j][strDisField].ToString();
                                obj[2] = table.Rows[j];
                                bInOrignDs = true;
                                break;
                            }
                        }
                        else
                        {
                            if (string.Compare(strValue, table.Rows[j][strValField].ToString(), true) == 0)//IgnoreCase
                            {
                                obj[0] = true;
                                obj[1] = table.Rows[j][strDisField].ToString();
                                strValue = table.Rows[j][strValField].ToString();
                                obj[2] = table.Rows[j];
                                bInOrignDs = true;
                                break;
                            }
                        }
                    }
                }
                if (!InWhere && !bInOrignDs/* && this.RefByWhere*/)
                {
                    string strModuleName = ((InfoDataSet)objDataSource).RemoteName.Substring(0, ((InfoDataSet)objDataSource).RemoteName.LastIndexOf('.'));
                    //如果在其自身的dataset中找不到数据，则自动按selectedvalue去数据库中下达。
                    //可能dataset有packetrecords，或whereitems
                    bool bInExtendDs = false;
                    if (this.WhereItemCache)
                    {
                        if (!((InfoDataSet)objDataSource).RealDataSet.Tables.Contains("RefValWhereTable"))
                        {
                            ((InfoDataSet)objDataSource).RealDataSet.Tables.Add("RefValWhereTable");
                        }
                        int m = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows.Count;
                        for (int n = 0; n < m; n++)
                        {
                            if (!this.IgnoreCase)
                            {
                                if (strValue == ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strValField].ToString())
                                {
                                    obj[0] = true;
                                    obj[1] = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strDisField].ToString();
                                    obj[2] = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n];
                                    bInExtendDs = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (string.Compare(strValue, ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strValField].ToString(), true) == 0)//IgnoreCase
                                {
                                    obj[0] = true;
                                    obj[1] = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strDisField].ToString();
                                    obj[2] = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n];
                                    strValue = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strValField].ToString();
                                    bInExtendDs = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!bInExtendDs)
                    {
                        if (this.WhereItemCache && strValue == notFoundValue)
                        {
                            obj[0] = false;
                            obj[1] = strValue;
                            goto nnFind;
                        }
                        //check type 
                        DataTable table1 = ((InfoDataSet)objDataSource).RealDataSet.Tables[strSourceTab];
                        Type datatype = table1.Columns[strValField].DataType;
                        try
                        {
                            if (datatype != typeof(Guid))
                            {
                                Convert.ChangeType(strValue, datatype);
                            }
                            else
                            {
                                Guid id = new Guid(strValue);
                            }
                        }
                        catch
                        {
                            obj[0] = false;
                            obj[1] = strValue;
                            goto nnFind;
                        }

                        string sCurProject = CliUtils.fCurrentProject;
                        string strSql = "";
                        if (this.SelectAlias != null && this.SelectAlias != "" && this.SelectCommand != null && this.SelectCommand != "")
                        {
                            strSql = this.SelectCommand;
                        }
                        else
                        {
                            //modified by lily 2007/7/16 此處，tablename不應考慮別名，而應該直接抓from的名字，否則會報錯
                            strSql = DBUtils.GetCommandText(((InfoDataSet)objDataSource), this.GetDataMember());
                            //modified by lily 2007/5/24 for schema.table

                        }
                        string strWhere = CliUtils.GetTableNameForColumn(strSql, strValField) + " = ";

                        string type = table.Columns[strValField].DataType.ToString().ToLower();
                        if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                         || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                         || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
                        {
                            strWhere += strValue;
                        }
                        else
                        {
                            strWhere += "'" + strValue + "'";
                        }
                        if (strWhere != "")
                        {
                            strSql = CliUtils.InsertWhere(strSql, strWhere);
                        }

                        string whereitemstring = WhereString(strSql);
                        if (whereitemstring.Length > 0)
                        {
                            strSql = CliUtils.InsertWhere(strSql, whereitemstring);
                        }
                        DataSet ds = CliUtils.ExecuteSql(strModuleName, strSourceTab, strSql, true, sCurProject);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (this.WhereItemCache)
                            {
                                ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Merge(ds.Tables[0]);
                            }
                            obj[0] = true;
                            obj[1] = ds.Tables[0].Rows[0][strDisField].ToString();
                            obj[2] = ds.Tables[0].Rows[0];
                            if (this.IgnoreCase)
                            {
                                strValue = ds.Tables[0].Rows[0][strValField].ToString();
                            }
                        }
                        else
                        {
                            notFoundValue = strValue;
                            obj[0] = false;
                            obj[1] = strValue;
                        }
                    }
                }
            nnFind:
                if (Repaint)
                    RefreshRefColumns();
                if (!(bool)obj[0] && bReport)
                {
                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoRefVal", "msg_RefValValueNotFound");
                    MessageBox.Show(String.Format(message, strValue));
                }
            }
            return obj;
        }

        public object[] CheckValid_And_ReturnLinkValue(ref string strText)
        {
            strText = strText != null ? strText.ToString() : string.Empty;

            object[] obj = new object[] { false, strText };
            if (!string.IsNullOrEmpty(LinkObject) && !string.IsNullOrEmpty(LinkDisplayMember))
            {
                SetAll();
                if (objDataSource != null && strSourceTab != "" && strValField != null && strValField != "")
                {
                    //////////// 2006/07/07 ////////////
                    bool InWhere = false;
                    bool bInOrignDs = false;
                    DataTable table = ((InfoDataSet)objDataSource).RealDataSet.Tables[strSourceTab];
                    DataRow[] whereRow = table.Select(this.WhereString());
                    int x = whereRow.Length;
                    for (int y = 0; y < x; y++)
                    {
                        if (!this.IgnoreCase)
                        {
                            if (strText == whereRow[y][strValField].ToString())
                            {
                                obj[0] = true;
                                obj[1] = whereRow[y][LinkDisplayMember].ToString();
                                InWhere = true;
                                break;
                            }
                        }
                        else
                        {
                            if (string.Compare(strText, whereRow[y][strValField].ToString(), true) == 0)//IgnoreCase
                            {
                                obj[0] = true;
                                obj[1] = whereRow[y][LinkDisplayMember].ToString();
                                strText = whereRow[y][strValField].ToString();
                                InWhere = true;
                                break;
                            }
                        }
                    }
                    //////////// 2006/07/07 ////////////
                    if (!false)
                    {
                        int i = table.Rows.Count;
                        for (int j = 0; j < i; j++)
                        {
                            if (!this.IgnoreCase)
                            {
                                if (strText == table.Rows[j][strValField].ToString())
                                {
                                    obj[0] = true;
                                    obj[1] = table.Rows[j][LinkDisplayMember].ToString();
                                    bInOrignDs = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (string.Compare(strText, table.Rows[j][strValField].ToString(), true) == 0)//IgnoreCase
                                {
                                    obj[0] = true;
                                    obj[1] = table.Rows[j][LinkDisplayMember].ToString();
                                    strText = table.Rows[j][strValField].ToString();
                                    bInOrignDs = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!InWhere && !bInOrignDs/* && this.RefByWhere*/)
                    {
                        string strModuleName = ((InfoDataSet)objDataSource).RemoteName.Substring(0, ((InfoDataSet)objDataSource).RemoteName.LastIndexOf('.'));
                        //如果在其自身的dataset中找不到数据，则自动按selectedvalue去数据库中下达。
                        //可能dataset有packetrecords，或whereitems
                        bool bInExtendDs = false;
                        if (this.WhereItemCache)
                        {
                            if (!((InfoDataSet)objDataSource).RealDataSet.Tables.Contains("RefValWhereTable"))
                            {
                                ((InfoDataSet)objDataSource).RealDataSet.Tables.Add("RefValWhereTable");
                            }
                            int m = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows.Count;
                            for (int n = 0; n < m; n++)
                            {
                                if (!this.IgnoreCase)
                                {
                                    if (strText == ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strValField].ToString())
                                    {
                                        obj[0] = true;
                                        obj[1] = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][LinkDisplayMember].ToString();
                                        bInExtendDs = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (string.Compare(strText, ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strValField].ToString(), true) == 0)//IgnoreCase
                                    {
                                        obj[0] = true;
                                        obj[1] = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][LinkDisplayMember].ToString();
                                        strText = ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Rows[n][strValField].ToString();
                                        bInExtendDs = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!bInExtendDs)
                        {
                            if (this.WhereItemCache && strText == notFoundValue)
                            {
                                obj[0] = false;
                                obj[1] = strText;
                                goto nnFind;
                            }
                            //check type 
                            DataTable table1 = ((InfoDataSet)objDataSource).RealDataSet.Tables[strSourceTab];
                            Type datatype = table1.Columns[strValField].DataType;
                            try
                            {
                                if (datatype != typeof(Guid))
                                {
                                    Convert.ChangeType(strText, datatype);
                                }
                                else
                                {
                                    Guid id = new Guid(strText);
                                }
                            }
                            catch
                            {
                                obj[0] = false;
                                obj[1] = strText;
                                goto nnFind;
                            }

                            string sCurProject = CliUtils.fCurrentProject;
                            string strSql = "";
                            if (this.SelectAlias != null && this.SelectAlias != "" && this.SelectCommand != null && this.SelectCommand != "")
                            {
                                strSql = this.SelectCommand;
                            }
                            else
                            {
                                //modified by lily 2007/7/16 此處，tablename不應考慮別名，而應該直接抓from的名字，否則會報錯
                                strSql = DBUtils.GetCommandText(((InfoDataSet)objDataSource), this.GetDataMember());
                                //modified by lily 2007/5/24 for schema.table

                            }
                            string strWhere = CliUtils.GetTableNameForColumn(strSql, strValField) + " = ";

                            string type = table.Columns[strValField].DataType.ToString().ToLower();
                            if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                             || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                             || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
                            {
                                strWhere += strText;
                            }
                            else
                            {
                                strWhere += "'" + strText + "'";
                            }
                            if (strWhere != "")
                            {
                                strSql = CliUtils.InsertWhere(strSql, strWhere);
                            }

                            string whereitemstring = WhereString(strSql);
                            if (whereitemstring.Length > 0)
                            {
                                strSql = CliUtils.InsertWhere(strSql, whereitemstring);
                            }
                            DataSet ds = CliUtils.ExecuteSql(strModuleName, strSourceTab, strSql, true, sCurProject);
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                if (this.WhereItemCache)
                                {
                                    ((InfoDataSet)objDataSource).RealDataSet.Tables["RefValWhereTable"].Merge(ds.Tables[0]);
                                }
                                obj[0] = true;
                                obj[1] = ds.Tables[0].Rows[0][LinkDisplayMember].ToString();
                                if (this.IgnoreCase)
                                {
                                    strText = ds.Tables[0].Rows[0][strValField].ToString();
                                }
                            }
                            else
                            {
                                notFoundValue = strText;
                                obj[0] = false;
                                obj[1] = strText;
                            }
                        }
                    }
                nnFind:
                    if (!(bool)obj[0])
                    {
                        //String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoRefVal", "msg_RefValValueNotFound");
                        //MessageBox.Show(String.Format(message, strText));
                    }
                }
            }
            return obj;
        }

        private void RefreshRefColumns()
        {
            if (this.OwnerComp != null)
            {
                if (AllCtrls.Count > 0)
                {
                    AllCtrls.Clear();
                }
                GetAllCtrls(((Form)this.OwnerComp).Controls);
                foreach (Control ctrl in AllCtrls)
                {
                    if (ctrl is InfoDataGridView)
                    {
                        foreach (DataGridViewColumn col in ((InfoDataGridView)ctrl).Columns)
                        {
                            if (col is InfoDataGridViewRefValColumn && ((InfoDataGridViewRefValColumn)col).RefValue == this)
                            {
                                col.Width += 1;
                                col.Width -= 1;
                            }
                        }
                    }
                }
            }
        }

        internal string WhereString()
        {
            string strFilter = "";
            if (this.whereItem.Count != 0 && this.OwnerComp != null)
            {
                foreach (WhereItem wi in this.whereItem)
                {
                    string whereValue = "";
                    //if (this.OwnerComp != null)
                    whereValue = this.GetValue(wi.Value);
                    if (objDataSource == null)
                    {
                        SetAll();
                    }
                    string type = ((InfoDataSet)objDataSource).RealDataSet.Tables[strSourceTab].Columns[wi.FieldName].DataType.ToString().ToLower();
                    if (wi.GetSign() != "like begin with value" && wi.GetSign() != "like with value")
                    {
                        if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                         || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                         || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
                        {
                            strFilter += wi.FieldName + wi.GetSign() + whereValue + " and ";
                        }
                        else
                        {
                            strFilter += wi.FieldName + wi.GetSign() + "'" + whereValue + "' and ";
                        }
                    }
                    else
                    {
                        if (wi.GetSign() == "like begin with value")
                        {
                            strFilter += wi.FieldName + " like '" + whereValue + "%' and ";
                        }
                        if (wi.GetSign() == "like with value")
                        {
                            strFilter += wi.FieldName + " like '%" + whereValue + "%' and ";
                        }
                    }
                }
                if (strFilter != string.Empty)
                {
                    strFilter = strFilter.Substring(0, strFilter.LastIndexOf(" and "));
                }
            }
            NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strFilter,true);
            OnQueryWhere(args);
            strFilter = args.WhereString;
            return strFilter;
        }
        
        internal string WhereString(string sql)
        {
            string strFilter = "";
            if (this.whereItem.Count != 0 && this.OwnerComp != null)
            {
                foreach (WhereItem wi in this.whereItem)
                {
                    string whereValue = "";
                    if (this.OwnerComp != null)
                        whereValue = this.GetValue(wi.Value);
                    if (objDataSource == null)
                    {
                        SetAll();
                    }
                    string type = ((InfoDataSet)objDataSource).RealDataSet.Tables[strSourceTab].Columns[wi.FieldName].DataType.ToString().ToLower();
                    if (wi.GetSign() != "like begin with value" && wi.GetSign() != "like with value")
                    {
                        if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                         || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                         || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
                        {
                            strFilter += CliUtils.GetTableNameForColumn(sql, wi.FieldName) + wi.GetSign() + whereValue + " and ";
                        }
                        else if (type == "system.datetime")
                        {
                            whereValue = CliUtils.FormatDateString(whereValue); 
                            strFilter += CliUtils.GetTableNameForColumn(sql, wi.FieldName) + wi.GetSign() + whereValue + " and ";
                        }
                        else
                        {
                            strFilter += CliUtils.GetTableNameForColumn(sql, wi.FieldName) + wi.GetSign() + "'" + whereValue + "' and ";
                        }
                    }
                    else
                    {
                        if (wi.GetSign() == "like begin with value")
                        {
                            strFilter += CliUtils.GetTableNameForColumn(sql, wi.FieldName) + " like '" + whereValue + "%' and ";
                        }
                        if (wi.GetSign() == "like with value")
                        {
                            strFilter += CliUtils.GetTableNameForColumn(sql, wi.FieldName) + " like '%" + whereValue + "%' and ";
                        }
                    }
                }
                if (strFilter != string.Empty)
                {
                    strFilter = strFilter.Substring(0, strFilter.LastIndexOf(" and "));
                }
            }
            NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strFilter);
            OnQueryWhere(args);
            strFilter = args.WhereString;
            return strFilter;
        }

        public void DoColumnMatch(string strTxtValue)
        {
            string strText = strTxtValue;
            object[] obj = this.CheckValid_And_ReturnDisplayValue(ref strText, false, false);
            this.DoColumnMatch(strTxtValue, (DataRow)obj[2]);
        }

        internal void DoColumnMatch(string strTxtValue, DataRow dr)
        {
            DataRowView drv = null;
            object datasource = null;
            string strFilter = this.WhereString();
            if (this.infotxt != null)
            {
                drv = ((IGetCurrentDataRow)this.infotxt).GetCurrentDataRow();
                if (this.infotxt.DataBindings["SelectedValue"] != null)
                {
                    datasource = this.infotxt.DataBindings["SelectedValue"].DataSource;
                }
            }
            else if (this.infodgv != null)
            {
                drv = (DataRowView)this.infodgv.CurrentRow.DataBoundItem;
                datasource = this.infodgv.DataSource;
            }
            if (AllCtrls.Count > 0)
                AllCtrls.Clear();
            GetAllCtrls(((Form)this.OwnerComp).Controls);
            foreach (ColumnMatch cm in this.columnMatch)
            {
                if (drv != null)
                {
                    if (cm.SrcGetValue == null || cm.SrcGetValue == "")
                    {
                        //((InfoBindingSource)this.DataSource).Filter = strFilter;
                        //int i = ((InfoBindingSource)this.DataSource).List.Count;
                        //for (int j = 0; j < i; j++)
                        //{
                        //    DataRowView rowView = (DataRowView)((InfoBindingSource)this.DataSource).List[j];
                        //    if (rowView.Row[strValField].ToString() == strTxtValue)
                        //    {
                        //        drv.Row[cm.DestField] = ((DataRowView)((InfoBindingSource)this.DataSource).List[j]).Row[cm.SrcField];
                        //    }
                        //}
                        //Modified by lily 2007/10/23 for when ValueMember is empty, match null to columnMatch column.
                        if (dr == null)
                        {
                            //return;
                            drv.Row[cm.DestField] = DBNull.Value;
                        }
                        else
                        {
                            drv.Row[cm.DestField] = dr[cm.SrcField];
                        }
                        //Modified by lily 2007/10/23 for when ValueMember is empty, match null to columnMatch column.
                    }
                    else
                    {
                        drv.Row[cm.DestField] = GetValue(cm.SrcGetValue);
                    }

                    foreach (Control ctrl in AllCtrls)
                    {
                        if (ctrl is InfoRefvalBox)
                        {
                            InfoRefvalBox box = ctrl as InfoRefvalBox;
                            if (datasource == box.TextBoxBindingSource && string.Compare(box.TextBoxBindingMember, cm.DestField, true) == 0)
                            {
                                box.TextBoxSelectedValue = drv.Row[cm.DestField].ToString();
                            }
                        }
                        else if (ctrl.DataBindings.Count > 0)
                        {
                            Binding binding = ctrl.DataBindings[0];
                            if (binding.DataSource == datasource && string.Compare(binding.BindingMemberInfo.BindingField, cm.DestField, true) == 0)
                            {
                                binding.ReadValue();
                            }

                        }
                    }
                }
            }
        }

        public void DoLinkMatch(string strLinkValue)
        {
            if (AllCtrls.Count > 0)
                AllCtrls.Clear();
            GetAllCtrls(((Form)this.OwnerComp).Controls);
            foreach (Control ctrl in AllCtrls)
            {
                if (ctrl.Name == LinkObject)
                {
                    if (ctrl is TextBox)
                    {
                        ctrl.Text = strLinkValue;
                    }
                }
            }
        }
        public void DoLinkMatch(string strLinkValue,Form form)
        {
            if (AllCtrls.Count > 0)
                AllCtrls.Clear();
            GetAllCtrls(form.Controls);
            foreach (Control ctrl in AllCtrls)
            {
                if (ctrl.Name == LinkObject)
                {
                    if (ctrl is TextBox)
                    {
                        ctrl.Text = strLinkValue;
                    }
                }
            }
        }

        private List<Control> AllCtrls = new List<Control>();
        private void GetAllCtrls(Control.ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                AllCtrls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllCtrls(ctrl.Controls);
                }
            }
        }
    }

    /*public class RefCommandTextEditor : System.Drawing.Design.UITypeEditor
    {
        public RefCommandTextEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            InfoRefVal refval = (InfoRefVal)context.Instance;
            IDbConnection con = null;
            switch(refval.ConnectionType)
            { 
                case "1":
                    con = new SqlConnection(refval.SelectAlias);
                    break;
                case "2":
                    con = new OleDbConnection(refval.SelectAlias);
                    break;
                case "3":
                    con = new OracleConnection(refval.SelectAlias);
                    break;
                case "4":
                    con = new OdbcConnection(refval.SelectAlias);
                    break;
            }
            String commandText = "";
            if (con != null)
            {
                CommandTextOptionDialog dialog = new CommandTextOptionDialog(con, refval.SelectCommand);
                edSvc.ShowDialog(dialog);
                commandText = dialog.CommandText;
                dialog.Dispose();
            }
            return commandText;
        }
    }*/

    public class RefConnectionEditor : System.Drawing.Design.UITypeEditor
    {
        public RefConnectionEditor()
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
                EditorService =
                    provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                ListBox ColumnList = new ListBox();
                ColumnList.SelectionMode = SelectionMode.One;
                InfoRefVal refval = context.Instance as InfoRefVal;
                if (refval != null)
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
