using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;

namespace System.Xml
{
    public static class XmlNodeExtension
    {
        public static XmlNode SelectSingleNode(this XmlNode node, string xpath, bool ignoreCase)
        {
            if (!xpath.Contains(NODE_SPLITTER))
            {
                return node.ChildNodes.OfType<XmlNode>().FirstOrDefault(c => IsMatch(c, xpath, ignoreCase));
            }
            else
            {
                var parentPath = GetParentPath(xpath);
                var childPath = GetChildPath(xpath);
                foreach (XmlNode parentNode in SelectNodes(node, parentPath, ignoreCase))
                {
                    var childNode = SelectSingleNode(parentNode, childPath, ignoreCase);
                    if (childNode != null)
                    {
                        return childNode;
                    }
                }
                return null;
            }
        }

        public static IEnumerable<XmlNode> SelectNodes(this XmlNode node, string xpath, bool ignoreCase)
        {
            if (!xpath.Contains(NODE_SPLITTER))
            {
                return node.ChildNodes.OfType<XmlNode>().Where(c => IsMatch(c, xpath, ignoreCase));
            }
            else
            {
                List<XmlNode> listNodes = new List<XmlNode>();
                var parentPath = GetParentPath(xpath);
                var childPath = GetChildPath(xpath);
                foreach (XmlNode parentNode in SelectNodes(node, parentPath, ignoreCase))
                {
                    var childNodes = SelectNodes(parentNode, childPath, ignoreCase);
                    listNodes.AddRange(childNodes);
                }
                return listNodes;
            }
        }

        public static bool IsMatch(this XmlNode node, string xpath, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return true;
            }

            var name = GetName(xpath);
            if(string.Compare(node.Name, name, ignoreCase) != 0)
            {
                return false;
            }
            var attributeName = GetAttributeName(xpath);
            var attributeValue = GetAttributeValue(xpath);
            var attribute = node.Attributes[attributeName];
            if (attribute == null)
            {
                if (string.IsNullOrEmpty(attributeValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return string.Compare(attribute.Value, attributeValue, ignoreCase) == 0;
        }

        const char NODE_SPLITTER = '/';
        const char ATTRIBUTE_LEFT_SPLITTER = '[';
        const char ATTRIBUTE_RIGHT_SPLITTER = ']';
        const char ATTRIBUTE_SPLITTER = '=';
        const char ATTRIBUTE_PREFIX = '@';
        const char ATTRIBUTE_QUTEO = '\'';

        private static string GetParentPath(string xpath)
        {
            var index = xpath.IndexOf(NODE_SPLITTER);
            if (index == 0 || index == xpath.Length - 1)
            {
                throw new ArgumentException("path");
            }
            return xpath.Substring(0, index);
        }

        private static string GetChildPath(string xpath)
        {
            var index = xpath.IndexOf(NODE_SPLITTER);
            if (index == 0 || index == xpath.Length - 1)
            {
                throw new ArgumentException("path");
            }
            return xpath.Substring(index + 1);
        }

        private static string GetName(string xpath)
        {
            var attributeleftIndex = xpath.IndexOf(ATTRIBUTE_LEFT_SPLITTER);
            if (attributeleftIndex > 0)
            {
                return xpath.Substring(0, attributeleftIndex);
            }
            else
            {
                return xpath;
            }
        }

        private static string GetAttributePath(string xpath)
        {
            var attributeleftIndex = xpath.IndexOf(ATTRIBUTE_LEFT_SPLITTER);
            var attributerightIndex = xpath.IndexOf(ATTRIBUTE_RIGHT_SPLITTER);
            if (attributeleftIndex > 0 && attributerightIndex > attributeleftIndex + 1)
            {
                return xpath.Substring(attributeleftIndex + 1, attributerightIndex - attributeleftIndex - 1);
            }
            return string.Empty;
        }

        private static string GetAttributeName(string xpath)
        {
            var attributePath = GetAttributePath(xpath);
            if (string.IsNullOrEmpty(attributePath))
            {
                return string.Empty;
            }
            var index = attributePath.IndexOf(ATTRIBUTE_SPLITTER);
            if (index == 0 || index == xpath.Length - 1)
            {
                throw new ArgumentException("path");
            }

            return attributePath.Substring(0, index).TrimStart(ATTRIBUTE_PREFIX);
        }

        private static string GetAttributeValue(string xpath)
        {
            var attributePath = GetAttributePath(xpath);
            if (string.IsNullOrEmpty(attributePath))
            {
                return string.Empty;
            }

            var index = attributePath.IndexOf(ATTRIBUTE_SPLITTER);
            if (index == 0 || index == xpath.Length - 1)
            {
                throw new ArgumentException("path");
            }
            return attributePath.Substring(index + 1).Trim(ATTRIBUTE_QUTEO);
        }

       
    }
}

namespace EFWCFModule
{
    /// <summary>
    /// Provider of config
    /// </summary>
    public class Config
    {
        /// <summary>
        /// EFConfig.xml
        /// </summary>
        readonly static string ConfigFile = string.Format(@"{0}\EFConfig.xml", Environment.CurrentDirectory);
        /// <summary>
        /// Config
        /// </summary>
        const string CONFIG_NODE = "Config";

        /// <summary>
        /// Creates a new instance of config provider
        /// </summary>
        /// <param name="configType">Config type</param>
        protected Config(Type configType)
        {
            if (configType == null)
            {
                throw new ArgumentNullException("configType");
            }
            _document = new XmlDocument();
            if (File.Exists(ConfigFile))
            {
                _document.Load(ConfigFile);
            }
            else
            {
                var declaration = _document.CreateXmlDeclaration("1.0", "UTF-8", null);
                _document.AppendChild(declaration);
                var documentNode = _document.CreateElement(CONFIG_NODE);
                _document.AppendChild(documentNode);
            }
            _typeNode = _document.DocumentElement.SelectSingleNode(configType.Name);
            if (_typeNode == null)
            {
                _typeNode = _document.CreateElement(configType.Name);
                _document.DocumentElement.AppendChild(_typeNode);
            }
        }

        private XmlDocument _document;
        /// <summary>
        /// Gets document of config
        /// </summary>
        private XmlDocument Document
        {
            get
            {
                return _document;
            }
        }

        private XmlNode _typeNode;
        /// <summary>
        /// Gets node of config
        /// </summary>
        private XmlNode TypeNode
        {
            get
            {
                return _typeNode;
            }
        }

        /// <summary>
        /// Reads property value
        /// </summary>
        /// <param name="path">Path of node</param>
        /// <returns>Property value</returns>
        protected Dictionary<string, string> ReadValue(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            var node = GetNode(path);
            if (node == null)
            {
                return null;
            }
            else
            {
                return ReadValue(node);
            }
        }

        /// <summary>
        /// Reads list of property values
        /// </summary>
        /// <param name="path">Path of node</param>
        /// <returns>List of property values</returns>
        protected List<Dictionary<string, string>> ReadValueList(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            var valueList = new List<Dictionary<string, string>>();
            var nodes = GetNodes(path);
            foreach (XmlNode node in nodes)
            {
                valueList.Add(ReadValue(node));
            }
            return valueList;
        }

        /// <summary>
        /// Reads property value
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Property value</returns>
        private Dictionary<string, string> ReadValue(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            var dictionary = new Dictionary<string, string>();
            foreach (XmlAttribute attribute in node.Attributes)
            {
                dictionary.Add(attribute.Name, attribute.Value);
            }
            return dictionary;
        }

        /// <summary>
        /// Writes property value
        /// </summary>
        /// <param name="path">Path of node</param>
        /// <param name="value">Property value</param>
        protected void WriteValue(string path, Dictionary<string, string> value)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            var node = GetNode(path);
            if (node == null)
            {
                throw new ObjectNotFoundException(string.Format("XmlNode:{0} not found.", path));
            }
            WriteValue(node, value);
        }

        /// <summary>
        /// Writes property value
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="value">Property value</param>
        protected void WriteValue(XmlNode node, Dictionary<string, string> value)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (value != null)
            {
                foreach (var keyvalue in value)
                {
                    var attribute = node.Attributes[keyvalue.Key];
                    if (attribute == null)
                    {
                        attribute = node.OwnerDocument.CreateAttribute(keyvalue.Key);
                        node.Attributes.Append(attribute);
                    }
                    attribute.Value = keyvalue.Value;
                }
            }
        }

        /// <summary>
        /// Adds node
        /// </summary>
        /// <param name="parentPath">Path of parent node</param>
        /// <param name="name">Name of node</param>
        /// <param name="value">Property value</param>
        protected void AddValue(string parentPath, string name, Dictionary<string, string> value)
        {
            var parentNode = string.IsNullOrEmpty(parentPath) ? TypeNode : GetNode(parentPath);
            if (parentNode == null)
            {
                throw new ObjectNotFoundException(string.Format("XmlNode:{0} not found.", parentPath));
            }
            foreach (XmlNode childNode in parentNode.SelectNodes(name))
            {
                if(value == null || value.All( c => c.Value.Equals(childNode.Attributes[c.Key])))
                {
                    return;
                }
            }
            var node = Document.CreateElement(name);
            parentNode.AppendChild(node);
            WriteValue(node, value);
        }

        /// <summary>
        /// Deletes node
        /// </summary>
        /// <param name="path">Path of node</param>
        /// <param name="value">Property value</param>
        protected void DeleteValue(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            var nodes = GetNodes(path);
            foreach (XmlNode node in nodes)
            {
                node.ParentNode.RemoveChild(node);
            }
        }

        /// <summary>
        /// Gets node
        /// </summary>
        /// <param name="path">Path of node</param>
        /// <returns>Node</returns>
        private XmlNode GetNode(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            //return TypeNode.SelectSingleNode(path);
            return TypeNode.SelectSingleNode(path, true);
        }

        /// <summary>
        /// Gets list of nodes
        /// </summary>
        /// <param name="path">Path of node</param>
        /// <returns>List of nodes</returns>
        private IEnumerable GetNodes(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            //return TypeNode.SelectNodes(path);
            return TypeNode.SelectNodes(path, true);
        }

        /// <summary>
        /// Saves config
        /// </summary>
        public void Save()
        {
            Document.Save(ConfigFile);
        }
    }

    /// <summary>
    /// Config of package
    /// </summary>
    public class PackageConfig: Config
    {
        /// <summary>
        /// Creates a new instance of package config
        /// </summary>
        public PackageConfig() 
            : base(typeof(PackageProvider))
        { }

        /// <summary>
        /// Solution
        /// </summary>
        const string SOLUTION_NODE = "Solution";
        /// <summary>
        /// Module
        /// </summary>
        const string MODULE_NODE = "Module";
        /// <summary>
        /// Method
        /// </summary>
        const string METHOD_NODE = "Method";
        /// <summary>
        /// Service
        /// </summary>
        const string SERVICE_NODE = "Service";
        /// <summary>
        /// Active
        /// </summary>
        public const string ACTIVE_PROPERTY = "Active";
        /// <summary>
        /// Name
        /// </summary>
        public const string NAME_PROPERTY = "Name";
        
        /// <summary>
        /// Adds solution
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        public void AddSolution(string solutionName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            var value = new Dictionary<string, string>();
            value.Add(NAME_PROPERTY, solutionName);
            AddValue(string.Empty, SOLUTION_NODE, value);
        }

        /// <summary>
        /// Removes solution
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        public void RemoveSolution(string solutionName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            var solutionPath = GetSolutionPath(solutionName);
            DeleteValue(solutionPath);
        }

        /// <summary>
        /// Gets list of solutions
        /// </summary>
        /// <returns>List of solutions</returns>
        public List<string> GetSolutions()
        {
            var solutionPath = GetSolutionPath(null);
            return ReadValueList(solutionPath).Select(c => c[NAME_PROPERTY]).ToList();
        }

        /// <summary>
        /// Adds module
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        public void AddModule(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var solutionPath = GetSolutionPath(solutionName);
            var value = new Dictionary<string, string>();
            value.Add(NAME_PROPERTY, assemblyName);
            AddValue(solutionPath, MODULE_NODE, value);
        }

        /// <summary>
        /// Removes module
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        public void RemoveModule(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var modulePath = GetModulePath(solutionName, assemblyName);
            DeleteValue(modulePath);
        }

        /// <summary>
        /// Gets list of modules
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="active">Whether module is actitve</param>
        /// <returns>List of modules</returns>
        public List<string> GetModules(string solutionName, bool active)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            var modulePath = GetModulePath(solutionName, null);

            return ReadValueList(modulePath)
                .Where(c => active.Equals(IsModuleActive(c))).Select(c => c[NAME_PROPERTY]).ToList();
        }

        /// <summary>
        /// Gets whether module is actitve
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Whether module is actitve</returns>
        public bool IsModuleActive(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var modulePath = GetModulePath(solutionName, assemblyName);
            return IsModuleActive(ReadValue(modulePath));
        }

        /// <summary>
        /// Gets whether module is actitve
        /// </summary>
        /// <param name="propertyValue">Value of property</param>
        /// <returns>Whether module is actitve</returns>
        private bool IsModuleActive(Dictionary<string, string> propertyValue)
        {
            return propertyValue == null ? false
                : propertyValue.ContainsKey(ACTIVE_PROPERTY) && string.Compare(bool.TrueString, propertyValue[ACTIVE_PROPERTY]) == 0;
        }

        /// <summary>
        /// Sets whether module is actitve
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="active">Whether module is actitve</param>
        public void SetPackageActive(string solutionName, string assemblyName, bool active)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var packagePath = GetModulePath(solutionName, assemblyName);
            var propertyValue = new Dictionary<string, string>();
            propertyValue.Add(ACTIVE_PROPERTY, active.ToString());
            WriteValue(packagePath, propertyValue);
        }

        /// <summary>
        /// Adds method
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodInfo">Information of method</param>
        public void AddMethod(string solutionName, string assemblyName, MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            var packagePath = GetModulePath(solutionName, assemblyName);
            var value = new Dictionary<string, string>();
            value.Add(NAME_PROPERTY, methodInfo.Name);
            AddValue(packagePath, METHOD_NODE, value);
        }

        /// <summary>
        /// Removes method
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodInfo">Information of method</param>
        public void RemoveMethod(string solutionName, string assemblyName, MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            var methodPath = GetMethodPath(solutionName, assemblyName, methodInfo.Name);
            DeleteValue(methodPath);
        }

        /// <summary>
        /// Gets whether method requires logon first
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodInfo">Information of method</param>
        /// <returns>Whether method requires logon first</returns>
        public bool IsLogOnRequired(string solutionName, string assemblyName, MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            return IsLogOnRequired(solutionName, assemblyName, methodInfo.Name);
        }

        /// <summary>
        /// Gets whether method requires logon first
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <returns>Whether method requires logon first</returns>
        public bool IsLogOnRequired(string solutionName, string assemblyName, string methodName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var methodPath = GetMethodPath(solutionName, assemblyName, methodName);
            var propertyValue = ReadValue(methodPath);
            return propertyValue == null;
        }

        /// <summary>
        /// Adds service
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="serviceType">Type of service</param>
        public void AddService(string solutionName, string assemblyName, Type serviceType)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            var packagePath = GetModulePath(solutionName, assemblyName);
            var value = new Dictionary<string, string>();
            value.Add(NAME_PROPERTY, serviceType.FullName);
            AddValue(packagePath, SERVICE_NODE, value);
        }

        /// <summary>
        /// Removes service
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="serviceType">Type of service</param>
        public void RemoveService(string solutionName, string assemblyName, Type serviceType)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            var servicePath = GetServicePath(solutionName, assemblyName, serviceType.FullName);
            DeleteValue(servicePath);
        }

        /// <summary>
        /// Gets list of services
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of services</returns>
        public List<string> GetServices(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solution");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var servicePath = GetServicePath(solutionName, assemblyName, null);
            return ReadValueList(servicePath).Select(c => c[NAME_PROPERTY]).ToList();
        }
       
        /// <summary>
        /// Gets solution path
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <returns>Solution path</returns>
        private string GetSolutionPath(string solutionName)
        {
            return string.Format("{0}{1}", SOLUTION_NODE, GetNamePath(solutionName));
        }

        /// <summary>
        /// Gets module path
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Module path</returns>
        private string GetModulePath(string solutionName, string assemblyName)
        {
            return string.Format("{0}{1}/{2}{3}", SOLUTION_NODE, GetNamePath(solutionName), MODULE_NODE, GetNamePath(assemblyName));
        }

        /// <summary>
        /// Gets method path
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <returns>Method path</returns>
        private string GetMethodPath(string solutionName, string assemblyName, string methodName)
        {
            return string.Format("{0}{1}/{2}{3}/{4}{5}", SOLUTION_NODE, GetNamePath(solutionName), MODULE_NODE, GetNamePath(assemblyName)
                , METHOD_NODE, GetNamePath(methodName));
        }

        /// <summary>
        /// Gets service path
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="serviceName">Name of service</param>
        /// <returns>Service path</returns>
        private string GetServicePath(string solutionName, string assemblyName, string serviceName)
        {
            return string.Format("{0}{1}/{2}{3}/{4}{5}", SOLUTION_NODE, GetNamePath(solutionName), MODULE_NODE, GetNamePath(assemblyName)
                 , SERVICE_NODE, GetNamePath(serviceName));
        }

        /// <summary>
        /// Gets name property path
        /// </summary>
        /// <param name="name">Value of name</param>
        /// <returns>Name property path</returns>
        private string GetNamePath(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("[@{0}='{1}']", NAME_PROPERTY, name);
            }
        }
    }

    /// <summary>
    /// Config of service
    /// </summary>
    public class ServiceConfig : Config
    {
        /// <summary>
        /// Creates a new instance of service config
        /// </summary>
        public ServiceConfig()
            : base(typeof(ServiceProvider))
        {
            if (ReadValue(BINDING_NODE) == null)
            {
                AddValue(string.Empty, BINDING_NODE, null);
            }
        }

        /// <summary>
        /// Binding
        /// </summary>
        const string BINDING_NODE = "Binding";
        /// <summary>
        /// MaxBufferPoolSize
        /// </summary>
        const string MAX_BUFFER_POOL_SIZE_PROPERTY = "MaxBufferPoolSize";
        /// <summary>
        /// MaxReceivedMessageSize
        /// </summary>
        const string MAX_RECEIVED_MESSAGESIZE_PROPERTY = "MaxReceivedMessageSize";
        /// <summary>
        /// MaxArrayLength
        /// </summary>
        const string MAX_ARRAY_LENGTH_PROPERTY = "MaxArrayLength";
        /// <summary>
        /// MaxBytesPerRead
        /// </summary>
        const string MAX_BYTES_PER_READ_PROPERTY = "MaxBytesPerRead";
        /// <summary>
        /// MaxNameTableCharCount
        /// </summary>
        const string MAX_NAME_TABLE_CHAR_COUNT_PROPERTY = "MaxNameTableCharCount";
        /// <summary>
        /// MaxStringContentLength
        /// </summary>
        const string MAX_STRING_CONTENT_LENGTH_PROPERTY = "MaxStringContentLength";
        /// <summary>
        /// MaxDepth
        /// </summary>
        const string MAX_DEPTH_PROPERTY = "MaxDepth";

        /// <summary>
        /// Gets the maximum amount of memory allocated for the buffer manager
        /// </summary>
        public long MaxBufferPoolSize
        {
            get
            {
                return GetValue(MAX_BUFFER_POOL_SIZE_PROPERTY, 524288);
            }
            set
            {
                SetValue(MAX_BUFFER_POOL_SIZE_PROPERTY, (int)value);
            }
        }

        /// <summary>
        ///  Gets the maximum size for a message that can be processed by the binding
        /// </summary>
        public long MaxReceivedMessageSize
        {
            get
            {
                return GetValue(MAX_RECEIVED_MESSAGESIZE_PROPERTY, 65536);
            }
            set
            {
                SetValue(MAX_RECEIVED_MESSAGESIZE_PROPERTY, (int)value);
            }
        }

        /// <summary>
        /// Gets and sets the maximum allowed array length
        /// </summary>
        public int MaxArrayLength
        {
            get
            {
                return GetValue(MAX_ARRAY_LENGTH_PROPERTY, 16384);
            }
            set
            {
                SetValue(MAX_ARRAY_LENGTH_PROPERTY, value);
            }
        }

        /// <summary>
        /// Gets and sets the maximum allowed bytes returned for each read
        /// </summary>
        public int MaxBytesPerRead
        {
            get
            {
                return GetValue(MAX_BYTES_PER_READ_PROPERTY, 4096);
            }
            set
            {
                SetValue(MAX_BYTES_PER_READ_PROPERTY, value);
            }
        }

        /// <summary>
        /// Gets and sets the maximum nested node depth
        /// </summary>
        public int MaxDepth
        {
            get
            {
                return GetValue(MAX_DEPTH_PROPERTY, 32);
            }
            set
            {
                SetValue(MAX_DEPTH_PROPERTY, value);
            }
        }

        /// <summary>
        ///  Gets and sets the maximum characters allowed in a table name
        /// </summary>
        public int MaxNameTableCharCount
        {
            get
            {
                return GetValue(MAX_NAME_TABLE_CHAR_COUNT_PROPERTY, 16384);
            }
            set
            {
                SetValue(MAX_NAME_TABLE_CHAR_COUNT_PROPERTY, value);
            }
        }

        /// <summary>
        /// Gets and sets the maximum string length returned by the reader
        /// </summary>
        public int MaxStringContentLength
        {
            get
            {
                return GetValue(MAX_STRING_CONTENT_LENGTH_PROPERTY, 8192);
            }
            set
            {
                SetValue(MAX_STRING_CONTENT_LENGTH_PROPERTY, value);
            }
        
        }

        /// <summary>
        /// Gets value of property
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value of property</returns>
        private int GetValue(string propertyName, int defaultValue)
        {
            var propertyValue = ReadValue(BINDING_NODE);
            if (propertyValue != null && propertyValue.ContainsKey(propertyName))
            {
                try
                {
                    return int.Parse(propertyValue[propertyName]);
                }
                catch (Exception e)
                {
                    LogProvider.LogException(e);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Sets value of property
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="value">Value of property</param>
        private void SetValue(string propertyName, int value)
        {
            var propertyValue = new Dictionary<string, string>();
            propertyValue.Add(propertyName, value.ToString());
            WriteValue(BINDING_NODE, propertyValue);
        }
    }

    /// <summary>
    /// Config of sd module
    /// </summary>
    public class SDModuleConfig : Config
    {
        /// <summary>
        /// MaxReceivedMessageSize
        /// </summary>
        const string WEBCLIENT_NODE = "WebClient";

        const string PATH_PROPERTY = "Path";

        /// <summary>
        /// Creates a new instance of service config
        /// </summary>
        public SDModuleConfig()
            : base(typeof(EFWCFModule.EEPAdapter.SDModuleProvider))
        {
            if (ReadValue(WEBCLIENT_NODE) == null)
            {
                AddValue(string.Empty, WEBCLIENT_NODE, null);
            }
        }

        /// <summary>
        /// Gets and sets webclient path
        /// </summary>
        public string WebClientPath
        {
            get
            {
                return GetValue(PATH_PROPERTY, "");
            }
            set
            {
                SetValue(PATH_PROPERTY, value);
            }
        }

        /// <summary>
        /// Gets value of property
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value of property</returns>
        private string GetValue(string propertyName, string defaultValue)
        {
            var propertyValue = ReadValue(WEBCLIENT_NODE);
            if (propertyValue != null && propertyValue.ContainsKey(propertyName))
            {
                try
                {
                    return propertyValue[propertyName];
                }
                catch (Exception e)
                {
                    LogProvider.LogException(e);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Sets value of property
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="value">Value of property</param>
        private void SetValue(string propertyName, string value)
        {
            var propertyValue = new Dictionary<string, string>();
            propertyValue.Add(propertyName, value.ToString());
            WriteValue(WEBCLIENT_NODE, propertyValue);
        }
    }
}