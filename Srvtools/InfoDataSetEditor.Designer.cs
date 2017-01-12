namespace Srvtools
{
    partial class InfoDataSetEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoDataSetEditorDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.ImageCollection = new System.Windows.Forms.ImageList(this.components);
            this.tabDataSetColumnSelector = new System.Windows.Forms.TabControl();
            this.tabPageSelector = new System.Windows.Forms.TabPage();
            this.trvDataSetSchema = new Srvtools.TreeViewForInfoDataSetColumnSelector();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.lblVerticalGaps = new System.Windows.Forms.Label();
            this.txtVerticalGaps = new System.Windows.Forms.TextBox();
            this.txtHorizontalGaps = new System.Windows.Forms.TextBox();
            this.lblHorizontalGaps = new System.Windows.Forms.Label();
            this.txtRowsOrCols = new System.Windows.Forms.TextBox();
            this.cbRowsOrCols = new System.Windows.Forms.ComboBox();
            this.tabDataSetColumnSelector.SuspendLayout();
            this.tabPageSelector.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(149, 411);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ImageCollection
            // 
            this.ImageCollection.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageCollection.ImageStream")));
            this.ImageCollection.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageCollection.Images.SetKeyName(0, "");
            this.ImageCollection.Images.SetKeyName(1, "");
            this.ImageCollection.Images.SetKeyName(2, "");
            this.ImageCollection.Images.SetKeyName(3, "");
            this.ImageCollection.Images.SetKeyName(4, "");
            this.ImageCollection.Images.SetKeyName(5, "");
            this.ImageCollection.Images.SetKeyName(6, "");
            this.ImageCollection.Images.SetKeyName(7, "");
            this.ImageCollection.Images.SetKeyName(8, "");
            this.ImageCollection.Images.SetKeyName(9, "");
            this.ImageCollection.Images.SetKeyName(10, "");
            this.ImageCollection.Images.SetKeyName(11, "");
            this.ImageCollection.Images.SetKeyName(12, "");
            this.ImageCollection.Images.SetKeyName(13, "");
            this.ImageCollection.Images.SetKeyName(14, "");
            this.ImageCollection.Images.SetKeyName(15, "InfoRefValBox.ico");
            this.ImageCollection.Images.SetKeyName(16, "InfoTextBox.ico");
            this.ImageCollection.Images.SetKeyName(17, "InfoDataTimeBox.ico");
            // 
            // tabDataSetColumnSelector
            // 
            this.tabDataSetColumnSelector.Controls.Add(this.tabPageSelector);
            this.tabDataSetColumnSelector.Controls.Add(this.tabPageOptions);
            this.tabDataSetColumnSelector.Location = new System.Drawing.Point(0, 2);
            this.tabDataSetColumnSelector.Name = "tabDataSetColumnSelector";
            this.tabDataSetColumnSelector.SelectedIndex = 0;
            this.tabDataSetColumnSelector.Size = new System.Drawing.Size(382, 403);
            this.tabDataSetColumnSelector.TabIndex = 2;
            this.tabDataSetColumnSelector.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabDataSetColumnSelector_Selecting);
            // 
            // tabPageSelector
            // 
            this.tabPageSelector.Controls.Add(this.trvDataSetSchema);
            this.tabPageSelector.Location = new System.Drawing.Point(4, 21);
            this.tabPageSelector.Name = "tabPageSelector";
            this.tabPageSelector.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSelector.Size = new System.Drawing.Size(374, 378);
            this.tabPageSelector.TabIndex = 0;
            this.tabPageSelector.Text = "Selector";
            this.tabPageSelector.UseVisualStyleBackColor = true;
            // 
            // trvDataSetSchema
            // 
            this.trvDataSetSchema.CheckBoxes = true;
            this.trvDataSetSchema.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvDataSetSchema.HideSelection = false;
            this.trvDataSetSchema.ImageIndex = 0;
            this.trvDataSetSchema.ImageList = this.ImageCollection;
            this.trvDataSetSchema.Location = new System.Drawing.Point(3, 3);
            this.trvDataSetSchema.Name = "trvDataSetSchema";
            this.trvDataSetSchema.SelectedImageIndex = 0;
            this.trvDataSetSchema.Size = new System.Drawing.Size(368, 372);
            this.trvDataSetSchema.TabIndex = 1;
            this.trvDataSetSchema.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvDataSetSchema_AfterCheck);
            this.trvDataSetSchema.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvDataSetSchema_MouseDown);
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.lblVerticalGaps);
            this.tabPageOptions.Controls.Add(this.txtVerticalGaps);
            this.tabPageOptions.Controls.Add(this.txtHorizontalGaps);
            this.tabPageOptions.Controls.Add(this.lblHorizontalGaps);
            this.tabPageOptions.Controls.Add(this.txtRowsOrCols);
            this.tabPageOptions.Controls.Add(this.cbRowsOrCols);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 21);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(374, 378);
            this.tabPageOptions.TabIndex = 1;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // lblVerticalGaps
            // 
            this.lblVerticalGaps.AutoSize = true;
            this.lblVerticalGaps.Location = new System.Drawing.Point(19, 108);
            this.lblVerticalGaps.Name = "lblVerticalGaps";
            this.lblVerticalGaps.Size = new System.Drawing.Size(137, 12);
            this.lblVerticalGaps.TabIndex = 5;
            this.lblVerticalGaps.Text = "Vertical Gaps(pixels):";
            // 
            // txtVerticalGaps
            // 
            this.txtVerticalGaps.Location = new System.Drawing.Point(161, 105);
            this.txtVerticalGaps.MaxLength = 4;
            this.txtVerticalGaps.Name = "txtVerticalGaps";
            this.txtVerticalGaps.Size = new System.Drawing.Size(184, 21);
            this.txtVerticalGaps.TabIndex = 4;
            // 
            // txtHorizontalGaps
            // 
            this.txtHorizontalGaps.Location = new System.Drawing.Point(161, 64);
            this.txtHorizontalGaps.MaxLength = 4;
            this.txtHorizontalGaps.Name = "txtHorizontalGaps";
            this.txtHorizontalGaps.Size = new System.Drawing.Size(184, 21);
            this.txtHorizontalGaps.TabIndex = 3;
            // 
            // lblHorizontalGaps
            // 
            this.lblHorizontalGaps.AutoSize = true;
            this.lblHorizontalGaps.Location = new System.Drawing.Point(13, 67);
            this.lblHorizontalGaps.Name = "lblHorizontalGaps";
            this.lblHorizontalGaps.Size = new System.Drawing.Size(143, 12);
            this.lblHorizontalGaps.TabIndex = 2;
            this.lblHorizontalGaps.Text = "Horzontal Gaps(pixels):";
            // 
            // txtRowsOrCols
            // 
            this.txtRowsOrCols.Location = new System.Drawing.Point(161, 26);
            this.txtRowsOrCols.MaxLength = 2;
            this.txtRowsOrCols.Name = "txtRowsOrCols";
            this.txtRowsOrCols.Size = new System.Drawing.Size(184, 21);
            this.txtRowsOrCols.TabIndex = 1;
            // 
            // cbRowsOrCols
            // 
            this.cbRowsOrCols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRowsOrCols.FormattingEnabled = true;
            this.cbRowsOrCols.Items.AddRange(new object[] {
            "Vertical Rows",
            "Horizontal Columns"});
            this.cbRowsOrCols.Location = new System.Drawing.Point(15, 28);
            this.cbRowsOrCols.Name = "cbRowsOrCols";
            this.cbRowsOrCols.Size = new System.Drawing.Size(140, 20);
            this.cbRowsOrCols.TabIndex = 0;
            this.cbRowsOrCols.SelectedIndexChanged += new System.EventHandler(this.cbRowsOrCols_SelectedIndexChanged);
            // 
            // InfoDataSetEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(381, 439);
            this.Controls.Add(this.tabDataSetColumnSelector);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoDataSetEditorDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataSet Column Selector";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Crimson;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InfoDataSetEditorDialog_FormClosing);
            this.Load += new System.EventHandler(this.InfoDataSetEditorDialog_Load);
            this.tabDataSetColumnSelector.ResumeLayout(false);
            this.tabPageSelector.ResumeLayout(false);
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private TreeViewForInfoDataSetColumnSelector trvDataSetSchema;
        private System.Windows.Forms.TabControl tabDataSetColumnSelector;
        private System.Windows.Forms.TabPage tabPageSelector;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.TextBox txtRowsOrCols;
        private System.Windows.Forms.ComboBox cbRowsOrCols;
        private System.Windows.Forms.Label lblVerticalGaps;
        private System.Windows.Forms.TextBox txtVerticalGaps;
        private System.Windows.Forms.TextBox txtHorizontalGaps;
        private System.Windows.Forms.Label lblHorizontalGaps;
        private System.Windows.Forms.ImageList ImageCollection;
    }
}