<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWReport3.aspx.vb" Inherits="Template_VBWReport3" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:crystalreportsource id="CrystalReportSource1" runat="server"></cr:crystalreportsource>
        <InfoLight:WebDataSource ID="WebDataSource1" runat="server" WebDataSetID="WData">
        </InfoLight:WebDataSource>
        <InfoLight:WebClientQuery ID="WebClientQuery1" runat="server" DataSourceID="WebDataSource1">
        </InfoLight:WebClientQuery>
        <asp:Panel ID="Panel1" runat="server" Height="131px" Width="100%">
        </asp:Panel>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="PRINT" Width="116px" />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="CLEAR" Width="116px" /><br />
        <cr:crystalreportviewer id="CrystalReportViewer1" runat="server" autodatabind="true"
            displaygrouptree="False" displaypage="False"></cr:crystalreportviewer>
        &nbsp;
    
    </div>
    </form>
</body>
</html>
