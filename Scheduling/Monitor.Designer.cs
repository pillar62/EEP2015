namespace Scheduling
{
    partial class Monitor
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
            this.lvMonitor = new System.Windows.Forms.ListView();
            this.SchedulName = new System.Windows.Forms.ColumnHeader();
            this.StartTime = new System.Windows.Forms.ColumnHeader();
            this.EndTime = new System.Windows.Forms.ColumnHeader();
            this.Status = new System.Windows.Forms.ColumnHeader();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvMonitor
            // 
            this.lvMonitor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SchedulName,
            this.StartTime,
            this.EndTime,
            this.Status});
            this.lvMonitor.ContextMenuStrip = this.contextMenuStrip1;
            this.lvMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lvMonitor.FullRowSelect = true;
            this.lvMonitor.GridLines = true;
            this.lvMonitor.Location = new System.Drawing.Point(0, 0);
            this.lvMonitor.Name = "lvMonitor";
            this.lvMonitor.Size = new System.Drawing.Size(423, 239);
            this.lvMonitor.TabIndex = 0;
            this.lvMonitor.UseCompatibleStateImageBehavior = false;
            this.lvMonitor.View = System.Windows.Forms.View.Details;
            // 
            // SchedulName
            // 
            this.SchedulName.Text = "Schedul Name";
            this.SchedulName.Width = 95;
            // 
            // StartTime
            // 
            this.StartTime.Text = "Start Time";
            this.StartTime.Width = 120;
            // 
            // EndTime
            // 
            this.EndTime.Text = "End Time";
            this.EndTime.Width = 120;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 70;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 239);
            this.Controls.Add(this.lvMonitor);
            this.MaximizeBox = false;
            this.Name = "Monitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monitor";
            this.Load += new System.EventHandler(this.Monitor_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvMonitor;
        private System.Windows.Forms.ColumnHeader SchedulName;
        private System.Windows.Forms.ColumnHeader StartTime;
        private System.Windows.Forms.ColumnHeader EndTime;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
    }
}