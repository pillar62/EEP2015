using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.Metadata.Edm;

namespace EFServerTools.Design
{
    /// <summary>
    /// Provider of meatadata workspace(design time)
    /// </summary>
    public class MetadataProvider
    {
        /// <summary>
        /// Gets a list of edmx file name
        /// </summary>
        /// <param name="directory">Name of directory</param>
        /// <returns>List of edmx file name</returns>
        public static List<string> GetMetadataFiles(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException("directory");
            }
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(string.Format("Directory:{0} not found.", directory));
            }
            return Directory.GetFiles(directory, "*.edmx", SearchOption.TopDirectoryOnly).Select(c => Path.GetFileNameWithoutExtension(c)).ToList();
        }

        /// <summary>
        /// Creates a new instance of medatadata provider
        /// </summary>
        /// <param name="directory">Name of directory</param>
        /// <param name="metadataFile">Name of demx file</param>
        public MetadataProvider(string directory, string metadataFile)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException("directory");
            }
            if (string.IsNullOrEmpty(metadataFile))
            {
                throw new ArgumentNullException("metadataFile");
            }
            var path = string.Format(@"{0}\{1}.edmx", directory, metadataFile);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Edmx file not found.", path);
            }
            _xml = new XmlDocument();
            _xml.Load(path);
            _fileName = path;
        }

        #region Const String
        /// <summary>
        /// 1.0
        /// </summary>
        const string VERSION1 = "1.0";
        /// <summary>
        /// 2.0
        /// </summary>
        const string VERSION2 = "2.0";

        /// <summary>
        /// edmx
        /// </summary>
        const string EDMX_PREFIX = "edmx";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2007/06/edmx
        /// </summary>
        const string EDMX_URI1 = "http://schemas.microsoft.com/ado/2007/06/edmx";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2008/10/edmx
        /// </summary>
        const string EDMX_URI2 = "http://schemas.microsoft.com/ado/2008/10/edmx";
        /// <summary>
        /// csdl
        /// </summary>
        const string CSDL_PREFIX = "csdl";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2006/04/edm
        /// </summary>
        const string CSDL_URI1 = "http://schemas.microsoft.com/ado/2006/04/edm";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2006/04/edm
        /// </summary>
        const string CSDL_URI2 = "http://schemas.microsoft.com/ado/2008/09/edm";
        /// <summary>
        /// ssdl
        /// </summary>
        const string SSDL_PREFIX = "ssdl";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2006/04/edm/ssdl
        /// </summary>
        const string SSDL_URI1 = "http://schemas.microsoft.com/ado/2006/04/edm/ssdl";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2009/02/edm/ssdl
        /// </summary>
        const string SSDL_URI2 = "http://schemas.microsoft.com/ado/2009/02/edm/ssdl";
        /// <summary>
        /// mapping
        /// </summary>
        const string MAPPING_PREFIX = "mapping";
        /// <summary>
        /// urn:schemas-microsoft-com:windows:storage:mapping:CS
        /// </summary>
        const string MAPPING_URI1 = "urn:schemas-microsoft-com:windows:storage:mapping:CS";
        /// <summary>
        /// http://schemas.microsoft.com/ado/2008/09/mapping/cs
        /// </summary>
        const string MAPPING_URI2 = "http://schemas.microsoft.com/ado/2008/09/mapping/cs";

        /// <summary>
        /// EDMX
        /// </summary>
        const string EDMX_NODE = "Edmx";
        /// <summary>
        /// Runtime
        /// </summary>
        const string RUNTIME_NODE = "Runtime";
        /// <summary>
        /// StorageModels
        /// </summary>
        const string STORAGE_MODELS_NODE = "StorageModels";
        /// <summary>
        /// ConceptualModels
        /// </summary>
        const string CONCEPTUAL_MODELS_NODE = "ConceptualModels";
        /// <summary>
        /// Schema
        /// </summary>
        const string SCHEMA_NODE = "Schema";
        /// <summary>
        /// EntityContainer
        /// </summary>
        const string ENTITY_CONTAINER_NODE = "EntityContainer";
        /// <summary>
        /// EntitySet
        /// </summary>
        const string ENTITY_SET_NODE = "EntitySet";
        /// <summary>
        /// AssociationSet
        /// </summary>
        const string ASSOCIATION_NODE = "AssociationSet";
        /// <summary>
        /// End
        /// </summary>
        const string END_NODE = "End";
        /// <summary>
        /// EntityType
        /// </summary>
        const string ENTITY_TYPE_NODE = "EntityType";
        /// <summary>
        /// Key
        /// </summary>
        const string KEY_NODE = "Key";
        /// <summary>
        /// PropertyRef
        /// </summary>
        const string PROPERTY_REF_NODE = "PropertyRef";
        /// <summary>
        /// Property
        /// </summary>
        const string PROPERTY_NODE = "Property";
        /// <summary>
        /// NavigationProperty
        /// </summary>
        const string NAVIGATION_PROPERTY_NODE = "NavigationProperty";
        /// <summary>
        /// Mappings
        /// </summary>
        const string MAPPINGS_NODE = "Mappings";
        /// <summary>
        /// Mapping
        /// </summary>
        const string MAPPING_NODE = "Mapping";
        /// <summary>
        /// EntityContainerMapping
        /// </summary>
        const string ENTITY_CONTAINER_MAPPING_NODE = "EntityContainerMapping";
        /// <summary>
        /// EntitySetMapping
        /// </summary>
        const string ENTITY_SET_MAPPING_NODE = "EntitySetMapping";
        /// <summary>
        /// EntityTypeMapping
        /// </summary>
        const string ENTITY_TYPE_MAPPING_NODE = "EntityTypeMapping";
        /// <summary>
        /// MappingFragment
        /// </summary>
        const string MAPPING_FRAGMENT_NODE = "MappingFragment";
        /// <summary>
        /// ScalarProperty
        /// </summary>
        const string SCALAR_PROPERTY_NODE = "ScalarProperty";

        /// <summary>
        /// Version
        /// </summary>
        const string VERSION_PROPERY = "Version";
        /// <summary>
        /// Name
        /// </summary>
        const string NAME_PROPERTY = "Name";
        /// <summary>
        /// Type
        /// </summary>
        const string TYPE_PROPERTY = "Type";
        /// <summary>
        /// NameSpace
        /// </summary>
        const string NAMESPACE_PROPERTY = "Namespace";
        /// <summary>
        /// EntityType
        /// </summary>
        const string ENTITY_TYPE_PROPERTY = "EntityType"; 
        /// <summary>
        /// Association
        /// </summary>
        const string ASSOCIATION_PROPERTY = "Association";
        /// <summary>
        /// Role
        /// </summary>
        const string ROLE_PROPERTY = "Role";
        /// <summary>
        /// EntitySet
        /// </summary>
        const string ENTITY_SET_PROPERTY = "EntitySet";
        /// <summary>
        /// StorageEntityContainer
        /// </summary>
        const string STORE_ENTITY_CONTAINER_PROPERTY = "StorageEntityContainer";
        /// <summary>
        /// CdmEntityContainer
        /// </summary>
        const string CDM_ENTITY_CONTAINER_PROPERTY = "CdmEntityContainer";
        /// <summary>
        /// TypeName
        /// </summary>
        const string TYPE_NAME_PROPERTY = "TypeName";

        /// <summary>
        /// StoreEntitySet
        /// </summary>
        const string STORE_ENTITY_SET_PROPERTY = "StoreEntitySet";
        /// <summary>
        /// ColumnName
        /// </summary>
        const string COLUMN_NAME_PROPERTY ="ColumnName";

        #endregion

        private string _fileName;
        /// <summary>
        /// Gets name of edmx file
        /// </summary>
        private string FileName
        {
            get
            {
                return _fileName;
            }
        }

        private XmlDocument _xml;
        /// <summary>
        /// Gets xml document of provider
        /// </summary>
        private XmlDocument Xml 
        {
            get 
            {
                return _xml;
            }
        }

        private string _version;
        /// <summary>
        /// Gets version of edmx
        /// </summary>
        private string Version
        {
            get
            {
                if (string.IsNullOrEmpty(_version))
                {
                    var edmxNode = GetEdmxNode();
                    _version = GetNodeAttributeValue(edmxNode, VERSION_PROPERY);
                }
                return _version;
            }
        }

        //private string _nameSpace;
        ///// <summary>
        ///// Gets namespace of schema
        ///// </summary>
        //private string NameSpace
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_nameSpace))
        //        {
        //            var schemaNode = GetSchemaNode();
        //            _nameSpace = GetNodeAttributeValue(schemaNode, NAMESPACE_PROPERTY);
        //        }
        //        return _nameSpace;
        //    }
        //}

        private XmlNamespaceManager _namespaceManager;
        /// <summary>
        /// Gets namespace manager of xml
        /// </summary>
        private XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (_namespaceManager == null)
                {
                    _namespaceManager = new XmlNamespaceManager(Xml.NameTable);
                    if (Version.Equals(VERSION1))
                    {
                        _namespaceManager.AddNamespace(EDMX_PREFIX, EDMX_URI1);
                        _namespaceManager.AddNamespace(SSDL_PREFIX, SSDL_URI1);
                        _namespaceManager.AddNamespace(CSDL_PREFIX, CSDL_URI1);
                        _namespaceManager.AddNamespace(MAPPING_PREFIX, MAPPING_URI1);
                    }
                    else if (Version.Equals(VERSION2))
                    {
                        _namespaceManager.AddNamespace(EDMX_PREFIX, EDMX_URI2);
                        _namespaceManager.AddNamespace(SSDL_PREFIX, SSDL_URI2);
                        _namespaceManager.AddNamespace(CSDL_PREFIX, CSDL_URI2);
                        _namespaceManager.AddNamespace(MAPPING_PREFIX, MAPPING_URI2);
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("Edmx:{0} not supported.", Version));
                    }
                }
                return _namespaceManager;
            }
        }

        /// <summary>
        /// Gets attribute value of node
        /// </summary>
        /// <param name="node">Xmlnode</param>
        /// <param name="attributeName">Name of attribute</param>
        /// <returns>Attribute value of node</returns>
        private string GetNodeAttributeValue(XmlNode node, string attributeName)
        {
            return GetNodeAttributeValue(node, attributeName, null);
        }

        /// <summary>
        /// Gets attribute value of node
        /// </summary>
        /// <param name="node">Xmlnode</param>
        /// <param name="attributeName">Name of attribute</param>
        /// <param name="namespaceURI">URI of namespace</param>
        /// <returns>Attribute value of node</returns>
        private string GetNodeAttributeValue(XmlNode node, string attributeName, string namespaceURI)
        {
            if (string.IsNullOrEmpty(namespaceURI))
            {
                return node.Attributes[attributeName] == null ? null : node.Attributes[attributeName].Value;
            }
            else
            {
                return node.Attributes[attributeName, namespaceURI] == null ? null : node.Attributes[attributeName, namespaceURI].Value;
            }
        }

        /// <summary>
        ///  Gets namespace of schema
        /// </summary>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Namespace of schema</returns>
        private string GetNameSpace(DataSpace dataSpace)
        {
            var schemaNode = GetSchemaNode(dataSpace);
            return GetNodeAttributeValue(schemaNode, NAMESPACE_PROPERTY);
        }

        /// <summary>
        /// Get prefix for data space
        /// </summary>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Prefix for data space</returns>
        private string GetSchemaPrefix(DataSpace dataSpace)
        {
            if (dataSpace == DataSpace.CSSpace)
            {
                return CSDL_PREFIX;
            }
            else if (dataSpace == DataSpace.SSpace)
            {
                return SSDL_PREFIX;
            }
            else
            {
                throw new NotSupportedException(string.Format("DataSpace:{0} not supported.", dataSpace));
            }
        }

        private string GetModuleName(DataSpace dataSpace)
        {
            if (dataSpace == DataSpace.CSSpace)
            {
                return CONCEPTUAL_MODELS_NODE;
            }
            else if (dataSpace == DataSpace.SSpace)
            {
                return STORAGE_MODELS_NODE;
            }
            else
            {
                throw new NotSupportedException(string.Format("DataSpace:{0} not supported.", dataSpace));
            }
        }

        /// <summary>
        /// Gets edmx node
        /// </summary>
        /// <returns>edmx node</returns>
        private XmlNode GetEdmxNode()
        {
            var edmxNode = Xml.DocumentElement;
            if (edmxNode == null)
            {
                throw new BadImageFormatException("edmx node not found.", FileName);
            }
            return edmxNode;
        }

        /// <summary>
        /// Gets schema node (conceptual models)
        /// </summary>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Schema node</returns>
        private XmlNode GetSchemaNode()
        {
            return GetSchemaNode(DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets schema node
        /// </summary>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Schema node</returns>
        private XmlNode GetSchemaNode(DataSpace dataSpace)
        {
            var schemaNode = Xml.SelectSingleNode(string.Format("{0}:{2}/{0}:{3}/{0}:{4}/{1}:{5}"
               , EDMX_PREFIX, GetSchemaPrefix(dataSpace), EDMX_NODE, RUNTIME_NODE, GetModuleName(dataSpace), SCHEMA_NODE), NamespaceManager);
            if (schemaNode == null)
            {
                throw new BadImageFormatException("Schema node not found.", FileName);
            }
            return schemaNode;
        }

        /// <summary>
        /// Gets entity container node (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <returns>Entity container node</returns>
        private XmlNode GetEntityContainerNode(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            return GetEntityContainerNode(containerName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets entity container node 
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Entity container node</returns>
        private XmlNode GetEntityContainerNode(string containerName, DataSpace dataSpace)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            var schemaNode = GetSchemaNode(dataSpace);
            var containerNodes = schemaNode.SelectNodes(string.Format("{0}:{1}", GetSchemaPrefix(dataSpace), ENTITY_CONTAINER_NODE), NamespaceManager);
            foreach (XmlNode containerNode in containerNodes)
	        {
                if (containerName.Equals(GetNodeAttributeValue(containerNode, NAME_PROPERTY)))
                {
                    return containerNode;
                }
	        }
            throw new ObjectNotFoundException(string.Format("Container:{0} not found.", containerName));
        }

        /// <summary>
        /// Gets entity set node (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Entity set node</returns>
        private XmlNode GetEntitySetNode(string containerName, string entitySetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            return GetEntitySetNode(containerName, entitySetName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets entity set node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Entity set node</returns>
        private XmlNode GetEntitySetNode(string containerName, string entitySetName, DataSpace dataSpace)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            var containerNode = GetEntityContainerNode(containerName, dataSpace);
            var entitySetNodes = containerNode.SelectNodes(string.Format("{0}:{1}", GetSchemaPrefix(dataSpace), ENTITY_SET_NODE), NamespaceManager);
            foreach (XmlNode entitySetNode in entitySetNodes)
            {
                if (entitySetName.Equals(GetNodeAttributeValue(entitySetNode, NAME_PROPERTY)))
                {
                    return entitySetNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("EntitySet:{0} not found.", entitySetName));
        }

        /// <summary>
        /// Gets association set node (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <returns>Association set node</returns>
        private XmlNode GetAssociationSetNode(string containerName, string relationshipSetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(relationshipSetName))
            {
                throw new ArgumentNullException("relationshipSetName");
            }
            return GetAssociationSetNode(containerName, relationshipSetName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets association set node 
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Association set node</returns>
        private XmlNode GetAssociationSetNode(string containerName, string relationshipSetName, DataSpace dataSpace)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(relationshipSetName))
            {
                throw new ArgumentNullException("relationshipSetName");
            }
            var containerNode = GetEntityContainerNode(relationshipSetName, dataSpace);
            var associationSetNodes = containerNode.SelectNodes(string.Format("{0}:{1}", GetSchemaPrefix(dataSpace), ASSOCIATION_NODE), NamespaceManager);
            foreach (XmlNode associationSetNode in associationSetNodes)
            {
                if (relationshipSetName.Equals(GetNodeAttributeValue(associationSetNode, ASSOCIATION_PROPERTY)))
                {
                    return associationSetNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("AssociationSet:{0} not found.", relationshipSetName));
        }

        /// <summary>
        /// Gets association set end node (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <param name="roleName">Name of role</param>
        /// <returns>Association set end node</returns>
        private XmlNode GetAssociationSetEndNode(string containerName, string relationshipSetName, string roleName)
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
            return GetAssociationSetEndNode(containerName, relationshipSetName, roleName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets association set end node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <param name="roleName">Name of role</param>
        /// <param name="entityTypeName">Full name of entity set type</param>
        /// <returns>Association set end node</returns>
        private XmlNode GetAssociationSetEndNode(string containerName, string relationshipSetName, string roleName, DataSpace dataSpace)
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
            var associationSetNode = GetAssociationSetNode(containerName, relationshipSetName, dataSpace);
            var associationSetEndNodes = associationSetNode.SelectNodes(string.Format("{0}:{1}", GetSchemaPrefix(dataSpace), END_NODE), NamespaceManager);
            foreach (XmlNode associationSetEndNode in associationSetEndNodes)
            {
                if (roleName.Equals(GetNodeAttributeValue(associationSetEndNode, ROLE_PROPERTY)))
                {
                    return associationSetEndNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("AssociationSetEnd:{0} not found.", roleName));
        }

        /// <summary>
        /// Gets entity type node (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Full name of entity set type</param>
        /// <returns>Entity type node</returns>
        private XmlNode GetEntityTypeNode(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            return GetEntityTypeNode(containerName, entityTypeName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets entity type node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Full name of entity set type</param>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Entity type node</returns>
        private XmlNode GetEntityTypeNode(string containerName, string entityTypeName, DataSpace dataSpace)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var schemaNode = GetSchemaNode(dataSpace);
            var entityTypeNodes = schemaNode.SelectNodes(string.Format("{0}:{1}", GetSchemaPrefix(dataSpace), ENTITY_TYPE_NODE), NamespaceManager);
            foreach (XmlNode entityTypeNode in entityTypeNodes)
            {
                var name = GetNodeAttributeValue(entityTypeNode, NAME_PROPERTY);
                if (string.Format("{0}.{1}", GetNameSpace(dataSpace), name).Equals(entityTypeName))
                {
                    return entityTypeNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("EntityType:{0} not found.", entityTypeName));
        }

        /// <summary>
        /// Gets property node (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Property node</returns>
        private XmlNode GetPropertyNode(string containerName, string entityTypeName, string propertyName)
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
            return GetPropertyNode(containerName, entityTypeName, propertyName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets property node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of property</param>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Property node</returns>
        private XmlNode GetPropertyNode(string containerName, string entityTypeName, string propertyName, DataSpace dataSpace)
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
            var entityTypeNode = GetEntityTypeNode(containerName, entityTypeName, dataSpace);
            var propertyNodes = entityTypeNode.SelectNodes(string.Format("{0}:{1}", GetSchemaPrefix(dataSpace), PROPERTY_NODE), NamespaceManager);
            foreach (XmlNode propertyNode in propertyNodes)
            {
                if (propertyName.Equals(GetNodeAttributeValue(propertyNode, NAME_PROPERTY)))
                {
                    return propertyNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("Property:{0} not found.", propertyName));
        }

        /// <summary>
        /// Gets mapping node
        /// </summary>
        /// <returns>Mapping node</returns>
        private XmlNode GetMappingNode()
        {
            var mappingNode = Xml.SelectSingleNode(string.Format("{0}:{2}/{0}:{3}/{0}:{4}/{1}:{5}"
                 , EDMX_PREFIX, MAPPING_PREFIX, EDMX_NODE, RUNTIME_NODE, MAPPINGS_NODE, MAPPING_NODE), NamespaceManager);
            if (mappingNode == null)
            {
                throw new BadImageFormatException("Mapping node not found.", FileName);
            }
            return mappingNode;
        }

        /// <summary>
        /// Gets entity container mapping node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <returns>Entity container mapping node</returns>
        private XmlNode GetEntityContainerMappingNode(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            var mappingNode = GetMappingNode();
            var containerMappingNodes = mappingNode.SelectNodes(string.Format("{0}:{1}", MAPPING_PREFIX, ENTITY_CONTAINER_MAPPING_NODE), NamespaceManager);
            foreach (XmlNode containerMappingNode in containerMappingNodes)
            {
                if (containerName.Equals(GetNodeAttributeValue(containerMappingNode, CDM_ENTITY_CONTAINER_PROPERTY)))
                {
                    return containerMappingNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("ContainerMapping:{0} not found.", containerName));
        }

        /// <summary>
        /// Gets entity type mapping node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Full name of entity set type</param>
        /// <returns>Entity mapping type node</returns>
        private XmlNode GetEntityTypeMappingNode(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var containerMappingNodes = GetEntityContainerMappingNode(containerName);
            var entityTypeMappingNodes = containerMappingNodes.SelectNodes(string.Format("{0}:{1}/{0}:{2}"
                , MAPPING_PREFIX, ENTITY_SET_MAPPING_NODE, ENTITY_TYPE_MAPPING_NODE), NamespaceManager);
            foreach (XmlNode entityTypeMappingNode in entityTypeMappingNodes)
            {
                var name = GetNodeAttributeValue(entityTypeMappingNode, TYPE_NAME_PROPERTY);
                if (entityTypeName.Equals(name))
                {
                    return entityTypeMappingNode;
                }
            }
            throw new ObjectNotFoundException(string.Format("EntityTypeMapping:{0} not found.", entityTypeName));
        }

        /// <summary>
        /// Gets mapping fragment node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Mapping fragment node</returns>
        private XmlNode GetMappingFragmentNode(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeMappingNode = GetEntityTypeMappingNode(containerName, entityTypeName);
            var mappingFragmentNode = entityTypeMappingNode.SelectSingleNode(string.Format("{0}:{1}", MAPPING_PREFIX, MAPPING_FRAGMENT_NODE), NamespaceManager);
            if (mappingFragmentNode == null)
            {
                throw new ObjectNotFoundException(string.Format("MappingFragment:{0} not found.", entityTypeName));
            }
            return mappingFragmentNode;
        }

        /// <summary>
        /// Gets mapping scalar property node
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Mapping scalar property node</returns>
        private XmlNode GetMappingScalarPropertyNode(string containerName, string entityTypeName, string propertyName)
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
            var mappingFragmentNode = GetMappingFragmentNode(containerName, entityTypeName);
            var mappingScalarPropertyNodes = mappingFragmentNode.SelectNodes(string.Format("{0}:{1}", MAPPING_PREFIX, SCALAR_PROPERTY_NODE), NamespaceManager);

            foreach (XmlNode mappingScalarPropertyNode in mappingScalarPropertyNodes)
            {
                var name = GetNodeAttributeValue(mappingScalarPropertyNode, NAME_PROPERTY);
                if (name.Equals(propertyName))
                {
                    return mappingScalarPropertyNode;
                }
            }
            throw new BadImageFormatException(string.Format("MappingScalarProperty:{0} not found.", propertyName));
        }

        /// <summary>
        /// Gets list of name of entity containers
        /// </summary>
        /// <returns>List of name of entity containers</returns>
        public List<string> GetEntityContainerNames()
        {
            var schemaNode = GetSchemaNode();
            var containerNodes = schemaNode.SelectNodes(string.Format("{0}:{1}", CSDL_PREFIX, ENTITY_CONTAINER_NODE), NamespaceManager);
            var list = new List<string>();
            foreach (XmlNode containerNode in containerNodes)
            {
                var name = GetNodeAttributeValue(containerNode, NAME_PROPERTY);
                if (!string.IsNullOrEmpty(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets list of name of entity sets
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <returns>List of name of entity sets</returns>
        public List<string> GetEntitySetNames(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            var containerNode = GetEntityContainerNode(containerName);
            var eneitySetNodes = containerNode.SelectNodes(string.Format("{0}:{1}", CSDL_PREFIX, ENTITY_SET_NODE), NamespaceManager);
            var list = new List<string>();
            foreach (XmlNode entitySetNode in eneitySetNodes)
            {
                var name = GetNodeAttributeValue(entitySetNode, NAME_PROPERTY);
                if (!string.IsNullOrEmpty(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets type of entity set (conceptual models)
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Type of entity set</returns>
        public string GetEntitySetType(string containerName, string entitySetName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            return GetEntitySetType(containerName, entitySetName, DataSpace.CSSpace);
        }

        /// <summary>
        /// Gets type of entity set
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="dataSpace">Model in entity framework </param>
        /// <returns>Type of entity set</returns>
        public string GetEntitySetType(string containerName, string entitySetName, DataSpace dataSpace)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            return GetNodeAttributeValue(GetEntitySetNode(containerName, entitySetName, dataSpace), ENTITY_TYPE_PROPERTY);
        }

        /// <summary>
        /// Gets list of name of entity sets which type is specialfied type
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Full name of entity set type</param>
        /// <returns>List of name of entity sets</returns>
        public List<string> GetEntitySetNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var containerNode = GetEntityContainerNode(containerName);
            var eneitySetNodes = containerNode.SelectNodes(string.Format("{0}:{1}", CSDL_PREFIX, ENTITY_SET_NODE), NamespaceManager);
            var list = new List<string>();
            foreach (XmlNode entitySetNode in eneitySetNodes)
            {
                var name = GetNodeAttributeValue(entitySetNode, NAME_PROPERTY);
                if (!string.IsNullOrEmpty(name) && entityTypeName.Equals(GetNodeAttributeValue(entitySetNode, ENTITY_TYPE_PROPERTY)))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets entity set name of association role
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="relationshipSetName">Name of relationship</param>
        /// <param name="roleName">Name of role</param>
        /// <returns>Name of entity set</returns>
        public string GetAssociationSetEndEntitySetName(string containerName, string relationshipSetName, string roleName)
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
            return GetNodeAttributeValue(GetAssociationSetEndNode(containerName, relationshipSetName, roleName), ENTITY_SET_PROPERTY);
        }

        /// <summary>
        /// Gets list of name of key properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of key properties</returns>
        public List<string> GetKeyPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }
            var entityTypeNode = GetEntityTypeNode(containerName, entityTypeName);
            var keyMemberNodes = entityTypeNode.SelectNodes(string.Format("{0}/{1}", KEY_NODE, PROPERTY_REF_NODE));
            var list = new List<string>();
            foreach (XmlNode keyMemberNode in keyMemberNodes)
            {
                var name = GetNodeAttributeValue(keyMemberNode, NAME_PROPERTY, null);
                if (!string.IsNullOrEmpty(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets list of name of properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of keys</returns>
        public List<string> GetPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            var entityTypeNode = GetEntityTypeNode(containerName, entityTypeName);
            var memberNodes = entityTypeNode.SelectNodes(string.Format("{0}:{1}", CSDL_PREFIX, PROPERTY_NODE), NamespaceManager);
            var list = new List<string>();
            foreach (XmlNode memberNode in memberNodes)
            {
                var name = GetNodeAttributeValue(memberNode, NAME_PROPERTY);
                if (!string.IsNullOrEmpty(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets list of name of navigation properties
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>List of name of navigation properties</returns>
        public List<string> GetNavgationPropertyNames(string containerName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                throw new ArgumentNullException("entitySetName");
            }
            var entityTypeNode = GetEntityTypeNode(containerName, entityTypeName);
            var memberNodes = entityTypeNode.SelectNodes(string.Format("{0}:{1}", CSDL_PREFIX, NAVIGATION_PROPERTY_NODE), NamespaceManager);
            var list = new List<string>();
            foreach (XmlNode memberNode in memberNodes)
            {
                var name = GetNodeAttributeValue(memberNode, NAME_PROPERTY);
                if (!string.IsNullOrEmpty(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets type of property
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Type of property</returns>
        public string GetPropertyType(string containerName, string entityTypeName, string propertyName)
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
            var memberNode = GetPropertyNode(containerName, entityTypeName, propertyName);
            return GetNodeAttributeValue(memberNode, TYPE_PROPERTY);
        }

        /// <summary>
        /// Removes property
        /// </summary>
        /// <param name="containerName">Name of context</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <param name="propertyName">Name of property</param>
        public void RemoveProperty(string containerName, string entityTypeName, string propertyName)
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
         
            var mslNode = GetMappingScalarPropertyNode(containerName, entityTypeName, propertyName);
            var csdlNode = GetPropertyNode(containerName, entityTypeName, propertyName, DataSpace.CSSpace);

            var containerNameS = GetNodeAttributeValue(GetEntityContainerMappingNode(containerName), STORE_ENTITY_CONTAINER_PROPERTY);
            var propertyNameS = GetNodeAttributeValue(mslNode, COLUMN_NAME_PROPERTY);
            var entitySetName = GetNodeAttributeValue(GetMappingFragmentNode(containerName, entityTypeName), STORE_ENTITY_SET_PROPERTY);
            var entityTypeNameS = GetEntitySetType(containerNameS, entitySetName, DataSpace.SSpace);

            var ssdlNode = GetPropertyNode(containerNameS, entityTypeNameS, propertyNameS, DataSpace.SSpace);
            //remove ssdl

            ssdlNode.ParentNode.RemoveChild(ssdlNode);
          
            //remove csdl
            csdlNode.ParentNode.RemoveChild(csdlNode);

            //remove msl

            mslNode.ParentNode.RemoveChild(mslNode);
        }

        /// <summary>
        /// Saves file
        /// </summary>
        public void Save()
        {
            Xml.Save(FileName);
        }
    }
}