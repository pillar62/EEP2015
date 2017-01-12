namespace EEPNetClient
{
    partial class CurProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurProject));
            this.lblSolution = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.infoCmbSolution = new Srvtools.InfoComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.infoCmbSolution)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.BackColor = System.Drawing.Color.Transparent;
            this.lblSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolution.ForeColor = System.Drawing.Color.Navy;
            this.lblSolution.Location = new System.Drawing.Point(12, 18);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(111, 15);
            this.lblSolution.TabIndex = 0;
            this.lblSolution.Text = "Current Solution";
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(116, 66);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(72, 24);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(207, 66);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // infoCmbSolution
            // 
            this.infoCmbSolution.EnterEnable = false;
            this.infoCmbSolution.Expression = "";
            this.infoCmbSolution.Filter = "";
            this.infoCmbSolution.FormattingEnabled = true;
            this.infoCmbSolution.Location = new System.Drawing.Point(129, 18);
            this.infoCmbSolution.Name = "infoCmbSolution";
            this.infoCmbSolution.SelectAlias = null;
            this.infoCmbSolution.SelectCommand = null;
            this.infoCmbSolution.SelectTop = null;
            this.infoCmbSolution.Size = new System.Drawing.Size(148, 20);
            this.infoCmbSolution.TabIndex = 4;
            // 
            // CurProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EEPNetClient.Properties.Resources.EEPINIT;
            this.ClientSize = new System.Drawing.Size(299, 116);
            this.Controls.Add(this.infoCmbSolution);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblSolution);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CurProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select A Solution";
            this.Load += new System.EventHandler(this.CurProject_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infoCmbSolution)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private Srvtools.InfoComboBox infoCmbSolution;
    }
}