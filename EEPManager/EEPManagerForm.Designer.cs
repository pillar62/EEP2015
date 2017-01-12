namespace EEPManager
{
    partial class EEPManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EEPManagerForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemSystem = new System.Windows.Forms.MenuItem();
            this.menuItemSecurityManager = new System.Windows.Forms.MenuItem();
            this.menuItemSolutionDefine = new System.Windows.Forms.MenuItem();
            this.menuItemPackageManager = new System.Windows.Forms.MenuItem();
            this.menuItemDatabaseAlias = new System.Windows.Forms.MenuItem();
            this.menuItemRefVal = new System.Windows.Forms.MenuItem();
            this.menuItemDD = new System.Windows.Forms.MenuItem();
            this.menuItemEM = new System.Windows.Forms.MenuItem();
            this.menuItemSLVFD = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSystem,
            this.menuItemHelp});
            // 
            // menuItemSystem
            // 
            this.menuItemSystem.Index = 0;
            this.menuItemSystem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSecurityManager,
            this.menuItemSolutionDefine,
            this.menuItemPackageManager,
            this.menuItemDatabaseAlias,
            this.menuItemRefVal,
            this.menuItemDD,
            this.menuItemEM,
            this.menuItemSLVFD,
            this.menuItemExit});
            this.menuItemSystem.Text = "&System";
            // 
            // menuItemSecurityManager
            // 
            this.menuItemSecurityManager.Index = 0;
            this.menuItemSecurityManager.Text = "Security Manager";
            this.menuItemSecurityManager.Click += new System.EventHandler(this.menuItemSecurityManager_Click);
            // 
            // menuItemSolutionDefine
            // 
            this.menuItemSolutionDefine.Index = 1;
            this.menuItemSolutionDefine.Text = "Solution Define";
            this.menuItemSolutionDefine.Click += new System.EventHandler(this.menuItemSolutionDefine_Click);
            // 
            // menuItemPackageManager
            // 
            this.menuItemPackageManager.Index = 2;
            this.menuItemPackageManager.Text = "Package Manager";
            this.menuItemPackageManager.Click += new System.EventHandler(this.menuItemPackageManager_Click);
            // 
            // menuItemDatabaseAlias
            // 
            this.menuItemDatabaseAlias.Index = 3;
            this.menuItemDatabaseAlias.Text = "Database Alias Options";
            this.menuItemDatabaseAlias.Click += new System.EventHandler(this.menuItemDatabaseAlias_Click);
            // 
            // menuItemRefVal
            // 
            this.menuItemRefVal.Index = 4;
            this.menuItemRefVal.Text = "Reference Value";
            this.menuItemRefVal.Click += new System.EventHandler(this.menuItemRefVal_Click);
            // 
            // menuItemDD
            // 
            this.menuItemDD.Index = 5;
            this.menuItemDD.Text = "Data Dictionary";
            this.menuItemDD.Click += new System.EventHandler(this.menuItemDD_Click);
            // 
            // menuItemEM
            // 
            this.menuItemEM.Index = 6;
            this.menuItemEM.Text = "ErrorLog Maintenance";
            this.menuItemEM.Click += new System.EventHandler(this.menuItemEM_Click);
            // 
            // menuItemSLVFD
            // 
            this.menuItemSLVFD.Index = 7;
            this.menuItemSLVFD.Text = "System Log Viewer";
            this.menuItemSLVFD.Click += new System.EventHandler(this.menuItemSLVFD_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 8;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 1;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "About EEP Manager";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // EEPManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EEPManager.Properties.Resources.EEPManager;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(569, 382);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "EEPManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EEPManagerForm_FormClosing);
            this.Load += new System.EventHandler(this.EEPManagerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItemSystem;
        private System.Windows.Forms.MenuItem menuItemSecurityManager;
        private System.Windows.Forms.MenuItem menuItemSolutionDefine;
        private System.Windows.Forms.MenuItem menuItemPackageManager;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemHelp;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.MenuItem menuItemDD;
        private System.Windows.Forms.MenuItem menuItemEM;
        private System.Windows.Forms.MenuItem menuItemSLVFD;
        private System.Windows.Forms.MenuItem menuItemDatabaseAlias;
        private System.Windows.Forms.MenuItem menuItemRefVal;
    }
}

