<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WSingle1.aspx.cs" Inherits="WSingle_WSingle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        &nbsp;</div>
        <InfoLight:WebFormView ID="WebFormView1" runat="server" Height="225px" Width="100%" DataSourceID="Master" LayOutColNum="2" AllValidateSucess="True" AutoEmptyDataText="False" BackColor="Azure" CellPadding="4" DataHasChanged="False" ForeColor="#333333" InsertBack="False" KeyValues="" NeedExecuteAdd="True" OldPageIndex="-1" ValidateFailed="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="DarkTurquoise" />
            <RowStyle BackColor="#EFF3FB" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
        <InfoLight:WebTranslate ID="WebTranslate1" runat="server" BindingObject="WebFormView1"
            CancelButtonCaption="CANCEL" DataSourceID="Master" OKButtonCaption="OK" Width="207px" />
    </form>
</body>
</html>
