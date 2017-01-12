using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using Infolight.EasilyReportTools.Tools;
using Infolight.EasilyReportTools.Design;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Reflection;
using Srvtools;

namespace Infolight.EasilyReportTools
{
    /// <summary>
    /// Class of report item collection
    /// </summary>
    public class ReportItemCollection : IList, ICollection, IEnumerable
    {
        /// <summary>
        /// Create a new instance of report item collection
        /// </summary>
        /// <param name="rpt">owner report</param>
        public ReportItemCollection(IReport rpt)
        {
            report = rpt;
        }

        public ReportItemCollection() { }
        
        private IReport report;
        /// <summary>
        /// Get the owner report
        /// </summary>
        public IReport Report
        {
            get { return report; }
        }

        private DataTable dictionaryTable;

        internal DataTable DictionaryTable
        {
            get 
            {
                if(dictionaryTable == null)
                {
                    if (this.Report != null)
                    {
                        object dataSource = this.Report.HeaderDataSource;
                        if (dataSource != null)
                        {
                            if (dataSource is InfoBindingSource)
                            { 
                                InfoBindingSource ibs = dataSource as InfoBindingSource;
                                dictionaryTable = DBUtils.GetDataDictionary(ibs, ibs.Site != null && ibs.Site.DesignMode).Tables[0];
                            }
                            else if (dataSource is WebDataSource)
                            {
                                WebDataSource wds = dataSource as WebDataSource;
                                dictionaryTable = DBUtils.GetDataDictionary(wds, wds.Site != null && wds.Site.DesignMode).Tables[0];
                            }
                        }
                    }
                }
                return dictionaryTable;
            }
        }

        //internal void SetReport(IReport rpt)
        //{
        //    this.report = rpt;
        //}

        private ArrayList list = new ArrayList();
        /// <summary>
        /// Add a report item to collection
        /// </summary>
        /// <param name="item">report item</param>
        /// <returns>index of item</returns>
        public int Add(ReportItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            return list.Add(item);
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public void Clear()
        {
            foreach (ReportItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        /// <summary>
        /// Determines whether collection contains the item
        /// </summary>
        /// <param name="item">report item</param>
        /// <returns>true if collection contains item, otherwise false</returns>
        public bool Contains(ReportItem item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Get index of item in collection
        /// </summary>
        /// <param name="item">report item</param>
        /// <returns>index of item</returns>
        public int IndexOf(ReportItem item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Insert a item into collection
        /// </summary>
        /// <param name="index">index of item</param>
        /// <param name="item">report item</param>
        public void Insert(int index, ReportItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            list.Insert(index, item);
        }

        /// <summary>
        /// Get a value indicating whether collection has a fixed size
        /// </summary>
        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        /// <summary>
        /// Get a value indicating whether collection is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        /// <summary>
        /// Remove item from collection
        /// </summary>
        /// <param name="item">report item</param>
        public void Remove(ReportItem item)
        {
            if (item != null)
            {
                item.SetCollection(null);
            }
            list.Remove(item);
        }

        /// <summary>
        /// Remove item from collection 
        /// </summary>
        /// <param name="index">index of item</param>
        public void RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        /// <summary>
        /// Get or set the report item
        /// </summary>
        /// <param name="index">index of item</param>
        /// <returns>report item</returns>
        public ReportItem this[int index]
        {
            get
            {
                return (ReportItem)list[index];
            }
            set
            {
                this[index].SetCollection(null);
                list[index] = value;
                this[index].SetCollection(this);
            }
        }

        /// <summary>
        /// Create a new copy of collection
        /// </summary>
        /// <param name="iReport">the owner report</param>
        /// <returns>a new copy of collection</returns>
        public ReportItemCollection Copy(IReport ireport)
        {
            ReportItemCollection newCollection = new ReportItemCollection(ireport);
            foreach (ReportItem reportItem in this)
            {
                newCollection.Add(reportItem.Copy());
            }
            return newCollection;
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is ReportItem)
            {
                ((ReportItem)value).SetCollection(this);
                return list.Add(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.Clear()
        {
            foreach (ReportItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is ReportItem)
            {
                ((ReportItem)value).SetCollection(this);
                list.Insert(index, value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            if (value is ReportItem)
            {
                ((ReportItem)value).SetCollection(null);
                list.Remove(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (value is ReportItem)
                {
                    ((ReportItem)list[index]).SetCollection(null);
                    list[index] = value;
                    ((ReportItem)list[index]).SetCollection(this);
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        /// <summary>
        /// Get count of collection
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// class of reprot item
    /// </summary>
    public abstract class ReportItem
    {
        private string format;
        /// <summary>
        /// Get or set format of item
        /// </summary>
        [Category("Infolight"),
        Description("The format of export item.")]
        public virtual string Format
        {
            get { return format; }
            set { format = value; }
        }

        private Font font;
        /// <summary>
        /// Get or set font of item
        /// </summary>
        [Category("Infolight"),
        Description("The font used to display text in this item.")]
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        private HorizontalAlignment alignment;
        /// <summary>
        /// Get or set alignment of item
        /// </summary>
        [Category("Infolight"),
        Description("The position of this item's content in the excel cell.")]
        public HorizontalAlignment ContentAlignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        private PositionAlign position;
        /// <summary>
        /// Get or set alignment in the report
        /// </summary>
        [Category("Infolight"),
        Description("The position of this item in Excel file.")]
        public PositionAlign Position
        {
            get { return position; }
            set { position = value; }
        }

        private bool newLine;
        /// <summary>
        /// Get or set a value indicating item display in new line
        /// </summary>
        [Category("Infolight"),
        Description("Indicates whether this item can display in the new line.")]
        public bool NewLine
        {
            get { return newLine; }
            set { newLine = value; }
        }

        private int cells = 1;
        /// <summary>
        /// Get or set the amount of cells occupied by item
        /// </summary>
        [Category("Infolight"),
        Description("Indicates how many cells is occupied by this item.")]
        public int Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        private ReportItemCollection collection;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ReportItemCollection Collection
        {
            get { return collection; }
        }

        internal void SetCollection(ReportItemCollection col)
        {
            collection = col;
        }

        [Browsable(false)]
        public abstract object Value
        {
            get;
        }

        public abstract ReportItem New();

        public abstract ReportItem Copy();

        /// <summary>
        /// Get string format of item
        /// </summary>
        /// <returns>string format of item</returns>
        public override string ToString()
        {
            return this.GetType().Name.Substring(6);
        }

        public enum PositionAlign
        { 
            Left,
            Right
        }
    }

    /// <summary>
    /// class of report index item
    /// </summary>
    public abstract class ReportIndexItem : ReportItem
    {
        private int index;
        /// <summary>
        /// Get or set index of item
        /// </summary>
        public virtual int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// Get string format of item
        /// </summary>
        /// <returns>string format of item</returns>
        public override string ToString()
        {
            return string.Format("{0}({1})",this.GetType().Name.Substring(6), Index);
        }
    }

    /// <summary>
    /// class of report constant item
    /// </summary>
    public class ReportConstantItem: ReportItem
    {
        /// <summary>
        /// Enum of style
        /// </summary>
        public enum StyleType
        {
            /// <summary>
            /// blank
            /// </summary>
            Blank,
            /// <summary>
            /// description
            /// </summary>
            Description,
            /// <summary>
            /// company name
            /// </summary>
            CompanyName,
            /// <summary>
            /// page index
            /// </summary>
            PageIndex,
            /// <summary>
            /// page index and total
            /// </summary>
            PageIndexAndTotalPageCount,
            /// <summary>
            /// query condition
            /// </summary>
            QueryCondition,
            /// <summary>
            /// report date
            /// </summary>
            ReportDate,
            /// <summary>
            /// report date and time
            /// </summary>
            ReportDateTime,
            /// <summary>
            /// report id
            /// </summary>
            ReportID,
            /// <summary>
            /// report name
            /// </summary>
            ReportName,
            /// <summary>
            /// user id
            /// </summary>
            UserID,
            /// <summary>
            /// user name
            /// </summary>
            UserName
        }

        private StyleType style = StyleType.Blank;
        /// <summary>
        /// Get or set the style of item
        /// </summary>
        [Category("Infolight"),
        Description("The content of this item.")]
        public StyleType Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }

        //private Hashtable styleHt;

        //public ReportConstantItem()
        //{
        //    styleHt = new Hashtable();
        //    styleHt.Add(StyleType.Blank, ERptMultiLanguage.GetLanValue(StyleType.Blank.ToString()));
        //    styleHt.Add(StyleType.CompanyName, ERptMultiLanguage.GetLanValue(StyleType.CompanyName.ToString()));
        //    styleHt.Add(StyleType.Description, ERptMultiLanguage.GetLanValue(StyleType.Description.ToString()));
        //    styleHt.Add(StyleType.PageIndex, ERptMultiLanguage.GetLanValue(StyleType.PageIndex.ToString()));
        //    styleHt.Add(StyleType.PageIndexAndTotalPageIndex, ERptMultiLanguage.GetLanValue(StyleType.PageIndexAndTotalPageIndex.ToString()));
        //    styleHt.Add(StyleType.QueryCondition, ERptMultiLanguage.GetLanValue(StyleType.QueryCondition.ToString()));
        //    styleHt.Add(StyleType.ReportDate, ERptMultiLanguage.GetLanValue(StyleType.ReportDate.ToString()));
        //    styleHt.Add(StyleType.ReportDateTime, ERptMultiLanguage.GetLanValue(StyleType.ReportDateTime.ToString()));
        //    styleHt.Add(StyleType.ReportID, ERptMultiLanguage.GetLanValue(StyleType.ReportID.ToString()));
        //    styleHt.Add(StyleType.ReportName, ERptMultiLanguage.GetLanValue(StyleType.ReportName.ToString()));
        //    styleHt.Add(StyleType.UserID, ERptMultiLanguage.GetLanValue(StyleType.UserID.ToString()));
        //    styleHt.Add(StyleType.UserName, ERptMultiLanguage.GetLanValue(StyleType.UserName.ToString()));

        //    this.content = ERptMultiLanguage.GetLanValue(StyleType.Blank.ToString());
        //}

        //internal Hashtable GetStyleHt()
        //{
        //    return styleHt;
        //}

        //private string content;
        //[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        //[Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //[Category("Infolight"),
        //Description("The content of this item.")]
        //public string Content
        //{
        //    get
        //    {
        //        //return content;
        //        return ERptMultiLanguage.GetLanValue(this.style.ToString());
        //    }
        //    set
        //    {
        //        content = value;

        //        bool changeFormat = false;
        //        ExecutionResult execRes = GetStyleType(content);
        //        if (execRes.Status)
        //        {
        //            if (this.Style != (StyleType)execRes.Anything)
        //            {
        //                changeFormat = true;
        //            }
        //            this.Style = (StyleType)execRes.Anything;
        //        }

        //        if (string.IsNullOrEmpty(this.format) || changeFormat)
        //        {
        //            this.format = content + ":" + "{0}";
        //        }
        //    }
        //}

        private string format = string.Empty;
        /// <summary>
        /// Get or set the format of item
        /// </summary>
        [Category("Infolight"),
        Description("The format of export item.")]
        public override string Format
        {
            get 
            {
                if (string.IsNullOrEmpty(format) && Style != StyleType.Blank)
                {
                    if (Style == StyleType.PageIndexAndTotalPageCount)
                    {
                        if (CliUtils.fClientLang == SYS_LANGUAGE.SIM || CliUtils.fClientLang == SYS_LANGUAGE.TRA)
                        {
                            return ERptMultiLanguage.GetLanValue(this.style.ToString()).Substring(0, 2) + ":" + "{0}";
                        }
                        else
                        {
                            return ERptMultiLanguage.GetLanValue(this.style.ToString()).Substring(0, 9) + ":" + "{0}";
                        }
                    }
                    else
                    {
                        return ERptMultiLanguage.GetLanValue(this.style.ToString()) + ":" + "{0}";
                    }
                }
                else
                {
                    return this.format;
                }
            }
            set { format = value; }
        }

        /// <summary>
        /// Get the value of item
        /// </summary>
        public override object Value
        {
            get 
            {
                switch (Style)
                {
                    case StyleType.Blank: return string.Empty;
                    case StyleType.CompanyName: return CliUtils.fSiteCode;
                    case StyleType.Description: return Collection.Report.Description;
                    case StyleType.PageIndex: return Collection.Report.CurrentPageIndex;
                    case StyleType.PageIndexAndTotalPageCount:
                        return string.Format("{0}/{1}", Collection.Report.CurrentPageIndex, Collection.Report.PageCount);
                    case StyleType.QueryCondition: return string.Empty;
                    case StyleType.ReportDate: return DateTime.Today.ToString("yyyy/MM/dd");
                    case StyleType.ReportDateTime:
                        return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    case StyleType.ReportID: return Collection.Report.ReportID;
                    case StyleType.ReportName: return Collection.Report.ReportName;
                    case StyleType.UserID: return CliUtils.fLoginUser;
                    case StyleType.UserName:  return CliUtils.fUserName;
                }
                return null;
            }
        }

        /// <summary>
        /// Create a new instance of report constant item
        /// </summary>
        /// <returns>a new instance of report constant item</returns>
        public override ReportItem New()
        {
            return new ReportConstantItem();
        }

        /// <summary>
        /// Create a new copy of report constant item
        /// </summary>
        /// <returns>a new copy of report constant item</returns>
        public override ReportItem Copy()
        {
            ReportConstantItem newItem = new ReportConstantItem();
            newItem.Cells = this.Cells;
            newItem.ContentAlignment = this.ContentAlignment;
            newItem.Font = this.Font;
            newItem.Format = this.Format;
            newItem.NewLine = this.NewLine;
            newItem.Position = this.Position;
            newItem.Style = this.Style;
            return newItem;
        }

        //private ExecutionResult GetStyleType(string strStyle)
        //{
        //    ExecutionResult execRes = new ExecutionResult();
        //    StyleType styleType = StyleType.Blank;

        //    foreach (DictionaryEntry de in styleHt)
        //    {
        //        if (de.Value.ToString() == strStyle)
        //        {
        //            styleType = (StyleType)de.Key;
        //            execRes.Status = true;
        //        }
        //    }

        //    execRes.Anything = styleType;

        //    return execRes;
        //}

        /// <summary>
        /// Get string format of item
        /// </summary>
        /// <returns>string format of item</returns>
        public override string ToString()
        {
            string strValue = ERptMultiLanguage.GetLanValue(Style.ToString());

            //if (this.Content == null || this.Content == String.Empty)
            //{
            //    strValue = ERptMultiLanguage.GetLanValue(StyleType.Blank.ToString());
            //}
            //else
            //{
            //    strValue = this.Content;
            //}
            return strValue; //直接返回内容
            //return string.Format("{0}({1})", this.GetType().Name.Substring(6), strValue);
        }
    }

    /// <summary>
    /// class of report parameter item
    /// </summary>
    public class ReportParameterItem : ReportIndexItem
    {
        /// <summary>
        /// Get the value of item
        /// </summary>
        public override object Value
        {
            get
            {
                object value = null;
                if (Collection != null && Collection.Report != null)
                {
                    IReport report = this.Collection.Report;
                    if (report != null)
                    {
                        value = Collection.Report.Parameters[Index].Value;
                        object owner = null;
                        if (report is EasilyReport)
                        {
                            owner = (report as EasilyReport).ContainerControl;
                        }
                        else if (report is WebEasilyReport)
                        {
                            owner = (report as WebEasilyReport).Page;
                        }
                        return GetValue(owner, (string)value);
                    }
                }
                return value;

            }
        }

        /// <summary>
        /// Create a new instance of report parameter item
        /// </summary>
        /// <returns>a new instance of report parameter item</returns>
        public override ReportItem New()
        {
            return new ReportParameterItem();
        }

        /// <summary>
        /// Create a new copy of report parameter item
        /// </summary>
        /// <returns>a new copy of report parameter item</returns>
        public override ReportItem Copy()
        {
            ReportParameterItem newItem = new ReportParameterItem();
            newItem.ContentAlignment = this.ContentAlignment;
            newItem.Cells = this.Cells;
            newItem.Font = this.Font;
            newItem.Format = this.Format;
            newItem.Index = this.Index;
            newItem.NewLine = this.NewLine;
            newItem.Position = this.Position;
            return newItem;
        }

        private object GetValue(object owner, string expression)
        {
            if (owner != null && !string.IsNullOrEmpty(expression))
            {
                Match match = Regex.Match(expression, @"\w+(?=\s*\(\s*\))");
                if (match.Success)
                {
                    Type type = owner.GetType();
                    MethodInfo method = type.GetMethod(match.Value, new Type[0]);
                    if (method != null)
                    {
                        return method.Invoke(owner, null);
                    }
                    else
                    {
                        return expression;
                    }
                }
            }
            return expression;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", ERptMultiLanguage.GetLanValue("ParameterItem"), Index);
        }
    }

    /// <summary>
    /// class of report image item
    /// </summary>
    public class ReportImageItem : ReportIndexItem
    {
        /// <summary>
        /// Get the value of item
        /// </summary>
        public override object Value
        {
            get 
            {
                if (Collection != null && Collection.Report != null)
                {
                    return Collection.Report.Images[Index].Image;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Create a new instance of report image item
        /// </summary>
        /// <returns>a new instance of report image item</returns>
        public override ReportItem New()
        {
            return new ReportImageItem();
        }

        /// <summary>
        /// Create a new copy of report image item
        /// </summary>
        /// <returns>a new copy of report image item</returns>
        public override ReportItem Copy()
        {
            ReportImageItem newItem;
            newItem = new ReportImageItem();
            newItem.ContentAlignment = this.ContentAlignment;
            newItem.Cells = this.Cells;
            newItem.Font = this.Font;
            newItem.Format = this.Format;
            newItem.Index = this.Index;
            newItem.NewLine = this.NewLine;
            newItem.Position = this.Position;
            return newItem;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", ERptMultiLanguage.GetLanValue("ImageItem"), Index);
        }
    }

    /// <summary>
    /// class of report datasource item
    /// </summary>
    public class ReportDataSourceItem : ReportItem, IReportGetValues
    {
        public ReportDataSourceItem()
        {
            
        }
        
        private string columnName;
        /// <summary>
        /// Get the field of item
        /// </summary>
        [Category("Infolight"),
        Description("The fields displayed in the item.")]
        [Editor(typeof(Infolight.EasilyReportTools.Design.PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public override string Format
        {
            get
            {
                if (!string.IsNullOrEmpty(ColumnName) && string.IsNullOrEmpty(base.Format) && this.Collection != null)
                {
                    DataTable ddTable = this.Collection.DictionaryTable;
                    if (ddTable != null)
                    {
                        DataRow[] rows = ddTable.Select(string.Format("{0}='{1}'", DDInfo.FieldName, ColumnName.Replace("'", "''")));
                        if(rows.Length > 0)
                        {
                            string mask = rows[0][DDInfo.EditMask].ToString();
                            if (!string.IsNullOrEmpty(mask))
                            {
                                return string.Format("{{0:{0}}}", mask);
                            }
                        }
                    }

                }

                return base.Format;
            }
            set
            {
                base.Format = value;
            }
        }

        /// <summary>
        /// Get the value of item
        /// </summary>
        public override object Value
        {
            get
            {
                object headerDataSource = Collection.Report.HeaderDataSource;
                if (headerDataSource != null)
                {
                    DataRow row = null;
                    if (headerDataSource is BindingSource && (headerDataSource as BindingSource).Current != null)
                    {
                        row = ((headerDataSource as BindingSource).Current as DataRowView).Row;
                    }
                    else if (headerDataSource is WebDataSource)
                    {
                        row = (headerDataSource as WebDataSource).CurrentRow;
                    }

                    if (row != null && ColumnName != null && row.Table.Columns.Contains(ColumnName))
                    {
                        return row[ColumnName];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Create a new instance of report datasource item
        /// </summary>
        /// <returns>a new instance of report datasource item</returns>
        public override ReportItem New()
        {
            return new ReportDataSourceItem();
        }

        /// <summary>
        /// Create a new copy of report datasource item
        /// </summary>
        /// <returns>a new copy of report datasource item</returns>
        public override ReportItem Copy()
        {
            ReportDataSourceItem newItem = new ReportDataSourceItem();
            newItem.ColumnName = this.ColumnName;
            newItem.ContentAlignment = this.ContentAlignment;
            newItem.Cells = this.Cells;
            newItem.Font = this.Font;
            newItem.Format = base.Format;
            newItem.NewLine = this.NewLine;
            newItem.Position = this.Position;
            return newItem;
        }

        /// <summary>
        /// Get string format of item
        /// </summary>
        /// <returns>string format of item</returns>
        public override string ToString()
        {
            return string.Format("{0}({1})", ERptMultiLanguage.GetLanValue("DataSourceItem"), this.ColumnName);//返回栏位值
            //return string.Format("{0}({1})", this.GetType().Name.Substring(6), this.ColumnName);
        }

        #region IReportGetValues Members

        public string[] GetValues(string propertyName)
        {
            object headerDataSource = Collection.Report.HeaderDataSource;

            DataTable table = null;
            if (headerDataSource is InfoBindingSource)
            {
                InfoBindingSource ibs = headerDataSource as InfoBindingSource;

                InfoDataSet ids = ibs.GetDataSource();
                if (ids.RealDataSet.Tables.Contains(ibs.DataMember))
                {
                    table = ids.RealDataSet.Tables[ibs.DataMember];
                }
                if (ids.RealDataSet.Relations.Contains(ibs.DataMember))
                {
                    table = ids.RealDataSet.Relations[ibs.DataMember].ChildTable;
                }

            }
            else if (headerDataSource is WebDataSource)
            {
                WebDataSource wds = headerDataSource as WebDataSource;
                if (string.IsNullOrEmpty(wds.SelectCommand) || string.IsNullOrEmpty(wds.SelectAlias))
                {
                    if (wds.DesignDataSet == null)
                    {
                        WebDataSet webDataSet = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                        if (webDataSet.RealDataSet != null)
                        {
                            wds.DesignDataSet = webDataSet.RealDataSet;
                        }
                    }
                    if (wds.DesignDataSet != null && wds.DesignDataSet.Tables.Contains(wds.DataMember))
                    {
                        table = wds.DesignDataSet.Tables[wds.DataMember];
                    }
                }
                else
                {
                    DataTable commandTable = wds.CommandTable;
                    if (commandTable != null)
                    {
                        table = commandTable;
                    }
                }
            }
            List<string> list = new List<string>();
            if (table != null)
            {
                foreach (DataColumn column in table.Columns)
                {
                    list.Add(column.ColumnName);
                }
            }
            return list.ToArray();
        }

        #endregion
    }

    /// <summary>
    /// class of image item collection
    /// </summary>
    public class ImageItemCollection : IList, ICollection, IEnumerable
    {
        private ArrayList list = new ArrayList();
        /// <summary>
        /// Add a image item to collection
        /// </summary>
        /// <param name="item">image item</param>
        /// <returns>index of item</returns>
        public int Add(ImageItem item)
        {
            return list.Add(item);
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Determines whether collection contains the item
        /// </summary>
        /// <param name="item">image item</param>
        /// <returns>true if collection contains item, otherwise false</returns>
        public bool Contains(ImageItem item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Get index of item in collection
        /// </summary>
        /// <param name="item">image item</param>
        /// <returns>index of item</returns>
        public int IndexOf(ImageItem item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Insert a item into collection
        /// </summary>
        /// <param name="index">index of item</param>
        /// <param name="item">image item</param>
        public void Insert(int index, ImageItem item)
        {
            list.Insert(index, item);
        }

        /// <summary>
        /// Get a value indicating whether collection has a fixed size
        /// </summary>
        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        /// <summary>
        /// Get a value indicating whether collection is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        /// <summary>
        /// Remove item from collection
        /// </summary>
        /// <param name="item">image item</param>
        public void Remove(ImageItem item)
        {
            list.Remove(item);
        }
        
        /// <summary>
        /// Remove item from collection
        /// </summary>
        /// <param name="index">index of item</param>
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        /// <summary>
        /// Get or set the image item
        /// </summary>
        /// <param name="index">index of item</param>
        /// <returns>image item</returns>
        public ImageItem this[int index]
        {
            get
            {
                return (ImageItem)list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        /// <summary>
        /// Create a new copy of collection
        /// </summary>
        /// <returns>a new copy of collection</returns>
        public ImageItemCollection Copy()
        {
            ImageItemCollection newCollection = new ImageItemCollection();
            foreach (ImageItem imageItem in this)
            {
                newCollection.Add(imageItem.Copy());
            }
            return newCollection;
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is ImageItem)
            {
                return list.Add(value);
            }
            return -1;
        }

        void IList.Clear()
        {
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is ImageItem)
            {
                list.Insert(index, value);
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            list.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        /// <summary>
        /// Get count of collection
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Class of image item
    /// </summary>
    public class ImageItem
    {
        private string name = "ImageItem";
        /// <summary>
        /// Get or set name of item
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Image image;
        /// <summary>
        /// Get or set image of item
        /// </summary>
        public Image Image
        {
            get
            {
                if (!string.IsNullOrEmpty(this.imageUrl))
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath(this.imageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            image = Image.FromFile(path);
                        }
                    }
                }

                return image;

            }
            set { image = value; }
        }

        private string imageUrl;
        /// <summary>
        /// Get or set url of image item
        /// </summary>
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [System.Web.UI.UrlProperty]
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        //private string imagePath;
        //[Browsable(false)]
        //public string ImagePath
        //{
        //    get { return imagePath; }
        //    set { imagePath = value; }
        //}

        /// <summary>
        /// Create a new copy of image item
        /// </summary>
        /// <returns></returns>
        public ImageItem Copy()
        {
            ImageItem imageItem = new ImageItem();
            imageItem.Name = this.Name;
            imageItem.Image = this.Image;
            imageItem.ImageUrl = this.ImageUrl;
  //          imageItem.ImagePath = this.ImagePath;
            return imageItem;
        }

        /// <summary>
        /// Get string format of item
        /// </summary>
        /// <returns>string format of item</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }

    /// <summary>
    /// Class of parameter collection 
    /// </summary>
    public class ParameterItemCollection : IList, ICollection, IEnumerable
    {
        private ArrayList list = new ArrayList();

        public int Add(ParameterItem item)
        {
            return list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(ParameterItem item)
        {
            return list.Contains(item);
        }

        public int IndexOf(ParameterItem item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, ParameterItem item)
        {
            list.Insert(index, item);
        }

        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public void Remove(ParameterItem item)
        {
            list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public ParameterItem this[int index]
        {
            get
            {
                return (ParameterItem)list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public ParameterItemCollection Copy()
        {
            ParameterItemCollection newCollection;
            newCollection = new ParameterItemCollection();
            foreach (ParameterItem paramItem in this)
            {
                newCollection.Add(paramItem.Copy());
            }
            return newCollection;
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is ParameterItem)
            {
                return list.Add(value);
            }
            return -1;
        }

        void IList.Clear()
        {
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is ParameterItem)
            {
                list.Insert(index, value);
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            list.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        /// <summary>
        /// Get count of collection
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Class of ParameterItem
    /// </summary>
    public class ParameterItem
    {
        private string name;
        /// <summary>
        /// Get or set name of item
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string paramValue;
        /// <summary>
        /// Get or set value of item
        /// </summary>
        public string Value
        {
            get { return paramValue; }
            set { paramValue = value; }
        }

        /// <summary>
        /// Create a new copy of parameter item
        /// </summary>
        /// <returns>a new copy of parameter item</returns>
        public ParameterItem Copy()
        {
            ParameterItem paramItem = new ParameterItem();
            paramItem.Name = this.Name;
            paramItem.Value = this.Value;
            return paramItem;
        }
    }

    /// <summary>
    /// Class of datasource item collection
    /// </summary>
    public class DataSourceItemCollection : IList, ICollection, IEnumerable
    {
        /// <summary>
        /// Create a new instance of datasource item collection
        /// </summary>
        /// <param name="rpt">owner report</param>
        public DataSourceItemCollection(IReport rpt)
        {
            report = rpt;
        }

        public DataSourceItemCollection() { }

        private IReport report;
        /// <summary>
        /// Get the owner report
        /// </summary>
        public IReport Report
        {
            get { return report; }
        }

        //internal void SetReport(IReport rpt)
        //{
        //    report = rpt;
        //}

        private ArrayList list = new ArrayList();
        /// <summary>
        /// Add a datasource item to collection
        /// </summary>
        /// <param name="item">datasource item</param>
        /// <returns>index of item</returns>
        public int Add(DataSourceItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            return list.Add(item);
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public void Clear()
        {
            foreach (DataSourceItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        /// <summary>
        /// Determines whether collection contains the item
        /// </summary>
        /// <param name="item">report item</param>
        /// <returns>true if collection contains item, otherwise false</returns>
        public bool Contains(DataSourceItem item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Get index of item in collection
        /// </summary>
        /// <param name="item">datasource item</param>
        /// <returns>index of item</returns>
        public int IndexOf(DataSourceItem item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Insert a item into collection
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, DataSourceItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            list.Insert(index, item);
        }

        /// <summary>
        /// Get a value indicating whether collection has a fixed size
        /// </summary>
        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        /// <summary>
        /// Get a value indicating whether collection is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        /// <summary>
        /// Remove item from collection
        /// </summary>
        /// <param name="item">data source item</param>
        public void Remove(DataSourceItem item)
        {
            if (item != null)
            {
                item.SetCollection(null);
            }
            list.Remove(item);
        }

        /// <summary>
        /// Remove item from collection
        /// </summary>
        /// <param name="index">index of item</param>
        public void RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        /// <summary>
        /// Get or set the datasource item
        /// </summary>
        /// <param name="index">index of item</param>
        /// <returns>datasource item</returns>
        public DataSourceItem this[int index]
        {
            get
            {
                return (DataSourceItem)list[index];
            }
            set
            {
                this[index].SetCollection(null);
                list[index] = value;
                this[index].SetCollection(this);
            }
        }

        /// <summary>
        /// Crate a new copy of collection
        /// </summary>
        /// <param name="ireport">owner report</param>
        /// <returns>a new copy of collection</returns>
        public DataSourceItemCollection Copy(IReport ireport)
        {
            DataSourceItemCollection newCollection = new DataSourceItemCollection(ireport);
            foreach (DataSourceItem item in this)
            {
                newCollection.Add(item.Copy());
            }
            return newCollection;
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is DataSourceItem)
            {
                ((DataSourceItem)value).SetCollection(this);
                return list.Add(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.Clear()
        {
            foreach (DataSourceItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is DataSourceItem)
            {
                ((DataSourceItem)value).SetCollection(this);
                list.Insert(index, value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            if (value is DataSourceItem)
            {
                ((DataSourceItem)value).SetCollection(null);
                list.Remove(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (value is DataSourceItem)
                {
                    ((DataSourceItem)list[index]).SetCollection(null);
                    list[index] = value;
                    ((DataSourceItem)list[index]).SetCollection(this);
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        /// <summary>
        /// Get count of collection
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Class of datasource item
    /// </summary>
    public class DataSourceItem
    {
        /// <summary>
        /// Create a new instance of datasource item
        /// </summary>
        public DataSourceItem()
        {
            fields = new FieldItemCollection(this);
        }

        private object dataSource;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal object DataSource
        {
            get
            {
                if (dataSource == null)
                {
                    if (this.Collection != null)
                    {
                        int index = this.Collection.IndexOf(this);
                        IReport report = this.Collection.Report;
                        if (report is EasilyReport)
                        {
                            if (index < (report as EasilyReport).DataSource.Count)
                            {
                                dataSource = (report as EasilyReport).DataSource[index].BindingSource;
                            }
                        }
                        else if (report is WebEasilyReport)
                        {
                            if (index < (report as WebEasilyReport).DataSource.Count)
                            {
                                string id = (report as WebEasilyReport).DataSource[index].DataSourceID;
                                dataSource = (report as WebEasilyReport).Parent.FindControl(id) as WebDataSource;
                                if (dataSource == null)
                                {
                                    dataSource = (report as WebEasilyReport).Page.FindControl(id) as WebDataSource;
                                }
                            }
                        }
                    }
                }

                return dataSource;
            }
            set { dataSource = value; }
        }

        /// <summary>
        /// Enum of group gap type
        /// </summary>
        public enum GroupGapType
        {
            None,
            EmptyRow,
            SingleLine,
            DoubleLine,
            Page,
            Sheet
        }

        private GroupGapType groupGap = GroupGapType.None;
        /// <summary>
        /// Get or set group gap of item
        /// </summary>
        public GroupGapType GroupGap
        {
            get { return groupGap; }
            set { groupGap = value; }
        }

        private bool groupTotal;
        /// <summary>
        /// Get or set group total of item
        /// </summary>
        public bool GroupTotal
        {
            get { return groupTotal; }
            set { groupTotal = value; }           
        }

        /// <summary>
        /// Enum of caption style type
        /// </summary>
        public enum CaptionStyleType
        {
            ColumnHeader,
            RowHeader
        }

        private CaptionStyleType captionStyle = CaptionStyleType.ColumnHeader;
        /// <summary>
        /// Get or set caption style of item
        /// </summary>
        [Category("Infolight"),
        Description("The style of the caption.")]
        public CaptionStyleType CaptionStyle
        {
            get { return captionStyle; }
            set { captionStyle = value; }
        }

        private FieldItemCollection fields;
        /// <summary>
        /// Get fields of datasource item
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        public FieldItemCollection Fields
        {
            get { return fields; }
        }

        private DataSourceItemCollection collection;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal DataSourceItemCollection Collection
        {
            get { return collection; }
        }

        internal void SetCollection(DataSourceItemCollection col)
        {
            collection = col;
        }

        /// <summary>
        /// Create a new copy of datasource item
        /// </summary>
        /// <returns>a new copy of datasource item</returns>
        public DataSourceItem Copy()
        {
            DataSourceItem dataSourceItem = new DataSourceItem();
            foreach (FieldItem field in this.Fields)
            {
                dataSourceItem.Fields.Add(field.Copy());
            }
            dataSourceItem.CaptionStyle = this.CaptionStyle;
            dataSourceItem.GroupGap = this.GroupGap;
            dataSourceItem.GroupTotal = this.GroupTotal;
            return dataSourceItem;
        }
    }

    /// <summary>
    /// Class of field item collection
    /// </summary>
    public class FieldItemCollection : IList, ICollection, IEnumerable
    {
        /// <summary>
        /// Create a new instance of field item collection
        /// </summary>
        /// <param name="item">owner of collection</param>
        public FieldItemCollection(DataSourceItem item)
        {
            owner = item;
        }

        public FieldItemCollection() { }

        private DataSourceItem owner;
        /// <summary>
        /// Get the owner of collection
        /// </summary>
        public DataSourceItem Owner
        {
            get { return owner; }
        }

        private DataTable dictionaryTable;

        internal DataTable DictionaryTable
        {
            get
            {
                if (dictionaryTable == null)
                {
                    if (this.Owner != null)
                    {
                        object dataSource = this.Owner.DataSource;
                        if (dataSource != null)
                        {
                            if (dataSource is InfoBindingSource)
                            {
                                InfoBindingSource ibs = dataSource as InfoBindingSource;
                                dictionaryTable = DBUtils.GetDataDictionary(ibs, ibs.Site != null && ibs.Site.DesignMode).Tables[0];
                            }
                            else if (dataSource is WebDataSource)
                            {
                                WebDataSource wds = dataSource as WebDataSource;
                                dictionaryTable = DBUtils.GetDataDictionary(wds, wds.Site != null && wds.Site.DesignMode).Tables[0];
                            }
                        }
                    }
                }
                return dictionaryTable;
            }
        }

        private ArrayList list = new ArrayList();
        /// <summary>
        /// Add a field item to collection
        /// </summary>
        /// <param name="item">field item</param>
        /// <returns>index of item</returns>
        public int Add(FieldItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            return list.Add(item);
        }

        public void Clear()
        {
            foreach (FieldItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        public bool Contains(FieldItem item)
        {
            return list.Contains(item);
        }

        public int IndexOf(FieldItem item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, FieldItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            list.Insert(index, item);
        }

        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public void Remove(FieldItem item)
        {
            if (item != null)
            {
                item.SetCollection(null);
            }
            list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        public FieldItem this[int index]
        {
            get
            {
                return (FieldItem)list[index];
            }
            set
            {
                this[index].SetCollection(null);
                list[index] = value;
                this[index].SetCollection(this);
            }
        }

        public FieldItem this[string columnName]
        {
            get
            {
                foreach (FieldItem field in this)
                {
                    if (field.ColumnName == columnName)
                    {
                        return field;
                    }
                }
                return null;
            }
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is FieldItem)
            {
                ((FieldItem)value).SetCollection(this);
                return list.Add(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.Clear()
        {
            foreach (FieldItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is FieldItem)
            {
                ((FieldItem)value).SetCollection(this);
                list.Insert(index, value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            if (value is FieldItem)
            {
                ((FieldItem)value).SetCollection(null);
                list.Remove(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (value is FieldItem)
                {
                    ((FieldItem)list[index]).SetCollection(null);
                    list[index] = value;
                    ((FieldItem)list[index]).SetCollection(this);
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    public class FieldItem: IReportGetValues
    {
        private string caption;
        /// <summary>
        /// Get or set caption of item
        /// </summary>
        [Category("Infolight"),
        Description(" The caption of this  Column.")]
        public string Caption
        {
            get 
            {
                if (string.IsNullOrEmpty(caption))
                {
                    return columnName;
                }
                else
                {
                    return caption;
                }
            }
            set { caption = value; }
        }

        private HorizontalAlignment captionAlignment;
        /// <summary>
        /// Get or set caption alignment of item
        /// </summary>
        [Category("Infolight"),
        Description("The alignment of the caption.")]
        public HorizontalAlignment CaptionAlignment
        {
            get { return captionAlignment; }
            set { captionAlignment = value; }
        }

        private string columnName;
        /// <summary>
        /// Get or set column name of item
        /// </summary>
        [Editor(typeof(Infolight.EasilyReportTools.Design.PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Infolight"),
        Description("Specifies the column which will be showed in the report.")]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private HorizontalAlignment columnAlignment;
        /// <summary>
        /// Get or set column alignment of item
        /// </summary>
        [Category("Infolight"),
        Description("The alignment of the selected column.")]
        public HorizontalAlignment ColumnAlignment
        {
            get { return columnAlignment; }
            set { columnAlignment = value; }

            //get 
            //{
            //    if (initial && !string.IsNullOrEmpty(ColumnName) && this.Collection != null)
            //    {
            //        DataTable ddTable = this.Collection.DictionaryTable;
            //        if (ddTable != null)
            //        {
            //            DataRow[] rows = ddTable.Select(string.Format("{0}='{1}'", DDInfo.FieldName, ColumnName.Replace("'", "''")));
            //            if (rows.Length > 0)
            //            {
            //                string type = rows[0][DDInfo.FieldType].ToString();
            //                if (string.Compare(type, "int", true) == 0 || string.Compare(type, "decimal", true) == 0
            //                    || string.Compare(type, "float", true) == 0 || string.Compare(type, "double", true) == 0)
            //                {
            //                    columnAlignment = HorizontalAlignment.Right;
            //                }
            //            }
            //        }
            //        initial = false;
            //    }

            //    return columnAlignment;
            //}
            //set 
            //{ 
            //    columnAlignment = value;
            //    initial = false;
            //}
        }

        private bool initial = true;

        private int width = 10;
        /// <summary>
        /// Get or set width of item
        /// </summary>
        [Category("Infolight"),
        Description("The current width of column.The width of characters in English as a unit ")]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int cells = 1;
        /// <summary>
        /// Get or set cells of item
        /// </summary>
        [Category("Infolight"),
        Description("The cells of column")]
        public int Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        private bool newLine;
        /// <summary>
        /// Get or set new line of item
        /// </summary>
        [Category("Infolight"),
        Description("Indicates whether this column can display in the new line.")]
        public bool NewLine
        {
            get { return newLine; }
            set { newLine = value; }
        }

        private int newLinePosition = 1;
        /// <summary>
        /// Get or set new line postion of item
        /// </summary>
        [Category("Infolight"),
        Description("Defines the position of the column displaied in the new line.")]
        public int NewLinePostion
        {
            get { return newLinePosition; }
            set { newLinePosition = value; }
        }

        private string format;
        /// <summary>
        /// Get or set format of item
        /// </summary>
        [Category("Infolight"),
        Description("The format of column")]
        public string Format
        {
            get
            {
                if (!string.IsNullOrEmpty(ColumnName) && string.IsNullOrEmpty(format) && this.Collection != null)
                {
                    DataTable ddTable = this.Collection.DictionaryTable;
                    if (ddTable != null)
                    {
                        DataRow[] rows = ddTable.Select(string.Format("{0}='{1}'", DDInfo.FieldName, ColumnName.Replace("'", "''")));
                        if (rows.Length > 0)
                        {
                            string mask = rows[0][DDInfo.EditMask].ToString();
                            if (!string.IsNullOrEmpty(mask))
                            {
                                return mask;
                            }
                        }
                    }

                }
                return format;
            }
            set
            {
                format = value;
            }
        }

        /// <summary>
        /// Enum of sum type
        /// </summary>
        public enum SumType
        {
            None,
            Sum,
            Count,
            Max,
            Min,
            Average
        }

        private SumType sum = SumType.None;
        /// <summary>
        /// Get or set
        /// </summary>
        [Category("Infolight"),
        Description("Indicates the calculation mode of this column and the result will be showed at the bottom of the report or at the Group Footer.")]
        public SumType Sum
        {
            get { return sum; }
            set { sum = value; }
        }

        public enum OrderType
        {
            None,
            Ascend,
            Descend
        }

        private OrderType order = OrderType.None;
        [Category("Infolight"),
        Description("Specifies the type of order.")]
        public OrderType Order
        {
            get { return order; }
            set { order = value; }
        }

        public enum GroupType
        {
            None,
            Normal,
            Excel
        }

        private GroupType group = GroupType.None;
        /// <summary>
        /// Get
        /// </summary>
        [Category("Infolight"),
        Description("Specifies the type of group.")]
        public GroupType Group
        {
            get { return group; }
            set { group = value; }
        }

        private string groupTotalCaption;
        /// <summary>
        /// Get or set group total caption of item
        /// </summary>
        [Category("Infolight"),
        Description("Specifies the caption of group total.")]
        public string GroupTotalCaption
        {
            get
            {
                return groupTotalCaption;
            }
            set
            {
                groupTotalCaption = value;
            }
        }

        private string totalCaption;
        /// <summary>
        /// Get or set total caption of item
        /// </summary>
        [Category("Infolight"),
        Description("Specifies the caption of total.")]
        public string TotalCaption
        {
            get
            {
                return totalCaption;
            }
            set
            {
                totalCaption = value;
            }
        }

        private bool suppressIfDuplicated;
        [Category("Infolight"),
        Description("Specifies the value whether suppress if duplicated.")]
        public bool SuppressIfDuplicated
        {
            get { return suppressIfDuplicated; }
            set { suppressIfDuplicated = value; }
        }
	

        private FieldItemCollection collection;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal FieldItemCollection Collection
        {
            get { return collection; }
        }

        internal void SetCollection(FieldItemCollection col)
        {
            collection = col;
        }

        public FieldItem Copy()
        {
            FieldItem fieldItem = new FieldItem();
            fieldItem.Caption = this.Caption;
            fieldItem.CaptionAlignment = this.CaptionAlignment;
            fieldItem.Cells = this.Cells;
            fieldItem.ColumnAlignment = this.ColumnAlignment;
            fieldItem.ColumnName = this.ColumnName;
            fieldItem.Format = this.Format;
            fieldItem.Group = this.Group;
            fieldItem.GroupTotalCaption = this.GroupTotalCaption;
            fieldItem.NewLine = this.NewLine;
            fieldItem.NewLinePostion = this.NewLinePostion;
            fieldItem.Order = this.Order;
            fieldItem.Sum = this.Sum;
            fieldItem.SuppressIfDuplicated = this.SuppressIfDuplicated;
            fieldItem.TotalCaption = this.TotalCaption;
            fieldItem.Width = this.Width;
            return fieldItem;
        }

        public override string ToString()
        {
            return this.Caption;
        }

        #region IReportGetValues Members

        public string[] GetValues(string propertyName)
        {
            object headerDataSource = Collection.Owner.DataSource;

            DataTable table = null;
            if (headerDataSource is InfoBindingSource)
            {
                InfoBindingSource ibs = headerDataSource as InfoBindingSource;

                InfoDataSet ids = ibs.GetDataSource();
                if (ids.RealDataSet.Tables.Contains(ibs.DataMember))
                {
                    table = ids.RealDataSet.Tables[ibs.DataMember];
                }
                if (ids.RealDataSet.Relations.Contains(ibs.DataMember))
                {
                    table = ids.RealDataSet.Relations[ibs.DataMember].ChildTable;
                }

            }
            else if (headerDataSource is WebDataSource)
            {
                WebDataSource wds = headerDataSource as WebDataSource;
                if (string.IsNullOrEmpty(wds.SelectCommand) || string.IsNullOrEmpty(wds.SelectAlias))
                {
                    if (wds.DesignDataSet == null)
                    {
                        WebDataSet webDataSet = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                        if (webDataSet.RealDataSet != null)
                        {
                            wds.DesignDataSet = webDataSet.RealDataSet;
                        }
                    }
                    if (wds.DesignDataSet != null && wds.DesignDataSet.Tables.Contains(wds.DataMember))
                    {
                        table = wds.DesignDataSet.Tables[wds.DataMember];
                    }
                }
                else
                {
                    DataTable commandTable = wds.CommandTable;
                    if (commandTable != null)
                    {
                        table = commandTable;
                    }
                }
            }
            List<string> list = new List<string>();
            if (table != null)
            {
                foreach (DataColumn column in table.Columns)
                {
                    list.Add(column.ColumnName);
                }
            }
            return list.ToArray();
        }

        #endregion
    }
}
