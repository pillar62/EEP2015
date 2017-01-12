namespace Srvtools
{
    partial class InfoRefvalBox
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
            this.InTextBox = new Srvtools.InfoTextBox();
            this.SuspendLayout();
            // 
            // InButton
            // 
            this.InButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.InButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.InButton.Location = new System.Drawing.Point(95, 0);
            this.InButton.Name = "InButton";
            this.InButton.Size = new System.Drawing.Size(20, 22);
            this.InButton.TabIndex = 2;
            this.InButton.TabStop = false;
            this.InButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InButton.Click += new System.EventHandler(this.InButton_Click);
            // 
            // InTextBox
            // 
            this.InTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InTextBox.EnterEnable = true;
            this.InTextBox.LeaveText = null;
            this.InTextBox.Location = new System.Drawing.Point(0, 0);
            this.InTextBox.Name = "InTextBox";
            this.InTextBox.RefVal = null;
            this.InTextBox.Size = new System.Drawing.Size(95, 21);
            this.InTextBox.TabIndex = 1;
            // 
            // InfoRefvalBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InTextBox);
            this.Controls.Add(this.InButton);
            this.Name = "InfoRefvalBox";
            this.Size = new System.Drawing.Size(115, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button InButton;
        private Srvtools.InfoTextBox InTextBox;
	}
}
