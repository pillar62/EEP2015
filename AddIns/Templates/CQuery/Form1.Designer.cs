namespace TAG_NAMESPACE
{
    partial class TAG_FORMNAME
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TAG_FORMNAME));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Query = new Srvtools.InfoDataSet(this.components);
            this.ibsQuery = new Srvtools.InfoBindingSource(this.components);
            this.infoStatusStrip1 = new Srvtools.InfoStatusStrip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.infoDataGridView1 = new Srvtools.InfoDataGridView();
            this.clientQuery1 = new Srvtools.ClientQuery();
            ((System.ComponentModel.ISupportInitialize)(this.Query)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsQuery)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Query
            // 
            this.Query.Active = false;
            this.Query.AlwaysClose = false;
            this.Query.PacketRecords = 100;
            this.Query.Position = -1;
            this.Query.RemoteName = "";
            this.Query.ServerModify = false;
            // 
            // ibsQuery
            // 
            this.ibsQuery.AllowAdd = true;
            this.ibsQuery.AllowDelete = true;
            this.ibsQuery.AllowPrint = true;
            this.ibsQuery.AllowUpdate = true;
            this.ibsQuery.AutoApply = false;
            this.ibsQuery.AutoApplyMaster = false;
            this.ibsQuery.AutoDisibleControl = false;
            this.ibsQuery.CloseProtect = false;
            this.ibsQuery.DataSource = this.Query;
            this.ibsQuery.DelayInterval = 300;
            this.ibsQuery.EnableFlag = false;
            this.ibsQuery.FocusedControl = null;
            this.ibsQuery.OwnerComp = null;
            this.ibsQuery.Position = 0;
            this.ibsQuery.RelationDelay = false;
            this.ibsQuery.text = null;
            // 
            // infoStatusStrip1
            // 
            this.infoStatusStrip1.Location = new System.Drawing.Point(0, 376);
            this.infoStatusStrip1.Name = "infoStatusStrip1";
            this.infoStatusStrip1.ShowCompany = false;
            this.infoStatusStrip1.ShowDate = true;
            this.infoStatusStrip1.ShowEEPAlias = true;
            this.infoStatusStrip1.ShowNavigatorStatus = true;
            this.infoStatusStrip1.ShowSolution = true;
            this.infoStatusStrip1.ShowUserID = true;
            this.infoStatusStrip1.ShowUserName = true;
            this.infoStatusStrip1.Size = new System.Drawing.Size(648, 22);
            this.infoStatusStrip1.TabIndex = 2;
            this.infoStatusStrip1.Text = "infoStatusStrip1";
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
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.infoDataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(648, 376);
            this.splitContainer1.SplitterDistance = 148;
            this.splitContainer1.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(530, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(530, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // infoDataGridView1
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.infoDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.infoDataGridView1.AutoGenerateColumns = false;
            this.infoDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoDataGridView1.DataSource = this.ibsQuery;
            this.infoDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoDataGridView1.EnterEnable = true;
            this.infoDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.infoDataGridView1.Name = "infoDataGridView1";
            this.infoDataGridView1.RowHeadersWidth = 25;
            this.infoDataGridView1.RowTemplate.Height = 24;
            this.infoDataGridView1.Size = new System.Drawing.Size(648, 224);
            this.infoDataGridView1.SureDelete = false;
            this.infoDataGridView1.TabIndex = 0;
            this.infoDataGridView1.TotalActive = true;
            this.infoDataGridView1.TotalBackColor = System.Drawing.SystemColors.Info;
            this.infoDataGridView1.TotalCaption = null;
            this.infoDataGridView1.TotalCaptionFont = new System.Drawing.Font("SimSun", 9F);
            this.infoDataGridView1.TotalFont = new System.Drawing.Font("SimSun", 9F);
            // 
            // clientQuery1
            // 
            this.clientQuery1.BindingSource = this.ibsQuery;
            this.clientQuery1.Caption = "";
            this.clientQuery1.Font = new System.Drawing.Font("SimSun", 9F);
            this.clientQuery1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.clientQuery1.GapHorizontal = 80;
            this.clientQuery1.GapVertical = 20;
            this.clientQuery1.isShow = ((System.Collections.Generic.List<string>)(resources.GetObject("clientQuery1.isShow")));
            this.clientQuery1.isShowInsp = false;
            this.clientQuery1.KeepCondition = false;
            this.clientQuery1.Margin = new System.Drawing.Printing.Margins(100, 30, 30, 30);
            this.clientQuery1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(648, 398);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.infoStatusStrip1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Query)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsQuery)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Srvtools.InfoBindingSource ibsQuery;
        private Srvtools.InfoDataSet Query;
        private Srvtools.InfoStatusStrip infoStatusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private Srvtools.InfoDataGridView infoDataGridView1;
        private Srvtools.ClientQuery clientQuery1;
    }
}
