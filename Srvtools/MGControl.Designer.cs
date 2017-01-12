namespace Srvtools
{
    partial class MGControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MGControl));
            this.tView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemModify = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.accessUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnApplyTree = new System.Windows.Forms.Button();
            this.btnReloadTree = new System.Windows.Forms.Button();
            this.gbMenuInfo = new System.Windows.Forms.GroupBox();
            this.btnSelectReport = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.preViewImage = new System.Windows.Forms.PictureBox();
            this.lblImageUrl = new System.Windows.Forms.Label();
            this.btnImageUrl = new System.Windows.Forms.Button();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.btnSelImage = new System.Windows.Forms.Button();
            this.cmbModuleType = new System.Windows.Forms.ComboBox();
            this.lblModuleType = new System.Windows.Forms.Label();
            this.btnSelPackage = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSEQ_NO = new System.Windows.Forms.TextBox();
            this.txtItemType = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtForm = new System.Windows.Forms.TextBox();
            this.txtItemParam = new System.Windows.Forms.TextBox();
            this.txtPackage = new System.Windows.Forms.TextBox();
            this.txtParent = new System.Windows.Forms.TextBox();
            this.txtCaption = new System.Windows.Forms.TextBox();
            this.txtMenuID = new System.Windows.Forms.TextBox();
            this.lblForm = new System.Windows.Forms.Label();
            this.lblItemType = new System.Windows.Forms.Label();
            this.lblItemParam = new System.Windows.Forms.Label();
            this.lblSEQ_NO = new System.Windows.Forms.Label();
            this.lblPackage = new System.Windows.Forms.Label();
            this.lblParent = new System.Windows.Forms.Label();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblMenuID = new System.Windows.Forms.Label();
            this.btnGroups = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.cbLanguageID = new System.Windows.Forms.ComboBox();
            this.btnUsers = new System.Windows.Forms.Button();
            this.infoCmbSolution = new System.Windows.Forms.ComboBox();
            this.infodsSolutions = new Srvtools.InfoDataSet(this.components);
            this.infodsMenus = new Srvtools.InfoDataSet(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.accessPageControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.gbMenuInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preViewImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infodsSolutions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infodsMenus)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tView
            // 
            this.tView.AllowDrop = true;
            this.tView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tView.ContextMenuStrip = this.contextMenuStrip;
            this.tView.Location = new System.Drawing.Point(7, 72);
            this.tView.Margin = new System.Windows.Forms.Padding(4);
            this.tView.Name = "tView";
            this.tView.Size = new System.Drawing.Size(234, 329);
            this.tView.TabIndex = 2;
            this.tView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tView_ItemDrag);
            this.tView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tView_AfterSelect);
            this.tView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tView_NodeMouseDoubleClick);
            this.tView.DragDrop += new System.Windows.Forms.DragEventHandler(this.tView_DragDrop);
            this.tView.DragEnter += new System.Windows.Forms.DragEventHandler(this.tView_DragEnter);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemAdd,
            this.ItemDelete,
            this.ItemModify,
            this.ItemGroups,
            this.accessUsersToolStripMenuItem,
            this.accessPageControlToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(196, 158);
            // 
            // ItemAdd
            // 
            this.ItemAdd.Name = "ItemAdd";
            this.ItemAdd.Size = new System.Drawing.Size(195, 22);
            this.ItemAdd.Text = "add";
            this.ItemAdd.Click += new System.EventHandler(this.ItemAdd_Click);
            // 
            // ItemDelete
            // 
            this.ItemDelete.Name = "ItemDelete";
            this.ItemDelete.Size = new System.Drawing.Size(195, 22);
            this.ItemDelete.Text = "delete";
            this.ItemDelete.Click += new System.EventHandler(this.ItemDelete_Click);
            // 
            // ItemModify
            // 
            this.ItemModify.Name = "ItemModify";
            this.ItemModify.Size = new System.Drawing.Size(195, 22);
            this.ItemModify.Text = "modify";
            this.ItemModify.Click += new System.EventHandler(this.ItemModify_Click);
            // 
            // ItemGroups
            // 
            this.ItemGroups.Name = "ItemGroups";
            this.ItemGroups.Size = new System.Drawing.Size(195, 22);
            this.ItemGroups.Text = "Access Groups";
            this.ItemGroups.Click += new System.EventHandler(this.ItemGroups_Click);
            // 
            // accessUsersToolStripMenuItem
            // 
            this.accessUsersToolStripMenuItem.Name = "accessUsersToolStripMenuItem";
            this.accessUsersToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.accessUsersToolStripMenuItem.Text = "Access Users";
            this.accessUsersToolStripMenuItem.Click += new System.EventHandler(this.accessUsersToolStripMenuItem_Click);
            // 
            // btnApplyTree
            // 
            this.btnApplyTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyTree.Location = new System.Drawing.Point(30, 409);
            this.btnApplyTree.Margin = new System.Windows.Forms.Padding(4);
            this.btnApplyTree.Name = "btnApplyTree";
            this.btnApplyTree.Size = new System.Drawing.Size(66, 29);
            this.btnApplyTree.TabIndex = 24;
            this.btnApplyTree.Text = "Apply";
            this.btnApplyTree.Click += new System.EventHandler(this.btnApplyTree_Click);
            // 
            // btnReloadTree
            // 
            this.btnReloadTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReloadTree.Location = new System.Drawing.Point(135, 409);
            this.btnReloadTree.Margin = new System.Windows.Forms.Padding(4);
            this.btnReloadTree.Name = "btnReloadTree";
            this.btnReloadTree.Size = new System.Drawing.Size(66, 29);
            this.btnReloadTree.TabIndex = 25;
            this.btnReloadTree.Text = "Reload";
            this.btnReloadTree.Click += new System.EventHandler(this.btnReloadTree_Click);
            // 
            // gbMenuInfo
            // 
            this.gbMenuInfo.BackColor = System.Drawing.Color.LightYellow;
            this.gbMenuInfo.Controls.Add(this.btnSelectReport);
            this.gbMenuInfo.Controls.Add(this.panel1);
            this.gbMenuInfo.Controls.Add(this.pbImage);
            this.gbMenuInfo.Controls.Add(this.lblImage);
            this.gbMenuInfo.Controls.Add(this.btnSelImage);
            this.gbMenuInfo.Controls.Add(this.cmbModuleType);
            this.gbMenuInfo.Controls.Add(this.lblModuleType);
            this.gbMenuInfo.Controls.Add(this.btnSelPackage);
            this.gbMenuInfo.Controls.Add(this.btnModify);
            this.gbMenuInfo.Controls.Add(this.btnDelete);
            this.gbMenuInfo.Controls.Add(this.btnAdd);
            this.gbMenuInfo.Controls.Add(this.btnCancel);
            this.gbMenuInfo.Controls.Add(this.txtSEQ_NO);
            this.gbMenuInfo.Controls.Add(this.txtItemType);
            this.gbMenuInfo.Controls.Add(this.btnOK);
            this.gbMenuInfo.Controls.Add(this.txtForm);
            this.gbMenuInfo.Controls.Add(this.txtItemParam);
            this.gbMenuInfo.Controls.Add(this.txtPackage);
            this.gbMenuInfo.Controls.Add(this.txtParent);
            this.gbMenuInfo.Controls.Add(this.txtCaption);
            this.gbMenuInfo.Controls.Add(this.txtMenuID);
            this.gbMenuInfo.Controls.Add(this.lblForm);
            this.gbMenuInfo.Controls.Add(this.lblItemType);
            this.gbMenuInfo.Controls.Add(this.lblItemParam);
            this.gbMenuInfo.Controls.Add(this.lblSEQ_NO);
            this.gbMenuInfo.Controls.Add(this.lblPackage);
            this.gbMenuInfo.Controls.Add(this.lblParent);
            this.gbMenuInfo.Controls.Add(this.lblCaption);
            this.gbMenuInfo.Controls.Add(this.lblMenuID);
            this.gbMenuInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMenuInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMenuInfo.Location = new System.Drawing.Point(0, 0);
            this.gbMenuInfo.Margin = new System.Windows.Forms.Padding(4);
            this.gbMenuInfo.Name = "gbMenuInfo";
            this.gbMenuInfo.Padding = new System.Windows.Forms.Padding(4);
            this.gbMenuInfo.Size = new System.Drawing.Size(360, 490);
            this.gbMenuInfo.TabIndex = 26;
            this.gbMenuInfo.TabStop = false;
            this.gbMenuInfo.Text = "Menu Infomation";
            // 
            // btnSelectReport
            // 
            this.btnSelectReport.Location = new System.Drawing.Point(295, 269);
            this.btnSelectReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectReport.Name = "btnSelectReport";
            this.btnSelectReport.Size = new System.Drawing.Size(21, 21);
            this.btnSelectReport.TabIndex = 54;
            this.btnSelectReport.UseVisualStyleBackColor = true;
            this.btnSelectReport.Click += new System.EventHandler(this.btnSelectReport_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.preViewImage);
            this.panel1.Controls.Add(this.lblImageUrl);
            this.panel1.Controls.Add(this.btnImageUrl);
            this.panel1.Controls.Add(this.txtImageUrl);
            this.panel1.Location = new System.Drawing.Point(11, 173);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 49);
            this.panel1.TabIndex = 49;
            // 
            // preViewImage
            // 
            this.preViewImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.preViewImage.Location = new System.Drawing.Point(97, 9);
            this.preViewImage.Name = "preViewImage";
            this.preViewImage.Size = new System.Drawing.Size(35, 32);
            this.preViewImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.preViewImage.TabIndex = 57;
            this.preViewImage.TabStop = false;
            // 
            // lblImageUrl
            // 
            this.lblImageUrl.Location = new System.Drawing.Point(17, 3);
            this.lblImageUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageUrl.Name = "lblImageUrl";
            this.lblImageUrl.Size = new System.Drawing.Size(72, 32);
            this.lblImageUrl.TabIndex = 54;
            this.lblImageUrl.Text = "ImageUrl";
            this.lblImageUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnImageUrl
            // 
            this.btnImageUrl.Location = new System.Drawing.Point(284, 20);
            this.btnImageUrl.Margin = new System.Windows.Forms.Padding(4);
            this.btnImageUrl.Name = "btnImageUrl";
            this.btnImageUrl.Size = new System.Drawing.Size(21, 21);
            this.btnImageUrl.TabIndex = 56;
            this.btnImageUrl.UseVisualStyleBackColor = true;
            this.btnImageUrl.Click += new System.EventHandler(this.btnImageUrl_Click);
            this.btnImageUrl.Paint += new System.Windows.Forms.PaintEventHandler(this.btnImageUrl_Paint);
            // 
            // txtImageUrl
            // 
            this.txtImageUrl.Location = new System.Drawing.Point(133, 20);
            this.txtImageUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtImageUrl.Size = new System.Drawing.Size(151, 21);
            this.txtImageUrl.TabIndex = 55;
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(108, 180);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(40, 26);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 53;
            this.pbImage.TabStop = false;
            // 
            // lblImage
            // 
            this.lblImage.Location = new System.Drawing.Point(38, 178);
            this.lblImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(62, 32);
            this.lblImage.TabIndex = 52;
            this.lblImage.Text = "Icon";
            this.lblImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSelImage
            // 
            this.btnSelImage.Location = new System.Drawing.Point(150, 185);
            this.btnSelImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelImage.Name = "btnSelImage";
            this.btnSelImage.Size = new System.Drawing.Size(21, 21);
            this.btnSelImage.TabIndex = 51;
            this.btnSelImage.UseVisualStyleBackColor = true;
            this.btnSelImage.Click += new System.EventHandler(this.btnSelImage_Click);
            this.btnSelImage.Paint += new System.Windows.Forms.PaintEventHandler(this.btnSelImage_Paint);
            // 
            // cmbModuleType
            // 
            this.cmbModuleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModuleType.FormattingEnabled = true;
            this.cmbModuleType.Location = new System.Drawing.Point(108, 141);
            this.cmbModuleType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbModuleType.Name = "cmbModuleType";
            this.cmbModuleType.Size = new System.Drawing.Size(137, 23);
            this.cmbModuleType.TabIndex = 49;
            this.cmbModuleType.SelectedIndexChanged += new System.EventHandler(this.cmbModuleType_SelectedIndexChanged);
            // 
            // lblModuleType
            // 
            this.lblModuleType.Location = new System.Drawing.Point(8, 138);
            this.lblModuleType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModuleType.Name = "lblModuleType";
            this.lblModuleType.Size = new System.Drawing.Size(92, 29);
            this.lblModuleType.TabIndex = 48;
            this.lblModuleType.Text = "Module Type";
            this.lblModuleType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSelPackage
            // 
            this.btnSelPackage.Location = new System.Drawing.Point(295, 229);
            this.btnSelPackage.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelPackage.Name = "btnSelPackage";
            this.btnSelPackage.Size = new System.Drawing.Size(21, 21);
            this.btnSelPackage.TabIndex = 47;
            this.btnSelPackage.UseVisualStyleBackColor = true;
            this.btnSelPackage.Click += new System.EventHandler(this.btnSelPackage_Click);
            // 
            // btnModify
            // 
            this.btnModify.AutoSize = true;
            this.btnModify.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModify.Location = new System.Drawing.Point(130, 419);
            this.btnModify.Margin = new System.Windows.Forms.Padding(4);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(62, 29);
            this.btnModify.TabIndex = 46;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(69, 419);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 29);
            this.btnDelete.TabIndex = 45;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(10, 419);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 29);
            this.btnAdd.TabIndex = 44;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(267, 419);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 29);
            this.btnCancel.TabIndex = 43;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSEQ_NO
            // 
            this.txtSEQ_NO.Location = new System.Drawing.Point(108, 389);
            this.txtSEQ_NO.Margin = new System.Windows.Forms.Padding(4);
            this.txtSEQ_NO.Name = "txtSEQ_NO";
            this.txtSEQ_NO.Size = new System.Drawing.Size(220, 21);
            this.txtSEQ_NO.TabIndex = 41;
            // 
            // txtItemType
            // 
            this.txtItemType.Location = new System.Drawing.Point(108, 349);
            this.txtItemType.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemType.Name = "txtItemType";
            this.txtItemType.Size = new System.Drawing.Size(220, 21);
            this.txtItemType.TabIndex = 40;
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(200, 419);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(62, 29);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "Ok";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtForm
            // 
            this.txtForm.Location = new System.Drawing.Point(108, 309);
            this.txtForm.Margin = new System.Windows.Forms.Padding(4);
            this.txtForm.Name = "txtForm";
            this.txtForm.Size = new System.Drawing.Size(220, 21);
            this.txtForm.TabIndex = 38;
            // 
            // txtItemParam
            // 
            this.txtItemParam.Location = new System.Drawing.Point(108, 269);
            this.txtItemParam.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemParam.Name = "txtItemParam";
            this.txtItemParam.Size = new System.Drawing.Size(208, 21);
            this.txtItemParam.TabIndex = 37;
            // 
            // txtPackage
            // 
            this.txtPackage.Location = new System.Drawing.Point(108, 229);
            this.txtPackage.Margin = new System.Windows.Forms.Padding(4);
            this.txtPackage.Name = "txtPackage";
            this.txtPackage.Size = new System.Drawing.Size(187, 21);
            this.txtPackage.TabIndex = 36;
            this.txtPackage.TextChanged += new System.EventHandler(this.txtPackage_TextChanged);
            // 
            // txtParent
            // 
            this.txtParent.Location = new System.Drawing.Point(108, 101);
            this.txtParent.Margin = new System.Windows.Forms.Padding(4);
            this.txtParent.Name = "txtParent";
            this.txtParent.Size = new System.Drawing.Size(220, 21);
            this.txtParent.TabIndex = 35;
            // 
            // txtCaption
            // 
            this.txtCaption.Location = new System.Drawing.Point(108, 61);
            this.txtCaption.Margin = new System.Windows.Forms.Padding(4);
            this.txtCaption.Name = "txtCaption";
            this.txtCaption.Size = new System.Drawing.Size(220, 21);
            this.txtCaption.TabIndex = 34;
            // 
            // txtMenuID
            // 
            this.txtMenuID.Location = new System.Drawing.Point(108, 21);
            this.txtMenuID.Margin = new System.Windows.Forms.Padding(4);
            this.txtMenuID.Name = "txtMenuID";
            this.txtMenuID.Size = new System.Drawing.Size(220, 21);
            this.txtMenuID.TabIndex = 33;
            // 
            // lblForm
            // 
            this.lblForm.Location = new System.Drawing.Point(3, 308);
            this.lblForm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForm.Name = "lblForm";
            this.lblForm.Size = new System.Drawing.Size(97, 22);
            this.lblForm.TabIndex = 31;
            this.lblForm.Text = "Form Name";
            this.lblForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblItemType
            // 
            this.lblItemType.Location = new System.Drawing.Point(13, 349);
            this.lblItemType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblItemType.Name = "lblItemType";
            this.lblItemType.Size = new System.Drawing.Size(87, 15);
            this.lblItemType.TabIndex = 30;
            this.lblItemType.Text = "Solution";
            this.lblItemType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblItemParam
            // 
            this.lblItemParam.Location = new System.Drawing.Point(13, 272);
            this.lblItemParam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblItemParam.Name = "lblItemParam";
            this.lblItemParam.Size = new System.Drawing.Size(87, 15);
            this.lblItemParam.TabIndex = 29;
            this.lblItemParam.Text = "Item params";
            this.lblItemParam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSEQ_NO
            // 
            this.lblSEQ_NO.Location = new System.Drawing.Point(13, 392);
            this.lblSEQ_NO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSEQ_NO.Name = "lblSEQ_NO";
            this.lblSEQ_NO.Size = new System.Drawing.Size(87, 15);
            this.lblSEQ_NO.TabIndex = 28;
            this.lblSEQ_NO.Text = "Sequence";
            this.lblSEQ_NO.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPackage
            // 
            this.lblPackage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPackage.Location = new System.Drawing.Point(13, 223);
            this.lblPackage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(87, 29);
            this.lblPackage.TabIndex = 27;
            this.lblPackage.Text = "Package ";
            this.lblPackage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblParent
            // 
            this.lblParent.Location = new System.Drawing.Point(13, 97);
            this.lblParent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParent.Name = "lblParent";
            this.lblParent.Size = new System.Drawing.Size(87, 29);
            this.lblParent.TabIndex = 26;
            this.lblParent.Text = "Parent ID";
            this.lblParent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCaption
            // 
            this.lblCaption.Location = new System.Drawing.Point(13, 57);
            this.lblCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(87, 29);
            this.lblCaption.TabIndex = 25;
            this.lblCaption.Text = "Caption";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMenuID
            // 
            this.lblMenuID.Location = new System.Drawing.Point(13, 17);
            this.lblMenuID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMenuID.Name = "lblMenuID";
            this.lblMenuID.Size = new System.Drawing.Size(87, 29);
            this.lblMenuID.TabIndex = 24;
            this.lblMenuID.Text = "Menu ID";
            this.lblMenuID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGroups
            // 
            this.btnGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGroups.AutoSize = true;
            this.btnGroups.Location = new System.Drawing.Point(120, 447);
            this.btnGroups.Margin = new System.Windows.Forms.Padding(4);
            this.btnGroups.Name = "btnGroups";
            this.btnGroups.Size = new System.Drawing.Size(121, 29);
            this.btnGroups.TabIndex = 48;
            this.btnGroups.Text = "Access Groups";
            this.btnGroups.Click += new System.EventHandler(this.btnGroups_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // cbLanguageID
            // 
            this.cbLanguageID.FormattingEnabled = true;
            this.cbLanguageID.Items.AddRange(new object[] {
            "Default",
            "English",
            "Traditional Chinese",
            "Simplified Chinese",
            "HongKong",
            "Japanese",
            "Korean",
            "User-defined1",
            "User-defined2"});
            this.cbLanguageID.Location = new System.Drawing.Point(7, 39);
            this.cbLanguageID.Name = "cbLanguageID";
            this.cbLanguageID.Size = new System.Drawing.Size(206, 23);
            this.cbLanguageID.TabIndex = 49;
            this.cbLanguageID.Text = "Default";
            this.cbLanguageID.SelectedIndexChanged += new System.EventHandler(this.cbLanguageID_SelectedIndexChanged);
            // 
            // btnUsers
            // 
            this.btnUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUsers.AutoSize = true;
            this.btnUsers.Location = new System.Drawing.Point(3, 447);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(113, 29);
            this.btnUsers.TabIndex = 54;
            this.btnUsers.Text = "Access Users";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // infoCmbSolution
            // 
            this.infoCmbSolution.DataSource = this.infodsSolutions;
            this.infoCmbSolution.DisplayMember = "solutionInfo.ITEMNAME";
            this.infoCmbSolution.FormattingEnabled = true;
            this.infoCmbSolution.Location = new System.Drawing.Point(7, 8);
            this.infoCmbSolution.Margin = new System.Windows.Forms.Padding(4);
            this.infoCmbSolution.Name = "infoCmbSolution";
            this.infoCmbSolution.Size = new System.Drawing.Size(206, 23);
            this.infoCmbSolution.TabIndex = 47;
            this.infoCmbSolution.ValueMember = "solutionInfo.ITEMTYPE";
            this.infoCmbSolution.SelectedIndexChanged += new System.EventHandler(this.infoCmbSolution_SelectedIndexChanged);
            // 
            // infodsSolutions
            // 
            this.infodsSolutions.Active = false;
            this.infodsSolutions.AlwaysClose = false;
            this.infodsSolutions.DataCompressed = false;
            this.infodsSolutions.DeleteIncomplete = true;
            this.infodsSolutions.LastKeyValues = null;
            this.infodsSolutions.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.infodsSolutions.PacketRecords = -1;
            this.infodsSolutions.Position = -1;
            this.infodsSolutions.RemoteName = "GLModule.solutionInfo";
            this.infodsSolutions.ServerModify = false;
            // 
            // infodsMenus
            // 
            this.infodsMenus.Active = false;
            this.infodsMenus.AlwaysClose = true;
            this.infodsMenus.DataCompressed = false;
            this.infodsMenus.DeleteIncomplete = true;
            this.infodsMenus.LastKeyValues = null;
            this.infodsMenus.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.infodsMenus.PacketRecords = -1;
            this.infodsMenus.Position = -1;
            this.infodsMenus.RemoteName = "GLModule.sqlMenus";
            this.infodsMenus.ServerModify = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.infoCmbSolution);
            this.panel2.Controls.Add(this.btnUsers);
            this.panel2.Controls.Add(this.btnGroups);
            this.panel2.Controls.Add(this.cbLanguageID);
            this.panel2.Controls.Add(this.btnReloadTree);
            this.panel2.Controls.Add(this.btnApplyTree);
            this.panel2.Controls.Add(this.tView);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(256, 484);
            this.panel2.TabIndex = 54;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gbMenuInfo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(260, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(360, 490);
            this.panel3.TabIndex = 55;
            // 
            // accessPageControlToolStripMenuItem
            // 
            this.accessPageControlToolStripMenuItem.Name = "accessPageControlToolStripMenuItem";
            this.accessPageControlToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.accessPageControlToolStripMenuItem.Text = "Access Page Control";
            this.accessPageControlToolStripMenuItem.Click += new System.EventHandler(this.accessPageControlToolStripMenuItem_Click);
            // 
            // MGControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MGControl";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(620, 490);
            this.Load += new System.EventHandler(this.MGControl_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.gbMenuInfo.ResumeLayout(false);
            this.gbMenuInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preViewImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infodsSolutions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infodsMenus)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ItemAdd;
        private System.Windows.Forms.ToolStripMenuItem ItemDelete;
        private System.Windows.Forms.ToolStripMenuItem ItemGroups;
        private System.Windows.Forms.Button btnApplyTree;
        private System.Windows.Forms.Button btnReloadTree;
        private System.Windows.Forms.GroupBox gbMenuInfo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSEQ_NO;
        private System.Windows.Forms.TextBox txtItemType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtForm;
        private System.Windows.Forms.TextBox txtItemParam;
        private System.Windows.Forms.TextBox txtPackage;
        private System.Windows.Forms.TextBox txtParent;
        private System.Windows.Forms.TextBox txtCaption;
        private System.Windows.Forms.TextBox txtMenuID;
        private System.Windows.Forms.Label lblForm;
        private System.Windows.Forms.Label lblItemType;
        private System.Windows.Forms.Label lblItemParam;
        private System.Windows.Forms.Label lblSEQ_NO;
        private System.Windows.Forms.Label lblPackage;
        private System.Windows.Forms.Label lblParent;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblMenuID;
        private Srvtools.InfoDataSet infodsMenus;
        private System.Windows.Forms.ToolStripMenuItem ItemModify;
        private Srvtools.InfoDataSet infodsSolutions;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox infoCmbSolution;
        private System.Windows.Forms.Button btnGroups;
        private System.Windows.Forms.Button btnSelPackage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblModuleType;
        private System.Windows.Forms.ComboBox cmbModuleType;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Button btnSelImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.TextBox txtImageUrl;
        private System.Windows.Forms.Label lblImageUrl;
        private System.Windows.Forms.Button btnImageUrl;
        private System.Windows.Forms.PictureBox preViewImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbLanguageID;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.ToolStripMenuItem accessUsersToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSelectReport;
        private System.Windows.Forms.ToolStripMenuItem accessPageControlToolStripMenuItem;

    }
}
