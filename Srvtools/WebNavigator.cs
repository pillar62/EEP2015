using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Web.UI.Design.WebControls;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Resources;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace Srvtools
{
    public class WebInfoBaseControl : WebControl, IBaseWebControl
    {
        internal Control FindChildControl(string strid, Control ct)
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
                        Control ctrtn = FindChildControl(strid, ctchild);
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
        internal Control FindChildControl(string strid, Control ct, FindControlType type, Type ReturnControlType)
        {
            string fieldName = "ID";
            if (type == FindControlType.DataSourceID)
            {
                fieldName = "DataSourceID";
            }
            else if (type == FindControlType.BindingObject)
            {
                fieldName = "BindingObject";
            }
            else if (type == FindControlType.MasterDataSource)
            {
                fieldName = "MasterDataSource";
            }

            Type ctType = ct.GetType();
            PropertyInfo pi = ctType.GetProperty(fieldName);
            if (pi != null && pi.GetValue(ct, null) != null && pi.GetValue(ct, null).ToString() == strid && ReturnControlType.IsInstanceOfType(ct))
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = FindChildControl(strid, ctchild, type, ReturnControlType);
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

        internal Control ExtendedFindChildControl(string strid, FindControlType type, Type ReturnControlType)
        {
            Control objContentPlaceHolder = this.Page.Form.FindControl("ContentPlaceHolder1");
            if (objContentPlaceHolder != null)
            {
                return this.FindChildControl(strid, objContentPlaceHolder, type, ReturnControlType);
            }
            else
            {
                return this.FindChildControl(strid, this.Page.Form, type, ReturnControlType);
            }
        }

        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return FindChildControl(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return FindChildControl(ObjID, this.Page.Form);
                else
                    return FindChildControl(ObjID, this.Page);
            }
        }
    }

    [ToolboxData("<{0}:WebNavigator runat=server></{0}:WebNavigator>")]
    [DefaultEvent("Command")]
    [ToolboxBitmap(typeof(WebNavigator), "Resources.WebNavigator.ico")]
    public class WebNavigator : WebInfoBaseControl, IPostBackEventHandler
    {
        public WebNavigator()
        {
            _NavControls = new ControlsCollection(this, typeof(ControlItem));
            _QueryFields = new WebQueryFiledsCollection(this, typeof(WebQueryField));
            _navStates = new WebNavigatorStateCollection(this, typeof(WebNavigatorStateItem));
            _AddDefaultControls = true;
            _ControlType = CtrlType.Image;
            _QueryMode = QueryModeType.Normal;
            _GetServerText = true;
            _QueryKeepCondition = false;
            //this.Command += new CommandEventHandler(WebNavigator_Command);
        }

        private SYS_LANGUAGE language;

        private void WebNavigatorCommand(CommandEventArgs e)
        {
            object obj = GetObjByID(this.BindingObject);
            object vobj = GetObjByID(this.ViewBindingObject);
            bool ViewExist = false;
            WebGridView viewGridView = new WebGridView();
            WebGridView gridView = new WebGridView();
            WebDetailsView detailView = new WebDetailsView();
            WebFormView formView = new WebFormView();
            WebDataSource ds = new WebDataSource();
            language = CliSysMegLag.GetClientLanguage();
            if (this.ShowDataStyle == NavigatorStyle.GridStyle && obj is WebGridView)
            {
                gridView = (WebGridView)obj;
                if (gridView.AutoPostBackMultiCheckBoxes)
                {
                    gridView.PostBackMultiCheckBoxes();
                }
                if (gridView.AutoPostBackWebGridTextBoxes)
                {
                    gridView.PostBackMultiTextBoxes();
                }
                ds = (WebDataSource)GetObjByID(gridView.DataSourceID);
                WebValidate validate = (WebValidate)gridView.ExtendedFindChildControl(gridView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                WebNavigator bindingNav = gridView.GetBindingNavigator();
                #region GridView Default Operation
                switch (e.CommandName)
                {
                    case "cmdFirst":
                        if (this.GridViewMoveMode == MoveMode.PageMode)
                        {
                            if (gridView.AllowPaging)
                                gridView.PageIndex = 0;
                        }
                        else if (this.GridViewMoveMode == MoveMode.RowMode)
                        {
                            if (gridView.AllowPaging)
                                gridView.PageIndex = 0;
                            gridView.SelectedIndex = 0;
                        }
                        break;
                    case "cmdPrevious":
                        if (this.GridViewMoveMode == MoveMode.PageMode)
                        {
                            if (gridView.AllowPaging && gridView.PageIndex > 0)
                                gridView.PageIndex -= 1;
                        }
                        else if (this.GridViewMoveMode == MoveMode.RowMode)
                        {
                            if (gridView.SelectedIndex == 0)
                            {
                                if (gridView.AllowPaging && gridView.PageIndex > 0)
                                {
                                    gridView.PageIndex -= 1;
                                    gridView.SelectedIndex = gridView.PageSize - 1;
                                }
                            }
                            else
                            {
                                gridView.SelectedIndex -= 1;
                            }
                        }
                        break;
                    case "cmdNext":
                        if (this.GridViewMoveMode == MoveMode.PageMode)
                        {
                            if (gridView.AllowPaging && gridView.PageIndex != gridView.PageCount - 1)
                            {
                                gridView.PageIndex += 1;
                            }
                            if (gridView.PageIndex == gridView.PageCount - 1)
                            {
                                int j = gridView.PageIndex;
                                Object c = (this.Page.Master == null) ? this.Page.FindControl(gridView.DataSourceID) : this.Parent.FindControl(gridView.DataSourceID);
                                if (c != null)
                                {
                                    WebDataSource webDs = ((WebDataSource)c);
                                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                    {
                                        DataTable table = new DataTable();
                                        if (webDs.CommandTable != null)
                                        {
                                            table = webDs.CommandTable;
                                        }
                                        else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                        {
                                            table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                        }
                                        int i = table.Rows.Count;
                                        while (i <= gridView.PageSize * (j + 1) && (!webDs.Eof))
                                        {
                                            webDs.GetNextPacket();
                                            i += webDs.PacketRecords;
                                        }
                                    }
                                }
                            }
                        }
                        else if (this.GridViewMoveMode == MoveMode.RowMode)
                        {
                            if (gridView.AllowPaging)
                            {
                                if (gridView.PageIndex == gridView.PageCount - 1)
                                {
                                    if (gridView.SelectedIndex != gridView.Rows.Count - 1)
                                        gridView.SelectedIndex += 1;
                                }
                                else
                                {
                                    //不是最后一行
                                    if (gridView.SelectedIndex != gridView.PageSize - 1)
                                    {
                                        gridView.SelectedIndex += 1;
                                    }
                                    //最后一行
                                    else
                                    {
                                        gridView.PageIndex += 1;
                                        gridView.SelectedIndex = 0;
                                        if (gridView.PageIndex == gridView.PageCount - 1)
                                        {
                                            int j = gridView.PageIndex;
                                            Object c = (this.Page.Master == null) ? this.Page.FindControl(gridView.DataSourceID) : this.Parent.FindControl(gridView.DataSourceID);
                                            if (c != null)
                                            {
                                                WebDataSource webDs = ((WebDataSource)c);
                                                if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                                {
                                                    DataTable table = new DataTable();
                                                    if (webDs.CommandTable != null)
                                                    {
                                                        table = webDs.CommandTable;
                                                    }
                                                    else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                                    {
                                                        table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                                    }
                                                    int i = table.Rows.Count;
                                                    while (i <= gridView.PageSize * (j + 1) && (!webDs.Eof))
                                                    {
                                                        webDs.GetNextPacket();
                                                        i += webDs.PacketRecords;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "cmdLast":
                        if (gridView.AllowPaging)
                        {
                            gridView.PageIndex = gridView.PageCount - 1;
                            int j = gridView.PageIndex;
                            Object c = (this.Page.Master == null) ? this.Page.FindControl(gridView.DataSourceID) : this.Parent.FindControl(gridView.DataSourceID);
                            if (c != null)
                            {
                                WebDataSource webDs = ((WebDataSource)c);
                                if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                {
                                    DataTable table = new DataTable();
                                    if (webDs.CommandTable != null)
                                    {
                                        table = webDs.CommandTable;
                                    }
                                    else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                    {
                                        table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                    }
                                    int i = table.Rows.Count;
                                    bool b = false;
                                    while (i <= gridView.PageSize * (j + 1) && (!webDs.Eof))
                                    {
                                        webDs.GetNextPacket();
                                        i += webDs.PacketRecords;
                                        b = true;
                                    }

                                    if (this.GridViewMoveMode == MoveMode.RowMode)
                                    {
                                        if (!b && gridView.PageSize > 1)
                                        {
                                            gridView.SelectedIndex = (i % gridView.PageSize - 1 + gridView.PageSize) % gridView.PageSize;
                                        }
                                        else
                                        {
                                            gridView.SelectedIndex = gridView.PageSize - 1;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "cmdAdd":
                        if (ds.AllowAdd)
                        {
                            if (!CheckInsertedNotApply(gridView, 1))
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "InsertedNotApply", true);

                                ScriptHelper.ShowMessage(this, "InsertedNotApply", message);
                                formView.NeedExecuteAdd = false;
                                return;
                            }

                            formView.NeedExecuteAdd = true;
                            if (gridView.EditURL != null && gridView.EditURL != "")
                            {
                                gridView.OpenEditURL(WebGridView.OpenEditMode.Insert);
                                return;
                            }
                            else if (!string.IsNullOrEmpty(gridView.EditURLPanel))
                            {
                                object obj_pan = this.GetObjByID(gridView.EditURLPanel);
                                if (obj_pan != null && obj_pan is IModalPanel)
                                {
                                    IModalPanel panel = obj_pan as IModalPanel;
                                    panel.Open(WebGridView.OpenEditMode.Insert, new GridViewCommandEventArgs(null, new CommandEventArgs("Insert", gridView.ID)));
                                }
                                return;
                            }
                            else
                            {
                                gridView.GridInserting = true;
                                if (!gridView.TotalActive)
                                    gridView.ShowFooter = true;
                                else
                                    gridView.DataBind();
                                gridView.OnAdding(new EventArgs());
                            }
                            if (bindingNav != null && bindingNav != this)
                            {
                                bindingNav.SetState(NavigatorState.Inserting);
                                bindingNav.SetNavState("Inserting");
                            }
                            this.SetState(NavigatorState.Inserting);
                            this.SetNavState("Inserting");
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToAdd", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdOK":
                        gridView.OnInserting(new EventArgs());
                        if (!gridView.SkipInsert)
                        {
                            doOkForGridInsert(gridView, ds);
                        }
                        gridView.SkipInsert = false;
                        if (!gridView.ValidateFailed)
                            FlowOK();
                        break;
                    case "cmdCancel":
                        if (validate != null)
                            validate.Text = string.Empty;
                        gridView.GridInserting = false;
                        if (gridView.EditIndex != -1)
                            gridView.EditIndex = -1;
                        if (!gridView.TotalActive)
                            gridView.ShowFooter = false;
                        else
                            gridView.DataBind();

                        if (bindingNav != null && bindingNav != this)
                        {
                            if (gridView.DataHasChanged)
                            {
                                bindingNav.SetState(NavigatorState.Changed);
                                bindingNav.SetNavState("Changing");
                            }
                            else
                            {
                                bindingNav.SetState(NavigatorState.Browsing);
                                bindingNav.SetNavState("Browsed");
                            }
                        }
                        if (gridView.DataHasChanged)
                        {
                            this.SetState(NavigatorState.Changed);
                            this.SetNavState("Changing");
                        }
                        else
                        {
                            this.SetState(NavigatorState.Browsing);
                            this.SetNavState("Browsed");
                        }
                        FlowCancel();
                        gridView.OnCanceled(EventArgs.Empty);
                        break;
                    case "cmdApply":
                        foreach (GridViewRow row in gridView.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                                {
                                    gridView.UpdateRow(row.RowIndex, false);

                                    if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                                        return;
                                }
                            }
                        }
                        if (gridView.GridInserting)
                            doOkForGridInsert(gridView, ds);
                        bool bSucess = ds.ApplyUpdates();
                        if (bSucess && !gridView.ValidateFailed)
                        {
                            if (ds.AutoRecordLock)
                            {
                                ds.RemoveLock();
                            }
                            if (bindingNav != null && bindingNav != this)
                            {
                                bindingNav.SetState(NavigatorState.ApplySucess);
                                bindingNav.SetNavState("Browsed");
                            }
                            this.SetState(NavigatorState.ApplySucess);
                            this.SetNavState("Browsed");
                            FlowApply();
                        }
                        else
                        {
                            this.SetState(NavigatorState.ApplyFail);
                            if (gridView.EditIndex != -1)
                            {
                                this.SetNavState("Editing");
                                if (bindingNav != null && bindingNav != this)
                                    bindingNav.SetNavState("Editing");
                            }
                            else
                            {
                                this.SetNavState("Inserting");
                                if (bindingNav != null && bindingNav != this)
                                    bindingNav.SetNavState("Inserting");
                            }
                        }
                        break;
                    case "cmdAbort":
                        if (validate != null)
                            validate.Text = string.Empty;
                        if (gridView.EditIndex != -1)
                            gridView.EditIndex = -1;
                        if (gridView.ShowFooter && !gridView.TotalActive)
                        {
                            gridView.ShowFooter = false;
                        }
                        ds.InnerDataSet.RejectChanges();
                        if (ds.AutoRecordLock)
                        {
                            ds.RemoveLock();
                        }
                        this.SetState(NavigatorState.Aborted);
                        this.SetNavState("Browsed");
                        gridView.DataBind();
                        FlowAbort();
                        gridView.OnCanceled(EventArgs.Empty);
                        break;
                    case "cmdQuery":
                        DoQuery(ds);
                        break;
                    case "cmdPrint":
                        if (ds.AllowPrint)
                        { }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdExport":
                        Export();
                        break;
                    default:
                        GridViewFlowCommand(e);
                        break;
                }
                #endregion
            }
            else if (this.ShowDataStyle == NavigatorStyle.DetailStyle && obj is WebDetailsView)
            {
                detailView = (WebDetailsView)obj;
                if (vobj is WebGridView)
                {
                    viewGridView = (WebGridView)vobj;
                    ViewExist = true;
                }
                ds = (WebDataSource)GetObjByID(detailView.DataSourceID);
                WebValidate validate = (WebValidate)detailView.ExtendedFindChildControl(detailView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                WebNavigator bindingNav = detailView.GetBindingNavigator();
                #region DetailsView Default Operation
                switch (e.CommandName)
                {
                    case "cmdFirst":
                        if (ViewExist)
                        {
                            if (this.GridViewMoveMode == MoveMode.PageMode)
                            {
                                if (viewGridView.AllowPaging)
                                    viewGridView.PageIndex = 0;
                            }
                            else if (this.GridViewMoveMode == MoveMode.RowMode)
                            {
                                if (viewGridView.AllowPaging)
                                    viewGridView.PageIndex = 0;
                                viewGridView.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            if (detailView.AllowPaging)
                                detailView.PageIndex = 0;
                        }
                        break;
                    case "cmdPrevious":
                        if (ViewExist)
                        {
                            if (this.GridViewMoveMode == MoveMode.PageMode)
                            {
                                if (viewGridView.AllowPaging && viewGridView.PageIndex > 0)
                                    viewGridView.PageIndex -= 1;
                            }
                            else if (this.GridViewMoveMode == MoveMode.RowMode)
                            {
                                if (viewGridView.SelectedIndex == 0)
                                {
                                    if (viewGridView.AllowPaging && viewGridView.PageIndex > 0)
                                    {
                                        viewGridView.PageIndex -= 1;
                                        viewGridView.SelectedIndex = viewGridView.PageSize - 1;
                                    }
                                }
                                else
                                {
                                    viewGridView.SelectedIndex -= 1;
                                }
                            }
                        }
                        else
                        {
                            if (detailView.AllowPaging && detailView.PageIndex > 0)
                                detailView.PageIndex -= 1;
                        }
                        break;
                    case "cmdNext":
                        if (ViewExist)
                        {
                            if (this.GridViewMoveMode == MoveMode.PageMode)
                            {
                                if (viewGridView.AllowPaging && viewGridView.PageIndex != viewGridView.PageCount - 1)
                                {
                                    viewGridView.PageIndex += 1;
                                }
                                if (viewGridView.PageIndex == viewGridView.PageCount - 1)
                                {
                                    Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID)
                                        : this.Parent.FindControl(viewGridView.DataSourceID);
                                    if (c != null)
                                    {
                                        ((WebDataSource)c).GetNextPacket();
                                    }
                                }
                            }
                            else if (this.GridViewMoveMode == MoveMode.RowMode)
                            {
                                if (viewGridView.AllowPaging)
                                {
                                    if (viewGridView.PageIndex == viewGridView.PageCount - 1)
                                    {
                                        if (viewGridView.SelectedIndex != viewGridView.Rows.Count - 1)
                                            viewGridView.SelectedIndex += 1;
                                    }
                                    else
                                    {
                                        //不是最后一行
                                        if (viewGridView.SelectedIndex != viewGridView.PageSize - 1)
                                        {
                                            viewGridView.SelectedIndex += 1;
                                        }
                                        //最后一行
                                        else
                                        {
                                            viewGridView.PageIndex += 1;
                                            viewGridView.SelectedIndex = 0;
                                            if (viewGridView.PageIndex == viewGridView.PageCount - 1)
                                            {
                                                int j = viewGridView.PageIndex;
                                                Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID) : this.Parent.FindControl(viewGridView.DataSourceID);
                                                if (c != null)
                                                {
                                                    WebDataSource webDs = ((WebDataSource)c);
                                                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                                    {
                                                        DataTable table = new DataTable();
                                                        if (webDs.CommandTable != null)
                                                        {
                                                            table = webDs.CommandTable;
                                                        }
                                                        else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                                        {
                                                            table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                                        }
                                                        int i = table.Rows.Count;
                                                        while (i <= viewGridView.PageSize * (j + 1) && (!webDs.Eof))
                                                        {
                                                            webDs.GetNextPacket();
                                                            i += webDs.PacketRecords;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (detailView.AllowPaging && detailView.PageIndex != detailView.PageCount - 1)
                            {
                                detailView.PageIndex += 1;
                                if (detailView.PageIndex == detailView.PageCount - 1)
                                {
                                    Object c = (this.Page.Master == null) ? this.Page.FindControl(detailView.DataSourceID)
                                        : this.Parent.FindControl(detailView.DataSourceID);
                                    if (c != null)
                                    {
                                        ((WebDataSource)c).GetNextPacket();
                                    }
                                }
                            }
                        }
                        break;
                    case "cmdLast":
                        if (ViewExist)
                        {
                            //if (viewGridView.AllowPaging)
                            //{
                            //    viewGridView.PageIndex = viewGridView.PageCount - 1;
                            //    Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID)
                            //     : this.Parent.FindControl(viewGridView.DataSourceID);
                            //    if (c != null)
                            //    {
                            //        ((WebDataSource)c).GetNextPacket();
                            //    }
                            //}
                            if (viewGridView.AllowPaging)
                            {
                                viewGridView.PageIndex = viewGridView.PageCount - 1;
                                int j = viewGridView.PageIndex;
                                Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID) : this.Parent.FindControl(viewGridView.DataSourceID);
                                if (c != null)
                                {
                                    WebDataSource webDs = ((WebDataSource)c);
                                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                    {
                                        DataTable table = new DataTable();
                                        if (webDs.CommandTable != null)
                                        {
                                            table = webDs.CommandTable;
                                        }
                                        else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                        {
                                            table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                        }
                                        int i = table.Rows.Count;
                                        bool b = false;
                                        while (i <= viewGridView.PageSize * (j + 1) && (!webDs.Eof))
                                        {
                                            webDs.GetNextPacket();
                                            i += webDs.PacketRecords;
                                            b = true;
                                        }

                                        if (this.GridViewMoveMode == MoveMode.RowMode)
                                        {
                                            if (!b && viewGridView.PageSize > 1)
                                            {
                                                viewGridView.SelectedIndex = (i % viewGridView.PageSize - 1 + viewGridView.PageSize) % viewGridView.PageSize;
                                            }
                                            else
                                            {
                                                viewGridView.SelectedIndex = viewGridView.PageSize - 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (detailView.AllowPaging)
                            {
                                detailView.PageIndex = detailView.PageCount - 1;
                                Object c = (this.Page.Master == null) ? this.Page.FindControl(detailView.DataSourceID)
                                     : this.Parent.FindControl(detailView.DataSourceID);
                                if (c != null)
                                {
                                    ((WebDataSource)c).GetNextPacket();
                                }
                            }
                        }
                        break;
                    case "cmdAdd":
                        if (ds.AllowAdd)
                        {
                            if (!CheckInsertedNotApply(detailView, 2))
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "InsertedNotApply", true);

                                ScriptHelper.ShowMessage(this, "InsertedNotApply", message);
                                formView.NeedExecuteAdd = false;
                                return;
                            }
                            formView.NeedExecuteAdd = true;
                            if (validate != null)
                                detailView.SetValidateFlag(validate);
                            detailView.ChangeMode(DetailsViewMode.Insert);
                            if (bindingNav != null && bindingNav != this)
                            {
                                bindingNav.SetState(NavigatorState.Inserting);
                                bindingNav.SetNavState("Inserting");
                            }
                            this.SetState(NavigatorState.Inserting);
                            this.SetNavState("Inserting");
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToAdd", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdUpdate":
                        if (ds.AllowUpdate)
                        {
                            if (!CheckInsertedNotApply(detailView, 2))
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "InsertedNotApply", true);

                                ScriptHelper.ShowMessage(this, "InsertedNotApply", message);
                                return;
                            }
                            formView.NeedExecuteAdd = true;
                            if (!ds.IsEmpty)
                            {
                                if (validate != null)
                                    detailView.SetValidateFlag(validate);
                                detailView.ChangeMode(DetailsViewMode.Edit);
                                if (bindingNav != null && bindingNav != this)
                                {
                                    bindingNav.SetState(NavigatorState.Editing);
                                    bindingNav.SetNavState("Editing");
                                }
                                this.SetState(NavigatorState.Editing);
                                this.SetNavState("Editing");
                            }
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToUpdate", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdDelete":
                        if (ds.InnerDataSet.Tables[ds.DataMember].Rows.Count != 0)
                            detailView.DeleteItem();
                        break;
                    case "cmdOK":
                        if (detailView.CurrentMode == DetailsViewMode.Insert)
                            detailView.InsertItem(false);
                        else if (detailView.CurrentMode == DetailsViewMode.Edit)
                            detailView.UpdateItem(false);
                        if (!detailView.ValidateFailed)
                            FlowOK();
                        break;
                    case "cmdCancel":
                        if (detailView.CurrentMode == DetailsViewMode.Insert
                            || detailView.CurrentMode == DetailsViewMode.Edit)
                        {
                            detailView.ChangeMode(DetailsViewMode.ReadOnly);
                            if (validate != null)
                                validate.Text = string.Empty;
                        }
                        if (validate != null)
                            detailView.RemoveValidateFlag(validate);
                        if (detailView.DataHasChanged)
                        {
                            this.SetState(NavigatorState.Changed);
                            this.SetNavState("Changing");
                        }
                        else
                        {
                            this.SetState(NavigatorState.Browsing);
                            this.SetNavState("Browsed");
                        }
                        this.SetNavState("Browsed");
                        FlowCancel();
                        detailView.OnCanceled(EventArgs.Empty);

                        break;
                    case "cmdApply":
                        if (detailView.CurrentMode == DetailsViewMode.Insert)
                            detailView.InsertItem(false);
                        else if (detailView.CurrentMode == DetailsViewMode.Edit)
                            detailView.UpdateItem(false);
                        if (detailView.CurrentMode == DetailsViewMode.ReadOnly)
                        {
                            bool bSucess = ds.ApplyUpdates();
                            if (bSucess && !detailView.ValidateFailed)
                            {
                                if (ds.AutoRecordLock)
                                {
                                    ds.RemoveLock();
                                }
                                if (bindingNav != null && bindingNav != this)
                                {
                                    bindingNav.SetState(NavigatorState.ApplySucess);
                                    bindingNav.SetNavState("Browsed");
                                }
                                this.SetState(NavigatorState.ApplySucess);
                                this.SetNavState("Browsed");
                                FlowApply();
                            }
                            else
                            {
                                this.SetState(NavigatorState.ApplyFail);
                                if (detailView.CurrentMode == DetailsViewMode.Edit)
                                {
                                    if (bindingNav != null && bindingNav != this)
                                    {
                                        bindingNav.SetNavState("Editing");
                                    }
                                    this.SetNavState("Editing");
                                }
                                else if (detailView.CurrentMode == DetailsViewMode.Insert)
                                {
                                    if (bindingNav != null && bindingNav != this)
                                    {
                                        bindingNav.SetNavState("Inserting");
                                    }
                                    this.SetNavState("Inserting");
                                }
                            }
                        }
                        break;
                    case "cmdAbort":
                        if (detailView.CurrentMode == DetailsViewMode.Insert
                            || detailView.CurrentMode == DetailsViewMode.Edit)
                        {
                            detailView.ChangeMode(DetailsViewMode.ReadOnly);
                            if (validate != null)
                                validate.Text = string.Empty;
                        }
                        ds.InnerDataSet.RejectChanges();
                        if (ds.AutoRecordLock)
                        {
                            ds.RemoveLock();
                        }
                        if (validate != null)
                            detailView.RemoveValidateFlag(validate);
                        if (bindingNav != null && bindingNav != this)
                        {
                            bindingNav.SetState(NavigatorState.Aborted);
                            bindingNav.SetNavState("Browsed");
                        }
                        this.SetState(NavigatorState.Aborted);
                        this.SetNavState("Browsed");
                        detailView.DataBind();
                        FlowAbort();
                        detailView.OnCanceled(EventArgs.Empty);
                        break;
                    case "cmdQuery":
                        if (ViewExist)
                        {
                            ds = this.GetObjByID(viewGridView.DataSourceID) as WebDataSource;
                        }
                        DoQuery(ds);
                        break;
                    case "cmdPrint":
                        if (ViewExist)
                        {
                            if (ds.AllowPrint)
                            { }
                            else
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                                CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                            }
                        }
                        else
                        {
                            if (ds.AllowPrint)
                            { }
                            else
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                                CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                            }
                        }
                        break;
                    case "cmdExport":
                        Export();
                        break;
                    default:
                        DetailsViewFlowCommand(e);
                        break;
                }
                #endregion
            }
            else if (this.ShowDataStyle == NavigatorStyle.FormViewStyle && obj is WebFormView)
            {
                formView = (WebFormView)obj;
                ds = (WebDataSource)GetObjByID(formView.DataSourceID);
                WebValidate validate = (WebValidate)formView.ExtendedFindChildControl(formView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                if (vobj is WebGridView)
                {
                    viewGridView = (WebGridView)vobj;
                    ViewExist = true;
                }
                #region WebFormView Default Operation
                switch (e.CommandName)
                {
                    case "cmdFirst":
                        if (ViewExist)
                        {
                            //if (viewGridView.AllowPaging)
                            //    viewGridView.PageIndex = 0;
                            if (this.GridViewMoveMode == MoveMode.PageMode)
                            {
                                if (viewGridView.AllowPaging)
                                    viewGridView.PageIndex = 0;
                            }
                            else if (this.GridViewMoveMode == MoveMode.RowMode)
                            {
                                if (viewGridView.AllowPaging)
                                    viewGridView.PageIndex = 0;
                                viewGridView.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            if (formView.AllowPaging)
                                formView.PageIndex = 0;
                        }
                        break;
                    case "cmdPrevious":
                        if (ViewExist)
                        {
                            //if (viewGridView.AllowPaging && viewGridView.PageIndex > 0)
                            //    viewGridView.PageIndex -= 1;
                            if (this.GridViewMoveMode == MoveMode.PageMode)
                            {
                                if (viewGridView.AllowPaging && viewGridView.PageIndex > 0)
                                    viewGridView.PageIndex -= 1;
                            }
                            else if (this.GridViewMoveMode == MoveMode.RowMode)
                            {
                                if (viewGridView.SelectedIndex == 0)
                                {
                                    if (viewGridView.AllowPaging && viewGridView.PageIndex > 0)
                                    {
                                        viewGridView.PageIndex -= 1;
                                        viewGridView.SelectedIndex = viewGridView.PageSize - 1;
                                    }
                                }
                                else
                                {
                                    viewGridView.SelectedIndex -= 1;
                                }
                            }
                        }
                        else
                        {
                            if (formView.AllowPaging && formView.PageIndex > 0)
                                formView.PageIndex -= 1;
                        }
                        break;
                    case "cmdNext":
                        if (ViewExist)
                        {
                            if (this.GridViewMoveMode == MoveMode.PageMode)
                            {
                                if (viewGridView.AllowPaging && viewGridView.PageIndex != viewGridView.PageCount - 1)
                                {
                                    viewGridView.PageIndex += 1;
                                }
                                if (viewGridView.PageIndex == viewGridView.PageCount - 1)
                                {
                                    Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID)
                                        : this.Parent.FindControl(viewGridView.DataSourceID);
                                    if (c != null)
                                    {
                                        ((WebDataSource)c).GetNextPacket();
                                    }
                                }
                            }
                            else if (this.GridViewMoveMode == MoveMode.RowMode)
                            {
                                if (viewGridView.AllowPaging)
                                {
                                    if (viewGridView.PageIndex == viewGridView.PageCount - 1)
                                    {
                                        if (viewGridView.SelectedIndex != viewGridView.Rows.Count - 1)
                                            viewGridView.SelectedIndex += 1;
                                    }
                                    else
                                    {
                                        //不是最后一行
                                        if (viewGridView.SelectedIndex != viewGridView.PageSize - 1)
                                        {
                                            viewGridView.SelectedIndex += 1;
                                        }
                                        //最后一行
                                        else
                                        {
                                            viewGridView.PageIndex += 1;
                                            viewGridView.SelectedIndex = 0;
                                            if (viewGridView.PageIndex == viewGridView.PageCount - 1)
                                            {
                                                int j = viewGridView.PageIndex;
                                                Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID) : this.Parent.FindControl(viewGridView.DataSourceID);
                                                if (c != null)
                                                {
                                                    WebDataSource webDs = ((WebDataSource)c);
                                                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                                    {
                                                        DataTable table = new DataTable();
                                                        if (webDs.CommandTable != null)
                                                        {
                                                            table = webDs.CommandTable;
                                                        }
                                                        else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                                        {
                                                            table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                                        }
                                                        int i = table.Rows.Count;
                                                        while (i <= viewGridView.PageSize * (j + 1) && (!webDs.Eof))
                                                        {
                                                            webDs.GetNextPacket();
                                                            i += webDs.PacketRecords;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (formView.AllowPaging && formView.PageIndex != formView.PageCount - 1)
                            {
                                formView.PageIndex += 1;
                                if (formView.PageIndex == formView.PageCount - 1)
                                {
                                    Object c = (this.Page.Master == null) ? this.Page.FindControl(formView.DataSourceID)
                                        : this.Parent.FindControl(formView.DataSourceID);
                                    if (c != null)
                                    {
                                        ((WebDataSource)c).GetNextPacket();
                                    }
                                }
                            }
                        }
                        break;
                    case "cmdLast":
                        if (ViewExist)
                        {
                            //if (viewGridView.AllowPaging)
                            //{
                            //    viewGridView.PageIndex = viewGridView.PageCount - 1;
                            //    Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID) 
                            //        : this.Parent.FindControl(viewGridView.DataSourceID);
                            //    if (c != null)
                            //    {
                            //        ((WebDataSource)c).GetNextPacket();
                            //    }
                            //}
                            if (viewGridView.AllowPaging)
                            {
                                viewGridView.PageIndex = viewGridView.PageCount - 1;
                                int j = viewGridView.PageIndex;
                                Object c = (this.Page.Master == null) ? this.Page.FindControl(viewGridView.DataSourceID) : this.Parent.FindControl(viewGridView.DataSourceID);
                                if (c != null)
                                {
                                    WebDataSource webDs = ((WebDataSource)c);
                                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                                    {
                                        DataTable table = new DataTable();
                                        if (webDs.CommandTable != null)
                                        {
                                            table = webDs.CommandTable;
                                        }
                                        else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                                        {
                                            table = webDs.InnerDataSet.Tables[webDs.DataMember];
                                        }
                                        int i = table.Rows.Count;
                                        bool b = false;
                                        while (i <= viewGridView.PageSize * (j + 1) && (!webDs.Eof))
                                        {
                                            webDs.GetNextPacket();
                                            i += webDs.PacketRecords;
                                            b = true;
                                        }

                                        if (this.GridViewMoveMode == MoveMode.RowMode)
                                        {
                                            if (!b && viewGridView.PageSize > 1)
                                            {
                                                viewGridView.SelectedIndex = (i % viewGridView.PageSize - 1 + viewGridView.PageSize) % viewGridView.PageSize;
                                            }
                                            else
                                            {
                                                viewGridView.SelectedIndex = viewGridView.PageSize - 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (formView.AllowPaging)
                            {
                                formView.PageIndex = formView.PageCount - 1;
                                Object c = (this.Page.Master == null) ? this.Page.FindControl(formView.DataSourceID)
                                    : this.Parent.FindControl(formView.DataSourceID);
                                if (c != null)
                                {
                                    ((WebDataSource)c).GetNextPacket();
                                }
                            }
                        }
                        break;
                    case "cmdAdd":
                        if (ds.AllowAdd)
                        {
                            if (!CheckInsertedNotApply(formView, 3))
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "InsertedNotApply", true);

                                ScriptHelper.ShowMessage(this, "InsertedNotApply", message);
                                formView.NeedExecuteAdd = false;
                                return;
                            }
                            formView.NeedExecuteAdd = true;
                            formView.ChangeMode(FormViewMode.Insert);
                            formView.OnAdding(EventArgs.Empty);
                            this.SetState(NavigatorState.Inserting);
                            this.SetNavState("Inserting");
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToAdd", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdUpdate":
                        if (ds.AllowUpdate)
                        {
                            if (!CheckInsertedNotApply(formView, 3))
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "InsertedNotApply", true);

                                ScriptHelper.ShowMessage(this, "InsertedNotApply", message);
                                return;
                            }
                            formView.NeedExecuteAdd = true;
                            if (!ds.IsEmpty)
                            {
                                formView.ChangeMode(FormViewMode.Edit);
                                this.SetState(NavigatorState.Editing);
                                this.SetNavState("Editing");
                            }
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToUpdate", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdDelete":
                        if (ds.AllowDelete)
                            formView.DeleteItem();
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToDelete", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdOK":
                        if (formView.CurrentMode == FormViewMode.Insert)
                            formView.InsertItem(false);
                        else if (formView.CurrentMode == FormViewMode.Edit)
                            formView.UpdateItem(false);
                        if (!formView.ValidateFailed)
                            FlowOK();
                        break;
                    case "cmdCancel":
                        if (formView.CurrentMode == FormViewMode.Insert
                            || formView.CurrentMode == FormViewMode.Edit)
                        {
                            formView.ChangeMode(formView.DefaultMode);
                            if (validate != null)
                                validate.Text = string.Empty;
                            if (formView.DataHasChanged)
                            {
                                this.SetState(NavigatorState.Changed);
                                this.SetNavState("Changing");
                            }
                            else
                            {
                                this.SetState(NavigatorState.Browsing);
                                this.SetNavState("Browsed");
                            }
                            FlowCancel();
                            formView.OnCanceled(EventArgs.Empty);
                        }
                        break;
                    case "cmdApply":
                        if (formView.CurrentMode == FormViewMode.Insert)
                            formView.InsertItem(false);
                        else if (formView.CurrentMode == FormViewMode.Edit)
                            formView.UpdateItem(false);
                        if (formView.CurrentMode == FormViewMode.ReadOnly) //防止formview updating event 调用 e.cancel == true; navigator state会变
                        {
                            bool bSucess = ds.ApplyUpdates();
                            if (bSucess && !formView.ValidateFailed)
                            {
                                if (ds.AutoRecordLock)
                                {
                                    ds.RemoveLock();
                                }
                                this.SetState(NavigatorState.ApplySucess);
                                this.SetNavState("Browsed");
                                FlowApply();
                            }
                            else
                            {
                                this.SetState(NavigatorState.ApplyFail);
                                if (formView.CurrentMode == FormViewMode.Edit)
                                    this.SetNavState("Editing");
                                else if (formView.CurrentMode == FormViewMode.Insert)
                                    this.SetNavState("Inserting");
                            }
                        }
                        break;
                    case "cmdAbort":
                        ds.InnerDataSet.RejectChanges();
                        formView.DataBind();
                        if (formView.CurrentMode == FormViewMode.Insert || formView.CurrentMode == FormViewMode.Edit)
                        {
                            formView.ChangeMode(formView.DefaultMode);
                            if (validate != null)
                                validate.Text = string.Empty;
                        }
                        if (ds.AutoRecordLock)
                        {
                            ds.RemoveLock();
                        }
                        this.SetState(NavigatorState.Aborted);
                        this.SetNavState("Browsed");
                        FlowAbort();
                        formView.OnCanceled(EventArgs.Empty);
                        break;
                    case "cmdQuery":
                        if (ViewExist)
                        {
                            ds = this.GetObjByID(viewGridView.DataSourceID) as WebDataSource;
                        }
                        DoQuery(ds);
                        break;
                    case "cmdPrint":
                        if (ViewExist)
                        {
                            if (ds.AllowPrint)
                            { }
                            else
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                                CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                            }
                        }
                        else
                        {
                            if (ds.AllowPrint)
                            { }
                            else
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                                CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                            }
                        }
                        break;
                    case "cmdExport":
                        Export();
                        break;
                    default:
                        FormViewFlowCommand(e);
                        break;
                }
                #endregion
            }
            else if (this.ShowDataStyle == NavigatorStyle.ASPxGridView && obj.GetType().Name == "ASPxGridView")
            {
                var aspxGridView = obj.GetType();

                object objPageIndex = obj.GetType().GetProperty("PageIndex").GetValue(obj, null);
                int pageIndex = int.Parse(objPageIndex.ToString());
                object objPageCount = obj.GetType().GetProperty("PageCount").GetValue(obj, null);
                int pageCount = int.Parse(objPageCount.ToString());

                String strDataSourceID = obj.GetType().GetProperty("DataSourceID").GetValue(obj, null).ToString();
                ds = (WebDataSource)GetObjByID(strDataSourceID);
                //WebValidate validate = (WebValidate)gridView.ExtendedFindChildControl(gridView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                #region ASPxGridView Default Operation
                switch (e.CommandName)
                {
                    case "cmdFirst":
                        obj.GetType().GetProperty("PageIndex").SetValue(obj, 0, null);
                        break;
                    case "cmdPrevious":
                        if (objPageIndex != null)
                        {
                            obj.GetType().GetProperty("PageIndex").SetValue(obj, pageIndex - 1, null);
                        }
                        break;
                    case "cmdNext":
                        if (objPageIndex != null)
                        {
                            if (pageIndex == pageCount - 1)
                            {
                                ((WebDataSource)ds).GetNextPacket();
                            }
                            obj.GetType().GetProperty("PageIndex").SetValue(obj, pageIndex + 1, null);
                        }
                        break;
                    case "cmdLast":
                        bool b = false;
                        while (!ds.Eof)
                        {
                            ds.GetNextPacket();
                            b = true;
                        }
                        DataBind();
                        obj.GetType().GetProperty("PageIndex").SetValue(obj, pageCount - 1, null);
                        break;
                    case "cmdAdd":
                        if (ds.AllowAdd)
                        {
                            //AddNewRow
                            obj.GetType().GetMethod("AddNewRow").Invoke(obj, null);
                            this.SetState(NavigatorState.Inserting);
                            this.SetNavState("Inserting");
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToAdd", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdUpdate":
                        if (ds.AllowUpdate)
                        {
                            if (!ds.IsEmpty)
                            {
                                this.SetState(NavigatorState.Editing);
                                this.SetNavState("Editing");
                            }
                        }
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToUpdate", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdDelete":
                        if (ds.AllowDelete)
                            ;
                        else
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToDelete", true);
                            CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                        }
                        break;
                    case "cmdOK":
                    case "cmdApply":
                        obj.GetType().GetMethod("UpdateEdit").Invoke(obj, null);
                        this.SetState(NavigatorState.Changed);
                        this.SetNavState("Changing");
                        break;
                    case "cmdCancel":
                    case "cmdAbort":
                        obj.GetType().GetMethod("CancelEdit").Invoke(obj, null);

                        //if (formView.DataHasChanged)
                        //{
                        //    this.SetState(NavigatorState.Changed);
                        //    this.SetNavState("Changing");
                        //}
                        //else
                        {
                            this.SetState(NavigatorState.Browsing);
                            this.SetNavState("Browsed");
                        }
                        break;
                    //case "cmdApply":
                    //    if (formView.CurrentMode == FormViewMode.Insert)
                    //        formView.InsertItem(false);
                    //    else if (formView.CurrentMode == FormViewMode.Edit)
                    //        formView.UpdateItem(false);
                    //    if (formView.CurrentMode == FormViewMode.ReadOnly) //防止formview updating event 调用 e.cancel == true; navigator state会变
                    //    {
                    //        bool bSucess = ds.ApplyUpdates();
                    //        if (bSucess && !formView.ValidateFailed)
                    //        {
                    //            if (ds.AutoRecordLock)
                    //            {
                    //                ds.RemoveLock();
                    //            }
                    //            this.SetState(NavigatorState.ApplySucess);
                    //            this.SetNavState("Browsed");
                    //            FlowApply();
                    //        }
                    //        else
                    //        {
                    //            this.SetState(NavigatorState.ApplyFail);
                    //            if (formView.CurrentMode == FormViewMode.Edit)
                    //                this.SetNavState("Editing");
                    //            else if (formView.CurrentMode == FormViewMode.Insert)
                    //                this.SetNavState("Inserting");
                    //        }
                    //    }
                    //    break;
                    //case "cmdAbort":
                    //    ds.InnerDataSet.RejectChanges();
                    //    formView.DataBind();
                    //    if (formView.CurrentMode == FormViewMode.Insert || formView.CurrentMode == FormViewMode.Edit)
                    //    {
                    //        formView.ChangeMode(formView.DefaultMode);

                    //    }
                    //    if (ds.AutoRecordLock)
                    //    {
                    //        ds.RemoveLock();
                    //    }
                    //    this.SetState(NavigatorState.Aborted);
                    //    this.SetNavState("Browsed");
                    //    FlowAbort();
                    //    formView.OnCanceled(EventArgs.Empty);
                    //    break;
                    case "cmdQuery":
                        if (ViewExist)
                        {
                            ds = this.GetObjByID(viewGridView.DataSourceID) as WebDataSource;
                        }
                        DoQuery(ds);
                        break;
                    case "cmdPrint":
                        if (ViewExist)
                        {
                            if (ds.AllowPrint)
                            { }
                            else
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                                CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                            }
                        }
                        else
                        {
                            if (ds.AllowPrint)
                            { }
                            else
                            {
                                language = CliUtils.fClientLang;
                                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToPrint", true);
                                CliUtils.RegisterStartupScript(this, "alert('" + message + "')");
                            }
                        }
                        break;
                    case "cmdExport":
                        Export();
                        break;
                    default:
                        FormViewFlowCommand(e);
                        break;
                }
                #endregion

            }
        }

        private void DoQuery(WebDataSource ds)
        {
            if (this.QueryMode == QueryModeType.Normal)
            {
                string param = "MultiLan=" + this.MultiLanguage.ToString() + "&Params=";
                DataColumnCollection dcc = (ds.CommandTable == null) ? ds.InnerDataSet.Tables[0].Columns : ds.CommandTable.Columns;
                // 添加要用于查询的字段
                StringBuilder Params = new StringBuilder();
                StringBuilder DataTypes = new StringBuilder();
                StringBuilder Conditions = new StringBuilder();
                StringBuilder IsNvarchars = new StringBuilder();
                if (this.QueryFields.Count > 0)
                {
                    foreach (WebQueryField f in this.QueryFields)
                    {
                        if (dcc.Contains(f.FieldName))
                        {
                            Params.Append(dcc[f.FieldName].ColumnName);
                            Params.Append(";");
                            DataTypes.Append(dcc[f.FieldName].DataType);
                            DataTypes.Append(";");
                            Conditions.Append(f.Condition);
                            Conditions.Append(";");
                            IsNvarchars.Append(f.IsNvarChar);
                            IsNvarchars.Append(";");
                        }
                    }
                }
                else
                {
                    foreach (DataColumn dc in dcc)
                    {
                        Params.Append(dc.ColumnName);
                        Params.Append(";");
                        DataTypes.Append(dc.DataType);
                        DataTypes.Append(";");

                    }
                }
                param += Params.Remove(Params.Length - 1, 1).ToString();
                param += "&DataTypes=";
                param += DataTypes.Remove(DataTypes.Length - 1, 1).ToString();
                if (Conditions.Length > 0)
                {
                    param += "&Conditions=";
                    param += Conditions.Remove(Conditions.Length - 1, 1).ToString();
                }
                if (IsNvarchars.Length > 0)
                {
                    param += "&IsNvarchars=";
                    param += IsNvarchars.Remove(IsNvarchars.Length - 1, 1).ToString();
                }
                // 添加原始page的url
                param += "&PagePath=" + Page.Request.FilePath;
                // 添加query的对象(WebDataSource)
                param += "&DataSourceID=" + ds.ID;
                // 添加query的dataset对象id, 和DbAlias, CommandText对象
                string dsId = "", selAlias = "", cmdText = "";
                dsId = ds.WebDataSetID;
                selAlias = ds.SelectAlias;
                cmdText = ds.SelectCommand;
                param += "&DataSetID=" + dsId + "&DbAlias=" + selAlias + "&CommandText=" + cmdText;
                string itemparam = this.Page.Request.QueryString["ItemParam"] != null ? HttpUtility.UrlEncode(this.Page.Request.QueryString["ItemParam"]) : string.Empty;
                param += "&ItemParam=" + itemparam;
                string flowUrl = this.getFlowUrl();
                param += flowUrl;
                if (QueryStyle == QueryStyleType.Dialog)
                {
                    param += "&Dialog=true";
                }

                string url = @"../InnerPages/frmNavQuery.aspx?";
                if (SessionRequest.Enable)
                {
                    SessionRequest sessionRequest = new SessionRequest(this.Page);
                    url += sessionRequest.SetRequestValue(param);
                }
                else
                {
                    url += param;
                }
                if (QueryStyle == QueryStyleType.NewPage)
                {
                    Page.Response.Redirect(url);
                }
                else
                {
                    string script = string.Format("window.open('{0}','','left=200,top=200,width=600,scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no')"
                        , url, flowUrl);
                    ScriptHelper.RegisterStartupScript(this, script);
                }
            }
            else if (this.QueryMode == QueryModeType.ClientQuery)
            {
                WebClientQuery wcq = CopyQueryFileds(1, true);
                if (QueryStyle == QueryStyleType.NewPage)
                {
                    wcq.Execute();
                }
                else
                {
                    wcq.Execute(true);
                }
            }
            else if (this.QueryMode == QueryModeType.AnyQuery)
            {
                WebAnyQuery waq = CopyAnyQueryFileds(1, true);
                if (QueryStyle == QueryStyleType.NewPage)
                {
                    waq.Execute();
                }
                else
                {
                    waq.Execute(true);
                }
            }
        }

        public void Export()
        {
            string path = this.Page.MapPath(this.Page.AppRelativeVirtualPath);
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            path = string.Format("{0}\\ExcelDoc\\{1}", directory, string.Format("{0}-{1:yyMMddHHmmss}", filename, DateTime.Now));
            path = Path.ChangeExtension(path, "xls");
            string newfilename = Path.GetFileName(path);

            InfoDataSet ids = new InfoDataSet();
            WebDataSource wds = null;
            if (!string.IsNullOrEmpty(this.ViewBindingObject))
            {
                try
                {
                    wds = this.GetObjByID((this.GetObjByID(this.ViewBindingObject) as WebGridView).DataSourceID) as WebDataSource;
                }
                catch
                {
                    throw new Exception("Type of ViewBindingObject should be WebGridView");
                }
            }
            else
            {
                object view = this.GetObjByID(this.BindingObject);
                if (view != null)
                {
                    string datasourceid = view.GetType().GetProperty("DataSourceID").GetValue(view, null).ToString();
                    wds = this.GetObjByID(datasourceid) as WebDataSource;
                }
                else
                {
                    throw new Exception(string.Format("Can not find BindingObject: {0}", this.BindingObject));
                }
            }

            wds.ToExcel(0, string.Empty, true);
        }

        [Obsolete("This method is obsolete, set open parameter of WebDataSource.ToExcel() to be true")]
        public void Export(string filename)
        {
            string eurl = "../InnerPages/frmExport.aspx?File=" + filename;
            string script = "window.open('" + eurl + "','download','height=120,width=240,top=300,left=300, scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no')";

            ScriptHelper.RegisterStartupScript(this, script);
        }

        [Obsolete("This method is obsolete, set open parameter of WebDataSource.ToExcel() to be true")]
        public static void Export(string filename, Page page)
        {
            string eurl = "../InnerPages/frmExport.aspx?File=" + filename;
            string script = "window.open('" + eurl + "','download','height=100,width=200, scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no')";

            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script>" + script + "</script>");
        }

        protected virtual string getFlowUrl()
        {
            return "";
        }

        protected virtual void FlowApply()
        { }

        protected virtual void FlowAbort()
        { }

        protected virtual void FlowOK()
        { }

        protected virtual void FlowCancel()
        { }

        protected virtual void GridViewFlowCommand(CommandEventArgs e)
        { }

        protected virtual void DetailsViewFlowCommand(CommandEventArgs e)
        { }

        protected virtual void FormViewFlowCommand(CommandEventArgs e)
        { }

        private WebClientQuery CopyQueryFileds(int colunmcount)
        {
            return CopyQueryFileds(colunmcount, false);
        }
        private WebClientQuery CopyQueryFileds(int columncount, bool newpage)
        {
            WebClientQuery wcq = new WebClientQuery();

            if (newpage)
            {
                wcq.ID = this.ID + "QueryTemp";
            }
            else
            {
                object objQuery = this.GetObjByID(this.ID + "QueryTempPanel");
                if (objQuery != null)
                {
                    wcq = objQuery as WebClientQuery;
                    return wcq;
                }
                wcq.ID = this.ID + "QueryTempPanel";

            }
            if (this.QueryFields.Count > 0)
            {
                WebDataSource ds = null;
                object obj = null;
                if (this.ViewBindingObject != null && this.ViewBindingObject != "")
                {
                    obj = this.GetObjByID(this.ViewBindingObject);
                }
                else if (this.BindingObject != null && this.BindingObject != "")
                {
                    obj = this.GetObjByID(this.BindingObject);
                }
                if (obj != null)
                {
                    String strDataSourceID = String.Empty;
                    if (obj is CompositeDataBoundControl)
                    {
                        CompositeDataBoundControl dataControl = (CompositeDataBoundControl)obj;
                        strDataSourceID = dataControl.DataSourceID;
                    }
                    else if (obj.GetType().Name == "ASPxGridView")
                    {
                        strDataSourceID = obj.GetType().GetProperty("DataSourceID").GetValue(obj, null).ToString();
                    }
                    foreach (Control ctrl in this.Page.Form.Controls)
                    {
                        if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == strDataSourceID)
                        {
                            ds = (WebDataSource)ctrl;
                        }
                    }
                }
                if (ds == null)
                {
                    throw new Exception("Can't find datasource of binding object");
                }

                wcq.DataSourceID = ds.ID;
                wcq.GapVertical = 4;
                int columnindex = 0;
                int index = 0;
                string[] arrQueryText = null;
                if (this.ViewState["QueryTemp"] != null)
                {
                    string strQueryText = this.ViewState["QueryTemp"].ToString();
                    arrQueryText = strQueryText.Split(';');
                }
                wcq.KeepCondition = this.QueryKeepConditon;
                foreach (WebQueryField wqf in this.QueryFields)
                {
                    WebQueryColumns wqc = new WebQueryColumns(wqf.Name, true, wqf.FieldName, wqf.Caption, 120, "ClientQuery" + wqf.Mode + "Column", "And", wqf.Condition, "Left");
                    wqc.DefaultValue = wqf.DefaultValue;
                    wqc.IsNvarChar = wqf.IsNvarChar;
                    if (columnindex == columncount)
                    {
                        wqc.NewLine = true;
                        columnindex = 1;
                    }
                    else
                    {
                        wqc.NewLine = false;
                        columnindex++;
                    }

                    if (arrQueryText != null)
                    {
                        wqc.Text = arrQueryText[index];
                        index++;
                    }
                    if (wqf.Condition == "")
                    {
                        Type tp = ds.InnerDataSet.Tables[ds.DataMember].Columns[wqf.FieldName].DataType;
                        if (tp == typeof(string))
                        {
                            wqc.Operator = "%";
                        }
                        else
                        {
                            wqc.Operator = "=";
                        }
                    }
                    if (wqf.Mode == "")
                    {
                        wqc.ColumnType = "ClientQueryTextBoxColumn";
                    }

                    if (wqf.Mode == "RefVal" || wqf.Mode == "ComboBox")
                    {
                        wqc.WebRefVal = wqf.RefVal;
                    }
                    else if (wqf.Mode == "RefButton")
                    {
                        wqc.WebRefButton = wqf.RefVal;
                    }
                    wcq.Columns.Add(wqc);
                }
            }
            else
            {
                throw new Exception("No QueryFields in WebNavigator");
            }
            this.Page.Form.Controls.Add(wcq);    //为了使用Wcq的Page属性
            return wcq;

        }

        private WebAnyQuery CopyAnyQueryFileds(int columncount, bool newpage)
        {
            WebAnyQuery waq = new WebAnyQuery();

            if (newpage)
            {
                waq.ID = this.ID + "QueryTemp";
            }
            else
            {
                object objQuery = this.GetObjByID(this.ID + "QueryTempPanel");
                if (objQuery != null)
                {
                    waq = objQuery as WebAnyQuery;
                    return waq;
                }
                waq.ID = this.ID + "QueryTempPanel";

            }
            if (this.QueryFields.Count > 0)
            {
                WebDataSource ds = null;
                object obj = null;
                if (this.ViewBindingObject != null && this.ViewBindingObject != "")
                {
                    obj = this.GetObjByID(this.ViewBindingObject);
                }
                else if (this.BindingObject != null && this.BindingObject != "")
                {
                    obj = this.GetObjByID(this.BindingObject);
                }
                if (obj != null && obj is CompositeDataBoundControl)
                {
                    CompositeDataBoundControl dataControl = (CompositeDataBoundControl)obj;
                    foreach (Control ctrl in this.Page.Form.Controls)
                    {
                        if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == dataControl.DataSourceID)
                        {
                            ds = (WebDataSource)ctrl;
                        }
                    }
                }
                if (ds == null)
                {
                    throw new Exception("Can't find datasource of binding object");
                }

                waq.DataSourceID = ds.ID;

                int index = 0;
                string[] arrQueryText = null;
                if (this.ViewState["QueryTemp"] != null)
                {
                    string strQueryText = this.ViewState["QueryTemp"].ToString();
                    arrQueryText = strQueryText.Split(';');
                }

                foreach (WebQueryField wqf in this.QueryFields)
                {
                    WebAnyQueryColumns wqc = new WebAnyQueryColumns(wqf.Name, wqf.FieldName, wqf.Caption, 120, "AnyQuery" + wqf.Mode + "Column", wqf.Condition);
                    wqc.DefaultValue = wqf.DefaultValue;

                    if (arrQueryText != null)
                    {
                        wqc.Text = arrQueryText[index];
                        index++;
                    }
                    if (wqf.Condition == "")
                    {
                        Type tp = ds.InnerDataSet.Tables[ds.DataMember].Columns[wqf.FieldName].DataType;
                        if (tp == typeof(string))
                        {
                            wqc.Operator = "%";
                        }
                        else
                        {
                            wqc.Operator = "=";
                        }
                    }
                    if (wqf.Mode == "")
                    {
                        wqc.ColumnType = "AnyQueryTextBoxColumn";
                    }

                    if (wqf.Mode == "RefVal" || wqf.Mode == "ComboBox")
                    {
                        wqc.WebRefVal = wqf.RefVal;
                    }
                    else if (wqf.Mode == "RefButton")
                    {
                        wqc.WebRefButton = wqf.RefVal;
                    }

                    waq.Columns.Add(wqc);

                    waq.AllowAddQueryField = this.AllowAddQueryField;
                    waq.AnyQueryID = this.AnyQueryID;
                    waq.MaxColumnCount = this.MaxColumnCount;
                    waq.QueryColumnMode = this.QueryColumnMode;
                    waq.DisplayAllOperator = this.DisplayAllOperator;
                }
            }
            else
            {
                throw new Exception("No QueryFields in WebNavigator");
            }
            this.Page.Form.Controls.Add(waq);    //为了使用Wcq的Page属性
            return waq;
        }

        public void Show(Panel pn)
        {
            this.Show(pn, 1);
        }

        public void Show(Panel pn, int Columns)
        {
            if (Columns < 1)
            {
                throw new Exception("Parameter Columns of WebNavigator.Show() should larger than 1");
            }
            WebClientQuery wcq = this.CopyQueryFileds(Columns);
            wcq.Show(pn);
        }

        public void Clear(Panel pn)
        {
            WebClientQuery wcq = this.CopyQueryFileds(1);
            wcq.Get_isShow().Add(pn.ID);
            wcq.Clear(pn);
        }

        public string GetWhere(Panel pn)
        {
            WebClientQuery wcq = this.CopyQueryFileds(1);
            wcq.Get_isShow().Add(pn.ID);
            return wcq.GetWhere(pn);
        }

        public void Execute(Panel pn)
        {
            string strwhere = GetWhere(pn);
            NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strwhere);
            OnQueryWhere(args);
            if (!args.Cancel)
            {
                strwhere = args.WhereString;
                WebDataSource ds = null;
                object obj = null;
                if (this.ViewBindingObject != null && this.ViewBindingObject != "")
                {
                    obj = this.GetObjByID(this.ViewBindingObject);
                }
                else if (this.BindingObject != null && this.BindingObject != "")
                {
                    obj = this.GetObjByID(this.BindingObject);
                }
                if (obj != null && obj is CompositeDataBoundControl)
                {
                    CompositeDataBoundControl dataControl = (CompositeDataBoundControl)obj;
                    foreach (Control ctrl in this.Page.Form.Controls)
                    {
                        if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == dataControl.DataSourceID)
                        {
                            ds = (WebDataSource)ctrl;
                        }
                    }
                }
                ds.SetWhere(strwhere);
                this.Page.DataBind();
            }
        }

        public string GetWhereText(Panel pn)
        {
            WebClientQuery wcq = this.CopyQueryFileds(1);
            wcq.Get_isShow().Add(pn.ID);
            wcq.GetWhere(pn);
            return QueryTranslate.Translate(wcq);
        }

        public string GetWhereText()
        {
            WebClientQuery wcq = this.CopyQueryFileds(1);
            return QueryTranslate.Translate(wcq);
        }

        public bool CheckInsertedNotApply(object obj, int type)
        {
            WebDataSource source = null;

            if (type == 1)  //GridView
            {
                WebGridView gridView = (WebGridView)obj;
                string sourceID = gridView.DataSourceID;
                if (sourceID.Length != 0)
                {
                    source = (WebDataSource)(gridView.GetObjByID(sourceID));
                }
                else
                {
                    return true;
                }
            }
            else if (type == 2)  //DetailsView
            {
                WebDetailsView detailsView = (WebDetailsView)obj;
                string sourceID = detailsView.DataSourceID;
                if (sourceID.Length != 0)
                {
                    source = (WebDataSource)(detailsView.GetObjByID(sourceID));
                }
                else
                {
                    return true;
                }
            }
            else    //FormView
            {
                WebFormView formView = (WebFormView)obj;
                string sourceID = formView.DataSourceID;
                if (sourceID.Length != 0)
                {
                    source = (WebDataSource)(formView.GetObjByID(sourceID));
                }
                else
                {
                    return true;
                }
            }

            if (source != null && source.AutoApply)
            {
                DataSet dataset = source.InnerDataSet;
                //modified by ccm 和webdatasourceView.ExecuteInsert逻辑统一
                bool changed = dataset.GetChanges() != null;
                if (source.AutoApplyForInsert)
                {
                    return !changed;
                }
                else
                {
                    foreach (Control control in source.Parent.Controls)
                    {
                        if (control is WebDataSource && (control as WebDataSource).MasterDataSource == source.ID)
                        {
                            return true;
                        }
                    }
                    return !changed;
                }
            }

            return true;
        }

        public void PerformApply()
        {
            object obj = GetObjByID(this.BindingObject);
            WebGridView gridView = new WebGridView();
            WebDetailsView detailView = new WebDetailsView();
            WebFormView formView = new WebFormView();
            WebDataSource ds = new WebDataSource();
            if (this.ShowDataStyle == NavigatorStyle.GridStyle && obj is WebGridView)
            {
                gridView = (WebGridView)obj;
                ds = (WebDataSource)GetObjByID(gridView.DataSourceID);
                foreach (GridViewRow row in gridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                        {
                            gridView.UpdateRow(row.RowIndex, false);
                        }
                    }
                    if (gridView.ShowFooter)
                    {
                        doOkForGridInsert(gridView, ds);
                    }
                }
                ds.ApplyUpdates();
            }
            else if (this.ShowDataStyle == NavigatorStyle.DetailStyle && obj is WebDetailsView)
            {
                detailView = (WebDetailsView)obj;
                if (detailView.CurrentMode == DetailsViewMode.Insert)
                    detailView.InsertItem(false);
                else if (detailView.CurrentMode == DetailsViewMode.Edit)
                    detailView.UpdateItem(false);
                detailView.ChangeMode(DetailsViewMode.ReadOnly);
                ds.ApplyUpdates();
            }
            else if (this.ShowDataStyle == NavigatorStyle.FormViewStyle && obj is WebFormView)
            {
                formView = (WebFormView)obj;
                if (formView.CurrentMode == FormViewMode.Insert)
                    formView.InsertItem(false);
                else if (formView.CurrentMode == FormViewMode.Edit)
                    formView.UpdateItem(false);

                ds.ApplyUpdates();
                if (formView.CurrentMode == FormViewMode.Insert
                    || formView.CurrentMode == FormViewMode.Edit)
                {
                    formView.ChangeMode(formView.DefaultMode);
                }
            }
        }

        private void doOkForGridInsert(WebGridView gridView, WebDataSource ds)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                string fieldName;
                if (gridView.Columns[i] is BoundField)
                {
                    fieldName = ((BoundField)gridView.Columns[i]).DataField;
                    if (!((BoundField)gridView.Columns[i]).ReadOnly)
                    {
                        TextBox txt = (TextBox)gridView.FooterRow.FindControl("InfoTextBox" + fieldName);
                        if (txt != null)
                        {
                            if (txt.Text == "")
                            {
                                ht.Add(fieldName, DBNull.Value);
                            }
                            else
                            {
                                ht.Add(fieldName, txt.Text);
                            }
                        }
                    }
                    else
                    {
                        Label lbl = (Label)gridView.FooterRow.FindControl("InfoLabel" + fieldName);
                        if (lbl != null)
                        {
                            if (lbl.Text == "")
                            {
                                ht.Add(fieldName, DBNull.Value);
                            }
                            else
                            {
                                ht.Add(fieldName, lbl.Text);
                            }
                        }
                    }
                }
                else if (gridView.Columns[i] is TemplateField)
                {
                    fieldName = gridView.Columns[i].SortExpression;
                    object strValue = GetValue(fieldName, gridView);
                    if (strValue == null || strValue.ToString() == "")
                    //modified by lily 2007/4/12 如果多个Templatefield，且没有设定SortExpression，会报错。
                    {
                        if (fieldName != null && fieldName != "")
                            ht.Add(fieldName, DBNull.Value);
                    }
                    else
                        ht.Add(fieldName, strValue);
                }
            }
            WebValidate validate = (WebValidate)gridView.ExtendedFindChildControl(gridView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            if (validate != null)
            {
                validate.Text = string.Empty;
                if (ds.PrimaryKey.Length > 0)
                {
                    object[] value = new object[ds.PrimaryKey.Length];
                    for (int i = 0; i < ds.PrimaryKey.Length; i++)
                    {
                        string columnName = ds.PrimaryKey[i].ColumnName;
                        if (ht.ContainsKey(columnName))
                        {
                            value[i] = ht[columnName];
                        }
                        else if (ds.RelationValues != null && ds.RelationValues.Contains(columnName))
                        {
                            value[i] = ds.RelationValues[columnName];
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, ds.GetType(), ds.ID, columnName, null);
                        }

                    }
                    if (!validate.CheckDuplicate(ds, value))
                    {
                        gridView.ValidateFailed = true;
                        return;
                    }
                }
                bool validateSucess = validate.CheckValidate(ht);
                if (validateSucess)
                {
                    gridView.ValidateFailed = false;
                    ds.Insert(ht);
                    gridView.GridInserting = false;
                    gridView.OnAfterInsert(EventArgs.Empty);
                    if (!gridView.TotalActive)
                        gridView.ShowFooter = false;
                    else
                        gridView.DataBind();
                }
                else
                {
                    gridView.ValidateFailed = true;
                    gridView.ValidateFormat();
                }

                if (gridView.ValidateFailed)
                    return;
            }
            else
            {
                ds.Insert(ht);
                gridView.GridInserting = false;
                gridView.OnAfterInsert(EventArgs.Empty);
                if (!gridView.TotalActive)
                    gridView.ShowFooter = false;
                else
                    gridView.DataBind();
            }
            WebNavigator bindingNav = gridView.GetBindingNavigator();
            if (bindingNav != null && bindingNav != this)
            {
                if (ds.AutoApply)
                {
                    bindingNav.SetState(WebNavigator.NavigatorState.ApplySucess);
                    bindingNav.SetNavState("Browsed");
                }
                else
                {
                    bindingNav.SetState(WebNavigator.NavigatorState.Changed);
                    bindingNav.SetNavState("Changing");
                }
            }
            if (ds.AutoApply)
            {
                this.SetState(WebNavigator.NavigatorState.ApplySucess);
                this.SetNavState("Browsed");
            }
            else
            {
                this.SetState(WebNavigator.NavigatorState.Changed);
                this.SetNavState("Changing");
            }
        }

        private object GetValue(string fieldName, WebGridView gridView)
        {
            object strValue = null;
            Control ctrl = new Control();
            bool HasFoundControl = false;
            foreach (AddNewRowControlItem ai in gridView.AddNewRowControls)
            {
                if (ai.FieldName == fieldName)
                {
                    ctrl = gridView.FooterRow.FindControl(ai.ControlID);
                    switch (ai.ControlType)
                    {
                        case WebGridView.AddNewRowControlType.TextBox:
                            strValue = ((TextBox)ctrl).Text;
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.HtmlInputText:
                            strValue = ((HtmlInputText)ctrl).Value;
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.Label:
                            strValue = ((Label)ctrl).Text;
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.DropDownList:
                            strValue = ((DropDownList)ctrl).SelectedValue;
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.CheckBox:
                            strValue = ((CheckBox)ctrl).Checked;
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.RefVal:
                            strValue = ((WebRefValBase)ctrl).BindingValue;
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.DateTimePicker:
                            IDateTimePicker dtp = (IDateTimePicker)ctrl;
                            if (dtp.DateTimeType == dateTimeType.VarChar)
                            {
                                strValue = dtp.DateString;
                            }
                            else
                            {
                                strValue = dtp.Text;
                            }
                            HasFoundControl = true;
                            break;
                        case WebGridView.AddNewRowControlType.WebImage:
                            {
                                if ((ctrl as WebImage).ImageStyle == WebImage.WebImageStyle.ImageField)
                                {
                                    strValue = ((WebImage)ctrl).FileBytes;
                                }
                                else
                                {
                                    strValue = ((WebImage)ctrl).ImageUrl;
                                }
                                HasFoundControl = true;
                                break;
                            }
                        case WebGridView.AddNewRowControlType.WebListBoxList:
                            {
                                strValue = ((WebListBoxList)ctrl).Text;
                                HasFoundControl = true;
                                break;
                            }
                        case WebGridView.AddNewRowControlType.WebCheckBox:
                            {
                                strValue = ((WebCheckBox)ctrl).CheckBinding;
                                HasFoundControl = true;
                                break;
                            }
                        case WebGridView.AddNewRowControlType.HiddenField:
                            {
                                strValue = ((HiddenField)ctrl).Value;
                                HasFoundControl = true;
                                break;
                            }
                    }
                }
            }
            if (!HasFoundControl)
            {
                TextBox txt = (TextBox)gridView.FooterRow.FindControl("InfoTextBox" + fieldName.ToLower());
                if (txt != null)
                {
                    strValue = txt.Text;
                }
            }
            return strValue;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                SetState(NavigatorState.Browsing);
                this.SetNavState("Browsed");
                if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["ClientQueryID"] == this.ID + "QueryTemp")
                {
                    this.ViewState["QueryTemp"] = Page.Request.QueryString["Querytext"];
                }
            }
        }

        public enum NavigatorState
        {
            Editing,
            Inserting,
            Browsing,
            Changed,
            ApplySucess,
            ApplyFail,
            Aborted
        }

        public void SetState(NavigatorState state)
        {
            if (this.LinkLable != null && this.LinkLable != "")
            {
                object obj = this.GetObjByID(this.LinkLable);
                if (obj != null && obj is Label)
                {
                    language = CliSysMegLag.GetClientLanguage();
                    language = CliUtils.fClientLang;
                    String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebNavigator", "StatesName", true);
                    string[] states = message.Split(';');
                    switch (state)
                    {
                        case NavigatorState.Editing:
                            ((Label)obj).Text = states[0];
                            break;
                        case NavigatorState.Inserting:
                            ((Label)obj).Text = states[1];
                            break;
                        case NavigatorState.Browsing:
                            ((Label)obj).Text = states[2];
                            break;
                        case NavigatorState.Changed:
                            ((Label)obj).Text = states[3];
                            break;
                        case NavigatorState.ApplySucess:
                            ((Label)obj).Text = states[4];
                            break;
                        case NavigatorState.ApplyFail:
                            ((Label)obj).Text = states[5];
                            break;
                        case NavigatorState.Aborted:
                            ((Label)obj).Text = states[6];
                            break;
                    }
                }
            }
            if (this.StatusStrip != null && this.StatusStrip != "")
            {
                object obj = this.GetObjByID(this.StatusStrip);
                if (obj != null && obj is WebStatusStrip)
                {
                    //language = CliSysMegLag.GetClientLanguage();
                    language = CliUtils.fClientLang;
                    String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebNavigator", "StatesName", true);
                    string[] states = message.Split(';');
                    switch (state)
                    {
                        case NavigatorState.Editing:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[0];
                            break;
                        case NavigatorState.Inserting:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[1];
                            break;
                        case NavigatorState.Browsing:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[2];
                            break;
                        case NavigatorState.Changed:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[3];
                            break;
                        case NavigatorState.ApplySucess:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[4];
                            break;
                        case NavigatorState.ApplyFail:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[5];
                            break;
                        case NavigatorState.Aborted:
                            ((WebStatusStrip)obj).NavigatorStatusText = states[6];
                            break;
                    }
                    ((WebStatusStrip)obj).CountViewID = string.IsNullOrEmpty(this.ViewBindingObject) ? this.BindingObject : this.ViewBindingObject;
                }
            }


            this.State = state;
            object o = this.GetObjByID(this.BindingObject);
            if (o != null)
            {
                if (o is WebGridView)
                {
                    WebGridView gridView = (WebGridView)o;
                    WebGridView detailControl = DetailControl(gridView);
                    if (state == NavigatorState.Changed)
                    {
                        gridView.DataHasChanged = true;
                        if (detailControl != null)
                        {
                            detailControl.DataHasChanged = true;
                        }
                    }
                    else if (state == NavigatorState.Aborted || state == NavigatorState.ApplySucess)
                    {
                        gridView.DataHasChanged = false;
                        if (detailControl != null)
                        {
                            detailControl.DataHasChanged = false;
                        }
                    }
                }
                else if (o is WebDetailsView)
                {
                    WebDetailsView detailsView = (WebDetailsView)o;
                    WebGridView detailControl = DetailControl(detailsView);
                    if (state == NavigatorState.Changed)
                    {
                        detailsView.DataHasChanged = true;
                        if (detailControl != null)
                        {
                            detailControl.DataHasChanged = true;
                        }
                    }
                    else if (state == NavigatorState.Aborted || state == NavigatorState.ApplySucess)
                    {
                        detailsView.DataHasChanged = false;
                        if (detailControl != null)
                        {
                            detailControl.DataHasChanged = false;
                        }
                    }
                }
                else if (o is WebFormView)
                {
                    WebFormView formView = (WebFormView)o;
                    WebGridView detailControl = DetailControl(formView);
                    if (state == NavigatorState.Changed)
                    {
                        formView.DataHasChanged = true;
                        if (detailControl != null)
                        {
                            detailControl.DataHasChanged = true;
                        }
                    }
                    else if (state == NavigatorState.Aborted || state == NavigatorState.ApplySucess)
                    {
                        formView.DataHasChanged = false;
                        if (detailControl != null)
                        {
                            detailControl.DataHasChanged = false;
                        }
                    }
                }
            }
        }

        public virtual void SetNavState(string StateText)
        {
            if (this.ID == "InPageNavigatorForAddAndQuery" || this.ID == "InPageNavigatorForOKAndCancel")
                return;
            InnerSetNavState(StateText);
            this.ViewState["CurrentNavState"] = StateText;
        }

        protected virtual void InnerSetNavState(string StateText)
        {
            foreach (WebNavigatorStateItem stateItem in this.NavStates)
            {
                if (stateItem.StateText == StateText)
                {
                    List<string> States = new List<string>(new string[] { "Normal", "Insert", "Modify", "Inquery", "Prepare", "PreInsert", "InquerySingle", "InqueryMulti", "PrepareSingle", "PrepareMulti" });
                    if (States.Contains(CurrentNavState))
                        this.ViewState["PreFLState"] = CurrentNavState;
                    this.ViewState["PreviousNavState"] = CurrentNavState;
                    this.ViewState["CurrentNavState"] = StateText;
                    break;
                }
            }
        }

        [Browsable(false)]
        public string CurrentNavState
        {
            get
            {
                object obj = this.ViewState["CurrentNavState"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
        }

        private WebGridView DetailControl(DataBoundControl MasterControl)
        {
            WebGridView gdView = null;
            object obj = this.GetObjByID(MasterControl.DataSourceID);
            if (obj != null && obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                WebDataSource wdsDetail = null;
                wdsDetail = (WebDataSource)this.ExtendedFindChildControl(wds.ID, FindControlType.MasterDataSource, typeof(WebDataSource));
                if (wdsDetail != null)
                {
                    gdView = (WebGridView)this.ExtendedFindChildControl(wdsDetail.ID, FindControlType.DataSourceID, typeof(WebGridView));
                }
            }
            return gdView;
        }

        #region Properties
        private WebQueryFiledsCollection _QueryFields;
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Category("Infolight"),
         Description("Specifies the amount of fields which are going to be shown in the query page")]
        public WebQueryFiledsCollection QueryFields
        {
            get
            {
                return _QueryFields;
            }
        }

        public enum QueryModeType
        {
            Normal,
            ClientQuery,
            AnyQuery
        }

        private QueryModeType _QueryMode;
        [Category("Infolight"),
        Description("The mode of query after the query button clicked")]
        [DefaultValue(typeof(QueryModeType), "Normal")]
        public QueryModeType QueryMode
        {
            get { return _QueryMode; }
            set { _QueryMode = value; }
        }

        private bool _DisplayAllOperator;
        [Category("Infolight"), DefaultValue(false)]
        public bool DisplayAllOperator
        {
            get
            {
                return _DisplayAllOperator;
            }
            set
            {
                _DisplayAllOperator = value;
            }
        }

        private AnyQueryColumnMode _QueryColumnMode = AnyQueryColumnMode.ByBindingSource;
        [Category("Infolight"), DefaultValue(AnyQueryColumnMode.ByBindingSource)]
        public AnyQueryColumnMode QueryColumnMode
        {
            get
            {
                return _QueryColumnMode;
            }
            set
            {
                _QueryColumnMode = value;
            }
        }

        public enum QueryStyleType
        {
            NewPage,
            Dialog
        }

        private QueryStyleType _QueryStyle;

        [DefaultValue(typeof(QueryStyleType), "NewPage")]
        public QueryStyleType QueryStyle
        {
            get { return _QueryStyle; }
            set { _QueryStyle = value; }
        }

        public enum MoveMode
        {
            PageMode,
            RowMode
        }

        private MoveMode _gridViewMoveMode;

        [DefaultValue(typeof(MoveMode), "PageMode")]
        public MoveMode GridViewMoveMode
        {
            get { return _gridViewMoveMode; }
            set { _gridViewMoveMode = value; }
        }

        public bool _QueryKeepCondition;
        [Category("Infolight"),
        Description("Indicates whether the text will be cleared after excute query")]
        [DefaultValue(false)]
        public bool QueryKeepConditon
        {
            get
            {
                return _QueryKeepCondition;
            }
            set
            {
                _QueryKeepCondition = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(typeof(NavigatorState), "Browsing")]
        public NavigatorState State
        {
            get
            {
                object obj = this.ViewState["State"];
                if (obj != null)
                {
                    return (NavigatorState)obj;
                }
                return NavigatorState.Browsing;
            }
            set
            {
                this.ViewState["State"] = value;
            }
        }

        private WebNavigatorStateCollection _navStates;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        public WebNavigatorStateCollection NavStates
        {
            get
            {
                return _navStates;
            }
        }

        private string _BindingDevObject;
        [Category("Infolight"),
        Description("Specifies the control to bind to")]
        [Editor(typeof(GridEditor), typeof(UITypeEditor))]
        public string BindingDevObject
        {
            get
            {
                return _BindingDevObject;
            }
            set
            {
                _BindingDevObject = value;
            }
        }

        private string _BindingObject;
        [Category("Infolight"),
        Description("Specifies the control to bind to")]
        [Editor(typeof(GridEditor), typeof(UITypeEditor))]
        public string BindingObject
        {
            get
            {
                return _BindingObject;
            }
            set
            {
                _BindingObject = value;
            }
        }

        private string _ViewBindingObject;
        [Category("Infolight"),
        Description("Specifies the control of view to bind to")]
        [Editor(typeof(ViewGridEditor), typeof(UITypeEditor))]
        public string ViewBindingObject
        {
            get
            {
                return _ViewBindingObject;
            }
            set
            {
                _ViewBindingObject = value;
            }
        }

        private string _LinkLable;
        [Category("Infolight"),
        Description("Specifies the label to display the status of the WebNavigator")]
        [Editor(typeof(LinkLabelEditor), typeof(UITypeEditor))]
        public string LinkLable
        {
            get
            {
                return _LinkLable;
            }
            set
            {
                _LinkLable = value;
            }
        }

        private string _StatusStrip;
        [Category("Infolight"),
        Description("StatusStrip to display the status of WebNavigator")]
        [Editor(typeof(StatusStripEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string StatusStrip
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public enum NavigatorStyle
        {
            GridStyle,
            DetailStyle,
            FormViewStyle,
            ASPxGridView
        }

        private NavigatorStyle _ShowDataStyle;
        [Category("Infolight"),
        Description("Specifies the type of BindingObject")]
        public NavigatorStyle ShowDataStyle
        {
            get
            {
                return _ShowDataStyle;
            }
            set
            {
                _ShowDataStyle = value;
            }
        }

        private ControlsCollection _NavControls;
        [Category("Infolight"),
        Description("Specifies the control to bind to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public ControlsCollection NavControls
        {
            get
            {
                return _NavControls;
            }
        }

        [Category("Infolight")]
        [Description("Specifies the size of the NavControls")]
        [DefaultValue(25)]
        public int ControlsSize
        {
            get
            {
                object obj = this.ViewState["ControlsSize"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 25;
            }
            set
            {
                this.ViewState["ControlsSize"] = value;
                foreach (ControlItem item in this.NavControls)
                {
                    item.Size = value;
                }
            }
        }

        public enum CtrlType
        {
            Button, HyperLink, Image
        }

        private CtrlType _ControlType;
        [Category("Infolight")]
        [Description("Specifies the type of the NavControls")]
        [DefaultValue(typeof(CtrlType), "Image")]
        public CtrlType ControlType
        {
            get
            {
                return _ControlType;
            }
            set
            {
                _ControlType = value;
                switch (_ControlType)
                {
                    case CtrlType.Button:
                        foreach (ControlItem item in _NavControls)
                        {
                            item.ControlType = CtrlType.Button;
                            item.Size = 40;
                        }
                        break;
                    case CtrlType.Image:
                        foreach (ControlItem item in _NavControls)
                        {
                            item.ControlType = CtrlType.Image;
                            item.Size = 25;
                        }
                        break;
                    case CtrlType.HyperLink:
                        foreach (ControlItem item in _NavControls)
                        {
                            item.ControlType = CtrlType.HyperLink;
                            item.Size = 45;
                        }
                        break;
                }
            }
        }

        private bool _GetServerText;
        [Category("Infolight"),
         Description("Indicates whether the caption of inner WebNavigator's items use server settings automatically")]
        [DefaultValue(true)]
        public bool GetServerText
        {
            get
            {
                return _GetServerText;
            }
            set
            {
                _GetServerText = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether to alert when user is going to delete the data"),
        DefaultValue(true)]
        public bool SureDelete
        {
            get
            {
                object obj = this.ViewState["SureDelete"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["SureDelete"] = value;
            }
        }

        [Category("Infolight")]
        [Description("Specifies the distance between NavControls")]
        [DefaultValue(0)]
        public int ControlsGap
        {
            get
            {
                object obj = this.ViewState["ControlsGap"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 0;
            }
            set
            {
                this.ViewState["ControlsGap"] = value;
            }
        }
        #endregion

        private static readonly object EventBeforeCommand = new object();

        protected virtual void OnBeforeCommand(BeforeCommandArgs e)
        {
            BeforeCommandEventHandler clickHandler = (BeforeCommandEventHandler)Events[EventBeforeCommand];
            if (clickHandler != null)
            {
                clickHandler(this, e);
            }
        }

        [Category("Action"),
        Description("OnBeforeCommand event")]
        public event BeforeCommandEventHandler BeforeCommand
        {
            add { Events.AddHandler(EventBeforeCommand, value); }
            remove { Events.RemoveHandler(EventBeforeCommand, value); }
        }

        private static readonly object EventCommand = new object();

        protected virtual void OnCommand(CommandEventArgs e)
        {
            WebNavigatorCommand(e);
            CommandEventHandler clickHandler = (CommandEventHandler)Events[EventCommand];
            if (clickHandler != null)
            {
                clickHandler(this, e);
            }
        }

        [Category("Action"),
        Description("OnCommand event")]
        public event CommandEventHandler Command
        {
            add { Events.AddHandler(EventCommand, value); }
            remove { Events.RemoveHandler(EventCommand, value); }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            BeforeCommandArgs bArgs = new BeforeCommandArgs(eventArgument, null);
            OnBeforeCommand(bArgs);
            if (!bArgs.Cancel)
                OnCommand(new CommandEventArgs(eventArgument, null));
        }

        private object EventQueryWhere = new object();
        [Category("Infolight"),
        Description("The event ocured before query")]
        public event NavigatorQueryWhereEventHandler QueryWhere
        {
            add
            {
                Events.AddHandler(EventQueryWhere, value);
            }
            remove
            {
                Events.RemoveHandler(EventQueryWhere, value);
            }
        }

        private bool _MultiLanguage;
        [Category("Infolight")]
        [DefaultValue(false)]
        public bool MultiLanguage
        {
            get
            {
                return _MultiLanguage;
            }
            set
            {
                _MultiLanguage = value;
            }
        }

        public void OnQueryWhere(NavigatorQueryWhereEventArgs value)
        {
            NavigatorQueryWhereEventHandler handler = (NavigatorQueryWhereEventHandler)Events[EventQueryWhere];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        private bool _AddDefaultControls;
        [Browsable(false)]
        [DefaultValue(true)]
        public bool AddDefaultControls
        {
            get
            {
                return _AddDefaultControls;
            }
            set
            {
                _AddDefaultControls = value;
            }
        }

        private bool _AutoDisableColumns = true;
        [Browsable(false)]
        [Category("Infolight"), DefaultValue(true)]
        public bool AutoDisableColumns
        {
            get
            {
                return _AutoDisableColumns;
            }
            set
            {
                _AutoDisableColumns = value;
            }
        }

        private String _AnyQueryID = String.Empty;
        [Category("Infolight")]
        public String AnyQueryID
        {
            get
            {
                if (_AnyQueryID == String.Empty && this.ID != null)
                    _AnyQueryID = this.ID;
                return _AnyQueryID;
            }
            set
            {
                _AnyQueryID = value;
            }
        }

        private bool _AllowAddQueryField = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool AllowAddQueryField
        {
            get
            {
                return _AllowAddQueryField;
            }
            set
            {
                _AllowAddQueryField = value;
            }
        }

        private int _MaxColumnCount = -1;
        [Category("Infolight"), DefaultValue(-1)]
        public int MaxColumnCount
        {
            get
            {
                return _MaxColumnCount;
            }
            set
            {
                _MaxColumnCount = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
#if !VS90
            if (this.NavControls.Count == 0 && AddDefaultControls)
            {
                string[] ctrlTexts = { "<<", "<", ">", ">>", "add", "update", "delete", "ok", "cancel", "apply", "abort", "query", "print", "export" };
                if (this.GetServerText)
                {
                    language = CliSysMegLag.GetClientLanguage();
                    language = CliUtils.fClientLang;
                    String message = SysMsg.GetSystemMessage(language,
                                                             "Srvtools",
                                                             "WebNavigator",
                                                             "ControlText", true);
                    ctrlTexts = message.Split(';');
                }
                // Add First Control
                ControlItem FirstItem = new ControlItem
                    ("First", ctrlTexts[0], CtrlType.Image, "../image/uipics/first.gif", "../image/uipics/first2.gif", "../image/uipics/first3.gif", 26, true);
                this.NavControls.Add(FirstItem);
                // Add Previous Control
                ControlItem PreviousItem = new ControlItem
                    ("Previous", ctrlTexts[1], CtrlType.Image, "../image/uipics/previous.gif", "../image/uipics/previous2.gif", "../image/uipics/previous3.gif", 26, true);
                this.NavControls.Add(PreviousItem);
                // Add Next Control
                ControlItem NextItem = new ControlItem
                    ("Next", ctrlTexts[2], CtrlType.Image, "../image/uipics/next.gif", "../image/uipics/next2.gif", "../image/uipics/next3.gif", 26, true);
                this.NavControls.Add(NextItem);
                // Add Last Control
                ControlItem LastItem = new ControlItem
                    ("Last", ctrlTexts[3], CtrlType.Image, "../image/uipics/last.gif", "../image/uipics/last2.gif", "../image/uipics/last3.gif", 26, true);
                this.NavControls.Add(LastItem);
                // Add Add Control
                ControlItem AddItem = new ControlItem
                    ("Add", ctrlTexts[4], CtrlType.Image, "../image/uipics/add.gif", "../image/uipics/add2.gif", "../image/uipics/add3.gif", 26, true);
                this.NavControls.Add(AddItem);
                // Add Update Control
                ControlItem UpdateItem = new ControlItem
                    ("Update", ctrlTexts[5], CtrlType.Image, "../image/uipics/edit.gif", "../image/uipics/edit2.gif", "../image/uipics/edit3.gif", 26, true);
                this.NavControls.Add(UpdateItem);
                // Add Delete Control
                ControlItem DeleteItem = new ControlItem
                    ("Delete", ctrlTexts[6], CtrlType.Image, "../image/uipics/delete.gif", "../image/uipics/delete2.gif", "../image/uipics/delete3.gif", 26, true);
                this.NavControls.Add(DeleteItem);
                // Add Ok Control
                ControlItem OKItem = new ControlItem
                    ("OK", ctrlTexts[7], CtrlType.Image, "../image/uipics/ok.gif", "../image/uipics/ok2.gif", "../image/uipics/ok3.gif", 26, true);
                this.NavControls.Add(OKItem);
                // Add Cancel Control
                ControlItem CancelItem = new ControlItem
                    ("Cancel", ctrlTexts[8], CtrlType.Image, "../image/uipics/cancel.gif", "../image/uipics/cancel2.gif", "../image/uipics/cancel3.gif", 26, true);
                this.NavControls.Add(CancelItem);
                // Add Apply Control
                ControlItem ApplyItem = new ControlItem
                    ("Apply", ctrlTexts[9], CtrlType.Image, "../image/uipics/apply.gif", "../image/uipics/apply2.gif", "../image/uipics/apply3.gif", 26, true);
                this.NavControls.Add(ApplyItem);
                // Add Abort Control
                ControlItem AbortItem = new ControlItem
                    ("Abort", ctrlTexts[10], CtrlType.Image, "../image/uipics/abort.gif", "../image/uipics/abort2.gif", "../image/uipics/abort3.gif", 26, true);
                this.NavControls.Add(AbortItem);
                // Add Query Control
                ControlItem QueryItem = new ControlItem
                    ("Query", ctrlTexts[11], CtrlType.Image, "../image/uipics/query.gif", "../image/uipics/query2.gif", "../image/uipics/query3.gif", 26, true);
                this.NavControls.Add(QueryItem);
                // Add Print Control
                ControlItem PrintItem = new ControlItem
                    ("Print", ctrlTexts[12], CtrlType.Image, "../image/uipics/print.gif", "../image/uipics/print2.gif", "../image/uipics/print3.gif", 26, true);
                this.NavControls.Add(PrintItem);
                // Add Export Control
                ControlItem ExportItem = new ControlItem
                    ("Export", ctrlTexts[13], CtrlType.Image, "../image/uipics/export.gif", "../image/uipics/export2.gif", "../image/uipics/export3.gif", 26, true);
                this.NavControls.Add(ExportItem);
                this.InitializeStates();
                RenderFlowItems(writer);
            }
#endif
            base.Render(writer);
        }

        protected virtual void InitializeStates()
        {
            #region NavigatorStates
            foreach (WebNavigatorStateItem stateItem in this.NavStates)
            {
                stateItem.EnableControls = "";
                switch (stateItem.StateText)
                {
                    case "Initial":
                    case "Browsed":
                        stateItem.EnableControls = "First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export";
                        break;
                    case "Inserting":
                    case "Editing":
                        stateItem.EnableControls = "OK;Cancel;Apply;Abort";
                        break;
                    case "Changing":
                        stateItem.EnableControls = "First;Previous;Next;Last;Add;Update;Delete;Apply;Abort;Export";
                        break;
                    case "Applying":
                    case "Querying":
                    case "Printing":
                        break;
                }
            }
            #endregion
        }

        protected virtual void RenderFlowItems(HtmlTextWriter writer) { }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);

            if (this.NavControls.Count == 0)
            {
                writer.Write(this.ID);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
                writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
                WebGridView gdView = null;
                if (this.BindingObject != null && this.BindingObject != "")
                {
                    object obj = this.GetObjByID(this.BindingObject);
                    if (obj is WebGridView)
                    {
                        gdView = (WebGridView)obj;
                    }
                }
                foreach (ControlItem ctrl in this.NavControls)
                {
                    createButton(writer, ctrl, gdView);
                }
                writer.RenderEndTag(); // </tr>
                writer.RenderEndTag(); // </table>
            }
        }

        private bool GetDataSourceAllowProperty(string name)
        {
            bool allow = true;
            if (this.BindingObject != null)
            {
                DataBoundControl view = (DataBoundControl)this.FindControl(this.BindingObject);
                if (view != null)
                {
                    WebDataSource wds = (WebDataSource)this.FindControl(view.DataSourceID);
                    if (wds != null)
                    {
                        switch (name)
                        {
                            case "Add": allow = wds.AllowAdd; break;
                            case "Delete": allow = wds.AllowDelete; break;
                            case "Update": allow = wds.AllowUpdate; break;
                            case "Print": allow = wds.AllowPrint; break;
                        }
                    }
                }
            }
            return allow;
        }

        protected virtual bool getUserSetEnabled(string ctrlName, string navState)
        {
            if (this.ID == "InPageNavigatorForAddAndQuery" || this.ID == "InPageNavigatorForOKAndCancel")
                return true;
            foreach (WebNavigatorStateItem item in this.NavStates)
            {
                if (item.StateText == navState)
                {
                    if (string.IsNullOrEmpty(item.EnableControls))
                    {
                        return IsControlEnabled(ctrlName);
                    }
                    else
                    {
                        return (item.EnableControls.IndexOf(ctrlName) != -1);
                    }
                }
            }
            return false;
        }

        private bool IsControlEnabled(string ctrlName)
        {
            bool enable = true;
            #region set default
            if (ctrlName == "First" || ctrlName == "Previous" || ctrlName == "Next" || ctrlName == "Last" || ctrlName == "Add" || ctrlName == "Update" || ctrlName == "Delete" || ctrlName == "Export")
            {
                switch (this.CurrentNavState)
                {
                    case "Initial":
                    case "Browsed":
                    case "Changing":
                        enable = true;
                        break;
                    case "Inserting":
                    case "Editing":
                    case "Applying":
                    case "Querying":
                    case "Printing":
                        enable = false;
                        break;
                }
                if (enable)
                {
                    enable = GetDataSourceAllowProperty(ctrlName);
                }
            }
            else if (ctrlName == "OK" || ctrlName == "Cancel")
            {
                switch (this.CurrentNavState)
                {
                    case "Inserting":
                    case "Editing":
                        enable = true;
                        break;
                    case "Initial":
                    case "Browsed":
                    case "Applying":
                    case "Changing":
                    case "Querying":
                    case "Printing":
                        enable = false;
                        break;
                }
            }
            else if (ctrlName == "Apply" || ctrlName == "Abort")
            {
                switch (this.CurrentNavState)
                {
                    case "Inserting":
                    case "Editing":
                    case "Changing":
                        enable = true;
                        break;
                    case "Initial":
                    case "Browsed":
                    case "Applying":
                    case "Querying":
                    case "Printing":
                        enable = false;
                        break;
                }
            }
            else if (ctrlName == "Query" || ctrlName == "Print")
            {
                switch (this.CurrentNavState)
                {
                    case "Initial":
                    case "Browsed":
                        enable = true;
                        break;
                    case "Inserting":
                    case "Editing":
                    case "Changing":
                    case "Applying":
                    case "Querying":
                    case "Printing":
                        enable = false;
                        break;
                }
                if (enable)
                {
                    enable = GetDataSourceAllowProperty(ctrlName);
                }
            }
            #endregion
            return enable;
        }

        protected virtual void createButton(HtmlTextWriter writer, ControlItem ctrl, WebGridView gdView)
        {
            string command = "cmd" + ctrl.ControlName, text = ctrl.ControlText, imageUrl = ctrl.ImageUrl, mouseOverImageUrl = ctrl.MouseOverImageUrl, disenableImageUrl = ctrl.DisenableImageUrl;
            int size = ctrl.Size;
            bool IsVisible = ctrl.ControlVisible;
            bool IsEnable = this.DesignMode ? true : getUserSetEnabled(ctrl.ControlName, this.CurrentNavState);
            CtrlType ct = ctrl.ControlType;
            ClientScriptManager csm = Page.ClientScript;

            string tooltiptext = this.AddDefaultControls ? SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebNavigator", "ControlText", true) : SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebGridView", "ControlText", true);
            String sureDelete = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebNavigator", "sureDeleteText", true);
            string[] arrtext = tooltiptext.Split(';');
            if (arrtext.Length == 6)
            {
                switch (ctrl.ControlName)
                {
                    case "Add":
                        if (this.GetServerText)
                        {
                            text = arrtext[0];
                            tooltiptext = arrtext[0];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Query":
                        if (this.GetServerText)
                        {
                            text = arrtext[1];
                            tooltiptext = arrtext[1];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Apply":
                        if (this.GetServerText)
                        {
                            text = arrtext[2];
                            tooltiptext = arrtext[2];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Abort":
                        if (this.GetServerText)
                        {
                            text = arrtext[3];
                            tooltiptext = arrtext[3];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "OK":
                        if (this.GetServerText)
                        {
                            text = arrtext[4];
                            tooltiptext = arrtext[4];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Cancel":
                        if (this.GetServerText)
                        {
                            text = arrtext[5];
                            tooltiptext = arrtext[5];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                }
            }
            else if (arrtext.Length >= 14)
            {
                switch (ctrl.ControlName)
                {
                    case "First":
                        if (this.GetServerText)
                        {
                            text = arrtext[0];
                            tooltiptext = arrtext[0];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Previous":
                        if (this.GetServerText)
                        {
                            text = arrtext[1];
                            tooltiptext = arrtext[1];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Next":
                        if (this.GetServerText)
                        {
                            text = arrtext[2];
                            tooltiptext = arrtext[2];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Last":
                        if (this.GetServerText)
                        {
                            text = arrtext[3];
                            tooltiptext = arrtext[3];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Add":
                        if (this.GetServerText)
                        {
                            text = arrtext[4];
                            tooltiptext = arrtext[4];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Update":
                        if (this.GetServerText)
                        {
                            text = arrtext[5];
                            tooltiptext = arrtext[5];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Delete":
                        if (this.GetServerText)
                        {
                            text = arrtext[6];
                            tooltiptext = arrtext[6];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "OK":
                        if (this.GetServerText)
                        {
                            text = arrtext[7];
                            tooltiptext = arrtext[7];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Cancel":
                        if (this.GetServerText)
                        {
                            text = arrtext[8];
                            tooltiptext = arrtext[8];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Apply":
                        if (this.GetServerText)
                        {
                            text = arrtext[9];
                            tooltiptext = arrtext[9];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Abort":
                        if (this.GetServerText)
                        {
                            text = arrtext[10];
                            tooltiptext = arrtext[10];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Query":
                        if (this.GetServerText)
                        {
                            text = arrtext[11];
                            tooltiptext = arrtext[11];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Print":
                        if (this.GetServerText)
                        {
                            text = arrtext[12];
                            tooltiptext = arrtext[12];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    case "Export":
                        if (this.GetServerText)
                        {
                            text = arrtext[13];
                            tooltiptext = arrtext[13];
                        }
                        else
                        {
                            text = ctrl.ControlText;
                            tooltiptext = ctrl.ControlName;
                        }
                        break;
                    default:
                        text = ctrl.ControlText;
                        tooltiptext = ctrl.ControlName;
                        break;
                }
            }


            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (ct == CtrlType.Button && IsVisible)
            {

                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + command);

                if (!IsEnable)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return false;");
                }
                else
                {
                    if (ctrl.ControlName == "Add")
                    {
                        if (gdView != null && gdView.EditURL != null && gdView.EditURL != "" && !gdView.OpenEditUrlInServerMode)
                        {
                            string url = gdView.getURL(WebGridView.OpenEditMode.Insert, null);
                            if (url != "")
                                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "window.open('" + url + "','','height=" + gdView.OpenEditHeight + ",width=" + gdView.OpenEditWidth + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');return false;");
                        }
                        else
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "var button = document.getElementById('" + this.ClientID + command + "');button.disabled=true;" + csm.GetPostBackEventReference(this, command));
                            //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, command));
                        }
                    }
                    else if (ctrl.ControlName == "Delete")
                    {
                        if (this.SureDelete)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "if(confirm('" + sureDelete + "')){" + "var button = document.getElementById('" + this.ClientID + command + "');button.disabled=true;" + csm.GetPostBackEventReference(this, command) + "}");
                        }
                        else
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "var button = document.getElementById('" + this.ClientID + command + "');button.disabled=true;" + csm.GetPostBackEventReference(this, command));
                        }
                    }
                    else if (ctrl.ControlName == "Apply" || ctrl.ControlName.ToLower() == "ok" || ctrl.ControlName == "Update")
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "var button = document.getElementById('" + this.ClientID + command + "');button.disabled=true;" + csm.GetPostBackEventReference(this, command));
                        //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "var start=new Date().getTime(); while(true) if(new Date().getTime()-start>5000) break; var aaa = document.getElementById('cmdApply');aaa.disabled=true;");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, command));
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, command));
                    }
                    // render Button tag
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + size + "px; height:" + ((this.Height.Value <= 20) ? 25 : this.Height.Value) + "px; color:" + this.ForeColor.Name.Replace("ff", "#") + ";background-color:" + this.BackColor.Name.Replace("ff", "#") + ";");
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, tooltiptext);// new add by ccm
                    if (!IsEnable)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "nav" + ctrl.ControlName);
                    writer.RenderBeginTag(HtmlTextWriterTag.Button);
                    writer.Write(text);
                    writer.RenderEndTag();
                }
            }
            else if (ct == CtrlType.HyperLink && IsVisible)
            {
                if (ctrl.ControlName == "Add")
                {
                    if (gdView != null && gdView.EditURL != null && gdView.EditURL != "" && !gdView.OpenEditUrlInServerMode)
                    {
                        string url = gdView.getURL(WebGridView.OpenEditMode.Insert, null);
                        if (url != "")
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:window.open('" + url + "','','height=" + gdView.OpenEditHeight + ",width=" + gdView.OpenEditWidth + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');return false;");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + csm.GetPostBackClientHyperlink(this, command));
                    }
                }
                else if (ctrl.ControlName == "Delete")
                {
                    if (this.SureDelete)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:if(confirm('" + sureDelete + "')){" + csm.GetPostBackClientHyperlink(this, command) + "}");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + csm.GetPostBackClientHyperlink(this, command));
                    }
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + csm.GetPostBackEventReference(this, command));
                }
                // render Link tag
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + size + "px; color:" + this.ForeColor.Name.Replace("ff", "#") + ";background-color:" + this.BackColor.Name.Replace("ff", "#") + ";");
                writer.AddAttribute(HtmlTextWriterAttribute.Title, tooltiptext);// new add by ccm
                if (!IsEnable)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "nav" + ctrl.ControlName);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(text);
                writer.RenderEndTag();
            }
            else if (ct == CtrlType.Image && imageUrl != null && imageUrl != "" && IsVisible)
            {
                if (ctrl.ControlName == "Add")
                {
                    if (gdView != null && gdView.EditURL != null && gdView.EditURL != "" && !gdView.OpenEditUrlInServerMode)
                    {
                        string url = gdView.getURL(WebGridView.OpenEditMode.Insert, null);
                        if (url != "")
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "window.open('" + url + "','','height=" + gdView.OpenEditHeight + ",width=" + gdView.OpenEditWidth + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');return false;");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackClientHyperlink(this, command));
                    }
                }
                else if (ctrl.ControlName == "Delete")
                {
                    if (this.SureDelete)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "if(confirm('" + sureDelete + "')){" + csm.GetPostBackEventReference(this, command) + "}");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, command));
                    }
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, command));
                }
                // render Image tag
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + size + "px;");
                if (!IsEnable)
                {
                    if (disenableImageUrl != null && disenableImageUrl != "")
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, disenableImageUrl);
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
                        writer.AddAttribute("onmouseover", "this.src='" + mouseOverImageUrl + "'");
                        writer.AddAttribute("onmouseout", "this.src='" + imageUrl + "'");
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
                    writer.AddAttribute("onmouseover", "this.src='" + mouseOverImageUrl + "'");
                    writer.AddAttribute("onmouseout", "this.src='" + imageUrl + "'");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, tooltiptext);// new add by ccm
                writer.AddAttribute(HtmlTextWriterAttribute.Title, tooltiptext);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "nav" + ctrl.ControlName);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            writer.RenderEndTag(); // </td>
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + this.ControlsGap + "px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
        }
    }

    #region ControlsCollection class
    public class ControlsCollection : InfoOwnerCollection
    {
        public bool bInit = false;
        public ControlsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(ControlItem))
        {
            bInit = true;
        }

        public new ControlItem this[int index]
        {
            get
            {
                return (ControlItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ControlItem)
                    {
                        //原来的Collection设置为0
                        ((ControlItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ControlItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region ControlItem class
    public class ControlItem : InfoOwnerCollectionItem, IGetValues
    {
        public ControlItem()
        {
        }

        public ControlItem(string controlname, string controltext, WebNavigator.CtrlType controltype, string imageurl, string mouseoverimageurl, string disenableimageurl, int size, bool controlvisible)
        {
            _ControlName = controlname;
            _ControlText = controltext;
            _ControlType = controltype;
            _ImageUrl = imageurl;
            _MouseOverImageUrl = mouseoverimageurl;
            _DisenableImageUrl = disenableimageurl;
            _Size = size;
            _ControlVisible = controlvisible;
        }

        public override string ToString()
        {
            return _ControlName;
        }

        #region Properties
        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _ControlName;
            }
            set
            {
                _ControlName = value;
            }
        }

        private string _ControlName;
        [Category("Design"),
        NotifyParentProperty(true),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ControlName
        {
            get
            {
                return _ControlName;
            }
            set
            {
                _ControlName = value;
            }
        }

        private string _ControlText;
        [Category("Design"),
        NotifyParentProperty(true)]
        public string ControlText
        {
            get
            {
                return _ControlText;
            }
            set
            {
                _ControlText = value;
            }
        }

        private WebNavigator.CtrlType _ControlType = WebNavigator.CtrlType.Button;
        [Category("Design"),
        NotifyParentProperty(true)]
        public WebNavigator.CtrlType ControlType
        {
            get
            {
                return _ControlType;
            }
            set
            {
                _ControlType = value;
            }
        }

        private string _ImageUrl;
        [Category("Appearance"),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        NotifyParentProperty(true)]
        public string ImageUrl
        {
            get
            {
                return _ImageUrl;
            }
            set
            {
                if (value.StartsWith("~"))
                {
                    value = value.Replace("~", "..");
                }
                _ImageUrl = value;
            }
        }

        private string _MouseOverImageUrl;
        [Category("Appearance"),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        NotifyParentProperty(true)]
        public string MouseOverImageUrl
        {
            get
            {
                return _MouseOverImageUrl;
            }
            set
            {
                if (value.StartsWith("~"))
                {
                    value = value.Replace("~", "..");
                }
                _MouseOverImageUrl = value;
            }
        }

        private string _DisenableImageUrl;
        [Category("Appearance"),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        NotifyParentProperty(true)]
        public string DisenableImageUrl
        {
            get
            {
                return _DisenableImageUrl;
            }
            set
            {
                if (value.StartsWith("~"))
                {
                    value = value.Replace("~", "..");
                }
                _DisenableImageUrl = value;
            }
        }

        private int _Size = 40;
        [Category("Appearance"),
        NotifyParentProperty(true)]
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
            }
        }

        private bool _ControlVisible = true;
        [Category("Appearance"),
        NotifyParentProperty(true)]
        public bool ControlVisible
        {
            get
            {
                return _ControlVisible;
            }
            set
            {
                _ControlVisible = value;
            }
        }
        #endregion

        public string[] GetValues(string sKind)
        {
            return new string[] { "First", "Previous", "Next", "Last", "Add", "Update", "Delete", "OK", "Cancel", "Apply", "Abort", "Query", "Print", "Export" };
        }
    }
    #endregion

    public class WebQueryFiledsCollection : InfoOwnerCollection
    {
        public WebQueryFiledsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebQueryField))
        {
        }

        public DataSet DsForDD = new DataSet();

        public new WebQueryField this[int index]
        {
            get
            {
                return (WebQueryField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebQueryField)
                    {
                        //原来的Collection设置为0
                        ((WebQueryField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebQueryField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WebQueryField : InfoOwnerCollectionItem, IGetValues
    {
        public WebQueryField()
        {
            _Caption = "";
            _Mode = "";
            _Condtion = "";
            _DefaultValue = "";
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }



        private string _Caption;

        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        private string _Condtion;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Condition
        {
            get { return _Condtion; }
            set { _Condtion = value; }
        }

        private string _Mode;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        private string _DefaultValue;
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }

        private string _RefVal;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string RefVal
        {
            get { return _RefVal; }
            set { _RefVal = value; }
        }

        private string _FieldName;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
                if (this.Owner != null)
                {
                    if (Owner.GetType() == typeof(WebNavigator))
                    {
                        if (((WebNavigator)this.Owner).Site == null)
                        {
                            this.Caption = GetDDText(_FieldName);
                        }
                        else if (((WebNavigator)this.Owner).Site.DesignMode)
                        {
                            this.Caption = GetDDText(_FieldName);
                        }
                    }
                    if (Owner.GetType() == typeof(WebGridView))
                    {
                        if (((WebGridView)this.Owner).Site == null)
                        {
                            this.Caption = GridGetDDText(_FieldName);
                        }
                        else if (((WebGridView)this.Owner).Site.DesignMode)
                        {
                            this.Caption = GridGetDDText(_FieldName);
                        }
                    }
                }
            }
        }

        private string GridGetDDText(string ControlText)
        {
            DataSet Dset = ((WebQueryFiledsCollection)this.Collection).DsForDD;
            string strCaption = "";

            if (Dset.Tables.Count == 0)
            {
                WebGridView wng = (WebGridView)this.Owner;
                object obj = null;
                obj = wng.GetObjByID(wng.DataSourceID);
                if (obj is WebDataSource)
                {
                    ((WebQueryFiledsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(obj as WebDataSource, true);
                    Dset = ((WebQueryFiledsCollection)this.Collection).DsForDD;
                }
            }

            if (Dset.Tables.Count > 0)
            {
                int x = Dset.Tables[0].Rows.Count;
                for (int y = 0; y < x; y++)
                {
                    if (string.Compare(Dset.Tables[0].Rows[y]["FIELD_NAME"].ToString(), ControlText, true) == 0)//IgnoreCase
                    {
                        strCaption = Dset.Tables[0].Rows[y]["CAPTION"].ToString();
                    }
                }
            }
            return strCaption;
        }

        private bool isNvarChar;

        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }

        private string GetDDText(string fieldName)
        {
            DataSet Dset = ((WebQueryFiledsCollection)this.Collection).DsForDD;
            string strCaption = "";

            if (Dset.Tables.Count == 0)
            {
                WebNavigator wng = (WebNavigator)this.Owner;
                object obj = null;
                if (wng.ViewBindingObject != null && wng.ViewBindingObject != "")
                {
                    obj = wng.GetObjByID(wng.ViewBindingObject);
                }
                else if (wng.BindingObject != null && wng.BindingObject != "")
                {
                    obj = wng.GetObjByID(wng.BindingObject);
                }
                if (obj != null)
                {
                    obj = wng.GetObjByID(((CompositeDataBoundControl)obj).DataSourceID);
                }
                if (obj is WebDataSource)
                {
                    ((WebQueryFiledsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(obj as WebDataSource, true);
                    Dset = ((WebQueryFiledsCollection)this.Collection).DsForDD;
                }
            }

            if (Dset.Tables.Count > 0)
            {
                int x = Dset.Tables[0].Rows.Count;
                for (int y = 0; y < x; y++)
                {
                    if (string.Compare(Dset.Tables[0].Rows[y]["FIELD_NAME"].ToString(), fieldName, true) == 0)//IgnoreCase
                    {
                        strCaption = Dset.Tables[0].Rows[y]["CAPTION"].ToString();
                    }
                }
            }
            return strCaption;
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebNavigator)
                {
                    WebNavigator wn = (WebNavigator)this.Owner;
                    if (wn.Page != null)
                    {
                        object obj = null;
                        if (wn.ViewBindingObject != null && wn.ViewBindingObject != "")
                        {
                            obj = wn.GetObjByID(wn.ViewBindingObject);
                        }
                        else if (wn.BindingObject != null && wn.BindingObject != "")
                        {
                            obj = wn.GetObjByID(wn.BindingObject);
                        }
                        if (obj != null && obj is CompositeDataBoundControl)
                        {
                            CompositeDataBoundControl dataControl = (CompositeDataBoundControl)obj;
                            foreach (Control ctrl in wn.Page.Controls)
                            {
                                if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == dataControl.DataSourceID)
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
                else if (this.Owner is CompositeDataBoundControl)
                {
                    CompositeDataBoundControl dataControl = (CompositeDataBoundControl)this.Owner;
                    if (dataControl.Page != null && dataControl.DataSourceID != null && dataControl.DataSourceID != "")
                    {
                        object obj = null;
                        if (dataControl is WebGridView)
                        {
                            obj = ((WebGridView)dataControl).GetObjByID(dataControl.DataSourceID);
                        }
                        else if (dataControl is WebDetailsView)
                        {
                            obj = ((WebDetailsView)dataControl).GetObjByID(dataControl.DataSourceID);
                        }

                        if (obj != null && obj is WebDataSource)
                        {
                            WebDataSource ds = (WebDataSource)obj;
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
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "condition", true) == 0)//IgnoreCase
            {
                values.Add("=");
                values.Add("!=");
                values.Add(">");
                values.Add("<");
                values.Add(">=");
                values.Add("<=");
                values.Add("%");
                values.Add("%%");
            }
            else if (string.Compare(sKind, "mode", true) == 0)//IgnoreCase
            {
                values.Add("TextBox");
                values.Add("ComboBox");
                values.Add("RefVal");
                values.Add("Calendar");
                values.Add("RefButton");
            }
            else if (string.Compare(sKind, "refval", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebNavigator)
                {
                    WebNavigator wng = (WebNavigator)this.Owner;
                    foreach (Control ct in wng.Page.Controls)
                    {
                        if (ct is WebRefVal || ct is WebRefButton)
                        {
                            values.Add(ct.ID);
                        }
                    }
                    if (wng.Page.Form != null)
                    {
                        foreach (Control ct in wng.Page.Form.Controls)
                        {
                            if (ct is WebRefVal || ct is WebRefButton)
                            {
                                values.Add(ct.ID);
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
    }

    #region GridEditor
    public class GridEditor : UITypeEditor
    {
        public GridEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private List<Control> AllCtrls = new List<Control>();
        private void GetAllCtrls(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                AllCtrls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllCtrls(ctrl.Controls);
                }
            }
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null)
            {
                if (context.Instance is Control)
                {
                    AllCtrls.Clear();
                    ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                    GetAllCtrls(ctrlList);
                    foreach (Control ctrl in AllCtrls)
                    {
                        if (ctrl is GridView || ctrl is DetailsView || ctrl is WebFormView)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
                else if (context.Instance is WebSecControl)
                {
                    object obj = ((WebSecControl)context.Instance).Owner;
                    if (obj != null && obj is WebSecColumns)
                    {
                        AllCtrls.Clear();
                        ControlCollection ctrlList = ((WebSecColumns)obj).Page.Controls;
                        GetAllCtrls(ctrlList);
                        foreach (Control ctrl in AllCtrls)
                        {
                            if (ctrl is GridView || ctrl is DetailsView || ctrl is WebFormView)
                            {
                                objName.Add(ctrl.ID);
                            }
                        }
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

    public class ViewGridEditor : UITypeEditor
    {
        public ViewGridEditor()
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
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is GridView)
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

    public class LinkLabelEditor : UITypeEditor
    {
        public LinkLabelEditor()
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
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is Label && !(ctrl is WebValidate))
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

    public class StatusStripEditor : UITypeEditor
    {
        public StatusStripEditor()
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
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebStatusStrip)
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

    public delegate void BeforeCommandEventHandler(object sender, BeforeCommandArgs e);
    public class BeforeCommandArgs : CommandEventArgs
    {
        public BeforeCommandArgs(string commandName, object argument)
            : base(commandName, argument)
        { }

        public BeforeCommandArgs(CommandEventArgs e)
            : base(e)
        { }

        private bool _Cancel;
        public bool Cancel
        {
            get
            {
                return _Cancel;
            }
            set
            {
                _Cancel = value;
            }
        }
    }

    #region NavigatorStates
    public class WebNavigatorStateCollection : InfoOwnerCollection
    {
        public WebNavigatorStateCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebNavigatorStateItem))
        {
            this.Owner = aOwner;
            InitStateItems();
        }

        private new object Owner = null;

        private string WebNavigatorType()
        {
            string type = "";
            if (Owner != null)
            {
                type = Owner.GetType().ToString();
            }
            return type;
        }

        protected virtual void InitStateItems()
        {
#if !VS90
            WebNavigatorStateItem InitialStateItem = new WebNavigatorStateItem();
            base.Add(InitialStateItem);
            InitialStateItem.StateText = "Initial";

            WebNavigatorStateItem BrowsedStateItem = new WebNavigatorStateItem();
            base.Add(BrowsedStateItem);
            BrowsedStateItem.StateText = "Browsed";

            WebNavigatorStateItem InsertingStateItem = new WebNavigatorStateItem();
            base.Add(InsertingStateItem);
            InsertingStateItem.StateText = "Inserting";

            WebNavigatorStateItem EditingStateItem = new WebNavigatorStateItem();
            base.Add(EditingStateItem);
            EditingStateItem.StateText = "Editing";

            WebNavigatorStateItem ApplyingStateItem = new WebNavigatorStateItem();
            base.Add(ApplyingStateItem);
            ApplyingStateItem.StateText = "Applying";

            WebNavigatorStateItem ChangingStateItem = new WebNavigatorStateItem();
            base.Add(ChangingStateItem);
            ChangingStateItem.StateText = "Changing";

            WebNavigatorStateItem QueryingStateItem = new WebNavigatorStateItem();
            base.Add(QueryingStateItem);
            QueryingStateItem.StateText = "Querying";

            WebNavigatorStateItem PrintingStateItem = new WebNavigatorStateItem();
            base.Add(PrintingStateItem);
            PrintingStateItem.StateText = "Printing";

            string navType = WebNavigatorType();
            if (navType == "FLTools.FLWebNavigator")
            {
                WebNavigatorStateItem NormalStateItem = new WebNavigatorStateItem();
                base.Add(NormalStateItem);
                NormalStateItem.StateText = "Normal";

                WebNavigatorStateItem InsertStateItem = new WebNavigatorStateItem();
                base.Add(InsertStateItem);
                InsertStateItem.StateText = "Insert";

                WebNavigatorStateItem ModifyStateItem = new WebNavigatorStateItem();
                base.Add(ModifyStateItem);
                ModifyStateItem.StateText = "Modify";

                WebNavigatorStateItem InqueryStateItem = new WebNavigatorStateItem();
                base.Add(InqueryStateItem);
                InqueryStateItem.StateText = "Inquery";

                WebNavigatorStateItem PrepareStateItem = new WebNavigatorStateItem();
                base.Add(PrepareStateItem);
                PrepareStateItem.StateText = "Prepare";

                WebNavigatorStateItem PreInsertStateItem = new WebNavigatorStateItem();
                base.Add(PreInsertStateItem);
                PreInsertStateItem.StateText = "PreInsert";

                WebNavigatorStateItem InqueryMultiStateItem = new WebNavigatorStateItem();
                base.Add(InqueryMultiStateItem);
                InqueryMultiStateItem.StateText = "InqueryMulti";

                WebNavigatorStateItem InquerySingleStateItem = new WebNavigatorStateItem();
                base.Add(InquerySingleStateItem);
                InquerySingleStateItem.StateText = "InquerySingle";

                WebNavigatorStateItem PrepareSingleStateItem = new WebNavigatorStateItem();
                base.Add(PrepareSingleStateItem);
                PrepareSingleStateItem.StateText = "PrepareSingle";

                WebNavigatorStateItem PrepareMultiStateItem = new WebNavigatorStateItem();
                base.Add(PrepareMultiStateItem);
                PrepareMultiStateItem.StateText = "PrepareMulti";
            }
#endif
        }

        public new WebNavigatorStateItem this[int index]
        {
            get
            {
                return (WebNavigatorStateItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebNavigatorStateItem)
                    {
                        //原来的Collection设置为0
                        ((WebNavigatorStateItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebNavigatorStateItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }

        new public void Remove(object value)
        {
            RemoveStateItem(value);
        }

        protected virtual void RemoveStateItem(object value)
        {
            WebNavigatorStateItem stateItem = value as WebNavigatorStateItem;
            if (stateItem != null)
            {
                if (stateItem.StateText == "Initial"
                    || stateItem.StateText == "Browsed"
                    || stateItem.StateText == "Inserting"
                    || stateItem.StateText == "Editing"
                    || stateItem.StateText == "Applying"
                    || stateItem.StateText == "Changing"
                    || stateItem.StateText == "Querying"
                    || stateItem.StateText == "Printing")
                {
                    throw new Exception("Default WebNavigatorStateItem can not be removed");
                }
                else
                {
                    string navType = WebNavigatorType();
                    if (navType == "FLTools.FLWebNavigator")
                    {
                        if (stateItem.StateText == "Normal"
                            || stateItem.StateText == "Insert"
                            || stateItem.StateText == "Modify"
                            || stateItem.StateText == "Inquery"
                            || stateItem.StateText == "Prepare")
                        {
                            throw new Exception("Default WebNavigatorStateItem can not be removed");
                        }
                        else
                        {
                            base.Remove(value);
                        }
                    }
                    else
                    {
                        base.Remove(value);
                    }
                }
            }
        }

        new public void RemoveAt(int index)
        {
            RemoveAtStateItem(index);
        }

        protected virtual void RemoveAtStateItem(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                WebNavigatorStateItem stateItem = this[index];
                if (stateItem.StateText == "Initial"
                    || stateItem.StateText == "Browsed"
                    || stateItem.StateText == "Inserting"
                    || stateItem.StateText == "Editing"
                    || stateItem.StateText == "Applying"
                    || stateItem.StateText == "Changing"
                    || stateItem.StateText == "Querying"
                    || stateItem.StateText == "Printing")
                {
                    throw new Exception("Default WebNavigatorStateItem can not be removed");
                }
                else
                {
                    string navType = WebNavigatorType();
                    if (navType == "FLTools.FLWebNavigator")
                    {
                        if (stateItem.StateText == "Normal"
                            || stateItem.StateText == "Insert"
                            || stateItem.StateText == "Modify"
                            || stateItem.StateText == "Inquery"
                            || stateItem.StateText == "Prepare")
                        {
                            throw new Exception("Default WebNavigatorStateItem can not be removed");
                        }
                        else
                        {
                            base.RemoveAt(index);
                        }
                    }
                    else
                    {
                        base.RemoveAt(index);
                    }
                }
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        new public void Clear()
        {
            this.ClearExceptDefaultStateItem();
        }

        new public void Add(object value)
        {
            WebNavigatorStateItem stateItem = value as WebNavigatorStateItem;
            if (stateItem != null)
            {
                foreach (WebNavigatorStateItem si in this)
                {
                    if (si.StateText == stateItem.StateText)
                    {
                        si.EnableControls = stateItem.EnableControls;
                        si.Description = stateItem.Description;
                        return;
                    }
                }

                base.Add(stateItem);
            }
        }

        protected virtual void ClearExceptDefaultStateItem()
        {
            string navType = WebNavigatorType();
            if (navType == "Srvtools.WebNavigator")
            {
                // The number of Default FLNavigatorStateItem is 7
                while (this.Count > 7)
                {
                    base.RemoveAt(7);
                }
            }
            else if (navType == "FLTools.FLWebNavigator")
            {
                // The number of Default FLNavigatorStateItem is 12
                while (this.Count > 12)
                {
                    base.RemoveAt(12);
                }
            }
        }
    }

    public class WebNavigatorStateItem : InfoOwnerCollectionItem
    {
        private string _name;
        private string _stateText;
        private string _description;
        private string _enableControls;

        public WebNavigatorStateItem()
        {
        }

        [NotifyParentProperty(true)]
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

        [NotifyParentProperty(true)]
        public string StateText
        {
            get
            {
                return _stateText;
            }
            set
            {
                if (_stateText == "Initial"
                    || _stateText == "Browsed"
                    || _stateText == "Inserting"
                    || _stateText == "Editing"
                    || _stateText == "Applying"
                    || _stateText == "Changing"
                    || _stateText == "Querying"
                    || _stateText == "Printing")
                {
                    throw new Exception("Default StateText Can not be Changed");
                }
                else if (value == null || value.Trim() == "")
                {
                    throw new Exception("Empty StateText not allowed");
                }
                else
                {
                    WebNavigatorStateCollection stateCollection = this.Collection as WebNavigatorStateCollection;
                    if (stateCollection != null)
                    {
                        foreach (WebNavigatorStateItem stateItem in stateCollection)
                        {
                            if (stateItem.StateText == value.Trim())
                            {
                                throw new Exception("StateText already exists");
                            }
                        }
                    }
                    _stateText = value.Trim();
                }
            }
        }

        [NotifyParentProperty(true)]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        [Editor(typeof(WebEnableControlsEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string EnableControls
        {
            get
            {
                return _enableControls;
            }
            set
            {
                _enableControls = value;
            }
        }

        public override string ToString()
        {
            return StateText;
        }
    }
    #endregion

    #region Register Script
    internal class ScriptHelper
    {
        public static void RegisterStartupScript(Control ctrl, string script)
        {
            RegisterStartupScript(ctrl, null, String.Empty, script);
        }

        public static void RegisterStartupScript(Control ctrl, string key, string script)
        {
            RegisterStartupScript(ctrl, null, key, script);

        }

        public static void RegisterStartupScript(Control ctrl, Page page, string key, string script)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString();
            }

            if (page == null)
            {
                page = ctrl.Page;
            }

#if AjaxTools
            Control panel = ctrl.Parent;
            while (panel != null && panel.GetType() != typeof(UpdatePanel))
            {
                panel = panel.Parent;
            }
            if (panel != null)
            {
                ScriptManager.RegisterStartupScript(panel as UpdatePanel, page.GetType(), key, script, true);
            }
            else
            {
#endif
                page.ClientScript.RegisterStartupScript(page.GetType(), key, "<script>" + script + "</script>");
#if AjaxTools
            }
#endif

        }

        public static void ShowMessage(Control ctrl, string key, string message)
        {
            string script = "alert('" + message + "')";
            RegisterStartupScript(ctrl, key, script);
        }

        public static void ShowMessage(Control ctrl, string message)
        {
            ShowMessage(ctrl, string.Empty, message);
        }
    }
    #endregion
}
