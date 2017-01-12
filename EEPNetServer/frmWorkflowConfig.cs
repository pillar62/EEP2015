using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace EEPNetServer
{
    public partial class frmWorkflowConfig : Form
    {
        public frmWorkflowConfig()
        {
            InitializeComponent();

            if (File.Exists("Workflow.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Workflow.xml");

                XmlNode nodeActive = doc.ChildNodes[0].SelectSingleNode("Active");
                if (nodeActive != null)
                {
                    chkActive.Checked = Convert.ToBoolean(nodeActive.InnerText);
                }
                else
                {
                    chkActive.Checked = true;
                }

                XmlNode nodeEmail = doc.ChildNodes[0].SelectSingleNode("Email");
                if (nodeEmail != null)
                {
                    txtEmail.Text = nodeEmail.InnerText;
                }

                XmlNode nodePassword = doc.ChildNodes[0].SelectSingleNode("Password");
                if (nodePassword != null)
                {
                    txtPassword.Text = GetPwdString(nodePassword.InnerText);
                }

                XmlNode enableSSL = doc.ChildNodes[0].SelectSingleNode("EnableSSL");
                if (enableSSL != null)
                {
                    cbEnableSSL.Checked = Convert.ToBoolean(enableSSL.InnerText);
                }
                else
                {
                    cbEnableSSL.Checked = false;
                }

                XmlNode port = doc.ChildNodes[0].SelectSingleNode("Port");
                if (port != null)
                {
                    tbPort.Text = port.InnerText;
                }

                XmlNode nodeSMTP = doc.ChildNodes[0].SelectSingleNode("SMTP");
                if (nodeSMTP != null)
                {
                    txtSMTP.Text = nodeSMTP.InnerText;
                }

                XmlNode nodeSender = doc.ChildNodes[0].SelectSingleNode("Sender");
                if (nodeSender != null)
                {
                    chkSender.Checked = Convert.ToBoolean(nodeSender.InnerText);
                }
                else
                {
                    chkSender.Checked = true;
                }

                XmlNode nodeFlowName = doc.ChildNodes[0].SelectSingleNode("FlowName");
                if (nodeFlowName != null)
                {
                    chkFlowName.Checked = Convert.ToBoolean(nodeFlowName.InnerText);
                }
                else
                {
                    chkFlowName.Checked = true;
                }

                XmlNode nodeActivityName = doc.ChildNodes[0].SelectSingleNode("ActivityName");
                if (nodeActivityName != null)
                {
                    chkActivityName.Checked = Convert.ToBoolean(nodeActivityName.InnerText);
                }
                else
                {
                    chkActivityName.Checked = true;
                }

                XmlNode nodeContent = doc.ChildNodes[0].SelectSingleNode("Content");
                if (nodeContent != null)
                {
                    chkContent.Checked = Convert.ToBoolean(nodeContent.InnerText);
                }
                else
                {
                    chkContent.Checked = true;
                }

                XmlNode nodeDescription = doc.ChildNodes[0].SelectSingleNode("Description");
                if (nodeDescription != null)
                {
                    chkDescription.Checked = Convert.ToBoolean(nodeDescription.InnerText);
                }
                else
                {
                    chkDescription.Checked = true;
                }

                XmlNode nodeDateTime = doc.ChildNodes[0].SelectSingleNode("DateTime");
                if (nodeDateTime != null)
                {
                    chkDateTime.Checked = Convert.ToBoolean(nodeDateTime.InnerText);
                }
                else
                {
                    chkDateTime.Checked = true;
                }

                XmlNode nodeComment = doc.ChildNodes[0].SelectSingleNode("Comment");
                if (nodeComment != null)
                {
                    chkComment.Checked = Convert.ToBoolean(nodeComment.InnerText);
                }
                else
                {
                    chkComment.Checked = true;
                }

                XmlNode nodeHyperLink = doc.ChildNodes[0].SelectSingleNode("HyperLink");
                if (nodeHyperLink != null)
                {
                    chkHyperLink.Checked = Convert.ToBoolean(nodeHyperLink.InnerText);
                }
                else
                {
                    chkHyperLink.Checked = true;
                }

                XmlNode nodeActivityDescription = doc.ChildNodes[0].SelectSingleNode("ActivityDescription");
                if (nodeActivityDescription != null)
                {
                    checkBoxActivityDescription.Checked = Convert.ToBoolean(nodeActivityDescription.InnerText);
                }
                else
                {
                    checkBoxActivityDescription.Checked = false;
                } 

                XmlNode nodeApproveButton = doc.ChildNodes[0].SelectSingleNode("ApproveButton");
                if (nodeApproveButton != null)
                {
                    checkBoxApproveButton.Checked = Convert.ToBoolean(nodeApproveButton.InnerText);
                }
                else
                {
                    checkBoxApproveButton.Checked = false;
                }

                XmlNode nodeReturnButton = doc.ChildNodes[0].SelectSingleNode("ReturnButton");
                if (nodeReturnButton != null)
                {
                    checkBoxReturnButton.Checked = Convert.ToBoolean(nodeReturnButton.InnerText);
                }
                else
                {
                    checkBoxReturnButton.Checked = false;
                }

                XmlNode nodeRejectButton = doc.ChildNodes[0].SelectSingleNode("RejectButton");
                if (nodeRejectButton != null)
                {
                    checkBoxRejectButton.Checked = Convert.ToBoolean(nodeRejectButton.InnerText);
                }
                else
                {
                    checkBoxRejectButton.Checked = false;
                }

                //-----------------------------------------------------------------

                XmlNode nodeSender2 = doc.ChildNodes[0].SelectSingleNode("Sender2");
                if (nodeSender2 != null)
                {
                    chkSender2.Checked = Convert.ToBoolean(nodeSender2.InnerText);
                }
                else
                {
                    chkSender2.Checked = true;
                }

                XmlNode nodeFlowName2 = doc.ChildNodes[0].SelectSingleNode("FlowName2");
                if (nodeFlowName2 != null)
                {
                    chkFlowName2.Checked = Convert.ToBoolean(nodeFlowName2.InnerText);
                }
                else
                {
                    chkFlowName2.Checked = true;
                }

                XmlNode nodeActivityName2 = doc.ChildNodes[0].SelectSingleNode("ActivityName2");
                if (nodeActivityName2 != null)
                {
                    chkActivityName2.Checked = Convert.ToBoolean(nodeActivityName2.InnerText);
                }
                else
                {
                    chkActivityName2.Checked = true;
                }

                XmlNode nodeDescription2 = doc.ChildNodes[0].SelectSingleNode("Description2");
                if (nodeSender2 != null)
                {
                    chkDescription2.Checked = Convert.ToBoolean(nodeDescription2.InnerText);
                }
                else
                {
                    chkDescription2.Checked = true;
                }

                XmlNode nodeContent2 = doc.ChildNodes[0].SelectSingleNode("Content2");
                if (nodeContent2 != null)
                {
                    chkContent2.Checked = Convert.ToBoolean(nodeContent2.InnerText);
                }
                else
                {
                    chkContent2.Checked = true;
                }

                XmlNode nodesms = doc.ChildNodes[0].SelectSingleNode("SMS");
                if (nodesms != null)
                {
                    tbSMS.Text = nodesms.InnerText;
                }

            }
            if (File.Exists("WorkflowPush.xml"))
            {
                var doc = new XmlDocument();
                doc.Load("WorkflowPush.xml");
                var controls = new List<Control>();
                for (int i = 0; i < tabPage2.Controls.Count; i++)
                {
                    controls.Add(tabPage2.Controls[i]);
                }
                for (int i = 0; i < groupBox3.Controls.Count; i++)
                {
                    controls.Add(groupBox3.Controls[i]);
                }
                for (int i = 0; i < groupBox4.Controls.Count; i++)
                {
                    controls.Add(groupBox4.Controls[i]);
                }

                for (int i = 0; i < controls.Count; i++)
                {
                    var control = controls[i];
                    if (control is CheckBox)
                    { 
                        if(control.Tag != null)
                        {
                            var property = (string)control.Tag;
                            var node = doc.ChildNodes[0].SelectSingleNode(property);
                            if (node != null)
                            {
                                (control as CheckBox).Checked = Convert.ToBoolean(node.InnerText);
                            }
                            else
                            {
                                (control as CheckBox).Checked = true;
                            }
                        }
                    }
                    else if (control is TextBox)
                    {
                        if (control.Tag != null)
                        {
                            var property = (string)control.Tag;
                            var node = doc.ChildNodes[0].SelectSingleNode(property);
                            if (node != null)
                            {
                                (control as TextBox).Text =node.InnerText;
                            }
                         
                        }
                    }
                }
                var nodePushService = doc.ChildNodes[0].SelectSingleNode("PushService");
                if (nodePushService != null)
                {
                    textBoxPushService.Text = nodePushService.InnerText;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                bool active = chkActive.Checked;
                string email = txtEmail.Text.Trim();
                //A6-Sensitive Data Exposure
                //string password = txtPassword.Text.Trim();
                string smtp = txtSMTP.Text.Trim();
                bool sender1 = chkSender.Checked;
                bool flowName = chkFlowName.Checked;
                bool activityName = chkActivityName.Checked;
                bool content = chkContent.Checked;
                bool description = chkDescription.Checked;
                bool dateTime = chkDateTime.Checked;
                bool comment = chkComment.Checked;
                bool hyperLink = chkHyperLink.Checked;

                bool sender2 = chkSender2.Checked;
                bool flowName2 = chkFlowName2.Checked;
                bool activityName2 = chkActivityName2.Checked;
                bool description2 = chkDescription2.Checked;
                bool content2 = chkContent2.Checked;

                bool activitydescription = checkBoxActivityDescription.Checked;

                bool approveButton = checkBoxApproveButton.Checked;
                bool returnButton = checkBoxReturnButton.Checked;
                bool rejectButton = checkBoxRejectButton.Checked;

                string sms = tbSMS.Text;

                bool enableSSL = cbEnableSSL.Checked;
                string port = tbPort.Text.Trim();

                XmlDocument doc = new XmlDocument();
                XmlNode nodeRoot = doc.CreateElement("Root");
                doc.AppendChild(nodeRoot);

                XmlNode nodeActive = doc.CreateElement("Active");
                nodeActive.InnerText = active.ToString();
                nodeRoot.AppendChild(nodeActive);

                XmlNode nodeEmail = doc.CreateElement("Email");
                nodeEmail.InnerText = email;
                nodeRoot.AppendChild(nodeEmail);

                XmlNode nodePassword = doc.CreateElement("Password");
                //A6-Sensitive Data Exposure
                //nodePassword.InnerText = GetPwdString(password);
                nodePassword.InnerText = GetPwdString( txtPassword.Text.Trim());
                nodeRoot.AppendChild(nodePassword);

                XmlNode nodeSLL = doc.CreateElement("EnableSSL");
                nodeSLL.InnerText = enableSSL.ToString() ;
                nodeRoot.AppendChild(nodeSLL);

                XmlNode nodePort = doc.CreateElement("Port");
                nodePort.InnerText = port;
                nodeRoot.AppendChild(nodePort);

                XmlNode nodeSmtp = doc.CreateElement("SMTP");
                nodeSmtp.InnerText = smtp;
                nodeRoot.AppendChild(nodeSmtp);

                XmlNode nodeSender = doc.CreateElement("Sender");
                nodeSender.InnerText = sender1.ToString();
                nodeRoot.AppendChild(nodeSender);

                XmlNode nodeFlowName = doc.CreateElement("FlowName");
                nodeFlowName.InnerText = flowName.ToString();
                nodeRoot.AppendChild(nodeFlowName);

                XmlNode nodeActivityName = doc.CreateElement("ActivityName");
                nodeActivityName.InnerText = activityName.ToString();
                nodeRoot.AppendChild(nodeActivityName);

                XmlNode nodeContent = doc.CreateElement("Content");
                nodeContent.InnerText = content.ToString();
                nodeRoot.AppendChild(nodeContent);

                XmlNode nodeDescription = doc.CreateElement("Description");
                nodeDescription.InnerText = description.ToString();
                nodeRoot.AppendChild(nodeDescription);

                XmlNode nodeDateTime = doc.CreateElement("DateTime");
                nodeDateTime.InnerText = dateTime.ToString();
                nodeRoot.AppendChild(nodeDateTime);

                XmlNode nodeComment = doc.CreateElement("Comment");
                nodeComment.InnerText = comment.ToString();
                nodeRoot.AppendChild(nodeComment);

                XmlNode nodeHyperLink = doc.CreateElement("HyperLink");
                nodeHyperLink.InnerText = hyperLink.ToString();
                nodeRoot.AppendChild(nodeHyperLink);

                XmlNode nodeActivityDescription = doc.CreateElement("ActivityDescription");
                nodeActivityDescription.InnerText = activitydescription.ToString();
                nodeRoot.AppendChild(nodeActivityDescription);

                XmlNode nodeApproveButton = doc.CreateElement("ApproveButton");
                nodeApproveButton.InnerText = approveButton.ToString();
                nodeRoot.AppendChild(nodeApproveButton);

                XmlNode nodeReturnButton = doc.CreateElement("ReturnButton");
                nodeReturnButton.InnerText = returnButton.ToString();
                nodeRoot.AppendChild(nodeReturnButton);

                XmlNode nodeRejctButton = doc.CreateElement("RejectButton");
                nodeRejctButton.InnerText = rejectButton.ToString();
                nodeRoot.AppendChild(nodeRejctButton);

                // ---------------------------------------------------------

                XmlNode nodeSender2 = doc.CreateElement("Sender2");
                nodeSender2.InnerText = sender2.ToString();
                nodeRoot.AppendChild(nodeSender2);

                XmlNode nodeFlowName2 = doc.CreateElement("FlowName2");
                nodeFlowName2.InnerText = flowName2.ToString();
                nodeRoot.AppendChild(nodeFlowName2);

                XmlNode nodeActivityName2 = doc.CreateElement("ActivityName2");
                nodeActivityName2.InnerText = activityName2.ToString();
                nodeRoot.AppendChild(nodeActivityName2);

                XmlNode nodeDescription2 = doc.CreateElement("Description2");
                nodeDescription2.InnerText = description2.ToString();
                nodeRoot.AppendChild(nodeDescription2);

                XmlNode nodeContent2 = doc.CreateElement("Content2");
                nodeContent2.InnerText = content2.ToString();
                nodeRoot.AppendChild(nodeContent2);

                XmlNode nodeSMS = doc.CreateElement("SMS");
                nodeSMS.InnerText = sms.ToString();
                nodeRoot.AppendChild(nodeSMS);


                try
                {
                    doc.Save("Workflow.xml");
                }
                catch(Exception ex)
                {
                    throw ex;
                }


                XmlDocument docPush = new XmlDocument();
                XmlNode nodeRootPush = docPush.CreateElement("Root");
                docPush.AppendChild(nodeRootPush);
                var controls = new List<Control>();
                for (int i = 0; i < tabPage2.Controls.Count; i++)
                {
                    controls.Add(tabPage2.Controls[i]);
                }
                for (int i = 0; i < groupBox3.Controls.Count; i++)
                {
                    controls.Add(groupBox3.Controls[i]);
                }
                for (int i = 0; i < groupBox4.Controls.Count; i++)
                {
                    controls.Add(groupBox4.Controls[i]);
                }

                for (int i = 0; i < controls.Count; i++)
                {
                    var control = controls[i];
                    if (control is CheckBox)
                    {
                        if (control.Tag != null)
                        {
                            var property = (string)control.Tag;

                            XmlNode nodeProperty = docPush.CreateElement(property);
                            nodeProperty.InnerText = (control as CheckBox).Checked.ToString();
                            nodeRootPush.AppendChild(nodeProperty);
                        }
                    }
                    else if (control is TextBox)
                    {
                        if (control.Tag != null)
                        {
                            var property = (string)control.Tag;

                            XmlNode nodeProperty = docPush.CreateElement(property);
                            nodeProperty.InnerText = (control as TextBox).Text;
                            nodeRootPush.AppendChild(nodeProperty);
                        }
                    }
                }
                //XmlNode nodePushService = docPush.CreateElement("PushService");
                //nodePushService.InnerText = textBoxPushService.Text;
                //nodeRootPush.AppendChild(nodePushService);
             

                docPush.Save("WorkflowPush.xml");
            }

            this.Close();
        }

        private bool CheckInput()
        {
            if (txtEmail.Text != null || txtEmail.Text.Trim() != string.Empty)
            {
                if(!ValidateEmail(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Email's format(xxx@xxx.xx)");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please input email.");
                return false;
            }

            if (txtPassword.Text == null || txtPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please input password.");
                return false;
            }

            if (txtSMTP.Text == null || txtSMTP.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please input SMTP.");
                return false;
            }

            return true;
        }

        private string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        public static bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}