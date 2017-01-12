namespace OfficeTools.Design
{
    partial class frmImageColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageColumns));
            this.propertyGridImageColumns = new System.Windows.Forms.PropertyGrid();
            this.listBoxImageColumns = new System.Windows.Forms.ListBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.labelProperties = new System.Windows.Forms.Label();
            this.labelMember = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGridImageColumns
            // 
            this.propertyGridImageColumns.HelpVisible = false;
            this.propertyGridImageColumns.Location = new System.Drawing.Point(248, 30);
            this.propertyGridImageColumns.Name = "propertyGridImageColumns";
            this.propertyGridImageColumns.Size = new System.Drawing.Size(227, 235);
            this.propertyGridImageColumns.TabIndex = 1;
            this.propertyGridImageColumns.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridImageColumns_PropertyValueChanged);
            // 
            // listBoxImageColumns
            // 
            this.listBoxImageColumns.FormattingEnabled = true;
            this.listBoxImageColumns.HorizontalScrollbar = true;
            this.listBoxImageColumns.ItemHeight = 12;
            this.listBoxImageColumns.Location = new System.Drawing.Point(17, 30);
            this.listBoxImageColumns.Name = "listBoxImageColumns";
            this.listBoxImageColumns.Size = new System.Drawing.Size(187, 196);
            this.listBoxImageColumns.TabIndex = 0;
            this.listBoxImageColumns.SelectedIndexChanged += new System.EventHandler(this.listBoxImageColumns_SelectedIndexChanged);
            // 
            // btnUp
            // 
            this.btnUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUp.BackgroundImage")));
            this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUp.Location = new System.Drawing.Point(210, 30);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.TabIndex = 4;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDown.BackgroundImage")));
            this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDown.Location = new System.Drawing.Point(210, 60);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.TabIndex = 5;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(118, 238);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(86, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(17, 238);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(86, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // labelProperties
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Location = new System.Drawing.Point(246, 15);
            this.labelProperties.Name = "labelProperties";
            this.labelProperties.Size = new System.Drawing.Size(71, 12);
            this.labelProperties.TabIndex = 3;
            this.labelProperties.Text = "properties:";
            // 
            // labelMember
            // 
            this.labelMember.AutoSize = true;
            this.labelMember.Location = new System.Drawing.Point(15, 15);
            this.labelMember.Name = "labelMember";
            this.labelMember.Size = new System.Drawing.Size(47, 12);
            this.labelMember.TabIndex = 4;
            this.labelMember.Text = "member:";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(319, 271);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(400, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmImageColumns
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(487, 308);
            this.Controls.Add(this.labelMember);
            this.Controls.Add(this.labelProperties);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.listBoxImageColumns);
            this.Controls.Add(this.propertyGridImageColumns);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmImageColumns";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DataSourceImageColumnItemEditor";
            this.Load += new System.EventHandler(this.frmImageColumns_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGridImageColumns;
        private System.Windows.Forms.ListBox listBoxImageColumns;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.Label labelMember;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}