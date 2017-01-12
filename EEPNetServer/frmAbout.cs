using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using Srvtools;

namespace EEPNetServer
{
	/// <summary>
	/// Summary description for WinForm2.
	/// </summary>
    public class frmAbout : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Button button1;
        private Label label2;
        private Label label1;
        private Label lbCaption;
        private Label lbDescription;
        private Label label3;
        private Label lbLicense;
        private Label label4;
        private Label label5;
        private Button button2;
        private System.Windows.Forms.Button btnOK;

        public frmAbout()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.btnOK = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCaption = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbLicense = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(220, 230);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(115, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Close";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 25);
            this.button1.TabIndex = 8;
            this.button1.Text = "Register...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(135, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 17);
            this.label2.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(135, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 16);
            this.label1.TabIndex = 18;
            // 
            // lbCaption
            // 
            this.lbCaption.AutoSize = true;
            this.lbCaption.BackColor = System.Drawing.Color.Transparent;
            this.lbCaption.Font = new System.Drawing.Font("Colonna MT", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCaption.ForeColor = System.Drawing.Color.White;
            this.lbCaption.Location = new System.Drawing.Point(14, 14);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(123, 17);
            this.lbCaption.TabIndex = 19;
            this.lbCaption.Text = "About EEP.Net";
            // 
            // lbDescription
            // 
            this.lbDescription.AutoSize = true;
            this.lbDescription.BackColor = System.Drawing.Color.Transparent;
            this.lbDescription.Font = new System.Drawing.Font("Colonna MT", 12F, System.Drawing.FontStyle.Bold);
            this.lbDescription.ForeColor = System.Drawing.Color.White;
            this.lbDescription.Location = new System.Drawing.Point(17, 51);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(187, 17);
            this.lbDescription.TabIndex = 20;
            this.lbDescription.Text = "Infolight EEP.NET 2010";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(20, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "2010 Infolight System Corp.";
            // 
            // lbLicense
            // 
            this.lbLicense.AutoSize = true;
            this.lbLicense.BackColor = System.Drawing.Color.Transparent;
            this.lbLicense.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLicense.Location = new System.Drawing.Point(23, 157);
            this.lbLicense.Name = "lbLicense";
            this.lbLicense.Size = new System.Drawing.Size(74, 16);
            this.lbLicense.TabIndex = 22;
            this.lbLicense.Text = "Licensed to:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(23, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 16);
            this.label4.TabIndex = 23;
            this.label4.Text = "All rights reserved.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(23, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 16);
            this.label5.TabIndex = 24;
            this.label5.Text = "Serial number:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(116, 230);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 25);
            this.button2.TabIndex = 25;
            this.button2.Text = "UnRegister SD";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnOK;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(360, 270);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbLicense);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.lbCaption);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " About EEP.NET";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void frmAbout_Load(object sender, System.EventArgs e)
        {
            label2.Text = x64f2717168e0a936.Company;
            label1.Text = x64f2717168e0a936.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmRegister frm = new frmRegister();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                frmAbout_Load(null, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //
            x64f2717168e0a936.x4a8195996de95b53();
            label1.Text = x64f2717168e0a936.Text;
        }
    }
}
