using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1032
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

        //������
        public object[] smRT10321(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmdRT10321.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINEdrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "���w�@�o�ɡA���i�������u��C" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "�w���׮ɡA���i�������u��C" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "")
                {
                    return new object[] { 0, "�w��������u��ɡA���i���а���C" };
                }
            }

            //�]�w��J�Ѽƪ���
            try
            {
                cmdRT10321.InfoParameters[0].Value = sdata[0];
                cmdRT10321.InfoParameters[1].Value = sdata[1];
                cmdRT10321.InfoParameters[2].Value = sdata[2];
                cmdRT10321.InfoParameters[2].Value = sdata[3];
                /*���o�έp�����G�A�ñN���G��^*/
                double ii = cmdRT10321.ExecuteNonQuery();
                return new object[] { 0, "�D�u�M�u���������u�榨�\" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����M�u���������u��@�~,���~�T��" + ex };
            }
        }

        //�M�u����
        public object[] smRT10322(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmdRT10322.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "���M�u��Ƥw�@�o�ɡA���i���סC" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "���M�u��Ƥw���׮ɡA���i���е��סC" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() == ""|| RSXX.Tables[0].Rows[0]["SNDWORKDAT"].ToString() == "")
                {
                    return new object[] { 0, "���M�u��Ʃ|�����ͩ�����u��ɡA���i���סC" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "" || RSXX.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "���M�u��Ƥ�������u��|�����׮ɡA���i���סC" };
                }
            }

            //�]�w��J�Ѽƪ���
            try
            {
                cmdRT10322.InfoParameters[0].Value = sdata[0];
                cmdRT10322.InfoParameters[1].Value = sdata[1];
                cmdRT10322.InfoParameters[2].Value = sdata[2];
                cmdRT10322.InfoParameters[2].Value = sdata[3];
                /*���o�έp�����G�A�ñN���G��^*/
                double ii = cmdRT10322.ExecuteNonQuery();
                return new object[] { 0, "�D�u�M�u���u���צ��\" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����D�u�M�u���u���ק@�~,���~�T��" + ex };
            }
        }

        //�@�@�@�o
        public object[] smRT10323(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmdRT10323.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "���M�u��Ƥw�@�o�ɡA���i���Ƨ@�o�C" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "")
                {
                    return new object[] { 0, "���M�u��Ƥw���ͩ�����u��ɡA���i�@�o�C" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "���M�u��Ƥw���׮ɡA���i�@�o�C" };
                }
            }

            //�]�w��J�Ѽƪ���
            try
            {
                cmdRT10323.InfoParameters[0].Value = sdata[0];
                cmdRT10323.InfoParameters[1].Value = sdata[1];
                cmdRT10323.InfoParameters[2].Value = sdata[2];
                cmdRT10323.InfoParameters[3].Value = sdata[3];
                /*���o�έp�����G�A�ñN���G��^*/
                double ii = cmdRT10323.ExecuteNonQuery();
                return new object[] { 0, "�D�u�M�u��Ƨ@�o���\�C" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����D�u�M�u��Ƨ@�o�@�~,���~�T��" + ex };
            }
        }

        //�D�u����@�o����
        public object[] smRT10324(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //�}�Ҹ�Ƴs��
            IDbConnection conn = cmdRT10324.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "���D�u�M�u��Ʃ|���@�o�A���i����C" };
                }
            }

            //�]�w��J�Ѽƪ���
            try
            {
                cmdRT10324.InfoParameters[0].Value = sdata[0];
                cmdRT10324.InfoParameters[1].Value = sdata[1];
                cmdRT10324.InfoParameters[2].Value = sdata[2];
                cmdRT10324.InfoParameters[3].Value = sdata[3];
                /*���o�έp�����G�A�ñN���G��^*/
                double ii = cmdRT10324.ExecuteNonQuery();
                return new object[] { 0, "�D�u�M�u��Ƨ@�o���ন�\" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "�L�k����D�u�M�u��Ƨ@�o����@�~,���~�T���G" + ex };
            }
        }
    }
}