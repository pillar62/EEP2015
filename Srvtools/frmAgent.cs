using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net.Mail;

namespace Srvtools
{
    public partial class frmAgent : InfoForm
    {
        public frmAgent()
        {
            InitializeComponent();
        }

        private string _roleId = "";
        public string RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        private string _roleName = "";
        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }

        private void frmAgent_Load(object sender, EventArgs e)
        {
            SYS_LANGUAGE language = CliUtils.fClientLang;
            try
            {
                string[] captions = SysMsg.GetSystemMessage(language, "Srvtools", "frmAgent", "UITexts").Split(',');
                this.colAgent.HeaderText = captions[0];
                this.colFlowDesc.HeaderText = captions[1];
                this.colSDate.HeaderText = captions[2];
                this.colSTime.HeaderText = captions[3];
                this.colEDate.HeaderText = captions[4];
                this.colETime.HeaderText = captions[5];
                this.colParAgent.HeaderText = captions[6];
                this.colRemark.HeaderText = captions[7];
                this.lblRoleId.Text = captions[8];
                this.lblRoleName.Text = captions[9];
            }
            catch { }

            string flowDefPath = string.Format("{0}\\WorkFlow\\", EEPRegistry.Server);
            DirectoryInfo dir = new DirectoryInfo(flowDefPath);
            if (dir.Exists)
            {
                object[] obj = CliUtils.CallFLMethod("GetFLDescriptions", new object[] { flowDefPath });
                if (Convert.ToInt16(obj[0]) == 0)
                {
                    ArrayList flDescs = (ArrayList)obj[1];
                    flDescs.Insert(0, "*");
                    this.colFlowDesc.DataSource = flDescs;
                }
                this.txtRoleId.Text = this._roleId;
                this.txtRoleName.Text = this._roleName;
            }
            this.dsRoleAgent.SetWhere("ROLE_ID='" + _roleId + "'");
        }

        public string DefRoleId()
        {
            return this.RoleId;
        }

        void navRoleAgent_BeforeItemClick(object sender, BeforeItemClickEventArgs e)
        {
            if (e.ItemName == "Apply" || e.ItemName == "OK")
            {
                if (bsRoleAgent.Current != null)
                {
                    (bsRoleAgent.Current as DataRowView).EndEdit();
                }
                DataTable table = dsRoleAgent.RealDataSet.Tables[0].GetChanges();
                if (table != null)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        if (row.RowState != DataRowState.Deleted)
                        {
                            string start = row["START_DATE"].ToString() + row["START_TIME"].ToString();

                            string end = row["END_DATE"].ToString() + row["END_TIME"].ToString();
                            if (start.CompareTo(end) > 0)
                            {
                                MessageBox.Show("START_DATE can not larger than END_DATE.");
                                e.Cancel = true;
                            }
                            #region send mail
                            var userid = row["AGENT"].ToString();
                            var dtUser = Srvtools.CliUtils.ExecuteSql("GLModule", "cmdDDUse", "select * from USERS where USERID = '" + userid + "'", true, Srvtools.CliUtils.fCurrentProject);
                            var starttime = row["START_DATE"].ToString().Substring(0, 4) + "-" + row["START_DATE"].ToString().Substring(4, 2) + "-" + row["START_DATE"].ToString().Substring(6) + " " + row["START_TIME"].ToString();
                            var endtime = row["END_DATE"].ToString().Substring(0, 4) + "-" + row["END_DATE"].ToString().Substring(4, 2) + "-" + row["END_DATE"].ToString().Substring(6) + " " + row["END_TIME"].ToString();
                            if (dtUser.Tables[0].Rows.Count > 0 && dtUser.Tables[0].Rows[0].ToString() != "")
                            {
                                string toemail = dtUser.Tables[0].Rows[0]["EMAIL"].ToString();
                                if (toemail != "")
                                {
                                    string username = dtUser.Tables[0].Rows[0]["USERNAME"].ToString();
                                    bool active = false;
                                    string sendFrom = string.Empty;
                                    string password = string.Empty;
                                    string smtp = string.Empty;
                                    int port = 0;
                                    bool enableSSL = false;
                                    var subject = "";
                                    var body = "";
                                    var times = "";
                                    SYS_LANGUAGE language = CliUtils.fClientLang;

                                    subject = "Agent Notification";
                                    times = "Period of agency";
                                    string[] captions = SysMsg.GetSystemMessage(language, "Srvtools", "frmAgent", "UITexts").Split(',');
                                    if (captions.Length > 10)
                                    {
                                        subject = captions[10];
                                        times = captions[11];
                                    }
                                    body = captions[8] + ":" + txtRoleId.Text + "(" + txtRoleName.Text + ") <br /> " + captions[0] + ": " + userid + "(" + username + ") <br /> "
                                        + captions[1] + ": " + row["FLOW_DESC"].ToString() + " <br /> " + times + ": " + starttime + " ~ " + endtime + " ";

                                    XmlDocument doc = new XmlDocument();
                                    string xmlFileName = string.Format("{0}\\Workflow.xml", EEPRegistry.Server);
                                    if (File.Exists(xmlFileName))
                                        doc.Load(xmlFileName);
                                    else
                                        return;

                                    XmlNode nodeActive = doc.ChildNodes[0].SelectSingleNode("Active");
                                    if (nodeActive != null)
                                    {
                                        active = Convert.ToBoolean(nodeActive.InnerText);
                                    }
                                    if (!active)
                                    {
                                        return;
                                    }


                                    XmlNode nodeEmail = doc.ChildNodes[0].SelectSingleNode("Email");
                                    if (nodeEmail != null)
                                    {
                                        sendFrom = nodeEmail.InnerText;
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    XmlNode nodePassword = doc.ChildNodes[0].SelectSingleNode("Password");
                                    if (nodePassword != null)
                                    {
                                        password = GetPwdString(nodePassword.InnerText);
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    XmlNode nodeSMTP = doc.ChildNodes[0].SelectSingleNode("SMTP");
                                    if (nodeSMTP != null)
                                    {
                                        smtp = nodeSMTP.InnerText;
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    XmlNode nodeSLL = doc.ChildNodes[0].SelectSingleNode("EnableSSL");
                                    if (nodeSLL != null)
                                    {
                                        enableSSL = Convert.ToBoolean(nodeSLL.InnerText);
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    XmlNode nodePort = doc.ChildNodes[0].SelectSingleNode("Port");
                                    if (nodePort != null)
                                    {
                                        if (!string.IsNullOrEmpty(nodePort.InnerText))
                                        {
                                            port = Convert.ToInt32(nodePort.InnerText);
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    try
                                    {
                                        //Builed The MSG
                                        MailMessage msg = new MailMessage();
                                        msg.To.Add(toemail);
                                        msg.From = new MailAddress(sendFrom, "Infolight", System.Text.Encoding.UTF8);
                                        msg.Subject = subject;
                                        msg.SubjectEncoding = System.Text.Encoding.UTF8;
                                        msg.Body = body;
                                        msg.BodyEncoding = System.Text.Encoding.UTF8;
                                        msg.IsBodyHtml = true;
                                        msg.Priority = MailPriority.High;

                                        //Add the Creddentials
                                        SmtpClient client = new SmtpClient();
                                        client.Credentials = new System.Net.NetworkCredential(sendFrom, password);
                                        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                                        client.EnableSsl = enableSSL;
                                        if (smtp != "")
                                            client.Host = smtp;
                                        if (port != 0)
                                            client.Port = port;

                                        //client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                                        object userState = msg;
                                        if (client != null)
                                        {
                                            //you can also call client.Send(msg)
                                            client.Send(msg);
                                            //client.Send(msg);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        //LogError(ex);
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
            }
        }

        private static string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }
    }
}