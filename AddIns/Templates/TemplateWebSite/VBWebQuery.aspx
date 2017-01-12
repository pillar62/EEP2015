<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebQuery.aspx.vb" Inherits="Template_VBWebQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" DataMember=""
                WebDataSetID="WMaster">
            </InfoLight:WebDataSource>
            <InfoLight:WebClientQuery ID="WebClientQuery1" runat="server" DataSourceID="Master">
            </InfoLight:WebClientQuery>
            <asp:Panel ID="Panel1" runat="server" Height="133px" Width="648px">
            </asp:Panel>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Query" Width="94px" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Clear" Width="89px" /><br />
            <InfoLight:WebGridView ID="WebGridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" CreateInnerNavigator="False" DataSourceID="Master" ForeColor="#333333"
                GetServerText="True" InnerNavigatorShowStyle="Image" Width="646px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </InfoLight:WebGridView>
        </div>
    
    </div>
    </form>
</body>
</html>
