using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Collections;
using System.Security.Cryptography;

namespace Srvtools
{
    public class QueryStringEncrypt
    {
        //private Page page;

        //public Page Page
        //{
        //    get { return page; }
        //}

        public static string EncryptCode
        {
            get
            {
                return (string)HttpContext.Current.Session["EncryptCode"];
            }
            set
            {
                HttpContext.Current.Session["EncryptCode"] = value;
            }
        }

        private static void InitCode()
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            if (EncryptCode == null)
            {
                int codelen = rd.Next(4, 9);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < codelen; i++)
                {
                    builder.Append((char)((int)'A' + rd.Next(0, 26)));
                }
                EncryptCode = builder.ToString();
            }
        }

        public static string Encrypt(string basestring)
        {
            StringBuilder encryptstring = new StringBuilder(basestring);
            encryptstring.AppendFormat("&Info={0}", EncryptBase(HttpUtility.UrlDecode(basestring)));
            return encryptstring.ToString();
        }

        private static string EncryptBase(string basestring)
        {
            InitCode();
            basestring = basestring.Replace("'", "").Replace("\\", "");
            string codestring = string.Format("{0}{1}", basestring, EncryptCode);
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            provider.Initialize();
            byte[] hash = provider.ComputeHash(Encoding.UTF8.GetBytes(codestring));
            StringBuilder encryptstring = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                encryptstring.AppendFormat("{0:X2}", hash[i]);
            }
            return encryptstring.ToString();
        }

        public static void Check(Page page, string[] excludeparam)
        {
            Check(page, excludeparam, true);
        }

        public static void Check(Page page, string[] excludeparam, bool dialog)
        {
            string code = EncryptCode;
            if (code == null)
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            }
            string info = page.Request.QueryString["Info"];
            if (info == null || info.Length != 32)
            {
                throw new PageAuthorityException("InfoCode invalid");
            }
            string basestring = page.Request.QueryString.ToString();
            basestring = basestring.Replace(string.Format("&Info={0}", info), string.Empty);
            basestring = HttpUtility.UrlDecode(basestring);
            if (excludeparam != null)
            {
                foreach (string str in excludeparam)
	            {
            		string param = page.Request.QueryString[str.TrimStart('&')];
                    if (param != null)
                    {
                        basestring = basestring.Replace(string.Format("{0}={1}", str, param), string.Empty);
                    }
	            }
            }
            if (dialog)
            {
                basestring = basestring.Replace("'", "\\'");
            }
            if (!EncryptBase(basestring).Equals(info))
            {
                throw new PageAuthorityException("QueryString invalid");
            }
        }


        //public static int EncryptLength
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["EncryptLength"] == null 
        //            ? 0 : (int)HttpContext.Current.Session["EncryptLength"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["EncryptLength"] = value;
        //    }
        //}
	
        //private string basestring = null;

        //public string BaseString
        //{
        //    get
        //    {
        //        if (basestring == null)
        //        {
        //            basestring = HttpUtility.UrlDecode(Decrpyt(Page.Request.QueryString["Infolight"]));
        //        }
        //        return basestring;
        //    }
        //}

        //private Hashtable table = new Hashtable(); 

        //public string this[string key]
        //{
        //    get
        //    {
        //        if (table.Contains(key))
        //        {
        //            return (string)table[key];
        //        }
        //        if (BaseString != null)
        //        {
        //            string[] strs = BaseString.Split('&');
        //            foreach (string str in strs)
        //            {
        //                int index = str.IndexOf('=');
        //                if (index != -1)
        //                {
        //                    string strkey = str.Substring(0, index);
        //                    string strvalue = str.Length > index + 1? str.Substring(index + 1) : string.Empty;
        //                    table.Add(strkey, strvalue);
        //                }
        //            }
        //            return (string)table[key];
        //        }
        //        return null;
        //    }
        //}

        //public ICollection Keys
        //{
        //    get { return table.Keys; }
        //}
        
        //private static void InitCode()
        //{
        //    Random rd = new Random(DateTime.Now.Millisecond);
        //    if (EncryptCode == null)
        //    {
        //        int codelen = rd.Next(4, 9);
        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < codelen; i++)
        //        {
        //            builder.Append((char)((int)'A' + rd.Next(0, 26)));
        //        }
        //        EncryptCode = builder.ToString();
        //    }
        //    if (EncryptLength == 0)
        //    {
        //        EncryptLength = rd.Next(2, int.MaxValue);
        //    }
        //}

        //public static string Encrypt(string basestring)
        //{
        //    InitCode();
        //    //先全反过来
        //    string encryptcode = EncryptCode;
        //    List<int> encryptlist = new List<int>();
        //    int sum = 0;
        //    for (int i = 0; i < basestring.Length; i ++ )//加上掩码
        //    {
        //        int encryptchar = EncryptChar(basestring[basestring.Length - i - 1], encryptcode[i % encryptcode.Length]);
        //        sum += encryptchar;
        //        encryptlist.Add(encryptchar);
        //    }
        //    //补满为4的整数倍
        //    if (basestring.Length % 4 > 0)
        //    {
        //        for (int i = 0; i < 4 - basestring.Length % 4; i++)
        //        {
        //            encryptlist.Add(0);
        //        }
        //    }
        //    //每四个一起取出
        //    List<string> liststr = new List<string>();
        //    for (int i = 0; i < encryptlist.Count / 4; i++)
        //    {
        //        //7A 7B 7C 7D  01 02 03 04 -> FFFABCD 0001234
        //        StringBuilder builderlow = new StringBuilder();
        //        int inthigh = 0;
        //        for (int j = 0; j < 4; j++)
        //        {
        //            int high = encryptlist[4 * i + j] / 16;
        //            int low = encryptlist[4 * i + j] % 16;
        //            inthigh = inthigh << 3;
        //            inthigh += high;
        //            builderlow.Append(low.ToString("X1"));
        //        }
        //        liststr.Add(string.Format("{0:X3}{1}", inthigh, builderlow));
        //    }
        //    liststr.Add(string.Format("{0:X3}{1:X4}", sum % 128, basestring.Length));

        //    encryptlist.Clear();
        //    StringBuilder builder = new StringBuilder();
        //    int reclength = EncryptLength % liststr.Count;
        //    if (reclength == 0)
        //    {
        //        reclength = liststr.Count;
        //    }
        //    int count = liststr.Count / reclength;
        //    int remain = liststr.Count % reclength;
        //    for (int i = 0; i < count; i++) // FFFABCD 0001234 -> F0F0F0A1B2C3D4
        //    {
        //        for (int j = 0; j < 7; j++)
        //        {
        //            for (int k = 0; k < reclength; k++)
        //            {
        //                builder.Append(liststr[i * reclength + k][j]);
        //            }
        //        }     
        //    }
        //    if (remain > 0)
        //    {
        //        for (int j = 0; j < 7; j++)
        //        {
        //            for (int k = 0; k < remain; k++)
        //            {
        //                builder.Append(liststr[count * reclength + k][j]);
        //            }
        //        }     
        //    }
        //    return builder.ToString();
        //}

        //private static int EncryptChar(char basechar, char codechar)
        //{
        //    return ((int)basechar + (int)codechar) % 128;
        //}

        //private string Decrpyt(string encryptstring)
        //{
        //    if (encryptstring != null && EncryptCode != null && EncryptLength != 0)
        //    {
        //        if (encryptstring.Length % 7 != 0)
        //        {
        //            throw new FormatException();
        //        }
        //        List<string> liststr = new List<string>();
        //        int listcount = encryptstring.Length / 7;
        //        int reclength = EncryptLength % listcount;
        //        if (reclength == 0)
        //        {
        //            reclength = listcount;
        //        }
        //        int count = listcount / reclength;
        //        int remain = listcount % reclength;
        //        for (int i = 0; i < count; i++)
        //        {
        //            for (int j = 0; j < reclength; j++)
        //            {
        //                StringBuilder builderpart = new StringBuilder();
        //                for (int k = 0; k < 7; k++)
        //                {
        //                    builderpart.Append(encryptstring[i * reclength * 7 + k * reclength + j]);  
        //                }
        //                liststr.Add(builderpart.ToString());
        //            }
        //        }
        //        for (int j = 0; j < remain; j++)
        //        {
        //            StringBuilder builderpart = new StringBuilder();
        //            for (int k = 0; k < 7; k++)
        //            {
        //                builderpart.Append(encryptstring[count * reclength * 7 + k * remain + j]);
        //            }
        //            liststr.Add(builderpart.ToString());
        //        }
        //        List<int> encryptlist = new List<int>();
        //        int sum = 0;
        //        string check = liststr[liststr.Count - 1];
        //        int checksum = int.Parse(check.Substring(0,3), System.Globalization.NumberStyles.HexNumber);
        //        int checklength = int.Parse(check.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
        //        for (int i = 0; i < liststr.Count - 1; i++)
        //        {
        //            int inthigh = int.Parse(liststr[i].Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
        //            for (int j = 0; j < 4; j++)
        //            {
        //                int high = (inthigh >> 3 * (3 - j)) % 16;
        //                int low = int.Parse(liststr[i].Substring(3 + j, 1), System.Globalization.NumberStyles.HexNumber);
        //                int encryptchar = high * 16 + low;
        //                if (i * 4 + j < checklength)
        //                {
        //                    encryptlist.Add(encryptchar);
        //                    sum += encryptchar;
        //                }
        //            }
        //        }
        //        if (sum % 128 != checksum)
        //        {
        //            throw new FormatException();
        //        }
        //        liststr.Clear();
        //        StringBuilder builder = new StringBuilder();
        //        string encryptcode = EncryptCode;
        //        for (int i = 0; i < encryptlist.Count; i++)
        //        {
        //            builder.Insert(0, DecryptChar(encryptlist[i], encryptcode[i % encryptcode.Length]));
        //        }
        //        return builder.ToString();
        //    }
        //    return null;
        //}

        //private char DecryptChar(int encryptchar, char codechar)
        //{
        //    return (char)((encryptchar - (int)codechar + 128) % 128);
        //}
    }

    public class PageAuthorityException : Exception
    {
        public PageAuthorityException() : base() { }

        public PageAuthorityException(string message) : base(message) { }

        public PageAuthorityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
