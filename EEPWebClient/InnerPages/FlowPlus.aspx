<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowPlus.aspx.cs" Inherits="InnerPages_FlowPlus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flow Plus</title>
    <link href="../css/innerpage/flowplus.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_container">
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

        <fieldset class="centeralign_fieldset">
            <legend class="color_legend">
                <%= this.getHtmlText(1) %></legend>
            <table>
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
                        <asp:Button ID="btnLRRoles" runat="server" Text=" > " OnClick="btnLRRoles_Click"
                            CssClass="btn_square_mouseout" onmouseout="this.className='btn_square_mouseout'"
                            onmouseover="this.className='btn_square_mouseover'" />
                        <br />
                        <asp:Button ID="btnRLRoles" runat="server" Text=" < " OnClick="btnRLRoles_Click"
                            CssClass="btn_square_mouseout" onmouseout="this.className='btn_square_mouseout'"
                            onmouseover="this.className='btn_square_mouseover'" />
                    </td>
                    <td>
                        <asp:ListBox ID="lstRolesTo" runat="server" SelectionMode="Multiple" CssClass="lst"
                            AppendDataBoundItems="True"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td style="float: left">
                                    <%= this.getHtmlText(8) %>
                                </td>
                                <td style="float: left">
                                    <asp:TextBox ID="txtSearchRoleId" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td style="float: left">
                                    <%= this.getHtmlText(9) %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchRoleName" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" 
                                    CssClass="btn_rect_mouseout" onmouseout="this.className='btn_rect_mouseout'" 
                                    onmouseover="this.className='btn_rect_mouseover'" />
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
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="panDownload" runat="server" Style="text-align: left;">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="button_container">
            <asp:Button ID="btnOk" runat="server" Text="OK" OnClick="btnOk_Click" CssClass="btn_rect_mouseout"
                onmouseout="this.className='btn_rect_mouseout'" onmouseover="this.className='btn_rect_mouseover'" />
        </div>
        <div class="button_container">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                CssClass="btn_rect_mouseout" onmouseout="this.className='btn_rect_mouseout'"
                onmouseover="this.className='btn_rect_mouseover'" />
        </div>
        <asp:Panel ID="panResult" runat="server" Width="95%" Visible="false">
            <fieldset id="resultContainer" class="rightalign_fieldset">
                <legend>
                    <%= this.getHtmlText(7) %></legend>
                <asp:Label ID="result" runat="server" Width="100%" ForeColor="Red" Style="text-align: left;"></asp:Label>
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn_rect_mouseout"
                    onmouseout="this.className='btn_rect_mouseout'" 
                    onmouseover="this.className='btn_rect_mouseover'" onclick="btnClose_Click" />
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
