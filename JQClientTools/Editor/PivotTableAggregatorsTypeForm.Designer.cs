namespace JQClientTools.Editor
{
    partial class PivotTableAggregatorsTypeForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbleft = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbright = new System.Windows.Forms.ListBox();
            this.btrightall = new System.Windows.Forms.Button();
            this.btright = new System.Windows.Forms.Button();
            this.btleft = new System.Windows.Forms.Button();
            this.btleftall = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(192, 263);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(192, 292);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbleft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(161, 342);
            this.panel1.TabIndex = 3;
            // 
            // lbleft
            // 
            this.lbleft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbleft.FormattingEnabled = true;
            this.lbleft.ItemHeight = 12;
            this.lbleft.Location = new System.Drawing.Point(0, 0);
            this.lbleft.Name = "lbleft";
            this.lbleft.Size = new System.Drawing.Size(161, 342);
            this.lbleft.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbright);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(311, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(163, 342);
            this.panel2.TabIndex = 4;
            // 
            // lbright
            // 
            this.lbright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbright.FormattingEnabled = true;
            this.lbright.ItemHeight = 12;
            this.lbright.Location = new System.Drawing.Point(0, 0);
            this.lbright.Name = "lbright";
            this.lbright.Size = new System.Drawing.Size(163, 342);
            this.lbright.TabIndex = 1;
            // 
            // btrightall
            // 
            this.btrightall.Location = new System.Drawing.Point(192, 43);
            this.btrightall.Name = "btrightall";
            this.btrightall.Size = new System.Drawing.Size(75, 23);
            this.btrightall.TabIndex = 5;
            this.btrightall.Text = ">>";
            this.btrightall.UseVisualStyleBackColor = true;
            this.btrightall.Click += new System.EventHandler(this.btrightall_Click);
            // 
            // btright
            // 
            this.btright.Location = new System.Drawing.Point(192, 91);
            this.btright.Name = "btright";
            this.btright.Size = new System.Drawing.Size(75, 23);
            this.btright.TabIndex = 6;
            this.btright.Text = ">";
            this.btright.UseVisualStyleBackColor = true;
            this.btright.Click += new System.EventHandler(this.btright_Click);
            // 
            // btleft
            // 
            this.btleft.Location = new System.Drawing.Point(192, 141);
            this.btleft.Name = "btleft";
            this.btleft.Size = new System.Drawing.Size(75, 23);
            this.btleft.TabIndex = 7;
            this.btleft.Text = "<";
            this.btleft.UseVisualStyleBackColor = true;
            this.btleft.Click += new System.EventHandler(this.btleft_Click);
            // 
            // btleftall
            // 
            this.btleftall.Location = new System.Drawing.Point(192, 194);
            this.btleftall.Name = "btleftall";
            this.btleftall.Size = new System.Drawing.Size(75, 23);
            this.btleftall.TabIndex = 8;
            this.btleftall.Text = "<<";
            this.btleftall.UseVisualStyleBackColor = true;
            this.btleftall.Click += new System.EventHandler(this.btleftall_Click);
            // 
            // PivotTableAggregatorsTypeForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(474, 342);
            this.Controls.Add(this.btleftall);
            this.Controls.Add(this.btleft);
            this.Controls.Add(this.btright);
            this.Controls.Add(this.btrightall);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PivotTableAggregatorsTypeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PivotTableAggregatorsTypeForm";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btrightall;
        private System.Windows.Forms.Button btright;
        private System.Windows.Forms.Button btleft;
        private System.Windows.Forms.Button btleftall;
        private System.Windows.Forms.ListBox lbleft;
        private System.Windows.Forms.ListBox lbright;
    }
}