<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportViewerTemplate.aspx.cs"
    Inherits="ReportViewerTemplate" %>
<%@ Register Assembly="EFClientTools" Namespace="EFClientTools" TagPrefix="EFClientTools" %>


<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


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
            <rsweb:reportviewer ID="ReportViewer1" runat="server" Width="100%" Height="580px"
                OnLoad="ReportViewer_Load" Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                PageCountMode="Actual">
                <LocalReport EnableExternalImages="True" OnSubreportProcessing="SubreportProcessing">
                    <DataSources>
                        <rsweb:reportdatasource DataSourceId="DataSource" Name="TestDataSet" />
                    </DataSources>
                </LocalReport>
            </rsweb:reportviewer>
        </div>
    </form>
</body>
</html>
