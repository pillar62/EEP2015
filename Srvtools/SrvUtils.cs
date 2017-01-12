using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Srvtools
{
	public class SrvUtils
	{
        private static int remotePort;
        /// <summary>
        /// 当前Server使用的端口号
        /// </summary>
        public static int RemotePort
        {
            get { return remotePort; }
            set { remotePort = value; }
        }

        public static UserInfo[] GetUsersInfos()
        {
            return SrvGL.GetUsersInfos();
        }

        static public object[]GetValue(string str, DataModule dm)
        {
            string strval = "";
            Char[] cs = str.ToCharArray();
            if (cs.Length == 0)
            {
                strval = "";
                return new object[] { 0, strval };
            }
            if (cs[0].Equals('\\'))
            {
                if (cs.Length > 1)
                {
                    strval = str.Substring(1);
                }
                else
                {
                    strval = "";
                }

                return new object[] { 0, strval };
            }
            else if (cs[0].Equals('_'))
            {
                switch (str.ToLower())
                {
                    case "_usercode": strval = dm.GetClientInfo(ClientInfoType.LoginUser).ToString(); break;
                    case "_username": 
                        {
                            strval = dm.GetClientInfo(ClientInfoType.LoginUser).ToString();
                            UserInfo info = SrvGL.GetUsersInfo(strval.ToLower());
                            strval = (info == null) ? string.Empty : info.UserName;
                            break;
                        }
                    case "_solution": strval = dm.GetClientInfo(ClientInfoType.CurrentProject).ToString();break;
                    case "_database": strval = dm.GetClientInfo(ClientInfoType.LoginDB).ToString();break;
                    case "_sitecode": strval = dm.GetClientInfo(ClientInfoType.SiteCode).ToString();break;
                    case "_ipaddress": strval = dm.GetClientInfo(ClientInfoType.ComputerIp).ToString();break;
                    case "_language": strval = dm.GetClientInfo(ClientInfoType.ClientLang).ToString();break;
                    case "_today": strval = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day; break;
                    case "_servertoday": strval = DateTime.Now.ToShortDateString();break;
                    case "_sysdate": strval = DateTime.Now.ToString(); break; 
                    case "_firstday":
                        { 
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(1 - day);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastday":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(1 - day);
                            retday = retday.AddMonths(1);
                            retday = retday.AddDays(-1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_firstdaylm":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(1 - day);
                            retday = retday.AddMonths(-1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastdaylm":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(- day);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_firstdayty":
                        {
                            int year = DateTime.Now.Year;
                            DateTime retday = new DateTime(year, 1, 1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastdayty":
                        {
                            int year = DateTime.Now.Year;
                            DateTime retday = new DateTime(year, 12, 31);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_firstdayly":
                        {
                            int year = DateTime.Now.Year - 1;
                            DateTime retday = new DateTime(year, 1, 1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastdayly":
                        {
                            int year = DateTime.Now.Year - 1;
                            DateTime retday = new DateTime(year, 12, 31);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    default: strval = "";break;
                }
                return new object[] { 0, strval};
            }
            else
            {
                return new object[] { 1 };
            }

        }
    }
}


