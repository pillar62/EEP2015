using System.Xml;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Net;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using mshtml;
using System.Text;
using EnvDTE80;
using EnvDTE;
using System;
using Srvtools;
using System.Collections.Generic;

namespace EEPSDModule2015
{
    public class SDMOduleDTE
    {
        private string slnName;
        private string slnFullName;
        private string csproj;
        private string dllproj;
        private string uniqueName;
        private string formName;
        private string userID;
        private string password;
        private string database;
        private string eepDirectory;
        private string printWaitingTime;
        private string aspxproj;
        private object[] Directorys;
        private string WebSiteName;
        private string WebSitePath;
        private bool[] documentSetting;
        private int printLanguage;
        string PageTitle = "";
        private DTE2 dte2 = null;
        Object param;

        static SYS_LANGUAGE language = SYS_LANGUAGE.TRA;


        public SDMOduleDTE(Object param, string userID, string password, string database, string eepDirectory, string printWaitingTime, bool[] documentsetting ,DTE2 dte2,int printLanguage)
        {
            this.userID = userID;
            this.password = password;
            this.database = database;
            this.eepDirectory = eepDirectory;
            this.printWaitingTime = printWaitingTime;
            this.documentSetting = documentsetting;
            object[] RealParams = (object[])param;
            this.printLanguage = printLanguage;
            this.dte2 = dte2;
            if (((string[])RealParams[0])[0] == "sln")
            {
                slnFullName = ((string[])RealParams[0])[1];
                slnName = ((string[])RealParams[0])[2];
            }

            if (((string[])RealParams[1])[0] == "csproj" )
            {
                csproj = ((string[])RealParams[1])[2];
                uniqueName = ((string[])RealParams[1])[1];
            }

            if (((string[])RealParams[1])[0] == "dllproj")
            {
                dllproj = ((string[])RealParams[1])[2];
                uniqueName = ((string[])RealParams[1])[1];
            }
            if (((string[])RealParams[1])[0] == "website")
            {
                WebSitePath = ((string[])RealParams[1])[1];
                WebSiteName = ((string[])RealParams[1])[2];
            }

            if (((string[])RealParams[2])[0] == "cs")
            {
                formName = ((string[])RealParams[2])[2];
            }

            if (((string[])RealParams[2])[0] == "aspx")
            {
                aspxproj = ((string[])RealParams[2])[2];
                string directorystring = ((string[])RealParams[2])[1];
                if (directorystring.StartsWith("\\"))
                {
                    directorystring = directorystring.Substring(1);
                }
                if (directorystring == "")
                    Directorys = new object[0];
                else
                {
                    string[] directorystrings = directorystring.Split(new char[] { '\\' });

                    Directorys = new object[directorystrings.Length];
                    for (int i = 0; i < directorystrings.Length; i++)
                        Directorys[i] = directorystrings[i];
                }
            }

        }

        public Object WinWork(/*bool islast, ref DTE2 dte*/)
        {
            //this.dte2 = dte;
            //if (dte2 == null)
            //{
                //string proi = OverallMethod.SYS_VS == "EEP2006" ? "VisualStudio.DTE.8.0" : "VisualStudio.DTE.9.0";
                //System.Type type = System.Type.GetTypeFromProgID(proi);
                //object obj = System.Activator.CreateInstance(type);
                //dte2 = (DTE2)obj;
                //}
            try
            {
                //MessageFilter.Register();

                dte2.MainWindow.Activate();
                dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                Application.DoEvents();
                dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMaximize;
                Application.DoEvents();

                if (dte2.Solution.FullName != slnFullName)
                    dte2.Solution.Open(slnFullName);
                System.Threading.Thread.Sleep(1000);

                Solution solu = null;
                if (dte2 != null)
                {
                    solu = dte2.Solution;
                }
                try
                {
                    CliUtils.LoadLoginServiceConfig(eepDirectory + @"\AddIns\MWizard.dll.config");
                }
                catch
                {
                    MessageBox.Show(PublicTest.GetSystemMessage(language, "SDModule", "Dte", "WrongMWizard"), "IPC");
                }
                string dllname = "";
                string dllfullname = "";
                ProjectItem selectedItem = null;
                foreach (Project P in solu.Projects)
                {
                    if (P.UniqueName == uniqueName)
                    {
                        object[] objTemp = (object[])P.ConfigurationManager.ActiveConfiguration.OutputGroups.Item("Built").FileNames;
                        dllname = (string)objTemp[0];
                        objTemp = (object[])P.ConfigurationManager.ActiveConfiguration.OutputGroups.Item("Built").FileURLs;
                        dllfullname = (string)objTemp[0];
                        if (dllfullname.StartsWith("file:///"))
                        { dllfullname = dllfullname.Substring(8); }
                        foreach (ProjectItem fPI in P.ProjectItems)
                        {
                            if (fPI.Name.CompareTo(formName) == 0)
                            {
                                selectedItem = fPI;
                            }
                        }
                        break;
                    }
                    else if (P.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                    {
                        foreach (ProjectItem PI in P.ProjectItems)
                        {
                            if (PI.Kind == "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}")
                            {
                                if (PI.SubProject != null)
                                {
                                    if (PI.SubProject.UniqueName == uniqueName)
                                    {
                                        Project fP = PI.SubProject;
                                        object[] objTemp = (object[])fP.ConfigurationManager.ActiveConfiguration.OutputGroups.Item("Built").FileNames;
                                        dllname = (string)objTemp[0];
                                        objTemp = (object[])fP.ConfigurationManager.ActiveConfiguration.OutputGroups.Item("Built").FileURLs;
                                        dllfullname = (string)objTemp[0];
                                        if (dllfullname.StartsWith("file:///"))
                                        { dllfullname = dllfullname.Substring(8); }
                                        foreach (ProjectItem fPI in fP.ProjectItems)
                                        {
                                            if (fPI.Name.CompareTo(formName) == 0)
                                            {
                                                selectedItem = fPI;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                param = new object[] { dllfullname, formName.Substring(0, formName.IndexOf('.')), userID, password, database, "", slnName.Substring(0, slnName.IndexOf('.')), slnFullName, selectedItem };
                if (dllfullname == "" || dllfullname == null)
                { return new Object[] { -1, PublicTest.GetSystemMessage(language, "SDModule", "Dte", "projectDLLnotFind") }; }
                if (formName == "" || formName == null)
                { return new Object[] { -1, PublicTest.GetSystemMessage(language, "SDModule", "Dte", "FormnotFind") }; }
                if (slnName == "" || slnName == null || slnFullName == "" || slnFullName == null)
                { return new Object[] { -1, PublicTest.GetSystemMessage(language, "SDModule", "Dte", "solutionnotFind") }; }
                if (selectedItem == null)
                { return new Object[] { -1, PublicTest.GetSystemMessage(language, "SDModule", "Dte", "projectItemnotFind") }; }

                Object objback = GetFormImage(param);
                if ((int)((object[])objback)[0] == -1)
                { return new Object[] { -1, (string)((object[])objback)[1] }; }
                else if ((int)((object[])objback)[0] == 1)
                {
                    return new object[] { 1, (object[])((object[])objback)[1] };
                }
            }
            catch (Exception ex)
            {
                return new Object[] { -1, ex.Message + ((ex.InnerException != null) ? ("\r\n" + ex.InnerException.Message) : "") };
            }
            finally
            {
                //if (islast)
                //{
                    //dte2.Quit();

                //}
                //MessageFilter.Revoke();
                dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;

                System.Threading.Thread.Sleep(/*3*/1000);
            }

            return null;
        }

        public Object WebWork(/*bool islast,ref DTE2 dte*/)
        {
            try
            {
                dte2.MainWindow.Activate();
                if (dte2.Solution.FullName != slnFullName)
                    dte2.Solution.Open(slnFullName);
                System.Threading.Thread.Sleep(3000);
                Solution solu = null;
                if (dte2 != null)
                {
                    solu = dte2.Solution;
                }
                Object Params = new Object[] { slnFullName, WebSiteName, WebSitePath, Directorys, aspxproj, userID, password, database, slnName.Substring(0, slnName.IndexOf('.')), printWaitingTime };
                Object objback = GetPageInfo(Params);
                if ((int)((object[])objback)[0] == -1)
                { return new Object[] { -1, (string)((object[])objback)[1] }; }
                else if ((int)((object[])objback)[0] == 1)
                {
                    return new object[] { 1, (object[])((object[])objback)[1] };
                }
            }
            catch (Exception ex)
            {
                return new Object[] { -1, ex.Message + ((ex.InnerException != null) ? ("\r\n" + ex.InnerException.Message) : "") };
            }
            finally
            {
                //if (islast)
                //{
                    //dte2.Quit();
                //}
                //else { dte2.MainWindow.Visible = false; dte = dte2; }
                //MessageFilter.Revoke();
                //dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                System.Threading.Thread.Sleep(/*3*/1000);
            }
            return null;
        }

        public Object JQWork(/*bool islast,ref DTE2 dte*/)
        {
            try
            {
                dte2.MainWindow.Activate();
                if (dte2.Solution.FullName != slnFullName)
                    dte2.Solution.Open(slnFullName);
                System.Threading.Thread.Sleep(3000);
                Solution solu = null;
                if (dte2 != null)
                {
                    solu = dte2.Solution;
                }
                Object Params = new Object[] { slnFullName, WebSiteName, WebSitePath, Directorys, aspxproj, userID, password, database, slnName.Substring(0, slnName.IndexOf('.')), printWaitingTime };
                Object objback = GetJQPageInfo(Params);
                if ((int)((object[])objback)[0] == -1)
                { return new Object[] { -1, (string)((object[])objback)[1] }; }
                else if ((int)((object[])objback)[0] == 1)
                {
                    return new object[] { 1, (object[])((object[])objback)[1] };
                }
            }
            catch (Exception ex)
            {
                return new Object[] { -1, ex.Message + ((ex.InnerException != null) ? ("\r\n" + ex.InnerException.Message) : "") };
            }
            finally
            {
                //if (islast)
                //{
                //dte2.Quit();
                //}
                //else { dte2.MainWindow.Visible = false; dte = dte2; }
                //MessageFilter.Revoke();
                //dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                System.Threading.Thread.Sleep(/*3*/1000);
            }
            return null;
        }


        public Object ServiceWork(/*bool islast,ref DTE2 dte*/)
        {
            try
            {
                dte2.MainWindow.Activate();
                if (dte2.Solution.FullName != slnFullName)
                    dte2.Solution.Open(slnFullName);
                System.Threading.Thread.Sleep(3000);
                Solution solu = null;
                if (dte2 != null)
                {
                    solu = dte2.Solution;
                }
                Object Params = new Object[] { slnFullName, dllproj, userID, password, database, slnName.Substring(0, slnName.IndexOf('.')) };
                Object objback = GetServiceInfo(Params);
                if ((int)((object[])objback)[0] == -1)
                {
                    return new Object[] { -1, (string)((object[])objback)[1] };
                }
                else if ((int)((object[])objback)[0] == 1)
                {
                    return new object[] { 1, (object[])((object[])objback)[1] };
                }
            }
            catch (Exception ex)
            {
                return new Object[] { -1, ex.Message + ((ex.InnerException != null) ? ("\r\n" + ex.InnerException.Message) : "") };
            }
            finally
            {
                //if (islast)
                //{
                //dte2.Quit();
                //}
                //else { dte2.MainWindow.Visible = false; dte = dte2; }
                //MessageFilter.Revoke();
                //dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                System.Threading.Thread.Sleep(/*3*/1000);
            }
            return null;
        }

        public Object GetServiceInfo(Object Params)
        {
            try
            {
                Object[] RealParams = (object[])Params;
                String SolutionFileName = RealParams[0].ToString();
                String ProjectName = RealParams[1].ToString();
                String UserID = RealParams[2].ToString();
                String Password = RealParams[3].ToString();
                String DBName = RealParams[4].ToString();
                String Solutionname = RealParams[5].ToString();
                CheckRemotingObject(UserID, Password, DBName, "", Solutionname);
                if (dte2.Solution == null || dte2.Solution.FileName.CompareTo(SolutionFileName) != 0)
                    dte2.Solution.Open(SolutionFileName);

                ArrayList RemoteNameList = new ArrayList();
                ArrayList InfoCommandList = new ArrayList();
                ArrayList UpdateComponentList = new ArrayList();
                List<string> DataModuleFileList = new List<string>();
                ArrayList CodeList = new ArrayList();
                foreach (Project P in dte2.Solution.Projects)
                {
                    Project aProject = P;
                    System.Type ty = null;
                    ProjectItem PI = FindProjectItem(ref aProject, ProjectName, ref ty);
                    if (PI != null)
                    {
                        //Get Server Form Infomation
                        DataModuleFileList.Add(CaptureDataModule(SolutionFileName, ProjectName, InfoCommandList, UpdateComponentList, CodeList));
                    }
                }

                Object[] aModuleList = new Object[DataModuleFileList.Count];
                for (int I = 0; I < aModuleList.Length; I++)
                    aModuleList[I] = DataModuleFileList[I];
                Object[] aCommandList = new Object[InfoCommandList.Count];
                for (int I = 0; I < aCommandList.Length; I++)
                    aCommandList[I] = InfoCommandList[I];
                Object[] aUpdateList = new Object[UpdateComponentList.Count];
                for (int I = 0; I < aUpdateList.Length; I++)
                    aUpdateList[I] = UpdateComponentList[I];
                Object[] aCodeList = new Object[CodeList.Count];
                for (int I = 0; I < aCodeList.Length; I++)
                    aCodeList[I] = CodeList[I];

                return new object[] { 1, new Object[] { null, null, null,null, aModuleList, null, aCommandList, aUpdateList, aCodeList } };
            }
            catch (Exception E)
            {
                return new Object[] { -1, E.Message + ((E.InnerException != null) ? ("\r\n" + E.InnerException.Message) : "") };
            }
        }

        public Object GetFormImage(Object Params)
        {
            object[] RealParams = (Object[])Params;
            string DllFileName = RealParams[0].ToString();
            string FormName = RealParams[1].ToString();
            string DllName = DllFileName;
            string UserID = RealParams[2].ToString();
            string Password = RealParams[3].ToString();
            string DBName = RealParams[4].ToString();
            string SiteCode = RealParams[5].ToString();
            string CurrentProject = RealParams[6].ToString();
            string SolutionFileName = RealParams[7].ToString();
            ProjectItem selectedItem = (ProjectItem)RealParams[8];
            CheckRemotingObject(UserID, Password, DBName, SiteCode, CurrentProject);
            InfoForm aForm = null;
            Object TextBoxList = null;
            Object GridColumnList = null;
            Object DefaultValidateList = null;
            ArrayList InfoCommandList = new ArrayList();
            ArrayList UpdateComponentList = new ArrayList();
            ArrayList CodeList = new ArrayList();
            Object[] Tables = null;
            List<string> DataModuleFileList = new List<string>();
            //copy xml from EEPNetClient to SDModule folder
            string eepDirectory = EEPRegistry.Client + "\\" + CurrentProject;
            string sdDirectory = Directory.GetCurrentDirectory() + "\\" + CurrentProject;
            if (Directory.Exists(eepDirectory))
            {
                bool b = Directory.Exists(sdDirectory);

                string[] files = Directory.GetFiles(eepDirectory);
                foreach (string filename in files)
                {
                    FileInfo fi = new FileInfo(filename);
                    if (fi.Extension.ToLower() == ".xml")
                    {
                        if (!b)
                        {
                            Directory.CreateDirectory(sdDirectory);
                        }
                        fi.CopyTo(sdDirectory + "\\" + fi.Name, true);
                    }
                }
            }
            //end
            try
            {
                DllName = System.IO.Path.GetFileName(DllFileName);
                DllName = DllName.Substring(0, DllName.IndexOf('.', 1));

                Assembly A = Assembly.LoadFile(DllFileName);
                Type myType = A.GetType(DllName + "." + FormName);

                if (myType != null)
                {
                    object o2 = Activator.CreateInstance(myType);
                    if (!(o2 is Srvtools.InfoForm))
                    {
                        string sMess = PublicTest.GetSystemMessage(language, "SDModule", "Dte", "NotEEPForm");
                        return new Object[] { -1, String.Format(sMess, FormName) };
                    }
                    aForm = (Srvtools.InfoForm)o2;
                    aForm.SetOwnerComponent();
                    InfoDataSet Master = aForm.GetIntfObject(typeof(IInfoDataSet)) as InfoDataSet;
                    String ModuleName = Master.RemoteName;
                    string FileName = string.Empty;
                    if (documentSetting[0])
                    {
                        #region get winform image
                        Application.DoEvents();
                        aForm.Show();
                        aForm.Activate();
                        Application.DoEvents();
                        Type t = aForm.GetType();
                        FieldInfo[] myFields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                        for (int i = 0; i < myFields.Length; i++)
                        {
                            object newobj = myFields[i].GetValue(aForm);
                            if ((null != newobj) && (null != newobj.GetType()) && (newobj.GetType().ToString() == "Srvtools.MultiLanguage"))
                            {
                                MultiLanguage oMul = (MultiLanguage)newobj;
                                //printLanaguage can not change the combobox item
                                if (this.printLanguage == 0)
                                    oMul.GroupIndex = MultiLanguage.LanguageGroups.English;
                                else if (this.printLanguage == 1)
                                    oMul.GroupIndex = MultiLanguage.LanguageGroups.ChineseTra;
                                else if (this.printLanguage == 2)
                                    oMul.GroupIndex = MultiLanguage.LanguageGroups.ChineseSim;
                                else
                                    oMul.GroupIndex = MultiLanguage.LanguageGroups.English;

                                oMul.SetLanguage(false);
                            }
                        }
                        aForm.Activate();
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(500);
                        Srvtools.ScreenCapture B = new ScreenCapture();
                        FileName = Path.GetTempPath() + Path.GetRandomFileName() + ".jpg";
                        B.CaptureWindowToFile(aForm.Handle, FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                    }
                    aForm.Visible = false;
                    Application.DoEvents();
                    if (ModuleName != "")
                    {
                        ModuleName = ModuleName.Substring(0, ModuleName.IndexOf('.'));
                        String DataSetName = Master.RemoteName;
                        DataSetName = DataSetName.Substring(DataSetName.IndexOf('.') + 1, DataSetName.Length - DataSetName.IndexOf('.') - 1);
                        DataSet aDataSet = CliUtils.GetSqlCommand(ModuleName, DataSetName, Master, "", CurrentProject, "");
                        String TableName = GetTableNameByProvider(CurrentProject, ModuleName, DataSetName);
                        Tables = new Object[2];
                        Tables[0] = TableName;
                        Object[] ChildRelation = new Object[aDataSet.Tables[0].ChildRelations.Count];
                        Tables[1] = ChildRelation;
                        GetRealChildTable(CurrentProject, ModuleName, aDataSet.Tables[0].ChildRelations, ref ChildRelation);
                        //Capture Form

                        //get client code
                        if (documentSetting[9])
                        {
                            object[] ClientCode = GetClientCode(selectedItem);
                            CodeList.Add(ClientCode);
                        }

                        //server
                        if (documentSetting[1] || documentSetting[7] || documentSetting[8] || documentSetting[10])
                            DataModuleFileList.Add(CaptureDataModule(SolutionFileName, ModuleName, InfoCommandList, UpdateComponentList, CodeList));

                        //3
                        if (documentSetting[4])
                            TextBoxList = GetTextBoxList(aForm, CurrentProject);
                        //4
                        if (documentSetting[5])
                            GridColumnList = GetGridColumnList(aForm, CurrentProject);
                        //5
                        if (documentSetting[6])
                            DefaultValidateList = GetDefaultValidateList(aForm, CurrentProject);
                    }
                    else
                    {
                        string sMess = PublicTest.GetSystemMessage(language, "SDModule", "Dte", "NullRemotename");
                        return new Object[] { -1, String.Format(sMess, DllName, FormName) };
                    }
                    Application.DoEvents();
                    aForm.Dispose();
                    Application.DoEvents();
                    //dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                    //Application.DoEvents();
                    //dte2.MainWindow.WindowState = vsWindowState.vsWindowStateMaximize;
                    //Application.DoEvents();

                    Object[] aModuleList = new Object[DataModuleFileList.Count];
                    for (int I = 0; I < aModuleList.Length; I++)
                        aModuleList[I] = DataModuleFileList[I];
                    Object[] aCommandList = new Object[InfoCommandList.Count];
                    for (int I = 0; I < aCommandList.Length; I++)
                        aCommandList[I] = InfoCommandList[I];
                    Object[] aUpdateList = new Object[UpdateComponentList.Count];
                    for (int I = 0; I < aUpdateList.Length; I++)
                        aUpdateList[I] = UpdateComponentList[I];
                    Object[] aCodeList = new Object[CodeList.Count];
                    for (int I = 0; I < aCodeList.Length; I++)
                        aCodeList[I] = CodeList[I];
                    return new object[] { 1, new Object[] { FileName, Tables, TextBoxList, GridColumnList, aModuleList, DefaultValidateList, aCommandList, aUpdateList, aCodeList } };
                }
                else
                {
                    string sMess = PublicTest.GetSystemMessage(language, "SDModule", "Dte", "FormDllNotFind");
                    return new Object[] { -1, String.Format(sMess, FormName, DllName) };
                }

            }

            catch (Exception E)
            {
                return new Object[] { -1, E.Message + ((E.InnerException != null) ? ("\r\n" + E.InnerException.Message) : "") };
                //MessageBox.Show(E.ToString());
            }
            finally
            {
                if (aForm != null)
                    aForm.Dispose();
            }

        }

        private object[] GetClientCode(ProjectItem selectedItem)
        {
            Window W = selectedItem.Open(Constants.vsViewKindCode);
            W.SetFocus();
            TextSelection ts = (TextSelection)W.Selection;
            ts.SelectAll();
            string tst = ts.Text;
            W.Close(vsSaveChanges.vsSaveChangesNo);
            return new object[] { selectedItem.Name, tst };
        }

        private String GetTableNameByProvider(String SolutionName, String ModuleName, String DataSetName)
        {
            return CliUtils.GetTableName(ModuleName, DataSetName, SolutionName);
        }

        private String GetTableNameByRelationName(DataRelationCollection Relations, String RelationName, String ModuleName, String SolutionName)
        {
            foreach (DataRelation R in Relations)
            {
                if (String.Compare(R.RelationName, RelationName) == 0)
                {
                    return CliUtils.GetTableName(ModuleName, R.ChildTable.TableName, SolutionName);
                }
                else
                {
                    return GetTableNameByRelationName(R.ChildTable.ChildRelations, RelationName, ModuleName, SolutionName);
                }
            }
            return RelationName;
        }
        private void GetRealChildTable(String SolutionName, String ModuleName, DataRelationCollection Relations, ref Object[] Tables)
        {
            int Index = 0;
            foreach (DataRelation R in Relations)
            {
                Object[] Child = new Object[2];
                String DataSetName = R.ChildTable.TableName;
                String TableName = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName);
                Child[0] = TableName;
                Object[] ChildRelation = new Object[R.ChildTable.ChildRelations.Count];
                Child[1] = ChildRelation;
                Tables[Index++] = Child;
                GetRealChildTable(SolutionName, ModuleName, R.ChildTable.ChildRelations, ref ChildRelation);
            }
        }
        private Object GetTextBoxList(InfoForm aForm, String SolutionName)
        {
            ArrayList List = new ArrayList();
            DoGetTextBoxList(aForm, List, SolutionName);
            Object[] Result = new Object[List.Count];
            for (int I = 0; I < List.Count; I++)
                Result[I] = List[I];
            return Result;
        }
        private void DoGetTextBoxList(Control aForm, ArrayList List, String SolutionName)
        {
            foreach (Control C in aForm.Controls)
            {
                if (C.GetType().Equals(typeof(InfoTextBox)) || C.GetType().Equals(typeof(TextBox)))
                {
                    TextBox TB = (TextBox)C;
                    if (TB.DataBindings.Count > 0)
                    {
                        Binding B = TB.DataBindings[0];
                        string TableName = GetTableNameByBindingSource((InfoBindingSource)B.DataSource, SolutionName);
                        List.Add(TableName + "." + B.BindingMemberInfo.BindingField);
                    }
                }
                DoGetTextBoxList(C, List, SolutionName);
            }
        }
        private String GetTableNameByBindingSource(InfoBindingSource aBindingSource, String SolutionName)
        {
            String RelationName = aBindingSource.DataMember;
            while (aBindingSource.DataSource.GetType().ToString() != typeof(InfoDataSet).ToString())
                aBindingSource = (InfoBindingSource)aBindingSource.DataSource;
            InfoDataSet aDataSet = (InfoDataSet)aBindingSource.DataSource;
            String ModuleName = aDataSet.RemoteName;
            ModuleName = ModuleName.Substring(0, ModuleName.IndexOf('.'));
            if (aDataSet.RealDataSet.Tables[0].TableName == RelationName)
            {
                return CliUtils.GetTableName(ModuleName, RelationName, SolutionName);
            }
            else
            {
                return GetTableNameByRelationName(aDataSet.RealDataSet.Tables[0].ChildRelations, RelationName, ModuleName, SolutionName);
            }
        }
        private Object GetGridColumnList(InfoForm aForm, String SolutionName)
        {
            ArrayList List = new ArrayList();
            DoGetGridColumnList(aForm, List, SolutionName);
            Object[] Result = new Object[List.Count];
            for (int I = 0; I < List.Count; I++)
            {
                ArrayList GridData = (ArrayList)List[I];
                ArrayList ColumnList = (ArrayList)GridData[1];
                Object[] ColumnObject = new Object[ColumnList.Count];
                for (int J = 0; J < ColumnList.Count; J++)
                    ColumnObject[J] = ColumnList[J];
                Result[I] = new Object[2] { GridData[0], ColumnObject };
            }
            return Result;
        }
        private void DoGetGridColumnList(Control aForm, ArrayList List, String SolutionName)
        {
            ArrayList ColumnList = new ArrayList();
            ArrayList GridData = new ArrayList();
            foreach (Control C in aForm.Controls)
            {
                if (C.GetType().Equals(typeof(InfoDataGridView)) || C.GetType().Equals(typeof(DataGridView)))
                {
                    DataGridView Grid = (DataGridView)C;
                    foreach (DataGridViewColumn Column in Grid.Columns)
                    {
                        if (Column.DataPropertyName != null && Column.DataPropertyName != "" && Column.HeaderText != null)
                            ColumnList.Add(Column.DataPropertyName + "." + Column.HeaderText);
                    }
                    if (Grid.DataSource != null && Grid.DataSource.ToString() != "")
                    {
                        string TableName = GetTableNameByBindingSource((InfoBindingSource)Grid.DataSource, SolutionName);
                        GridData.Add(Grid.Name + "." + TableName);
                        GridData.Add(ColumnList);
                        List.Add(GridData);
                    }
                }
                DoGetGridColumnList(C, List, SolutionName);
            }
        }
        private Object GetDefaultValidateList(InfoForm aForm, String SolutionName)
        {
            ArrayList List = new ArrayList();
            DoGetDefaultValidateList(aForm, List, SolutionName);
            Object[] Result = new Object[List.Count];
            for (int I = 0; I < List.Count; I++)
                Result[I] = List[I];
            return Result;
        }
        private void DoGetDefaultValidateList(Control aForm, ArrayList List, String SolutionName)
        {
            InfoForm aInfoForm = (InfoForm)aForm;
            ArrayList aList = aInfoForm.GetIntfObjects(typeof(IFindContainer));
            foreach (Object C in aList)
            {
                if (C.GetType().Equals(typeof(DefaultValidate)))
                {
                    DefaultValidate DV = (DefaultValidate)C;
                    foreach (FieldItem FI in DV.FieldItems)
                    {
                        if (DV.BindingSource != null)
                        {
                            string TableName = GetTableNameByBindingSource((InfoBindingSource)DV.BindingSource, SolutionName);
                            Object[] aItem = new Object[] { TableName, FI.FieldName, FI.CheckNull.ToString(),
                            FI.Validate, FI.CheckRangeFrom,FI.CheckRangeTo,FI.CarryOn.ToString(), FI.DefaultValue};
                            List.Add(aItem);
                        }
                    }
                }
            }
        }

        private void CheckRemotingObject(string UserID, string Password, string DBName, string SiteCode, string CurrentProject)
        {
            try
            {
                CliUtils.fLoginUser = UserID;
                CliUtils.fLoginPassword = Password;
                CliUtils.fLoginDB = DBName;
                //CliUtils.fSiteCode = SiteCode;
                CliUtils.fComputerName = Dns.GetHostName();
                CliUtils.fComputerIp = Dns.GetHostEntry(CliUtils.fComputerName).AddressList[0].ToString();
                CliUtils.fCurrentProject = CurrentProject;
                try
                {
                    string message = "";
                    bool rtn = CliUtils.Register(ref message);
                    if (rtn)
                    {
                        CliUtils.GetSysXml(EEPRegistry.Server + "\\SysMsg.xml");
                    }
                    else
                    {
                        MessageBox.Show(message);
                    }

                    string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ":0";
                    object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            catch { }
        }

        private string CaptureDataModule(String SolutionFileName, String ModuleName, ArrayList InfoCommandList, ArrayList UpdateComponentList, ArrayList CodeList)
        {
            if (dte2.Solution == null || dte2.Solution.FileName.CompareTo(SolutionFileName) != 0)
                dte2.Solution.Open(SolutionFileName);
            ModuleName = GetToken(ref ModuleName, new char[] { '.' });
            String aFileName = "";
            
            foreach (Project P in dte2.Solution.Projects)
            {
                Project aProject = P;
                System.Type ty = null;
                ProjectItem PI = FindProjectItem(ref aProject, ModuleName, ref ty);
                if (PI != null)
                {
                    if (documentSetting[10])
                    {
                        #region Get server code
                        Window CW = null;

                        CW = PI.Open(Constants.vsViewKindCode);
                        System.Threading.Thread.Sleep(1000);
                        //CW.Activate();
                        //Application.DoEvents();
                        CW.SetFocus();
                        TextSelection ts = (TextSelection)CW.Selection;
                        ts.SelectAll();
                        string sts = ts.Text;
                        CodeList.Add(new object[] { ModuleName + "." + PI.Name, sts });
                        if (CW != null)
                            CW.Close(vsSaveChanges.vsSaveChangesNo);
                        #endregion
                    }
                    
                    Window W = null;
                    //if (InfoCommandList != null)
                    //{
                        //W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                        W = PI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
                        Application.DoEvents();
                        //W.Visible = true;
                        //W.Activate();
                        if (documentSetting[7] || documentSetting[8])
                        {
                            object obyu = System.Activator.CreateInstance(ty);

                            DataModule fsf = (DataModule)obyu;
                            FieldInfo[] infos = fsf.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                            for (int i = 0; i < infos.Length; i++)
                            {
                                if (infos[i].GetValue(fsf).GetType().FullName == "Srvtools.UpdateComponent")
                                {
                                    UpdateComponent aUpdate = infos[i].GetValue(fsf) as UpdateComponent;
                                    if (aUpdate.FieldAttrs.Count > 0)
                                    {
                                        int Index2 = 0;
                                        Object[] FieldAttrs = new Object[aUpdate.FieldAttrs.Count];
                                        foreach (FieldAttr aAttr in aUpdate.FieldAttrs)
                                        {
                                            FieldAttrs[Index2] = new Object[] { aAttr.DataField, aAttr.DefaultMode.ToString(), aAttr.DefaultValue, aAttr.CheckNull.ToString() };
                                            Index2++;
                                        }
                                        UpdateComponentList.Add(new Object[] { ModuleName + "." + infos[i].Name, FieldAttrs });
                                    }
                                }
                                else if (infos[i].GetValue(fsf).GetType().FullName == "Srvtools.InfoCommand")
                                {
                                    InfoCommand aCommand = infos[i].GetValue(fsf) as InfoCommand;
                                    InfoCommandList.Add(new Object[] { ModuleName + "." + aCommand.Name, aCommand.CommandText });
                                }
                                else if (infos[i].GetValue(fsf).GetType().FullName == "Srvtools.InfoTransaction")
                                { }
                            }
                        }
                    //}
                        if (documentSetting[1])
                        {
                            #region Get server form image
                            W.Activate();
                            Application.DoEvents();
                            IntPtr aHandle = new IntPtr(dte2.MainWindow.HWnd);
                            Srvtools.ScreenCapture B = new ScreenCapture();
                            StringBuilder SB = new StringBuilder(200);
                            string FileName = Path.GetTempPath() + Path.GetRandomFileName() + ".jpg";
                            B.CaptureWindowToFile(aHandle, FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            aFileName = FileName;
                            if (W != null)
                                W.Close(vsSaveChanges.vsSaveChangesNo);
                            #endregion
                        }
                    break;
                }
            }

            //Object[] Result = new Object[FileList.Count];
            //int Index = 0;
            //foreach (String S in FileList)
            //    Result[Index++] = S;

            return aFileName;
        }

        private ProjectItem FindProjectItem(ref Project aProject, string ModuleName, ref System.Type ty)
        {
            if (aProject.Name.CompareTo(ModuleName) == 0)
            {
                object[] dllpathobj = (object[])(aProject.ConfigurationManager.ActiveConfiguration.OutputGroups.Item("Built").FileURLs);
                string dllpath = dllpathobj[0].ToString();
                if (dllpath.StartsWith("file:///"))
                { dllpath = dllpath.Substring(8); }

                foreach (ProjectItem PI in aProject.ProjectItems)
                {
                    Assembly ass = Assembly.LoadFile(dllpath);

                    Type[] tys = ass.GetTypes();
                    foreach (Type t in tys)
                    {
                        if (t.BaseType.Name == "DataModule" && PI.Name.Substring(0, PI.Name.LastIndexOf(".")) == t.Name)
                        {
                            ty = t;
                            return PI;
                        }
                    }
                }

                return null;
            }
            else if (aProject.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
            {
                foreach (ProjectItem PI in aProject.ProjectItems)
                {
                    if (PI.SubProject != null)
                    {
                        Project SubProject = PI.SubProject;
                        ProjectItem aItem = FindProjectItem(ref SubProject, ModuleName, ref ty);
                        if (aItem != null)
                        {
                            aProject = SubProject;
                            return aItem;
                        }
                    }
                }
                return null;
            }
            else
                return null;
        }

        public Object GetJQPageInfo(Object Params)
        {
            try
            {
                Object[] RealParams = (object[])Params;
                String SolutionFileName = RealParams[0].ToString();
                String WebSiteName = RealParams[1].ToString();
                String WebSitePath = RealParams[2].ToString();
                Object[] FolderOffset = (Object[])RealParams[3];
                String PageName = RealParams[4].ToString();
                String UserID = RealParams[5].ToString();
                String Password = RealParams[6].ToString();
                String DBName = RealParams[7].ToString();
                String Solutionname = RealParams[8].ToString();
                String PrintWaitingTime = RealParams[9].ToString();
                //不检查
                CheckRemotingObject(UserID, Password, DBName, "", Solutionname);
                if (dte2.Solution == null || dte2.Solution.FileName.CompareTo(SolutionFileName) != 0)
                    dte2.Solution.Open(SolutionFileName);

                //Get Page Controls  
                Object[] Tables = null;
                Object DefaultValidateList = null;
                ArrayList RemoteNameList = new ArrayList();
                ArrayList InfoCommandList = new ArrayList();
                ArrayList UpdateComponentList = new ArrayList();
                List<string> DataModuleFileList = new List<string>();
                ArrayList CodeList = new ArrayList();
                List<JQDataGridPrint> gridList = new List<JQDataGridPrint>();
                List<JQDataFormPrint> formList = new List<JQDataFormPrint>();
                List<JQDefaultPrint> defaultList = new List<JQDefaultPrint>();
                List<JQValidatePrint> validateList = new List<JQValidatePrint>();

                foreach (Project P in dte2.Solution.Projects)
                {
                    if ((string.Compare(P.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0) && (P.Name.CompareTo(WebSitePath) == 0 || P.FullName.CompareTo(WebSitePath) == 0))
                    {
                        object[] webPageString = GetJQPage(P.ProjectItems, PageName, FolderOffset, CodeList);
                        PageTitle = ((object[])webPageString)[1].ToString();
                        ArrayList al = (ArrayList)(((object[])webPageString)[0]);
                        if (documentSetting[1] || documentSetting[7] || documentSetting[8] || documentSetting[10])
                        {
                            for (int i = 0; i < al.Count; i++)
                            {
                                //Get Server Form Infomation
                                DataModuleFileList.Add(CaptureDataModule(SolutionFileName, (string)al[i], InfoCommandList, UpdateComponentList, CodeList));
                            }
                        }
                        gridList = (List<JQDataGridPrint>)((object[])webPageString)[3];
                        formList = (List<JQDataFormPrint>)((object[])webPageString)[4];
                        defaultList = (List<JQDefaultPrint>)((object[])webPageString)[5];
                        validateList = (List<JQValidatePrint>)((object[])webPageString)[6];
                        //DefaultValidateList = CombineValidateAndDefault((ArrayList)webPageString[5], (ArrayList)webPageString[6]);
                        break;
                    }
                }

                Object[] aModuleList = new Object[DataModuleFileList.Count];
                for (int I = 0; I < aModuleList.Length; I++)
                    aModuleList[I] = DataModuleFileList[I];
                Object[] aCommandList = new Object[InfoCommandList.Count];
                for (int I = 0; I < aCommandList.Length; I++)
                    aCommandList[I] = InfoCommandList[I];
                Object[] aUpdateList = new Object[UpdateComponentList.Count];
                for (int I = 0; I < aUpdateList.Length; I++)
                    aUpdateList[I] = UpdateComponentList[I];
                Object[] aCodeList = new Object[CodeList.Count];
                for (int I = 0; I < aCodeList.Length; I++)
                    aCodeList[I] = CodeList[I];

                String FileName = string.Empty;
                if (documentSetting[0])
                {
                    //Capture Page Image
                    FileName = GetJQPageImage(WebSiteName, PageName, FolderOffset, UserID, Password, DBName,
                        Solutionname, PrintWaitingTime, PageTitle, WebSitePath);
                }
                return new object[] { 1, new Object[] { FileName, Tables, gridList, formList, aModuleList, null, aCommandList, aUpdateList, aCodeList, defaultList, validateList } };
            }
            catch (Exception E)
            {
                return new Object[] { -1, E.Message + ((E.InnerException != null) ? ("\r\n" + E.InnerException.Message) : "") };
            }
        }


        public Object GetPageInfo(Object Params)
        {
            try
            {
                Object[] RealParams = (object[])Params;
                String SolutionFileName = RealParams[0].ToString();
                String WebSiteName = RealParams[1].ToString();
                String WebSitePath = RealParams[2].ToString();
                Object[] FolderOffset = (Object[])RealParams[3];
                String PageName = RealParams[4].ToString();
                String UserID = RealParams[5].ToString();
                String Password = RealParams[6].ToString();
                String DBName = RealParams[7].ToString();
                String Solutionname = RealParams[8].ToString();
                String PrintWaitingTime = RealParams[9].ToString();
                //不检查
                CheckRemotingObject(UserID, Password, DBName, "", Solutionname);
                if (dte2.Solution == null || dte2.Solution.FileName.CompareTo(SolutionFileName) != 0)
                    dte2.Solution.Open(SolutionFileName);

                //Get Page Controls  
                Object TextBoxList = null;
                Object[] Tables = null;
                Object DefaultValidateList = null;
                ArrayList RemoteNameList = new ArrayList();
                ArrayList InfoCommandList = new ArrayList();
                ArrayList UpdateComponentList = new ArrayList();
                List<string> DataModuleFileList = new List<string>();
                ArrayList CodeList = new ArrayList();
                Object[] GridColumnList = null;
                foreach (Project P in dte2.Solution.Projects)
                {
                    if ((string.Compare(P.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0) && (P.Name.CompareTo(WebSitePath) == 0 || P.FullName.CompareTo(WebSitePath) == 0))
                    {
                        object[] webPageString = GetWebPage(P.ProjectItems, PageName, FolderOffset, CodeList);
                        PageTitle = ((object[])webPageString)[1].ToString();
                        ArrayList al = (ArrayList)(((object[])webPageString)[0]);
                        if (documentSetting[1] || documentSetting[7] || documentSetting[8] || documentSetting[10])
                        {
                            for (int i = 0; i < al.Count; i++)
                            {
                                //Get Server Form Infomation
                                DataModuleFileList.Add(CaptureDataModule(SolutionFileName, (string)al[i], InfoCommandList, UpdateComponentList, CodeList));
                            }
                        }
                        ArrayList GridColumn = (ArrayList)((object[])webPageString)[3];
                        GridColumnList = new Object[GridColumn.Count];
                        for (int I = 0; I < GridColumn.Count; I++)
                            GridColumnList[I] = GridColumn[I];
                        DefaultValidateList = CombineValidateAndDefault((ArrayList)webPageString[4], (ArrayList)webPageString[5]);
                        break;
                    }
                }

                Object[] aModuleList = new Object[DataModuleFileList.Count];
                for (int I = 0; I < aModuleList.Length; I++)
                    aModuleList[I] = DataModuleFileList[I];
                Object[] aCommandList = new Object[InfoCommandList.Count];
                for (int I = 0; I < aCommandList.Length; I++)
                    aCommandList[I] = InfoCommandList[I];
                Object[] aUpdateList = new Object[UpdateComponentList.Count];
                for (int I = 0; I < aUpdateList.Length; I++)
                    aUpdateList[I] = UpdateComponentList[I];
                Object[] aCodeList = new Object[CodeList.Count];
                for (int I = 0; I < aCodeList.Length; I++)
                    aCodeList[I] = CodeList[I];

                String FileName = string.Empty;
                if (documentSetting[0])
                {
                    //Capture Page Image
                    FileName = GetPageImage2(WebSiteName, PageName, FolderOffset, UserID, Password, DBName,
                        Solutionname, PrintWaitingTime, PageTitle, WebSitePath);
                }
                return new object[] { 1, new Object[] { FileName, Tables, TextBoxList, GridColumnList, aModuleList, DefaultValidateList, aCommandList, aUpdateList, aCodeList } };
            }
            catch (Exception E)
            {
                return new Object[] { -1, E.Message + ((E.InnerException != null) ? ("\r\n" + E.InnerException.Message) : "") };
            }
        }

        private Object CombineValidateAndDefault(ArrayList arrayList, ArrayList arrayList_2)
        {
            ArrayList DefaultValidateList = new ArrayList();
            DoCombineValidateAndDefault(arrayList, arrayList_2, DefaultValidateList);
            Object[] Result = new Object[DefaultValidateList.Count];
            for (int I = 0; I < DefaultValidateList.Count; I++)
                Result[I] = DefaultValidateList[I];
            return Result;
        }

        private void DoCombineValidateAndDefault(ArrayList DefaultList, ArrayList ValidateList, ArrayList DefaultValidateList)
        {
            for (int i = 0; i < DefaultList.Count; i++)
            {
                object[] defaultobj = (object[])DefaultList[i];
                string datasourcedefaultID = (string)defaultobj[0];
                ArrayList arldefault = (ArrayList)defaultobj[1];
                for (int j = 0; j < ValidateList.Count; j++)
                {
                    object[] Validateobj = (object[])ValidateList[j];
                    string datasourcevalidateID = (string)Validateobj[0];
                    ArrayList arlvalidate = (ArrayList)Validateobj[1];
                    ArrayList defaultremovelist = new ArrayList();
                    if (datasourcevalidateID == datasourcedefaultID)
                    {
                        foreach (object[] defatult in arldefault)
                        {
                            string s = (string)defatult[0];
                            foreach (object[] validate in arlvalidate)
                            {
                                if ((string)validate[0] == s)
                                {
                                    DefaultValidateList.Add(new object[] { datasourcedefaultID, s, validate[1], validate[2], validate[3], validate[4], defatult[2], defatult[1] });
                                    arlvalidate.Remove(validate);
                                    defaultremovelist.Add(defatult);
                                    break;
                                }
                            }
                        }
                    }
                    foreach (object[] de in defaultremovelist)
                    {
                        arldefault.Remove(de);
                    }
                }
                foreach (object[] def in arldefault)
                {
                    DefaultValidateList.Add(new object[] { datasourcedefaultID, def[0], "False", "", "", "", def[2], def[1] });
                }
            }
            for (int i = 0; i < ValidateList.Count; i++)
            {
                object[] Validateobj = (object[])ValidateList[i];
                string datasourcevalidateID = (string)Validateobj[0];
                ArrayList arlvalidate = (ArrayList)Validateobj[1];
                foreach (object[] vali in arlvalidate)
                {
                    DefaultValidateList.Add(new object[] { datasourcevalidateID, vali[0], vali[1], vali[2], vali[3], vali[4], "False", "" });
                }
            }
            //WRONG!
            //myReverserClass Comparer = new myReverserClass();
            //DefaultValidateList.Sort(Comparer);
        }

        public object[] GetWebPage(ProjectItems PIs, String PageName, Object[] FolderOffset, ArrayList CodeList)
        {
            if (PIs == null)
                return null;
            if (FolderOffset.Length > 0)
            {
                String TargetFolder = (String)FolderOffset[0];
                foreach (ProjectItem PI in PIs)
                {
                    if (PI.Name.CompareTo(TargetFolder) == 0)
                    {
                        if (FolderOffset.Length == 1)
                        {
                            Object[] NewFolderOffset = new Object[0];
                            return GetWebPage(PI.ProjectItems, PageName, NewFolderOffset, CodeList);
                        }
                        else
                        {
                            Object[] NewFolderOffset = new Object[FolderOffset.Length - 1];
                            for (int I = 0; I < FolderOffset.Length - 1; I++)
                                NewFolderOffset[I] = FolderOffset[I + 1];
                            return GetWebPage(PI.ProjectItems, PageName, NewFolderOffset, CodeList);
                        }
                    }
                }
            }
            else
            {
                foreach (ProjectItem PI in PIs)
                {
                    if (PI.Name.CompareTo(PageName) == 0)
                    {
                        string title = "";
                        ArrayList webdatasourceList = new ArrayList();
                        ArrayList webviewLIst = new ArrayList();
                        ArrayList WebValidateList = new ArrayList();
                        ArrayList WebDefaultList = new ArrayList();
                        ArrayList ModuleList = new ArrayList();
                        Window FCodeDesignWindow = PI.Open(Constants.vsViewKindCode);
                        //FCodeDesignWindow.Activate();
                        TextSelection modulets = (TextSelection)FCodeDesignWindow.Document.Selection;
                        modulets.SelectAll();
                        string smodulets = modulets.Text;
                        Hashtable hs = new Hashtable();
                        if (documentSetting[9])
                            CodeList.Add(new object[] { FCodeDesignWindow.Caption, smodulets });
                        string pattenformodule = "this[.].*?[.]RemoteName = \".*?\"";
                        MatchCollection Matchs = Regex.Matches(smodulets, pattenformodule, RegexOptions.IgnoreCase);
                        foreach (Match nMatch in Matchs)
                        {
                            string result = nMatch.ToString();
                            string datasetname = result.Substring(5, result.IndexOf(".RemoteName") - 5);
                            string result2 = result.Substring(result.IndexOf(".RemoteName") + 1);
                            string s = result2.Substring(14, result2.Length - 15);
                            string ss = ((string[])(s.Split(new char[] { '.' })))[0];
                            hs.Add(datasetname, ss);
                            if (!ModuleList.Contains(ss))
                                ModuleList.Add(ss);
                        }
                        FCodeDesignWindow.Close(vsSaveChanges.vsSaveChangesNo);
                        Window FDesignWindow = PI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
                        FDesignWindow.Activate();
                        HTMLWindow W = (HTMLWindow)FDesignWindow.Object;
                        //object o = W.CurrentTabObject;

                        // webform designer though, is a special case 
                        //if (W != null)
                        //{
                        //    // make sure current tab is the designer tab 
                        //    W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
                        //    Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSP = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)o;
                        //    //Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage.DispDesignerDocument d = htmlWindow.CurrentTabObject as Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage.DispDesignerDocument;
                        //    Guid guidVsMDDDesigner = new Guid("7494682A-37A0-11d2-A273-00C04F8EF4FF");
                        //    Guid guidVsMDDDesigner2 = new Guid("7494682A-37A0-11d2-A273-00C04F8EF4FF");
                            
                        //    IntPtr ptr;
                        //    int hr =oleSP.QueryService(ref guidVsMDDDesigner, ref guidVsMDDDesigner2, out ptr);
                        //    System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);

                        //    if (ptr != IntPtr.Zero)
                        //    {
                        //        IServiceProvider dotnetSP = (IServiceProvider)Marshal.GetObjectForIUnknown(ptr);
                        //        Marshal.Release(ptr);
                        //        //designerHost = (IDesignerHost)dotnetSP.GetService(typeof(IDesignerHost));
                        //    }
                        //}


                        W.CurrentTab = vsHTMLTabs.vsHTMLTabsSource;
                        TextSelection ts = (TextSelection)FDesignWindow.Document.Selection;
                        ts.SelectAll();
                        string sts = ts.Text;
                        StringReader sr = new StringReader(sts);
                        string firstline = sr.ReadLine();
                        while (firstline != "</html>" && firstline != null)
                        {
                            if (firstline.Contains("<title>"))
                            {
                                string spatten2 = @"<title>.*</title>";
                                MatchCollection lineMatchs2 = Regex.Matches(firstline, spatten2);
                                foreach (Match nMatch in lineMatchs2)
                                {
                                    string result = nMatch.ToString();
                                    title = result.Substring(7, result.Length - 15);
                                }
                                break;
                            }
                            firstline = sr.ReadLine();
                        }
                        sr.Close();
                        W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
                        Application.DoEvents();
                        IHTMLDocument2 IHTMLDocument = (IHTMLDocument2)W.CurrentTabObject;
                        foreach (IHTMLElement IHTMLElement in (IHTMLElementCollection)IHTMLDocument.activeElement.children)
                        {
                            if (IHTMLElement.tagName.ToLower() == "webdatasource" || IHTMLElement.tagName.ToLower() == "infolight:webdatasource")
                            {
                                Object webdatasource = Getwebdatasource(IHTMLElement);
                                webdatasourceList.Add(webdatasource);
                            }
                            else if (IHTMLElement.tagName.ToLower() == "webgridview" || IHTMLElement.tagName.ToLower() == "infolight:webgridview")
                            {
                                Object webgridview = GetWebGridView(IHTMLElement);
                                webviewLIst.Add(webgridview);
                            }
                            else if (IHTMLElement.tagName.ToLower() == "webformview" || IHTMLElement.tagName.ToLower() == "infolight:webformview")
                            {
                                Object webformview = GetWebFormView(IHTMLElement);
                                webviewLIst.Add(webformview);
                            }
                            else if (IHTMLElement.tagName.ToLower() == "webdetailsview" || IHTMLElement.tagName.ToLower() == "infolight:webdetailsview")
                            {
                                Object webdetailview = GetWebDetailView(IHTMLElement);
                                webviewLIst.Add(webdetailview);
                            }
                            else if (IHTMLElement.tagName.ToLower() == "webvalidate" || IHTMLElement.tagName.ToLower() == "infolight:webvalidate")
                            {
                                Object WebValidate = GetWebValidate(IHTMLElement);
                                WebValidateList.Add(WebValidate);
                            }
                            else if (IHTMLElement.tagName.ToLower() == "webdefault" || IHTMLElement.tagName.ToLower() == "infolight:webdefault")
                            {
                                Object WebDefault = GetWebDefault(IHTMLElement);
                                WebDefaultList.Add(WebDefault);
                            }
                            else if (IHTMLElement.tagName.ToLower() == "ajaxgridview" || IHTMLElement.tagName.ToLower() == "ajaxTools:AjaxGridView")
                            {
                                Object AjaxGridView = GetAjaxGridView(IHTMLElement);
                                webviewLIst.Add(AjaxGridView);
                            }
                            else
                            {
                                getHTMLElement(IHTMLElement, webdatasourceList, webviewLIst, WebValidateList, WebDefaultList);
                            }
                        }
                        Application.DoEvents();
                        if (FDesignWindow != null)
                            FDesignWindow.Close(vsSaveChanges.vsSaveChangesNo);
                        replaceTableName(webdatasourceList, webviewLIst, hs);
                        ArrayList newwebviewlist = replaceWebViewList(webviewLIst);
                        return new object[] { ModuleList, title, webdatasourceList, newwebviewlist, WebDefaultList, WebValidateList, hs };
                    }
                }
            }
            return null;
        }

        private ArrayList replaceWebViewList(ArrayList webviewLIst)
        {
            ArrayList newwebviewlist = new ArrayList();
            foreach (object[] viewitem in webviewLIst)
            {
                newwebviewlist.Add(new object[] { (string)viewitem[0] + "." + (string)viewitem[1], viewitem[2] });
            }
            return newwebviewlist;
        }

        private void replaceTableName(ArrayList webdatasourceList, List<JQDataFormPrint> webviewLIst, Hashtable hs)
        {
            foreach (JQDataFormPrint viewitem in webviewLIst)
            {
                foreach (object[] datasourceitem in webdatasourceList)
                {
                    if ((datasourceitem[0] as string).Equals(viewitem.DataMember))
                    {
                        if (hs.ContainsKey(((string)datasourceitem[1])))
                        {
                            string ModuleName = hs[(string)datasourceitem[1]] as string;
                            string RealTableName = CliUtils.GetTableName(ModuleName, (string)datasourceitem[2], slnName.Substring(0, slnName.Length - 4));
                            viewitem.DataMember = RealTableName;
                            break;
                        }
                    }
                }
            }
        }
        private void replaceTableName(ArrayList webdatasourceList, ArrayList webviewLIst, Hashtable hs)
        {
            foreach (object[] viewitem in webviewLIst)
            {
                foreach (object[] datasourceitem in webdatasourceList)
                {
                    if ((datasourceitem[0] as string).Equals((string)viewitem[1]))
                    {
                        if (hs.ContainsKey(((string)datasourceitem[1])))
                        {
                            string ModuleName = hs[(string)datasourceitem[1]] as string;
                            string RealTableName = CliUtils.GetTableName(ModuleName, (string)datasourceitem[2], slnName.Substring(0, slnName.Length - 4));
                            viewitem[1] = RealTableName;
                            break;
                        }
                    }
                }
            }
        }

        private void getHTMLElement(IHTMLElement IHTMLElement, ArrayList webdatasourceList, ArrayList webviewLIst, ArrayList WebValidateList, ArrayList WebDefaultList)
        {
            foreach (IHTMLElement IHTMLElementchi in (IHTMLElementCollection)IHTMLElement.children)
            {
                if (IHTMLElementchi.tagName.ToLower() == "webdatasource" || IHTMLElementchi.tagName.ToLower() == "infolight:webdatasource")
                {
                    Object webdatasource = Getwebdatasource(IHTMLElementchi);
                    webdatasourceList.Add(webdatasource);
                }
                else if (IHTMLElementchi.tagName.ToLower() == "webgridview" || IHTMLElementchi.tagName.ToLower() == "infolight:webgridview")
                {
                    Object webgridview = GetWebGridView(IHTMLElementchi);
                    webviewLIst.Add(webgridview);
                }
                else if (IHTMLElementchi.tagName.ToLower() == "webformview" || IHTMLElementchi.tagName.ToLower() == "infolight:webformview")
                {
                    Object webformview = GetWebFormView(IHTMLElementchi);
                    webviewLIst.Add(webformview);
                }
                else if (IHTMLElementchi.tagName.ToLower() == "webdetailsview" || IHTMLElementchi.tagName.ToLower() == "infolight:webdetailsview")
                {
                    Object webdetailview = GetWebDetailView(IHTMLElementchi);
                    webviewLIst.Add(webdetailview);
                }
                else if (IHTMLElementchi.tagName.ToLower() == "webvalidate" || IHTMLElementchi.tagName.ToLower() == "infolight:webvalidate")
                {
                    Object WebValidate = GetWebValidate(IHTMLElementchi);
                    WebValidateList.Add(WebValidate);
                }
                else if (IHTMLElementchi.tagName.ToLower() == "webdefault" || IHTMLElementchi.tagName.ToLower() == "infolight:webdefault")
                {
                    Object WebDefault = GetWebDefault(IHTMLElementchi);
                    WebDefaultList.Add(WebDefault);
                }
                else if (IHTMLElementchi.tagName.ToLower() == "ajaxgridview" || IHTMLElementchi.tagName.ToLower() == "ajaxTools:AjaxGridView")
                {
                    Object AjaxGridView = GetAjaxGridView(IHTMLElementchi);
                    webviewLIst.Add(AjaxGridView);
                }
                else
                {
                    getHTMLElement(IHTMLElementchi, webdatasourceList, webviewLIst, WebValidateList, WebDefaultList);
                }
            }
        }

        private String GetToken(ref String AString, char[] Fmt)
        {
            String Result = "";
            while (AString.Length != 0 && AString[0] == ' ')
            {
                AString = AString.Remove(1, 1);
            }

            if (AString.Length == 0)
                return Result;
            if (AString.Contains("@"))
            {
                AString = AString.Substring(AString.IndexOf("@") + 1);
            }
            Boolean Found = false;
            int I = 0;
            while (I < AString.Length)
            {
                Found = false;
                if ((byte)AString[I] <= 128)
                {
                    foreach (char C in Fmt)
                    {
                        if (AString[I] == C)
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (!Found)
                        I++;
                }
                else
                {
                    I = I + 2;
                }
                if (Found)
                    break;
            }

            if (Found)
            {
                Result = AString.Substring(0, I);
                AString = AString.Remove(0, I + 1);
            }
            else
            {
                Result = AString;
                AString = "";
            }

            return Result;
        }

        private Object GetWebValidate(IHTMLElement IHTMLElement)
        {
            ArrayList WebValidateList = new ArrayList();
            string datasource = IHTMLElement.getAttribute("DataSourceID", 0).ToString();
            string datasourcepatten = "DataSourceID=\".*?\"";
            string patten = "<InfoLight:ValidateFieldItem.*?(\r\n)??.*?</InfoLight:ValidateFieldItem>";
            string s = IHTMLElement.outerHTML;
            MatchCollection datasourceMatchs = Regex.Matches(s, datasourcepatten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in datasourceMatchs)
            {
                string result = nMatch.ToString();
                datasource = result.Substring(14, result.Length - 15);
            }
            MatchCollection Matchs = Regex.Matches(s, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                string FieldName = "";
                string CheckNull = "False";
                string Validate = "";
                string CheckRangeFrom = "";
                string CheckRangeTo = "";
                string result = nMatch.ToString();
                string fnpt = "FieldName=\".*?\"";
                string cnpt = "CheckNull=\".*?\"";
                string vpt = "Validate=\".*?\"";
                string crfpt = "CheckRangeFrom=\".*?\"";
                string crtpt = "CheckRangeTo=\".*?\"";
                MatchCollection Matchs1 = Regex.Matches(result, fnpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch1 in Matchs1)
                {
                    FieldName = nMatch1.ToString().Substring(11, nMatch1.ToString().Length - 12);
                }
                MatchCollection Matchs2 = Regex.Matches(result, cnpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch2 in Matchs2)
                {
                    CheckNull = nMatch2.ToString().Substring(11, nMatch2.ToString().Length - 12);
                }
                MatchCollection Matchs3 = Regex.Matches(result, vpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch3 in Matchs3)
                {
                    Validate = nMatch3.ToString().Substring(10, nMatch3.ToString().Length - 11);
                }
                MatchCollection Matchs4 = Regex.Matches(result, crfpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch4 in Matchs4)
                {
                    CheckRangeFrom = nMatch4.ToString().Substring(16, nMatch4.ToString().Length - 17);
                }
                MatchCollection Matchs5 = Regex.Matches(result, crtpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch5 in Matchs5)
                {
                    CheckRangeTo = nMatch5.ToString().Substring(14, nMatch5.ToString().Length - 15);
                }
                WebValidateList.Add(new object[] { FieldName, CheckNull, Validate, CheckRangeFrom, CheckRangeTo });
            }
            return new object[] { datasource, WebValidateList };
        }

        private Object GetWebDefault(IHTMLElement IHTMLElement)
        {
            ArrayList WebDefaultList = new ArrayList();
            string datasource = IHTMLElement.getAttribute("DataSourceID", 0).ToString();
            string patten = "<InfoLight:DefaultFieldItem.*?(\r\n)??.*?</InfoLight:DefaultFieldItem>";
            string s = IHTMLElement.outerHTML;
            MatchCollection Matchs = Regex.Matches(s, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                string DefaultValue = "";
                string FieldName = "";
                string CarryOn = "";
                string result = nMatch.ToString();
                string dvpt = "DefaultValue =\".*?\"";
                string fnpt = "FieldName =\".*?\"";
                string copt = "CarryOn =\".*?\"";
                MatchCollection Matchs1 = Regex.Matches(result, fnpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch1 in Matchs1)
                {
                    FieldName = nMatch1.ToString().Substring(11, nMatch1.ToString().Length - 12);
                }
                MatchCollection Matchs2 = Regex.Matches(result, dvpt, RegexOptions.IgnoreCase);
                foreach (Match nMatch2 in Matchs2)
                {
                    DefaultValue = nMatch2.ToString().Substring(14, nMatch2.ToString().Length - 15);
                }
                MatchCollection Matchs3 = Regex.Matches(result, copt, RegexOptions.IgnoreCase);
                foreach (Match nMatch3 in Matchs3)
                {
                    CarryOn = nMatch3.ToString().Substring(9, nMatch3.ToString().Length - 10);
                }

                WebDefaultList.Add(new object[] { FieldName, DefaultValue, CarryOn });
            }
            return new object[] { datasource, WebDefaultList };
        }

        private Object GetWebFormView(IHTMLElement IHTMLElement)
        {
            ArrayList artest = new ArrayList();
            ArrayList webformviewList = new ArrayList();
            string WebFormViewID = IHTMLElement.id;
            string datasource = IHTMLElement.getAttribute("DataSourceID", 0).ToString();
            string datasourcepatten = "DataSourceID=\".*?\"";
            string patten1 = "WebFormView id=\".*?\"";
            string patten2 = @"\bID="".*?""(.|\n)*? runat="".*?""(.|\n)*? Text=(""|').*?(""|')";
            string s = IHTMLElement.innerHTML;
            //string s2 = IHTMLElement.outerHTML.Remove(IHTMLElement.outerHTML.IndexOf(IHTMLElement.innerHTML), IHTMLElement.innerHTML.Length);
            //MatchCollection datasourceMatchs = Regex.Matches(s2, datasourcepatten, RegexOptions.IgnoreCase);
            //foreach (Match nMatch in datasourceMatchs)
            //{
            //    string result = nMatch.ToString();
            //    datasource = result.Substring(14, result.Length - 15);
            //}

            string subs = s.Substring(s.IndexOf("<ItemTemplate"), s.IndexOf("</ItemTemplate>") - s.IndexOf("<ItemTemplate"));
            MatchCollection subMatchs = Regex.Matches(subs, patten2, RegexOptions.IgnoreCase);
            foreach (Match snMatch in subMatchs)
            {
                string ID = "";
                string Text = "";
                string result = snMatch.ToString();
                if (result.Contains("<%# Bind("))
                    continue;
                string res1 = result.Substring(4);
                ID = res1.Substring(0, res1.IndexOf("\""));

                result = result.Substring(result.IndexOf(" Text=\"") + 1);
                Text = result.Substring(6, result.Length - 7);
                webformviewList.Add(ID + "." + Text);
            }
            Object[] webformview = new Object[webformviewList.Count];
            for (int I = 0; I < webformviewList.Count; I++)
                webformview[I] = webformviewList[I];

            return new object[] { WebFormViewID, datasource, webformview };
        }

        private object GetAjaxGridView(IHTMLElement IHTMLElement)
        {
            string ID = IHTMLElement.id;
            
            string datasource = IHTMLElement.getAttribute("DataSourceID", 0).ToString();
            ArrayList ajaxGridViewList = new ArrayList();

            string s = IHTMLElement.innerHTML;
            string s1 = s.Substring(s.IndexOf("<Columns"), s.LastIndexOf("</Columns>") - s.IndexOf("<Columns"));
            string patten = "DataField=\".*?\"(.|\r\n)*?HeaderText=\".*?\"";
            MatchCollection Matchs = Regex.Matches(s1, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                string DataField = "";
                string HeaderText = "";
                string result = nMatch.ToString();
                result = result.Substring(11);
                DataField = result.Substring(0, result.IndexOf("\"")).TrimEnd(new char[] { ' ', '\r', '\n' });
                result = result.Substring(result.IndexOf(" HeaderText=", StringComparison.OrdinalIgnoreCase) + 1);
                HeaderText = result.Substring(12, result.Length - 13);
                if (DataField != "")
                    ajaxGridViewList.Add(DataField + "." + HeaderText);
            }
            Object[] ajaxGridView = new Object[ajaxGridViewList.Count];
            for (int I = 0; I < ajaxGridViewList.Count; I++)
                ajaxGridView[I] = ajaxGridViewList[I];

            return new object[] { ID, datasource, ajaxGridView };
        }

        private Object GetWebGridView(IHTMLElement IHTMLElement)
        {
            return GetWebDetailView(IHTMLElement);
        }

        private Object GetWebDetailView(IHTMLElement IHTMLElement)
        {
            ArrayList WebDetailsViewList = new ArrayList();

            string datasourcepatten = "DataSourceID=\".*?\"";
            string ID = IHTMLElement.id;
            string datasource = IHTMLElement.getAttribute("DataSourceID", 0).ToString();
            string s = IHTMLElement.innerHTML;
            //string s2 = IHTMLElement.outerHTML.Remove(IHTMLElement.outerHTML.IndexOf(IHTMLElement.innerHTML), IHTMLElement.innerHTML.Length);
            string patten = "HeaderText=\".*?\"(.|\r\n)*?SortExpression=\".*?\"";
            //MatchCollection datasourceMatchs = Regex.Matches(s2, datasourcepatten, RegexOptions.IgnoreCase);

            //foreach (Match nMatch in datasourceMatchs)
            //{
            //    string result = nMatch.ToString();
            //    datasource = result.Substring(14, result.Length - 15);
            //}
            MatchCollection Matchs = Regex.Matches(s, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                string DataField = "";
                string HeaderText = "";
                string result = nMatch.ToString();
                result = result.Substring(12);
                HeaderText = result.Substring(0, result.IndexOf("\"")).TrimEnd(new char[] { ' ', '\r', '\n' });
                result = result.Substring(result.IndexOf(" SortExpression=", StringComparison.OrdinalIgnoreCase) + 1);
                DataField = result.Substring(16, result.Length - 17);
                if (DataField != "")
                    WebDetailsViewList.Add(DataField + "." + HeaderText);
            }
            Object[] WebDetailsView = new Object[WebDetailsViewList.Count];
            for (int I = 0; I < WebDetailsViewList.Count; I++)
                WebDetailsView[I] = WebDetailsViewList[I];

            return new object[] { ID, datasource, WebDetailsView };
        }

        private Object Getwebdatasource(IHTMLElement IHTMLElement)
        {
            string webDataSourceID = IHTMLElement.getAttribute("id", 0).ToString();
            string webDataSetID = IHTMLElement.getAttribute("WebDataSetID", 0).ToString();
            string dataMember = IHTMLElement.getAttribute("DataMember", 0).ToString();

            return new object[] { webDataSourceID, webDataSetID, dataMember };
        }

        public String GetPageImage2(String WebSiteName, String PageName, Object[] FolderOffset, String UserID, String Password, String DBName, String SolutionName, String PrintWaitingTime, String PageTitle, String WebSitePath)
        {
            String FileName = "";
            dte2.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate();
            UIHierarchy A = (UIHierarchy)dte2.ActiveWindow.Object;
            RenameNoneLoginTag(PageName, FolderOffset, UserID, Password, DBName, SolutionName, WebSitePath);
            if (PrintWaitingTime == "0")
                PrintWaitingTime = "5";

            foreach (UIHierarchyItem aItem in A.UIHierarchyItems)
            {
                foreach (UIHierarchyItem B in aItem.UIHierarchyItems)
                {
                    if (B.Name.CompareTo(WebSiteName) == 0)
                    {
                        B.UIHierarchyItems.Expanded = true;
                        foreach (UIHierarchyItem C in B.UIHierarchyItems)
                        {
                            if (C.Name.CompareTo("NoneLogin.aspx") == 0)
                            {
                                C.Select(vsUISelectionType.vsUISelectionTypeSelect);
                                try
                                {
                                    dte2.MainWindow.Activate();
                                    dte2.ActiveWindow.Activate();
                                    C.DTE.ExecuteCommand("File.ViewinBrowser", String.Empty);
                                    System.Threading.Thread.Sleep(5000);//wait 5s for ie load
                                    Srvtools.ScreenCapture SC = new ScreenCapture();
                                    StringBuilder SB = new StringBuilder(200);
                                    FileName = Path.GetTempPath() + Path.GetRandomFileName() + ".jpg";
                                    PageTitle = PageTitle.Replace("&nbsp", " ").Trim();
                                    String WindowName = String.Format(PageTitle + " - Microsoft Internet Explorer", WebSiteName, FolderOffset[0], PageName); //String WindowName = String.Format(@"http://localhost:1130/EEPWebClient/{1}/{2} - Microsoft Internet Explorer", WebSiteName, FolderOffset[0], PageName);
                                    IntPtr IEHandle = FindIEHandle(PageTitle);
                                    int WaitCount = 0;
                                    while (IEHandle.ToInt32() == 0)
                                    {
                                        System.Threading.Thread.Sleep(50);
                                        IEHandle = FindIEHandle(PageTitle);
                                        WaitCount++;
                                        if (WaitCount > Int32.Parse(printWaitingTime)*1000/50) //printWaitingTime
                                            break;
                                    }
                                    ShowWindow(IEHandle, 3);
                                    SetActiveWindow(IEHandle);
                                    System.Threading.Thread.Sleep(Int32.Parse(printWaitingTime) * 1000);
                                    SC.CaptureWindowToFile(IEHandle, FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    PostMessage(IEHandle, 16, 0, 0);
                                    break;
                                }
                                catch (Exception E)
                                {
                                    MessageBox.Show(E.Message);
                                }
                            }
                        }
                    }
                }
            }
            return FileName;
        }
        private IntPtr FindIEHandle(string PageTitle)
        {
            foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
            {
                if (pro.MainWindowTitle.StartsWith(PageTitle))
                {
                    return pro.MainWindowHandle;
                }
                else if (pro.ProcessName.ToLower() == "firefox")
                {
                    return pro.MainWindowHandle;
                }
            }
            return (IntPtr)0;
        }
        private IntPtr FindIEHandle2(string pagename)
        {
            foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
            {
                if (pro.MainWindowTitle.Contains(pagename))
                {
                    return pro.MainWindowHandle;
                }
                else if (pro.ProcessName.ToLower() == "firefox")
                {
                    return pro.MainWindowHandle;
                }
            }
            return (IntPtr)0;
        }
        private void RenameNoneLoginTag(String PageName, Object[] FolderOffset, String UserID, String Password, String DBName, String SolutionName, String WebSitePath)
        {
            String FileName = WebSitePath;
            if(FileName .EndsWith("\\")) FileName = FileName.Substring(0,FileName.Length -1);
            String BackupFileName = FileName + @"\NoneLogin.aspx.cs.txt";
            FileName = FileName + @"\NoneLogin.aspx.cs";
            if (!File.Exists(BackupFileName))
                return;
            System.IO.File.Copy(BackupFileName, FileName, true);
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName);
            String Context = SR.ReadToEnd();
            SR.Close();
            Context = Context.Replace("TAG_USERID", UserID);
            Context = Context.Replace("TAG_PASSWORD", Password);
            Context = Context.Replace("TAG_DBNAME", DBName);
            Context = Context.Replace("TAG_SOLUTION", SolutionName);
            //Language
            string lang = "SYS_LANGUAGE.ENG";
            if(this.printLanguage ==1)
                lang = "SYS_LANGUAGE.TRA";
            else if(this.printLanguage == 2)
                lang = "SYS_LANGUAGE.SIM";
            Context = Context.Replace("TAG_LANG", lang);
            //end
            String FixupPageName = "";
            foreach (Object O in FolderOffset)
                FixupPageName = FixupPageName + (String)O + @"\";
            FixupPageName = FixupPageName + PageName;
            Context = Context.Replace("TAG_PAGENAME", FixupPageName);
            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
        }

        public object[] GetJQPage(ProjectItems PIs, String PageName, Object[] FolderOffset, ArrayList CodeList)
        {
            if (PIs == null)
                return null;
            if (FolderOffset.Length > 0)
            {
                String TargetFolder = (String)FolderOffset[0];
                foreach (ProjectItem PI in PIs)
                {
                    if (PI.Name.CompareTo(TargetFolder) == 0)
                    {
                        if (FolderOffset.Length == 1)
                        {
                            Object[] NewFolderOffset = new Object[0];
                            return GetJQPage(PI.ProjectItems, PageName, NewFolderOffset, CodeList);
                        }
                        else
                        {
                            Object[] NewFolderOffset = new Object[FolderOffset.Length - 1];
                            for (int I = 0; I < FolderOffset.Length - 1; I++)
                                NewFolderOffset[I] = FolderOffset[I + 1];
                            return GetJQPage(PI.ProjectItems, PageName, NewFolderOffset, CodeList);
                        }
                    }
                }
            }
            else
            {
                foreach (ProjectItem PI in PIs)
                {
                    if (PI.Name.CompareTo(PageName) == 0)
                    {
                        string title = "";
                        ArrayList datasourceList = new ArrayList();
                        List<JQDataGridPrint> datagridList = new List<JQDataGridPrint>();
                        List<JQDataFormPrint> viewLIst = new List<JQDataFormPrint>();
                        List<JQValidatePrint> ValidateList = new List<JQValidatePrint>();
                        List<JQDefaultPrint> DefaultList = new List<JQDefaultPrint>();
                        ArrayList ModuleList = new ArrayList();
                        Hashtable hs = new Hashtable();
                        Window FCodeWindow = PI.Open(Constants.vsViewKindCode);
                        FCodeWindow.Activate();
                        TextSelection modulets = (TextSelection)FCodeWindow.Document.Selection;
                        modulets.SelectAll();
                        string smodulets = modulets.Text;
                        FCodeWindow.Close(vsSaveChanges.vsSaveChangesNo);

                        Window FCodeDesignWindow = PI.Open(Constants.vsViewKindDesigner);
                        TextSelection designs = (TextSelection)FCodeDesignWindow.Document.Selection;
                        if (designs != null)
                        {
                            designs.SelectAll();
                            string smodulets2 = designs.Text;
                            if (smodulets2.IndexOf(@"<script") != -1 && smodulets2.LastIndexOf(@"</script>") != -1)
                                smodulets2 = smodulets2.Substring(smodulets2.IndexOf(@"<script"), smodulets2.LastIndexOf(@"</script>") - smodulets2.IndexOf(@"<script") + 9);
                            if (documentSetting[9])
                            {
                                CodeList.Add(new object[] { FCodeDesignWindow.Caption, smodulets2 });
                                CodeList.Add(new object[] { FCodeWindow.Caption, smodulets });
                            }
                            FCodeDesignWindow.Close(vsSaveChanges.vsSaveChangesNo);
                        }
                        System.Threading.Thread.Sleep(500);
                        Window FDesignWindow = PI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
                        FDesignWindow.Activate();
                        HTMLWindow W = (HTMLWindow)FDesignWindow.Object;
                        //object o = W.CurrentTabObject;

                        // webform designer though, is a special case 
                        //if (W != null)
                        //{
                        //    // make sure current tab is the designer tab 
                        //    W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
                        //    Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSP = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)o;
                        //    //Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage.DispDesignerDocument d = htmlWindow.CurrentTabObject as Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage.DispDesignerDocument;
                        //    Guid guidVsMDDDesigner = new Guid("7494682A-37A0-11d2-A273-00C04F8EF4FF");
                        //    Guid guidVsMDDDesigner2 = new Guid("7494682A-37A0-11d2-A273-00C04F8EF4FF");

                        //    IntPtr ptr;
                        //    int hr =oleSP.QueryService(ref guidVsMDDDesigner, ref guidVsMDDDesigner2, out ptr);
                        //    System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);

                        //    if (ptr != IntPtr.Zero)
                        //    {
                        //        IServiceProvider dotnetSP = (IServiceProvider)Marshal.GetObjectForIUnknown(ptr);
                        //        Marshal.Release(ptr);
                        //        //designerHost = (IDesignerHost)dotnetSP.GetService(typeof(IDesignerHost));
                        //    }
                        //}


                        W.CurrentTab = vsHTMLTabs.vsHTMLTabsSource;
                        TextSelection ts = (TextSelection)FDesignWindow.Document.Selection;
                        ts.SelectAll();
                        string sts = ts.Text;
                        StringReader sr = new StringReader(sts);
                        string firstline = sr.ReadLine();
                        while (firstline != "</html>" && firstline != null)
                        {
                            if (firstline.Contains("<title>"))
                            {
                                string spatten2 = @"<title>.*</title>";
                                MatchCollection lineMatchs2 = Regex.Matches(firstline, spatten2);
                                foreach (Match nMatch in lineMatchs2)
                                {
                                    string result = nMatch.ToString();
                                    title = result.Substring(7, result.Length - 15);
                                }
                                break;
                            }
                            firstline = sr.ReadLine();
                        }
                        sr.Close();
                        W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
                        Application.DoEvents();
                        IHTMLDocument2 IHTMLDocument = (IHTMLDocument2)W.CurrentTabObject;
                        foreach (IHTMLElement IHTMLElement in (IHTMLElementCollection)IHTMLDocument.activeElement.children)
                        {
                            if (IHTMLElement.tagName.ToLower().Contains(":jqdatagrid"))
                            {
                                Object webdatasource = GetJQRemoteName(IHTMLElement,ModuleList);
                                datasourceList.Add(webdatasource);
                                JQDataGridPrint webdatagrid = GetJQDataGrid(IHTMLElement);
                                datagridList.Add(webdatagrid);
                            }
                            else if (IHTMLElement.tagName.ToLower().Contains(":jqdataform"))
                            {
                                JQDataFormPrint webformview = GetJQFormView(IHTMLElement);
                                viewLIst.Add(webformview);
                            }
                            else if (IHTMLElement.tagName.ToLower().Contains(":jqvalidate"))
                            {
                                JQValidatePrint WebValidate = GetJQValidate(IHTMLElement);
                                ValidateList.Add(WebValidate);
                            }
                            else if (IHTMLElement.tagName.ToLower().Contains(":jqdefault"))
                            {
                                JQDefaultPrint WebDefault = GetJQDefault(IHTMLElement);
                                DefaultList.Add(WebDefault);
                            }
                            //else if (IHTMLElement.tagName.ToLower() == "ajaxgridview" || IHTMLElement.tagName.ToLower() == "ajaxTools:AjaxGridView")
                            //{
                            //    Object AjaxGridView = GetAjaxGridView(IHTMLElement);
                            //    viewLIst.Add(AjaxGridView);
                            //}
                            else
                            {
                                getJQHTMLElement(IHTMLElement,ModuleList, datasourceList, datagridList, viewLIst, ValidateList, DefaultList);
                            }
                        }
                        Application.DoEvents();
                        if (FDesignWindow != null)
                            FDesignWindow.Close(vsSaveChanges.vsSaveChangesNo);
                        replaceTableName(datasourceList, viewLIst, hs);
                        String FileName = string.Empty;
                        return new object[] { ModuleList, title, datasourceList, datagridList, viewLIst, DefaultList, ValidateList, hs };
                    }
                }
            }
            return null;
        }

        private void getJQHTMLElement(IHTMLElement IHTMLElement, ArrayList ModuleList, ArrayList datasourceList, List<JQDataGridPrint> datagridList, List<JQDataFormPrint> viewLIst, List<JQValidatePrint> ValidateList, List<JQDefaultPrint> DefaultList)
        {
            foreach (IHTMLElement IHTMLElementchi in (IHTMLElementCollection)IHTMLElement.children)
            {
                if (IHTMLElementchi.tagName.ToLower().Contains(":jqdatagrid"))
                {
                    Object webdatasource = GetJQRemoteName(IHTMLElementchi, ModuleList);
                    datasourceList.Add(webdatasource);
                    JQDataGridPrint webdatagrid = GetJQDataGrid(IHTMLElementchi);
                    datagridList.Add(webdatagrid);
                }
                else if (IHTMLElementchi.tagName.ToLower().Contains(":jqdataform"))
                {
                    JQDataFormPrint webformview = GetJQFormView(IHTMLElementchi);
                    viewLIst.Add(webformview);
                }
                else if (IHTMLElementchi.tagName.ToLower().Contains(":validate"))
                {
                    JQValidatePrint WebValidate = GetJQValidate(IHTMLElementchi);
                    ValidateList.Add(WebValidate);
                }
                else if (IHTMLElementchi.tagName.ToLower().Contains( ":jqdefault"))
                {
                    JQDefaultPrint WebDefault = GetJQDefault(IHTMLElementchi);
                    DefaultList.Add(WebDefault);
                }
                //else if (IHTMLElement.tagName.ToLower() == "ajaxgridview" || IHTMLElement.tagName.ToLower() == "ajaxTools:AjaxGridView")
                //{
                //    Object AjaxGridView = GetAjaxGridView(IHTMLElement);
                //    viewLIst.Add(AjaxGridView);
                //}
                else
                {
                    getJQHTMLElement(IHTMLElementchi, ModuleList, datasourceList, datagridList, viewLIst, ValidateList, DefaultList);
                }
            }
        }

        private Object GetJQRemoteName(IHTMLElement IHTMLElement, ArrayList ModuleList)
        {
            string RemoteName = IHTMLElement.getAttribute("RemoteName", 0).ToString();
            string DataMember = IHTMLElement.getAttribute("DataMember", 0).ToString();
            if (!ModuleList.Contains(RemoteName))
            {
                ModuleList.Add(RemoteName);
            }
            return new object[] { RemoteName, DataMember };
        }

        private JQDataGridPrint GetJQDataGrid(IHTMLElement IHTMLElement)
        {
            JQDataGridPrint dataGrid = new JQDataGridPrint();
            dataGrid.ID = IHTMLElement.id;
            dataGrid.DataOptions = IHTMLElement.getAttribute("data-options").ToString();
            dataGrid.RemoteName = IHTMLElement.getAttribute("RemoteName").ToString();
            dataGrid.DataMember = IHTMLElement.getAttribute("DataMember").ToString();
            dataGrid.Title = IHTMLElement.getAttribute("Title").ToString();
            dataGrid.AutoApply = IHTMLElement.getAttribute("AutoApply").ToString();
            dataGrid.AlwaysClose = IHTMLElement.getAttribute("AlwaysClose").ToString();
            dataGrid.Pagination = IHTMLElement.getAttribute("Pagination").ToString();
            dataGrid.PageSize = IHTMLElement.getAttribute("PageSize").ToString();
            dataGrid.QueryAutoColumn = IHTMLElement.getAttribute("QueryAutoColumn").ToString();
            dataGrid.DuplicateCheck = IHTMLElement.getAttribute("DuplicateCheck").ToString();
            dataGrid.JQDataGridQueryColumnsPrintList = new List<JQDataGridQueryColumnsPrint>();
            dataGrid.JQDataGridColumnsPrintList = new List<JQDataGridColumnsPrint>();
            dataGrid.JQDataGridToolItemsPrintList = new List<JQDataGridToolItemsPrint>();

            string s = IHTMLElement.innerHTML;
            string s1 = s.Substring(s.IndexOf("<Columns"), s.LastIndexOf("</Columns>") - s.IndexOf("<Columns"));
            string patten = "<.*?:JQGridColumn .*?(.|\r\n)*? />";
            MatchCollection Matchs = Regex.Matches(s1, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                JQDataGridColumnsPrint columnPrint = new JQDataGridColumnsPrint();
                #region columns
                string result = nMatch.ToString();
                int fieldindex = result.IndexOf("FieldName=\"", StringComparison.CurrentCultureIgnoreCase);
                if (fieldindex != -1)
                {
                    string fieldnamestring = result.Substring(fieldindex + 11);
                    int fieldendindex = fieldnamestring.IndexOf("\"");
                    if (fieldindex != -1)
                        columnPrint.DataField = fieldnamestring.Substring(0, fieldendindex);
                }
                int captionindex = result.IndexOf("Caption=\"", StringComparison.CurrentCultureIgnoreCase);
                if (captionindex != -1)
                {
                    string captionstring = result.Substring(captionindex + 9);
                    int captionendindex = captionstring.IndexOf("\"");
                    if (captionindex != -1)
                        columnPrint.HeaderText = captionstring.Substring(0, captionendindex);
                }
                int editorindex = result.IndexOf("Editor=\"", StringComparison.CurrentCultureIgnoreCase);
                if (editorindex != -1)
                {
                    string editorstring = result.Substring(editorindex + 8);
                    int editorendindex = editorstring.IndexOf("\"");
                    if (editorendindex != -1)
                        columnPrint.Editor = editorstring.Substring(0, editorendindex);
                }
                int editorOptionsindex = result.IndexOf("EditorOptions=\"", StringComparison.CurrentCultureIgnoreCase);
                if (editorOptionsindex != -1)
                {
                    string editorstring = result.Substring(editorOptionsindex + 15);
                    int editorOptionsendindex = editorstring.IndexOf("\"");
                    if (editorOptionsendindex != -1)
                        columnPrint.EditorOption = editorstring.Substring(0, editorOptionsendindex);
                }
                int formatindex = result.IndexOf("Format=\"", StringComparison.CurrentCultureIgnoreCase);
                if (formatindex != -1)
                {
                    string editorstring = result.Substring(formatindex + 8);
                    int formatendindex = editorstring.IndexOf("\"");
                    if (formatendindex != -1)
                        columnPrint.Format = editorstring.Substring(0, formatendindex);
                }
                int sortableindex = result.IndexOf("Sortable=\"", StringComparison.CurrentCultureIgnoreCase);
                if (sortableindex != -1)
                {
                    string editorstring = result.Substring(sortableindex + 10);
                    int sortableendindex = editorstring.IndexOf("\"");
                    if (sortableendindex != -1)
                        columnPrint.Sortable = editorstring.Substring(0, sortableendindex);
                }
                int frozenindex = result.IndexOf("Frozen=\"", StringComparison.CurrentCultureIgnoreCase);
                if (frozenindex != -1)
                {
                    string editorstring = result.Substring(frozenindex + 8);
                    int frozenendindex = editorstring.IndexOf("\"");
                    if (frozenendindex != -1)
                        columnPrint.Frozen = editorstring.Substring(0, frozenendindex);
                }
                int totalindex = result.IndexOf("Total=\"", StringComparison.CurrentCultureIgnoreCase);
                if (totalindex != -1)
                {
                    string editorstring = result.Substring(totalindex + 7);
                    int totalendindex = editorstring.IndexOf("\"");
                    if (totalendindex != -1)
                        columnPrint.Total = editorstring.Substring(0, totalendindex);
                }
                #endregion
                if (columnPrint.DataField != "")
                    dataGrid.JQDataGridColumnsPrintList.Add(columnPrint);
            }
            if (s.IndexOf("<QueryColumns") > 0 && s.LastIndexOf("</QueryColumns>") - s.IndexOf("<QueryColumns") > 0)
            {
                string s2 = s.Substring(s.IndexOf("<QueryColumns"), s.LastIndexOf("</QueryColumns>") - s.IndexOf("<QueryColumns"));
                string patten2 = "<.*?:JQQueryColumn .*?(.|\r\n)*? />";
                MatchCollection queryMatchs = Regex.Matches(s2, patten2, RegexOptions.IgnoreCase);
                foreach (Match nMatch in queryMatchs)
                {
                    JQDataGridQueryColumnsPrint queryColumnPrint = new JQDataGridQueryColumnsPrint();
                    #region querycolumns
                    string result = nMatch.ToString();
                    int fieldindex = result.IndexOf("FieldName=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (fieldindex != -1)
                    {
                        string fieldnamestring = result.Substring(fieldindex + 11);
                        int fieldendindex = fieldnamestring.IndexOf("\"");
                        if (fieldindex != -1)
                            queryColumnPrint.DataField = fieldnamestring.Substring(0, fieldendindex);
                    }
                    int captionindex = result.IndexOf("Caption=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (captionindex != -1)
                    {
                        string captionstring = result.Substring(captionindex + 9);
                        int captionendindex = captionstring.IndexOf("\"");
                        if (captionindex != -1)
                            queryColumnPrint.HeaderText = captionstring.Substring(0, captionendindex);
                    }
                    int editorindex = result.IndexOf("Editor=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (editorindex != -1)
                    {
                        string editorstring = result.Substring(editorindex + 8);
                        int editorendindex = editorstring.IndexOf("\"");
                        if (editorendindex != -1)
                            queryColumnPrint.Editor = editorstring.Substring(0, editorendindex);
                    }
                    int editorOptionsindex = result.IndexOf("Condition=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (editorOptionsindex != -1)
                    {
                        string editorstring = result.Substring(editorOptionsindex + 11);
                        int editorOptionsendindex = editorstring.IndexOf("\"");
                        if (editorOptionsendindex != -1)
                            queryColumnPrint.Condition = editorstring.Substring(0, editorOptionsendindex);
                    }
                    int formatindex = result.IndexOf("DefaultValue=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (formatindex != -1)
                    {
                        string editorstring = result.Substring(formatindex + 14);
                        int formatendindex = editorstring.IndexOf("\"");
                        if (formatendindex != -1)
                            queryColumnPrint.DefaultValue = editorstring.Substring(0, formatendindex);
                    }
                    int sortableindex = result.IndexOf("AndOr=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (sortableindex != -1)
                    {
                        string editorstring = result.Substring(sortableindex + 7);
                        int sortableendindex = editorstring.IndexOf("\"");
                        if (sortableendindex != -1)
                            queryColumnPrint.AndOr = editorstring.Substring(0, sortableendindex);
                    }
                    int frozenindex = result.IndexOf("NewLine=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (frozenindex != -1)
                    {
                        string editorstring = result.Substring(frozenindex + 9);
                        int frozenendindex = editorstring.IndexOf("\"");
                        if (frozenendindex != -1)
                            queryColumnPrint.NewLine = editorstring.Substring(0, frozenendindex);
                    }
                    #endregion
                    if (queryColumnPrint.DataField != "")
                        dataGrid.JQDataGridQueryColumnsPrintList.Add(queryColumnPrint);
                }
            }
            if (s.IndexOf("<TooItems") > 0 && s.LastIndexOf("</TooItems>") - s.IndexOf("<TooItems") > 0)
            {
                string s3 = s.Substring(s.IndexOf("<TooItems"), s.LastIndexOf("</TooItems>") - s.IndexOf("<TooItems"));
                string patten3 = "<.*?:JQToolItem .*?(.|\r\n)*? />";
                MatchCollection toolsMatchs = Regex.Matches(s3, patten3, RegexOptions.IgnoreCase);
                foreach (Match toolMatch in toolsMatchs)
                {
                    JQDataGridToolItemsPrint toolItemsPrint = new JQDataGridToolItemsPrint();
                    #region toolMatch
                    string result = toolMatch.ToString();
                    int edindex = result.IndexOf("ID=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (edindex != -1)
                    {
                        string fieldnamestring = result.Substring(edindex + 4);
                        int idendindex = fieldnamestring.IndexOf("\"");
                        if (idendindex != -1)
                            toolItemsPrint.ID = fieldnamestring.Substring(0, idendindex);
                    }
                    int iconindex = result.IndexOf("Icon=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (iconindex != -1)
                    {
                        string captionstring = result.Substring(iconindex + 6);
                        int iconendindex = captionstring.IndexOf("\"");
                        if (iconendindex != -1)
                            toolItemsPrint.Icon = captionstring.Substring(0, iconendindex);
                    }
                    int itemTypeindex = result.IndexOf("ItemType=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (itemTypeindex != -1)
                    {
                        string editorstring = result.Substring(itemTypeindex + 10);
                        int itemTypendeindex = editorstring.IndexOf("\"");
                        if (itemTypendeindex != -1)
                            toolItemsPrint.ItemType = editorstring.Substring(0, itemTypendeindex);
                    }
                    int textindex = result.IndexOf("Text=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (textindex != -1)
                    {
                        string editorstring = result.Substring(textindex + 6);
                        int textendindex = editorstring.IndexOf("\"");
                        if (textendindex != -1)
                            toolItemsPrint.Text = editorstring.Substring(0, textendindex);
                    }
                    int onClickindex = result.IndexOf("OnClick=\"", StringComparison.CurrentCultureIgnoreCase);
                    if (onClickindex != -1)
                    {
                        string editorstring = result.Substring(onClickindex + 9);
                        int onClickendindex = editorstring.IndexOf("\"");
                        if (onClickendindex != -1)
                            toolItemsPrint.OnClick = editorstring.Substring(0, onClickendindex);
                    }
                    #endregion
                    dataGrid.JQDataGridToolItemsPrintList.Add(toolItemsPrint);
                }
            }
            return dataGrid;
        }

        private JQDataFormPrint GetJQFormView(IHTMLElement IHTMLElement)
        {
            JQDataFormPrint dataForm = new JQDataFormPrint();
            dataForm.ID = IHTMLElement.id;
            dataForm.DataOptions = IHTMLElement.getAttribute("data-options").ToString();
            dataForm.RemoteName = IHTMLElement.getAttribute("RemoteName").ToString();
            dataForm.DataMember = IHTMLElement.getAttribute("DataMember").ToString();
            dataForm.IsShowFlowIcon = IHTMLElement.getAttribute("IsShowFlowIcon").ToString();
            dataForm.ValidateStyle = IHTMLElement.getAttribute("ValidateStyle").ToString();
            dataForm.ContinueAdd = IHTMLElement.getAttribute("ContinueAdd").ToString();
            dataForm.DuplicateCheck = IHTMLElement.getAttribute("DuplicateCheck").ToString();
            dataForm.JQDataFormColumnsPrintList = new List<JQDataFormColumnsPrint>();

            string s = IHTMLElement.innerHTML;
            string s1 = s.Substring(s.IndexOf("<Columns"), s.LastIndexOf("</Columns>") - s.IndexOf("<Columns"));
            string patten = "<.*?:JQFormColumn .*?(.|\r\n)*? />";
            MatchCollection Matchs = Regex.Matches(s1, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                JQDataFormColumnsPrint formColumn = new JQDataFormColumnsPrint();
                #region column
                string result = nMatch.ToString();
                int fieldindex = result.IndexOf("FieldName=\"", StringComparison.CurrentCultureIgnoreCase);
                if (fieldindex != -1)
                {
                    string fieldnamestring = result.Substring(fieldindex + 11);
                    int fieldendindex = fieldnamestring.IndexOf("\"");
                    if (fieldindex != -1)
                        formColumn.DataField = fieldnamestring.Substring(0, fieldendindex);
                }
                int captionindex = result.IndexOf("Caption=\"", StringComparison.CurrentCultureIgnoreCase);
                if (captionindex != -1)
                {
                    string captionstring = result.Substring(captionindex + 9);
                    int captionendindex = captionstring.IndexOf("\"");
                    if (captionindex != -1)
                        formColumn.HeaderText = captionstring.Substring(0, captionendindex);
                }
                int editorindex = result.IndexOf("Editor=\"", StringComparison.CurrentCultureIgnoreCase);
                if (editorindex != -1)
                {
                    string editorstring = result.Substring(editorindex + 8);
                    int editorendindex = editorstring.IndexOf("\"");
                    if (editorendindex != -1)
                        formColumn.Editor = editorstring.Substring(0, editorendindex);
                }
                int editorOptionsindex = result.IndexOf("EditorOptions=\"", StringComparison.CurrentCultureIgnoreCase);
                if (editorOptionsindex != -1)
                {
                    string editorstring = result.Substring(editorOptionsindex + 15);
                    int editorOptionsendindex = editorstring.IndexOf("\"");
                    if (editorOptionsendindex != -1)
                        formColumn.EditorOption = editorstring.Substring(0, editorOptionsendindex);
                }
                int formatindex = result.IndexOf("Format=\"", StringComparison.CurrentCultureIgnoreCase);
                if (formatindex != -1)
                {
                    string editorstring = result.Substring(formatindex + 8);
                    int formatendindex = editorstring.IndexOf("\"");
                    if (formatendindex != -1)
                        formColumn.Format = editorstring.Substring(0, formatendindex);
                }

                #endregion
                if (formColumn.DataField != "")
                    dataForm.JQDataFormColumnsPrintList.Add(formColumn);
            }

            return dataForm;
        }

        private JQValidatePrint GetJQValidate(IHTMLElement IHTMLElement)
        {
            JQValidatePrint validate = new JQValidatePrint();
            validate.ID = IHTMLElement.id;
            validate.BindingObjectID = IHTMLElement.getAttribute("BindingObjectID", 0).ToString();
            validate.JQValidateItemPrintList = new List<JQValidateItemPrint>();
            string patten = "<.*?:JQValidateColumn *?(\r\n)??.*?>";
            string s = IHTMLElement.innerHTML;
            MatchCollection Matchs = Regex.Matches(s, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                JQValidateItemPrint item = new JQValidateItemPrint();
                #region column
                string result = nMatch.ToString();
                int fieldindex = result.IndexOf("FieldName=\"", StringComparison.CurrentCultureIgnoreCase);
                if (fieldindex != -1)
                {
                    string fieldnamestring = result.Substring(fieldindex + 11);
                    int fieldendindex = fieldnamestring.IndexOf("\"");
                    if (fieldindex != -1)
                        item.FieldName = fieldnamestring.Substring(0, fieldendindex);
                }
                int checkNullindex = result.IndexOf("CheckNull=\"", StringComparison.CurrentCultureIgnoreCase);
                if (checkNullindex != -1)
                {
                    string fieldnamestring = result.Substring(checkNullindex + 11);
                    int checkNullendindex = fieldnamestring.IndexOf("\"");
                    if (checkNullendindex != -1)
                        item.CheckNull = fieldnamestring.Substring(0, checkNullendindex);
                }
                int validateTypeindex = result.IndexOf("ValidateType=\"", StringComparison.CurrentCultureIgnoreCase);
                if (validateTypeindex != -1)
                {
                    string fieldnamestring = result.Substring(validateTypeindex + 14);
                    int validateTypeendindex = fieldnamestring.IndexOf("\"");
                    if (validateTypeendindex != -1)
                        item.ValidateType = fieldnamestring.Substring(0, validateTypeendindex);
                }
                int checkRangeFromindex = result.IndexOf("CheckRangeFrom=\"", StringComparison.CurrentCultureIgnoreCase);
                if (checkRangeFromindex != -1)
                {
                    string fieldnamestring = result.Substring(checkRangeFromindex + 16);
                    int checkRangeFromendindex = fieldnamestring.IndexOf("\"");
                    if (fieldindex != -1)
                        item.CheckRangeFrom = fieldnamestring.Substring(0, checkRangeFromendindex);
                }
                int checkRangeToindex = result.IndexOf("CheckRangeTo=\"", StringComparison.CurrentCultureIgnoreCase);
                if (checkRangeToindex != -1)
                {
                    string fieldnamestring = result.Substring(checkRangeToindex + 14);
                    int checkRangeToendindex = fieldnamestring.IndexOf("\"");
                    if (checkRangeToendindex != -1)
                        item.CheckRangeTo = fieldnamestring.Substring(0, checkRangeToendindex);
                }
                #endregion
                validate.JQValidateItemPrintList.Add(item);
            }
            return validate;
        }

        private JQDefaultPrint GetJQDefault(IHTMLElement IHTMLElement)
        {
            JQDefaultPrint defaultp = new JQDefaultPrint();
            defaultp.ID = IHTMLElement.id;
            defaultp.BindingObjectID = IHTMLElement.getAttribute("BindingObjectID", 0).ToString();
            defaultp.JQDefaultItemPrintList = new List<JQDefaultItemPrint>();
            string patten = "<.*?:JQDefaultColumn *?(\r\n)??.*?>";
            string s = IHTMLElement.outerHTML;
            MatchCollection Matchs = Regex.Matches(s, patten, RegexOptions.IgnoreCase);
            foreach (Match nMatch in Matchs)
            {
                JQDefaultItemPrint item = new JQDefaultItemPrint();
                #region column
                string result = nMatch.ToString();
                int fieldindex = result.IndexOf("FieldName=\"", StringComparison.CurrentCultureIgnoreCase);
                if (fieldindex != -1)
                {
                    string fieldnamestring = result.Substring(fieldindex + 11);
                    int fieldendindex = fieldnamestring.IndexOf("\"");
                    if (fieldindex != -1)
                        item.FieldName = fieldnamestring.Substring(0, fieldendindex);
                }
                int defaultValueindex = result.IndexOf("DefaultValue=\"", StringComparison.CurrentCultureIgnoreCase);
                if (defaultValueindex != -1)
                {
                    string fieldnamestring = result.Substring(defaultValueindex + 14);
                    int defaultValueendindex = fieldnamestring.IndexOf("\"");
                    if (defaultValueendindex != -1)
                        item.DefaultValue = fieldnamestring.Substring(0, defaultValueendindex);
                }
                int carryOnindex = result.IndexOf("CarryOn=\"", StringComparison.CurrentCultureIgnoreCase);
                if (carryOnindex != -1)
                {
                    string fieldnamestring = result.Substring(carryOnindex + 9);
                    int carryOnendindex = fieldnamestring.IndexOf("\"");
                    if (carryOnendindex != -1)
                        item.CarryOn = fieldnamestring.Substring(0, carryOnendindex);
                }
                #endregion
                defaultp.JQDefaultItemPrintList.Add(item);
            }
            return defaultp;
        }

        public String GetJQPageImage(String WebSiteName, String PageName, Object[] FolderOffset, String UserID, String Password, String DBName, String SolutionName, String PrintWaitingTime, String PageTitle, String WebSitePath)
        {
            String FileName = "";
            dte2.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate();
            UIHierarchy A = (UIHierarchy)dte2.ActiveWindow.Object;
            RenameNoneLogonTagForJQ(PageName, FolderOffset, UserID, Password, DBName, SolutionName, WebSitePath);
            if (PrintWaitingTime == "0")
                PrintWaitingTime = "5";

            foreach (UIHierarchyItem aItem in A.UIHierarchyItems)
            {
                foreach (UIHierarchyItem B in aItem.UIHierarchyItems)
                {
                    if (B.Name.CompareTo(WebSiteName) == 0)
                    {
                        B.UIHierarchyItems.Expanded = true;
                        foreach (UIHierarchyItem C in B.UIHierarchyItems)
                        {
                            if (C.Name.CompareTo("NoneLogon.aspx") == 0)
                            {
                                C.Select(vsUISelectionType.vsUISelectionTypeSelect);
                                try
                                {
                                    dte2.MainWindow.Activate();
                                    dte2.ActiveWindow.Activate();
                                    C.DTE.ExecuteCommand("File.ViewinBrowser", String.Empty);
                                    System.Threading.Thread.Sleep(5000);//wait 5s for ie load
                                    Srvtools.ScreenCapture SC = new ScreenCapture();
                                    StringBuilder SB = new StringBuilder(200);
                                    FileName = Path.GetTempPath() + Path.GetRandomFileName() + ".jpg";
                                    PageTitle = PageTitle.Replace("&nbsp", " ").Trim();
                                    String WindowName = String.Format(PageTitle + " - Microsoft Internet Explorer", WebSiteName, FolderOffset[0], PageName); //String WindowName = String.Format(@"http://localhost:1130/EEPWebClient/{1}/{2} - Microsoft Internet Explorer", WebSiteName, FolderOffset[0], PageName);
                                    IntPtr IEHandle = FindIEHandle(PageTitle);
                                    int WaitCount = 0;
                                    while (IEHandle.ToInt32() == 0)
                                    {
                                        System.Threading.Thread.Sleep(50);
                                        IEHandle = FindIEHandle(PageTitle);
                                        if (IEHandle.ToInt32() == 0) IEHandle = FindIEHandle2(PageName);
                                        WaitCount++;
                                        if (WaitCount > Int32.Parse(printWaitingTime) * 1000 / 50) //printWaitingTime
                                            break;
                                    }
                                    ShowWindow(IEHandle, 3);
                                    SetActiveWindow(IEHandle);
                                    System.Threading.Thread.Sleep(Int32.Parse(printWaitingTime) * 1000);
                                    SC.CaptureWindowToFile(IEHandle, FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    PostMessage(IEHandle, 16, 0, 0);
                                    break;
                                }
                                catch (Exception E)
                                {
                                    MessageBox.Show(E.Message);
                                }
                            }
                        }
                    }
                }
            }
            return FileName;
        }

        private void RenameNoneLogonTagForJQ(String PageName, Object[] FolderOffset, String UserID, String Password, String DBName, String SolutionName, String WebSitePath)
        {
            String FileName = WebSitePath;
            if (FileName.EndsWith("\\")) FileName = FileName.Substring(0, FileName.Length - 1);
            String BackupFileName = FileName + @"\NoneLogon.aspx.cs.txt";
            FileName = FileName + @"\NoneLogon.aspx.cs";
            if (!File.Exists(BackupFileName))
                return;
            System.IO.File.Copy(BackupFileName, FileName, true);
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName);
            String Context = SR.ReadToEnd();
            SR.Close();
            Context = Context.Replace("TAG_USERID", UserID);
            Context = Context.Replace("TAG_PASSWORD", Password);
            Context = Context.Replace("TAG_DBNAME", DBName);
            Context = Context.Replace("TAG_SOLUTION", SolutionName);
            //Language
            string lang = "en-us";
            if (this.printLanguage == 1)
                lang = "zh-tw";
            else if (this.printLanguage == 2)
                lang = "zh-cn";
            Context = Context.Replace("TAG_LANG", lang);
            //end
            String FixupPageName = "";
            foreach (Object O in FolderOffset)
                FixupPageName = FixupPageName + (String)O + @"\";
            FixupPageName = FixupPageName + PageName;
            Context = Context.Replace("TAG_PAGENAME", FixupPageName);
            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
        }


        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern IntPtr SetActiveWindow(IntPtr wHandle);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        //备用
        public void WriteLog(string tableName, string OptionAction, string ErrorString, string OptionTime, string WrongString)
        {
            DirectoryInfo directorInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);//獲得當前DLL的目錄
            string sPath = directorInfo.Parent.FullName;//獲得當前DLL的上级目錄的全路径
            sPath = sPath + "/Log.txt";
            using (FileStream fsMyfile = new FileStream(sPath, FileMode.Append, FileAccess.Write))
            {    // 创建一个数据流写入器，和打开的文件关联
                StreamWriter swMyfile = new StreamWriter(fsMyfile);

                // 以文本方式写一个文件
                if (OptionAction == "OK")
                {
                    swMyfile.WriteLine(WrongString + OptionTime + "  " + tableName);
                }
                else
                {
                    swMyfile.WriteLine(OptionTime + "  " + tableName + "  " + OptionAction + ".  " + ErrorString.Replace(":", "") + "SQL:" + WrongString);
                }
                swMyfile.WriteLine(); // 冲刷数据(把数据真正写到文件中去)
                // 注释该句试试看，程序将报错
                swMyfile.Flush();

            }
        }
    }

}
