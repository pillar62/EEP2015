namespace EFServerTools.Design.EFCommandDesign
{
    partial class EFCommandEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EFCommandEditor));
            this.imageListProperties = new System.Windows.Forms.ImageList();
            this.contextMenuStripScript = new System.Windows.Forms.ContextMenuStrip();
            this.contextMenuStripEntityContainer = new System.Windows.Forms.ContextMenuStrip();
            this.contextMenuStripEntitySet = new System.Windows.Forms.ContextMenuStrip();
            this.contextMenuStripProperty = new System.Windows.Forms.ContextMenuStrip();
            this.labelEntitySql = new System.Windows.Forms.Label();
            this.labelEntitySets = new System.Windows.Forms.Label();
            this.labelProperties = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listViewProperties = new System.Windows.Forms.ListView();
            this.listBoxEntitySets = new System.Windows.Forms.ListBox();
            this.labelContainerName = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelMeatadataFile = new System.Windows.Forms.Label();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.comboBoxMeatadataFile = new System.Windows.Forms.ComboBox();
            this.comboBoxContainerName = new System.Windows.Forms.ComboBox();
            this.textBoxCommandText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // imageListProperties
            // 
            this.imageListProperties.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListProperties.ImageStream")));
            this.imageListProperties.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListProperties.Images.SetKeyName(0, "property");
            this.imageListProperties.Images.SetKeyName(1, "navproperty");
            // 
            // contextMenuStripScript
            // 
            this.contextMenuStripScript.Name = "contextMenuStripScript";
            this.contextMenuStripScript.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripScript.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // contextMenuStripEntityContainer
            // 
            this.contextMenuStripEntityContainer.Name = "contextMenuStripScript";
            this.contextMenuStripEntityContainer.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripEntityContainer.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // contextMenuStripEntitySet
            // 
            this.contextMenuStripEntitySet.Name = "contextMenuStripScript";
            this.contextMenuStripEntitySet.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripEntitySet.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // contextMenuStripProperty
            // 
            this.contextMenuStripProperty.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.contextMenuStripProperty.Name = "contextMenuStripScript";
            this.contextMenuStripProperty.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripProperty.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // labelEntitySql
            // 
            this.labelEntitySql.AutoSize = true;
            this.labelEntitySql.Location = new System.Drawing.Point(28, 311);
            this.labelEntitySql.Name = "labelEntitySql";
            this.labelEntitySql.Size = new System.Drawing.Size(71, 12);
            this.labelEntitySql.TabIndex = 26;
            this.labelEntitySql.Text = "Entity SQL:";
            // 
            // labelEntitySets
            // 
            this.labelEntitySets.AutoSize = true;
            this.labelEntitySets.Location = new System.Drawing.Point(28, 56);
            this.labelEntitySets.Name = "labelEntitySets";
            this.labelEntitySets.Size = new System.Drawing.Size(71, 12);
            this.labelEntitySets.TabIndex = 24;
            this.labelEntitySets.Text = "Entity Set:";
            // 
            // labelProperties
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Location = new System.Drawing.Point(240, 56);
            this.labelProperties.Name = "labelProperties";
            this.labelProperties.Size = new System.Drawing.Size(71, 12);
            this.labelProperties.TabIndex = 25;
            this.labelProperties.Text = "Properties:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(425, 519);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // listViewProperties
            // 
            this.listViewProperties.Location = new System.Drawing.Point(238, 74);
            this.listViewProperties.Name = "listViewProperties";
            this.listViewProperties.Size = new System.Drawing.Size(268, 220);
            this.listViewProperties.SmallImageList = this.imageListProperties;
            this.listViewProperties.TabIndex = 23;
            this.listViewProperties.UseCompatibleStateImageBehavior = false;
            this.listViewProperties.View = System.Windows.Forms.View.List;
            // 
            // listBoxEntitySets
            // 
            this.listBoxEntitySets.FormattingEnabled = true;
            this.listBoxEntitySets.ItemHeight = 12;
            this.listBoxEntitySets.Location = new System.Drawing.Point(28, 74);
            this.listBoxEntitySets.Name = "listBoxEntitySets";
            this.listBoxEntitySets.Size = new System.Drawing.Size(190, 220);
            this.listBoxEntitySets.TabIndex = 18;
            this.listBoxEntitySets.SelectedIndexChanged += new System.EventHandler(this.listBoxEntitySets_SelectedIndexChanged);
            // 
            // labelContainerName
            // 
            this.labelContainerName.AutoSize = true;
            this.labelContainerName.Location = new System.Drawing.Point(240, 29);
            this.labelContainerName.Name = "labelContainerName";
            this.labelContainerName.Size = new System.Drawing.Size(107, 12);
            this.labelContainerName.TabIndex = 22;
            this.labelContainerName.Text = "Entity Container:";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(332, 519);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelMeatadataFile
            // 
            this.labelMeatadataFile.AutoSize = true;
            this.labelMeatadataFile.Location = new System.Drawing.Point(28, 29);
            this.labelMeatadataFile.Name = "labelMeatadataFile";
            this.labelMeatadataFile.Size = new System.Drawing.Size(65, 12);
            this.labelMeatadataFile.TabIndex = 21;
            this.labelMeatadataFile.Text = "Edmx File:";
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(431, 302);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 19;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // comboBoxMeatadataFile
            // 
            this.comboBoxMeatadataFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMeatadataFile.FormattingEnabled = true;
            this.comboBoxMeatadataFile.Location = new System.Drawing.Point(97, 21);
            this.comboBoxMeatadataFile.Name = "comboBoxMeatadataFile";
            this.comboBoxMeatadataFile.Size = new System.Drawing.Size(121, 20);
            this.comboBoxMeatadataFile.TabIndex = 16;
            this.comboBoxMeatadataFile.SelectedIndexChanged += new System.EventHandler(this.comboBoxMeatadataFile_SelectedIndexChanged);
            // 
            // comboBoxContainerName
            // 
            this.comboBoxContainerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContainerName.FormattingEnabled = true;
            this.comboBoxContainerName.Location = new System.Drawing.Point(352, 21);
            this.comboBoxContainerName.Name = "comboBoxContainerName";
            this.comboBoxContainerName.Size = new System.Drawing.Size(121, 20);
            this.comboBoxContainerName.TabIndex = 17;
            this.comboBoxContainerName.SelectedIndexChanged += new System.EventHandler(this.comboBoxContainerName_SelectedIndexChanged);
            // 
            // textBoxCommandText
            // 
            this.textBoxCommandText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCommandText.DetectUrls = false;
            this.textBoxCommandText.Font = new System.Drawing.Font("Arial", 9F);
            this.textBoxCommandText.Location = new System.Drawing.Point(27, 334);
            this.textBoxCommandText.Name = "textBoxCommandText";
            this.textBoxCommandText.Size = new System.Drawing.Size(479, 169);
            this.textBoxCommandText.TabIndex = 20;
            this.textBoxCommandText.Text = "";
            this.textBoxCommandText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCommandText_KeyPress);
            // 
            // FormCommandEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 562);
            this.Controls.Add(this.labelEntitySql);
            this.Controls.Add(this.labelEntitySets);
            this.Controls.Add(this.labelProperties);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.listViewProperties);
            this.Controls.Add(this.listBoxEntitySets);
            this.Controls.Add(this.labelContainerName);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelMeatadataFile);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.comboBoxMeatadataFile);
            this.Controls.Add(this.comboBoxContainerName);
            this.Controls.Add(this.textBoxCommandText);
            this.Name = "FormCommandEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity SQL Editor";
            this.Load += new System.EventHandler(this.FormCommandEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListProperties;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripScript;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEntityContainer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEntitySet;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripProperty;
        private System.Windows.Forms.Label labelEntitySql;
        private System.Windows.Forms.Label labelEntitySets;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listViewProperties;
        private System.Windows.Forms.ListBox listBoxEntitySets;
        private System.Windows.Forms.Label labelContainerName;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelMeatadataFile;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.ComboBox comboBoxMeatadataFile;
        private System.Windows.Forms.ComboBox comboBoxContainerName;
        private System.Windows.Forms.RichTextBox textBoxCommandText;
    }
}