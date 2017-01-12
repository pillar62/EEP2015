<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowPause.aspx.cs" Inherits="InnerPages_FlowPause" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/innerpage/flowpause.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_container">
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="ddlOrgKind" runat="server" >
                </asp:DropDownList>
            </td>
            <td id="split"></td>
            <td>
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout'" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
