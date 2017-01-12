namespace Srvtools
{
    partial class frmAgent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAgent));
            Srvtools.StateItem stateItem1 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem2 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem3 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem4 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem5 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem6 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem7 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem8 = new Srvtools.StateItem();
            Srvtools.FieldItem fieldItem1 = new Srvtools.FieldItem();
            Srvtools.FieldItem fieldItem2 = new Srvtools.FieldItem();
            Srvtools.FieldItem fieldItem3 = new Srvtools.FieldItem();
            Srvtools.FieldItem fieldItem4 = new Srvtools.FieldItem();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dsRoleAgent = new Srvtools.InfoDataSet(this.components);
            this.bsRoleAgent = new Srvtools.InfoBindingSource(this.components);
            this.navRoleAgent = new Srvtools.InfoNavigator();
            this.bindingNavigatorAbortItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorApplyItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCancelItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorEditItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorExportItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorOKItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorRefreshItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorQueryItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorPrintItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bsUsers = new Srvtools.InfoBindingSource(this.components);
            this.dsUsers = new Srvtools.InfoDataSet(this.components);
            this.rvUsers = new Srvtools.InfoRefVal();
            this.dvRoleAgent = new Srvtools.DefaultValidate(this.components);
            this.txtRoleId = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.lblRoleName = new System.Windows.Forms.Label();
            this.lblRoleId = new System.Windows.Forms.Label();
            this.infoDataGridView1 = new Srvtools.InfoDataGridView();
            this.colAgent = new Srvtools.InfoDataGridViewRefValColumn();
            this.colFlowDesc = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSDate = new Srvtools.InfoDataGridViewCalendarColumn();
            this.colSTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEDate = new Srvtools.InfoDataGridViewCalendarColumn();
            this.colETime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParAgent = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsRoleAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRoleAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navRoleAgent)).BeginInit();
            this.navRoleAgent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rvUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvRoleAgent)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dsRoleAgent
            // 
            this.dsRoleAgent.Active = true;
            this.dsRoleAgent.AlwaysClose = false;
            this.dsRoleAgent.DataCompressed = false;
            this.dsRoleAgent.DeleteIncomplete = true;
            this.dsRoleAgent.LastKeyValues = null;
            this.dsRoleAgent.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsRoleAgent.PacketRecords = -1;
            this.dsRoleAgent.Position = -1;
            this.dsRoleAgent.RemoteName = "GLModule.cmdRoleAgent";
            this.dsRoleAgent.ServerModify = false;
            // 
            // bsRoleAgent
            // 
            this.bsRoleAgent.AllowAdd = true;
            this.bsRoleAgent.AllowDelete = true;
            this.bsRoleAgent.AllowPrint = true;
            this.bsRoleAgent.AllowUpdate = true;
            this.bsRoleAgent.AutoApply = false;
            this.bsRoleAgent.AutoApplyMaster = false;
            this.bsRoleAgent.AutoDisableControl = true;
            this.bsRoleAgent.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.bsRoleAgent.AutoRecordLock = false;
            this.bsRoleAgent.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.bsRoleAgent.CloseProtect = false;
            this.bsRoleAgent.DataMember = "cmdRoleAgent";
            this.bsRoleAgent.DataSource = this.dsRoleAgent;
            this.bsRoleAgent.DelayInterval = 300;
            this.bsRoleAgent.DisableKeyFields = false;
            this.bsRoleAgent.EnableFlag = false;
            this.bsRoleAgent.FocusedControl = null;
            this.bsRoleAgent.OwnerComp = null;
            this.bsRoleAgent.RelationDelay = false;
            this.bsRoleAgent.ServerModifyCache = false;
            this.bsRoleAgent.text = "bsRoleAgent";
            // 
            // navRoleAgent
            // 
            this.navRoleAgent.AbortItem = this.bindingNavigatorAbortItem;
            this.navRoleAgent.AddNewItem = this.bindingNavigatorAddNewItem;
            this.navRoleAgent.AnyQueryID = "navRoleAgent";
            this.navRoleAgent.ApplyItem = this.bindingNavigatorApplyItem;
            this.navRoleAgent.BindingSource = this.bsRoleAgent;
            this.navRoleAgent.BeforeItemClick += new BeforeItemClickEventHandler(navRoleAgent_BeforeItemClick);
            this.navRoleAgent.CancelItem = this.bindingNavigatorCancelItem;
            this.navRoleAgent.CopyItem = null;
            this.navRoleAgent.CountItem = null;
            this.navRoleAgent.DeleteItem = this.bindingNavigatorDeleteItem;
            this.navRoleAgent.DescriptionItem = null;
            this.navRoleAgent.DetailBindingSource = null;
            this.navRoleAgent.DetailKeyField = null;
            this.navRoleAgent.EditItem = this.bindingNavigatorEditItem;
            this.navRoleAgent.ExportItem = this.bindingNavigatorExportItem;
            this.navRoleAgent.ForeColors = System.Drawing.Color.Empty;
            this.navRoleAgent.GetRealRecordsCount = false;
            this.navRoleAgent.GetServerText = false;
            this.navRoleAgent.HideItemStates = false;
            this.navRoleAgent.InternalQuery = true;
            this.navRoleAgent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.bindingNavigatorEditItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorOKItem,
            this.bindingNavigatorCancelItem,
            this.bindingNavigatorApplyItem,
            this.bindingNavigatorAbortItem,
            this.bindingNavigatorSeparator3,
            this.bindingNavigatorRefreshItem,
            this.bindingNavigatorQueryItem,
            this.bindingNavigatorPrintItem,
            this.bindingNavigatorExportItem,
            this.bindingNavigatorSeparator4,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem});
            this.navRoleAgent.Location = new System.Drawing.Point(0, 0);
            this.navRoleAgent.MoveFirstItem = null;
            this.navRoleAgent.MoveLastItem = null;
            this.navRoleAgent.MoveNextItem = null;
            this.navRoleAgent.MovePreviousItem = null;
            this.navRoleAgent.MultiLanguage = false;
            this.navRoleAgent.Name = "navRoleAgent";
            this.navRoleAgent.OKItem = this.bindingNavigatorOKItem;
            this.navRoleAgent.PositionItem = null;
            this.navRoleAgent.PreQueryCondition = ((System.Collections.Generic.List<string>)(resources.GetObject("navRoleAgent.PreQueryCondition")));
            this.navRoleAgent.PreQueryField = ((System.Collections.Generic.List<string>)(resources.GetObject("navRoleAgent.PreQueryField")));
            this.navRoleAgent.PreQueryValue = ((System.Collections.Generic.List<string>)(resources.GetObject("navRoleAgent.PreQueryValue")));
            this.navRoleAgent.PrintItem = this.bindingNavigatorPrintItem;
            this.navRoleAgent.QueryFont = new System.Drawing.Font("SimSun", 9F);
            this.navRoleAgent.QueryKeepCondition = false;
            this.navRoleAgent.QueryMargin = new System.Drawing.Printing.Margins(100, 30, 10, 30);
            this.navRoleAgent.QueryMode = Srvtools.InfoNavigator.QueryModeType.ClientQuery;
            this.navRoleAgent.QuerySQLSend = true;
            this.navRoleAgent.Size = new System.Drawing.Size(729, 25);
            stateItem1.Description = null;
            stateItem1.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem1.EnabledControls")));
            stateItem1.EnabledControlsEdited = false;
            stateItem1.StateText = "Initial";
            stateItem2.Description = null;
            stateItem2.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem2.EnabledControls")));
            stateItem2.EnabledControlsEdited = false;
            stateItem2.StateText = "Browsed";
            stateItem3.Description = null;
            stateItem3.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem3.EnabledControls")));
            stateItem3.EnabledControlsEdited = false;
            stateItem3.StateText = "Inserting";
            stateItem4.Description = null;
            stateItem4.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem4.EnabledControls")));
            stateItem4.EnabledControlsEdited = false;
            stateItem4.StateText = "Editing";
            stateItem5.Description = null;
            stateItem5.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem5.EnabledControls")));
            stateItem5.EnabledControlsEdited = false;
            stateItem5.StateText = "Applying";
            stateItem6.Description = null;
            stateItem6.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem6.EnabledControls")));
            stateItem6.EnabledControlsEdited = false;
            stateItem6.StateText = "Changing";
            stateItem7.Description = null;
            stateItem7.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem7.EnabledControls")));
            stateItem7.EnabledControlsEdited = false;
            stateItem7.StateText = "Querying";
            stateItem8.Description = null;
            stateItem8.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem8.EnabledControls")));
            stateItem8.EnabledControlsEdited = false;
            stateItem8.StateText = "Printing";
            this.navRoleAgent.States.Add(stateItem1);
            this.navRoleAgent.States.Add(stateItem2);
            this.navRoleAgent.States.Add(stateItem3);
            this.navRoleAgent.States.Add(stateItem4);
            this.navRoleAgent.States.Add(stateItem5);
            this.navRoleAgent.States.Add(stateItem6);
            this.navRoleAgent.States.Add(stateItem7);
            this.navRoleAgent.States.Add(stateItem8);
            this.navRoleAgent.StatusStrip = null;
            this.navRoleAgent.SureAbort = false;
            this.navRoleAgent.SureDelete = true;
            this.navRoleAgent.SureDeleteText = null;
            this.navRoleAgent.SureInsert = false;
            this.navRoleAgent.SureInsertText = null;
            this.navRoleAgent.TabIndex = 1;
            this.navRoleAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.navRoleAgent.ViewBindingSource = null;
            this.navRoleAgent.ViewCountItem = this.bindingNavigatorCountItem;
            this.navRoleAgent.ViewCountItemFormat = "of {0}";
            this.navRoleAgent.ViewMoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.navRoleAgent.ViewMoveLastItem = this.bindingNavigatorMoveLastItem;
            this.navRoleAgent.ViewMoveNextItem = this.bindingNavigatorMoveNextItem;
            this.navRoleAgent.ViewMovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.navRoleAgent.ViewPositionItem = this.bindingNavigatorPositionItem;
            this.navRoleAgent.ViewQueryItem = this.bindingNavigatorQueryItem;
            this.navRoleAgent.ViewRefreshItem = this.bindingNavigatorRefreshItem;
            this.navRoleAgent.ViewScrollProtect = false;
            // 
            // bindingNavigatorAbortItem
            // 
            this.bindingNavigatorAbortItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAbortItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAbortItem.Image")));
            this.bindingNavigatorAbortItem.Name = "bindingNavigatorAbortItem";
            this.bindingNavigatorAbortItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAbortItem.Text = "abort";
            this.bindingNavigatorAbortItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "add";
            this.bindingNavigatorAddNewItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorApplyItem
            // 
            this.bindingNavigatorApplyItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorApplyItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorApplyItem.Image")));
            this.bindingNavigatorApplyItem.Name = "bindingNavigatorApplyItem";
            this.bindingNavigatorApplyItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorApplyItem.Text = "apply";
            this.bindingNavigatorApplyItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorCancelItem
            // 
            this.bindingNavigatorCancelItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorCancelItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorCancelItem.Image")));
            this.bindingNavigatorCancelItem.Name = "bindingNavigatorCancelItem";
            this.bindingNavigatorCancelItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorCancelItem.Text = "cancel";
            this.bindingNavigatorCancelItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "delete";
            this.bindingNavigatorDeleteItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorEditItem
            // 
            this.bindingNavigatorEditItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorEditItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorEditItem.Image")));
            this.bindingNavigatorEditItem.Name = "bindingNavigatorEditItem";
            this.bindingNavigatorEditItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorEditItem.Text = "edit";
            this.bindingNavigatorEditItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorExportItem
            // 
            this.bindingNavigatorExportItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorExportItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorExportItem.Image")));
            this.bindingNavigatorExportItem.Name = "bindingNavigatorExportItem";
            this.bindingNavigatorExportItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorExportItem.Text = "export";
            this.bindingNavigatorExportItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Enabled = false;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "first";
            this.bindingNavigatorMoveFirstItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Enabled = false;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "previous";
            this.bindingNavigatorMovePreviousItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Enabled = false;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "next";
            this.bindingNavigatorMoveNextItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Enabled = false;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "last";
            this.bindingNavigatorMoveLastItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorOKItem
            // 
            this.bindingNavigatorOKItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorOKItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorOKItem.Image")));
            this.bindingNavigatorOKItem.Name = "bindingNavigatorOKItem";
            this.bindingNavigatorOKItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorOKItem.Text = "ok";
            this.bindingNavigatorOKItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorSeparator3
            // 
            this.bindingNavigatorSeparator3.Name = "bindingNavigatorSeparator3";
            this.bindingNavigatorSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorRefreshItem
            // 
            this.bindingNavigatorRefreshItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorRefreshItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorRefreshItem.Image")));
            this.bindingNavigatorRefreshItem.Name = "bindingNavigatorRefreshItem";
            this.bindingNavigatorRefreshItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorRefreshItem.Text = "refresh";
            this.bindingNavigatorRefreshItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorQueryItem
            // 
            this.bindingNavigatorQueryItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorQueryItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorQueryItem.Image")));
            this.bindingNavigatorQueryItem.Name = "bindingNavigatorQueryItem";
            this.bindingNavigatorQueryItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorQueryItem.Text = "query";
            this.bindingNavigatorQueryItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorPrintItem
            // 
            this.bindingNavigatorPrintItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorPrintItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorPrintItem.Image")));
            this.bindingNavigatorPrintItem.Name = "bindingNavigatorPrintItem";
            this.bindingNavigatorPrintItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorPrintItem.Text = "print";
            this.bindingNavigatorPrintItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorSeparator4
            // 
            this.bindingNavigatorSeparator4.Name = "bindingNavigatorSeparator4";
            this.bindingNavigatorSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 25);
            this.bindingNavigatorPositionItem.Text = "1";
            this.bindingNavigatorPositionItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.bindingNavigatorPositionItem.ToolTipText = "position";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(39, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.bindingNavigatorCountItem.ToolTipText = "count";
            // 
            // bsUsers
            // 
            this.bsUsers.AllowAdd = true;
            this.bsUsers.AllowDelete = true;
            this.bsUsers.AllowPrint = true;
            this.bsUsers.AllowUpdate = true;
            this.bsUsers.AutoApply = false;
            this.bsUsers.AutoApplyMaster = false;
            this.bsUsers.AutoDisableControl = false;
            this.bsUsers.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.bsUsers.AutoRecordLock = false;
            this.bsUsers.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.bsUsers.CloseProtect = false;
            this.bsUsers.DataMember = "userInfo";
            this.bsUsers.DataSource = this.dsUsers;
            this.bsUsers.DelayInterval = 300;
            this.bsUsers.DisableKeyFields = false;
            this.bsUsers.EnableFlag = false;
            this.bsUsers.FocusedControl = null;
            this.bsUsers.OwnerComp = null;
            this.bsUsers.RelationDelay = false;
            this.bsUsers.ServerModifyCache = false;
            this.bsUsers.text = "bsUsers";
            // 
            // dsUsers
            // 
            this.dsUsers.Active = true;
            this.dsUsers.AlwaysClose = false;
            this.dsUsers.DataCompressed = false;
            this.dsUsers.DeleteIncomplete = true;
            this.dsUsers.LastKeyValues = null;
            this.dsUsers.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsUsers.PacketRecords = -1;
            this.dsUsers.Position = -1;
            this.dsUsers.RemoteName = "GLModule.userInfo";
            this.dsUsers.ServerModify = false;
            // 
            // rvUsers
            // 
            this.rvUsers.ActiveBox = null;
            this.rvUsers.ActiveColumn = null;
            this.rvUsers.AllowAddData = false;
            this.rvUsers.AlwaysClose = false;
            this.rvUsers.AutoGridSize = false;
            this.rvUsers.AutoLocate = true;
            this.rvUsers.Caption = null;
            this.rvUsers.CheckData = false;
            this.rvUsers.DataSource = this.bsUsers;
            this.rvUsers.DisplayMember = "USERNAME";
            this.rvUsers.EditingDisplayMember = "USERID";
            this.rvUsers.FLookupValue = "";
            this.rvUsers.Font = new System.Drawing.Font("SimSun", 9F);
            this.rvUsers.FormSize = new System.Drawing.Size(410, 255);
            this.rvUsers.IgnoreCase = false;
            this.rvUsers.MultiLanguage = false;
            this.rvUsers.PacketRecords = -1;
            this.rvUsers.RefByWhere = false;
            this.rvUsers.SelectAlias = null;
            this.rvUsers.SelectCommand = null;
            this.rvUsers.SelectTop = null;
            this.rvUsers.Styles = Srvtools.InfoRefVal.ShowStyle.gridStyle;
            this.rvUsers.ValueMember = "USERID";
            this.rvUsers.WhereItemCache = true;
            // 
            // dvRoleAgent
            // 
            this.dvRoleAgent.BindingSource = this.bsRoleAgent;
            this.dvRoleAgent.CarryOn = false;
            this.dvRoleAgent.CheckKeyFieldEmpty = true;
            this.dvRoleAgent.DefaultActive = true;
            this.dvRoleAgent.DuplicateCheck = false;
            this.dvRoleAgent.DuplicateCheckMode = Srvtools.DefaultValidate.DupCheckMode.ByLocal;
            fieldItem1.CarryOn = false;
            fieldItem1.CheckNull = false;
            fieldItem1.CheckRangeFrom = "";
            fieldItem1.CheckRangeTo = "";
            fieldItem1.DefaultValue = "DefRoleId()";
            fieldItem1.FieldName = "ROLE_ID";
            fieldItem1.Validate = "";
            fieldItem1.ValidateLabelLink = "";
            fieldItem1.WarningMsg = "";
            fieldItem2.CarryOn = false;
            fieldItem2.CheckNull = false;
            fieldItem2.CheckRangeFrom = "";
            fieldItem2.CheckRangeTo = "";
            fieldItem2.DefaultValue = "000000";
            fieldItem2.FieldName = "START_TIME";
            fieldItem2.Validate = "";
            fieldItem2.ValidateLabelLink = "";
            fieldItem2.WarningMsg = "";
            fieldItem3.CarryOn = false;
            fieldItem3.CheckNull = false;
            fieldItem3.CheckRangeFrom = "";
            fieldItem3.CheckRangeTo = "";
            fieldItem3.DefaultValue = "235959";
            fieldItem3.FieldName = "END_TIME";
            fieldItem3.Validate = "";
            fieldItem3.ValidateLabelLink = "";
            fieldItem3.WarningMsg = "";
            fieldItem4.CarryOn = false;
            fieldItem4.CheckNull = false;
            fieldItem4.CheckRangeFrom = "";
            fieldItem4.CheckRangeTo = "";
            fieldItem4.DefaultValue = "*";
            fieldItem4.FieldName = "FLOW_DESC";
            fieldItem4.Validate = "";
            fieldItem4.ValidateLabelLink = "";
            fieldItem4.WarningMsg = "";
            this.dvRoleAgent.FieldItems.Add(fieldItem1);
            this.dvRoleAgent.FieldItems.Add(fieldItem2);
            this.dvRoleAgent.FieldItems.Add(fieldItem3);
            this.dvRoleAgent.FieldItems.Add(fieldItem4);
            this.dvRoleAgent.LeaveValidation = false;
            this.dvRoleAgent.MultiLanguage = false;
            this.dvRoleAgent.ValidActive = false;
            this.dvRoleAgent.ValidateChar = "*";
            this.dvRoleAgent.ValidateColor = System.Drawing.Color.Red;
            this.dvRoleAgent.ValidateListBox = null;
            this.dvRoleAgent.ValidateMode = Srvtools.DefaultValidate.ValidMode.All;
            // 
            // txtRoleId
            // 
            this.txtRoleId.BackColor = System.Drawing.SystemColors.Window;
            this.txtRoleId.Location = new System.Drawing.Point(68, 18);
            this.txtRoleId.Name = "txtRoleId";
            this.txtRoleId.ReadOnly = true;
            this.txtRoleId.Size = new System.Drawing.Size(116, 21);
            this.txtRoleId.TabIndex = 3;
            this.txtRoleId.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtRoleName);
            this.panel1.Controls.Add(this.lblRoleName);
            this.panel1.Controls.Add(this.lblRoleId);
            this.panel1.Controls.Add(this.txtRoleId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(729, 45);
            this.panel1.TabIndex = 6;
            // 
            // txtRoleName
            // 
            this.txtRoleName.BackColor = System.Drawing.SystemColors.Window;
            this.txtRoleName.Location = new System.Drawing.Point(286, 18);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.ReadOnly = true;
            this.txtRoleName.Size = new System.Drawing.Size(116, 21);
            this.txtRoleName.TabIndex = 6;
            this.txtRoleName.WordWrap = false;
            // 
            // lblRoleName
            // 
            this.lblRoleName.AutoSize = true;
            this.lblRoleName.Location = new System.Drawing.Point(217, 21);
            this.lblRoleName.Name = "lblRoleName";
            this.lblRoleName.Size = new System.Drawing.Size(53, 12);
            this.lblRoleName.TabIndex = 5;
            this.lblRoleName.Text = "RoleName";
            // 
            // lblRoleId
            // 
            this.lblRoleId.AutoSize = true;
            this.lblRoleId.Location = new System.Drawing.Point(12, 21);
            this.lblRoleId.Name = "lblRoleId";
            this.lblRoleId.Size = new System.Drawing.Size(41, 12);
            this.lblRoleId.TabIndex = 4;
            this.lblRoleId.Text = "RoleId";
            // 
            // infoDataGridView1
            // 
            this.infoDataGridView1.AutoGenerateColumns = false;
            this.infoDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAgent,
            this.colFlowDesc,
            this.colSDate,
            this.colSTime,
            this.colEDate,
            this.colETime,
            this.colParAgent,
            this.colRemark});
            this.infoDataGridView1.DataSource = this.bsRoleAgent;
            this.infoDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoDataGridView1.EnterEnable = true;
            this.infoDataGridView1.EnterRefValControl = false;
            this.infoDataGridView1.Location = new System.Drawing.Point(0, 70);
            this.infoDataGridView1.Name = "infoDataGridView1";
            this.infoDataGridView1.RowTemplate.Height = 23;
            this.infoDataGridView1.Size = new System.Drawing.Size(729, 260);
            this.infoDataGridView1.SureDelete = false;
            this.infoDataGridView1.TabIndex = 7;
            this.infoDataGridView1.TotalActive = false;
            this.infoDataGridView1.TotalBackColor = System.Drawing.SystemColors.Info;
            this.infoDataGridView1.TotalCaption = null;
            this.infoDataGridView1.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.infoDataGridView1.TotalFont = new System.Drawing.Font("SimSun", 9F);
            // 
            // colAgent
            // 
            this.colAgent.DataPropertyName = "AGENT";
            this.colAgent.DataSource = this.dsUsers;
            this.colAgent.DisplayMember = "USERNAME";
            this.colAgent.ExternalRefVal = null;
            this.colAgent.HeaderCellStyle = dataGridViewCellStyle1;
            this.colAgent.HeaderText = "AGENT";
            this.colAgent.Name = "colAgent";
            this.colAgent.RefValue = this.rvUsers;
            this.colAgent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAgent.ValueMember = "USERID";
            // 
            // colFlowDesc
            // 
            this.colFlowDesc.DataPropertyName = "FLOW_DESC";
            this.colFlowDesc.HeaderText = "FLOW_DESC";
            this.colFlowDesc.Name = "colFlowDesc";
            this.colFlowDesc.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFlowDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colSDate
            // 
            this.colSDate.DataPropertyName = "START_DATE";
            this.colSDate.HeaderCellStyle = dataGridViewCellStyle2;
            this.colSDate.HeaderText = "START_DATE";
            this.colSDate.Name = "colSDate";
            this.colSDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSDate.ShowCheckBox = false;
            this.colSDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSDate.Width = 80;
            // 
            // colSTime
            // 
            this.colSTime.DataPropertyName = "START_TIME";
            this.colSTime.HeaderText = "START_TIME";
            this.colSTime.Name = "colSTime";
            this.colSTime.Width = 80;
            // 
            // colEDate
            // 
            this.colEDate.DataPropertyName = "END_DATE";
            this.colEDate.HeaderCellStyle = dataGridViewCellStyle3;
            this.colEDate.HeaderText = "END_DATE";
            this.colEDate.Name = "colEDate";
            this.colEDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEDate.ShowCheckBox = false;
            this.colEDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEDate.Width = 80;
            // 
            // colETime
            // 
            this.colETime.DataPropertyName = "END_TIME";
            this.colETime.HeaderText = "END_TIME";
            this.colETime.Name = "colETime";
            this.colETime.Width = 80;
            // 
            // colParAgent
            // 
            this.colParAgent.DataPropertyName = "PAR_AGENT";
            this.colParAgent.HeaderText = "PAR_AGENT";
            this.colParAgent.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.colParAgent.Name = "colParAgent";
            this.colParAgent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colParAgent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colRemark
            // 
            this.colRemark.DataPropertyName = "REMARK";
            this.colRemark.HeaderText = "REMARK";
            this.colRemark.Name = "colRemark";
            // 
            // frmAgent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 330);
            this.Controls.Add(this.infoDataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.navRoleAgent);
            this.Name = "frmAgent";
            this.Text = "frmAgent";
            this.Load += new System.EventHandler(this.frmAgent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsRoleAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRoleAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navRoleAgent)).EndInit();
            this.navRoleAgent.ResumeLayout(false);
            this.navRoleAgent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rvUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvRoleAgent)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private InfoDataSet dsRoleAgent;
        private InfoBindingSource bsRoleAgent;
        private InfoNavigator navRoleAgent;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAbortItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorApplyItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorCancelItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorEditItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorExportItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorOKItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator3;
        private System.Windows.Forms.ToolStripButton bindingNavigatorRefreshItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorQueryItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorPrintItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator4;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private DefaultValidate dvRoleAgent;
        private InfoDataSet dsUsers;
        private InfoBindingSource bsUsers;
        private InfoRefVal rvUsers;
        private System.Windows.Forms.TextBox txtRoleId;
        private System.Windows.Forms.Panel panel1;
        private InfoDataGridView infoDataGridView1;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label lblRoleName;
        private System.Windows.Forms.Label lblRoleId;
        private InfoDataGridViewRefValColumn colAgent;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFlowDesc;
        private InfoDataGridViewCalendarColumn colSDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTime;
        private InfoDataGridViewCalendarColumn colEDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colETime;
        private System.Windows.Forms.DataGridViewComboBoxColumn colParAgent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
    }
}