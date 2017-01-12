<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Timeout.aspx.cs" Inherits="Timeout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>您的登錄逾時,請重新登錄!</title>
    <link href="css/Timeout2012.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="inhalt" runat="server" style="background-image:url('Image/main/timeout_bg_tw.png')">
        <table id="tab_container" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td id="content">
                    <br />
                    <br />
                    <p>
                        對不起，您的登錄逾時！請重新登錄!</p>
                    <p>
                        Sorry! Your Login already timeout! Please Relogin!</p>
                </td>
            </tr>
            <tr>
                <td id="footer">
                    <asp:HyperLink ID="Relogin" runat="server" Font-Bold="True" Font-Size="12px" Target="_parent"><<重新登錄>></asp:HyperLink>
                </td>
            </tr>
            <tr>
            <td style="height:70px;"></td></tr>
        </table>
    </div>
    </form>
</body>
</html>
