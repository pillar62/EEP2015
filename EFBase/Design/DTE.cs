using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.IO;

namespace EFBase.Design
{
    /// <summary>
    /// DTE Object
    /// </summary>
    public static class DTE
    {
        #region Const String
        /// <summary>
        /// VisualStudio.DTE.10.0
        /// </summary>
        const string DTE_ID = "VisualStudio.DTE.11.0";

        /// <summary>
        /// Name(EFDesign)
        /// </summary>
        const string NAME = "EFDesign";
        /// <summary>
        /// DTE
        /// </summary>
        const string TYPE_NAME = "DTE";
        /// <summary>
        /// Version(1.0.0.0)
        /// </summary>
        const string VERSION = "1.0.0.0";
        /// <summary>
        /// PublicKeyToken(7dd71ffbf91e022f)
        /// </summary>
        const long PUBLICKEY_TOKEN = 0x7dd71ffbf91e022f; 
        #endregion

        /// <summary>
        /// Gets propety value of dte object
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <returns>Value of property</returns>
        private static object GetDTEValue(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            var property = DTEObject.GetType().GetProperty(name);
            if (property == null)
            {
                throw new MissingMemberException(DTEObject.GetType().Name, name);
            }
            return property.GetValue(DTEObject, null);
        }

        private static object _dteObject;
        /// <summary>
        /// Gets dte object
        /// </summary>
        private static object DTEObject
        {
            get
            {
                if (_dteObject == null)
                {
                    var assemblyName = new AssemblyName();
                    assemblyName.Name = NAME;
                    assemblyName.CultureInfo = new System.Globalization.CultureInfo(string.Empty);
                    assemblyName.Version = new Version(VERSION);
                    assemblyName.ProcessorArchitecture = ProcessorArchitecture.MSIL;
                    assemblyName.SetPublicKeyToken(BitConverter.GetBytes(PUBLICKEY_TOKEN).Reverse().ToArray());

                    Assembly assembly = Assembly.Load(assemblyName);
                    Type type = assembly.GetTypes().FirstOrDefault(c => c.Name.Equals(TYPE_NAME));
                    if (type == null)
                    {
                        throw new EntryPointNotFoundException(string.Format("TYPE_NAME:{0} not found.", TYPE_NAME));
                    }
                    _dteObject = type.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { DTE_ID });
                }
                return _dteObject;
            }
        }

        /// <summary>
        /// Gets full name of active document
        /// </summary>
        public static string ActiveDocumentFullName
        {
            get
            {
                return (string)GetDTEValue("ActiveDocumentFullName");
            }
        }

        /// <summary>
        /// Gets current directory
        /// </summary>
        public static string CurrentDirectory
        {
            get
            {
                var activeDocumentFullName = ActiveDocumentFullName;
                return string.IsNullOrEmpty(activeDocumentFullName) ? string.Empty : Path.GetDirectoryName(activeDocumentFullName);
            }
        }

        /// <summary>
        /// Gets full name of solution
        /// </summary>
        public static string SolutionFullName
        {
            get
            {
                return (string)GetDTEValue("SolutionFullName");
            }
        }

        public static string CurrentSolution
        {
            get
            {
                var solutionFullName = SolutionFullName;
                return string.IsNullOrEmpty(solutionFullName) ? string.Empty : Path.GetFileNameWithoutExtension(solutionFullName);
            }
        }

        public static string GetProjectFullName(string projectKind)
        {
            var method = DTEObject.GetType().GetMethod("GetProjectFullName");
            return (string)method.Invoke(DTEObject, new object[] { projectKind });
        }
    }
}
