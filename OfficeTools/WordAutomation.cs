using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

namespace OfficeTools
{
    /// <summary>
    /// The class of WordAutomationXml
    /// </summary>
    public class WordAutomationXml: OfficeAutomation
    {
        /// <summary>
        /// Create a new instance of WordAutomationXml
        /// </summary>
        public WordAutomationXml()
        {
            ProgressInfo = 0;
            ProgressCount = 0;
            TagInfo = string.Empty;
            Log = new PlateLog();
        }

        /// <summary>
        /// Create a new instance of WordAutomationXml
        /// </summary>
        /// <param name="filename">Name of word file</param>
        /// <param name="plate">The WordPlate whose defination is used</param>
        public WordAutomationXml(string filename, IOfficePlate plate)
            : this()
        {
            FileName = filename;
            Plate = plate;
            ErrorReport = plate.MarkException;
        }

        /// <summary>
        /// Run Word Automation
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
                throw new Exception("Word file is not recognized as a xml");
            }
            XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xml.NameTable);
            xmlmgr.AddNamespace("w", "http://schemas.microsoft.com/office/word/2003/wordml");
            xmlmgr.AddNamespace("wx", "http://schemas.microsoft.com/office/word/2003/auxHint");
            string rowname = "/w:wordDocument/w:body/wx:sect/w:p/w:r/w:t";
            XmlNodeList nodelist = xml.SelectNodes(rowname, xmlmgr);
            this.ProgressCount = nodelist.Count;
            Log.StartLog(FileName + ".log", "Word Plate");

            for (int i = 0; i < nodelist.Count; i++)
            {
                this.TagInfo = string.Format("Process Tag:{0}/{1}", i.ToString(), nodelist.Count.ToString());
                this.ProgressInfo = i;
                ProcessTag(nodelist[i]);
            }

            rowname = "/w:wordDocument/w:body/wx:sect/w:tbl/w:tr";
            nodelist = xml.SelectNodes(rowname, xmlmgr);
            this.ProgressCount = nodelist.Count;
            for (int i = 0; i < nodelist.Count; i++)
            {
                this.TagInfo = string.Format("Process Row:{0}/{1}", i.ToString(), nodelist.Count.ToString());
                this.ProgressInfo = i;
                XmlNodeList nodecell = nodelist[i].SelectNodes("w:tc/w:p/w:r/w:t", xmlmgr);
                int dsmaxcount = int.MaxValue;
                foreach (XmlNode xn in nodecell)
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
                    ProcessTag(nodecell);
                }
                else
                {
                    CopyRow(dsmaxcount, nodelist[i], xmlmgr);
                }
            
            }

            Log.EndLog();
            this.TagInfo = "Process Finished";
            this.ProgressInfo = this.ProgressCount;
            xml.Save(FileName);
        }

        /// <summary>
        /// Copy the row in table
        /// </summary>
        /// <param name="count">The times of copy</param>
        /// <param name="node">The node of row</param>
        protected void CopyRow(int count, XmlNode node, XmlNamespaceManager xmlmgr)
        {
            for (int i = 0; i < count; i++)
            {
                this.TagInfo = string.Format("Insert new row:{0}/{1}", i.ToString(), count.ToString());
                XmlNode newNode = node.CloneNode(true);
                string rowname = "w:tc/w:p/w:r/w:t";
                XmlNodeList nodelist = newNode.SelectNodes(rowname, xmlmgr);
                foreach (XmlNode nodecell in nodelist)
                {
                    string strvalue = nodecell.InnerText.Trim();
                    if (strvalue.StartsWith("$") && strvalue.Contains("{0}"))
                    {
                        nodecell.InnerXml = nodecell.InnerXml.Replace(nodecell.InnerText, string.Format(strvalue, i.ToString()));
                    }
                }
                ProcessTag(nodelist);
                node.ParentNode.InsertBefore(newNode, node);
            }
            node.ParentNode.RemoveChild(node);
        }

        /// <summary>
        /// Process tags of the whole row
        /// </summary>
        /// <param name="nodeRow">The node of row</param>
        protected void ProcessTag(XmlNode nodeRow)
        {
            string newinnertext = nodeRow.InnerText;
            string[] arrinnertext = nodeRow.InnerText.Split(' ');
            foreach (string str in arrinnertext)
            {
                if (str.StartsWith("$") && str.Length > 1)
                {
                    this.TagInfo = "Process tag:" + str;
                    object[] obj = Automation.Run(Plate, str.Substring(1));
                    if ((PlateException)obj[0] == PlateException.None)
                    {
                        newinnertext = newinnertext.Replace(str, obj[1].ToString().Replace("&", "&&").Replace(">", "&gt").Replace("<", "&lt"));
                    }
                    else
                    {
                        if (ErrorReport)
                        {
                            newinnertext = newinnertext.Replace(str, string.Format("$Error: {0}", obj[0].ToString()));
                        }
                        Log.WriteErrorInfo(obj[0].ToString(), str, obj[1].ToString());
                    }
                }
            }
            nodeRow.InnerText = newinnertext;
        }

        /// <summary>
        /// Process tags of the whole row
        /// </summary>
        /// <param name="nodeRow">The node list of row</param>
        protected void ProcessTag(XmlNodeList nodeRow)
        {
            foreach (XmlNode nodecell in nodeRow)
            {
                string strvalue = nodecell.InnerText.Trim();
                if (strvalue.StartsWith("$") && strvalue.Length > 1)
                {
                    this.TagInfo = "Process tag:" + strvalue;
                    object[] obj = Automation.Run(Plate, strvalue.Substring(1));
                    if ((PlateException)obj[0] == PlateException.None)
                    {
                        nodecell.InnerText = obj[1].ToString().Replace("&", "&&").Replace(">", "&gt").Replace("<", "&lt");
                    }
                    else
                    {
                        if (ErrorReport)
                        {
                            nodecell.InnerText = string.Format("$Error: {0}", obj[0].ToString());
                        }
                        Log.WriteErrorInfo(obj[0].ToString(), strvalue, obj[1].ToString());
                    }
                }
            }
        }
    }

    /// <summary>
    /// The class of WordAutomationCom
    /// </summary>
    public class WordAutomationCom : OfficeAutomation
    {
        /// <summary>
        /// Create a new instance of WordAutomationCom
        /// </summary>
        public WordAutomationCom()
        {
            ProgressInfo = 0;
            ProgressCount = 0;
            TagInfo = string.Empty;
            Log = new PlateLog();
        }

        /// <summary>
        /// Create a new instance of WordAutomationCom
        /// </summary>
        /// <param name="filename">Name of word file</param>
        /// <param name="plate">The WordPlate whose defination is used</param>
        public WordAutomationCom(string filename, IOfficePlate plate)
            : this()
        {
            FileName = filename;
            Plate = plate;
            ErrorReport = plate.MarkException;
        }

        /// <summary>
        /// Run Word Automation
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
            Word.Application objWord = new Word.Application();
            if (objWord == null)
            {
                throw new Exception("Word could not be started. Check that your office installation and project references are correct");
            }
            objWord.Visible = false;
            objWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;
            object objMiss = Missing.Value;
            object strfile = FileName;
            Word.Document objDocument = objWord.Documents.Open(ref strfile, ref objMiss, ref objMiss, ref objMiss
                , ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss
                , ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
            Word.Row objRow = null;
            Log.StartLog(FileName + ".log", "Word Plate");

            try
            {
                this.ProgressCount = objDocument.Sentences.Count;
                for (int i = 1; i <= objDocument.Sentences.Count; i++)
                {
                    this.ProgressInfo = i - 1; 
                    this.TagInfo = string.Format("Process Line:{0}/{1}", i.ToString(), objDocument.Sentences.Count.ToString());
                    if (!objDocument.Sentences[i].Text.EndsWith("\a"))
                    {
                        string newtext = ProcessTag(objDocument.Sentences[i].Text);
                        if (string.Compare(objDocument.Sentences[i].Text, newtext) != 0)
                        {
                            objDocument.Sentences[i].Text = newtext;
                        }
                    }
                }
                
                for (int i = 1; i <= objDocument.Tables.Count; i++)
                {
                    this.TagInfo = string.Format("Process Table:{0}/{1}", i.ToString(), objDocument.Tables.Count.ToString());
                    this.ProgressInfo = 0;
                    this.ProgressCount = objDocument.Tables[i].Rows.Count;
                    ArrayList listValue = new ArrayList();
                    objDocument.Tables[i].Rows.Add(ref objMiss);//新增一个空行,最后会删除
                    int rowindex = 1;
                    while (rowindex < objDocument.Tables[i].Rows.Count)
                    {
                        this.ProgressInfo++;
                        int dsmaxcount = int.MaxValue;
                        objRow = objDocument.Tables[i].Rows[rowindex];
                        object[] objRowVallue = new object[objRow.Cells.Count];
                        for (int j = 1; j <= objRow.Cells.Count; j++)
                        {
                            objRowVallue[j - 1] = objRow.Cells[j].Range.Text.TrimEnd(new char[] { '\r', '\a'});
                            string strvalue = objRowVallue[j - 1].ToString().Trim();
                            if (strvalue.StartsWith("$") && strvalue.Length > 1)
                            {
                                strvalue = strvalue.Substring(1);
                                int dscount = GetDataSourceRowCount(ref strvalue);
                                if (dscount >= 0)
                                {
                                    objRowVallue[j - 1] = strvalue;
                                    dsmaxcount = Math.Min(dsmaxcount, dscount);
                                }
                            }
                        }
                        if (dsmaxcount == int.MaxValue)
                        {
                            rowindex++;
                            CopyRow(1, objRowVallue, listValue);
                        }
                        else
                        {
                            rowindex += dsmaxcount;
                            CopyRow(dsmaxcount, objRowVallue, listValue);
                            objRow.Range.Copy();
                            CopyFormat(objRow.Next.Range, dsmaxcount);
                            objRow.Delete();
                        }
                    }
                    objDocument.Tables[i].Rows.Last.Delete();

                    for (int j = 1; j <= objDocument.Tables[i].Rows.Count; j++)
                    {
                        for (int k = 1; k <= objDocument.Tables[i].Columns.Count; k++)
                        {
                            objDocument.Tables[i].Cell(j, k).Range.Text = ((object[])listValue[j - 1])[k - 1].ToString().TrimEnd("\r\a".ToCharArray());
                        }
                    }
                }
                this.TagInfo = "Process Finished";
                this.ProgressInfo = this.ProgressCount;

                objDocument.Save();
            }
            catch (Exception e)
            {
                Log.WriteExceptionInfo(e);
                throw; 
            }
            finally
            {
                Log.EndLog();
                ((Word._Application)objWord).Quit(ref objMiss, ref objMiss, ref objMiss);
                if (objRow != null)
                {
                    Marshal.ReleaseComObject(objRow);
                }
                Marshal.ReleaseComObject(objDocument);
                Marshal.ReleaseComObject(objWord);

                objRow = null;
                objDocument = null;
                objWord = null;
                objMiss = null;

                GC.Collect();
            }
        } 

        protected void CopyFormat(Word.Range rowbefore, int count)
        {
            for (int i = 0; i < count; i++)
            {
                rowbefore.Paste();
            }
        }

        protected string ProcessTag(string strRow)
        {
            string innertext = strRow;
            string newinnertext = innertext;
            string[] arrinnertext = innertext.Split(' ');
            foreach (string str in arrinnertext)
            {
                if (str.StartsWith("$") && str.Length > 1)
                {
                    this.TagInfo = "Process tag:" + str;
                    object[] obj = Automation.Run(Plate, str.Substring(1));
                    if ((PlateException)obj[0] == PlateException.None)
                    {
                        newinnertext = newinnertext.Replace(str, obj[1].ToString().Replace("&", "&&").Replace(">", "&gt").Replace("<", "&lt"));
                    }
                    else
                    {
                        if (ErrorReport)
                        {
                            newinnertext = newinnertext.Replace(str, string.Format("$Error: {0}", obj[0].ToString()));
                        }
                        Log.WriteErrorInfo(obj[0].ToString(), str, obj[1].ToString());
                    }
                }
            }
            return newinnertext;
        }
    }
}
