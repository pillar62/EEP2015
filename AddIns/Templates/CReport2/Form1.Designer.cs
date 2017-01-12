namespace CReport2
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Master = new Srvtools.InfoDataSet(this.components);
            this.ibsMaster = new Srvtools.InfoBindingSource(this.components);
            this.clientQuery1 = new Srvtools.ClientQuery(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Master)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsMaster)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Master
            // 
            this.Master.Active = false;
            this.Master.AlwaysClose = false;
            this.Master.LastKeyValues = null;
            this.Master.PacketRecords = -1;
            this.Master.Position = -1;
            this.Master.RemoteName = "";
            this.Master.ServerModify = false;
            // 
            // ibsMaster
            // 
            this.ibsMaster.AllowAdd = true;
            this.ibsMaster.AllowDelete = true;
            this.ibsMaster.AllowPrint = true;
            this.ibsMaster.AllowUpdate = true;
            this.ibsMaster.AutoApply = false;
            this.ibsMaster.AutoApplyMaster = false;
            this.ibsMaster.AutoDisibleControl = false;
            this.ibsMaster.AutoRecordLock = false;
            this.ibsMaster.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.ibsMaster.CloseProtect = false;
            this.ibsMaster.DataSource = this.Master;
            this.ibsMaster.DelayInterval = 300;
            this.ibsMaster.DisableKeyFields = false;
            this.ibsMaster.EnableFlag = false;
            this.ibsMaster.FocusedControl = null;
            this.ibsMaster.OwnerComp = null;
            this.ibsMaster.Position = 0;
            this.ibsMaster.RelationDelay = false;
            this.ibsMaster.text = "ibsMaster";
            // 
            // clientQuery1
            // 
            this.clientQuery1.BindingSource = this.ibsMaster;
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
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(474, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.DisplayGroupTree = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 79);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.Size = new System.Drawing.Size(637, 289);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 79);
            this.panel1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 365);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.crystalReportViewer1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Master)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsMaster)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Srvtools.InfoDataSet Master;
        private Srvtools.InfoBindingSource ibsMaster;
        private Srvtools.ClientQuery clientQuery1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Panel panel1;

    }
}