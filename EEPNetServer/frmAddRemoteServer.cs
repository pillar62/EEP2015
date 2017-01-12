using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace EEPNetServer
{
    /// <summary>
    /// Summary description for WinForm.
    /// </summary>
    public class frmAddRemoteServer : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btnOK;
        private TextBox txtRemoteIpAddress;
        private Label lblRemoteIpAddress;
        private Label label1;
        private TextBox tbPort;
        private Label label2;
        private System.Windows.Forms.Button btnCancel;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddRemoteServer));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtRemoteIpAddress = new System.Windows.Forms.TextBox();
            this.lblRemoteIpAddress = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(141, 95);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(71, 24);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(230, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            // 
            // txtRemoteIpAddress
            // 
            this.txtRemoteIpAddress.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtRemoteIpAddress.Location = new System.Drawing.Point(109, 23);
            this.txtRemoteIpAddress.Name = "txtRemoteIpAddress";
            this.txtRemoteIpAddress.Size = new System.Drawing.Size(192, 21);
            this.txtRemoteIpAddress.TabIndex = 0;
            // 
            // lblRemoteIpAddress
            // 
            this.lblRemoteIpAddress.AutoSize = true;
            this.lblRemoteIpAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemoteIpAddress.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblRemoteIpAddress.Location = new System.Drawing.Point(33, 30);
            this.lblRemoteIpAddress.Name = "lblRemoteIpAddress";
            this.lblRemoteIpAddress.Size = new System.Drawing.Size(65, 15);
            this.lblRemoteIpAddress.TabIndex = 9;
            this.lblRemoteIpAddress.Text = "IP Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(69, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Port";
            // 
            // tbPort
            // 
            this.tbPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbPort.Location = new System.Drawing.Point(109, 56);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(76, 21);
            this.tbPort.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(191, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "(Default:8989)";
            // 
            // frmAddRemoteServer
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(345, 136);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.txtRemoteIpAddress);
            this.Controls.Add(this.lblRemoteIpAddress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAddRemoteServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Server Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public string IpAddress
        {
            get
            {
                return this.txtRemoteIpAddress.Text;
            }
            set
            {
                this.txtRemoteIpAddress.Text = value;
            }
        }

        public int Port
        {
            get
            {
                if (tbPort.Text.Length == 0)
                {
                    return 8989;
                }
                else
                {
                    try
                    {
                        return int.Parse(tbPort.Text.Trim());
                    }
                    catch
                    { 
                        return 8989;
                    }
                }
            }
            set
            {
                this.tbPort.Text = value.ToString();
            }
        }

        public frmAddRemoteServer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public frmAddRemoteServer(string sDB, string sString, string dbType, bool bMod)
        {
            InitializeComponent();
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbPort.Text.Trim().Length > 0)
            {
                try
                {
                    int port = int.Parse(tbPort.Text);
                    if (port < 0 || port > 65535)
                    {
                        MessageBox.Show(this, "Port Number should in 0-65535.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MessageBox.Show(this, "Port Number is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
