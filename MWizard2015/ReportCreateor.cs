using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using System.Xml;
using System.Data;
using System.Windows.Forms;
using Srvtools;
using System.Data.Common;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel.Design;
//using Microsoft.Reporting.WinForms;
using System.Web.UI;
#if VS90
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
#endif

namespace MWizard2015
{
    public static class ReportCreator
    {
        private const string ms = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition";
        private const string rd = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";

        public static Project FindProject(Solution2 sln, string projFullName)
        {
            foreach (Project proj in sln.Projects)
            {
                if (proj.Name.ToLower() == projFullName.ToLower() || proj.FullName.ToLower() == projFullName.ToLower())
                {
                    return proj;
                }
            }
            return null;
        }

        public static List<string> FindAllClientProject(Solution2 sln)
        {
            List<string> projList = new List<string>();
            string webPath = GetWebClientPath();
            foreach (Project proj in sln.Projects)
            {
                if (proj.Name != "AjaxTools" && proj.Name != "ChartTools" && proj.Name != "EEPNetClient"
                    && proj.Name != "EEPNetAutoRun" && proj.Name != "EEPNetAutoRunForWeb" && proj.Name != webPath
                    && proj.Name != "EEPManager" && proj.Name != "EEPMessgenger" && proj.Name != "InfoRemoteModule"
                    && proj.Name != "EEPNetFLClient" && proj.Name != "EEPNetRunStep" && proj.Name != "EEPNetServer"
                    && proj.Name != "FLCore" && proj.Name != "FLDesigner" && proj.Name != "FLDesignerCore"
                    && proj.Name != "FLRuntime" && proj.Name != "FLTools" && proj.Name != "GLModule"
                    && proj.Name != "InitEEP" && proj.Name != "MWizard" && proj.Name != "Srvtools")
                {
                    projList.Add(proj.Name);
                }
            }
            return projList;
        }

        public static ProjectItem FindProjectItem(Project proj, string projItemName)
        {
            foreach (ProjectItem projItem in proj.ProjectItems)
            {
                if (projItem.Name.ToLower() == projItemName.ToLower())
                {
                    return projItem;
                }
            }
            return null;
        }

        public static ProjectItem FindProjectItem(ProjectItem ownerProjItem, string projItemName)
        {
            foreach (ProjectItem projItem in ownerProjItem.ProjectItems)
            {
                if (projItem.Name.ToLower() == projItemName.ToLower())
                {
                    return projItem;
                }
            }
            return null;
        }

        public static ProjectItem CreateProjectItem(Project proj, string projItemName, int projItemType)
        {
            ProjectItem projItem = null;
            switch (projItemType)
            {
                case 0: // 文件夹
                    projItem = proj.ProjectItems.AddFolder(projItemName, "");
                    break;
                /*要添加任意ProjectItem入口*/
                default:
                    break;
            }
            return projItem;
        }

        public static ProjectItem CreateProjectItem(ProjectItem ownerProjItem, string projItemName, int projItemType)
        {
            ProjectItem projItem = null;
            switch (projItemType)
            {
                case 0: // 文件夹
                    ownerProjItem.ProjectItems.AddFolder(projItemName, "");
                    break;
                /*要添加任意ProjectItem入口*/
                default:
                    break;
            }
            return projItem;
        }

        private static string GetRptFileName(string txtFileNameString)
        {
            string filename = txtFileNameString;
            if (string.IsNullOrEmpty(filename))
            {
                filename = "Report1.rdlc";
            }
            else
            {
                if (!filename.ToLower().EndsWith(".rdlc"))
                {
                    filename += ".rdlc";
                }
            }

            return filename;
        }

        private static bool IsFileExisted(ProjectItems ownerProjs, string filename)
        {
            //判断有无已存在的同名文件
            foreach (ProjectItem item in ownerProjs)
            {
                if (item.Name == filename)
                {
                    if (MessageBox.Show("There is another File which name is " + filename + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        string Path = item.get_FileNames(0);
                        Path = System.IO.Path.GetDirectoryName(Path);
                        item.Delete();
                    }
                    else
                        return false;
                    break;
                }
            }
            return true;
        }

        public static void GenWebReportFiles(ProjectItem ownerProj, ProjectItem projItem, ReportParameter rptParams, List<string> rptFileNames, List<string>RealTableName)
        {
            DataSet dsDesigner = new DataSet();
            string projDir = ownerProj.ContainingProject.FullName;
            string dir = projDir + ownerProj.Name + "\\";
            dsDesigner.ReadXmlSchema(dir + projItem.Name);
            if (rptParams.RptSet.ReportTables.Count == 1) // Single
            {
                string filename = GetRptFileName(rptFileNames[0]);
                if (!IsFileExisted(ownerProj.ProjectItems, filename))
                    return;

                if (rptParams.RptStyle == EEPReportStyle.Label)
                {
                    ProjectItem genItem = ownerProj.ProjectItems.AddFromFileCopy(projDir + @"Template\SingleLabel.rdlc");
                    genItem.Name = filename;
                    GenSingleLabelRdlc(dir + filename, rptParams, dsDesigner.Tables[0], RealTableName[0]);
                }
                else if (rptParams.RptStyle == EEPReportStyle.Table)
                {
                    ProjectItem genItem = ownerProj.ProjectItems.AddFromFileCopy(projDir + @"Template\SingleTable.rdlc");
                    genItem.Name = filename;
                    GenSingleTableRdlc(dir + filename, rptParams, dsDesigner.Tables[0], RealTableName[0]);
                }
            }
            else if (rptParams.RptSet.ReportTables.Count >= 2) // Master-Details
            {
                string masterFileName = GetRptFileName(rptFileNames[0]);
                string detailsFileName = GetRptFileName(rptFileNames[1]);
                string masterRealTableName = RealTableName[0];
                string detailRealTableName = RealTableName[1];
                if (!IsFileExisted(ownerProj.ProjectItems, masterFileName) || !IsFileExisted(ownerProj.ProjectItems, detailsFileName))
                    return;

                ProjectItem masterItem = ownerProj.ProjectItems.AddFromFileCopy(projDir + @"Template\LabelMaster.rdlc");
                masterItem.Name = masterFileName;

                ProjectItem detailsItem = ownerProj.ProjectItems.AddFromFileCopy(projDir + @"Template\TableDetails.rdlc");
                detailsItem.Name = detailsFileName;

                GenMasterDetailsRdlc(dir + masterItem.Name, dir + detailsItem.Name, rptParams, dsDesigner, masterRealTableName, detailRealTableName);
            }
        }

        public static void GenWinReportFiles(Project ownerProj, ProjectItem projItem, ReportParameter rptParams, List<string> rptFileNames, List<string> RealTableName)
        {
            DataSet dsDesigner = new DataSet();
            string projDir = ownerProj.FullName.Substring(0, ownerProj.FullName.IndexOf(ownerProj.Name));
            string ctPath = EEPRegistry.Client;
            string ct = "";
            if (ctPath.ToLower().IndexOf("eepnetclient") != -1)
                ct = "eepnetclient";
            else if (ctPath.ToLower().IndexOf("eepnetflclient") != -1)
                ct = "eepnetflclient";
            if (!string.IsNullOrEmpty(ct))
                ctPath = ctPath.Substring(0, ctPath.ToLower().IndexOf(ct));
            dsDesigner.ReadXmlSchema(projDir + ownerProj.Name + "\\" + projItem.Name);
            if (rptParams.RptSet.ReportTables.Count == 1) // Single
            {
                string filename = GetRptFileName(rptFileNames[0]);
                if (!IsFileExisted(ownerProj.ProjectItems, filename))
                    return;

                if (rptParams.RptStyle == EEPReportStyle.Label)
                {
                    ProjectItem genItem = ownerProj.ProjectItems.AddFromFileCopy(ctPath + @"EEPNetReport\SingleLabel.rdlc");
                    genItem.Name = filename;
                    try
                    {
                        GenSingleLabelRdlc(projDir + ownerProj.Name + "\\" + filename, rptParams, dsDesigner.Tables[0], RealTableName[0]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (rptParams.RptStyle == EEPReportStyle.Table)
                {
                    ProjectItem genItem = ownerProj.ProjectItems.AddFromFileCopy(ctPath + @"EEPNetReport\SingleTable.rdlc");
                    genItem.Name = filename;
                    try
                    {
                        GenSingleTableRdlc(projDir + ownerProj.Name + "\\" + filename, rptParams, dsDesigner.Tables[0], RealTableName[0]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (rptParams.RptSet.ReportTables.Count >= 2) // Master-Details
            {
                string masterFileName = GetRptFileName(rptFileNames[0]);
                string detailsFileName = GetRptFileName(rptFileNames[1]);
                string masterRealTableName = RealTableName[0];
                string detailRealTableName = RealTableName[1];
                if (!IsFileExisted(ownerProj.ProjectItems, masterFileName) || !IsFileExisted(ownerProj.ProjectItems, detailsFileName))
                    return;

                ProjectItem masterItem = ownerProj.ProjectItems.AddFromFileCopy(ctPath + @"EEPNetReport\LabelMaster.rdlc");
                masterItem.Name = masterFileName;

                ProjectItem detailsItem = ownerProj.ProjectItems.AddFromFileCopy(ctPath + @"EEPNetReport\TableDetails.rdlc");
                detailsItem.Name = detailsFileName;

                try
                {
                    GenMasterDetailsRdlc(projDir + ownerProj.Name + "\\" + masterItem.Name, projDir + ownerProj.Name + "\\" + detailsItem.Name, rptParams, dsDesigner, masterRealTableName, detailRealTableName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void GenSingleLabelRdlc(string rdlcFileName, ReportParameter rptParams, DataTable tabDesigner, string RealTableName)
        {
            GenSingleLabelRdlc(rdlcFileName, rptParams, tabDesigner, RealTableName, false, "", null);
        }

        private static void GenSingleLabelRdlc(string rdlcFileName, ReportParameter rptParams, DataTable tabDesigner, string RealTableName, bool isMaster, string detailsRptFileName, DataColumn[] relationParentColumns)
        {
            //DataTable dtDataDic = GetDDTable(rptParams.ClientType, rptParams.SelectAlias, tabDesigner.TableName);
            DataTable dtDataDic = GetDDTable(rptParams.ClientType, rptParams.SelectAlias, RealTableName);
            XmlDocument doc = new XmlDocument();
            doc.Load(rdlcFileName);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ms", ms);
            nsmgr.AddNamespace("rd", rd);
            //生成DataSources
            GenDataSources(doc, nsmgr);
            //生成DataSets
            GenDataSets(doc, nsmgr, tabDesigner);
            //生成List
            GenList(doc, nsmgr, rptParams, tabDesigner.TableName);

            XmlNode startNode = GetStartXPathNode(doc, nsmgr, true, isMaster);
            //给Title赋值
            XmlNode nTitleValue = GetTitleNode(doc, nsmgr, true).SelectSingleNode("ms:Value", nsmgr);
            ReportTable rptTable = rptParams.RptSet.ReportTables[0];
            nTitleValue.InnerText = rptTable.TableDescription;

            #region GenDataRegion
            // Top Init
            float curLeft = 0.5f;
            float curTop = 0.5f;
            int layoutIndex = 0;
            XmlDocument docrealContext = startNode.OwnerDocument;
            XmlNode temNode = startNode;
            if (!isMaster)
            {
                temNode = docrealContext.CreateElement("ReportItems", temNode.NamespaceURI);
            }
            for (int i = 0; i < rptTable.ReportColumns.Count; i++)
            {
                string colName = rptTable.ReportColumns[i].ColumnName;
                string EditMask = GetFieldEditMask(dtDataDic, colName);
                if (layoutIndex >= rptParams.LayoutColumnNum)
                {
                    layoutIndex = 0;
                    curLeft = 0.5f;
                    curTop += 0.94f;

                    //GenLabel
                    GenLabel(temNode, curLeft, curTop, "lbl" + colName, GetFieldCaption(dtDataDic, colName) + ":", rptParams.HorGaps, rptParams.VertGaps, false);
                    //GenrTextBox
                    curLeft += (float)Convert.ToDouble(rptParams.HorGaps);
                    GenLabel(temNode, curLeft, curTop, "txt" + colName, EditMask, rptParams.HorGaps, rptParams.VertGaps, false);
                    layoutIndex++;
                    curLeft += (float)Convert.ToDouble(rptParams.HorGaps) + 0.5f;
                }
                else
                {
                    //GenLabel
                    GenLabel(temNode, curLeft, curTop, "lbl" + colName, GetFieldCaption(dtDataDic, colName) + ":", rptParams.HorGaps, rptParams.VertGaps, false);
                    //GenrTextBox
                    curLeft += (float)Convert.ToDouble(rptParams.HorGaps);
                    GenLabel(temNode, curLeft, curTop, "txt" + colName, EditMask, rptParams.HorGaps, rptParams.VertGaps, false);
                    layoutIndex++;
                    curLeft += (float)Convert.ToDouble(rptParams.HorGaps) + 0.5f;
                }
            }
            if (!isMaster)
            {
                startNode.AppendChild(temNode);
            }

            GenReportBodyHeight(doc, curTop, nsmgr, isMaster);

            #endregion

            if (isMaster)
            {
                XmlNode nSubReport = GetStartXPathNode(doc, nsmgr, true, isMaster).SelectSingleNode("ms:Subreport[@Name='subreport1']", nsmgr);
                //Parameters
                XmlNode nParameters = doc.CreateElement("Parameters", nSubReport.NamespaceURI);
                foreach (DataColumn col in relationParentColumns)
                {
                    //Parameters.Parameter
                    XmlNode nParameter = doc.CreateElement("Parameter", nSubReport.NamespaceURI);
                    XmlAttribute aParamName = doc.CreateAttribute("Name");
                    aParamName.Value = "p" + col.ColumnName;
                    nParameter.Attributes.Append(aParamName);
                    //Parameters.Parameter.Value
                    string EditMask = GetFieldEditMask(dtDataDic, col.ColumnName);
                    XmlNode nParamValue = doc.CreateElement("Value", nSubReport.NamespaceURI);
                    nParamValue.InnerText = EditMask;
                    nParameter.AppendChild(nParamValue);
                    nParameters.AppendChild(nParameter);
                }
                nSubReport.AppendChild(nParameters);
                nSubReport.SelectSingleNode("ms:ReportName", nsmgr).InnerText = detailsRptFileName;
            }
            doc.Save(rdlcFileName);
        }        

        private static void GenSingleTableRdlc(string rdlcFileName, ReportParameter rptParams, DataTable tabDesigner, string RealTableName)
        {
            GenSingleTableRdlc(rdlcFileName, rptParams, tabDesigner, RealTableName, false, null);
        }

        private static void GenSingleTableRdlc(string rdlcFileName, ReportParameter rptParams, DataTable tabDesigner, string RealTableName, bool isDetails, DataRelation relation)
        {
            DataTable dtDataDic = GetDDTable(rptParams.ClientType, rptParams.SelectAlias, RealTableName);
            XmlDocument doc = new XmlDocument();
            doc.Load(rdlcFileName);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ms", ms);
            nsmgr.AddNamespace("rd", rd);
            //生成DataSources
            GenDataSources(doc, nsmgr);
            //生成DataSets
            GenDataSets(doc, nsmgr, tabDesigner);

            XmlNode startNode = GetStartXPathNode(doc, nsmgr, false, isDetails);
            XmlNode temNode = startNode;
            //ReportItems
            if (!isDetails)
            {
                temNode = doc.CreateElement("ReportItems", startNode.NamespaceURI);
            }
            ReportTable rptTable = isDetails ? rptParams.RptSet.ReportTables[1] : rptParams.RptSet.ReportTables[0];
            if (isDetails)
            {
                XmlNode nReport = doc.SelectSingleNode("ms:Report", nsmgr);
                //ReportParameters
                XmlNode nReportParameters = doc.CreateElement("ReportParameters", nReport.NamespaceURI);
                foreach (DataColumn parentCol in relation.ParentColumns)
                {
                    //ReportParameter
                    XmlNode nReportParameter = doc.CreateElement("ReportParameter", nReport.NamespaceURI);
                    XmlAttribute aReportParameterName = doc.CreateAttribute("Name");
                    aReportParameterName.Value = "p" + parentCol.ColumnName;
                    nReportParameter.Attributes.Append(aReportParameterName);
                    //ReportParameter.DataType
                    XmlNode nDataType = doc.CreateElement("DataType", nReport.NamespaceURI);
                    string type = parentCol.DataType.ToString();
                    if (type.IndexOf('.') != -1)
                        type = type.Substring(type.LastIndexOf('.') + 1);
                    if (type.StartsWith("Int"))
                        type = "Integer";
                    nDataType.InnerText = type;
                    nReportParameter.AppendChild(nDataType);
                    //ReportParameter.AllowBlank
                    XmlNode nAllowBlank = doc.CreateElement("AllowBlank", nReport.NamespaceURI);
                    nAllowBlank.InnerText = "true";
                    nReportParameter.AppendChild(nAllowBlank);
                    //ReportParameter.Prompt
                    XmlNode nPrompt = doc.CreateElement("Prompt", nReport.NamespaceURI);
                    nPrompt.InnerText = parentCol.ColumnName;
                    nReportParameter.AppendChild(nPrompt);
                    nReportParameters.AppendChild(nReportParameter);
                }
                nReport.AppendChild(nReportParameters);
                //caption
                float childColLeft = 0.25f;
                float childColTop = 0.25f;
                //XmlDocument docrealContext = startNode.OwnerDocument;
                //XmlNode temNode = startNode;
                //temNode = docrealContext.CreateElement("ReportItems", temNode.NamespaceURI);
                foreach (DataColumn parentCol in relation.ParentColumns)
                {
                    //caption 
                    GenLabel(temNode, childColLeft, childColTop, "cap" + parentCol.ColumnName, GetFieldCaption(dtDataDic, parentCol.ColumnName), rptParams.HorGaps, rptParams.VertGaps, false);
                    childColLeft += (float)Convert.ToDouble(rptParams.HorGaps);
                    //value
                    GenLabel(temNode, childColLeft, childColTop, "txt" + parentCol.ColumnName, "=Parameters!p" + parentCol.ColumnName + ".Value", rptParams.HorGaps, rptParams.VertGaps, false);
                    childColLeft += (float)Convert.ToDouble(rptParams.HorGaps) + 0.5f;
                }                
            }
            else
            {
                //给Title赋值
                XmlNode nTitleValue = GetTitleNode(doc, nsmgr, false).SelectSingleNode("ms:Value", nsmgr);
                nTitleValue.InnerText = rptTable.TableDescription;
            }

            #region GenDataRegion
            //float curLeft = 0.5f;
            float curTop = 0.5f;
            if (isDetails)
            {
                curTop = GetDataRegionTop(doc, nsmgr, false, isDetails);
            }
            //Table
            XmlNode nTable = doc.CreateElement("Table", startNode.NamespaceURI);
            XmlAttribute aTableName = doc.CreateAttribute("Name");
            aTableName.Value = "table1";
            nTable.Attributes.Append(aTableName);
            if (isDetails)
            {
                //Filters
                XmlNode nFilters = doc.CreateElement("Filters", nTable.NamespaceURI);

                for (int i = 0; i < relation.ParentColumns.Length; i++)
                {
                    //Filters.Filter
                    XmlNode nFilter = doc.CreateElement("Filter", nTable.NamespaceURI);
                    //Filters.Filter.Operator
                    XmlNode nOperator = doc.CreateElement("Operator", nTable.NamespaceURI);
                    nOperator.InnerText = "Equal";
                    nFilter.AppendChild(nOperator);
                    //Filters.Filter.FilterValues
                    XmlNode nFilterValues = doc.CreateElement("FilterValues", nTable.NamespaceURI);
                    //Filters.Filter.FilterValues.FilterValue
                    XmlNode nFilterValue = doc.CreateElement("FilterValue", nTable.NamespaceURI);
                    nFilterValue.InnerText = "=Parameters!p" + relation.ParentColumns[i].ColumnName + ".Value";
                    nFilterValues.AppendChild(nFilterValue);
                    nFilter.AppendChild(nFilterValues);
                    //Filters.Filter.FilterExpression
                    XmlNode nFilterExpression = doc.CreateElement("FilterExpression", nTable.NamespaceURI);
                    nFilterExpression.InnerText = "=Fields!" + relation.ChildColumns[i].ColumnName + ".Value";
                    nFilter.AppendChild(nFilterExpression);
                    nFilters.AppendChild(nFilter);
                }
                nTable.AppendChild(nFilters);
            }
            //Table.Left
            XmlNode nTableLeft = doc.CreateElement("Left", startNode.NamespaceURI);
            nTableLeft.InnerText = "0.5cm";
            nTable.AppendChild(nTableLeft);
            //Table.Top
            XmlNode nTableTop = doc.CreateElement("Top", startNode.NamespaceURI);
            nTableTop.InnerText = curTop.ToString() + "cm";
            nTable.AppendChild(nTableTop);
            //Table.DataSetName
            XmlNode nTableDataSetName = doc.CreateElement("DataSetName", startNode.NamespaceURI);
            nTableDataSetName.InnerText = "NewDataSet_" + tabDesigner.TableName;
            nTable.AppendChild(nTableDataSetName);
            //Table.ZIndex
            XmlNode nTableZIndex = doc.CreateElement("ZIndex", startNode.NamespaceURI);
            nTableZIndex.InnerText = "2";
            nTable.AppendChild(nTableZIndex);
            //Table.Width
            XmlNode nWidth = doc.CreateElement("Width", startNode.NamespaceURI);
            nWidth.InnerText = (2.7f * rptTable.ReportColumns.Count).ToString() + "cm";
            nTable.AppendChild(nWidth);
            //Table.Style
            XmlNode nTableStyle = doc.CreateElement("Style", startNode.NamespaceURI);
            //Table.Style.FontFamily
            XmlNode nFontFamily = doc.CreateElement("FontFamily", startNode.NamespaceURI);
            nFontFamily.InnerText = "PMingLiU";
            nTableStyle.AppendChild(nFontFamily);
            nTable.AppendChild(nTableStyle);
            //Table.TableGroups
            if (HasGroupCondition(rptTable))
            {
                GenTableGroups(nTable, rptTable, rptParams, RealTableName);
            }
            //Table.Header
            XmlNode nHeader = doc.CreateElement("Header", startNode.NamespaceURI);
            GenTableRows(nHeader, rptTable, rptParams, "hd", rptParams.ClientType, rptParams.SelectAlias, RealTableName);
            //Table.Header.RepeatOnNewPage
            XmlNode nRepeatOnNewPage = doc.CreateElement("RepeatOnNewPage", startNode.NamespaceURI);
            nRepeatOnNewPage.InnerText = "true";
            nHeader.AppendChild(nRepeatOnNewPage);
            nTable.AppendChild(nHeader);
            //Table.Details
            XmlNode nDetails = doc.CreateElement("Details", startNode.NamespaceURI);
            GenTableRows(nDetails, rptTable, rptParams, "cnt", rptParams.ClientType, rptParams.SelectAlias, RealTableName);
            nTable.AppendChild(nDetails);
            //Table.TableColumns
            GenTableColumns(nTable, rptTable.ReportColumns.Count, rptParams.HorGaps);
            temNode.AppendChild(nTable);
            if (!isDetails)
            {
                startNode.AppendChild(temNode);
            }
            #endregion
            doc.Save(rdlcFileName);
        }

        private static void GenMasterDetailsRdlc(string rdlcMasterFileName, string rdlcDetailsFileName, ReportParameter rptParams, DataSet dsDesigner, string masterRealTableName, string detailRealTabelName)
        {
            DataRelation relation = dsDesigner.Relations[0];
            //Master
            string rdlcName = rdlcDetailsFileName.Substring(rdlcDetailsFileName.LastIndexOf('\\') + 1, rdlcDetailsFileName.LastIndexOf('.') - rdlcDetailsFileName.LastIndexOf('\\') - 1);
            try
            {
                GenSingleLabelRdlc(rdlcMasterFileName, rptParams, dsDesigner.Tables[0], masterRealTableName, true, rdlcName, relation.ParentColumns);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Details
            try
            {
                GenSingleTableRdlc(rdlcDetailsFileName, rptParams, dsDesigner.Tables[1], detailRealTabelName, true, relation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void GenList(XmlDocument doc, XmlNamespaceManager nsmgr, ReportParameter rptParams, string designerTableName)
        {
            XmlNode nBodyRptItems = doc.SelectSingleNode("ms:Report/ms:Body/ms:ReportItems", nsmgr);
            XmlNode nRect = nBodyRptItems.SelectSingleNode("ms:Rectangle[@Name='rectContainer']", nsmgr);
            nBodyRptItems.RemoveChild(nRect);

            //List
            XmlNode nList = doc.CreateElement("List", nBodyRptItems.NamespaceURI);
            XmlAttribute aListName = doc.CreateAttribute("Name");
            aListName.Value = "lstContainer";
            nList.Attributes.Append(aListName);
            //生成Group设置
            if (HasGroupCondition(rptParams.RptSet.ReportTables[0]))
            {
                //Grouping
                XmlNode nGrouping = doc.CreateElement("Grouping", nList.NamespaceURI);
                XmlAttribute aGroupingName = doc.CreateAttribute("Name");
                aGroupingName.Value = "lstContainer_Details_" + designerTableName;
                nGrouping.Attributes.Append(aGroupingName);
                //Grouping.PageBreakAtEnd
                XmlNode nPageBreakAtEnd = doc.CreateElement("PageBreakAtEnd", nList.NamespaceURI);
                nPageBreakAtEnd.InnerText = "true";
                nGrouping.AppendChild(nPageBreakAtEnd);
                //Grouping.GroupExpressions
                XmlNode nGroupExpressions = doc.CreateElement("GroupExpressions", nList.NamespaceURI);
                List<ReportColumn> lstGroupConditionFields = GetGroupConditionRptColumns(rptParams.RptSet.ReportTables[0]);
                foreach (ReportColumn cdtField in lstGroupConditionFields)
                {
                    //Grouping.GroupExpressions.GroupExpression
                    XmlNode nGroupExpression = doc.CreateElement("GroupExpression", nList.NamespaceURI);

                    nGroupExpression.InnerText = "=Fields!" + cdtField.ColumnName + ".Value";
                    nGroupExpressions.AppendChild(nGroupExpression);
                }
                nGrouping.AppendChild(nGroupExpressions);
                nList.AppendChild(nGrouping);
            }
            //List.ReportItems
            XmlNode nListRptItems = doc.CreateElement("ReportItems", nBodyRptItems.NamespaceURI);
            nListRptItems.AppendChild(nRect);
            nList.AppendChild(nListRptItems);
            //List.Style
            XmlNode nListStyle = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
            //List.Style.FontFamily
            XmlNode nFont = doc.CreateElement("FontFamily", nBodyRptItems.NamespaceURI);
            nFont.InnerText = "PMingLiU";
            nListStyle.AppendChild(nFont);
            nList.AppendChild(nListStyle);
            nBodyRptItems.AppendChild(nList);
        }

        private static bool HasGroupCondition(ReportTable rptTable)
        {
            foreach (ReportColumn rptCol in rptTable.ReportColumns)
            {
                if (rptCol.IsGroupCondition)
                    return true;
            }
            return false;
        }

        private static List<ReportColumn> GetGroupConditionRptColumns(ReportTable rptTable)
        {
            List<ReportColumn> lstGCols = new List<ReportColumn>();
            foreach (ReportColumn rptCol in rptTable.ReportColumns)
            {
                if (rptCol.IsGroupCondition)
                    lstGCols.Add(rptCol);
            }
            return lstGCols;
        }

        public static void GenDataSources(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            XmlNode nReport = doc.SelectSingleNode("ms:Report", nsmgr);
            //DataSources
            XmlNode nDataSources = doc.CreateElement("DataSources", nReport.NamespaceURI);
            //DataSources.DataSource
            XmlNode nDataSource = doc.CreateElement("DataSource", nReport.NamespaceURI);
            XmlAttribute aName = doc.CreateAttribute("Name");
            aName.Value = "DummyDataSource";
            nDataSource.Attributes.Append(aName);
            //DataSources.DataSource.ConnectionProperties
            XmlNode nConnectionProperties = doc.CreateElement("ConnectionProperties", nReport.NamespaceURI);
            //DataSources.DataSource.ConnectionProperties.ConnectString
            XmlNode nConnectString = doc.CreateElement("ConnectString", nReport.NamespaceURI);
            nConnectString.InnerText = "Data Source=" + Environment.UserName + ";Initial Catalog=aspnetdb;Integrated Security=True;";
            nConnectionProperties.AppendChild(nConnectString);
            //DataSources.DataSource.ConnectionProperties.DataProvider
            XmlNode nDataProvider = doc.CreateElement("DataProvider", nReport.NamespaceURI);
            nDataProvider.InnerText = "SQL";
            nConnectionProperties.AppendChild(nDataProvider);
            nDataSource.AppendChild(nConnectionProperties);
            //DataSources.DataSource.DataSourceID
            XmlNode nDataSourceID = doc.CreateElement("rd", "DataSourceID", rd);
            nDataSourceID.InnerText = Guid.NewGuid().ToString();
            nDataSource.AppendChild(nDataSourceID);
            nDataSources.AppendChild(nDataSource);
            nReport.AppendChild(nDataSources);
        }

        public static void GenDataSets(XmlDocument doc, XmlNamespaceManager nsmgr, DataTable tabDesigner)
        {
            XmlNode nReport = doc.SelectSingleNode("ms:Report", nsmgr);
            //DataSets
            XmlNode nDataSets = doc.CreateElement("DataSets", nReport.NamespaceURI);

            //DataSet
            XmlNode nDataSet = doc.CreateElement("DataSet", nReport.NamespaceURI);
            XmlAttribute aDataSetName = doc.CreateAttribute("Name");
            aDataSetName.InnerText = "NewDataSet_" + tabDesigner.TableName;
            nDataSet.Attributes.Append(aDataSetName);
            //DataSet.DataSetInfo
            XmlNode nDataSetInfo = doc.CreateElement("rd", "DataSetInfo", rd);
            //DataSet.DataSetInfo.DataSetName
            XmlNode nDataSetName = doc.CreateElement("DataSetName");
            nDataSetName.InnerText = "NewDataSet";
            nDataSetInfo.AppendChild(nDataSetName);
            //DataSet.DataSetInfo.TableName
            XmlNode nTableName = doc.CreateElement("TableName");
            nTableName.InnerText = tabDesigner.TableName;
            nDataSetInfo.AppendChild(nTableName);
            nDataSet.AppendChild(nDataSetInfo);
            //DataSet.Query
            XmlNode nQuery = doc.CreateElement("Query", nReport.NamespaceURI);
            //DataSet.Query.UseGenericDesigner
            XmlNode nUseGenericDesigner = doc.CreateElement("rd", "UseGenericDesigner", rd);
            nUseGenericDesigner.InnerText = "true";
            nQuery.AppendChild(nUseGenericDesigner);
            //DataSet.Query.CommandText
            XmlNode nCommandText = doc.CreateElement("CommandText", nReport.NamespaceURI);
            nQuery.AppendChild(nCommandText);
            //DataSet.Query.DataSourceName
            XmlNode nDataSourceName = doc.CreateElement("DataSourceName", nReport.NamespaceURI);
            nDataSourceName.InnerText = "DummyDataSource";
            nQuery.AppendChild(nDataSourceName);
            nDataSet.AppendChild(nQuery);
            //DataSet.Fields
            XmlNode nFields = doc.CreateElement("Fields", nReport.NamespaceURI);
            foreach (DataColumn colDesigner in tabDesigner.Columns)
            {
                //DataSet.Fields.Field
                XmlNode nField = doc.CreateElement("Field", nReport.NamespaceURI);
                XmlAttribute aFieldName = doc.CreateAttribute("Name");
                aFieldName.InnerText = colDesigner.ColumnName;
                nField.Attributes.Append(aFieldName);
                //DataSet.Fields.Field.TypeName
                XmlNode nTypeName = doc.CreateElement("rd", "TypeName", rd);
                nTypeName.InnerText = colDesigner.DataType.ToString();
                nField.AppendChild(nTypeName);
                //DataSet.Fields.Field.DataField
                XmlNode nDataField = doc.CreateElement("DataField", nReport.NamespaceURI);
                nDataField.InnerText = colDesigner.ColumnName;
                nField.AppendChild(nDataField);
                nFields.AppendChild(nField);
            }
            nDataSet.AppendChild(nFields);
            nDataSets.AppendChild(nDataSet);
            nReport.AppendChild(nDataSets);
        }

        public static void GenLabel(XmlNode parentNode, float left, float top, string name, string caption, string HorGaps, string VertGaps, bool isTableCell)
        {
            XmlDocument doc = parentNode.OwnerDocument;

            XmlNode nLabel = doc.CreateElement("Textbox", parentNode.NamespaceURI);
            XmlAttribute aName = doc.CreateAttribute("Name");
            aName.Value = name;
            nLabel.Attributes.Append(aName);
            string temp = name.Substring(0, 2);

            if (!isTableCell)
            {
                // Left
                XmlNode nLeft = doc.CreateElement("Left", parentNode.NamespaceURI);
                nLeft.InnerText = left.ToString() + "cm";
                nLabel.AppendChild(nLeft);
                // Top
                XmlNode nTop = doc.CreateElement("Top", parentNode.NamespaceURI);
                nTop.InnerText = top.ToString() + "cm";
                nLabel.AppendChild(nTop);
                // Width
                XmlNode nWidth = doc.CreateElement("Width", parentNode.NamespaceURI);
                nWidth.InnerText = HorGaps + "cm";
                nLabel.AppendChild(nWidth);
                // Height
                XmlNode nHeight = doc.CreateElement("Height", parentNode.NamespaceURI);
                nHeight.InnerText = VertGaps + "cm";
                nLabel.AppendChild(nHeight);
            }
            // DefaultName
            XmlNode nDefaultName = doc.CreateElement("rd", "DefaultName", rd);
            nDefaultName.InnerText = name;
            nLabel.AppendChild(nDefaultName);
            // ZIndex
            XmlNode nZIndex = doc.CreateElement("ZIndex", parentNode.NamespaceURI);
            nZIndex.InnerText = "2";
            nLabel.AppendChild(nZIndex);
            // Style
            XmlNode nStyle = doc.CreateElement("Style", parentNode.NamespaceURI);
            // Style.PaddingLeft
            XmlNode nPaddingLeft = doc.CreateElement("PaddingLeft", parentNode.NamespaceURI);
            nPaddingLeft.InnerText = "2pt";
            nStyle.AppendChild(nPaddingLeft);
            // Style.PaddingTop
            XmlNode nPaddingTop = doc.CreateElement("PaddingTop", parentNode.NamespaceURI);
            nPaddingTop.InnerText = "2pt";
            nStyle.AppendChild(nPaddingTop);
            // Style.PaddingRight
            XmlNode nPaddingRight = doc.CreateElement("PaddingRight", parentNode.NamespaceURI);
            nPaddingRight.InnerText = "2pt";
            nStyle.AppendChild(nPaddingRight);
            // Style.PaddingBottom
            XmlNode nPaddingBottom = doc.CreateElement("PaddingBottom", parentNode.NamespaceURI);
            nPaddingBottom.InnerText = "2pt";
            nStyle.AppendChild(nPaddingBottom);
            if (isTableCell)
            {
                //Style.BorderStyle
                XmlNode nBorderStyle = doc.CreateElement("BorderStyle", parentNode.NamespaceURI);
                XmlNode nBorderDefault = doc.CreateElement("Default", parentNode.NamespaceURI);
                nBorderDefault.InnerText = "Solid";
                nBorderStyle.AppendChild(nBorderDefault);
                nStyle.AppendChild(nBorderStyle);

                if (temp == "hd")
                {
                    XmlNode nBackgroundColor = doc.CreateElement("BackgroundColor", parentNode.NamespaceURI);
                    nBackgroundColor.InnerText = "SteelBlue";
                    nStyle.AppendChild(nBackgroundColor);
                }
            }
            // Sytle.FontFamily
            XmlNode nFontFamily = doc.CreateElement("FontFamily", parentNode.NamespaceURI);
            nFontFamily.InnerText = "PMingLiU";
            nStyle.AppendChild(nFontFamily);
            nLabel.AppendChild(nStyle);

            // CanGrow
            XmlNode nCanGrow = doc.CreateElement("CanGrow", parentNode.NamespaceURI);
            nCanGrow.InnerText = "true";
            nLabel.AppendChild(nCanGrow);
            // Value
            XmlNode nValue = doc.CreateElement("Value", parentNode.NamespaceURI);
            nValue.InnerText = caption;
            nLabel.AppendChild(nValue);

            parentNode.AppendChild(nLabel);
        }

        public static void GenTableRows(XmlNode nParent, ReportTable rptTable, ReportParameter rptParams, string tag, ClientType dataBaseType, string dbName, string RealTableName)
        {
            XmlDocument doc = nParent.OwnerDocument;
            //TableRows
            XmlNode nTableRows = doc.CreateElement("TableRows", nParent.NamespaceURI);
            //TableRows.TableRow
            XmlNode nTableRow = doc.CreateElement("TableRow", nParent.NamespaceURI);
            GenTableCells(nTableRow, rptTable, rptParams, tag, dataBaseType, dbName, RealTableName);
            //TableRows.TableRow.Height
            XmlNode nHeight = doc.CreateElement("Height", nParent.NamespaceURI);
            nHeight.InnerText = rptParams.VertGaps + "cm";
            nTableRow.AppendChild(nHeight);
            nTableRows.AppendChild(nTableRow);
            nParent.AppendChild(nTableRows);
        }

        public static void GenTableCells(XmlNode nParent, ReportTable rptTable, ReportParameter rptParams, string tag, ClientType databaseType, string dbName, string RealTableName)
        {
            DataTable tabDataDic = null;
            //if (tag == "hd")
            //{
            tabDataDic = GetDDTable(databaseType, dbName, RealTableName);
            //}
            XmlDocument doc = nParent.OwnerDocument;
            //TableCells
            XmlNode nTableCells = doc.CreateElement("TableCells", nParent.NamespaceURI);
            List<ReportColumn> lstGCols = GetGroupConditionRptColumns(rptTable);
            foreach (ReportColumn rptCol in rptTable.ReportColumns)
            {
                if (!lstGCols.Contains(rptCol))
                {
                    lstGCols.Add(rptCol);
                }
            }
            foreach (ReportColumn rptCol in lstGCols)
            {
                //TableCells.TableCell
                XmlNode nTableCell = doc.CreateElement("TableCell", nParent.NamespaceURI);
                //TableCells.TableCell.ReportItems
                XmlNode nReportItems = doc.CreateElement("ReportItems", nParent.NamespaceURI);
                //TableCell Expression
                if (tag == "hd")
                {
                    GenLabel(nReportItems, 0, 0, tag + rptCol.ColumnName, GetFieldCaption(tabDataDic, rptCol.ColumnName), rptParams.HorGaps, rptParams.VertGaps, true);
                }
                else if (tag == "cnt")
                {
                    if (!rptCol.IsGroupCondition)
                    {

                        string EditMask = GetFieldEditMask(tabDataDic, rptCol.ColumnName);
                        GenLabel(nReportItems, 0, 0, tag + rptCol.ColumnName, EditMask, rptParams.HorGaps, rptParams.VertGaps, true);
                    }
                    else
                    {
                        GenLabel(nReportItems, 0, 0, tag + rptCol.ColumnName, "", rptParams.HorGaps, rptParams.VertGaps, true);
                    }
                }
                else if (tag == "hdg")
                {
                    if (rptCol.IsGroupCondition)
                    {

                        string EditMask = GetFieldEditMask(tabDataDic, rptCol.ColumnName);
                        GenLabel(nReportItems, 0, 0, tag + rptCol.ColumnName, EditMask, rptParams.HorGaps, rptParams.VertGaps, true);
                    }
                    else
                    {
                        GenLabel(nReportItems, 0, 0, tag + rptCol.ColumnName, "", rptParams.HorGaps, rptParams.VertGaps, true);
                    }
                }
                nTableCell.AppendChild(nReportItems);
                nTableCells.AppendChild(nTableCell);
            }
            nParent.AppendChild(nTableCells);
        }

        public static void GenTableColumns(XmlNode nParent, int columnCount, string HorGaps)
        {
            XmlDocument doc = nParent.OwnerDocument;
            //TableColumns
            XmlNode nTableColumns = doc.CreateElement("TableColumns", nParent.NamespaceURI);
            for (int i = 0; i < columnCount; i++)
            {
                //TableColumns.TableColumn
                XmlNode nTableColumn = doc.CreateElement("TableColumn", nParent.NamespaceURI);
                //TableColumns.TableColumn.Width
                XmlNode nWidth = doc.CreateElement("Width", nParent.NamespaceURI);
                nWidth.InnerText = HorGaps + "cm";
                nTableColumn.AppendChild(nWidth);
                nTableColumns.AppendChild(nTableColumn);
            }
            nParent.AppendChild(nTableColumns);
        }

        public static void GenTableGroups(XmlNode nParent, ReportTable rptTable, ReportParameter rptParams, string RealTableName)
        {
            XmlDocument doc = nParent.OwnerDocument;
            //TableGroups
            XmlNode nTableGroups = doc.CreateElement("TableGroups", nParent.NamespaceURI);
            //TableGroups.TableGroup
            XmlNode nTableGroup = doc.CreateElement("TableGroup", nParent.NamespaceURI);
            //TableGroups.TableGroup.Header
            XmlNode nHeader = doc.CreateElement("Header", nParent.NamespaceURI);
            GenTableRows(nHeader, rptTable, rptParams, "hdg", ClientType.ctMsSql, string.Empty, RealTableName);
            nTableGroup.AppendChild(nHeader);

            //TableGroups.TableGroup.Grouping
            XmlNode nGrouping = doc.CreateElement("Grouping", nParent.NamespaceURI);
            XmlAttribute aGroupingName = doc.CreateAttribute("Name");
            aGroupingName.Value = "table1_Group1";
            nGrouping.Attributes.Append(aGroupingName);
            //TableGroups.TableGroup.Grouping.PageBreakAtEnd
            XmlNode nPageBreakAtEnd = doc.CreateElement("PageBreakAtEnd", nParent.NamespaceURI);
            nPageBreakAtEnd.InnerText = "true";
            nGrouping.AppendChild(nPageBreakAtEnd);
            //TableGroups.TableGroup.Grouping.GroupExpressions
            XmlNode nGroupExpressions = doc.CreateElement("GroupExpressions", nParent.NamespaceURI);
            List<ReportColumn> lstGroupConditionFields = GetGroupConditionRptColumns(rptTable);
            foreach (ReportColumn cdtField in lstGroupConditionFields)
            {
                //TableGroups.TableGroup.Grouping.GroupExpressions.GroupExpression
                XmlNode nGroupExpression = doc.CreateElement("GroupExpression", nParent.NamespaceURI);
                nGroupExpression.InnerText = "=Fields!" + cdtField.ColumnName + ".Value";
                nGroupExpressions.AppendChild(nGroupExpression);
            }
            nGrouping.AppendChild(nGroupExpressions);
            nTableGroup.AppendChild(nGrouping);
            nTableGroups.AppendChild(nTableGroup);
            nParent.AppendChild(nTableGroups);
        }

        private static XmlNode GetStartXPathNode(XmlDocument doc, XmlNamespaceManager nsmgr, bool isLabelStyle, bool isMasterDetail)
        {
            string path = "";

            if (isLabelStyle)
            {
                if (isMasterDetail)
                {
                    path = "ms:Report/ms:Body/ms:ReportItems/ms:List[@Name='lstContainer']/ms:ReportItems/ms:Rectangle[@Name='rectContainer']/ms:ReportItems";
                }
                else
                {
                    path = "ms:Report/ms:Body/ms:ReportItems/ms:List[@Name='lstContainer']/ms:ReportItems/ms:Rectangle[@Name='rectContainer']";
                }
            }
            else
            {
                if (isMasterDetail)
                {
                    path = "ms:Report/ms:Body/ms:ReportItems/ms:Rectangle[@Name='rectContainer']/ms:ReportItems";
                }
                else
                {
                    path = "ms:Report/ms:Body/ms:ReportItems/ms:Rectangle[@Name='rectContainer']";
                }
            }
            return doc.SelectSingleNode(path, nsmgr);
        }

        private static XmlNode GetTitleNode(XmlDocument doc, XmlNamespaceManager nsmgr, bool isLabelStyle)
        {
            string path = "ms:Report/ms:PageHeader/ms:ReportItems";

            XmlNode startNode = doc.SelectSingleNode(path, nsmgr);
            return startNode.SelectSingleNode("ms:Textbox[@Name='txtTitle']", nsmgr);
        }

        private static XmlNode GetRptWidthNode(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            XmlNode startNode = doc.SelectSingleNode("ms:Report/ms:Width", nsmgr);
            return startNode;
        }

        public static float GetRptWidth(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            XmlNode nRptWidth = GetRptWidthNode(doc, nsmgr);
            return ConvertCMToFloat(nRptWidth);
        }

        private static XmlNode GetRptHeightNode(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            XmlNode startNode = doc.SelectSingleNode("ms:Report/ms:Body/ms:Height", nsmgr);
            return startNode;
        }

        public static float GetRptHeight(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            XmlNode nRptHeight = GetRptHeightNode(doc, nsmgr);
            return ConvertCMToFloat(nRptHeight);
        }

        private static void GenReportBodyHeight(XmlDocument doc, float curTop, XmlNamespaceManager nsmgr, bool isMaster)
        {
            string path = "ms:Report/ms:Body/ms:Height";
            XmlNode HeightNode = doc.SelectSingleNode(path, nsmgr);
            if (isMaster)
            {
                HeightNode.InnerText = Convert.ToString(curTop + 4) + "cm";
                path = "ms:Report/ms:Body/ms:ReportItems/ms:List[@Name='lstContainer']/ms:ReportItems/ms:Rectangle[@Name='rectContainer']/ms:ReportItems/ms:Subreport[@Name='subreport1']/ms:Top";
                XmlNode SubTopNode = doc.SelectSingleNode(path, nsmgr);
                SubTopNode.InnerText = Convert.ToString(curTop + 1) + "cm";
            }
            else
            {
                HeightNode.InnerText = Convert.ToString(curTop + 0.5) + "cm";
            }
        }

        private static float ConvertCMToFloat(XmlNode node)
        {
            string cm = node.InnerText;
            return Convert.ToSingle(cm.Remove(cm.IndexOf("cm"), 2));
        }

        private static float GetDataRegionTop(XmlDocument doc, XmlNamespaceManager nsmgr, bool isLabelStyle, bool isMasterDetail)
        {
            //XmlNode nTitle = GetTitleNode(doc, nsmgr, isLabelStyle);
            //XmlNode nHeight = nTitle.SelectSingleNode("ms:Height", nsmgr);
            //return ConvertCMToFloat(nTitle.SelectSingleNode("ms:Top", nsmgr)) + ConvertCMToFloat(nTitle.SelectSingleNode("ms:Height", nsmgr)) + 0.5f;

            XmlNode nLine1Top = GetStartXPathNode(doc, nsmgr, isLabelStyle, isMasterDetail).SelectSingleNode("ms:Line[@Name='line1']/ms:Top", nsmgr);
            return ConvertCMToFloat(nLine1Top) + 0.25f;
        }

        public static string GetServerPath()
        {
            string srvPath = EEPRegistry.Server + "\\"; 
            return srvPath;
        }

        public static string GetWebClientPath()
        {
            string wctPath = EEPRegistry.WebClient + "\\";
            return wctPath;
        }

        public static string GetWinClientPath(string projName)
        {
            string ctPath = EEPRegistry.Client;
            string ct = "";
            if (ctPath.ToLower().IndexOf("eepnetclient") != -1)
                ct = "eepnetclient";
            else if (ctPath.ToLower().IndexOf("eepnetflclient") != -1)
                ct = "eepnetflclient";
            if (!string.IsNullOrEmpty(ct))
                ctPath = ctPath.Substring(0, ctPath.ToLower().IndexOf(ct)) + projName + "\\";

            return ctPath;
        }

        public static DataTable GetDDTable(ClientType databaseType, string dbName, string tableName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SystemFile.DBFile);
            XmlNode nDataBase = doc.SelectSingleNode("InfolightDB/DataBase");
            foreach (XmlNode node in nDataBase.ChildNodes)
            {
                if (node.Name == dbName)
                {
                    string conString = node.Attributes["String"].Value;
                    string password = WzdUtils.GetPwdString(node.Attributes["Password"].Value);
                    if ((conString.Length > 0) && (password.Length > 0) && password != String.Empty)
                    {
                        if (conString[conString.Length - 1] != ';')
                        {
                            conString = conString + ";Password=" + password;
                        }
                        else
                        {
                            conString = conString + "Password=" + password;
                        }
                    }
                    //Connection
                    DbConnection con = WzdUtils.AllocateConnection(dbName, databaseType, false);
                    //Command
                    InfoCommand cmd = new InfoCommand(databaseType);
                    cmd.Connection = con;
                    tableName = WzdUtils.RemoveQuote(tableName, databaseType);
                    cmd.CommandText = "Select * from COLDEF where TABLE_NAME = '" + tableName + "'";
                    //Adapter
                    IDbDataAdapter adapter = WzdUtils.AllocateDataAdapter(databaseType);
                    adapter.SelectCommand = cmd.GetInternalCommand();
                    DataTable tab = new DataTable();
                    WzdUtils.FillDataAdapter(databaseType, adapter, tab);
                    return tab;
                }
            }

            return null;
        }

        public static string GetFieldCaption(DataTable tabDataDic, string fieldName)
        {
            if (tabDataDic != null && tabDataDic.Rows.Count > 0)
            {
                foreach (DataRow row in tabDataDic.Rows)
                {
                    if (row["FIELD_NAME"] != null && row["CAPTION"] != null 
                        && row["FIELD_NAME"].ToString().ToLower() == fieldName.ToLower() 
                        && !string.IsNullOrEmpty(row["CAPTION"].ToString()))
                    {
                        return row["CAPTION"].ToString();
                    }
                }
            }
            return fieldName;
        }

        public static string GetFieldEditMask(DataTable tabDataDic, string fieldName)
        {
            if (tabDataDic != null && tabDataDic.Rows.Count > 0)
            {
                foreach (DataRow row in tabDataDic.Rows)
                {
                    if (row["FIELD_NAME"] != null && row["FIELD_NAME"].ToString().ToLower() == fieldName.ToLower())
                    {
                        if (row["EDITMASK"] != null && !string.IsNullOrEmpty(row["EDITMASK"].ToString()))
                        {
                            return "=Format(Fields!" + fieldName + ".Value,\"" + row["EDITMASK"].ToString() + "\")";
                        }
                        else
                        {
                            if (row["FIELD_TYPE"] != null)
                            {
                                switch (row["FIELD_TYPE"].ToString().ToLower())
                                {
                                    case "datetime":
                                        return "=Format(Fields!" + fieldName + ".Value,\"d\")";
                                    case "money":
                                        return "=Format(Fields!" + fieldName + ".Value,\"C2\")";
                                }
                            }
                        }
                    }
                }
            }
            return "=Fields!" + fieldName + ".Value";
        }
    }

    public static class WebClientCreator
    {
        public static bool GetForm(ProjectItem projItem, string FormName, bool isMasterDetails)
        {
            string TemplatePath = GetTemplatePath();           
            if (TemplatePath == "")
            {
                MessageBox.Show("Cannot find WebTemplate path: {0}", TemplatePath);
                return false;
            }
            if (projItem != null)
            {
                string BaseFormName = GetBaseForm(isMasterDetails);
                foreach (ProjectItem PI in projItem.ProjectItems)
                {
                    if ( (FormName + ".aspx" == PI.Name) ||
                        (FormName + ".aspx.resx" == PI.Name) ||
                        (FormName + ".aspx.vi-VN.resx" == PI.Name))
                    {
                        DialogResult dr = MessageBox.Show("There is another File which name is " + FormName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            string Path = PI.get_FileNames(0);

                            PI.Name = Guid.NewGuid().ToString();
                            
                            PI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");

                            PI.Delete();

                            File.Delete(Path);
                        }
                        else
                        {
                            return false;
                        }
                    }
                } 

                //Copy Template
                ProjectItem TempPI = projItem;
                projItem = TempPI.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + BaseFormName + ".aspx");
                projItem.Name = FormName + ".aspx";
                ProjectItem P1 = TempPI.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + BaseFormName + ".aspx.resx");
                P1.Name = FormName + ".aspx.resx";
                ProjectItem P2 = TempPI.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + BaseFormName + ".aspx.vi-VN.resx");
                P2.Name = FormName + ".aspx.vi-VN.resx";
                //FResxFileName = P2.Name;              
            }
            return true;
        }

        private static string GetBaseForm(bool isMasterDetails)
        {

            if (isMasterDetails)
            {
                return "WMasterDetailReport";
            }
            else
            {
                return "WebReport";
            }
        }

        private static string GetTemplatePath()
        {
            string TemplatePath = EEPRegistry.WebClient + "\\Template";
            return TemplatePath;
        }

        public static void SaveWebDataSet(InfoDataSet WizardDataSet, InfoDataSet WizardDetailDataSet, ProjectItem projItem)
        {
            string Path = projItem.get_FileNames(0);
            string FileName = Path + ".vi-VN.resx";

            string keyName = "WebDataSets";

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileName);
            XmlNode aNode = xDoc.DocumentElement.FirstChild;
            while (aNode != null)
            {
                if (aNode.InnerText.Contains(keyName))
                {
                    int x, y;
                    String temp;
                    string tempInnerText = aNode.InnerText;
                    string headstr = tempInnerText.Substring(0, tempInnerText.IndexOf("<WebDataSets>") + 13);
                    string footstr = tempInnerText.Substring(tempInnerText.LastIndexOf("</WebDataSet>"), tempInnerText.Length - tempInnerText.LastIndexOf("</WebDataSet>"));
                    tempInnerText = tempInnerText.Substring(19, tempInnerText.Length - 50);
                    string[] InnerTextarr = new string[2];
                    if (tempInnerText.IndexOf("</WebDataSet>") != -1)
                    {
                        InnerTextarr[0] = tempInnerText.Substring(0, tempInnerText.IndexOf("</WebDataSet>"));
                        InnerTextarr[1] = tempInnerText.Substring(tempInnerText.IndexOf("</WebDataSet>") + 13, tempInnerText.Length - tempInnerText.IndexOf("</WebDataSet>") - 13);
                    }
                    else
                    {
                        InnerTextarr[0] = tempInnerText;
                    }
                    for (int i = 0; i < 2; )
                    {
                        x = InnerTextarr[i].IndexOf("<Active>");
                        y = InnerTextarr[i].IndexOf("</Active>");
                        temp = InnerTextarr[i].Substring(x, (y - x) + 9);
                        InnerTextarr[i] = InnerTextarr[i].Replace(temp, "<Active>" + WizardDataSet.Active + "</Active>");

                        x = InnerTextarr[i].IndexOf("<PacketRecords>");
                        y = InnerTextarr[i].IndexOf("</PacketRecords>");
                        temp = InnerTextarr[i].Substring(x, (y - x) + 16);
                        InnerTextarr[i] = InnerTextarr[i].Replace(temp, "<PacketRecords>" + WizardDataSet.PacketRecords + "</PacketRecords>");

                        if (i == 0)
                        {
                            x = InnerTextarr[i].IndexOf("<RemoteName>");
                            y = InnerTextarr[i].IndexOf("</RemoteName>");
                            temp = InnerTextarr[i].Substring(x, (y - x) + 13);
                            InnerTextarr[i] = InnerTextarr[i].Replace(temp, "<RemoteName>" + WizardDataSet.RemoteName + "</RemoteName>");
                        }
                        else
                        {
                            x = InnerTextarr[i].IndexOf("<RemoteName>");
                            y = InnerTextarr[i].IndexOf("</RemoteName>");
                            temp = InnerTextarr[i].Substring(x, (y - x) + 13);
                            InnerTextarr[i] = InnerTextarr[i].Replace(temp, "<RemoteName>" + WizardDetailDataSet.RemoteName + "</RemoteName>");
                        }

                        x = InnerTextarr[i].IndexOf("<ServerModify>");
                        y = InnerTextarr[i].IndexOf("</ServerModify>");
                        temp = InnerTextarr[i].Substring(x, (y - x) + 15);
                        InnerTextarr[i] = InnerTextarr[i].Replace(temp, "<ServerModify>" + WizardDataSet.ServerModify + "</ServerModify>");

                        if (tempInnerText.IndexOf("</WebDataSet>") == -1)
                        {
                            i = 2;
                        }
                        else
                        {
                            i++;
                        }
                    }                    
                    aNode.InnerText = headstr + InnerTextarr[0];
                    if (tempInnerText.IndexOf("</WebDataSet>") != -1)
                    {
                        aNode.InnerText += "</WebDataSet>" + InnerTextarr[1];
                    }
                    aNode.InnerText += footstr;
                    break;
                }
                aNode = aNode.NextSibling;
            }
            xDoc.Save(FileName);
        }        

        public static void WebCreateXSD(IDesignerHost FDesignerHost, ClientParam cParam, WebClientParam wecParam, Project proj)
        {
            WebDataSet aWebDataSet = new WebDataSet();

            ProjectItem webformDir = ReportCreator.FindProjectItem(proj, wecParam.FolderName);
            
            if (aWebDataSet != null)
            {
                aWebDataSet.SetWizardDesignMode(true);
                aWebDataSet.RemoteName = cParam.ProviderName;
                aWebDataSet.PacketRecords = 100;
                aWebDataSet.Active = true;
                
                String s;
                s = EEPRegistry.WebClient;

                string filePath = s + "\\" + wecParam.FolderName + "\\";
                bool CreateFileSucess = true;
                string fileName = "";
                try
                {
                    fileName = filePath + cParam.FormName + ".xsd";
                    aWebDataSet.RealDataSet.WriteXmlSchema(fileName);
                }
                catch
                {
                    CreateFileSucess = false;
                    MessageBox.Show("Failed to create xsd file!");
                }
                finally
                {
                    if (CreateFileSucess && File.Exists(fileName))
                    {
                        webformDir.ProjectItems.AddFromFile(fileName);
                    }
                    if (aWebDataSet != null)
                    {
                        aWebDataSet.Dispose();
                    }
                }
            }
        }

        public static void WriteWebDataSourceHTML(Window FDesignWindow, ClientParam cParam, ProjectItem projItem)
        {
            String FileName = FDesignWindow.Document.FullName;
            FDesignWindow.Close(vsSaveChanges.vsSaveChangesYes);

            String UpdateHTML = "";

            UpdateHTML = String.Format("<rsweb:reportviewer id=\"ReportViewer1\" runat=\"server\" width=\"100%\" Font-Names=\"Verdana\""
                + " Font-Size=\"8pt\" Height=\"400px\">"
                + "<LocalReport ReportPath=\"\">"
                + "<DataSources>"
                + "<LocalReport ReportPath=\"{1}\">"
                + "<rsweb:ReportDataSource DataSourceId=\"Master\" Name={0} />"
                + "</DataSources>"
                + "</LocalReport>"
                + "</rsweb:reportviewer>",
                "\"NewDataSet_" + cParam.ProviderName.Substring(cParam.ProviderName.IndexOf('.') + 1, cParam.ProviderName.Length - cParam.ProviderName.IndexOf('.') - 1) + "\"", cParam.FolderName + @"\" + cParam.RptFileName + ".rdlc");

            //Start Update Process
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName, Encoding.Default);
            String Context = SR.ReadToEnd();
            SR.Close();

            //Update HTML
            int x, y;
            string temp = "";
            x = Context.IndexOf("<rsweb");
            y = Context.IndexOf("</rsweb");
            temp = Context.Substring(x, (y - x) + 21);
            Context = Context.Replace(temp, UpdateHTML);

            //Page Title
            Context = Context.Replace("<title>Untitled Page</title>", "<title>" + cParam.FormTitle + "</title>");

            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
          
            FDesignWindow = projItem.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow.Activate();
        }        

        internal static void GenReportViewProperty(Window FDesignWindow, ProjectItem reportDir, bool isMasterDetails, string RootName, string RptName)
        {
            String FileName = FDesignWindow.Document.FullName;
            string FormName = FileName.Substring(FileName.LastIndexOf("\\") + 1, FileName.Length - FileName.LastIndexOf("\\") - 1);
            FDesignWindow.Close(vsSaveChanges.vsSaveChangesYes);
           //Start Update Process
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName, Encoding.Default);
            String Context = SR.ReadToEnd();
            SR.Close();            

            //Gen Report Property
            if (isMasterDetails)
            {
                Context = Context.Replace("<LocalReport ReportPath=\"\">", "<LocalReport ReportPath=\"" + RootName + "\\" + RptName + ".rdlc\" OnSubreportProcessing=\"SubreportProcessing\">");
            }
            else
            {
                Context = Context.Replace("<LocalReport ReportPath=\"\">", "<LocalReport ReportPath=\"" + RootName + "\\" + RptName + ".rdlc\">");
            }

            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
            reportDir = ReportCreator.FindProjectItem(reportDir, FormName);
            FDesignWindow = reportDir.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow.Activate();
        }             
    }

    public static class WinClientCreator
    {
        public static void GenDataSet(Project GlobalProject, ClientParam cParam, WinClientParam winParam, IDesignerHost FDesignerHost)
        {
            InfoDataSet FDataSet = FDesignerHost.CreateComponent(typeof(InfoDataSet), "id" + cParam.TableName) as InfoDataSet;
            FDataSet.RemoteName = cParam.ProviderName;
            FDataSet.Active = true;
            FDataSet.AlwaysClose = true;

            InfoBindingSource MainBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + cParam.TableName) as InfoBindingSource;
            MainBindingSource.DataSource = FDataSet;
            MainBindingSource.DataMember = FDataSet.RealDataSet.Tables[0].TableName;

            WinCreateXSD(GlobalProject, FDataSet, cParam, winParam);
        }

        public static void WinCreateXSD(Project GlobalProject, InfoDataSet FDataSet, ClientParam cParam, WinClientParam winParam)
        {            
            string filePath = winParam.OutputPath + "\\" + winParam.PackageName + "\\";
            bool CreateFileSucess = true;
            string fileName = "";
            try
            {
                fileName = filePath + FDataSet.Site.Name + ".xsd";
                FDataSet.RealDataSet.WriteXmlSchema(fileName);
            }
            catch
            {
                CreateFileSucess = false;
                MessageBox.Show("Failed to create xsd file!");
            }
            finally
            {
                if (CreateFileSucess && File.Exists(fileName))
                {
                    GlobalProject.ProjectItems.AddFromFile(fileName);
                }
            }
        }

        public static void GetWinQuery(Form FRootForm, ClientParam cParam, List<TBlockFieldItem> list)
        {
            InfoBindingSource ibs = FRootForm.Container.Components["ibs" + cParam.TableName] as InfoBindingSource;
            foreach (TBlockFieldItem aFieldItem in list)
            {
                CreateWinQueryField(FRootForm, aFieldItem, "", ibs);
            }
        }

        private static void CreateWinQueryField(Form FRootForm, TBlockFieldItem aFieldItem, string Range, InfoBindingSource ibs)
        {
            ClientQuery aClientQuery = FRootForm.Container.Components["clientQuery1"] as ClientQuery;
            if (aClientQuery != null)
            {
                if (ibs != null && aClientQuery != null)
                    aClientQuery.BindingSource = ibs;
                if (aFieldItem.QueryMode != null && (aFieldItem.QueryMode.ToUpper() == "NORMAL" || aFieldItem.QueryMode.ToUpper() == "RANGE"))
                {
                    QueryColumns qColumn = new QueryColumns();
                    qColumn.Column = aFieldItem.DataField;
                    qColumn.Caption = aFieldItem.Description;
                    if (qColumn.Caption == "")
                        qColumn.Caption = aFieldItem.DataField;
                    if (aFieldItem.QueryMode.ToUpper() == "NORMAL")
                    {
                        if (aFieldItem.DataType == typeof(DateTime))
                            qColumn.Operator = "=";
                        if (aFieldItem.DataType == typeof(int) || aFieldItem.DataType == typeof(float) ||
                            aFieldItem.DataType == typeof(double) || aFieldItem.DataType == typeof(Int16))
                            qColumn.Operator = "=";
                        if (aFieldItem.DataType == typeof(String))
                            qColumn.Operator = "%";
                    }
                    if (aFieldItem.QueryMode.ToUpper() == "RANGE")
                    {
                        qColumn.Condition = "And";
                        if (Range == "")
                        {
                            qColumn.Operator = "<=";
                            CreateWinQueryField(FRootForm, aFieldItem, ">=", null);
                        }
                        else
                        {
                            qColumn.Operator = Range;
                        }
                    }
                    switch (aFieldItem.ControlType.ToUpper())
                    {                        
                        case "DATETIMEBOX":
                            qColumn.ColumnType = "ClientQueryCalendarColumn";
                            break;
                        case "CHECKBOX":
                            qColumn.ColumnType = "ClientQueryCheckBoxColumn";
                            break;
                        default:
                            qColumn.ColumnType = "ClientQueryTextBoxColumn";
                            break;
                    }
                    aClientQuery.Columns.Add(qColumn);
                }
            }
        }

        public static void GetReportViewProperty(Form FRootForm, Project proj, bool isMasterDetail,string TableName, string PackageName, string FormName, string RptName, string ChildTable, String language)
        {
            string projdir = proj.FullName.Substring(0, proj.FullName.LastIndexOf("\\") + 1) + FormName + ".Designer" + language;
            System.IO.StreamReader SR1 = new System.IO.StreamReader(projdir);
            string Context1 = SR1.ReadToEnd();
            SR1.Close();

            if (language == String.Empty || language == ".cs")
            {
                string updatestr = "this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;\r\n"
                    + "this.reportViewer1.LocalReport.ReportEmbeddedResource = \"" + PackageName + "." + RptName + ".rdlc\";\r\n";

                int x = Context1.IndexOf("this.reportViewer1.Dock");
                int y = Context1.IndexOf("this.reportViewer1.Location");
                string temp = Context1.Substring(x, y - x);

                Context1 = Context1.Replace(temp, updatestr);

                System.IO.FileStream Filefs1 = new System.IO.FileStream(projdir, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                System.IO.StreamWriter SW1 = new System.IO.StreamWriter(Filefs1, Encoding.UTF8);
                SW1.Write(Context1);
                SW1.Close();
                Filefs1.Close();


                string dir = proj.FullName.Substring(0, proj.FullName.LastIndexOf("\\") + 1);
                dir += FormName + language;
                System.IO.StreamReader SR = new System.IO.StreamReader(dir, Encoding.UTF8);
                string Context = SR.ReadToEnd();
                SR.Close();

                Context = Context.Replace("new ReportDataSource(\"NewDataSet_\", this.DataSet.RealDataSet.Tables[0])", "new ReportDataSource(\"NewDataSet_" + TableName + "\", this.id" + TableName + ".RealDataSet.Tables[0])");

                if (isMasterDetail)
                {
                    Context = Context.Replace("\"NewDataSet_\", Detail", "\"NewDataSet_" + (ChildTable.Split(','))[0] + "\", this.id" + TableName + ".RealDataSet.Tables[1]");
                }

                System.IO.FileStream Filefs = new System.IO.FileStream(dir, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
                SW.Write(Context);
                SW.Close();
                Filefs.Close();
            }
            else if (language == ".vb")
            {
                string updatestr = "Me.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill\r\n"
    + "Me.reportViewer1.LocalReport.ReportEmbeddedResource = \"" + PackageName + "." + RptName + ".rdlc\"\r\n";

                int x = Context1.IndexOf("Me.reportViewer1.Dock");
                int y = Context1.IndexOf("Me.reportViewer1.Location");
                string temp = Context1.Substring(x, y - x);

                Context1 = Context1.Replace(temp, updatestr);

                System.IO.FileStream Filefs1 = new System.IO.FileStream(projdir, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                System.IO.StreamWriter SW1 = new System.IO.StreamWriter(Filefs1, Encoding.UTF8);
                SW1.Write(Context1);
                SW1.Close();
                Filefs1.Close();


                string dir = proj.FullName.Substring(0, proj.FullName.LastIndexOf("\\") + 1);
                dir += FormName + language;
                System.IO.StreamReader SR = new System.IO.StreamReader(dir, Encoding.UTF8);
                string Context = SR.ReadToEnd();
                SR.Close();

                Context = Context.Replace("New Microsoft.Reporting.WinForms.ReportDataSource(\"NewDataSet_\", Me.DataSet.RealDataSet.Tables(0))", "New Microsoft.Reporting.WinForms.ReportDataSource(\"NewDataSet_" + TableName + "\", Me.id" + TableName + ".RealDataSet.Tables(0))");

                if (isMasterDetail)
                {
                    Context = Context.Replace("\"NewDataSet_\", Detail", "\"NewDataSet_" + (ChildTable.Split(','))[0] + "\", Me.id" + TableName + ".RealDataSet.Tables(1)");
                }

                System.IO.FileStream Filefs = new System.IO.FileStream(dir, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
                SW.Write(Context);
                SW.Close();
                Filefs.Close();


                dir = proj.FullName.Substring(0, proj.FullName.LastIndexOf("\\") + 1);
                dir += "My Project\\Application.Designer.vb";
                System.IO.StreamReader SR2 = new System.IO.StreamReader(dir, Encoding.UTF8);
                String Context2 = SR2.ReadToEnd();
                SR2.Close();

                Context2 = Context2.Replace("TAG_FORMNAME", FRootForm.Name);

                System.IO.FileStream Filefs2 = new System.IO.FileStream(dir, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                System.IO.StreamWriter SW2 = new System.IO.StreamWriter(Filefs2, Encoding.UTF8);
                SW2.Write(Context2);
                SW2.Close();
                Filefs2.Close();
            }
        }
    }
}
