<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDateTimeViewer.aspx.cs" Inherits="InnerPages_frmDateTimeViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/innerpage/datetimeviewer.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table id="main_table" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="center">
        <table id="table1">
        <tr>
            <td class="td1">
                <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click">Previous</asp:LinkButton></td>
            
             <td class="td1">
                 <asp:DropDownList ID="ddlYear" runat="server" Width="90%" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True">
                 </asp:DropDownList></td>
             <td class="td2">
                 <asp:Label ID="lblYear" runat="server" Text="Year" Width="100%"></asp:Label></td>
             <td class="td3">
                 <asp:DropDownList ID="ddlMonth" runat="server" Width="100%" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True">
                 </asp:DropDownList></td>
             <td class="td2">
                 <asp:Label ID="lblMonth" runat="server" Text="Month" Width="100%"></asp:Label></td>
             <td class="td4">
                 <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click">Next</asp:LinkButton></td>
        </tr>
        </table>
        </td>    
    </tr>
    <tr>
        <td class="td5">
        <table id="table2">
        <tr>
            <td class="td6">
                <asp:Label ID="lblSun" runat="server" Text="Sun"></asp:Label></td>
            <td class="td6">
                <asp:Label ID="lblMon" runat="server" Text="Mon"></asp:Label></td>
            <td class="td6">
                <asp:Label ID="lblTue" runat="server" Text="Tue"></asp:Label></td>
            <td class="td6">
                <asp:Label ID="lblWed" runat="server" Text="Wed"></asp:Label></td>
            <td class="td6">
                <asp:Label ID="lblThu" runat="server" Text="Thu"></asp:Label></td>
            <td class="td6">
                <asp:Label ID="lblFri" runat="server" Text="Fri"></asp:Label></td>
            <td class="td6">
                <asp:Label ID="lblSat" runat="server" Text="Sat"></asp:Label></td>
               
        </tr>
        </table>
        </td>
    </tr>
    <tr>
        <td class="td7">
            <asp:Panel ID="pnCalendar" runat="server" Height="100%" Width="100%">
               </asp:Panel>
        </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
