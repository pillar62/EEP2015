namespace MWizard2015
{
    partial class fmFieldSetting
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpCommon = new System.Windows.Forms.TabPage();
            this.tbEditMask = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbQueryMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefVal = new System.Windows.Forms.Button();
            this.lvFields = new System.Windows.Forms.ListView();
            this.FieldName = new System.Windows.Forms.ColumnHeader();
            this.Caption = new System.Windows.Forms.ColumnHeader();
            this.CheckNull = new System.Windows.Forms.ColumnHeader();
            this.DefaultValue = new System.Windows.Forms.ColumnHeader();
            this.RefValNo = new System.Windows.Forms.ColumnHeader();
            this.QueryMode = new System.Windows.Forms.ColumnHeader();
            this.cbRefValNo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbDataValueField = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbDataTextField = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbTableName = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCheckNull = new System.Windows.Forms.ComboBox();
            this.cbControlType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDefaultValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCaption = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bsSYS_REFVAL_D3 = new System.Windows.Forms.BindingSource(this.components);
            this.bsSYS_REFVAL = new System.Windows.Forms.BindingSource(this.components);
            this.bsSYS_REFVAL_D1 = new System.Windows.Forms.BindingSource(this.components);
            this.bsSYS_REFVAL_D2 = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCommon);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(828, 460);
            this.tabControl.TabIndex = 0;
            // 
            // tpCommon
            // 
            this.tpCommon.Controls.Add(this.tbEditMask);
            this.tpCommon.Controls.Add(this.label2);
            this.tpCommon.Controls.Add(this.cbQueryMode);
            this.tpCommon.Controls.Add(this.label1);
            this.tpCommon.Controls.Add(this.btnRefVal);
            this.tpCommon.Controls.Add(this.lvFields);
            this.tpCommon.Controls.Add(this.cbRefValNo);
            this.tpCommon.Controls.Add(this.label10);
            this.tpCommon.Controls.Add(this.cbDataValueField);
            this.tpCommon.Controls.Add(this.label9);
            this.tpCommon.Controls.Add(this.cbDataTextField);
            this.tpCommon.Controls.Add(this.label8);
            this.tpCommon.Controls.Add(this.cbTableName);
            this.tpCommon.Controls.Add(this.label7);
            this.tpCommon.Controls.Add(this.cbCheckNull);
            this.tpCommon.Controls.Add(this.cbControlType);
            this.tpCommon.Controls.Add(this.label6);
            this.tpCommon.Controls.Add(this.label5);
            this.tpCommon.Controls.Add(this.tbDefaultValue);
            this.tpCommon.Controls.Add(this.label4);
            this.tpCommon.Controls.Add(this.tbCaption);
            this.tpCommon.Controls.Add(this.label3);
            this.tpCommon.Location = new System.Drawing.Point(4, 21);
            this.tpCommon.Name = "tpCommon";
            this.tpCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommon.Size = new System.Drawing.Size(820, 435);
            this.tpCommon.TabIndex = 0;
            this.tpCommon.Text = "Common";
            this.tpCommon.UseVisualStyleBackColor = true;
            // 
            // tbEditMask
            // 
            this.tbEditMask.Location = new System.Drawing.Point(659, 180);
            this.tbEditMask.Name = "tbEditMask";
            this.tbEditMask.Size = new System.Drawing.Size(143, 21);
            this.tbEditMask.TabIndex = 22;
            this.tbEditMask.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(569, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "Edit Mask";
            // 
            // cbQueryMode
            // 
            this.cbQueryMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQueryMode.FormattingEnabled = true;
            this.cbQueryMode.Items.AddRange(new object[] {
            "",
            "None",
            "Normal",
            "Range"});
            this.cbQueryMode.Location = new System.Drawing.Point(659, 145);
            this.cbQueryMode.Name = "cbQueryMode";
            this.cbQueryMode.Size = new System.Drawing.Size(143, 20);
            this.cbQueryMode.TabIndex = 20;
            this.cbQueryMode.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(569, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Query Mode";
            // 
            // btnRefVal
            // 
            this.btnRefVal.Location = new System.Drawing.Point(781, 333);
            this.btnRefVal.Name = "btnRefVal";
            this.btnRefVal.Size = new System.Drawing.Size(21, 20);
            this.btnRefVal.TabIndex = 18;
            this.btnRefVal.Text = "...";
            this.btnRefVal.UseVisualStyleBackColor = true;
            this.btnRefVal.Click += new System.EventHandler(this.btnRefVal_Click);
            // 
            // lvFields
            // 
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FieldName,
            this.Caption,
            this.CheckNull,
            this.DefaultValue,
            this.RefValNo,
            this.QueryMode});
            this.lvFields.Location = new System.Drawing.Point(12, 10);
            this.lvFields.MultiSelect = false;
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(539, 403);
            this.lvFields.TabIndex = 17;
            this.lvFields.UseCompatibleStateImageBehavior = false;
            this.lvFields.View = System.Windows.Forms.View.Details;
            this.lvFields.SelectedIndexChanged += new System.EventHandler(this.lvFields_SelectedIndexChanged);
            this.lvFields.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFields_ColumnClick);
            // 
            // FieldName
            // 
            this.FieldName.Text = "Field Name";
            this.FieldName.Width = 100;
            // 
            // Caption
            // 
            this.Caption.Text = "Caption";
            this.Caption.Width = 100;
            // 
            // CheckNull
            // 
            this.CheckNull.Text = "Check Null";
            this.CheckNull.Width = 70;
            // 
            // DefaultValue
            // 
            this.DefaultValue.Text = "Default Value";
            this.DefaultValue.Width = 80;
            // 
            // RefValNo
            // 
            this.RefValNo.Text = "RefVal No";
            this.RefValNo.Width = 80;
            // 
            // QueryMode
            // 
            this.QueryMode.Text = "QueryMode";
            this.QueryMode.Width = 80;
            // 
            // cbRefValNo
            // 
            this.cbRefValNo.Enabled = false;
            this.cbRefValNo.FormattingEnabled = true;
            this.cbRefValNo.Location = new System.Drawing.Point(659, 334);
            this.cbRefValNo.Name = "cbRefValNo";
            this.cbRefValNo.Size = new System.Drawing.Size(116, 20);
            this.cbRefValNo.TabIndex = 16;
            this.cbRefValNo.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(587, 337);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 15;
            this.label10.Text = "RefValNo";
            // 
            // cbDataValueField
            // 
            this.cbDataValueField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataValueField.Enabled = false;
            this.cbDataValueField.FormattingEnabled = true;
            this.cbDataValueField.Location = new System.Drawing.Point(659, 302);
            this.cbDataValueField.Name = "cbDataValueField";
            this.cbDataValueField.Size = new System.Drawing.Size(143, 20);
            this.cbDataValueField.TabIndex = 14;
            this.cbDataValueField.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(558, 305);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "Data Value Field";
            // 
            // cbDataTextField
            // 
            this.cbDataTextField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataTextField.Enabled = false;
            this.cbDataTextField.FormattingEnabled = true;
            this.cbDataTextField.Location = new System.Drawing.Point(659, 273);
            this.cbDataTextField.Name = "cbDataTextField";
            this.cbDataTextField.Size = new System.Drawing.Size(143, 20);
            this.cbDataTextField.TabIndex = 12;
            this.cbDataTextField.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(558, 276);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "Data Text Field";
            // 
            // cbTableName
            // 
            this.cbTableName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTableName.Enabled = false;
            this.cbTableName.FormattingEnabled = true;
            this.cbTableName.Location = new System.Drawing.Point(659, 244);
            this.cbTableName.Name = "cbTableName";
            this.cbTableName.Size = new System.Drawing.Size(143, 20);
            this.cbTableName.TabIndex = 10;
            this.cbTableName.SelectedIndexChanged += new System.EventHandler(this.cbTableName_SelectedIndexChanged);
            this.cbTableName.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(576, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "Table Name";
            // 
            // cbCheckNull
            // 
            this.cbCheckNull.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheckNull.FormattingEnabled = true;
            this.cbCheckNull.Items.AddRange(new object[] {
            "",
            "N",
            "Y"});
            this.cbCheckNull.Location = new System.Drawing.Point(659, 72);
            this.cbCheckNull.Name = "cbCheckNull";
            this.cbCheckNull.Size = new System.Drawing.Size(143, 20);
            this.cbCheckNull.TabIndex = 8;
            this.cbCheckNull.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // cbControlType
            // 
            this.cbControlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbControlType.FormattingEnabled = true;
            this.cbControlType.Items.AddRange(new object[] {
            "TextBox",
            "ComboBox",
            "RefValBox",
            "ValidateBox",
            "DateTimeBox",
            "CheckBox"});
            this.cbControlType.Location = new System.Drawing.Point(659, 213);
            this.cbControlType.Name = "cbControlType";
            this.cbControlType.Size = new System.Drawing.Size(143, 20);
            this.cbControlType.TabIndex = 7;
            this.cbControlType.SelectedValueChanged += new System.EventHandler(this.cbControlType_SelectedValueChanged);
            this.cbControlType.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(564, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "Control Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(569, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "Check Null";
            // 
            // tbDefaultValue
            // 
            this.tbDefaultValue.Location = new System.Drawing.Point(659, 107);
            this.tbDefaultValue.Name = "tbDefaultValue";
            this.tbDefaultValue.Size = new System.Drawing.Size(143, 21);
            this.tbDefaultValue.TabIndex = 3;
            this.tbDefaultValue.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(564, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Default Value";
            // 
            // tbCaption
            // 
            this.tbCaption.Location = new System.Drawing.Point(659, 36);
            this.tbCaption.Name = "tbCaption";
            this.tbCaption.Size = new System.Drawing.Size(143, 21);
            this.tbCaption.TabIndex = 1;
            this.tbCaption.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(587, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Caption";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(353, 474);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(220, 474);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fmFieldSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 517);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl);
            this.Name = "fmFieldSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fmFieldSetting";
            this.tabControl.ResumeLayout(false);
            this.tpCommon.ResumeLayout(false);
            this.tpCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCommon;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL_D3;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL_D1;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL_D2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbCheckNull;
        private System.Windows.Forms.ComboBox cbControlType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDefaultValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCaption;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDataValueField;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbDataTextField;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbTableName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbRefValNo;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader FieldName;
        private System.Windows.Forms.ColumnHeader Caption;
        private System.Windows.Forms.ColumnHeader CheckNull;
        private System.Windows.Forms.ColumnHeader DefaultValue;
        private System.Windows.Forms.ColumnHeader RefValNo;
        private System.Windows.Forms.Button btnRefVal;
        private System.Windows.Forms.ComboBox cbQueryMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader QueryMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbEditMask;

    }
}