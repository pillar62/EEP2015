namespace EEPNetRunStep
{
    partial class frmEEPNetRunStep
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
            this.labUserID = new System.Windows.Forms.Label();
            this.labPassword = new System.Windows.Forms.Label();
            this.labDataBase = new System.Windows.Forms.Label();
            this.labSolution = new System.Windows.Forms.Label();
            this.tbUserID = new System.Windows.Forms.TextBox();
            this.tbDataBase = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSolution = new System.Windows.Forms.TextBox();
            this.labPackageName = new System.Windows.Forms.Label();
            this.labFormName = new System.Windows.Forms.Label();
            this.labTimes = new System.Windows.Forms.Label();
            this.tbPackageName = new System.Windows.Forms.TextBox();
            this.tbFormName = new System.Windows.Forms.TextBox();
            this.tbTimes = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labUserID
            // 
            this.labUserID.AutoSize = true;
            this.labUserID.Location = new System.Drawing.Point(12, 30);
            this.labUserID.Name = "labUserID";
            this.labUserID.Size = new System.Drawing.Size(47, 12);
            this.labUserID.TabIndex = 0;
            this.labUserID.Text = "UserID:";
            // 
            // labPassword
            // 
            this.labPassword.AutoSize = true;
            this.labPassword.Location = new System.Drawing.Point(12, 68);
            this.labPassword.Name = "labPassword";
            this.labPassword.Size = new System.Drawing.Size(59, 12);
            this.labPassword.TabIndex = 1;
            this.labPassword.Text = "Password:";
            // 
            // labDataBase
            // 
            this.labDataBase.AutoSize = true;
            this.labDataBase.Location = new System.Drawing.Point(12, 109);
            this.labDataBase.Name = "labDataBase";
            this.labDataBase.Size = new System.Drawing.Size(65, 12);
            this.labDataBase.TabIndex = 2;
            this.labDataBase.Text = "Data Base:";
            // 
            // labSolution
            // 
            this.labSolution.AutoSize = true;
            this.labSolution.Location = new System.Drawing.Point(12, 148);
            this.labSolution.Name = "labSolution";
            this.labSolution.Size = new System.Drawing.Size(59, 12);
            this.labSolution.TabIndex = 3;
            this.labSolution.Text = "Solution:";
            // 
            // tbUserID
            // 
            this.tbUserID.Location = new System.Drawing.Point(87, 27);
            this.tbUserID.Name = "tbUserID";
            this.tbUserID.Size = new System.Drawing.Size(100, 21);
            this.tbUserID.TabIndex = 4;
            // 
            // tbDataBase
            // 
            this.tbDataBase.Location = new System.Drawing.Point(87, 106);
            this.tbDataBase.Name = "tbDataBase";
            this.tbDataBase.Size = new System.Drawing.Size(100, 21);
            this.tbDataBase.TabIndex = 5;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(87, 65);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(100, 21);
            this.tbPassword.TabIndex = 6;
            // 
            // tbSolution
            // 
            this.tbSolution.Location = new System.Drawing.Point(87, 144);
            this.tbSolution.Name = "tbSolution";
            this.tbSolution.Size = new System.Drawing.Size(100, 21);
            this.tbSolution.TabIndex = 7;
            // 
            // labPackageName
            // 
            this.labPackageName.AutoSize = true;
            this.labPackageName.Location = new System.Drawing.Point(208, 30);
            this.labPackageName.Name = "labPackageName";
            this.labPackageName.Size = new System.Drawing.Size(83, 12);
            this.labPackageName.TabIndex = 8;
            this.labPackageName.Text = "Package Name:";
            // 
            // labFormName
            // 
            this.labFormName.AutoSize = true;
            this.labFormName.Location = new System.Drawing.Point(208, 68);
            this.labFormName.Name = "labFormName";
            this.labFormName.Size = new System.Drawing.Size(65, 12);
            this.labFormName.TabIndex = 9;
            this.labFormName.Text = "Form Name:";
            // 
            // labTimes
            // 
            this.labTimes.AutoSize = true;
            this.labTimes.Location = new System.Drawing.Point(208, 109);
            this.labTimes.Name = "labTimes";
            this.labTimes.Size = new System.Drawing.Size(41, 12);
            this.labTimes.TabIndex = 10;
            this.labTimes.Text = "Times:";
            // 
            // tbPackageName
            // 
            this.tbPackageName.Location = new System.Drawing.Point(297, 27);
            this.tbPackageName.Name = "tbPackageName";
            this.tbPackageName.Size = new System.Drawing.Size(100, 21);
            this.tbPackageName.TabIndex = 11;
            // 
            // tbFormName
            // 
            this.tbFormName.Location = new System.Drawing.Point(297, 65);
            this.tbFormName.Name = "tbFormName";
            this.tbFormName.Size = new System.Drawing.Size(100, 21);
            this.tbFormName.TabIndex = 12;
            // 
            // tbTimes
            // 
            this.tbTimes.Location = new System.Drawing.Point(297, 106);
            this.tbTimes.Name = "tbTimes";
            this.tbTimes.Size = new System.Drawing.Size(100, 21);
            this.tbTimes.TabIndex = 13;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(264, 143);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 14;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmEEPNetRunStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 197);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.tbTimes);
            this.Controls.Add(this.tbFormName);
            this.Controls.Add(this.tbPackageName);
            this.Controls.Add(this.labTimes);
            this.Controls.Add(this.labFormName);
            this.Controls.Add(this.labPackageName);
            this.Controls.Add(this.tbSolution);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbDataBase);
            this.Controls.Add(this.tbUserID);
            this.Controls.Add(this.labSolution);
            this.Controls.Add(this.labDataBase);
            this.Controls.Add(this.labPassword);
            this.Controls.Add(this.labUserID);
            this.MaximizeBox = false;
            this.Name = "frmEEPNetRunStep";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEEPNetRunStep";
            this.Load += new System.EventHandler(this.frmEEPNetRunStep_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labUserID;
        private System.Windows.Forms.Label labPassword;
        private System.Windows.Forms.Label labDataBase;
        private System.Windows.Forms.Label labSolution;
        private System.Windows.Forms.TextBox tbUserID;
        private System.Windows.Forms.TextBox tbDataBase;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSolution;
        private System.Windows.Forms.Label labPackageName;
        private System.Windows.Forms.Label labFormName;
        private System.Windows.Forms.Label labTimes;
        private System.Windows.Forms.TextBox tbPackageName;
        private System.Windows.Forms.TextBox tbFormName;
        private System.Windows.Forms.TextBox tbTimes;
        private System.Windows.Forms.Button btnRun;
    }
}

