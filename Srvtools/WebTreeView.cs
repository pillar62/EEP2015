using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Reflection;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebTreeView), "Resources.InfoTreeView.png")]
    public class WebTreeView : TreeView,IGetValues
    {
        
        
        public WebTreeView()
            : base()
        {
            editcolumn = new WebTreeViewItemCollection(this, typeof(WebTreeViewItem));
            _parentcaption = "";
            _keycaption = "";
            _textcaption = "";
        }

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(wtvDataSourceEditor), typeof(UITypeEditor))]
        public string WebDataSourceID
        {
            get
            {
                object obj = this.ViewState["WebDataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["WebDataSourceID"] = value;
            }
        }

        private string _parentfield;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of parent node")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ParentField
        {
            get
            {
                return _parentfield;
            }
            set
            {
                _parentfield = value;
            }    
        }

        private string _keyfield;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of node's key")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string KeyField
        {
            get
            {
                return _keyfield;
            }
            set
            {
                _keyfield = value;
            }
        }

        private string _textfield;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of node's text")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string TextField
        {
            get
            {
                return _textfield;
            }
            set
            {
                _textfield = value;
            }  
        }

        private string _parentcaption;
        [Category("Infolight"),
        Description("Specifies the caption of parent")]
        [NotifyParentProperty(true)]
        public string ParentCaption
        {
            get
            {
                return _parentcaption;
            }
            set
            {
                _parentcaption = value;
            }
        }

        private string _keycaption;
        [Category("Infolight"),
        Description("Specifies the caption of node's key")]
        [NotifyParentProperty(true)]
        public string KeyCaption
        {
            get
            {
                return _keycaption;
            }
            set
            {
                _keycaption = value;
            }
        }

        private string _textcaption;
        [Category("Infolight"),
        Description("Specifies the caption of node's text")]
        [NotifyParentProperty(true)]
        public string TextCaption
        {
            get
            {
                return _textcaption;
            }
            set
            {
                _textcaption = value;
            }
        }

        private WebTreeViewItemCollection editcolumn;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of node's text")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public WebTreeViewItemCollection EditColumn
        {
            get
            {
                return editcolumn;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["TreeViewID"] == this.ID)
            {
                if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["Nodekey"] != null
                  && this.Page.Request.QueryString["Nodeparent"] != null && this.Page.Request.QueryString["Nodetext"] != null
                  && this.Page.Request.QueryString["Nodemode"] != null)
                {
                    WebDataSource ds = (WebDataSource)GetObjByID(this.WebDataSourceID);
                    string edittext = this.Page.Request.QueryString["Edittext"];
                    string[] arredittext = edittext.Split(';');
                    int textcount = this.EditColumn.Count;

                    DataTable dt = ds.InnerDataSet.Tables[ds.DataMember];
                    if (string.Compare(this.Page.Request.QueryString["Nodemode"], "add", true) == 0)//IgnoreCase
                    {
                        DataRow rowadd = dt.NewRow();
                        rowadd[this.KeyField] = this.Page.Request.QueryString["Nodekey"];
                        rowadd[this.ParentField] = this.Page.Request.QueryString["Nodeparent"];
                        rowadd[this.TextField] = this.Page.Request.QueryString["Nodetext"];
                        for (int i = 0; i < textcount; i++)
                        {
                            rowadd[((WebTreeViewItem)this.EditColumn[i]).Column] = arredittext[i];
                        }
                        dt.Rows.Add(rowadd);
                        ds.ApplyUpdates();

                    }
                    else if (string.Compare(this.Page.Request.QueryString["Nodemode"], "update", true) == 0)//IgnoreCase
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][this.KeyField].ToString() == this.Page.Request.QueryString["Nodekey"])
                            {
                                dt.Rows[i][this.ParentField] = this.Page.Request.QueryString["Nodeparent"];
                                dt.Rows[i][this.TextField] = this.Page.Request.QueryString["Nodetext"];
                                for (int j = 0; j < textcount; j++)
                                {
                                    dt.Rows[i][((WebTreeViewItem)this.EditColumn[j]).Column] = arredittext[j];
                                }
                            }
                        }
                        ds.ApplyUpdates();
                    }

                    Page.Response.Redirect(Page.Request.Path);

                }
            }
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
        public object GetObjByID(string ObjID)
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

        public void Initial()
        {
            ArrayList lstkey = new ArrayList();
            ArrayList lstparent = new ArrayList();
            ArrayList lsttext = new ArrayList();

            ArrayList lstmainkey = new ArrayList();
            ArrayList lstmaintext = new ArrayList();
            ArrayList lstchildkey = new ArrayList();
            ArrayList lstchildparent = new ArrayList();
            ArrayList lstchildtext = new ArrayList();

            WebDataSource wds = (WebDataSource)GetObjByID(this.WebDataSourceID);

            this.Nodes.Clear();
            if (wds != null)
            {

                DataTable dt = wds.InnerDataSet.Tables[wds.DataMember];
                int nodecount = dt.Rows.Count;
                for (int i = 0; i < nodecount; i++)
                {
                    lstkey.Add(dt.Rows[i][this.KeyField]);
                    lstparent.Add(dt.Rows[i][this.ParentField]);
                    lsttext.Add(dt.Rows[i][this.TextField]);
                }

                for (int i = 0; i < nodecount; i++)
                {
                    if (lstkey[i].ToString() != lstparent[i].ToString())
                    {
                        if (lstparent[i].ToString() == string.Empty)
                        {
                            lstmainkey.Add(lstkey[i]);
                            lstmaintext.Add(lsttext[i]);
                        }
                        else
                        {
                            lstchildkey.Add(lstkey[i]);
                            lstchildparent.Add(lstparent[i]);
                            lstchildtext.Add(lsttext[i]);
                        }
                    }
                }

                int mainnodecount = lstmainkey.Count;
                TreeNode[] nodemain = new TreeNode[mainnodecount];

                for (int i = 0; i < mainnodecount; i++)
                {
                    nodemain[i] = new TreeNode();
                    nodemain[i].Text = lstmaintext[i].ToString();
                    nodemain[i].Value = lstmainkey[i].ToString();
                    this.Nodes.Add(nodemain[i]);
                }

                int childnodecount = lstchildkey.Count;
                TreeNode[] nodechild = new TreeNode[childnodecount];
                for (int i = 0; i < childnodecount; i++)
                {
                    nodechild[i] = new TreeNode();
                    nodechild[i].Text = lstchildtext[i].ToString();
                    nodechild[i].Value = lstchildkey[i].ToString();
                }

                for (int i = 0; i < childnodecount; i++)
                {
                    for (int j = 0; j < mainnodecount; j++)
                    {
                        if (lstchildparent[i].ToString() == lstmainkey[j].ToString())
                        {
                            nodemain[j].ChildNodes.Add(nodechild[i]);
                        }
                    }
                    for (int k = 0; k < childnodecount; k++)
                    {
                        if (lstchildparent[i].ToString() == lstchildkey[k].ToString())
                        {
                            nodechild[k].ChildNodes.Add(nodechild[i]);
                        }
                    }
                }
            }
            this.ExpandAll();
        }

        public void InsertItem()
        {
            TreeNode nodeSelect = this.SelectedNode;
            string strKey = "";
            string strParent = "";
            string strText = "";
            string strKeyField = this.KeyField;
            string strParentField = this.ParentField;
            string strTextField = this.TextField;
            string strQueryString = "";
            string strEditCaption = "";
            string strEditColumn = "";
            string strEditColumnType = "";
            string strDefaultValue = "";
            string strRefVal = "";

            if (this.EditColumn.Count > 0)
            {
                foreach (WebTreeViewItem wtvt in this.EditColumn)
                {
                    strEditCaption += wtvt.Caption + ";";
                    strEditColumn += wtvt.Column + ";";
                    strEditColumnType += wtvt.ColumnType + ";";
                    strDefaultValue += GetDefaultValue(wtvt.DefaultValue).ToString() + ";";
                }
                strEditCaption = strEditCaption.Substring(0, strEditCaption.LastIndexOf(';'));
                strEditCaption = HttpUtility.UrlEncode(strEditCaption);
                strEditColumn = strEditColumn.Substring(0, strEditColumn.LastIndexOf(';'));
                strEditColumn = HttpUtility.UrlEncode(strEditColumn);
                strEditColumnType = strEditColumnType.Substring(0, strEditColumnType.LastIndexOf(';'));
               
                strDefaultValue = strDefaultValue.Substring(0, strDefaultValue.LastIndexOf(';'));
                strDefaultValue = HttpUtility.UrlEncode(strDefaultValue);
                #region refval
                strRefVal += "&Refvalvf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataValueField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaltf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataTextField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvalselcmd=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectCommand + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));


                strRefVal += "&Refvalselalias=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectAlias + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldstid=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.WebDataSetID + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldm=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.DataMember + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                #region Refvalcolumnmatch
                strRefVal += "&Refvalcolumnmatch=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.ColumnMatch.Count > 0)
                        {
                            foreach (WebColumnMatch wcm in wrv.ColumnMatch)
                            {
                                strRefVal += wcm.DestControlID + "," + wcm.SrcField + "," + wcm.SrcGetValue + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalcolumns
                strRefVal += "&Refvalcolumns=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.Columns.Count > 0)
                        {
                            foreach (WebRefColumn wrc in wrv.Columns)
                            {
                                strRefVal += wrc.ColumnName + "," + HttpUtility.UrlEncode(wrc.HeadText) + "," + wrc.Width.ToString() + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalwhereitem
                strRefVal += "&Refvalwhereitem=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.WhereItem.Count > 0)
                        {
                            foreach (WebWhereItem wwi in wrv.WhereItem)
                            {
                                strRefVal += wwi.Condition + "," + wwi.FieldName + "," + HttpUtility.UrlEncode(wwi.Value) + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #endregion
            }
            int pageheight = 170 + 22 * this.EditColumn.Count; 

            WebDataSource wds = (WebDataSource)GetObjByID(this.WebDataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;
            string strPagePath = this.Page.Request.Path;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            if (this.Page.Request.QueryString != null)
            {
                //strQueryString = this.Page.Request.QueryString.ToString();
            }
            strQueryString = strQueryString.Replace('&', ';');
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');

           

            if(nodeSelect != null)
            {
                strKey = nodeSelect.Value;
                if (nodeSelect.Parent != null)
                {
                  strParent = nodeSelect.Parent.Value;
                }
                strText = nodeSelect.Text;
                strText = HttpUtility.UrlEncode(strText);
            }
            
          
            this.Page.Response.Write("<script type='text/javascript'>window.open('../InnerPages/frmTreeView.aspx?Nodekey=" + strKey
                                    + "&Nodeparent=" + strParent + "&Nodetext=" + strText +"&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&TreeViewID=" + this.ID
                                    + "&Keyfield=" + strKeyField+ "&Parentfield=" + strParentField + "&Textfield=" + strTextField
                                    + "&Keycaption=" + HttpUtility.UrlEncode(KeyCaption) + "&Parentcaption=" + HttpUtility.UrlEncode(ParentCaption) + "&Textcaption=" + HttpUtility.UrlEncode(TextCaption)
                                    + "&Pagepath=" + strPagePath + "&Psypagepath=" + strPsyPagePath + "&Querystring=" + strQueryString
                                    + "&Editcaption=" + strEditCaption + "&Editcolumn=" + strEditColumn + "&Editcolumntype=" + strEditColumnType + "&Editdefaultvalue=" + strDefaultValue
                                    + strRefVal + "&Mode=Add', 'AddTreeNode','width=200,height=" + pageheight.ToString() + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');</script>");
        }

        public string GetInsertItemURL()
        {
            TreeNode nodeSelect = this.SelectedNode;
            string strKey = "";
            string strParent = "";
            string strText = "";
            string strKeyField = this.KeyField;
            string strParentField = this.ParentField;
            string strTextField = this.TextField;
            string strQueryString = "";
            string strEditCaption = "";
            string strEditColumn = "";
            string strEditColumnType = "";
            string strDefaultValue = "";
            string strRefVal = "";

            if (this.EditColumn.Count > 0)
            {
                foreach (WebTreeViewItem wtvt in this.EditColumn)
                {
                    strEditCaption += wtvt.Caption + ";";
                    strEditColumn += wtvt.Column + ";";
                    strEditColumnType += wtvt.ColumnType + ";";
                    strDefaultValue += GetDefaultValue(wtvt.DefaultValue).ToString() + ";";
                }
                strEditCaption = strEditCaption.Substring(0, strEditCaption.LastIndexOf(';'));
                strEditCaption = HttpUtility.UrlEncode(strEditCaption);
                strEditColumn = strEditColumn.Substring(0, strEditColumn.LastIndexOf(';'));
                strEditColumn = HttpUtility.UrlEncode(strEditColumn);
                strEditColumnType = strEditColumnType.Substring(0, strEditColumnType.LastIndexOf(';'));

                strDefaultValue = strDefaultValue.Substring(0, strDefaultValue.LastIndexOf(';'));
                strDefaultValue = HttpUtility.UrlEncode(strDefaultValue);
                #region refval
                strRefVal += "&Refvalvf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataValueField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaltf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataTextField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvalselcmd=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectCommand + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvalselalias=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectAlias + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldstid=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.WebDataSetID + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldm=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.DataMember + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                #region Refvalcolumnmatch
                strRefVal += "&Refvalcolumnmatch=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.ColumnMatch.Count > 0)
                        {
                            foreach (WebColumnMatch wcm in wrv.ColumnMatch)
                            {
                                strRefVal += wcm.DestControlID + "," + wcm.SrcField + "," + wcm.SrcGetValue + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalcolumns
                strRefVal += "&Refvalcolumns=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.Columns.Count > 0)
                        {
                            foreach (WebRefColumn wrc in wrv.Columns)
                            {
                                strRefVal += wrc.ColumnName + "," + HttpUtility.UrlEncode(wrc.HeadText) + "," + wrc.Width.ToString() + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalwhereitem
                strRefVal += "&Refvalwhereitem=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.WhereItem.Count > 0)
                        {
                            foreach (WebWhereItem wwi in wrv.WhereItem)
                            {
                                strRefVal += wwi.Condition + "," + wwi.FieldName + "," + HttpUtility.UrlEncode(wwi.Value) + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #endregion
            }
            int pageheight = 170 + 22 * this.EditColumn.Count;

            WebDataSource wds = (WebDataSource)GetObjByID(this.WebDataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;
            string strPagePath = this.Page.Request.Path;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            if (this.Page.Request.QueryString != null)
            {
                //strQueryString = this.Page.Request.QueryString.ToString();
            }
            strQueryString = strQueryString.Replace('&', ';');
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');



            if (nodeSelect != null)
            {
                strKey = nodeSelect.Value;
                if (nodeSelect.Parent != null)
                {
                    strParent = nodeSelect.Parent.Value;
                }
                strText = nodeSelect.Text;
                strText = HttpUtility.UrlEncode(strText);
            }


            string url =    "../InnerPages/frmTreeView.aspx?Nodekey=" + strKey
                            + "&Nodeparent=" + strParent + "&Nodetext=" + strText + "&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&TreeViewID=" + this.ID
                            + "&Keyfield=" + strKeyField + "&Parentfield=" + strParentField + "&Textfield=" + strTextField
                            + "&Keycaption=" + HttpUtility.UrlEncode(KeyCaption) + "&Parentcaption=" + HttpUtility.UrlEncode(ParentCaption) + "&Textcaption=" + HttpUtility.UrlEncode(TextCaption)
                            + "&Pagepath=" + strPagePath + "&Psypagepath=" + strPsyPagePath + "&Querystring=" + strQueryString
                            + "&Editcaption=" + strEditCaption + "&Editcolumn=" + strEditColumn + "&Editcolumntype=" + strEditColumnType + "&Editdefaultvalue=" + strDefaultValue
                            + strRefVal + "&Mode=Add', 'AddTreeNode','width=200,height=" + pageheight.ToString();
            return url;
        }

        public void DeleteItem()
        {
            TreeNode nodedelete = this.SelectedNode;
            if (nodedelete != null)
            {
                if (nodedelete.ChildNodes.Count > 0)
                {
                    this.Page.Response.Write("<script>window.alert('delete the childnode first!')</script>");
                }
                else
                {
                    WebDataSource ds = (WebDataSource)GetObjByID(this.WebDataSourceID);

                    DataSet dataset = ds.InnerDataSet;
                    WebDataSet wds = new WebDataSet();
                    wds.RemoteName = ds.RemoteName;
                    wds.RealDataSet = dataset;

                    DataTable dt = wds.RealDataSet.Tables[ds.DataMember];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][this.KeyField].ToString() == nodedelete.Value)
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    wds.ApplyUpdates();
                    this.Page.Response.Redirect(this.Page.Request.Path);
                }
            }
            
        }

        public void UpdateItem()
        {
            TreeNode nodeUpdate = this.SelectedNode;
            string strKey = "";
            string strParent = "";
            string strText = "";
            string strKeyField = this.KeyField;
            string strParentField = this.ParentField;
            string strTextField = this.TextField;
            string strQueryString = "";
            string strEditCaption = "";
            string strEditColumn = "";
            string strEditColumnType = "";
            string strDefaultValue = "";
            string strRefVal = "";
            if (this.EditColumn.Count > 0)
            {
                foreach (WebTreeViewItem wtvt in this.EditColumn)
                {
                    strEditCaption += wtvt.Caption + ";";
                    strEditColumn += wtvt.Column + ";";
                    strEditColumnType += wtvt.ColumnType + ";";
                    strDefaultValue += GetDefaultValue(wtvt.DefaultValue).ToString() + ";";
                }
                strEditCaption = strEditCaption.Substring(0, strEditCaption.LastIndexOf(';'));
                strEditCaption = HttpUtility.UrlEncode(strEditCaption);
                strEditColumn = strEditColumn.Substring(0, strEditColumn.LastIndexOf(';'));
                strEditColumn = HttpUtility.UrlEncode(strEditColumn);
                strEditColumnType = strEditColumnType.Substring(0, strEditColumnType.LastIndexOf(';'));
             
                strDefaultValue = strDefaultValue.Substring(0, strDefaultValue.LastIndexOf(';'));
                strDefaultValue = HttpUtility.UrlEncode(strDefaultValue);
                #region refval
                strRefVal += "&Refvalvf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataValueField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaltf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataTextField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvalselcmd=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectCommand + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));


                strRefVal += "&Refvalselalias=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectAlias + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldstid=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.WebDataSetID + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldm=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.DataMember + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                #region Refvalcolumnmatch
                strRefVal += "&Refvalcolumnmatch=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.ColumnMatch.Count > 0)
                        {
                            foreach (WebColumnMatch wcm in wrv.ColumnMatch)
                            {
                                strRefVal += wcm.DestControlID + "," + wcm.SrcField + "," + wcm.SrcGetValue + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalcolumns
                strRefVal += "&Refvalcolumns=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.Columns.Count > 0)
                        {
                            foreach (WebRefColumn wrc in wrv.Columns)
                            {
                                strRefVal += wrc.ColumnName + "," + HttpUtility.UrlEncode(wrc.HeadText) + "," + wrc.Width.ToString() + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalwhereitem
                strRefVal += "&Refvalwhereitem=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.WhereItem.Count > 0)
                        {
                            foreach (WebWhereItem wwi in wrv.WhereItem)
                            {
                                strRefVal += wwi.Condition + "," + wwi.FieldName + "," + HttpUtility.UrlEncode(wwi.Value) + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #endregion
            }


            int pageheight = 170 + 22 * this.EditColumn.Count; 
            WebDataSource wds = (WebDataSource)GetObjByID(this.WebDataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;

            string strPagePath = this.Page.Request.Path;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');
            if (this.Page.Request.QueryString != null)
            {
    //             this.Page.Request.QueryString.ToString()
            }
            strQueryString = strQueryString.Replace('&', ';');
            
            
          

            if (nodeUpdate != null)
            {
                strKey = nodeUpdate.Value;
                if (nodeUpdate.Parent != null)
                {
                    strParent = nodeUpdate.Parent.Value;
                }
                strText = nodeUpdate.Text;
                strText = HttpUtility.UrlEncode(strText);

                this.Page.Response.Write("<script type='text/javascript'>window.open('../InnerPages/frmTreeView.aspx?Nodekey=" + strKey
                                       + "&Nodeparent=" + strParent + "&Nodetext=" + strText + "&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&TreeViewID=" + this.ID
                                       + "&Keyfield=" + strKeyField + "&Parentfield=" + strParentField + "&Textfield=" + strTextField
                                       + "&Keycaption=" + HttpUtility.UrlEncode(KeyCaption) + "&Parentcaption=" + HttpUtility.UrlEncode(ParentCaption) + "&Textcaption=" + HttpUtility.UrlEncode(TextCaption)
                                       + "&Pagepath=" + strPagePath + "&Psypagepath=" + strPsyPagePath + "&Querystring=" + strQueryString
                                       + "&Editcaption=" + strEditCaption + "&Editcolumn=" + strEditColumn + "&Editcolumntype=" + strEditColumnType + "&Editdefaultvalue=" + strDefaultValue
                                       + strRefVal + "&Mode=Update', 'UpdateTreeNode','width=200,height=" + pageheight.ToString() + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');</script>");
            }
           
        }

        public string GetUpdateItemURL()
        {

            TreeNode nodeUpdate = this.SelectedNode;
            string strKey = "";
            string strParent = "";
            string strText = "";
            string strKeyField = this.KeyField;
            string strParentField = this.ParentField;
            string strTextField = this.TextField;
            string strQueryString = "";
            string strEditCaption = "";
            string strEditColumn = "";
            string strEditColumnType = "";
            string strDefaultValue = "";
            string strRefVal = "";
            if (this.EditColumn.Count > 0)
            {
                foreach (WebTreeViewItem wtvt in this.EditColumn)
                {
                    strEditCaption += wtvt.Caption + ";";
                    strEditColumn += wtvt.Column + ";";
                    strEditColumnType += wtvt.ColumnType + ";";
                    strDefaultValue += GetDefaultValue(wtvt.DefaultValue).ToString() + ";";
                }
                strEditCaption = strEditCaption.Substring(0, strEditCaption.LastIndexOf(';'));
                strEditCaption = HttpUtility.UrlEncode(strEditCaption);
                strEditColumn = strEditColumn.Substring(0, strEditColumn.LastIndexOf(';'));
                strEditColumn = HttpUtility.UrlEncode(strEditColumn);
                strEditColumnType = strEditColumnType.Substring(0, strEditColumnType.LastIndexOf(';'));

                strDefaultValue = strDefaultValue.Substring(0, strDefaultValue.LastIndexOf(';'));
                strDefaultValue = HttpUtility.UrlEncode(strDefaultValue);
                #region refval
                strRefVal += "&Refvalvf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataValueField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaltf=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        strRefVal += wrv.DataTextField + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvalselcmd=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectCommand + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvalselalias=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.SelectAlias + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldstid=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.WebDataSetID + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                strRefVal += "&Refvaldm=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);
                        WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                        strRefVal += refvalwds.DataMember + ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));

                #region Refvalcolumnmatch
                strRefVal += "&Refvalcolumnmatch=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.ColumnMatch.Count > 0)
                        {
                            foreach (WebColumnMatch wcm in wrv.ColumnMatch)
                            {
                                strRefVal += wcm.DestControlID + "," + wcm.SrcField + "," + wcm.SrcGetValue + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalcolumns
                strRefVal += "&Refvalcolumns=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.Columns.Count > 0)
                        {
                            foreach (WebRefColumn wrc in wrv.Columns)
                            {
                                strRefVal += wrc.ColumnName + "," + HttpUtility.UrlEncode(wrc.HeadText) + "," + wrc.Width.ToString() + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #region Refvalwhereitem
                strRefVal += "&Refvalwhereitem=";
                foreach (WebTreeViewItem wtvi in this.EditColumn)
                {
                    if (wtvi.WebRefVal == string.Empty || wtvi.WebRefVal == null)
                    {
                        strRefVal += ";";
                    }
                    else
                    {
                        WebRefVal wrv = (WebRefVal)GetObjByID(wtvi.WebRefVal);

                        if (wrv.WhereItem.Count > 0)
                        {
                            foreach (WebWhereItem wwi in wrv.WhereItem)
                            {
                                strRefVal += wwi.Condition + "," + wwi.FieldName + "," + HttpUtility.UrlEncode(wwi.Value) + ":";

                            }
                            strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(':'));
                        }
                        strRefVal += ";";
                    }
                }
                strRefVal = strRefVal.Substring(0, strRefVal.LastIndexOf(';'));
                #endregion

                #endregion
            }


            int pageheight = 170 + 22 * this.EditColumn.Count;
            WebDataSource wds = (WebDataSource)GetObjByID(this.WebDataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;

            string strPagePath = this.Page.Request.Path;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');
            if (this.Page.Request.QueryString != null)
            {
                //             this.Page.Request.QueryString.ToString()
            }
            strQueryString = strQueryString.Replace('&', ';');




            if (nodeUpdate != null)
            {
                strKey = nodeUpdate.Value;
                if (nodeUpdate.Parent != null)
                {
                    strParent = nodeUpdate.Parent.Value;
                }
                strText = nodeUpdate.Text;
                strText = HttpUtility.UrlEncode(strText);

                string url =   "../InnerPages/frmTreeView.aspx?Nodekey=" + strKey
                               + "&Nodeparent=" + strParent + "&Nodetext=" + strText + "&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&TreeViewID=" + this.ID
                               + "&Keyfield=" + strKeyField + "&Parentfield=" + strParentField + "&Textfield=" + strTextField
                               + "&Keycaption=" + HttpUtility.UrlEncode(KeyCaption) + "&Parentcaption=" + HttpUtility.UrlEncode(ParentCaption) + "&Textcaption=" + HttpUtility.UrlEncode(TextCaption)
                               + "&Pagepath=" + strPagePath + "&Psypagepath=" + strPsyPagePath + "&Querystring=" + strQueryString
                               + "&Editcaption=" + strEditCaption + "&Editcolumn=" + strEditColumn + "&Editcolumntype=" + strEditColumnType + "&Editdefaultvalue=" + strDefaultValue
                               + strRefVal + "&Mode=Update', 'UpdateTreeNode','width=200,height=" + pageheight.ToString();
                return url;
            }
            else
            {
                return "";
            }
        
        
        }

        private object GetDefaultValue(string Default)
        {
            return CliUtils.GetValue(Default, this.Page);
        }


        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "parentfield", true) == 0 || string.Compare(sKind, "keyfield", true) == 0 
                || string.Compare(sKind, "textfield", true) == 0)//IgnoreCase
            {
                if (this is WebTreeView)
                {
                    WebTreeView wtv = (WebTreeView)this;
                    if (wtv.Page != null && wtv.WebDataSourceID != null && wtv.WebDataSourceID != "")
                    {
                        foreach (Control ctrl in wtv.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wtv.WebDataSourceID)
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

        #endregion
    }


    #region wtvDataSourceEditor
    public class wtvDataSourceEditor : UITypeEditor
    {
        public wtvDataSourceEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is WebTreeView)
            {
                ControlCollection ctrlList = ((WebTreeView)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }
    #endregion

    public class WebTreeViewItemCollection : InfoOwnerCollection
    {
        public WebTreeViewItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebTreeViewItem))
        {

        }
        public DataSet DsForDD = new DataSet();
        public new WebTreeViewItem this[int index]
        {
            get
            {
                return (WebTreeViewItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebTreeViewItem)
                    {
                        //原来的Collection设置为0
                        ((WebTreeViewItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebTreeViewItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebTreeViewItem : InfoOwnerCollectionItem, IGetValues
    {
        public WebTreeViewItem()
        {
            columntype = "TextBoxColumn";
            caption = "";
            _defaultvalue = "";
        
        }


        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string caption;
        [Category("InfoLight")]
        [NotifyParentProperty(true)]
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                if (value != null && value != "")
                {
                    caption = value;
                    _name = caption;
                }
                else
                {
                    if (column != null && column != "")
                    {
                        caption = column;
                        _name = column;
                    }
                    else
                    {
                        _name = "treeviewitem";
                    }
                }
            }
        }

        private string column;
        [Category("InfoLight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
                if (this.Owner != null && Caption == "")
                {
                    if (((WebTreeView)this.Owner).Site == null)
                    {
                       this.Caption = column;
                    }
                    else if (((WebTreeView)this.Owner).Site.DesignMode)
                    {
                       this.Caption = column;
                    }
                }
            }
        }

        private string columntype;
        [Category("InfoLight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnType
        {
            get
            {
                return columntype;
            }
            set
            {
                columntype = value;
            }
        }

        private string _webrefval;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        public string WebRefVal
        {
            get
            {
                return _webrefval;
            }
            set
            {
                if (columntype != "ComboBoxColumn" && columntype != "RefValColumn" && value != null)
                {
                    //MessageBox.Show("WebRefval can be set only when\ncolumntype is combobox & refval.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _webrefval = value;
                }
            }
        }

        private string _defaultvalue;
        public string DefaultValue
        {
            get
            {
                return _defaultvalue;
            }
            set
            {
                _defaultvalue = value;
            }

        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebTreeView)
            {
                if (string.Compare(sKind, "column", true) == 0)//IgnoreCase
                {
                      WebTreeView wtv = (WebTreeView)this.Owner;
                      if (wtv.Page != null && wtv.WebDataSourceID != null && wtv.WebDataSourceID != "")
                      {
                          foreach (Control ctrl in wtv.Page.Controls)
                          {
                              if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wtv.WebDataSourceID)
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
                else if (string.Compare(sKind, "columntype", true) == 0)//IgnoreCase
                {
                    values.Add("TextBoxColumn");
                    values.Add("ComboBoxColumn");
                    values.Add("RefValColumn");
                    values.Add("CalendarColumn");
                    values.Add("RadioButtonColumn");
                }
                else if (string.Compare(sKind, "webrefval", true) == 0)//IgnoreCase
                {
                    if (this.Owner is WebTreeView)
                    {
                        WebTreeView wtv = (WebTreeView)this.Owner;
                        foreach (Control ct in wtv.Page.Controls)
                        {
                            if (ct is WebRefVal)
                            {
                                values.Add(ct.ID);
                            }

                        }
                        if (wtv.Page.Form != null)
                        {
                            foreach (Control ct in wtv.Page.Form.Controls)
                            {
                                if (ct is WebRefVal)
                                {
                                    values.Add(ct.ID);
                                }
                            }
                        }
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

            return retList;
        }

        #endregion
    }
}
