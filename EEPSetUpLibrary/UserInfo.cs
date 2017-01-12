using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections;

namespace EEPSetUpLibrary
{
    /// <summary>
    /// 存放用户信息的类
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 创建一个用户信息类
        /// </summary>
        /// <param name="ip">用户的网络地址</param>
        public UserInfo(IPEndPoint ip)
        {
            _IP = ip;
            _ConnectTime = DateTime.Now;
            _FilesToDownLoad = new FileInfoCollection();
        }

        private IPEndPoint _IP;
        /// <summary>
        /// 获取用户的网络地址
        /// </summary>
        public IPEndPoint IP
        {
            get { return _IP;}
        }

        /// <summary>
        /// 获取用户的唯一标识号
        /// </summary>
        public int ID
        {
            get { return this.ToString().GetHashCode(); }
        }

        private DateTime _ConnectTime;
        /// <summary>
        /// 获取用户连接时的时间
        /// </summary>
        public DateTime ConnectTime
        {
            get { return _ConnectTime; }
        }

        private FileInfoCollection _FilesToDownLoad;
        /// <summary>
        /// 获取用户需要更新的文件集合
        /// </summary>
	    public FileInfoCollection FilesToDownLoad
	    {
		    get { return _FilesToDownLoad;}
            set { _FilesToDownLoad = value; }
	    }

        /// <summary>
        /// 获取当前实例的文字说明
        /// </summary>
        /// <returns>文件实例的文字说明</returns>
        public override string ToString()
        {
            return this.IP.ToString();
        }
    }


    /// <summary>
    /// 存放多个用户信息类的集合
    /// </summary>
    public class UserInfoCollection : Hashtable
    {        
        /// <summary>
        /// 获取集合中的某个用户信息实例
        /// </summary>
        /// <param name="key">要获取用户的标识号</param>
        /// <returns>获取的用户信息实例</returns>
        public new UserInfo this[object key]
        {
            get
            {
                return (UserInfo)base[key];
            }
            set
            {
                base[key] = value;
            }
        }
    }

}
