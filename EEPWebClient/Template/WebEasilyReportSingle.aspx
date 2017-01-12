<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebEasilyReportSingle.aspx.cs" Inherits="Template_WebEasilyReport" %>

<%@ Register Assembly="EasilyReport" Namespace="Infolight.EasilyReportTools" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxTools:AjaxScriptManager ID="AjaxScriptManager1" runat="server">
        </ajaxTools:AjaxScriptManager>
        <InfoLight:WebDataSource ID="Master" runat="server" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
        </asp:UpdatePanel>
        <cc1:WebEasilyReport id="WebEasilyReport1" runat="server" 
        datasourceid="Master" Visible="False" HeaderFont="新細明體, 12pt, style=Bold">
            <Images>
                <cc1:ImageItem ImageUrl="~/Image/main/Logo.gif" Name="ImageItem" />
            </Images>
<Format ColumnGap="0" ColumnGridLine="True" RowGap="0" RowGridLine="True" PageRecords="30" PageHeight="0" PageSize="A4" Orientation="Vertical" DateFormat="Date" PageIndexFormat="Current" UserFormat="ID" ExportFormat="Excel" MarginLeft="0" MarginRight="0" MarginTop="0" MarginBottom="0" ColumnInsideGridLine="False"></Format>

<MailSetting Port="25" Encoding="gb2312"></MailSetting>
            <HeaderItems>
                <cc1:ReportImageItem Cells="0" ContentAlignment="Left" Format="{0}" Index="0" 
                    NewLine="False" Position="Left" />
                <cc1:ReportConstantItem Cells="0" ContentAlignment="Center" Format="{0}" 
                    NewLine="True" Position="Left" Style="ReportName" 
                    Font="Microsoft Sans Serif, 15.75pt, style=Bold" />
                <cc1:ReportConstantItem Cells="2" ContentAlignment="Left" 
                    Format="列印日期:{0}" NewLine="True" Position="Left" Style="ReportDate" />
                <cc1:ReportConstantItem Cells="0" ContentAlignment="Right" Format="頁次:{0}" 
                    NewLine="False" Position="Right" Style="PageIndexAndTotalPageCount" />
            </HeaderItems>
            <DataSource>
                <cc1:WebDataSourceItem DataSourceID="Master" />
            </DataSource>
</cc1:WebEasilyReport>
        <InfoLight:WebClientQuery ID="WebClientQuery1" runat="server" DataSourceID="Master">
        </InfoLight:WebClientQuery>
        <asp:Panel ID="Panel1" runat="server" Height="63px" Width="100%">
        </asp:Panel>
        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="Query" 
            Width="100px" />&nbsp;
        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" 
            Width="100px" />&nbsp;
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
            Width="100px" />&nbsp;
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Report Design" Width="100px" />
        <br />
        <InfoLight:WebGridView ID="WebGridView1" runat="server" DataSourceID="Master" CellPadding="4" CreateInnerNavigator="False" ForeColor="#333333" GridLines="None">
            <Columns>
                <asp:CommandField ButtonType="Image" CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif"
                    EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif"
                    ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" />
            </Columns>
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <AlternatingRowStyle BackColor="White" />
        </InfoLight:WebGridView>
        </div>
    </form>
</body>
</html>
