namespace OfficeTools.Design
{
    partial class frmDesigner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDesigner));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDataSource = new System.Windows.Forms.TabPage();
            this.btnUndoDataSource = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btnSaveDataSource = new System.Windows.Forms.Button();
            this.dataGridViewDataSource = new System.Windows.Forms.DataGridView();
            this.ColumnCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataSource = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnDataMember = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnImageColumns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageTag = new System.Windows.Forms.TabPage();
            this.btnUndoTag = new System.Windows.Forms.Button();
            this.btnSaveTag = new System.Windows.Forms.Button();
            this.dataGridViewTag = new System.Windows.Forms.DataGridView();
            this.ColumnDataField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFormat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageProperty = new System.Windows.Forms.TabPage();
            this.btnUndoProperty = new System.Windows.Forms.Button();
            this.tbEmailAddressDetail = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbTemplateFileName = new System.Windows.Forms.TextBox();
            this.tbOutputFileName = new System.Windows.Forms.TextBox();
            this.tbOutputFilePath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOutputMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDetail = new System.Windows.Forms.Button();
            this.tbEmailAddress = new System.Windows.Forms.TextBox();
            this.tbEmailTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPlateMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbShowAction = new System.Windows.Forms.CheckBox();
            this.cbMarkException = new System.Windows.Forms.CheckBox();
            this.btnSaveProperty = new System.Windows.Forms.Button();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.panelMessage = new System.Windows.Forms.Panel();
            this.dataGridViewDebug = new System.Windows.Forms.DataGridView();
            this.ColumnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageDataSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataSource)).BeginInit();
            this.tabPageTag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTag)).BeginInit();
            this.tabPageProperty.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDebug)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageDataSource);
            this.tabControl.Controls.Add(this.tabPageTag);
            this.tabControl.Controls.Add(this.tabPageProperty);
            this.tabControl.Controls.Add(this.tabPageDebug);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.ImageList = this.imageList;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(621, 360);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDataSource
            // 
            this.tabPageDataSource.Controls.Add(this.btnUndoDataSource);
            this.tabPageDataSource.Controls.Add(this.btnSaveDataSource);
            this.tabPageDataSource.Controls.Add(this.dataGridViewDataSource);
            this.tabPageDataSource.ImageKey = "DataSet";
            this.tabPageDataSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabPageDataSource.Location = new System.Drawing.Point(4, 23);
            this.tabPageDataSource.Name = "tabPageDataSource";
            this.tabPageDataSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataSource.Size = new System.Drawing.Size(613, 333);
            this.tabPageDataSource.TabIndex = 0;
            this.tabPageDataSource.Text = "DataSource Defination";
            this.tabPageDataSource.UseVisualStyleBackColor = true;
            // 
            // btnUndoDataSource
            // 
            this.btnUndoDataSource.Enabled = false;
            this.btnUndoDataSource.ImageKey = "undo";
            this.btnUndoDataSource.ImageList = this.imageList;
            this.btnUndoDataSource.Location = new System.Drawing.Point(430, 290);
            this.btnUndoDataSource.Name = "btnUndoDataSource";
            this.btnUndoDataSource.Size = new System.Drawing.Size(75, 24);
            this.btnUndoDataSource.TabIndex = 1;
            this.btnUndoDataSource.Text = " Undo";
            this.btnUndoDataSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUndoDataSource.UseVisualStyleBackColor = true;
            this.btnUndoDataSource.Click += new System.EventHandler(this.btnUndoDataSource_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Tag");
            this.imageList.Images.SetKeyName(1, "DataSet");
            this.imageList.Images.SetKeyName(2, "error");
            this.imageList.Images.SetKeyName(3, "help");
            this.imageList.Images.SetKeyName(4, "Property");
            this.imageList.Images.SetKeyName(5, "warning");
            this.imageList.Images.SetKeyName(6, "stop");
            this.imageList.Images.SetKeyName(7, "run");
            this.imageList.Images.SetKeyName(8, "save");
            this.imageList.Images.SetKeyName(9, "address");
            this.imageList.Images.SetKeyName(10, "open");
            this.imageList.Images.SetKeyName(11, "undo");
            // 
            // btnSaveDataSource
            // 
            this.btnSaveDataSource.Enabled = false;
            this.btnSaveDataSource.ImageKey = "save";
            this.btnSaveDataSource.ImageList = this.imageList;
            this.btnSaveDataSource.Location = new System.Drawing.Point(520, 290);
            this.btnSaveDataSource.Name = "btnSaveDataSource";
            this.btnSaveDataSource.Size = new System.Drawing.Size(75, 24);
            this.btnSaveDataSource.TabIndex = 2;
            this.btnSaveDataSource.Text = " Save";
            this.btnSaveDataSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveDataSource.UseVisualStyleBackColor = true;
            this.btnSaveDataSource.Click += new System.EventHandler(this.btnSaveDataSource_Click);
            // 
            // dataGridViewDataSource
            // 
            this.dataGridViewDataSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDataSource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCaption,
            this.ColumnDataSource,
            this.ColumnDataMember,
            this.ColumnImageColumns});
            this.dataGridViewDataSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewDataSource.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewDataSource.Name = "dataGridViewDataSource";
            this.dataGridViewDataSource.RowTemplate.Height = 23;
            this.dataGridViewDataSource.Size = new System.Drawing.Size(607, 269);
            this.dataGridViewDataSource.TabIndex = 0;
            this.dataGridViewDataSource.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewDataSource_UserAddedRow);
            this.dataGridViewDataSource.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewDataSource_CellBeginEdit);
            this.dataGridViewDataSource.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewDataSource_UserDeletedRow);
            // 
            // ColumnCaption
            // 
            this.ColumnCaption.HeaderText = "Caption";
            this.ColumnCaption.Name = "ColumnCaption";
            // 
            // ColumnDataSource
            // 
            this.ColumnDataSource.HeaderText = "DataSource";
            this.ColumnDataSource.Name = "ColumnDataSource";
            this.ColumnDataSource.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnDataSource.Width = 150;
            // 
            // ColumnDataMember
            // 
            this.ColumnDataMember.HeaderText = "DataMember";
            this.ColumnDataMember.Name = "ColumnDataMember";
            this.ColumnDataMember.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnDataMember.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnImageColumns
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ColumnImageColumns.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnImageColumns.HeaderText = "ImageColumns";
            this.ColumnImageColumns.Name = "ColumnImageColumns";
            // 
            // tabPageTag
            // 
            this.tabPageTag.Controls.Add(this.btnUndoTag);
            this.tabPageTag.Controls.Add(this.btnSaveTag);
            this.tabPageTag.Controls.Add(this.dataGridViewTag);
            this.tabPageTag.ImageKey = "Tag";
            this.tabPageTag.Location = new System.Drawing.Point(4, 23);
            this.tabPageTag.Name = "tabPageTag";
            this.tabPageTag.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTag.Size = new System.Drawing.Size(613, 333);
            this.tabPageTag.TabIndex = 1;
            this.tabPageTag.Text = "Tag Defination";
            this.tabPageTag.UseVisualStyleBackColor = true;
            // 
            // btnUndoTag
            // 
            this.btnUndoTag.Enabled = false;
            this.btnUndoTag.ImageKey = "undo";
            this.btnUndoTag.ImageList = this.imageList;
            this.btnUndoTag.Location = new System.Drawing.Point(430, 290);
            this.btnUndoTag.Name = "btnUndoTag";
            this.btnUndoTag.Size = new System.Drawing.Size(75, 24);
            this.btnUndoTag.TabIndex = 1;
            this.btnUndoTag.Text = " Undo";
            this.btnUndoTag.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUndoTag.UseVisualStyleBackColor = true;
            this.btnUndoTag.Click += new System.EventHandler(this.btnUndoTag_Click);
            // 
            // btnSaveTag
            // 
            this.btnSaveTag.Enabled = false;
            this.btnSaveTag.ImageKey = "save";
            this.btnSaveTag.ImageList = this.imageList;
            this.btnSaveTag.Location = new System.Drawing.Point(520, 290);
            this.btnSaveTag.Name = "btnSaveTag";
            this.btnSaveTag.Size = new System.Drawing.Size(75, 24);
            this.btnSaveTag.TabIndex = 2;
            this.btnSaveTag.Text = " Save";
            this.btnSaveTag.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveTag.UseVisualStyleBackColor = true;
            this.btnSaveTag.Click += new System.EventHandler(this.btnSaveTag_Click);
            // 
            // dataGridViewTag
            // 
            this.dataGridViewTag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDataField,
            this.ColumnExpression,
            this.ColumnFormat});
            this.dataGridViewTag.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewTag.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewTag.Name = "dataGridViewTag";
            this.dataGridViewTag.RowTemplate.Height = 23;
            this.dataGridViewTag.Size = new System.Drawing.Size(607, 267);
            this.dataGridViewTag.TabIndex = 0;
            this.dataGridViewTag.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewTag_CellBeginEdit);
            this.dataGridViewTag.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewTag_UserDeletedRow);
            // 
            // ColumnDataField
            // 
            this.ColumnDataField.HeaderText = "DataField";
            this.ColumnDataField.Name = "ColumnDataField";
            // 
            // ColumnExpression
            // 
            this.ColumnExpression.HeaderText = "Expression";
            this.ColumnExpression.Name = "ColumnExpression";
            this.ColumnExpression.Width = 350;
            // 
            // ColumnFormat
            // 
            this.ColumnFormat.HeaderText = "Format";
            this.ColumnFormat.Name = "ColumnFormat";
            // 
            // tabPageProperty
            // 
            this.tabPageProperty.Controls.Add(this.btnUndoProperty);
            this.tabPageProperty.Controls.Add(this.tbEmailAddressDetail);
            this.tabPageProperty.Controls.Add(this.groupBox3);
            this.tabPageProperty.Controls.Add(this.groupBox2);
            this.tabPageProperty.Controls.Add(this.groupBox1);
            this.tabPageProperty.Controls.Add(this.btnSaveProperty);
            this.tabPageProperty.ImageKey = "Property";
            this.tabPageProperty.Location = new System.Drawing.Point(4, 23);
            this.tabPageProperty.Name = "tabPageProperty";
            this.tabPageProperty.Size = new System.Drawing.Size(613, 333);
            this.tabPageProperty.TabIndex = 2;
            this.tabPageProperty.Text = "Other Property";
            this.tabPageProperty.UseVisualStyleBackColor = true;
            // 
            // btnUndoProperty
            // 
            this.btnUndoProperty.Enabled = false;
            this.btnUndoProperty.ImageKey = "undo";
            this.btnUndoProperty.ImageList = this.imageList;
            this.btnUndoProperty.Location = new System.Drawing.Point(520, 260);
            this.btnUndoProperty.Name = "btnUndoProperty";
            this.btnUndoProperty.Size = new System.Drawing.Size(75, 24);
            this.btnUndoProperty.TabIndex = 4;
            this.btnUndoProperty.Text = " Undo";
            this.btnUndoProperty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUndoProperty.UseVisualStyleBackColor = true;
            this.btnUndoProperty.Click += new System.EventHandler(this.btnUndoProperty_Click);
            // 
            // tbEmailAddressDetail
            // 
            this.tbEmailAddressDetail.Location = new System.Drawing.Point(233, 114);
            this.tbEmailAddressDetail.Multiline = true;
            this.tbEmailAddressDetail.Name = "tbEmailAddressDetail";
            this.tbEmailAddressDetail.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tbEmailAddressDetail.Size = new System.Drawing.Size(250, 75);
            this.tbEmailAddressDetail.TabIndex = 5;
            this.tbEmailAddressDetail.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnOpen);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tbTemplateFileName);
            this.groupBox3.Controls.Add(this.tbOutputFileName);
            this.groupBox3.Controls.Add(this.tbOutputFilePath);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cbOutputMode);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(20, 187);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(463, 120);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File Config";
            // 
            // btnOpen
            // 
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpen.ImageKey = "open";
            this.btnOpen.ImageList = this.imageList;
            this.btnOpen.Location = new System.Drawing.Point(434, 58);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(21, 21);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.TabStop = false;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 11;
            this.label6.Tag = "";
            this.label6.Text = "Template Filename:";
            this.toolTip.SetToolTip(this.label6, "Specifies the file used as template");
            // 
            // tbTemplateFileName
            // 
            this.tbTemplateFileName.Location = new System.Drawing.Point(125, 22);
            this.tbTemplateFileName.Name = "tbTemplateFileName";
            this.tbTemplateFileName.Size = new System.Drawing.Size(100, 21);
            this.tbTemplateFileName.TabIndex = 0;
            this.tbTemplateFileName.TextChanged += new System.EventHandler(this.Item_Changed);
            // 
            // tbOutputFileName
            // 
            this.tbOutputFileName.Location = new System.Drawing.Point(357, 22);
            this.tbOutputFileName.Name = "tbOutputFileName";
            this.tbOutputFileName.Size = new System.Drawing.Size(100, 21);
            this.tbOutputFileName.TabIndex = 1;
            this.tbOutputFileName.TextChanged += new System.EventHandler(this.Item_Changed);
            // 
            // tbOutputFilePath
            // 
            this.tbOutputFilePath.Location = new System.Drawing.Point(125, 58);
            this.tbOutputFilePath.Name = "tbOutputFilePath";
            this.tbOutputFilePath.Size = new System.Drawing.Size(310, 21);
            this.tbOutputFilePath.TabIndex = 2;
            this.tbOutputFilePath.TextChanged += new System.EventHandler(this.Item_Changed);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 10;
            this.label5.Tag = "";
            this.label5.Text = "Output Mode:";
            this.toolTip.SetToolTip(this.label5, "Specifies the action after output file");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 5;
            this.label2.Tag = "";
            this.label2.Text = "Output FilePath:";
            this.toolTip.SetToolTip(this.label2, "Specifies the path to output file");
            // 
            // cbOutputMode
            // 
            this.cbOutputMode.FormattingEnabled = true;
            this.cbOutputMode.Items.AddRange(new object[] {
            "None",
            "Launch",
            "Email"});
            this.cbOutputMode.Location = new System.Drawing.Point(125, 89);
            this.cbOutputMode.Name = "cbOutputMode";
            this.cbOutputMode.Size = new System.Drawing.Size(100, 20);
            this.cbOutputMode.TabIndex = 4;
            this.cbOutputMode.SelectedIndexChanged += new System.EventHandler(this.Item_Changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 7;
            this.label4.Tag = "";
            this.label4.Text = "Output Filename:";
            this.toolTip.SetToolTip(this.label4, "Specifies the name of outputed file");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDetail);
            this.groupBox2.Controls.Add(this.tbEmailAddress);
            this.groupBox2.Controls.Add(this.tbEmailTitle);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(233, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 87);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "E-mail Config";
            // 
            // btnDetail
            // 
            this.btnDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDetail.ImageKey = "address";
            this.btnDetail.ImageList = this.imageList;
            this.btnDetail.Location = new System.Drawing.Point(221, 58);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(21, 21);
            this.btnDetail.TabIndex = 2;
            this.btnDetail.TabStop = false;
            this.btnDetail.UseVisualStyleBackColor = true;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // tbEmailAddress
            // 
            this.tbEmailAddress.Location = new System.Drawing.Point(113, 58);
            this.tbEmailAddress.Name = "tbEmailAddress";
            this.tbEmailAddress.Size = new System.Drawing.Size(108, 21);
            this.tbEmailAddress.TabIndex = 1;
            this.tbEmailAddress.TextChanged += new System.EventHandler(this.Item_Changed);
            // 
            // tbEmailTitle
            // 
            this.tbEmailTitle.Location = new System.Drawing.Point(114, 25);
            this.tbEmailTitle.Name = "tbEmailTitle";
            this.tbEmailTitle.Size = new System.Drawing.Size(130, 21);
            this.tbEmailTitle.TabIndex = 0;
            this.tbEmailTitle.TextChanged += new System.EventHandler(this.Item_Changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 4;
            this.label1.Tag = "";
            this.label1.Text = "E-mail Address:";
            this.toolTip.SetToolTip(this.label1, "Specifies the addresses to send e-mail");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 6;
            this.label3.Tag = "";
            this.label3.Text = "E-mail Title:";
            this.toolTip.SetToolTip(this.label3, "Specifies the title of e-mail");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPlateMode);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbShowAction);
            this.groupBox1.Controls.Add(this.cbMarkException);
            this.groupBox1.Location = new System.Drawing.Point(20, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 131);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other Config";
            // 
            // cbPlateMode
            // 
            this.cbPlateMode.FormattingEnabled = true;
            this.cbPlateMode.Items.AddRange(new object[] {
            "Xml",
            "Com"});
            this.cbPlateMode.Location = new System.Drawing.Point(94, 94);
            this.cbPlateMode.Name = "cbPlateMode";
            this.cbPlateMode.Size = new System.Drawing.Size(82, 20);
            this.cbPlateMode.TabIndex = 2;
            this.cbPlateMode.SelectedIndexChanged += new System.EventHandler(this.Item_Changed);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 11;
            this.label7.Tag = "";
            this.label7.Text = "Plate Mode:";
            // 
            // cbShowAction
            // 
            this.cbShowAction.AutoSize = true;
            this.cbShowAction.Location = new System.Drawing.Point(19, 25);
            this.cbShowAction.Name = "cbShowAction";
            this.cbShowAction.Size = new System.Drawing.Size(90, 16);
            this.cbShowAction.TabIndex = 0;
            this.cbShowAction.Tag = "";
            this.cbShowAction.Text = "Show Action";
            this.toolTip.SetToolTip(this.cbShowAction, "Specifies whether infomation is displayed durning the output progress");
            this.cbShowAction.UseVisualStyleBackColor = true;
            this.cbShowAction.CheckedChanged += new System.EventHandler(this.Item_Changed);
            // 
            // cbMarkException
            // 
            this.cbMarkException.AutoSize = true;
            this.cbMarkException.Location = new System.Drawing.Point(19, 60);
            this.cbMarkException.Name = "cbMarkException";
            this.cbMarkException.Size = new System.Drawing.Size(108, 16);
            this.cbMarkException.TabIndex = 1;
            this.cbMarkException.Tag = "";
            this.cbMarkException.Text = "Mark Exception";
            this.toolTip.SetToolTip(this.cbMarkException, "Specifies whether mark exception when output encounter error");
            this.cbMarkException.UseVisualStyleBackColor = true;
            this.cbMarkException.CheckedChanged += new System.EventHandler(this.Item_Changed);
            // 
            // btnSaveProperty
            // 
            this.btnSaveProperty.Enabled = false;
            this.btnSaveProperty.ImageKey = "save";
            this.btnSaveProperty.ImageList = this.imageList;
            this.btnSaveProperty.Location = new System.Drawing.Point(520, 290);
            this.btnSaveProperty.Name = "btnSaveProperty";
            this.btnSaveProperty.Size = new System.Drawing.Size(75, 24);
            this.btnSaveProperty.TabIndex = 3;
            this.btnSaveProperty.Text = " Save";
            this.btnSaveProperty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveProperty.UseVisualStyleBackColor = true;
            this.btnSaveProperty.Click += new System.EventHandler(this.btnSaveProperty_Click);
            // 
            // tabPageDebug
            // 
            this.tabPageDebug.Controls.Add(this.statusStrip);
            this.tabPageDebug.Controls.Add(this.panelMessage);
            this.tabPageDebug.Controls.Add(this.menuStrip);
            this.tabPageDebug.ImageKey = "run";
            this.tabPageDebug.Location = new System.Drawing.Point(4, 23);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Size = new System.Drawing.Size(613, 333);
            this.tabPageDebug.TabIndex = 3;
            this.tabPageDebug.Text = "Plate Debug";
            this.tabPageDebug.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 311);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(613, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.AutoSize = false;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(250, 17);
            this.toolStripStatusLabel.Text = "Ready";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // panelMessage
            // 
            this.panelMessage.BackColor = System.Drawing.Color.Silver;
            this.panelMessage.Controls.Add(this.dataGridViewDebug);
            this.panelMessage.Location = new System.Drawing.Point(1, 27);
            this.panelMessage.Name = "panelMessage";
            this.panelMessage.Size = new System.Drawing.Size(612, 284);
            this.panelMessage.TabIndex = 0;
            // 
            // dataGridViewDebug
            // 
            this.dataGridViewDebug.AllowUserToAddRows = false;
            this.dataGridViewDebug.AllowUserToDeleteRows = false;
            this.dataGridViewDebug.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDebug.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNo,
            this.ColumnImage,
            this.ColumnDescription,
            this.ColumnTag});
            this.dataGridViewDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDebug.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDebug.Name = "dataGridViewDebug";
            this.dataGridViewDebug.ReadOnly = true;
            this.dataGridViewDebug.RowTemplate.Height = 23;
            this.dataGridViewDebug.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDebug.Size = new System.Drawing.Size(612, 284);
            this.dataGridViewDebug.TabIndex = 0;
            // 
            // ColumnNo
            // 
            this.ColumnNo.HeaderText = "";
            this.ColumnNo.Name = "ColumnNo";
            this.ColumnNo.ReadOnly = true;
            this.ColumnNo.Width = 30;
            // 
            // ColumnImage
            // 
            this.ColumnImage.HeaderText = "";
            this.ColumnImage.Name = "ColumnImage";
            this.ColumnImage.ReadOnly = true;
            this.ColumnImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnImage.Width = 20;
            // 
            // ColumnDescription
            // 
            this.ColumnDescription.HeaderText = "Description";
            this.ColumnDescription.Name = "ColumnDescription";
            this.ColumnDescription.ReadOnly = true;
            this.ColumnDescription.Width = 400;
            // 
            // ColumnTag
            // 
            this.ColumnTag.HeaderText = "Tag";
            this.ColumnTag.Name = "ColumnTag";
            this.ColumnTag.ReadOnly = true;
            this.ColumnTag.Width = 115;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(613, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.cancelToolStripMenuItem,
            this.editFileToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.debugToolStripMenuItem.Text = "&Debug";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("startToolStripMenuItem.Image")));
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Enabled = false;
            this.cancelToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cancelToolStripMenuItem.Image")));
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // editFileToolStripMenuItem
            // 
            this.editFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editFileToolStripMenuItem.Image")));
            this.editFileToolStripMenuItem.Name = "editFileToolStripMenuItem";
            this.editFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.editFileToolStripMenuItem.Text = "Edit File";
            this.editFileToolStripMenuItem.Click += new System.EventHandler(this.editFileToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.Checked = true;
            this.outputToolStripMenuItem.CheckOnClick = true;
            this.outputToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.outputToolStripMenuItem.Text = "Output";
            this.outputToolStripMenuItem.CheckedChanged += new System.EventHandler(this.outputToolStripMenuItem_CheckedChanged);
            // 
            // frmDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 359);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDesigner";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plate Designer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDesigner_FormClosing);
            this.Load += new System.EventHandler(this.frmDesigner_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageDataSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataSource)).EndInit();
            this.tabPageTag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTag)).EndInit();
            this.tabPageProperty.ResumeLayout(false);
            this.tabPageProperty.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageDebug.ResumeLayout(false);
            this.tabPageDebug.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDebug)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDataSource;
        private System.Windows.Forms.TabPage tabPageTag;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TabPage tabPageProperty;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.Panel panelMessage;
        private System.Windows.Forms.DataGridView dataGridViewDataSource;
        private System.Windows.Forms.DataGridView dataGridViewTag;
        private System.Windows.Forms.TextBox tbOutputFileName;
        private System.Windows.Forms.TextBox tbEmailTitle;
        private System.Windows.Forms.TextBox tbOutputFilePath;
        private System.Windows.Forms.TextBox tbEmailAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveProperty;
        private System.Windows.Forms.Button btnSaveDataSource;
        private System.Windows.Forms.Button btnSaveTag;
        private System.Windows.Forms.ComboBox cbOutputMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbMarkException;
        private System.Windows.Forms.CheckBox cbShowAction;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataField;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnExpression;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFormat;
        private System.Windows.Forms.TextBox tbTemplateFileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.TextBox tbEmailAddressDetail;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnUndoDataSource;
        private System.Windows.Forms.Button btnUndoTag;
        private System.Windows.Forms.Button btnUndoProperty;

        private System.Windows.Forms.DataGridView dataGridViewDebug;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo;
        private System.Windows.Forms.DataGridViewImageColumn ColumnImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTag;
        private System.Windows.Forms.ToolStripMenuItem editFileToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbPlateMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCaption;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnDataSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnImageColumns;
    }
}