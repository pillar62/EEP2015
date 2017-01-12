namespace CMasterDetail
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Srvtools.StateItem stateItem1 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem2 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem3 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem4 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem5 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem6 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem7 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem8 = new Srvtools.StateItem();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Master = new Srvtools.InfoDataSet(this.components);
            this.ibsMaster = new Srvtools.InfoBindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.infoNavigator1 = new Srvtools.InfoNavigator();
            this.bindingNavigatorAbortItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorApplyItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCancelItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorEditItem = new System.Windows.Forms.ToolStripButton();
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
            this.infoStatusStrip1 = new Srvtools.InfoStatusStrip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.infoDataGridView1 = new Srvtools.InfoDataGridView();
            this.ibsDetail = new Srvtools.InfoBindingSource(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.Master)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsMaster)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoNavigator1)).BeginInit();
            this.infoNavigator1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // Master
            // 
            this.Master.Active = false;
            this.Master.AlwaysClose = false;
            this.Master.PacketRecords = 100;
            this.Master.Position = -1;
            this.Master.RemoteName = null;
            this.Master.ServerModify = false;
            // 
            // ibsMaster
            // 
            this.ibsMaster.AutoApply = true;
            this.ibsMaster.AutoApplyMaster = false;
            this.ibsMaster.AutoDisibleControl = false;
            this.ibsMaster.CloseProtect = false;
            this.ibsMaster.DataSource = this.Master;
            this.ibsMaster.DelayInterval = 300;
            this.ibsMaster.EnableFlag = false;
            this.ibsMaster.FocusedControl = null;
            this.ibsMaster.OwnerComp = null;
            this.ibsMaster.Position = 0;
            this.ibsMaster.RelationDelay = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.infoNavigator1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(644, 197);
            this.panel1.TabIndex = 1;
            // 
            // infoNavigator1
            // 
            this.infoNavigator1.AbortItem = this.bindingNavigatorAbortItem;
            this.infoNavigator1.AddNewItem = this.bindingNavigatorAddNewItem;
            this.infoNavigator1.ApplyItem = this.bindingNavigatorApplyItem;
            this.infoNavigator1.BindingSource = this.ibsMaster;
            this.infoNavigator1.CancelItem = this.bindingNavigatorCancelItem;
            this.infoNavigator1.CountItem = null;
            this.infoNavigator1.DeleteItem = this.bindingNavigatorDeleteItem;
            this.infoNavigator1.DescriptionItem = this.toolStripLabel1;
            this.infoNavigator1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.infoNavigator1.EditItem = this.bindingNavigatorEditItem;
            this.infoNavigator1.ForeColors = System.Drawing.Color.Empty;
            this.infoNavigator1.GetServerText = true;
            this.infoNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.bindingNavigatorSeparator4,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.toolStripLabel1});
            this.infoNavigator1.Location = new System.Drawing.Point(0, 0);
            this.infoNavigator1.MoveFirstItem = null;
            this.infoNavigator1.MoveLastItem = null;
            this.infoNavigator1.MoveNextItem = null;
            this.infoNavigator1.MovePreviousItem = null;
            this.infoNavigator1.Name = "infoNavigator1";
            this.infoNavigator1.OKItem = this.bindingNavigatorOKItem;
            this.infoNavigator1.PositionItem = null;
            this.infoNavigator1.PreQueryCondition = ((System.Collections.Generic.List<string>)(resources.GetObject("infoNavigator1.PreQueryCondition")));
            this.infoNavigator1.PreQueryField = ((System.Collections.Generic.List<string>)(resources.GetObject("infoNavigator1.PreQueryField")));
            this.infoNavigator1.PreQueryValue = ((System.Collections.Generic.List<string>)(resources.GetObject("infoNavigator1.PreQueryValue")));
            this.infoNavigator1.PrintItem = this.bindingNavigatorPrintItem;
            this.infoNavigator1.QueryKeepCondition = false;
            this.infoNavigator1.QuerySQLSend = true;
            this.infoNavigator1.Size = new System.Drawing.Size(644, 35);
            stateItem1.Description = "起始";
            stateItem1.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem1.EnabledControls")));
            stateItem1.EnabledControlsEdited = false;
            stateItem1.StateText = "Initial";
            stateItem2.Description = "瀏覽";
            stateItem2.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem2.EnabledControls")));
            stateItem2.EnabledControlsEdited = false;
            stateItem2.StateText = "Browsed";
            stateItem3.Description = "新增中";
            stateItem3.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem3.EnabledControls")));
            stateItem3.EnabledControlsEdited = false;
            stateItem3.StateText = "Inserting";
            stateItem4.Description = "編輯中";
            stateItem4.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem4.EnabledControls")));
            stateItem4.EnabledControlsEdited = false;
            stateItem4.StateText = "Editing";
            stateItem5.Description = "存檔中";
            stateItem5.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem5.EnabledControls")));
            stateItem5.EnabledControlsEdited = false;
            stateItem5.StateText = "Applying";
            stateItem6.Description = "已改變";
            stateItem6.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem6.EnabledControls")));
            stateItem6.EnabledControlsEdited = false;
            stateItem6.StateText = "Changing";
            stateItem7.Description = "查詢中";
            stateItem7.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem7.EnabledControls")));
            stateItem7.EnabledControlsEdited = false;
            stateItem7.StateText = "Querying";
            stateItem8.Description = "列印中";
            stateItem8.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem8.EnabledControls")));
            stateItem8.EnabledControlsEdited = false;
            stateItem8.StateText = "Printing";
            this.infoNavigator1.States.Add(stateItem1);
            this.infoNavigator1.States.Add(stateItem2);
            this.infoNavigator1.States.Add(stateItem3);
            this.infoNavigator1.States.Add(stateItem4);
            this.infoNavigator1.States.Add(stateItem5);
            this.infoNavigator1.States.Add(stateItem6);
            this.infoNavigator1.States.Add(stateItem7);
            this.infoNavigator1.States.Add(stateItem8);
            this.infoNavigator1.StatusStrip = this.infoStatusStrip1;
            this.infoNavigator1.SureDelete = true;
            this.infoNavigator1.SureDeleteText = "Are you sure to delete current record?";
            this.infoNavigator1.SureInsert = false;
            this.infoNavigator1.SureInsertText = "Are you sure to insert record?";
            this.infoNavigator1.TabIndex = 0;
            this.infoNavigator1.Text = "infoNavigator1";
            this.infoNavigator1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.infoNavigator1.ViewBindingSource = null;
            this.infoNavigator1.ViewCountItem = this.bindingNavigatorCountItem;
            this.infoNavigator1.ViewCountItemFormat = "of {0}";
            this.infoNavigator1.ViewMoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.infoNavigator1.ViewMoveLastItem = this.bindingNavigatorMoveLastItem;
            this.infoNavigator1.ViewMoveNextItem = this.bindingNavigatorMoveNextItem;
            this.infoNavigator1.ViewMovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.infoNavigator1.ViewPositionItem = this.bindingNavigatorPositionItem;
            this.infoNavigator1.ViewQueryItem = this.bindingNavigatorQueryItem;
            this.infoNavigator1.ViewRefreshItem = this.bindingNavigatorRefreshItem;
            // 
            // bindingNavigatorAbortItem
            // 
            this.bindingNavigatorAbortItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAbortItem.Image")));
            this.bindingNavigatorAbortItem.Name = "bindingNavigatorAbortItem";
            this.bindingNavigatorAbortItem.Size = new System.Drawing.Size(33, 32);
            this.bindingNavigatorAbortItem.Text = "abort";
            this.bindingNavigatorAbortItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(26, 32);
            this.bindingNavigatorAddNewItem.Text = "add";
            this.bindingNavigatorAddNewItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorApplyItem
            // 
            this.bindingNavigatorApplyItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorApplyItem.Image")));
            this.bindingNavigatorApplyItem.Name = "bindingNavigatorApplyItem";
            this.bindingNavigatorApplyItem.Size = new System.Drawing.Size(35, 32);
            this.bindingNavigatorApplyItem.Text = "apply";
            this.bindingNavigatorApplyItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorCancelItem
            // 
            this.bindingNavigatorCancelItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorCancelItem.Image")));
            this.bindingNavigatorCancelItem.Name = "bindingNavigatorCancelItem";
            this.bindingNavigatorCancelItem.Size = new System.Drawing.Size(38, 32);
            this.bindingNavigatorCancelItem.Text = "cancel";
            this.bindingNavigatorCancelItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(36, 32);
            this.bindingNavigatorDeleteItem.Text = "delete";
            this.bindingNavigatorDeleteItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(77, 12);
            this.toolStripLabel1.Text = "toolStripLabel1";
            this.toolStripLabel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorEditItem
            // 
            this.bindingNavigatorEditItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorEditItem.Image")));
            this.bindingNavigatorEditItem.Name = "bindingNavigatorEditItem";
            this.bindingNavigatorEditItem.Size = new System.Drawing.Size(26, 32);
            this.bindingNavigatorEditItem.Text = "edit";
            this.bindingNavigatorEditItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(27, 32);
            this.bindingNavigatorMoveFirstItem.Text = "first";
            this.bindingNavigatorMoveFirstItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(49, 32);
            this.bindingNavigatorMovePreviousItem.Text = "previous";
            this.bindingNavigatorMovePreviousItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(29, 32);
            this.bindingNavigatorMoveNextItem.Text = "next";
            this.bindingNavigatorMoveNextItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(24, 32);
            this.bindingNavigatorMoveLastItem.Text = "last";
            this.bindingNavigatorMoveLastItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // bindingNavigatorOKItem
            // 
            this.bindingNavigatorOKItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorOKItem.Image")));
            this.bindingNavigatorOKItem.Name = "bindingNavigatorOKItem";
            this.bindingNavigatorOKItem.Size = new System.Drawing.Size(23, 32);
            this.bindingNavigatorOKItem.Text = "ok";
            this.bindingNavigatorOKItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorSeparator3
            // 
            this.bindingNavigatorSeparator3.Name = "bindingNavigatorSeparator3";
            this.bindingNavigatorSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // bindingNavigatorRefreshItem
            // 
            this.bindingNavigatorRefreshItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorRefreshItem.Image")));
            this.bindingNavigatorRefreshItem.Name = "bindingNavigatorRefreshItem";
            this.bindingNavigatorRefreshItem.Size = new System.Drawing.Size(41, 32);
            this.bindingNavigatorRefreshItem.Text = "refresh";
            this.bindingNavigatorRefreshItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorQueryItem
            // 
            this.bindingNavigatorQueryItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorQueryItem.Image")));
            this.bindingNavigatorQueryItem.Name = "bindingNavigatorQueryItem";
            this.bindingNavigatorQueryItem.Size = new System.Drawing.Size(36, 32);
            this.bindingNavigatorQueryItem.Text = "query";
            this.bindingNavigatorQueryItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorPrintItem
            // 
            this.bindingNavigatorPrintItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorPrintItem.Image")));
            this.bindingNavigatorPrintItem.Name = "bindingNavigatorPrintItem";
            this.bindingNavigatorPrintItem.Size = new System.Drawing.Size(31, 32);
            this.bindingNavigatorPrintItem.Text = "print";
            this.bindingNavigatorPrintItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // bindingNavigatorSeparator4
            // 
            this.bindingNavigatorSeparator4.Name = "bindingNavigatorSeparator4";
            this.bindingNavigatorSeparator4.Size = new System.Drawing.Size(6, 35);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 35);
            this.bindingNavigatorPositionItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bindingNavigatorPositionItem.ToolTipText = "position";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(34, 32);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bindingNavigatorCountItem.ToolTipText = "count";
            // 
            // infoStatusStrip1
            // 
            this.infoStatusStrip1.Location = new System.Drawing.Point(0, 207);
            this.infoStatusStrip1.Name = "infoStatusStrip1";
            this.infoStatusStrip1.ShowCompany = false;
            this.infoStatusStrip1.ShowDate = true;
            this.infoStatusStrip1.ShowEEPAlias = true;
            this.infoStatusStrip1.ShowNavigatorStatus = true;
            this.infoStatusStrip1.ShowSolution = true;
            this.infoStatusStrip1.ShowUserID = true;
            this.infoStatusStrip1.ShowUserName = true;
            this.infoStatusStrip1.Size = new System.Drawing.Size(644, 22);
            this.infoStatusStrip1.TabIndex = 2;
            this.infoStatusStrip1.Text = "infoStatusStrip1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.infoStatusStrip1);
            this.panel2.Controls.Add(this.infoDataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 197);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(644, 229);
            this.panel2.TabIndex = 2;
            // 
            // infoDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.infoDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.infoDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoDataGridView1.EnterEnable = true;
            this.infoDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.infoDataGridView1.Name = "infoDataGridView1";
            this.infoDataGridView1.RowHeadersWidth = 25;
            this.infoDataGridView1.RowTemplate.Height = 23;
            this.infoDataGridView1.Size = new System.Drawing.Size(644, 229);
            this.infoDataGridView1.SureDelete = false;
            this.infoDataGridView1.TabIndex = 0;
            this.infoDataGridView1.TotalActive = false;
            this.infoDataGridView1.TotalBackColor = System.Drawing.SystemColors.Info;
            this.infoDataGridView1.TotalCaption = null;
            this.infoDataGridView1.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.infoDataGridView1.TotalFont = new System.Drawing.Font("SimSun", 9F);
            this.infoDataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.infoDataGridView1_CellContentClick);
            // 
            // ibsDetail
            // 
            this.ibsDetail.AutoApply = false;
            this.ibsDetail.AutoApplyMaster = false;
            this.ibsDetail.AutoDisibleControl = false;
            this.ibsDetail.CloseProtect = false;
            this.ibsDetail.DataSource = this.ibsMaster;
            this.ibsDetail.DelayInterval = 300;
            this.ibsDetail.EnableFlag = false;
            this.ibsDetail.FocusedControl = null;
            this.ibsDetail.OwnerComp = null;
            this.ibsDetail.Position = 0;
            this.ibsDetail.RelationDelay = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 197);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(644, 2);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(644, 426);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Master)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsMaster)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoNavigator1)).EndInit();
            this.infoNavigator1.ResumeLayout(false);
            this.infoNavigator1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Srvtools.InfoDataSet Master;
        private Srvtools.InfoBindingSource ibsMaster;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private Srvtools.InfoDataGridView infoDataGridView1;
        private Srvtools.InfoBindingSource ibsDetail;
        private Srvtools.InfoNavigator infoNavigator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAbortItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorApplyItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorCancelItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorEditItem;
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
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Srvtools.InfoStatusStrip infoStatusStrip1;
    }
}
