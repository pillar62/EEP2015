using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EEPManager
{
    public partial class MasterIPConfigForm : Form
    {
        public MasterIPConfigForm()
        {
            InitializeComponent();
        }

        private void MasterIPConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Data Validation 
            string errString = "";
            // Ip Address
            bool ipError = false;

                string[] ipSlice = this.txtMasterServerIP.Text.Split('.');
                if (ipSlice.GetLength(0) != 4)
                {
                    ipError = true;
                }
                else
                {
                    for (int i = 0; i < ipSlice.GetLength(0); ++i)
                    {
                        if (ipSlice[i].Length > 0 && ipSlice[i].Length < 4)
                        {
                            try
                            {
                                int num = int.Parse(ipSlice[i]);
                                if (num < 0 || num > 255)
                                {
                                    ipError = true;
                                    break;
                                }
                            }
                            catch
                            {
                                ipError = true;
                                break;
                            }
                        }
                        else
                        {
                            ipError = true;
                            break;
                        }
                    }
                }

                if (ipError)
                {
                    errString += "Master Ip Address is not valid.\n\r";
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterIPConfigForm_Load(object sender, EventArgs e)
        {

        }
    }
}