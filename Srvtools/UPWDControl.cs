using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(UPWDControl), "Resources.UPWDControl.png")]
    public partial class UPWDControl : UserControl
    {
        private SYS_LANGUAGE language;
        public bool isOK = false;

        public UPWDControl()
        {
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            InitializeComponent();
        }

        public UPWDControl(IContainer container)
        {
            container.Add(this);

            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            language = CliUtils.fClientLang;
            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "UPWDControl", "NewPasswordErrorMessage");
                MessageBox.Show(message);
                txtConfirmPwd.Text = "";
                txtNewPwd.Text = "";
                return;
            }

            if (txtNewPwd.Text.Length >= CliUtils.fPassWordMinSize && txtNewPwd.Text.Length <= CliUtils.fPassWordMaxSize)
            {
                if (CliUtils.fPassWordCharNum)
                {
                    int x = 0, y = 0;
                    for (int i = 0; i < txtNewPwd.Text.Length; i++)
                    {
                        if (!char.IsLetterOrDigit(txtNewPwd.Text, i))
                        {
                            string message = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordCharCheck");
                            MessageBox.Show(message);
                            txtNewPwd.Focus();
                            return;
                        }
                        else if (char.IsLetter(txtNewPwd.Text, i))
                        {
                            x++;
                        }
                        else if (char.IsNumber(txtNewPwd.Text, i))
                        {
                            y++;
                        }
                    }
                    if (x == 0 || y == 0)
                    {
                        string message = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordCharNum");
                        MessageBox.Show(message);
                        return;
                    }
                }

                string sParam = txtUserID.Text + ':' + txtOldPwd.Text + ':' + txtNewPwd.Text + ":" + CliUtils.fLoginDB;
                object[] myRet = CliUtils.CallMethod("GLModule", "ChangePassword", new object[] { (object)sParam });
                if (myRet[1].ToString() == "E")
                {
                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "UPWDControl", "OldPasswordErrorMessage");
                    MessageBox.Show(message);
                }
                else if (myRet[1].ToString() == "O")
                {
                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "UPWDControl", "ChangeSucceed");
                    MessageBox.Show(message);
                    isOK = true;
                    (this.Parent as Form).Close();
                }
            }
            else
            {
                String message = String.Format(SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordLength"), CliUtils.fPassWordMinSize, CliUtils.fPassWordMaxSize);
                MessageBox.Show(message);
            }
        }

        private void UPWDControl_Load(object sender, EventArgs e)
        {
            language = CliUtils.fClientLang;
            string message = SysMsg.GetSystemMessage(language, "Srvtools", "UPWDControl", "LabelName");
            string[] user = message.Split(';');
            lblUserID.Text = user[0];
            lblUserName.Text = user[1];
            lblOldPwd.Text = user[2];
            lblNewPwd.Text = user[3];
            lblConfirmPwd.Text = user[4];
            if (CliUtils.fLoginUser != null && CliUtils.fLoginUser.Length != 0)
            {
                string userName = null;
                txtUserID.Text = CliUtils.fLoginUser;

                object[] param = new object[1];
                param[0] = CliUtils.fLoginUser;
                //modified by lily 2007/10/24 
                //object[] myRet = CliUtils.CallMethod("GLModule", "GetUserName", param);
                //if (myRet[0].ToString() != "1" || myRet[0] != null)
                //    userName = (string)myRet[1];
                userName = CliUtils.fUserName;
                //modified by lily 2007/10/24 
                txtUserName.Text = userName;
            }
        }
    }
}
