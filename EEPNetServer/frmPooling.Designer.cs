namespace EEPNetServer
{
    partial class frmPooling
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
            this.listBox = new System.Windows.Forms.ListBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonRelease = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelModule = new System.Windows.Forms.Label();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.groupBoxTotal = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelUsed = new System.Windows.Forms.Label();
            this.groupBoxCurrent = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBoxTotal.SuspendLayout();
            this.groupBoxCurrent.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 12;
            this.listBox.Location = new System.Drawing.Point(-1, -1);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(204, 268);
            this.listBox.TabIndex = 0;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(92, 84);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonRelease
            // 
            this.buttonRelease.Location = new System.Drawing.Point(164, 99);
            this.buttonRelease.Name = "buttonRelease";
            this.buttonRelease.Size = new System.Drawing.Size(75, 23);
            this.buttonRelease.TabIndex = 2;
            this.buttonRelease.Text = "Release";
            this.buttonRelease.UseVisualStyleBackColor = true;
            this.buttonRelease.Click += new System.EventHandler(this.buttonRelease_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "UserID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "DllModuleName:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "AllocateTime:";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(105, 23);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(0, 12);
            this.labelUser.TabIndex = 6;
            // 
            // labelModule
            // 
            this.labelModule.AutoSize = true;
            this.labelModule.Location = new System.Drawing.Point(105, 50);
            this.labelModule.Name = "labelModule";
            this.labelModule.Size = new System.Drawing.Size(0, 12);
            this.labelModule.TabIndex = 7;
            // 
            // labelDateTime
            // 
            this.labelDateTime.AutoSize = true;
            this.labelDateTime.Location = new System.Drawing.Point(105, 75);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(0, 12);
            this.labelDateTime.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "Max Count:";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(109, 32);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(0, 12);
            this.labelMax.TabIndex = 10;
            // 
            // groupBoxTotal
            // 
            this.groupBoxTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTotal.Controls.Add(this.buttonClear);
            this.groupBoxTotal.Controls.Add(this.label4);
            this.groupBoxTotal.Controls.Add(this.labelUsed);
            this.groupBoxTotal.Controls.Add(this.label7);
            this.groupBoxTotal.Controls.Add(this.labelMax);
            this.groupBoxTotal.Controls.Add(this.buttonRefresh);
            this.groupBoxTotal.Location = new System.Drawing.Point(209, 7);
            this.groupBoxTotal.Name = "groupBoxTotal";
            this.groupBoxTotal.Size = new System.Drawing.Size(254, 113);
            this.groupBoxTotal.TabIndex = 11;
            this.groupBoxTotal.TabStop = false;
            this.groupBoxTotal.Text = "Total";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Used Count:";
            // 
            // labelUsed
            // 
            this.labelUsed.AutoSize = true;
            this.labelUsed.Location = new System.Drawing.Point(109, 56);
            this.labelUsed.Name = "labelUsed";
            this.labelUsed.Size = new System.Drawing.Size(0, 12);
            this.labelUsed.TabIndex = 12;
            // 
            // groupBoxCurrent
            // 
            this.groupBoxCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCurrent.Controls.Add(this.buttonRelease);
            this.groupBoxCurrent.Controls.Add(this.label1);
            this.groupBoxCurrent.Controls.Add(this.labelDateTime);
            this.groupBoxCurrent.Controls.Add(this.label2);
            this.groupBoxCurrent.Controls.Add(this.labelModule);
            this.groupBoxCurrent.Controls.Add(this.label3);
            this.groupBoxCurrent.Controls.Add(this.labelUser);
            this.groupBoxCurrent.Location = new System.Drawing.Point(209, 126);
            this.groupBoxCurrent.Name = "groupBoxCurrent";
            this.groupBoxCurrent.Size = new System.Drawing.Size(254, 128);
            this.groupBoxCurrent.TabIndex = 12;
            this.groupBoxCurrent.TabStop = false;
            this.groupBoxCurrent.Text = "Current";
            this.groupBoxCurrent.Visible = false;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(173, 84);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 13;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // frmPooling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 266);
            this.Controls.Add(this.groupBoxCurrent);
            this.Controls.Add(this.groupBoxTotal);
            this.Controls.Add(this.listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPooling";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pooling Manager";
            this.groupBoxTotal.ResumeLayout(false);
            this.groupBoxTotal.PerformLayout();
            this.groupBoxCurrent.ResumeLayout(false);
            this.groupBoxCurrent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonRelease;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelModule;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.GroupBox groupBoxTotal;
        private System.Windows.Forms.GroupBox groupBoxCurrent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelUsed;
        private System.Windows.Forms.Button buttonClear;
    }
}