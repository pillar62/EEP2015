namespace Srvtools
{
    partial class ControlDescriptionEditorDialog
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.groupBoxDB = new System.Windows.Forms.GroupBox();
            this.buttonWriteDB = new System.Windows.Forms.Button();
            this.buttonReadDB = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBoxDB.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonRefresh);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxDB);
            this.splitContainer1.Size = new System.Drawing.Size(392, 366);
            this.splitContainer1.SplitterDistance = 273;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(392, 273);
            this.dataGridView.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Enabled = false;
            this.buttonRefresh.Location = new System.Drawing.Point(41, 36);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(130, 23);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.Text = "Refresh controls";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // groupBoxDB
            // 
            this.groupBoxDB.Controls.Add(this.buttonWriteDB);
            this.groupBoxDB.Controls.Add(this.buttonReadDB);
            this.groupBoxDB.Location = new System.Drawing.Point(190, 7);
            this.groupBoxDB.Name = "groupBoxDB";
            this.groupBoxDB.Size = new System.Drawing.Size(162, 75);
            this.groupBoxDB.TabIndex = 5;
            this.groupBoxDB.TabStop = false;
            this.groupBoxDB.Text = "DataBase";
            // 
            // buttonWriteDB
            // 
            this.buttonWriteDB.Enabled = false;
            this.buttonWriteDB.Location = new System.Drawing.Point(13, 51);
            this.buttonWriteDB.Name = "buttonWriteDB";
            this.buttonWriteDB.Size = new System.Drawing.Size(130, 23);
            this.buttonWriteDB.TabIndex = 1;
            this.buttonWriteDB.Text = "Write to dataBase";
            this.buttonWriteDB.UseVisualStyleBackColor = true;
            this.buttonWriteDB.Click += new System.EventHandler(this.buttonWriteDB_Click);
            // 
            // buttonReadDB
            // 
            this.buttonReadDB.Location = new System.Drawing.Point(14, 17);
            this.buttonReadDB.Name = "buttonReadDB";
            this.buttonReadDB.Size = new System.Drawing.Size(130, 23);
            this.buttonReadDB.TabIndex = 0;
            this.buttonReadDB.Text = "Read from dataBase";
            this.buttonReadDB.UseVisualStyleBackColor = true;
            this.buttonReadDB.Click += new System.EventHandler(this.buttonReadDB_Click);
            // 
            // ControlDescriptionEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 366);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ControlDescriptionEditorDialog";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBoxDB.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.GroupBox groupBoxDB;
        private System.Windows.Forms.Button buttonWriteDB;
        private System.Windows.Forms.Button buttonReadDB;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}