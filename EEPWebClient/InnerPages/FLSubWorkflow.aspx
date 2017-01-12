<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FLSubWorkflow.aspx.cs"
    Inherits="InnerPages_FLSubWorkflow" %>

<%@ Register Assembly="AjaxTools" Namespace="AjaxTools" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/innerpage/flowurge.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Panel1" runat="server" Height="600px" Width="800px">
            <table border="1">
                <tr>
                    <td align="center" style="border: medium inset #666666; background-color: White"
                        width="550">
                        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center" ScrollBars="Auto"
                            Width="550px" Height="500px">
                        </asp:Panel>
                    </td>
                    <td valign="top" width="240">
                        <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Center">
                            <table border="2" cellpadding="2" width="240">
                                <tr>
                                    <td>
                                        <asp:Button ID="btAdd" runat="server" CssClass="btn_rect_mouseout" OnClick="btAdd_Click"
                                            Text="Add" Width="94px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btUp" runat="server" CssClass="btn_rect_mouseout" OnClick="btUp_Click"
                                            Text="Up" Width="94px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btAddSrow" runat="server" CssClass="btn_rect_mouseout" OnClick="btAddSrow_Click"
                                            Text="AddSameRow" Width="94px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btDown" runat="server" CssClass="btn_rect_mouseout" OnClick="btDown_Click"
                                            Text="Down" Width="94px" />
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                    <asp:Button ID="btDelete" runat="server" CssClass="btn_rect_mouseout" OnClick="btDelete_Click"
                                            Text="Delete" Width="94px" />
                                    </td>
                                    
                                    <td>
                                        
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table border="1" width="240">
                                <tr>
                                    <td width="80">
                                        <asp:Label ID="lbName" runat="server" CssClass="btn_rect_mouseout" Text="Name"></asp:Label>
                                    </td>
                                    <td width="110">
                                        <asp:TextBox ID="tbName" runat="server" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td width="80px">
                                        <asp:Label ID="lbDesc" runat="server" CssClass="btn_rect_mouseout" Text="Description"></asp:Label>
                                    </td>
                                    <td width="110px">
                                        <asp:TextBox ID="tbDesc" runat="server" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td width="80">
                                        <asp:Label ID="lbPlusA" runat="server" CssClass="btn_rect_mouseout" Text="PlusApprove"></asp:Label>
                                    </td>
                                    <td width="110">
                                        <asp:DropDownList ID="ddlPlusApprove" runat="server" Width="110px" AutoPostBack="True">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td width="80px">
                                        <asp:Label ID="lbKind" runat="server" CssClass="btn_rect_mouseout" Text="SendToKind"></asp:Label>
                                    </td>
                                    <td valign="top" width="110px">
                                        <asp:DropDownList ID="ddlKind" runat="server" AutoPostBack="True" Width="110px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Role</asp:ListItem>
                                            <asp:ListItem>Manager</asp:ListItem>
                                            <asp:ListItem>User</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td width="80">
                                        <asp:Label ID="lbRole" runat="server" CssClass="btn_rect_mouseout" Text="SendToRole"></asp:Label>
                                    </td>
                                    <td valign="top" width="110">
                                        <asp:DropDownList ID="ddlRole" runat="server" Width="110px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="80">
                                        <asp:Label ID="lbUser" runat="server" CssClass="btn_rect_mouseout" Text="SendToUser"></asp:Label>
                                    </td>
                                    <td valign="top" width="110">
                                        <asp:DropDownList ID="ddlUser" runat="server" Width="110px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="80">
                                        <asp:Label ID="lbType" runat="server" CssClass="btn_rect_mouseout" Text="Type"></asp:Label>
                                    </td>
                                    <td valign="top" width="110">
                                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Width="110px">
                                            <asp:ListItem Selected="True">Stand</asp:ListItem>
                                            <asp:ListItem>Notify</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table border="1" width="240">
                                <tr>
                                    <td  align="left" style="width: 170px">
                                        <asp:Label ID="lbSFilename" runat="server" Text="Save FileName" 
                                            CssClass="btn_rect_mouseout"></asp:Label>
                                        <asp:TextBox ID="tbFileName" runat="server" Width="170px"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btSave" runat="server" CssClass="btn_rect_mouseout" OnClick="btSave_Click"
                                            Text="Save" />
                                    </td>
                                </tr>
                                <tr>
                                    <td  align="left" style="width: 170px">
                                        <asp:Label ID="lbLFileName" runat="server" Text="Load FileName" 
                                            CssClass="btn_rect_mouseout"></asp:Label>
                                        <asp:DropDownList ID="ddlFiles" runat="server" Width="170px">
                                        </asp:DropDownList></td>
                                    <td align="center">
                                        <asp:Button ID="btLoad" runat="server" CssClass="btn_rect_mouseout" Text="Load" 
                                            onclick="btLoad_Click" />
                                    </td>
                                </tr>
                                <tr>
                                <td align="center" colspan="2">
                                <asp:Button ID="btClose" runat="server" CssClass="btn_rect_mouseout" Text="Close" 
                                            onclick="btClose_Click" />
                                </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
