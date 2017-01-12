using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;

namespace OfficeTools
{
    /// <summary>
    /// The class to process excel file in xml mode
    /// </summary>
    public class ExcelAutomationXml : OfficeAutomation
    {
        /// <summary>
        /// Create a new instance of ExcelAutomationXml
        /// </summary>
        public ExcelAutomationXml()
        {
            ProgressInfo = 0;
            ProgressCount = 0;
            TagInfo = string.Empty;
            Log = new PlateLog();
        }

        /// <summary>
        /// Create a new instance of ExcelAutomationXml
        /// </summary>
        /// <param name="filename">Name of excel file</param>
        /// <param name="plate">The Excelplate whose defination is used</param>
        public ExcelAutomationXml(string filename, IOfficePlate plate)
            : this()
        {
            FileName = filename;
            Plate = plate;
            ErrorReport = plate.MarkException;
        }

        /// <summary>
        /// Run Excel Automation
        /// </summary>
        public override void Run()
        {
            if (FileName.Trim().Length == 0)
            {
                throw new Exception("Property of FileName hasn't be initialized");
            }
            if (Plate == null)
            {
                throw new Exception("Property of Plate hasn't be initialized");
            }

            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(FileName);
            }
            catch
            {
                throw new Exception("Excel file is not recognized as a xml");
            }
            XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xml.NameTable);
            xmlmgr.AddNamespace("sheet", "urn:schemas-microsoft-com:office:spreadsheet");
            string rowname = "/sheet:Workbook/sheet:Worksheet/sheet:Table/sheet:Row";
            XmlNodeList nodelist = xml.SelectNodes(rowname, xmlmgr);
            this.ProgressCount = nodelist.Count;
            Log.StartLog(FileName + ".log", "Excel Plate");

            for (int i = 0; i < nodelist.Count; i++)
            {
                this.TagInfo = string.Format("Process row:{0}/{1}", i.ToString(), nodelist.Count.ToString());
                this.ProgressInfo = i;
                XmlNode nodeRow = nodelist[i];
                int dsmaxcount = int.MaxValue;
                foreach (XmlNode xn in nodeRow.ChildNodes)
                {
                    string strvalue = xn.InnerText.Trim();
                    if (strvalue.StartsWith("$") && strvalue.Length > 1)
                    {
                        strvalue = strvalue.Substring(1);
                        int dscount = GetDataSourceRowCount(ref strvalue);
                        if (dscount >= 0)
                        {
                            xn.InnerXml = xn.InnerXml.Replace(xn.InnerText, strvalue);
                            dsmaxcount = Math.Min(dsmaxcount, dscount);
                        }
                    }
                }
                if (dsmaxcount == int.MaxValue)
                {
                    ProcessTag(nodeRow);
                }
                else
                {
                    CopyRow(dsmaxcount, nodeRow);
                }
            }
            Log.EndLog();
            this.TagInfo = "Process Finished";
            this.ProgressInfo = this.ProgressCount;
            xml.Save(FileName);
        }

        /// <summary>
        /// Copy the row in worksheet
        /// </summary>
        /// <param name="count">The times of copy</param>
        /// <param name="node">The node of row</param>
        protected void CopyRow(int count, XmlNode node)
        {
            for (int i = 0; i < count; i++)
            {
                this.TagInfo = string.Format("Insert new row:{0}/{1}", i.ToString(), count.ToString());
                XmlNode newNode = node.CloneNode(true);
                if (i > 0 && newNode.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"] != null) //修正有index属性的row复制时的问题
                {
                    newNode.Attributes.Remove(newNode.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"]);
                }
                foreach (XmlNode xn in newNode.ChildNodes)
                {
                    string strvalue = xn.InnerText.Trim();
                    if (strvalue.StartsWith("$") && strvalue.Contains("{0}"))
                    {
                        xn.InnerXml = xn.InnerXml.Replace(xn.InnerText, string.Format(strvalue, i.ToString()));
                    }
                }
                ProcessTag(newNode);
                node.ParentNode.InsertBefore(newNode, node);
            }
            XmlAttribute rowcount = node.ParentNode.Attributes["ExpandedRowCount", "urn:schemas-microsoft-com:office:spreadsheet"];
            if (rowcount != null)
            {
                int intrcount = Convert.ToInt32(rowcount.Value);
                intrcount += count - 1;
                rowcount.Value = intrcount.ToString();
            }
            node.ParentNode.RemoveChild(node);
        }

        /// <summary>
        /// Process tags of the whole row
        /// </summary>
        /// <param name="nodeRow">The node of row</param>
        protected void ProcessTag(XmlNode nodeRow)
        {
            foreach (XmlNode xn in nodeRow.ChildNodes)
            {
                string strvalue = xn.InnerText.Trim();
                if (strvalue.StartsWith("$") && strvalue.Length > 1)
                {
                    this.TagInfo = "Process tag:" + strvalue;
                    object[] obj = Automation.Run(Plate, strvalue.Substring(1));
                    if ((PlateException)obj[0] == PlateException.None)
                    {
                        xn.InnerXml = xn.InnerXml.Replace(xn.InnerText, obj[1].ToString().Replace("&", "&&").Replace(">", "&gt").Replace("<", "&lt"));
                    }
                    else
                    {
                        if (ErrorReport)
                        {
                            xn.InnerXml = xn.InnerXml.Replace(xn.InnerText, string.Format("$Error: {0}", obj[0].ToString()));
                        }
                        Log.WriteErrorInfo(obj[0].ToString(), strvalue, obj[1].ToString());
                    }
                }
            }
        }
    }

    /// <summary>
    /// The class to process excel file in com mode
    /// </summary>
    public class ExcelAutomationCom : OfficeAutomation
    {
        /// <summary>
        /// Create a new instance of ExcelAutomationCom
        /// </summary>
        public ExcelAutomationCom()
        {
            ProgressInfo = 0;
            ProgressCount = 0;
            TagInfo = string.Empty;
            Log = new PlateLog();
        }

        /// <summary>
        /// Create a new instance of ExcelAutomationCom
        /// </summary>
        /// <param name="filename">Name of excel file</param>
        /// <param name="plate">The Excelplate whose defination is used</param>
        public ExcelAutomationCom(string filename, IOfficePlate plate)
            : this()
        {
            FileName = filename;
            Plate = plate;
            ErrorReport = plate.MarkException;
        }

        /// <summary>
        /// Run Excel Automation
        /// </summary>
        public override void Run()
        {
            if (FileName.Length == 0)
            {
                throw new Exception("Property of FileName hasn't be initialized");
            }
            if (Plate == null)
            {
                throw new Exception("Property of Plate hasn't be initialized");
            }

            Excel.Application objExcel = new Excel.Application();
            objExcel.DisplayAlerts = false;
            if (objExcel == null)
            {
                throw new Exception("EXCEL could not be started. Check that your office installation and project references are correct");
            }
            objExcel.Visible = false;
            Excel.Workbook objWorkBook = objExcel.Workbooks.Open(this.FileName, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            Excel.Sheets objWorkSheets = objWorkBook.Worksheets;
            Excel.Worksheet objWorkSheet = null;
            Excel.Range objRange = null;
            Excel.Worksheet objTempSheet = null;
            Log.StartLog(FileName + ".log", "Excel Plate");

            try
            {
                for (int i = 1; i <= objWorkSheets.Count; i++)
                {
                    this.TagInfo = string.Format("Process Sheets:{0}/{1}", i.ToString(), objWorkSheets.Count.ToString());
                    objWorkSheet = (Excel.Worksheet)objWorkSheets[i];
                    int rowcount = objWorkSheet.UsedRange.Rows.Count;
                    int columncount = objWorkSheet.UsedRange.Columns.Count;
                    objRange = objWorkSheet.UsedRange;
                    if (objRange.Cells.Count == 1)
                    {
                        objRange = objRange.get_Resize(1, 2);
                    }
                    object[,] objValue = (object[,])objRange.get_Value(Missing.Value);
                    ArrayList listValue = new ArrayList();
                    
                    objTempSheet = (Excel.Worksheet)objWorkSheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);//Temp Sheet to copy format
                    int tempindex = 1;
                    this.ProgressCount = rowcount;
                    int rowoffset = (objWorkSheet.UsedRange.Rows[1, Missing.Value] as Excel.Range).Row - 1;
                    for (int j = 1; j <= rowcount; j++)
                    {
                        this.TagInfo = string.Format("Process Rows:{0}/{1}", j.ToString(), rowcount.ToString());
                        this.ProgressInfo = j - 1;
                        int dsmaxcount = int.MaxValue;
                        object[] objRowVallue = new object[columncount];
                        for (int k = 1; k <= columncount; k++)
                        {
                            objRowVallue[k - 1] = objValue[j, k];
                            if (objRowVallue[k - 1] is string)
                            {
                                string strvalue = objRowVallue[k - 1].ToString().Trim();
                                if (strvalue.StartsWith("$") && strvalue.Length > 1)
                                {
                                    strvalue = strvalue.Substring(1);
                                    int dscount = GetDataSourceRowCount(ref strvalue);
                                    if (dscount >= 0)
                                    {
                                        objRowVallue[k - 1] = strvalue;
                                        dsmaxcount = Math.Min(dsmaxcount, dscount);
                                    }
                                }
                            }
                        }

                        objRange = (Excel.Range)objWorkSheet.UsedRange.Rows[j, Missing.Value]; ;
                        if (dsmaxcount == int.MaxValue)
                        {
                            CopyRow(1, objRowVallue, listValue);
                            CopyFormat(objTempSheet, objRange, 1, tempindex);
                            tempindex++;
                        }
                        else
                        {
                            //change chart formula
                            int chartcount = (objWorkSheet.ChartObjects(Missing.Value) as Excel.ChartObjects).Count;
                            if (chartcount >= 1)
                            {
                                int seriescount = ((objWorkSheet.ChartObjects(1) as Excel.ChartObject).Chart.SeriesCollection(Missing.Value) as Excel.SeriesCollection).Count;
                                for (int iser = 1; iser <= seriescount; iser++)
                                {
                                    string formula = ((objWorkSheet.ChartObjects(1) as Excel.ChartObject).Chart.SeriesCollection(iser) as Excel.Series).Formula;
                                    ((objWorkSheet.ChartObjects(1) as Excel.ChartObject).Chart.SeriesCollection(iser) as Excel.Series).Formula
                                        = GetNewChartFomurla(formula, j + rowoffset, listValue.Count + 1 + rowoffset, dsmaxcount);
                                }
                            }
                            //change cell formula, change the currentsheet and tempsheet, only simple formula
                            //currentsheet from current row, tempsheet from previous row
                            foreach (Excel.Range cell in objTempSheet.UsedRange.Cells)
                            {
                                if (cell.Formula != null && cell.Formula.ToString().StartsWith("="))
                                {
                                    cell.Formula = GetNewCellFomurla(cell.Formula.ToString(), j + rowoffset, listValue.Count + 1 + rowoffset, dsmaxcount);
                                }
                            }
                            foreach (Excel.Range cell in objWorkSheet.UsedRange.Cells)
                            {
                                if (cell.Formula != null && cell.Row >= j && cell.Formula.ToString().StartsWith("="))
                                {
                                    cell.Formula = GetNewCellFomurla(cell.Formula.ToString(), j + rowoffset, listValue.Count + 1 + rowoffset, dsmaxcount);
                                }
                            }

                            CopyRow(dsmaxcount, objRowVallue, listValue);
                            CopyFormat(objTempSheet, objRange, dsmaxcount, tempindex);
                            tempindex += dsmaxcount;
                        }
                    }

                    objRange = objWorkSheet.UsedRange;
                    objRange = objRange.get_Resize(objTempSheet.UsedRange.Rows.Count, objTempSheet.UsedRange.Columns.Count);//copy does not effect the size of range sometimes
                    objTempSheet.UsedRange.Copy(objRange);
                    objTempSheet.Delete();

                    objValue = new object[listValue.Count, columncount];
                    //objRange = objWorkSheet.UsedRange;
                    if (objRange.Cells.Count == 1)
                    {
                        objRange = objRange.get_Resize(1, 2);
                    }
                    object[,] objFomurla = (object[,])objRange.Formula;
                    for (int j = 0; j < listValue.Count; j++)
                    {
                        for (int k = 0; k < columncount; k++)
                        {
                            if (((object[])listValue[j])[k] is Bitmap)
                            {
                                
                            }
                            else if (j >= objFomurla.GetUpperBound(0) || k >= objFomurla.GetUpperBound(1)
                                 || !objFomurla[j + 1, k + 1].ToString().StartsWith("="))
                            {
                                objValue[j, k] = ((object[])listValue[j])[k];
                            }
                            else
                            {
                                objValue[j, k] = objFomurla[j + 1, k + 1];
                            }
                        }

                    } 
                    //objRange = objWorkSheet.UsedRange;
                    
                    objRange.set_Value(Missing.Value, objValue);

                    #region insert image
                    for (int j = 0; j < listValue.Count; j++)
                    {
                        for (int k = 0; k < columncount; k++)
                        {
                            if (((object[])listValue[j])[k] is Bitmap)
                            {
                                Bitmap pic = ((object[])listValue[j])[k] as Bitmap;
                                if (object.Equals(pic.Tag, true))
                                {
                                    Excel.Range temprange = objRange.Cells[j + 1, k + 1] as Excel.Range;
                                    double heightinexcel = (double)pic.Height / (double)pic.VerticalResolution * 72;
                                    if (((double)temprange.RowHeight) < heightinexcel)
                                    {
                                        temprange.RowHeight = Math.Min(heightinexcel, 409);
                                    }
                                    double widthinexcel = (double)pic.Width / (double)pic.HorizontalResolution * 96 / 8 - 0.62;
                                    if (((double)temprange.ColumnWidth) < widthinexcel)
                                    {
                                        temprange.ColumnWidth = Math.Min(widthinexcel, 255);
                                    }
                                }
                            }
                        }
                    }
                    for (int j = 0; j < listValue.Count; j++)
                    {
                        for (int k = 0; k < columncount; k++)
                        {
                            if (((object[])listValue[j])[k] is Bitmap)
                            {
                                Bitmap pic = ((object[])listValue[j])[k] as Bitmap;
                                Excel.Range temprange = objRange.Cells[j + 1, k + 1] as Excel.Range;

                                double left = (double)temprange.Left;
                                double top = (double)temprange.Top;
                                pic.Save(Environment.CurrentDirectory + "\\temp.jpg");
                                Excel.Picture picinsheet = (objWorkSheet.Pictures(Missing.Value) as Excel.Pictures).Insert(Environment.CurrentDirectory + "\\temp.jpg", Missing.Value);
                                picinsheet.Left = left;
                                picinsheet.Top = top;
                                //picinsheet.Width = Math.Min(pic.Width,(double)temprange.Width);
                                //picinsheet.Height =Math.Min(pic.Heig ht,(double)temprange.Height);
                            }
                        }
                    }  
                    #endregion
                }
                this.TagInfo = "Process Finished";
                this.ProgressInfo = this.ProgressCount;
                objWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            catch (Exception e)
            {
                Log.WriteExceptionInfo(e);
                throw;
            }
            finally
            {
                Log.EndLog();
                objExcel.Quit();
                Marshal.ReleaseComObject(objRange);
                Marshal.ReleaseComObject(objWorkSheet);
                Marshal.ReleaseComObject(objTempSheet);
                Marshal.ReleaseComObject(objWorkSheets);
                Marshal.ReleaseComObject(objWorkBook);
                Marshal.ReleaseComObject(objExcel);

                objRange = null;
                objWorkSheet = null;
                objTempSheet = null;
                objWorkSheets = null;
                objWorkBook = null;
                objExcel = null;
                
                GC.Collect();
            }
        }

        /// <summary>
        /// Copy the row to a new worksheet
        /// </summary>
        /// <param name="sheet">The sheet to copy to</param>
        /// <param name="row">The row to copy</param>
        /// <param name="count">The times to copy</param>
        /// <param name="index">The index of row to copy to</param>
        protected void CopyFormat(Excel.Worksheet sheet, Excel.Range row, int count, int index)
        {
            int rowindex = index;
            for (int i = 0; i < count; i++)
            {
                row.Copy(sheet.get_Range(sheet.Cells[rowindex, 1], sheet.Cells[rowindex, 1]));
                rowindex++;
            }
        }

        /// <summary>
        /// Modify the formula of the chart
        /// </summary>
        /// <param name="formula">The text of formula</param>
        /// <param name="rowindex">The index of row in template</param>
        /// <param name="newrowindex">The index of row in new file</param>
        /// <param name="count">The count of rows to add</param>
        /// <returns>The text of new formula</returns>
        protected string GetNewChartFomurla(string formula, int rowindex, int newrowindex, int count)
        {
            string[] arrformula = formula.Split(',');

            for (int i = 1; i <= 2; i++)
            {
                Match mcx = Regex.Match(arrformula[i], @"\$\w+\$\w+(:\$\w+\$\w+)*");
                if (mcx.Success)
                {
                    string formulacell = mcx.Value;
                    string[] arrformulacell = formulacell.Split(':');
                    string formulafrom = arrformulacell[0];
                    string formulato = (arrformulacell.Length == 1) ? arrformulacell[0] : arrformulacell[1];
                    if (formulafrom.Split('$')[2] == rowindex.ToString())
                    {
                        formulafrom = formulafrom.Substring(0, formulafrom.LastIndexOf('$') + 1) + newrowindex.ToString();
                        formulato = formulato.Substring(0, formulato.LastIndexOf('$') + 1) + ((int)(newrowindex + count - 1)).ToString();

                        arrformula[i] = arrformula[i].Substring(0, mcx.Index) + formulafrom + ":" + formulato;
                    }
                }
            }

            StringBuilder sbulider = new StringBuilder();
            for (int i = 0; i < arrformula.Length; i++)
            {
                if (sbulider.Length != 0)
                {
                    sbulider.Append(",");
                }
                sbulider.Append(arrformula[i]);
            }
            return sbulider.ToString();
        }

        /// <summary>
        /// Modify the fomurla of cell
        /// </summary>
        /// <param name="formula">The text of formula</param>
        /// <param name="rowindex">The index of row in template</param>
        /// <param name="newrowindex">The index of row in new file</param>
        /// <param name="count">The count of rows to add</param>
        /// <returns>The text of new formula</returns>
        protected string GetNewCellFomurla(string formula, int rowindex, int newrowindex, int count)
        {
            Match mcx = Regex.Match(formula, @"\((\s*\d+,)*\s*\D+\d+(:\D+\d+\s*)*\)");
            if (mcx.Success)
            {
                string formulacell = mcx.Value.TrimStart('(').TrimEnd(')');
                string[] arrformulacell = formulacell.Split(',');
                if (arrformulacell.Length == 2)//subtotal function has ',' in fomurla
                {
                    formulacell = arrformulacell[1];
                }
                arrformulacell = formulacell.Split(':');
                string formulafrom = arrformulacell[0].Trim();
                string formulato = (arrformulacell.Length == 1) ? arrformulacell[0].Trim() : arrformulacell[1].Trim();
                
              
                string fromrow = Regex.Match(formulafrom, @"\d+").Value;
                if (fromrow == rowindex.ToString())
                {
                    string fromcolumn = Regex.Match(formulafrom, @"\D+").Value;

                    string tocolumn = Regex.Match(formulato, @"\D+").Value;
                    string torow = Regex.Match(formulato, @"\d+").Value;

                    StringBuilder sbuilder = new StringBuilder();
                    sbuilder.Append(fromcolumn);
                    sbuilder.Append(newrowindex);
                    sbuilder.Append(":");
                    sbuilder.Append(tocolumn);
                    sbuilder.Append(newrowindex + count - 1);

                    return formula.Replace(formulacell, sbuilder.ToString());
                }
            }
        
            return formula;
        }
    }
}
