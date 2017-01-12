namespace Srvtools.Report
{
    partial class frmSelectReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectReport));
            this.labelReportID = new System.Windows.Forms.Label();
            this.listBoxReportID = new System.Windows.Forms.ListBox();
            this.labelTemplateName = new System.Windows.Forms.Label();
            this.listBoxTemplateName = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.infoDataSetSYS_REPORT = new Srvtools.InfoDataSet(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.infoDataSetSYS_REPORT)).BeginInit();
            this.SuspendLayout();
            // 
            // labelReportID
            // 
            this.labelReportID.AutoSize = true;
            this.labelReportID.Location = new System.Drawing.Point(34, 30);
            this.labelReportID.Name = "labelReportID";
            this.labelReportID.Size = new System.Drawing.Size(53, 12);
            this.labelReportID.TabIndex = 0;
            this.labelReportID.Text = "ReportID";
            // 
            // listBoxReportID
            // 
            this.listBoxReportID.FormattingEnabled = true;
            this.listBoxReportID.ItemHeight = 12;
            this.listBoxReportID.Location = new System.Drawing.Point(36, 56);
            this.listBoxReportID.Name = "listBoxReportID";
            this.listBoxReportID.Size = new System.Drawing.Size(120, 208);
            this.listBoxReportID.TabIndex = 1;
            this.listBoxReportID.SelectedIndexChanged += new System.EventHandler(this.listBoxReportID_SelectedIndexChanged);
            // 
            // labelTemplateName
            // 
            this.labelTemplateName.AutoSize = true;
            this.labelTemplateName.Location = new System.Drawing.Point(176, 30);
            this.labelTemplateName.Name = "labelTemplateName";
            this.labelTemplateName.Size = new System.Drawing.Size(83, 12);
            this.labelTemplateName.TabIndex = 2;
            this.labelTemplateName.Text = "Template Name";
            // 
            // listBoxTemplateName
            // 
            this.listBoxTemplateName.FormattingEnabled = true;
            this.listBoxTemplateName.ItemHeight = 12;
            this.listBoxTemplateName.Location = new System.Drawing.Point(178, 56);
            this.listBoxTemplateName.Name = "listBoxTemplateName";
            this.listBoxTemplateName.Size = new System.Drawing.Size(120, 208);
            this.listBoxTemplateName.TabIndex = 3;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(178, 280);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(60, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(244, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(54, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // infoDataSetSYS_REPORT
            // 
            this.infoDataSetSYS_REPORT.Active = false;
            this.infoDataSetSYS_REPORT.AlwaysClose = false;
            this.infoDataSetSYS_REPORT.DataCompressed = false;
            this.infoDataSetSYS_REPORT.DeleteIncomplete = true;
            this.infoDataSetSYS_REPORT.LastKeyValues = null;
            this.infoDataSetSYS_REPORT.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.infoDataSetSYS_REPORT.PacketRecords = 100;
            this.infoDataSetSYS_REPORT.Position = -1;
            this.infoDataSetSYS_REPORT.RemoteName = "GLModule.cmdSYS_REPORT";
            this.infoDataSetSYS_REPORT.ServerModify = false;
            // 
            // frmSelectReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 315);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listBoxTemplateName);
            this.Controls.Add(this.labelTemplateName);
            this.Controls.Add(this.listBoxReportID);
            this.Controls.Add(this.labelReportID);
            this.Name = "frmSelectReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please Select Report";
            this.Load += new System.EventHandler(this.frmSelectReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infoDataSetSYS_REPORT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelReportID;
        private System.Windows.Forms.ListBox listBoxReportID;
        private System.Windows.Forms.Label labelTemplateName;
        private System.Windows.Forms.ListBox listBoxTemplateName;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private InfoDataSet infoDataSetSYS_REPORT;
    }
}