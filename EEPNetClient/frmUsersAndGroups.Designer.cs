namespace EEPNetClient
{
    partial class frmUsersAndGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUsersAndGroups));
            this.ugControl1 = new Srvtools.UGControl();
            this.SuspendLayout();
            // 
            // ugControl1
            // 
            this.ugControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ugControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugControl1.Location = new System.Drawing.Point(0, 0);
            this.ugControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ugControl1.Name = "ugControl1";
            this.ugControl1.Size = new System.Drawing.Size(601, 419);
            this.ugControl1.TabIndex = 0;
            this.ugControl1.Load += new System.EventHandler(this.ugControl1_Load);
            // 
            // frmUsersAndGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 419);
            this.Controls.Add(this.ugControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUsersAndGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Users And Groups";
            this.ResumeLayout(false);

        }

        #endregion

        private Srvtools.UGControl ugControl1;
    }
}