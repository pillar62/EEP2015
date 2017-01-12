<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmListBoxList.aspx.cs" Inherits="InnerPages_frmListBoxList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../css/innerpage/listboxlist.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function queryStringValue(pname) {
            return document.location.search.match(new RegExp("[\?\&]" + pname + "=([^\&]*)(\&?)", "i"));
        }
        function pageload() {
            var queryContainer = document.getElementById('queryContainer');
            var displayQuery = queryStringValue('ListBoxQuery')[1];
            if (queryContainer && displayQuery == 'False') {
                queryContainer.style.display = 'none';
                queryContainer.style.visibility = 'hidden';
            }
        }
    </script>
</head>
<body onload="pageload()">
    <form id="form1" runat="server">
    <div>
        <center>
            <table style="text-align: center;">
                <tr>
                    <td class="top_align" style="text-align: center;">
                        <asp:ListBox ID="ListBox1" runat="server" Height="250px" SelectionMode="Multiple"
                            Width="150px"></asp:ListBox>
                    </td>
                    <td class="top_align" style="text-align: center;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSelAll" runat="server" Text=">>" OnClick="btnSelAll_Click" CssClass="btn_square_mouseout"
                                        onmouseout="this.className='btn_square_mouseout';" onmouseover="this.className='btn_square_mouseover';" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSelect" runat="server" Text=">" OnClick="btnSelect_Click" CssClass="btn_square_mouseout"
                                        onmouseout="this.className='btn_square_mouseout';" onmouseover="this.className='btn_square_mouseover';" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnRemove" runat="server" Text="<" OnClick="btnRemove_Click" CssClass="btn_square_mouseout"
                                        onmouseout="this.className='btn_square_mouseout';" onmouseover="this.className='btn_square_mouseover';" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnRemAll" runat="server" Text="<<" OnClick="btnRemAll_Click" CssClass="btn_square_mouseout"
                                        onmouseout="this.className='btn_square_mouseout';" onmouseover="this.className='btn_square_mouseover';" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="top_align" style="text-align: center;">
                        <asp:ListBox ID="ListBox2" runat="server" Height="250px" SelectionMode="Multiple"
                            Width="150px"></asp:ListBox>
                    </td>
                </tr>
                <tr id="queryContainer">
                    <td colspan="3">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <%= this.GetField(true) %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchValue" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <%= this.GetField(false) %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchText" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="search" OnClick="btnSearch_Click"
                                        CssClass="btn_rect_mouseout" onmouseout="this.className='btn_rect_mouseout';"
                                        onmouseover="this.className='btn_rect_mouseover';" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>                
                    <td colspan="3">
                        <table  class="btn_class">
                            <tr>
                                <td class="okbtn_align">
                                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="btn_rect_mouseout"
                                        onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';" />
                                </td>
                                <td class="cancelbtn_align">
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn_rect_mouseout"
                                        onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
