using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace OfficeTools
{
    /// <summary>
    /// The interface of IAutomation
    /// </summary>
    public interface IAutomation
    {
        /// <summary>
        /// The infomation durning the output process
        /// </summary>
        string TagInfo { get; set;}

        /// <summary>
        /// The finished process number the output process
        /// </summary>
        int ProgressInfo { get; set;}

        /// <summary>
        /// The total process count durning the output process
        /// </summary>
        int ProgressCount { get; set;}

        /// <summary>
        /// Run Excel Automation
        /// </summary>
        void Run();
    }

    public abstract class OfficeAutomation: IAutomation
    {
        private string _FileName;
        /// <summary>
        /// Name of excel file
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private IOfficePlate _Plate;
        /// <summary>
        /// The officeplate whose defination is used
        /// </summary>
        public IOfficePlate Plate
        {
            get { return _Plate; }
            set { _Plate = value; }
        }

        private bool _ErrorReport;
        /// <summary>
        /// Record error infomation in excel file
        /// </summary>
        public bool ErrorReport
        {
            get { return _ErrorReport; }
            set { _ErrorReport = value; }
        }

        private PlateLog _Log;
        /// <summary>
        /// The log of output process
        /// </summary>
        public PlateLog Log
        {
            get { return _Log; }
            set { _Log = value; }
        }

        #region IAutomation Members

        private string _TagInfo;
        /// <summary>
        /// The infomation durning the output process
        /// </summary>
        public string TagInfo
        {
            get { return _TagInfo; }
            set { _TagInfo = value; }
        }

        private int _ProgressInfo;
        /// <summary>
        /// The finished process number the output process
        /// </summary>
        public int ProgressInfo
        {
            get { return _ProgressInfo; }
            set { _ProgressInfo = value; }
        }

        private int _ProgressCount;
        /// <summary>
        /// The total process count durning the output process
        /// </summary>
        public int ProgressCount
        {
            get { return _ProgressCount; }
            set { _ProgressCount = value; }
        }

        /// <summary>
        /// Run Excel Automation, in this class is abstract
        /// </summary>
        public abstract void Run();

        #endregion

        /// <summary>
        /// Get the count of data in datasource
        /// </summary>
        /// <param name="sTag">The text of tag</param>
        /// <returns>The count of data in datasource</returns>
        protected int GetDataSourceRowCount(ref string sTag)
        {
            int indexleftb = sTag.IndexOf('(');
            int indexrightb = sTag.IndexOf(')');
            if (indexleftb > 0 && indexrightb > indexleftb + 1)
            {
                string tagname = sTag.Substring(0, indexleftb).Trim();
                for (int i = 0; i < Plate.DataSource.Count; i++)
                {
                    if ((Plate.DataSource[i] as DataSourceItem).Caption == tagname)
                    {
                        string tagparam = sTag.Substring(indexleftb + 1, indexrightb - indexleftb - 1);
                        string[] strparam = tagparam.Split(',');
                        if (strparam.Length == 2)
                        {
                            if(string.Compare(strparam[0].Trim(), "x",true)== 0)
                            {
                                sTag = "$" + tagname + "({0}," + strparam[1].Trim() + ")";
                                Hashtable table = ((DataSourceItem)Plate.DataSource[i]).GetTable();
                                if (table.Contains(((DataSourceItem)Plate.DataSource[i]).DataMember))
                                {
                                    return ((DataView)table[((DataSourceItem)Plate.DataSource[i]).DataMember]).Count;
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Copy the array of value to list
        /// </summary>
        /// <param name="count">The times to copy</param>
        /// <param name="value">The value to copy</param>
        /// <param name="list">The list to copy to</param>
        protected void CopyRow(int count, object[] value, ArrayList list)
        {
            for (int i = 0; i < count; i++)
            {
                this.TagInfo = string.Format("Insert new row:{0}/{1}", i.ToString(), count.ToString());

                object[] newrow = new object[value.Length];
                for (int j = 0; j < value.Length; j++)
                {
                    newrow[j] = value[j];
                    if (newrow[j] is string)
                    {
                        string strvalue = newrow[j].ToString();
                        if (strvalue.StartsWith("$") && strvalue.Contains("{0}"))
                        {
                            strvalue = string.Format(strvalue, i.ToString());
                        }
                        newrow[j] = strvalue;
                    }
                }
                ProcessTag(newrow, list);
            }
        }

        /// <summary>
        /// Process tags and insert into the list
        /// </summary>
        /// <param name="value">The text of tag to process</param>
        /// <param name="list">The list to insert into</param>
        protected void ProcessTag(object[] value, ArrayList list)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] is string)
                {
                    string strvalue = value[i].ToString();
                    if (strvalue.StartsWith("$") && strvalue.Length > 1)
                    {
                        this.TagInfo = "Process tag:" + strvalue;
                        object[] obj = Automation.Run(Plate, strvalue.Substring(1));
                        if ((PlateException)obj[0] == PlateException.None)
                        {
                            value[i] = obj[1];
                        }
                        else
                        {
                            if (ErrorReport)
                            {
                                value[i] = string.Format("$Error: {0}", obj[0].ToString());
                            }
                            Log.WriteErrorInfo(obj[0].ToString(), strvalue, obj[1].ToString());
                        }
                    }
                }
            }
            list.Add(value);
        }

    }
}
