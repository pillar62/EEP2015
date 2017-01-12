using System;
using System.DirectoryServices;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;

namespace Srvtools
{
    //修改adclass 2007.9.3
    public class ADClass
    {
        public string ADPath
        {
            get;
            set;
        }
        public string ADUser
        {
            get;
            set;
        }
        public string ADPassword
        {
            get;
            set;
        }

        public ADClass()
        { 

        }

        public bool TestDirecoryOjbect()
        {
            DirectoryEntry de = GetDirectoryObject();
            try
            {
                string name = de.Name;
                return true;
            }
            catch
            {
                return false;
            }
        
        }

        private string GetProperty(SearchResult result, string propertyname)
        {
            if (result.Properties.Contains(propertyname) && result.Properties[propertyname].Count > 0)
            {
                return result.Properties[propertyname][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetProperty(DirectoryEntry oDE, string PropertyName)
        {
            if (oDE.Properties.Contains(PropertyName) && oDE.Properties[PropertyName].Count > 0)
            {
                return oDE.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public List<ADGroup> GetADUserForGroup()
        {
            List<ADGroup> lstGroup = new List<ADGroup>();
            DirectoryEntry de = GetDirectoryObject();

            //create instance fo the direcory searcher
            DirectorySearcher deSearch = new DirectorySearcher();

            //set the search filter
            deSearch.SearchRoot = de;
            //deSearch.PropertiesToLoad.Add("cn");
            deSearch.Filter = "objectCategory=group";

            //get the group result
            SearchResultCollection results = deSearch.FindAll();

            foreach (SearchResult Result in results)
            {
                string groupid = GetProperty(Result, "name");
                string description = GetProperty(Result, "description");
                ADGroup group = new ADGroup(groupid, description);

                foreach (object member in Result.Properties["member"])
                {
                    DirectoryEntry deUser = new DirectoryEntry(ADPath + "/" + member.ToString(), ADUser, ADPassword, AuthenticationTypes.Secure);
                    try
                    {
                        if (deUser.SchemaEntry.Name == "user")
                        {
                            string strUserAccountControl = GetProperty(deUser, "userAccountControl");
                            if (strUserAccountControl.Length > 0)
                            {
                                int userAccountControl = Convert.ToInt32(strUserAccountControl);
                                if (IsAccountActive(userAccountControl))//active users
                                {
                                    string userid = GetProperty(deUser, "sAMAccountName");
                                    group.Users.Add(userid);
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        deUser.Close();
                    }
                }
                lstGroup.Add(group);
            }

            de.Close();

            return lstGroup;
        }

        public List<ADUser> GetADAllUser()
        {
            List<ADUser> lstUser = new List<ADUser>();
            DirectoryEntry de = GetDirectoryObject();

            //create instance fo the direcory searcher
            DirectorySearcher deSearch = new DirectorySearcher();

            //set the search filter
            deSearch.SearchRoot = de;
            deSearch.Filter = "objectCategory=user";

            //find the first instance

            deSearch.PageSize = 1000;
            var results = SafeFindAll(deSearch);
            //SearchResultCollection results = deSearch.FindAll();

            foreach (SearchResult Result in results)
            {
                string strUserAccountControl = GetProperty(Result, "userAccountControl");
                if (strUserAccountControl.Length > 0)
                {
                    int userAccountControl = Convert.ToInt32(strUserAccountControl);
                    if (IsAccountActive(userAccountControl))//active users
                    {
                        string name = GetProperty(Result, "name");
                        string id = GetProperty(Result, "sAMAccountName");
                        string description = GetProperty(Result, "description");
                        string email = GetProperty(Result, "mail");
                        string createdate = GetProperty(Result, "whenCreated");
                        DateTime datecreate = DateTime.Today;
                        try
                        {
                            datecreate = Convert.ToDateTime(createdate);
                        }
                        catch { }
                        ADUser user = new ADUser(name, id, datecreate, email, description);
                        lstUser.Add(user);
                    }
                }
            }
            
            de.Close();
            return lstUser;
        }


        public IEnumerable<SearchResult> SafeFindAll(DirectorySearcher searcher)
        {
            using (SearchResultCollection results = searcher.FindAll())
            {
                foreach (SearchResult result in results)
                {
                    yield return result;
                }
            } // SearchResultCollection will be disposed here
        }

        public bool IsUserValid(string userid, string password)
        {
            try
            {
                //if the object can be created then return true
                DirectoryEntry deUser = GetUser(userid, password);
                deUser.Close();
                return true;
            }
            catch (Exception)
            {
                //otherwise return false
                return false;
            }
        }

        public bool IsAccountActive(int userAccountControl)
        {
            int flagExists = userAccountControl & 0x002;
            //if a match is found, then the disabled flag exists within the control flags
            if (flagExists > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DirectoryEntry GetUser(string userid, string password)
        {
            //create an instance of the DirectoryEntry
            DirectoryEntry de = GetDirectoryObject(userid, password);
 

            //create instance fo the direcory searcher
            DirectorySearcher deSearch = new DirectorySearcher();

            deSearch.SearchRoot = de;
            //set the search filter
            //deSearch.Filter = "(&(objectClass=user)(cn=" + UserName + "))";
            deSearch.Filter = "(&(objectCategory=user)(sAMAccountName=" + userid + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            //find the first instance
            SearchResult results = deSearch.FindOne();

            //if a match is found, then create directiry object and return, otherwise return Null
            if (results != null)
            {
                //create the user object based on the admin priv.
                de = new DirectoryEntry(results.Path, ADUser, ADPassword, AuthenticationTypes.Secure);
                return de;
            }
            else
            {
                return null;
            }
        }

        private DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry oDE;
            if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor <= 1)//xp
            {
                oDE = new DirectoryEntry(ADPath, ADPath + @"\" + ADUser, ADPassword, AuthenticationTypes.Secure);
            }
            else
            {
                oDE = new DirectoryEntry(ADPath, ADUser, ADPassword, AuthenticationTypes.Secure);
            }
            return oDE;
        }

        private DirectoryEntry GetDirectoryObject(string UserName, string Password)
        {
            DirectoryEntry oDE;
            if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor <= 1)//xp
            {
                oDE = new DirectoryEntry(ADPath, ADPath + @"\" + UserName, Password, AuthenticationTypes.Secure);
            }
            else
            {
                oDE = new DirectoryEntry(ADPath, UserName, Password, AuthenticationTypes.Secure);
            }
            return oDE;
        }
    }

    [Serializable]
    public class ADUser
    {
        public ADUser() : this(string.Empty, string.Empty,DateTime.Today, string.Empty, string.Empty) { }

        public ADUser(string name, string id, DateTime createdate, string email, string description)
        {
            _Name = name;
            _ID = id;
            _CreateDate = createdate;
            _Email = email;
            _Description = description;
        }

        private string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private DateTime _CreateDate;

        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
	
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
    }

    [Serializable]
    public class ADGroup        
    {
        public ADGroup() : this(string.Empty, string.Empty) { }

        public ADGroup(string id, string description)
        {
            _ID = id;
            _Description = description;
            _Users = new ArrayList();
        }

        private string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
	

        private ArrayList _Users;

        public ArrayList Users
        {
            get { return _Users; }
        }
    }
}
