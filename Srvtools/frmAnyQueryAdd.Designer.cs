namespace Srvtools
{
    partial class frmAnyQueryAdd
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
            this.Label1 = new System.Windows.Forms.Label();
            this.cbColumnName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbColumnType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCaption = new System.Windows.Forms.TextBox();
            this.tbDefaultValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbOperator = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.cbRefVal = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCondition = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(33, 85);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(47, 12);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Column:";
            // 
            // cbColumnName
            // 
            this.cbColumnName.FormattingEnabled = true;
            this.cbColumnName.Location = new System.Drawing.Point(133, 83);
            this.cbColumnName.Name = "cbColumnName";
            this.cbColumnName.Size = new System.Drawing.Size(160, 20);
            this.cbColumnName.TabIndex = 2;
            this.cbColumnName.SelectedIndexChanged += new System.EventHandler(this.cbColumnName_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "Caption:";
            // 
            // cbColumnType
            // 
            this.cbColumnType.FormattingEnabled = true;
            this.cbColumnType.Items.AddRange(new object[] {
            "AnyQueryTextBoxColumn",
            "AnyQueryComboBoxColumn",
            "AnyQueryCheckBoxColumn",
            "AnyQueryRefValColumn",
            "AnyQueryCalendarColumn"});
            this.cbColumnType.Location = new System.Drawing.Point(133, 115);
            this.cbColumnType.Name = "cbColumnType";
            this.cbColumnType.Size = new System.Drawing.Size(160, 20);
            this.cbColumnType.TabIndex = 3;
            this.cbColumnType.Text = "AnyQueryTextBoxColumn";
            this.cbColumnType.SelectedIndexChanged += new System.EventHandler(this.cbColumnType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "Column Type:";
            // 
            // tbCaption
            // 
            this.tbCaption.Location = new System.Drawing.Point(133, 18);
            this.tbCaption.Name = "tbCaption";
            this.tbCaption.Size = new System.Drawing.Size(160, 21);
            this.tbCaption.TabIndex = 0;
            // 
            // tbDefaultValue
            // 
            this.tbDefaultValue.Location = new System.Drawing.Point(133, 211);
            this.tbDefaultValue.Name = "tbDefaultValue";
            this.tbDefaultValue.Size = new System.Drawing.Size(160, 21);
            this.tbDefaultValue.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "Default Value:";
            // 
            // cbOperator
            // 
            this.cbOperator.FormattingEnabled = true;
            this.cbOperator.Items.AddRange(new object[] {
            "=",
            "!=",
            ">",
            "<",
            ">=",
            "<=",
            "%**",
            "**%",
            "%%",
            "!%%",
            "<->",
            "!<->",
            "IN",
            "NOT IN"});
            this.cbOperator.Location = new System.Drawing.Point(133, 147);
            this.cbOperator.Name = "cbOperator";
            this.cbOperator.Size = new System.Drawing.Size(160, 20);
            this.cbOperator.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "Operator:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(58, 283);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(189, 283);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "Width:";
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(133, 244);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(160, 21);
            this.tbWidth.TabIndex = 7;
            this.tbWidth.Text = "120";
            // 
            // cbRefVal
            // 
            this.cbRefVal.Enabled = false;
            this.cbRefVal.FormattingEnabled = true;
            this.cbRefVal.Location = new System.Drawing.Point(133, 179);
            this.cbRefVal.Name = "cbRefVal";
            this.cbRefVal.Size = new System.Drawing.Size(160, 20);
            this.cbRefVal.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 181);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "RefVal:";
            // 
            // cbCondition
            // 
            this.cbCondition.FormattingEnabled = true;
            this.cbCondition.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.cbCondition.Location = new System.Drawing.Point(133, 51);
            this.cbCondition.Name = "cbCondition";
            this.cbCondition.Size = new System.Drawing.Size(160, 20);
            this.cbCondition.TabIndex = 1;
            this.cbCondition.Text = "AND";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "Condition:";
            // 
            // frmAnyQueryAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 329);
            this.Controls.Add(this.cbCondition);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbRefVal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbWidth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbOperator);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDefaultValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbCaption);
            this.Controls.Add(this.cbColumnType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbColumnName);
            this.Controls.Add(this.Label1);
            this.MaximizeBox = false;
            this.Name = "frmAnyQueryAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QueryAdd";
            this.Load += new System.EventHandler(this.frmAnyQueryAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ComboBox cbColumnName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbColumnType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCaption;
        private System.Windows.Forms.TextBox tbDefaultValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbOperator;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.ComboBox cbRefVal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbCondition;
        private System.Windows.Forms.Label label8;
    }
}