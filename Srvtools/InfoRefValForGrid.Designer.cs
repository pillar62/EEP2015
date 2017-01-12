namespace Srvtools
{
    partial class InfoRefValForGrid
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
            this.InButton = new System.Windows.Forms.Button();
            this.InTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // InButton
            // 
            this.InButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.InButton.Font = new System.Drawing.Font("ËÎÌו", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InButton.Location = new System.Drawing.Point(97, 0);
            this.InButton.Name = "InButton";
            this.InButton.Size = new System.Drawing.Size(20, 24);
            this.InButton.TabIndex = 1;
            this.InButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InButton.Click += new System.EventHandler(this.InButton_Click);
            // 
            // InTextBox
            // 
            this.InTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InTextBox.Location = new System.Drawing.Point(0, 0);
            this.InTextBox.Name = "InTextBox";
            this.InTextBox.Size = new System.Drawing.Size(97, 21);
            this.InTextBox.TabIndex = 0;
            this.InTextBox.Enter += new System.EventHandler(this.InTextBox_Enter);
            this.InTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InTextBox_KeyDown);
            // 
            // InfoRefValForGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InTextBox);
            this.Controls.Add(this.InButton);
            this.Name = "InfoRefValForGrid";
            this.Size = new System.Drawing.Size(117, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InButton;
        private System.Windows.Forms.TextBox InTextBox;

    }
}
