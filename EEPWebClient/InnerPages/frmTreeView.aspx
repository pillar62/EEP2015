<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmTreeView.aspx.cs" Inherits="InnerPages_frmTreeView" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/innerpage/treeview.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width = "160" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="height: 99px" ><table width="100%">
                <tr style="height: 33%">
                    <td  style="width:30%" align="center">
                        <asp:Label ID ="lblKey" runat ="server" Text ="Key" />
                    </td>
                    <td style="width: 70%">
                        <asp:TextBox ID="txtKey" runat="server" Text="" Width="98%" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr style="height: 33%">
                    <td align="center" style="height: 33%">
                        <asp:Label ID="lblParent" runat="server" Text="Parent"></asp:Label>
                    </td>
                    <td style="height: 33%">
                        <asp:DropDownList ID="ddlParent" runat="server" Width="100%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="height: 33%">
                    <td align="center" style="height: 44%">
                        <asp:Label ID="lblText" runat="server" Text="Text"></asp:Label>
                    </td>
                    <td style="height: 44%">
                        <asp:TextBox ID="txtText" runat="server" Text="" Width="98%"></asp:TextBox>
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnTreeView" runat="server" Height="100%" Width="100%">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
            <table  style ="width:100%">
                <tr>
                    <td style ="width:50%" align ="center">
                        <asp:Button ID ="btnOk" runat ="server" Text ="Ok" Width="60px" OnClick="BtnOk_Click"  CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" />
                    </td>
                    <td style ="width:50%" align ="center" >
                        <asp:Button ID ="btnCancel" runat ="server" Text ="Cancel" Width="60px" OnClick="btnCancel_Click"  CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" />
                    </td>
                </tr>
            </table>
            </td>
        </tr>
    </table>
        </div>
    </form>
</body>
</html>
