namespace JQToCordovaWizard
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxVirtualPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listViewCordovaPage = new System.Windows.Forms.ListView();
            this.buttonUnSelectAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.checkedListBoxJqueryPage = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFolder = new System.Windows.Forms.ComboBox();
            this.comboBoxCordova = new System.Windows.Forms.ComboBox();
            this.comboBoxWebsite = new System.Windows.Forms.ComboBox();
            this.labelInfomation = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonStart = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(519, 50);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 25);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxVirtualPath);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.listViewCordovaPage);
            this.splitContainer1.Panel1.Controls.Add(this.buttonUnSelectAll);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSelectAll);
            this.splitContainer1.Panel1.Controls.Add(this.checkedListBoxJqueryPage);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxFolder);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxCordova);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxWebsite);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelInfomation);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCancel);
            this.splitContainer1.Panel2.Controls.Add(this.buttonStart);
            this.splitContainer1.Size = new System.Drawing.Size(616, 478);
            this.splitContainer1.SplitterDistance = 378;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 8;
            // 
            // textBoxVirtualPath
            // 
            this.textBoxVirtualPath.Location = new System.Drawing.Point(202, 17);
            this.textBoxVirtualPath.Name = "textBoxVirtualPath";
            this.textBoxVirtualPath.Size = new System.Drawing.Size(392, 21);
            this.textBoxVirtualPath.TabIndex = 26;
            this.textBoxVirtualPath.Text = "localhost/JQWebClient";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "IIS Virtual Directory: http://";
            // 
            // listViewCordovaPage
            // 
            this.listViewCordovaPage.Location = new System.Drawing.Point(325, 126);
            this.listViewCordovaPage.Name = "listViewCordovaPage";
            this.listViewCordovaPage.Size = new System.Drawing.Size(269, 212);
            this.listViewCordovaPage.TabIndex = 24;
            this.listViewCordovaPage.UseCompatibleStateImageBehavior = false;
            this.listViewCordovaPage.View = System.Windows.Forms.View.List;
            // 
            // buttonUnSelectAll
            // 
            this.buttonUnSelectAll.Location = new System.Drawing.Point(159, 344);
            this.buttonUnSelectAll.Name = "buttonUnSelectAll";
            this.buttonUnSelectAll.Size = new System.Drawing.Size(100, 23);
            this.buttonUnSelectAll.TabIndex = 23;
            this.buttonUnSelectAll.Text = "Unselect All";
            this.buttonUnSelectAll.UseVisualStyleBackColor = true;
            this.buttonUnSelectAll.Click += new System.EventHandler(this.buttonUnSelectAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(39, 344);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(100, 23);
            this.buttonSelectAll.TabIndex = 22;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // checkedListBoxJqueryPage
            // 
            this.checkedListBoxJqueryPage.FormattingEnabled = true;
            this.checkedListBoxJqueryPage.Location = new System.Drawing.Point(21, 126);
            this.checkedListBoxJqueryPage.MultiColumn = true;
            this.checkedListBoxJqueryPage.Name = "checkedListBoxJqueryPage";
            this.checkedListBoxJqueryPage.Size = new System.Drawing.Size(261, 212);
            this.checkedListBoxJqueryPage.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(331, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "Cordova Project:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "Page Folder:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "JQuery Website:";
            // 
            // comboBoxFolder
            // 
            this.comboBoxFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFolder.FormattingEnabled = true;
            this.comboBoxFolder.Location = new System.Drawing.Point(120, 89);
            this.comboBoxFolder.Name = "comboBoxFolder";
            this.comboBoxFolder.Size = new System.Drawing.Size(162, 20);
            this.comboBoxFolder.TabIndex = 16;
            this.comboBoxFolder.SelectedIndexChanged += new System.EventHandler(this.comboBoxFolder_SelectedIndexChanged);
            // 
            // comboBoxCordova
            // 
            this.comboBoxCordova.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCordova.FormattingEnabled = true;
            this.comboBoxCordova.Location = new System.Drawing.Point(438, 89);
            this.comboBoxCordova.Name = "comboBoxCordova";
            this.comboBoxCordova.Size = new System.Drawing.Size(156, 20);
            this.comboBoxCordova.TabIndex = 14;
            this.comboBoxCordova.SelectedIndexChanged += new System.EventHandler(this.comboBoxCordova_SelectedIndexChanged);
            // 
            // comboBoxWebsite
            // 
            this.comboBoxWebsite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWebsite.FormattingEnabled = true;
            this.comboBoxWebsite.Location = new System.Drawing.Point(120, 52);
            this.comboBoxWebsite.Name = "comboBoxWebsite";
            this.comboBoxWebsite.Size = new System.Drawing.Size(162, 20);
            this.comboBoxWebsite.TabIndex = 13;
            this.comboBoxWebsite.SelectedIndexChanged += new System.EventHandler(this.comboBoxWebsite_SelectedIndexChanged);
            // 
            // labelInfomation
            // 
            this.labelInfomation.AutoSize = true;
            this.labelInfomation.Location = new System.Drawing.Point(19, 28);
            this.labelInfomation.Name = "labelInfomation";
            this.labelInfomation.Size = new System.Drawing.Size(185, 12);
            this.labelInfomation.TabIndex = 9;
            this.labelInfomation.Text = "Click Start button to convert ";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(21, 61);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(472, 14);
            this.progressBar.TabIndex = 8;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(519, 52);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 15;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 478);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export to Cordova";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonUnSelectAll;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.CheckedListBox checkedListBoxJqueryPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFolder;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox comboBoxCordova;
        private System.Windows.Forms.ComboBox comboBoxWebsite;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelInfomation;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ListView listViewCordovaPage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxVirtualPath;
    }
}