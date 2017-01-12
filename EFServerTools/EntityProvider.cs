using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCFModule;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Metadata.Edm;
using System.Reflection;
using System.Data.EntityClient;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using Database = EFWCFModule.EEPAdapter.DatabaseProvider;

namespace EFServerTools
{
    /// <summary>
    /// Provider of entity
    /// </summary>
    public static class EntityProvider
    {
        /// <summary>
        /// Gets provider or database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <returns>Provider of database</returns>
        private static string GetProvider(string database, string developerID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            return Database.GetProvider(database, developerID);
        }

        /// <summary>
        /// Gets connection string of database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <returns>Connection string of database</returns>
        private static string GetProviderConnectionString(string database, string developerID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            return Database.GetProviderConnectionString(database, developerID);
        }

        /// <summary>
        /// Gets split stystem table of database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <returns>Split stystem table</returns>
        private static bool GetSplitSystemTable(string database, string developerID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            return Database.GetSplitSystemTable(database, developerID);
        }

        #region EntityConnection

        /// <summary>
        /// Creates a new entity connection
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="assembly">Assembly</param>
        /// <param name="metadataFile">Name of edmx file</param>
        /// <returns>A new entity connection</returns>
        public static EntityConnection CreateEntityConnection(string database, Assembly assembly, string metadataFile)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadata");
            }
            return CreateEntityConnection(database, assembly, metadataFile, false, null);
        }

        /// <summary>
        /// Create a new entity connection
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="assembly">Assembly</param>
        /// <param name="metadataFile">Name of edmx file</param>
        /// <param name="useSystemDataBase">Use system database</param>
        /// <returns>A new entity connection</returns>
        public static EntityConnection CreateEntityConnection(string database, Assembly assembly, string metadataFile, bool useSystemDataBase, string developerID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadata");
            }
            if (useSystemDataBase)
            {
                if (GetSplitSystemTable(database, developerID))
                {
                    database = Database.GetSystemDatabase(developerID);
                }
            }
            var provider = GetProvider(database, developerID);
            var providerConnectionString = GetProviderConnectionString(database, developerID);
            return CreateEntityConnection(provider, providerConnectionString, assembly, metadataFile);
        }

        /// <summary>
        /// Creates a new entity connection
        /// </summary>
        /// <param name="provider">Provider of database</param>
        /// <param name="providerConnectionString">Connection string of database</param>
        /// <param name="assembly">Assembly</param>
        /// <param name="metadataFile">Name of edmx file</param>
        /// <returns>A new entity connection</returns>
        private static EntityConnection CreateEntityConnection(string provider, string providerConnectionString, Assembly assembly, string metadataFile)
        {
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException("provider");
            }
            if (string.IsNullOrEmpty(providerConnectionString))
            {
                throw new ArgumentNullException("providerConnectionString");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadata");
            }
            var builder = new EntityConnectionStringBuilder();
            builder.Provider = provider;
            builder.ProviderConnectionString = providerConnectionString;
            builder.Metadata = string.Format("res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl", assembly.FullName, metadataFile);
            return new EntityConnection(builder.ToString());
        }
        #endregion

        #region ObjectContext

        /// <summary>
        /// Creates a new object context
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="assembly">Assembly</param>
        /// <param name="metadataFile">Name of edmx file</param>
        /// <param name="containerName">Name of context</param>
        /// <returns>A new object context</returns>
        public static ObjectContext CreateContext(string database, Assembly assembly, string metadataFile, string containerName)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadata");
            }
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            return CreateContext(database, assembly, metadataFile, containerName, false, null);
        }

        /// <summary>
        /// Creates a new object context
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="assembly">Assembly</param>
        /// <param name="metadataFile">Name of edmx file</param>
        /// <param name="containerName">Name of context</param>
        /// <param name="useSystemDataBase">Use system database</param>
        /// <returns>A new object context</returns>
        public static ObjectContext CreateContext(string database, Assembly assembly, string metadataFile, string containerName, bool useSystemDataBase, string developerID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadata");
            }
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }

            var connection = CreateEntityConnection(database, assembly, metadataFile, useSystemDataBase, developerID);
            var contextType = assembly.GetTypes().FirstOrDefault(c => c.Name.Equals(containerName));
            if (contextType == null)
            {
                throw new ObjectNotFoundException(string.Format("Container:{0} not found.", containerName));
            }
            var context = (ObjectContext)contextType.GetConstructor(new Type[] { typeof(EntityConnection) }).Invoke(new object[] { connection });
            context.MetadataWorkspace.LoadFromAssembly(assembly);
            return context;
        }

        /// <summary>
        /// Creates a new object context
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="command">ICommand</param>
        /// <param name="assembly">Assembly</param>
        /// <returns>A new object context</returns>
        internal static ObjectContext CreateContext(ClientInfo clientInfo, ICommand command, Assembly assembly)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(command.MetadataFile))
            {
                throw new ArgumentNullException("command.MetadataFile");
            }
            if (string.IsNullOrEmpty(command.ContextName))
            {
                throw new ArgumentNullException("command.ContextName");
            }
            var database = string.IsNullOrEmpty(command.DataBase) ? clientInfo.Database : command.DataBase;
            var metadataFile = command.MetadataFile;
            var containerName = command.ContextName;
            var useSystemDatabase = command.UseSystemDB;
            var context = CreateContext(database, assembly, metadataFile, containerName, useSystemDatabase, clientInfo.SDDeveloperID);
            context.ContextOptions.LazyLoadingEnabled = false;
            return context;
        }
        #endregion

        #region ObjectQuery

        #region Const String
        /// <summary>
        /// ,
        /// </summary>
        const string COMMA_STRING = ",";
        /// <summary>
        /// ORDER BY
        /// </summary>
        const string ORDERBY_STRING = "ORDER BY";
        /// <summary>
        /// DESC
        /// </summary>
        const string DESCENDING_STRING = " DESC";
        /// <summary>
        /// AND
        /// </summary>
        const string AND_STRING = " AND ";
        /// <summary>
        /// OR
        /// </summary>
        const string OR_STRING = " OR ";
        /// <summary>
        /// Condition strings
        /// </summary>
        static readonly string[] CONDITION_STRINGS = new string[] { " = ", " <> ", " > ", " >= ", " < ", " <= ", " LIKE ", " LIKE ", " IN " };
        #endregion

        /// <summary>
        /// Set where
        /// </summary>
        /// <param name="query">Query of objects</param>
        /// <param name="parameters">Parameters of query</param>
        /// <returns>Query of objects</returns>
        internal static ObjectQuery<EntityObject> SetWhere(ObjectQuery<EntityObject> query, IEnumerable<WhereParameter> parameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("paramters");
            }
            if (parameters.Count() > 0)
            {
                var parameterPrefix = GetParameterPrefix(query.Context);
                var builder = new StringBuilder();
                foreach (var where in parameters)
                {
                    if ((where.Values == null || where.Values.Count == 0)
                        && (where.Value == null || String.IsNullOrEmpty(where.Value.ToString())))
                        continue;
                    var field = where.Field;
                    if (string.IsNullOrEmpty(field))
                    {
                        throw new ArgumentNullException("parameters.Field");
                    }
                    if (builder.Length > 0) //add "and" or "or"
                    {
                        if (where.And)
                        {
                            builder.Append(AND_STRING);
                        }
                        else
                        {
                            builder.Append(OR_STRING);
                        }
                    }
                    builder.Append(string.Format("{0}.{1}", query.Name, field)); // add field
                    builder.Append(CONDITION_STRINGS[(int)where.Condition]); // add condition

                    if (where.Condition == WhereCondition.In)
                    {
                        builder.Append("{");

                        foreach (var value in (IEnumerable)where.Values)
                        {
                            if (!builder[builder.Length - 1].Equals('{'))
                            {
                                builder.Append(",");
                            }
                            var parameterName = GetParameterName(query, field);
                            var parameterValue = GetParameterValue(where.Condition, value);
                            var parameter = new ObjectParameter(parameterName, parameterValue);
                            builder.Append(string.Format("{0}{1}", parameterPrefix, parameterName));
                            query.Parameters.Add(parameter);
                        }
                        builder.Append("}");
                    }
                    else
                    {
                        var parameterName = GetParameterName(query, field);
                        var parameterValue = GetParameterValue(where.Condition, where.Value);
                        var parameter = new ObjectParameter(parameterName, parameterValue);
                        builder.Append(string.Format("{0}{1}", parameterPrefix, parameterName));
                        query.Parameters.Add(parameter);
                    }
                }
                if (builder.Length > 0)
                {
                    return query.Where(builder.ToString());
                }
                else 
                {
                    return query;
                }
            }
            else
            {
                return query;
            }
        }

#warning DataBase
        /// <summary>
        /// Gets prefix of parameter
        /// </summary>
        /// <param name="query">Object context</param>
        /// <returns>Prefix of parameter</returns>
        internal static string GetParameterPrefix(ObjectContext context)
        {
            if (context.Connection is System.Data.SqlClient.SqlConnection)
            {
                return "@";
            }
#if Oracle
            else if (context.Connection is DDTek.Oracle.OracleConnection)
            {
                return ":";
            }
#elif Oracle2
            else if (context.Connection is Oracle.DataAccess.Client.OracleConnection)
            {
                return ":";
            }
#endif
            else
            {
                return "@";
            }
        }

        /// <summary>
        /// Gets name of parameter
        /// </summary>
        /// <param name="query">Query of objects</param>
        /// <param name="field">Name of field</param>
        /// <returns>Name of parameter</returns>
        private static string GetParameterName(ObjectQuery<EntityObject> query, string field)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            var parameterName = field;
            var index = 0;
            while (true)
            {
                if (!query.Parameters.Contains(parameterName))
                {
                    return parameterName;
                }
                else
                {
                    parameterName = string.Format("{0}{1}", field, index);
                    index++;
                }
            }
        }

        /// <summary>
        /// Gets value of parameter
        /// </summary>
        /// <param name="condition">Condition of where</param>
        /// <param name="value">Value of field</param>
        /// <returns>Value of parameter</returns>
        private static object GetParameterValue(WhereCondition condition, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (condition == WhereCondition.BeginWith)
            {
                return string.Format("{0}%", value);
            }
            else if (condition == WhereCondition.Contain)
            {
                return string.Format("%{0}%", value);
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Set order
        /// </summary>
        /// <param name="query">Query of objects</param>
        /// <param name="parameters">Parameters of query</param>
        /// <returns>Query of objects</returns>
        internal static ObjectQuery<EntityObject> SetOrder(ObjectQuery<EntityObject> query, IEnumerable<OrderParameter> parameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("paramters");
            }
            if (parameters.Count() > 0)
            {
                var builder = new StringBuilder();
                foreach (var order in parameters)
                {
                    var field = order.Field;
                    if (string.IsNullOrEmpty(field))
                    {
                        throw new ArgumentNullException("parameters.Field");
                    }
                    if (builder.Length > 0)
                    {
                        builder.Append(COMMA_STRING);
                    }
                    builder.Append(string.Format("{0}.{1}", query.Name, field));
                    if (order.Direction == OrderDirection.Descending)
                    {
                        builder.Append(DESCENDING_STRING);
                    }
                }
                return query.OrderBy(builder.ToString());
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// Gets packages
        /// </summary>
        /// <param name="query">Query of objects</param>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="startIndex">Index to start</param>
        /// <param name="count">Count of packages</param>
        /// <returns>List of objects</returns>
        internal static IQueryable<EntityObject> GetPackages(ObjectQuery<EntityObject> query, string containerName, string entitySetName, int startIndex, int count)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "Value must be equal or greater than 0.");
            }
            if (count < -1)
            {
                throw new ArgumentOutOfRangeException("count", count, "Value must be geater than 0.");
            }
            if (!IsOrdered(query))
            {
                var provider = new MetadataProvider(query.Context.MetadataWorkspace);
                var entityTypeName = provider.GetEntitySetType(containerName, entitySetName).Name;
                var listKeys = provider.GetKeyPropertyNames(containerName, entityTypeName);
                var orderParameters = new List<OrderParameter>();
                foreach (var key in listKeys)
                {
                    orderParameters.Add(new OrderParameter() { Field = key });
                }
                query = SetOrder(query, orderParameters);
            }
            if (count > 0)
            {
                return query.Skip(startIndex).Take(count);
            }
            else
            {
                return query.Skip(startIndex);
            }
        }

        /// <summary>
        /// Query is ordered
        /// </summary>
        /// <param name="query">Query of objects</param>
        /// <returns>Query is ordered</returns>
        private static bool IsOrdered(ObjectQuery<EntityObject> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return query.CommandText.IndexOf(ORDERBY_STRING, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        #endregion

        #region EntitySQL
#warning 这里的正则要整理
        #region Const String
        /// <summary>
        /// (?&lt;={0}){1}
        /// </summary>
        const string PREFIX_REG = "(?<={0}){1}";
        /// <summary>
        /// (?&lt;={0}){1}(?={2})
        /// </summary>
        const string PREFIX_SUFFIX_REG = "(?<={0}){1}(?={2})";
        /// <summary>
        /// ^\s*select\s+value\s+
        /// </summary>
        const string SELECT_REG = @"^\s*select\s+value\s+";
        /// <summary>
        /// \s+from\s+
        /// </summary>
        const string FROM_REG = @"\s+from\s+";
        /// <summary>
        /// ^\s*\bselect\s+value\s+\w+\s+from\b\s*
        /// </summary>
        const string SELECT_FROM_REG = @"^\s*\bselect\s+value\s+\w+\s+from\b\s*";
        /// <summary>
        /// \.
        /// </summary>
        const string DOT_REG = @"\.";
        /// <summary>
        /// \w+
        /// </summary>
        const string WORD_REG = @"\w+";
        /// <summary>
        /// \s*\bwhere\b\s*
        /// </summary>
        const string WHERE_REG = @"\s*\bwhere\b\s*";
        /// <summary>
        /// \s*\border\s+by\b\s*
        /// </summary>
        const string ORDERBY_REG = @"\s*\border\s+by\b\s*"; 
        #endregion

        /// <summary>
        /// Gets name of container
        /// </summary>
        /// <param name="entitySQL">Entity sql</param>
        /// <returns>Name of container</returns>
        internal static string GetContainerName(string entitySQL)
        {
            if (string.IsNullOrEmpty(entitySQL))
            {
                return string.Empty;
            }

            var match = Regex.Match(entitySQL, string.Format(PREFIX_REG, SELECT_FROM_REG, WORD_REG), RegexOptions.IgnoreCase);
            return match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// Gets name of entity set
        /// </summary>
        /// <param name="entitySQL">Entity sql</param>
        /// <returns>Name of entity set</returns>
        internal static string GetEntitySetName(string entitySQL)
        {
            if (string.IsNullOrEmpty(entitySQL))
            {
                return string.Empty;
            }

            var match = Regex.Match(entitySQL, string.Format(PREFIX_REG, SELECT_FROM_REG + WORD_REG + DOT_REG, WORD_REG), RegexOptions.IgnoreCase);
            return match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// Gets name of alias
        /// </summary>
        /// <param name="entitySQL">Entity sql</param>
        /// <returns>Name of alias</returns>
        internal static string GetAliasName(string entitySQL)
        {
            if (string.IsNullOrEmpty(entitySQL))
            {
                return string.Empty;
            }

            var match = Regex.Match(entitySQL, string.Format(PREFIX_SUFFIX_REG, SELECT_REG, WORD_REG, FROM_REG), RegexOptions.IgnoreCase);
            return match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// Determines whether sql contains select part
        /// </summary>
        /// <param name="entitySQL">Entity sql</param>
        /// <returns>Whether sql contains select part</returns>
        internal static bool ContainSelect(string entitySQL)
        {
            if (string.IsNullOrEmpty(entitySQL))
            {
                return false;
            }
            return Regex.IsMatch(entitySQL, SELECT_FROM_REG, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines whether sql contains where part
        /// </summary>
        /// <param name="entitySQL"></param>
        /// <returns>Whether sql contains where part</returns>
        internal static bool ContainWhere(string entitySQL)
        {
            if (string.IsNullOrEmpty(entitySQL))
            {
                return false;
            }
            return Regex.IsMatch(entitySQL, WHERE_REG, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines whether sql contains order part
        /// </summary>
        /// <param name="entitySQL">Entity sql</param>
        /// <returns>Whether sql contains order part</returns>
        internal static bool ContainOrder(string entitySQL)
        {
            if (string.IsNullOrEmpty(entitySQL))
            {
                return false;
            }
            return Regex.IsMatch(entitySQL, ORDERBY_REG, RegexOptions.IgnoreCase);
        }

        #endregion


#warning 以下三个方法有待商榷
        internal static IRelatedEnd GetRelatedEnd(ObjectContext context, EntityObject sourceObject, string targetEntitySetName)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (sourceObject == null)
            {
                throw new ArgumentNullException("sourceObject");
            }
            if (string.IsNullOrEmpty(targetEntitySetName))
            {
                throw new ArgumentNullException("targetEntitySetName");
            }
            var provider = new MetadataProvider(context.MetadataWorkspace);
            var relatedEnd = ((IEntityWithRelationships)sourceObject).RelationshipManager.GetAllRelatedEnds()
                .FirstOrDefault(c => provider.GetAssociationSetEndEntitySetName(context.DefaultContainerName, c.RelationshipName, c.TargetRoleName).Equals(targetEntitySetName));
            if (relatedEnd == null)
            {
                throw new ObjectNotFoundException(string.Format("RelatedEnd to entity set:{0} not found.", targetEntitySetName));
            }
            return relatedEnd;
        }

        internal static List<EntityObject> GetDetailObjects(ObjectContext context, List<EntityObject> masterObjects, string detailEntitySetName)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (masterObjects == null)
            {
                throw new ArgumentNullException("masterObjects");
            }
            if (string.IsNullOrEmpty(detailEntitySetName))
            {
                throw new ArgumentNullException("detailEntitySetName");
            }
            var list = new List<EntityObject>();
            foreach (var masterObject in masterObjects)
            {
                var relatedEnd = EntityProvider.GetRelatedEnd(context, masterObject, detailEntitySetName);
                if (relatedEnd is IEnumerable)
                {
                    foreach (EntityObject obj in (IEnumerable)relatedEnd)
                    {
                        list.Add(obj);
                    }
                }
            }

            return list;
        }

        //internal static EntityObject CreateObject(ObjectContext context, string entitySetName)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException("context");
        //    }
        //    if (string.IsNullOrEmpty(entitySetName))
        //    {
        //        throw new ArgumentNullException("entitySetName");
        //    }
        //    MetadataProvider provider = new MetadataProvider(context.MetadataWorkspace);

        //    var typeName = provider.GetEntitySetType(context.DefaultContainerName, entitySetName).Name;
        //    var type = context.GetType().Assembly.GetTypes()
        //        .Where(c => c.Name.Equals(typeName) && typeof(EntityObject).IsAssignableFrom(c)).FirstOrDefault();
        //    if (type == null)
        //    {
        //        throw new ObjectNotFoundException(string.Format("Type:{0} not found.", typeName));
        //    }
        //    var constructor = type.GetConstructors(BindingFlags.Public).FirstOrDefault();
        //    if (constructor == null)
        //    {
        //        throw new MissingMemberException(typeName, typeName);
        //    }
        //    return (EntityObject)constructor.Invoke(null);
        //}

    }

    /// <summary>
    /// Provider of meatadata workspace(runtime)
    /// </summary>
    public class MetadataProvider
    {
        /// <summary>
        /// Creates a new metadata workspace
        /// </summary>
        /// <param name="command">ICommand</param>
        /// <param name="assembly">Assembly</param>
        /// <returns>A new metadata workspace</returns>
        internal static MetadataWorkspace CreateMetadata(ICommand command, Assembly assembly)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(command.MetadataFile))
            {
                throw new ArgumentNullException("command.MetadataFile");
            }
            var metadataFile = command.MetadataFile;
            return CreateMetadata(assembly, metadataFile);
        }

        /// <summary>
        /// Creates a new metadata workspace
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="metadataFile">Name of edmx file</param>
        /// <returns>A new metadata workspace</returns>
        public static MetadataWorkspace CreateMetadata(Assembly assembly, string metadataFile)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadataFile");
            }
            var paths = new string[] 
            {
                string.Format("res://{0}/{1}.csdl", assembly.FullName, metadataFile),
                string.Format("res://{0}/{1}.ssdl", assembly.FullName, metadataFile),
                string.Format("res://{0}/{1}.msl", assembly.FullName, metadataFile)
            };
            var metadata = new MetadataWorkspace(paths, new Assembly[] { assembly });
            return metadata;
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        public static void ClearCache()
        {
            MetadataWorkspace.ClearCache();
        }

        /// <summary>
        /// Creates a new instance of metadata provider
        /// </summary>
        /// <param name="metadata">Metadata workspace</param>
        public MetadataProvider(MetadataWorkspace metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }
            _metadata = metadata;
        }

        private MetadataWorkspace _metadata;
        /// <summary>
        /// Gets metadata workspace of provider
        /// </summary>
        public MetadataWorkspace Metadata
        {
            get
            {
                return _metadata;
            }
        }

        /// <summary>
        /// Gets entity container element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <returns>Entity container element</returns>
        private EntityContainer GetEntityContainerElement(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            var container = Metadata.GetEntityContainer(containerName, DataSpace.CSpace);
            if (container == null)
            {
                throw new ObjectNotFoundException(string.Format("Container:{0} not found.", containerName));
            }
            return container;
        }

        /// <summary>
        /// Gets entity set element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Entity set element</returns>
        private EntitySet GetEntitySetElement(string containerName, string entitySetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            var container = GetEntityContainerElement(containerName);
            var entitySetElement = container.GetEntitySetByName(entitySetName, false);
            if (entitySetElement == null)
            {
                throw new ObjectNotFoundException(string.Format("EntitySet:{0} not found.", entitySetName));
            }
            return entitySetElement;
        }

        /// <summary>
        /// Gets association set element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of relationship set</param>
        /// <returns>Association set element</returns>
        private AssociationSet GetAssociationSetElement(string containerName, string relationshipSetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(relationshipSetName))
            {
                throw new ArgumentNullException("relationshipSetName");
            }
            var container = GetEntityContainerElement(containerName);
            var relationshipSetElement = container.BaseEntitySets
                .FirstOrDefault(c=> c.BuiltInTypeKind == BuiltInTypeKind.AssociationSet && c.ElementType.FullName.Equals(relationshipSetName));
            if (relationshipSetElement == null)
            {
                throw new ObjectNotFoundException(string.Format("RelationshipSet:{0} not found.", relationshipSetName));
            }
            return (AssociationSet)relationshipSetElement;
        }

        /// <summary>
        /// Gets association set end element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <param name="roleName">Name of role</param>
        /// <returns>Association set end element</returns>
        private AssociationSetEnd GetAssociationSetEndElement(string containerName, string relationshipSetName, string roleName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(relationshipSetName))
            {
                throw new ArgumentNullException("relationshipSetName");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            var associationSetElement = GetAssociationSetElement(containerName, relationshipSetName);

            var endElement = associationSetElement.AssociationSetEnds.FirstOrDefault(c => c.Name.Equals(roleName));
            if (endElement == null)
            {
                throw new ObjectNotFoundException(string.Format("AssociationSetEnd:{0} not found.", roleName));
            }
            return endElement;
        }
 
        /// <summary>
        /// Gets entity type element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Entity type element</returns>
        private EntityType GetEntityTypeElement(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeElement = Metadata.GetItems<EntityType>(DataSpace.CSpace).FirstOrDefault(c => c.Name.Equals(entityTypeName));
            if (entityTypeElement == null)
            {
                throw new ObjectNotFoundException(string.Format("EntityType:{0} not found.", entityTypeName));
            }
            return entityTypeElement;
        }

        /// <summary>
        /// Gets property element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="memberName">Name of property</param>
        /// <returns>Property element</returns>
        private EdmMember GetPropertyElement(string containerName, string entityTypeName, string propertyName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            var entityTypeElement = GetEntityTypeElement(containerName, entityTypeName);
            var propertyElement = entityTypeElement.Members.FirstOrDefault(c => c.Name.Equals(propertyName));
            if (propertyElement == null)
            {
                throw new ObjectNotFoundException(string.Format("Property:{0} not found.", propertyName));
            }
            return propertyElement;
        }

        /// <summary>
        /// Gets association type element
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of relationship set</param>
        /// <returns>Association type element</returns>
        private AssociationType GetAssociationTypeElement(string containerName, string relationshipSetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(relationshipSetName))
            {
                throw new ArgumentNullException("relationshipSetName");
            }
            var associationSetElement = GetAssociationSetElement(containerName, relationshipSetName);
            if (associationSetElement == null)
            {
                throw new ObjectNotFoundException(string.Format("AssociationSet:{0} not found.", relationshipSetName));
            }
            return associationSetElement.ElementType;
        }

        /// <summary>
        /// Gets whether navigation property is collection type
        /// </summary>
        /// <param name="property">Navigation property</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Whether navigation property is collection type</returns>
        private bool IsCollectionNavigationProperty(string containerName, string entityTypeName, NavigationProperty navigationProperty)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            if (navigationProperty == null)
            {
                throw new ArgumentNullException("navigationProperty");
            }
            var associationType = GetAssociationTypeElement(containerName, navigationProperty.RelationshipType.ToString());

            var entityEnd = associationType.RelationshipEndMembers.Where(c => entityTypeName.Equals(c.GetEntityType().Name)).FirstOrDefault();
            if (entityEnd == null || entityEnd.RelationshipMultiplicity != RelationshipMultiplicity.One)
            {
                return false;
            }
            entityEnd = associationType.RelationshipEndMembers.Where(c => !entityTypeName.Equals(c.GetEntityType().Name)).FirstOrDefault();
            if (entityEnd == null || entityEnd.RelationshipMultiplicity != RelationshipMultiplicity.Many)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets list of name of entity containers
        /// </summary>
        /// <returns>List of name of entity containers</returns>
        internal List<string> GetEntityContainerNames()
        {
            var context = Metadata.GetItems<EntityContainer>(DataSpace.CSpace);
            return context.Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets list of name of entity sets
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <returns>List of name of entity sets</returns>
        internal List<string> GetEntitySetNames(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            var containerElement = GetEntityContainerElement(containerName);
            return containerElement.BaseEntitySets.Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets type of entity set
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Type of entity set</returns>
        internal EntityType GetEntitySetType(string containerName, string entitySetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            return GetEntitySetElement(containerName, entitySetName).ElementType;
        }

        /// <summary>
        /// Gets list of name of entity sets which type is specialfied type
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of entity sets</returns>
        internal List<string> GetEntitySetNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var containerElement = GetEntityContainerElement(containerName);
            return containerElement.BaseEntitySets.Where(c => c.ElementType.Name.Equals(entityTypeName)).Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets entity set name of association role
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <param name="roleName">Name of role</param>
        /// <returns>Name of entity set</returns>
        internal string GetAssociationSetEndEntitySetName(string containerName, string relationshipSetName, string roleName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(relationshipSetName))
            {
                throw new ArgumentNullException("relationshipSetName");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            return GetAssociationSetEndElement(containerName, relationshipSetName, roleName).EntitySet.Name;
        }

        /// <summary>
        /// Gets list of name of key properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of key members</returns>
        internal List<string> GetKeyPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeElement = GetEntityTypeElement(containerName, entityTypeName);
            return entityTypeElement.KeyMembers.Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets list of name of properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of keys</returns>
        internal List<string> GetPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeElement = GetEntityTypeElement(containerName, entityTypeName);
            return entityTypeElement.Members.Where(c=> c is EdmProperty).Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets list of name of navigation properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of navigation properties</returns>
        internal List<string> GetNavigationPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeElement = GetEntityTypeElement(containerName, entityTypeName);
            return entityTypeElement.Members.Where(c => c is NavigationProperty).Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets list of name of navigation properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of navigation properties</returns>
        internal List<string> GetCollectionNavigationPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeElement = GetEntityTypeElement(containerName, entityTypeName);
            return entityTypeElement.Members
                .Where(c => c is NavigationProperty && IsCollectionNavigationProperty(containerName, entityTypeName, c as NavigationProperty))
                .Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets type of property
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Type of property</returns>
        internal EdmType GetPropertyType(string containerName, string entityTypeName, string propertyName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            var memberElement = GetPropertyElement(containerName, entityTypeName, propertyName);
            return memberElement.TypeUsage.EdmType;
        }

        /// <summary>
        /// Gets association referential constraint
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of navigation property</param>
        /// <returns>Association referential constraint</returns>
        internal Dictionary<string, string> GetAssociationReferentialConstraint(string containerName, string entityTypeName, string propertyName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            var constraints = new Dictionary<string, string>();
            var memberElement = GetPropertyElement(containerName, entityTypeName, propertyName);
            if (memberElement is NavigationProperty)
            {
                var navigationProperty = memberElement as NavigationProperty;
                var associationType = GetAssociationTypeElement(containerName, navigationProperty.RelationshipType.ToString());
                foreach (var constraint in associationType.ReferentialConstraints)
                {
                    if(entityTypeName.Equals(constraint.FromRole.GetEntityType().Name))
                    {
                        for (int i = 0; i < constraint.FromProperties.Count; i++)
                        {
                            constraints[constraint.FromProperties[0].Name] = constraint.ToProperties[0].Name;
                        }
                    }
                    else if(entityTypeName.Equals(constraint.ToRole.GetEntityType().Name))
                    {
                        for (int i = 0; i < constraint.ToProperties.Count; i++)
                        {
                            constraints[constraint.ToProperties[0].Name] = constraint.FromProperties[0].Name;
                        }
                    }
                }
                return constraints;
            }
            else
            {
                throw new EntityException(string.Format("Property:{0} is not navigation type.", propertyName));
            }
        }
    }
}