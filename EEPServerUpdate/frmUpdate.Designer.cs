namespace EEPServerUpdate
{
    partial class frmUpdate
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
            this.progressBarUpdate = new System.Windows.Forms.ProgressBar();
            this.lbUpdate = new System.Windows.Forms.Label();
            this.lbtime = new System.Windows.Forms.Label();
            this.lbprocess = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarUpdate
            // 
            this.progressBarUpdate.Location = new System.Drawing.Point(14, 33);
            this.progressBarUpdate.Name = "progressBarUpdate";
            this.progressBarUpdate.Size = new System.Drawing.Size(300, 23);
            this.progressBarUpdate.TabIndex = 0;
            // 
            // lbUpdate
            // 
            this.lbUpdate.AutoSize = true;
            this.lbUpdate.Location = new System.Drawing.Point(157, 12);
            this.lbUpdate.Name = "lbUpdate";
            this.lbUpdate.Size = new System.Drawing.Size(65, 12);
            this.lbUpdate.TabIndex = 1;
            this.lbUpdate.Text = "Updatefile";
            // 
            // lbtime
            // 
            this.lbtime.AutoSize = true;
            this.lbtime.Location = new System.Drawing.Point(271, 69);
            this.lbtime.Name = "lbtime";
            this.lbtime.Size = new System.Drawing.Size(29, 12);
            this.lbtime.TabIndex = 2;
            this.lbtime.Text = "Time";
            // 
            // lbprocess
            // 
            this.lbprocess.AutoSize = true;
            this.lbprocess.Location = new System.Drawing.Point(64, 69);
            this.lbprocess.Name = "lbprocess";
            this.lbprocess.Size = new System.Drawing.Size(11, 12);
            this.lbprocess.TabIndex = 3;
            this.lbprocess.Text = "%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Update File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Time Remain:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "finished";
            // 
            // frmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 95);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbprocess);
            this.Controls.Add(this.lbUpdate);
            this.Controls.Add(this.lbtime);
            this.Controls.Add(this.progressBarUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEPUpdate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpdate_FormClosing);
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarUpdate;
        private System.Windows.Forms.Label lbUpdate;
        private System.Windows.Forms.Label lbtime;
        private System.Windows.Forms.Label lbprocess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

