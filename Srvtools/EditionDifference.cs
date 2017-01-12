using System;
using System.IO;
using System.Runtime.InteropServices;

#if VS90
using EnvDTE90;
#endif
using EnvDTE80;
using EnvDTE;

namespace Srvtools
{
    public static class EditionDifference
    {
        static DTE2 dte2 = (DTE2)Marshal.GetActiveObject("VisualStudio.DTE.14.0");

        public static Solution ActiveSolution()
        {
            Solution solution = null;
            if (dte2 != null)
            {
                solution = dte2.Solution;
            }
            return solution;
        }

        public static string ActiveSolutionName()
        {
            string project = "";
            Solution solution = ActiveSolution();
            if (solution != null)
            {
                project = Path.ChangeExtension(Path.GetFileName(solution.FileName), "");
                if ((project.Length >= 1) && (project[project.Length - 1] == '.'))
                {
                    project = project.Substring(0, project.Length - 1);
                }
            }
            return project;
        }

        public static Document ActiveDocument()
        {
            Document doc = null;
            if (dte2 != null)
            {
                doc = dte2.ActiveDocument;
            }
            return doc;
        }

        public static string ActiveDocumentFullName()
        {
            string docName = "";
            Document doc = ActiveDocument();
            if (doc != null)
            {
                docName = doc.FullName;
            }
            return docName;
        }

        public static string ActiveDocumentPath()
        {
            string docPath = "";
            Document doc = ActiveDocument();
            if (doc != null)
            {
                docPath = doc.Path;
            }
            return docPath;
        }

        public static void AddProjectItem(string fileName)
        {
            Document doc = ActiveDocument();
            if (doc != null)
            {
                doc.ProjectItem.ContainingProject.ProjectItems.AddFromFile(fileName);
            }
        }

        public static Window ActiveWindow()
        {
            Document actDoc = ActiveDocument();
            Window window = null;
            if (actDoc != null)
            {
                window = actDoc.ActiveWindow;
            }
            return window;
        }

        public static object ActiveWindowObject()
        {
            object obj = null;
            Window window = ActiveWindow();
            if (window != null)
            {
                obj = window.Object;
            }
            return obj;
        }
    }
}