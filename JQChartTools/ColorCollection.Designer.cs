namespace JQChartTools
{
    partial class ColorCollection
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btcolorSelect = new System.Windows.Forms.Button();
            this.btadd = new System.Windows.Forms.Button();
            this.btdelete = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Location = new System.Drawing.Point(13, 9);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(146, 114);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "CurrentColorList";
            this.columnHeader1.Width = 115;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(165, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 1;
            // 
            // btcolorSelect
            // 
            this.btcolorSelect.Location = new System.Drawing.Point(271, 27);
            this.btcolorSelect.Name = "btcolorSelect";
            this.btcolorSelect.Size = new System.Drawing.Size(29, 23);
            this.btcolorSelect.TabIndex = 2;
            this.btcolorSelect.Text = "..";
            this.btcolorSelect.UseVisualStyleBackColor = true;
            this.btcolorSelect.Click += new System.EventHandler(this.btcolorSelect_Click);
            // 
            // btadd
            // 
            this.btadd.Location = new System.Drawing.Point(202, 69);
            this.btadd.Name = "btadd";
            this.btadd.Size = new System.Drawing.Size(98, 23);
            this.btadd.TabIndex = 3;
            this.btadd.Text = "AddToList";
            this.btadd.UseVisualStyleBackColor = true;
            this.btadd.Click += new System.EventHandler(this.btadd_Click);
            // 
            // btdelete
            // 
            this.btdelete.Location = new System.Drawing.Point(82, 129);
            this.btdelete.Name = "btdelete";
            this.btdelete.Size = new System.Drawing.Size(54, 23);
            this.btdelete.TabIndex = 4;
            this.btdelete.Text = "Delete";
            this.btdelete.UseVisualStyleBackColor = true;
            this.btdelete.Click += new System.EventHandler(this.btdelete_Click);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(153, 185);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 5;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(237, 185);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btcolorSelect);
            this.panel1.Controls.Add(this.btdelete);
            this.panel1.Controls.Add(this.btadd);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 167);
            this.panel1.TabIndex = 7;
            // 
            // ColorCollection
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(335, 219);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Name = "ColorCollection";
            this.Text = "ColorCollection";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btcolorSelect;
        private System.Windows.Forms.Button btadd;
        private System.Windows.Forms.Button btdelete;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}