using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Collections;
using System.Drawing;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(InfoClientMove), "Resources.InfoClientMove.png")]
    public partial class InfoClientMove : InfoBaseComp
    {
        private SYS_LANGUAGE language;
        
        public InfoClientMove()
        {
            _KeyMatchColumns = new keyMatchColumns(this, typeof(keyMatchColumn));
            _MatchColumns = new matchColumns(this, typeof(matchColumn));
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            InitializeComponent();
        }

        public InfoClientMove(IContainer container)
        {
            container.Add(this);
            _KeyMatchColumns = new keyMatchColumns(this, typeof(keyMatchColumn));
            _MatchColumns = new matchColumns(this, typeof(matchColumn));
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            InitializeComponent();
        }

        private InfoBindingSource _SrcBindingSorce;
        [Category("Infolight"),
         Description("The InfoBindingSource of source table")]
        public InfoBindingSource SrcBindingSorce
        {
            get
            {
                return _SrcBindingSorce;
            }
            set
            {
                _SrcBindingSorce = value;
            }
        }

        private InfoBindingSource _DestBindingSorce;
        [Category("Infolight"),
         Description("The InfoBindingSource of destination table")]
        public InfoBindingSource DestBindingSorce
        {
            get
            {
                return _DestBindingSorce;
            }
            set
            {
                _DestBindingSorce = value;
            }
        }

        private bool _AlwaysInsert;
        [Category("Infolight"),
         Description("Indicates whether data is always inserted even the same data of key columns exsiting in database")]
        public bool AlwaysInsert
        {
            get
            {
                return _AlwaysInsert;
            }
            set
            {
                _AlwaysInsert = value;
            }
        }

        private bool _AlwaysReplace;
        [Category("Infolight"),
         Description("Indicates whether data is always replaced even the same data of key columns exsiting in database")]
        public bool AlwaysReplace
        {
            get
            {
                return _AlwaysReplace;
            }
            set
            {
                _AlwaysReplace = value;
            }
        }

        private keyMatchColumns _KeyMatchColumns;
        [Category("Infolight"),
         Description("Specifies the columns storing the relational infomation between source table and destination table")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public keyMatchColumns KeyMatchColumns
        {
            get
            {
                return _KeyMatchColumns;
            }
            set
            {
                _KeyMatchColumns = value;
            }
        }

        private matchColumns _MatchColumns;
        [Category("Infolight"),
         Description("Specifies the columns storing the infomation to transfer between source table and destination table")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public matchColumns MatchColumns
        {
            get
            {
                return _MatchColumns;
            }
            set
            {
                _MatchColumns = value;
            }
        }

        public bool Excute(bool progress)
        {
            if (this.SrcBindingSorce != null && this.DestBindingSorce != null)
            {
                bool tempSrc, tempDest;
                tempSrc = this.SrcBindingSorce.AutoApply;
                this.SrcBindingSorce.AutoApply = false;
                tempDest = this.DestBindingSorce.AutoApply;
                this.DestBindingSorce.AutoApply = false;
                
                DataSet dsSrc = ((InfoDataSet)this.SrcBindingSorce.GetDataSource()).RealDataSet;
                DataTable dtSrc = new DataTable();
                bool isGet = false;
                for (int i = 0; i < dsSrc.Relations.Count; i++)
                {
                    if (this.SrcBindingSorce.DataMember == dsSrc.Relations[i].RelationName)
                    {
                        dtSrc = dsSrc.Relations[i].ChildTable;
                        isGet = true;
                    }
                }
                if (isGet == false)
                    dtSrc = dsSrc.Tables[0];
                DataSet dsDest = ((InfoDataSet)this.DestBindingSorce.GetDataSource()).RealDataSet;
                DataTable dtDest = new DataTable();
                isGet = false;
                for (int i = 0; i < dsDest.Relations.Count; i++)
                {
                    if (this.DestBindingSorce.DataMember == dsDest.Relations[i].RelationName)
                    {
                        dtDest = dsDest.Relations[i].ChildTable;
                        isGet = true;
                    }
                }
                if (isGet == false)
                    dtDest = dsDest.Tables[0];
                
                
                DataRowView[] drvDest = new DataRowView[DestBindingSorce.List.Count];
                for (int i = 0; i < DestBindingSorce.List.Count; i++)
                    drvDest[i] = (DataRowView)DestBindingSorce.List[i];

                ArrayList strSrc = new ArrayList();
                ArrayList strDest = new ArrayList();
                foreach (keyMatchColumn kmc in this.KeyMatchColumns)
                {
                    if (kmc.SrcColumnName == null || kmc.DestColumnName == null || kmc.SrcColumnName == "" || kmc.DestColumnName == "")
                    {
                        language = CliUtils.fClientLang;
                        string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoClientMove", "iskeyMatchColumn");
                        MessageBox.Show(message);
                        return false;
                    }
                    else
                    {
                        strSrc.Add(kmc.SrcColumnName);
                        strDest.Add(kmc.DestColumnName);
                    }
                }
                InfoClientMoveProgress icmp = new InfoClientMoveProgress();
                if (progress == true)
                    icmp.Show();

                for (int i = 0; i < dtSrc.Rows.Count; i++)
                {
                    int flag = 0;
                    if (this.AlwaysInsert == false && strSrc != null)
                    {
                        for (int j = 0; j < drvDest.Length; j++)
                        {
                            for (int x = 0; x < strSrc.Count; x++)
                                if (dtSrc.Rows[i][strSrc[x].ToString()].ToString() == drvDest[j].Row[strDest[x].ToString()].ToString())
                                    flag++;
                            if (flag == strSrc.Count)
                            {
                                if (this.AlwaysReplace == true)
                                {
                                    OnAfterMove(new EventArgs());
                                    foreach (matchColumn mc in this.MatchColumns)
                                    {
                                        if (mc.Expression != null && mc.Expression != "")
                                        {
                                            DataColumn dcExpression = new DataColumn();
                                            dcExpression.ColumnName = mc.Expression;
                                            dcExpression.Expression = mc.Expression;
                                            if (dtSrc.Columns.Contains(dcExpression.ColumnName) == false)
                                                dtSrc.Columns.Add(dcExpression);
                                            drvDest[j].Row[mc.DestColumnName] = dtSrc.Rows[i][mc.Expression];
                                        }
                                        else
                                        {
                                            drvDest[j].Row[mc.DestColumnName] = dtSrc.Rows[i][mc.SrcColumnName].ToString();
                                        }
                                    }
                                    OnAfterInsert(new EventArgs());
                                }
                                break;
                            }
                            else
                                flag = 0;
                        }
                    }
                    if (flag != strSrc.Count || strSrc.Count == 0)
                    {
                        DataRowView drv = DestBindingSorce.AddNew() as DataRowView;
                        drv.BeginEdit();
                        //DataRow dr = dtDest.NewRow();
                        //DataRow dr = dtDest.Rows[dtDest.Rows.Count - 1];
                        //DataRowView drv = ((InfoBindingSource)(DestBindingSorce.DataSource)).Current as DataRowView;
                        //dr[dsDest.Tables[0].PrimaryKey[0].ToString()] = drv.Row[dsDest.Tables[0].PrimaryKey[0].ToString()]; 
                        foreach (matchColumn mc in this.MatchColumns)
                        {
                            if (mc.Expression != null && mc.Expression != "")
                            {
                                DataColumn dcExpression = new DataColumn();
                                dcExpression.ColumnName = mc.Expression;
                                dcExpression.Expression = mc.Expression;
                                if (dtSrc.Columns.Contains(dcExpression.ColumnName) == false)
                                    dtSrc.Columns.Add(dcExpression);
                                drv[mc.DestColumnName] = dtSrc.Rows[i][mc.Expression];
                            }
                            else
                            {
                                if ((mc.SrcColumnName == "" || mc.DestColumnName == "" || mc.SrcColumnName == null || mc.DestColumnName == null)
                                   && (mc.Expression == null || mc.Expression == ""))
                                {
                                    language = CliUtils.fClientLang;
                                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoClientMove", "ismatchColumn");
                                    MessageBox.Show(message);
                                    return false;
                                }
                                else
                                    drv[mc.DestColumnName] = dtSrc.Rows[i][mc.SrcColumnName];
                            }
                        }
                        drv.EndEdit();
                        OnAfterMove(new EventArgs());
                        //dtDest.WriteXml(@"C:\A.xml");
                        //dtDest.Rows.Add(dr);
                        OnAfterInsert(new EventArgs());
                    }

                    if (progress == true)
                        icmp.progressBar1.Value = ((i + 1) * 100) / dtSrc.Rows.Count;
                }
                if (progress == true)
                    icmp.Close();
                this.SrcBindingSorce.AutoApply = tempSrc;
                this.DestBindingSorce.AutoApply = tempDest;
            }
            else
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoClientMove", "isBindingSorce");
                MessageBox.Show(String.Format(message, this.Site.Name));
                return false;
            }
            return true;
        }

        internal static readonly object EventOnAfterInsert = new object();
        public event EventHandler AfterInsert
        {
            add { base.Events.AddHandler(EventOnAfterInsert, value); }
            remove { base.Events.RemoveHandler(EventOnAfterInsert, value); }
        }
        protected void OnAfterInsert(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterInsert];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        internal static readonly object EventOnAfterMove = new object();
        public event EventHandler AfterMove
        {
            add { base.Events.AddHandler(EventOnAfterMove, value); }
            remove { base.Events.RemoveHandler(EventOnAfterMove, value); }
        }
        protected void OnAfterMove(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterMove];
            if (handler != null)
            {
                handler(this, value);
            }
        }
    }

    public class keyMatchColumns : InfoOwnerCollection
    {
        public keyMatchColumns(object aOwner, Type aItemType)
            : base(aOwner, typeof(keyMatchColumn))
        {

        }

        new public keyMatchColumn this[int index]
        {
            get
            {
                return (keyMatchColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is keyMatchColumn)
                    {
                        //原来的Collection设置为0
                        ((keyMatchColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((keyMatchColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class keyMatchColumn : InfoOwnerCollectionItem
    {
        private string _Name = "";
        override public string Name
        {
            get
            {
                if (DestColumnName != null)
                    return this.DestColumnName;
                else
                    return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _SrcColumnName;
        [Editor(typeof(GetSrcColumnName), typeof(UITypeEditor))]
        public string SrcColumnName
        {
            get
            {
                return _SrcColumnName;
            }
            set
            {
                _SrcColumnName = value;
            }
        }

        private string _DestColumnName;
        [Editor(typeof(GetDestColumnName), typeof(UITypeEditor))]
        public string DestColumnName
        {
            get
            {
                return _DestColumnName;
            }
            set
            {
                _DestColumnName = value;
            }
        }
    }

    public class matchColumns : InfoOwnerCollection
    {
        public matchColumns(object aOwner, Type aItemType)
            : base(aOwner, typeof(matchColumn))
        {

        }

        new public matchColumn this[int index]
        {
            get
            {
                return (matchColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is matchColumn)
                    {
                        //原来的Collection设置为0
                        ((matchColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((matchColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class matchColumn : InfoOwnerCollectionItem
    {
        private string _Name = "";
        override public string Name
        {
            get
            {
                if (DestColumnName != null)
                    return this.DestColumnName;
                else
                    return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _SrcColumnName;
        [Category("Sorce"),
        Editor(typeof(GetSrcColumnName), typeof(UITypeEditor))]
        public string SrcColumnName
        {
            get
            {
                return _SrcColumnName;
            }
            set
            {
                _SrcColumnName = value;
            }
        }

        private string _Expression;
        [Category("Sorce")]
        public string Expression
        {
            get
            {
                return _Expression;
            }
            set
            {
                _Expression = value;
            }
        }


        private string _DestColumnName;
        [Category("Destination"),
        Editor(typeof(GetDestColumnName), typeof(UITypeEditor))]
        public string DestColumnName
        {
            get
            {
                return _DestColumnName;
            }
            set
            {
                _DestColumnName = value;
            }
        }
    }

    public class GetSrcColumnName : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetSrcColumnName()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
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
                ColumnList.Items.Add("( None )");
                keyMatchColumn ifc = new keyMatchColumn();
                matchColumn mc = new matchColumn();
                if (context.Instance is keyMatchColumn)
                {
                    ifc = context.Instance as keyMatchColumn;
                    if (ifc != null)
                    {
                        if (((InfoClientMove)(ifc.Owner)).SrcBindingSorce != null)
                        {
                            InfoBindingSource ibs = ((InfoClientMove)(ifc.Owner)).SrcBindingSorce;
                            DataView dataView = ibs.List as DataView;

                            if (dataView != null)
                            {
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    ColumnList.Items.Add(column.ColumnName);
                                }
                            }
                            else
                            {
                                int iRelationPos = -1;
                                DataSet ds = ((InfoDataSet)((InfoClientMove)(ifc.Owner)).SrcBindingSorce.GetDataSource()).RealDataSet;
                                for (int i = 0; i < ds.Relations.Count; i++)
                                {
                                    if (((InfoClientMove)(ifc.Owner)).SrcBindingSorce.DataMember == ds.Relations[i].RelationName)
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
                        }
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
                else if (context.Instance is matchColumn)
                {
                    mc = context.Instance as matchColumn;
                    if (mc != null)
                    {
                        if (((InfoClientMove)(mc.Owner)).SrcBindingSorce != null)
                        {
                            InfoBindingSource ibs = ((InfoClientMove)(mc.Owner)).SrcBindingSorce;
                            DataView dataView = ibs.List as DataView;

                            if (dataView != null)
                            {
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    ColumnList.Items.Add(column.ColumnName);
                                }
                            }
                            else
                            {
                                int iRelationPos = -1;
                                DataSet ds = ((InfoDataSet)((InfoClientMove)(mc.Owner)).SrcBindingSorce.GetDataSource()).RealDataSet;
                                for (int i = 0; i < ds.Relations.Count; i++)
                                {
                                    if (((InfoClientMove)(mc.Owner)).SrcBindingSorce.DataMember == ds.Relations[i].RelationName)
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
                        }
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
            }
            return value;
        }
    }

    public class GetDestColumnName : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetDestColumnName()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
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
                ColumnList.Items.Add("( None )");
                keyMatchColumn ifc = new keyMatchColumn();
                matchColumn mc = new matchColumn();
                if (context.Instance is keyMatchColumn)
                {
                    ifc = context.Instance as keyMatchColumn;
                    if (ifc != null)
                    {
                        if (((InfoClientMove)(ifc.Owner)).DestBindingSorce != null)
                        {
                            InfoBindingSource ibs = ((InfoClientMove)(ifc.Owner)).DestBindingSorce;
                            DataView dataView = ibs.List as DataView;

                            if (dataView != null)
                            {
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    ColumnList.Items.Add(column.ColumnName);
                                }
                            }
                            else
                            {
                                int iRelationPos = -1;
                                DataSet ds = ((InfoDataSet)((InfoClientMove)(ifc.Owner)).DestBindingSorce.GetDataSource()).RealDataSet;
                                for (int i = 0; i < ds.Relations.Count; i++)
                                {
                                    if (((InfoClientMove)(ifc.Owner)).DestBindingSorce.DataMember == ds.Relations[i].RelationName)
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
                        }
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
                else if (context.Instance is matchColumn)
                {
                    mc = context.Instance as matchColumn;
                    if (mc != null)
                    {
                        if (((InfoClientMove)(mc.Owner)).DestBindingSorce != null)
                        {
                            InfoBindingSource ibs = ((InfoClientMove)(mc.Owner)).DestBindingSorce;
                            DataView dataView = ibs.List as DataView;

                            if (dataView != null)
                            {
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    ColumnList.Items.Add(column.ColumnName);
                                }
                            }
                            else
                            {
                                int iRelationPos = -1;
                                DataSet ds = ((InfoDataSet)((InfoClientMove)(mc.Owner)).DestBindingSorce.GetDataSource()).RealDataSet;
                                for (int i = 0; i < ds.Relations.Count; i++)
                                {
                                    if (((InfoClientMove)(mc.Owner)).DestBindingSorce.DataMember == ds.Relations[i].RelationName)
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
                        }
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
            }
            return value;
        }
    }
}
