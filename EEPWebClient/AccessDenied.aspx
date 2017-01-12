<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessDenied.aspx.cs" Inherits="AccessDenied" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>您沒有權限使用本頁面</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="341">
            <tr>
                <td>
                    <asp:Image ID="top" runat="server" /></td>
            </tr>
            <tr>
                <td background="../Image/login/2.jpg">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" width="9%">
                                <asp:Image ID="left" runat="server" /></td>
                            <td align="center" style="width: 82%">
                                <table style="width: 280px;" cellpadding="6" cellspacing="1">
                                    <tr>
                                        <td style="font-size: 12px; font-weight: bold; color: #ff0000;" align="left">
                                            發現錯誤:</td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 12px; background-color: #ffffff;" align="left">
                                            對不起，您沒有權限使用本頁面！<br />
                                                Sorry! Access Denied On This Page!
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="font-size: 10px; background-color: #ffffff; font-weight: bold;">
                                            </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right" width="9%"><asp:Image ID="right" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 9px">
                    <asp:Image ID="foot" runat="server" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
