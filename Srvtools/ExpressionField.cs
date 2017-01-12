using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Data;

namespace Srvtools
{
    public class ExpressionField : DataControlField
    {
        private DataTable tab = null;
        private WebGridView gridView = null;
        private WebDetailsView detView = null;

        public ExpressionField()
        { 
        }

        protected override DataControlField CreateField()
        {
            return new ExpressionField();
        }

        protected override void CopyProperties(DataControlField newField)
        {
            ((ExpressionField)newField).Expression = this.Expression;
            base.CopyProperties(newField);
        }

        public string Expression
        {
            get
            {
                object o = this.ViewState["Expression"];
                if (o != null)
                {
                    return (string)o;
                }
                return "";
            }
            set
            {
                ViewState["Expression"] = value;
                OnFieldChanged();
            }
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            if (cellType == DataControlCellType.DataCell)
            {
                Label lbl = new Label();
                lbl.ID = "ExpressionLabel" + this.Expression + rowIndex.ToString();
                if (DesignMode)
                {
                    lbl.Text = "[CalField]";
                }
                else
                {
                    if (tab != null && tab.Columns.Contains(this.Expression)
                        && (gridView != null || detView != null))
                    {
                        lbl.Text = this.Expression;
                    }
                }
                cell.Controls.Add(lbl);
            }
        }

        /*public override bool Initialize(bool sortingEnabled, Control control)
        {
            if (!DesignMode)
            {
                object obj = null;
                string dm = "";
                if (control is WebGridView)
                {
                    gridView = (WebGridView)control;
                    obj = gridView.GetObjByID(gridView.DataSourceID);
                    if (obj != null && obj is WebDataSource)
                    {
                        WebDataSource wds = (WebDataSource)obj;
                        if (wds.MasterDataSource != null && wds.MasterDataSource != "")
                        {
                            object objMaster = gridView.GetObjByID(wds.MasterDataSource);
                            obj = objMaster;
                        }
                        dm = wds.DataMember;
                    }
                }
                else if (control is WebDetailsView)
                {
                    detView = (WebDetailsView)control;
                    obj = detView.GetObjByID(detView.DataSourceID);
                    if (obj != null && obj is WebDataSource)
                    {
                        WebDataSource wds = (WebDataSource)obj;
                        dm = wds.DataMember;
                    }
                }

                if (obj != null && obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    tab = wds.InnerDataSet.Tables[dm];
                    if (!tab.Columns.Contains(this.Expression))
                    {
                        DataColumn col = new DataColumn(this.Expression, typeof(decimal), this.Expression);
                        tab.Columns.Add(col);
                    }
                }
            }
            return base.Initialize(sortingEnabled, control);
        }*/
        public override bool Initialize(bool sortingEnabled, Control control)
        {
            if (!DesignMode)
            {
                WebDataSource wds = null;
                string dm = "";
                if (control is WebGridView)
                {
                    gridView = (WebGridView)control;
                    object obj = gridView.GetObjByID(gridView.DataSourceID);
                    if (obj != null && obj is WebDataSource)
                    {
                        WebDataSource wdsDetail = (WebDataSource)obj;
                        if (wdsDetail.MasterDataSource != null && wdsDetail.MasterDataSource != "")
                        {
                            object objMaster = gridView.GetObjByID(wdsDetail.MasterDataSource);
                            if (objMaster != null && objMaster is WebDataSource)
                            {
                                wds = (WebDataSource)objMaster;
                            }
                        }
                        else
                        {
                            wds = wdsDetail;
                        }
                        dm = wdsDetail.DataMember;
                    }
                }
                else if (control is WebDetailsView)
                {
                    detView = (WebDetailsView)control;
                    object obj = detView.GetObjByID(detView.DataSourceID);
                    if (obj != null && obj is WebDataSource)
                    {
                        wds = (WebDataSource)obj;
                        dm = wds.DataMember;
                    }
                }

                if (wds != null && wds.InnerDataSet != null)
                {
                    tab = wds.InnerDataSet.Tables[dm];
                    if (!tab.Columns.Contains(this.Expression))
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = this.Expression;
                        col.Expression = this.Expression;
                        //DataColumn col = new DataColumn(this.Expression, typeof(decimal), this.Expression);
                        tab.Columns.Add(col);
                    }
                }
            }
            return base.Initialize(sortingEnabled, control);
        }
    }
}
