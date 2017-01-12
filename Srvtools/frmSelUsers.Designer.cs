namespace Srvtools
{
    partial class frmSelUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelUsers));
            this.lstLibrary = new System.Windows.Forms.ListBox();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnSel = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAC = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.infodsMenuUsers = new Srvtools.InfoDataSet(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.infodsMenuUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // lstLibrary
            // 
            this.lstLibrary.FormattingEnabled = true;
            this.lstLibrary.ItemHeight = 12;
            this.lstLibrary.Location = new System.Drawing.Point(12, 37);
            this.lstLibrary.Name = "lstLibrary";
            this.lstLibrary.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstLibrary.Size = new System.Drawing.Size(160, 184);
            this.lstLibrary.TabIndex = 0;
            // 
            // lstSelected
            // 
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.ItemHeight = 12;
            this.lstSelected.Location = new System.Drawing.Point(237, 37);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(160, 184);
            this.lstSelected.TabIndex = 1;
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(186, 48);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(35, 23);
            this.btnSelAll.TabIndex = 2;
            this.btnSelAll.Text = ">>";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSel
            // 
            this.btnSel.Location = new System.Drawing.Point(186, 77);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(35, 23);
            this.btnSel.TabIndex = 3;
            this.btnSel.Text = ">";
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(186, 160);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(35, 23);
            this.btnRemoveAll.TabIndex = 4;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(186, 189);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(35, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(58, 244);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAC
            // 
            this.btnAC.Location = new System.Drawing.Point(247, 244);
            this.btnAC.Name = "btnAC";
            this.btnAC.Size = new System.Drawing.Size(98, 23);
            this.btnAC.TabIndex = 5;
            this.btnAC.Text = "AccessControl";
            this.btnAC.UseVisualStyleBackColor = true;
            this.btnAC.Click += new System.EventHandler(this.btnAC_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(154, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "User List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Selected Users";
            // 
            // infodsMenuUsers
            // 
            this.infodsMenuUsers.Active = false;
            this.infodsMenuUsers.AlwaysClose = false;
            this.infodsMenuUsers.DeleteIncomplete = true;
            this.infodsMenuUsers.LastKeyValues = null;
            this.infodsMenuUsers.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.infodsMenuUsers.PacketRecords = -1;
            this.infodsMenuUsers.Position = -1;
            this.infodsMenuUsers.RemoteName = "";
            this.infodsMenuUsers.ServerModify = false;
            // 
            // frmSelUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 284);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAC);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnSel);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.lstLibrary);
            this.Name = "frmSelUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Access Define";
            this.Load += new System.EventHandler(this.frmSelUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infodsMenuUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstLibrary;
        private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnSel;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAC;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private InfoDataSet infodsMenuUsers;
    }
}