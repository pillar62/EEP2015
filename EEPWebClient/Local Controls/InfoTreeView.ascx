<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InfoTreeView.ascx.cs" Inherits="InfoTreeView" %>
<table style="width: 170px">
    <tr>
        <td style="width: 152px">
            <asp:DropDownList ID="ddlSolution" runat="server" AutoPostBack="true" Width="170px">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td style="width: 152px; height: 179px">
            <asp:TreeView ID="tView" runat="server" Height="154px" ImageSet="Simple" NodeIndent="10"
                ShowLines="True" Width="170px">
                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px"
                    NodeSpacing="0px" VerticalPadding="0px" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px"
                    VerticalPadding="0px" ImageUrl="~/Image/main/b5.gif" />
                <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
                <ParentNodeStyle Font-Bold="False" ImageUrl="~/Image/main/b4.gif" />
                <RootNodeStyle ImageUrl="~/Image/main/b4.gif" />
                <LeafNodeStyle ImageUrl="~/Image/main/b6.gif" />
            </asp:TreeView>
        </td>
    </tr>
</table>
