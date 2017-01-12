using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;
using Word;

namespace EEPSDModule2015
{
    class PublicTest
    {
        static String FWebSite = "";
        static String FUserID = "";
        static String FPassword = "";
        static SYS_LANGUAGE language = SYS_LANGUAGE.ENG;
        static String FSolution = "";
        static String FDir = "";
        static String FDataBase = "";
        static String Ftype = "A";
        static int printwaitingtime = 5;
        static bool[] documentOptions = new bool[] { true, false, false, false, true, true, true, true, false, false, false };

        public static object UIPrint(DTE2 FDTE2, Dictionary<string, object> hs, object[] options/*,string[][][] selectedstring,int checkedcount*/)
        {
            Ftype = options[0].ToString();
            FSolution = options[1].ToString();
            FWebSite = options[2].ToString();
            FDataBase = options[3].ToString();
            FUserID = options[4].ToString();
            FPassword = options[5].ToString();
            language = (SYS_LANGUAGE)options[6];

            if (options.Length > 7)
            {
                documentOptions = options[7] as bool[];
            }
            if (options.Length > 8)
            {
                printwaitingtime = (int)options[8];
            }
            if (options.Length > 9)
            {
                PrintTempFilePath = (string)options[9];
            }
            Word._Application oWord;
            Word._Document oDoc;

            try
            {
                oWord = new Word.Application();
            }
            catch (Exception ex)
            {
                return new object[] { 1, ex.Message };
            }
            Selection selection;
            try
            {
                oWord.Visible = true;

                //oDoc = oWord.Documents.Add(ref oTemplate, ref oMissing, ref oMissing, ref oMissing);
                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
            ref oMissing, ref oMissing);

                selection = oWord.Selection;
                selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                selection.Font.Bold = 2;
                selection.TypeText("【 UI " + GetSystemMessage(language, "SDModule", "Document", "list") + " 】" + "\r\n");
            }
            catch (Exception ex)
            {
                object closesave = Word.WdSaveOptions.wdDoNotSaveChanges;
                object option = Word.WdOriginalFormat.wdWordDocument;
                object f = false;
                oWord.Quit(ref closesave, ref option, ref f);
                return new object[] { 1, ex.Message };
            }

            ArrayList paramArray = new ArrayList();

            //zhongyao ******************************************************
            PrintFormList(selection, oDoc, hs, options, paramArray);
            //****************************************

            selection.TypeBackspace();
            selection.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            int bookmarkcount = 1;
            object type = Word.WdBreakType.wdPageBreak;
            selection.InsertBreak(ref type);
            if (FDTE2 != null)
            {
                foreach (Object param in paramArray)
                {
                    //bool islast = false;
                    //if (paramArray.IndexOf(param) + 1 == paramArray.Count)
                    //    islast = true;
                    Object objback = null;
                    try
                    {
                        SDMOduleDTE DteWinForm = new SDMOduleDTE(param, FUserID, FPassword, FDataBase, FDir, printwaitingtime.ToString(), documentOptions, FDTE2, (int)language);
                        if (((string[])(((object[])(object[])param)[2]))[0] == "cs")
                        {
                            objback = DteWinForm.WinWork(/*islast, ref dte*/);
                        }
                        else if (((string[])(((object[])(object[])param)[2]))[0] == "aspx")
                        {
                            if (Ftype == "A")
                                objback = DteWinForm.WebWork(/*islast,ref dte*/);
                            else if (Ftype == "Q")
                                objback = DteWinForm.JQWork();
                        }
                        else if (((string[])(((object[])(object[])param)[2]))[0] == "dllproj")
                        {
                            objback = DteWinForm.ServiceWork();
                        }
                    }
                    catch (Exception ex) { return new object[] { 1, ex.Message }; }
                    object[] objAll = null;
                    if ((int)((object[])objback)[0] == -1)
                    {
                        //MessageBox.Show((string)((object[])objback)[1]);
                        continue;
                    }

                    else if ((int)((object[])objback)[0] == 1)
                    {
                        objAll = (object[])((object[])objback)[1];
                        string formname = ((string[])((object[])param)[2])[2];
                        string jobdetail = GetSystemMessage(language, "SDModule", "Document", "jobdetail");
                        string[] jobdetaillist = jobdetail.Split(new char[] { ',' });
                        object[] objDes = new object[] { jobdetaillist[0] + ":", formname, jobdetaillist[1] + ":", "", jobdetaillist[2] + ":", "" };
                        oWord.Activate();
                        //oWord.ShowMe();
                        PrintSecondPage(oDoc, oWord, objDes, objAll, ref bookmarkcount);

                        if (Ftype == "Q")
                        {
                            PrintOtherPageForJQ(oDoc, oWord, objAll);
                        }
                        else
                        {
                            PrintOtherPage(oDoc, oWord, objAll);
                        }
                    }
                }
            }

            object Wdbookmark = WdGoToItem.wdGoToBookmark;
            foreach (Bookmark o in oDoc.Bookmarks)
            {
                if (o.Name.StartsWith("source_"))
                {
                    string sourcename = o.Name.Substring(7);
                    object osd = (object)o;
                    selection.GoTo(ref Wdbookmark, ref oMissing, ref oMissing, ref osd);
                    foreach (Bookmark b in oDoc.Bookmarks)
                    {
                        if (b.Name.Equals("goal_" + sourcename, StringComparison.Ordinal))
                        {
                            object oo = (object)b;
                            string selectiontext = selection.Text;
                            selectiontext = selectiontext.Trim(new char[] { '\r', '\a', '\n' });
                            selection.Text = "";
                            object oa = (object)selectiontext;
                            oDoc.Hyperlinks.Add(selection.Range, ref oMissing, ref oo, ref oa, ref oa, ref oMissing);
                        }
                    }
                }
            }

            object filename = "Page" + DateTime.Now.ToString("yyyyMMddhhss");
            if (Ftype == "S") filename = "Service" + DateTime.Now.ToString("yyyyMMddhhss");
            object path = PrintTempFilePath + @"\" + filename + ".doc";

            if ((string)filename != "")
            {
                try
                {
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                }
                catch (Exception ex)
                { return new object[] { 1, ex.Message }; }
            }
            return new object[] { 0, filename };
        }

        private static void PrintFormList(Selection selection, _Document oDoc, Dictionary<string, object> hs, object[] options, ArrayList paramArray)
        {
            string type = options[0].ToString();
            string sln = options[1].ToString();
            string website = options[2].ToString();

            object WDCount1 = 1;
            object WdLine = Word.WdUnits.wdLine;
            object WdCell = Word.WdUnits.wdCell;
            object oMissing = System.Reflection.Missing.Value;

            Word.Range tableLocation = selection.Range;
            object WDWord9TableBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
            object WDAutoFitWindow = WdAutoFitBehavior.wdAutoFitWindow;

            int countheadercount = 4;
            if (documentOptions[2] || documentOptions[3])
                countheadercount = 6;
            Word.Table mainTable = oDoc.Tables.Add(tableLocation, hs.Count + 1, countheadercount, ref WDWord9TableBehavior, ref WDAutoFitWindow);

            //Add Tabel Items
            mainTable.Range.Font.Size = 9;
            mainTable.Range.Bold = 2;
            mainTable.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            mainTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
            mainTable.Rows[1].Range.Font.Size = 13;
            string maindetail = GetSystemMessage(language, "SDModule", "Document", "maindetail");
            string[] maindetaillist = maindetail.Split(new char[] { ',' });
            if (maindetaillist.Length == 6)
            {
                mainTable.Cell(1, 1).Range.Text = maindetaillist[0];
                mainTable.Cell(1, 2).Range.Text = maindetaillist[1];
                mainTable.Cell(1, 3).Range.Text = maindetaillist[2];
                if (countheadercount == 6)
                {
                    mainTable.Cell(1, 4).Range.Text = maindetaillist[3];
                    mainTable.Cell(1, 5).Range.Text = maindetaillist[4];
                    mainTable.Cell(1, 6).Range.Text = maindetaillist[5];
                }
                else
                {
                    mainTable.Cell(1, 4).Range.Text = maindetaillist[5];
                }
            }
            else
            {
                mainTable.Cell(1, 1).Range.Text = "Solution Name";
                mainTable.Cell(1, 2).Range.Text = "Project Name";
                mainTable.Cell(1, 3).Range.Text = "Item Name";
                if (countheadercount == 6)
                {
                    mainTable.Cell(1, 4).Range.Text = "Sign For Customer";
                    mainTable.Cell(1, 5).Range.Text = "Confirmation Date";
                    mainTable.Cell(1, 6).Range.Text = "Remarks";
                }
                else
                {
                    mainTable.Cell(1, 4).Range.Text = "Remarks";
                }
            }
            selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);

            int addcount = 0;
            int count1 = 0;
            int k = 0;
            int bookmarkcount = 1;
            string[] slns = { "sln", sln, sln.Substring(sln.LastIndexOf("\\") + 1) };

            foreach (var dr in hs.Keys)
            {
                if (dr.ToString().EndsWith(".aspx"))
                {
                    string[] projname = { "website", website, website.EndsWith("\\") ? website.Remove(website.Length - 1).Substring(website.Remove(website.Length - 1).LastIndexOf("\\") + 1) : website.Substring(website.LastIndexOf("\\") + 1) };

                    //mainTable.Cell(2 + count1 + k, 3).Range.Text = selectedstring[i][j][k].ToString();
                    object tags = hs[dr];// new string[] { "aspx", sc, fInfo.Name };

                    Object param = new object[] { slns, projname, tags };
                    paramArray.Add(param);

                    string[] folderstrs = dr.ToString().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    string dirs = "";//保留
                    for (int i = 0; i < folderstrs.Length - 1; i++)
                    {
                        if (dirs != "")
                            dirs += "\\";
                        dirs += folderstrs[i];
                    }

                    int folderdepth = folderstrs.Length - 1;
                    int addcountbackup = addcount;
                    int adddepth = folderdepth;
                    if (adddepth < addcount)
                    { addcount = adddepth; }
                    for (; addcount < adddepth; addcount++)
                    {
                        selection.MoveRight(ref WdCell, ref WDCount1, ref oMissing);
                        selection.InsertColumnsRight();
                        selection.TypeText(GetSystemMessage(language, "SDModule", "Document", "itemname"));
                    }
                    for (addcount = 0; addcount < adddepth; addcount++)
                    {
                        mainTable.Cell(2 + count1 + k, 3 + addcount).Range.Text = folderstrs[addcount];
                    }
                    mainTable.Cell(2 + count1 + k, 3 + folderdepth).Range.Text = folderstrs[folderdepth];
                    mainTable.Cell(2 + count1 + k, 3 + folderdepth).Range.Select();
                    System.Threading.Thread.Sleep(1000);
                    string bookmarkname = "source_aspx_" + folderstrs[folderdepth].Substring(0, folderstrs[folderdepth].LastIndexOf('.'));
                    if (oDoc.Bookmarks.Exists(bookmarkname))
                    {
                        bookmarkname = bookmarkname + "_" + bookmarkcount;
                        bookmarkcount++;
                    }
                    oDoc.Bookmarks.Add(bookmarkname, ref oMissing);
                    mainTable.Cell(2 + count1, 2).Range.Text = website.EndsWith("\\") ? website.Remove(website.Length - 1).Substring(website.Remove(website.Length - 1).LastIndexOf("\\") + 1) : website.Substring(website.LastIndexOf("\\") + 1);
                }
                else if (dr.ToString().EndsWith(".dll"))
                {
                    string[] projname = { "dllproj", hs[dr].ToString(), hs[dr].ToString() };
                    object tags = new string[] { "dllproj", hs[dr].ToString(), hs[dr].ToString() + ".dll" };

                    Object param = new object[] { slns, projname, tags };
                    paramArray.Add(param);

                    mainTable.Cell(2 + count1 + k, 2).Range.Text = hs[dr].ToString();
                    mainTable.Cell(2 + count1 + k, 2).Range.Select();
                    System.Threading.Thread.Sleep(1000);
                    string bookmarkname = "source_dll_" + hs[dr].ToString();
                    if (oDoc.Bookmarks.Exists(bookmarkname))
                    {
                        bookmarkname = bookmarkname + "_" + bookmarkcount;
                        bookmarkcount++;
                    }
                    oDoc.Bookmarks.Add(bookmarkname, ref oMissing);
                }
                else
                {
                    //string formnametotle = selectedstring[i][j][k].ToString();
                    //mainTable.Cell(2 + count1  + k, 3).Range.Text = formnametotle;
                    //mainTable.Cell(2 + count1  + k, 3).Range.Select();
                    //string bookmarkname = "source_cs_" + formnametotle.Substring(0, formnametotle.LastIndexOf('.'));
                    //if (oDoc.Bookmarks.Exists(bookmarkname))
                    //{
                    //    bookmarkname = bookmarkname + "_" + bookmarkcount;
                    //    bookmarkcount++;
                    //}
                    //System.Threading.Thread.Sleep(1000);
                    //oDoc.Bookmarks.Add(bookmarkname, ref oMissing);
                    //foreach (TreeNode formnode in treeView1.Nodes[i].Nodes[j].Nodes)
                    //{
                    //    if (formnode.Text == selectedstring[i][j][k].ToString())
                    //    {
                    //        string[] formname = (string[])formnode.Tag;
                    //        Object param = new object[] { sln, projname, formname };
                    //        paramArray.Add(param);
                    //        break;
                    //    }
                    //}
                }
                k++;
                mainTable.Cell(2 + count1, 1).Range.Text = sln.Substring(sln.LastIndexOf("\\") + 1);
            }

            object checkcountcount = hs.Count + 2;
            selection.MoveDown(ref WdLine, ref checkcountcount, ref oMissing);
            mainTable.Range.Columns.AutoFit();
            mainTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
        }

        private static void PrintOtherPage(_Document oDoc, _Application oWord, object[] objAll)
        {
            try
            {
                object[] tablelist = (object[])objAll[1];
                object[] fieldlist = (object[])objAll[2];
                object[] GridColumnList = (object[])objAll[3];
                object[] DataModuleFileList = (object[])objAll[4];
                object[] DefaultValidateList = (object[])objAll[5];
                object[] CommandList = (object[])objAll[6];
                object[] UpdateList = (object[])objAll[7];
                object[] CodeList = (object[])objAll[8];
                Word.Selection selection = oWord.Selection;
                object WdLine = Word.WdUnits.wdLine;
                object Wdstory = Word.WdUnits.wdStory;
                object WDCount1 = 1;
                object oMissing = System.Reflection.Missing.Value;
                object WDWord9TableBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object WDAutoFitWindow = WdAutoFitBehavior.wdAutoFitWindow;
                int tablecount = 0;
                int captioncount = 1;
                selection.EndKey(ref Wdstory, ref oMissing);
                //Add ServerPicture
                //if (_MDIset.documentsetting[1])
                //{
                //    foreach (string sfile in DataModuleFileList)
                //    {
                //        object LinkToFile = false;
                //        object SaveWithDocument = true;
                //        selection.InlineShapes.AddPicture(sfile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                //        selection.TypeParagraph();
                //    }
                //    selection.TypeParagraph();
                //}

                if (fieldlist != null && fieldlist.Length > 0)
                {
                    //Field Table
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + "." + GetSystemMessage(language, "SDModule", "Document", "fielddescri"));
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    Table fieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Length + 1, 8, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    fieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    fieldTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    fieldTable.Rows[1].Range.Font.Bold = 2;
                    string fielddecsridetail = GetSystemMessage(language, "SDModule", "Document", "fielddescridetail");
                    string[] fielddecdetailt = fielddecsridetail.Split(new char[] { ',' });
                    if (fielddecdetailt.Length == 4)
                    {
                        fieldTable.Cell(1, 1).Range.Text = fielddecdetailt[0];
                        fieldTable.Cell(1, 2).Range.Text = fielddecdetailt[1];
                        fieldTable.Cell(1, 3).Range.Text = fielddecdetailt[2];
                        fieldTable.Cell(1, 4).Range.Text = fielddecdetailt[3];
                    }
                    else
                    {
                        //fieldTable.Cell(1, 1).Range.Text = "Field Name";
                        //fieldTable.Cell(1, 2).Range.Text = "Field Caption";
                        //fieldTable.Cell(1, 3).Range.Text = "Table Name";
                        //fieldTable.Cell(1, 4).Range.Text = "Description";
                        fieldTable.Cell(1, 1).Range.Text = "FieldName";
                        fieldTable.Cell(1, 2).Range.Text = "Caption";
                        fieldTable.Cell(1, 3).Range.Text = "Editor";
                        fieldTable.Cell(1, 4).Range.Text = "EditorOptions";
                        fieldTable.Columns[4].PreferredWidth = 20;
                        fieldTable.Cell(1, 5).Range.Text = "Format";
                        fieldTable.Cell(1, 6).Range.Text = "Sortable";
                        fieldTable.Cell(1, 7).Range.Text = "Frozen";
                        fieldTable.Cell(1, 8).Range.Text = "Total";

                    }//selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        string fieldname = (string)fieldlist[i];
                        string[] fieldnamesub = fieldname.Split(new char[] { '.' });
                        fieldTable.Cell(i + 2, 1).Range.Text = fieldnamesub[1];
                        fieldTable.Cell(i + 2, 2).Range.Text = fieldnamesub[1]; //getFieldCaption(fieldnamesub[1], fieldnamesub[0]);
                        fieldTable.Cell(i + 2, 3).Range.Text = fieldnamesub[0];
                        fieldTable.Cell(i + 2, 4).Range.Text = fieldnamesub[0]; //getFieldDescription(fieldnamesub[1], fieldnamesub[0]);
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    }
                    fieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }

                //grdView
                if (GridColumnList != null && GridColumnList.Length != 0)
                {
                    //Srvtools.CliUtils.GetTableName("S01", "Master", "Solution1");
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + "." + GetSystemMessage(language, "SDModule", "Document", "griddescri"));
                    captioncount++;
                    selection.TypeParagraph();
                    for (int i = 0; i < GridColumnList.Length; i++)
                    {
                        selection.Font.Bold = 2;
                        selection.Font.Size = 12;

                        object[] GridColumn = (object[])GridColumnList[i];
                        string sGridColumn = (string)GridColumn[0];
                        string[] subGridColumn = sGridColumn.Split(new char[] { '.' });
                        string tablename = subGridColumn[1];
                        selection.TypeText("【" + subGridColumn[0] + "】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();
                        object[] gridColumnList = (object[])GridColumn[1];
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, gridColumnList.Length + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;
                        string fielddecsridetail = GetSystemMessage(language, "SDModule", "Document", "fielddescridetail");
                        string[] fielddecdetailt = fielddecsridetail.Split(new char[] { ',' });
                        if (fielddecdetailt.Length == 4)
                        {
                            GridViewTable.Cell(1, 1).Range.Text = fielddecdetailt[0];
                            GridViewTable.Cell(1, 2).Range.Text = fielddecdetailt[1];
                            GridViewTable.Cell(1, 3).Range.Text = fielddecdetailt[2];
                            GridViewTable.Cell(1, 4).Range.Text = fielddecdetailt[3];
                        }
                        else
                        {
                            GridViewTable.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable.Cell(1, 2).Range.Text = "Field Caption";
                            GridViewTable.Cell(1, 3).Range.Text = "Table Name";
                            GridViewTable.Cell(1, 4).Range.Text = "Description";
                        }
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        for (int j = 0; j < gridColumnList.Length; j++)
                        {
                            if (((string)gridColumnList[j]).IndexOf('.') != -1)
                            {
                                string[] gcl = ((string)gridColumnList[j]).Split(new char[] { '.' });
                                GridViewTable.Cell(j + 2, 1).Range.Text = gcl[0];
                                GridViewTable.Cell(j + 2, 2).Range.Text = gcl[1];
                                GridViewTable.Cell(j + 2, 3).Range.Text = tablename;
                                GridViewTable.Cell(j + 2, 4).Range.Text = tablename;//getFieldDescription(gcl[0], tablename);
                            }
                            //原本为WinForm使用的，在WinForm也抓Caption的时候这个没有用处了
                            else
                            {
                                if (gridColumnList[j].GetType() == typeof(string))
                                {
                                    GridViewTable.Cell(j + 2, 1).Range.Text = (string)gridColumnList[j];
                                    GridViewTable.Cell(j + 2, 2).Range.Text = (string)gridColumnList[j]; //getFieldCaption((string)gridColumnList[j], tablename);
                                    GridViewTable.Cell(j + 2, 3).Range.Text = tablename;
                                    GridViewTable.Cell(j + 2, 4).Range.Text = tablename;//getFieldDescription((string)gridColumnList[j], tablename);
                                }
                            }
                            //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }

                //Default value and validate table
                if (DefaultValidateList != null && DefaultValidateList.Length != 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". " + GetSystemMessage(language, "SDModule", "Document", "default"));
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    Table DefaultvalidateTable = oDoc.Tables.Add(selection.Range, DefaultValidateList.Length + 1, 8, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    DefaultvalidateTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    DefaultvalidateTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    DefaultvalidateTable.Rows[1].Range.Font.Bold = 2;
                    string defaultdecsridetail = GetSystemMessage(language, "SDModule", "Document", "defaultdetail");
                    string[] defaultdecdetailt = defaultdecsridetail.Split(new char[] { ',' });
                    if (defaultdecdetailt.Length == 8)
                    {
                        DefaultvalidateTable.Cell(1, 1).Range.Text = defaultdecdetailt[0];
                        DefaultvalidateTable.Cell(1, 2).Range.Text = defaultdecdetailt[1];
                        DefaultvalidateTable.Cell(1, 3).Range.Text = defaultdecdetailt[2];
                        DefaultvalidateTable.Cell(1, 4).Range.Text = defaultdecdetailt[3];
                        DefaultvalidateTable.Cell(1, 5).Range.Text = defaultdecdetailt[4];
                        DefaultvalidateTable.Cell(1, 6).Range.Text = defaultdecdetailt[5];
                        DefaultvalidateTable.Cell(1, 7).Range.Text = defaultdecdetailt[6];
                        DefaultvalidateTable.Cell(1, 8).Range.Text = defaultdecdetailt[7];
                    }
                    else
                    {
                        DefaultvalidateTable.Cell(1, 1).Range.Text = "BindingSource";
                        DefaultvalidateTable.Cell(1, 2).Range.Text = "Field Name";
                        DefaultvalidateTable.Cell(1, 3).Range.Text = "Check Null";
                        DefaultvalidateTable.Cell(1, 4).Range.Text = "Validate";
                        DefaultvalidateTable.Cell(1, 5).Range.Text = "Check Range From";
                        DefaultvalidateTable.Cell(1, 6).Range.Text = "Check Range To";
                        DefaultvalidateTable.Cell(1, 7).Range.Text = "CarryOn";
                        DefaultvalidateTable.Cell(1, 8).Range.Text = "Default Value";
                    }
                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    for (int i = 0; i < DefaultValidateList.Length; i++)
                    {
                        object[] DefaultValidate = (object[])DefaultValidateList[i];
                        DefaultvalidateTable.Cell(i + 2, 1).Range.Text = (string)DefaultValidate[0];
                        DefaultvalidateTable.Cell(i + 2, 2).Range.Text = (string)DefaultValidate[1];
                        DefaultvalidateTable.Cell(i + 2, 3).Range.Text = (string)DefaultValidate[2];
                        DefaultvalidateTable.Cell(i + 2, 4).Range.Text = (string)DefaultValidate[3];
                        DefaultvalidateTable.Cell(i + 2, 5).Range.Text = (string)DefaultValidate[4];
                        DefaultvalidateTable.Cell(i + 2, 6).Range.Text = (string)DefaultValidate[5];
                        DefaultvalidateTable.Cell(i + 2, 7).Range.Text = (string)DefaultValidate[6];
                        DefaultvalidateTable.Cell(i + 2, 8).Range.Text = (string)DefaultValidate[7];
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    }
                    DefaultvalidateTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }

                //InfoCommand Component table
                if (CommandList != null && CommandList.Length != 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". InfoCommand " + GetSystemMessage(language, "SDModule", "Document", "component") + ":");
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    Table CommandTable = oDoc.Tables.Add(selection.Range, CommandList.Length + 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    CommandTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    CommandTable.Columns[1].PreferredWidth = 25;
                    CommandTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    CommandTable.Columns[2].PreferredWidth = 100;
                    CommandTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    CommandTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    CommandTable.Rows[1].Range.Font.Bold = 2;
                    string infocommanddetail = GetSystemMessage(language, "SDModule", "Document", "infocommanddetail");
                    string[] infocommanddetailt = infocommanddetail.Split(new char[] { ',' });
                    if (infocommanddetailt.Length == 2)
                    {
                        CommandTable.Cell(1, 1).Range.Text = infocommanddetailt[0];
                        CommandTable.Cell(1, 2).Range.Text = infocommanddetailt[1];
                    }
                    else
                    {
                        CommandTable.Cell(1, 1).Range.Text = "Name";
                        CommandTable.Cell(1, 2).Range.Text = "Command Text";
                    }
                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    CommandTable.Range.Font.Size = 9;
                    CommandTable.Rows[1].Range.Font.Size = 10;
                    for (int i = 0; i < CommandList.Length; i++)
                    {
                        object[] Command = (object[])CommandList[i];
                        CommandTable.Cell(i + 2, 1).Range.Text = (string)Command[0];
                        string s = (string)Command[1];
                        s = s.Replace(",", ", ");
                        CommandTable.Cell(i + 2, 2).Range.Text = s;
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    }
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }

                //UpdateComponent table
                if (UpdateList != null && UpdateList.Length != 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". UpdateComponent " + GetSystemMessage(language, "SDModule", "Document", "component") + ":");
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    int UpdateCount = 0;
                    for (int i = 0; i < UpdateList.Length; i++)
                    {
                        object[] fUpdateList = (object[])UpdateList[i];
                        if (((object[])fUpdateList[1]) != null)
                        { UpdateCount += ((object[])fUpdateList[1]).Length; }
                    }
                    Table UpdateComponentTable = oDoc.Tables.Add(selection.Range, UpdateCount + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    UpdateComponentTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    UpdateComponentTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    UpdateComponentTable.Rows[1].Range.Font.Bold = 2;
                    string updatecompomentdetail = GetSystemMessage(language, "SDModule", "Document", "updatecompomentdetail");
                    string[] updatecompomentdetailt = updatecompomentdetail.Split(new char[] { ',' });
                    if (updatecompomentdetailt.Length == 4)
                    {
                        UpdateComponentTable.Cell(1, 1).Range.Text = updatecompomentdetailt[0];
                        UpdateComponentTable.Cell(1, 2).Range.Text = updatecompomentdetailt[1];
                        UpdateComponentTable.Cell(1, 3).Range.Text = updatecompomentdetailt[2];
                        UpdateComponentTable.Cell(1, 4).Range.Text = updatecompomentdetailt[3];
                    }
                    else
                    {
                        UpdateComponentTable.Cell(1, 1).Range.Text = "Name";
                        UpdateComponentTable.Cell(1, 2).Range.Text = "Field Name";
                        UpdateComponentTable.Cell(1, 3).Range.Text = "Default Mode";
                        UpdateComponentTable.Cell(1, 4).Range.Text = "Default Value";
                        UpdateComponentTable.Cell(1, 5).Range.Text = "Check Null";
                    }
                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    for (int i = 0; i < UpdateList.Length; i++)
                    {
                        object[] UpdateComponent = (object[])UpdateList[i];
                        for (int j = 0; j < ((object[])UpdateComponent[1]).Length; j++)
                        {
                            UpdateComponentTable.Cell(tablecount + 2, 1).Range.Text = (string)UpdateComponent[0];
                            object[] fUpdateComponent = (object[])((object[])UpdateComponent[1])[j];
                            UpdateComponentTable.Cell(tablecount + 2, 2).Range.Text = (string)fUpdateComponent[0];
                            UpdateComponentTable.Cell(tablecount + 2, 3).Range.Text = (string)fUpdateComponent[1];
                            UpdateComponentTable.Cell(tablecount + 2, 4).Range.Text = (string)fUpdateComponent[2];
                            UpdateComponentTable.Cell(tablecount + 2, 5).Range.Text = (string)fUpdateComponent[3];
                            //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                            tablecount++;
                        }
                    }
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                    tablecount = 0;
                }

                if (CodeList.Length > 0)
                {
                    //if (_MDIset.documentsetting[9])
                    //{
                    object[] clientCode = (object[])CodeList[0];

                    //client code
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ".  " + GetSystemMessage(language, "SDModule", "Document", "code") + ":");
                    captioncount++;
                    selection.TypeParagraph();
                    if (clientCode != null)
                    {
                        selection.TypeText("[" + (string)clientCode[0] + "]");
                        selection.TypeParagraph();
                        selection.Font.Bold = 0;
                        selection.Font.Size = 9;
                        selection.TypeText((string)clientCode[1]);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                    //}
                    //if (_MDIset.documentsetting[10])
                    //{
                    object[] datamoduleCode = (object[])CodeList[CodeList.Length - 1];

                    //datamodule code
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    //selection.TypeText(captioncount.ToString() + ". DataModule " + GetSystemMessage(language, "SDModule", "Document", "code") + ":");
                    captioncount++;
                    selection.TypeParagraph();
                    if (datamoduleCode != null)
                    {
                        selection.TypeText("[" + (string)datamoduleCode[0] + "]");
                        selection.TypeParagraph();
                        selection.Font.Bold = 0;
                        selection.Font.Size = 9;
                        selection.TypeText((string)datamoduleCode[1]);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                //}
                object type = Word.WdBreakType.wdPageBreak;
                selection.InsertBreak(ref type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void PrintOtherPageForJQ(_Document oDoc, _Application oWord, object[] objAll)
        {
            try
            {
                object[] tablelist = (object[])objAll[1];
                List<JQDataGridPrint> GridColumnList = (List<JQDataGridPrint>)objAll[2];
                List<JQDataFormPrint> formList = (List<JQDataFormPrint>)objAll[3];
                object[] DataModuleFileList = (object[])objAll[4];
                object[] DefaultValidateList = (object[])objAll[5];
                object[] CommandList = (object[])objAll[6];
                object[] UpdateList = (object[])objAll[7];
                object[] CodeList = (object[])objAll[8];
                List<JQDefaultPrint> DefaultList = null;
                List<JQValidatePrint> ValidateList = null;
                if (objAll.Length > 9)
                {
                    DefaultList = (List<JQDefaultPrint>)objAll[9];
                    ValidateList = (List<JQValidatePrint>)objAll[10];
                }
                Word.Selection selection = oWord.Selection;
                object WdLine = Word.WdUnits.wdLine;
                object Wdstory = Word.WdUnits.wdStory;
                object WDCount1 = 1;
                object oMissing = System.Reflection.Missing.Value;
                object WDWord9TableBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object WDAutoFitWindow = WdAutoFitBehavior.wdAutoFitWindow;
                int tablecount = 0;
                int captioncount = 1;
                selection.EndKey(ref Wdstory, ref oMissing);
                //Add ServerPicture
                //if (_MDIset.documentsetting[1])
                //{
                //    foreach (string sfile in DataModuleFileList)
                //    {
                //        object LinkToFile = false;
                //        object SaveWithDocument = true;
                //        selection.InlineShapes.AddPicture(sfile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                //        selection.TypeParagraph();
                //    }
                //    selection.TypeParagraph();
                //}

                //grdView
                if (GridColumnList != null && GridColumnList.Count != 0)
                {
                    //Srvtools.CliUtils.GetTableName("S01", "Master", "Solution1");
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + "." + GetSystemMessage(language, "SDModule", "Document", "griddescri"));
                    captioncount++;
                    selection.TypeParagraph();
                    for (int i = 0; i < GridColumnList.Count; i++)
                    {
                        selection.Font.Bold = 2;
                        selection.Font.Size = 12;
                        JQDataGridPrint grid = GridColumnList[i];
                        selection.TypeText("【" + grid.ID + "】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();
                        //datagrid property
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 5, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "DataMember";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = grid.RemoteName;
                        GridViewTable.Cell(1, 4).Range.Text = grid.DataMember;
                        GridViewTable.Cell(2, 1).Range.Text = "Title";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "AutoApply";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = grid.Title;
                        GridViewTable.Cell(2, 4).Range.Text = grid.AutoApply;
                        GridViewTable.Cell(3, 1).Range.Text = "AlwaysClose";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "Pagination";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = grid.AlwaysClose;
                        GridViewTable.Cell(3, 4).Range.Text = grid.Pagination;
                        GridViewTable.Cell(4, 1).Range.Text = "PageSize";
                        GridViewTable.Cell(4, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 3).Range.Text = "QueryAutoColumn";
                        GridViewTable.Cell(4, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 2).Range.Text = grid.PageSize;
                        GridViewTable.Cell(4, 4).Range.Text = grid.QueryAutoColumn;
                        GridViewTable.Cell(5, 1).Range.Text = "DuplicateCheck";
                        GridViewTable.Cell(5, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(5, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(5, 3).Range.Text = "";
                        GridViewTable.Cell(5, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(5, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(5, 2).Range.Text = grid.DuplicateCheck;
                        GridViewTable.Cell(5, 4).Range.Text = "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        //columns
                        List<JQDataGridColumnsPrint> gridColumnList = grid.JQDataGridColumnsPrintList;
                        GridViewTable = oDoc.Tables.Add(selection.Range, gridColumnList.Count + 1, 8, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;
                        string fielddecsridetail = GetSystemMessage(language, "SDModule", "Document", "jqfielddescridetail");
                        string[] fielddecdetailt = fielddecsridetail.Split(new char[] { ',' });
                        if (fielddecdetailt.Length == 8)
                        {
                            GridViewTable.Cell(1, 1).Range.Text = fielddecdetailt[0];
                            GridViewTable.Cell(1, 2).Range.Text = fielddecdetailt[1];
                            GridViewTable.Cell(1, 3).Range.Text = fielddecdetailt[2];
                            GridViewTable.Cell(1, 4).Range.Text = fielddecdetailt[3];
                            GridViewTable.Columns[4].PreferredWidth = 10;
                            GridViewTable.Cell(1, 5).Range.Text = fielddecdetailt[4];
                            GridViewTable.Cell(1, 6).Range.Text = fielddecdetailt[5];
                            GridViewTable.Cell(1, 7).Range.Text = fielddecdetailt[6];
                            GridViewTable.Cell(1, 8).Range.Text = fielddecdetailt[7];
                        }
                        else
                        {
                            GridViewTable.Cell(1, 1).Range.Text = "FieldName";
                            GridViewTable.Cell(1, 2).Range.Text = "Caption";
                            GridViewTable.Cell(1, 3).Range.Text = "Editor";
                            GridViewTable.Cell(1, 4).Range.Text = "EditorOptions";
                            GridViewTable.Columns[4].PreferredWidth = 10;
                            GridViewTable.Cell(1, 5).Range.Text = "Format";
                            GridViewTable.Cell(1, 6).Range.Text = "Sortable";
                            GridViewTable.Cell(1, 7).Range.Text = "Frozen";
                            GridViewTable.Cell(1, 8).Range.Text = "Total";
                        }
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        for (int j = 0; j < gridColumnList.Count; j++)
                        {
                            JQDataGridColumnsPrint field = gridColumnList[j];
                            GridViewTable.Cell(j + 2, 1).Range.Text = field.DataField;
                            GridViewTable.Cell(j + 2, 2).Range.Text = field.HeaderText;
                            GridViewTable.Cell(j + 2, 3).Range.Text = field.Editor;
                            GridViewTable.Cell(j + 2, 4).Range.Text = field.EditorOption;
                            GridViewTable.Cell(j + 2, 5).Range.Text = field.Format;
                            GridViewTable.Cell(j + 2, 6).Range.Text = field.Sortable;
                            GridViewTable.Cell(j + 2, 7).Range.Text = field.Frozen;
                            GridViewTable.Cell(j + 2, 8).Range.Text = field.Total;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        //query columns
                        if (grid.JQDataGridQueryColumnsPrintList.Count > 0)
                        {
                            selection.TypeText("【Query Columns】");
                            selection.TypeParagraph();

                            Table GridViewQueryFieldTable = oDoc.Tables.Add(selection.Range, grid.JQDataGridQueryColumnsPrintList.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewQueryFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                            GridViewQueryFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewQueryFieldTable.Rows[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewQueryFieldTable.Rows[1].Range.Font.Bold = 2;
                            string gridquerycolumn = GetSystemMessage(language, "SDModule", "Document", "jqgridquerycolumn");
                            string[] gridquerycolumns = gridquerycolumn.Split(new char[] { ',' });
                            if (gridquerycolumns.Length == 7)
                            {
                                GridViewQueryFieldTable.Cell(1, 1).Range.Text = gridquerycolumns[0];
                                GridViewQueryFieldTable.Cell(1, 2).Range.Text = gridquerycolumns[1];
                                GridViewQueryFieldTable.Cell(1, 3).Range.Text = gridquerycolumns[2];
                                GridViewQueryFieldTable.Cell(1, 4).Range.Text = gridquerycolumns[3];
                                GridViewQueryFieldTable.Columns[4].PreferredWidth = 10;
                                GridViewQueryFieldTable.Cell(1, 5).Range.Text = gridquerycolumns[4];
                                GridViewQueryFieldTable.Cell(1, 6).Range.Text = gridquerycolumns[5];
                                GridViewQueryFieldTable.Cell(1, 7).Range.Text = gridquerycolumns[6];
                            }
                            else
                            {
                                GridViewQueryFieldTable.Cell(1, 1).Range.Text = "FieldName";
                                GridViewQueryFieldTable.Cell(1, 2).Range.Text = "Caption";
                                GridViewQueryFieldTable.Cell(1, 3).Range.Text = "Condition";
                                GridViewQueryFieldTable.Cell(1, 4).Range.Text = "Editor";
                                GridViewQueryFieldTable.Cell(1, 5).Range.Text = "DefaultValue";
                                GridViewQueryFieldTable.Cell(1, 6).Range.Text = "AndOr";
                                GridViewQueryFieldTable.Cell(1, 7).Range.Text = "NewLine";
                            }
                            for (int j = 0; j < grid.JQDataGridQueryColumnsPrintList.Count; j++)
                            {
                                JQDataGridQueryColumnsPrint field = grid.JQDataGridQueryColumnsPrintList[j];
                                GridViewQueryFieldTable.Cell(j + 2, 1).Range.Text = field.DataField;
                                GridViewQueryFieldTable.Cell(j + 2, 2).Range.Text = field.HeaderText;
                                GridViewQueryFieldTable.Cell(j + 2, 3).Range.Text = field.Condition;
                                GridViewQueryFieldTable.Cell(j + 2, 4).Range.Text = field.Editor;
                                GridViewQueryFieldTable.Cell(j + 2, 5).Range.Text = field.DefaultValue;
                                GridViewQueryFieldTable.Cell(j + 2, 6).Range.Text = field.AndOr;
                                GridViewQueryFieldTable.Cell(j + 2, 7).Range.Text = field.NewLine;
                            }
                            GridViewQueryFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();

                        }
                        //ToolItems
                        if (grid.JQDataGridToolItemsPrintList.Count > 0)
                        {
                            selection.TypeText("【Tool Items】");
                            selection.TypeParagraph();

                            Table GridViewtooItemsTable = oDoc.Tables.Add(selection.Range, grid.JQDataGridToolItemsPrintList.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewtooItemsTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                            GridViewtooItemsTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewtooItemsTable.Rows[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewtooItemsTable.Rows[1].Range.Font.Bold = 2;
                            string gridtoolitem = GetSystemMessage(language, "SDModule", "Document", "jqgridtoolitem");
                            string[] gridtoolitems = gridtoolitem.Split(new char[] { ',' });
                            if (gridtoolitems.Length == 7)
                            {
                                GridViewtooItemsTable.Cell(1, 1).Range.Text = gridtoolitems[0];
                                GridViewtooItemsTable.Cell(1, 2).Range.Text = gridtoolitems[1];
                                GridViewtooItemsTable.Cell(1, 3).Range.Text = gridtoolitems[2];
                                GridViewtooItemsTable.Cell(1, 4).Range.Text = gridtoolitems[3];
                                GridViewtooItemsTable.Columns[4].PreferredWidth = 10;
                                GridViewtooItemsTable.Cell(1, 5).Range.Text = gridtoolitems[4];
                            }
                            else
                            {
                                GridViewtooItemsTable.Cell(1, 1).Range.Text = "ID";
                                GridViewtooItemsTable.Cell(1, 2).Range.Text = "Icon";
                                GridViewtooItemsTable.Cell(1, 3).Range.Text = "ItemType";
                                GridViewtooItemsTable.Cell(1, 4).Range.Text = "Text";
                                GridViewtooItemsTable.Cell(1, 5).Range.Text = "OnClick";
                            }
                            for (int j = 0; j < grid.JQDataGridToolItemsPrintList.Count; j++)
                            {
                                JQDataGridToolItemsPrint field = grid.JQDataGridToolItemsPrintList[j];
                                GridViewtooItemsTable.Cell(j + 2, 1).Range.Text = field.ID;
                                GridViewtooItemsTable.Cell(j + 2, 2).Range.Text = field.Icon;
                                GridViewtooItemsTable.Cell(j + 2, 3).Range.Text = field.ItemType;
                                GridViewtooItemsTable.Cell(j + 2, 4).Range.Text = field.Text;
                                GridViewtooItemsTable.Cell(j + 2, 5).Range.Text = field.OnClick;
                            }
                            GridViewtooItemsTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();

                        }
                        selection.TypeParagraph();
                    }
                }

                if (formList != null && formList.Count > 0)
                {
                    //Field Table
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + "." + GetSystemMessage(language, "SDModule", "Document", "formdescri"));
                    captioncount++;
                    selection.TypeParagraph();
                    for (int i = 0; i < formList.Count; i++)
                    {
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        JQDataFormPrint form = formList[i];
                        selection.TypeText(form.ID + GetSystemMessage(language, "SDModule", "Document", "fielddescri"));
                        selection.TypeParagraph();
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 3, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "DataMember";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = form.RemoteName;
                        GridViewTable.Cell(1, 4).Range.Text = form.DataMember;
                        GridViewTable.Cell(2, 1).Range.Text = "IsShowFlowIcon";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "DuplicateCheck";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = form.IsShowFlowIcon;
                        GridViewTable.Cell(2, 4).Range.Text = form.DuplicateCheck;
                        GridViewTable.Cell(3, 1).Range.Text = "ValidateStyle";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "ContinueAdd";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = form.ValidateStyle;
                        GridViewTable.Cell(3, 4).Range.Text = form.ContinueAdd;
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        Table fieldTable = oDoc.Tables.Add(selection.Range, form.JQDataFormColumnsPrintList.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        fieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        fieldTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        fieldTable.Rows[1].Range.Font.Bold = 2;
                        string fielddecsridetail = GetSystemMessage(language, "SDModule", "Document", "jqformcolumn");
                        string[] fielddecdetailt = fielddecsridetail.Split(new char[] { ',' });
                        if (fielddecdetailt.Length > 4)
                        {
                            fieldTable.Cell(1, 1).Range.Text = fielddecdetailt[0];
                            fieldTable.Cell(1, 2).Range.Text = fielddecdetailt[1];
                            fieldTable.Cell(1, 3).Range.Text = fielddecdetailt[2];
                            fieldTable.Cell(1, 4).Range.Text = fielddecdetailt[3];
                            fieldTable.Cell(1, 5).Range.Text = fielddecdetailt[4];
                        }
                        else
                        {
                            fieldTable.Cell(1, 1).Range.Text = "FieldName";
                            fieldTable.Cell(1, 2).Range.Text = "Caption";
                            fieldTable.Cell(1, 3).Range.Text = "Editor";
                            fieldTable.Cell(1, 4).Range.Text = "EditorOptions";
                            fieldTable.Cell(1, 5).Range.Text = "Format";
                        }
                        for (int j = 0; j < form.JQDataFormColumnsPrintList.Count; j++)
                        {
                            JQDataFormColumnsPrint field = form.JQDataFormColumnsPrintList[j];
                            fieldTable.Cell(j + 2, 1).Range.Text = field.DataField;
                            fieldTable.Cell(j + 2, 2).Range.Text = field.HeaderText;
                            fieldTable.Cell(j + 2, 3).Range.Text = field.Editor;
                            fieldTable.Cell(j + 2, 4).Range.Text = field.EditorOption;
                            fieldTable.Cell(j + 2, 5).Range.Text = field.Format;
                        }
                        fieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }

                if (DefaultList != null && DefaultList.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". " + GetSystemMessage(language, "SDModule", "Document", "default"));
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < DefaultList.Count; i++)
                    {
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        JQDefaultPrint defaultp = DefaultList[i];
                        selection.TypeText(defaultp.ID + GetSystemMessage(language, "SDModule", "Document", "fielddescri"));
                        selection.TypeParagraph();
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "BindingOjbectID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = defaultp.BindingObjectID;
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        //selection.TypeParagraph();

                        Table DefaultvalidateTable = oDoc.Tables.Add(selection.Range, defaultp.JQDefaultItemPrintList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        DefaultvalidateTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        DefaultvalidateTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        DefaultvalidateTable.Rows[1].Range.Font.Bold = 2;
                        string defaultdecsridetail = GetSystemMessage(language, "SDModule", "Document", "jqdefaultdetail");
                        string[] defaultdecdetailt = defaultdecsridetail.Split(new char[] { ',' });
                        if (defaultdecdetailt.Length == 3)
                        {
                            DefaultvalidateTable.Cell(1, 1).Range.Text = defaultdecdetailt[0];
                            DefaultvalidateTable.Cell(1, 2).Range.Text = defaultdecdetailt[1];
                            DefaultvalidateTable.Cell(1, 3).Range.Text = defaultdecdetailt[2];
                        }
                        else
                        {
                            DefaultvalidateTable.Cell(1, 1).Range.Text = "Field Name";
                            DefaultvalidateTable.Cell(1, 2).Range.Text = "CarryOn";
                            DefaultvalidateTable.Cell(1, 3).Range.Text = "Default Value";
                        }
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        for (int j = 0; j < defaultp.JQDefaultItemPrintList.Count; j++)
                        {
                            JQDefaultItemPrint Default = defaultp.JQDefaultItemPrintList[j];
                            DefaultvalidateTable.Cell(i + 2, 1).Range.Text = Default.FieldName;
                            DefaultvalidateTable.Cell(i + 2, 2).Range.Text = Default.CarryOn;
                            DefaultvalidateTable.Cell(i + 2, 3).Range.Text = Default.DefaultValue;
                            //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        }
                        DefaultvalidateTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                if (ValidateList != null && ValidateList.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". " + GetSystemMessage(language, "SDModule", "Document", "validate"));
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < ValidateList.Count; i++)
                    {
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        JQValidatePrint validate = ValidateList[i];

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "BindingOjbectID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = validate.BindingObjectID;
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        //selection.TypeParagraph();

                        Table DefaultvalidateTable = oDoc.Tables.Add(selection.Range, validate.JQValidateItemPrintList.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        DefaultvalidateTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        DefaultvalidateTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        DefaultvalidateTable.Rows[1].Range.Font.Bold = 2;
                        string defaultdecsridetail = GetSystemMessage(language, "SDModule", "Document", "jqvalidatedetail");
                        string[] defaultdecdetailt = defaultdecsridetail.Split(new char[] { ',' });
                        if (defaultdecdetailt.Length == 7)
                        {
                            DefaultvalidateTable.Cell(1, 1).Range.Text = defaultdecdetailt[0];
                            DefaultvalidateTable.Cell(1, 2).Range.Text = defaultdecdetailt[1];
                            DefaultvalidateTable.Cell(1, 3).Range.Text = defaultdecdetailt[2];
                            DefaultvalidateTable.Cell(1, 4).Range.Text = defaultdecdetailt[3];
                            DefaultvalidateTable.Cell(1, 5).Range.Text = defaultdecdetailt[4];
                            DefaultvalidateTable.Cell(1, 6).Range.Text = defaultdecdetailt[5];
                            DefaultvalidateTable.Cell(1, 7).Range.Text = defaultdecdetailt[6];
                        }
                        else
                        {
                            DefaultvalidateTable.Cell(1, 1).Range.Text = "Field Name";
                            DefaultvalidateTable.Cell(1, 2).Range.Text = "Check Null";
                            DefaultvalidateTable.Cell(1, 3).Range.Text = "ValidateType";
                            DefaultvalidateTable.Cell(1, 4).Range.Text = "Check Range From";
                            DefaultvalidateTable.Cell(1, 5).Range.Text = "Check Range To";
                            DefaultvalidateTable.Cell(1, 6).Range.Text = "CheckMethod";
                            DefaultvalidateTable.Cell(1, 7).Range.Text = "Message";
                        }
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        for (int j = 0; j < validate.JQValidateItemPrintList.Count; j++)
                        {
                            JQValidateItemPrint Validate = validate.JQValidateItemPrintList[j];
                            DefaultvalidateTable.Cell(i + 2, 1).Range.Text = Validate.FieldName;
                            DefaultvalidateTable.Cell(i + 2, 2).Range.Text = Validate.CheckNull;
                            DefaultvalidateTable.Cell(i + 2, 3).Range.Text = Validate.ValidateType;
                            DefaultvalidateTable.Cell(i + 2, 4).Range.Text = Validate.CheckRangeFrom;
                            DefaultvalidateTable.Cell(i + 2, 5).Range.Text = Validate.CheckRangeTo;
                            DefaultvalidateTable.Cell(j + 2, 6).Range.Text = Validate.CheckMethod;
                            DefaultvalidateTable.Cell(j + 2, 7).Range.Text = Validate.Message;
                            //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        }
                        DefaultvalidateTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }

                //InfoCommand Component table
                if (CommandList != null && CommandList.Length != 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". InfoCommand " + GetSystemMessage(language, "SDModule", "Document", "component") + ":");
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    Table CommandTable = oDoc.Tables.Add(selection.Range, CommandList.Length + 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    CommandTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    CommandTable.Columns[1].PreferredWidth = 25;
                    CommandTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    CommandTable.Columns[2].PreferredWidth = 100;
                    CommandTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    CommandTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    CommandTable.Rows[1].Range.Font.Bold = 2;
                    string infocommanddetail = GetSystemMessage(language, "SDModule", "Document", "infocommanddetail");
                    string[] infocommanddetailt = infocommanddetail.Split(new char[] { ',' });
                    if (infocommanddetailt.Length == 2)
                    {
                        CommandTable.Cell(1, 1).Range.Text = infocommanddetailt[0];
                        CommandTable.Cell(1, 2).Range.Text = infocommanddetailt[1];
                    }
                    else
                    {
                        CommandTable.Cell(1, 1).Range.Text = "Name";
                        CommandTable.Cell(1, 2).Range.Text = "Command Text";
                    }
                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    CommandTable.Range.Font.Size = 9;
                    CommandTable.Rows[1].Range.Font.Size = 10;
                    for (int i = 0; i < CommandList.Length; i++)
                    {
                        object[] Command = (object[])CommandList[i];
                        CommandTable.Cell(i + 2, 1).Range.Text = (string)Command[0];
                        string s = (string)Command[1];
                        s = s.Replace(",", ", ");
                        CommandTable.Cell(i + 2, 2).Range.Text = s;
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    }
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }

                //UpdateComponent table
                if (UpdateList != null && UpdateList.Length != 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". UpdateComponent " + GetSystemMessage(language, "SDModule", "Document", "component") + ":");
                    captioncount++;
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    int UpdateCount = 0;
                    for (int i = 0; i < UpdateList.Length; i++)
                    {
                        object[] fUpdateList = (object[])UpdateList[i];
                        if (((object[])fUpdateList[1]) != null)
                        { UpdateCount += ((object[])fUpdateList[1]).Length; }
                    }
                    Table UpdateComponentTable = oDoc.Tables.Add(selection.Range, UpdateCount + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    UpdateComponentTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    UpdateComponentTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    UpdateComponentTable.Rows[1].Range.Font.Bold = 2;
                    string updatecompomentdetail = GetSystemMessage(language, "SDModule", "Document", "updatecompomentdetail");
                    string[] updatecompomentdetailt = updatecompomentdetail.Split(new char[] { ',' });
                    if (updatecompomentdetailt.Length == 4)
                    {
                        UpdateComponentTable.Cell(1, 1).Range.Text = updatecompomentdetailt[0];
                        UpdateComponentTable.Cell(1, 2).Range.Text = updatecompomentdetailt[1];
                        UpdateComponentTable.Cell(1, 3).Range.Text = updatecompomentdetailt[2];
                        UpdateComponentTable.Cell(1, 4).Range.Text = updatecompomentdetailt[3];
                    }
                    else
                    {
                        UpdateComponentTable.Cell(1, 1).Range.Text = "Name";
                        UpdateComponentTable.Cell(1, 2).Range.Text = "Field Name";
                        UpdateComponentTable.Cell(1, 3).Range.Text = "Default Mode";
                        UpdateComponentTable.Cell(1, 4).Range.Text = "Default Value";
                        UpdateComponentTable.Cell(1, 5).Range.Text = "Check Null";
                    }
                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    for (int i = 0; i < UpdateList.Length; i++)
                    {
                        object[] UpdateComponent = (object[])UpdateList[i];
                        for (int j = 0; j < ((object[])UpdateComponent[1]).Length; j++)
                        {
                            UpdateComponentTable.Cell(tablecount + 2, 1).Range.Text = (string)UpdateComponent[0];
                            object[] fUpdateComponent = (object[])((object[])UpdateComponent[1])[j];
                            UpdateComponentTable.Cell(tablecount + 2, 2).Range.Text = (string)fUpdateComponent[0];
                            UpdateComponentTable.Cell(tablecount + 2, 3).Range.Text = (string)fUpdateComponent[1];
                            UpdateComponentTable.Cell(tablecount + 2, 4).Range.Text = (string)fUpdateComponent[2];
                            UpdateComponentTable.Cell(tablecount + 2, 5).Range.Text = (string)fUpdateComponent[3];
                            //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                            tablecount++;
                        }
                    }
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                    tablecount = 0;
                }

                if (CodeList != null && CodeList.Length > 0)
                {
                    //client code
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(captioncount.ToString() + ". " + GetSystemMessage(language, "SDModule", "Document", "code") + ":");
                    captioncount++;
                    selection.TypeParagraph();
                    foreach (object[] clientCode in CodeList)
                    {
                        if (clientCode != null)
                        {
                            selection.Font.Bold = 2;
                            selection.Font.Size = 14;
                            selection.TypeText("[" + (string)clientCode[0] + "]");
                            selection.TypeParagraph();
                            selection.Font.Bold = 0;
                            selection.Font.Size = 9;
                            selection.TypeText((string)clientCode[1]);
                            selection.TypeParagraph();
                            selection.TypeParagraph();
                        }
                    }
                }

                //}
                object type = Word.WdBreakType.wdPageBreak;
                selection.InsertBreak(ref type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void PrintSecondPage(_Document oDoc, _Application oWord, object[] objDes, object[] objAll, ref int bookMarkCount)
        {
            try
            {
                //Add FormTable
                Word.Selection selection = oWord.Selection;
                object oMissing = System.Reflection.Missing.Value;
                object WdLine = Word.WdUnits.wdLine;
                object WDWord9TableBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object WDAutoFitWindow = WdAutoFitBehavior.wdAutoFitWindow;
                Table formTable = oDoc.Tables.Add(selection.Range, 2, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                selection.SelectColumn();
                selection.Cells.Merge();
                selection.Columns.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                selection.Columns.PreferredWidth = 13;
                selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                selection.Cells.Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                object WDCount1 = 1;
                object wdCharacter = WdUnits.wdCharacter;
                selection.MoveRight(ref wdCharacter, ref WDCount1, ref oMissing);
                selection.SelectColumn();
                selection.Cells.Merge();
                selection.Columns.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                selection.Columns.PreferredWidth = 50;
                selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                selection.MoveRight(ref wdCharacter, ref WDCount1, ref oMissing);
                selection.SelectColumn();
                selection.Columns.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                selection.Columns.PreferredWidth = 13;
                selection.Columns.Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                selection.MoveRight(ref wdCharacter, ref WDCount1, ref oMissing);
                selection.Columns.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                selection.Columns.PreferredWidth = 24;
                selection.MoveRight(ref wdCharacter, ref WDCount1, ref oMissing);
                selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                formTable.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;//居中
                formTable.Range.Font.Size = 10;
                formTable.Cell(1, 1).Range.Text = (string)objDes[0];
                formTable.Cell(1, 1).Range.Font.Bold = 2;
                formTable.Cell(1, 2).Range.Text = (string)objDes[1];
                string formname = (string)objDes[1];
                formTable.Cell(1, 2).Range.Select();
                string bookmarkname = "goal_";
                if (formname.EndsWith(".cs"))
                {
                    bookmarkname += "cs_" + formname.Substring(0, formname.LastIndexOf('.'));
                }
                else if (formname.EndsWith(".aspx"))
                {
                    bookmarkname += "aspx_" + formname.Substring(0, formname.LastIndexOf('.'));
                }
                else if (formname.EndsWith(".dll"))
                {
                    bookmarkname += "dll_" + formname.Substring(0, formname.LastIndexOf('.'));
                }

                if (oDoc.Bookmarks.Exists(bookmarkname))
                {
                    bookmarkname = bookmarkname + "_" + bookMarkCount;
                    bookMarkCount++;
                }
                oDoc.Bookmarks.Add(bookmarkname, ref oMissing);

                formTable.Cell(1, 3).Range.Text = (string)objDes[2];
                formTable.Cell(1, 3).Range.Font.Bold = 2;
                formTable.Cell(1, 4).Range.Text = (string)objDes[3];
                formTable.Cell(2, 3).Range.Text = (string)objDes[4];
                formTable.Cell(2, 3).Range.Font.Bold = 2;
                formTable.Cell(2, 4).Range.Text = (string)objDes[5];
                selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);

                selection.TypeParagraph();
                selection.TypeParagraph();
                // Add picture
                if (documentOptions[0])
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("picture");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;

                    if (objAll[0] != null && objAll[0].ToString() != "")
                    {
                        string csJPGFile = (string)objAll[0];
                        object LinkToFile = false;
                        object SaveWithDocument = true;
                        selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                    }
                    selection.TypeParagraph();
                }
                if (documentOptions[1])
                {
                    if (objAll[4] != null && (objAll[4] as object[]).Length != 0)
                    {
                        for (int i = 0; i < (objAll[4] as object[]).Length; i++)
                        {
                            string csJPGFile = (string)((object[])objAll[4])[i];
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                    }
                    selection.TypeParagraph();
                }
                object type = Word.WdBreakType.wdPageBreak;
                selection.InsertBreak(ref type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static public string GetSystemMessage(SYS_LANGUAGE lang, string Module, string Comp, string sMsg)
        {
            string s = string.Format("{0}\\SDSYSmsg.xml", EEPRegistry.Server);
            if (System.IO.File.Exists(s))
            {
                XmlDocument msgXml = new XmlDocument();
                msgXml.Load(s);

                XmlNode aNode = msgXml.DocumentElement.SelectSingleNode(string.Format("{0}/{1}/{2}/{3}", Module, Comp, sMsg, lang));
                if (aNode != null)
                {
                    return aNode.InnerText;
                }
            }
            return string.Empty;
        }

        public static object PrintTableSchema(object[] paramters)
        {
            Word._Application oWord;
            Word._Document oDoc;
            string filepath = string.Empty;
            string filename = string.Empty;
            string filetype = string.Empty;
            DateTime dt;
            Selection selection;
            try
            {
                oWord = new Word.Application();
            }
            catch (Exception ex) { return new object[] { 1, ex.Message }; }
            try
            {
                oWord.Visible = true;

                #region Print Table Schema

                MemoryStream ms = new MemoryStream(paramters[0] as byte[]);
                PrintTempFilePath = paramters[1] as string;
                XmlDocument xml = new XmlDocument();
                xml.Load(ms);
                XmlNode root = xml.SelectSingleNode("PrintNodeXML");

                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing);
                object type = Word.WdBreakType.wdPageBreak;
                selection = oWord.Selection;
                selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                selection.set_Style(ref oHeadingStyle1);
                selection.Font.Bold = 2;
                selection.Font.Size = 14;
                selection.TypeText(GetSystemMessage(language, "SDModule", "Document", "SchemaList"));
                selection.TypeParagraph();
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    selection.set_Style(ref oHeadingStyle2);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 12;
                    selection.TypeText(root.ChildNodes[i].Attributes["xElementName"].Value);
                    selection.TypeParagraph();

                    int fieldcount = root.ChildNodes[i].ChildNodes[1].ChildNodes.Count;
                    Table fieldTable = oDoc.Tables.Add(selection.Range, fieldcount + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    fieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[1].PreferredWidth = 20;
                    fieldTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[2].PreferredWidth = 18;
                    fieldTable.Columns[3].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[3].PreferredWidth = 17;
                    fieldTable.Columns[4].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[4].PreferredWidth = 10;
                    fieldTable.Columns[5].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[5].PreferredWidth = 7;
                    fieldTable.Columns[6].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[6].PreferredWidth = 8;
                    fieldTable.Columns[7].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[7].PreferredWidth = 20;
                    fieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    fieldTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    fieldTable.Rows[1].Range.Font.Bold = 2;
                    string fieldtext = GetSystemMessage(language, "SDModule", "Document", "field");
                    //string fieldtext = "Name,Caption,DataType,Length,Scale,IsNullable,Remark";
                    string[] fieldt = fieldtext.Split(new char[] { ',' });
                    fieldTable.Cell(1, 1).Range.Text = fieldt[0];
                    fieldTable.Cell(1, 2).Range.Text = fieldt[1];
                    fieldTable.Cell(1, 3).Range.Text = fieldt[2];
                    fieldTable.Cell(1, 4).Range.Text = fieldt[3];
                    fieldTable.Cell(1, 5).Range.Text = fieldt[4];
                    fieldTable.Cell(1, 6).Range.Text = fieldt[5];
                    fieldTable.Cell(1, 7).Range.Text = fieldt[6];

                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    for (int j = 0; j < fieldcount; j++)
                    {
                        XmlAttributeCollection xac = root.ChildNodes[i].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes;
                        fieldTable.Cell(j + 2, 1).Range.Text = xac["IsKey"].Value.ToLower() == "true" ? xac["Name"].Value + "(*)" : xac["Name"].Value;
                        fieldTable.Cell(j + 2, 2).Range.Text = xac["Caption"].Value;
                        fieldTable.Cell(j + 2, 3).Range.Text = xac["DataType"].Value;
                        fieldTable.Cell(j + 2, 4).Range.Text = xac["Length"].Value;
                        fieldTable.Cell(j + 2, 5).Range.Text = xac["Scale"].Value;
                        fieldTable.Cell(j + 2, 6).Range.Text = xac["isNullable"].Value;
                        fieldTable.Cell(j + 2, 7).Range.Text = "";
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    }
                    selection.EndKey(ref Wdstory, ref oMissing);
                }
                selection.HomeKey(ref Wdstory, ref oMissing);
                object level3 = 3;
                oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                dt = DateTime.Now;
                filename = "TableSchema" + dt.ToString("yyyyMMddhhmmssfff");
                object path = PrintTempFilePath + @"\Schema" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                filepath = (string)path;
                filetype = "T";
                #endregion
            }
            catch (Exception ex)
            {
                return new object[] { 1, ex.Message };
            }
            finally
            {
                object closesave = Word.WdSaveOptions.wdDoNotSaveChanges;
                object option = Word.WdOriginalFormat.wdWordDocument;
                object f = false;
                //oWord.Quit(ref closesave, ref option, ref f);
            }
            if (filename != string.Empty)
            {
                byte[] filebyte = File.ReadAllBytes((string)filepath);
                return new object[] { 0, filebyte, filename, filetype, dt.ToString("yyyy-MM-dd HH:mm:ss") };
            }
            else
                return new object[] { 1, "No Print Success" };
        }


        static object oMissing = System.Reflection.Missing.Value;
        static object WDWord9TableBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
        static object WDAutoFitWindow = WdAutoFitBehavior.wdAutoFitWindow;
        static object WdLine = Word.WdUnits.wdLine;
        static object Wdstory = Word.WdUnits.wdStory;
        static object WdTable = Word.WdUnits.wdTable;
        static object WDCount1 = 1;
        static object oHeadingStyle1 = Word.WdBuiltinStyle.wdStyleHeading1;
        static object oHeadingStyle2 = Word.WdBuiltinStyle.wdStyleHeading2;
        static object oTrue = true;
        static object oFalse = false;

        static string PrintTempFilePath = EEPRegistry.Server + @"\SDModuleFile\";
        public static object RemotePrint(object[] paramters)
        {
            Word._Application oWord;
            Word._Document oDoc;
            string filepath = string.Empty;
            string filename = string.Empty;
            string filetype = string.Empty;
            DateTime dt;
            Selection selection;
            try
            {
                oWord = new Word.Application();
            }
            catch (Exception ex) { return new object[] { 1, ex.Message }; }
            try
            {
                if (((string)paramters[1]) == "P")
                {
                    #region Print Page
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Page List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadPage(pagexml, selection, oWord, oDoc, printSetting);
                        selection.InsertBreak(ref type);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "SilverlightPage" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"\Page" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "P";
                    #endregion
                }
                else if (((string)paramters[1]) == "S")
                {
                    #region Print Service
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 2)
                    {
                        printName.Add(paramters[i].ToString());
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 1] as byte[], 0, (paramters[i + 1] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Service  List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());

                        selection.TypeParagraph();
                        LoadService(pagexml, selection, oWord, oDoc, printSetting);
                        selection.InsertBreak(ref type);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "Service" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"\Service" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "S";
                    #endregion
                }
                else if (((string)paramters[1]) == "T")
                {
                    #region Print Table Schema

                    MemoryStream ms = new MemoryStream(paramters[2] as byte[]);
                    XmlDocument xml = new XmlDocument();
                    xml.Load(ms);
                    XmlNode root = xml.SelectSingleNode("PrintNodeXML");

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Word.WdBreakType.wdPageBreak;
                    selection = oWord.Selection;
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Table Schema List");
                    selection.TypeParagraph();
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        selection.set_Style(ref oHeadingStyle2);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 12;
                        selection.TypeText(root.ChildNodes[i].Attributes["xElementName"].Value);
                        selection.TypeParagraph();

                        int fieldcount = root.ChildNodes[i].ChildNodes[1].ChildNodes.Count;
                        Table fieldTable = oDoc.Tables.Add(selection.Range, fieldcount + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        fieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[1].PreferredWidth = 20;
                        fieldTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[2].PreferredWidth = 18;
                        fieldTable.Columns[3].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[3].PreferredWidth = 17;
                        fieldTable.Columns[4].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[4].PreferredWidth = 10;
                        fieldTable.Columns[5].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[5].PreferredWidth = 7;
                        fieldTable.Columns[6].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[6].PreferredWidth = 8;
                        fieldTable.Columns[7].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        fieldTable.Columns[7].PreferredWidth = 20;
                        fieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        fieldTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        fieldTable.Rows[1].Range.Font.Bold = 2;
                        //string fieldtext = SDSysMsg.GetSystemMessage(language, "SDModule", "Document", "field");
                        string fieldtext = "Name,Caption,DataType,Length,Scale,IsNullable,Remark";
                        string[] fieldt = fieldtext.Split(new char[] { ',' });
                        fieldTable.Cell(1, 1).Range.Text = fieldt[0];
                        fieldTable.Cell(1, 2).Range.Text = fieldt[1];
                        fieldTable.Cell(1, 3).Range.Text = fieldt[2];
                        fieldTable.Cell(1, 4).Range.Text = fieldt[3];
                        fieldTable.Cell(1, 5).Range.Text = fieldt[4];
                        fieldTable.Cell(1, 6).Range.Text = fieldt[5];
                        fieldTable.Cell(1, 7).Range.Text = fieldt[6];

                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        for (int j = 0; j < fieldcount; j++)
                        {
                            XmlAttributeCollection xac = root.ChildNodes[i].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes;
                            fieldTable.Cell(j + 2, 1).Range.Text = xac["IsKey"].Value.ToLower() == "true" ? xac["Name"].Value + "(*)" : xac["Name"].Value;
                            fieldTable.Cell(j + 2, 2).Range.Text = xac["Caption"].Value;
                            fieldTable.Cell(j + 2, 3).Range.Text = xac["DataType"].Value;
                            fieldTable.Cell(j + 2, 4).Range.Text = xac["Length"].Value;
                            fieldTable.Cell(j + 2, 5).Range.Text = xac["Scale"].Value;
                            fieldTable.Cell(j + 2, 6).Range.Text = xac["isNullable"].Value;
                            fieldTable.Cell(j + 2, 7).Range.Text = "";
                            //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                        }
                        selection.EndKey(ref Wdstory, ref oMissing);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "TableSchema" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"\Schema" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "T";
                    #endregion
                }
                else if (((string)paramters[1]) == "W")
                {
                    #region Print Web Page
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Web Page List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadWebPage(pagexml, selection, oWord, oDoc, printSetting);
                        selection.InsertBreak(ref type);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "HTMLPage" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"\Page" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "W";
                    #endregion
                }
                else
                    return new object[] { 1, "Error" };
            }
            catch (Exception ex)
            {
                return new object[] { 1, ex.Message };
            }
            finally
            {
                object closesave = Word.WdSaveOptions.wdDoNotSaveChanges;
                object option = Word.WdOriginalFormat.wdWordDocument;
                object f = false;
                oWord.Quit(ref closesave, ref option, ref f);
                System.Threading.Thread.Sleep(2000);
                //oWord.Quit(ref closesave, ref option, ref f);
            }
            if (filename != string.Empty)
            {
                byte[] filebyte = File.ReadAllBytes((string)filepath);
                return new object[] { 0, filebyte, filename, filetype, dt.ToString("yyyy-MM-dd HH:mm:ss") };
            }
            else
                return new object[] { 1, "No Print Success" };
        }
        public static void LoadService(string xaml, Selection selection, Word._Application oWord,
    Word._Document oDoc, object[] printSetting)
        {
            XmlDocument xml = new XmlDocument();
            xaml = System.Text.RegularExpressions.Regex.Replace(xaml, "^[^<]+", "");
            TextReader tr = new StringReader(xaml);
            xml.Load(tr);
            if (xml.SelectSingleNode("WrapPanel") != null)
            {
                XmlNode children = xml.SelectSingleNode("WrapPanel").SelectSingleNode("Children");
                if (children != null)
                {
                    #region SDCommand
                    //if ((String)printSetting[0] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDCommandList = children.SelectNodes("SDCommand");
                        if (SDCommandList != null && SDCommandList.Count > 0)
                        {
                            selection.TypeText("【SDCommand】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDCommandList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "CommandText";
                            GridViewTable.Cell(1, 3).Range.Text = "KeyFields";
                            //}
                            for (int j = 0; j < SDCommandList.Count; j++)
                            {
                                XmlNode co = (SDCommandList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDCommandList[j] as XmlNode).SelectSingleNode("CommandText");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDCommandList[j] as XmlNode).SelectSingleNode("KeyFields");
                                if (co != null)
                                {
                                    XmlNodeList keylist = co.SelectNodes("KeyItem");
                                    if (keylist != null && keylist.Count > 0)
                                    {
                                        string keyfieldsstring = "";
                                        foreach (XmlNode key in keylist)
                                        {
                                            if (key.FirstChild != null)
                                            {
                                                if (keyfieldsstring != "")
                                                    keyfieldsstring += ",";
                                                keyfieldsstring += key.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 3).Range.Text = keyfieldsstring;
                                    }
                                }
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDUpdateComponent
                    //if ((String)printSetting[1] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDUpdateComponentList = children.SelectNodes("SDUpdateComponent");
                        if (SDUpdateComponentList != null && SDUpdateComponentList.Count > 0)
                        {
                            selection.TypeText("【SDUpdateComponent】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDUpdateComponentList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "SelectCmd";
                            GridViewTable.Cell(1, 3).Range.Text = "ServerModify";
                            //}
                            for (int j = 0; j < SDUpdateComponentList.Count; j++)
                            {
                                XmlNode co = (SDUpdateComponentList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDUpdateComponentList[j] as XmlNode).SelectSingleNode("SelectCmd");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDUpdateComponentList[j] as XmlNode).SelectSingleNode("ServerModify");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 3).Range.Text = co.InnerText;

                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDDataSource
                    //if ((String)printSetting[1] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDDataSource = children.SelectNodes("SDDataSource");
                        if (SDDataSource != null && SDDataSource.Count > 0)
                        {
                            selection.TypeText("【SDDataSource】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDDataSource.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "Master";
                            GridViewTable.Cell(1, 3).Range.Text = "MasterColumns";
                            GridViewTable.Cell(1, 4).Range.Text = "Detail";
                            GridViewTable.Cell(1, 5).Range.Text = "DetailColumns";
                            //}
                            for (int j = 0; j < SDDataSource.Count; j++)
                            {
                                XmlNode co = (SDDataSource[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("Master");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("MasterColumns");
                                if (co != null)
                                {
                                    XmlNodeList ColumnItemlist = co.SelectNodes("ColumnItem");
                                    if (ColumnItemlist != null && ColumnItemlist.Count > 0)
                                    {
                                        string ColumnItemString = "";
                                        foreach (XmlNode columnItem in ColumnItemlist)
                                        {
                                            if (columnItem.FirstChild != null)
                                            {
                                                if (ColumnItemString != "")
                                                    ColumnItemString += ",";
                                                ColumnItemString += columnItem.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 3).Range.Text = ColumnItemString;
                                    }
                                }
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("Detail");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 4).Range.Text = co.InnerText;
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("DetailColumns");
                                if (co != null)
                                {
                                    XmlNodeList ColumnItemlist = co.SelectNodes("ColumnItem");
                                    if (ColumnItemlist != null && ColumnItemlist.Count > 0)
                                    {
                                        string ColumnItemString = "";
                                        foreach (XmlNode columnItem in ColumnItemlist)
                                        {
                                            if (columnItem.FirstChild != null)
                                            {
                                                if (ColumnItemString != "")
                                                    ColumnItemString += ",";
                                                ColumnItemString += columnItem.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 5).Range.Text = ColumnItemString;
                                    }
                                }
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDAutoNumber
                    //if ((String)printSetting[0] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDAutoNumberList = children.SelectNodes("SDAutoNumber");
                        if (SDAutoNumberList != null && SDAutoNumberList.Count > 0)
                        {
                            selection.TypeText("【SDAutoNumber】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDAutoNumberList.Count + 1, 10, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "Active";
                            GridViewTable.Cell(1, 3).Range.Text = "UpdateComp";
                            GridViewTable.Cell(1, 4).Range.Text = "AutoNoID";
                            GridViewTable.Cell(1, 5).Range.Text = "isNumFill";
                            GridViewTable.Cell(1, 6).Range.Text = "NumDig";
                            GridViewTable.Cell(1, 7).Range.Text = "OverFlow";
                            GridViewTable.Cell(1, 8).Range.Text = "StartValue";
                            GridViewTable.Cell(1, 9).Range.Text = "Step";
                            GridViewTable.Cell(1, 10).Range.Text = "TargetColumn";

                            //}
                            for (int j = 0; j < SDAutoNumberList.Count; j++)
                            {
                                XmlNode co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("Active");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("UpdateComp");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 3).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("AutoNoID");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 4).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("isNumFill");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 5).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("NumDig");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 6).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("OverFlow");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 7).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("StartValue");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 8).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("Step");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 9).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("TargetColumn");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 10).Range.Text = co.InnerText;
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDTransaction
                    //if ((String)printSetting[0] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDTransactionList = children.SelectNodes("SDTransaction");
                        if (SDTransactionList != null && SDTransactionList.Count > 0)
                        {
                            selection.TypeText("【SDTransaction】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDTransactionList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "UpdateComp";
                            GridViewTable.Cell(1, 3).Range.Text = "Transactions";
                            //}
                            for (int j = 0; j < SDTransactionList.Count; j++)
                            {
                                XmlNode co = (SDTransactionList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDTransactionList[j] as XmlNode).SelectSingleNode("UpdateComp");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDTransactionList[j] as XmlNode).SelectSingleNode("Transactions");
                                if (co != null)
                                {
                                    XmlNodeList keylist = co.SelectNodes("KeyItem");
                                    if (keylist != null && keylist.Count > 0)
                                    {
                                        string keyfieldsstring = "";
                                        foreach (XmlNode key in keylist)
                                        {
                                            if (key.FirstChild != null)
                                            {
                                                if (keyfieldsstring != "")
                                                    keyfieldsstring += ",";
                                                keyfieldsstring += key.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 3).Range.Text = keyfieldsstring;
                                    }
                                }
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                }
            }
        }

        public static void LoadPage(string xaml, Selection selection, Word._Application oWord,
            Word._Document oDoc, object[] printSetting)
        {
            XmlDocument xml = new XmlDocument();

            //Page里面有一个ClientCS:的标签，在xml的外面，所以读取报错了，要拿走这个标签及之后的内容，保留之前的XML文本即可
            if (xaml.IndexOf("ClientCS:") != -1)
            {
                xaml = xaml.Substring(0, xaml.IndexOf("ClientCS:"));
            }
            TextReader tr = new StringReader(xaml);
            xml.Load(tr);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("SLTools", "clr-namespace:SLTools;assembly=SLTools");

            #region servicedatasource
            if (printSetting[0].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList ServiceDataSourceList = xml.SelectNodes("descendant::SLTools:ServiceDataSource", nsmgr);
                    if (ServiceDataSourceList != null && ServiceDataSourceList.Count > 0)
                    {
                        selection.TypeText("【ServiceDataSource】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, ServiceDataSourceList.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;

                        GridViewTable.Cell(1, 1).Range.Text = "Name";
                        GridViewTable.Cell(1, 2).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 3).Range.Text = "PacketRecords";
                        GridViewTable.Cell(1, 4).Range.Text = "AlwaysClose";
                        //}
                        for (int j = 0; j < ServiceDataSourceList.Count; j++)
                        {
                            XmlAttributeCollection co = (ServiceDataSourceList[j] as XmlNode).Attributes;

                            GridViewTable.Cell(j + 2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(j + 2, 2).Range.Text = co["RemoteName"].Value;
                            GridViewTable.Cell(j + 2, 3).Range.Text = co["PacketRecords"].Value;
                            GridViewTable.Cell(j + 2, 4).Range.Text = co["AlwaysClose"].Value;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region navigator
            if (printSetting[1].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLNavigatorList = xml.SelectNodes("descendant::SLTools:SLNavigator", nsmgr);
                    if (SLNavigatorList != null && SLNavigatorList.Count > 0)
                    {
                        selection.TypeText("【SLNavigator】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, SLNavigatorList.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;

                        GridViewTable.Cell(1, 1).Range.Text = "Name";
                        GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                        GridViewTable.Cell(1, 3).Range.Text = "DataObjectID";
                        GridViewTable.Cell(1, 4).Range.Text = "NavMode";
                        //}
                        for (int j = 0; j < SLNavigatorList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLNavigatorList[j] as XmlNode).Attributes;

                            GridViewTable.Cell(j + 2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(j + 2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.Cell(j + 2, 3).Range.Text = co["DataObjectID"].Value;
                            GridViewTable.Cell(j + 2, 4).Range.Text = co["NavMode"].Value;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region datagrid
            if (printSetting[2].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDataGridList = xml.SelectNodes("descendant::SLTools:SLDataGrid", nsmgr);
                    if (SLDataGridList != null && SLDataGridList.Count > 0)
                    {
                        selection.TypeText("【SLDataGrid】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDataGridList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDataGridList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                            #region DataGridFieldList
                            XmlNodeList DataGridFieldList = (SLDataGridList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DataFieldDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode Columns in DataGridFieldList)
                            {
                                if (Columns.LocalName == "SLDataGrid.Columns")
                                {
                                    foreach (XmlNode Column in Columns.ChildNodes)
                                    {
                                        if (Column.LocalName == "DataGridTextColumn")
                                        {
                                            XmlAttributeCollection innerControlAttris = Column.Attributes;
                                            string width = innerControlAttris["Width"] != null ? innerControlAttris["Width"].Value : "";
                                            string Header = innerControlAttris["Header"].Value;
                                            string Binding = innerControlAttris["Binding"].Value;
                                            string fieldname = Binding.Substring(Binding.IndexOf('=') + 1, Binding.IndexOf('}') - Binding.IndexOf('=') - 1);
                                            DataFieldDictionary.Add(fieldname, new object[] { fieldname, "TextColumn", Header, width });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DataFieldDictionary.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "Field Type";
                            GridViewTable2.Cell(1, 3).Range.Text = "Header";
                            GridViewTable2.Cell(1, 4).Range.Text = "Width";
                            //}
                            int count = 2;
                            foreach (var d in DataFieldDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                    GridViewTable2.Cell(count, 4).Range.Text = value[3].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region dataform
            if (printSetting[3].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDataFormList = xml.SelectNodes("descendant::SLTools:SLDataForm", nsmgr);
                    if (SLDataFormList != null && SLDataFormList.Count > 0)
                    {
                        selection.TypeText("【SLDataForm】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDataFormList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDataFormList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                            #region DataFormFieldList
                            XmlNodeList DataFormFieldList = (SLDataFormList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DataFieldDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode ReadOnlyTemplateNode in DataFormFieldList)
                            {
                                if (ReadOnlyTemplateNode.LocalName == "SLDataForm.ReadOnlyTemplate")
                                {
                                    foreach (XmlNode DataTemplateNode in ReadOnlyTemplateNode.ChildNodes)
                                    {
                                        if (DataTemplateNode.LocalName == "DataTemplate")
                                        {
                                            foreach (XmlNode GridNode in DataTemplateNode.ChildNodes)
                                            {
                                                if (GridNode.LocalName == "Grid")
                                                {
                                                    foreach (XmlNode FormFieldNode in GridNode.ChildNodes)
                                                    {
                                                        if (FormFieldNode.LocalName == "DataField")
                                                        {
                                                            XmlAttributeCollection fieldAttriCol = FormFieldNode.Attributes;
                                                            string name = fieldAttriCol["x:Name"].Value;
                                                            string label = fieldAttriCol["Label"].Value;
                                                            DataFieldDictionary.Add(name, null);
                                                            if (FormFieldNode.FirstChild != null)
                                                            {
                                                                if (FormFieldNode.FirstChild.LocalName == "TextBox")
                                                                {
                                                                    XmlAttributeCollection innerControlAttris = FormFieldNode.FirstChild.Attributes;
                                                                    string width = innerControlAttris["Width"].Value;
                                                                    string text = innerControlAttris["Text"].Value;
                                                                    string fieldname = text.Substring(text.IndexOf('=') + 1, text.IndexOf('}') - text.IndexOf('=') - 1);
                                                                    DataFieldDictionary[name] = new object[] { fieldname, "Textbox", label, width };
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DataFieldDictionary.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "Field Type";
                            GridViewTable2.Cell(1, 3).Range.Text = "Caption";
                            GridViewTable2.Cell(1, 4).Range.Text = "Width";
                            //}
                            int count = 2;
                            foreach (var d in DataFieldDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                    GridViewTable2.Cell(count, 4).Range.Text = value[3].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region default
            if (printSetting[4].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDefaultList = xml.SelectNodes("descendant::SLTools:SLDefault", nsmgr);
                    if (SLDefaultList != null && SLDefaultList.Count > 0)
                    {
                        selection.TypeText("【SLDefault】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDefaultList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDefaultList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataObjectID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataObjectID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();

                            #region DefaultList
                            XmlNodeList DefaultList = (SLDefaultList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DefaultDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode DefaultValues in DefaultList)
                            {
                                if (DefaultValues.LocalName == "SLDefault.DefaultValues")
                                {
                                    foreach (XmlNode DefaultItem in DefaultValues.ChildNodes)
                                    {
                                        if (DefaultItem.LocalName == "SLDefaultItem")
                                        {
                                            XmlAttributeCollection fieldAttriCol = DefaultItem.Attributes;
                                            string name = fieldAttriCol["FieldName"].Value;
                                            string DefaultValue = fieldAttriCol["DefaultValue"] != null ? fieldAttriCol["DefaultValue"].Value : "";
                                            string DefaultMethod = fieldAttriCol["DefaultMethod"] != null ? fieldAttriCol["DefaultMethod"].Value : "";
                                            DefaultDictionary.Add(name, new object[] { name, DefaultValue, DefaultMethod });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DefaultDictionary.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "DefaultValue";
                            GridViewTable2.Cell(1, 3).Range.Text = "DefaultMethod";
                            //}
                            int count = 2;
                            foreach (var d in DefaultDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region validator
            if (printSetting[5].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLValidatorList = xml.SelectNodes("descendant::SLTools:SLValidator", nsmgr);
                    if (SLValidatorList != null && SLValidatorList.Count > 0)
                    {
                        selection.TypeText("【SLValidator】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLValidatorList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLValidatorList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataObjectID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataObjectID"].Value;
                            GridViewTable.Cell(1, 3).Range.Text = "DuplicateCheck";
                            GridViewTable.Cell(1, 4).Range.Text = "DuplicateCheckMode";
                            GridViewTable.Cell(2, 3).Range.Text = co["DuplicateCheck"].Value;
                            GridViewTable.Cell(2, 4).Range.Text = co["DuplicateCheckMode"].Value;

                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();

                            #region ValidatortList
                            XmlNodeList ValidatortList = (SLValidatorList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> ValidatortDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode Validatorts in ValidatortList)
                            {
                                if (Validatorts.LocalName == "SLValidator.Validates")
                                {
                                    foreach (XmlNode Validatort in Validatorts.ChildNodes)
                                    {
                                        if (Validatort.LocalName == "SLValidateItem")
                                        {
                                            XmlAttributeCollection fieldAttriCol = Validatort.Attributes;
                                            string name = fieldAttriCol["FieldName"].Value;
                                            string CheckNull = fieldAttriCol["CheckNull"] != null ? fieldAttriCol["CheckNull"].Value : "";
                                            string CHeckNullMessage = fieldAttriCol["CHeckNullMessage"] != null ? fieldAttriCol["CHeckNullMessage"].Value : "";
                                            string CaptionControlName = fieldAttriCol["CaptionControlName"] != null ? fieldAttriCol["CaptionControlName"].Value : "";
                                            string ValidatortMethod = fieldAttriCol["ValidatortMethod"] != null ? fieldAttriCol["ValidatortMethod"].Value : "";
                                            string InvalidMessage = fieldAttriCol["InvalidMessage"] != null ? fieldAttriCol["InvalidMessage"].Value : "";

                                            ValidatortDictionary.Add(name, new object[] { name, CaptionControlName, CheckNull, CHeckNullMessage, InvalidMessage, ValidatortMethod });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, ValidatortDictionary.Count + 1, 6, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "CaptionControlName";
                            GridViewTable2.Cell(1, 3).Range.Text = "CheckNull";
                            GridViewTable2.Cell(1, 4).Range.Text = "CHeckNullMessage";
                            GridViewTable2.Cell(1, 5).Range.Text = "InvalidMessage";
                            GridViewTable2.Cell(1, 6).Range.Text = "ValidatortMethod";
                            //}
                            int count = 2;
                            foreach (var d in ValidatortDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                    GridViewTable2.Cell(count, 4).Range.Text = value[3].ToString();
                                    GridViewTable2.Cell(count, 5).Range.Text = value[4].ToString();
                                    GridViewTable2.Cell(count, 6).Range.Text = value[5].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion
        }

        public static void LoadWebPage(string xaml, Selection selection, Word._Application oWord,
            Word._Document oDoc, object[] printSetting)
        {
            XmlDocument xml = new XmlDocument();

            //Page里面有一个ClientCS:的标签，在xml的外面，所以读取报错了，要拿走这个标签及之后的内容，保留之前的XML文本即可
            if (xaml.IndexOf("ClientCS:") != -1)
            {
                xaml = xaml.Substring(0, xaml.IndexOf("ClientCS:"));
            }
            if (xaml.StartsWith("<div>"))
            {
                string s = " xmlns:cc1=\"clr-namespace:cc1\" xmlns:dx=\"clr-namespace:dx\"";
                xaml = xaml.Insert(4, s);
            }
            TextReader tr = new StringReader(xaml);
            xml.Load(tr);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("cc1", "clr-namespace:cc1");
            nsmgr.AddNamespace("dx", "clr-namespace:dx");
            nsmgr.AddNamespace("JQTools", "clr-namespace:JQTools");

            #region servicedatasource
            if (printSetting[0].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList ServiceDataSourceList = xml.SelectNodes("descendant::cc1:WebDataSource", nsmgr);
                    if (ServiceDataSourceList != null && ServiceDataSourceList.Count > 0)
                    {
                        selection.TypeText("【WebDataSource】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, ServiceDataSourceList.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;

                        GridViewTable.Cell(1, 1).Range.Text = "ID";
                        GridViewTable.Cell(1, 2).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 3).Range.Text = "PreviewSolution";
                        GridViewTable.Cell(1, 4).Range.Text = "PreviewDatabase";
                        //}
                        for (int j = 0; j < ServiceDataSourceList.Count; j++)
                        {
                            XmlAttributeCollection co = (ServiceDataSourceList[j] as XmlNode).Attributes;

                            GridViewTable.Cell(j + 2, 1).Range.Text = co["ID"].Value;
                            GridViewTable.Cell(j + 2, 2).Range.Text = co["RemoteName"].Value;
                            GridViewTable.Cell(j + 2, 3).Range.Text = co["PreviewSolution"].Value;
                            GridViewTable.Cell(j + 2, 4).Range.Text = co["PreviewDatabase"].Value;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region datagrid
            if (printSetting[2].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDataGridList = xml.SelectNodes("descendant::dx:ASPxGridView", nsmgr);
                    if (SLDataGridList != null && SLDataGridList.Count > 0)
                    {
                        selection.TypeText("【ASPxGridView】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDataGridList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDataGridList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "ID";
                            GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                            GridViewTable.Cell(2, 1).Range.Text = co["ID"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                            #region DataGridFieldList
                            XmlNodeList DataGridFieldList = (SLDataGridList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DataFieldDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode Columns in DataGridFieldList)
                            {
                                if (Columns.LocalName == "Columns")
                                {
                                    foreach (XmlNode Column in Columns.ChildNodes)
                                    {
                                        if (Column.LocalName == "GridViewDataTextColumn")
                                        {
                                            XmlAttributeCollection innerControlAttris = Column.Attributes;
                                            string width = innerControlAttris["Width"] != null ? innerControlAttris["Width"].Value : "";
                                            string fieldname = innerControlAttris["FieldName"].Value;
                                            string Caption = innerControlAttris["Caption"].Value;
                                            DataFieldDictionary.Add(fieldname, new object[] { fieldname, Caption, width });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DataFieldDictionary.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "Caption";
                            GridViewTable2.Cell(1, 3).Range.Text = "Width";
                            //}
                            int count = 2;
                            foreach (var d in DataFieldDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion


        }
        public static void openWord(string filepath)
        {
            Word.Application app = new Word.Application();
            Word.Document doc = null;
            object _filepath = (object)filepath;
            object unknow = Type.Missing;
            object visable = true;
            app.Visible = true;
            doc = app.Documents.Open(ref _filepath,
             ref unknow, ref unknow, ref unknow, ref unknow, ref unknow,
             ref unknow, ref unknow, ref unknow, ref unknow, ref unknow,
             ref visable, ref unknow, ref unknow, ref unknow, ref unknow);
        }

        const string SPREADSHEETSTRING = "urn:schemas-microsoft-com:office:spreadsheet";
        const string OFFICESTRING = "urn:schemas-microsoft-com:office:office";
        const string EXCELSTRING = "urn:schemas-microsoft-com:office:excel";

        #region Export
        public static void ExportToExcel(System.Data.DataTable table, string filePath, string title, List<string> columns)
        {
            XmlDocument xml = CreateFile(filePath);
            XmlNode nodeworkbook = xml.SelectSingleNode("Workbook");
            if (columns.Count == 0)
            {
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                }
            }

            int maxrowcount = title.Length > 0 ? 65534 : 65535;
            XmlNode nodetable = CreateWorkSheet(string.Format("{0}{1}", table.TableName, 0), nodeworkbook, table, title, columns);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (i != 0 && i % maxrowcount == 0)
                {
                    nodetable = CreateWorkSheet(string.Format("{0}{1}", table.TableName, i / maxrowcount), nodeworkbook, table, title, columns);
                }
                XmlNode noderow = xml.CreateElement("Row");
                nodetable.AppendChild(noderow);
                foreach (var column in columns)
                {
                    System.Data.DataColumn dc = table.Columns[column];
                    ToExcel(noderow, table.Rows[i][dc], dc.DataType);
                }
            }
            xml.Save(filePath);
        }

        private static void ToExcel(XmlNode nodeRow, object value, Type type)
        {
            XmlDocument xml = nodeRow.OwnerDocument;
            XmlNode nodecell = xml.CreateElement("Cell");
            nodeRow.AppendChild(nodecell);
            XmlNode nodedata = xml.CreateElement("Data");
            XmlAttribute atttype = xml.CreateAttribute("ss", "Type", SPREADSHEETSTRING);
            if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                         || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                         || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                         || type == typeof(Double) || type == typeof(Decimal))
            {
                atttype.Value = "Number";
                if (value == DBNull.Value)
                {
                    nodedata.InnerText = "0";      //empty value can not set to number column
                }
                else
                {
                    nodedata.InnerText = value.ToString();
                }
            }
            else if (type == typeof(DateTime))
            {
                atttype.Value = "String";
                nodedata.InnerText = value == DBNull.Value ? "" : ((DateTime)value).ToString("yyyy/MM/dd");
            }
            else
            {
                atttype.Value = "String";
                nodedata.InnerText = value.ToString().Replace(">", "&gt").Replace("<", "&lt");
            }
            nodedata.Attributes.Append(atttype);
            nodecell.AppendChild(nodedata);
        }

        private static XmlNode CreateWorkSheet(string sheetName, XmlNode nodeWorkbook, System.Data.DataTable table, string title, List<string> columns)
        {
            XmlDocument xml = nodeWorkbook.OwnerDocument;
            XmlNode nodeworksheet = xml.CreateElement("Worksheet");

            XmlAttribute attname = xml.CreateAttribute("ss", "Name", SPREADSHEETSTRING);
            attname.Value = sheetName;
            nodeworksheet.Attributes.Append(attname);
            XmlAttribute attxmln = xml.CreateAttribute("xmlns");
            attxmln.Value = SPREADSHEETSTRING;
            nodeworksheet.Attributes.Append(attxmln);
            nodeWorkbook.AppendChild(nodeworksheet);

            XmlNode nodetable = xml.CreateElement("Table");
            nodeworksheet.AppendChild(nodetable);

            if (title.Length > 0)
            {
                XmlNode noderowtitle = xml.CreateElement("Row");
                nodetable.AppendChild(noderowtitle);

                XmlNode nodecell = xml.CreateElement("Cell");
                noderowtitle.AppendChild(nodecell);
                if (columns.Count > 1)
                {
                    XmlAttribute attmerge = xml.CreateAttribute("ss", "MergeAcross", SPREADSHEETSTRING);
                    attmerge.Value = (columns.Count - 1).ToString();
                    nodecell.Attributes.Append(attmerge);
                }
                XmlAttribute attstyle = xml.CreateAttribute("ss", "StyleID", SPREADSHEETSTRING);
                attstyle.Value = "title";
                nodecell.Attributes.Append(attstyle);

                XmlNode nodedata = xml.CreateElement("Data");
                XmlAttribute atttype = xml.CreateAttribute("ss", "Type", SPREADSHEETSTRING);
                atttype.Value = "String";
                nodedata.InnerText = title;
                nodedata.Attributes.Append(atttype);
                nodecell.AppendChild(nodedata);
            }

            XmlNode noderow = xml.CreateElement("Row");
            nodetable.AppendChild(noderow);
            foreach (var column in columns)
            {
                System.Data.DataColumn dc = table.Columns[column];
                ToExcel(noderow, dc.Caption, typeof(string));
            }

            return nodetable;
        }

        private static XmlDocument CreateFile(string filePath)
        {
            string directoryname = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", null, null));

            XmlNode nodeworkbook = xml.CreateElement("Workbook");
            XmlAttribute attxmlns = xml.CreateAttribute("xmlns");
            attxmlns.Value = SPREADSHEETSTRING;
            XmlAttribute attxmlnso = xml.CreateAttribute("xmlns:o");
            attxmlnso.Value = OFFICESTRING;
            XmlAttribute attxmlnsx = xml.CreateAttribute("xmlns:x");
            attxmlnsx.Value = EXCELSTRING;
            XmlAttribute attxmlnsss = xml.CreateAttribute("xmlns:ss");
            attxmlnsss.Value = SPREADSHEETSTRING;
            nodeworkbook.Attributes.Append(attxmlns);
            nodeworkbook.Attributes.Append(attxmlnso);
            nodeworkbook.Attributes.Append(attxmlnsx);
            nodeworkbook.Attributes.Append(attxmlnsss);
            xml.AppendChild(nodeworkbook);

            XmlNode nodestyles = xml.CreateElement("Styles");
            nodeworkbook.AppendChild(nodestyles);

            XmlNode nodestyle = xml.CreateElement("Style");
            XmlAttribute attid = xml.CreateAttribute("ss", "ID", SPREADSHEETSTRING);
            attid.Value = "title";
            nodestyle.Attributes.Append(attid);
            nodestyles.AppendChild(nodestyle);

            XmlElement nodealignment = xml.CreateElement("Alignment");
            XmlAttribute atthorizontal = xml.CreateAttribute("ss", "Horizontal", SPREADSHEETSTRING);
            atthorizontal.Value = "Center";
            nodealignment.Attributes.Append(atthorizontal);
            nodestyle.AppendChild(nodealignment);

            return xml;
        }
        #endregion

        #region Import
        public static void ImportFromExcel(System.Data.DataTable table, Stream file, int beginrow, int begincell)
        {
            List<List<string>> list = XmlRead(file);

            CheckAllowDBNull(table, GetMinColumnCount(list, beginrow, begincell));

            System.Data.DataTable tableread = table.Clone();
            tableread.Rows.Clear();

            for (int i = beginrow; i < list.Count; i++)
            {
                System.Data.DataRow dr = tableread.NewRow();
                for (int j = begincell; j < list[i].Count && j < tableread.Columns.Count; j++)
                {
                    object value = DBNull.Value;
                    try
                    {
                        if (tableread.Columns[j].DataType != typeof(Guid))
                        {
                            if (tableread.Columns[j].DataType == typeof(decimal))
                            {
                                value = Convert.ToDecimal(double.Parse(list[i][j]));
                            }
                            else
                            {
                                value = Convert.ChangeType(list[i][j], tableread.Columns[j].DataType);
                            }
                        }
                        else
                        {
                            Guid id = new Guid(list[i][j]);
                        }
                    }
                    catch { }
                    if (value == DBNull.Value && !tableread.Columns[j].AllowDBNull)
                    {
                        throw new System.Data.NoNullAllowedException(string.Format("Column:{0} in table does not allow null value,but data in excel file can not cast"
                            , tableread.Columns[j].ColumnName));
                    }
                    if (value == DBNull.Value && tableread.Columns[j].DataType == typeof(bool))
                    {
                        value = false;
                    }
                    dr[j] = value;
                }
                try
                {
                    tableread.Rows.Add(dr);
                }
                catch
                {

                }
            }

            table.Merge(tableread);
        }

        private static int GetMinColumnCount(List<List<string>> list, int beginrow, int begincell)
        {
            int count = list.Count;
            int min = int.MaxValue;
            for (int i = beginrow; i < count; i++)
            {
                min = Math.Min(list[i].Count, min);
            }
            return Math.Max(min - begincell, 0);
        }

        private static List<List<string>> XmlRead(Stream file)
        {
            List<List<string>> list = new List<List<string>>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(file);
            }
            catch
            {
                throw new XmlException("File is not stored as xml file, you can select Officetools.ExcelRead as alternative");
            }
            XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xml.NameTable);
            xmlmgr.AddNamespace("sheet", SPREADSHEETSTRING);
            XmlNode table = xml.SelectSingleNode("/sheet:Workbook/sheet:Worksheet/sheet:Table", xmlmgr);
            XmlNodeList rows = table.SelectNodes("sheet:Row", xmlmgr);
            //解决空行的问题
            int rowindex = 0;
            foreach (XmlNode row in rows)
            {
                if (row.Attributes["Index", SPREADSHEETSTRING] != null)
                {
                    while (rowindex < Convert.ToInt32(row.Attributes["Index", SPREADSHEETSTRING].Value) - 1)
                    {
                        list.Add(new List<string>());
                        rowindex++;
                    }
                }
                List<string> listrow = new List<string>();
                int celindex = 0;
                foreach (XmlNode cell in row.ChildNodes)
                {
                    if (cell.Attributes["Index", SPREADSHEETSTRING] != null)
                    {
                        while (celindex < Convert.ToInt32(cell.Attributes["Index", SPREADSHEETSTRING].Value) - 1)
                        {
                            listrow.Add(string.Empty);
                            celindex++;
                        }
                    }
                    listrow.Add(cell.InnerText.Trim());
                    celindex++;
                }
                if (listrow.Exists(c => !string.IsNullOrEmpty(c)))
                {
                    list.Add(listrow);
                }
                rowindex++;
            }
            return list;
        }

        private static void CheckAllowDBNull(System.Data.DataTable table, int index)
        {
            for (int i = index; i < table.Columns.Count; i++)
            {
                if (!table.Columns[i].AllowDBNull)
                {
                    throw new System.Data.NoNullAllowedException(string.Format("Column:{0} in table does not allow null value,but no data in excel file"
                        , table.Columns[i].ColumnName));
                }
            }
        }
        #endregion

    }
}
