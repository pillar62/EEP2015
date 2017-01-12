using System;
using System.Collections.Generic;
using System.Text;

namespace EEPNetServer
{
    public class LoginBridge: ILogin
    {
        #region ILogin Members

        public bool Enabled
        {
            get { return false; }
        }

        public bool GetMenuRight(string userid, string password)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CheckUser(string userid, string password)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string[] GetAllGroups()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string[] GetAllUsers()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetDBConnection(string DBAlais)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string[] GetUserGroups(string userid, string password)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public  object GetUserInfo(string userid, string password, UserInfoType type)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
