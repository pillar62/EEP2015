<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmListBoxListCheck.aspx.cs"
    Inherits="InnerPages_frmListBoxListCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/innerpage/listboxlistcheck.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/JavaScript">
        function SetChecked(count, checked) {
            for (i = 0; i < count; i++) {
                document.getElementById('check' + i).checked = checked;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table>
                <tr>
                    <td colspan="4">
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
                                    <asp:Panel ID="PanelCheckBox" runat="server" Height="350px" Width="400px" ScrollBars="Auto"
                                        CssClass="pnl_background">
                                        <asp:Table ID="TableCheckBox" runat="server" CellPadding="1" CellSpacing="1">
                                        </asp:Table>
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
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="BtnSelectAll" runat="server" Text="Select All" Width="60px" CssClass="btn_mouseout"
                            onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout'" />
                    </td>
                    <td align="center">
                        <asp:Button ID="BtnUnSelectAll" runat="server" Text="UnSelect All" Width="60px" CssClass="btn_mouseout"
                            onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout'" />
                    </td>
                    <td align="center">
                        <asp:Button ID="btnOK" runat="server" Text="OK" Width="60px" OnClick="btnOK_Click"
                            CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout'" />
                    </td>
                    <td align="center">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="60px" OnClientClick="window.close();"
                            CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout'" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
