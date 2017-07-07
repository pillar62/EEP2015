<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT308RF.aspx.cs"
    Inherits="RT308RF" %>
<%@ Register Assembly="EFClientTools" Namespace="EFClientTools" TagPrefix="EFClientTools" %>
<%@ Register assembly="DevExpress.XtraReports.v15.2.Web, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager" runat="server" />
            <EFClientTools:WebDataSource ID="DataSource" runat="server" RemoteName="" PacketRecords="-1" />
            <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Theme="Office2010Silver" ClientInstanceName="aspxViewer1" OnCacheReportDocument="ASPxDocumentViewer1_CacheReportDocument" OnRestoreReportDocumentFromCache="ASPxDocumentViewer1_RestoreReportDocumentFromCache">
                <SettingsReportViewer ShouldDisposeReport="False" />
            <ToolbarItems>
                        <dx:ReportToolbarButton ItemKind="Search" Text="´M§ä" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="PrintReport" Text="¦C¦L" />
                        <dx:ReportToolbarButton ItemKind="PrintPage" Text="¦C¦L¥»­¶" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" Text="­º­¶" />
                        <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" Text="¤W­¶" />
                        <dx:ReportToolbarLabel ItemKind="PageLabel" Text="­¶¦¸" />
                        <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                        </dx:ReportToolbarComboBox>
                        <dx:ReportToolbarLabel ItemKind="OfLabel" Text="¡þ" />
                        <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                        <dx:ReportToolbarButton ItemKind="NextPage" Text="¤U­¶" />
                        <dx:ReportToolbarButton ItemKind="LastPage" Text="¥½­¶" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="SaveToDisk" Text="¿é¥XÀÉ®×" />
                        <dx:ReportToolbarButton ItemKind="SaveToWindow" Text="·sµøµ¡ÂsÄý" />
                        <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                            <Elements>
                                <dx:ListElement Value="pdf" />
                                <dx:ListElement Value="xls" />
                                <dx:ListElement Value="xlsx" />
                                <dx:ListElement Value="rtf" />
                                <dx:ListElement Value="mht" />
                                <dx:ListElement Value="html" />
                                <dx:ListElement Value="txt" />
                                <dx:ListElement Value="csv" />
                                <dx:ListElement Value="png" />
                            </Elements>
                        </dx:ReportToolbarComboBox>
                    </ToolbarItems>
            </dx:ASPxDocumentViewer>
            <br />

            <br />

        </div>
    </form>
</body>
</html>
