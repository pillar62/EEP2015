<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAnyQuery.aspx.cs" Inherits="InnerPages_frmAnyQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/innerpage/anyquery.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="center">
        <table>
            <tr>
                <td id="td1" class="opbtn_mouseout">
                    <asp:ImageButton ID="ImageButtonQuery" runat="server" OnClick="ImageButtonQuery_Click"
                        onmouseout="document.getElementById('td1').className='opbtn_mouseout'" onmouseover="document.getElementById('td1').className='opbtn_mouseover'"
                        ImageUrl="~/Image/UIPics/Query.gif" ToolTip="Query" Width="24px" Height="24px" />
                </td>
                <td id="td2" class="opbtn_mouseout">
                    <asp:ImageButton ID="ImageButtonAdd" runat="server" OnClick="ImageButtonAdd_Click"
                        onmouseout="document.getElementById('td2').className='opbtn_mouseout'" onmouseover="document.getElementById('td2').className='opbtn_mouseover'"
                        ImageUrl="~/Image/UIPics/Add.gif" ToolTip="Add" Width="24px" Height="24px" />
                </td>
                <td id="td3" class="opbtn_mouseout">
                    <asp:ImageButton ID="ImageButtonSubtract" runat="server" CommandName="Delete" ImageUrl="~/Image/UIPics/Delete.gif"
                        onmouseout="document.getElementById('td3').className='opbtn_mouseout'" onmouseover="document.getElementById('td3').className='opbtn_mouseover'"
                        OnClick="ImageButtonSubtract_Click" ToolTip="Delete" Width="24px" Height="24px" />
                </td>
                <td id="td4" class="opbtn_mouseout">
                    <asp:ImageButton ID="ImageButtonSave" runat="server" OnClick="ImageButtonSave_Click"
                        onmouseout="document.getElementById('td4').className='opbtn_mouseout'" onmouseover="document.getElementById('td4').className='opbtn_mouseover'"
                        ImageUrl="~/Image/UIPics/Apply.gif" ToolTip="Save" Width="24px" Height="24px" />
                </td>
                <td id="td5" class="opbtn_mouseout">
                    <asp:ImageButton ID="ImageButtonLoad" runat="server" OnClick="ImageButtonLoad_Click"
                        onmouseout="document.getElementById('td5').className='opbtn_mouseout'" onmouseover="document.getElementById('td5').className='opbtn_mouseover'"
                        ToolTip="Load" ImageUrl="~/Image/UIPics/Continue.gif" Width="24px" Height="24px" />
                </td>
                <td id="td6" class="opbtn_mouseout">
                    <asp:ImageButton ID="ImageButtonQuit" runat="server" ImageUrl="~/Image/UIPics/Return.gif"
                        onmouseout="document.getElementById('td6').className='opbtn_mouseout'" onmouseover="document.getElementById('td6').className='opbtn_mouseover'"
                        OnClick="ImageButtonQuit_Click" ToolTip="Quit" Width="24px" Height="24px" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Label" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" Width="150px">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBox1" runat="server" Width="150px" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="OK" CssClass="btn_rect_mouseout" />
                        <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Cancel"
                            CssClass="btn_rect_mouseout" />
                        <asp:Button ID="ButtonDelete" runat="server" OnClick="ButtonDelete_Click" Text="Delete"
                            CssClass="btn_rect_mouseout" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
                    <asp:Panel ID="Panel1" runat="server" CssClass="gen_container">
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
