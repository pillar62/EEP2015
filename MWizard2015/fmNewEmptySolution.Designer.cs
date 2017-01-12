namespace MWizard2015
{
    partial class fmNewEmptySolution
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbSolutionPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbSolutionName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.cbCreateDirectory = new System.Windows.Forms.CheckBox();
            this.cbWorkflow = new System.Windows.Forms.CheckBox();
            this.gbContainProjects = new System.Windows.Forms.GroupBox();
            this.cbCordova = new System.Windows.Forms.CheckBox();
            this.cbJQuery = new System.Windows.Forms.CheckBox();
            this.cbEntityFramework = new System.Windows.Forms.CheckBox();
            this.gbContainProjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Solution Path";
            // 
            // tbSolutionPath
            // 
            this.tbSolutionPath.Location = new System.Drawing.Point(96, 34);
            this.tbSolutionPath.Name = "tbSolutionPath";
            this.tbSolutionPath.Size = new System.Drawing.Size(372, 22);
            this.tbSolutionPath.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbSolutionName
            // 
            this.tbSolutionName.Location = new System.Drawing.Point(96, 76);
            this.tbSolutionName.Name = "tbSolutionName";
            this.tbSolutionName.Size = new System.Drawing.Size(372, 22);
            this.tbSolutionName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Solution Name";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(169, 241);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(300, 241);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbCreateDirectory
            // 
            this.cbCreateDirectory.AutoSize = true;
            this.cbCreateDirectory.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCreateDirectory.Location = new System.Drawing.Point(29, 10);
            this.cbCreateDirectory.Name = "cbCreateDirectory";
            this.cbCreateDirectory.Size = new System.Drawing.Size(161, 18);
            this.cbCreateDirectory.TabIndex = 7;
            this.cbCreateDirectory.Text = "Create directory for solution";
            this.cbCreateDirectory.UseVisualStyleBackColor = true;
            this.cbCreateDirectory.Visible = false;
            // 
            // cbWorkflow
            // 
            this.cbWorkflow.AutoSize = true;
            this.cbWorkflow.Location = new System.Drawing.Point(22, 26);
            this.cbWorkflow.Name = "cbWorkflow";
            this.cbWorkflow.Size = new System.Drawing.Size(72, 16);
            this.cbWorkflow.TabIndex = 8;
            this.cbWorkflow.Text = "Workflow";
            this.cbWorkflow.UseVisualStyleBackColor = true;
            // 
            // gbContainProjects
            // 
            this.gbContainProjects.Controls.Add(this.cbCordova);
            this.gbContainProjects.Controls.Add(this.cbJQuery);
            this.gbContainProjects.Controls.Add(this.cbWorkflow);
            this.gbContainProjects.Controls.Add(this.cbEntityFramework);
            this.gbContainProjects.Location = new System.Drawing.Point(96, 112);
            this.gbContainProjects.Name = "gbContainProjects";
            this.gbContainProjects.Size = new System.Drawing.Size(372, 110);
            this.gbContainProjects.TabIndex = 10;
            this.gbContainProjects.TabStop = false;
            this.gbContainProjects.Text = "Contain Projects";
            // 
            // cbCordova
            // 
            this.cbCordova.AutoSize = true;
            this.cbCordova.Location = new System.Drawing.Point(22, 82);
            this.cbCordova.Name = "cbCordova";
            this.cbCordova.Size = new System.Drawing.Size(65, 16);
            this.cbCordova.TabIndex = 11;
            this.cbCordova.Text = "Cordova";
            this.cbCordova.UseVisualStyleBackColor = true;
            // 
            // cbJQuery
            // 
            this.cbJQuery.AutoSize = true;
            this.cbJQuery.Location = new System.Drawing.Point(22, 54);
            this.cbJQuery.Name = "cbJQuery";
            this.cbJQuery.Size = new System.Drawing.Size(57, 16);
            this.cbJQuery.TabIndex = 10;
            this.cbJQuery.Text = "JQuery";
            this.cbJQuery.UseVisualStyleBackColor = true;
            // 
            // cbEntityFramework
            // 
            this.cbEntityFramework.AutoSize = true;
            this.cbEntityFramework.Location = new System.Drawing.Point(22, 110);
            this.cbEntityFramework.Name = "cbEntityFramework";
            this.cbEntityFramework.Size = new System.Drawing.Size(108, 16);
            this.cbEntityFramework.TabIndex = 9;
            this.cbEntityFramework.Text = "Entity Framework";
            this.cbEntityFramework.UseVisualStyleBackColor = true;
            this.cbEntityFramework.Visible = false;
            // 
            // fmNewEmptySolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 276);
            this.Controls.Add(this.gbContainProjects);
            this.Controls.Add(this.cbCreateDirectory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbSolutionName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbSolutionPath);
            this.Controls.Add(this.label1);
            this.Name = "fmNewEmptySolution";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP Solution Wizard";
            this.Load += new System.EventHandler(this.fmNewEmptySolution_Load);
            this.gbContainProjects.ResumeLayout(false);
            this.gbContainProjects.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSolutionPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbSolutionName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox cbCreateDirectory;
        private System.Windows.Forms.CheckBox cbWorkflow;
        private System.Windows.Forms.GroupBox gbContainProjects;
        private System.Windows.Forms.CheckBox cbJQuery;
        private System.Windows.Forms.CheckBox cbEntityFramework;
        private System.Windows.Forms.CheckBox cbCordova;
    }
}