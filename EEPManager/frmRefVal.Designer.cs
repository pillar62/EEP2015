namespace EEPManager
{
    partial class frmRefVal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRefVal));
            this.lbRefVal = new System.Windows.Forms.ListBox();
            this.ibsRefVal = new Srvtools.InfoBindingSource(this.components);
            this.idsRefVal = new Srvtools.InfoDataSet(this.components);
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbCaption = new System.Windows.Forms.TextBox();
            this.tbTableName = new System.Windows.Forms.TextBox();
            this.tbSelectCommand = new System.Windows.Forms.TextBox();
            this.idgRefVal = new Srvtools.InfoDataGridView();
            this.rEFVALNODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FIELD_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HEADER_TEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WIDTH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ibsRefVal_d = new Srvtools.InfoBindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnAUD = new System.Windows.Forms.Panel();
            this.pnOC = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbSelectAlias = new System.Windows.Forms.ComboBox();
            this.tbDisplayMember = new System.Windows.Forms.ComboBox();
            this.tbValueMember = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ibsRefVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsRefVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idgRefVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsRefVal_d)).BeginInit();
            this.pnAUD.SuspendLayout();
            this.pnOC.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbRefVal
            // 
            this.lbRefVal.DataSource = this.ibsRefVal;
            this.lbRefVal.DisplayMember = "REFVAL_NO";
            this.lbRefVal.FormattingEnabled = true;
            this.lbRefVal.ItemHeight = 12;
            this.lbRefVal.Location = new System.Drawing.Point(11, 26);
            this.lbRefVal.Name = "lbRefVal";
            this.lbRefVal.Size = new System.Drawing.Size(152, 244);
            this.lbRefVal.TabIndex = 1;
            this.lbRefVal.ValueMember = "REFVAL_NO";
            this.lbRefVal.SelectedValueChanged += new System.EventHandler(this.lbRefVal_SelectedValueChanged);
            // 
            // ibsRefVal
            // 
            this.ibsRefVal.AllowAdd = true;
            this.ibsRefVal.AllowDelete = true;
            this.ibsRefVal.AllowPrint = true;
            this.ibsRefVal.AllowUpdate = true;
            this.ibsRefVal.AutoApply = false;
            this.ibsRefVal.AutoApplyMaster = false;
            this.ibsRefVal.AutoDisibleControl = false;
            this.ibsRefVal.AutoRecordLock = false;
            this.ibsRefVal.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.ibsRefVal.CloseProtect = false;
            this.ibsRefVal.DataMember = "cmdSysRefVal";
            this.ibsRefVal.DataSource = this.idsRefVal;
            this.ibsRefVal.DelayInterval = 300;
            this.ibsRefVal.DisableKeyFields = false;
            this.ibsRefVal.EnableFlag = false;
            this.ibsRefVal.FocusedControl = null;
            this.ibsRefVal.OwnerComp = null;
            this.ibsRefVal.Position = 0;
            this.ibsRefVal.RelationDelay = false;
            this.ibsRefVal.text = "ibsRefVal";
            // 
            // idsRefVal
            // 
            this.idsRefVal.Active = true;
            this.idsRefVal.AlwaysClose = false;
            this.idsRefVal.DeleteIncomplete = true;
            this.idsRefVal.LastKeyValues = null;
            this.idsRefVal.PacketRecords = -1;
            this.idsRefVal.Position = -1;
            this.idsRefVal.RemoteName = "GLModule.cmdSysRefVal";
            this.idsRefVal.ServerModify = false;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(264, 64);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(326, 21);
            this.tbDescription.TabIndex = 2;
            // 
            // tbCaption
            // 
            this.tbCaption.Location = new System.Drawing.Point(264, 34);
            this.tbCaption.Name = "tbCaption";
            this.tbCaption.Size = new System.Drawing.Size(100, 21);
            this.tbCaption.TabIndex = 3;
            // 
            // tbTableName
            // 
            this.tbTableName.Location = new System.Drawing.Point(488, 100);
            this.tbTableName.Name = "tbTableName";
            this.tbTableName.Size = new System.Drawing.Size(102, 21);
            this.tbTableName.TabIndex = 4;
            // 
            // tbSelectCommand
            // 
            this.tbSelectCommand.Location = new System.Drawing.Point(263, 136);
            this.tbSelectCommand.Name = "tbSelectCommand";
            this.tbSelectCommand.Size = new System.Drawing.Size(247, 21);
            this.tbSelectCommand.TabIndex = 9;
            // 
            // idgRefVal
            // 
            this.idgRefVal.AutoGenerateColumns = false;
            this.idgRefVal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idgRefVal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rEFVALNODataGridViewTextBoxColumn,
            this.FIELD_NAME,
            this.HEADER_TEXT,
            this.WIDTH});
            this.idgRefVal.DataSource = this.ibsRefVal_d;
            this.idgRefVal.EnterEnable = true;
            this.idgRefVal.EnterRefValControl = false;
            this.idgRefVal.Location = new System.Drawing.Point(175, 224);
            this.idgRefVal.Name = "idgRefVal";
            this.idgRefVal.RowTemplate.Height = 23;
            this.idgRefVal.Size = new System.Drawing.Size(415, 118);
            this.idgRefVal.SureDelete = false;
            this.idgRefVal.TabIndex = 10;
            this.idgRefVal.TotalActive = true;
            this.idgRefVal.TotalBackColor = System.Drawing.SystemColors.Info;
            this.idgRefVal.TotalCaption = null;
            this.idgRefVal.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.idgRefVal.TotalFont = new System.Drawing.Font("SimSun", 9F);
            // 
            // rEFVALNODataGridViewTextBoxColumn
            // 
            this.rEFVALNODataGridViewTextBoxColumn.DataPropertyName = "REFVAL_NO";
            this.rEFVALNODataGridViewTextBoxColumn.HeaderText = "REFVAL_NO";
            this.rEFVALNODataGridViewTextBoxColumn.Name = "rEFVALNODataGridViewTextBoxColumn";
            // 
            // FIELD_NAME
            // 
            this.FIELD_NAME.DataPropertyName = "FIELD_NAME";
            this.FIELD_NAME.HeaderText = "FIELD_NAME";
            this.FIELD_NAME.Name = "FIELD_NAME";
            // 
            // HEADER_TEXT
            // 
            this.HEADER_TEXT.DataPropertyName = "HEADER_TEXT";
            this.HEADER_TEXT.HeaderText = "HEADER_TEXT";
            this.HEADER_TEXT.Name = "HEADER_TEXT";
            // 
            // WIDTH
            // 
            this.WIDTH.DataPropertyName = "WIDTH";
            this.WIDTH.HeaderText = "WIDTH";
            this.WIDTH.Name = "WIDTH";
            this.WIDTH.Width = 70;
            // 
            // ibsRefVal_d
            // 
            this.ibsRefVal_d.AllowAdd = true;
            this.ibsRefVal_d.AllowDelete = true;
            this.ibsRefVal_d.AllowPrint = true;
            this.ibsRefVal_d.AllowUpdate = true;
            this.ibsRefVal_d.AutoApply = false;
            this.ibsRefVal_d.AutoApplyMaster = false;
            this.ibsRefVal_d.AutoDisibleControl = false;
            this.ibsRefVal_d.AutoRecordLock = false;
            this.ibsRefVal_d.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.ibsRefVal_d.CloseProtect = false;
            this.ibsRefVal_d.DataMember = "Relation1";
            this.ibsRefVal_d.DataSource = this.ibsRefVal;
            this.ibsRefVal_d.DelayInterval = 300;
            this.ibsRefVal_d.DisableKeyFields = false;
            this.ibsRefVal_d.EnableFlag = false;
            this.ibsRefVal_d.FocusedControl = null;
            this.ibsRefVal_d.OwnerComp = null;
            this.ibsRefVal_d.Position = 0;
            this.ibsRefVal_d.RelationDelay = false;
            this.ibsRefVal_d.text = "ibsRefVal_d";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(205, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Caption";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(423, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "TableName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(173, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "DisplayMember";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(411, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "ValueMember";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(186, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "SelectAlias";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(174, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "SelectCommand";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "Select Reference Value";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(88, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(39, 32);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnAUD
            // 
            this.pnAUD.Controls.Add(this.btnUpdate);
            this.pnAUD.Controls.Add(this.btnDelete);
            this.pnAUD.Controls.Add(this.btnAdd);
            this.pnAUD.Location = new System.Drawing.Point(6, 284);
            this.pnAUD.Name = "pnAUD";
            this.pnAUD.Size = new System.Drawing.Size(161, 58);
            this.pnAUD.TabIndex = 22;
            // 
            // pnOC
            // 
            this.pnOC.Controls.Add(this.btnCancel);
            this.pnOC.Controls.Add(this.btnOk);
            this.pnOC.Location = new System.Drawing.Point(6, 284);
            this.pnOC.Name = "pnOC";
            this.pnOC.Size = new System.Drawing.Size(165, 32);
            this.pnOC.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(88, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(1, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 24;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(515, 134);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 24;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tbSelectAlias
            // 
            this.tbSelectAlias.FormattingEnabled = true;
            this.tbSelectAlias.Location = new System.Drawing.Point(263, 101);
            this.tbSelectAlias.Name = "tbSelectAlias";
            this.tbSelectAlias.Size = new System.Drawing.Size(103, 20);
            this.tbSelectAlias.TabIndex = 25;
            // 
            // tbDisplayMember
            // 
            this.tbDisplayMember.FormattingEnabled = true;
            this.tbDisplayMember.Location = new System.Drawing.Point(264, 173);
            this.tbDisplayMember.Name = "tbDisplayMember";
            this.tbDisplayMember.Size = new System.Drawing.Size(102, 20);
            this.tbDisplayMember.TabIndex = 26;
            // 
            // tbValueMember
            // 
            this.tbValueMember.FormattingEnabled = true;
            this.tbValueMember.Location = new System.Drawing.Point(488, 173);
            this.tbValueMember.Name = "tbValueMember";
            this.tbValueMember.Size = new System.Drawing.Size(102, 20);
            this.tbValueMember.TabIndex = 27;
            // 
            // frmRefVal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 362);
            this.Controls.Add(this.tbValueMember);
            this.Controls.Add(this.tbDisplayMember);
            this.Controls.Add(this.tbSelectAlias);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.pnOC);
            this.Controls.Add(this.pnAUD);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idgRefVal);
            this.Controls.Add(this.tbSelectCommand);
            this.Controls.Add(this.tbTableName);
            this.Controls.Add(this.tbCaption);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lbRefVal);
            this.Name = "frmRefVal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmRefVal";
            this.Load += new System.EventHandler(this.frmRefVal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ibsRefVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsRefVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idgRefVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsRefVal_d)).EndInit();
            this.pnAUD.ResumeLayout(false);
            this.pnOC.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbRefVal;
        private Srvtools.InfoDataSet idsRefVal;
        private Srvtools.InfoBindingSource ibsRefVal;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbCaption;
        private System.Windows.Forms.TextBox tbTableName;
        private System.Windows.Forms.TextBox tbSelectCommand;
        private Srvtools.InfoDataGridView idgRefVal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Srvtools.InfoBindingSource ibsRefVal_d;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnAUD;
        private System.Windows.Forms.Panel pnOC;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEFVALNODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FIELD_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn HEADER_TEXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn WIDTH;
        private System.Windows.Forms.ComboBox tbSelectAlias;
        private System.Windows.Forms.ComboBox tbDisplayMember;
        private System.Windows.Forms.ComboBox tbValueMember;
    }
}