<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebAccessMenu.aspx.cs" Inherits="InnerPages_WebAccessMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div dir="ltr">
            <infolight:webdatasource id="wdsSolution" runat="server" datamember="solutionInfo"
                webdatasetid="WSolution">
        </infolight:webdatasource>
            <table id="MenuCotent" cellpadding="0" cellspacing="0">
                <tr height="50%">
                    <td style="width: 106px; height: 50%" align="right">
                        <asp:Label ID="Label1" runat="server" Text="Copy From:"></asp:Label></td>
                    <td style="height: 50%">
                        <table style="position: relative;">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlCopy" runat="server" Width="160px">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:Button ID="btnCopy" runat="server" OnClick="btnCopy_Click" Text="Copy" Width="90px"
                                        CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" /></td>
                                <td>
                                    <asp:Button ID="btnEqual" runat="server" Text="Equal" OnClick="btnEqual_Click" Width="90px"
                                        CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" /></td>
                            </tr>
                        </table>
                        &nbsp; &nbsp;&nbsp;&nbsp;</td>
                </tr>
                <tr height="50%">
                    <td style="width: 106px; height: 50%;" align="right">
                        <asp:Label ID="Label2" runat="server" Text="Solution:"></asp:Label></td>
                    <td style="height: 50%">
                        <table style="position: relative;">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlSolution" runat="server" DataSourceID="wdsSolution" DataTextField="ITEMNAME"
                                        DataValueField="ITEMTYPE" OnSelectedIndexChanged="ddlSolution_SelectedIndexChanged"
                                        AutoPostBack="True" Width="160px">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:Button ID="btnSelectAll" runat="server" OnClick="btnSelectAll_Click" Text="Select All"
                                        Width="90px" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" /></td>
                                <td>
                                    <asp:Button ID="btnCancelAll" runat="server" OnClick="btnCancelAll_Click" Text="Cancel All"
                                        Width="90px" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr height="50%">
                    <td style="width: 106px" align="right">
                        &nbsp;</td>
                    <td>
                        <table style="position: relative;">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblMenu" runat="server" RepeatColumns="2">
                                    </asp:CheckBoxList><br />
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" CssClass="btn_mouseout"
                                        onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                        Width="60px" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
