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
            this.clientQuery1 = new Srvtools.ClientQuery(this.components);
            this.easilyReport1 = new Infolight.EasilyReportTools.EasilyReport();
            this.btnPrint = new System.Windows.Forms.Button();
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
            this.Query.DeleteIncomplete = true;
            this.Query.LastKeyValues = null;
            this.Query.Locale = new System.Globalization.CultureInfo("zh-CN");
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
            this.ibsQuery.AutoDisableControl = false;
            this.ibsQuery.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.ibsQuery.AutoRecordLock = false;
            this.ibsQuery.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.ibsQuery.CloseProtect = false;
            this.ibsQuery.DataSource = this.Query;
            this.ibsQuery.DelayInterval = 300;
            this.ibsQuery.DisableKeyFields = false;
            this.ibsQuery.EnableFlag = false;
            this.ibsQuery.FocusedControl = null;
            this.ibsQuery.OwnerComp = null;
            this.ibsQuery.Position = 0;
            this.ibsQuery.RelationDelay = false;
            this.ibsQuery.ServerModifyCache = false;
            this.ibsQuery.text = "ibsQuery";
            // 
            // infoStatusStrip1
            // 
            this.infoStatusStrip1.Location = new System.Drawing.Point(0, 372);
            this.infoStatusStrip1.Name = "infoStatusStrip1";
            this.infoStatusStrip1.ShowCompany = false;
            this.infoStatusStrip1.ShowDate = true;
            this.infoStatusStrip1.ShowEEPAlias = true;
            this.infoStatusStrip1.ShowNavigatorStatus = true;
            this.infoStatusStrip1.ShowProgress = false;
            this.infoStatusStrip1.ShowSolution = true;
            this.infoStatusStrip1.ShowUserID = true;
            this.infoStatusStrip1.ShowUserName = true;
            this.infoStatusStrip1.Size = new System.Drawing.Size(648, 26);
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
            this.splitContainer1.Panel1.Controls.Add(this.btnPrint);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.infoDataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(648, 372);
            this.splitContainer1.SplitterDistance = 146;
            this.splitContainer1.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(531, 60);
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
            this.button1.Location = new System.Drawing.Point(531, 22);
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
            this.infoDataGridView1.EnterRefValControl = false;
            this.infoDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.infoDataGridView1.Name = "infoDataGridView1";
            this.infoDataGridView1.RowHeadersWidth = 25;
            this.infoDataGridView1.RowTemplate.Height = 24;
            this.infoDataGridView1.Size = new System.Drawing.Size(648, 222);
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
            this.clientQuery1.isShow = null;
            this.clientQuery1.isShowInsp = false;
            this.clientQuery1.KeepCondition = false;
            this.clientQuery1.Margin = new System.Drawing.Printing.Margins(100, 30, 30, 30);
            this.clientQuery1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // easilyReport1
            // 
            this.easilyReport1.BindingSource = this.ibsQuery;
            this.easilyReport1.Description = null;
            this.easilyReport1.FieldFont = new System.Drawing.Font("SimSun", 9F);
            this.easilyReport1.FilePath = "C:\\EasilyReport20090619.xls";
            this.easilyReport1.FooterFont = new System.Drawing.Font("SimSun", 9F);
            this.easilyReport1.Format.ColumnGap = 0;
            this.easilyReport1.Format.ColumnGridLine = true;
            this.easilyReport1.Format.ColumnInsideGridLine = true;
            this.easilyReport1.Format.DateFormat = Infolight.EasilyReportTools.ReportFormat.DateFormatType.Date;
            this.easilyReport1.Format.ExportFormat = Infolight.EasilyReportTools.ReportFormat.ExportType.Excel;
            this.easilyReport1.Format.MarginBottom = 0;
            this.easilyReport1.Format.MarginLeft = 0;
            this.easilyReport1.Format.MarginRight = 0;
            this.easilyReport1.Format.MarginTop = 0;
            this.easilyReport1.Format.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.easilyReport1.Format.PageHeight = 0;
            this.easilyReport1.Format.PageIndexFormat = Infolight.EasilyReportTools.ReportFormat.PageIndexFormatType.Current;
            this.easilyReport1.Format.PageRecords = 30;
            this.easilyReport1.Format.PageSize = Infolight.EasilyReportTools.ReportFormat.PageType.A4;
            this.easilyReport1.Format.RowGap = 0;
            this.easilyReport1.Format.RowGridLine = true;
            this.easilyReport1.Format.UserFormat = Infolight.EasilyReportTools.ReportFormat.UserFormatType.ID;
            this.easilyReport1.HeaderBindingSource = null;
            this.easilyReport1.HeaderFont = new System.Drawing.Font("SimSun", 9F);
            this.easilyReport1.MailSetting.Body = null;
            this.easilyReport1.MailSetting.Encoding = "gb2312";
            this.easilyReport1.MailSetting.Host = null;
            this.easilyReport1.MailSetting.MailFrom = null;
            this.easilyReport1.MailSetting.MailTo = null;
            this.easilyReport1.MailSetting.Password = null;
            this.easilyReport1.MailSetting.Port = 25;
            this.easilyReport1.MailSetting.Subject = null;
            this.easilyReport1.OutputMode = Infolight.EasilyReportTools.OutputModeType.None;
            this.easilyReport1.ReportID = "EasilyReport";
            this.easilyReport1.ReportName = "EasilyReport20090619";
            this.easilyReport1.Setting.CaptionStyle = Infolight.EasilyReportTools.ReportSetting.CaptionStyleType.ColumnHeader;
            this.easilyReport1.Setting.GroupGap = Infolight.EasilyReportTools.ReportSetting.GroupGapType.None;
            this.easilyReport1.Setting.GroupTotal = false;
            this.easilyReport1.Setting.GroupTotalCaption = null;
            this.easilyReport1.Setting.TotalCaption = null;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(531, 96);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(88, 32);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // TAG_FORMNAME
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(648, 398);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.infoStatusStrip1);
            this.Name = "TAG_FORMNAME";
            this.Load += new System.EventHandler(this.TAG_FORMNAME_Load);
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
        private System.Windows.Forms.Button btnPrint;
        private Infolight.EasilyReportTools.EasilyReport easilyReport1;
    }
}
