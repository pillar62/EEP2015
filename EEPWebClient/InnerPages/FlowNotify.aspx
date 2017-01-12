<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowNotify.aspx.cs" Inherits="InnerPages_FlowNotify"
    Theme="InnerPageSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flow Notify</title>
    <link href="../css/innerpage/flownotify.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_container">
        <table cellpadding="0px" border="0px" cellspacing="0px">
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
                    <fieldset class="centeralign_fieldset">
                        <legend class="color_legend">
                            <%= this.getHtmlText(0) %></legend>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <%= this.getHtmlText(3) %>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <%= this.getHtmlText(4) %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstUsersFrom" runat="server" SelectionMode="Multiple" AppendDataBoundItems="True"
                                        CssClass="lst"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnLRUsers" runat="server" OnClick="btnLRUsers_Click" Text=" > "
                                        CssClass="btn_square_mouseout" onmouseout="this.className='btn_square_mouseout'"
                                        onmouseover="this.className='btn_square_mouseover'" />
                                    <asp:Button ID="btnRLUsers" runat="server" OnClick="btnRLUsers_Click" Text=" < "
                                        CssClass="btn_square_mouseout" onmouseout="this.className='btn_square_mouseout'"
                                        onmouseover="this.className='btn_square_mouseover'" />
                                </td>
                                <td>
                                    <asp:ListBox ID="lstUsersTo" runat="server" SelectionMode="Multiple" AppendDataBoundItems="True"
                                        CssClass="lst"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="float: left">
                                                <%= this.getHtmlText(11) %>
                                            </td>
                                            <td style="float: left">
                                                <asp:TextBox ID="txtSearchUserId" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td style="float: left">
                                                <%= this.getHtmlText(12) %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearchUserName" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnUserGo" runat="server" OnClick="btnUserGo_Click" CssClass="btn_rect_mouseout"
                                                    onmouseout="this.className='btn_rect_mouseout'" onmouseover="this.className='btn_rect_mouseover'" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset class="centeralign_fieldset">
                        <legend class="color_legend">
                            <%= this.getHtmlText(1) %></legend>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <%= this.getHtmlText(3) %>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <%= this.getHtmlText(4) %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstRolesFrom" runat="server" SelectionMode="Multiple" AppendDataBoundItems="True"
                                        CssClass="lst"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnLRRoles" runat="server" OnClick="btnLRRoles_Click" Text=" > "
                                        CssClass="btn_square_mouseout" onmouseout="this.className='btn_square_mouseout'"
                                        onmouseover="this.className='btn_square_mouseover'" />
                                    <asp:Button ID="btnRLRoles" runat="server" OnClick="btnRLRoles_Click" Text=" < "
                                        CssClass="btn_square_mouseout" onmouseout="this.className='btn_square_mouseout'"
                                        onmouseover="this.className='btn_square_mouseover'" />
                                </td>
                                <td>
                                    <asp:ListBox ID="lstRolesTo" runat="server" SelectionMode="Multiple" AppendDataBoundItems="True"
                                        CssClass="lst"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="float: left">
                                                <%= this.getHtmlText(8) %>
                                            </td>
                                            <td style="float: left">
                                                <asp:TextBox ID="txtSearchRoleId" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td style="float: left">
                                                <%= this.getHtmlText(9) %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearchRoleName" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnRoleGo" runat="server" OnClick="btnRoleGo_Click" CssClass="btn_rect_mouseout"
                                                    onmouseout="this.className='btn_rect_mouseout'" onmouseover="this.className='btn_rect_mouseover'" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset class="centeralign_fieldset">
                        <legend class="color_legend">
                            <%= this.getHtmlText(2) %></legend>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Height="50px" Width="420px"></asp:TextBox>
                    </fieldset>
                    <div class="oc_button_container">
                        <asp:Button ID="btnOk" runat="server" Text="OK" OnClick="btnOk_Click" CssClass="btn_rect_mouseout"
                            onmouseout="this.className='btn_rect_mouseout'" onmouseover="this.className='btn_rect_mouseover'" />
                    </div>
                    <div class="oc_button_container">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            CssClass="btn_rect_mouseout" onmouseout="this.className='btn_rect_mouseout'"
                            onmouseover="this.className='btn_rect_mouseover'" />
                    </div>
                    <asp:Panel ID="panResult" runat="server" Width="100%" Visible="false">
                        <fieldset id="resultContainer" class="rightalign_fieldset">
                            <legend>
                                <%= this.getHtmlText(7) %></legend>
                            <asp:Label ID="result" runat="server" Width="100%"></asp:Label>
                            <div id="btnClose">
                                <a href="javascript:window.close();" class="oc_button">Close</a>
                            </div>
                        </fieldset>
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
    </div>
    </form>
</body>
</html>
