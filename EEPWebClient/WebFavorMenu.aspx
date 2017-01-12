<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebFavorMenu.aspx.cs" Inherits="WebFavorMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Favor Menu</title>
    <script type="text/javascript">
        function Close() {
            window.close();
        }
    </script>
    <link href="css/ClientMain.css" rel="stylesheet" type="text/css" />
</head>
<body style="vertical-align: middle; text-align: center">
    <form id="form1" runat="server">
    <div>
        <table style="vertical-align: middle; text-align: center;">
            <tr>
                <td style="width: 45%; height: 18px;">
                    <asp:Label ID="Label1" runat="server" Text="Menus" CssClass="label"></asp:Label>
                </td>
                <td style="width: 10%; height: 18px;">
                </td>
                <td style="width: 45%; height: 18px;">
                    <asp:Label ID="Label2" runat="server" Text="Favors" CssClass="label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 45%; height: 80%;">
                    <asp:Panel ID="Panel1" runat="server" Height="250px"  Width="100%">
                        <asp:ListBox ID="lbAll" runat="server" style="Width:150px ;Height:250px"></asp:ListBox>
                    </asp:Panel>
                </td>
                <td style="vertical-align: middle; width: 10%; height: 100%;">
                    <table style="width: 100%; height: 136%">
                        <tr>
                            <td style="vertical-align: top; width: 100%; height: 100px; text-align: center">
                                <asp:Button ID="btnAddAll" runat="server" Text=">>" Width="25px" OnClick="btnAddAll_Click"
                                    CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" /><br />
                                <asp:Button ID="btnAdd" runat="server" Text=">" Width="25px" OnClick="btnAdd_Click"
                                    CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: bottom; width: 100%; height: 100px; text-align: center">
                                <asp:Button ID="btnRemove" runat="server" Text="<" Width="25px" OnClick="btnRemove_Click"
                                    CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" /><br />
                                <asp:Button ID="btnRemoveAll" runat="server" Text="<<" Width="25px" OnClick="btnRemoveAll_Click"
                                    CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 45%; height: 80%;">
                    <asp:Panel ID="SelectedPanel" runat="server" Height="250px" Width="100%">
                        <asp:DropDownList ID="ddlGroup" runat="server" Width="120px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <asp:ListBox ID="lbFavor" runat="server" style="Width:150px; Height:230px" ></asp:ListBox>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 45%">
                    &nbsp;<asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="btn_mouseout"
                        onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                </td>
                <td style="width: 10%">
                </td>
                <td style="width: 45%">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';"
                        onmouseout="this.className='btn_mouseout';" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
