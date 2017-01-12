using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EFWCFModule;
using System.Reflection;
using System.Data.Metadata.Edm;
using System.Data;

namespace EFServerTools
{
    /// <summary>
    /// EFModule
    /// </summary>
    public class EFModule : Component, IEFModule
    {
        #region IEFModule Members
        private ClientInfo _clientInfo;
        /// <summary>
        /// Gets or sets information of client
        /// </summary>
        public ClientInfo ClientInfo
        {
            get
            {
                return _clientInfo;
            }
            set
            {
                _clientInfo = value;
            }
        }

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <returns>List of command names</returns>
        public List<string> GetCommandNames()
        {
            return this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(c => typeof(ICommand).IsAssignableFrom(c.FieldType)).Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets container name of entity
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <returns>Container name of entity</returns>
        public string GetEntityContainerName(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                return command.ContextName;
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets name of entity object class
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Name of entity object class</returns>
        public string GetObjectClassName(string commandName, string entitySetName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entitySetName))
                {
                    entitySetName = command.EntitySetName;
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                return provider.GetEntitySetType(command.ContextName, entitySetName).Name;
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets primary keys of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Primary keys of entity object</returns>
        public List<string> GetEntityPrimaryKeys(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entityTypeName))
                {
                    entityTypeName = GetObjectClassName(commandName, null);
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                return provider.GetKeyPropertyNames(command.ContextName, entityTypeName);
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetEntityFields(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entityTypeName))
                {
                    entityTypeName = GetObjectClassName(commandName, null);
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                return provider.GetPropertyNames(command.ContextName, entityTypeName);
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public Dictionary<String, int> GetEntityFieldsLength(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entityTypeName))
                {
                    entityTypeName = GetObjectClassName(commandName, null);
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                return null;
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldTypes(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entityTypeName))
                {
                    entityTypeName = GetObjectClassName(commandName, null);
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                var listProperties = provider.GetPropertyNames(command.ContextName, entityTypeName);
                var fieldTypes = new Dictionary<string, string>();
                foreach (var property in listProperties)
                {
                    fieldTypes[property] = provider.GetPropertyType(command.ContextName, entityTypeName, property).ToString();
                }
                return fieldTypes;
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets mapping of fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Mapping of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldMappings(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entityTypeName))
                {
                    entityTypeName = GetObjectClassName(commandName, null);
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                var listProperties = provider.GetNavigationPropertyNames(command.ContextName, entityTypeName);
                var fieldMappings = new Dictionary<string, string>();
                foreach (var property in listProperties)
                {
                    var constraints =  provider.GetAssociationReferentialConstraint(command.ContextName, entityTypeName, property);
                    foreach (var constraint in constraints)
                    {
                        if (!fieldMappings.ContainsKey(constraint.Key))
                        {
                            fieldMappings[constraint.Key] = string.Format("{0}.{1}", property, constraint.Value);
                        }
                    }
                }
                return fieldMappings;
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        public List<string> GetEntityNavigationFields(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            try
            {
                if (string.IsNullOrEmpty(entityTypeName))
                {
                    entityTypeName = GetObjectClassName(commandName, null);
                }
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                return provider.GetCollectionNavigationPropertyNames(command.ContextName, entityTypeName);
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets list of name of entity sets which type is specialfied type
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Name of entity object</returns>
        public List<string> GetEntitySetNames(string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var command = GetCommand(commandName);
            try
            {
                var metadata = MetadataProvider.CreateMetadata(command, this.GetType().Assembly);
                var provider = new MetadataProvider(metadata);
                return provider.GetEntitySetNames(command.ContextName, entityTypeName);
            }
            finally
            {
                //System.Data.Metadata.Edm.MetadataWorkspace.ClearCache();
            }
        }

        /// <summary>
        /// Gets count of entity objects
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of entity objects</returns>
        public int GetObjectCount(string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }
            var command = GetCommand(commandName);
            using (var context = EntityProvider.CreateContext(ClientInfo, command, this.GetType().Assembly))
            {
                context.Connection.Open();
                command.Context = context;
                command.Module = this;
                if (command.CommandType == CommandType.Text)
                {
                    var objects = command.GetObjects(ClientInfo);
                    objects = EntityProvider.SetWhere(objects, packetInfo.WhereParameters);
                    return objects.Count();
                }
                else if (command.CommandType == CommandType.StoredProcedure)
                {
                    var objects = new List<System.Data.Objects.DataClasses.EntityObject>();
                    foreach (System.Data.Objects.DataClasses.EntityObject item in (System.Collections.IEnumerable)command.ExecuteStoredProcedure())
                    {
                        objects.Add(item);
                    }
                    return objects.Count;
                }
                else
                {
                    throw new NotSupportedException(string.Format("CommandType:{0} not supported.", command.CommandType));
                }
            }
        }

        /// <summary>
        /// Gets a list of entity objects
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>List of entity objects</returns>
        public List<System.Data.Objects.DataClasses.EntityObject> GetObjects(string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }
            
            var command = GetCommand(commandName);
            using (var context = EntityProvider.CreateContext(ClientInfo, command, this.GetType().Assembly))
            {
                context.Connection.Open();
                command.Context = context;
                command.Module = this;
                if (command.CommandType == CommandType.Text)
                {
                    var objects = command.GetObjects(ClientInfo);

                    //where
                    if (packetInfo.WhereParameters != null)
                    {
                        objects = EntityProvider.SetWhere(objects, packetInfo.WhereParameters);
                    }
                    //order by
                    if (packetInfo.OrderParameters != null)
                    {
                        objects = EntityProvider.SetOrder(objects, packetInfo.OrderParameters);
                    }
                    //package records
                    return EntityProvider.GetPackages(objects, command.ContextName, command.EntitySetName, packetInfo.StartIndex, packetInfo.Count).ToList();
                }
                else if (command.CommandType == CommandType.StoredProcedure)
                {
                    var objects = new List<System.Data.Objects.DataClasses.EntityObject>();
                    foreach (System.Data.Objects.DataClasses.EntityObject item in (System.Collections.IEnumerable)command.ExecuteStoredProcedure())
                    {
                        objects.Add(item);
                    }
                    return objects;
                }
                else
                {
                    throw new NotSupportedException(string.Format("CommandType:{0} not supported.", command.CommandType));
                }
            }

        }

        /// <summary>
        /// Gets entity object by key
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="keyValues">Key and values</param>
        /// <returns>Entity object</returns>
        public object GetObjectByKey(string commandName, string entitySetName, Dictionary<string, object> keyValues)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (keyValues == null)
            {
                throw new ArgumentNullException("keyValues");
            }
            var command = GetCommand(commandName);
            using (var context = EntityProvider.CreateContext(ClientInfo, command, this.GetType().Assembly))
            {
                context.Connection.Open();
                if (string.IsNullOrEmpty(entitySetName))
                {
                    command.Context = context;
                    command.Module = this;
                    return command.GetObjectByKey(keyValues);
                }
                else
                {
                    var entityKey = new EntityKey(string.Format("{0}.{1}", context.DefaultContainerName, entitySetName), keyValues);
                    return context.GetObjectByKey(entityKey);
                }
            }

        }

        /// <summary>
        /// Gets a list of detail entity objects
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="masterObject">Master entity object</param>
        /// <returns>Master entity object with detail entity objects</returns>
        public void GetDetailObjects(string commandName, System.Data.Objects.DataClasses.EntityObject masterObject)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (masterObject == null)
            {
                throw new ArgumentNullException("masterObject");
            }
            var command = GetCommand(commandName);
            var objectCommand = GetObjectCommand(command, masterObject);
            if (objectCommand == null)
            {
                throw new ObjectNotFoundException(string.Format("Command:{0} not found", commandName));
            }
            var returnObject = masterObject;
            using (var context = EntityProvider.CreateContext(ClientInfo, command, this.GetType().Assembly))
            {
                context.Connection.Open();
                var relations = GetComponent<IRelation>(objectCommand);
                foreach (var relation in relations)
                {
                    relation.Context = context;
                    relation.Module = this;
                    relation.GetDetailObjects(masterObject, ClientInfo);
                }
            }
        }

        /// <summary>
        /// Update entity objects to database
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="objects">List of entity objects</param>
        /// <param name="states">State of entity objects</param>
        /// <returns>Count of rows affected</returns>
        public int UpdateObjects(string commandName, List<System.Data.Objects.DataClasses.EntityObject> objects, Dictionary<EntityKey, EntityState> states)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            var command = GetCommand(commandName);
            using (var context = EntityProvider.CreateContext(ClientInfo, command, this.GetType().Assembly))
            {
                context.Connection.Open();
                command.Context = context;
                command.Module = this;

                var count = UpdateObjectsByCommand(command, objects, states, null, context);

                context.SaveChanges();



                if (command.ServerModify)
                {
                    #region Refresh data include details
                    for (int i = 0; i < objects.Count; i++)
                    {
                        var obj = objects[i];
                        if (obj.EntityKey != null && obj.EntityKey.EntitySetName.Equals(command.EntitySetName))
                        {
                            if (!states.ContainsKey(obj.EntityKey) || !states[obj.EntityKey].Equals(EntityState.Deleted))
                            {
                                var keyValues = new Dictionary<string, object>();
                                foreach (var keyValue in obj.EntityKey.EntityKeyValues)
                                {
                                    keyValues.Add(keyValue.Key, keyValue.Value);
                                }

                                objects[i] = (System.Data.Objects.DataClasses.EntityObject)command.GetObjectByKey(keyValues);
                            }
                        }
                    }
                    #endregion
                }

                return count;
            }
        }

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        public object CallMethod(string methodName, object[] param)
        {
            if(string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null)
            {
                throw new MissingMethodException(this.GetType().Name, methodName);
            }
            if (method.GetParameters().Count() == 0)
            {
                return method.Invoke(this, null);
            }
            else if (method.GetParameters().Count() == 1)
            {
                return method.Invoke(this, new object[] { param });
            }
            else
            {
                throw new ArgumentOutOfRangeException("method.GetParameters().Count()", method.GetParameters().Count(), "Value must be equal to 0 or 1.");
            }
        }

        #endregion

        /// <summary>
        /// Find command
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <returns>Command</returns>
        private ICommand GetCommand(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            { 
                throw new ArgumentNullException("commandName");
            }
            var field = this.GetType().GetField(commandName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null && typeof(ICommand).IsAssignableFrom(field.FieldType))
            {
                return (ICommand)field.GetValue(this);
            }
            else
            {
                throw new ObjectNotFoundException(string.Format("Command:{0} not found.", commandName));
            }
        }

        /// <summary>
        /// Gets a list of components related to command
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        /// <param name="commandName">Name of command</param>
        /// <returns>List of components related to command</returns>
        private List<T> GetComponent<T>(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var command = GetCommand(commandName);
            return GetComponent<T>(command);
        }

        /// <summary>
        /// Gets a list of components related to command
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        /// <param name="command">Command</param>
        /// <returns>List of components related to command</returns>
        internal List<T> GetComponent<T>(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            var list = new List<T>();

            if (command is T)
            {
                list.Add((T)command);
            }
            else
            {
                var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    if (typeof(T).IsAssignableFrom(field.FieldType))
                    {
                        var component = field.GetValue(this);
                        var relatedCommand = GetRelatedCommand(component);
                        if (relatedCommand != null && command.Equals(relatedCommand))
                        {
                            list.Add((T)component);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Gets command related to component
        /// </summary>
        /// <param name="component">Component</param>
        /// <returns>Command</returns>
        private ICommand GetRelatedCommand(object component)
        {
            if (component == null)
            {
                return null;
            }
            if (component is ICommand)
            {
                return component as ICommand;
            }
            else if (component is IUseCommand)
            {
                return (component as IUseCommand).Command;
            }
            else if (component is IUseUpdateComponent)
            {
                var updateComponent = (component as IUseUpdateComponent).UpdateComponent;
                if (updateComponent == null)
                {
                    return null;
                }
                return GetRelatedCommand(updateComponent);
            }
            else if (component is IRelation)
            {
                return (component as IRelation).MasterCommand;
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Gets command which objects belong to 
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="masterObject">Master entity object</param>
        /// <returns>Command</returns>
        private ICommand GetObjectCommand(ICommand command, System.Data.Objects.DataClasses.EntityObject masterObject)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (masterObject == null)
            {
                throw new ArgumentNullException("masterObject");
            }
            if (masterObject.EntityKey == null)
            {
                throw new ArgumentNullException("masterObject.EntityKey");
            }
            if (string.Compare(command.EntitySetName, masterObject.EntityKey.EntitySetName) == 0)
            {
                return command;
            }
            else
            {
                var relations = GetComponent<IRelation>(command);
                foreach (var relation in relations)
                {
                    ICommand objectCommand = GetObjectCommand(relation.DetailCommand, masterObject);
                    if (objectCommand != null)
                    {
                        return objectCommand;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Update entity objects to database
        /// </summary>
        /// <param name="command">ICommand</param>
        /// <param name="objects">List of entity objects</param>
        /// <param name="states">State of entity objects</param>
        /// <param name="masterEntitySetName">Name of master entity set</param>
        /// <param name="context">Object context</param>
        private int UpdateObjectsByCommand(ICommand command, List<System.Data.Objects.DataClasses.EntityObject> objects, Dictionary<EntityKey, EntityState> states, string masterEntitySetName, System.Data.Objects.ObjectContext context)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(command.EntitySetName))
            {
                throw new ArgumentNullException("command.EntitySetName");
            }

            int count = 0;

            var listObjects = new List<System.Data.Objects.DataClasses.EntityObject>();
            foreach (var obj in objects)
            {
                if (obj.EntityKey == null && masterEntitySetName == null)
                {
                    listObjects.Add(obj);
                }
                else if (obj.EntityKey != null && command.EntitySetName.Equals(obj.EntityKey.EntitySetName))
                {
                    listObjects.Add(obj);
                }
            }
            UpdateObjects(command, listObjects, states, masterEntitySetName, context);
            var relations = GetComponent<IRelation>(command);
            foreach (var relation in relations)
            {
                count += UpdateObjectsByCommand(relation.DetailCommand, objects, states, command.EntitySetName, context);
            }

            return count;
        }

        /// <summary>
        /// Update entity objects to database
        /// </summary>
        /// <param name="command">ICommand</param>
        /// <param name="objects">List of entity objects</param>
        /// <param name="states">State of entity objects</param>
        /// <param name="masterEntitySetName">Name of master entity set</param>
        /// <param name="context">Object context</param>
        /// <returns>Count of rows affected</returns>
        private int UpdateObjects(ICommand command, List<System.Data.Objects.DataClasses.EntityObject> objects, Dictionary<EntityKey, EntityState> states, string masterEntitySetName, System.Data.Objects.ObjectContext context)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var autoNubmers = GetComponent<IAutoNumber>(command);
            foreach (var autoNumber in autoNubmers)
            {
                autoNumber.Context = context;
                autoNumber.Module = this;
                autoNumber.Execute(objects, states);
            }

            int count = 0;

            var updateComponents = GetComponent<IUpdateComponent>(command);
            foreach (var updateComponent in updateComponents)
            {
                updateComponent.Context = context;
                updateComponent.Module = this;
                count += updateComponent.Update(objects, states, masterEntitySetName);
            }

            var transcations = GetComponent<ITransaction>(command);
            foreach (var transcation in transcations)
            {
                transcation.Context = context;
                transcation.Module = this;
                transcation.Execute(objects, states);
            }

            var logs = GetComponent<ILog>(command);
            foreach (var log in logs)
            {
                log.Context = context;
                log.Module = this;
                log.Log(objects, states);
            }

            var relations = GetComponent<IRelation>(command);
            foreach (var relation in relations)
            {
                //get detail objects
                var detailObjects = EntityProvider.GetDetailObjects(context, objects, relation.DetailCommand.EntitySetName);
                count += UpdateObjects(relation.DetailCommand, detailObjects, states, relation.MasterCommand.EntitySetName, context);
            }

            return count;
        }

        public object GetExpressionValue(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return string.Empty;
            }
            return null;

        }
    }
}