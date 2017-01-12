<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowUrge.aspx.cs" Inherits="InnerPages_FlowUrge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/innerpage/flowurge.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_content">
        <fieldset class="rightalign_fieldset">
            <legend><%= this.getCaption(0) %></legend>
            <asp:TextBox ID="txtRemmark" runat="server" TextMode="MultiLine" Height="100px" Width="98%"></asp:TextBox>
            <div class="button_container">
                <asp:Button ID="btnOk" runat="server" CssClass="btn_rect_mouseout"
                    onmouseout="this.className='btn_rect_mouseout';" 
                    onmouseover="this.className='btn_rect_mouseover';" onclick="btnOk_Click" />
            </div>
            <div class="button_container">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn_rect_mouseout"
                    onmouseout="this.className='btn_rect_mouseout';" 
                    onmouseover="this.className='btn_rect_mouseover';" onclick="btnCancel_Click" />
            </div>
        </fieldset>
        <hr />
        <asp:Panel ID="panResult" runat="server" Visible="false">
            <fieldset id="resultContainer" class="rightalign_fieldset">
                <legend><%= this.getCaption(1) %></legend>
                <table style="width:100%;">
                    <tr>
                        <td style="text-align: left; width:90%;">
                            <asp:Label ID="result" runat="server" SkinID="FullWidthMessageLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right; width:10%;">
                            <asp:Button ID="btnClose" runat="server" CssClass="btn_rect_mouseout"
                                onmouseout="this.className='btn_rect_mouseout';" 
                                onmouseover="this.className='btn_rect_mouseover';" onclick="btnClose_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
