<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail7.aspx.cs" Inherits="Template_WMasterDetail7" Theme="ControlSkin" StylesheetTheme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxtools:ajaxscriptmanager id="AjaxScriptManager1" runat="server" renderextshowmodelscripts="True"></ajaxtools:ajaxscriptmanager>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster" cachedataset="True"></InfoLight:WebDataSource>
        <InfoLight:WebDataSource ID="Detail" runat="server" MasterDataSource="Master" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
    
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="DarkSlateGray" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusBackColor="White"
            TitleForeColor="White" Width="100%" SkinID="StatusStripSkin1" /><BR />
                <BR /><BR /><BR />
</contenttemplate>
        </asp:UpdatePanel>
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BackColor="White"
            BindingObject="wfvMaster" BorderColor="#E0E0E0" BorderStyle="Groove" BorderWidth="2px"
            OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle" StatusStrip="WebStatusStrip1"
            Width="100%" SkinID="WebNavigatorManagerSkin1">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="掸" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/first3.gif" ImageUrl="../image/uipics/first.gif"
                    MouseOverImageUrl="../image/uipics/first2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="掸" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/previous3.gif" ImageUrl="../image/uipics/previous.gif"
                    MouseOverImageUrl="../image/uipics/previous2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Next" ControlText="掸" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/next3.gif" ImageUrl="../image/uipics/next.gif"
                    MouseOverImageUrl="../image/uipics/next2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Last" ControlText="ソ掸" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/next3.gif" ImageUrl="../image/uipics/last.gif"
                    MouseOverImageUrl="../image/uipics/last2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Add" ControlText="穝糤" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif"
                    MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Update" ControlText="э" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/edit3.gif" ImageUrl="../image/uipics/edit.gif"
                    MouseOverImageUrl="../image/uipics/edit2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="埃" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/delete3.gif" ImageUrl="../image/uipics/delete.gif"
                    MouseOverImageUrl="../image/uipics/delete2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="絋粄" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/ok3.gif" ImageUrl="../image/uipics/ok.gif"
                    MouseOverImageUrl="../image/uipics/ok2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/cancel3.gif" ImageUrl="../image/uipics/cancel.gif"
                    MouseOverImageUrl="../image/uipics/cancel2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="郎" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/apply3.gif" ImageUrl="../image/uipics/apply.gif"
                    MouseOverImageUrl="../image/uipics/apply2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="斌" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/abort3.gif" ImageUrl="../image/uipics/abort.gif"
                    MouseOverImageUrl="../image/uipics/abort2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Query" ControlText="琩高" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/query3.gif" ImageUrl="../image/uipics/query.gif"
                    MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Print" ControlText="ゴ" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/print3.gif" ImageUrl="../image/uipics/print.gif"
                    MouseOverImageUrl="../image/uipics/print2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Export" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../Image/UIPics/Export3.gif" ImageUrl="../Image/UIPics/Export.gif"
                    MouseOverImageUrl="../Image/UIPics/Export2.gif" Size="25" />
            </NavControls>
            <NavStates>
                <InfoLight:WebNavigatorStateItem StateText="Initial" />
                <InfoLight:WebNavigatorStateItem StateText="Browsed" />
                <InfoLight:WebNavigatorStateItem StateText="Inserting" />
                <InfoLight:WebNavigatorStateItem StateText="Editing" />
                <InfoLight:WebNavigatorStateItem StateText="Applying" />
                <InfoLight:WebNavigatorStateItem StateText="Changing" />
                <InfoLight:WebNavigatorStateItem StateText="Querying" />
                <InfoLight:WebNavigatorStateItem StateText="Printing" />
            </NavStates>
        </InfoLight:WebNavigator>
    
    </div>
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" 
        DataSourceID="Master" Height="87px" LayOutColNum="2" OnAfterInsertLocate="wfvMaster_AfterInsertLocate"
            OnCanceled="wfvMaster_Canceled" 
        OnPageIndexChanged="wfvMaster_PageIndexChanged" Width="100%" 
        SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <ajaxTools:AjaxGridView ID="AjaxGridViewDetail" runat="server" DataSourceID="Detail" gridset-gridpanel="UpdatePanel1" pagingset-allowpage="False"><ToolItems>
<ajaxTools:ExtGridToolItem ButtonName="btnAdd" Text="add" SysHandlerType="Add" IconUrl="~/Image/Ext/add.gif"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ButtonName="btnEdit" Text="edit" SysHandlerType="Edit" IconUrl="~/Image/Ext/edit.gif"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ButtonName="btnDelete" Text="delete" SysHandlerType="Delete" IconUrl="~/Image/Ext/delete.gif"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ToolItemType="Separation"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ButtonName="btnSave" Text="save" SysHandlerType="Save" IconUrl="~/Image/Ext/save.gif"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ButtonName="btnAbort" Text="abort" SysHandlerType="Abort" IconUrl="~/Image/Ext/abort.gif"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ToolItemType="Fill"></ajaxTools:ExtGridToolItem>
<ajaxTools:ExtGridToolItem ButtonName="btnRefresh" Text="refresh" IconUrl="~/Image/Ext/refresh.gif"></ajaxTools:ExtGridToolItem>
</ToolItems>
</ajaxTools:AjaxGridView>
    </form>
</body>
</html>
