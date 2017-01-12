using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Srvtools
{
    public class SessionRequest
    {
        public SessionRequest(Page page)
        {
            _page = page;
        }

        private Page _page;
        public Page Page
        {
            get { return _page; }
        }

        const string REQUEST_NAME = "RequestID";


        private static bool enable = true;

        public static bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        public string this[string key]
        {
            get 
            {
                if (Enable)
                {
                    string sessionKey = Page.Request.QueryString[REQUEST_NAME];
                    string queryString = (string)Page.Session[sessionKey];
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        string[] list = queryString.Split('&');
                        foreach (string str in list)
                        {
                            if (str.StartsWith(string.Format("{0}=", key), StringComparison.OrdinalIgnoreCase))
                            {

                                if (str.Length > key.Length + 1)
                                {
                                    return HttpUtility.UrlDecode(str.Substring(key.Length + 1));
                                }
                                else
                                {
                                    return string.Empty;
                                }
                            }
                        }
                    }
                    return null;
                }
                else
                {
                    return Page.Request.QueryString[key];
                }  
            }
        }

        public string SetRequestValue(string value)
        {
            Guid id = Guid.NewGuid();
            string key = id.ToString("N");
            Page.Session[key] = value;
            return string.Format("{0}={1}", REQUEST_NAME, key);
        }

    }
}
