<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebMenuUtilityMain.aspx.cs"
    Inherits="WebMenuUtilityMain" Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <table id="MenuCotent" width="500">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label1" runat="server" Text="Menu ID:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1">
                                    <asp:TextBox ID="tbMenuID" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label2" runat="server" Text="Caption:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 5%">
                                    <asp:TextBox ID="tbCaption" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label3" runat="server" Text="Parent ID:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:TextBox ID="tbParentID" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label4" runat="server" Text="Module Type:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:DropDownList ID="ddlModuleType" runat="server" Enabled="False">
                                        <asp:ListItem Value="W"></asp:ListItem>
                                        <asp:ListItem Value="F"></asp:ListItem>
                                        <asp:ListItem Value="S"></asp:ListItem>
                                        <asp:ListItem>C</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label5" runat="server" Text="Package:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:TextBox ID="tbPackage" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label6" runat="server" Text="Item Params:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:TextBox ID="tbItem" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label7" runat="server" Text="Form Name:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:TextBox ID="tbFormName" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label8" runat="server" Text="Solution:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:TextBox ID="tbSolution" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <asp:Label ID="Label9" runat="server" Text="Sequence:"></asp:Label></td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <asp:TextBox ID="tbSequence" runat="server" Enabled="False" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <table style="position: relative; width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete"
                                                    CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                                    Width="60px" />
                                                &nbsp; &nbsp;
                                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" CssClass="btn_mouseout"
                                                    onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                                    Width="60px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <table style="position: relative; width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnModify" runat="server" OnClick="btnModify_Click" Text="Modify"
                                                    CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                                    Width="60px" />
                                                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                                <asp:Button ID="btnOK" runat="server" Enabled="False" OnClick="btnOK_Click" Text="OK"
                                                    CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                                    Width="60px" />
                                                <asp:Button ID="btnCancel" runat="server" Enabled="False" OnClick="btnCancel_Click"
                                                    Text="Cancel" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                                    onmouseover="this.className='btn_mouseover';" Width="60px" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1" rowspan="1" style="width: 25%; height: 18%">
                                    <table style="position: relative; width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAccessGroup" runat="server" OnClick="btnAccessGroup_Click" Text="Access Group"
                                                    CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                                    Width="100px" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="left" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                                    <table style="position: relative; width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAccessUser" runat="server" Text="AccessUser" OnClick="btnAccessUser_Click"
                                                    CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                                    Width="100px" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            &nbsp; &nbsp;
        </div>
    </form>
</body>
</html>
