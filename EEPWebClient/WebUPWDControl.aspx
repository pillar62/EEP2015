<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebUPWDControl.aspx.cs" Inherits="WebUPWDControl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change password</title>
    <link href="css/UpdatePwd.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="inhalt">
        <table id="mainTable" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td runat="server" id="td11" style="background-image:url('Image/main/changepwd_bg_tw.png');">
                    <div id="layer1" align="center">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="labelUserID" runat="server" Text="User ID:" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserID" runat="server" Enabled="False" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labelUserName" runat="server" Text="User Name:" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" Enabled="False" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labelOldPassword" runat="server" Text="Old Password:" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labelNewPassword" runat="server" Text="New Password:" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labelConfirmPassword" runat="server" Text="Confirm Password:" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right" style="height:20px">
                                    <asp:Label ID="labelMessage" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="btnContainer">
                                    <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="btn_mouseout"
                                        onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'" />
                                </td>
                                <td class="btnContainer">
                                    <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" CssClass="btn_mouseout"
                                        onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'" />
                                </td>
                                <td class="btnContainer">
                                    <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go Back" CssClass="btn_mouseout"
                                        onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
