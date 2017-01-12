<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <link href="css/Error.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblMsg" runat="server" Text="The error message:"></asp:Label><br />
        <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" CssClass="input"></asp:TextBox><br />
        <asp:Label ID="lblStackTrace" runat="server" Text="The error stack trace:"></asp:Label><br />
        <asp:TextBox ID="txtStackTrace" runat="server" TextMode="MultiLine"  CssClass="input"></asp:TextBox>
        <asp:TextBox ID="TextBoxServerStack" runat="server" TextMode="MultiLine" Visible="False" CssClass="input"></asp:TextBox><br />
        <br />
        <asp:Label ID="lblDesc" runat="server" Text="Feedback description:"></asp:Label><br />
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="input"></asp:TextBox><br />
        <asp:Button ID="ButtonServerInfo" runat="server" Text="Server Stack" OnClick="ButtonServerInfo_Click" CssClass="btn_mouseout"
                            onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'" />
        <asp:Button ID="btnFeedback" runat="server" Text="Feedback" OnClick="btnFeedback_Click" CssClass="btn_mouseout"
                            onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'" /></div>
    </form>
</body>
</html>
