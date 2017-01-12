//#define UseFL
namespace EEPManager
{
    partial class frmSecurityMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSecurityMain));
            Srvtools.StateItem stateItem1 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem2 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem3 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem4 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem5 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem6 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem7 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem8 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem9 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem10 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem11 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem12 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem13 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem14 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem15 = new Srvtools.StateItem();
            Srvtools.StateItem stateItem16 = new Srvtools.StateItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpUG = new System.Windows.Forms.TabPage();
            this.ugControl1 = new Srvtools.UGControl();
            this.tpMG = new System.Windows.Forms.TabPage();
            this.mgControl1 = new Srvtools.MGControl();
#if UseFL
            this.tpOrg = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbOrgKind = new System.Windows.Forms.ComboBox();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.tView = new System.Windows.Forms.TreeView();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblOrgNo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvOrgRoles = new Srvtools.InfoDataGridView();
            this.colRoleId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsOrgRoles = new Srvtools.InfoBindingSource(this.components);
            this.dsOrgRoles = new Srvtools.InfoDataSet(this.components);
            this.btnRoleAdd = new System.Windows.Forms.Button();
            this.btnRoleDelete = new System.Windows.Forms.Button();
            this.lblOrgDesc = new System.Windows.Forms.Label();
            this.lblUpperOrg = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblOrgManager = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLevelNo = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtOrgNo = new System.Windows.Forms.TextBox();
            this.btnOrgDelete = new System.Windows.Forms.Button();
            this.txtOrgDesc = new System.Windows.Forms.TextBox();
            this.btnOrgUpdate = new System.Windows.Forms.Button();
            this.cmbUpperOrg = new System.Windows.Forms.ComboBox();
            this.btnOrgAdd = new System.Windows.Forms.Button();
            this.cmbOrgManager = new System.Windows.Forms.ComboBox();
            this.cmbLevelNo = new System.Windows.Forms.ComboBox();
            this.tpOL = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvOrgLevel = new Srvtools.InfoDataGridView();
            this.colLevelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevelDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsOrgLevel = new Srvtools.InfoBindingSource(this.components);
            this.dsOrgLevel = new Srvtools.InfoDataSet(this.components);
            this.navOrgLevel = new Srvtools.InfoNavigator();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvOrgKind = new Srvtools.InfoDataGridView();
            this.colOrgKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKindDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsOrgKind = new Srvtools.InfoBindingSource(this.components);
            this.dsOrgKind = new Srvtools.InfoDataSet(this.components);
            this.navOrgKind = new Srvtools.InfoNavigator();
            this.bindingNavigatorAbortItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorAddNewItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorApplyItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCancelItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorEditItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorExportItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveNextItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorOKItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator32 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorRefreshItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorQueryItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorPrintItem2 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator42 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem2 = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem2 = new System.Windows.Forms.ToolStripLabel();
#endif
            this.tabControl.SuspendLayout();
            this.tpUG.SuspendLayout();
            this.tpMG.SuspendLayout();
#if UseFL
            this.tpOrg.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrgRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOrgRoles)).BeginInit();
            this.tpOL.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrgLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOrgLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navOrgLevel)).BeginInit();
            this.navOrgLevel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrgKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOrgKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navOrgKind)).BeginInit();
            this.navOrgKind.SuspendLayout();
#endif
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpUG);
            this.tabControl.Controls.Add(this.tpMG);
#if UseFL
            this.tabControl.Controls.Add(this.tpOrg);
            this.tabControl.Controls.Add(this.tpOL);
#endif
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(640, 525);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tpUG
            // 
            this.tpUG.Controls.Add(this.ugControl1);
            this.tpUG.Location = new System.Drawing.Point(4, 24);
            this.tpUG.Name = "tpUG";
            this.tpUG.Padding = new System.Windows.Forms.Padding(3);
            this.tpUG.Size = new System.Drawing.Size(632, 497);
            this.tpUG.TabIndex = 0;
            this.tpUG.Text = "Users & Groups";
            this.tpUG.UseVisualStyleBackColor = true;
            // 
            // ugControl1
            // 
            this.ugControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugControl1.Location = new System.Drawing.Point(3, 3);
            this.ugControl1.Name = "ugControl1";
            this.ugControl1.Size = new System.Drawing.Size(626, 491);
            this.ugControl1.TabIndex = 0;
            // 
            // tpMG
            // 
            this.tpMG.Controls.Add(this.mgControl1);
            this.tpMG.Location = new System.Drawing.Point(4, 24);
            this.tpMG.Name = "tpMG";
            this.tpMG.Padding = new System.Windows.Forms.Padding(3);
            this.tpMG.Size = new System.Drawing.Size(632, 497);
            this.tpMG.TabIndex = 1;
            this.tpMG.Text = "Menu Utility";
            this.tpMG.UseVisualStyleBackColor = true;
#if UseFL
            // 
            // tpOrg
            // 
            this.tpOrg.Controls.Add(this.panel3);
            this.tpOrg.Controls.Add(this.panel2);
            this.tpOrg.Location = new System.Drawing.Point(4, 24);
            this.tpOrg.Name = "tpOrg";
            this.tpOrg.Size = new System.Drawing.Size(632, 497);
            this.tpOrg.TabIndex = 2;
            this.tpOrg.Text = "Organization";
            this.tpOrg.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbOrgKind);
            this.panel3.Controls.Add(this.txtQuery);
            this.panel3.Controls.Add(this.tView);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.btnReload);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(280, 497);
            this.panel3.TabIndex = 26;
            // 
            // cmbOrgKind
            // 
            this.cmbOrgKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrgKind.FormattingEnabled = true;
            this.cmbOrgKind.Location = new System.Drawing.Point(5, 14);
            this.cmbOrgKind.Name = "cmbOrgKind";
            this.cmbOrgKind.Size = new System.Drawing.Size(255, 23);
            this.cmbOrgKind.TabIndex = 0;
            this.cmbOrgKind.SelectedIndexChanged += new System.EventHandler(this.cmbOrgKind_SelectedIndexChanged);
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtQuery.Location = new System.Drawing.Point(5, 433);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(194, 21);
            this.txtQuery.TabIndex = 24;
            // 
            // tView
            // 
            this.tView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tView.Location = new System.Drawing.Point(5, 48);
            this.tView.Name = "tView";
            this.tView.Size = new System.Drawing.Size(269, 367);
            this.tView.TabIndex = 1;
            this.tView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tView_AfterSelect);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuery.Location = new System.Drawing.Point(205, 434);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(53, 25);
            this.btnQuery.TabIndex = 13;
            this.btnQuery.Text = "query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReload.Location = new System.Drawing.Point(109, 461);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(62, 25);
            this.btnReload.TabIndex = 12;
            this.btnReload.Text = "reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblOrgNo);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.lblOrgDesc);
            this.panel2.Controls.Add(this.lblUpperOrg);
            this.panel2.Controls.Add(this.lblUser);
            this.panel2.Controls.Add(this.lblOrgManager);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.lblLevelNo);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.txtOrgNo);
            this.panel2.Controls.Add(this.btnOrgDelete);
            this.panel2.Controls.Add(this.txtOrgDesc);
            this.panel2.Controls.Add(this.btnOrgUpdate);
            this.panel2.Controls.Add(this.cmbUpperOrg);
            this.panel2.Controls.Add(this.btnOrgAdd);
            this.panel2.Controls.Add(this.cmbOrgManager);
            this.panel2.Controls.Add(this.cmbLevelNo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(280, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(352, 497);
            this.panel2.TabIndex = 26;
            // 
            // lblOrgNo
            // 
            this.lblOrgNo.AutoSize = true;
            this.lblOrgNo.Location = new System.Drawing.Point(12, 17);
            this.lblOrgNo.Name = "lblOrgNo";
            this.lblOrgNo.Size = new System.Drawing.Size(43, 15);
            this.lblOrgNo.TabIndex = 2;
            this.lblOrgNo.Text = "OrgNo";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgvOrgRoles);
            this.panel1.Controls.Add(this.btnRoleAdd);
            this.panel1.Controls.Add(this.btnRoleDelete);
            this.panel1.Location = new System.Drawing.Point(11, 209);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 249);
            this.panel1.TabIndex = 25;
            // 
            // dgvOrgRoles
            // 
            this.dgvOrgRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrgRoles.AutoGenerateColumns = false;
            this.dgvOrgRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrgRoles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRoleId,
            this.colGroupName});
            this.dgvOrgRoles.DataSource = this.bsOrgRoles;
            this.dgvOrgRoles.EnterEnable = true;
            this.dgvOrgRoles.EnterRefValControl = false;
            this.dgvOrgRoles.Location = new System.Drawing.Point(3, 3);
            this.dgvOrgRoles.Name = "dgvOrgRoles";
            this.dgvOrgRoles.RowTemplate.Height = 23;
            this.dgvOrgRoles.Size = new System.Drawing.Size(261, 241);
            this.dgvOrgRoles.SureDelete = false;
            this.dgvOrgRoles.TabIndex = 22;
            this.dgvOrgRoles.TotalActive = false;
            this.dgvOrgRoles.TotalBackColor = System.Drawing.SystemColors.Info;
            this.dgvOrgRoles.TotalCaption = null;
            this.dgvOrgRoles.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.dgvOrgRoles.TotalFont = new System.Drawing.Font("SimSun", 9F);
            this.dgvOrgRoles.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrgRoles_CellValueChanged);
            // 
            // colRoleId
            // 
            this.colRoleId.DataPropertyName = "ROLE_ID";
            this.colRoleId.HeaderText = "ROLE_ID";
            this.colRoleId.Name = "colRoleId";
            this.colRoleId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colGroupName
            // 
            this.colGroupName.DataPropertyName = "GROUPNAME";
            this.colGroupName.HeaderText = "GROUPNAME";
            this.colGroupName.Name = "colGroupName";
            this.colGroupName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // bsOrgRoles
            // 
            this.bsOrgRoles.AllowAdd = true;
            this.bsOrgRoles.AllowDelete = true;
            this.bsOrgRoles.AllowPrint = true;
            this.bsOrgRoles.AllowUpdate = true;
            this.bsOrgRoles.AutoApply = false;
            this.bsOrgRoles.AutoApplyMaster = false;
            this.bsOrgRoles.AutoDisableControl = false;
            this.bsOrgRoles.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.bsOrgRoles.AutoRecordLock = false;
            this.bsOrgRoles.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.bsOrgRoles.CloseProtect = false;
            this.bsOrgRoles.DataMember = "cmdOrgRoles";
            this.bsOrgRoles.DataSource = this.dsOrgRoles;
            this.bsOrgRoles.DelayInterval = 300;
            this.bsOrgRoles.DisableKeyFields = false;
            this.bsOrgRoles.EnableFlag = false;
            this.bsOrgRoles.FocusedControl = null;
            this.bsOrgRoles.OwnerComp = null;
            this.bsOrgRoles.RelationDelay = false;
            this.bsOrgRoles.ServerModifyCache = false;
            this.bsOrgRoles.text = "bsOrgRoles";
            // 
            // dsOrgRoles
            // 
            this.dsOrgRoles.Active = true;
            this.dsOrgRoles.AlwaysClose = false;
            this.dsOrgRoles.DataCompressed = false;
            this.dsOrgRoles.DeleteIncomplete = true;
            this.dsOrgRoles.LastKeyValues = null;
            this.dsOrgRoles.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsOrgRoles.PacketRecords = -1;
            this.dsOrgRoles.Position = -1;
            this.dsOrgRoles.RemoteName = "GLModule.cmdOrgRoles";
            this.dsOrgRoles.ServerModify = false;
            // 
            // btnRoleAdd
            // 
            this.btnRoleAdd.Location = new System.Drawing.Point(270, 3);
            this.btnRoleAdd.Name = "btnRoleAdd";
            this.btnRoleAdd.Size = new System.Drawing.Size(58, 23);
            this.btnRoleAdd.TabIndex = 15;
            this.btnRoleAdd.Text = "add";
            this.btnRoleAdd.UseVisualStyleBackColor = true;
            this.btnRoleAdd.Click += new System.EventHandler(this.btnRoleAdd_Click);
            // 
            // btnRoleDelete
            // 
            this.btnRoleDelete.Location = new System.Drawing.Point(270, 32);
            this.btnRoleDelete.Name = "btnRoleDelete";
            this.btnRoleDelete.Size = new System.Drawing.Size(59, 23);
            this.btnRoleDelete.TabIndex = 16;
            this.btnRoleDelete.Text = "delete";
            this.btnRoleDelete.UseVisualStyleBackColor = true;
            this.btnRoleDelete.Click += new System.EventHandler(this.btnRoleDelete_Click);
            // 
            // lblOrgDesc
            // 
            this.lblOrgDesc.AutoSize = true;
            this.lblOrgDesc.Location = new System.Drawing.Point(12, 49);
            this.lblOrgDesc.Name = "lblOrgDesc";
            this.lblOrgDesc.Size = new System.Drawing.Size(55, 15);
            this.lblOrgDesc.TabIndex = 3;
            this.lblOrgDesc.Text = "OrgDesc";
            // 
            // lblUpperOrg
            // 
            this.lblUpperOrg.AutoSize = true;
            this.lblUpperOrg.Location = new System.Drawing.Point(12, 80);
            this.lblUpperOrg.Name = "lblUpperOrg";
            this.lblUpperOrg.Size = new System.Drawing.Size(61, 15);
            this.lblUpperOrg.TabIndex = 4;
            this.lblUpperOrg.Text = "UpperOrg";
            // 
            // lblUser
            // 
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUser.Location = new System.Drawing.Point(98, 143);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(225, 23);
            this.lblUser.TabIndex = 23;
            // 
            // lblOrgManager
            // 
            this.lblOrgManager.AutoSize = true;
            this.lblOrgManager.Location = new System.Drawing.Point(12, 113);
            this.lblOrgManager.Name = "lblOrgManager";
            this.lblOrgManager.Size = new System.Drawing.Size(77, 15);
            this.lblOrgManager.TabIndex = 5;
            this.lblOrgManager.Text = "OrgManager";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(286, 464);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 25);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblLevelNo
            // 
            this.lblLevelNo.AutoSize = true;
            this.lblLevelNo.Location = new System.Drawing.Point(12, 179);
            this.lblLevelNo.Name = "lblLevelNo";
            this.lblLevelNo.Size = new System.Drawing.Size(52, 15);
            this.lblLevelNo.TabIndex = 6;
            this.lblLevelNo.Text = "LevelNo";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(218, 464);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(58, 25);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtOrgNo
            // 
            this.txtOrgNo.Location = new System.Drawing.Point(98, 14);
            this.txtOrgNo.Name = "txtOrgNo";
            this.txtOrgNo.Size = new System.Drawing.Size(225, 21);
            this.txtOrgNo.TabIndex = 7;
            this.txtOrgNo.TextChanged += new System.EventHandler(this.txtOrgNo_TextChanged);
            // 
            // btnOrgDelete
            // 
            this.btnOrgDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOrgDelete.Location = new System.Drawing.Point(150, 464);
            this.btnOrgDelete.Name = "btnOrgDelete";
            this.btnOrgDelete.Size = new System.Drawing.Size(53, 25);
            this.btnOrgDelete.TabIndex = 19;
            this.btnOrgDelete.Text = "delete";
            this.btnOrgDelete.UseVisualStyleBackColor = true;
            this.btnOrgDelete.Click += new System.EventHandler(this.btnOrgDelete_Click);
            // 
            // txtOrgDesc
            // 
            this.txtOrgDesc.Location = new System.Drawing.Point(98, 46);
            this.txtOrgDesc.Name = "txtOrgDesc";
            this.txtOrgDesc.Size = new System.Drawing.Size(225, 21);
            this.txtOrgDesc.TabIndex = 8;
            // 
            // btnOrgUpdate
            // 
            this.btnOrgUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOrgUpdate.Location = new System.Drawing.Point(85, 464);
            this.btnOrgUpdate.Name = "btnOrgUpdate";
            this.btnOrgUpdate.Size = new System.Drawing.Size(53, 25);
            this.btnOrgUpdate.TabIndex = 18;
            this.btnOrgUpdate.Text = "update";
            this.btnOrgUpdate.UseVisualStyleBackColor = true;
            this.btnOrgUpdate.Click += new System.EventHandler(this.btnOrgUpdate_Click);
            // 
            // cmbUpperOrg
            // 
            this.cmbUpperOrg.FormattingEnabled = true;
            this.cmbUpperOrg.Location = new System.Drawing.Point(98, 77);
            this.cmbUpperOrg.Name = "cmbUpperOrg";
            this.cmbUpperOrg.Size = new System.Drawing.Size(225, 23);
            this.cmbUpperOrg.TabIndex = 9;
            // 
            // btnOrgAdd
            // 
            this.btnOrgAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOrgAdd.Location = new System.Drawing.Point(20, 464);
            this.btnOrgAdd.Name = "btnOrgAdd";
            this.btnOrgAdd.Size = new System.Drawing.Size(53, 25);
            this.btnOrgAdd.TabIndex = 17;
            this.btnOrgAdd.Text = "add";
            this.btnOrgAdd.UseVisualStyleBackColor = true;
            this.btnOrgAdd.Click += new System.EventHandler(this.btnOrgAdd_Click);
            // 
            // cmbOrgManager
            // 
            this.cmbOrgManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrgManager.FormattingEnabled = true;
            this.cmbOrgManager.Location = new System.Drawing.Point(98, 110);
            this.cmbOrgManager.Name = "cmbOrgManager";
            this.cmbOrgManager.Size = new System.Drawing.Size(225, 23);
            this.cmbOrgManager.TabIndex = 10;
            this.cmbOrgManager.SelectedIndexChanged += new System.EventHandler(this.cmbOrgManager_SelectedIndexChanged);
            // 
            // cmbLevelNo
            // 
            this.cmbLevelNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevelNo.FormattingEnabled = true;
            this.cmbLevelNo.Location = new System.Drawing.Point(98, 176);
            this.cmbLevelNo.Name = "cmbLevelNo";
            this.cmbLevelNo.Size = new System.Drawing.Size(225, 23);
            this.cmbLevelNo.TabIndex = 11;
            // 
            // tpOL
            // 
            this.tpOL.Controls.Add(this.tabControl1);
            this.tpOL.Location = new System.Drawing.Point(4, 24);
            this.tpOL.Name = "tpOL";
            this.tpOL.Size = new System.Drawing.Size(632, 497);
            this.tpOL.TabIndex = 3;
            this.tpOL.Text = "OrgLevel";
            this.tpOL.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(192, 72);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvOrgLevel);
            this.tabPage1.Controls.Add(this.navOrgLevel);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(184, 41);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "OrgLevel";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvOrgLevel
            // 
            this.dgvOrgLevel.AutoGenerateColumns = false;
            this.dgvOrgLevel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrgLevel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLevelNo,
            this.colLevelDesc});
            this.dgvOrgLevel.DataSource = this.bsOrgLevel;
            this.dgvOrgLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrgLevel.EnterEnable = true;
            this.dgvOrgLevel.EnterRefValControl = false;
            this.dgvOrgLevel.Location = new System.Drawing.Point(3, 28);
            this.dgvOrgLevel.Name = "dgvOrgLevel";
            this.dgvOrgLevel.RowTemplate.Height = 23;
            this.dgvOrgLevel.Size = new System.Drawing.Size(178, 10);
            this.dgvOrgLevel.SureDelete = false;
            this.dgvOrgLevel.TabIndex = 1;
            this.dgvOrgLevel.TotalActive = false;
            this.dgvOrgLevel.TotalBackColor = System.Drawing.SystemColors.Info;
            this.dgvOrgLevel.TotalCaption = null;
            this.dgvOrgLevel.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.dgvOrgLevel.TotalFont = new System.Drawing.Font("SimSun", 9F);
            // 
            // colLevelNo
            // 
            this.colLevelNo.DataPropertyName = "LEVEL_NO";
            this.colLevelNo.HeaderText = "LEVEL_NO";
            this.colLevelNo.Name = "colLevelNo";
            this.colLevelNo.Width = 150;
            // 
            // colLevelDesc
            // 
            this.colLevelDesc.DataPropertyName = "LEVEL_DESC";
            this.colLevelDesc.HeaderText = "LEVEL_DESC";
            this.colLevelDesc.Name = "colLevelDesc";
            this.colLevelDesc.Width = 150;
            // 
            // bsOrgLevel
            // 
            this.bsOrgLevel.AllowAdd = true;
            this.bsOrgLevel.AllowDelete = true;
            this.bsOrgLevel.AllowPrint = true;
            this.bsOrgLevel.AllowUpdate = true;
            this.bsOrgLevel.AutoApply = true;
            this.bsOrgLevel.AutoApplyMaster = false;
            this.bsOrgLevel.AutoDisableControl = false;
            this.bsOrgLevel.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.bsOrgLevel.AutoRecordLock = false;
            this.bsOrgLevel.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.bsOrgLevel.CloseProtect = false;
            this.bsOrgLevel.DataMember = "cmdOrgLevel";
            this.bsOrgLevel.DataSource = this.dsOrgLevel;
            this.bsOrgLevel.DelayInterval = 300;
            this.bsOrgLevel.DisableKeyFields = false;
            this.bsOrgLevel.EnableFlag = false;
            this.bsOrgLevel.FocusedControl = null;
            this.bsOrgLevel.OwnerComp = null;
            this.bsOrgLevel.RelationDelay = false;
            this.bsOrgLevel.ServerModifyCache = false;
            this.bsOrgLevel.text = "bsOrgLevel";
            // 
            // dsOrgLevel
            // 
            this.dsOrgLevel.Active = true;
            this.dsOrgLevel.AlwaysClose = false;
            this.dsOrgLevel.DataCompressed = false;
            this.dsOrgLevel.DeleteIncomplete = true;
            this.dsOrgLevel.LastKeyValues = null;
            this.dsOrgLevel.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsOrgLevel.PacketRecords = -1;
            this.dsOrgLevel.Position = -1;
            this.dsOrgLevel.RemoteName = "GLModule.cmdOrgLevel";
            this.dsOrgLevel.ServerModify = false;
            // 
            // navOrgLevel
            // 
            this.navOrgLevel.AbortItem = this.bindingNavigatorAbortItem;
            this.navOrgLevel.AddNewItem = this.bindingNavigatorAddNewItem;
            this.navOrgLevel.AnyQueryID = "";
            this.navOrgLevel.ApplyItem = this.bindingNavigatorApplyItem;
            this.navOrgLevel.BindingSource = this.bsOrgLevel;
            this.navOrgLevel.CancelItem = this.bindingNavigatorCancelItem;
            this.navOrgLevel.CopyItem = null;
            this.navOrgLevel.CountItem = null;
            this.navOrgLevel.DeleteItem = this.bindingNavigatorDeleteItem;
            this.navOrgLevel.DescriptionItem = null;
            this.navOrgLevel.DetailBindingSource = null;
            this.navOrgLevel.DetailKeyField = null;
            this.navOrgLevel.EditItem = this.bindingNavigatorEditItem;
            this.navOrgLevel.ExportItem = this.bindingNavigatorExportItem;
            this.navOrgLevel.ForeColors = System.Drawing.Color.Empty;
            this.navOrgLevel.GetRealRecordsCount = false;
            this.navOrgLevel.GetServerText = false;
            this.navOrgLevel.HideItemStates = false;
            this.navOrgLevel.InternalQuery = true;
            this.navOrgLevel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.navOrgLevel.Location = new System.Drawing.Point(3, 3);
            this.navOrgLevel.MoveFirstItem = null;
            this.navOrgLevel.MoveLastItem = null;
            this.navOrgLevel.MoveNextItem = null;
            this.navOrgLevel.MovePreviousItem = null;
            this.navOrgLevel.MultiLanguage = false;
            this.navOrgLevel.Name = "navOrgLevel";
            this.navOrgLevel.OKItem = this.bindingNavigatorOKItem;
            this.navOrgLevel.PositionItem = null;
            this.navOrgLevel.PreQueryCondition = ((System.Collections.Generic.List<string>)(resources.GetObject("navOrgLevel.PreQueryCondition")));
            this.navOrgLevel.PreQueryField = ((System.Collections.Generic.List<string>)(resources.GetObject("navOrgLevel.PreQueryField")));
            this.navOrgLevel.PreQueryValue = ((System.Collections.Generic.List<string>)(resources.GetObject("navOrgLevel.PreQueryValue")));
            this.navOrgLevel.PrintItem = this.bindingNavigatorPrintItem;
            this.navOrgLevel.QueryFont = new System.Drawing.Font("SimSun", 9F);
            this.navOrgLevel.QueryKeepCondition = false;
            this.navOrgLevel.QueryMargin = new System.Drawing.Printing.Margins(100, 30, 10, 30);
            this.navOrgLevel.QueryMode = Srvtools.InfoNavigator.QueryModeType.ClientQuery;
            this.navOrgLevel.QuerySQLSend = true;
            this.navOrgLevel.Size = new System.Drawing.Size(178, 25);
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
            this.navOrgLevel.States.Add(stateItem1);
            this.navOrgLevel.States.Add(stateItem2);
            this.navOrgLevel.States.Add(stateItem3);
            this.navOrgLevel.States.Add(stateItem4);
            this.navOrgLevel.States.Add(stateItem5);
            this.navOrgLevel.States.Add(stateItem6);
            this.navOrgLevel.States.Add(stateItem7);
            this.navOrgLevel.States.Add(stateItem8);
            this.navOrgLevel.StatusStrip = null;
            this.navOrgLevel.SureAbort = false;
            this.navOrgLevel.SureDelete = true;
            this.navOrgLevel.SureDeleteText = null;
            this.navOrgLevel.SureInsert = false;
            this.navOrgLevel.SureInsertText = null;
            this.navOrgLevel.TabIndex = 2;
            this.navOrgLevel.Text = "infoNavigator1";
            this.navOrgLevel.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.navOrgLevel.ViewBindingSource = null;
            this.navOrgLevel.ViewCountItem = this.bindingNavigatorCountItem;
            this.navOrgLevel.ViewCountItemFormat = "of {0}";
            this.navOrgLevel.ViewMoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.navOrgLevel.ViewMoveLastItem = this.bindingNavigatorMoveLastItem;
            this.navOrgLevel.ViewMoveNextItem = this.bindingNavigatorMoveNextItem;
            this.navOrgLevel.ViewMovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.navOrgLevel.ViewPositionItem = this.bindingNavigatorPositionItem;
            this.navOrgLevel.ViewQueryItem = this.bindingNavigatorQueryItem;
            this.navOrgLevel.ViewRefreshItem = this.bindingNavigatorRefreshItem;
            this.navOrgLevel.ViewScrollProtect = false;
            // 
            // bindingNavigatorAbortItem
            // 
            this.bindingNavigatorAbortItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAbortItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAbortItem.Image")));
            this.bindingNavigatorAbortItem.Name = "bindingNavigatorAbortItem";
            this.bindingNavigatorAbortItem.Size = new System.Drawing.Size(23, 20);
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
            this.bindingNavigatorApplyItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorApplyItem.Text = "apply";
            this.bindingNavigatorApplyItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorCancelItem
            // 
            this.bindingNavigatorCancelItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorCancelItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorCancelItem.Image")));
            this.bindingNavigatorCancelItem.Name = "bindingNavigatorCancelItem";
            this.bindingNavigatorCancelItem.Size = new System.Drawing.Size(23, 20);
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
            this.bindingNavigatorEditItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorEditItem.Text = "edit";
            this.bindingNavigatorEditItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorExportItem
            // 
            this.bindingNavigatorExportItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorExportItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorExportItem.Image")));
            this.bindingNavigatorExportItem.Name = "bindingNavigatorExportItem";
            this.bindingNavigatorExportItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorExportItem.Text = "export";
            this.bindingNavigatorExportItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "first";
            this.bindingNavigatorMoveFirstItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
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
            this.bindingNavigatorOKItem.Size = new System.Drawing.Size(23, 20);
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
            this.bindingNavigatorRefreshItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorRefreshItem.Text = "refresh";
            this.bindingNavigatorRefreshItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorQueryItem
            // 
            this.bindingNavigatorQueryItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorQueryItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorQueryItem.Image")));
            this.bindingNavigatorQueryItem.Name = "bindingNavigatorQueryItem";
            this.bindingNavigatorQueryItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorQueryItem.Text = "query";
            this.bindingNavigatorQueryItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorPrintItem
            // 
            this.bindingNavigatorPrintItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorPrintItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorPrintItem.Image")));
            this.bindingNavigatorPrintItem.Name = "bindingNavigatorPrintItem";
            this.bindingNavigatorPrintItem.Size = new System.Drawing.Size(23, 20);
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
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.bindingNavigatorPositionItem.ToolTipText = "position";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(39, 17);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.bindingNavigatorCountItem.ToolTipText = "count";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvOrgKind);
            this.tabPage2.Controls.Add(this.navOrgKind);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(184, 41);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "OrgKind";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvOrgKind
            // 
            this.dgvOrgKind.AutoGenerateColumns = false;
            this.dgvOrgKind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrgKind.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrgKind,
            this.colKindDesc});
            this.dgvOrgKind.DataSource = this.bsOrgKind;
            this.dgvOrgKind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrgKind.EnterEnable = true;
            this.dgvOrgKind.EnterRefValControl = false;
            this.dgvOrgKind.Location = new System.Drawing.Point(3, 28);
            this.dgvOrgKind.Name = "dgvOrgKind";
            this.dgvOrgKind.RowTemplate.Height = 23;
            this.dgvOrgKind.Size = new System.Drawing.Size(178, 10);
            this.dgvOrgKind.SureDelete = false;
            this.dgvOrgKind.TabIndex = 2;
            this.dgvOrgKind.TotalActive = false;
            this.dgvOrgKind.TotalBackColor = System.Drawing.SystemColors.Info;
            this.dgvOrgKind.TotalCaption = null;
            this.dgvOrgKind.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.dgvOrgKind.TotalFont = new System.Drawing.Font("SimSun", 9F);
            // 
            // colOrgKind
            // 
            this.colOrgKind.DataPropertyName = "ORG_KIND";
            this.colOrgKind.HeaderText = "ORG_KIND";
            this.colOrgKind.Name = "colOrgKind";
            this.colOrgKind.Width = 150;
            // 
            // colKindDesc
            // 
            this.colKindDesc.DataPropertyName = "KIND_DESC";
            this.colKindDesc.HeaderText = "KIND_DESC";
            this.colKindDesc.Name = "colKindDesc";
            this.colKindDesc.Width = 150;
            // 
            // bsOrgKind
            // 
            this.bsOrgKind.AllowAdd = true;
            this.bsOrgKind.AllowDelete = true;
            this.bsOrgKind.AllowPrint = true;
            this.bsOrgKind.AllowUpdate = true;
            this.bsOrgKind.AutoApply = true;
            this.bsOrgKind.AutoApplyMaster = false;
            this.bsOrgKind.AutoDisableControl = false;
            this.bsOrgKind.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.bsOrgKind.AutoRecordLock = false;
            this.bsOrgKind.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.bsOrgKind.CloseProtect = false;
            this.bsOrgKind.DataMember = "cmdOrgKind";
            this.bsOrgKind.DataSource = this.dsOrgKind;
            this.bsOrgKind.DelayInterval = 300;
            this.bsOrgKind.DisableKeyFields = false;
            this.bsOrgKind.EnableFlag = false;
            this.bsOrgKind.FocusedControl = null;
            this.bsOrgKind.OwnerComp = null;
            this.bsOrgKind.RelationDelay = false;
            this.bsOrgKind.ServerModifyCache = false;
            this.bsOrgKind.text = "bsOrgKind";
            // 
            // dsOrgKind
            // 
            this.dsOrgKind.Active = true;
            this.dsOrgKind.AlwaysClose = false;
            this.dsOrgKind.DataCompressed = false;
            this.dsOrgKind.DeleteIncomplete = true;
            this.dsOrgKind.LastKeyValues = null;
            this.dsOrgKind.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsOrgKind.PacketRecords = -1;
            this.dsOrgKind.Position = -1;
            this.dsOrgKind.RemoteName = "GLModule.cmdOrgKind";
            this.dsOrgKind.ServerModify = false;
            // 
            // navOrgKind
            // 
            this.navOrgKind.AbortItem = this.bindingNavigatorAbortItem2;
            this.navOrgKind.AddNewItem = this.bindingNavigatorAddNewItem2;
            this.navOrgKind.AnyQueryID = "";
            this.navOrgKind.ApplyItem = this.bindingNavigatorApplyItem2;
            this.navOrgKind.BindingSource = this.bsOrgKind;
            this.navOrgKind.CancelItem = this.bindingNavigatorCancelItem2;
            this.navOrgKind.CopyItem = null;
            this.navOrgKind.CountItem = null;
            this.navOrgKind.DeleteItem = this.bindingNavigatorDeleteItem2;
            this.navOrgKind.DescriptionItem = null;
            this.navOrgKind.DetailBindingSource = null;
            this.navOrgKind.DetailKeyField = null;
            this.navOrgKind.EditItem = this.bindingNavigatorEditItem2;
            this.navOrgKind.ExportItem = this.bindingNavigatorExportItem2;
            this.navOrgKind.ForeColors = System.Drawing.Color.Empty;
            this.navOrgKind.GetRealRecordsCount = false;
            this.navOrgKind.GetServerText = false;
            this.navOrgKind.HideItemStates = false;
            this.navOrgKind.InternalQuery = true;
            this.navOrgKind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem2,
            this.bindingNavigatorMovePreviousItem2,
            this.bindingNavigatorMoveNextItem2,
            this.bindingNavigatorMoveLastItem2,
            this.bindingNavigatorSeparator12,
            this.bindingNavigatorAddNewItem2,
            this.bindingNavigatorDeleteItem2,
            this.bindingNavigatorEditItem2,
            this.bindingNavigatorSeparator22,
            this.bindingNavigatorOKItem2,
            this.bindingNavigatorCancelItem2,
            this.bindingNavigatorApplyItem2,
            this.bindingNavigatorAbortItem2,
            this.bindingNavigatorSeparator32,
            this.bindingNavigatorRefreshItem2,
            this.bindingNavigatorQueryItem2,
            this.bindingNavigatorPrintItem2,
            this.bindingNavigatorExportItem2,
            this.bindingNavigatorSeparator42,
            this.bindingNavigatorPositionItem2,
            this.bindingNavigatorCountItem2});
            this.navOrgKind.Location = new System.Drawing.Point(3, 3);
            this.navOrgKind.MoveFirstItem = null;
            this.navOrgKind.MoveLastItem = null;
            this.navOrgKind.MoveNextItem = null;
            this.navOrgKind.MovePreviousItem = null;
            this.navOrgKind.MultiLanguage = false;
            this.navOrgKind.Name = "navOrgKind";
            this.navOrgKind.OKItem = this.bindingNavigatorOKItem2;
            this.navOrgKind.PositionItem = null;
            this.navOrgKind.PreQueryCondition = ((System.Collections.Generic.List<string>)(resources.GetObject("navOrgKind.PreQueryCondition")));
            this.navOrgKind.PreQueryField = ((System.Collections.Generic.List<string>)(resources.GetObject("navOrgKind.PreQueryField")));
            this.navOrgKind.PreQueryValue = ((System.Collections.Generic.List<string>)(resources.GetObject("navOrgKind.PreQueryValue")));
            this.navOrgKind.PrintItem = this.bindingNavigatorPrintItem2;
            this.navOrgKind.QueryFont = new System.Drawing.Font("SimSun", 9F);
            this.navOrgKind.QueryKeepCondition = false;
            this.navOrgKind.QueryMargin = new System.Drawing.Printing.Margins(100, 30, 10, 30);
            this.navOrgKind.QueryMode = Srvtools.InfoNavigator.QueryModeType.ClientQuery;
            this.navOrgKind.QuerySQLSend = true;
            this.navOrgKind.Size = new System.Drawing.Size(178, 25);
            stateItem9.Description = null;
            stateItem9.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem9.EnabledControls")));
            stateItem9.EnabledControlsEdited = false;
            stateItem9.StateText = "Initial";
            stateItem10.Description = null;
            stateItem10.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem10.EnabledControls")));
            stateItem10.EnabledControlsEdited = false;
            stateItem10.StateText = "Browsed";
            stateItem11.Description = null;
            stateItem11.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem11.EnabledControls")));
            stateItem11.EnabledControlsEdited = false;
            stateItem11.StateText = "Inserting";
            stateItem12.Description = null;
            stateItem12.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem12.EnabledControls")));
            stateItem12.EnabledControlsEdited = false;
            stateItem12.StateText = "Editing";
            stateItem13.Description = null;
            stateItem13.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem13.EnabledControls")));
            stateItem13.EnabledControlsEdited = false;
            stateItem13.StateText = "Applying";
            stateItem14.Description = null;
            stateItem14.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem14.EnabledControls")));
            stateItem14.EnabledControlsEdited = false;
            stateItem14.StateText = "Changing";
            stateItem15.Description = null;
            stateItem15.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem15.EnabledControls")));
            stateItem15.EnabledControlsEdited = false;
            stateItem15.StateText = "Querying";
            stateItem16.Description = null;
            stateItem16.EnabledControls = ((System.Collections.Generic.List<string>)(resources.GetObject("stateItem16.EnabledControls")));
            stateItem16.EnabledControlsEdited = false;
            stateItem16.StateText = "Printing";
            this.navOrgKind.States.Add(stateItem9);
            this.navOrgKind.States.Add(stateItem10);
            this.navOrgKind.States.Add(stateItem11);
            this.navOrgKind.States.Add(stateItem12);
            this.navOrgKind.States.Add(stateItem13);
            this.navOrgKind.States.Add(stateItem14);
            this.navOrgKind.States.Add(stateItem15);
            this.navOrgKind.States.Add(stateItem16);
            this.navOrgKind.StatusStrip = null;
            this.navOrgKind.SureAbort = false;
            this.navOrgKind.SureDelete = true;
            this.navOrgKind.SureDeleteText = null;
            this.navOrgKind.SureInsert = false;
            this.navOrgKind.SureInsertText = null;
            this.navOrgKind.TabIndex = 1;
            this.navOrgKind.Text = "infoNavigator1";
            this.navOrgKind.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.navOrgKind.ViewBindingSource = null;
            this.navOrgKind.ViewCountItem = this.bindingNavigatorCountItem2;
            this.navOrgKind.ViewCountItemFormat = "of {0}";
            this.navOrgKind.ViewMoveFirstItem = this.bindingNavigatorMoveFirstItem2;
            this.navOrgKind.ViewMoveLastItem = this.bindingNavigatorMoveLastItem2;
            this.navOrgKind.ViewMoveNextItem = this.bindingNavigatorMoveNextItem2;
            this.navOrgKind.ViewMovePreviousItem = this.bindingNavigatorMovePreviousItem2;
            this.navOrgKind.ViewPositionItem = this.bindingNavigatorPositionItem2;
            this.navOrgKind.ViewQueryItem = this.bindingNavigatorQueryItem2;
            this.navOrgKind.ViewRefreshItem = this.bindingNavigatorRefreshItem2;
            this.navOrgKind.ViewScrollProtect = false;
            // 
            // bindingNavigatorAbortItem2
            // 
            this.bindingNavigatorAbortItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAbortItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAbortItem2.Image")));
            this.bindingNavigatorAbortItem2.Name = "bindingNavigatorAbortItem2";
            this.bindingNavigatorAbortItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorAbortItem2.Text = "abort";
            this.bindingNavigatorAbortItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorAddNewItem2
            // 
            this.bindingNavigatorAddNewItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem2.Image")));
            this.bindingNavigatorAddNewItem2.Name = "bindingNavigatorAddNewItem2";
            this.bindingNavigatorAddNewItem2.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem2.Text = "add";
            this.bindingNavigatorAddNewItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorApplyItem2
            // 
            this.bindingNavigatorApplyItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorApplyItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorApplyItem2.Image")));
            this.bindingNavigatorApplyItem2.Name = "bindingNavigatorApplyItem2";
            this.bindingNavigatorApplyItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorApplyItem2.Text = "apply";
            this.bindingNavigatorApplyItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorCancelItem2
            // 
            this.bindingNavigatorCancelItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorCancelItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorCancelItem2.Image")));
            this.bindingNavigatorCancelItem2.Name = "bindingNavigatorCancelItem2";
            this.bindingNavigatorCancelItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorCancelItem2.Text = "cancel";
            this.bindingNavigatorCancelItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorDeleteItem2
            // 
            this.bindingNavigatorDeleteItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem2.Image")));
            this.bindingNavigatorDeleteItem2.Name = "bindingNavigatorDeleteItem2";
            this.bindingNavigatorDeleteItem2.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem2.Text = "delete";
            this.bindingNavigatorDeleteItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorEditItem2
            // 
            this.bindingNavigatorEditItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorEditItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorEditItem2.Image")));
            this.bindingNavigatorEditItem2.Name = "bindingNavigatorEditItem2";
            this.bindingNavigatorEditItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorEditItem2.Text = "edit";
            this.bindingNavigatorEditItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorExportItem2
            // 
            this.bindingNavigatorExportItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorExportItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorExportItem2.Image")));
            this.bindingNavigatorExportItem2.Name = "bindingNavigatorExportItem2";
            this.bindingNavigatorExportItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorExportItem2.Text = "export";
            this.bindingNavigatorExportItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveFirstItem2
            // 
            this.bindingNavigatorMoveFirstItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem2.Image")));
            this.bindingNavigatorMoveFirstItem2.Name = "bindingNavigatorMoveFirstItem2";
            this.bindingNavigatorMoveFirstItem2.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem2.Text = "first";
            this.bindingNavigatorMoveFirstItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMovePreviousItem2
            // 
            this.bindingNavigatorMovePreviousItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem2.Image")));
            this.bindingNavigatorMovePreviousItem2.Name = "bindingNavigatorMovePreviousItem2";
            this.bindingNavigatorMovePreviousItem2.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem2.Text = "previous";
            this.bindingNavigatorMovePreviousItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveNextItem2
            // 
            this.bindingNavigatorMoveNextItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem2.Enabled = false;
            this.bindingNavigatorMoveNextItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem2.Image")));
            this.bindingNavigatorMoveNextItem2.Name = "bindingNavigatorMoveNextItem2";
            this.bindingNavigatorMoveNextItem2.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem2.Text = "next";
            this.bindingNavigatorMoveNextItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorMoveLastItem2
            // 
            this.bindingNavigatorMoveLastItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem2.Enabled = false;
            this.bindingNavigatorMoveLastItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem2.Image")));
            this.bindingNavigatorMoveLastItem2.Name = "bindingNavigatorMoveLastItem2";
            this.bindingNavigatorMoveLastItem2.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem2.Text = "last";
            this.bindingNavigatorMoveLastItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorSeparator12
            // 
            this.bindingNavigatorSeparator12.Name = "bindingNavigatorSeparator12";
            this.bindingNavigatorSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorSeparator22
            // 
            this.bindingNavigatorSeparator22.Name = "bindingNavigatorSeparator22";
            this.bindingNavigatorSeparator22.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorOKItem2
            // 
            this.bindingNavigatorOKItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorOKItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorOKItem2.Image")));
            this.bindingNavigatorOKItem2.Name = "bindingNavigatorOKItem2";
            this.bindingNavigatorOKItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorOKItem2.Text = "ok";
            this.bindingNavigatorOKItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorSeparator32
            // 
            this.bindingNavigatorSeparator32.Name = "bindingNavigatorSeparator32";
            this.bindingNavigatorSeparator32.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorRefreshItem2
            // 
            this.bindingNavigatorRefreshItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorRefreshItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorRefreshItem2.Image")));
            this.bindingNavigatorRefreshItem2.Name = "bindingNavigatorRefreshItem2";
            this.bindingNavigatorRefreshItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorRefreshItem2.Text = "refresh";
            this.bindingNavigatorRefreshItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorQueryItem2
            // 
            this.bindingNavigatorQueryItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorQueryItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorQueryItem2.Image")));
            this.bindingNavigatorQueryItem2.Name = "bindingNavigatorQueryItem2";
            this.bindingNavigatorQueryItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorQueryItem2.Text = "query";
            this.bindingNavigatorQueryItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorPrintItem2
            // 
            this.bindingNavigatorPrintItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorPrintItem2.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorPrintItem2.Image")));
            this.bindingNavigatorPrintItem2.Name = "bindingNavigatorPrintItem2";
            this.bindingNavigatorPrintItem2.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorPrintItem2.Text = "print";
            this.bindingNavigatorPrintItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // bindingNavigatorSeparator42
            // 
            this.bindingNavigatorSeparator42.Name = "bindingNavigatorSeparator42";
            this.bindingNavigatorSeparator42.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem2
            // 
            this.bindingNavigatorPositionItem2.Name = "bindingNavigatorPositionItem2";
            this.bindingNavigatorPositionItem2.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem2.Text = "0";
            this.bindingNavigatorPositionItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.bindingNavigatorPositionItem2.ToolTipText = "position";
            // 
            // bindingNavigatorCountItem2
            // 
            this.bindingNavigatorCountItem2.Name = "bindingNavigatorCountItem2";
            this.bindingNavigatorCountItem2.Size = new System.Drawing.Size(39, 17);
            this.bindingNavigatorCountItem2.Text = "of {0}";
            this.bindingNavigatorCountItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.bindingNavigatorCountItem2.ToolTipText = "count";
#endif
            // 
            // mgControl1
            // 
            this.mgControl1.AutoSize = true;
            this.mgControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mgControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mgControl1.Location = new System.Drawing.Point(3, 3);
            this.mgControl1.Margin = new System.Windows.Forms.Padding(4);
            this.mgControl1.Name = "mgControl1";
            this.mgControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mgControl1.Size = new System.Drawing.Size(626, 491);
            this.mgControl1.TabIndex = 3;
            // 
            // frmSecurityMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(640, 525);
            this.Controls.Add(this.tabControl);
            this.Name = "frmSecurityMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Security";
            this.Load += new System.EventHandler(this.frmSecurityMain_Load);
            this.tabControl.ResumeLayout(false);
            this.tpUG.ResumeLayout(false);
            this.tpMG.ResumeLayout(false);
            this.tpMG.PerformLayout();
#if UseFL
            this.tpOrg.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrgRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOrgRoles)).EndInit();
            this.tpOL.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrgLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOrgLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navOrgLevel)).EndInit();
            this.navOrgLevel.ResumeLayout(false);
            this.navOrgLevel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrgKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOrgKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navOrgKind)).EndInit();
            this.navOrgKind.ResumeLayout(false);
            this.navOrgKind.PerformLayout();
#endif
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpUG;
        private Srvtools.UGControl ugControl1;
        private System.Windows.Forms.TabPage tpMG;
#if UseFL
        private System.Windows.Forms.TabPage tpOrg;
        private System.Windows.Forms.TreeView tView;
        private System.Windows.Forms.ComboBox cmbOrgKind;
        private System.Windows.Forms.Label lblLevelNo;
        private System.Windows.Forms.Label lblOrgManager;
        private System.Windows.Forms.Label lblUpperOrg;
        private System.Windows.Forms.Label lblOrgDesc;
        private System.Windows.Forms.Label lblOrgNo;
        private System.Windows.Forms.TextBox txtOrgDesc;
        private System.Windows.Forms.TextBox txtOrgNo;
        private System.Windows.Forms.ComboBox cmbLevelNo;
        private System.Windows.Forms.ComboBox cmbOrgManager;
        private System.Windows.Forms.ComboBox cmbUpperOrg;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnRoleDelete;
        private System.Windows.Forms.Button btnRoleAdd;
        private System.Windows.Forms.Button btnOrgUpdate;
        private System.Windows.Forms.Button btnOrgAdd;
        private System.Windows.Forms.Button btnOrgDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Srvtools.InfoDataGridView dgvOrgRoles;
        private Srvtools.InfoDataSet dsOrgRoles;
        private Srvtools.InfoBindingSource bsOrgRoles;
        private System.Windows.Forms.TabPage tpOL;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Srvtools.InfoDataSet dsOrgLevel;
        private Srvtools.InfoBindingSource bsOrgLevel;
        private Srvtools.InfoBindingSource bsOrgKind;
        private Srvtools.InfoDataGridView dgvOrgLevel;
        private Srvtools.InfoDataGridView dgvOrgKind;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgNo;
        private Srvtools.InfoDataSet dsOrgKind;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevelNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevelDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgKind;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKindDesc;
        private System.Windows.Forms.DataGridViewComboBoxColumn colRoleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroupName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Srvtools.InfoNavigator navOrgKind;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAbortItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorApplyItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorCancelItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorEditItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorExportItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem2;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator12;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator22;
        private System.Windows.Forms.ToolStripButton bindingNavigatorOKItem2;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator32;
        private System.Windows.Forms.ToolStripButton bindingNavigatorRefreshItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorQueryItem2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorPrintItem2;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator42;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem2;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem2;
        private Srvtools.InfoNavigator navOrgLevel;
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
#endif
        private Srvtools.MGControl mgControl1;
    }
}

