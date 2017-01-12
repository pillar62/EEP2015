<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogOn.aspx.cs" Inherits="LogOn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <div id="header"></div>
            <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" BorderPadding="4"
                PasswordLabelText="密码:" RememberMeText="记住密码." UserNameLabelText="用户:">
                <LayoutTemplate>
                    <div id="info" class="posttext">

                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td width="40%" rowspan="14" align="center" valign="middle">
                                    <img src="img/logo.png" width="167" height="54" /></td>
                                <td width="14%">&nbsp;</td>
                                <td width="46%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <img src="img/EEP.png" width="307" height="56" /></td>
                            </tr>
                            <tr>
                                <td class="whtext">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="用户:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server" class="form01"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" Height="1px" Width="1px"
                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="whtext">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="密码:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form01"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="whtext">
                                    <asp:Label ID="DatabaseLabel" runat="server" AssociatedControlID="Database" Text="资料库:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="Database" runat="server" CssClass="form02">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="whtext">
                                    <asp:Label ID="SolutionLabel" runat="server" AssociatedControlID="Solution" Text="解决方案:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="Solution" runat="server" CssClass="form02">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr runat="server" id="checkcodetr">
                                <td  class="whtext">
                                    <asp:Label ID="lbCheckCode" runat="server" Text="验证码"  AssociatedControlID="Solution" ></asp:Label>
                                </td>
                                <td >
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCheckCode" runat="server" CssClass="form01" Width="84px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Image
                                                    ID="Image1" runat="server" />
                                            </td>
                                        </tr>
                                    </table>                                
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="Remember" runat="server" Text="记住密码." />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Literal ID="FailureLabel" runat="server" EnableViewState="True"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="bt">
                        <ul>
                            <li>
                                <asp:Button ID="LoginButton" runat="server" Text="Login" 
                                     ValidationGroup="Login1" CommandName="Login" CssClass="buttonStyle" />
                            </li>
                            <li>
                                <asp:Button ID="Logout" runat="server" CssClass="buttonStyle" Text="" OnClick="Logout_Click" /></li>
                        </ul>
                    </div>
                </LayoutTemplate>
            </asp:Login>
        </div>
    <asp:HiddenField ID="hfOValue" runat="server" />
    <asp:HiddenField ID="hfNValue" runat="server" />
    </form>
</body>
</html>
