namespace MWizard2015
{
    partial class ErrorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblStackTrace = new System.Windows.Forms.Label();
            this.txtStack = new System.Windows.Forms.TextBox();
            this.btnSOrHStack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(108, 166);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Location = new System.Drawing.Point(22, 27);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(426, 120);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblMessage.ForeColor = System.Drawing.Color.Maroon;
            this.lblMessage.Location = new System.Drawing.Point(20, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(55, 12);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Message:";
            // 
            // lblStackTrace
            // 
            this.lblStackTrace.AutoSize = true;
            this.lblStackTrace.BackColor = System.Drawing.Color.Transparent;
            this.lblStackTrace.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblStackTrace.ForeColor = System.Drawing.Color.Maroon;
            this.lblStackTrace.Location = new System.Drawing.Point(20, 155);
            this.lblStackTrace.Name = "lblStackTrace";
            this.lblStackTrace.Size = new System.Drawing.Size(39, 12);
            this.lblStackTrace.TabIndex = 4;
            this.lblStackTrace.Text = "Stack:";
            // 
            // txtStack
            // 
            this.txtStack.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStack.Location = new System.Drawing.Point(22, 171);
            this.txtStack.Multiline = true;
            this.txtStack.Name = "txtStack";
            this.txtStack.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStack.Size = new System.Drawing.Size(426, 127);
            this.txtStack.TabIndex = 5;
            // 
            // btnSOrHStack
            // 
            this.btnSOrHStack.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSOrHStack.Location = new System.Drawing.Point(22, 166);
            this.btnSOrHStack.Name = "btnSOrHStack";
            this.btnSOrHStack.Size = new System.Drawing.Size(80, 23);
            this.btnSOrHStack.TabIndex = 6;
            this.btnSOrHStack.Text = "Show Stack";
            this.btnSOrHStack.UseVisualStyleBackColor = true;
            this.btnSOrHStack.Click += new System.EventHandler(this.btnSOrHStack_Click);
            // 
            // ErrorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(471, 209);
            this.Controls.Add(this.btnSOrHStack);
            this.Controls.Add(this.lblStackTrace);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtStack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP Error Message";
            this.Load += new System.EventHandler(this.ErrorDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblStackTrace;
        private System.Windows.Forms.TextBox txtStack;
        private System.Windows.Forms.Button btnSOrHStack;
    }
}