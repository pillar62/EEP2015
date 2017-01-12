namespace Infolight.EasilyReportTools.UI
{
    partial class fmPictures
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
            this.lbPictures = new System.Windows.Forms.Label();
            this.lbxPicutres = new System.Windows.Forms.ListBox();
            this.btAdd = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btSelect = new System.Windows.Forms.Button();
            this.lbPictureName = new System.Windows.Forms.Label();
            this.tbPictureName = new System.Windows.Forms.TextBox();
            this.btChange = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPictures
            // 
            this.lbPictures.AutoSize = true;
            this.lbPictures.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPictures.Location = new System.Drawing.Point(18, 14);
            this.lbPictures.Name = "lbPictures";
            this.lbPictures.Size = new System.Drawing.Size(79, 14);
            this.lbPictures.TabIndex = 0;
            this.lbPictures.Text = "Pictures:";
            // 
            // lbxPicutres
            // 
            this.lbxPicutres.FormattingEnabled = true;
            this.lbxPicutres.ItemHeight = 12;
            this.lbxPicutres.Location = new System.Drawing.Point(21, 32);
            this.lbxPicutres.Name = "lbxPicutres";
            this.lbxPicutres.Size = new System.Drawing.Size(156, 208);
            this.lbxPicutres.TabIndex = 1;
            this.lbxPicutres.SelectedIndexChanged += new System.EventHandler(this.lbxPicutres_SelectedIndexChanged);
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(21, 246);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(75, 23);
            this.btAdd.TabIndex = 2;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btRemove
            // 
            this.btRemove.Location = new System.Drawing.Point(102, 246);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(75, 23);
            this.btRemove.TabIndex = 3;
            this.btRemove.Text = "Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(192, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(290, 287);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(102, 275);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(75, 23);
            this.btSelect.TabIndex = 5;
            this.btSelect.Text = "Select";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // lbPictureName
            // 
            this.lbPictureName.AutoSize = true;
            this.lbPictureName.Location = new System.Drawing.Point(34, 68);
            this.lbPictureName.Name = "lbPictureName";
            this.lbPictureName.Size = new System.Drawing.Size(83, 12);
            this.lbPictureName.TabIndex = 6;
            this.lbPictureName.Text = "Picture Name:";
            this.lbPictureName.Visible = false;
            // 
            // tbPictureName
            // 
            this.tbPictureName.Location = new System.Drawing.Point(34, 83);
            this.tbPictureName.Name = "tbPictureName";
            this.tbPictureName.Size = new System.Drawing.Size(123, 21);
            this.tbPictureName.TabIndex = 7;
            this.tbPictureName.Visible = false;
            this.tbPictureName.TextChanged += new System.EventHandler(this.tbPictureName_TextChanged);
            // 
            // btChange
            // 
            this.btChange.Location = new System.Drawing.Point(21, 275);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(75, 23);
            this.btChange.TabIndex = 8;
            this.btChange.Text = "Change";
            this.btChange.UseVisualStyleBackColor = true;
            this.btChange.Click += new System.EventHandler(this.btChange_Click);
            // 
            // fmPictures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 311);
            this.Controls.Add(this.btChange);
            this.Controls.Add(this.tbPictureName);
            this.Controls.Add(this.lbPictureName);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.lbxPicutres);
            this.Controls.Add(this.lbPictures);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmPictures";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Picture";
            this.Load += new System.EventHandler(this.fmPictures_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbPictures;
        private System.Windows.Forms.ListBox lbxPicutres;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.Label lbPictureName;
        private System.Windows.Forms.TextBox tbPictureName;
        private System.Windows.Forms.Button btChange;
    }
}