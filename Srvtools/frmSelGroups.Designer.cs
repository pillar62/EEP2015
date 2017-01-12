namespace Srvtools
{
    partial class frmSelGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelGroups));
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnSel = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstLibrary = new System.Windows.Forms.ListBox();
            this.infodsMenuGroups = new Srvtools.InfoDataSet(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.infodsMenuGroups)).BeginInit();
            this.SuspendLayout();
            // 
            // lstSelected
            // 
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.ItemHeight = 12;
            this.lstSelected.Location = new System.Drawing.Point(233, 37);
            this.lstSelected.MultiColumn = true;
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(160, 184);
            this.lstSelected.TabIndex = 1;
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(182, 50);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(35, 23);
            this.btnSelAll.TabIndex = 2;
            this.btnSelAll.Text = ">>";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSel
            // 
            this.btnSel.Location = new System.Drawing.Point(182, 79);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(35, 23);
            this.btnSel.TabIndex = 3;
            this.btnSel.Text = ">";
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(182, 162);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(35, 23);
            this.btnRemoveAll.TabIndex = 4;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(182, 191);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(35, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "<";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(55, 244);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(151, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstLibrary
            // 
            this.lstLibrary.FormattingEnabled = true;
            this.lstLibrary.ItemHeight = 12;
            this.lstLibrary.Location = new System.Drawing.Point(10, 37);
            this.lstLibrary.MultiColumn = true;
            this.lstLibrary.Name = "lstLibrary";
            this.lstLibrary.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstLibrary.Size = new System.Drawing.Size(160, 184);
            this.lstLibrary.TabIndex = 0;
            // 
            // infodsMenuGroups
            // 
            this.infodsMenuGroups.Active = false;
            this.infodsMenuGroups.AlwaysClose = false;
            this.infodsMenuGroups.DeleteIncomplete = true;
            this.infodsMenuGroups.LastKeyValues = null;
            this.infodsMenuGroups.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.infodsMenuGroups.PacketRecords = -1;
            this.infodsMenuGroups.Position = -1;
            this.infodsMenuGroups.RemoteName = "";
            this.infodsMenuGroups.ServerModify = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(244, 244);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Access Control";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Groups List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "Selected Groups";
            // 
            // frmSelGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 284);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnSel);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.lstLibrary);
            this.Name = "frmSelGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Group Access Define";
            this.Load += new System.EventHandler(this.frmSelGroups_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infodsMenuGroups)).EndInit();
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
        private System.Windows.Forms.Button btnCancel;
        private Srvtools.InfoDataSet infodsMenuGroups;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}