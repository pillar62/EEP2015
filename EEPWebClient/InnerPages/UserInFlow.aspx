<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInFlow.aspx.cs" Inherits="InnerPages_UserInFlow"
    Theme="InnerPageSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User In Flow</title>
    <link href="../css/innerpage/userinflow.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="main_content">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="CaptionUserId" runat="server" Text="UserId:" Font-Bold="True"></asp:Label></td>
                    <td  Width="280px">
                        <asp:Label ID="UserIdLabel" runat="server" Font-Bold="True"></asp:Label></td>
                    <td>
                        <asp:Label ID="CaptionUserName" runat="server" Text="UserName:" Font-Bold="True"></asp:Label></td>
                    <td  Width="280px">
                        <asp:Label ID="UserNameLabel" runat="server" Font-Bold="True"></asp:Label></td>
                </tr>
            </table>
            <fieldset class="centeralign_fieldset">
                <legend>
                    <%= this.getHtmlText(2) %>
                </legend>
                <asp:GridView ID="gdvByRole" SkinID="FlowCommentGrid" runat="server" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="RoleId" HeaderText="RoleId">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RoleName" HeaderText="RoleName">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FlowDescription" HeaderText="FlowDescription">
                            <ItemStyle Width="280px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ActivityName" HeaderText="ActivityName">
                            <ItemStyle Width="280px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <fieldset class="centeralign_fieldset">
                <legend>
                    <%= this.getHtmlText(3) %>
                </legend>
                <asp:GridView ID="gdvByUser" SkinID="FlowCommentGrid" runat="server" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="UserId" HeaderText="UserId">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UserName" HeaderText="UserName">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FlowDescription" HeaderText="FlowDescription">
                            <ItemStyle Width="280px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ActivityName" HeaderText="ActivityName">
                            <ItemStyle Width="280px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </div>
    </form>
</body>
</html>
