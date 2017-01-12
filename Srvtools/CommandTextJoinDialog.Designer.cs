namespace Srvtools
{
    partial class CommandTextJoinDialog
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
            this.cmbLeft = new System.Windows.Forms.ComboBox();
            this.cmbOperator = new System.Windows.Forms.ComboBox();
            this.cmbRight = new System.Windows.Forms.ComboBox();
            this.rbdInner = new System.Windows.Forms.RadioButton();
            this.rbdRight = new System.Windows.Forms.RadioButton();
            this.rbdCross = new System.Windows.Forms.RadioButton();
            this.rbdLeft = new System.Windows.Forms.RadioButton();
            this.rbdFull = new System.Windows.Forms.RadioButton();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblRight = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbLeft
            // 
            this.cmbLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLeft.FormattingEnabled = true;
            this.cmbLeft.Location = new System.Drawing.Point(23, 47);
            this.cmbLeft.Name = "cmbLeft";
            this.cmbLeft.Size = new System.Drawing.Size(148, 20);
            this.cmbLeft.TabIndex = 0;
            this.cmbLeft.SelectedIndexChanged += new System.EventHandler(this.cmbLeft_SelectedIndexChanged);
            // 
            // cmbOperator
            // 
            this.cmbOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOperator.FormattingEnabled = true;
            this.cmbOperator.Location = new System.Drawing.Point(180, 47);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(43, 20);
            this.cmbOperator.TabIndex = 1;
            // 
            // cmbRight
            // 
            this.cmbRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRight.FormattingEnabled = true;
            this.cmbRight.Location = new System.Drawing.Point(231, 47);
            this.cmbRight.Name = "cmbRight";
            this.cmbRight.Size = new System.Drawing.Size(148, 20);
            this.cmbRight.TabIndex = 2;
            // 
            // rbdInner
            // 
            this.rbdInner.AutoSize = true;
            this.rbdInner.Location = new System.Drawing.Point(23, 153);
            this.rbdInner.Name = "rbdInner";
            this.rbdInner.Size = new System.Drawing.Size(83, 16);
            this.rbdInner.TabIndex = 3;
            this.rbdInner.Text = "Inner Join";
            this.rbdInner.UseVisualStyleBackColor = true;
            this.rbdInner.Click += new System.EventHandler(this.rbdInner_Click);
            // 
            // rbdRight
            // 
            this.rbdRight.AutoSize = true;
            this.rbdRight.Location = new System.Drawing.Point(22, 122);
            this.rbdRight.Name = "rbdRight";
            this.rbdRight.Size = new System.Drawing.Size(191, 16);
            this.rbdRight.TabIndex = 4;
            this.rbdRight.Text = "Right Join(Right Outer Join)";
            this.rbdRight.UseVisualStyleBackColor = true;
            this.rbdRight.Click += new System.EventHandler(this.rbdRight_Click);
            // 
            // rbdCross
            // 
            this.rbdCross.AutoSize = true;
            this.rbdCross.Location = new System.Drawing.Point(23, 219);
            this.rbdCross.Name = "rbdCross";
            this.rbdCross.Size = new System.Drawing.Size(83, 16);
            this.rbdCross.TabIndex = 5;
            this.rbdCross.Text = "Cross Join";
            this.rbdCross.UseVisualStyleBackColor = true;
            this.rbdCross.Click += new System.EventHandler(this.rbdCross_Click);
            // 
            // rbdLeft
            // 
            this.rbdLeft.AutoSize = true;
            this.rbdLeft.Checked = true;
            this.rbdLeft.Location = new System.Drawing.Point(23, 90);
            this.rbdLeft.Name = "rbdLeft";
            this.rbdLeft.Size = new System.Drawing.Size(179, 16);
            this.rbdLeft.TabIndex = 6;
            this.rbdLeft.TabStop = true;
            this.rbdLeft.Text = "Left Join(Left Outer Join)";
            this.rbdLeft.UseVisualStyleBackColor = true;
            this.rbdLeft.Click += new System.EventHandler(this.rbdLeft_Click);
            // 
            // rbdFull
            // 
            this.rbdFull.AutoSize = true;
            this.rbdFull.Location = new System.Drawing.Point(22, 188);
            this.rbdFull.Name = "rbdFull";
            this.rbdFull.Size = new System.Drawing.Size(179, 16);
            this.rbdFull.TabIndex = 7;
            this.rbdFull.Text = "Full Join(Full Outer Join)";
            this.rbdFull.UseVisualStyleBackColor = true;
            this.rbdFull.Click += new System.EventHandler(this.rbdFull_Click);
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(21, 22);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(35, 12);
            this.lblLeft.TabIndex = 9;
            this.lblLeft.Text = "Left:";
            // 
            // lblRight
            // 
            this.lblRight.AutoSize = true;
            this.lblRight.Location = new System.Drawing.Point(229, 22);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(41, 12);
            this.lblRight.TabIndex = 10;
            this.lblRight.Text = "Right:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(212, 258);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(304, 258);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CommandTextJoinDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 307);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblRight);
            this.Controls.Add(this.lblLeft);
            this.Controls.Add(this.rbdFull);
            this.Controls.Add(this.rbdLeft);
            this.Controls.Add(this.rbdCross);
            this.Controls.Add(this.rbdRight);
            this.Controls.Add(this.rbdInner);
            this.Controls.Add(this.cmbRight);
            this.Controls.Add(this.cmbOperator);
            this.Controls.Add(this.cmbLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommandTextJoinDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Join Option";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLeft;
        private System.Windows.Forms.ComboBox cmbOperator;
        private System.Windows.Forms.ComboBox cmbRight;
        private System.Windows.Forms.RadioButton rbdInner;
        private System.Windows.Forms.RadioButton rbdRight;
        private System.Windows.Forms.RadioButton rbdCross;
        private System.Windows.Forms.RadioButton rbdLeft;
        private System.Windows.Forms.RadioButton rbdFull;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}