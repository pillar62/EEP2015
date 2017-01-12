using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFClientTools.Common;
using System.Reflection;
using EFClientTools.EFServerReference;
using System.Collections;
using System.Runtime.Serialization;
using EFClientTools.Web;

namespace EFClientTools.Common
{
    public class EntityProvider
    {
        public const string ClientServiceReferenceNameSpace = "EFClientTools.EFServerReference.{0}";

        public static List<string> GetEntityProperties(string entityClassName)
        {
            List<string> entityProperties = null;
            Type entityType = GetEntityType(entityClassName);

            entityProperties = entityType.GetProperties().Where(p => (TypeHelper.IsPrimitive(p.PropertyType))).Select(p => p.Name).ToList();

            return entityProperties;
        }

        [Obsolete]
        public static Hashtable GetEntityPropertiesTypes(string entityClassName)
        {
            List<PropertyInfo> entityProperties = new List<PropertyInfo>();
            Hashtable resValue = new Hashtable();
            Type entityType = GetEntityType(entityClassName);

            entityProperties = entityType.GetProperties().Where(p => (TypeHelper.IsPrimitive(p.PropertyType))).ToList();
            //entityProperties = entityType.GetProperties().Where(p => (TypeHelper.IsPrimitive(p.PropertyType))).Select(p => p.Name).ToList();

            foreach (var property in entityProperties)
            {
                resValue.Add(property.Name, property.PropertyType);
            }

            return resValue;
        }

        public static Type GetEntityType(string entityClassName)
        {
            Type type = null;
            if (!string.IsNullOrWhiteSpace(entityClassName))
            {
                Assembly assembly = Assembly.GetAssembly(typeof(EntityObject));
                if (assembly != null)
                {
                    type = assembly.GetType(string.Format(ClientServiceReferenceNameSpace, entityClassName));
                }
            }
            return type;
        }

        public static object GetEntityByKey(ref IList lst, Dictionary<string, object> dicKeyValues)
        {
            object entity = null;
            foreach (object o in lst)
            {
                bool match = true;
                Type entityType = o.GetType();
                foreach (KeyValuePair<string, object> pair in dicKeyValues)
                {
                    PropertyInfo prop = entityType.GetProperty(pair.Key);
                    if (!Convert.ChangeType(pair.Value, prop.PropertyType).Equals(prop.GetValue(o, null)))
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    entity = o;
                    break;
                }
            }
            return entity;
        }

        public static PropertyInfo GetMasterRelatedProperty(string masterClass, string detailClass)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(EntityObject));
            Type masterType = GetEntityType(masterClass);
            Type detailType = GetEntityType(detailClass);
            if (masterType != null && detailType != null)
            {
                PropertyInfo prop = masterType.GetProperties().SingleOrDefault(p => p.PropertyType.IsGenericType
                    && p.PropertyType.Name == "List`1"
                    && p.PropertyType.GetGenericArguments()[0] == detailType);
                if (prop != null)
                {
                    return prop;
                }
            }
            return null;
        }

        public static PropertyInfo GetDetailRelatedProperty(string masterClass, string detailClass)
        {
            Type masterType = GetEntityType(masterClass);
            Type detailType = GetEntityType(detailClass);
            if (masterType != null && detailType != null)
            {
                PropertyInfo prop = detailType.GetProperties().SingleOrDefault(p => p.PropertyType == masterType);
                if (prop != null)
                {
                    return prop;
                }
            }
            return null;
        }



        public static PropertyInfo GetEntityProperty(string entityClass, string entityProperty)
        {
            Type entityType = GetEntityType(entityClass);
            if (entityType != null)
            {
                PropertyInfo prop = null;
                if (entityProperty.IndexOf('.') == -1)
                {
                    prop = entityType.GetProperty(entityProperty);
                }
                else
                {
                    string[] relatedProps = entityProperty.Split('.');
                    PropertyInfo propRelated = entityType.GetProperty(relatedProps[0]);
                    if (propRelated != null)
                    {
                        Type relatedEntityType = propRelated.PropertyType;
                        prop = relatedEntityType.GetProperty(relatedProps[1]);
                    }
                }
                return prop;
            }
            return null;
        }

        public static Type GetEntityPropertyType(string entityClass, string entityProperty)
        {
            PropertyInfo prop = GetEntityProperty(entityClass, entityProperty);
            if (prop != null)
            {
                return prop.PropertyType;
            }
            return null;
        }

        public static List<string> GetDetailEntityClassNames(string masterClassName)
        {
            return GetEntityNavigations(masterClassName, true);
        }

        public static List<string> GetEntityNavigations(string entityClassName, bool isGetTypeName)
        {
            List<string> detailClassNames = new List<string>();

            Type masterClassType = EntityProvider.GetEntityType(entityClassName);
            IEnumerable<PropertyInfo> detailObjectCollection = masterClassType.GetProperties().Where(c => typeof(IList).IsAssignableFrom(c.PropertyType) && c.PropertyType.IsGenericType);

            if (detailObjectCollection.Count() != 0)
            {
                foreach (var detailObject in detailObjectCollection)
                {
                    string detailClassName = isGetTypeName == true ? detailObject.PropertyType.GetGenericArguments().ElementAt(0).Name : detailObject.Name;
                    detailClassNames.Add(detailClassName);
                }
            }

            return detailClassNames;
        }

        public static Dictionary<String, String> GetDetailEntityClassNameAndEntitySetName(String entityClassName)
        {
            Dictionary<String, String> detailClasses = new Dictionary<String, String>();

            Type masterClassType = EntityProvider.GetEntityType(entityClassName);
            IEnumerable<PropertyInfo> detailObjectCollection = masterClassType.GetProperties().Where(c => typeof(IList).IsAssignableFrom(c.PropertyType) && c.PropertyType.IsGenericType);

            if (detailObjectCollection.Count() != 0)
            {
                foreach (var detailObject in detailObjectCollection)
                {
                    detailClasses.Add(detailObject.PropertyType.GetGenericArguments().ElementAt(0).Name, detailObject.Name);
                }
            }

            return detailClasses;
        }

        public static List<string> GetEntityNavigationFields(string entityClassName)
        {
            return GetEntityNavigations(entityClassName, false);
        }

        public static List<string> GetClientEntityProperties(EFDataSource eds, Assembly assembly)
        {
            List<string> lst = new List<string>();
            if (eds != null && !string.IsNullOrEmpty(eds.RemoteName) && eds.RemoteName.IndexOf('.') != -1)
            {
                List<string> entityClasses = eds.GetEntityClasses();
                string assemblyName = eds.RemoteName.Split('.')[0];
                foreach (string entity in entityClasses)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        DataContractAttribute[] attributes = type.GetCustomAttributes(typeof(DataContractAttribute), false) as DataContractAttribute[];
                        if (attributes.Length == 1 && attributes[0].Name == entity)
                        {
                            string ns = attributes[0].Namespace;
                            if (!string.IsNullOrEmpty(ns))
                            {
                                ns = ns.Substring(ns.LastIndexOf('/') + 1);
                                if (string.Compare(ns, assemblyName, true) == 0)
                                {
                                    lst.Add(type.Name);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return lst;
        }

        public static string GetClientEntityClassName(string assemblyName, string serverEntityClassName)
        {
            Assembly assembly = typeof(EntityObject).Assembly;
            string result = string.Empty;
            foreach (Type type in assembly.GetTypes())
            {
                DataContractAttribute[] attributes = type.GetCustomAttributes(typeof(DataContractAttribute), false) as DataContractAttribute[];
                if (attributes.Length == 1 && attributes[0].Name == serverEntityClassName)
                {
                    string ns = attributes[0].Namespace;
                    if (!string.IsNullOrEmpty(ns))
                    {
                        ns = ns.Substring(ns.LastIndexOf('/') + 1);
                        if (ns == assemblyName)
                        {
                            result = type.Name;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static string GetServerEntityClassName(string assemblyName, string clientEntityClassName)
        {
            Assembly assembly = typeof(EntityObject).Assembly;
            string result = string.Empty;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == clientEntityClassName)
                {
                    DataContractAttribute[] attributes = type.GetCustomAttributes(typeof(DataContractAttribute), false) as DataContractAttribute[];
                    if (attributes.Length == 1)
                    {
                        result = attributes[0].Name;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
