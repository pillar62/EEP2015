using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Srvtools;

namespace EEPManager
{
    public partial class frmErrorLogMaintenance : Form
    {
        public System.Drawing.Image x;

        public frmErrorLogMaintenance()
        {
            InitializeComponent();
        }

        private void Enlarge_Click(object sender, EventArgs e)
        {
            x = this.pictureBox1.Image;
            frmErrlogLargeImage frmEMIL = new frmErrlogLargeImage(x);
            frmEMIL.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime timeForm = new DateTime();
            timeForm = (DateTime)this.infoDateTimeFrom.Value;

            DateTime timeTo = new DateTime();
            timeTo = (DateTime)this.infoDateTimeTo.Value;
            timeTo = timeTo.AddDays(1);

            string t1 = timeForm.Year.ToString() + "-" + timeForm.Month.ToString() + "-" + timeForm.Day.ToString() + " "
                + timeForm.Hour.ToString() + ":" + timeForm.Minute.ToString() + ":" + timeForm.Second.ToString();
            string t2 = timeTo.Year.ToString() + "-" + timeTo.Month.ToString() + "-" + timeTo.Day.ToString() + " "
                + timeTo.Hour.ToString() + ":" + timeTo.Minute.ToString() + ":" + timeTo.Second.ToString();

            string strFilter = "";

            object[] param = new object[1];
            param[0] = CliUtils.fLoginDB;
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", param);
            string type = "";
            if (myRet != null && (int)myRet[0] == 0)
                type = myRet[1].ToString();

            if (type == "1")
            {
                strFilter = "ERRDATE >= '" + t1 + "' and ERRDATE <= '" + t2 + "'";
            }
            else if (type == "3")
            {
                strFilter = "ERRDATE >= to_date('" + t1 + "', 'yyyy-mm-dd hh24:mi:ss') and ERRDATE <= to_date('" + t2 + "', 'yyyy-mm-dd hh24:mi:ss')";
            }
            else if (type == "2")
            {
                strFilter = "ERRDATE >= '" + t1 + "' and ERRDATE <= '" + t2 + "'";
            }
            else if (type == "4")
            {
                strFilter = "ERRDATE >= '" + t1 + "' and ERRDATE <= '" + t2 + "'";
            }
            else if (type == "5")
            {
                strFilter = "ERRDATE >= '" + t1 + "' and ERRDATE <= '" + t2 + "'";
            }
            else if (type == "6")
            {
                t1 = timeForm.ToString("yyyyMMddHHmmss");
                t2 = timeTo.ToString("yyyyMMddHHmmss");

                strFilter = "ERRDATE >= to_Date('" + t1 + "',  '%Y%m%d%H%M%S') and ERRDATE <= to_Date('" + t2 + "',  '%Y%m%d%H%M%S')"; ;
            }

            this.infoDsErrorLog.SetWhere(strFilter);
        }

        private void dgvErrorLog_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataRow dr = null;
                dr = this.infoDsErrorLog.RealDataSet.Tables[0].Rows[e.RowIndex];
                object o = dr["ERRSCREEN"];
                if (o != null && o != DBNull.Value)
                {
                    byte[] errimage = (byte[])o;
                    int length = errimage.Length;

                    if (errimage.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream(errimage, 0, length);
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                    pictureBox1.Show();

                }
                else
                    pictureBox1.Image = null;

            }
            catch
            {
                return;
            }
        }

        private void frmErrorLogMaintenance_Load(object sender, EventArgs e)
        {
            DateTime dateTemp = DateTime.Today;
            infoDateTimeFrom.Text = dateTemp.AddDays(-1).ToString("yyyy/MM/dd hh:mm:ss");
            infoDateTimeTo.Text = dateTemp.ToString("yyyy/MM/dd hh:mm:ss");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFilter = null;
            switch (this.comboBox1.SelectedIndex)
            {
                case 1:
                    strFilter = "STATUS = 'E'";
                    this.infoDsErrorLog.SetWhere(strFilter);
                    break;
                case 2:
                    strFilter = "STATUS = 'P'";
                    this.infoDsErrorLog.SetWhere(strFilter);
                    break;
                case 3:
                    strFilter = "STATUS = 'W'";
                    this.infoDsErrorLog.SetWhere(strFilter);
                    break;
                case 4:
                    strFilter = "STATUS = 'F'";
                    this.infoDsErrorLog.SetWhere(strFilter);
                    break;
                default:
                    strFilter = "STATUS = 'E' or STATUS = 'F' or STATUS = 'P' or STATUS = 'W' or STATUS = 'F' ";
                    this.infoDsErrorLog.SetWhere(strFilter);
                    break;
            }
        }
   }
}
