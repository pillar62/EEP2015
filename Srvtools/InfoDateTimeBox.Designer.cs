namespace Srvtools
{
    partial class InfoDateTimeBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // maskedTextBox
            // 
            this.maskedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox.Location = new System.Drawing.Point(0, 0);
            this.maskedTextBox.Name = "maskedTextBox";
            this.maskedTextBox.Size = new System.Drawing.Size(150, 21);
            this.maskedTextBox.TabIndex = 0;
            this.maskedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // button
            // 
            this.button.Dock = System.Windows.Forms.DockStyle.Right;
            this.button.Location = new System.Drawing.Point(150, 0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(21, 21);
            this.button.TabIndex = 1;
            this.button.TabStop = false;
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // InfoDateTimeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.maskedTextBox);
            this.Controls.Add(this.button);
            this.Name = "InfoDateTimeBox";
            this.Size = new System.Drawing.Size(171, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTextBox;
        private System.Windows.Forms.Button button;
    }
}
