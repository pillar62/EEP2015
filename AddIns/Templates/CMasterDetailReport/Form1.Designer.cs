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
            this.infoStatusStrip1 = new Srvtools.InfoStatusStrip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btQuery = new System.Windows.Forms.Button();
            this.btClear = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.clientQuery1 = new Srvtools.ClientQuery(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoStatusStrip1
            // 
            this.infoStatusStrip1.Location = new System.Drawing.Point(0, 412);
            this.infoStatusStrip1.Name = "infoStatusStrip1";
            this.infoStatusStrip1.ShowCompany = false;
            this.infoStatusStrip1.ShowDate = true;
            this.infoStatusStrip1.ShowEEPAlias = true;
            this.infoStatusStrip1.ShowNavigatorStatus = true;
            this.infoStatusStrip1.ShowProgress = false;
            this.infoStatusStrip1.ShowSolution = true;
            this.infoStatusStrip1.ShowUserID = true;
            this.infoStatusStrip1.ShowUserName = true;
            this.infoStatusStrip1.Size = new System.Drawing.Size(588, 22);
            this.infoStatusStrip1.TabIndex = 3;
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
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.reportViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(588, 412);
            this.splitContainer1.SplitterDistance = 136;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btQuery);
            this.panel1.Controls.Add(this.btClear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 136);
            this.panel1.TabIndex = 7;
            // 
            // btQuery
            // 
            this.btQuery.Location = new System.Drawing.Point(457, 41);
            this.btQuery.Name = "btQuery";
            this.btQuery.Size = new System.Drawing.Size(75, 23);
            this.btQuery.TabIndex = 5;
            this.btQuery.Text = "Query";
            this.btQuery.UseVisualStyleBackColor = true;
            this.btQuery.Click += new System.EventHandler(this.btQuery_Click);
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(457, 70);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(75, 23);
            this.btClear.TabIndex = 6;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(588, 272);
            this.reportViewer1.TabIndex = 0;
            // 
            // clientQuery1
            // 
            this.clientQuery1.BindingSource = null;
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
            // TAG_FORMNAME
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(588, 434);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.infoStatusStrip1);
            this.Name = "TAG_FORMNAME";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Srvtools.InfoStatusStrip infoStatusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Button btQuery;
        private System.Windows.Forms.Button btClear;
        private Srvtools.ClientQuery clientQuery1;
        private System.Windows.Forms.Panel panel1;
    }
}
