using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using EnvDTE80;
using EnvDTE;

namespace EFDesign
{
    public class DTE
    {
        public DTE(string progID)
        {
           _dteObject =  (DTE2)Marshal.GetActiveObject(progID);
        }

        private DTE2 _dteObject;

        private DTE2 DTEObject
        {
            get 
            {
                return _dteObject;
            }
        }

        public string ActiveDocumentFullName
        {
            get
            {
                return DTEObject.ActiveDocument != null ? DTEObject.ActiveDocument.FullName : string.Empty;
            }
        }

        public string SolutionFullName
        {
            get
            {
                return DTEObject.Solution != null ? DTEObject.Solution.FullName : string.Empty;
            }
        }

        public string GetProjectFullName(string projectKind)
        {
            foreach (Project project in DTEObject.Solution.Projects)
            {
                if (project.Kind == projectKind)
                {
                    return project.FullName; 
                }
            }
            return string.Empty;
        }
    }
}
