namespace EEPManager
{
    partial class SolutionDefineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionDefineForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.dgSolution = new System.Windows.Forms.DataGridView();
            this.iTEMTYPEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iTEMNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.solutionInfo = new Srvtools.InfoDataSet(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgSolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionInfo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(138, 11);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 28);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgSolution
            // 
            this.dgSolution.AutoGenerateColumns = false;
            this.dgSolution.BackgroundColor = System.Drawing.Color.Linen;
            this.dgSolution.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgSolution.ColumnHeadersHeight = 25;
            this.dgSolution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iTEMTYPEDataGridViewTextBoxColumn,
            this.iTEMNAMEDataGridViewTextBoxColumn});
            this.dgSolution.DataMember = "solutionInfo";
            this.dgSolution.DataSource = this.solutionInfo;
            this.dgSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSolution.Location = new System.Drawing.Point(0, 0);
            this.dgSolution.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgSolution.Name = "dgSolution";
            this.dgSolution.RowHeadersWidth = 25;
            this.dgSolution.RowTemplate.Height = 24;
            this.dgSolution.Size = new System.Drawing.Size(393, 205);
            this.dgSolution.TabIndex = 11;
            // 
            // iTEMTYPEDataGridViewTextBoxColumn
            // 
            this.iTEMTYPEDataGridViewTextBoxColumn.DataPropertyName = "ITEMTYPE";
            this.iTEMTYPEDataGridViewTextBoxColumn.HeaderText = "Solution ID";
            this.iTEMTYPEDataGridViewTextBoxColumn.Name = "iTEMTYPEDataGridViewTextBoxColumn";
            this.iTEMTYPEDataGridViewTextBoxColumn.Width = 130;
            // 
            // iTEMNAMEDataGridViewTextBoxColumn
            // 
            this.iTEMNAMEDataGridViewTextBoxColumn.DataPropertyName = "ITEMNAME";
            this.iTEMNAMEDataGridViewTextBoxColumn.HeaderText = "Solution Name";
            this.iTEMNAMEDataGridViewTextBoxColumn.Name = "iTEMNAMEDataGridViewTextBoxColumn";
            this.iTEMNAMEDataGridViewTextBoxColumn.Width = 150;
            // 
            // solutionInfo
            // 
            this.solutionInfo.Active = true;
            this.solutionInfo.AlwaysClose = false;
            this.solutionInfo.DeleteIncomplete = true;
            this.solutionInfo.LastKeyValues = null;
            this.solutionInfo.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.solutionInfo.PacketRecords = -1;
            this.solutionInfo.Position = -1;
            this.solutionInfo.RefCommandText = null;
            this.solutionInfo.RefDBAlias = null;
            this.solutionInfo.RemoteName = "GLModule.solutionInfo";
            this.solutionInfo.ServerModify = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 205);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(393, 49);
            this.panel1.TabIndex = 12;
            // 
            // SolutionDefineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 254);
            this.Controls.Add(this.dgSolution);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "SolutionDefineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solution Define";
            this.Load += new System.EventHandler(this.SolutionDefineForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgSolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionInfo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private Srvtools.InfoDataSet solutionInfo;
        private System.Windows.Forms.DataGridView dgSolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn iTEMTYPEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iTEMNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel panel1;
    }
}