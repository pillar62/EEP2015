using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT104
{
    public partial class Component : DataModule
    {
        public Component()
        {
            InitializeComponent();
        }

        public Component(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public string getFix()
        {
            return string.Format("E{0:yyMMdd}", DateTime.Now.Date);
        }

        //�Τ�@�o
        public object[] smRT104B(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmdRT104B.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = " + sdata[1] + " and CUSID ='" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "���Τ�w���u�A���i�@�o�C" };
                }
                if (ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "���Τ�w�@�o�A���i���а���C" };
                }
            }
            //�]�w��J�Ѽƪ���
            try
            {
                cmdRT104B.InfoParameters[0].Value = sdata[0];
                cmdRT104B.InfoParameters[1].Value = sdata[1];
                cmdRT104B.InfoParameters[2].Value = sdata[2];
                cmdRT104B.InfoParameters[3].Value = sdata[3];
                /*���o�έp�����G�A�ñN���G��^*/
                double ii = cmdRT104B.ExecuteNonQuery();
                return new object[] { 0, "�Τ�ӽЧ@�o���\" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����Τ�ӽЧ@�o�@�~,���~�T��" + ex };
            }
        }

        //�Τ�@�o���� 
        public object[] smRT104C(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmdRT104C.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = " + sdata[1] + " and CUSID ='" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "���Τ�|���@�o�A���i�@�o����C" };
                }
            }

            //�]�w��J�Ѽƪ���
            try
            {
                cmdRT104C.InfoParameters[0].Value = sdata[0];
                cmdRT104C.InfoParameters[1].Value = sdata[1];
                cmdRT104C.InfoParameters[2].Value = sdata[2];
                cmdRT104C.InfoParameters[3].Value = sdata[3];
                /*���o�έp�����G�A�ñN���G��^*/
                double ii = cmdRT104C.ExecuteNonQuery();
                return new object[] { 0, "�Τ�ӽЧ@�o���ন�\" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����Τ�ӽЧ@�o�@�~,���~�T��" + ex };
            }
        }

        public object[] smRT104FAR(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string COMTYPE;
            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmd.Connection;
            conn.Open();
            string ZIP, sKEY;
            string selectSql = " SELECT RTCtyTown.ZIP AS zip, RTLessorAVSCmtyH.COMTYPE  "
                             + " FROM RTLessorAVSCmtyH "
                             + " INNER JOIN RTCtyTown ON RTLessorAVSCmtyH.CUTID = RTCtyTown.CUTID AND RTLessorAVSCmtyH.TOWNSHIP = RTCtyTown.TOWNSHIP "
                             + " where RTLessorAVSCmtyH.comq1 = " + sdata[0];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ZIP = ds.Tables[0].Rows[0]["zip"].ToString();
                COMTYPE = ds.Tables[0].Rows[0]["COMTYPE"].ToString();
            }
            else
            {
                ZIP = "";
                COMTYPE = "";
            }

            string CMTYSEQ = sdata[0];
            CMTYSEQ = CMTYSEQ.PadLeft(4, '0'); //���Ͻs�����ɹs            

            selectSql = "select max(MEMBERID) AS MEMBERID from RTLessorAVSCust where MEMBERID like 'FBB_" + ZIP + CMTYSEQ + "%' ";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                var MEMBERID = ds.Tables[0].Rows[0]["MEMBERID"].ToString().Replace("FBB_" + ZIP + CMTYSEQ, "");
                if (MEMBERID == "")
                    sKEY = "FBB_" + ZIP + CMTYSEQ + "001";
                else
                {
                    MEMBERID = (int.Parse(MEMBERID) + 1).ToString();
                    sKEY = "FBB_" + ZIP + CMTYSEQ + MEMBERID.PadLeft(3, '0');

                }
            }
            else
            {
                sKEY = "FBB_" + ZIP + CMTYSEQ + "001";
            }
            //�]�w��J�Ѽƪ���
            try
            {
                if (COMTYPE == "B")
                    return new object[] { 0, sKEY };
                else
                    return new object[] { 0, "" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����Τ�ӽЧ@�o�@�~,���~�T��" + ex };
            }
        }

        public object[] smRT104GetLINEQ1(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string LINEQ1 = "";
            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmd.Connection;
            conn.Open();
            string selectSql = " SELECT LINEQ1 FROM RTLessorAVSCmtyLine"
                             + " WHERE COMQ1 = " + sdata[0];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                LINEQ1 = ds.Tables[0].Rows[0]["LINEQ1"].ToString();
            }
            else
            {
                LINEQ1 = "";
            }

            //�]�w��J�Ѽƪ���
            try
            {
                return new object[] { 0, LINEQ1 };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����Τ�ӽЧ@�o�@�~,���~�T��" + ex };
            }
        }

        private void ucRTLessorAVSCust_BeforeModify(object sender, UpdateComponentBeforeModifyEventArgs e)
        {
            //���o�O�Ҫ��Ǹ� �H�� ������
            string sgtserial = ucRTLessorAVSCust.GetFieldCurrentValue("GTSERIAL").ToString(); //�O�Ҫ��Ǹ�
            string sDOCKETDAT = ucRTLessorAVSCust.GetFieldCurrentValue("DOCKETDAT").ToString(); ; //������
            string sFINISHDAT = ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT").ToString();//���u��
            string sSTRBILLINGDAT = ucRTLessorAVSCust.GetFieldCurrentValue("STRBILLINGDAT").ToString(); //�}�l�p�O��
            string sGTMONEY = ucRTLessorAVSCust.GetFieldCurrentValue("GTMONEY").ToString(); //�O�Ҫ� ���B
            double dGTMONEY = 0;
            //�ק�H���B�ק��
            string sNow = DateTime.Now.Date.ToString("yyyyMMdd");
            string UUSR = this.GetClientInfo(ClientInfoType.LoginUser).ToString();
            ucRTLessorAVSCust.SetFieldValue("UDAT", DateTime.Now);
            ucRTLessorAVSCust.SetFieldValue("UUSR", UUSR);

            string ss = "";
            if (sFINISHDAT != "" && (sDOCKETDAT == "" || sSTRBILLINGDAT == ""))
            {
                DateTime dDT = Convert.ToDateTime(ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT"));
                ss = Convert.ToString(dDT.AddDays(7));

                //�P�_ �p�G���u�餣���ť� �B���u��άO�}�l�p�O�鬰�ť� �N�n�㧹�u��+�C�� �@�������� �� �}�l�p�O��
                if (sDOCKETDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("DOCKETDAT", ss);
                    sDOCKETDAT = ss;
                }

                if (sSTRBILLINGDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("STRBILLINGDAT", ss);
                }
            }

            //�P�_ �p�G�O�Ҫ����B���="" �N��0 ���M�N�ഫ���Ʀr
            if (sGTMONEY != "")
            {
                dGTMONEY = Convert.ToInt64(sGTMONEY);
            }
            else
            {
                dGTMONEY = 0;
            }

            //�P�_ �O�Ҫ��Ǹ���쬰�� �B�O�Ҫ������B �A�B�z
            if (sgtserial == "" && dGTMONEY > 0)
            {
                string ssql = " select isnull(max(gtserial),'') as maxgtserial from RTlessoravsCust where gtserial <> '' and substring(gtserial, 1,3) = 'AVS' ";

                //�}�Ҹ�Ƴs��
                IDbConnection conn = cmdRT104C.Connection;
                conn.Open();
                cmd.CommandText = ssql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds.Tables[0].Rows[0]["maxgtserial"].ToString();
                    if (ss1 == "")
                    {
                        ss = "AVS000000001";
                    }
                    else
                    {
                        ss = ss1.Substring(3, 9);
                        ss = (Convert.ToInt64(ss) + 1).ToString();
                        ss = "000000000" + ss;
                        ss = "AVS" + ss.Substring(ss.Length - 9, 9);
                    }
                }
                ucRTLessorAVSCust.SetFieldValue("GTSERIAL", ss);
            }
        }

        private void ucRTLessorAVSCust_BeforeInsert(object sender, UpdateComponentBeforeInsertEventArgs e)
        {
            //���o�O�Ҫ��Ǹ� �H�� ������
            string sgtserial = ucRTLessorAVSCust.GetFieldCurrentValue("GTSERIAL").ToString();
            string sDOCKETDAT = ucRTLessorAVSCust.GetFieldCurrentValue("DOCKETDAT").ToString();
            string sFINISHDAT = ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT").ToString();//���u��
            string sSTRBILLINGDAT = ucRTLessorAVSCust.GetFieldCurrentValue("STRBILLINGDAT").ToString(); //�}�l�p�O��
            string sGTMONEY = ucRTLessorAVSCust.GetFieldCurrentValue("GTMONEY").ToString(); //�O�Ҫ� ���B
            double dGTMONEY = 0;

            string ss = "";
            if (sFINISHDAT != "" && (sDOCKETDAT == "" || sSTRBILLINGDAT == ""))
            {
                DateTime dDT = Convert.ToDateTime(ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT"));
                ss = Convert.ToString(dDT.AddDays(7));

                //�P�_ �p�G���u�餣���ť� �B���u��άO�}�l�p�O�鬰�ť� �N�n�㧹�u��+�C�� �@�������� �� �}�l�p�O��
                if (sDOCKETDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("DOCKETDAT", ss);
                    sDOCKETDAT = ss;
                }

                if (sSTRBILLINGDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("STRBILLINGDAT", ss);
                }
            }

            //�P�_ �p�G�O�Ҫ����B���="" �N��0 ���M�N�ഫ���Ʀr
            if (sGTMONEY != "")
            {
                dGTMONEY = Convert.ToInt64(sGTMONEY);
            }
            else
            {
                dGTMONEY = 0;
            }

            //�P�_ �O�Ҫ��Ǹ���쬰�� �B�O�Ҫ������B �A�B�z
            if (sgtserial == "" && dGTMONEY > 0)
            {
                string ssql = " select isnull(max(gtserial),'') as maxgtserial from RTlessoravsCust where gtserial <> '' and substring(gtserial, 1,3) = 'AVS' ";

                //�}�Ҹ�Ƴs��
                IDbConnection conn = cmdRT104C.Connection;
                conn.Open();
                cmd.CommandText = ssql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds.Tables[0].Rows[0]["maxgtserial"].ToString();
                    if (ss1 == "")
                    {
                        ss = "AVS000000001";
                    }
                    else
                    {
                        ss = ss1.Substring(3, 9);
                        ss = (Convert.ToInt64(ss) + 1).ToString();
                        ss = "000000000" + ss;
                        ss = "AVS" + ss.Substring(ss.Length - 9, 9);
                    }
                }
                ucRTLessorAVSCust.SetFieldValue("GTSERIAL", ss);
            }
        }
    }
}
