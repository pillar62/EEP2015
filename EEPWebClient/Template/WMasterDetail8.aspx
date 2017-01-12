<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail8.aspx.cs" Inherits="Template_WMasterDetail1" Theme="ControlSkin" StylesheetTheme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxtools:ajaxscriptmanager id="AjaxScriptManager1" runat="server"></ajaxtools:ajaxscriptmanager>
        <InfoLight:webdatasource id="Master" runat="server" autoapply="True" WebDataSetID="WMaster" cachedataset="True"></InfoLight:webdatasource>
    
    </div>
        <InfoLight:webdatasource id="Detail" runat="server" MasterDataSource="Master" WebDataSetID="WMaster"></InfoLight:webdatasource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="White"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" Width="100%" 
        StatusBackColor="White" StatusForeColor="MediumBlue" 
        TitleBackColor="MediumBlue" TitleForeColor="White" SkinID="StatusStripSkin1" />
        <ajaxtools:ajaxgridview id="AjaxGridView1" runat="server" datasourceid="Master" PagingSet-DisplayPageInfo="False" GridSet-Width="300"><ToolItems>
<AjaxTools:ExtGridToolItem ButtonName="btnQuery" Text="query" SysHandlerType="Query" IconUrl="~/Image/Ext/query.gif"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ToolItemType="Fill"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ButtonName="btnRefresh" Text="refresh" IconUrl="~/Image/Ext/refresh.gif"></AjaxTools:ExtGridToolItem>
</ToolItems>
</ajaxtools:ajaxgridview>
        <AjaxTools:AjaxLayout ID="AjaxLayout1" runat="server" View="AjaxGridView1">
            <Details>
                <AjaxTools:MultiViewItem ControlId="AjaxGridViewDetail" Title="Detail" />
            </Details>
            <ToolItems>
                <AjaxTools:ExtGridToolItem ButtonName="btnAdd" IconUrl="~/Image/Ext/add.gif" SysHandlerType="Add"
                    Text="add" />
                <AjaxTools:ExtGridToolItem ButtonName="btnEdit" IconUrl="~/Image/Ext/edit.gif" SysHandlerType="Edit"
                    Text="edit" />
                <AjaxTools:ExtGridToolItem ButtonName="btnDelete" IconUrl="~/Image/Ext/delete.gif"
                    SysHandlerType="Delete" Text="delete" />
                <AjaxTools:ExtGridToolItem ToolItemType="Separation" />
                <AjaxTools:ExtGridToolItem ButtonName="btnOK" IconUrl="~/Image/Ext/ok.gif" SysHandlerType="OK"
                    Text="ok" />
                <AjaxTools:ExtGridToolItem ButtonName="btnCancel" IconUrl="~/Image/Ext/cancel.gif"
                    SysHandlerType="Cancel" Text="cancel" />
            </ToolItems>
            <Masters>
                <AjaxTools:MultiViewItem ControlId="AjaxFormView1" Title="Master" />
            </Masters>
        </AjaxTools:AjaxLayout>
        <AjaxTools:AjaxFormView ID="AjaxFormView1" runat="server" DataSourceID="Master">
        </AjaxTools:AjaxFormView>
        <ajaxtools:ajaxgridview id="AjaxGridViewDetail" runat="server" datasourceid="Detail" pagingset-allowpage="False"><ToolItems>
<AjaxTools:ExtGridToolItem ButtonName="btnAdd" Text="add" SysHandlerType="Add" IconUrl="~/Image/Ext/add.gif"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ButtonName="btnEdit" Text="edit" SysHandlerType="Edit" IconUrl="~/Image/Ext/edit.gif"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ButtonName="btnDelete" Text="delete" SysHandlerType="Delete" IconUrl="~/Image/Ext/delete.gif"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ToolItemType="Separation"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ButtonName="btnSave" Text="save" SysHandlerType="Save" IconUrl="~/Image/Ext/save.gif"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ButtonName="btnAbort" Text="abort" SysHandlerType="Abort" IconUrl="~/Image/Ext/abort.gif"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ToolItemType="Separation"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ToolItemType="Fill"></AjaxTools:ExtGridToolItem>
<AjaxTools:ExtGridToolItem ButtonName="btnRefresh" Text="refresh" IconUrl="~/Image/Ext/refresh.gif"></AjaxTools:ExtGridToolItem>
</ToolItems>
</ajaxtools:ajaxgridview>
    </form>
</body>
</html>
