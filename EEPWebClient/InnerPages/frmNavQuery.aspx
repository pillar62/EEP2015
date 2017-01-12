<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmNavQuery.aspx.cs" Inherits="InnerPages_frmNavQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/innerpage/navquery.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table id="main_table">
        <tr>
            <td class="margin_td">
            </td>
            <td class="middle_td">
                <center>
                    <table cellpadding="0px" border="0px" cellspacing="0px" width="100%">
                        <tr>
                            <td class="left-top">
                            </td>
                            <td class="top">
                            </td>
                            <td class="right-top">
                            </td>
                        </tr>
                        <tr>
                            <td class="left">
                            </td>
                            <td class="center">
                                <asp:Panel ID="Panel1" runat="server">
                                </asp:Panel>
                            </td>
                            <td class="right">
                            </td>
                        </tr>
                        <tr>
                            <td class="left-bottom">
                            </td>
                            <td class="bottom">
                            </td>
                            <td class="right-bottom">
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel2" runat="server">
                    </asp:Panel>
                </center>
            </td>
            <td class="margin_td">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
