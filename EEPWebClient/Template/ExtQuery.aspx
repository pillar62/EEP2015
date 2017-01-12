<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExtQuery.aspx.cs" Inherits="Template_WSingle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster" DataMember="Customers" CacheDataSet="True">
        </InfoLight:WebDataSource>
        <InfoLight:WebClientQuery ID="WebClientQuery1" runat="server" DataSourceID="Master">
        </InfoLight:WebClientQuery>
        <ajaxtools:ajaxscriptmanager id="AjaxScriptManager1" runat="server"></ajaxtools:ajaxscriptmanager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" Height="133px" Width="100%">
        </asp:Panel>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Query" Width="94px" />
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Clear" Width="89px" />
                        <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        

        <ajaxtools:ajaxgridview id="AjaxGridView1" runat="server" 
        datasourceid="Master" GridSet-Width="680">
            <ToolItems>
                <AjaxTools:ExtGridToolItem ButtonName="btnRefresh" IconUrl="~/Image/Ext/refresh.gif" Text="refresh" />
            </ToolItems>
</ajaxtools:ajaxgridview>
    
    </div>
</form>
</body>
</html>
