using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Objects.DataClasses;
using System.IO;
using System.ServiceModel;
using System.Collections;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Xml;

namespace EFWCFModule
{
    /// <summary>
    /// Interface of IModuleProvider
    /// </summary>
    public interface IModuleProvider
    {
        /// <summary>
        /// Gets or sets Information of client
        /// </summary>
        ClientInfo ClientInfo { get; set; }

        string SqlSentence { get; set; }

        /// <summary>
        /// Gets modules of solution
        /// </summary>
        /// <returns>Modules of solution</returns>
        List<string> GetModules();

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of command names</returns>
        List<string> GetCommandNames(string assemblyName);

        /// <summary>
        /// Gets name of entity type
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <returns>Name of entity type</returns>
        string GetObjectClassName(string assemblyName, string commandName, string entitySetName);

        /// <summary>
        /// Gets count of data
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of data</returns>
        int GetDataCount(string assemblyName, string commandName, PacketInfo packetInfo);

        /// <summary>
        /// Gets fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        List<string> GetFields(string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        Dictionary<String, int> GetFieldsLength(string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        List<string> GetEntityNavigationFields(string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets primary fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        List<string> GetPrimaryKeys(string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        Dictionary<string, string> GetEntityFieldTypes(string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets serialized dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        object GetSerializedDataSet(string assemblyName, string commandName, PacketInfo packetInfo);

        
        /// <summary>
        /// Gets total dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="totals">totals</param>
        /// <returns>Dataset</returns>
        object GetDataTotal(string assemblyName, string commandName, PacketInfo packetInfo, Dictionary<string, string> totals);

        /// <summary>
        /// Updates dataset to database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="dataset">Dataset</param>
        /// <returns>Dataset</returns>
        object UpdateDataSet(string assemblyName, string commandName, object dataset);

        object ExecuteIOTMethod(string assemblyName, string commandName, object dataset);

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        object CallMethod(string assemblyName, string methodName, object[] parameters);

        /// <summary>
        /// Do record lock
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Lock</returns>
        EFWCFModule.EEPAdapter.LockType DoRecordLock(string assemblyName, string commandName, PacketInfo packetInfo, EFWCFModule.EEPAdapter.LockType type, ref string user);
    }

    public class EntityModuleProvider: IModuleProvider
    {
        #region IModuleProvider Members

        /// <summary>
        /// Gets or sets Information of client
        /// </summary>
        public ClientInfo ClientInfo { get; set; }

        public string SqlSentence { get; set; }

        /// <summary>
        /// Gets modules of solution
        /// </summary>
        /// <returns>Modules of solution</returns>
        public List<string> GetModules()
        {
            return PackageProvider.GetModules(ClientInfo.Solution);
        }

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of command names</returns>
        public List<string> GetCommandNames(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetCommandNames();
        }

        /// <summary>
        /// Gets name of entity type
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <returns>Name of entity type</returns>
        public string GetObjectClassName(string assemblyName, string commandName, string entitySetName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetObjectClassName(commandName, entitySetName);
        }

        /// <summary>
        /// Gets count of data
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of data</returns>
        public int GetDataCount(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetObjectCount(commandName, packetInfo);
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetFields(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetEntityFields(commandName, entityTypeName);
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public Dictionary<String, int> GetFieldsLength(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetEntityFieldsLength(commandName, entityTypeName);
        }

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        public List<string> GetEntityNavigationFields(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetEntityNavigationFields(commandName, entityTypeName);
        }

        /// <summary>
        /// Gets primary fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetPrimaryKeys(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetEntityPrimaryKeys(commandName, entityTypeName);
        }


        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldTypes(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.GetEntityFieldTypes(commandName, entityTypeName);
        }

        /// <summary>
        /// Gets serialized dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        public object GetSerializedDataSet(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Updates dataset to database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="dataset">Dataset</param>
        /// <returns>Dataset</returns>
        public object UpdateDataSet(string assemblyName, string commandName, object dataset)
        {
            throw new NotSupportedException();
        }

        public object ExecuteIOTMethod(string assemblyName, string commandName, object dataset)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets total dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="totals">totals</param>
        /// <returns>Dataset</returns>
        public object GetDataTotal(string assemblyName, string commandName, PacketInfo packetInfo, Dictionary<string, string> totals)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        public object CallMethod(string assemblyName, string methodName, object[] parameters)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var module = PackageProvider.LoadModule(ClientInfo, assemblyName);
            return module.CallMethod(methodName, parameters);
        }

        /// <summary>
        /// Do record lock
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="type">Lock type</param>
        /// <returns>Lock</returns>
        public EFWCFModule.EEPAdapter.LockType DoRecordLock(string assemblyName, string commandName, PacketInfo packetInfo, EFWCFModule.EEPAdapter.LockType type, ref string user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    /// <summary> re
    /// Provider of package
    /// </summary>
    public static class PackageProvider
    {
        /// <summary>
        /// Temp
        /// </summary>
        const string TEMP_ASSEMBLY_DIRECTORY_NAME = "Temp";

#if Oracle2
        /// <summary>
        /// EFGlobalModule
        /// </summary>
        internal const string GLOBAL_ASSEMBLY_NAME = "EFGlobalModule_Oracle";
#else
        /// <summary>
        /// EFGlobalModule
        /// </summary>
        internal const string GLOBAL_ASSEMBLY_NAME = "EFGlobalModule";
#endif

        private static string _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// Gets or sets current directory
        /// </summary>
        public static string CurrentDirectory
        { 
            get
            {
                return _currentDirectory;
            }
            set 
            {
                _currentDirectory = value;
            }
        }

        private static string _tempAssemblyDirectory;
        /// <summary>
        /// Gets or sets temporary directory
        /// </summary>
        public static string TempAssemblyDirectory
        {
            get 
            {
                return _tempAssemblyDirectory;
            }
            set 
            {
                _tempAssemblyDirectory = value;
            }
        }

        private static Hashtable AssemblyPaths = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        public static void ClearTempAssemblies()
        {
            var directoryPath = Path.Combine(CurrentDirectory, TEMP_ASSEMBLY_DIRECTORY_NAME);
            //if (Directory.Exists(directoryPath))
            //{
            //    Directory.Delete(directoryPath, true);
            //}

            if (Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.Delete(directoryPath, true);
                }
                catch
                { 
                    
                }
            }
        }

        /// <summary>
        /// Loads assembly
        /// </summary>
        /// <param name="assemblyFile">File name of assembly</param>
        /// <returns>Assembly</returns>
        internal static Assembly LoadAssembly(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
            {
                throw new ArgumentNullException("assemblyFile");
            }
            lock (typeof(PackageProvider))
            {
                //if (true)
                //{
                  // return Assembly.LoadFile(assemblyFile);
                //}
                //else
                //{
                if (AssemblyPaths.Contains(assemblyFile.ToLower()) && File.Exists((string)AssemblyPaths[assemblyFile.ToLower()]))
                {
                    return Assembly.LoadFile((string)AssemblyPaths[assemblyFile.ToLower()]);
                }
                else
                {
                    if (string.IsNullOrEmpty(TempAssemblyDirectory))
                    {
                        TempAssemblyDirectory = DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    var fileInfo = new FileInfo(assemblyFile);
                    var fileName = fileInfo.Name;
                    var directoryName = fileInfo.Directory.Name;
                    var directoryPath = Path.Combine(CurrentDirectory, TEMP_ASSEMBLY_DIRECTORY_NAME, directoryName, TempAssemblyDirectory);
                    var newAssemblyFile = Path.Combine(directoryPath, fileName);

                    if (File.Exists(newAssemblyFile))
                    {
                        TempAssemblyDirectory = DateTime.Now.ToString("yyyyMMddHHmmss");
                        directoryPath = Path.Combine(CurrentDirectory, TEMP_ASSEMBLY_DIRECTORY_NAME, directoryName, TempAssemblyDirectory);
                        newAssemblyFile = Path.Combine(directoryPath, fileName);
                    }
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    File.Copy(assemblyFile, newAssemblyFile);
                    AssemblyPaths[assemblyFile.ToLower()] = newAssemblyFile;
                    return Assembly.LoadFile(newAssemblyFile);
                }
                //}
            }
        }

        /// <summary>
        /// Loads assembly
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Assembly</returns>
        internal static Assembly LoadAssembly(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            return LoadAssembly(GetAssemblyFile(solutionName, assemblyName));
        }

        /// <summary>
        /// Unloads assembly
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        public static void UnLoadAssembly(string solutionName, string assemblyName)
        {
            lock (typeof(PackageProvider))
            {
                var assemblyFile = GetAssemblyFile(solutionName, assemblyName);
                if(AssemblyPaths.Contains(assemblyFile))
                {
                    AssemblyPaths.Remove(assemblyFile);
                }
            }
        }

        /// <summary>
        /// Get assembly file name
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Assembly file name</returns>
        internal static string GetAssemblyFile(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            
            if (assemblyName.Equals(GLOBAL_ASSEMBLY_NAME))
            {
                return Path.Combine(CurrentDirectory, string.Format("{0}.dll", GLOBAL_ASSEMBLY_NAME));
            }
            else
            {
                return string.IsNullOrEmpty(solutionName) ? Path.Combine(CurrentDirectory, string.Format("{0}.dll", assemblyName))
                 : Path.Combine(CurrentDirectory, solutionName, string.Format("{0}.dll", assemblyName));
            }
        }

        /// <summary>
        /// Loads server module
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Server module</returns>
        internal static IEFModule LoadModule(ClientInfo clientInfo, string assemblyName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(clientInfo.Solution))
            {
                throw new ArgumentNullException("clientInfo.Solution");
            }

            IEFModule module = LoadType<IEFModule>(clientInfo.Solution, assemblyName);
            module.ClientInfo = clientInfo;
            return module;
        }

        private static IGlobalModule _globalModule;
        /// <summary>
        /// Gets global module
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Global module</returns>
        internal static IGlobalModule LoadGlobalModule(ClientInfo clientInfo)
        {
            var module = LoadType<IGlobalModule>(string.Empty, GLOBAL_ASSEMBLY_NAME);
            module.ClientInfo = clientInfo;
            return module;
            //if (_globalModule == null)
            //{
            //    IGlobalModule module = LoadType<IGlobalModule>(string.Empty, GLOBAL_ASSEMBLY_NAME);
            //    _globalModule = module;
            //}
            //_globalModule.ClientInfo = clientInfo;
            //return _globalModule;
        }

        /// <summary>
        /// Gets type from assembly
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Object of type</returns>
        private static Type GetType<T>(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
           
            var assembly = LoadAssembly(solutionName, assemblyName);
            var type = assembly.GetTypes().FirstOrDefault(c => typeof(T).IsAssignableFrom(c));
            if (type == null)
            {
                throw new BadImageFormatException(string.Format("Type:{0} not found in server dll.", typeof(T).Name));
            }
            return type;
        }

        /// <summary>
        /// Loads type from assembly
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Object of type</returns>
        private static T LoadType<T>(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (!string.IsNullOrEmpty(solutionName) && !assemblyName.Equals(GLOBAL_ASSEMBLY_NAME))
            {
                if (!IsModuleActive(solutionName, assemblyName))
                {
                    throw new InvalidOperationException(string.Format("Module:{0}/{1} not active.", solutionName, assemblyName));
                }
            }
            var type = GetType<T>(solutionName, assemblyName);
            return (T)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Gets modules of solution
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <returns>Modules of solution</returns>
        public static List<string> GetModules(string solutionName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            var config = new PackageConfig();
            return config.GetModules(solutionName, true);
        }

        /// <summary>
        /// Gets list of module file names
        /// </summary>
        /// <param name="active">Active</param>
        /// <returns>List of module file names</returns>
        public static List<string> GetModuleFileNames(bool active)
        {
            var config = new PackageConfig();
            var listModuleFileNames = new List<string>();
            var solutions = config.GetSolutions();
            foreach (var solution in solutions)
            {
                var modules = config.GetModules(solution, active);
                foreach (var module in modules)
                {
                    listModuleFileNames.Add(GetAssemblyFile(solution, module));
                }
            }
            return listModuleFileNames;
        }

        /// <summary>
        /// Gets whether module is actitve
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Whether module is actitve</returns>
        public static bool IsModuleActive(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var config = new PackageConfig();
            return config.IsModuleActive(solutionName, assemblyName);
        }

        /// <summary>
        /// Gets module type
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>Module type</returns>
        public static Type GetModuleType(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            return GetType<IEFModule>(solutionName, assemblyName);
        }

        /// <summary>
        /// Gets list of methods
        /// </summary>
        /// <param name="moduleType">Type of module</param>
        /// <returns>List of methods</returns>
        public static List<MethodInfo> GetMethods(Type moduleType)
        {
            if (moduleType == null)
            {
                throw new ArgumentNullException("moduleType");
            }
            return moduleType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToList();
        }

        /// <summary>
        /// Checks whether method requires logon first
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        public static void CheckMethodLogOnRequired(string solutionName, string assemblyName, string methodName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }

            var config = new PackageConfig();
            if (config.IsLogOnRequired(solutionName, assemblyName, methodName))
            {
                throw new InvalidOperationException(string.Format("Method:{0}.{1} can not be invoked before logon.", assemblyName, methodName));
            }
        }
    }

    /// <summary>
    /// Provider of service
    /// </summary>
    public static class ServiceProvider
    {
        /// <summary>
        /// Gets a list of known types
        /// </summary>
        /// <param name="provider">Provider of custom attribute</param>
        /// <returns>List of known types</returns>
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            var listKnownTypes = new List<Type>();
            var modules = PackageProvider.GetModuleFileNames(true).Where(c => File.Exists(c)).ToList();
            modules.Add(PackageProvider.GetAssemblyFile(string.Empty, PackageProvider.GLOBAL_ASSEMBLY_NAME));
            foreach (var module in modules)
            {
                try
                {
                    var assembly = PackageProvider.LoadAssembly(module);
                    var types = assembly.GetTypes().Where(c => typeof(EntityObject).IsAssignableFrom(c));
                    foreach (var type in types)
                    {
                        if (listKnownTypes.FirstOrDefault(c => c.FullName.Equals(type.FullName)) == null)
                        {
                            listKnownTypes.Add(type);
                        }
                    }
                    //listKnownTypes.AddRange(assembly.GetTypes().Where(c => typeof(EntityObject).IsAssignableFrom(c)));
                }
                catch (Exception e)
                {
                    LogProvider.LogException(e);
                }
            }
            return listKnownTypes;
        }

        /// <summary>
        /// Gets list of service type
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of service type</returns>
        public static IEnumerable<Type> GetServiceTypes(string solutionName, string assemblyName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var assembly = PackageProvider.LoadAssembly(solutionName, assemblyName);
            var serviceContractTypes = assembly.GetTypes().Where(c => c.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length > 0);

            var listServiceTypes = new List<Type>();
            foreach (var serviceContractType in serviceContractTypes)
            {
                listServiceTypes.AddRange(assembly.GetTypes()
                    .Where(c => serviceContractType.IsAssignableFrom(c) && !c.IsInterface && !c.IsAbstract && !listServiceTypes.Contains(c)));
            }
            return listServiceTypes;
        }

        /// <summary>
        /// Gets list of service contract type
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <returns>List of service contract type</returns>
        public static IEnumerable<Type> GetServiceContractTypes(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return serviceType.Assembly.GetTypes()
                .Where(c => c.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length > 0 && c.IsAssignableFrom(serviceType));
        }

        /// <summary>
        /// Gets list of operation contract
        /// </summary>
        /// <param name="serviceContractType">Type or service contract</param>
        /// <returns>List of operation contract</returns>
        public static IEnumerable<MethodInfo> GetOperationContracts(Type serviceContractType)
        {
            if (serviceContractType == null)
            {
                throw new ArgumentNullException("serviceContractType");
            }
            return serviceContractType.GetMethods()
                .Where(c => c.GetCustomAttributes(typeof(OperationContractAttribute), false).Length > 0);
        }

        /// <summary>
        /// Gets service type
        /// </summary>
        /// <param name="solutionName">Name of solution</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="typeName">Full name of type</param>
        /// <returns>Service type</returns>
        public static Type GetServiceType(string solutionName, string assemblyName, string typeName)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            var assembly = PackageProvider.LoadAssembly(solutionName, assemblyName);
            return assembly.GetType(typeName, true);
        }

        /// <summary>
        /// http://localhost:{0}/{1}/{2}
        /// </summary>
        const string URI_STRING = "http://localhost:{0}/{1}/{2}";

        /// <summary>
        /// Gets service URI
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Service URI</returns>
        public static Uri GetServiceURI(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return new Uri(string.Format(URI_STRING, 8990, serviceType.Namespace, serviceType.Name));
        }

        private static Dictionary<Uri, ServiceHost> _services = new Dictionary<Uri, ServiceHost>();
        /// <summary>
        /// Gets list of service
        /// </summary>
        public static Dictionary<Uri, ServiceHost> Services
        {
            get
            {
                return _services;
            }
        }

        /// <summary>
        /// Start services in config
        /// </summary>
        public static void StartServices()
        {
            var config = new PackageConfig();
            var solutions = config.GetSolutions();
            foreach (var solution in solutions)
            {
                var modules = config.GetModules(solution, true);
                foreach (var module in modules)
                {
                    var services = config.GetServices(solution, module);
                    foreach (var service in services)
                    {
                        var serviceType = GetServiceType(solution, module, service);
                        try
                        {
                            StartService(serviceType);
                        }
                        catch(Exception e)
                        {
                            LogProvider.LogException(e);
                            config.RemoveService(solution, module, serviceType);
                            config.Save();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Starts service
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Service URI</returns>
        public static Uri StartService(Type serviceType)
        {
            return StartService(serviceType, null);
        }

        /// <summary>
        /// Starts service
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <param name="serviceContractTypes">Type of service contract</param>
        /// <returns>Service URI</returns>
        public static Uri StartService(Type serviceType, IEnumerable<Type> serviceContractTypes)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (serviceContractTypes == null)
            {
                serviceContractTypes = GetServiceContractTypes(serviceType);
            }
            var uri = GetServiceURI(serviceType);
            lock (typeof(ServiceProvider))
            {
                if (Services.ContainsKey(uri))
                {
                    
                }
                else
                {
                    var host = new ServiceHost(serviceType, uri);
                    foreach (var serviceContractType in serviceContractTypes)
                    {
                        host.AddServiceEndpoint(serviceContractType, Binding, serviceContractType.Name);
                    }
                    ApplyBehaviors(host);
                    host.Open();
                    Services.Add(uri, host);
                }
            }
            return uri;
        }

        public static Uri GetListernerServiceURI(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return new Uri(string.Format(URI_STRING, 8001, serviceType.Namespace, serviceType.Name));
        }

        public static Uri StartListernerService(Type serviceType, IEnumerable<Type> serviceContractTypes)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (serviceContractTypes == null)
            {
                serviceContractTypes = GetServiceContractTypes(serviceType);
            }
            var uri = GetListernerServiceURI(serviceType);
            lock (typeof(ServiceProvider))
            {
                if (Services.ContainsKey(uri))
                {

                }
                else
                {
                    var host = new ServiceHost(serviceType, uri);
                    foreach (var serviceContractType in serviceContractTypes)
                    {
                        host.AddServiceEndpoint(serviceContractType, Binding, serviceContractType.Name);
                    }
                    ApplyBehaviors(host);
                    host.Open();
                    Services.Add(uri, host);
                }
            }
            return uri;
        }

        /// <summary>
        /// Adds behavior to service host
        /// </summary>
        /// <param name="host">Service host</param>
        private static void ApplyBehaviors(ServiceHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            var metadataBehavior = (ServiceMetadataBehavior)GetBehavior(host, typeof(ServiceMetadataBehavior));
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(metadataBehavior);
            }
            metadataBehavior.HttpGetEnabled = true;
            var debugBehavior = (ServiceDebugBehavior)GetBehavior(host, typeof(ServiceDebugBehavior));
            if (debugBehavior == null)
            {
                debugBehavior = new ServiceDebugBehavior();
                host.Description.Behaviors.Add(debugBehavior);
            }
            debugBehavior.IncludeExceptionDetailInFaults = true;

            var silverlightFaultBehavior = (Silverlight.SilverlightFaultBehavior)GetBehavior(host, typeof(Silverlight.SilverlightFaultBehavior));
            if (silverlightFaultBehavior == null)
            {
                silverlightFaultBehavior = new Silverlight.SilverlightFaultBehavior();
                host.Description.Behaviors.Add(silverlightFaultBehavior);
            }

            foreach (var operationName in GetOperationNames())
            {
                var operation = host.Description.Endpoints[0].Contract.Operations.Find(operationName);
                if (operation != null)
                {
                    var serializerBehavior = (DataContractSerializerOperationBehavior)GetBehavior(operation, typeof(DataContractSerializerOperationBehavior));
                    if (serializerBehavior == null)
                    {
                        serializerBehavior = new DataContractSerializerOperationBehavior(operation);
                        operation.Behaviors.Add(serializerBehavior);
                    }
                    serializerBehavior.MaxItemsInObjectGraph = 2147483647;
                    //serializerBehavior.IgnoreExtensionDataObject = true;
                }
            }
        }

        private static IEnumerable<string> GetOperationNames()
        {
            return new string[] { "GetObjects", "GetDataSet" };
        }

        /// <summary>
        /// Gets behavior of service host
        /// </summary>
        /// <param name="host">Service host</param>
        /// <param name="behaviorType">type of behavior</param>
        /// <returns>behavior</returns>
        private static IServiceBehavior GetBehavior(ServiceHost host, Type behaviorType)
        {
            if (host.Description.Behaviors.Contains(behaviorType))
            {
                return host.Description.Behaviors[behaviorType];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets behavior of service operation
        /// </summary>
        /// <param name="host">Service host</param>
        /// <param name="behaviorType">type of behavior</param>
        /// <returns>behavior</returns>
        private static IOperationBehavior GetBehavior(OperationDescription opreration, Type behaviorType)
        {
            if (opreration.Behaviors.Contains(behaviorType))
            {
                return opreration.Behaviors[behaviorType];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Stops service
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        public static void StopService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            lock (typeof(ServiceProvider))
            {
                var uri = GetServiceURI(serviceType);
                if (Services.ContainsKey(uri))
                {
                    Services[uri].Close();
                    Services.Remove(uri);
                }
            }
        }

        /// <summary>
        /// Gets whether service is active
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Whether service is active</returns>
        public static bool IsServiceActive(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            lock (typeof(ServiceProvider))
            {
                var uri = GetServiceURI(serviceType);
                if (Services.ContainsKey(uri))
                {
                    return Services[uri].State == CommunicationState.Opened;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void StartCrossDomainService()
        {
            Uri crossDomainUri = new Uri("http://localhost:8990/");
            ServiceHost host = new ServiceHost(typeof(CrossDomainService), crossDomainUri);
            host.AddServiceEndpoint(typeof(ICrossDomain), new WebHttpBinding(), "");
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new WebHttpBehavior());
            }
            host.Open();
        }

        #region Binding

        /// <summary>
        /// Gets default binding
        /// </summary>
        public static Binding Binding
        {
            get
            {
                var config = new ServiceConfig();

                CustomBinding myBinding = new CustomBinding();

                // add the custom binding elements
                myBinding.Elements.Add(new BinaryMessageEncodingBindingElement()
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
                myBinding.Elements.Add(new HttpTransportBindingElement() { MaxReceivedMessageSize = 2147483647, MaxBufferSize = 2147483647 });

                return myBinding;

                return new BasicHttpBinding()
                {

                    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                    {
                        MaxArrayLength = 2147483647,
                        MaxBytesPerRead = 40960,
                        MaxDepth = 64,
                        MaxNameTableCharCount = 163840,
                        MaxStringContentLength = 20971520
                    },
                    SendTimeout = new TimeSpan(0, 30, 0),
                    ReceiveTimeout = new TimeSpan(0, 30, 0),
                    MaxBufferSize = 2147483647,
                    MaxBufferPoolSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647

                };

                //return new BasicHttpBinding()
                //{
                    
                //    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                //    {
                //        MaxArrayLength = config.MaxArrayLength,
                //        MaxBytesPerRead = config.MaxBytesPerRead,
                //        MaxDepth = config.MaxDepth,
                //        MaxNameTableCharCount = config.MaxNameTableCharCount,
                //        MaxStringContentLength = config.MaxStringContentLength
                //    },
                //    MaxBufferSize = (int)config.MaxReceivedMessageSize,
                //    MaxBufferPoolSize = config.MaxBufferPoolSize,
                //    MaxReceivedMessageSize = config.MaxReceivedMessageSize
                //};
            }
        }

        #endregion

    }

    /// <summary>
    /// Provider of log
    /// </summary>
    internal static class LogProvider
    {
        /// <summary>
        /// Error.log
        /// </summary>
        readonly static string ErrorLogFile = string.Format(@"{0}\Error.log", Environment.CurrentDirectory);

        /// <summary>
        /// DateTime
        /// </summary>
        const string DATETIME_STRING = "DateTime:{0:yyyy-MM-dd HH:mm:ss}";
        /// <summary>
        /// Type
        /// </summary>
        const string ERROR_TYPE_STRING = "Type:{0}";
        /// <summary>
        /// Message
        /// </summary>
        const string ERROR_MESSAGE_STRING = "Message:{0}";
        /// <summary>
        /// Stack
        /// </summary>
        const string ERROR_STACK_STRING = "Stack:\r\n{0}";

        /// <summary>
        /// Logs exception
        /// </summary>
        /// <param name="e"></param>
        public static void LogException(Exception e)
        {
            lock (typeof(LogProvider))
            {
                using (var writer = new StreamWriter(ErrorLogFile, true))
                {
                    writer.WriteLine(string.Format(DATETIME_STRING, DateTime.Now));
                    writer.WriteLine(string.Format(ERROR_TYPE_STRING, e.GetType().Name));
                    writer.WriteLine(string.Format(ERROR_MESSAGE_STRING, e.Message));
                    writer.WriteLine(string.Format(ERROR_STACK_STRING, e.StackTrace));
                    writer.Flush();
                }
            }
        }
    }
}
