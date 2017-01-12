namespace EEPNetServer
{
    partial class frmLoginComputers
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
            this.listView = new System.Windows.Forms.ListView();
            this.Computer = new System.Windows.Forms.ColumnHeader();
            this.LoginTime = new System.Windows.Forms.ColumnHeader();
            this.LastActiveTime = new System.Windows.Forms.ColumnHeader();
            this.LoginCount = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Computer,
            this.LoginTime,
            this.LastActiveTime,
            this.LoginCount});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(467, 218);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // Computer
            // 
            this.Computer.Text = "Computer";
            this.Computer.Width = 100;
            // 
            // LoginTime
            // 
            this.LoginTime.Text = "Login Time";
            this.LoginTime.Width = 150;
            // 
            // LastActiveTime
            // 
            this.LastActiveTime.Text = "Last Active Time";
            this.LastActiveTime.Width = 150;
            // 
            // LoginCount
            // 
            this.LoginCount.Text = "Count";
            // 
            // frmLoginComputers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 218);
            this.Controls.Add(this.listView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoginComputers";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LoginComputers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader Computer;
        private System.Windows.Forms.ColumnHeader LoginTime;
        private System.Windows.Forms.ColumnHeader LastActiveTime;
        private System.Windows.Forms.ColumnHeader LoginCount;
    }
}