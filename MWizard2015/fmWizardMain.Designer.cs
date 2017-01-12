namespace MWizard2015
{
    partial class fmWizardMain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelSolution = new System.Windows.Forms.Panel();
            this.rbSetServerPath = new System.Windows.Forms.RadioButton();
            this.rbEmptySolution = new System.Windows.Forms.RadioButton();
            this.panelAdo = new System.Windows.Forms.Panel();
            this.rbJQueryToJQMobile = new System.Windows.Forms.RadioButton();
            this.rbJQMobileForm = new System.Windows.Forms.RadioButton();
            this.rbRDLC = new System.Windows.Forms.RadioButton();
            this.rbJQueryWebForm = new System.Windows.Forms.RadioButton();
            this.rbServerPackage = new System.Windows.Forms.RadioButton();
            this.rbWebReport = new System.Windows.Forms.RadioButton();
            this.rbWinFormPackage = new System.Windows.Forms.RadioButton();
            this.rbWinReport = new System.Windows.Forms.RadioButton();
            this.rbWebForm = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panelSolution.SuspendLayout();
            this.panelAdo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(730, 356);
            this.panel1.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNext.Location = new System.Drawing.Point(185, 306);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 27);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(461, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.panel9);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panelSolution);
            this.panel2.Controls.Add(this.panelAdo);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(726, 289);
            this.panel2.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(176, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 16);
            this.label9.TabIndex = 1;
            this.label9.Text = "Server";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(353, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 16);
            this.label10.TabIndex = 2;
            this.label10.Text = "Client";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(593, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "Report";
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.label6);
            this.panel9.Location = new System.Drawing.Point(11, 192);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(106, 84);
            this.panel9.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(4, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Solution";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Location = new System.Drawing.Point(11, 50);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(106, 136);
            this.panel7.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(65, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "net";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "ADO.";
            // 
            // panelSolution
            // 
            this.panelSolution.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSolution.Controls.Add(this.rbSetServerPath);
            this.panelSolution.Controls.Add(this.rbEmptySolution);
            this.panelSolution.Location = new System.Drawing.Point(123, 192);
            this.panelSolution.Name = "panelSolution";
            this.panelSolution.Size = new System.Drawing.Size(593, 84);
            this.panelSolution.TabIndex = 0;
            // 
            // rbSetServerPath
            // 
            this.rbSetServerPath.AutoSize = true;
            this.rbSetServerPath.Location = new System.Drawing.Point(196, 42);
            this.rbSetServerPath.Name = "rbSetServerPath";
            this.rbSetServerPath.Size = new System.Drawing.Size(113, 16);
            this.rbSetServerPath.TabIndex = 1;
            this.rbSetServerPath.TabStop = true;
            this.rbSetServerPath.Text = "Set Server Path";
            this.rbSetServerPath.UseVisualStyleBackColor = true;
            this.rbSetServerPath.Visible = false;
            // 
            // rbEmptySolution
            // 
            this.rbEmptySolution.AutoSize = true;
            this.rbEmptySolution.Location = new System.Drawing.Point(10, 42);
            this.rbEmptySolution.Name = "rbEmptySolution";
            this.rbEmptySolution.Size = new System.Drawing.Size(137, 16);
            this.rbEmptySolution.TabIndex = 0;
            this.rbEmptySolution.TabStop = true;
            this.rbEmptySolution.Text = "Create EEP Solution";
            this.rbEmptySolution.UseVisualStyleBackColor = true;
            this.rbEmptySolution.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // panelAdo
            // 
            this.panelAdo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAdo.Controls.Add(this.rbJQueryToJQMobile);
            this.panelAdo.Controls.Add(this.rbJQMobileForm);
            this.panelAdo.Controls.Add(this.rbRDLC);
            this.panelAdo.Controls.Add(this.rbJQueryWebForm);
            this.panelAdo.Controls.Add(this.rbServerPackage);
            this.panelAdo.Controls.Add(this.rbWebReport);
            this.panelAdo.Controls.Add(this.rbWinFormPackage);
            this.panelAdo.Controls.Add(this.rbWinReport);
            this.panelAdo.Controls.Add(this.rbWebForm);
            this.panelAdo.Location = new System.Drawing.Point(123, 50);
            this.panelAdo.Name = "panelAdo";
            this.panelAdo.Size = new System.Drawing.Size(593, 136);
            this.panelAdo.TabIndex = 9;
            // 
            // rbJQueryToJQMobile
            // 
            this.rbJQueryToJQMobile.AutoSize = true;
            this.rbJQueryToJQMobile.Location = new System.Drawing.Point(196, 100);
            this.rbJQueryToJQMobile.Name = "rbJQueryToJQMobile";
            this.rbJQueryToJQMobile.Size = new System.Drawing.Size(131, 16);
            this.rbJQueryToJQMobile.TabIndex = 10;
            this.rbJQueryToJQMobile.TabStop = true;
            this.rbJQueryToJQMobile.Text = "JQuery to JQMobile";
            this.rbJQueryToJQMobile.UseVisualStyleBackColor = true;
            // 
            // rbJQMobileForm
            // 
            this.rbJQMobileForm.AutoSize = true;
            this.rbJQMobileForm.Location = new System.Drawing.Point(196, 77);
            this.rbJQMobileForm.Name = "rbJQMobileForm";
            this.rbJQMobileForm.Size = new System.Drawing.Size(107, 16);
            this.rbJQMobileForm.TabIndex = 9;
            this.rbJQMobileForm.TabStop = true;
            this.rbJQMobileForm.Text = "JQ Mobile Form";
            this.rbJQMobileForm.UseVisualStyleBackColor = true;
            // 
            // rbRDLC
            // 
            this.rbRDLC.AutoSize = true;
            this.rbRDLC.Location = new System.Drawing.Point(433, 60);
            this.rbRDLC.Name = "rbRDLC";
            this.rbRDLC.Size = new System.Drawing.Size(89, 16);
            this.rbRDLC.TabIndex = 8;
            this.rbRDLC.TabStop = true;
            this.rbRDLC.Text = "Create RDLC";
            this.rbRDLC.UseVisualStyleBackColor = true;
            // 
            // rbJQueryWebForm
            // 
            this.rbJQueryWebForm.AutoSize = true;
            this.rbJQueryWebForm.Location = new System.Drawing.Point(196, 54);
            this.rbJQueryWebForm.Name = "rbJQueryWebForm";
            this.rbJQueryWebForm.Size = new System.Drawing.Size(113, 16);
            this.rbJQueryWebForm.TabIndex = 7;
            this.rbJQueryWebForm.TabStop = true;
            this.rbJQueryWebForm.Text = "JQuery Web Form";
            this.rbJQueryWebForm.UseVisualStyleBackColor = true;
            // 
            // rbServerPackage
            // 
            this.rbServerPackage.AutoSize = true;
            this.rbServerPackage.Checked = true;
            this.rbServerPackage.Location = new System.Drawing.Point(10, 9);
            this.rbServerPackage.Name = "rbServerPackage";
            this.rbServerPackage.Size = new System.Drawing.Size(149, 16);
            this.rbServerPackage.TabIndex = 0;
            this.rbServerPackage.TabStop = true;
            this.rbServerPackage.Text = "Server Package Wizard";
            this.rbServerPackage.UseVisualStyleBackColor = true;
            this.rbServerPackage.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbWebReport
            // 
            this.rbWebReport.AutoSize = true;
            this.rbWebReport.Location = new System.Drawing.Point(433, 34);
            this.rbWebReport.Name = "rbWebReport";
            this.rbWebReport.Size = new System.Drawing.Size(125, 16);
            this.rbWebReport.TabIndex = 4;
            this.rbWebReport.TabStop = true;
            this.rbWebReport.Text = "Create Web Report";
            this.rbWebReport.UseVisualStyleBackColor = true;
            this.rbWebReport.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbWinFormPackage
            // 
            this.rbWinFormPackage.AutoSize = true;
            this.rbWinFormPackage.Location = new System.Drawing.Point(196, 9);
            this.rbWinFormPackage.Name = "rbWinFormPackage";
            this.rbWinFormPackage.Size = new System.Drawing.Size(95, 16);
            this.rbWinFormPackage.TabIndex = 1;
            this.rbWinFormPackage.TabStop = true;
            this.rbWinFormPackage.Text = "Windows Form";
            this.rbWinFormPackage.UseVisualStyleBackColor = true;
            this.rbWinFormPackage.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbWinReport
            // 
            this.rbWinReport.AutoSize = true;
            this.rbWinReport.Location = new System.Drawing.Point(433, 9);
            this.rbWinReport.Name = "rbWinReport";
            this.rbWinReport.Size = new System.Drawing.Size(149, 16);
            this.rbWinReport.TabIndex = 2;
            this.rbWinReport.TabStop = true;
            this.rbWinReport.Text = "Create Windows Report";
            this.rbWinReport.UseVisualStyleBackColor = true;
            this.rbWinReport.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbWebForm
            // 
            this.rbWebForm.AutoSize = true;
            this.rbWebForm.Location = new System.Drawing.Point(196, 32);
            this.rbWebForm.Name = "rbWebForm";
            this.rbWebForm.Size = new System.Drawing.Size(119, 16);
            this.rbWebForm.TabIndex = 3;
            this.rbWebForm.TabStop = true;
            this.rbWebForm.Text = "ASP.net Web Form";
            this.rbWebForm.UseVisualStyleBackColor = true;
            this.rbWebForm.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(21, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select a type of wizard:";
            // 
            // fmWizardMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 356);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmWizardMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InfoLight EEP Wizard Type";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panelSolution.ResumeLayout(false);
            this.panelSolution.PerformLayout();
            this.panelAdo.ResumeLayout(false);
            this.panelAdo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbWebForm;
        private System.Windows.Forms.RadioButton rbWinFormPackage;
        private System.Windows.Forms.RadioButton rbServerPackage;
        private System.Windows.Forms.RadioButton rbEmptySolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbWebReport;
        private System.Windows.Forms.RadioButton rbWinReport;
        private System.Windows.Forms.Panel panelSolution;
        private System.Windows.Forms.Panel panelAdo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbSetServerPath;
        private System.Windows.Forms.RadioButton rbJQueryWebForm;
        private System.Windows.Forms.RadioButton rbRDLC;
        private System.Windows.Forms.RadioButton rbJQMobileForm;
        private System.Windows.Forms.RadioButton rbJQueryToJQMobile;
    }
}