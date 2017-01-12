namespace AjaxTools
{
    partial class ExtGridColumnsEditor
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("TextBoxColumn");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("ComboBoxColumn");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("CheckBoxColumn");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("DatePickerColumn");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("ImageColumn");
            this.lblAvailableColumns = new System.Windows.Forms.Label();
            this.treeAvailableColumns = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lstExtGridColumns = new System.Windows.Forms.ListBox();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAvailableColumns
            // 
            this.lblAvailableColumns.AutoSize = true;
            this.lblAvailableColumns.Location = new System.Drawing.Point(12, 22);
            this.lblAvailableColumns.Name = "lblAvailableColumns";
            this.lblAvailableColumns.Size = new System.Drawing.Size(107, 12);
            this.lblAvailableColumns.TabIndex = 0;
            this.lblAvailableColumns.Text = "available columns";
            // 
            // treeAvailableColumns
            // 
            this.treeAvailableColumns.Location = new System.Drawing.Point(12, 46);
            this.treeAvailableColumns.Name = "treeAvailableColumns";
            treeNode1.Name = "nTextBoxColumn";
            treeNode1.Text = "TextBoxColumn";
            treeNode2.Name = "nComboBoxColumn";
            treeNode2.Text = "ComboBoxColumn";
            treeNode3.Name = "nCheckBoxColumn";
            treeNode3.Text = "CheckBoxColumn";
            treeNode4.Name = "nDatePickerColumn";
            treeNode4.Text = "DatePickerColumn";
            treeNode5.Name = "nImageColumn";
            treeNode5.Text = "ImageColumn";
            this.treeAvailableColumns.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.treeAvailableColumns.ShowLines = false;
            this.treeAvailableColumns.Size = new System.Drawing.Size(176, 85);
            this.treeAvailableColumns.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnMoveDown);
            this.panel1.Controls.Add(this.btnMoveUp);
            this.panel1.Controls.Add(this.lstExtGridColumns);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.treeAvailableColumns);
            this.panel1.Controls.Add(this.lblAvailableColumns);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 429);
            this.panel1.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(113, 151);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // lstExtGridColumns
            // 
            this.lstExtGridColumns.FormattingEnabled = true;
            this.lstExtGridColumns.ItemHeight = 12;
            this.lstExtGridColumns.Location = new System.Drawing.Point(14, 189);
            this.lstExtGridColumns.Name = "lstExtGridColumns";
            this.lstExtGridColumns.Size = new System.Drawing.Size(135, 220);
            this.lstExtGridColumns.TabIndex = 3;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(155, 189);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(33, 23);
            this.btnMoveUp.TabIndex = 4;
            this.btnMoveUp.Text = "up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(155, 218);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(33, 23);
            this.btnMoveDown.TabIndex = 5;
            this.btnMoveDown.Text = "down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(155, 267);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(33, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGrid1.Location = new System.Drawing.Point(208, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(246, 382);
            this.propertyGrid1.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(246, 394);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(59, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(349, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ExtGridColumnsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 429);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtGridColumnsEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExtGrid Columns";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAvailableColumns;
        private System.Windows.Forms.TreeView treeAvailableColumns;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lstExtGridColumns;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}