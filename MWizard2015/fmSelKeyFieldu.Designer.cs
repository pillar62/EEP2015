namespace MWizard2015
{
    partial class fmSelKeyField
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbRelationFields = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lbDetailFields = new System.Windows.Forms.ListBox();
            this.lbMasterFields = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Master Fields";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Detail Fields";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(214, 84);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(56, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Relation Fields";
            // 
            // lbRelationFields
            // 
            this.lbRelationFields.FormattingEnabled = true;
            this.lbRelationFields.ItemHeight = 12;
            this.lbRelationFields.Location = new System.Drawing.Point(25, 216);
            this.lbRelationFields.Name = "lbRelationFields";
            this.lbRelationFields.Size = new System.Drawing.Size(373, 88);
            this.lbRelationFields.TabIndex = 6;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(404, 216);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(56, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lbDetailFields
            // 
            this.lbDetailFields.FormattingEnabled = true;
            this.lbDetailFields.ItemHeight = 12;
            this.lbDetailFields.Location = new System.Drawing.Point(286, 35);
            this.lbDetailFields.Name = "lbDetailFields";
            this.lbDetailFields.Size = new System.Drawing.Size(174, 136);
            this.lbDetailFields.TabIndex = 3;
            // 
            // lbMasterFields
            // 
            this.lbMasterFields.FormattingEnabled = true;
            this.lbMasterFields.ItemHeight = 12;
            this.lbMasterFields.Location = new System.Drawing.Point(25, 35);
            this.lbMasterFields.Name = "lbMasterFields";
            this.lbMasterFields.Size = new System.Drawing.Size(174, 136);
            this.lbMasterFields.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(146, 322);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(257, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // fmSelKeyField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 366);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lbRelationFields);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbDetailFields);
            this.Controls.Add(this.lbMasterFields);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "fmSelKeyField";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fmSelKeyField";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbRelationFields;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListBox lbDetailFields;
        private System.Windows.Forms.ListBox lbMasterFields;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}