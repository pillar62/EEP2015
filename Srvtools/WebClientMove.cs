using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Resources;
using System.IO;
using System.Xml.Serialization;
using Srvtools;
using System.Collections;
using System.Web.UI;
using System.Drawing;

namespace Srvtools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebClientMove), "Resources.WebClientMove.png")]
    public partial class WebClientMove : WebControl, IGetValues
    {
        private SYS_LANGUAGE language;
        
        public WebClientMove()
        {
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            _KeyMatchColumns = new WebKeyMatchColumns(this, typeof(WebKeyMatchColumn));
            _MatchColumns = new WebMatchColumns(this, typeof(WebMatchColumn));
        }

        public WebClientMove(IContainer container)
        {
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            _KeyMatchColumns = new WebKeyMatchColumns(this, typeof(WebKeyMatchColumn));
            _MatchColumns = new WebMatchColumns(this, typeof(WebMatchColumn));
            container.Add(this);
        }

        [Category("Infolight"),
        Description("The WebDataSource of source table")]
        [Editor(typeof(WebGetSource), typeof(UITypeEditor))]
        public string SrcDataSorce
        {
            get
            {
                object obj = this.ViewState["SrcDataSorce"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["SrcDataSorce"] = value;
            }
        }

        [Category("Infolight"),
        Description("The WebDataSource of destination table")]
        [Editor(typeof(WebGetSource), typeof(UITypeEditor))]
        public string DestDataSorce
        {
            get
            {
                object obj = this.ViewState["DestDataSorce"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DestDataSorce"] = value;
            }
        }

        private bool _AlwaysInsert;
        [Category("Infolight"),
        Description("Indicates whether data is always inserted even the same data of key columns exsiting in database")]
        public bool AlwaysInsert
        {
            get
            {
                return _AlwaysInsert;
            }
            set
            {
                _AlwaysInsert = value;
            }
        }

        private bool _AlwaysReplace;
        [Category("Infolight"),
         Description("Indicates whether data is always replaced even the same data of key columns exsiting in database")]
        public bool AlwaysReplace
        {
            get
            {
                return _AlwaysReplace;
            }
            set
            {
                _AlwaysReplace = value;
            }
        }

        private WebKeyMatchColumns _KeyMatchColumns;
        [Category("Infolight"),
         Description("Specifies the columns storing the relational infomation between source table and destination table")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public WebKeyMatchColumns KeyMatchColumns
        {
            get
            {
                return _KeyMatchColumns;
            }
        }

        private WebMatchColumns _MatchColumns;
        [Category("Infolight"),
         Description("Specifies the columns storing the infomation to transfer between source table and destination table")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public WebMatchColumns MatchColumns
        {
            get
            {
                return _MatchColumns;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "srcdatasorce", true) == 0 || string.Compare(sKind, "destdatasorce", true) == 0)//IgnoreCase
            {
                if (this.Page != null)
                {
                    foreach (System.Web.UI.Control ctrl in this.Page.Controls)
                        if (ctrl is WebDataSource)
                        {
                            WebDataSource ds = (WebDataSource)ctrl;
                            values.Add(ds.ID);
                        }
                    if (values.Count > 0)
                    {
                        int i = values.Count;
                        retList = new string[i];
                        for (int j = 0; j < i; j++)
                            retList[j] = values[j];
                    }
                }
            }
            return retList;
        }

        public void Execute(bool progress)
        {
            if ((SrcDataSorce != null || SrcDataSorce != "") && (DestDataSorce != "" || DestDataSorce != null))
            {
                bool tempSrc = false;
                bool tempDest = false;
                WebGridView wgv = new WebGridView();
                WebDetailsView wdv = new WebDetailsView();
                WebFormView wfv = new WebFormView();

                WebDataSource wsSrc = new WebDataSource();
                DataSet dsSrc = new DataSet();
                WebDataSource wsDest = new WebDataSource();
                DataSet dsDest = new DataSet();
                WebDataSource wsDestM = new WebDataSource();
                foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                {
                    if (c is WebDataSource && (c as WebDataSource).ID == this.SrcDataSorce)
                    {
                        wsSrc = c as WebDataSource;
                        tempSrc = wsSrc.AutoApply;
                        wsSrc.AutoApply = false;
                        dsSrc = wsSrc.InnerDataSet;
                    }
                    if (c is WebDataSource && (c as WebDataSource).ID == this.DestDataSorce)
                    {
                        wsDest = c as WebDataSource;
                        tempDest = wsDest.AutoApply;
                        wsDest.AutoApply = false;
                        dsDest = wsDest.InnerDataSet;
                        foreach (System.Web.UI.Control ctrl in this.Page.Form.Controls)
                            if (ctrl is WebDataSource && (ctrl as WebDataSource).ID == wsDest.MasterDataSource)
                            {
                                wsDestM = ctrl as WebDataSource;
                                //added by lily 2006/12/14 for detail has no innerdataset
                                dsDest = wsDestM.InnerDataSet;
                                //added by lily 2006/12/14 for detail has no innerdataset
                                break;
                            }
                    }
                }
                bool isGet = false;
                DataTable dtSrc = new DataTable();
                for (int i = 0; i < dsSrc.Relations.Count; i++)
                    foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                        if (c is WebDataSource && (c as WebDataSource).ID == this.SrcDataSorce
                            && (c as WebDataSource).DataMember == dsSrc.Relations[i].ChildTable.ToString())
                        {
                            dtSrc = dsSrc.Relations[i].ChildTable;
                            isGet = true;
                        }
                if (isGet == false)
                    if (dsSrc.Tables.Count > 0)
                        dtSrc = dsSrc.Tables[0];
                    else
                        dtSrc = wsSrc.CommandTable;
                DataTable dtDest = new DataTable();
                isGet = false;
                for (int i = 0; i < dsDest.Relations.Count; i++)
                    foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                        if (c is WebDataSource && (c as WebDataSource).ID == this.DestDataSorce
                            && (c as WebDataSource).DataMember == dsDest.Relations[i].ChildTable.ToString())
                        {
                            dtDest = dsDest.Relations[i].ChildTable;
                            isGet = true;
                        }
                if (isGet == false)
                    dtDest = dsDest.Tables[0];
                DataTable dtDestM = new DataTable();
                isGet = false;
                for (int i = 0; i < wsDestM.InnerDataSet.Relations.Count; i++)
                    if (this.DestDataSorce == wsDestM.InnerDataSet.Relations[i].ChildTable.ToString())
                    {
                        dtDestM = wsDestM.InnerDataSet.Relations[i].ChildTable;
                        isGet = true;
                    }
                if (isGet == false)
                    dtDestM = wsDestM.InnerDataSet.Tables[0];


                DataTable dsDestMaster =dsDest.Tables[0];
                string MasterKey = null;
                foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                {
                    if (c is WebFormView)
                        foreach (System.Web.UI.Control ctrl in this.Page.Form.Controls)
                            if (ctrl is WebDataSource && (ctrl as WebDataSource).ID == (c as WebFormView).DataSourceID
                                && (ctrl as WebDataSource).DataMember == dsDestMaster.ToString())
                            {
                                MasterKey = dsDestMaster.Rows[(c as WebFormView).DataItemIndex][dsDestMaster.PrimaryKey[0].ToString()].ToString();
                                break;
                            }
                    if (c is WebDetailsView)
                        foreach (System.Web.UI.Control ctrl in this.Page.Form.Controls)
                            if (ctrl is WebDataSource && (ctrl as WebDataSource).ID == (c as WebDetailsView).DataSourceID 
                                && (ctrl as WebDataSource).DataMember == dsDestMaster.ToString())
                            {
                                MasterKey = dsDestMaster.Rows[(c as WebDetailsView).DataItemIndex][dsDestMaster.PrimaryKey[0].ToString()].ToString();
                                break;
                            }
                }
                
                //modified by lily 2006/12/14 for: List<> can be added when needed, DataRow[] cannot
                //DataRow[] drDestTemp = new DataRow[dtDest.Rows.Count];

                List<DataRow> drDestTemp = new List<DataRow>();

                for (int i = 0; i < dtDest.Rows.Count; i++)
                    if (dtDest.Rows[i][dsDestMaster.PrimaryKey[0].ToString()].ToString() == MasterKey)
                        drDestTemp.Add(dtDest.Rows[i]);
                        //drDestTemp[i] = dtDest.Rows[i];

                ArrayList strSrc = new ArrayList();
                ArrayList strDest = new ArrayList();
                foreach (WebKeyMatchColumn kmc in this.KeyMatchColumns)
                    if (kmc.SrcColumnName == null || kmc.DestColumnName == null || kmc.SrcColumnName == "" || kmc.DestColumnName == "")
                    {
                        language = CliUtils.fClientLang;
                        string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoClientMove", "iskeyMatchColumn", true);
                        this.Page.Response.Write("<script>alert(\"" + message + "\");</script>");
                        return;
                    }
                    else
                    {
                        strSrc.Add(kmc.SrcColumnName);
                        strDest.Add(kmc.DestColumnName);
                    }

                for (int i = 0; i < dtSrc.Rows.Count; i++)
                {
                    int flag = 0;
                    Hashtable ht = new Hashtable();
                    if (this.AlwaysInsert == false && strSrc != null && drDestTemp.Count > 0)
                        for (int j = 0; j < drDestTemp.Count; j++)
                        {
                            for (int x = 0; x < strSrc.Count; x++)
                                if (drDestTemp[j] != null && dtSrc.Rows[i][strSrc[x].ToString()].ToString() == drDestTemp[j][strDest[x].ToString()].ToString())
                                    flag++;
                            if (flag == strSrc.Count)
                            {
                                if (this.AlwaysReplace == true)
                                {
                                    for (int x = 0; x < this.MatchColumns.Count; x++)
                                    {
                                        WebMatchColumn mc = this.MatchColumns[x];
                                        if (mc.Expression != null && mc.Expression != "")
                                        {
                                            DataColumn dcExpression = new DataColumn();
                                            dcExpression.ColumnName = mc.Expression;
                                            dcExpression.Expression = mc.Expression;
                                            if (dtSrc.Columns.Contains(dcExpression.ColumnName) == false)
                                                dtSrc.Columns.Add(dcExpression);
                                            drDestTemp[j][mc.DestColumnName] = dtSrc.Rows[i][mc.Expression].ToString();
                                        }
                                        else
                                            drDestTemp[j][mc.DestColumnName] = dtSrc.Rows[i][mc.SrcColumnName].ToString();
                                    }
                                    OnAfterMove(new EventArgs());
                                }
                                break;
                            }
                            else
                                flag = 0;
                        }
                    if (flag != strSrc.Count || strSrc.Count == 0)
                    {
                        DataRow dr = dtDest.NewRow();
                        ht.Add(dsDest.Tables[0].PrimaryKey[0].ToString(), MasterKey); 
                        foreach (WebMatchColumn mc in this.MatchColumns)
                            if (mc.Expression != null && mc.Expression != "")
                            {
                                DataColumn dcExpression = new DataColumn();
                                dcExpression.ColumnName = mc.Expression;
                                dcExpression.Expression = mc.Expression;
                                if (dtSrc.Columns.Contains(dcExpression.ColumnName) == false)
                                    dtSrc.Columns.Add(dcExpression);
                                ht.Add(mc.DestColumnName, dtSrc.Rows[i][mc.Expression]);
                            }
                            else
                            {
                                if ((mc.SrcColumnName == "" || mc.DestColumnName == "" || mc.SrcColumnName == null || mc.DestColumnName == null)
                                   && (mc.Expression == null || mc.Expression == ""))
                                {
                                    language = CliUtils.fClientLang;
                                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoClientMove", "ismatchColumn", true);
                                    this.Page.Response.Write("<script>alert(\"" + message + "\");</script>");
                                    return;
                                }
                                else
                                    ht.Add(mc.DestColumnName, dtSrc.Rows[i][mc.SrcColumnName]);
                            }
                        OnAfterMove(new EventArgs());
                        int isExsite = 0;
                        for (int j = 0; j < dtDestM.Rows.Count; j++)
                        {
                            isExsite = 0;
                            for (int x = 0; x < dtDestM.Columns.Count; x++)
                                if (ht[dtDestM.Columns[x].ToString()] != null && ht[dtDestM.Columns[x].ToString()].ToString() == dtDestM.Rows[j][dtDestM.Columns[x].ToString()].ToString())
                                    isExsite++;
                            if (isExsite == ht.Count) break;
                        }
                        if (isExsite != ht.Count)
                        {
                            wsDest.Insert(ht);
                            OnAfterInsert(new EventArgs());
                        }
                    }
                }
                wsSrc.AutoApply = tempSrc;
                wsDest.AutoApply = tempDest;
                if (progress == true)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(LongTask));
                    thread.Start();
                    this.Page.Session["State"] = 1;
                    OpenProgressBar(this.Page);
                }
                this.Page.DataBind();
            }
        }

        internal static readonly object EventOnAfterInsert = new object();
        public event EventHandler AfterInsert
        {
            add { base.Events.AddHandler(EventOnAfterInsert, value); }
            remove { base.Events.RemoveHandler(EventOnAfterInsert, value); }
        }
        protected void OnAfterInsert(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterInsert];
            if (handler != null)
                handler(this, value);
        }

        internal static readonly object EventOnAfterMove = new object();
        public event EventHandler AfterMove
        {
            add { base.Events.AddHandler(EventOnAfterMove, value); }
            remove { base.Events.RemoveHandler(EventOnAfterMove, value); }
        }
        protected void OnAfterMove(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterMove];
            if (handler != null)
                handler(this, value);
        }

        private void LongTask()
        {
            for (int i = 1; i < 11; i++)
            {
                System.Threading.Thread.Sleep(200);
                this.Page.Session["State"] = i;
            }
            this.Page.Session["State"] = 100;
        }

        public static void OpenProgressBar(System.Web.UI.Page Page)
        {
            int count = 0;
            for (int i = 0; i < Page.Request.Path.Length; i++)
                if (Page.Request.Path[i] == '/') count++;
            count -= 2;
            string progressPath = null;
            for (int i = 0; i < count; i++)
                progressPath += "../";
            progressPath += "Progress.aspx";

            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language='JavaScript' type='text/javascript'>\n");
            sbScript.Append("<!--\n");
            sbScript.Append("window.showModalDialog('" + progressPath + "','','dialogHeight: 120px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: No; status: No;scroll:No;');\n");
            sbScript.Append("// -->\n");
            sbScript.Append("</script>\n");
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "OpenProgressBar", sbScript.ToString());
        }
    }

    public class WebGetSource : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public WebGetSource()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public class WebKeyMatchColumns : InfoOwnerCollection
    {
        public WebKeyMatchColumns(object aOwner, Type aItemType)
            : base(aOwner, typeof(WebKeyMatchColumn))
        {

        }

        new public WebKeyMatchColumn this[int index]
        {
            get
            {
                return (WebKeyMatchColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is WebKeyMatchColumn)
                    {
                        //原来的Collection设置为0
                        ((WebKeyMatchColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebKeyMatchColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class WebKeyMatchColumn : InfoOwnerCollectionItem, IGetValues
    {
        private string _Name = "";
        override public string Name
        {
            get
            {
                if (DestColumnName != null)
                    return this.DestColumnName;
                else
                    return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _SrcColumnName;
        [Editor(typeof(WebGetSourceColumns), typeof(UITypeEditor))]
        public string SrcColumnName
        {
            get
            {
                return _SrcColumnName;
            }
            set
            {
                _SrcColumnName = value;
            }
        }

        private string _DestColumnName;
        [Editor(typeof(WebGetSourceColumns), typeof(UITypeEditor))]
        public string DestColumnName
        {
            get
            {
                return _DestColumnName;
            }
            set
            {
                _DestColumnName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "srccolumnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebClientMove)
                {
                    WebClientMove wcm = (WebClientMove)this.Owner;
                    if (wcm.Page != null && wcm.SrcDataSorce != null && wcm.SrcDataSorce != "")
                    {
                        foreach (System.Web.UI.Control ctrl in wcm.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wcm.SrcDataSorce)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.WebDataSetID != null && ds.WebDataSetID != "")
                                {
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
                                else if (ds.SelectCommand != null && ds.SelectCommand != "")
                                {
                                    foreach (DataColumn column in ds.CommandTable.Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                    break;
                                }
                            }
                        }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "destcolumnname", true) == 0)
            {
                if (this.Owner is WebClientMove)
                {
                    WebClientMove wcm = (WebClientMove)this.Owner;
                    if (wcm.Page != null && wcm.DestDataSorce != null && wcm.DestDataSorce != "")
                    {
                        foreach (System.Web.UI.Control ctrl in wcm.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wcm.DestDataSorce)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null && ds.WebDataSetID != null && ds.WebDataSetID != "")
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
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            return retList;
        }
    }

    public class WebMatchColumns : InfoOwnerCollection
    {
        public WebMatchColumns(object aOwner, Type aItemType)
            : base(aOwner, typeof(WebMatchColumn))
        {

        }

        new public WebMatchColumn this[int index]
        {
            get
            {
                return (WebMatchColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is WebMatchColumn)
                    {
                        //原来的Collection设置为0
                        ((WebMatchColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebMatchColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class WebMatchColumn : InfoOwnerCollectionItem, IGetValues
    {
        private string _Name = "";
        override public string Name
        {
            get
            {
                if (DestColumnName != null)
                    return this.DestColumnName;
                else
                    return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _SrcColumnName;
        [Category("Sorce")]
        [Editor(typeof(WebGetSourceColumns), typeof(UITypeEditor))]
        public string SrcColumnName
        {
            get
            {
                return _SrcColumnName;
            }
            set
            {
                _SrcColumnName = value;
            }
        }

        private string _Expression;
        [Category("Sorce")]
        public string Expression
        {
            get
            {
                return _Expression;
            }
            set
            {
                _Expression = value;
            }
        }

        private string _DestColumnName;
        [Category("Destination")]
        [Editor(typeof(WebGetSourceColumns), typeof(UITypeEditor))]
        public string DestColumnName
        {
            get
            {
                return _DestColumnName;
            }
            set
            {
                _DestColumnName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "srccolumnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebClientMove)
                {
                    WebClientMove wcm = (WebClientMove)this.Owner;
                    if (wcm.Page != null && wcm.SrcDataSorce != null && wcm.SrcDataSorce != "")
                    {
                        foreach (System.Web.UI.Control ctrl in wcm.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wcm.SrcDataSorce)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.WebDataSetID != null && ds.WebDataSetID != "")
                                {
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
                                else if (ds.SelectCommand != null && ds.SelectCommand != "")
                                {
                                    foreach (DataColumn column in ds.CommandTable.Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                    break;
                                }
                            }
                        }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "destcolumnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebClientMove)
                {
                    WebClientMove wcm = (WebClientMove)this.Owner;
                    if (wcm.Page != null && wcm.DestDataSorce != null && wcm.DestDataSorce != "")
                    {
                        foreach (System.Web.UI.Control ctrl in wcm.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wcm.DestDataSorce)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null && ds.WebDataSetID != null && ds.WebDataSetID != "")
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
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            return retList;
        }
    }

    public class WebGetSourceColumns : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public WebGetSourceColumns()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }
}
