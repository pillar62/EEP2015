namespace EEPNetClient
{
    partial class frmUserPWD
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
            this.upwdControl1 = new Srvtools.UPWDControl(this.components);
            this.SuspendLayout();
            // 
            // upwdControl1
            // 
            this.upwdControl1.BackColor = System.Drawing.Color.Transparent;
            this.upwdControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.upwdControl1.Location = new System.Drawing.Point(1, 0);
            this.upwdControl1.Name = "upwdControl1";
            this.upwdControl1.Size = new System.Drawing.Size(309, 252);
            this.upwdControl1.TabIndex = 0;
            // 
            // frmUserPWD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 254);
            this.Controls.Add(this.upwdControl1);
            this.MaximizeBox = false;
            this.Name = "frmUserPWD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.ResumeLayout(false);

        }

        #endregion

        public Srvtools.UPWDControl upwdControl1;
    }
}