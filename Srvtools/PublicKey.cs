using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Linq;

namespace Srvtools
{
    public static class PublicKey
    {
        public static readonly string SPLIT_STRING = "--";

        public static string GetPublicKey(string userId, string dataBase, string solution)
        {
            int i1 = userId.Length;
            int i2 = dataBase.Length;
            int i3 = solution.Length;

            string s = userId + i1.ToString() + dataBase + i2.ToString() + solution + i3.ToString() +
                (i1 * i2 * i3).ToString() + (i1 + i2 + i3).ToString();

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] pwdBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(s));
            byte[] result = md5.ComputeHash(pwdBytes);
            string ss = BitConverter.ToString(result);

            //return userId + "-" + dataBase + "-" + solution + "-" + ss.Replace("-", "");
            return userId + SPLIT_STRING + dataBase + SPLIT_STRING + solution + SPLIT_STRING + ss.Replace("-", "");
        }

        public static string GetPublicKey2(string userId, string dataBase, string solution, int language)
        {
            int i1 = userId.Length;
            int i2 = dataBase.Length;
            int i3 = solution.Length;

            string s = userId + i1.ToString() + dataBase + i2.ToString() + solution + i3.ToString() +
                (i1 * i2 * i3).ToString() + (i1 + i2 + i3).ToString();

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] pwdBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(s));
            byte[] result = md5.ComputeHash(pwdBytes);
            string ss = BitConverter.ToString(result);

            //return userId + "-" + dataBase + "-" + solution + "-" + language.ToString() + "-" + ss.Replace("-", "");
            return userId + SPLIT_STRING + dataBase + SPLIT_STRING + solution + SPLIT_STRING + language.ToString() + SPLIT_STRING + ss.Replace("-", "");
        }

        public static bool CheckPublicKey(string key)
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(key, SPLIT_STRING);
            //string[] ss = key.Split("-".ToCharArray());
            if (ss.Length == 4)
            {
                string userId = ss[0];
                string dataBase = ss[1];
                string solution = ss[2];

                string s1 = GetPublicKey(userId, dataBase, solution);
                if (key.Equals(s1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckPublicKey2(string key)
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(key, SPLIT_STRING);
            //string[] ss = key.Split("-".ToCharArray());
            if (ss.Length == 5)
            {
                string userId = ss[0];
                string dataBase = ss[1];
                string solution = ss[2];
                int language = int.Parse(ss[3]);

                string s1 = GetPublicKey2(userId, dataBase, solution, language);
                if (key.Equals(s1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 随便产生hash sn的key 防止破解
        /// </summary>
        private static byte[] hmacKey;
        private static byte[] HmacKey
        {
            get
            {
                if (hmacKey == null)
                {
                    var md5 = new MD5CryptoServiceProvider();
                    var ssoKey = string.IsNullOrEmpty(ServerConfig.SSOKey) ? DateTime.Now.ToString("yyyyMMddHHmmss") : ServerConfig.SSOKey;
                    hmacKey = md5.ComputeHash(Encoding.ASCII.GetBytes(ssoKey));
                }
                return hmacKey;
            }
        }

        public static string GetEncryptKey(string userId, string userName, string dataBase, string solution, string dataBaseType, string ipAddress)
        {
            var infos = new List<string>();
            infos.Add(userId.Replace("-", "|"));
            infos.Add(userName.Replace("-", "|"));
            infos.Add(dataBase);
            infos.Add(solution);
            infos.Add(ipAddress);
            infos.Add(dataBaseType);
            //加入时间
            infos.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            var info = Encoding.UTF8.GetBytes(string.Join("-", infos));

            var md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(info);

            var key = hash.Concat(info).ToArray();
            return Convert.ToBase64String(EncryptWithSN(key));
        }

        public static string[] CheckEncryptKey(string publickey)
        {
            var key = DecryptWithSN(Convert.FromBase64String(publickey));

            if (key.Length <= 16)
            {
                throw new Exception("PublishKey is invalid");
            }
            var hash = key.Take(16).ToArray();
            var info = key.Skip(16).ToArray();
            var md5 = new MD5CryptoServiceProvider();
            if (Convert.ToBase64String(md5.ComputeHash(info)) != Convert.ToBase64String(hash))
            {
                throw new Exception("PublishKey is invalid");
            }
            var infos = Encoding.UTF8.GetString(info).Split('-');
            if (infos.Length < 7)
            {
                throw new Exception("PublishKey is invalid");
            }
            var dateTime = DateTime.Now;
            if (!DateTime.TryParseExact(infos[6], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                throw new Exception("PublishKey is invalid");
            }
            var timeSpan = DateTime.Now - dateTime;
            if (timeSpan.TotalHours > (double)ServerConfig.SSOTimeOut)
            {
                throw new Exception("Timeout, relogon please");
            }
            if (infos[0] != null)
            {
                infos[0] = infos[0].Replace("|", "-");
            }
            if (infos[1] != null)
            {
                infos[1] = infos[1].Replace("|", "-");
            }
            return infos;
        }

        private static byte[] EncryptWithSN(byte[] key)
        {
            var sha1 = new HMACSHA1(HmacKey);
            var encryptCodes = sha1.ComputeHash(Encoding.ASCII.GetBytes(x64f2717168e0a936.Text));
            for (int i = 0; i < key.Length; i++)
            {
                var encryptCode = encryptCodes[i % encryptCodes.Length];
                key[i] += encryptCode;
            }
            return key;
        }

        private static byte[] DecryptWithSN(byte[] key)
        {
            var sha1 = new HMACSHA1(HmacKey);
            var encryptCodes = sha1.ComputeHash(Encoding.ASCII.GetBytes(x64f2717168e0a936.Text));
            for (int i = 0; i < key.Length; i++)
            {
                var encryptCode = encryptCodes[i % encryptCodes.Length];
                key[i] -= encryptCode;
            }
            return key;
        }
    }
}
