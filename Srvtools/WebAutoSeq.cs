using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Resources;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

namespace Srvtools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebAutoSeq), "Resources.WebAutoSeq.ico")]
    public class WebAutoSeq : WebControl, IGetValues
    {
        public WebAutoSeq()
        {
            this.StartValue = 1;
            this.NumDig = 2;
            this.Step = 1;
        }

        private bool active = true;
        [Category("Infolight"),
        Description("Indicates whether WebAutoSeq is enabled or disabled")]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        private string fieldname;
        [Category("Infolight"),
        Description("The column which WebAutoSeq is applied to")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string FieldName
        {
            get
            {
                return fieldname;
            }
            set
            {
                fieldname = value;
            }
        }

        private int numdig;
        [Category("Infolight"),
        Description("The digit of coding by WebAutoSeq")]
        [NotifyParentProperty(true)]
        public int NumDig
        {
            get
            {
                return numdig;
            }
            set
            {
                numdig = value;
            }
        }

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }


        [Category("Infolight"),
        Description("The ID of WebDataSource of the master table while the control is bound to a WebDataSource of the detail table")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string MasterDataSourceID
        {
            get
            {
                object obj = this.ViewState["MasterDataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["MasterDataSourceID"] = value;
            }
        }

        private bool renumber;
        [Category("Infolight"),
        Description("Indicates whether the data of the detail table need to be recoding after the masterdatasource has applied")]
        [NotifyParentProperty(true)]
        public bool ReNumber
        {
            get
            {
                return renumber;
            }
            set
            {
                renumber = value;
            }
        }

        private int startvalue;
        [Category("Infolight"),
        Description("The first number of each regular coding by WebAutoSeq")]
        [NotifyParentProperty(true)]
        public int StartValue
        {
            get
            {
                return startvalue;
            }
            set
            {
                startvalue = value;
            }
        }

        private int step;
        [Category("Infolight"),
        Description("The increment between the coding by WebAutoSeq")]
        [NotifyParentProperty(true)]
        public int Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

        private string _getFixed;
        [Category("Infolight"),
        Description("Pre-code of AutoSeq")]
        public String GetFixed
        {
            get { return _getFixed; }
            set { _getFixed = value; }
        }

        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        private object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }

        private string FixString
        {
            get
            {
                return CliUtils.GetValue(this.GetFixed, this.Page).ToString();
            }
        }

        private string GetValue(int num, string fixString, bool digtal)
        {
            if (digtal)
            {
                return fixString + num.ToString();
            }
            {
                StringBuilder format = new StringBuilder(fixString);
                format.Append("{0:");
                format.Append('0', this.NumDig);
                format.Append("}");
                return string.Format(format.ToString(), num);
            }
        }

        private DataView GetDataView(WebDataSource webDataSource)
        {
            if (string.IsNullOrEmpty(webDataSource.MasterDataSource))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, webDataSource.GetType(), webDataSource.ID, "MasterDataSource", null);
            }
            DataView view = webDataSource.View;
            string fix = this.FixString;
            if (fix == null)
            {
                fix = string.Empty;
            }
            if (!view.Table.Columns.Contains(this.FieldName))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, this.GetType(), this.ID, "FieldName", this.FieldName);
            }
            else if (view.Table.Columns[this.FieldName].DataType == typeof(int) || view.Table.Columns[this.FieldName].DataType == typeof(Decimal))
            {
                if (!string.IsNullOrEmpty(fix))
                {
                    int temp;
                    if (!int.TryParse(fix, out temp))
                    {
                        throw new EEPException(EEPException.ExceptionType.ColumnTypeInvalid, this.GetType(), this.ID, FieldName, view.Table.Columns[this.FieldName].DataType.Name);
                    }
                }
            }
            else if (view.Table.Columns[this.FieldName].DataType != typeof(string) && view.Table.Columns[this.FieldName].DataType != typeof(char))
            {
                throw new EEPException(EEPException.ExceptionType.ColumnTypeInvalid, this.GetType(), this.ID, FieldName, view.Table.Columns[this.FieldName].DataType.Name);
            }
            return view;
        }

        public string GetValue()
        {
            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                WebDataSource wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                if (wds == null)
                {
                    throw new EEPException(EEPException.ExceptionType.ControlNotFound, this.GetType(), this.ID, "WebDataSource", this.DataSourceID);
                }
                DataView view = GetDataView(wds);
                if (view != null)
                {
                    bool digtal = view.Table.Columns[this.FieldName].DataType == typeof(int) || view.Table.Columns[this.FieldName].DataType == typeof(Decimal);
                    int num = this.StartValue;
                    string fix = this.FixString;
                    for (int i = 0; i < view.Count; i++)
                    {
                        string value = view[i][this.FieldName].ToString();
                        if (value.Length > fix.Length)
                        {
                            if (fix.Length > 0 && value.StartsWith(fix))
                            {
                                value = value.Substring(fix.Length);
                            }
                            int intValue;
                            if (int.TryParse(value, out intValue))
                            {
                                if (intValue >= num)
                                {
                                    num = intValue + Step;
                                }
                            }
                        }
                    }

                    return GetValue(num, fix, digtal);
                }
            }
            return string.Empty;
        }

        public void ResetDetail()
        {
            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                WebDataSource wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                DataView view = GetDataView(wds);
                if (view != null)
                {
                    bool digtal = view.Table.Columns[this.FieldName].DataType == typeof(int) || view.Table.Columns[this.FieldName].DataType == typeof(Decimal);
                    int num = this.StartValue;
                    string fix = this.FixString;
                    List<string> primaryKey = new List<string>();
                    foreach (DataColumn dc in view.Table.PrimaryKey)
                    {
                        primaryKey.Add(dc.ColumnName);
                    }
                    for (int i = 0; i < view.Count; i++)
                    {
                        DataRow row = view[i].Row;
                        int tempnum = num;
                        if (row[this.FieldName].ToString() != GetValue(tempnum, fix, digtal))
                        {
                            if (primaryKey.Contains(this.FieldName))
                            {
                                int index = primaryKey.IndexOf(FieldName);
                                object[] key = new object[view.Table.PrimaryKey.Length];
                                for (int j = 0; j < view.Table.PrimaryKey.Length; j++)
                                {
                                    key[j] = row[view.Table.PrimaryKey[j].ColumnName];
                                }

                                while (true)
                                {
                                    key[index] = GetValue(tempnum, fix, digtal);
                                    DataRow dr = view.Table.Rows.Find(key);
                                    if (dr == null)
                                    {
                                        break;
                                    }
                                    else if (row[this.FieldName].ToString() != GetValue(tempnum, fix, digtal))
                                    {

                                        tempnum += Step;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            Hashtable keys = new Hashtable();
                            Hashtable values = new Hashtable();
                            Hashtable oldvalues = new Hashtable();
                            foreach (DataColumn dc in view.Table.Columns)
                            {
                                keys.Add(dc.ColumnName, dc.ColumnName);
                                oldvalues.Add(dc.ColumnName, row[dc.ColumnName]);
                                if (dc.ColumnName == this.FieldName)
                                {
                                    row[this.FieldName] = GetValue(tempnum, fix, digtal);
                                }
                                values.Add(dc.ColumnName, row[dc.ColumnName]);
                            }

                            wds.Update(keys, values, oldvalues);
                        }
                        if (tempnum == num)
                        {
                            num += Step;
                        }
                    }
                }
            }
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            List<string> values = new List<string>();
            if (this is WebAutoSeq)
            {
                if (string.Compare(sKind, "datasourceid", true) == 0 || string.Compare(sKind, "masterdatasourceid", true) == 0)//IgnoreCase
                {
                    ControlCollection ctrlList = ((WebAutoSeq)this).Page.Controls;
                    foreach (Control ctrl in ctrlList)
                    {
                        if (ctrl is WebDataSource)
                        {
                            values.Add(ctrl.ID);
                        }
                    }
                }
                else if (string.Compare(sKind, "fieldname", true) == 0)
                {
                    WebAutoSeq was = (WebAutoSeq)this;
                    if (was.Page != null && was.DataSourceID != null && was.DataSourceID != "")
                    {
                        foreach (Control ctrl in was.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == was.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null)
                                {
                                    WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                    if (wds != null)
                                    {
                                        ds.DesignDataSet = wds.RealDataSet;
                                    }
                                }
                                if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                                {
                                    foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return values.ToArray();
        }

        #endregion


    }
}
