namespace EEPNetClient
{
    partial class frmFavorMenu
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFavorMenu));
            this.lbAll = new System.Windows.Forms.ListBox();
            this.lbFavor = new System.Windows.Forms.ListBox();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.imglst = new System.Windows.Forms.ImageList(this.components);
            this.tView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // lbAll
            // 
            this.lbAll.FormattingEnabled = true;
            this.lbAll.ItemHeight = 12;
            this.lbAll.Location = new System.Drawing.Point(12, 12);
            this.lbAll.Name = "lbAll";
            this.lbAll.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAll.Size = new System.Drawing.Size(159, 268);
            this.lbAll.TabIndex = 0;
            // 
            // lbFavor
            // 
            this.lbFavor.FormattingEnabled = true;
            this.lbFavor.ItemHeight = 12;
            this.lbFavor.Location = new System.Drawing.Point(216, 36);
            this.lbFavor.Name = "lbFavor";
            this.lbFavor.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFavor.Size = new System.Drawing.Size(160, 244);
            this.lbFavor.TabIndex = 1;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(179, 238);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(31, 30);
            this.btnRemoveAll.TabIndex = 5;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(179, 188);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(31, 30);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(179, 79);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 30);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(179, 25);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(31, 30);
            this.btnAddAll.TabIndex = 6;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(222, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(72, 296);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(91, 30);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbGroup
            // 
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(216, 12);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(160, 20);
            this.cbGroup.TabIndex = 10;
            this.cbGroup.SelectedIndexChanged += new System.EventHandler(this.cbGroup_SelectedIndexChanged);
            // 
            // tView
            // 
            this.tView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tView.Location = new System.Drawing.Point(12, 12);
            this.tView.Name = "tView";
            this.tView.Size = new System.Drawing.Size(159, 268);
            this.tView.TabIndex = 11;
            // 
            // frmFavorMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(390, 342);
            this.Controls.Add(this.tView);
            this.Controls.Add(this.cbGroup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lbFavor);
            this.Controls.Add(this.lbAll);
            this.MaximizeBox = false;
            this.Name = "frmFavorMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmFavorMenu";
            this.Load += new System.EventHandler(this.frmFavorMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbAll;
        private System.Windows.Forms.ListBox lbFavor;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbGroup;
        private System.Windows.Forms.ImageList imglst;
        private System.Windows.Forms.TreeView tView;
    }
}