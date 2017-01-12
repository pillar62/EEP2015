using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using EnvDTE;
using System.Threading;
using EFClientTools.EFServerReference;
using EFClientTools.Web;
using EFClientTools.Beans;

namespace MWizard2015.WCF
{
    public class EFAssembly : IDisposable
    {
        private static byte[] buffer = null;

        public static Assembly LoadAssembly(string assemblyPath)
        {
            Assembly assembly = null;


            try
            {
                if (WzdUtils.FAddIn == null)
                {
                    ArgumentNullException exception = new ArgumentNullException("WzdUtils.FAddIn");

                    WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(exception));
                }
                else
                {
                    string path = WzdUtils.GetServerPath(WzdUtils.FAddIn, true);
                    path = path.Remove(path.LastIndexOf('\\')) + assemblyPath;
                    String fullDllName = path;
                    buffer = System.IO.File.ReadAllBytes(fullDllName);
                    assembly = Assembly.Load(buffer);
                }
            }
            catch (Exception ex)
            {
                WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
            }

            return assembly;
        }

        //private static AddIn vAddIn;

        //public static AddIn VAddIn
        //{
        //    get { return EFAssembly.vAddIn; }
        //    set { EFAssembly.vAddIn = value; }
        //}

        public void Dispose()
        {
            buffer = null;
        }

        public class EFClientToolsAssemblyAdapt
        {
            private const string EFCLIENTTOOLSPATH = "\\EFClientTools\\bin\\Debug\\";
            private const string EFCLIENTTOOLSDLL = "EFClientTools.dll";
            private const string REMOTENAMEEDITORDIALOGTYPE = "EFClientTools.Editor.RemoteNameEditorDialog";
            private const string CLIENTENTITYPROVIDER = "EFClientTools.Common.EntityProvider";
            private const string DESIGNCLIENTUTILITY = "EFClientTools.DesignClientUtility";

            private static Assembly _EFClientToolsAssembly;
            public static Assembly EFClientToolsAssembly
            {
                get
                {
                    _EFClientToolsAssembly = LoadEFClientTools();
                    return _EFClientToolsAssembly;
                }
                set { _EFClientToolsAssembly = value; }
            }

            public static Assembly LoadEFClientTools()
            {
                return LoadAssembly(EFCLIENTTOOLSPATH + EFCLIENTTOOLSDLL);
            }

            public class RemoteNameEditorDialog
            {
                public RemoteNameEditorDialog(string remoteName)
                {
                    remoteNameEditorDialogType = EFClientToolsAssembly.GetType(REMOTENAMEEDITORDIALOGTYPE);
                    remoteNameEditorDialogObject = Activator.CreateInstance(RemoteNameEditorDialogType, new object[] { remoteName });
                }

                public RemoteNameEditorDialog()
                    : this(string.Empty)
                {

                }

                private Type remoteNameEditorDialogType;
                public Type RemoteNameEditorDialogType
                {
                    get { return remoteNameEditorDialogType; }
                }

                private object remoteNameEditorDialogObject;
                private object RemoteNameEditorDialogObject
                {
                    get { return remoteNameEditorDialogObject; }
                }

                public Form RemoteNameEditorDialogForm
                {
                    get { return (Form)RemoteNameEditorDialogObject; }
                }

                private object GetValue(string propertyName)
                {
                    return RemoteNameEditorDialogObject.GetType().GetProperty(propertyName).GetValue(RemoteNameEditorDialogObject, null);
                }

                public string ReturnValue
                {
                    get { return GetValue("ReturnValue").ToString(); }
                }

                public string SelectedCommandName
                {
                    get { return GetValue("SelectedCommandName").ToString(); }
                }

                public string ReturnClassName
                {
                    get { return GetValue("ReturnClassName").ToString(); }
                }

                public string EntitySetName
                {
                    get { return GetValue("EntitySetName").ToString(); }
                }
            }

            public static class EntityProvider
            {
                static EntityProvider()
                {
                    entityProviderObject = Activator.CreateInstance(EFClientToolsAssembly.GetType(CLIENTENTITYPROVIDER));
                }

                private static object entityProviderObject;
                public static object EntityProviderObject
                {
                    get { return entityProviderObject; }
                }

                private static object InvokeMethod(string methodName, object[] parameters)
                {
                    return EntityProviderObject.GetType().GetMethod(methodName).Invoke(null, parameters);
                }

                [Obsolete]
                public static Hashtable GetEntityPropertiesTypes(string entityClassName)
                {
                    return InvokeMethod("GetEntityPropertiesTypes", new object[] { entityClassName }) as Hashtable;
                }

                public static List<string> GetDetailEntityClassNames(string masterClassName)
                {
                    return InvokeMethod("GetDetailEntityClassNames", new object[] { masterClassName }) as List<string>;
                }

                public static Dictionary<String, String> GetDetailEntityClassNameAndEntitySetName(string masterClassName)
                {
                    return InvokeMethod("GetDetailEntityClassNameAndEntitySetName", new object[] { masterClassName }) as Dictionary<String, String>;
                }

                public static List<string> GetEntityNavigationFields(string masterClassName)
                {
                    return InvokeMethod("GetEntityNavigationFields", new object[] { masterClassName }) as List<string>;
                }

                public static List<string> GetClientEntityProperties(EFDataSource eds, Assembly assembly)
                {
                    return InvokeMethod("GetClientEntityProperties", new object[] { eds, assembly }) as List<string>;
                }

                public static string GetServerEntityClassName(string assemblyName, string clientEntityClassName)
                {
                    return InvokeMethod("GetServerEntityClassName", new object[] { assemblyName, clientEntityClassName }) as string;
                }
            }

            public static class DesignClientUtility
            {
                static DesignClientUtility()
                {
                    designClientUtilityObject = Activator.CreateInstance(EFClientToolsAssembly.GetType(DESIGNCLIENTUTILITY));
                }

                private static object designClientUtilityObject;
                public static object DesignClientUtilityObject
                {
                    get { return designClientUtilityObject; }
                }

                private static object InvokeMethod(string methodName, object[] parameters)
                {
                    return DesignClientUtilityObject.GetType().GetMethod(methodName).Invoke(null, parameters);
                }

                public static List<string> GetCommandNames(string assemblyName)
                {
                    return InvokeMethod("GetCommandNames", new object[] { assemblyName }) as List<string>;
                }

                public static List<string> GetModuleNames()
                {
                    return InvokeMethod("GetModuleNames", null) as List<string>;
                }

                [Obsolete]
                public static List<COLDEFInfo> GetColumnDefination(string assemblyName, string commandName, string entityTypeName)
                {
                    return InvokeMethod("GetColumnDefination", new object[] { assemblyName, commandName, entityTypeName }) as List<COLDEFInfo>;
                }

                public static Dictionary<string, object> GetEntityPropertiesTypes(string assemblyName, string commandName, string entityTypeName)
                {
                    return InvokeMethod("GetEntityPropertiesTypes", new object[] { assemblyName, commandName, entityTypeName }) as Dictionary<string, object>;
                }

                public static Dictionary<string, string> GetEntityPropertieMappings(string assemblyName, string commandName, string entityTypeName)
                {
                    return InvokeMethod("GetEntityPropertieMappings", new object[] { assemblyName, commandName, entityTypeName }) as Dictionary<string, string>;
                }

                public static List<string> GetEntityPrimaryKeys(string assemblyName, string commandName, string clientEntityClassName)
                {
                    return InvokeMethod("GetEntityPrimaryKeys", new object[] { assemblyName, commandName, clientEntityClassName }) as List<string>;
                }

                public static List<EntityObject> GetAllDataByTableName(String tableName)
                {
                    return InvokeMethod("GetAllDataByTableName", new object[] { tableName }) as List<EntityObject>;
                }

                public static List<EntityObject> GetAllDataByTableName(String dbAlias, String tableName)
                {
                    return InvokeMethod("GetAllDataByTableName", new object[] { dbAlias, tableName }) as List<EntityObject>;
                }
            }
        }


    }
}
