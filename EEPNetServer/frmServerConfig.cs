using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Xml;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Threading;
using System.Runtime.Remoting.Proxies;

namespace EEPNetServer
{
    public partial class frmServerConfig : Form
    {
        public frmServerConfig()
        {
            InitializeComponent();
        }

        private void frmServerConfig_Load(object sender, EventArgs e)
        {
            string s = SystemFile.ServerConfigFile;
            XmlDocument CfgXml = new XmlDocument();
            if (File.Exists(s))
            {
                CfgXml.Load(s);
                XmlNode aNode = CfgXml.DocumentElement.FirstChild;
                while (aNode != null)
                {
                    string serverType = aNode.LocalName;
                    if (string.Compare(serverType, "maxtimeout", true) == 0)//IgnoreCase
                    {
                        txtMaxTimeOut.Text = aNode.Attributes["Value"].InnerText;
                    }
                    else if (string.Compare(serverType, "ssotimeout", true) == 0)//IgnoreCase
                    {
                        txtSSOTimeout.Text = aNode.Attributes["Value"].InnerText;
                    }
                    else if (string.Compare(serverType, "ssokey", true) == 0)//IgnoreCase
                    {
                        txtSSOKey.Text = aNode.Attributes["Value"].InnerText;
                    }
                    else if (string.Compare(serverType, "recordlockindatabase", true) == 0)//IgnoreCase
                    {
                        checkBoxRecordLock.Checked = string.Compare(aNode.Attributes["Value"].InnerText, bool.TrueString, true) == 0;
                    }
                    else if (string.Compare(serverType, "localserver", true) == 0)//IgnoreCase
                    {
                        txtMaxUser.Text = aNode.Attributes["MaxUser"].InnerText;
                        //if (string.Compare(aNode.Attributes["IsMaster"].InnerText, "true", true) == 0)//IgnoreCase
                        //{
                        //    cbxIsMaster.Checked = true;
                        //}
                        //else
                        //{
                        //    cbxIsMaster.Checked = false;
                        //}
                        cbxIsMaster.Checked = false;
                        txtMasterServerIP.Text = aNode.Attributes["MasterServerIP"].InnerText;
                        if (aNode.Attributes["MasterServerKey"] != null && aNode.Attributes["MasterServerKey"].InnerText != "")
                        {
                            txtMasterServerKey.Text = aNode.Attributes["MasterServerKey"].Value;
                        }
                        else
                        {
                            txtMasterServerKey.Text = "";
                        }
                    }
                    else if (string.Compare(serverType, "remoteserver", true) == 0)//IgnoreCase
                    {
                        bool active = aNode.Attributes["Active"] == null ? false : bool.Parse(aNode.Attributes["Active"].Value);
                        string ipAddress = aNode.Attributes["IpAddress"].InnerText;
                        try
                        {
                            string ip = aNode.Attributes["IpAddress"].Value;
                            int port = (aNode.Attributes["Port"] != null && aNode.Attributes["Port"].Value.Length > 0)
                                ? int.Parse(aNode.Attributes["Port"].Value) : 8989;
                            lbxRemoteServer.Items.Add(new RemoteServer(0, ip, port, active));
                        }
                        catch
                        { 
                        
                        }
                    }
                    aNode = aNode.NextSibling;
                }
            }

            this.textBox1.Text = ServerConfig.UserLoginCount.ToString();
            for (int i = 0; i < lbxRemoteServer.Items.Count; i++)
            {
                lbxRemoteServer.SelectedIndex = i;
            }
            if (lbxRemoteServer.Items.Count > 0)
            {
                lbxRemoteServer.SelectedIndex = 0;
            }
        }

        private void lbxRemoteServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbxRemoteServer.SelectedIndex != -1)
            {
                RefreshRemoteServerInfo();
            }
        }

        private void RefreshRemoteServerInfo()
        {
            if (this.lbxRemoteServer.SelectedItem != null)
            {
                RemoteServer rs = this.lbxRemoteServer.SelectedItem as RemoteServer;
                txtRemoteIpAddress.Text = rs.ToString();
                txtRemoteMaxUser.Text = "Unknown";
                txtRemoteCurrentUser.Text = "Unknown";
                if (rs.Activated)
                { 
                    frmProgress frm = new frmProgress(rs.IpAddress, rs.Port, string.Format("Connect to {0}", rs));
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Srvtools.LoginService service = frm.Module;
                        if (service != null)
                        {
                            try
                            {
                                int user = 0, max = 0;
                                service.GetLoginInfo(ref user, ref max);
                                txtRemoteMaxUser.Text = max.ToString();
                                txtRemoteCurrentUser.Text = user.ToString();
                                ServerConfig.RegisterRemoteServer(rs.IpAddress, rs.Port);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                                rs.Activated = false;
                                ServerConfig.DeRegisterRemoteServer(rs.IpAddress, rs.Port);
                            }
                        }
                        else
                        {
                            rs.Activated = false;
                            ServerConfig.DeRegisterRemoteServer(rs.IpAddress, rs.Port);
                        }
                    }
                    else
                    {
                        rs.Activated = false;
                        ServerConfig.DeRegisterRemoteServer(rs.IpAddress, rs.Port);
                    }
                }
                checkBoxActive.Checked = rs.Activated;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddRemoteServer fAddRemoteServer = new frmAddRemoteServer();
            fAddRemoteServer.Text = "Add A Remote Server";
            if (fAddRemoteServer.ShowDialog() == DialogResult.OK)
            {
                lbxRemoteServer.SelectedIndex = lbxRemoteServer.Items.Add(new RemoteServer(0, fAddRemoteServer.IpAddress, fAddRemoteServer.Port, false));
                RefreshRemoteServerInfo();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int index = this.lbxRemoteServer.SelectedIndex;
            if (index != -1)
            {
                frmAddRemoteServer fAddRemoteServer = new frmAddRemoteServer();
                fAddRemoteServer.Text = "Modify A Remote Server";
                RemoteServer rs = this.lbxRemoteServer.Items[index] as RemoteServer;
                fAddRemoteServer.IpAddress = rs.IpAddress;
                fAddRemoteServer.Port = rs.Port;

                if (fAddRemoteServer.ShowDialog() == DialogResult.OK)
                {
                    lbxRemoteServer.Items[index] = new RemoteServer(0, fAddRemoteServer.IpAddress, fAddRemoteServer.Port, false);
                    RefreshRemoteServerInfo();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxRemoteServer.SelectedIndex != -1)
            {
                lbxRemoteServer.Items.RemoveAt(lbxRemoteServer.SelectedIndex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Data Validation 
            string errString = "";
            // Ip Address
            //if (this.cbxIsMaster.Checked == false && this.txtMasterServerPort.Text.Length > 0)
            //{
            //    try
            //    {
            //        int port = int.Parse(txtMasterServerPort.Text);
            //        if (port < 0 || port > 65535)
            //        {
            //            errString += "Port Number should in 0-65535";
            //        }
            //    }
            //    catch
            //    {
            //        errString += "Port Number is not valid.\n\r";
            //    }
            //}

            // Max Time Out
            double maxTimeOut = 0;
            if (txtMaxTimeOut.Text != "")
            {
                string sMaxTimeOut = txtMaxTimeOut.Text;
                if (!double.TryParse(sMaxTimeOut, out maxTimeOut))
                {
                    maxTimeOut = 0;
                }
            }

            double ssoTimeOut = 24;
            if (txtSSOTimeout.Text != "")
            {
                string sSSOTimeOut = txtSSOTimeout.Text;
                if (!double.TryParse(sSSOTimeOut, out ssoTimeOut))
                {
                }
            }

            // Max User
           
            try
            { 
                int num = 0;
                num = int.Parse(this.txtMaxUser.Text);
                if (num < 0)
                {
                    errString += "Max User must be a positive integer or zero.\n\r";
                }
                else
                {
                    ServerConfig.MaxUser = num;
                }
            }
            catch
            {
                errString += "Max User must be a positive integer or zero.\n\r";
            }

            if (errString != "")
            {
                MessageBox.Show(errString);
                return;
            }


            string s = SystemFile.ServerConfigFile;

            if (File.Exists(s)) File.Delete(s);

            FileStream aFileStream = new FileStream(s, FileMode.Create);
            try
            {
                XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                w.Formatting = Formatting.Indented;
                w.WriteStartElement("ServerConfig");

                w.WriteStartElement("MaxTimeOut");
                w.WriteAttributeString("Value", maxTimeOut.ToString("N2"));
                w.WriteEndElement();

                w.WriteStartElement("SSOTimeOut");
                w.WriteAttributeString("Value", ssoTimeOut.ToString());
                w.WriteEndElement();

                w.WriteStartElement("SSOKey");
                w.WriteAttributeString("Value", this.txtSSOKey.Text);
                w.WriteEndElement();

                w.WriteStartElement("RecordLockInDatabase");
                w.WriteAttributeString("Value", this.checkBoxRecordLock.Checked.ToString());
                w.WriteEndElement();

                w.WriteStartElement("LocalServer");
                w.WriteAttributeString("MaxUser", this.txtMaxUser.Text);
                w.WriteAttributeString("IsMaster", this.cbxIsMaster.Checked.ToString());
                string[] strmasteripaddresss = this.txtMasterServerIP.Text.Split(':');
                w.WriteAttributeString("MasterServerIP", this.txtMasterServerIP.Text);
                w.WriteAttributeString("MasterServerKey", txtMasterServerKey.Text);
                w.WriteEndElement();

                for (int i = 0; i < lbxRemoteServer.Items.Count; i++)
                {
                    w.WriteStartElement("RemoteServer");
                    RemoteServer rs = this.lbxRemoteServer.Items[i] as RemoteServer;
                    w.WriteAttributeString("IpAddress", rs.IpAddress);
                    w.WriteAttributeString("Port", rs.Port.ToString());
                    w.WriteAttributeString("Active", rs.Activated.ToString());
                    w.WriteEndElement();
                }

                w.WriteEndElement();
                w.Close();
            }
            finally
            {
                aFileStream.Close();
            }
            this.Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Pen pen = new Pen(Brushes.DarkGray);
            //e.Graphics.DrawLine(pen, 22, 140, this.Width - 32, 140);
            //pen.Brush = Brushes.White;
            //e.Graphics.DrawLine(pen, 22, 141, this.Width - 32, 141);

            //Pen pen2 = new Pen(Brushes.DarkGray);
            //e.Graphics.DrawLine(pen2, 22, 42, this.Width - 32, 42);
            //pen2.Brush = Brushes.White;
            //e.Graphics.DrawLine(pen2, 22, 43, this.Width - 32, 43);
        }

        private void cbxIsMaster_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbxIsMaster.Checked)
            {
                this.txtMasterServerIP.BackColor = System.Drawing.SystemColors.Control;
                this.txtMasterServerIP.ReadOnly = true;
                this.txtMasterServerKey.BackColor = System.Drawing.SystemColors.Control;
                this.txtMasterServerKey.ReadOnly = true;
            }
            else
            {
                this.txtMasterServerIP.BackColor = System.Drawing.SystemColors.HighlightText;
                this.txtMasterServerIP.ReadOnly = false;
                this.txtMasterServerKey.BackColor = System.Drawing.SystemColors.HighlightText;
                this.txtMasterServerKey.ReadOnly = false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = ServerConfig.UserLoginCount.ToString();
        }

        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            if (this.lbxRemoteServer.SelectedItem != null)
            {
                RemoteServer rs = this.lbxRemoteServer.SelectedItem as RemoteServer;
                if (rs.Activated != checkBoxActive.Checked)
                {
                    rs.Activated = checkBoxActive.Checked;
                    RefreshRemoteServerInfo();
                }
            }
        }


    }
}