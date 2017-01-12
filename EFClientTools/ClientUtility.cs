using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFClientTools.EFServerReference;
using System.ServiceModel;
using System.Reflection;
using System.Collections;
using System.ServiceModel.Channels;
using EFClientTools.Common;
using EFClientTools.Beans;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.IO;


namespace EFClientTools
{
    public class ClientUtility
    {
        public static EFServiceClient Client
        {
            get
            {
                EFServiceClient client = null;
                if (!string.IsNullOrEmpty(ServerIPAddress))
                {
                    var address = string.Format("http://{0}/EFWCFModule/EFService/IEFService", ServerIPAddress);
                    var binding = new CustomBinding ("CustomBinding_IEFService");
                    client = new EFServiceClient(binding, new EndpointAddress(address));
                }
                else
                {
                    client =  new EFServiceClient();
                }
                try
                {
                    client.CanOpen();
                }
                catch (EndpointNotFoundException ex)
                {
                    var address = client.Endpoint.Address;
                    var listenerAddress = string.Format("http://{0}:{1}/EFWCFModule/EFService/IEFService", address.Uri.Host, 8001);
                    try
                    {
                        if (!client.ChannelFactory.CreateChannel(new EndpointAddress(listenerAddress)).StartServer())
                        {
                            throw ex;
                        }
                    }
                    catch
                    {
                        throw ex;
                    }
                }
                return client;
            }
        }

        public static string ServerIPAddress
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["ServerIPAddress"];
            }
            set
            {
                System.Web.HttpContext.Current.Session["ServerIPAddress"] = value;
            }
        }


        public static ClientInfo ClientInfo
        {
            get 
            {
                if (System.Web.HttpContext.Current == null) return null;
                var _clientInfo = (ClientInfo)System.Web.HttpContext.Current.Session["ClientInfo"];
                if (_clientInfo == null)
                {
                    var context = System.Web.HttpContext.Current;
                    var publicKey = context.Request.QueryString["publicKey"];
                    if (!string.IsNullOrEmpty(publicKey))
                    { 
                        //重建session
                        EFServiceClient client = EFClientTools.ClientUtility.Client;
                        try
                        {
                            var p = context.Request.QueryString["p"];
                            var  parameters = ParameterDecode(p);
                            var clientInfo = client.LogOn(new ClientInfo() { SecurityKey = publicKey, UseDataSet = true, SDDeveloperID = GetParameterValue(parameters, "developerid") });
                            
                            if (!string.IsNullOrEmpty(p))
                            {
                                clientInfo.Database = GetParameterValue(parameters, "database");
                                clientInfo.Solution = GetParameterValue(parameters, "solution");
                                clientInfo.Locale = GetParameterValue(parameters, "locale");
                                clientInfo.SDDeveloperID = GetParameterValue(parameters, "developerid");
                            }
                            clientInfo.IsSDModule = true;
                            clientInfo.LogonResult = LogonResult.Logoned;
                            _clientInfo = clientInfo;
                            System.Web.HttpContext.Current.Session["ClientInfo"] = _clientInfo;
                            return _clientInfo;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + ":" + publicKey);
                        }
                    }
                    throw new Exception("Timeout, relogon please");
                }
                return _clientInfo;
            }
        }

        private static string ParameterDecode(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                try
                {
                    var key = Convert.FromBase64String(parameter);
                    if (key.Length > 16)
                    {
                        var hash = key.Take(16).ToArray();
                        var info = key.Skip(16).ToArray();
                        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        if (Convert.ToBase64String(md5.ComputeHash(info)) == Convert.ToBase64String(hash))
                        {
                            return Encoding.ASCII.GetString(info);
                        }
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private static string GetParameterValue(string parameters, string name)
        {
            var match = System.Text.RegularExpressions.Regex.Match(parameters, string.Format(@"(?<=(\&|^){0}=).+?(?=(\&|$))", name));
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string remoteName;
        public static string RemoteName 
        {
            get
            {
                return remoteName;
            }
            set
            {
                remoteName = value;
                if (!string.IsNullOrEmpty(remoteName))
                {
                    ClientInfo.SecurityKey = ClientInfo.UserID;
                }
            }
        }

        public static string PublicKey
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["PublicKey"];
            }
            set
            {
                System.Web.HttpContext.Current.Session["PublicKey"] = value;
            }
        }


        public static PacketInfo PacketCreator(int start, int count, List<WhereParameter> lstWhere, List<OrderParameter> lstOrder)
        {
            return new PacketInfo() 
            { 
                StartIndex = start,
                Count = count,
                WhereParameters = (lstWhere == null ? new List<WhereParameter>() : lstWhere),
                OrderParameters = (lstOrder == null ? new List<OrderParameter>() : lstOrder)
            };
        }

        public static List<EntityObject> GetObjects(IEFDataSource datasource)
        {
            if(datasource == null)
            {
                throw new ArgumentNullException("datasource");
            }
            if (string.IsNullOrEmpty(datasource.RemoteName))
            {
                throw new ArgumentNullException("datasource.RemoteName");
            }
            //split remotename
            var remoteName = datasource.RemoteName;
            var arrayRemote = remoteName.Split('.');
            return Client.GetObjects(ClientInfo, arrayRemote[0], arrayRemote[1], new PacketInfo());
        }

        //Ext Use
        public static List<string> GetEntitySetNames(string module, string command, string entityTypeName)
        {
            return Client.GetEntitySetNames(ClientInfo, module, command, entityTypeName);
        }

        public static string GetObjectClassName(string module, string command)
        {
            return GetObjectClassName(module, command, "");
        }

        public static string GetObjectClassName(string module, string command, string entitySetName)
        {
            return Client.GetObjectClassName(ClientInfo, module, command, entitySetName);
        }

        public static EntityObject GetEntityByKey(string module, string command, Dictionary<string, object> dicKeyValues, bool getDetails)
        {
            return GetEntityByKey(module, command, "", dicKeyValues, getDetails);
        }

        public static EntityObject GetEntityByKey(string module, string command, string entitySetName, Dictionary<string, object> dicKeyValues, bool getDetails)
        {
            Type entityType = EntityProvider.GetEntityType(ClientUtility.GetObjectClassName(module, command));
            if (entityType != null)
            {
                Dictionary<string, object> convertedKeyValues = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> pair in dicKeyValues)
                {
                    PropertyInfo prop = entityType.GetProperty(pair.Key);
                    convertedKeyValues.Add(pair.Key, Convert.ChangeType(pair.Value, TypeHelper.GetNullablePrimitiveType(prop.PropertyType)));
                }
                EntityObject entity = Client.GetObjectByKey(ClientInfo, module, command, entitySetName, convertedKeyValues) as EntityObject;
                if (entity != null && getDetails)
                {
                    entity = GetDetailObjects(module, command, entity);
                }
                return entity;
            }
            return null;
        }

        public static List<EntityObject> GetObjects(string module, string command, PacketInfo package)
        {
            return Client.GetObjects(ClientInfo, module, command, package);
        }

        public static EntityObject GetDetailObjects(string module, string command, EntityObject masterEntity)
        {
            return Client.GetDetailObjects(ClientInfo, module, command, masterEntity);
        }

        public static int GetObjectCount(string module, string command, List<WhereParameter> lstWhere)
        {
            return Client.GetObjectCount(ClientInfo, module, command, PacketCreator(0, -1, lstWhere, null));
        }

        public static List<EntityObject> UpdateObjects(string module, string command, List<EntityObject> objects, Dictionary<EntityKey, EntityState> states)
        {
            return Client.UpdateObjects(ClientInfo, module, command, objects, states);
        }

        public static object CallMethod(string module, string command, List<object> parameters)
        {
            return Client.CallMethod(ClientInfo, module, command, parameters);
        }

        private static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here      
            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        public static DataSet ExecuteSQL(string database, string commandText)
        {
            var obj = Client.ExecuteSQL(ClientInfo, database, commandText, new PacketInfo() { WhereParameters = new List<WhereParameter>() });
            return Deserialize<DataSet>((string)obj);
        }

        public static void ExecuteCommand(string database, string commandText)
        {
            Client.ExecuteCommand(ClientInfo, database, commandText);
        }

        public static void SetEntityProperties(Dictionary<string, object> dicRecord, ref EntityObject entity)
        {
            Type entityType = entity.GetType();
            foreach (KeyValuePair<string, object> record in dicRecord)
            {
                PropertyInfo prop = entityType.GetProperty(record.Key);
                if (prop != null)
                {
                    Type propType = TypeHelper.GetNullablePrimitiveType(prop.PropertyType);

                    if (propType == typeof(Boolean) && record.Value != null && record.Value.ToString() == "on")
                    {
                        prop.SetValue(entity, Convert.ChangeType(true, propType), null);
                    }
                    else if (record.Value != null)
                    {
                        prop.SetValue(entity, Convert.ChangeType(record.Value, propType), null);
                    }
                    else
                    {
                        prop.SetValue(entity, null, null);
                    }
                }
            }
        }

        public static string GetSysValue(string key)
        {
            string strval = "";
            Char[] cs = key.ToCharArray();
            if (cs.Length != 0)
            {
                if (cs[0].Equals('\\'))
                {
                    strval = key.Substring(1);
                }
                else if (cs[0].Equals('_'))
                {
                    #region sysValue
                    switch (key.ToLower())
                    {
                        case "_usercode": strval = ClientInfo.UserID; break;
                        case "_username":
                            {
                                strval = ClientInfo.UserName;
                                break;
                            }
                        case "_solution": strval = ClientInfo.Solution; break;
                        case "_database": strval = ClientInfo.Database; break;
                        case "_sitecode": strval = ClientInfo.Site; break;
                        case "_ipaddress": strval = ClientInfo.IPAddress; break;
                        case "_language": strval = ClientInfo.Locale; break;
                        case "_today": strval = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day; break;
                        case "_sysdate": strval = DateTime.Now.ToString(); break;
                        case "_servertoday":
                            //object[] myRet = CallMethod("GLModule", "GetServerTime", new object[] { });
                            //if (myRet != null && (int)myRet[0] == 0)
                            //{
                            //    strval = (string)myRet[1];
                            //}
                            break;
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
                                DateTime retday = DateTime.Now.AddDays(-day);
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
                        default: strval = key; break;
                    }
                    #endregion
                }
                else
                {
                    strval = key;
                }
            }
            return strval;
        }

        /*public static void SetEntityProperties(string module, string command, Dictionary<string, object> dicRecord, Dictionary<string, object> dicFields, ref EntityObject entity)
        {
            Type entityType = entity.GetType();
            foreach (KeyValuePair<string, object> record in dicRecord)
            {
                string mapping = dicFields[record.Key] as string;
                if (!string.IsNullOrEmpty(mapping))
                {
                    PropertyInfo prop = entityType.GetProperty(record.Key);
                    if (prop != null)
                    {
                        prop.SetValue(entity, Convert.ChangeType(record.Value, TypeHelper.GetNullablePrimitiveType(prop.PropertyType)), null);
                    }
                    //if (mapping.IndexOf('.') != -1)
                    //{
                    //    string[] propPath = mapping.Split('.');
                    //    PropertyInfo refProp = entityType.GetProperty(string.Format("{0}Reference", propPath[0]));
                    //    if (refProp != null)
                    //    {
                    //        prop = entityType.GetProperty(propPath[0]);
                    //        List<string> entitySetNames = GetEntitySetNames(module, command, prop.PropertyType.Name);
                    //        if (entitySetNames.Count > 0)
                    //        {
                    //            PropertyInfo refEntityKeyProp = prop.PropertyType.GetProperty(propPath[1]);
                    //            if (refEntityKeyProp != null)
                    //            {
                    //                Dictionary<string, object> dicKeyValues = new Dictionary<string, object>();
                    //                dicKeyValues.Add(propPath[1], Convert.ChangeType(record.Value, GetNullablePrimitiveType(refEntityKeyProp.PropertyType)));
                    //                EntityObject refEntity = GetEntityByKey(module, command, entitySetNames[0], dicKeyValues);
                    //                if (refEntity != null)
                    //                {
                    //                    object o = Activator.CreateInstance(refProp.PropertyType);
                    //                    refProp.PropertyType.GetProperty("EntityKey").SetValue(o, refEntity.EntityKey, null);
                    //                    refProp.SetValue(entity, o, null);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    prop = entityType.GetProperty(record.Key);
                    //    if (prop != null)
                    //    {
                    //        prop.SetValue(entity, Convert.ChangeType(record.Value, GetNullablePrimitiveType(prop.PropertyType)), null);
                    //    }
                    //}
                }
            }
        }*/
    }

    /// <summary>
    /// For design time
    /// </summary>
    public class DesignClientUtility
    {
        public const string EndPointAddress = "http://localhost:8990/EFWCFModule/EFService/IEFService";

        public static Binding binding = new CustomBinding();



        public static EndpointAddress address = new EndpointAddress(EndPointAddress);
       
        private static EFServiceClient _client;
        public static EFServiceClient Client
        {
            get
            {
                if (_client == null || _client.State == CommunicationState.Faulted)
                {
                    var customBinding = binding as CustomBinding;
                    if (customBinding.Elements.Count == 0)
                    {
                        customBinding.Elements.Add(new BinaryMessageEncodingBindingElement()
                        {
                            ReaderQuotas = new XmlDictionaryReaderQuotas()
                            {
                                MaxArrayLength = 2147483647,
                                MaxBytesPerRead = 40960,
                                MaxDepth = 64,
                                MaxNameTableCharCount = 163840,
                                MaxStringContentLength = 20971520
                            }
                        });
                        customBinding.Elements.Add(new HttpTransportBindingElement() { MaxReceivedMessageSize = 2147483647, MaxBufferSize = 2147483647 });
                    }
                    _client = new EFServiceClient(binding, address);
                }
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        private static ClientInfo _clientInfo = new ClientInfo() { Solution = Design.DTE.CurrentSolution, IPAddress = "localhost", Locale = System.Globalization.CultureInfo.CurrentCulture.Name };

        public static ClientInfo ClientInfo
        {
            get { return _clientInfo; }
        }

        public static List<string> GetCommandNames(string assemblyName)
        {
            List<string> result = null;
            try
            {
                return DesignClientUtility.Client.GetCommandNames(ClientInfo, assemblyName);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }

            return result;
        }

        public static List<string> GetModuleNames()
        {
            List<string> result = null;
            try
            {
                return DesignClientUtility.Client.GetModuleNames(ClientInfo);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }

            return result;
        }

        /// <summary>
        /// 取得DD
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="commandName"></param>
        /// <param name="entityClassName"></param>
        /// <param name="exChangeEntityClassName">为True的时候，传入的ClientEntityClassName，False时传入的ServerEntityClassName</param>
        /// <param name="loginDataBase">登陆的数据库别名</param>
        /// <returns></returns>
        public static List<COLDEFInfo> GetColumnDefination(string assemblyName, string commandName, string entityClassName, bool exChangeEntityClassName, String loginDataBase)
        {
            ClientInfo.Database = loginDataBase;
            List<COLDEFInfo> colDefs = new List<COLDEFInfo>();

            try
            {
                string serverEntityClassName = string.Empty;

                serverEntityClassName = exChangeEntityClassName == true ? EntityProvider.GetServerEntityClassName(assemblyName, entityClassName) : entityClassName;

                List<String> entitySetNames = DesignClientUtility.Client.GetEntitySetNames(ClientInfo, assemblyName, commandName, serverEntityClassName);
                if (entitySetNames != null && entitySetNames.Count > 0)
                {
                    serverEntityClassName = entitySetNames[0];
                }

                var lists = DesignClientUtility.Client.GetColumnDefination(ClientInfo, assemblyName, commandName, serverEntityClassName);
                List<EFClientTools.EFServerReference.EntityObject> objects = (List<EFClientTools.EFServerReference.EntityObject>)lists;

                if (objects != null)
                {
                    foreach (var obj in objects)
                    {
                        COLDEFInfo colDef = new COLDEFInfo();

                        foreach (var prop in colDef.GetType().GetProperties())
                        {
                            object value = null;
                            value = obj.GetType().GetProperty(prop.Name).GetValue(obj, null);

                            if (string.Compare(prop.PropertyType.Name, "Int32", true) == 0)
                            {
                                if (value == null)
                                {
                                    prop.SetValue(colDef, 0, null);
                                }
                                else
                                {
                                    prop.SetValue(colDef, Convert.ToInt32(value), null);
                                }
                            }
                            else
                            {
                                if (value == null)
                                {
                                    prop.SetValue(colDef, string.Empty, null);
                                }
                                else
                                {
                                    prop.SetValue(colDef, value, null);
                                }
                            }

                        }

                        colDefs.Add(colDef);
                    }
                }

                if (objects == null || objects.Count == 0)
                {
                    colDefs = null;
                }

            }
            catch (Exception ex)
            {
                string message = string.Empty;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                else if (ex.Message == "Member 'DbConnectionSet.SystemDatabase' not found.")
                {
                    message = "Please set up SYSTEM DB of EEPNetServer !";
                }
                else
                {
                    message = ex.Message;
                }
                throw new Exception(message);
                //System.Windows.Forms.MessageBox.Show(message);
            }

            return colDefs;
        }

        public static List<COLDEFInfo> GetColumnDefination(string assemblyName, string commandName, string clientEntityClassName, String loinDataBase)
        {
            return GetColumnDefination(assemblyName, commandName, clientEntityClassName, true, loinDataBase);
        }

        public static Dictionary<string, object> GetEntityPropertiesTypes(string assemblyName, string commandName, string clientEntityClassName)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                string serverEntityClassName = EntityProvider.GetServerEntityClassName(assemblyName, clientEntityClassName);

                Dictionary<string, string> lists = DesignClientUtility.Client.GetEntityFieldTypes(ClientInfo, assemblyName, commandName, serverEntityClassName);

                foreach (var item in lists)
                {
                    result.Add(item.Key, Type.GetType(item.Value.Replace("Edm", "System")));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }

            return result;
        }

        public static Dictionary<string, string> GetEntityPropertieMappings(string assemblyName, string commandName, string clientEntityClassName)
        {
            Dictionary<string, string> result = null;
            try
            {
                string serverEntityClassName = EntityProvider.GetServerEntityClassName(assemblyName, clientEntityClassName);
                result = DesignClientUtility.Client.GetEntityFieldMappings(ClientInfo, assemblyName, commandName, serverEntityClassName);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }
            return result;
        }

        public static List<string> GetDetailEntityClassNames(string assemblyName, string commandName, string clientMasterEntityClassName)
        {
            List<String> result = null;
            try
            {
                string serverMasterEntityClassName = EntityProvider.GetServerEntityClassName(assemblyName, clientMasterEntityClassName);
                return DesignClientUtility.Client.GetEntityNavigationFields(ClientInfo, assemblyName, commandName, serverMasterEntityClassName);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }
            return result;
        }

        public static List<string> GetEntityPrimaryKeys(string assemblyName, string commandName, string clientEntityClassName)
        {
            List<String> result = null;
            try
            {
                string serverEntityClassName = EntityProvider.GetServerEntityClassName(assemblyName, clientEntityClassName);
                return DesignClientUtility.Client.GetEntityPrimaryKeys(ClientInfo, assemblyName, commandName, serverEntityClassName);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }
            return result;
        }

        public static List<EntityObject> GetAllDataByTableName(String tableName)
        {
            return DesignClientUtility.Client.GetAllDataByTableName(ClientInfo, tableName);
        }

        public static List<EntityObject> GetAllDataByTableNameDBAlias(String dbAlias, String tableName)
        {
            return DesignClientUtility.Client.GetAllDataByTableNameDBAlias(ClientInfo, dbAlias, tableName);
        }

        public static List<EntityObject> GetDataByTableNameWhere(List<object> param)
        {
            return DesignClientUtility.Client.GetDataByTableNameWhere(ClientInfo, param);
        }

        public static void SaveDataToTable(List<object> param, String tableName)
        {
            DesignClientUtility.Client.SaveDataToTable(ClientInfo, param, tableName);
        }

        public static void DeleteDataFromTable(object param, String tableName)
        {
            DesignClientUtility.Client.DeleteDataFromTable(ClientInfo, param, tableName);
        }

        public static String GetServerPath()
        {
            return DesignClientUtility.Client.GetServerPath(ClientInfo);
        }
    }
}
