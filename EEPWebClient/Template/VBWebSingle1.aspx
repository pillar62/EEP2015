<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebSingle1.aspx.vb" Inherits="Template_VBWebSingle1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        &nbsp;</div>
        <InfoLight:WebFormView ID="WebFormView1" runat="server" Height="225px" 
        Width="100%" DataSourceID="Master" LayOutColNum="2" SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
        <InfoLight:WebTranslate ID="WebTranslate1" runat="server" BindingObject="WebFormView1"
            CancelButtonCaption="CANCEL" DataSourceID="Master" OKButtonCaption="OK" Width="207px" />
    </form>
</body>
</html>
