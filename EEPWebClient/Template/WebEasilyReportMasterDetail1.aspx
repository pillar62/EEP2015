<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebEasilyReportMasterDetail1.aspx.cs" Inherits="Template_WebEasilyReportMasterDetail1" %>

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
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
        </asp:UpdatePanel>
        <cc1:webeasilyreport id="WebEasilyReport1" runat="server" 
        datasourceid="Detail" headerdatasourceid="Master" Visible="False" 
        HeaderFont="新細明體, 12pt">
            <Images>
                <cc1:ImageItem ImageUrl="~/Image/main/Logo.gif" Name="ImageItem" />
            </Images>
<Format ColumnGap="0" ColumnGridLine="True" RowGap="0" RowGridLine="True" PageRecords="30" PageHeight="0" PageSize="A4" Orientation="Vertical" DateFormat="Date" PageIndexFormat="Current" UserFormat="ID" ExportFormat="Excel" MarginLeft="0" MarginRight="0" MarginTop="0" MarginBottom="0" ColumnInsideGridLine="False"></Format>

<MailSetting Port="25" Encoding="gb2312"></MailSetting>

             <HeaderItems>
                <cc1:ReportImageItem Cells="0" ContentAlignment="Center" Format="{0}" Index="0" 
                    NewLine="False" Position="Left" />
                <cc1:ReportConstantItem Cells="0" ContentAlignment="Center" Font="PMingLiU, 18pt, style=Bold" 
                    Format="{0}" NewLine="True" Position="Left" Style="ReportName" />
                <cc1:ReportConstantItem Cells="2" ContentAlignment="Left" Format="列印日期:{0}" 
                    NewLine="True" Position="Left" Style="ReportDate" />
            </HeaderItems>
            <DataSource>
                <cc1:WebDataSourceItem DataSourceID="Detail" />
            </DataSource>
</cc1:webeasilyreport>
        <InfoLight:WebClientQuery ID="WebClientQuery1" runat="server" DataSourceID="Master">
        </InfoLight:WebClientQuery>
        <asp:Panel ID="Panel1" runat="server" Height="57px" Width="100%">
        </asp:Panel>
        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="Query" 
            Width="100px" />&nbsp;
        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" 
            Width="100px" />
        &nbsp;
        <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print" 
            Width="100px" />
        &nbsp;
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Report Design" Width="100px" />
&nbsp;<InfoLight:WebDataSource ID="Detail" runat="server" AllowAdd="True" AllowDelete="True"
            AllowPrint="True" AllowUpdate="True" AlwaysClose="False" AutoApply="False" AutoApplyForInsert="False"
            AutoRecordLock="False" AutoRecordLockMode="NoneReload" CacheDataSet="False" CommandName=""
            DataMember="" Eof="True" KeyValues="" LastIndex="-1" Marker="'" MasterDataSource="Master"
            PacketRecords="100" QuotePrefix="[" QuoteSuffix="]" RemoteName="SOrderDetails_SQL.Orders"
            SelectCommand="" TableName="" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
    
        <InfoLight:WebDataSource ID="Master" runat="server" AllowAdd="True" AllowDelete="True"
            AllowPrint="True" AllowUpdate="True" AlwaysClose="False" AutoApply="True" AutoApplyForInsert="False"
            AutoRecordLock="False" AutoRecordLockMode="NoneReload" CacheDataSet="False" CommandName=""
            DataMember="" Eof="True" KeyValues="" LastIndex="-1" Marker="'" MasterDataSource=""
            PacketRecords="100" QuotePrefix="[" QuoteSuffix="]" RemoteName="SOrderDetails_SQL.Orders"
            SelectCommand="" TableName="" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
    
    </div>
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" BackColor="Silver"
            BorderColor="White" BorderStyle="Groove" BorderWidth="2px" CellPadding="4" DataSourceID="Master"
            ForeColor="#333333" Height="87px" LayOutColNum="2" OnPageIndexChanged="wfvMaster_PageIndexChanged"
            Width="100%">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" ForeColor="Navy" />
            <PagerStyle BackColor="PowderBlue" ForeColor="White" HorizontalAlign="Center" />
            <Fields>
                <InfoLight:FormViewField ControlID="tbEmployeeID" FieldName="EmployeeID" />
            </Fields>
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
            <EditRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <InfoLight:WebGridView ID="wgvDetail" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif" ApplyIconUrl="../Image/UIPics/Apply.gif"
            BackColor="White" BorderStyle="Groove" BorderWidth="2px" CancelIconUrl="../Image/UIPics/Cancel.gif"
            CellPadding="1" CellSpacing="1" CreateInnerNavigator="False" DataSourceID="Detail"
            ForeColor="#333333" HeaderStyleWrap="False" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif"
            MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif"
            MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif"
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif"
            QueryIconUrl="../Image/UIPics/Query.gif" Width="100%">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AddNewRowControls>
                <InfoLight:AddNewRowControlItem ControlID="wdtpOrderDetailsLastDateTimeF" ControlType="DateTimePicker"
                    FieldName="LastDateTime" />
            </AddNewRowControls>
            <HeaderStyle BackColor="DarkTurquoise" Font-Bold="True" ForeColor="White" />
            <PagerSettings Mode="NumericFirstLast" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:CommandField ButtonType="Image" CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif"
                    EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif"
                    ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" />
            </Columns>
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="PowderBlue" ForeColor="Blue" HorizontalAlign="Center" />
            <AlternatingRowStyle BackColor="White" />
        </InfoLight:WebGridView>
    &nbsp;&nbsp;</form>
</body>
</html>
