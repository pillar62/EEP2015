namespace Srvtools
{
    partial class frmInfoSecurityEdit
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
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnRef = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.infoDataGridView1 = new Srvtools.InfoDataGridView();
            this.MenuID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ControlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descriptions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(540, 28);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(75, 23);
            this.btnAddAll.TabIndex = 1;
            this.btnAddAll.Text = "Add All";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(540, 106);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(540, 195);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnRef
            // 
            this.btnRef.Location = new System.Drawing.Point(540, 290);
            this.btnRef.Name = "btnRef";
            this.btnRef.Size = new System.Drawing.Size(75, 23);
            this.btnRef.TabIndex = 4;
            this.btnRef.Text = "Refrash";
            this.btnRef.UseVisualStyleBackColor = true;
            this.btnRef.Click += new System.EventHandler(this.btnRef_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(103, 342);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(343, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // infoDataGridView1
            // 
            this.infoDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MenuID,
            this.ControlName,
            this.Type,
            this.Descriptions});
            this.infoDataGridView1.EnterEnable = true;
            this.infoDataGridView1.EnterRefValControl = false;
            this.infoDataGridView1.Location = new System.Drawing.Point(13, 18);
            this.infoDataGridView1.Name = "infoDataGridView1";
            this.infoDataGridView1.RowTemplate.Height = 23;
            this.infoDataGridView1.Size = new System.Drawing.Size(504, 307);
            this.infoDataGridView1.SureDelete = false;
            this.infoDataGridView1.TabIndex = 0;
            this.infoDataGridView1.TotalActive = false;
            this.infoDataGridView1.TotalBackColor = System.Drawing.SystemColors.Info;
            this.infoDataGridView1.TotalCaption = null;
            this.infoDataGridView1.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.infoDataGridView1.TotalFont = new System.Drawing.Font("SimSun", 9F);
            // 
            // MenuID
            // 
            this.MenuID.HeaderText = "MenuID";
            this.MenuID.Name = "MenuID";
            this.MenuID.ReadOnly = true;
            this.MenuID.Visible = false;
            this.MenuID.Width = 45;
            // 
            // ControlName
            // 
            this.ControlName.HeaderText = "ControlName";
            this.ControlName.Name = "ControlName";
            this.ControlName.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 110;
            // 
            // Descriptions
            // 
            this.Descriptions.HeaderText = "Descriptions";
            this.Descriptions.Name = "Descriptions";
            this.Descriptions.Width = 250;
            // 
            // frmInfoSecurityEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 381);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRef);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.infoDataGridView1);
            this.Name = "frmInfoSecurityEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Security Control Export";
            this.Load += new System.EventHandler(this.frmInfoSecurityEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private InfoDataGridView infoDataGridView1;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnRef;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn MenuID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ControlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descriptions;
    }
}