namespace AjaxTools
{
    partial class SmartMenuCollectionEditorDialog
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("<--ROOT-->");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmartMenuCollectionEditorDialog));
            this.panTree = new System.Windows.Forms.Panel();
            this.tView = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsAdd = new System.Windows.Forms.ToolStripButton();
            this.tsDelete = new System.Windows.Forms.ToolStripButton();
            this.tsMoveUp = new System.Windows.Forms.ToolStripButton();
            this.tsMoveDown = new System.Windows.Forms.ToolStripButton();
            this.panPro = new System.Windows.Forms.Panel();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panTree.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panPro.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTree
            // 
            this.panTree.Controls.Add(this.tView);
            this.panTree.Controls.Add(this.toolStrip1);
            this.panTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.panTree.Location = new System.Drawing.Point(0, 0);
            this.panTree.Name = "panTree";
            this.panTree.Size = new System.Drawing.Size(285, 461);
            this.panTree.TabIndex = 2;
            // 
            // tView
            // 
            this.tView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tView.Location = new System.Drawing.Point(0, 25);
            this.tView.Name = "tView";
            treeNode1.Name = "<--ROOT-->";
            treeNode1.Text = "<--ROOT-->";
            this.tView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tView.Size = new System.Drawing.Size(285, 436);
            this.tView.TabIndex = 5;
            this.tView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tView_AfterSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAdd,
            this.tsDelete,
            this.tsMoveUp,
            this.tsMoveDown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(285, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsAdd
            // 
            this.tsAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsAdd.Image")));
            this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsAdd.Name = "tsAdd";
            this.tsAdd.Size = new System.Drawing.Size(45, 22);
            this.tsAdd.Text = "add";
            this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
            // 
            // tsDelete
            // 
            this.tsDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsDelete.Image")));
            this.tsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDelete.Name = "tsDelete";
            this.tsDelete.Size = new System.Drawing.Size(57, 22);
            this.tsDelete.Text = "delete";
            this.tsDelete.Click += new System.EventHandler(this.tsDelete_Click);
            // 
            // tsMoveUp
            // 
            this.tsMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("tsMoveUp.Image")));
            this.tsMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsMoveUp.Name = "tsMoveUp";
            this.tsMoveUp.Size = new System.Drawing.Size(68, 22);
            this.tsMoveUp.Text = "move up";
            this.tsMoveUp.Click += new System.EventHandler(this.tsMoveUp_Click);
            // 
            // tsMoveDown
            // 
            this.tsMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("tsMoveDown.Image")));
            this.tsMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsMoveDown.Name = "tsMoveDown";
            this.tsMoveDown.Size = new System.Drawing.Size(82, 22);
            this.tsMoveDown.Text = "move down";
            this.tsMoveDown.Click += new System.EventHandler(this.tsMoveDown_Click);
            // 
            // panPro
            // 
            this.panPro.Controls.Add(this.propGrid);
            this.panPro.Controls.Add(this.btnCancel);
            this.panPro.Controls.Add(this.btnOK);
            this.panPro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panPro.Location = new System.Drawing.Point(285, 0);
            this.panPro.Name = "panPro";
            this.panPro.Size = new System.Drawing.Size(276, 461);
            this.panPro.TabIndex = 4;
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(276, 420);
            this.propGrid.TabIndex = 7;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propGrid_PropertyValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(189, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(106, 426);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // SmartMenuCollectionEditorDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(561, 461);
            this.Controls.Add(this.panPro);
            this.Controls.Add(this.panTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SmartMenuCollectionEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "smart menu items editor";
            this.Load += new System.EventHandler(this.SmartMenuCollectionEditorDialog_Load);
            this.panTree.ResumeLayout(false);
            this.panTree.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panPro.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTree;
        private System.Windows.Forms.Panel panPro;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.TreeView tView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsAdd;
        private System.Windows.Forms.ToolStripButton tsDelete;
        private System.Windows.Forms.ToolStripButton tsMoveUp;
        private System.Windows.Forms.ToolStripButton tsMoveDown;

    }
}